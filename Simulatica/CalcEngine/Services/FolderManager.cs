using System;
using System.IO;


namespace CalcEngine
{
    public class FolderManager : IFolderManager
    {
        private SimulationConfig Config;
        private SimulationState State;
        public FolderManager(SimulationConfig c, SimulationState s)
        {
            Config = c;
            State = s;
        }

        public bool Create()
        {
            try
            {
                if (Config.OutputPath == "")
                {
                    Exception e = new Exception("FolderManager service - Unable to create simulation output folder. Output path is empty.");
                    State.ErrorList.Add(e);

                    return false;
                }
                else
                {
                    if (!Directory.Exists(Path.GetFullPath(Config.OutputPath)))
                    {
                        Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath));
                    }

                    if (Directory.Exists(Path.GetFullPath(Config.OutputPath) + "\\Data"))
                    {
                        Directory.Delete(Path.GetFullPath(Config.OutputPath) + "\\Data", true);
                    }

                    Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath) + "\\Data");

                    return true;
                }
            }
            catch (Exception e)
            {
                State.ErrorList.Add(e);
                return false;
            }
        }
    }
}
