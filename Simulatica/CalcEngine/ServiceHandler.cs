using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{

    public class ServiceHandler
    {
        public string Path { get; set; } = "";  //do wywalenia
        public int Port { get; set; } = 6060;   //Default Port do wywalenia

        public ServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SimulationConfig>();
            services.AddSingleton<SimulationState>();
            //services.AddSingleton<ParticleBlueprint>();

            services.AddTransient<Test>();
            services.AddTransient<ConfigLoader>();
            services.AddTransient<Emitter>();
            services.AddTransient<SmallSimulation>();
            services.AddTransient<Simulation>();
            services.AddSingleton<ISimulation>(sim);

            return services.BuildServiceProvider();
        }




        Func<IServiceProvider, ISimulation> sim = (var) => {

            var config = var.GetService<SimulationConfig>();

            if (config.FullRamMode)
            {
                Console.WriteLine("really");
                return var.GetService<SmallSimulation>();
            }
            else
            {
                return var.GetService<Simulation>();
            }

        };
    }
}
