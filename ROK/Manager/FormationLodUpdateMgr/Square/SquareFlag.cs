using System;
using UnityEngine;

namespace ROK
{
    public class SquareFlag : LodBase
    {
        public AnimationCurve m_scale_curve;

        public override void UpdateLod()
        {
            base.UpdateLod();
            float lodDistance = Common.GetLodDistance();
            float num = this.m_scale_curve.Evaluate(lodDistance);
            base.transform.localScale = new Vector3(num, num, num);
        }
    }
}