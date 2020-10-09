using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalcEngine.Services
{
    public class SimulationLoader : ILoader
    {
        private Simulation s1;
        private SmallSimulation s2;
        private SimulationState state;

        public SimulationLoader(Simulation sim, SmallSimulation small, SimulationState st)
        {
            s1 = sim;
            s2 = small;
            state = st;
        }

        public bool Load(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);

                //jsonString.IndexOf("FullRamMode:")
                if(jsonString.Contains(@"FullRamMode"":true"))
                {
                    JsonConvert.PopulateObject(jsonString, s2);
                }
                else
                {
                    JsonConvert.PopulateObject(jsonString, s1);
                }

                //Console.WriteLine(config.ToString());
                return true;
            }
            catch (Exception ex)
            {
                state.ErrorList.Add(ex);
                return false;
            }
        }
    }
}
