using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class LightingManager
    {
        private static readonly LightingManager m_instance = new LightingManager();

        private List<NightObject> m_night_object_list = new List<NightObject>();

        private LightingHandler m_lighting_handler;

        private bool m_is_night;

        public LightingHandler lightingHandler
        {
            get
            {
                return this.m_lighting_handler;
            }
            set
            {
                this.m_lighting_handler = value;
            }
        }

        public bool isNight
        {
            get
            {
                return this.m_is_night;
            }
            set
            {
                this.m_is_night = value;
            }
        }

        public static LightingManager GetInstance()
        {
            return LightingManager.m_instance;
        }

        public void UpdateLighting(Color org_ambient_color, Color org_direction_color, float org_direction_intensity, Color new_ambient_color, Color new_direction_color, float new_direction_intensity, float update_time, float passed_time)
        {
            this.m_lighting_handler.m_org_ambient_color = org_ambient_color;
            this.m_lighting_handler.m_org_direction_color = org_direction_color;
            this.m_lighting_handler.m_org_direction_intensity = org_direction_intensity;
            this.m_lighting_handler.m_new_ambient_color = new_ambient_color;
            this.m_lighting_handler.m_new_direction_color = new_direction_color;
            this.m_lighting_handler.m_new_direction_intensity = new_direction_intensity;
            this.m_lighting_handler.m_update_time = update_time;
            this.m_lighting_handler.m_update_timer = passed_time;
            RenderSettings.ambientLight = org_ambient_color;
            this.m_lighting_handler.m_direction_light.intensity = org_direction_intensity;
            this.m_lighting_handler.m_direction_light.color = org_direction_color;
            this.m_lighting_handler.m_is_updating_light = true;
        }

        public void UpdateThunderLighting(Color new_ambient_color, Color new_direction_color, float new_direction_intensity, float fade_in_time, float fade_out_time, float passed_time)
        {
            this.m_lighting_handler.m_thunder_ambient_color = new_ambient_color;
            this.m_lighting_handler.m_thunder_direction_color = new_direction_color;
            this.m_lighting_handler.AddThunder(new_direction_intensity, fade_in_time, fade_out_time);
        }

        public void RegisterNightObject(NightObject obj)
        {
            if (!this.m_night_object_list.Contains(obj))
            {
                this.m_night_object_list.Add(obj);
            }
        }

        public void SetIsNight(bool is_night)
        {
            if (is_night != isNight)
            {
                List<int> list = new List<int>();
                for (int i = 0; i < this.m_night_object_list.Count; i++)
                {
                    if (m_night_object_list[i] != null)
                    {
                        m_night_object_list[i].SetLightOn(is_night);
                    }
                    else
                    {
                        list.Add(i);
                    }
                }
                for (int j = list.Count - 1; j >= 0; j--)
                {
                    m_night_object_list.RemoveAt(list[j]);
                }
            }
            isNight = is_night;
        }
    }
}