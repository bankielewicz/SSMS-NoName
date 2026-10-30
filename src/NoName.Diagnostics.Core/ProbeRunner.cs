using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Diagnostics.Core
{
    /// <summary>
    /// Runs probes so that none can take the host down: exceptions become results, a time limit
    /// turns a hang into <see cref="ProbeStatus.Timeout"/>, and every start and result is logged.
    /// Probes start on the caller's context (the UI thread when a button is clicked); a probe that
    /// blocks that thread synchronously can't be timed out, so probes must stay asynchronous.
    /// </summary>
    public sealed class ProbeRunner
    {
        private readonly DiagnosticLog _log;
        private readonly Sanitizer _sanitizer;
        private readonly TimeSpan _timeout;

        public ProbeRunner(DiagnosticLog log, Sanitizer sanitizer, TimeSpan timeout)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _sanitizer = sanitizer ?? throw new ArgumentNullException(nameof(sanitizer));
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeout), "The time limit must be positive.");
            _timeout = timeout;
        }

        public async Task<ProbeResult> RunAsync(IProbe probe, CancellationToken cancellationToken = default)
        {
            if (probe == null)
                throw new ArgumentNullException(nameof(probe));

            _log.Write("probe.start", probe.Id, new Dictionary<string, object>
            {
                ["area"] = probe.Area,
                ["description"] = probe.Description,
            });

            var context = new ProbeContext(_sanitizer);
            Stopwatch watch = Stopwatch.StartNew();
            ProbeStatus status;
            string reason = null;
            IDictionary<string, object> error = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Task run = probe.RunAsync(context, cancellationToken) ?? Task.CompletedTask;

                Task finished;
                using (var stopDelay = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    finished = await Task.WhenAny(run, Task.Delay(_timeout, stopDelay.Token));
                    stopDelay.Cancel();
                }

                if (finished != run)
                {
                    ObserveLater(run);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        status = ProbeStatus.Skipped;
                        reason = "cancelled";
                    }
                    else
                    {
                        status = ProbeStatus.Timeout;
                        reason = "no result after " + _timeout.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " ms";
                    }
                }
                else
                {
                    await run;
                    if (context.Outcome.HasValue)
                    {
                        status = context.Outcome.Value;
                        reason = context.Reason;
                    }
                    else if (context.AnyRequiredStepFailed)
                    {
                        status = ProbeStatus.Fail;
                        reason = "a required step failed";
                    }
                    else
                    {
                        status = ProbeStatus.Pass;
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                status = ProbeStatus.Skipped;
                reason = "cancelled";
            }
            catch (Exception ex)
            {
                status = ProbeStatus.Error;
                reason = "the probe threw";
                error = ExceptionInfo.From(ex);
            }

            watch.Stop();
            var result = new ProbeResult
            {
                ProbeId = probe.Id,
                Area = probe.Area,
                Status = status,
                DurationMs = watch.ElapsedMilliseconds,
                Reason = reason,
                Data = context.Data,
                Steps = context.Steps,
                Error = error,
            };
            _log.Write("probe.result", probe.Id, result.ToFields());
            return result;
        }

        /// <summary>Runs probes one after another and logs a one-line status summary (the canary view).</summary>
        public async Task<IReadOnlyList<ProbeResult>> RunAllAsync(IEnumerable<IProbe> probes, CancellationToken cancellationToken = default)
        {
            if (probes == null)
                throw new ArgumentNullException(nameof(probes));

            var results = new List<ProbeResult>();
            foreach (IProbe probe in probes)
                results.Add(await RunAsync(probe, cancellationToken));

            var statuses = new Dictionary<string, object>();
            foreach (ProbeResult result in results)
                statuses[result.ProbeId] = result.Status;

            _log.Write("run.summary", "probes", new Dictionary<string, object>
            {
                ["count"] = results.Count,
                ["counts"] = results.GroupBy(r => r.Status).ToDictionary(g => g.Key.ToString(), g => (object)g.Count()),
                ["statuses"] = statuses,
            });
            return results;
        }

        private static void ObserveLater(Task task)
        {
            // A timed-out probe may still fault later; observe it so it isn't reported as unobserved.
            task.ContinueWith(t => _ = t.Exception, CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }
    }
}
