using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalcEngine
{
    public class ConfigLoader : ILoader
    {
        private SimulationConfig config;
        private SimulationState state;

        public ConfigLoader(SimulationConfig Config, SimulationState State)
        {
            config = Config;
            state = State;
        }

        public bool Load(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                JsonConvert.PopulateObject(jsonString, config);
                //Console.WriteLine(config.ToString());
                return true;
            }
            catch(Exception ex)
            {
                state.ErrorList.Add(ex);
                return false;
            }
        }
    }
}
