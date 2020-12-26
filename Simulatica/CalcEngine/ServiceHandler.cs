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
            services.AddTransient<Writer>();
            services.AddTransient<SimulationLoader>();
            services.AddTransient<Emitter>();
            services.AddTransient<SmallSimulation>();
            services.AddTransient<Simulation>();
            services.AddTransient<MultipleSimulation>();
            services.AddTransient<FolderManager>();
            services.AddTransient<MultipleFolderManager>();
            services.AddTransient<IFolderManager>(fol);
            services.AddSingleton<ILogger, Logger>();
            services.AddTransient<ISimulation>(sim);
            services.AddTransient<ILoader>(load);

            return services.BuildServiceProvider();
        }

        private ISimulation sim(IServiceProvider provider)
        {
            var config = provider.GetService<SimulationConfig>();
            ISimulation s;

            if (config.FullRamMode)
            {
                s = provider.GetService<SmallSimulation>();
            }
            else
            {
                s = provider.GetService<Simulation>();
            }

            if (config.MultipleSimulationCount > 1)
            {
                var tmp = provider.GetService<MultipleSimulation>();
                tmp.Simulation = s;

                return tmp;
            }

            return s;
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

        private IFolderManager fol(IServiceProvider provider)
        {
            var config = provider.GetService<SimulationConfig>();

            if (config.MultipleSimulationCount > 1)
            {
                return provider.GetService<MultipleFolderManager>();
            }
            else
            {
                return provider.GetService<FolderManager>();
            }
        }
    }
}
