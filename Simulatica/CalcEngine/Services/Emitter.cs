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
            string code = "using System;using System.Collections.Generic;using System.Text;namespace CalcEngine.Services{public class Hello0{public void Test(){Console.WriteLine(\"no dziala no\");}}}";

            CompilerResults results;

            //ICodeCompiler cscp = new CodeDomProvider();

            CompilerParameters param = new CompilerParameters(new[] { "Assembly"});

            param.GenerateExecutable = false;
            param.GenerateInMemory = true;

            //results = cscp.CompileAssemblyFromSource(param, code);

            //Console.WriteLine(results);
        }
    }
}

