using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NoName.Diagnostics.Core;
using Xunit;

namespace NoName.Diagnostics.Core.Tests
{
    public sealed class ProbeRunnerTests : IDisposable
    {
        private readonly string _dir = Path.Combine(Path.GetTempPath(), "noname-runner-tests-" + Guid.NewGuid().ToString("N"));
        private readonly DiagnosticLog _log;
        private readonly ProbeRunner _runner;

        public ProbeRunnerTests()
        {
            _log = new DiagnosticLog(_dir, sessionId: "t");
            _runner = new ProbeRunner(_log, new Sanitizer(verbose: false), TimeSpan.FromMilliseconds(500));
        }

        public void Dispose()
        {
            _log.Dispose();
            Directory.Delete(_dir, recursive: true);
        }

        [Fact]
        public async Task Completed_probe_passes_and_keeps_its_data()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-1", (ctx, ct) => { ctx.Set("answer", 42); return Task.CompletedTask; }));

            Assert.Equal(ProbeStatus.Pass, result.Status);
            Assert.Equal(42, result.Data["answer"]);
            Assert.True(result.DurationMs >= 0);
        }

        [Fact]
        public async Task Probe_that_throws_synchronously_is_an_error_not_a_crash()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-2", (ctx, ct) => throw new InvalidOperationException("sync boom")));

