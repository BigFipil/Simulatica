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
        public string DefaultPath { get; protected set; }

        public Logger(SimulationConfig config, SimulationState state)
        {
            Config = config;
            State = state;

            DefaultPath = Path.GetFullPath(Config.OutputPath) + "\\SimLog.txt";
            if (DefaultPath == null) DefaultPath = "";

            loggerPath = DefaultPath;

            File.WriteAllText(loggerPath, "\t\t\tSIMULATION LOGGER(STANDARD)\n" +
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
        public void Add(string text, string path)
        {
            //This method should not be used in MultipleSimulation mode, for changing the loggerpath. The logerPath should be changed directly instead.
            File.AppendAllText(path, text);
        }

        public void Add(string text, object[] obj)
        {
            throw new NotImplementedException();
        }
    }
}
