using Microsoft.Extensions.DependencyInjection;
using System;

namespace CalcEngine
{
    //TODO: Emiter zamienić na ClassicEmitter, który implementuje interfejs. w przyszłości może dojść jeszcze Emitter Refleksyjny, 
    //i clasa CodeProvider która wyznaczy odpowiedni serwis z DI contenter.

    /*
     * Każda cząstka będzie sprawdzana z każdą, nawet sobą samą. to domyślnie. Zapobieganie będzie w bluprincie.
     * porónywanie kolizji będzie domyślnie w silniku, w bluprincie będzie można przeciążyć metodę wywoływaną w trakcie kolizji
     */
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();

            ServiceProvider provider = startup.ConfigureServices(services);


            if(args.Length > 0)
            {
                if(Char.IsDigit(args[0][0]))
                {
                    startup.Port = Int32.Parse(args[0]);
                }
                else
                {
                    startup.Path = args[0];

                    if (!provider.GetService<Services.Loader>().Load(startup.Path))
                    {
                        var st = provider.GetService<SimulationState>();
                        Console.WriteLine("Loading Exeption. Error message:\n\n\t{0}", st.ErrorList[st.ErrorList.Count - 1]);

                        return;
                    }
                }
            }

            //Console.WriteLine(provider.GetService<ParticleBlueprint>().ToString());

            
            var t = provider.GetService<Test>();
            t.testDeseri();
            
            InMemoryCompiler.Program22.Main22();
            //provider.GetService<Services.Emitter>().CompileParticlesBlueprints();
            
            //provider.GetRequiredService<SimulationConfig>().testDeseri();
            //TODO: system uruchomieniowy

            //Console.WriteLine(provider.GetService<MyTypeBuilder>().ToString());
            //Console.WriteLine(provider.GetService<SimulationConfig>().ToString());
        }
    }
}
