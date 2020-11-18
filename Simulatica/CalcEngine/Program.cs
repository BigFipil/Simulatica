using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CalcEngine
{
    //TODO: Emiter zamienić na ClassicEmitter, który implementuje interfejs. w przyszłości może dojść jeszcze Emitter Refleksyjny, 
    //stworzyć Logger, do pliku.
    //Monogame visualisator, opróćz blendera

    /*
     * Każda cząstka będzie sprawdzana z każdą, nawet sobą samą. to domyślnie. Zapobieganie będzie w bluprincie.
     * porónywanie kolizji będzie domyślnie w silniku, w bluprincie będzie można przeciążyć metodę wywoływaną w trakcie kolizji
     * 
     * Initalize(), Calculate(Patricle type), Update().
     */
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            ServiceHandler sh = new ServiceHandler();

            ServiceProvider provider = sh.ConfigureServices(services);

            SimulationConfig config = provider.GetService<SimulationConfig>();


            if(args.Length > 0)
            {
                if(Char.IsDigit(args[0][0]))
                {
                    config.Port = Int32.Parse(args[0]);
                }
                else
                {
                    config.Path = args[0];
                    provider.GetService<ConfigLoader>().Load(config.Path);
                }
            }

            //Console.WriteLine(config);

            //Test t = provider.GetService<Test>();
            //t.testSeri2();
            ISimulation simulation = provider.GetService<ISimulation>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            simulation.Run();

            stopWatch.Stop();

            Console.WriteLine("\n\n\tElapsed Time: "+((float)stopWatch.ElapsedMilliseconds)/1000 +"[s]");


            if(config.PathToVisualiserEXE != "")
            {
                var proc = Process.Start(config.PathToVisualiserEXE, Path.GetFullPath(config.Path) + " " + Path.GetFullPath(config.OutputPath));
            }

            //var t = provider.GetService<Test>();
            //t.testDeseri();
        }
    }
}
