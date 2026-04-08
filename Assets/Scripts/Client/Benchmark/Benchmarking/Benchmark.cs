using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Client
{
    public abstract class Benchmark
    {
        public virtual void Initialize()
        {

        }

        public virtual void Run()
        {

        }

        public virtual void Shutdown()
        {

        }

        /// <summary>
        ///  返回每秒的数据吞吐量
        /// </summary>
        /// <param name="timeInMillis"></param>
        /// <returns></returns>
        public virtual double GetDataThroughput(double timeInMillis)
        {
            return 0.0d;
        }
    }
}

