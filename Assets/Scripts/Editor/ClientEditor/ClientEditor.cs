using Skyunion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Client
{
    public class ClientEditor : EditorWindow
    {
        private float m_cityAlpha;
        private Color m_SpriteColor;
        private Light m_light = null;
        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            Shader.SetGlobalFloat("_CityTransparency", 1.0f);

            var strColor = EditorPrefs.GetString("_SpriteColor", ColorUtility.ToHtmlStringRGBA(Color.white));
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + strColor, out color))
            {
                Shader.SetGlobalColor("_SpriteColor", color);
            }
            Shader.SetGlobalFloat("_TreeScale", 1.0f);
            Shader.SetGlobalFloat("_FogShade", 1.0f);

        }
        void OnEnable()
        {
            m_light = GameObject.FindObjectOfType<Light>();
            m_cityAlpha =  EditorPrefs.GetFloat("_CityTransparency", 1.0f);
            Shader.SetGlobalFloat("_CityTransparency", m_cityAlpha);
            
            var strColor = EditorPrefs.GetString("_SpriteColor", ColorUtility.ToHtmlStringRGBA(Color.white));
            Color color;
            if(ColorUtility.TryParseHtmlString("#" + strColor, out color))
            {
                Shader.SetGlobalColor("_SpriteColor", color);
            }
        }
        void OnDisable()
        {
        }
        internal static void MenuOpenWindow()
        {
            ClientEditor editor = (ClientEditor)GetWindow(typeof(ClientEditor), false, "ClientEditor", true);
        }
        void OnGUI()
        {
            Color spriteColor = Shader.GetGlobalColor("_SpriteColor");
            Color newColor = EditorGUILayout.ColorField("_SpriteColor", spriteColor);
            if(!newColor.Equals(spriteColor))
            {
                m_SpriteColor = newColor;
                Shader.SetGlobalColor("_SpriteColor", newColor);
                EditorPrefs.SetString("_SpriteColor", ColorUtility.ToHtmlStringRGBA(m_SpriteColor));
            }
            float CityTransparency = Shader.GetGlobalFloat("_CityTransparency");
            float newCityTransparency = EditorGUILayout.Slider("_CityTransparency", CityTransparency, 0, 1);
            if (!newCityTransparency.Equals(CityTransparency))
            {
                m_cityAlpha = newCityTransparency;
                Shader.SetGlobalFloat("_CityTransparency", newCityTransparency);
                EditorPrefs.SetFloat("_CityTransparency", m_cityAlpha);
            }

            if (Application.isPlaying)
            {
                bool bIsNight = EditorGUILayout.ToggleLeft("夜晚", LightManager.Instance().isNight);
                if(bIsNight != LightManager.Instance().isNight)
                {
                    if (bIsNight)
                    {
                        LightManager.Instance().SetIsNight(bIsNight);
                    }
                    else
                    {
                        LightManager.Instance().SetIsNight(bIsNight);
                    }
                }

                if(m_light != null)
                {
                    EditorGUILayout.BeginVertical();
                    var amcolor = EditorGUILayout.ColorField("ambientLight", RenderSettings.ambientLight);
                    EditorGUILayout.LabelField(amcolor.ToString());
                    var intensity = EditorGUILayout.FloatField("intensity", m_light.intensity);
                    var color = EditorGUILayout.ColorField("color", m_light.color);
                    EditorGUILayout.LabelField(amcolor.ToString());
                    if (amcolor.Equals(RenderSettings.ambientLight) || intensity.Equals(m_light.intensity) || color.Equals(m_light.color))
                    {
                        LightManager.Instance().UpdateLighting(amcolor, color, intensity, amcolor, color, intensity, 1.0f, 1.0f);
                        LightManager.Instance().Update();
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
}