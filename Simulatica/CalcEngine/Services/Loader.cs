using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalcEngine
{
    public class Loader
    {
        private SimulationConfig config;
        private SimulationState state;

        public Loader(SimulationConfig Config, SimulationState State)
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
