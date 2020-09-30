﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{

    public class Startup
    {
        public string Path { get; set; } = "";
        public int Port { get; set; } = 6060;   //Default Port

        public ServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SimulationConfig>();
            services.AddSingleton<SimulationState>();
            //services.AddSingleton<ParticleBlueprint>();

            return services.BuildServiceProvider();
        }
    }
}
