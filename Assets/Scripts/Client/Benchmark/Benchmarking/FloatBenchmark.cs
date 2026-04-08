using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Client
{
	public class FloatBenchmark : Benchmark
	{
		private const float randomFloat = float.Epsilon;
		private readonly uint LENGTH = 4;
		private readonly uint times = 125000;
		private float[] floatArray;


		public override void Initialize()
		{
			base.Initialize();
			floatArray = new float[LENGTH];
		}

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		public override void Run()
		{
			base.Run();
			for(int i = 0;i<times;i++)
			{
				// LOAD
				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] = randomFloat;
				}

				// ADD

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] += randomFloat;
				}

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] += randomFloat;
				}

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] += randomFloat;
				}

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] += randomFloat;
				}

				// SUBTRACT

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] -= randomFloat;
				}

				// MULTIPLY

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] *= randomFloat;
				}

				// DIVIDE

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] /= randomFloat;
				}

				// MODULO

				for (var j = 0; j < LENGTH; j++)
				{
					floatArray[j] %= randomFloat;
				}
			}
		}

		public override double GetDataThroughput(double timeInMillis)
		{
			return sizeof(double) * LENGTH * LENGTH *times*times * 8 / (timeInMillis / 1000);
		}
	}
}

