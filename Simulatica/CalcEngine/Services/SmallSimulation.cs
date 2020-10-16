using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

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

        [JsonProperty]
        private List<Thread> threads = new List<Thread>();

        private Mutex mutex = new Mutex();
        private bool threadActive;

        [JsonProperty]
        private int typePtr, particlePtr;

        public SmallSimulation(SimulationConfig C, SimulationState S, Emitter E)
        {
            Config = C;
            State = S;
            Emitter = E;

            Config.particleBlueprints[0].Quantity = 10;

            for(int i = 0; i < Config.Threads; i++)
            {
                threads.Add(new Thread(CalculateParticle));
            }
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

                for (int j = 0; j < (int)Config.particleBlueprints[i].Quantity; j++)
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
            if(Config.Threads == 1)
            for (double iter = 1; iter <= Config.TimeRangeSeconds; iter += Config.SimulationStepTime)
            {
                //Console.WriteLine(iter);

                for (int i = 0; i < objLists.Length; i++)
                {
                    //informacje o obliczanym typie dostepne tutaj
                    for (int j = 0; j < (int)Config.particleBlueprints[i].Quantity; j++)
                    {
                        Console.WriteLine(iter+": "+i+" -> "+j);
                        //informacje o oblicznanej czastce dostepne tutaj

                        for (int x = 0; x < objLists.Length; x++)
                        {
                            //informacje o porownywanym typie dostepne tutaj
                            var mi2 = types[i].GetMethod("Calculate", new Type[] { types[x] });

                            for (int y = 0; y < (int)Config.particleBlueprints[x].Quantity; y++)
                            {
                                mi2.Invoke(objLists[i][(int)j], new object[] { objLists[x][(int)y] });
                                //informacje o porownywanej czastce dostepne tutaj
                            }
                        }

                        MethodInfo mi = types[i].GetMethod("Update");
                        if(mi != null) mi.Invoke(objLists[i][j], null);
                            //zapis
                        }
                }
            }
            else
            {
                for (double iter = 1; iter <= Config.TimeRangeSeconds; iter += Config.SimulationStepTime)
                {
                    threadActive = true;

                    //foreach (var t in threads) t.Start();
                    //threads.Clear();
                    for (int i = 0; i < Config.Threads; i++)
                    {
                        threads[i] = new Thread(CalculateParticle);
                        threads[i].Start();
                    }

                    foreach (var t in threads) t.Join();

                    Console.WriteLine("iter: "+iter);

                    //Update for each
                    for (int i = 0; i < objLists.Length; i++)
                    {
                        Console.WriteLine("Method");
                        MethodInfo mi = types[i].GetMethod("Update");
                        for (int j = 0; j < (int)Config.particleBlueprints[i].Quantity; j++)
                        {
                            if (mi != null) mi.Invoke(objLists[i][j], null);
                        }
                    }

                    typePtr = 0;
                    particlePtr = 0;
                    Console.WriteLine(typePtr+particlePtr);
                }
            }

            #endregion


        }
        void CalculateParticle()
        {
            int tmpT = 0, tmpP = 0; 

            do
            {
                if (mutex.WaitOne())
                {

                    tmpT = typePtr;
                    tmpP = particlePtr;
                    particlePtr++;

                    if(particlePtr > (int)Config.particleBlueprints[typePtr].Quantity)
                    {
                        if(typePtr < types.Length-1)
                        {
                            typePtr++;
                            particlePtr = 1;
                            tmpT = typePtr;
                            tmpP = 0;
                        }
                        else
                        {
                            threadActive = false;
                            mutex.ReleaseMutex();
                            break;
                        }
                    }

                    mutex.ReleaseMutex();
                }
                Console.WriteLine(tmpT + "  " + tmpP);
                //Thread.Sleep(500);
                //calculation

            } while (threadActive);

        }
        void CalculateParticle2()
        {
            while (typePtr < types.Length && threadActive)
            {
                mutex.WaitOne();
                try
                {
                    while (particlePtr < (int)Config.particleBlueprints[typePtr].Quantity - 1 && threadActive)
                    {

                        //if (mutex.WaitOne())
                        //{
                        int calcParticle = particlePtr;
                        int calcType = typePtr;
                        Console.WriteLine(Thread.CurrentThread.Name + ": " + typePtr + "  " + particlePtr);
                        particlePtr++;
                        //mutex.ReleaseMutex();
                        //}
                        mutex.ReleaseMutex();
                        Thread.Sleep(500);
                        mutex.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    if (e.GetType() != typeof(IndexOutOfRangeException)) State.ErrorList.Add(e);
                    break;
                }

                //if (mutex.WaitOne())
                //{
                if (particlePtr >= (int)Config.particleBlueprints[typePtr].Quantity - 1)
                {
                    typePtr++;
                    if (typePtr >= types.Length) threadActive = false;
                    particlePtr = 0;
                }
                //mutex.ReleaseMutex();
                //}
            }
        }
    }
}
