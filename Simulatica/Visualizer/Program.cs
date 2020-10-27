using System;
using System.IO;
using Newtonsoft.Json;

namespace Visualizer
{
	class Program
	{
		static void Main(string[] args)
		{
			string path = "";

			if(args.Length > 0)
            {
				path = args[0];
            }

            if (File.Exists(path) && path != "")
            {
				string s = File.ReadAllText(path);
				AnimationConfig Config = JsonConvert.DeserializeObject<AnimationConfig>(s);

				Console.WriteLine(Config.SimulationBoxSize);
				Console.WriteLine(Config.OutputPath);

				using (var app = new App(Config))
					app.Run();
			}
            else
            {
				Console.WriteLine("File '.conig' can not be found in: "+path);
            }
		}
	}
}