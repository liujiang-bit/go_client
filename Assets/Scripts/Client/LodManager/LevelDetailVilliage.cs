using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LevelDetailVilliage : LevelDetailBase
    {
        public AnimationCurve m_scale_curve;
        public float m_max_scale = 7f;
        public override void UpdateLod()
        {
            base.UpdateLod();
            float scale = Mathf.Min(m_scale_curve.Evaluate(Common.GetLodDistance()),m_max_scale);
            this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}