using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class LandformGizmosHelper : MonoBehaviour
    {
        public bool DrawTreeLimitLine = true;

        public bool DrawTreeBaseLine = true;

        public bool DrawOutline = true;

        public bool DrawRealTreePoint = true;

        public bool DrawBuildingMeshTreePoint = true;

        private Transform m_treeBaseLineLayer;

        private Transform m_treeLimitLineLayer;

        private Transform m_treeOutLineLayer;

        private Transform m_buildingMeshLayer;

        private List<Vector3> m_treePoints = new List<Vector3>();

        public void SetLineBaseIns(Transform treeBaseLineLayer, Transform treeLimitLineLayer, Transform treeOutLineLayer, Transform buildingMeshLayer = null)
        {
            this.m_treeBaseLineLayer = treeBaseLineLayer;
            this.m_treeLimitLineLayer = treeLimitLineLayer;
            this.m_treeOutLineLayer = treeOutLineLayer;
            this.m_buildingMeshLayer = buildingMeshLayer;
        }

        public void SetTreePoints(List<Vector3> treePoints)
        {
            this.m_treePoints.Clear();
            this.m_treePoints.AddRange(treePoints);
        }

        private void OnDrawGizmos()
        {
            if (this.DrawTreeBaseLine && this.m_treeBaseLineLayer != null && this.m_treeBaseLineLayer.childCount > 0)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < this.m_treeBaseLineLayer.childCount; i++)
                {
                    Transform child = this.m_treeBaseLineLayer.GetChild(i);
                    if (child.childCount > 0)
                    {
                        for (int j = 0; j < child.childCount - 1; j++)
                        {
                            Gizmos.DrawLine(child.GetChild(j).position, child.GetChild(j + 1).position);
                        }
                        Gizmos.DrawLine(child.GetChild(child.childCount - 1).position, child.GetChild(0).position);
                    }
                }
            }
            if (this.DrawTreeLimitLine && this.m_treeLimitLineLayer != null && this.m_treeLimitLineLayer.childCount > 0)
            {
                Gizmos.color = Color.white;
                for (int k = 0; k < this.m_treeLimitLineLayer.childCount; k++)
                {
                    Transform child2 = this.m_treeLimitLineLayer.GetChild(k);
                    if (child2.childCount > 0)
                    {
                        for (int l = 0; l < child2.childCount - 1; l++)
                        {
                            Gizmos.DrawLine(child2.GetChild(l).position, child2.GetChild(l + 1).position);
                        }
                        Gizmos.DrawLine(child2.GetChild(child2.childCount - 1).position, child2.GetChild(0).position);
                    }
                }
            }
            if (this.DrawOutline && this.m_treeOutLineLayer != null && this.m_treeOutLineLayer.childCount > 0)
            {
                Gizmos.color = Color.red;
                for (int m = 0; m < this.m_treeOutLineLayer.childCount; m++)
                {
                    Transform child3 = this.m_treeOutLineLayer.GetChild(m);
                    if (child3.childCount > 0)
                    {
                        for (int n = 0; n < child3.childCount - 1; n++)
                        {
                            Gizmos.DrawLine(child3.GetChild(n).position, child3.GetChild(n + 1).position);
                        }
                        Gizmos.DrawLine(child3.GetChild(child3.childCount - 1).position, child3.GetChild(0).position);
                    }
                }
            }
            if (this.DrawRealTreePoint && this.m_treePoints.Count > 0)
            {
                Gizmos.color = Color.green;
                for (int num = 0; num <= this.m_treePoints.Count - 2; num += 3)
                {
                    Gizmos.DrawLine(this.m_treePoints[num], this.m_treePoints[num + 1]);
                    Gizmos.DrawLine(this.m_treePoints[num + 1], this.m_treePoints[num + 2]);
                    Gizmos.DrawLine(this.m_treePoints[num + 2], this.m_treePoints[num]);
                }
            }
            if (this.DrawBuildingMeshTreePoint && this.m_buildingMeshLayer != null && this.m_buildingMeshLayer.childCount > 0)
            {
                Gizmos.color = Color.yellow;
                for (int num2 = 0; num2 < this.m_buildingMeshLayer.childCount; num2++)
                {
                    Transform child4 = this.m_buildingMeshLayer.GetChild(num2);
                    if (child4.childCount > 0)
                    {
                        for (int num3 = 0; num3 < child4.childCount - 1; num3++)
                        {
                            Gizmos.DrawLine(child4.GetChild(num3).position, child4.GetChild(num3 + 1).position);
                        }
                        Gizmos.DrawLine(child4.GetChild(child4.childCount - 1).position, child4.GetChild(0).position);
                    }
                }
            }
        }
    }
}