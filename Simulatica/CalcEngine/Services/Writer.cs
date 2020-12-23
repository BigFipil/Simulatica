using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace CalcEngine
{
    public class Writer
    {
        private readonly SimulationConfig Config;
        private readonly SimulationState State;

        //private string currentPath = "";
        private FileStream fstream;
        private StreamWriter swriter;

        public Writer(SimulationConfig C, SimulationState S)
        {
            Config = C;
            State = S;

        }

        public void Write(Object obj, ulong iteration)
        {
            //File.AppendAllText(currentPath, obj.ToString()+"\n");
            swriter.Write((obj.ToString() + "\n"));
            swriter.Flush();
        }

        public void NewFile(ulong iteration)
        {
            if (State.currentPath != "")
            {
                fstream.Close();
                swriter.Close();
            }

            State.currentPath = Config.OutputPath + "\\Data\\" + "T=" + iteration * (float)Config.SimulationStepTime + ".txt";

            fstream = new FileStream(State.currentPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            fstream.Close();

            swriter = new StreamWriter(State.currentPath);
        }
    }
}
