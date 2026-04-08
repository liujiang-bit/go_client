using System;
using UnityEngine;

namespace Client
{
    public class LevelDetailScale : LevelDetailBase
    {

        [Header("是否是程序调用探索资源")]
        public bool IsExplore = false;
        
        [Header("图标根据镜头高度缩放")]
        public AnimationCurve m_scale_curve;
        
        [Header("选择探索的情况下高度缩放")]
        public AnimationCurve m_scale_explore_curve;

        public override void UpdateLod()
        {
            float num = 0;

            if (IsExplore)
            {
                num = this.m_scale_explore_curve.Evaluate(ClientUtils.lodManager.GetLodDistance());
            }
            else
            {
                num = this.m_scale_curve.Evaluate(ClientUtils.lodManager.GetLodDistance());
            }
            base.transform.localScale = new Vector3(num, num, num);
            base.UpdateLod();
        }
    }
}
