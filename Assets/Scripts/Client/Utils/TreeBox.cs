using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class TreeBox
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

        private TreeBox m_next;

        private TreeBox m_previous;

        private int m_index;

        public Vector3 PointA => m_pointA;

        public Vector3 PointB => m_pointB;

        public Vector3 PointC => m_pointC;

        public float Width => m_width;

        public float Height => m_height;

        public float X => m_x;

        public float Y => m_y;

        public float Density
        {
            get
            {
                return m_density;
            }
            set
            {
                m_density = value;
            }
        }

        public float RectArea => m_width * m_height;

        public float LengthOfLineAB => Vector3.Distance(m_pointA, m_pointB);

        public float LengthOfLineBC => Vector3.Distance(m_pointB, m_pointC);

        public float LengthOfLineAC => Vector3.Distance(m_pointA, m_pointC);

        public float TriangleArea
        {
            get
            {
                float num = (LengthOfLineAB + LengthOfLineBC + LengthOfLineAC) / 2f;
                return Convert.ToSingle(Mathf.Sqrt(num * (num - LengthOfLineAB) * (num - LengthOfLineBC) * (num - LengthOfLineAC)));
            }
        }

        public int TreeCountInRect
        {
            get
            {
                return m_treeCountInRect;
            }
            set
            {
                m_treeCountInRect = value;
            }
        }

        public int TreeCountInTriangle
        {
            get
            {
                return m_treeCountInTriangle;
            }
            set
            {
                m_treeCountInTriangle = value;
            }
        }

        public TreeBox Next
        {
            get
            {
                return m_next;
            }
            set
            {
                m_next = value;
            }
        }

        public TreeBox Previous
        {
            get
            {
                return m_previous;
            }
            set
            {
                m_previous = value;
            }
        }

        public int Index
        {
            get
            {
                return m_index;
            }
            set
            {
                m_index = value;
            }
        }

        public TreeBox(Vector3 pa, Vector3 pb, Vector3 pc)
        {
            m_pointA = pa;
            m_pointB = pb;
            m_pointC = pc;
            float x = pa.x;
            float x2 = pa.x;
            float z = pa.z;
            float z2 = pa.z;
            List<Vector3> list = new List<Vector3>
        {
            pa,
            pb,
            pc
        };
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
            m_x = x;
            m_y = z;
            m_width = x2 - x;
            m_height = z2 - z;
            m_rectPoints.Add(new Vector3(m_x, 0f, m_y));
            m_rectPoints.Add(new Vector3(m_x + m_width, 0f, m_y));
            m_rectPoints.Add(new Vector3(m_x + m_width, 0f, m_y + m_height));
            m_rectPoints.Add(new Vector3(m_x, 0f, m_y + m_height));
            m_density = 0f;
        }

        public List<Vector3> GetRectanglePoints()
        {
            return m_rectPoints;
        }

        public Vector3 Center()
        {
            return (PointA + PointB + PointC) / 3f;
        }

        public bool HaveTreePoint(Vector3 v)
        {
            if (m_treePoints.Contains(v))
            {
                return true;
            }
            return false;
        }

        public bool AddTreePoint(Vector3 v)
        {
            if (!HaveTreePoint(v))
            {
                m_treePoints.Add(v);
                return true;
            }
            return false;
        }

        public List<Vector3> ExistentTree()
        {
            return m_treePoints;
        }
    }
}