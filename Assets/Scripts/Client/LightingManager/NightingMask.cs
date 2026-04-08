using System;
using System.Collections;
using UnityEngine;

namespace Client
{
    public class NightingMask : NightingObject
    {
        private SpriteRenderer m_sprite_render;

        private string m_night = "city_building_night";

        private string m_day = "map_building";

        private bool m_canChange = true;

        private void Awake()
        {
            m_sprite_render = GetComponent<SpriteRenderer>();
        }

        protected override IEnumerator DoSetLightOn(bool b, float delay)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                DoSetLightOnNow(b);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected override void DoSetLightOnNow(bool b)
        {
            if (m_night != null && m_day != null)
            {
                MaskSprite component = GetComponent<MaskSprite>();
                if (!(component == null) && m_canChange)
                {
                    string mat_path = (!b) ? m_day : m_night;
                    component.UpdatedMaterial(mat_path);
                }
            }
        }

        public void SetCanChangeLight(bool canChange)
        {
            m_canChange = canChange;
        }
    }
}