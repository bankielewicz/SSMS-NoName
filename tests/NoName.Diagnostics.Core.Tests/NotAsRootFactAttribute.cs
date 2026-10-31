using System;
using System.Runtime.InteropServices;
using Xunit;

namespace NoName.Diagnostics.Core.Tests
{
    /// <summary>
    /// A fact that is reported as skipped, with the reason, when a Unix process is privileged (uid 0, whatever the account is called).
    /// Root ignores file and directory permissions, so a test that relies on them to make a
    /// delete fail can't run there; the skip keeps that visible in the results.
    /// </summary>
    public sealed class NotAsRootFactAttribute : FactAttribute
    {
        public NotAsRootFactAttribute()
        {
#if NET8_0_OR_GREATER
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.IsPrivilegedProcess)
                Skip = "Running as root: root ignores directory permissions, so a delete can't be made to fail.";
#endif
            // Environment.IsPrivilegedProcess needs .NET 6+. The net48 target is built and run only on
            // Windows, where a test is never skipped for being root, so nothing is lost without it.
        }
    }
}
