//
// AssetsMenuItem.cs
//
// Author:
//       fjy <jiyuan.feng@live.com>
//
// Copyright (c) 2019 fjy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Skyunion
{
    public static class BuildMenuItem
    {
        [MenuItem("Skyunion/Build/BuildContent")]
        private static void BuildContent()
        {
            //LogAssert.ignoreFailingMessages = true;
            //AddressableAssetSettings oldSettings = AddressableAssetSettingsDefaultObject.Settings;
            //AddressableAssetSettingsDefaultObject.Settings = Settings;

            //bool callbackCalled = false;
            //BuildScript.buildCompleted += (result) =>
            //{
            //    callbackCalled = true;
            //};
            //AddressableAssetSettings.BuildPlayerContent();
            //Assert.IsTrue(callbackCalled);

            //if (oldSettings != null)
            //    AddressableAssetSettingsDefaultObject.Settings = oldSettings;
            //AddressableAssetSettings.BuildPlayerContent();
            //UnityEngine.TestTools.LogAssert.ignoreFailingMessages = false;

            //BuildScript.BuildStandalonePlayer();

            //BuildCache.PurgeCache(true);
            //AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem("Skyunion/Build/BuildPlayer")]
        private static void BuildStandalonePlayer()
        {
            var levels = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }
            var targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;
            var outputPath = Application.dataPath + "/../../Packages/";
            Debug.Log("BuildTarget:"+EditorUserBuildSettings.activeBuildTarget);
            if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                outputPath += "Android/";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                outputPath += "Windows/";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX)
            {
                outputPath += "OSX/";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                outputPath += "iOS/";
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = levels,
                locationPathName = outputPath + targetName,
                target = EditorUserBuildSettings.activeBuildTarget,
                options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        [UnityEditor.Callbacks.PostProcessScene]
        public static void PostProcessScene()
        {
            var pluginSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Assets/Plugin.xml");
            var pluginTargetPath = Path.Combine(Application.streamingAssetsPath, "Plugin.xml");
            File.Copy(pluginSourcePath, pluginTargetPath, true);
            var logSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Assets/log4net.xml");
            var logTargetPath = Path.Combine(Application.streamingAssetsPath, "log4net.xml");
            File.Copy(logSourcePath, logTargetPath, true);

            var hotfixDllTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.dll.bytes");
            var hotfixPdbTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.pdb.bytes");
            var mode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
            var hotfixPatchTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.patch.bytes");
            if (File.Exists(hotfixDllTargetPath))
            {
                File.Delete(hotfixDllTargetPath);
            }
            if (File.Exists(hotfixPdbTargetPath))
            {
                File.Delete(hotfixPdbTargetPath);
            }
            if (File.Exists(hotfixPatchTargetPath))
            {
                File.Delete(hotfixPatchTargetPath);
            }
            if (BuildPipeline.isBuildingPlayer)
            {
                var pluginPath = Path.Combine(Application.streamingAssetsPath, "Plugin.xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(pluginPath);
                if (mode == HotfixMode.ILRT)
                {
                    xmlDocument.DocumentElement.SetAttribute("App", "ClientApp_ILRT");
                }
                else if (mode == HotfixMode.NativeCode)
                {
                    xmlDocument.DocumentElement.SetAttribute("App", "ClientApp_Native");
                }
                else if (mode == HotfixMode.Reflect)
                {
                    xmlDocument.DocumentElement.SetAttribute("App", "ClientApp_Reflect");
                }
                else if (mode == HotfixMode.IFix)
                {
                    xmlDocument.DocumentElement.SetAttribute("App", "ClientApp_IFix");
                    IFix.Editor.IFixEditor.PatchNotProgressBar();
                    //if (File.Exists(hotfixPatchTargetPath))
                    //{
                    //    CoreUtils.EncryptFile(hotfixDllTargetPath);
                    //}
                    IFix.Editor.IFixEditor.InjectAllAssemblys();
                }
                xmlDocument.Save(pluginPath);
            }
            else
            {
                if (mode == HotfixMode.ILRT)
                {
                    UnityScripsCompiling.OnUnityScripsCompilingCompleted();
                }
            }
            CoreUtils.EncryptFile(pluginTargetPath);
            CoreUtils.EncryptFile(logTargetPath);
        }
        
        [MenuItem("Skyunion/Build/一键打包")]
        private static void BuildPackage()
        {
            BuildContent();
            BuildStandalonePlayer();
        }

        [MenuItem("Skyunion/Build/自动打包")]
        private static void BuildPackageByConfig()
        {
            var path = Path.Combine(System.Environment.CurrentDirectory, "BuildConfig");
            var dic = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        var li = line.Split(':'); //将一行用,分开成键值对
                        if (li.Length == 2)
                        {
                            dic.Add(li[0], li[1]);
                        }
                    }
                }
                sr.Close();
            }
            if (dic.ContainsKey("BuildTarget"))
            {
                var value = (BuildTarget)int.Parse(dic["BuildTarget"]);
                if (EditorUserBuildSettings.activeBuildTarget != value)
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(value);
                }
            }
            if (dic.ContainsKey("VersionName"))
            {
                PlayerSettings.bundleVersion = dic["VersionName"];
            }

            if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                PlayerSettings.iOS.appleEnableAutomaticSigning = false;

                if (dic.ContainsKey("TeamID"))
                {
                    PlayerSettings.iOS.appleDeveloperTeamID = dic["TeamID"];
                }
                if (dic.ContainsKey("ProfileID"))
                {
                    PlayerSettings.iOS.iOSManualProvisioningProfileID = dic["ProfileID"];
                }
                if (dic.ContainsKey("ProfileType"))
                {
                    PlayerSettings.iOS.iOSManualProvisioningProfileType = (ProvisioningProfileType)int.Parse(dic["ProfileType"]);
                }
                PlayerSettings.iOS.buildNumber = GetVersionCode().ToString();
                if (dic.ContainsKey("BuildNumber"))
                {
                    var code = dic["BuildNumber"];
                    if (code.Length > 0 && code.Equals("0"))
                    {
                        PlayerSettings.iOS.buildNumber = code;
                    }
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = GetVersionCode();
                if (dic.ContainsKey("VersionCode") && dic["VersionCode"].Length > 0)
                {
                    var code = dic["BuildNumber"];
                    if (code.Length > 0 && code.Equals("0"))
                    {
                        PlayerSettings.Android.bundleVersionCode = int.Parse(code);
                    }
                }
                if (dic.ContainsKey("Obb"))
                {
                    PlayerSettings.Android.useAPKExpansionFiles = bool.Parse(dic["Obb"]);
                }
                if (dic.ContainsKey("KeyaliasName"))
                {
                    PlayerSettings.Android.keyaliasName = dic["KeyaliasName"];
                }
                if (dic.ContainsKey("KeyaliasPass"))
                {
                    PlayerSettings.Android.keystorePass = dic["KeyaliasPass"];
                    PlayerSettings.Android.keyaliasPass = dic["KeyaliasPass"];
                }
                if (dic.ContainsKey("ARMv7"))
                {
                    if (bool.Parse(dic["ARMv7"]))
                    {
                        PlayerSettings.Android.targetArchitectures |= AndroidArchitecture.ARMv7;
                    }
                    else
                    {
                        PlayerSettings.Android.targetArchitectures &= ~AndroidArchitecture.ARMv7;
                    }
                }
                if (dic.ContainsKey("ARM64"))
                {
                    if (bool.Parse(dic["ARM64"]))
                    {
                        PlayerSettings.Android.targetArchitectures |= AndroidArchitecture.ARM64;
                    }
                    else
                    {
                        PlayerSettings.Android.targetArchitectures &= ~AndroidArchitecture.ARM64;
                    }
                }
                
                if (dic.ContainsKey("targetSdkVersion"))
                {
                    var code = dic["targetSdkVersion"];
                    PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)int.Parse(code);
                }
                if (dic.ContainsKey("Symbol"))
                {
                    if (bool.Parse(dic["Symbol"]))
                    {
                        EditorUserBuildSettings.androidCreateSymbolsZip = true;
                    }
                    else
                    {
                        EditorUserBuildSettings.androidCreateSymbolsZip = false;
                    }
                }
            }

            if (dic.ContainsKey("Il2CPP"))
            {
                if (bool.Parse(dic["Il2CPP"]))
                {
                    PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, ScriptingImplementation.IL2CPP);
                }
                else
                {
                    PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, ScriptingImplementation.Mono2x);
                }
            }
            bool bBuildBundle = true;
            if (dic.ContainsKey("BuildBundle"))
            {
                bBuildBundle = bool.Parse(dic["BuildBundle"]);
            }

            bool bResetBundleGroup = true;
            if (dic.ContainsKey("ResetBundleGroup"))
            {
                bResetBundleGroup = bool.Parse(dic["ResetBundleGroup"]);
            }

            HotfixMode lastMode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
            bool bRestMode = false;
            if (dic.ContainsKey("HotFixMode"))
            {
                HotfixMode mode;
                if (Enum.TryParse<HotfixMode>(dic["HotFixMode"], out mode))
                {
                    EditorPrefs.SetInt("HofixService_HofixMode", (int)mode);
                    bRestMode = true;
                }
            }

            PlayerSettings.SplashScreen.showUnityLogo = false;
            if (dic.ContainsKey("LogoTime"))
            {
                var strTimes = dic["LogoTime"].Split('|');
                int count = PlayerSettings.SplashScreen.logos.Length;
                if (count > 0)
                {
                    if (strTimes.Length == 1)
                    {
                        float time = float.Parse(strTimes[0]);
                        var perfTime = time / count;
                        var logos = PlayerSettings.SplashScreen.logos;
                        for (int i = 0; i < count; i++)
                        {
                            logos[i].duration = perfTime;
                        }
                        PlayerSettings.SplashScreen.logos = logos;

                    }
                    else if (strTimes.Length > 1)
                    {
                        var logos = PlayerSettings.SplashScreen.logos;
                        for (int i = 0; i < count; i++)
                        {
                            if (i < strTimes.Length)
                            {
                                logos[i].duration = float.Parse(strTimes[i]);
                            }
                            else
                            {
                                logos[i].duration = logos[strTimes.Length].duration;
                            }
                        }
                        PlayerSettings.SplashScreen.logos = logos;
                    }
                }
            }

            try
            {
                if (bResetBundleGroup)
                {
                    DataServiceEditor.ReInitRes();
                }
                if (bBuildBundle)
                {
                    BuildContent();
                }

                BuildStandalonePlayer();
            }
            finally
            {
                if (bRestMode)
                {
                    EditorPrefs.SetInt("HofixService_HofixMode", (int)lastMode);
                }
            }
        }

        private static Dictionary<string, string> get_files_md5(string dir)
        {
            Dictionary<string, string> dicMd5 = new Dictionary<string, string>();
            var asset_dir = Path.Combine(dir, "assets");
            string[] old_files = Directory.GetFiles(asset_dir, "*.*", SearchOption.AllDirectories);
            var md5_file = Path.Combine(dir, "md5.xml");
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(md5_file))
            {
                var root = doc.CreateElement("Files");
                doc.AppendChild(root);
                foreach (var path in old_files)
                {
                    var name = path.Substring(asset_dir.Length + 1);
                    if (name.Substring(0, 4) == "bin\\" || name.Substring(0, 4) == "bin/")
                        continue;
                    if (name == "iggsdk_icon.png")
                        continue;
                    if (name == "google-services-desktop.json")
                        continue;

                    var fileNode = doc.CreateElement("File");

                    var md5 = CoreUtils.GetFileMd5(path).ToLower();

                    fileNode.SetAttribute("Name", name);
                    fileNode.SetAttribute("MD5", md5);
                    dicMd5[name] = md5;
                    root.AppendChild(fileNode);
                }
                doc.Save(md5_file);
            }
            else
            {
                doc.Load(md5_file);
                var root = doc.DocumentElement;
                foreach (var child in root.ChildNodes)
                {
                    var childElement = child as XmlElement;
                    var name = childElement.GetAttribute("Name");
                    var md5 = childElement.GetAttribute("MD5");
                    dicMd5[name] = md5;
                }
            }

            return dicMd5;
        }

        private static string extract_file(string zip_file, string sub_dir, string out_dir)
        {
            var md5 = CoreUtils.GetFileMd5(zip_file).ToLower();
            var dir = Path.Combine(out_dir, md5);
            var asset_dir = Path.Combine(dir, Path.GetFileName(sub_dir));
            sub_dir = sub_dir + "/";
            Debug.Log(asset_dir);
            if(!Directory.Exists(asset_dir))
            {
                Directory.CreateDirectory(asset_dir);

                var zip = new ZipFile(zip_file);
                foreach (ZipEntry theEntry in zip)
                {
                    if (theEntry.IsDirectory)
                        continue;

                    if (theEntry.Name.Length < sub_dir.Length)
                        continue;

                    if (!theEntry.Name.Substring(0, sub_dir.Length).Equals(sub_dir))
                    {
                        continue;
                    }

                    var fileName = Path.Combine(asset_dir, theEntry.Name.Substring(sub_dir.Length));

                    var file_dir = Path.GetDirectoryName(fileName);
                    if (!Directory.Exists(file_dir))
                    {
                        Directory.CreateDirectory(file_dir);
                    }
                    if (theEntry.Size > 0)
                    {
                        FileStream streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        var s = zip.GetInputStream(theEntry);
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
            }

            return dir;
        }
        [MenuItem("Skyunion/Build/生成补丁")]
        private static void BuildPatch()
        {
            var path = Path.Combine(System.Environment.CurrentDirectory, "BuildPatch");
            var dic = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        var li = line.Split(':'); //将一行用,分开成键值对
                        if (li.Length == 2)
                        {
                            dic.Add(li[0], li[1]);
                        }
                        if(li.Length == 3)
                        {
                            dic.Add(li[0], li[1]+":"+li[2]);
                        }
                    }
                }
                sr.Close();
            }

            var asset_dir = dic["SubDir"];
            var temp_dir = dic["Temp"];
            var old_file = dic["OldFile"];
            var new_file = dic["NewFile"];
            var patch_file = dic["PatchFile"];

            var old_dir = extract_file(old_file, asset_dir, temp_dir);
            var dic_old_files_md5 = get_files_md5(old_dir);

            var new_dir = extract_file(new_file, asset_dir, temp_dir);
            var dic_new_files_md5 = get_files_md5(new_dir);

            var diff_dir = Path.Combine(temp_dir, Path.GetFileName(old_dir) + "-" + Path.GetFileName(new_dir));
            var patch_dir = Path.Combine(diff_dir, "patch");
            if (!Directory.Exists(diff_dir))
            {
                Debug.Log("find diff files");
                List<string> vecBundleFiles = new List<string>();
                string catalogFile = "";
                string settingFile = "";

                List<string> vecPatchFiles = new List<string>();

                var src_dir = Path.Combine(diff_dir, "src");
                Directory.CreateDirectory(src_dir);
                foreach (var fileInfo in dic_new_files_md5)
                {
                    string md5;
                    if (!dic_old_files_md5.TryGetValue(fileInfo.Key, out md5) || !md5.Equals(fileInfo.Value))
                    {
                        var name = fileInfo.Key;
                        var patch_path = Path.Combine(src_dir, name);
                        var new_path = Path.Combine(new_dir, asset_dir, name);
                        var old_path = Path.Combine(old_dir, asset_dir, name);

                        Directory.CreateDirectory(Path.GetDirectoryName(patch_path));
                        if (Path.GetExtension(name) == ".bundle")
                        {
                            vecBundleFiles.Add(Path.GetFileName(name));
                        }
                        else if (Path.GetFileName(name) == "catalog.json")
                        {
                            catalogFile = patch_path;
                            var settingPath = Path.Combine(Path.GetDirectoryName(new_path), "settings.json");
                            var settingPatch = Path.Combine(Path.GetDirectoryName(patch_path), "settings.json");
                            settingFile = settingPatch;

                            File.Copy(settingPath, settingPatch, true);
                            vecPatchFiles.Add(Path.Combine(Path.GetDirectoryName(name), "settings.json"));
                        }
                        else if (Path.GetFileName(name) == "Hotfix.pdb.bytes")
                        {
                            continue;
                        }
                        File.Copy(new_path, patch_path);
                        vecPatchFiles.Add(name);
                    }
                }

                if (catalogFile != "")
                {
                    Debug.Log($"build catalogFile: {Path.GetDirectoryName(catalogFile)}");

                    JObject value = JObject.Parse(File.ReadAllText(catalogFile));
                    var resources = value["m_InternalIds"].ToArray<JToken>();
                    for (int i = 0; i < resources.Length; i++)
                    {
                        var resource = resources[i];
                        var name = resource.ToString();
                        if (Path.GetExtension(name) == ".bundle")
                        {
                            var baseName = Path.GetFileName(name);
                            for (int j = 0; j < vecBundleFiles.Count; j++)
                            {
                                if (baseName == vecBundleFiles[j])
                                {
                                    resource = name.Replace("{UnityEngine.AddressableAssets.Addressables.RuntimePath}", "{Skyunion.VersionUtil.HotfixAddressablesPath}");
                                    resources[i] = resource;
                                }
                            }
                        }
                    }
                    value["m_InternalIds"] = JToken.FromObject(resources);
                    File.WriteAllText(catalogFile, value.ToString());
                }
                if (settingFile != "")
                {
                    Debug.Log($"build settingFile: {Path.GetDirectoryName(settingFile)}");
                    JObject value = JObject.Parse(File.ReadAllText(settingFile));
                    var locations = value["m_CatalogLocations"];
                    var location = locations[0];
                    var internalId = location["m_InternalId"];
                    internalId = internalId.ToString().Replace("{UnityEngine.AddressableAssets.Addressables.RuntimePath}", "{Skyunion.VersionUtil.HotfixAddressablesPath}");
                    locations[0]["m_InternalId"] = internalId;
                    File.WriteAllText(settingFile, value.ToString());
                }

                if (!Directory.Exists(patch_dir))
                {
                    Debug.Log("build patch ...");
                    for (int i = 0; i < vecPatchFiles.Count; i++)
                    {
                        var name = vecPatchFiles[i];
                        var patch_path = Path.Combine(patch_dir, name);
                        var old_path = Path.Combine(old_dir, asset_dir, name);
                        var new_path = Path.Combine(src_dir, name);
                        if (File.Exists(old_path))
                        {
                            var newName = Path.Combine(patch_dir, name) + ".patch";
                            Debug.Log($"bsdiff:({i}/{vecPatchFiles.Count}){Path.GetFileName(newName)}");
                            Directory.CreateDirectory(Path.GetDirectoryName(newName));
                            lbsdiff.bsdiff2(old_path, new_path, newName);
                        }
                        else
                        {
                            Debug.Log($"copy:({i}/{vecPatchFiles.Count}){Path.GetFileName(patch_path)}");
                            Directory.CreateDirectory(Path.GetDirectoryName(patch_path));
                            File.Copy(new_path, patch_path, true);
                        }
                    }
                    Debug.Log("build patch successed!");
                }

                var patch_new_dir = Path.Combine(diff_dir, "new");
                if (!File.Exists(patch_new_dir))
                {
                    var start = Time.realtimeSinceStartup;
                    Debug.Log("test patch ...");
                    for (int i = 0; i < vecPatchFiles.Count; i++)
                    {
                        var name = vecPatchFiles[i];
                        var patch_path = Path.Combine(patch_dir, name);
                        var old_path = Path.Combine(old_dir, asset_dir, name);
                        var new_path = Path.Combine(patch_new_dir, name);
                        Directory.CreateDirectory(Path.GetDirectoryName(new_path));
                        if (File.Exists(old_path))
                        {
                            var patch_name = Path.Combine(patch_dir, name) + ".patch";
                            Debug.Log($"bspatch:({i}/{vecPatchFiles.Count}){Path.GetFileName(name)}");
                            lbsdiff.bspatch2(old_path, new_path, patch_name);
                        }
                        else
                        {
                            Debug.Log($"copy:({i}/{vecPatchFiles.Count}){Path.GetFileName(patch_path)}");
                            File.Copy(patch_path, new_path);
                        }
                    }
                    var duration = Time.realtimeSinceStartup - start;
                    Debug.Log($"test patch successed! use time:{duration}s");
                }

                var patch_zip_dir = Path.GetDirectoryName(patch_file);
                if (!Directory.Exists(patch_zip_dir))
                {
                    Directory.CreateDirectory(patch_zip_dir);
                }
                if (File.Exists(patch_file))
                {
                    File.Delete(patch_file);
                }

                (new FastZip()).CreateZip(patch_file, patch_dir, true, "");
                Debug.Log($"build patch successed! "+ patch_file);
            }
        }

        private static int GetVersionCode()
        {
            var version = PlayerSettings.bundleVersion;
            var versions = version.Split('.');
            int v3 = versions.Length > 2 ? int.Parse(versions[2]) : 0;
            return v3;
        }

        private static string GetBuildTargetName(BuildTarget target)
        {
            var name = PlayerSettings.productName;
            switch (target)
            {
                case BuildTarget.Android:
                    return name + PlayerSettings.Android.bundleVersionCode + ".apk";

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return name + PlayerSettings.Android.bundleVersionCode + ".exe";

#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return name + ".app";

#else
                    case BuildTarget.StandaloneOSXIntel:
                    case BuildTarget.StandaloneOSXIntel64:
                    case BuildTarget.StandaloneOSXUniversal:
                                        return name + ".app";

#endif

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }


    }
}