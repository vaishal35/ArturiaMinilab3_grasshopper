using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace MinilabPlugin
{
    public class MinilabPluginInfo : GH_AssemblyInfo
    {
        public override string Name => "MinilabPlugin";
        public override Bitmap Icon => null;
        public override string Description => "Listen to Arturia Minilab 3 MIDI messages.";
        public override Guid Id => new Guid("7D9F7A77-45EE-4BD7-963C-58B3F5FF1A01");
        public override string AuthorName => "OpenAI Codex";
        public override string AuthorContact => "";
    }
}
