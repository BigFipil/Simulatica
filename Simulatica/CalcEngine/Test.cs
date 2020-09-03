using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CalcEngine
{
    class Test
    {
        public void testSeri()
        {
            ParticleBlueprint p1 = new ParticleBlueprint();
            ParticleBlueprint p2 = new ParticleBlueprint();

            p1.properties = new Dictionary<string, string>();
            p1.methods = new Dictionary<string, string>();
            p2.properties = new Dictionary<string, string>();
            p2.methods = new Dictionary<string, string>();

            p1.properties.Add("name", "kapitan");
            p1.properties.Add("position", "VectorD3");
            p1.properties.Add("force", "double");

            p1.methods.Add("to_kapitan(kapitan p)", "position += new VectorD3(1,2,3)");
            p1.methods.Add("to_bomba(bomba p)", "position += p.position");

            p2.properties.Add("name", "bomba");
            p2.properties.Add("position", "VectorD3");
            p2.properties.Add("force", "double");
            p2.properties.Add("rotation", "double");

            p2.methods.Add("to_kapitan(kapitan p)", "position += new VectorD3(1,2,3)");
            p2.methods.Add("to_bomba(bomba p)", "position += p.position");


            SimulationConfig config = new SimulationConfig();

            config.particleBlueprints = new List<ParticleBlueprint>()
            {
                p1, p2,
            };

            //config.SimulationBoxSize = new VectorD3(2, 2, 2);
            //config.Threads = 5;
            
            string jsonString = JsonSerializer.Serialize(config);
            Console.Write(jsonString);
            File.WriteAllText("configTestowy", jsonString);
        } 

        public void testDeseri()
        {
            string jsonString = File.ReadAllText("configTestowy");
            SimulationConfig config = JsonSerializer.Deserialize<SimulationConfig>(jsonString);

            Console.Write(config.ToString());
        }
    }
}
