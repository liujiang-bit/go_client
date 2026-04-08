using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using Data;

namespace Game
{
    public enum FrameRateLevel
    {
        NONE,
        LOW,
        MEDIUM,
        HIGH
    }

    public class QualitySetting
    {

        private static Resolution m_originResolution;
        private static float m_platformScore = -1f;

        public static void Init()
        {
            m_originResolution = Screen.currentResolution;
            //默认关闭垂直同步
            var graphicLevel = GetGraphicLevel();
            // 还没设置过画质需要设置一下
            bool bChange = false;
            if (graphicLevel == 0)
            {
                graphicLevel = GetDefaultGraphicLevel();
                bChange = true;
            }
            SetGraphicLevel(graphicLevel);

            var frameRateLevel = GetFrameRateLevel();
            // 还没设置过画质需要设置一下
            if (frameRateLevel == 0)
            {
                frameRateLevel = GetDefaultFrameRateLevel();
                bChange = true;
            }
            SetFrameRateLevel(frameRateLevel);

            if(bChange)
            {
                PlayerPrefs.Save();
            }
        }

        public static CoreUtils.GraphicLevel GetDefaultGraphicLevel()
        {
            CoreUtils.GraphicLevel graphicLevel;
            if (SystemInfo.systemMemorySize > 1000 * 3)
            {
                graphicLevel = CoreUtils.GraphicLevel.HIGH;
            }
            else if (SystemInfo.systemMemorySize > 1000 * 2)
            {
                graphicLevel = CoreUtils.GraphicLevel.MEDIUM;
            }
            else
            {
                graphicLevel = CoreUtils.GraphicLevel.LOW;
            }

            return graphicLevel;
        }

        public static FrameRateLevel GetDefaultFrameRateLevel()
        {
            FrameRateLevel frameRateLevel;
            if (SystemInfo.systemMemorySize > 1000 * 3)
            {
                frameRateLevel = FrameRateLevel.HIGH;
            }
            else if (SystemInfo.systemMemorySize > 1000 * 2)
            {
                frameRateLevel = FrameRateLevel.MEDIUM;
            }
            else
            {
                frameRateLevel = FrameRateLevel.LOW;
            }

            return frameRateLevel;
        }

        public static CoreUtils.GraphicLevel GetGraphicLevel()
        {
            return (CoreUtils.GraphicLevel)PlayerPrefs.GetInt("GrahicLevel", 0);
        }

        public static void SetGraphicLevel(CoreUtils.GraphicLevel value)
        {
            CoreUtils.SetGraphicLevel((int)value);
            PlayerPrefs.SetInt("GrahicLevel", (int)value);
        }

        public static FrameRateLevel GetFrameRateLevel()
        {
            return (FrameRateLevel)PlayerPrefs.GetInt("FrameRateLevel", 0);
        }

        public static void SetFrameRateLevel(FrameRateLevel level)
        {
            //默认关闭垂直同步
            QualitySettings.vSyncCount = 0;
            if (QualityDefines != null && QualityDefines.Count > 0)
            {
                switch (level)
                {
                    case FrameRateLevel.LOW:
                        Application.targetFrameRate = QualityDefines[0].quality1;
                        break;
                    case FrameRateLevel.MEDIUM:
                        Application.targetFrameRate = QualityDefines[0].quality2;
                        break;
                    case FrameRateLevel.HIGH:
                        Application.targetFrameRate = QualityDefines[0].quality3;
                        break;
                    default:
                        break;
                }
            }
            PlayerPrefs.SetInt("FrameRateLevel", (int)level);
        }

        private static List<QualitySetDefine> m_qualityDefines;
        public static List<QualitySetDefine> QualityDefines
        {
            get
            {
                if(m_qualityDefines==null)
                {
                    m_qualityDefines = CoreUtils.dataService.QueryRecords<QualitySetDefine>();
                }
                return m_qualityDefines;
            }
        }
    }
}


