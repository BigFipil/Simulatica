using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine.Services
{
    public class Emitter
    {
        private readonly SimulationConfig config;

        public Emitter(SimulationConfig Config)
        {
            config = Config;
        }

        public void Test()
        {
            Console.WriteLine("\t\n" + config.ToString());
        }

        public void CompileParticlesBlueprints()
        {
            string tmp = "";
            string code = @"
using System;
using System.IO;
using System.Runtime;
using System.Collections.Generic;
using CalcEngine;

namespace Particles
{
";



            tmp += "";

            code += @"}";


            /*
            CompilerResults results;
            //ICodeCompiler cscp = new CodeDomProvider();
            CompilerParameters param = new CompilerParameters(new[] { "Assembly"});
            param.GenerateExecutable = false;
            param.GenerateInMemory = true;
            //results = cscp.CompileAssemblyFromSource(param, code);
            //Console.WriteLine(results);
            */
        }

        public string ClassSyntaxGenerator(ParticleBlueprint pb)
        {
            string code = "public class "+ pb.Name +"{\n";

            //if(pb.properties.ContainsKey("Name"))

            return code + "}";
        }
    }
}

