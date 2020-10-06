using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Linq;

namespace InMemoryCompiler
{
    class Program22
    {
        public static void Main22(CalcEngine.Emitter e)
        {
            /*
            string[] codeArr = new string[1];
            string code = @"
using System;
using System.IO;
using System.Runtime;
using System.Collections.Generic;
using CalcEngine;

namespace HelloWorld
{
    public class HelloWorldClass
    {
        public static void Main()
        {
            Console.WriteLine(""Hello World!"");
            Console.ReadLine();

            //double d = Math.PI;
            //List<int> a = new List<int>();
            VectorD3 pos;
        }
    }
}";
            codeArr[0] = code;
            */

            string code = "";// e.WholeSyntaxGenerator(e.config);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            Console.WriteLine(syntaxTree.GetRoot());
            //Console.WriteLine("\n\n"+ typeof(Enumerable).Assembly.Location+"\n\n");

            string assemblyName = Path.GetRandomFileName();
            /*MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(@"C:\Development\Simulatica\Simulatica\CalcEngine\bin\Debug\netcoreapp3.1\CalcEngine.dll"),
                MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\3.1.5\System.Console.dll"),
                //MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\3.1.5\System.IO.dll"),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };*/
            IEnumerable<MetadataReference> references = Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location))
                .Where((val)=>val.EndsWith(".dll"))
                .Where((val)=>val.Contains("System."))
                .Append(typeof(CalcEngine.Program).Assembly.Location)
                .Select((str)=>MetadataReference.CreateFromFile(str));

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
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
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Console.WriteLine("\n\n"+assembly.FullName + "   " + assembly.ToString());
                }
            }




            //compileInMemory(codeArr);
        }
        public static void compileInMemory(string[] code)
        {
            CompilerParameters compilerParameters = new CompilerParameters();
            string currentDirectory = Directory.GetCurrentDirectory();
            compilerParameters.GenerateInMemory = false;
            compilerParameters.TreatWarningsAsErrors = false;
            compilerParameters.GenerateExecutable = true;
            compilerParameters.CompilerOptions = "/optimize";
            string[] value = new string[]
            {
                "System.dll",
           //     "System.Core.dll",
                "mscorlib.dll",
                "System.Management.Automation.dll"
            };
            compilerParameters.ReferencedAssemblies.AddRange(value);
            CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
            CompilerResults compilerResults = null;
            cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);
            if (compilerResults.Errors.HasErrors)
            {
                string text = "Compile error: ";
                foreach (CompilerError compilerError in compilerResults.Errors)
                {
                    text = text + "\r\n" + compilerError.ToString();
                }
                throw new Exception(text);
            }
            Module module = compilerResults.CompiledAssembly.GetModules()[0];
            Type type = null;
            MethodInfo methodInfo = null;
            if (module != null)
            {
                type = module.GetType("HelloWorld.HelloWorldClass");
            }
            if (type != null)
            {
                methodInfo = type.GetMethod("Main");
            }
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, null);
            }
        }
    }


}