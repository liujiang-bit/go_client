using System;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class LightManager : MonoSingleton<LightManager>
    {
        private Light m_direction_light;

        private bool m_is_updating_light;

        private float m_update_timer;

        private float m_update_time = 1f;

        private Color m_org_ambient_color = Color.black;

        private Color m_org_direction_color = Color.black;

        private float m_org_direction_intensity = 1f;

        private Color m_new_ambient_color = Color.black;

        private Color m_new_direction_color = Color.black;

        private float m_new_direction_intensity = 1f;

        private static Color m_camera_fill_base_color = new Color(0.5255f, 0.6314f, 0.3216f, 1f);

        private static float m_fade_alpha = 1f;

        private static Color m_sprite_color;

        private Color m_light_ambient_color = Color.black;

        private Color m_light_direction_color = Color.black;

        private float m_light_direction_intensity = 1f;

        private Color m_thunder_ambient_color = Color.black;

        private Color m_thunder_direction_color = Color.black;

        private float m_thunder_direction_intensity = 1f;

        private List<Lighting> m_thunderList = new List<Lighting>();

        public Color GetCameraFillBaseColor()
        {
            return m_camera_fill_base_color;
        }

        public void SetCameraFillBaseColor(Color value)
        {
            m_camera_fill_base_color = value;
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.backgroundColor = m_sprite_color * m_camera_fill_base_color;
            }
        }

        public float GetfadeAlpha()
        {
            return m_fade_alpha;
        }

        public void SetfadeAlpha(float value)
        {
            m_fade_alpha = value;
            m_sprite_color.a = m_fade_alpha;
            Shader.SetGlobalColor("_SpriteColor", m_sprite_color);
        }

        private void AddThunder(float intensity, float fadein, float fadeout)
        {
            Lighting item = new Lighting(intensity, fadein, fadeout);
            m_thunderList.Add(item);
        }

        public override void Init()
        {
            m_direction_light = GameObject.FindObjectOfType<Light>();
        }

        public void Update()
        {
            try
            {
                if(m_direction_light == null)
                {
                    m_direction_light = GameObject.FindObjectOfType<Light>();
                    if (m_direction_light != null)
                    {
                        m_direction_light.intensity = m_org_direction_intensity;
                        m_direction_light.color = m_org_direction_color;
                    }
                    else
                    {
                        return;
                    }
                }
                if (m_is_updating_light || m_thunderList.Count > 0)
                {
                    Color color = RenderSettings.ambientLight;
                    float num = m_direction_light.intensity;
                    Color color2 = m_direction_light.color;
                    if (m_is_updating_light)
                    {
                        m_update_timer += Time.deltaTime;
                        float num2 = m_update_timer / m_update_time;
                        if (num2 > 1f)
                        {
                            num2 = 1f;
                        }
                        color = Color.Lerp(m_org_ambient_color, m_new_ambient_color, num2);
                        num = Mathf.Lerp(m_org_direction_intensity, m_new_direction_intensity, num2);
                        color2 = Color.Lerp(m_org_direction_color, m_new_direction_color, num2);
                        if (num2 >= 1f)
                        {
                            m_is_updating_light = false;
                            m_update_timer = 0f;
                        }
                        m_light_ambient_color = color;
                        m_light_direction_intensity = num;
                        m_light_direction_color = color2;
                    }
                    if (m_thunderList.Count > 0)
                    {
                        color = m_light_ambient_color;
                        num = m_light_direction_intensity;
                        color2 = m_light_direction_color;
                        float num3 = 0f;
                        for (int i = m_thunderList.Count - 1; i >= 0; i--)
                        {
                            Lighting thunder = m_thunderList[i];
                            thunder.m_timer += Time.deltaTime;
                            m_thunderList[i] = thunder;
                            if (thunder.m_timer <= thunder.m_fadein_time)
                            {
                                float b = thunder.m_timer / thunder.m_fadein_time;
                                Mathf.Clamp01(num3);
                                num3 = Mathf.Max(num3, b);
                            }
                            else if (thunder.m_timer <= thunder.m_fadein_time + thunder.m_fadeout_time)
                            {
                                float b2 = 1f - (thunder.m_timer - thunder.m_fadein_time) / thunder.m_fadeout_time;
                                Mathf.Clamp01(num3);
                                num3 = Mathf.Min(num3, b2);
                            }
                            else
                            {
                                m_thunderList.Remove(thunder);
                            }
                        }
                        color = Color.Lerp(color, m_thunder_ambient_color, num3);
                        num = Mathf.Lerp(num, m_thunder_direction_intensity, num3);
                        color2 = Color.Lerp(color2, m_thunder_direction_color, num3);
                    }
                    RenderSettings.ambientLight = color;
                    m_direction_light.intensity = num;
                    m_direction_light.color = color2;
                    UpdateSpriteColor();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateSpriteColor()
        {
            if (m_direction_light == null)
                return;
            m_sprite_color = RenderSettings.ambientLight + m_direction_light.intensity * m_direction_light.color * 0.75f;
            m_sprite_color.a = GetfadeAlpha();
            Shader.SetGlobalColor("_SpriteColor", m_sprite_color);
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.backgroundColor = m_sprite_color * m_camera_fill_base_color;
            }
        }

        private List<NightingObject> m_night_object_list = new List<NightingObject>();

        public bool isNight { get; private set; }

        public void UpdateLighting(Color org_ambient_color, Color org_direction_color, float org_direction_intensity, Color new_ambient_color, Color new_direction_color, float new_direction_intensity, float update_time, float passed_time)
        {
            m_org_ambient_color = org_ambient_color;
            m_org_direction_color = org_direction_color;
            m_org_direction_intensity = org_direction_intensity;
            m_new_ambient_color = new_ambient_color;
            m_new_direction_color = new_direction_color;
            m_new_direction_intensity = new_direction_intensity;
            m_update_time = update_time;
            m_update_timer = passed_time;
            RenderSettings.ambientLight = org_ambient_color;
            if (m_direction_light != null)
            {
                m_direction_light.intensity = org_direction_intensity;
                m_direction_light.color = org_direction_color;
            }
            m_is_updating_light = true;
        }

        public void UpdateThunderLighting(Color new_ambient_color, Color new_direction_color, float new_direction_intensity, float fade_in_time, float fade_out_time, float passed_time)
        {
            m_thunder_ambient_color = new_ambient_color;
            m_thunder_direction_color = new_direction_color;
            AddThunder(new_direction_intensity, fade_in_time, fade_out_time);
        }

        public void RegisterNightObject(NightingObject obj)
        {
            if (!m_night_object_list.Contains(obj))
            {
                m_night_object_list.Add(obj);
            }
        }

        public void SetIsNight(bool is_night)
        {
            if (is_night != isNight)
            {
                List<int> list = new List<int>();
                for (int i = 0; i < m_night_object_list.Count; i++)
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