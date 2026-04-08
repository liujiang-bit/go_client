using System;
using UnityEngine;

namespace ROK
{
    public class LodScalerMgr : LodBase
    {
        public AnimationCurve m_tree_scale_curve;

        public AnimationCurve m_unit_scale_curve;

        public AnimationCurve m_unitModel_scale_curve;

        public static LodScalerMgr instance;

        private int squares_num_in_screen;

        private int squares_num_in_screen_threshold = 20;

        private float m_unit_scale = 1f;

        private float m_unitModel_scale = 1f;

        public float unitScale
        {
            get
            {
                return this.m_unit_scale;
            }
            set
            {
                this.m_unit_scale = value;
            }
        }

        public float unitModelScale
        {
            get
            {
                return this.m_unitModel_scale;
            }
            set
            {
                this.m_unitModel_scale = value;
            }
        }

        private void Awake()
        {
            LodScalerMgr.instance = this;
        }

        public override void UpdateLod()
        {
            float lodDistance = Common.GetLodDistance();
            Shader.SetGlobalFloat("_TreeScale", this.m_tree_scale_curve.Evaluate(lodDistance));
            this.unitScale = this.m_unit_scale_curve.Evaluate(lodDistance);
            this.unitModelScale = this.m_unitModel_scale_curve.Evaluate(lodDistance);
        }

        public void SynSquresNumInScreen(int num, int threshold)
        {
            this.squares_num_in_screen = num;
            this.squares_num_in_screen_threshold = threshold;
        }

        public bool isGreatManySqureInScreen()
        {
            return this.squares_num_in_screen > this.squares_num_in_screen_threshold;
        }
    }
}