using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CalcEngine
{
    public class Emitter
    {
        private readonly SimulationConfig config;
        private readonly SimulationState state;

        public Emitter(SimulationConfig Config, SimulationState State)
        {
            config = Config;
            state = State;
        }

        public void Test()
        {
            //Console.WriteLine("\t\n" + config.ToString());
            Console.WriteLine(WholeSyntaxGenerator(config));
        }

        public Assembly CompileParticlesBlueprints()
        {
            string code = WholeSyntaxGenerator(config);

            state.syntaxTree = CSharpSyntaxTree.ParseText(code);

            string assemblyName = Path.GetRandomFileName();
            
            
            Console.WriteLine(state.syntaxTree);

            IEnumerable<MetadataReference> references = Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location))
                .Where((val) => val.EndsWith(".dll"))
                .Where((val) => val.Contains("System."))
                .Append(typeof(CalcEngine.Program).Assembly.Location)
                .Select((str) => MetadataReference.CreateFromFile(str));

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { state.syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));



            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        //Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                        state.ErrorList.Add(new Exception(diagnostic.Id + ": " + diagnostic.GetMessage() + " line: " +diagnostic.Location));
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    return Assembly.Load(ms.ToArray());

                    //Console.WriteLine("\n\n" + assembly.FullName + "   " + assembly.ToString());
                }
            }
            return null;
        }

        public string ClassSyntaxGenerator(ParticleBlueprint pb)
        {
            string code = "\npublic class "+ pb.Name +"{\n\n";

            foreach(var t in pb.properties)
            {
                //code += "\tpublic " + t.Value + " " + t.Key + " { get; set; }\n\n";
                code += "\tpublic " + t.Value + " " + t.Key;// + ";\n\n";

                if (t.Key.EndsWith("}") || t.Key.EndsWith(";")) code += "\n\n";
                else code += ";\n\n";
            }

            if (!config.SelfComparableParticles)
            {
                code += "\tpublic ulong Particle_ID_Numer;\n";
                code += "\tpublic static ulong Particle_ID_Numer_Count = 0;\n";

                //Constructor that sets ID
                code += "\tpublic " + pb.Name + "(){ Particle_ID_Numer = Particle_ID_Numer_Count; Particle_ID_Numer_Count++; }\n";
            }


            foreach (var m in pb.methods)
            {
                if(m.Key == "ToString()")
                {
                    code += "\n\tpublic override string ToString(){\n";
                }
                else
                {
                    code += "\n\tpublic void " + m.Key + "{\n";
                }


                if (!config.SelfComparableParticles && m.Key.StartsWith("Calculate("))
                {
                    code += "\t\tif(Particle_ID_Numer != p.Particle_ID_Numer){\n";
                    code += "\t\t\t" + m.Value;

                    if (!m.Value.EndsWith(";")) code += ";";

                    code += "\n\t\t}";
                    code += "\n\t}\n";
                }
                else
                {
                    code += "\t\t" + m.Value;

                    if (!m.Value.EndsWith(";")) code += ";";

                    code += "\n\t}\n";
                }
            }


            if (!pb.methods.ContainsKey("ToString()"))
            {
                code += "\n\tpublic override string ToString(){\n";

                code += "string tmp = \""+pb.Name+"\";" ;

                foreach(var t in pb.properties)
                {
                    code += "tmp += \":\"+"+ t.Key +";";
                }

                code += "return tmp;";

                code += "\n\t}\n";
            }


            return code + "}\n\n";
        }

        public string WholeSyntaxGenerator(SimulationConfig config)
        {
            string code = @"
using System;
using System.IO;
using System.Runtime;
using System.Collections.Generic;
using CalcEngine;

namespace Particles
{
    public static class Simulation{
    
        public static ulong Iteration;
        public static double StepTime, ActualTime;

        public static void Update(ulong iter, double sTime, double aTime){
            Iteration = iter; StepTime = sTime; ActualTime = aTime;
        }
    }
";



            foreach (var p in config.particleBlueprints)
            {
                code += ClassSyntaxGenerator(p);
            }

            code += "}";

            return code;
        }
    }
}

