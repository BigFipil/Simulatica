using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public class ParticleBlueprint
    {
        public string Name { get; set; } = "";
        public ulong Quantity { get; set; }
        public Dictionary<string, string> properties { get; set; }
        public Dictionary<string, string> methods { get; set; }
    }
}
