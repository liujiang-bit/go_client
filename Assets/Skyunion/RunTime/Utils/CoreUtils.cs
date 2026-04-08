using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using ILRuntime.Reflection;
using Skyunion;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Skyunion
{
    public static class CoreUtils
    {
        #region 服务
        static IADService _adService = null;
        public static IADService adService
        {
            get
            {
                return _adService ?? (_adService = PluginManager.Instance().FindModule<IADService>());
            }
            set
            {
                _adService = value;
            }
        }
        static ILogService _logService = null;
        public static ILogService logService
        {
            get
            {
                return _logService ?? (_logService = PluginManager.Instance().FindModule<ILogService>());
            }
            set
            {
                _logService = value;
            }
        }
        static IAssetService _assetService;
        public static IAssetService assetService
        {
            get
            {
                return _assetService ?? (_assetService = PluginManager.Instance().FindModule<IAssetService>());
            }
            set
            {
                _assetService = value;
            }
        }
        static INetServcice _netService;
        public static INetServcice netService
        {
            get
            {
                return _netService ?? (_netService = PluginManager.Instance().FindModule<INetServcice>());
            }
            set
            {
                _netService = value;
            }
        }
        static IHotFixService _hotService;
        public static IHotFixService hotService
        {
            get
            {
                return _hotService ?? (_hotService = PluginManager.Instance().FindModule<IHotFixService>());
            }
            set
            {
                _hotService = value;
            }
        }
        static IDataService _dataService;
        public static IDataService dataService
        {
            get
            {
                return _dataService ?? (_dataService = PluginManager.Instance().FindModule<IDataService>());
            }
            set
            {
                _dataService = value;
            }
        }
        static IAudioService _audioService;
        public static IAudioService audioService
        {
            get
            {
                return _audioService ?? (_audioService = PluginManager.Instance().FindModule<IAudioService>());
            }
            set
            {
                _audioService = value;
            }
        }
        #endregion
        #region 管理器
        static IInputManager _inputManager;
        public static IInputManager inputManager
        {
            get
            {
                return _inputManager ?? (_inputManager = PluginManager.Instance().FindModule<IInputManager>());
            }
            set
            {
                _inputManager = value;
            }
        }
        static IUIManager _uiManager;
        public static IUIManager uiManager
        {
            get
            {
                return _uiManager ?? (_uiManager = PluginManager.Instance().FindModule<IUIManager>());
            }
            set
            {
                _uiManager = value;
            }
        }
        #endregion
        public static void ClearCore()
        {
            _uiManager = null;
            _inputManager = null;
            _audioService = null;
            _dataService = null;
            _hotService = null;
            _netService = null;
            _logService = null;
            _assetService = null;
            _adService = null;
        }

        public enum GraphicLevel
        {
            NONE,
            LOW,
            MEDIUM,
            HIGH
        }
        private static GraphicLevel m_graphic_level = GraphicLevel.HIGH;
        public static void SetGraphicLevel(int level)
        {
            m_graphic_level = (GraphicLevel)level;
        }

        public static GraphicLevel GetGraphicLevel()
        {
            return m_graphic_level;
        }

        // 此接口路径需要完整路径
        internal static void LoadFileAsync(string path, Action<byte[]> completed)
        {
#if !UNITY_EDITOR
            if (VersionUtil.HasHotfix)
            {
                // 需要判断 只有 包内的资源才走热更新
                int nIndex = path.IndexOf(Application.streamingAssetsPath);
                if (nIndex != -1)
                {
                    var hotfixPath = Path.Combine(VersionUtil.HotfixRuntimePath, path.Substring(Application.streamingAssetsPath.Length+1));
                    if (File.Exists(hotfixPath))
                    {
                        path = hotfixPath;
                        path = path.Insert(0, "file://");
                    }
                }
            }
#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IPHONE
            path = path.Insert(0, "file://");
#endif

            UnityWebRequest webFile = UnityWebRequest.Get(path);
            Debug.Log($"LoadFileAsync:{path}");
            webFile.SendWebRequest().completed += (AsyncOperation op) =>
            {
                completed?.Invoke(webFile.downloadHandler.data);
            };
        }

        public static Task unZipFileAsync(string zipFile, string fileDir, Action onError = null, Action<long> onTotalFileCount = null, Action<long, long, string> onProgress = null)
        {
            return Task.Run(() =>
            {
                // 先计算总文件数
                int nTotalFileCount = 0;
                try
                {
                    ZipInputStream s = new ZipInputStream(File.OpenRead(zipFile.Trim()));
                    ZipEntry theEntry;
                    string path = fileDir;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (fileName != String.Empty)
                        {
                            nTotalFileCount++;
                        }
                    }
                    s.Close();
                    onTotalFileCount?.Invoke(nTotalFileCount);
                }
                catch (Exception ex)
                {
                    onError?.Invoke();
                    return;
                }

                try
                {
                    //读取压缩文件(zip文件),准备解压缩
                    ZipInputStream s = new ZipInputStream(File.OpenRead(zipFile.Trim()));
                    ZipEntry theEntry;
                    string path = fileDir;
                    //解压出来的文件保存的路径

                    int nFileCount = 0;
                    string rootDir = " ";
                    //根目录下的第一个子文件夹的名称
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        rootDir = Path.GetDirectoryName(theEntry.Name);
                        //得到根目录下的第一级子文件夹的名称
                        if (rootDir.IndexOf("\\") >= 0)
                        {
                            rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                        }
                        string dir = Path.GetDirectoryName(theEntry.Name);
                        //根目录下的第一级子文件夹的下的文件夹的名称
                        string fileName = Path.GetFileName(theEntry.Name);
                        //根目录下的文件名称
                        if (dir != " ")
                        //创建根目录下的子文件夹,不限制级别
                        {
                            if (!Directory.Exists(fileDir + "\\" + dir))
                            {
                                path = Path.Combine(fileDir, dir);
                                //在指定的路径创建文件夹
                                Directory.CreateDirectory(path);
                            }
                        }
                        else if (dir == " " && fileName != "")
                        //根目录下的文件
                        {
                            path = fileDir;
                        }
                        else if (dir != " " && fileName != "")
                        //根目录下的第一级子文件夹下的文件
                        {
                            if (dir.IndexOf("\\") > 0)
                            //指定文件保存的路径
                            {
                                path = Path.Combine(fileDir, dir);
                            }
                        }

                        if (dir == rootDir)
                        //判断是不是需要保存在根目录下的文件
                        {
                            path = Path.Combine(fileDir, rootDir);
                        }

                        //以下为解压缩zip文件的基本步骤
                        //基本思路就是遍历压缩文件里的所有文件,创建一个相同的文件。
                        if (fileName != String.Empty)
                        {
                            var filePath = Path.Combine(path, fileName);
                            FileStream streamWriter = File.Create(filePath);

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            streamWriter.Close();
                            nFileCount++;
                            onProgress?.Invoke(nFileCount, nTotalFileCount, filePath);

                        }
                    }
                    s.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    onError?.Invoke();
                }
            });
        }
        public static string unZipFile(string TargetFile, string fileDir)
        {
            string rootFile = " ";
            try
            {
                //读取压缩文件(zip文件),准备解压缩
                ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
                ZipEntry theEntry;
                string path = fileDir;
                //解压出来的文件保存的路径

                string rootDir = " ";
                //根目录下的第一个子文件夹的名称
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    rootDir = Path.GetDirectoryName(theEntry.Name);
                    //得到根目录下的第一级子文件夹的名称
                    if (rootDir.IndexOf("\\") >= 0)
                    {
                        rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                    }
                    string dir = Path.GetDirectoryName(theEntry.Name);
                    //根目录下的第一级子文件夹的下的文件夹的名称
                    string fileName = Path.GetFileName(theEntry.Name);
                    //根目录下的文件名称
                    if (dir != " ")
                    //创建根目录下的子文件夹,不限制级别
                    {
                        if (!Directory.Exists(fileDir + "\\" + dir))
                        {
                            path = Path.Combine(fileDir, dir);
                            //在指定的路径创建文件夹
                            Directory.CreateDirectory(path);
                        }
                    }
                    else if (dir == " " && fileName != "")
                    //根目录下的文件
                    {
                        path = fileDir;
                        rootFile = fileName;
                    }
                    else if (dir != " " && fileName != "")
                    //根目录下的第一级子文件夹下的文件
                    {
                        if (dir.IndexOf("\\") > 0)
                        //指定文件保存的路径
                        {
                            path = Path.Combine(fileDir, dir);
                        }
                    }

                    if (dir == rootDir)
                    //判断是不是需要保存在根目录下的文件
                    {
                        path = Path.Combine(fileDir, rootDir);
                    }

                    //以下为解压缩zip文件的基本步骤
                    //基本思路就是遍历压缩文件里的所有文件,创建一个相同的文件。
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(Path.Combine(path, fileName));

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();

                return rootFile;
            }
            catch (Exception ex)
            {
                return "1; " + ex.Message;
            }
        }
        public static void RestarGame()
        {
            Timer.CancelAllRegisteredTimers();
            SceneManager.LoadScene("GameApp", LoadSceneMode.Single);
        }

        // 刘海屏相关
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string _getSafeAreaInsets();
#endif
        public static string getSafeAreaInsets()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return _getSafeAreaInsets();
