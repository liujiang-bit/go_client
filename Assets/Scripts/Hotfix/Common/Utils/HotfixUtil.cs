using Data;
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HotfixUtil
    {
        private static TableBinary<LanguageImgDefine> mLanguageTable = null;
        
        
        public static string getLanguageImage(int id)
        {
            LanguageImgDefine lan = null;

            lan = CoreUtils.dataService.QueryRecord<LanguageImgDefine>(id);

            if (lan == null)
            {
                return "Not Found:" + id;
            }

            switch (LanguageUtils.GetLanguage())
            {
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseTraditional:
                    return lan.cn;
                case SystemLanguage.English:
                    return lan.en;
                case SystemLanguage.Arabic:
                    return lan.arabic;
                case SystemLanguage.Turkish:
                    return lan.tr;
                case SystemLanguage.Russian:
                    return lan.ru;
                default:
                    return lan.en;
            }
        }
        public static string getLanguageLink(int id)
        {
            Data.HyperlinkDefine linkCfg = CoreUtils.dataService.QueryRecord<Data.HyperlinkDefine>(id);
            if (linkCfg == null)
            {
                Debug.LogWarning("Not Found:" + id);
                return string.Empty;
            }
            var language = LanguageUtils.GetLanguage();
            string url = string.Empty;
            switch (language)
            {
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseTraditional:
                    url = linkCfg.cn;
                    break;
                case SystemLanguage.English:
                    url = linkCfg.en;
                    break;
                case SystemLanguage.Arabic:
                    url = linkCfg.arabic;
                    break;
                case SystemLanguage.Turkish:
                    url = linkCfg.tr;
                    break;
                case SystemLanguage.Russian:
                    url = linkCfg.ru;
                    break;
                default:
                    url = linkCfg.en;
                    break;
            }
            return url;
        }
        public static string getLanguageLinkName(int id)
        {
            Data.HyperlinkDefine linkCfg = CoreUtils.dataService.QueryRecord<Data.HyperlinkDefine>(id);
            if (linkCfg == null)
            {
                Debug.LogWarning("Not Found:" + id);
                return string.Empty;
            }
            return LanguageUtils.getText(linkCfg.l_comment);
        }

        public static bool IsEnumDefine(Type enumTYpe, int value)
        {
            Array values = Enum.GetValues(enumTYpe);
            for(int i = 0; i < values.Length; i++)
            {
                if((int)values.GetValue(i) == value)
                {
                    return true;
                }
            }

            return false;
        }
        public static string FormatFileSize(long fileSize)
        {
            if (fileSize < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} bytes", fileSize);
            }
        }

        public static bool IsLogShow()
        {
            return PlayerPrefs.GetInt("LogShow", 0) == 1;
        }

        public static void SetLogShow(bool bEnable)
        {
            PlayerPrefs.SetInt("LogShow", bEnable ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static bool IsDebugable()
        {
            return PlayerPrefs.GetInt("Debugable", 0) == 1;
        }

        public static void SetDebugable(bool bEnable)
        {
            PlayerPrefs.SetInt("Debugable", bEnable?1:0);
            PlayerPrefs.Save();
        }
        public static bool IsShowLoginView()
        {
            return PlayerPrefs.GetInt("LoginView", 0) == 0;
        }

        public static void SetShowLoginView(bool bEnable)
        {
            PlayerPrefs.SetInt("LoginView", bEnable ? 1 : 0);
            PlayerPrefs.Save();
        }
        public static bool IsDebugServerConfig()
        {
            return PlayerPrefs.GetInt("DebugServerConfig", 0) == 1;
        }

        public static void SetDebugServerConfig(bool bEnable)
        {
            PlayerPrefs.SetInt("DebugServerConfig", bEnable ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void OpenUrl(string url, string title="")
        {
            var param = new UI_Win_WebViewMediator.Param();
            param.url = url;
            param.title = title;

            CoreUtils.uiManager.ShowUI(UI.s_WebView, null, param);
        }
        public static void OpenBrowser(string url, string title = "")
        {
            var param = new UI_Win_WebViewMediator.Param();
            param.url = url;
            param.title = title;

            CoreUtils.uiManager.ShowUI(UI.s_WebView, null, param);
        }


        private static HashSet<string> FrameSets = new HashSet<string>();
        private static List<Action> FrameActionList = new List<Action>();
        public static void InvokOncePerfOneFrame(string key, Action action)
        {
            if(FrameSets.Contains(key))
            {
                return;
            }
            FrameSets.Add(key);
            FrameActionList.Add(action);
        }

        public static void Clear()
        {
            for(int i = 0; i < FrameActionList.Count; i++)
            {
                try
                {
                    FrameActionList[i]?.Invoke();
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            FrameSets.Clear();
            FrameActionList.Clear();
        }
    }
}