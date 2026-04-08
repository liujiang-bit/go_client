//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      游戏应用基类，初始化插件使用。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;
using System.Xml;
using System.Text;
using System;
using System.Reflection;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skyunion
{
    public class GameApp : MonoBehaviour
    {
        public bool mIsUsePluginConfig;
        public LogType logLevel = LogType.Exception;
        protected IPluginManager mPluginManager;
        protected virtual void OnAddPlugin()
        {
            PluginManager.Instance().Registered(new CorePlugin());
        }
        protected virtual void OnInitialized()
        {
        }
        protected virtual void OnAfterInitialized()
        {
        }

        public string mPlulicKey;
        private bool mCanUpdate = false;
        private float mStartTime;
        private string m_ObbPath = string.Empty;
        private IGooglePlayObbDownloader m_obbDownloader;
        private bool m_bDownloadObb = false;

        void Start()
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            var type = new Type[]{
                typeof(System.Globalization.CharUnicodeInfo),
                typeof(System.Globalization.CompareInfo),
                typeof(System.Globalization.CultureInfo),
                typeof(System.Globalization.CultureNotFoundException),
                typeof(System.Globalization.DateTimeFormatInfo),
                typeof(System.Globalization.DaylightTime),
                typeof(System.Globalization.GlobalizationExtensions),
                typeof(System.Globalization.IdnMapping),
                typeof(System.Globalization.NumberFormatInfo),
                typeof(System.Globalization.RegionInfo),
                typeof(System.Globalization.SortKey),
                typeof(System.Globalization.SortVersion),
                typeof(System.Globalization.StringInfo),
                typeof(System.Globalization.TextElementEnumerator),
                typeof(System.Globalization.TextInfo)
            };

             var calendar =  new System.Globalization.Calendar[]  {
                new System.Globalization.ChineseLunisolarCalendar(),
                new System.Globalization.GregorianCalendar(),
                new System.Globalization.HebrewCalendar(),
                new System.Globalization.HijriCalendar(),
                new System.Globalization.JapaneseCalendar(),
                new System.Globalization.JapaneseLunisolarCalendar(),
                new System.Globalization.JulianCalendar(),
                new System.Globalization.KoreanCalendar(),
                new System.Globalization.KoreanLunisolarCalendar(),
                new System.Globalization.PersianCalendar(),
                new System.Globalization.TaiwanCalendar(),
                new System.Globalization.TaiwanLunisolarCalendar(),
                new System.Globalization.ThaiBuddhistCalendar(),
                new System.Globalization.UmAlQuraCalendar(),
            };


            logLevel = (LogType)PlayerPrefs.GetInt("LogType", (int)logLevel);
            Debug.unityLogger.filterLogType = logLevel;
            mPluginManager = PluginManager.Instance();

            mStartTime = Time.realtimeSinceStartup;
            Debug.Log("boot time "+mStartTime);
            //DontDestroyOnLoad(gameObject);

            if (mIsUsePluginConfig)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                m_obbDownloader = GooglePlayObbDownloadManager.GetGooglePlayObbDownloader();
                m_obbDownloader.PublicKey = mPlulicKey;
                Debug.LogWarning($"dataPath{Application.dataPath}");
                Debug.LogWarning($"obbPath{m_obbDownloader.GetMainOBBPath()}");
                if (GooglePlayObbDownloadManager.IsDownloaderAvailable() && lzip.entryExists(Application.dataPath, "assets/bin/Data/0000000000000000f000000000000000") == false)
                {
                    m_ObbPath = m_obbDownloader.GetMainOBBPath();
                    if (!string.IsNullOrEmpty(m_ObbPath) && File.Exists(m_ObbPath))
                    {
                        long res = lzip.getTotalFiles(m_ObbPath, null);
                        Debug.LogWarning("OBB文件存在" + res);
                        if (res < 0)
                        {
                            Debug.LogWarning("需要获取读卡权限");
                            string[] permission = new string[] { "android.permission.READ_EXTERNAL_STORAGE" };
                            Permissions.requestPermissions(permission, (int requestCode, string[] permissions, int[] grantResults) =>
                            {
                                if(grantResults[0] == 0)
                                {
                                    LoadPluginFromFile();
                                }
                                else
                                {
                                    Permissions.GotoSetting("\"External Storage Read\" is required. Please grant it in \"application information > permission\"");
                                }
                            });
                        }
                        else
                        {
                            LoadPluginFromFile();
                        }
                    }
                    else
                    {
                        bool bCanWrite = true;
                        var temp = m_ObbPath + ".tmp";
                        try
                        {
                            var path = Path.GetDirectoryName(temp);
                            Directory.CreateDirectory(path);
                            File.WriteAllText(temp, "1");
                            File.Delete(temp);
                        }
                        catch(Exception ex)
                        {
                            bCanWrite = false;
                        }

                        if(bCanWrite == false)
                        {
                            Debug.LogWarning("需要获取写入外部文件权限");
                            string[] permission = new string[] { "android.permission.WRITE_EXTERNAL_STORAGE" };
                            Permissions.requestPermissions(permission, (int requestCode, string[] permissions, int[] grantResults) =>
                            {
                                if(grantResults[0] == 0)
                                {
                                    Debug.LogWarning("OBB文件不存在,准备下载" + m_ObbPath);
                                    m_bDownloadObb = true;
                                    m_obbDownloader.FetchOBB();
                                }
                                else
                                {
                                    Permissions.GotoSetting("\"External Storage Write\" is required. Please grant it in \"application information > permission\"");
                                }
                            });
                        }
                        else
                        {
                            Debug.LogWarning("OBB文件不存在,准备下载" + m_ObbPath);
                            m_bDownloadObb = true;
                            m_obbDownloader.FetchOBB();
                        }
                    }
                }
                else
                {
                    LoadPluginFromFile();
                }
#else
                LoadPluginFromFile();
#endif
                return;
            }

            OnAddPlugin();
        }

        private void OnDestroy()
        {
            if (mPluginManager != null)
            {
                mPluginManager.BeforeShut();
                mPluginManager.Shut();
                mPluginManager = null;
                mCanUpdate = false;
                CoreUtils.ClearCore();
            }
        }

        private void LoadPluginFromFile()
        {
            Debug.Log("boot time2 "+mStartTime);
            CoreUtils.LoadFileAsync(Path.Combine(Application.streamingAssetsPath, "Plugin.xml"), (data) =>
            {
                CoreUtils.Decrypt(data);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(Encoding.UTF8.GetString(data));

                var appNode = xmlDocument.DocumentElement.ChildNodes[0];
                var appName = xmlDocument.DocumentElement.GetAttribute("App");

#if UNITY_EDITOR
                var mode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
                if (mode == HotfixMode.ILRT)
                {
                    appName = "ClientApp_ILRT";
                }
                else if (mode == HotfixMode.Reflect)
                {
                    appName = "ClientApp_Reflect";
                }
                else if (mode == HotfixMode.NativeCode)
                {
                    appName = "ClientApp_Native";
                }
                else if (mode == HotfixMode.IFix)
                {
                    appName = "ClientApp_IFix";
                }
#endif

                Debug.Log("App:" + appName);
                foreach (var app in xmlDocument.DocumentElement.ChildNodes)
                {
                    var appElement = app as XmlElement;
                    if (appElement.Name.Equals(appName))
                    {
                        appNode = appElement;
                        break;
                    }
                }
                var dynamicPlugin = new Plugin("DynamicPlugin");
                mPluginManager.Registered(dynamicPlugin);
                foreach (var childNode in appNode.ChildNodes)
                {
                    var nodeElement = childNode as XmlElement;
                    if (nodeElement.Name.Equals("Module"))
                    {
                        var moduleName = nodeElement.GetAttribute("Name");
                        var moduleClass = nodeElement.GetAttribute("Class");
                        var assembly = nodeElement.GetAttribute("Assembly");
#if UNITY_EDITOR
                        if (moduleName.Equals("Skyunion.IDataService"))
                        {
                            var loadMode = EditorPrefs.GetInt("DataService_DataMode", (int)DataMode.Binary);
                            if (loadMode == (int)DataMode.Binary)
                            {
                                moduleClass = "Skyunion.DataServiceBinary";
                            }
                            else if (loadMode == (int)DataMode.SQLite)
                            {
                                moduleClass = "Skyunion.DataServiceSQLite";
                            }
                            else if (loadMode == (int)DataMode.Excel)
                            {
                                moduleClass = "Skyunion.DataServiceExcel";
                            }
                        }
#endif
                        Debug.Log("module:" + moduleClass+" "+Time.realtimeSinceStartup);
                        Type type;
                        if (assembly.Equals(string.Empty))
                        {
                            type = Type.GetType(moduleClass);
                        }
                        else
                        {
                            type = Assembly.Load(assembly).GetType(moduleClass);
                        }

                        IModule module = Activator.CreateInstance(type) as IModule;
                        dynamicPlugin.AddModule(moduleName, module);
                    }
                    else if (nodeElement.Name.Equals("Plugin"))
                    {
                        var pluginName = nodeElement.GetAttribute("Name");
                        var pluginClass = nodeElement.GetAttribute("Class");
                        var assembly = nodeElement.GetAttribute("Assembly");
                        Debug.Log("plugin:" + pluginClass);
                        Type type;
                        if (assembly.Equals(string.Empty))
                        {
                            type = Type.GetType(pluginClass);
                        }
                        else
                        {
                            type = Assembly.Load(assembly).GetType(pluginClass);
                        }
                        IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
                        mPluginManager.Registered(plugin);
                    }
                }
                CoreUtils.assetService.SetOBBPath(m_ObbPath);
                Init();
            });
        }

        private void Init()
        {
            mPluginManager.BeforeInit();
            mPluginManager.Init();
            mPluginManager.WaitInitAsync(() =>
            {
                OnInitialized();
                mPluginManager.AfterInit();
                OnAfterInitialized();
                Debug.Log(string.Format("total: {0} ms", Time.realtimeSinceStartup - mStartTime));
                mCanUpdate = true;
            });
        }

        void Update()
        {
            if (mPluginManager != null)
            {
                mPluginManager.Update();
            }
        }
        void LateUpdate()
        {
            if (mPluginManager != null)
            {
                mPluginManager.LateUpdate();
            }
        }

        protected void OnApplicationFocus(bool focus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(focus && m_bDownloadObb)
            {
                m_bDownloadObb = false;
                long res = lzip.getTotalFiles(m_ObbPath, null);
                Debug.LogWarning("OBB文件存在" + res);
                if (res < 0)
                {
                    Debug.LogWarning("需要获取读卡权限");
                    string[] permission = new string[] { "android.permission.READ_EXTERNAL_STORAGE" };
                    Permissions.requestPermissions(permission, (int requestCode, string[] permissions, int[] grantResults) =>
                    {
                        if(grantResults[0] == 0)
                        {
                            LoadPluginFromFile();
                        }
                        else
                        {
                            Permissions.GotoSetting("\"External Storage Read\" is required. Please grant it in \"application information > permission\"");
                        }
                    });
                }
                else
                {
                    LoadPluginFromFile();
                }
            }
#endif
        }

        protected virtual void OnApplicationQuit()
        {
            if (mPluginManager != null)
            {
                mPluginManager.BeforeShut();
                mPluginManager.Shut();
                mPluginManager = null;
                mCanUpdate = false;
                CoreUtils.ClearCore();
            }
        }
    }
}