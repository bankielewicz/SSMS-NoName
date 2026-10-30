using System;

namespace NoName.Diagnostics.Vsix
{
    /// <summary>Must match the symbols in VSCommandTable.vsct.</summary>
    internal static class PackageIds
    {
        public const string PackageGuidString = "82F933FB-7999-4A45-BDB9-D30245C261ED";
        public const string CommandSetGuidString = "8279B979-35CC-4CA7-9371-34A949C05C97";
        public const string ToolWindowGuidString = "C79105CB-64FE-4C5C-9C7B-45F0F7413D07";

        public static readonly Guid CommandSet = new Guid(CommandSetGuidString);

        public const int OpenWindow = 0x0100;
        public const int ProbeWindow = 0x0101;
        public const int CopyPrompt = 0x0102;
    }
}
