using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Client
{
    public class DoubleBenchmark : Benchmark
    {
        private const double randomFloat = double.Epsilon;
        private readonly uint LENGTH = 2;
        private readonly uint times = 125000;
        private double[] doubleArray;


        public override void Initialize()
        {
            base.Initialize();
            doubleArray = new double[LENGTH];
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public override void Run()
        {
            base.Run();
            for (var i = 0; i < times; i++)
            {
                // LOAD
                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] = randomFloat;
                }

                // ADD

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] += randomFloat;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] += randomFloat;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] += randomFloat;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] += randomFloat;
                }

                // SUBTRACT

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] -= randomFloat;
                }

                // MULTIPLY

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] *= randomFloat;
                }

                // DIVIDE

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] /= randomFloat;
                }

                // MODULO

                for (var j = 0; j < LENGTH; j++)
                {
                    doubleArray[j] %= randomFloat;
                }
            }


        }

        public override double GetDataThroughput(double timeInMillis)
        {
            return sizeof(double) * LENGTH * LENGTH * times * times * 8 / (timeInMillis / 1000);
        }
    }
}

