using UnityEngine;

/// <summary>
/// 挂载在CityBuildingContainer
/// </summary>
namespace Client
{
    public class LevelDetailCastle : LevelDetailBase
    {
        protected GridCollideMgr m_tile_collide;

        public AnimationCurve m_scale_curve;

        public float m_max_scale = 7f;

        private void Awake()
        {
            m_tile_collide = GetComponent<GridCollideMgr>();
        }

        public override void UpdateLod()
        {
            // int currentLodLevel = GetCurrentLodLevel();

            // GetPreviousLodLevel();
            // if (currentLodLevel <= 2)
            // {
//                Debug.Log("主城 lodlv："+currentLodLevel+"  cam_dis:"+Common.GetLodDistance()+" sc:"+m_scale_curve.Evaluate(Common.GetLodDistance()));
                m_tile_collide.SetScale(Mathf.Min(Mathf.Max(m_scale_curve.Evaluate(Common.GetLodDistance()),0), m_max_scale));
            // }
            // else if (currentLodLevel < 3)
            // {
            // }

            base.UpdateLod();
        }

        public static void SetMaxScaleS(LevelDetailCastle self, float max_scale)
        {
            self.SetMaxScale(max_scale);
        }

        private void SetMaxScale(float max_scale)
        {
            m_max_scale *= max_scale;
        }

        public static float GetCurrentCityScaleS(LevelDetailCastle self)
        {
            return self.GetCurrentCityScale();
        }

        private float GetCurrentCityScale()
        {
            return m_scale_curve.Evaluate(Common.GetLodDistance());
        }

        public static void ForceUpdateScaleS(LevelDetailCastle self)
        {
            self.m_tile_collide.SetScale(
                Mathf.Min(Mathf.Max(self.m_scale_curve.Evaluate(Common.GetLodDistance()),0), self.m_max_scale), isforce: true);
        }
    }
}