#elif UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var jcBangUtils = new AndroidJavaClass("com.unity.androidplugin.BangUtils");
                var jc_UnityDefault = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var jo_UnityActivity = jc_UnityDefault.GetStatic<AndroidJavaObject>("currentActivity");
                return jcBangUtils.CallStatic<string>("getSafeAreaInsets", jo_UnityActivity);
            }
            catch (Exception ex)
            {
                return "0, 0, 0, 0, 1";
            }
#else
            if (Screen.width == 2436 && Screen.height == 1125)
            {
                return "88, 88, 0, 42, 1";
            }
            return "0, 0, 0, 0, 1";
#endif
        }

        public static Vector4 getSafeAreaInset()
        {
            string strRet = getSafeAreaInsets();
            var values = strRet.Split(',');
            if (values.Length == 5)
            {
                return new Vector4(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), Convert.ToInt32(values[3]));
            }
            return Vector4.zero;
        }
        private static float m_screenScale = 1.0f;
        public static float getScreenScale()
        {
            return m_screenScale;
        }
        public static void setScreenScale(float scale)
        {
            m_screenScale = scale;
        }


        private static byte[] chEncpty = new byte[256]
        {
            0x85, 2, 0xE, 0x9C, 0xA7, 0xCB, 0xC7, 0x2F, 2, 0x75,
            0x8A, 0x2A, 0xB2, 0x2C, 0xE3, 0xB9, 0x43, 0xB6, 0x68,
            0x17, 0x48, 0xD3, 7, 0x2A, 0xC7, 0xF3, 0x28, 0x23,
            0x4B, 0x4F, 0xC2, 0xAF, 0xC7, 8, 0xF8, 0xFC, 7, 4,
            0x94, 0x5E, 0x42, 0xBA, 0x92, 0x97, 0x5F, 0xC, 0xF2,
            0x3F, 0x40, 0x77, 0x94, 0xFC, 0x4A, 0x6C, 0xB6, 0xE1,
            0xB2, 0xE4, 0x4B, 0x16, 0x37, 0xC0, 0x9B, 0x73, 0xE8,
            0x67, 0x4E, 0x6C, 0xCA, 0x3C, 0xE, 0x43, 0x94, 0xE6,
            0xE3, 0xB1, 0x6A, 0x3A, 0xE4, 0x36, 0xAC, 0x36, 0x39,
            0xE2, 0x49, 0x61, 0x32, 0x70, 0xD1, 0xF2, 0x8A, 0xBC,
            0x54, 0x9C, 0xDC, 0xE4, 0xAB, 0x3A, 0x91, 0x58, 0x47,
            0xFE, 0xC5, 0x22, 0x31, 0xC1, 0x41, 0xB1, 0x97, 0x55,
            0xCB, 0x2D, 0xC6, 0xCF, 0x3B, 0x2D, 0xC, 0x26, 0x1E,
            0x7F, 0x57, 0xC6, 0x38, 0xE, 9, 0x6D, 0x8E, 0x67, 0xC6,
            0xE, 0x46, 0x53, 0x1D, 0x31, 0xE0, 4, 0xD, 0x2C, 0xF,
            0xCC, 0xBD, 0xFE, 0xE0, 0x56, 0xC7, 0xAF, 0x6B, 0xE,
            0xA5, 0xAB, 0x2F, 0x8C, 8, 0xA8, 0xDA, 0x45, 2, 0x8E,
            0x5D, 0x4D, 0xFC, 9, 0xAB, 0x27, 0x8E, 0x5C, 0xE8,
            0x7E, 0x17, 0xAC, 0x5E, 0x11, 0xCA, 0x2C, 0xE6, 0x19,
            0x41, 0x7B, 0x77, 0x9F, 0x52, 0xEC, 0x53, 0x83, 0x54,
            0xE5, 0xA6, 0x8F, 0x9F, 0x81, 0xBC, 0x31, 0xF3, 0x85,
            0x25, 0x49, 0x60, 0x26, 0x5F, 0xEF, 0xE7, 0xC6, 0x57,
            0xF6, 0x4C, 0xBA, 0x7C, 0xD0, 0x6F, 0xB9, 0xEC, 0x87,
            0xE1, 0x9E, 0x91, 0x74, 0xA2, 0x8D, 0xB4, 0x53, 0x82,
            0x12, 0x1D, 0xAB, 0x47, 0x1A, 0xE7, 0x1E, 0xDE, 0x39,
            0xCD, 0x84, 0xAB, 0xD8, 0x9B, 0xEC, 0x32, 0xC3, 0x6E,
            0xE9, 0x13, 0x84, 0xD2, 2, 0x36, 0xDF, 0xA1, 0x95,
            0xC1, 0xEB, 0x13, 6, 0x9F, 0x4D, 0x37, 0xA0,
        };

        public static void Encrypt(byte [] data)
        {
            for (int j = 0; j < data.Length; j++)
            {
                data[j] ^= chEncpty[j % 256];
            }
        }

        public static void Decrypt(byte[] data)
        {
            for (int j = 0; j < data.Length; j++)
            {
                data[j] ^= chEncpty[j % 256];
            }
        }
        public static void EncryptFile(string path)
        {
            var data = File.ReadAllBytes(path);
            Encrypt(data);
            File.WriteAllBytes(path, data);
        }

        public static string GetFileMd5(string path)
        {
            if (!File.Exists(path))
                return "";
            var content = File.ReadAllBytes(path);
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(content);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var md5 = sBuilder.ToString();

            return md5;
        }
    }
}
