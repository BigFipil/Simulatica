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
            services.AddTransient<Loader>();
            services.AddTransient<Emitter>();
            services.AddTransient<ISimulation, SmallSimulation>();
            services.AddTransient<ISimulation, Simulation>();

            return services.BuildServiceProvider();
        }
    }
}
