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

            if(Config.OutputPath == "")
            {
                Config.OutputPath = Path.GetFullPath(Config.Path).Replace(Config.Path, "Result");
                if (!Directory.Exists(Config.OutputPath))
                {
                    Directory.CreateDirectory(Config.OutputPath);
                }

                Config.OutputPath += "\\";
            }
            else
            {
                if (!Directory.Exists(Config.OutputPath))
                {
                    Directory.CreateDirectory(Config.OutputPath);
                }
                else
                {
                    Directory.Delete(Config.OutputPath, true);
                    Directory.CreateDirectory(Config.OutputPath);
                }

                Config.OutputPath += "\\";
            }
        }

        public void Write(Object obj, ulong iteration)
        {
            File.AppendAllText(Config.OutputPath+"T="+iteration*(float)Config.SimulationStepTime+".txt", obj.ToString()+"\n");
        }
    }
}
