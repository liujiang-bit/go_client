using System;
using UnityEngine;

namespace ROK
{
    public class LodPass : LodBase
    {
        [Serializable]
        public struct PassSides
        {
            public float LodDistance;

            public GameObject[] Objects;
        }

        public AnimationCurve m_scale_curve;

        public LodPass.PassSides[] m_sideObjects;

        private float m_dxf = 1f;

        public override void UpdateLod()
        {
            float lodDistance = Common.GetLodDistance();
            float num = this.m_scale_curve.Evaluate(lodDistance);
            base.transform.localScale = new Vector3(num, num, num);
            for (int i = 0; i < this.m_sideObjects.Length; i++)
            {
                float lodDistance2 = this.m_sideObjects[i].LodDistance;
                if (this.m_dxf < lodDistance2 && lodDistance >= lodDistance2)
                {
                    this.SetSideObjsShow(false, this.m_sideObjects[i].Objects);
                }
                else if (this.m_dxf >= lodDistance2 && lodDistance < lodDistance2)
                {
                    this.SetSideObjsShow(true, this.m_sideObjects[i].Objects);
                }
            }
            this.m_dxf = lodDistance;
            base.UpdateLod();
        }

        private void SetSideObjsShow(bool isShow, GameObject[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(isShow);
            }
        }
    }
}