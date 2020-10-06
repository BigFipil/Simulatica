using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcEngine
{
    //TODO: Emiter zamienić na ClassicEmitter, który implementuje interfejs. w przyszłości może dojść jeszcze Emitter Refleksyjny, 
    //stworzyć własny DI contener

    /*
     * Każda cząstka będzie sprawdzana z każdą, nawet sobą samą. to domyślnie. Zapobieganie będzie w bluprincie.
     * porónywanie kolizji będzie domyślnie w silniku, w bluprincie będzie można przeciążyć metodę wywoływaną w trakcie kolizji
     */
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            ServiceHandler sh = new ServiceHandler();

            ServiceProvider provider = sh.ConfigureServices(services);


            if(args.Length > 0)
            {
                if(Char.IsDigit(args[0][0]))
                {
                    sh.Port = Int32.Parse(args[0]);
                }
                else
                {
                    sh.Path = args[0];

                    if (!provider.GetService<Loader>().Load(sh.Path))
                    {
                        var st = provider.GetService<SimulationState>();
                        Console.WriteLine("Loading Exeption. Error message:\n\n\t{0}", st.ErrorList[st.ErrorList.Count - 1]);

                        return;
                    }
                }
            }

            IEnumerable<ISimulation> SimulationTypes = provider.GetServices<ISimulation>();
            ISimulation simulation;

            if (provider.GetService<SimulationConfig>().FullRamMode)
            {
                simulation = SimulationTypes.FirstOrDefault(h => h.GetType().Name == "SmallSimulation");
            }
            else
            {
                simulation = SimulationTypes.FirstOrDefault(h => h.GetType().Name == "Simulation");
            }

            simulation?.Run();

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
