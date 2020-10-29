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

        ///<summary>
        ///This Dictionary stores information that are not neccesary to calculate simulation, but are very important during the post procesing of simulation output data.
        ///</summary>
        public Dictionary<string, string> outputInformations { get; set; }
    }
}
