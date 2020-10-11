using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{

    public class ServiceHandler
    {
        public ServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SimulationConfig>();
            services.AddSingleton<SimulationState>();
            //services.AddSingleton<ParticleBlueprint>();

            services.AddTransient<Test>();
            services.AddTransient<ConfigLoader>();
            services.AddTransient<SimulationLoader>();
            services.AddTransient<Emitter>();
            services.AddTransient<SmallSimulation>();
            services.AddTransient<Simulation>();
            services.AddSingleton<ISimulation>(sim);
            services.AddSingleton<ILoader>(load);

            return services.BuildServiceProvider();
        }


        private ISimulation sim(IServiceProvider provider)
        {
            var config = provider.GetService<SimulationConfig>();

            if (config.FullRamMode)
            {
                Console.WriteLine("really");
                return provider.GetService<SmallSimulation>();
            }
            else
            {
                return provider.GetService<Simulation>();
            }
        }

        private ILoader load(IServiceProvider provider)
        {
            var config = provider.GetService<SimulationConfig>();

            if (config.Path.EndsWith(".sim"))
            {
                return provider.GetService<SimulationLoader>();
            }
            else
            {
                return provider.GetService<ConfigLoader>();
            }
        }
    }
}
