using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Runtime.Serialization;

namespace CalcEngine
{
    public class SimulationConfig : ISerializable
    {
        /// <summary>
        /// Defines number of Threads being used in simulation
        /// </summary>
        public int Threads { get; private set; }
        /// <summary>
        /// Defines Size of a Box, inside of which the simulation is performed. This Box is also natural constrain for all particles included in simulation
        /// </summary>
        public VectorD3 SimulationBoxSize { get; private set; }
        /// <summary>
        /// Represents the period of time for simulation. 
        /// </summary>
        public double TimeRangeSeconds { get; private set; }
        /// <summary>
        /// Represents the step of time that is the smallest time in which simulation state is calculated. SImulation starts in time t = 0
        /// </summary>
        public double SimulationStepTime { get; private set; }
        /// <summary>
        /// Represents the step of time, after which actuall state of simulation is captured and saved to file; 
        /// </summary>
        public double DataSaveStepTime { get; private set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
