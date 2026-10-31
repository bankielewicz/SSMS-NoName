using System;
using System.Collections.Generic;
using System.Reflection;
using NoName.Diagnostics.Core;
using Xunit;

namespace NoName.Diagnostics.Core.Tests
{
    public class ExceptionInfoTests
    {
        [Fact]
        public void Reflection_wrapper_is_unwrapped_to_the_real_error()
        {
            var wrapped = new TargetInvocationException(new InvalidOperationException("proxy is null"));

            IDictionary<string, object> info = ExceptionInfo.From(wrapped);

            Assert.Equal("System.InvalidOperationException", info["type"]);
            Assert.Equal("proxy is null", info["message"]);
        }

        [Fact]
        public void HResult_is_logged_in_hex()
        {
            var ex = new System.Runtime.InteropServices.COMException("no service", unchecked((int)0x80004005));

            IDictionary<string, object> info = ExceptionInfo.From(ex);

            Assert.Equal("0x80004005", info["hresult"]);
        }

        [Fact]
        public void Aggregate_with_several_errors_lists_each_one()
        {
            var agg = new AggregateException(new ArgumentException("a"), new TimeoutException("b"));

            IDictionary<string, object> info = ExceptionInfo.From(agg);

            var inners = Assert.IsAssignableFrom<IList<object>>(info["inners"]);
            Assert.Equal(2, inners.Count);
        }

        [Fact]
        public void Aggregate_with_one_error_is_unwrapped()
        {
            var agg = new AggregateException(new TimeoutException("slow"));

            Assert.Equal("System.TimeoutException", ExceptionInfo.From(agg)["type"]);
        }

        [Fact]
        public void Inner_exception_is_included()
        {
            var ex = new InvalidOperationException("outer", new ArgumentNullException("arg"));

            var inner = Assert.IsAssignableFrom<IDictionary<string, object>>(ExceptionInfo.From(ex)["inner"]);
            Assert.Equal("System.ArgumentNullException", inner["type"]);
        }

        [Fact]
        public void Null_gives_null()
        {
            Assert.Null(ExceptionInfo.From(null));
        }

        [Fact]
        public void Unwrapping_stops_after_ten_wrappers()
        {
            // Ten wrappers are all removed: only the real error is logged.
            IDictionary<string, object> ten = ExceptionInfo.From(Wrapped(10));
            Assert.Equal("System.InvalidOperationException", ten["type"]);
            Assert.False(ten.ContainsKey("inner"));

            // An eleventh is left in place, so it is logged with the real error beneath it.
            foreach (int count in new[] { 11, 12 })
            {
                IDictionary<string, object> info = ExceptionInfo.From(Wrapped(count));
                Assert.Equal("System.Reflection.TargetInvocationException", info["type"]);
                var inner = Assert.IsAssignableFrom<IDictionary<string, object>>(info["inner"]);
                Assert.Equal("System.InvalidOperationException", inner["type"]);
                Assert.False(inner.ContainsKey("inner"));
            }
        }

        private static Exception Wrapped(int wrappers)
        {
            Exception ex = new InvalidOperationException("core");
            for (int i = 0; i < wrappers; i++)
                ex = new TargetInvocationException(ex);
            return ex;
        }

        [Fact]
        public void Thrown_exception_includes_a_stack()
        {
            Exception caught;
            try
            {
                throw new InvalidOperationException("boom");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.Contains(nameof(Thrown_exception_includes_a_stack), (string)ExceptionInfo.From(caught)["stack"]);
        }
    }
}
