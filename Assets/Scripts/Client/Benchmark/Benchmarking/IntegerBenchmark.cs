using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

namespace Client
{
    public class IntegerBenchmark : Benchmark
    {
        private readonly uint LENGTH = 2;
        private readonly uint times = 65000;

        private byte randomByte;
        private int randomInt;
        private long randomLong;
        private short randomShort;
        private byte[] resultByteArray;
        private int[] resultIntArray;
        private long[] resultLongArray;
        private short[] resultShortArray;

        public override void Initialize()
        {
            base.Initialize();
            var rand = new System.Random();

            randomInt = rand.Next();
            randomByte = (byte)rand.Next();
            randomLong = (long)int.MaxValue + rand.Next();
            randomShort = (short)rand.Next();

            resultByteArray = new byte[LENGTH];
            resultIntArray = new int[LENGTH];
            resultLongArray = new long[LENGTH];
            resultShortArray = new short[LENGTH];
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public override void Run()
        {
            for(int i = 0;i<times;i++)
            {
                // LOAD
                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] = randomByte;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] = randomShort;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] = randomInt;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] = randomLong;
                }

                // ADD

                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] += randomByte;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] += randomShort;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] += randomInt;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] += randomLong;
                }

                // SUBTRACT

                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] -= randomByte;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] -= randomShort;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] -= randomInt;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] -= randomLong;
                }

                // MULTIPLY

                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] *= randomByte;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] *= randomShort;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] *= randomInt;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] *= randomLong;
                }

                // DIVIDE

                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] /= randomByte;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] /= randomShort;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] /= randomInt;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] /= randomLong;
                }

                // MODULO

                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] %= randomByte;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] %= randomShort;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] %= randomInt;
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] %= randomLong;
                }

                // VARIOUS

                for (var j = 0; j < LENGTH; j++)
                {
                    resultByteArray[j] = Math.Max(randomByte, resultByteArray[j]);
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultShortArray[j] = Math.Min(randomShort, resultShortArray[j]);
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultIntArray[j] = (int)Math.BigMul(randomInt, randomInt);
                }

                for (var j = 0; j < LENGTH; j++)
                {
                    resultLongArray[j] = Math.BigMul(randomInt, randomInt);
                }
            }


        }


        public override double GetDataThroughput(double timeInMillis)
        {
            uint Len = LENGTH * times;
            return sizeof(byte) * Len * Len * 6 / (timeInMillis / 1000)
                   + sizeof(short) * Len * Len * 6 / (timeInMillis / 1000)
                   + sizeof(int) * Len * Len * 6 / (timeInMillis / 1000)
                   + sizeof(long) * Len * Len * 6 / (timeInMillis / 1000);
        }
    }
}

