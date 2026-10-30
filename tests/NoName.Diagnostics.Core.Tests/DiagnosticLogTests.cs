using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NoName.Diagnostics.Core;
using Xunit;

namespace NoName.Diagnostics.Core.Tests
{
    public sealed class DiagnosticLogTests : IDisposable
    {
        private static readonly DateTimeOffset Noon = new DateTimeOffset(2026, 10, 30, 12, 0, 0, TimeSpan.Zero);

        private readonly string _dir = Path.Combine(Path.GetTempPath(), "noname-log-tests-" + Guid.NewGuid().ToString("N"));

        public void Dispose()
        {
            if (Directory.Exists(_dir))
                Directory.Delete(_dir, recursive: true);
        }

        [Fact]
        public void Each_write_is_one_json_object_per_line_with_envelope_fields()
        {
            using (var log = new DiagnosticLog(_dir, () => Noon, "abc123"))
            {
                log.Write("probe", "PRB-OS-01", new Dictionary<string, object> { ["status"] = "Pass" });
            }

            // Lines: session start, the probe line, session end.
            string[] lines = File.ReadAllLines(Directory.GetFiles(_dir).Single());
            Assert.Equal(3, lines.Length);
            JsonElement probe = JsonDocument.Parse(lines[1]).RootElement;

            Assert.Equal("abc123", probe.GetProperty("session").GetString());
            Assert.Equal("probe", probe.GetProperty("kind").GetString());
            Assert.Equal("PRB-OS-01", probe.GetProperty("name").GetString());
            Assert.Equal("Pass", probe.GetProperty("data").GetProperty("status").GetString());
            Assert.Equal("2026-10-30T12:00:00.0000000+00:00", probe.GetProperty("ts").GetString());
            Assert.Equal("end", JsonDocument.Parse(lines[2]).RootElement.GetProperty("name").GetString());
        }

        [Fact]
        public void First_line_is_a_session_header()
        {
            using (var log = new DiagnosticLog(_dir, () => Noon, "abc123"))
            {
            }

            JsonElement first = JsonDocument.Parse(File.ReadLines(Directory.GetFiles(_dir).Single()).First()).RootElement;
            Assert.Equal("session", first.GetProperty("kind").GetString());
            Assert.Equal("start", first.GetProperty("name").GetString());
        }

        [Fact]
        public void Sequence_numbers_increase()
        {
            using (var log = new DiagnosticLog(_dir, () => Noon, "s"))
            {
                log.Write("event", "a");
                log.Write("event", "b");
            }

            long[] seqs = File.ReadAllLines(Directory.GetFiles(_dir).Single())
                .Select(l => JsonDocument.Parse(l).RootElement.GetProperty("seq").GetInt64())
                .ToArray();
            Assert.Equal(Enumerable.Range(1, seqs.Length).Select(i => (long)i), seqs);
        }

        [Fact]
        public void File_name_uses_the_clock_and_session_id()
        {
            using (var log = new DiagnosticLog(_dir, () => Noon, "abc123"))
            {
                Assert.Equal("session-20261030-120000-abc123.jsonl", Path.GetFileName(log.FilePath));
            }
        }

        [Fact]
        public void Line_is_on_disk_before_dispose()
        {
            using (var log = new DiagnosticLog(_dir, () => Noon, "s"))
            {
                log.Write("event", "flushed");

                string text;
                using (var stream = new FileStream(log.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                    text = reader.ReadToEnd();
                Assert.Contains("\"flushed\"", text);
            }
        }

        [Fact]
        public void Concurrent_writes_all_produce_valid_lines()
        {
            using (var log = new DiagnosticLog(_dir, () => Noon, "s"))
            {
                Parallel.For(0, 200, i => log.Write("event", "e" + i));
            }

            string[] lines = File.ReadAllLines(Directory.GetFiles(_dir).Single());
            Assert.Equal(202, lines.Length); // header + 200 + end
            foreach (string line in lines)
                JsonDocument.Parse(line);
        }

        [Fact]
        public void Line_written_event_receives_each_line()
        {
            var seen = new List<string>();
            using (var log = new DiagnosticLog(_dir, () => Noon, "s"))
            {
                log.LineWritten += seen.Add;
                log.Write("event", "observed");
            }

            Assert.Contains(seen, l => l.Contains("\"observed\""));
        }

        [Fact]
        public void Prune_keeps_the_newest_sessions()
        {
            Directory.CreateDirectory(_dir);
            foreach (string stamp in new[] { "20261028-090000", "20261029-090000", "20261030-090000", "20261030-100000" })
                File.WriteAllText(Path.Combine(_dir, $"session-{stamp}-x.jsonl"), "{}");
            File.WriteAllText(Path.Combine(_dir, "notes.txt"), "keep me");

            int deleted = DiagnosticLog.Prune(_dir, keep: 2);

            Assert.Equal(2, deleted);
            Assert.Equal(
                new[] { "notes.txt", "session-20261030-090000-x.jsonl", "session-20261030-100000-x.jsonl" },
                Directory.GetFiles(_dir).Select(Path.GetFileName).OrderBy(n => n, StringComparer.Ordinal));
        }
    }
}
