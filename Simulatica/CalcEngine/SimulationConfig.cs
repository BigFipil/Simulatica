using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

namespace CalcEngine
{
    public class SimulationConfig
    {
        /// <summary>
        /// Blueprints list of Particles that will be used to perform simulation.
        /// </summary>
        public List<ParticleBlueprint> particleBlueprints { get; set; }



        /*
         * Simulation configuration settings below
         */
        [JsonProperty]
        /// <summary>
        /// Defines number of Threads being used in simulation
        /// </summary>
        public int Threads { get; private set;}

        [JsonProperty]
        /// <summary>
        /// Defines Size of a Box, inside of which the simulation is performed. This Box is also natural constrain for all particles included in simulation
        /// </summary>
        public VectorD3 SimulationBoxSize { get; private set; }

        [JsonProperty]
        /// <summary>
        /// Represents the period of time for simulation. 
        /// </summary>
        public double TimeRangeSeconds { get; private set; }

        [JsonProperty]
        /// <summary>
        /// Represents the step of time that is the smallest time in which simulation state is calculated. SImulation starts in time t = 0
        /// </summary>
        public double SimulationStepTime { get; private set; }

        [JsonProperty]
        /// <summary>
        /// Represents the step of time, after which actuall state of simulation is captured and saved to file; 
        /// </summary>
        public double DataSaveStepTime { get; private set; }

        [JsonProperty]
        /// <summary>
        /// Defines whether the all particles used in simulation are emitted directly to program memory, and maintain there,
        /// or they are constantly writted to simulation file, and loading from Hard Drive when needed. If value of this
        /// property is true, all calculations are performed more quickly, but there is a limitation associated with
        /// quantity of particles (simulation size limit).
        /// </summary>
        public bool FullRamMode { get; private set; }

        [JsonIgnore]
        public string Path { get; set; } = "";  
        [JsonIgnore]
        public int Port { get; set; } = 6060;   

        public override string ToString()
        {
            string s = "\nSimulation Configuration: \n\n";

            foreach(var p in particleBlueprints)
            {
                s += "\tParticle: " + p.Name + "   quantity: "+p.Quantity+"\n";

                foreach (var prop in p.properties)
                {
                    s += "\t\t" + prop.Key + " " + prop.Value + "\n";
                }
                foreach (var met in p.methods)
                {
                    s += "\t\t" + met.Key + " " + met.Value + "\n";
                }
            }

            s += "\nThreads: " + Threads + "\n";
            s += "SimulationBoxSize: " + SimulationBoxSize.ToString() + "\n";
            s += "TimeRangeSeconds: " + TimeRangeSeconds + "\n";
            s += "SimulationStepTime: " + SimulationStepTime + "\n";
            s += "DataSaveStepTime: " + DataSaveStepTime + "\n";
            s += "FullRamMode: " + FullRamMode + "\n";

            return s;
        }

    }
}
