using System;
using UnityEngine;

namespace ROK
{
    public class LodAutoScale : LodBase
    {
        public AnimationCurve m_scale_curve;

        public override void UpdateLod()
        {
            float num = this.m_scale_curve.Evaluate(Common.GetLodDistance());
            base.transform.localScale = new Vector3(num, num, num);
            base.UpdateLod();
        }
    }
}
