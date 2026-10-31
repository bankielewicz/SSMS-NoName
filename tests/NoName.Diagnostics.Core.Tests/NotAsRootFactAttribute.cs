using System;
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
            if (!OperatingSystem.IsWindows() && Environment.IsPrivilegedProcess)
                Skip = "Running as root: root ignores directory permissions, so a delete can't be made to fail.";
        }
    }
}
