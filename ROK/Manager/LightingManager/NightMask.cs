using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class NightMask : NightObject
    {
        private SpriteRenderer m_sprite_render;

        private string m_night = "city_building_night";

        private string m_day = "map_building";

        private bool m_canChange = true;

        private void Awake()
        {
            this.m_sprite_render = base.GetComponent<SpriteRenderer>();
        }

        protected override void DoSetLightOnNow(bool b)
        {
            if (this.m_night == null || this.m_day == null)
            {
                return;
            }
            SpriteDualHelper component = base.GetComponent<SpriteDualHelper>();
            if (component == null)
            {
                return;
            }
            if (this.m_canChange)
            {
                string mat_path = (!b) ? this.m_day : this.m_night;
                SpriteDualHelper.GetUpdatedMaterial_S(component, mat_path, null);
            }
        }

        public void SetCanChangeLight(bool canChange)
        {
            this.m_canChange = canChange;
        }
    }
}
