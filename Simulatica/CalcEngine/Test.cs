using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CalcEngine
{
    public class Test
    {
        private SimulationConfig config;

        public Test(SimulationConfig Config)
        {
            config = Config;
        }

        public void testSeri()
        {
            ParticleBlueprint p1 = new ParticleBlueprint();
            ParticleBlueprint p2 = new ParticleBlueprint();

            p1.properties = new Dictionary<string, string>();
            p1.methods = new Dictionary<string, string>();
            p2.properties = new Dictionary<string, string>();
            p2.methods = new Dictionary<string, string>();

            p1.Name = "Point";
            p1.Quantity = 1000;
            p1.properties.Add("position", "VectorD3");
            p1.properties.Add("tposition", "VectorD3");
            p1.properties.Add("mass", "double");

            p1.methods.Add("Initialize()", "Random r = new Random(); position = new VectorD3(r.NextDouble() * 10, r.NextDouble() * 10, 0); mass = r.NextDouble(); tposition = new VectorD3(0,0,0);");
            p1.methods.Add("Calculate(Point p)", "position += new VectorD3(1,2,3)");
            p1.methods.Add("Calculate(Hole p)", "position += p.position");
            p1.methods.Add("Update()", "position += tposition;");

            p2.Name = "Hole";
            p2.Quantity = 13;
            p2.properties.Add("position", "VectorD3");

            p2.methods.Add("Initialize()", "Random r = new Random(); position = new VectorD3(r.NextDouble() * 10, r.NextDouble() * 10, 0);");
            p2.methods.Add("Calculate(Point p)", "");
            p2.methods.Add("Calculate(Hole p)", "");


            SimulationConfig config = new SimulationConfig();

            config.particleBlueprints = new List<ParticleBlueprint>()
            {
                p1, p2,
            };

            //config.SimulationBoxSize = new VectorD3(5, 12, 20);
            //config.Threads = 128;
            
            string jsonString = JsonConvert.SerializeObject(config);
            Console.Write(jsonString);
            File.WriteAllText("PointSim.conig", jsonString);
        } 

        public void testDeseri()
        {
            //string jsonString = File.ReadAllText("c2");
            //JsonConvert.PopulateObject(jsonString, config);
            //SimulationConfig config = JsonSerializer.Deserialize<SimulationConfig>(jsonString);

            Console.Write(config.ToString());
        }
    }
}
