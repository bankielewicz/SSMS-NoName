using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Diagnostics.Core
{
    public enum ProbeStatus
    {
        Pass,
        Fail,
        Error,
        Timeout,
        Skipped,
        NotApplicable,
    }

    /// <summary>One capability check. Probes record each API call through <see cref="ProbeContext"/>.</summary>
    public interface IProbe
    {
        string Id { get; }

        string Area { get; }

        string Description { get; }

        Task RunAsync(ProbeContext context, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Collects what a probe observed. Each <c>Step</c> call records the API name, whether it worked,
    /// how long it took and any exception, then lets the probe carry on so later steps still log.
    /// </summary>
    public sealed class ProbeContext
    {
        private readonly object _gate = new object();
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        private readonly List<IDictionary<string, object>> _steps = new List<IDictionary<string, object>>();

        public ProbeContext(Sanitizer sanitizer)
        {
            Sanitizer = sanitizer ?? throw new ArgumentNullException(nameof(sanitizer));
        }

        public Sanitizer Sanitizer { get; }

        public IDictionary<string, object> Data
        {
            get
            {
                lock (_gate)
                    return new Dictionary<string, object>(_data);
            }
        }

        public IReadOnlyList<IDictionary<string, object>> Steps
        {
            get
            {
                lock (_gate)
                    return _steps.ToArray();
            }
        }

        public ProbeStatus? Outcome { get; private set; }

        public string Reason { get; private set; }

        public bool AnyRequiredStepFailed { get; private set; }

        public void Set(string key, object value)
        {
            lock (_gate)
                _data[key] = value;
        }

        /// <summary>Calls <paramref name="call"/>, records the outcome, and returns default when it throws.</summary>
        public T Step<T>(string api, Func<T> call, bool required = true)
        {
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                T result = call();
                Record(api, required, watch, null);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                Record(api, required, watch, ex);
                throw;
            }
            catch (Exception ex)
            {
                Record(api, required, watch, ex);
                return default(T);
            }
        }

        public void Step(string api, Action call, bool required = true)
        {
            Step<object>(api, () =>
            {
                call();
                return null;
            }, required);
        }

        public async Task<T> StepAsync<T>(string api, Func<Task<T>> call, bool required = true)
        {
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                T result = await call();
                Record(api, required, watch, null);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                Record(api, required, watch, ex);
                throw;
            }
            catch (Exception ex)
            {
                Record(api, required, watch, ex);
                return default(T);
            }
        }

        public void Fail(string reason) => SetOutcome(ProbeStatus.Fail, reason);

        public void Skip(string reason) => SetOutcome(ProbeStatus.Skipped, reason);

        public void NotApplicable(string reason) => SetOutcome(ProbeStatus.NotApplicable, reason);

        private void SetOutcome(ProbeStatus status, string reason)
        {
            lock (_gate)
            {
                Outcome = status;
                Reason = reason;
            }
        }

        private void Record(string api, bool required, Stopwatch watch, Exception error)
        {
            var step = new Dictionary<string, object>
            {
                ["api"] = api,
                ["ok"] = error == null,
                ["ms"] = watch.ElapsedMilliseconds,
                ["required"] = required,
            };
            if (error != null)
                step["error"] = ExceptionInfo.From(error);

            lock (_gate)
            {
                _steps.Add(step);
                if (error != null && required)
                    AnyRequiredStepFailed = true;
            }
        }
    }

    public sealed class ProbeResult
    {
        public string ProbeId { get; set; }

        public string Area { get; set; }

        public ProbeStatus Status { get; set; }

        public long DurationMs { get; set; }

        public string Reason { get; set; }

        public IDictionary<string, object> Data { get; set; }

        public IReadOnlyList<IDictionary<string, object>> Steps { get; set; }

        public IDictionary<string, object> Error { get; set; }

        public IDictionary<string, object> ToFields()
        {
            var fields = new Dictionary<string, object>
            {
                ["probe"] = ProbeId,
                ["area"] = Area,
                ["status"] = Status,
                ["ms"] = DurationMs,
            };
            if (Reason != null)
                fields["reason"] = Reason;
            if (Data != null && Data.Count > 0)
                fields["data"] = Data;
            if (Steps != null && Steps.Count > 0)
                fields["steps"] = Steps;
            if (Error != null)
                fields["error"] = Error;
            return fields;
        }
    }
}
