using UnityEngine;

namespace Client
{
    public class MatrixFlag : LevelDetailBase
    {
        public AnimationCurve m_scale_curve;

        public override void UpdateLod()
        {
            base.UpdateLod();
            float lodDistance = Common.GetLodDistance();
            float num = m_scale_curve.Evaluate(lodDistance);
            base.transform.localScale = new Vector3(num, num, num);
        }
    }
}