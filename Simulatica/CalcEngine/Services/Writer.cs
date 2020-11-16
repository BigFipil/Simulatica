﻿using System;
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
                Console.WriteLine("debug1");
                Config.OutputPath += "\\";
            }
            else
            {
                Console.WriteLine("debug2");
                if (!Directory.Exists(Path.GetFullPath(Config.OutputPath)))
                {
                    Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath));
                }
                else
                {
                    Directory.Delete(Path.GetFullPath(Config.OutputPath), true);
                    Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath));
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