            Assert.Equal(ProbeStatus.Error, result.Status);
            Assert.Equal("System.InvalidOperationException", result.Error["type"]);
        }

        [Fact]
        public async Task Probe_that_faults_asynchronously_is_an_error()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-3", async (ctx, ct) =>
            {
                await Task.Yield();
                throw new TimeoutException("async boom");
            }));

            Assert.Equal(ProbeStatus.Error, result.Status);
            Assert.Equal("async boom", result.Error["message"]);
        }

        [Fact]
        public async Task Probe_that_hangs_times_out()
        {
            var hang = new TaskCompletionSource<bool>();
            try
            {
                Task<ProbeResult> running = _runner.RunAsync(new Probe("P-4", (ctx, ct) => hang.Task));

                // The limit is 500 ms. If the runner ignored it, fail here rather than hang the suite.
                Task first = await Task.WhenAny(running, Task.Delay(TimeSpan.FromSeconds(10)));
                Assert.True(first == running, "the runner did not stop a hanging probe within 10 s of its 500 ms limit");

                Assert.Equal(ProbeStatus.Timeout, (await running).Status);
            }
            finally
            {
                hang.TrySetResult(true); // let a still-pending run finish
            }
        }

        [Fact]
        public async Task Failed_required_step_fails_the_probe_but_later_steps_still_run()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-5", (ctx, ct) =>
            {
                ctx.Step<int>("Missing.Api", () => throw new MissingMethodException("Missing.Api"));
                ctx.Step("Present.Api", () => 7);
                return Task.CompletedTask;
            }));

            Assert.Equal(ProbeStatus.Fail, result.Status);
            Assert.Equal(2, result.Steps.Count);
            Assert.Equal(false, result.Steps[0]["ok"]);
            Assert.Equal("Missing.Api", result.Steps[0]["api"]);
            Assert.Equal("System.MissingMethodException", ((IDictionary<string, object>)result.Steps[0]["error"])["type"]);
            Assert.Equal(true, result.Steps[1]["ok"]);
        }

        [Fact]
        public async Task Failed_optional_step_does_not_fail_the_probe()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-6", (ctx, ct) =>
            {
                ctx.Step("Optional.Api", () => throw new NotSupportedException(), required: false);
                return Task.CompletedTask;
            }));

            Assert.Equal(ProbeStatus.Pass, result.Status);
            Assert.Equal(false, result.Steps.Single()["ok"]);
        }

        [Fact]
        public async Task Async_step_records_success_and_returns_its_value()
        {
            int seen = 0;
            ProbeResult result = await _runner.RunAsync(new Probe("P-7", async (ctx, ct) =>
            {
                seen = await ctx.StepAsync("Async.Api", async () => { await Task.Yield(); return 5; });
            }));

            Assert.Equal(5, seen);
            Assert.Equal(ProbeStatus.Pass, result.Status);
            Assert.Equal(true, result.Steps.Single()["ok"]);
        }

        [Fact]
        public async Task Explicit_outcome_and_reason_win()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-8", (ctx, ct) => { ctx.NotApplicable("no query window"); return Task.CompletedTask; }));

            Assert.Equal(ProbeStatus.NotApplicable, result.Status);
            Assert.Equal("no query window", result.Reason);
        }

        [Fact]
        public async Task Cancelled_run_is_skipped()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                ProbeResult result = await _runner.RunAsync(
                    new Probe("P-9", (ctx, ct) => Task.Delay(5000, ct)), cts.Token);

                Assert.Equal(ProbeStatus.Skipped, result.Status);
            }
        }

        [Fact]
        public async Task Start_and_result_lines_are_logged()
        {
            await _runner.RunAsync(new Probe("P-10", (ctx, ct) => Task.CompletedTask));

            List<JsonElement> lines = ReadLines();
            Assert.Contains(lines, l => l.GetProperty("kind").GetString() == "probe.start" && l.GetProperty("name").GetString() == "P-10");
            JsonElement result = lines.Single(l => l.GetProperty("kind").GetString() == "probe.result");
            Assert.Equal("Pass", result.GetProperty("data").GetProperty("status").GetString());
        }

        [Fact]
        public async Task Run_all_logs_a_summary_of_each_status()
        {
            await _runner.RunAllAsync(new IProbe[]
            {
                new Probe("A", (ctx, ct) => Task.CompletedTask),
                new Probe("B", (ctx, ct) => throw new Exception("x")),
            });

            JsonElement summary = ReadLines().Single(l => l.GetProperty("kind").GetString() == "run.summary").GetProperty("data");
            Assert.Equal("Pass", summary.GetProperty("statuses").GetProperty("A").GetString());
            Assert.Equal("Error", summary.GetProperty("statuses").GetProperty("B").GetString());
        }

        [Fact]
        public async Task Cancelled_step_is_recorded_and_ends_the_probe_as_an_error()
        {
            // The caller didn't cancel, so a cancellation inside the probe is an error, not a skip.
            ProbeResult result = await _runner.RunAsync(new Probe("P-11", (ctx, ct) =>
            {
                ctx.Step("Cancelled.Api", () => throw new OperationCanceledException());
                ctx.Step("Never.Reached", () => 1);
                return Task.CompletedTask;
            }));

            Assert.Equal(ProbeStatus.Error, result.Status);
            Assert.Equal("Cancelled.Api", result.Steps.Single()["api"]);
            Assert.Equal(false, result.Steps.Single()["ok"]);
        }

        [Fact]
        public async Task Failed_async_step_fails_the_probe_and_returns_default()
        {
            int value = -1;
            ProbeResult result = await _runner.RunAsync(new Probe("P-12", async (ctx, ct) =>
            {
                value = await ctx.StepAsync<int>("Async.Missing", async () =>
                {
                    await Task.Yield();
                    throw new MissingMethodException("Async.Missing");
                });
            }));

            Assert.Equal(0, value);
            Assert.Equal(ProbeStatus.Fail, result.Status);
            Assert.Equal("System.MissingMethodException", ((IDictionary<string, object>)result.Steps.Single()["error"])["type"]);
        }

        [Fact]
        public async Task Cancelled_async_step_is_recorded_and_rethrown()
        {
            ProbeResult result = await _runner.RunAsync(new Probe("P-13", async (ctx, ct) =>
            {
                await ctx.StepAsync<int>("Async.Cancelled", async () =>
                {
                    await Task.Yield();
                    throw new OperationCanceledException();
                });
            }));

            Assert.Equal(ProbeStatus.Error, result.Status);
            Assert.Equal(false, result.Steps.Single()["ok"]);
        }

        [Fact]
        public async Task Explicit_fail_and_skip_are_reported_with_reasons()
        {
            ProbeResult failed = await _runner.RunAsync(new Probe("P-14", (ctx, ct) => { ctx.Fail("wrong value"); return Task.CompletedTask; }));
            ProbeResult skipped = await _runner.RunAsync(new Probe("P-15", (ctx, ct) => { ctx.Skip("no grid open"); return Task.CompletedTask; }));

            Assert.Equal(ProbeStatus.Fail, failed.Status);
            Assert.Equal("wrong value", failed.Reason);
            Assert.Equal(ProbeStatus.Skipped, skipped.Status);
            Assert.Equal("no grid open", skipped.Reason);
        }

        [Fact]
        public async Task Cancelling_a_probe_that_ignores_the_token_skips_it_without_waiting_for_the_time_limit()
        {
            var runner = new ProbeRunner(_log, new Sanitizer(verbose: false), TimeSpan.FromSeconds(30));
            var hang = new TaskCompletionSource<bool>();
            using (var cts = new CancellationTokenSource())
            {
                try
                {
                    // The probe cancels the caller's token as soon as it has started, then ignores it and hangs.
                    ProbeResult result = await runner.RunAsync(new Probe("P-16", (ctx, ct) =>
                    {
                        cts.Cancel();
                        return hang.Task;
                    }), cts.Token);

                    Assert.Equal(ProbeStatus.Skipped, result.Status);
                    Assert.True(result.DurationMs < 10000, "should end on cancellation, not after the 30 s limit");
                }
                finally
                {
                    hang.TrySetResult(true);
                }
            }
        }

        [Fact]
        public async Task Probes_receive_the_runners_own_sanitizer()
        {
            var sanitizer = new Sanitizer(verbose: true);
            var runner = new ProbeRunner(_log, sanitizer, TimeSpan.FromSeconds(5));
            Sanitizer seen = null;

            await runner.RunAsync(new Probe("P-17", (ctx, ct) =>
            {
                seen = ctx.Sanitizer;
                return Task.CompletedTask;
            }));

            Assert.Same(sanitizer, seen);
        }

        [Fact]
        public async Task Action_step_runs_the_action_and_records_a_successful_step()
        {
            int calls = 0;

            ProbeResult result = await _runner.RunAsync(new Probe("P-18", (ctx, ct) =>
            {
                ctx.Step("Action.Api", () => { calls++; });
                return Task.CompletedTask;
            }));

            Assert.Equal(1, calls);
            Assert.Equal(ProbeStatus.Pass, result.Status);
            IDictionary<string, object> step = result.Steps.Single();
            Assert.Equal("Action.Api", step["api"]);
            Assert.Equal(true, step["ok"]);
        }

        [Fact]
        public void Runner_arguments_are_checked()
        {
            Assert.Throws<ArgumentNullException>(() => new ProbeRunner(null, new Sanitizer(verbose: false), TimeSpan.FromSeconds(1)));
            Assert.Throws<ArgumentNullException>(() => new ProbeRunner(_log, null, TimeSpan.FromSeconds(1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProbeRunner(_log, new Sanitizer(verbose: false), TimeSpan.Zero));
            Assert.Throws<ArgumentNullException>(() => new ProbeContext(null));
        }

        [Fact]
        public async Task Null_probe_or_probe_list_is_rejected()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _runner.RunAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _runner.RunAllAsync(null));
        }

        private List<JsonElement> ReadLines()
        {
            using (var stream = new FileStream(_log.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd()
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => JsonDocument.Parse(l).RootElement.Clone())
                    .ToList();
            }
        }

        private sealed class Probe : IProbe
        {
            private readonly Func<ProbeContext, CancellationToken, Task> _run;

            public Probe(string id, Func<ProbeContext, CancellationToken, Task> run)
            {
                Id = id;
                _run = run;
            }

            public string Id { get; }

            public string Area => "Test";

            public string Description => "test probe";

            public Task RunAsync(ProbeContext context, CancellationToken cancellationToken) => _run(context, cancellationToken);
        }
    }
}
