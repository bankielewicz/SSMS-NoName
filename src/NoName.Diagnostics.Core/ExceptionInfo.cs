using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NoName.Diagnostics.Core
{
    /// <summary>Flattens an exception into loggable fields, unwrapping reflection and task wrappers.</summary>
    public static class ExceptionInfo
    {
        private const int MaxDepth = 4;
        private const int MaxStackLines = 25;

        public static IDictionary<string, object> From(Exception exception) => From(exception, 0);

        private static IDictionary<string, object> From(Exception exception, int depth)
        {
            if (exception == null)
                return null;

            Exception ex = Unwrap(exception);
            var info = new Dictionary<string, object>
            {
                ["type"] = ex.GetType().FullName,
                ["message"] = ex.Message,
                ["hresult"] = "0x" + ex.HResult.ToString("X8", CultureInfo.InvariantCulture),
            };

            if (!string.IsNullOrEmpty(ex.StackTrace))
                info["stack"] = string.Join("\n", ex.StackTrace.Split('\n').Take(MaxStackLines).Select(l => l.TrimEnd('\r')));

            if (depth < MaxDepth)
            {
                if (ex is AggregateException aggregate && aggregate.InnerExceptions.Count > 1)
                    info["inners"] = aggregate.InnerExceptions.Select(e => (object)From(e, depth + 1)).ToList();
                else if (ex.InnerException != null)
                    info["inner"] = From(ex.InnerException, depth + 1);
            }
            return info;
        }

        private static Exception Unwrap(Exception ex)
        {
            for (int i = 0; i < 10; i++)
            {
                if (ex is TargetInvocationException invocation && invocation.InnerException != null)
                    ex = invocation.InnerException;
                else if (ex is AggregateException aggregate && aggregate.InnerExceptions.Count == 1)
                    ex = aggregate.InnerExceptions[0];
                else
                    break;
            }
            return ex;
        }
    }
}
