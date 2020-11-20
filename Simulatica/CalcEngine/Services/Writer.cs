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

        private string currentPath = "";
        private FileStream fstream;
        private StreamWriter swriter;

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
                //Console.WriteLine("debug2");
                if (!Directory.Exists(Path.GetFullPath(Config.OutputPath)))
                {
                    Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath));
                }
                else
                {
                    Directory.Delete(Path.GetFullPath(Config.OutputPath), true);
                    Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath));
                }

                Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath)+"\\Data");
                Config.OutputPath += "\\";
            }
        }

        public void Write(Object obj, ulong iteration)
        {
            //File.AppendAllText(currentPath, obj.ToString()+"\n");
            swriter.Write((obj.ToString() + "\n"));
            swriter.Flush();
        }

        public void NewFile(ulong iteration)
        {
            if (currentPath != "")
            {
                fstream.Close();
                swriter.Close();
            }

            currentPath = Config.OutputPath + "Data\\" + "T=" + iteration * (float)Config.SimulationStepTime + ".txt";

            fstream = new FileStream(currentPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            fstream.Close();

            swriter = new StreamWriter(currentPath);
        }
    }
}
