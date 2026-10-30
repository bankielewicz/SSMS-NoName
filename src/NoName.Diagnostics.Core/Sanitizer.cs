using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace NoName.Diagnostics.Core
{
    /// <summary>
    /// Decides what a log may contain. By default values are logged as their shape (length and a
    /// SHA-256 prefix); verbose mode logs values. Secrets are never logged, verbose or not.
    /// </summary>
    public sealed class Sanitizer
    {
        public const string Removed = "[removed]";

        private static readonly HashSet<string> SecretKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            "password", "pwd", "accesstoken", "clientsecret",
        };

        // Connection-string settings that describe behaviour, not identity, so they stay readable.
        private static readonly HashSet<string> SafeKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            "integrated security", "trusted_connection", "encrypt", "trustservercertificate",
            "trust server certificate", "authentication", "connect timeout", "connection timeout",
            "timeout", "command timeout", "multipleactiveresultsets", "multiple active result sets",
            "pooling", "min pool size", "max pool size", "packet size", "applicationintent",
            "application intent", "multisubnetfailover", "multi subnet failover",
            "column encryption setting", "persist security info", "connect retry count",
            "connect retry interval", "application name", "app", "type system version",
        };

        public Sanitizer(bool verbose)
        {
            Verbose = verbose;
        }

        public bool Verbose { get; }

        /// <summary>First 16 hex characters of the SHA-256 of the UTF-8 text.</summary>
        public static string Hash(string value)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] digest = sha.ComputeHash(Encoding.UTF8.GetBytes(value ?? string.Empty));
                var sb = new StringBuilder(16);
                for (int i = 0; i < 8; i++)
                    sb.Append(digest[i].ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }

        public static IDictionary<string, object> Shape(string value)
        {
            if (value == null)
                return null;
            return new Dictionary<string, object> { ["len"] = value.Length, ["sha256"] = Hash(value) };
        }

        public object Text(string value)
        {
            if (value == null)
                return null;
            return Verbose ? (object)value : Shape(value);
        }

        public IDictionary<string, object> ConnectionString(string connectionString)
        {
            var logged = new Dictionary<string, object>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(connectionString))
                return logged;

            foreach (KeyValuePair<string, string> pair in ParseConnectionString(connectionString))
            {
                string key = NormalizeKey(pair.Key);
                if (IsSecretKey(key))
                    logged[key] = Removed;
                else if (Verbose || SafeKeys.Contains(key))
                    logged[key] = pair.Value;
                else
                    logged[key] = "sha256:" + Hash(pair.Value);
            }
            return logged;
        }

        public static bool IsSecretKey(string key)
        {
            if (key == null)
                return false;
            return SecretKeys.Contains(NormalizeKey(key).Replace(" ", string.Empty));
        }

        private static string NormalizeKey(string key)
        {
            var sb = new StringBuilder(key.Length);
            bool space = false;
            foreach (char c in key.Trim())
            {
                if (char.IsWhiteSpace(c))
                {
                    space = true;
                    continue;
                }
                if (space && sb.Length > 0)
                    sb.Append(' ');
                space = false;
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        /// <summary>Splits <c>key=value;</c> pairs; values may be quoted with " or ' (doubled to escape).</summary>
        internal static IEnumerable<KeyValuePair<string, string>> ParseConnectionString(string s)
        {
            int i = 0;
            int n = s.Length;
            while (i < n)
            {
                while (i < n && (s[i] == ';' || char.IsWhiteSpace(s[i])))
                    i++;
                if (i >= n)
                    yield break;

                int keyStart = i;
                while (i < n && s[i] != '=' && s[i] != ';')
                    i++;
                string key = s.Substring(keyStart, i - keyStart).Trim();
                if (i >= n || s[i] == ';')
                {
                    if (key.Length > 0)
                        yield return new KeyValuePair<string, string>(key, string.Empty);
                    continue;
                }

                i++; // '='
                while (i < n && s[i] != ';' && char.IsWhiteSpace(s[i]))
                    i++;

                string value;
                if (i < n && (s[i] == '"' || s[i] == '\''))
                {
                    char quote = s[i++];
                    var sb = new StringBuilder();
                    while (i < n)
                    {
                        if (s[i] == quote)
                        {
                            if (i + 1 < n && s[i + 1] == quote)
                            {
                                sb.Append(quote);
                                i += 2;
                                continue;
                            }
                            i++;
                            break;
                        }
                        sb.Append(s[i++]);
                    }
                    value = sb.ToString();
                    while (i < n && s[i] != ';')
                        i++;
                }
                else
                {
                    int valueStart = i;
                    while (i < n && s[i] != ';')
                        i++;
                    value = s.Substring(valueStart, i - valueStart).Trim();
                }

                if (key.Length > 0)
                    yield return new KeyValuePair<string, string>(key, value);
            }
        }
    }
}
