using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client
{
    public class GraphicsBenchmark : Benchmark
    {
        private Mesh mesh;
        private Material material;
        private Vector3 pos;

        private readonly uint LENGTH = 20;

        public GraphicsBenchmark(Mesh mesh, Material material)
        {
            this.mesh = mesh;
            this.material = material;
            this.pos = new Vector3(0, 0, -100000);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Run()
        {
            base.Run();
            if(mesh!=null&&material)
            {
                for (int i = 0;i< LENGTH;i++)
                {
                    if (material.SetPass(0))
                    {
                        Graphics.DrawMeshNow(mesh, pos, Quaternion.identity);
                    }
                }
            }

        }

        public override void Shutdown()
        {
            base.Shutdown();
            if(mesh!=null)
            {
                mesh.Clear();
            }
        }

        public override double GetDataThroughput(double timeInMillis)
        {
            return LENGTH;
        }
    }
}

