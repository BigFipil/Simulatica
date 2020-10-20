using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CalcEngine
{
    public class Writer
    {
        private readonly SimulationConfig Config;
        private readonly SimulationState State;

        public Writer(SimulationConfig C, SimulationState S)
        {
            Config = C;
            State = S;
        }

        public void Write(Object obj, ulong iteration)
        {
            Console.WriteLine(obj.ToString());
        }
    }
}
