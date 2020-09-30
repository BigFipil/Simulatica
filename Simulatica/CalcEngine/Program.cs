using Microsoft.Extensions.DependencyInjection;
using System;

namespace CalcEngine
{
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
                }
            }

            //Console.WriteLine(provider.GetService<ParticleBlueprint>().ToString());


            Test t = new Test();
            t.testDeseri();
            //provider.GetRequiredService<SimulationConfig>().testDeseri();
            //TODO: system uruchomieniowy

            //Console.WriteLine(provider.GetService<MyTypeBuilder>().ToString());
            //Console.WriteLine(provider.GetService<SimulationConfig>().ToString());
        }
    }
}
