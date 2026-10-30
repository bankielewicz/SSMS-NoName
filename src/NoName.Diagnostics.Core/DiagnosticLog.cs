using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NoName.Diagnostics.Core
{
    /// <summary>
    /// One JSON Lines file per session. Each line is flushed as it is written, so a crash of the
    /// host keeps everything logged before it.
    /// </summary>
    public sealed class DiagnosticLog : IDisposable
    {
        public const string Format = "noname-diagnostics/1";

        private readonly object _gate = new object();
        private readonly Func<DateTimeOffset> _clock;
        private StreamWriter _writer;
        private long _seq;

        public DiagnosticLog(string directory, Func<DateTimeOffset> clock = null, string sessionId = null)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("A log directory is required.", nameof(directory));

            _clock = clock ?? (() => DateTimeOffset.UtcNow);
            SessionId = string.IsNullOrEmpty(sessionId) ? Guid.NewGuid().ToString("N").Substring(0, 6) : sessionId;

            Directory.CreateDirectory(directory);
            string stamp = _clock().UtcDateTime.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            FilePath = Path.Combine(directory, "session-" + stamp + "-" + SessionId + ".jsonl");

            var stream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);
            _writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true, NewLine = "\n" };

            Write("session", "start", new Dictionary<string, object> { ["format"] = Format });
        }

        public string SessionId { get; }

        public string FilePath { get; }

        /// <summary>Raised after each line is on disk; handler exceptions are ignored.</summary>
        public event Action<string> LineWritten;

        public void Write(string kind, string name, IDictionary<string, object> data = null)
        {
            string line;
            lock (_gate)
            {
                if (_writer == null)
                    return;

                var entry = new Dictionary<string, object>
                {
                    ["ts"] = _clock(),
                    ["seq"] = ++_seq,
                    ["session"] = SessionId,
                    ["kind"] = kind,
                    ["name"] = name,
                };
                if (data != null)
                    entry["data"] = data;

                try
                {
                    line = Json.Serialize(entry);
                }
                catch (Exception ex)
                {
                    // A value whose ToString() throws must not lose the entry or break the log.
                    entry["data"] = new Dictionary<string, object> { ["serializationError"] = ExceptionInfo.From(ex) };
                    line = Json.Serialize(entry);
                }
                _writer.WriteLine(line);
            }
            OnLineWritten(line);
        }

        /// <summary>Deletes all but the newest <paramref name="keep"/> session files; returns how many were deleted.</summary>
        public static int Prune(string directory, int keep)
        {
            if (!Directory.Exists(directory))
                return 0;

            int deleted = 0;
            IEnumerable<string> oldest = Directory.GetFiles(directory, "session-*.jsonl")
                .OrderByDescending(Path.GetFileName, StringComparer.Ordinal)
                .Skip(Math.Max(keep, 0));
            foreach (string file in oldest)
            {
                try
                {
                    File.Delete(file);
                    deleted++;
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
            return deleted;
        }

        public void Dispose()
        {
            lock (_gate)
            {
                if (_writer == null)
                    return;
            }
            Write("session", "end");
            lock (_gate)
            {
                _writer.Dispose();
                _writer = null;
            }
        }

        private void OnLineWritten(string line)
        {
            try
            {
                LineWritten?.Invoke(line);
            }
            catch (Exception)
            {
                // A broken viewer must not stop logging.
            }
        }
    }
}
