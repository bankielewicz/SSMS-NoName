using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using NoName.Diagnostics.Core;
using Xunit;

namespace NoName.Diagnostics.Core.Tests
{
    public class JsonTests
    {
        [Fact]
        public void String_with_quotes_backslashes_and_control_characters_round_trips()
        {
            string value = "a\"b\\c\nd\te\u0001f" + (char)0x2028 + "g"; // U+2028 LINE SEPARATOR

            string json = Json.Serialize(value);

            Assert.Equal(value, JsonDocument.Parse(json).RootElement.GetString());
            Assert.DoesNotContain("\n", json);
        }

        [Fact]
        public void Nested_dictionaries_and_lists_keep_structure()
        {
            var value = new Dictionary<string, object>
            {
                ["name"] = "probe",
                ["count"] = 3,
                ["ok"] = true,
                ["missing"] = null,
                ["items"] = new List<object> { 1, "two", new Dictionary<string, object> { ["k"] = "v" } },
            };

            JsonElement root = JsonDocument.Parse(Json.Serialize(value)).RootElement;

            Assert.Equal("probe", root.GetProperty("name").GetString());
            Assert.Equal(3, root.GetProperty("count").GetInt32());
            Assert.True(root.GetProperty("ok").GetBoolean());
            Assert.Equal(JsonValueKind.Null, root.GetProperty("missing").ValueKind);
            Assert.Equal(3, root.GetProperty("items").GetArrayLength());
            Assert.Equal("v", root.GetProperty("items")[2].GetProperty("k").GetString());
        }

        [Fact]
        public void Numbers_use_invariant_culture()
        {
            CultureInfo saved = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                Assert.Equal("1.5", Json.Serialize(1.5));
                Assert.Equal("1234567", Json.Serialize(1234567L));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saved;
            }
        }

        [Fact]
        public void Non_finite_doubles_become_strings_so_the_line_stays_valid()
        {
            Assert.Equal("\"NaN\"", Json.Serialize(double.NaN));
            Assert.Equal("\"Infinity\"", Json.Serialize(double.PositiveInfinity));
        }

        [Fact]
        public void Enums_dates_and_guids_are_strings()
        {
            var when = new DateTimeOffset(2026, 10, 30, 12, 0, 0, TimeSpan.Zero);
            var id = new Guid("33f13ac3-80bb-4ecb-85bc-225435603a5e");

            Assert.Equal("\"Timeout\"", Json.Serialize(ProbeStatus.Timeout));
            Assert.Equal("\"2026-10-30T12:00:00.0000000+00:00\"", Json.Serialize(when));
            Assert.Equal("\"33f13ac3-80bb-4ecb-85bc-225435603a5e\"", Json.Serialize(id));
        }

        [Fact]
        public void Null_serializes_as_null()
        {
            Assert.Equal("null", Json.Serialize(null));
        }

        [Fact]
        public void Control_characters_and_line_separators_are_escaped_and_round_trip()
        {
            // Every control character, plus U+2028 LINE SEPARATOR and U+2029 PARAGRAPH SEPARATOR.
            var all = new StringBuilder("a\rb\bc\fd");
            for (char c = '\0'; c < ' '; c++)
                all.Append(c);
            all.Append((char)0x2028).Append((char)0x2029);
            string value = all.ToString();

            string json = Json.Serialize(value);

            Assert.Equal(value, JsonDocument.Parse(json).RootElement.GetString());

            // A round trip also holds for raw characters, so check the text itself: nothing that
            // line-based readers split on may appear unescaped, or one log entry would span lines.
            Assert.False(json.Any(ch => ch < ' ' || ch == (char)0x2028 || ch == (char)0x2029),
                "the serialized text must contain no raw control or line/paragraph separator characters");
            Assert.Contains("\\u2028", json);
            Assert.Contains("\\u2029", json);

            string expected = "\"a\\rb\\bc\\fd"
                + "\\u0000\\u0001\\u0002\\u0003\\u0004\\u0005\\u0006\\u0007"
                + "\\b\\t\\n\\u000b\\f\\r"
                + "\\u000e\\u000f\\u0010\\u0011\\u0012\\u0013\\u0014\\u0015\\u0016\\u0017"
                + "\\u0018\\u0019\\u001a\\u001b\\u001c\\u001d\\u001e\\u001f"
                + "\\u2028\\u2029\"";
            Assert.Equal(expected, json, ignoreCase: true); // hex digits may be either case
        }

        [Fact]
        public void Chars_dates_floats_and_decimals_use_invariant_text()
        {
            // de-DE writes decimals with a comma ("1,10"), so a value formatted with the current
            // culture instead of the invariant one would show up here.
            CultureInfo saved = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("de-DE");
                Assert.Equal("1,10", 1.10m.ToString()); // the culture really is in effect

                Assert.Equal("\"x\"", Json.Serialize('x'));
                Assert.Equal("\"2026-10-30T12:00:00.0000000Z\"", Json.Serialize(new DateTime(2026, 10, 30, 12, 0, 0, DateTimeKind.Utc)));
                Assert.Equal("0.25", Json.Serialize(0.25f));
                Assert.Equal("1.10", Json.Serialize(1.10m));
            }
            finally
            {
                CultureInfo.CurrentCulture = saved;
            }
        }

        [Fact]
        public void Non_generic_dictionary_keys_become_strings()
        {
            var table = new System.Collections.Hashtable { [7] = "seven" };

            Assert.Equal("{\"7\":\"seven\"}", Json.Serialize(table));
        }

        [Fact]
        public void Other_objects_are_written_with_ToString()
        {
            Assert.Equal("\"1.2.3\"", Json.Serialize(new Version(1, 2, 3)));
        }

        [Fact]
        public void Nesting_beyond_the_limit_is_cut_off_but_stays_valid()
        {
            object value = "leaf";
            for (int i = 0; i < 40; i++)
                value = new List<object> { value };

            string json = Json.Serialize(value);

            JsonDocument.Parse(json);
            Assert.Contains("[max depth]", json);
        }
    }
}
