//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System;
using System.Reflection;

namespace Skyunion
{
    public class DataServiceEditor : UnityEditor.Editor
    {
        public static string DataDataModeKey = "DataService_DataMode";
        private const string mSelectDataModeBinaryMenuPath = "Skyunion/DataService/二进制模式";
        private const string mSelectDataModeSQLiteMenuPath = "Skyunion/DataService/SQLite模式";
        private const string mSelectDataModeExcelMenuPath = "Skyunion/DataService/Excel模式(第一次比较慢)";

        [MenuItem("Skyunion/DataService/配置/设置配置的目录", priority = 1000)]
        public static void SelectExcelPath_Menu()
        {
            var excelPath = EditorPrefs.GetString("ExcelPath", "H:\\IG\\IG-config");
            excelPath = EditorUtility.OpenFolderPanel("Select Config Folder", excelPath, "");
            EditorPrefs.SetString("ExcelPath", excelPath);
        }

        private static string GetFileMd5(string path)
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
        [MenuItem("Skyunion/DataService/配置/同步代码", priority = 1000)]
        public static void SyncCodePath_Menu()
        {
            var excelPath = EditorPrefs.GetString("ExcelPath", "H:\\IG\\IG-config");
            if (!File.Exists(Path.Combine(excelPath, ".gitlab-ci.yml")))
            {
                Debug.LogError("not ig-config folder!");
                return;
            }

            var csPath = Path.Combine(excelPath, "gen/", "GameConfig");
            List<string> withoutExtensions = new List<string>() { ".cs" };
            string[] files = Directory.GetFiles(csPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            var csToPath = Path.Combine(Application.dataPath, "Scripts\\Hotfix\\Config");
            foreach (var path in files)
            {
                FileInfo fi = new FileInfo(path);
                var fileName = fi.Name;
                var newPath = Path.Combine(csToPath, fileName);

                if (!GetFileMd5(path).Equals(GetFileMd5(newPath)))
                {
                    Debug.Log($"同步代码：{fileName}");
                    File.Copy(path, newPath, true);
                }
            }
            Debug.Log("同步代码完成");
        }
        [MenuItem("Skyunion/DataService/配置/强制同步配置", priority = 1000)]
        public static void SyncConfigPath_Menu()
        {
            var excelPath = EditorPrefs.GetString("ExcelPath", "H:\\IG\\IG-config");
            if (!File.Exists(Path.Combine(excelPath, ".gitlab-ci.yml")))
            {
                Debug.LogError("not ig-config folder!");
                return;
            }

            var csPath = Path.Combine(excelPath, "Cache", "Bin");
            List<string> withoutExtensions = new List<string>() { ".bin" };
            string[] files = Directory.GetFiles(csPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            var csToPath = Path.Combine(Application.dataPath, "StreamingAssets\\Config\\Bin");
            foreach (var path in files)
            {
                FileInfo fi = new FileInfo(path);
                var fileName = fi.Name;
                var newPath = Path.Combine(csToPath, fileName);

                if (!GetFileMd5(path).Equals(GetFileMd5(newPath)))
                {
                    Debug.Log($"同步配置：{fileName}");
                    File.Copy(path, newPath, true);
                }
            }
            Debug.Log("同步配置完成");
        }
        [MenuItem("Skyunion/DataService/配置/拷贝性能测试代码", priority = 1000)]
        public static void GeneratePefmanceCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat($"float startTime1 = Time.realtimeSinceStartup;\n");
            foreach (var item in Assembly.Load("Hotfix").GetTypes())
            {
                if (item.IsClass && item.FullName.Length > 5 && item.FullName.Substring(0, 5).Equals("Data."))
                {
                    sb.AppendFormat($"CoreUtils.dataService.QueryTable<{item.FullName}>();\n");
                }
            }
            sb.Append("CoreUtils.logService.Info($\"LoadAllTable:{Time.realtimeSinceStartup - startTime1}\", Color.green);\n");

            sb.AppendFormat($"float startTime2 = Time.realtimeSinceStartup;\n");
            foreach (var item in Assembly.Load("Hotfix").GetTypes())
            {
                if (item.IsClass && item.FullName.Length > 5 && item.FullName.Substring(0, 5).Equals("Data."))
                {
                    sb.AppendFormat($"CoreUtils.dataService.QueryRecords<{item.FullName}>();\n");
                }
            }
            sb.Append("CoreUtils.logService.Info($\"ConvertAllTable:{Time.realtimeSinceStartup - startTime2}\", Color.green);\n");
            TextEditor te = new TextEditor();
            te.text = sb.ToString(); ;
            te.SelectAll();
            te.Copy();
            Debug.Log("已经赋值到粘贴板");
        }

        [MenuItem(mSelectDataModeBinaryMenuPath, priority = 1000)]
        public static void SelectBinaryMode_Menu()
        {
            EditorPrefs.SetInt(DataDataModeKey, (int)DataMode.Binary);
            Menu.SetChecked(mSelectDataModeBinaryMenuPath, true);
            Menu.SetChecked(mSelectDataModeSQLiteMenuPath, false);
            Menu.SetChecked(mSelectDataModeSQLiteMenuPath, false);
        }

        [MenuItem(mSelectDataModeBinaryMenuPath, true)]
        public static bool SelectBinaryMode_Check_Menu()
        {
            Menu.SetChecked(mSelectDataModeBinaryMenuPath,
                EditorPrefs.GetInt(DataDataModeKey, (int)DataMode.SQLite) == (int)DataMode.Binary);
            return true;
        }

        [MenuItem(mSelectDataModeSQLiteMenuPath, priority = 1000)]
        public static void SelectSQLiteMode_Menu()
        {
            EditorPrefs.SetInt(DataDataModeKey, (int)DataMode.SQLite);
            Menu.SetChecked(mSelectDataModeBinaryMenuPath, false);
            Menu.SetChecked(mSelectDataModeSQLiteMenuPath, true);
            Menu.SetChecked(mSelectDataModeExcelMenuPath, false);
        }

        [MenuItem(mSelectDataModeSQLiteMenuPath, true)]
        public static bool SelectSQLiteMode_Check_Menu()
        {
            Menu.SetChecked(mSelectDataModeSQLiteMenuPath,
                EditorPrefs.GetInt(DataDataModeKey, (int)DataMode.SQLite) == (int)DataMode.SQLite);
            return true;
        }

        [MenuItem(mSelectDataModeExcelMenuPath, priority = 1000)]
        public static void SelectExcelMode_Menu()
        {
            EditorPrefs.SetInt(DataDataModeKey, (int)DataMode.Excel);
            Menu.SetChecked(mSelectDataModeBinaryMenuPath, false);
            Menu.SetChecked(mSelectDataModeSQLiteMenuPath, false);
            Menu.SetChecked(mSelectDataModeExcelMenuPath, true);
        }

        [MenuItem(mSelectDataModeExcelMenuPath, true)]
        public static bool SelectExcelMode_Check_Menu()
        {
            Menu.SetChecked(mSelectDataModeExcelMenuPath,
                EditorPrefs.GetInt(DataDataModeKey, (int)DataMode.SQLite) == (int)DataMode.Excel);
            return true;
        }


        #region DataSys

        private static string configDir;


        [MenuItem("Tools/同步配置文件")]
        static void SycConfigs()
        {
            bool type = UnityEditor.EditorUtility.DisplayDialog("同步配置", "确认执行该操作吗", "确认", "取消");
            if (!type)
            {
                return;
            }

            configDir = EditorPrefs.GetString("ExcelPath", "H:\\IG\\IG-config");
            if (string.IsNullOrEmpty(configDir))
            {
                SelectExcelPath_Menu();
                return;
            }

            syscConfig(configDir);
        }

        public static void syscConfig(string configRootDir, bool buildDll = true)
        {
            string bytesSourceDir = configRootDir + "/Data/";
            string csSourceDir = configRootDir + "/gen/GameConfig/";

            if (!Directory.Exists(bytesSourceDir))
            {
                Debug.LogFormat("源配置文件目录不存在{0}", bytesSourceDir);
            }

            if (!Directory.Exists(csSourceDir))
            {
                Debug.LogFormat("源CS文件目录不存在{0}", csSourceDir);
            }


            string bytesTargetDir = string.Format("{0}/StreamingAssets/", Application.dataPath);
            Debug.LogFormat("copy configs {0} to {1}", bytesSourceDir, bytesTargetDir);
            string[] configFiles = { bytesSourceDir + "/Config.bytes" }; //Directory.GetFiles (bytesSourceDir, "*.bytes");


            foreach (var config in configFiles)
            {
                string target = Path.Combine(bytesTargetDir, Path.GetFileName(config));
                File.Copy(config, target, true);
            }

            string csTargetDir = Application.dataPath + "/Scripts/Hotfix/Config";
            Debug.LogFormat("copy cs {0} to {1}", csSourceDir, csTargetDir);
            string[] csFiles = Directory.GetFiles(csSourceDir, "*.cs");

            foreach (var cs in csFiles)
            {
                string target = Path.Combine(csTargetDir, Path.GetFileName(cs));


                //主工程
                if (File.Exists(target))
                {
                    File.Delete(target);
                }

                File.Copy(cs, target,
                    true);

            }


            AssetDatabase.Refresh();
        }


        //[MenuItem("Tools/检测资源分组")]
        //public static void CheckAAS()
        //{
        //    // 直接获取配置 没有的话就创建一个
        //    var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        //    // 设置资源的版本号   目前资源版本号要跟着 游戏版本号走
        //    setting.OverridePlayerVersion = PlayerSettings.bundleVersion;

        //    // 下面扫描整个工程的文件， 根据AddressableImportSettings自动
        //    HashSet<string> filesToImport = new HashSet<string>();
        //    var filesToAdd = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories);
        //    int prevLenght = System.Environment.CurrentDirectory.Length;
        //    foreach (var file in filesToAdd)
        //    {
        //        // If Directory.GetFiles accepted Regular Expressions, we could filter the metas before iterating.
        //        if (!file.EndsWith(".meta") && !file.EndsWith(".DS_Store"))
        //        {
        //            filesToImport.Add(file.Replace('\\', '/').Substring(prevLenght + 1));
        //        }
        //    }

        //    var importerType = typeof(AddressableImporter);
        //    var posFun = importerType.GetMethod("OnPostprocessAllAssets", System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        //    posFun.Invoke(null, new object[] { filesToImport.ToArray(), null, null, null });
        //}

        [MenuItem("Tools/初始化资源组")]
        public static void ReInitRes()
        {
            RepairRes();
            //CheckAAS();
            //return;

            var importer = Application.dataPath + "/AddressableAssetsData/AddressableImportSettings.asset";
            var importerTemp = System.Environment.CurrentDirectory + "/AddressableImportSettings.asset";
            if (File.Exists(importer))
            {
                if (File.Exists(importerTemp))
                {
                    File.Delete(importerTemp);
                }
                File.Move(importer, importerTemp);
                AssetDatabase.Refresh();
            }
            //string dir = Application.dataPath + "/AddressableAssetsData";
            //if (Directory.Exists(dir))
            //{
            //    Directory.Delete(dir,true);
            //}
            //try
            //{
            //    Directory.CreateDirectory(dir);
            //}
            //catch(Exception e)
            //{

            //}
            AssetDatabase.Refresh();
            AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(AddressableAssetSettingsDefaultObject.kDefaultConfigFolder, AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
            File.Move(importerTemp, importer);
            AssetDatabase.Refresh();
            HashSet<string> filesToImport = new HashSet<string>();
            var filesToAdd = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories);
            int prevLenght = System.Environment.CurrentDirectory.Length;
            foreach (var file in filesToAdd)
            {
                // If Directory.GetFiles accepted Regular Expressions, we could filter the metas before iterating.
                if (!file.EndsWith(".meta") && !file.EndsWith(".DS_Store"))
                {
                    filesToImport.Add(file.Replace('\\', '/').Substring(prevLenght + 1));
                }
            }

            var importerType = typeof(AddressableImporter);
            var posFun = importerType.GetMethod("OnPostprocessAllAssets", System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            posFun.Invoke(null, new object[] { filesToImport.ToArray(), new string[] { }, new string[] { }, new string[] { } });
        }
        //[MenuItem("Tools/清空资源分组")]
        public static void RepairRes()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogWarningFormat("[Addressables] settings file not found.\nPlease go to Menu/Window/Asset Management/Addressables/Groups, then click 'Create Addressables Settings' button.");
                return;
            }


            var entriesRemoved = new List<AddressableAssetEntry>();
            foreach(var group in  settings.groups)
            {
                if (group.ReadOnly)
                    continue;
                Debug.Log("RepairRes:"+group.name);
                var entries = group.entries.ToArray();
                foreach (var entri in entries)
                {
                    group.RemoveAssetEntry(entri);
                }
            }
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/刷新所有TP")]
        public static void RefreshTP()
        {
            HashSet<string> filesToImport = new HashSet<string>();
            var filesToAdd = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories);
            int prevLenght = System.Environment.CurrentDirectory.Length;
            foreach (var file in filesToAdd)
            {
                // If Directory.GetFiles accepted Regular Expressions, we could filter the metas before iterating.
                if (!file.EndsWith(".meta") && !file.EndsWith(".DS_Store"))
                {
                    filesToImport.Add(file.Replace('\\', '/').Substring(prevLenght + 1));
                }
            }

            var importerType = typeof(TPImporter.TexturePackerImporter);
            var posFun = importerType.GetMethod("OnPostprocessAllAssets", System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            posFun.Invoke(null, new object[] { filesToImport.ToArray(), new string[] { }, new string[] { }, new string[] { } });
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
        
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #endregion
    }
}