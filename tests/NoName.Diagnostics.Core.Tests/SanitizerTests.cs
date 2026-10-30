using System.Collections.Generic;
using NoName.Diagnostics.Core;
using Xunit;

namespace NoName.Diagnostics.Core.Tests
{
    public class SanitizerTests
    {
        [Fact]
        public void Shape_reports_length_and_a_stable_sha256_prefix()
        {
            IDictionary<string, object> shape = Sanitizer.Shape("hello");

            Assert.Equal(5, shape["len"]);
            Assert.Equal("2cf24dba5fb0a30e", shape["sha256"]);
        }

        [Fact]
        public void Text_logs_the_shape_by_default()
        {
            object logged = new Sanitizer(verbose: false).Text("SELECT secret FROM t");

            var shape = Assert.IsAssignableFrom<IDictionary<string, object>>(logged);
            Assert.Equal(20, shape["len"]);
        }

        [Fact]
        public void Text_logs_the_value_in_verbose_mode()
        {
            Assert.Equal("SELECT 1", new Sanitizer(verbose: true).Text("SELECT 1"));
        }

        [Fact]
        public void Text_keeps_null_as_null()
        {
            Assert.Null(new Sanitizer(verbose: false).Text(null));
        }

        [Fact]
        public void Connection_string_password_is_removed_even_in_verbose_mode()
        {
            const string cs = "Server=prod01;Database=Payroll;User ID=sa;Password=hunter2;Encrypt=True";

            IDictionary<string, object> logged = new Sanitizer(verbose: true).ConnectionString(cs);

            Assert.Equal(Sanitizer.Removed, logged["password"]);
            Assert.Equal("prod01", logged["server"]);
            Assert.DoesNotContain("hunter2", Json.Serialize(logged));
        }

        [Fact]
        public void Connection_string_values_are_hashed_by_default_but_safe_settings_stay_readable()
        {
            const string cs = "Data Source=prod01;Initial Catalog=Payroll;Integrated Security=True;Encrypt=True";

            IDictionary<string, object> logged = new Sanitizer(verbose: false).ConnectionString(cs);

            Assert.Equal("sha256:" + Sanitizer.Hash("prod01"), logged["data source"]);
            Assert.Equal("sha256:" + Sanitizer.Hash("Payroll"), logged["initial catalog"]);
            Assert.Equal("True", logged["integrated security"]);
            Assert.Equal("True", logged["encrypt"]);
        }

        [Fact]
        public void Connection_string_quoted_values_may_contain_semicolons()
        {
            const string cs = "Password=\"a;b=c\";Server='x;y'";

            IDictionary<string, object> logged = new Sanitizer(verbose: true).ConnectionString(cs);

            Assert.Equal(2, logged.Count);
            Assert.Equal(Sanitizer.Removed, logged["password"]);
            Assert.Equal("x;y", logged["server"]);
        }

        [Theory]
        [InlineData("Password")]
        [InlineData("pwd")]
        [InlineData(" PWD ")]
        [InlineData("AccessToken")]
        [InlineData("Access Token")]
        [InlineData("Client Secret")]
        public void Secret_keys_are_recognized_regardless_of_case_and_spacing(string key)
        {
            Assert.True(Sanitizer.IsSecretKey(key));
        }

        [Fact]
        public void Ordinary_keys_are_not_secret()
        {
            Assert.False(Sanitizer.IsSecretKey("Server"));
            Assert.False(Sanitizer.IsSecretKey("User ID"));
        }

        [Fact]
        public void Null_and_empty_input_are_handled()
        {
            Assert.Null(Sanitizer.Shape(null));
            Assert.Empty(new Sanitizer(verbose: false).ConnectionString(null));
            Assert.Empty(new Sanitizer(verbose: false).ConnectionString(""));
            Assert.False(Sanitizer.IsSecretKey(null));
        }

        [Fact]
        public void Connection_string_tolerates_empty_segments_bare_flags_and_missing_keys()
        {
            IDictionary<string, object> logged = new Sanitizer(verbose: true)
                .ConnectionString(";;Server = x ;  ;Flag;=orphan;Database=d;;");

            Assert.Equal(3, logged.Count);
            Assert.Equal("x", logged["server"]);
            Assert.Equal(string.Empty, logged["flag"]);
            Assert.Equal("d", logged["database"]);
        }

        [Fact]
        public void Connection_string_doubled_quotes_are_literal_and_text_after_a_closing_quote_is_ignored()
        {
            IDictionary<string, object> logged = new Sanitizer(verbose: true)
                .ConnectionString("Application Name='O''Brien''s tool';Server=\"a\"\"b\";Database='db' trailing;Encrypt=True");

            Assert.Equal("O'Brien's tool", logged["application name"]);
            Assert.Equal("a\"b", logged["server"]);
            Assert.Equal("db", logged["database"]);
            Assert.Equal("True", logged["encrypt"]);
        }
    }
}
