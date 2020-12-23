using System;
using System.IO;

namespace CalcEngine
{
    public class MultipleFolderManager : IFolderManager
    {
        private SimulationConfig Config;
        private SimulationState State;
        public MultipleFolderManager(SimulationConfig c, SimulationState s)
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
                    Exception e = new Exception("MultipleFolderManager service - Unable to create simulation output folder. Output path is empty.");
                    State.ErrorList.Add(e);

                    return false;
                }
                else
                {
                    if (!Directory.Exists(Path.GetFullPath(Config.OutputPath)))
                    {
                        Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath));
                    }

                    for(int i = 1; i < Config.MultipleSimulationCount; i++)
                    {

                        if (!Directory.Exists(Path.GetFullPath(Config.OutputPath + "\\" + i)))
                        {
                            Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath + "\\" + i));
                        }


                        if (Directory.Exists(Path.GetFullPath(Config.OutputPath + "\\" + i) + "\\Data"))
                        {
                            Directory.Delete(Path.GetFullPath(Config.OutputPath + "\\" + i) + "\\Data", true);
                        }

                        Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath + "\\" + i) + "\\Data");
                    }

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
