using System;
using UnityEngine;

namespace ROK
{
    public class LodDeposit : LodCastle
    {
        public float m_fade_in_time = 2f;

        private float m_fade_in_timer;

        private bool m_is_fade_in;

        public GameObject m_ground_object;

        public void FadeIn()
        {
            this.m_is_fade_in = true;
        }

        private void Update()
        {
            try
            {
                if (this.m_is_fade_in)
                {
                    this.m_fade_in_timer += Time.deltaTime;
                    if (this.m_fade_in_timer >= this.m_fade_in_time)
                    {
                        this.m_fade_in_timer = 0f;
                        this.m_is_fade_in = false;
                        this.m_tile_collide.SetTargetAlpha(0f);
                    }
                    else
                    {
                        float targetAlpha = 1f - this.m_fade_in_timer / this.m_fade_in_time;
                        this.m_tile_collide.SetTargetAlpha(targetAlpha);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private new void OnSpawn()
        {
            this.UpdateGroundObject();
            base.OnSpawn();
        }

        private new void OnDespawn()
        {
            this.m_tile_collide.SetTargetAlpha(1f);
            this.m_fade_in_timer = 0f;
            this.m_is_fade_in = false;
            base.OnDespawn();
        }

        public override void UpdateLod()
        {
            if (base.IsLodChanged())
            {
                this.UpdateGroundObject();
            }
            base.UpdateLod();
        }

        private void UpdateGroundObject()
        {
            if (this.m_ground_object != null)
            {
                int currentLodLevel = base.GetCurrentLodLevel();
                if (currentLodLevel == 0)
                {
                    this.m_ground_object.SetActive(true);
                }
                else if (currentLodLevel == 1)
                {
                    this.m_ground_object.SetActive(false);
                }
            }
        }
    }
}