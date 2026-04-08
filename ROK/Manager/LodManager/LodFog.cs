using System;
using UnityEngine;

namespace ROK
{
    public class LodFog : LodBase
    {
        public AnimationCurve m_curve;

        public override void UpdateLod()
        {
            Shader.SetGlobalFloat("_DepthTweak", this.m_curve.Evaluate(Common.GetLodDistance()));
            base.UpdateLod();
        }
    }
}
