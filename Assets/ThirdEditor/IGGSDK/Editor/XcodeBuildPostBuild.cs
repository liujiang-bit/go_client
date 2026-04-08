#if (UNITY_IOS)
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFramework
{
    public static class XcodeBuildPostBuild
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
        {
            if (target == BuildTarget.iOS)
            {

                {
                    // get plist
                    string AppCon = pathToBuildProject + "/Classes/UnityAppController.mm";
                    string str = File.ReadAllText(AppCon);

                    int index = -1;
                    // 添加IGGSDK头文件
                    index = str.IndexOf("IGGSDK");
                    if (index == -1)
                    {
                        str = str.Replace("#include <sys/sysctl.h>", "#include <sys/sysctl.h>\n\n#import <IGGSDKCore/IGGSDKCore.h>\n#import <IGGSDKMisc/IGGSDKMisc.h>\n");
                    }
                    index = str.IndexOf("didRegisterForRemoteNotificationsWithDeviceToken:deviceToken]");
                    if (index == -1)
                    {
                        string strFirst = "didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken\n{\n";
                        str = str.Replace(strFirst, strFirst + "\t[[IGGPushNotification sharedInstance] onApplication:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];\n");
                    }
                    index = str.IndexOf("didReceiveRemoteNotification:userInfo");
                    if (index == -1)
                    {
                        string strFirst = "didReceiveRemoteNotification:(NSDictionary*)userInfo\n{\n";
                        str = str.Replace(strFirst, strFirst + "\t[[IGGPushNotification sharedInstance] onApplication:application didReceiveRemoteNotification:userInfo];\n");
                    }
                    index = str.IndexOf("willFinishLaunchingWithOptions:launchOptions]");
                    if (index == -1)
                    {
                        string strFirst = "willFinishLaunchingWithOptions:(NSDictionary*)launchOptions\n{\n";
                        str = str.Replace(strFirst, strFirst + "\t[[IGGPushNotification sharedInstance] onApplication:application willFinishLaunchingWithOptions:launchOptions];\n");
                    }
                    index = str.IndexOf("didReceiveRemoteNotificationDel");
                    if (index == -1)
                    {
                        string strFirst = "didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:";
                        str = str.Replace(strFirst, "didReceiveRemoteNotificationDel:(NSDictionary *)userInfo fetchCompletionHandler:");
                    }
                    index = str.IndexOf("[NSMutableArray arrayWithArray:[[NSProcessInfo processInfo] arguments]]");
                    if (index == -1)
                    {
                        string strFirst = "didFinishLaunchingWithOptions:(NSDictionary*)launchOptions\n{\n";
                        string str1 = "\tNSMutableArray *newArguments = [NSMutableArray arrayWithArray:[[NSProcessInfo processInfo] arguments]];\n";
                        string str2 = "\t[newArguments addObject:@\"-FIRDebugEnabled\"];\n";
                        string str3 = "\t[[NSProcessInfo processInfo] setValue:[newArguments copy] forKey:@\"arguments\"];\n";
                        str = str.Replace(strFirst, strFirst+str1+str2+str3);
                    } 
                    File.WriteAllText(AppCon, str);
                }
                {
                    string preprocessorPath = pathToBuildProject + "/Classes/Preprocessor.h";
                    string text = File.ReadAllText(preprocessorPath);
                    text = text.Replace("UNITY_USES_REMOTE_NOTIFICATIONS 0", "UNITY_USES_REMOTE_NOTIFICATIONS 1");
                    File.WriteAllText(preprocessorPath, text);
                }

                PBXProject project = new PBXProject();
                string sPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
                project.ReadFromFile(sPath);
                string tn = PBXProject.GetUnityTargetName();
                string g = project.TargetGuidByName(tn);
                File.Copy(Application.dataPath + "/Plugins/Android/res/raw/iggsdk.json", pathToBuildProject + "/Data/Raw/IGGSDK.json");
                project.AddFileToBuild(g, project.AddFile("Data/Raw/IGGSDK.json", "Data/Raw/IGGSDK.json"));

                string entiPath = pathToBuildProject + "/Unity-iPhone/Base.entitlements";
                if(!File.Exists(entiPath))
                {
                    // add push notification
                    var entiDoc = new PlistDocument();
                    entiDoc.Create();
                    entiDoc.root["aps-environment"] = new PlistElementString("development");
                    entiDoc.WriteToFile(entiPath);
                }

                project.AddCapability(g, PBXCapabilityType.PushNotifications, entiPath, true);
                project.AddCapability(g, PBXCapabilityType.InAppPurchase, entiPath, true);
                project.AddCapability(g, PBXCapabilityType.GameCenter, entiPath, true);
                project.SetBuildProperty(g, "ENABLE_BITCODE", "NO");
                // add iap
                project.AddFrameworkToProject(g, "StoreKit.framework", true);

                // add gamecenter
                string infoPath = pathToBuildProject + "/Info.plist";
                var plistDocument = new PlistDocument();
                plistDocument.ReadFromFile(infoPath);
                var cap = plistDocument.root["UIRequiredDeviceCapabilities"] as PlistElementArray;
                cap.AddString("gamekit");
                plistDocument.root.SetString("NSPhotoLibraryUsageDescription", "This application needs your permission to access your album.");
                plistDocument.root.SetString("NSCameraUsageDescription", "This application needs your permission to access your camera.");
                plistDocument.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);
                if (plistDocument.root.values.ContainsKey("NSAppTransportSecurity"))
                {
                    plistDocument.root.values.Remove("NSAppTransportSecurity");
                }
                PlistElementDict ats = plistDocument.root.CreateDict("NSAppTransportSecurity");
                ats.SetBoolean("NSAllowsArbitraryLoads", true);
                plistDocument.WriteToFile(infoPath);
                File.WriteAllText(sPath, project.WriteToString());

                // 多语言应用名
                // var appNamePath = Application.dataPath + "/Plugins/iOS/AppName";
                // var dirs = Directory.GetDirectories(appNamePath);
                // foreach(var dir in dirs)
                // {
                //     var paths = Directory.GetFiles(dir, "*.strings");
                //     foreach (var path in paths)
                //     {
                //         string fName = path.Substring(appNamePath.Length + 1);
                //         var toPath = Path.Combine(pathToBuildProject, "Unity-iPhone Tests", fName);
                //         var toDir = Path.GetDirectoryName(toPath);
                //         if (!Directory.Exists(toDir))
                //         {
                //             Directory.CreateDirectory(toDir);
                //         }
                //         File.Copy(path, toPath, true);
                //         project.AddFileToBuild(g, project.AddFile(fName, "Unity-iPhone Tests/Supporting Files", PBXSourceTree.Group));
                //     }
                // }
                NativeLocale.AddLocalizedStringsIOS(pathToBuildProject, Path.Combine(Application.dataPath, "Plugins/iOS/AppName"));

            }
        }
    }
}
#endif