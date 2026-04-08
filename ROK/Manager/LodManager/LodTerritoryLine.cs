using System;
using UnityEngine;

namespace ROK
{
    public class LodTerritoryLine : LodBase
    {
        public AnimationCurve m_scale_curve;

        public float m_tile_factor;

        public Material m_activeMaterial;

        public Material m_inActiveMaterial;

        public MeshFilter m_meshFilter;

        public MeshRenderer m_meshRenderer;

        private bool m_inited;

        private Vector2[] m_posArray;

        private Color m_color;

        private float m_uvStep;

        private float m_width;

        private float m_smoothDistance;

        private float m_lodWidth;

        private float m_oldLodWidth = -1f;

        private Material m_usingMaterial;

        private Vector2[] m_smoothPosArray;

        private bool m_canUpdate = true;

        public override void UpdateLod()
        {
            if (!this.m_inited)
            {
                return;
            }
            this.UpdateWidth();
            base.UpdateLod();
        }

        public void Init(Vector2[] pos_array, Color color, float uvStep, float width, float smooth_distance)
        {
            this.m_inited = true;
            this.m_color = color;
            this.m_uvStep = uvStep;
            this.m_width = width;
            this.m_smoothDistance = smooth_distance;
            this.m_posArray = pos_array;
            this.UpdateWidth();
        }

        public void UpdateWidth()
        {
            float lodDistance = Common.GetLodDistance();
            this.m_lodWidth = Mathf.Floor(this.m_scale_curve.Evaluate(lodDistance));
            if (this.m_canUpdate && this.m_lodWidth != this.m_oldLodWidth)
            {
                this.m_oldLodWidth = this.m_lodWidth;
                float num = this.m_width * this.m_lodWidth;
                float num2 = 0f;
                if (this.m_uvStep > 0f)
                {
                    num2 = this.m_uvStep * this.m_lodWidth * this.m_tile_factor;
                    if (num2 > 4f)
                    {
                        num2 = 0f;
                    }
                }
                Vector2[] points;
                if (num2 <= 0f && num >= 2f)
                {
                    points = this.m_posArray;
                }
                else
                {
                    if (this.m_smoothPosArray == null)
                    {
                        this.m_smoothPosArray = Common.SmoothLine(this.m_posArray, this.m_smoothDistance, 2, 0);
                    }
                    points = this.m_smoothPosArray;
                }
                Material material;
                if (num2 <= 0f)
                {
                    material = this.m_activeMaterial;
                }
                else
                {
                    material = this.m_inActiveMaterial;
                }
                if (this.m_usingMaterial != material)
                {
                    this.m_usingMaterial = material;
                    this.m_meshRenderer.sharedMaterial = material;
                }
                LinePolygon.CreateMesh(this.m_meshFilter.mesh, points, num, this.m_color, num2);
            }
        }

        public void SetCanUpdate(bool canUpdate)
        {
            this.m_canUpdate = canUpdate;
        }
    }
}