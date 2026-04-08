using System;
using UnityEngine;

namespace Client
{
    public class LevelDetailFog : LevelDetailBase
    {
        public AnimationCurve m_curve;

        public override void UpdateLod()
        {
            Shader.SetGlobalFloat("_DepthTweak", this.m_curve.Evaluate(Common.GetLodDistance()));
            base.UpdateLod();
        }
    }
}
