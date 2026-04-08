using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class AutoTreeBox
    {
        private Vector3 m_pointA = default(Vector3);

        private Vector3 m_pointB = default(Vector3);

        private Vector3 m_pointC = default(Vector3);

        private float m_width;

        private float m_height;

        private float m_x;

        private float m_y;

        private float m_density;

        private int m_treeCountInRect;

        private int m_treeCountInTriangle;

        private List<Vector3> m_rectPoints = new List<Vector3>();

        private List<Vector3> m_treePoints = new List<Vector3>();

        private AutoTreeBox m_next;

        private AutoTreeBox m_previous;

        private int m_index;

        public Vector3 PointA
        {
            get
            {
                return this.m_pointA;
            }
        }

        public Vector3 PointB
        {
            get
            {
                return this.m_pointB;
            }
        }

        public Vector3 PointC
        {
            get
            {
                return this.m_pointC;
            }
        }

        public float Width
        {
            get
            {
                return this.m_width;
            }
        }

        public float Height
        {
            get
            {
                return this.m_height;
            }
        }

        public float X
        {
            get
            {
                return this.m_x;
            }
        }

        public float Y
        {
            get
            {
                return this.m_y;
            }
        }

        public float Density
        {
            get
            {
                return this.m_density;
            }
            set
            {
                this.m_density = value;
            }
        }

        public float RectArea
        {
            get
            {
                return this.m_width * this.m_height;
            }
        }

        public float LengthOfLineAB
        {
            get
            {
                return Vector3.Distance(this.m_pointA, this.m_pointB);
            }
        }

        public float LengthOfLineBC
        {
            get
            {
                return Vector3.Distance(this.m_pointB, this.m_pointC);
            }
        }

        public float LengthOfLineAC
        {
            get
            {
                return Vector3.Distance(this.m_pointA, this.m_pointC);
            }
        }

        public float TriangleArea
        {
            get
            {
                float num = (this.LengthOfLineAB + this.LengthOfLineBC + this.LengthOfLineAC) / 2f;
                return Convert.ToSingle(Mathf.Sqrt(num * (num - this.LengthOfLineAB) * (num - this.LengthOfLineBC) * (num - this.LengthOfLineAC)));
            }
        }

        public int TreeCountInRect
        {
            get
            {
                return this.m_treeCountInRect;
            }
            set
            {
                this.m_treeCountInRect = value;
            }
        }

        public int TreeCountInTriangle
        {
            get
            {
                return this.m_treeCountInTriangle;
            }
            set
            {
                this.m_treeCountInTriangle = value;
            }
        }

        public AutoTreeBox Next
        {
            get
            {
                return this.m_next;
            }
            set
            {
                this.m_next = value;
            }
        }

        public AutoTreeBox Previous
        {
            get
            {
                return this.m_previous;
            }
            set
            {
                this.m_previous = value;
            }
        }

        public int Index
        {
            get
            {
                return this.m_index;
            }
            set
            {
                this.m_index = value;
            }
        }

        public AutoTreeBox(Vector3 pa, Vector3 pb, Vector3 pc)
        {
            this.m_pointA = pa;
            this.m_pointB = pb;
            this.m_pointC = pc;
            float x = pa.x;
            float x2 = pa.x;
            float z = pa.z;
            float z2 = pa.z;
            List<Vector3> list = new List<Vector3>();
            list.Add(pa);
            list.Add(pb);
            list.Add(pc);
            for (int i = 1; i < list.Count; i++)
            {
                Vector3 vector = list[i];
                if (vector.x < x)
                {
                    x = vector.x;
                }
                if (vector.x > x2)
                {
                    x2 = vector.x;
                }
                if (vector.z < z)
                {
                    z = vector.z;
                }
                if (vector.z > z2)
                {
                    z2 = vector.z;
                }
            }
            this.m_x = x;
            this.m_y = z;
            this.m_width = x2 - x;
            this.m_height = z2 - z;
            this.m_rectPoints.Add(new Vector3(this.m_x, 0f, this.m_y));
            this.m_rectPoints.Add(new Vector3(this.m_x + this.m_width, 0f, this.m_y));
            this.m_rectPoints.Add(new Vector3(this.m_x + this.m_width, 0f, this.m_y + this.m_height));
            this.m_rectPoints.Add(new Vector3(this.m_x, 0f, this.m_y + this.m_height));
            this.m_density = 0f;
        }

        public List<Vector3> GetRectanglePoints()
        {
            return this.m_rectPoints;
        }

        public Vector3 Center()
        {
            return (this.PointA + this.PointB + this.PointC) / 3f;
        }

        public bool HaveTreePoint(Vector3 v)
        {
            return this.m_treePoints.Contains(v);
        }

        public bool AddTreePoint(Vector3 v)
        {
            if (!this.HaveTreePoint(v))
            {
                this.m_treePoints.Add(v);
                return true;
            }
            return false;
        }

        public List<Vector3> ExistentTree()
        {
            return this.m_treePoints;
        }
    }
}