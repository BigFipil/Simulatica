using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace CalcEngine
{
    class MultipleSimulation : ISimulation
    {
        [JsonProperty]
        public SimulationConfig Config { get; private set; }
        [JsonProperty]
        public SimulationState State { get; private set; }
        [JsonProperty]
        public Emitter Emitter { get; private set; }
        [JsonProperty]
        public Writer Writer { get; private set; }
        public Logger Logger { get; private set; }
        public ISimulation Simulation { get; set; }

        public MultipleSimulation(SimulationConfig C, SimulationState S, Emitter E, Writer W, Logger L)
        {
            Config = C;
            State = S;
            Emitter = E;
            Writer = W;
            Logger = L;
            //Simulation = Sim;
        }
        public void Run()
        {
            string path = Config.OutputPath;
            for (int i = 1; i < Config.MultipleSimulationCount; i++)
            {
                Logger.loggerPath = Path.GetDirectoryName(Logger.DefaultPath) + "\\" +i+ "\\MLog"+i+".txt"; // trzeba sie pozbyc rozszerzenia, bo zaburzamy przyszla kompatymilnosc
                //sciezki w loggerze musza byc bezrozszerzeniowa

                Config.OutputPath = path + "\\" + i;

                Simulation.Run();

                //var prop = Config.GetType().GetField(Config.MultipleSimulationParameter, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                //prop.SetValue(Config, "new value");

                if(Config.MultipleSimulationAction[0] == '+')
                {
                    double a = Double.Parse(Config.MultipleSimulationAction.Replace("+", ""));
                    double v = (double)typeof(SimulationConfig).GetProperty(Config.MultipleSimulationParameter).GetValue(Config);
                    typeof(SimulationConfig).GetProperty(Config.MultipleSimulationParameter).SetValue(Config, ((int)v+a), null);
                }

                if (Config.PathToVisualiserEXE != "")
                {
                    var proc = Process.Start(Config.PathToVisualiserEXE, Path.GetFullPath(Config.Path) + " " + Path.GetFullPath(Config.OutputPath));
                }
            }

            Config.PathToVisualiserEXE = "";
        }
    }
}
