using System;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Visualizer
{
	class Program
	{
		static void Main(string[] args)
		{
			string path = "";
			string resultPath = "";

			if (args.Length > 0)
            {
				path = args[0];
            }
			if (args.Length > 1)
			{
				resultPath = args[1];
			}

			if (File.Exists(path) && path != "")
            {
				string s = File.ReadAllText(path);
				AnimationConfig Config = JsonConvert.DeserializeObject<AnimationConfig>(s);

				Console.WriteLine(Config.SimulationType);
				Console.WriteLine(Config.OutputPath);
				foreach(var v in Config.particleBlueprints)
                {
					Console.WriteLine(v.Name);
					foreach (var t in v.outputInformations) Console.WriteLine("\t" + t.Key + " " + t.Value);
                }

				if (resultPath != "") Config.OutputPath = resultPath;

				Game game = null;

				if (Config.SimulationType == "3D")
				{
					game = new Visual3D(Config);
				}
				else
				{
					game = new Visual2D(Config);
				}

				game.Run();

			}
            else
            {
				Console.WriteLine("File '.conig' can not be found in: "+path);
            }
		}
	}
}