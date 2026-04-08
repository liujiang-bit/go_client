using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class LightingHandler : MonoBehaviour
    {
        public Light m_direction_light;

        [HideInInspector]
        public bool m_is_updating_light;

        [HideInInspector]
        public float m_update_timer;

        [HideInInspector]
        public float m_update_time = 1f;

        [HideInInspector]
        public Color m_org_ambient_color = Color.black;

        [HideInInspector]
        public Color m_org_direction_color = Color.black;

        [HideInInspector]
        public float m_org_direction_intensity = 1f;

        [HideInInspector]
        public Color m_new_ambient_color = Color.black;

        [HideInInspector]
        public Color m_new_direction_color = Color.black;

        [HideInInspector]
        public float m_new_direction_intensity = 1f;

        private static Color m_camera_fill_base_color = new Color(0.5255f, 0.6314f, 0.3216f, 1f);

        private static float m_fade_alpha = 1f;

        private static Color m_sprite_color;

        [HideInInspector]
        public Color m_light_ambient_color = Color.black;

        [HideInInspector]
        public Color m_light_direction_color = Color.black;

        [HideInInspector]
        public float m_light_direction_intensity = 1f;

        [HideInInspector]
        public Color m_thunder_ambient_color = Color.black;

        [HideInInspector]
        public Color m_thunder_direction_color = Color.black;

        [HideInInspector]
        public float m_thunder_direction_intensity = 1f;

        private List<Thunder> m_thunderList = new List<Thunder>();

        public static Color cameraFillBaseColor
        {
            get
            {
                return LightingHandler.m_camera_fill_base_color;
            }
            set
            {
                LightingHandler.m_camera_fill_base_color = value;
                //Camera camera = WorldCameraImpl.GetInstance().GetCamera();
                //> todo
                Camera camera = Camera.main;
                if (camera != null)
                {
                    camera.backgroundColor = LightingHandler.m_sprite_color * LightingHandler.m_camera_fill_base_color;
                }
            }
        }

        public static float fadeAlpha
        {
            get
            {
                return LightingHandler.m_fade_alpha;
            }
            set
            {
                LightingHandler.m_fade_alpha = value;
                LightingHandler.m_sprite_color.a = LightingHandler.m_fade_alpha;
                Shader.SetGlobalColor("_SpriteColor", LightingHandler.m_sprite_color);
            }
        }

        private void Start()
        {
            this.UpdateSpriteColor();
        }

        public void AddThunder(float intensity, float fadein, float fadeout)
        {
            Thunder item = new Thunder(intensity, fadein, fadeout);
            this.m_thunderList.Add(item);
        }

        private void Update()
        {
            try
            {
                if (this.m_is_updating_light || this.m_thunderList.Count > 0)
                {
                    Color color = RenderSettings.ambientLight;
                    float num = this.m_direction_light.intensity;
                    Color color2 = this.m_direction_light.color;
                    if (this.m_is_updating_light)
                    {
                        this.m_update_timer += Time.deltaTime;
                        float num2 = this.m_update_timer / this.m_update_time;
                        if (num2 > 1f)
                        {
                            num2 = 1f;
                        }
                        color = Color.Lerp(this.m_org_ambient_color, this.m_new_ambient_color, num2);
                        num = Mathf.Lerp(this.m_org_direction_intensity, this.m_new_direction_intensity, num2);
                        color2 = Color.Lerp(this.m_org_direction_color, this.m_new_direction_color, num2);
                        if (num2 >= 1f)
                        {
                            this.m_is_updating_light = false;
                            this.m_update_timer = 0f;
                        }
                        this.m_light_ambient_color = color;
                        this.m_light_direction_intensity = num;
                        this.m_light_direction_color = color2;
                    }
                    if (this.m_thunderList.Count > 0)
                    {
                        color = this.m_light_ambient_color;
                        num = this.m_light_direction_intensity;
                        color2 = this.m_light_direction_color;
                        float num3 = 0f;
                        for (int i = this.m_thunderList.Count - 1; i >= 0; i--)
                        {
                            Thunder thunder = this.m_thunderList[i];
                            thunder.m_timer += Time.deltaTime;
                            this.m_thunderList[i] = thunder;
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
                                this.m_thunderList.Remove(thunder);
                            }
                        }
                        color = Color.Lerp(color, this.m_thunder_ambient_color, num3);
                        num = Mathf.Lerp(num, this.m_thunder_direction_intensity, num3);
                        color2 = Color.Lerp(color2, this.m_thunder_direction_color, num3);
                    }
                    RenderSettings.ambientLight = color;
                    this.m_direction_light.intensity = num;
                    this.m_direction_light.color = color2;
                    this.UpdateSpriteColor();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateSpriteColor()
        {
            LightingHandler.m_sprite_color = RenderSettings.ambientLight + this.m_direction_light.intensity * this.m_direction_light.color * 0.75f;
            LightingHandler.m_sprite_color.a = LightingHandler.fadeAlpha;
            Shader.SetGlobalColor("_SpriteColor", LightingHandler.m_sprite_color);
            //Camera camera = WorldCameraImpl.GetInstance().GetCamera();
            //> todo
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.backgroundColor = LightingHandler.m_sprite_color * LightingHandler.m_camera_fill_base_color;
            }
        }
    }
}