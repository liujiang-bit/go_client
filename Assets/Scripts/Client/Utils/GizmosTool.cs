using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class GizmosTool : MonoBehaviour
    {
        public Material m_treeMaterial = null;
        public List<Sprite> m_treeSprites = new List<Sprite>();
        public float m_initDensity = 1;
        public int m_treeVolumePerSquareMeter = 4;
        public float m_prohibitRadius = 5;
        public float m_treeMaxScale = 2;
        public float m_treeMinScale = 0.5f;

        private List<TreeBox> m_autoTreeBoxes = new List<TreeBox>();

        public bool DrawTriangle;

        public bool DrawRect;

        public bool DrawTreeLine;

        public bool DrawRealTree;

        public bool DrawIndexLabel;

        private List<Vector3> m_treePoints = new List<Vector3>();

        private List<Rect> m_treeRects = new List<Rect>();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (m_autoTreeBoxes.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < m_autoTreeBoxes.Count; i++)
            {
                if (DrawTriangle)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(m_autoTreeBoxes[i].PointA, m_autoTreeBoxes[i].PointB);
                    Gizmos.DrawLine(m_autoTreeBoxes[i].PointB, m_autoTreeBoxes[i].PointC);
                    Gizmos.DrawLine(m_autoTreeBoxes[i].PointC, m_autoTreeBoxes[i].PointA);
                }
                if (DrawRect)
                {
                    Gizmos.color = Color.green;
                    List<Vector3> rectanglePoints = m_autoTreeBoxes[i].GetRectanglePoints();
                    Gizmos.DrawLine(rectanglePoints[0], rectanglePoints[1]);
                    Gizmos.DrawLine(rectanglePoints[1], rectanglePoints[2]);
                    Gizmos.DrawLine(rectanglePoints[2], rectanglePoints[3]);
                    Gizmos.DrawLine(rectanglePoints[3], rectanglePoints[0]);
                }
                if (DrawIndexLabel)
                {
                    TextTool.Draw(m_autoTreeBoxes[i].Center(), m_autoTreeBoxes[i].Index.ToString());
                }
                if (DrawTreeLine)
                {
                    Gizmos.color = Color.green;
                    foreach (Vector3 treePoint in m_treePoints)
                    {
                        Gizmos.DrawLine(treePoint, treePoint + new Vector3(0f, 2f, 0f));
                    }
                }
            }
        }

        public void AddTreePoints(List<Vector3> trees)
        {
            m_treePoints.AddRange(trees);
        }

        public void ClearTreePoints()
        {
            m_treePoints.Clear();
        }

        public void AddTreeRect(List<Rect> rects)
        {
            m_treeRects.AddRange(rects);
        }

        public void ClearRectPoints()
        {
            m_treeRects.Clear();
        }

        public void SetData(List<Vector3> triangleData)
        {
            m_autoTreeBoxes.Clear();
            for (int i = 0; i < triangleData.Count; i += 3)
            {
                TreeBox autoTreeBox = new TreeBox(triangleData[i], triangleData[i + 1], triangleData[i + 2]);
                autoTreeBox.Index = m_autoTreeBoxes.Count;
                m_autoTreeBoxes.Add(autoTreeBox);
            }
            if (m_autoTreeBoxes.Count >= 2)
            {
                for (int j = 1; j < m_autoTreeBoxes.Count - 1; j++)
                {
                    m_autoTreeBoxes[j].Previous = m_autoTreeBoxes[j - 1];
                    m_autoTreeBoxes[j].Next = m_autoTreeBoxes[j + 1];
                }
                m_autoTreeBoxes[0].Next = m_autoTreeBoxes[1];
                m_autoTreeBoxes[0].Previous = m_autoTreeBoxes[m_autoTreeBoxes.Count - 1];
                m_autoTreeBoxes[m_autoTreeBoxes.Count - 1].Next = m_autoTreeBoxes[0];
                m_autoTreeBoxes[m_autoTreeBoxes.Count - 1].Previous = m_autoTreeBoxes[m_autoTreeBoxes.Count - 2];
            }
        }

        public void SetDensity(float initDensity)
        {
            m_initDensity = initDensity;
            if (m_autoTreeBoxes.Count > 0)
            {
                float num = 0f;
                float num2 = 1f;
                float num3 = initDensity;
                if (num3 == 0f)
                {
                    num3 = Random.Range(num, num2);
                }
                float num4 = (num2 - num) / 10f;
                for (int i = 0; i < m_autoTreeBoxes.Count; i++)
                {
                    num3 = Random.Range((!(num3 - num4 < num)) ? (num3 - num4) : num, (!(num3 + num4 > num2)) ? (num3 + num4) : num2);
                    m_autoTreeBoxes[i].Density = num3;
                }
            }
        }

        public void SetTrees(int treeVolumePerSquareMeter, float prohibitRadius)
        {
            m_treeVolumePerSquareMeter = treeVolumePerSquareMeter;
            m_prohibitRadius = prohibitRadius;
            for (int i = 0; i < m_autoTreeBoxes.Count; i++)
            {
                m_autoTreeBoxes[i].TreeCountInRect = (int)(m_autoTreeBoxes[i].RectArea * m_autoTreeBoxes[i].Density * (float)treeVolumePerSquareMeter);
                m_autoTreeBoxes[i].TreeCountInTriangle = (int)((float)m_autoTreeBoxes[i].TreeCountInRect * m_autoTreeBoxes[i].TriangleArea / m_autoTreeBoxes[i].RectArea);
            }
            for (int j = 0; j < m_autoTreeBoxes.Count; j++)
            {
                for (int k = 0; k < m_autoTreeBoxes[j].TreeCountInRect; k++)
                {
                    TreeBox autoTreeBox = m_autoTreeBoxes[j];
                    float x = Random.Range(autoTreeBox.X, autoTreeBox.X + autoTreeBox.Width);
                    float z = Random.Range(autoTreeBox.Y, autoTreeBox.Y + autoTreeBox.Height);
                    Vector3 vector = new Vector3(x, 0f, z);
                    if (PointinTriangle(autoTreeBox.PointA, autoTreeBox.PointB, autoTreeBox.PointC, vector) && CheckNearbyAutoTreeBox(autoTreeBox, vector, prohibitRadius))
                    {
                        autoTreeBox.AddTreePoint(vector);
                        if (autoTreeBox.ExistentTree().Count >= autoTreeBox.TreeCountInTriangle)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private bool CheckNearbyAutoTreeBox(TreeBox myAtb, Vector3 p, float prohibitRadius)
        {
            if (!ValidLocation(myAtb, p, prohibitRadius))
            {
                return false;
            }
            if (myAtb.Next != null && !ValidLocation(myAtb.Next, p, prohibitRadius))
            {
                return false;
            }
            if (myAtb.Next != null && myAtb.Next.Next != null && !ValidLocation(myAtb.Next.Next, p, prohibitRadius))
            {
                return false;
            }
            if (myAtb.Previous != null && !ValidLocation(myAtb.Previous, p, prohibitRadius))
            {
                return false;
            }
            if (myAtb.Previous != null && myAtb.Previous.Previous != null && !ValidLocation(myAtb.Previous.Previous, p, prohibitRadius))
            {
                return false;
            }
            return true;
        }

        private bool ValidLocation(TreeBox atb, Vector3 p, float prohibitRadius)
        {
            foreach (Vector3 item in atb.ExistentTree())
            {
                if (Vector3.Distance(item, p) < prohibitRadius)
                {
                    return false;
                }
            }
            return true;
        }

        private bool PointinTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3 vector = C - A;
            Vector3 vector2 = B - A;
            Vector3 rhs = P - A;
            float num = Vector3.Dot(vector, vector);
            float num2 = Vector3.Dot(vector, vector2);
            float num3 = Vector3.Dot(vector, rhs);
            float num4 = Vector3.Dot(vector2, vector2);
            float num5 = Vector3.Dot(vector2, rhs);
            float num6 = 1f / (num * num4 - num2 * num2);
            float num7 = (num4 * num3 - num2 * num5) * num6;
            if (num7 < 0f || num7 > 1f)
            {
                return false;
            }
            float num8 = (num * num5 - num2 * num3) * num6;
            if (num8 < 0f || num8 > 1f)
            {
                return false;
            }
            return num7 + num8 <= 1f;
        }

        public List<Vector3> GetTreesPoint()
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < m_autoTreeBoxes.Count; i++)
            {
                list.AddRange(m_autoTreeBoxes[i].ExistentTree());
            }
            return list;
        }
    }
}