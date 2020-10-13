using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;

namespace CalcEngine
{
    public class SmallSimulation : ISimulation
    {
        [JsonProperty]
        public SimulationConfig Config { get; private set; }
        [JsonProperty]
        public SimulationState State { get; private set; }
        [JsonProperty]
        public Emitter Emitter { get; private set; }
        [JsonProperty]
        public Assembly Assembly { get; private set; }

        [JsonProperty]
        private Type[] types;

        [JsonProperty]
        private IList[] objLists;

        public SmallSimulation(SimulationConfig C, SimulationState S, Emitter E)
        {
            Config = C;
            State = S;
            Emitter = E;
        }

        //public void

        public void Run()
        {
            Console.WriteLine("small");

            if(State.ErrorList.Count > 0)
            {
                Console.WriteLine("Loading Exeption. Error message:\n\n\t{0}", State.ErrorList[State.ErrorList.Count - 1]);
                return;
            }


            #region Compilation
            Assembly = Emitter.CompileParticlesBlueprints();

            if(Assembly == null)
            {
                Console.WriteLine("Compilation errors occured: \n\n");
                Console.WriteLine(State.syntaxTree);
                foreach(var e in State.ErrorList)
                {
                    Console.WriteLine("\t"+ e.Message +". "+e.Data+"\n\n");
                }
                return;
            }
            #endregion
            #region List of Simulation Types
            types = new Type[Config.particleBlueprints.Count];
            for (int i = 0; i < types.Length; i++)
            {
                //types[i] = Assembly.GetType(Config.particleBlueprints[i].Name, false, true);
                try
                {
                    types[i] = Assembly.GetType("Particles." + Config.particleBlueprints[i].Name, true);
                }
                catch(Exception e)
                {
                    State.ErrorList.Add(e);
                    return;
                }

            }

            // Create particle type lists
            objLists = new IList[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                var listType = typeof(List<>).MakeGenericType(types[i]);
                objLists[i] = (IList)Activator.CreateInstance(listType);
            }
            #endregion
            #region Initalizing Particles
            for (int i = 0; i < objLists.Length; i++)
            {
                MethodInfo mi = types[i].GetMethod("Initialize");

                Console.WriteLine(mi.Name);

                for (ulong j = 0; j < Config.particleBlueprints[i].Quantity; j++)
                {
                    Object particle = Activator.CreateInstance(types[i]);

                    mi.Invoke(particle, null);

                    objLists[i].Add(particle);
                }
            }
            #endregion

            //var mi2 = types[0].GetMethod("Calculate", new Type[] { types[0] });
            //mi2.Invoke(objLists[0][0], new object[] { objLists[0][1]});

            Console.WriteLine(JsonConvert.SerializeObject(objLists[0][0]));

            /*
            Console.WriteLine(objLists[0].Count + " " + objLists[1].Count);
            Console.WriteLine(objLists[0][0].ToString());
            Console.WriteLine(JsonConvert.SerializeObject(objLists[0][0]));
            Console.WriteLine(JsonConvert.SerializeObject(objLists[1][1]));
            */
            #region Performing Calculations

            for (double iter = 0; iter < Config.TimeRangeSeconds; iter += Config.SimulationStepTime)
            {
                Console.WriteLine(iter);

                for (int i = 0; i < objLists.Length; i++)
                {
                    //informacje o obliczanym typie dostepne tutaj
                    for (ulong j = 0; j < Config.particleBlueprints[i].Quantity; j++)
                    {
                        //informacje o oblicznanej czastce dostepne tutaj

                        for (int x = 0; x < objLists.Length; x++)
                        {
                            //informacje o porownywanym typie dostepne tutaj

                            for (ulong y = 0; y < Config.particleBlueprints[x].Quantity; y++)
                            {
                                var mi2 = types[i].GetMethod("Calculate", new Type[] { types[x] });
                                mi2.Invoke(objLists[i][(int)j], new object[] { objLists[x][(int)y] });
                                //informacje o porownywanej czastce dostepne tutaj
                            }
                        }


                    }
                }
            }

            #endregion

            

        }
    }
}
