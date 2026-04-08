using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;
using Skyunion;

namespace Client
{
    public class BenchmarkRunner : MonoBehaviour
    {
        private readonly List<Benchmark> benchmarksToRun = new List<Benchmark>();
        public static Action onBenchmarksCompleted;


        public float CPUWeight = 0.5f;
        public float GPUWeight = 0.5f;

        private long CPUTotalTime;

        public Mesh mesh;
        public Material mat;

        private int gpuBenchmarkState;
        private int frameCount = 20;
        private float GPUaverageTime;
        private float GPUaverageCount;
        private long GPUtotalTime;
        private long GPUtotalCount;


        // Start is called before the first frame update
        void Start()
        {
            float score = PlayerPrefs.GetFloat("PlatfommScore", -1f);
            if (score<0)
            {
                CPUBenchmark();
                Timer.Register(0.01f, InitGPUBenchmark);
            }
            else
            {
                GameObject.Destroy(this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            GPUBenchmark();
        }


        private void CPUBenchmark()
        {
            benchmarksToRun.Add((Benchmark)Activator.CreateInstance(typeof(DoubleBenchmark)));
            benchmarksToRun.Add((Benchmark)Activator.CreateInstance(typeof(FloatBenchmark)));
            benchmarksToRun.Add((Benchmark)Activator.CreateInstance(typeof(IntegerBenchmark)));

            CPUTotalTime = 0;
            while(benchmarksToRun.Count>0)
            {
                var currentBenchmark = benchmarksToRun[0];
                currentBenchmark.Initialize();
                var sw = new Stopwatch();
                sw.Start();
                currentBenchmark.Run();
                sw.Stop();
                var timing = sw.ElapsedMilliseconds;
                CPUTotalTime += timing;
                sw.Reset();
                currentBenchmark.Shutdown();
                benchmarksToRun.RemoveAt(0);
            }
            GC.Collect();
            string logInfo =  string.Concat(DateTime.Now.ToString(), "    ","性能测试总时间：", CPUTotalTime, "毫秒\n");
            SaveData("CPU", logInfo);
            //UnityEngine.Debug.LogError("CPU 性能测试总时间 : " + totalTime);
        }

        private void InitGPUBenchmark()
        {
            gpuBenchmarkState = 1;
            frameCount = 0;
            GPUaverageTime = 0;
            GPUtotalTime = 0;
            GPUaverageCount = 0;
            GPUtotalCount = 0;
        }

        private void GPUBenchmark()
        {
            switch(gpuBenchmarkState)
            {
                case 1:
                    if (mesh == null)
                    {
                        UnityEngine.Debug.LogError("mesh is null");
                        break ;
                    }
                    GraphicsBenchmark graphics = new GraphicsBenchmark(mesh, mat);
                    double dataTroughPut = 0;
                    graphics.Initialize();
                    var sw = new Stopwatch();
                    long milliseconds = 0;

                    for (int n = 0; n < 40000; n++)
                    {
                        sw.Start();
                        graphics.Run();
                        sw.Stop();
                        dataTroughPut += graphics.GetDataThroughput(0);
                        milliseconds += sw.ElapsedMilliseconds;
                        if (milliseconds > 50)
                        {
                            break;
                        }
                    }
                    GPUtotalTime += milliseconds;
                    GPUtotalCount += (long)dataTroughPut;
                    graphics.Shutdown();
                    frameCount++;
                    if (frameCount>15)
                    {
                        gpuBenchmarkState = 2;
                    }
                    break;
                case 2:
                    gpuBenchmarkState = 0;
                    GPUaverageCount = (float)GPUtotalCount / frameCount;
                    GPUaverageTime = (float)GPUtotalTime / frameCount;
                    string logInfo = string.Concat(DateTime.Now.ToString(), "   ", GPUaverageTime ,"毫秒内绘制的球体个数：" ,GPUaverageCount,"\n");
                    SaveData("GPU",logInfo);
                    CaculateScore();
                    break;
                default:break;
            }


        }

        private void CaculateScore()
        {
            float score =(60/ CPUTotalTime * CPUWeight + GPUaverageCount / (GPUaverageTime * 10) * GPUWeight)*100;
            SaveData("Score",string.Concat(DateTime.Now.ToString(), "   score:", score, "\n"));
            PlayerPrefs.SetFloat("PlatfommScore",score);
            BenchmarkRunner.onBenchmarksCompleted?.Invoke();
        }


        private void SaveData(string type,string content)
        {
            string path = Application.persistentDataPath + "/Benchmark";
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string file =string.Concat(path ,"/",type,"Benchmark.txt");
            if(!File.Exists(file))
            {
                File.Create(file).Dispose();
            }
            File.AppendAllText(file,content);
        }
    }
}

