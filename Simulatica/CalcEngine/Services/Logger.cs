using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CalcEngine
{
    class Logger : ILogger
    {
        private SimulationConfig Config;
        private SimulationState State;
        public string loggerPath = "";

        public Logger(SimulationConfig config, SimulationState state)
        {
            Config = config;
            State = state;

            loggerPath = Path.GetDirectoryName(Path.GetFullPath(Config.Path)) + "\\SimLog.txt";
            if (loggerPath == null) loggerPath = "";

            File.WriteAllText(loggerPath, "\t\t\tSTANDARD SIMULATION LOGGER\n" +
                "\t\t\t--------------------------\n\n"+
                "\tDate: "+ DateTime.Now.ToString() +"\n"+
                "\tHardware information:\n" +
                "\t\t- Machine name:  " + Environment.MachineName
                +"\n\t\t- Real processor count: " +Environment.ProcessorCount
                +"\n\t\t- System: "+Environment.OSVersion.ToString()
                + "\n\tWirtual Assistent: false (function not implemented yet)"
                + "\n\tWirtual Assistent name: null"
                + "\n\n\n\tPath to simulation configuration file: " + Config.Path
                + "\n\tPath to Visualizer.exe: " + Config.PathToVisualiserEXE
                + "\n\tPath to output: " + Config.OutputPath);

            if(State.ErrorList.Count != 0)
            {
                string s = "\n\n\tThere were some loading error: \n";
                foreach (var v in State.ErrorList) s += "\t\t"+v.Message+"\n";
                File.AppendAllText(loggerPath, s);
            }
        }

        public void Add(string text)
        {
            File.AppendAllText(loggerPath, text);
        }

        public void Add(string text, object[] obj)
        {
            throw new NotImplementedException();
        }
    }
}
