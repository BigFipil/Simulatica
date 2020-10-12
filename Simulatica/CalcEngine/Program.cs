using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

            //Test t = provider.GetService<Test>();
            //t.testSeri();
            ISimulation simulation = provider.GetService<ISimulation>();

            simulation.Run();

            //var t = provider.GetService<Test>();
            //t.testDeseri();

            //InMemoryCompiler.Program22.Main22(provider.GetService<Services.Emitter>());
            //provider.GetService<Emitter>().CompileParticlesBlueprints();
            //provider.GetService<Services.Emitter>().Test();


            //Console.WriteLine(provider.GetService<MyTypeBuilder>().ToString());
            //Console.WriteLine(provider.GetService<SimulationConfig>().ToString());
        }
    }
}
