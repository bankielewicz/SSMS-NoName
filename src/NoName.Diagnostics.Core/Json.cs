using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NoName.Diagnostics.Core
{
    /// <summary>Dependency-free JSON writer for log lines (SSMS loads its own System.Text.Json).</summary>
    public static class Json
    {
        private const int MaxDepth = 32;

        public static string Serialize(object value)
        {
            var sb = new StringBuilder();
            Write(sb, value, 0);
            return sb.ToString();
        }

        private static void Write(StringBuilder sb, object value, int depth)
        {
            if (depth > MaxDepth)
            {
                WriteString(sb, "[max depth]");
                return;
            }

            switch (value)
            {
                case null:
                    sb.Append("null");
                    return;
                case string s:
                    WriteString(sb, s);
                    return;
                case bool b:
                    sb.Append(b ? "true" : "false");
                    return;
                case char c:
                    WriteString(sb, c.ToString());
                    return;
                case Enum e:
                    WriteString(sb, e.ToString());
                    return;
                case DateTimeOffset dto:
                    WriteString(sb, dto.ToString("o", CultureInfo.InvariantCulture));
                    return;
                case DateTime dt:
                    WriteString(sb, dt.ToString("o", CultureInfo.InvariantCulture));
                    return;
                case Guid g:
                    WriteString(sb, g.ToString("D"));
                    return;
                case double d:
                    WriteDouble(sb, d);
                    return;
                case float f:
                    WriteDouble(sb, f);
                    return;
                case decimal m:
                    sb.Append(m.ToString(CultureInfo.InvariantCulture));
                    return;
                case sbyte _:
                case byte _:
                case short _:
                case ushort _:
                case int _:
                case uint _:
                case long _:
                case ulong _:
                    sb.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
                    return;
                case IDictionary<string, object> map:
                    WriteObject(sb, map, depth);
                    return;
                case IDictionary legacyMap:
                    var copy = new Dictionary<string, object>();
                    foreach (DictionaryEntry entry in legacyMap)
                        copy[Convert.ToString(entry.Key, CultureInfo.InvariantCulture)] = entry.Value;
                    WriteObject(sb, copy, depth);
                    return;
                case IEnumerable items:
                    sb.Append('[');
                    bool first = true;
                    foreach (object item in items)
                    {
                        if (!first)
                            sb.Append(',');
                        first = false;
                        Write(sb, item, depth + 1);
                    }
                    sb.Append(']');
                    return;
                default:
                    WriteString(sb, value.ToString());
                    return;
            }
        }

        private static void WriteObject(StringBuilder sb, IDictionary<string, object> map, int depth)
        {
            sb.Append('{');
            bool first = true;
            foreach (KeyValuePair<string, object> pair in map)
            {
                if (!first)
                    sb.Append(',');
                first = false;
                WriteString(sb, pair.Key ?? string.Empty);
                sb.Append(':');
                Write(sb, pair.Value, depth + 1);
            }
            sb.Append('}');
        }

        private static void WriteDouble(StringBuilder sb, double d)
        {
            if (double.IsNaN(d) || double.IsInfinity(d))
                WriteString(sb, d.ToString(CultureInfo.InvariantCulture));
            else
                sb.Append(d.ToString("R", CultureInfo.InvariantCulture));
        }

        private static void WriteString(StringBuilder sb, string s)
        {
            sb.Append('"');
            foreach (char c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    default:
                        // Control characters, and the two Unicode line separators that some line-based
                        // readers split on, are escaped so each log entry stays on one line.
                        if (c < 0x20 || c == (char)0x2028 || c == (char)0x2029)
                            sb.Append("\\u").Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                        else
                            sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }
    }
}
