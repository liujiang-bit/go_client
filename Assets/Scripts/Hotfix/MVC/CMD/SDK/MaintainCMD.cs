using PureMVC.Interfaces;
using Skyunion;
using System;
using System.Collections.Generic;
using System.IO;
using U3D.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class MaintainCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch(notification.Name)
            {
                case CmdConstant.PackageUpdateCheck:
                    {
                        if (SDKBridge.appConfig != null)
                        {
                            long appversion = VersionUtil.GetVersionNumber();

                            var loginBox = SDKBridge.appConfig.getServerConfig().loginBox;

                            var forceVersion = VersionUtil.GetVersionNumber(loginBox.forceVersion);
                            if (appversion < forceVersion)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_Maintain, null, MaintainType.ForceUpdate);
                                return;
                            }

                            var version = VersionUtil.GetVersionNumber(loginBox.version);
                            if (appversion < version)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_Maintain, null, MaintainType.OptionalUpdate);
                                return;
                            }
                        }

                        // 如果读取不到配置，也需要可以登陆游戏。
                        SendNotification(CmdConstant.AutoLogin);
                    }
                    break;
                case CmdConstant.HotfixUpteCheck:
                    {
                        // 如果读取不到配置，也需要可以登陆游戏。
                        if (SDKBridge.appConfig == null)
                        {
                            SendNotification(CmdConstant.LoginToServer);
                            return;
                        }

                        // 
                        var serverConfig = SDKBridge.appConfig.getServerConfig();

                        int nHotFixNumber = 0;
                        string md5 = "";
                        var hotFixVersion = serverConfig.update.hotfixVersion;
                        if (!string.IsNullOrEmpty(hotFixVersion))
                        {
                            var versions = hotFixVersion.Split('|');
                            for (int i = 0; i < versions.Length; i++)
                            {
                                var numbers = versions[i].Split(',');
                                if (numbers[0].Equals(Application.version))
                                {
                                    nHotFixNumber = int.Parse(numbers[1]);
                                    if (numbers.Length > 2)
                                    {
                                        md5 = numbers[2];
                                    }
                                }
                            }
                        }
                        // 当前补丁版本号低于配置的版本号需要热更新
                        if (VersionUtil.HotfixNumber < nHotFixNumber)
                        {
                            PlayerPrefs.SetInt("HotFixNumber", nHotFixNumber);
                            PlayerPrefs.SetString("HotFixMd5", md5);
                            var strDownloadUrl = $"{serverConfig.update.hotfixUrl}/{VersionUtil.GetPlatform()}/{Application.version}/{nHotFixNumber}.zip";

                            UnityWebRequest uwr = UnityWebRequest.Head(strDownloadUrl);
                            var request = uwr.SendWebRequest();
                            request.completed += (AsyncOperation asyncOperation) =>
                            {
                                string size = uwr.GetResponseHeader("Content-Length");
                                if (uwr.isNetworkError || uwr.isHttpError)
                                {
                                    CoreUtils.uiManager.SetGuideStatus(true);
                                    Alert.CreateAlert(100032).SetRightButton(() =>
                                    {
                                        CoreUtils.uiManager.SetGuideStatus(false);
                                        SendNotification(CmdConstant.HotfixUpteCheck);
                                    }).Show();
                                }
                                else
                                {
                                    var nTotalSize = Convert.ToInt64(size);

                                    var savePath = Path.Combine(Application.temporaryCachePath, "HotFix", Application.version, nHotFixNumber.ToString()) + ".zip";
                                    CoreUtils.logService.Info($"Download HotFix File:{savePath}", Color.green);
                                    long nDownloadedSize = 0;
                                    if (File.Exists(savePath))
                                    {
                                        var fi = new FileInfo(savePath);
                                        nDownloadedSize = fi.Length;
                                    }
                                    else
                                    {
                                        var dir = Path.GetDirectoryName(savePath);
                                        if(!Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                    }

                                    long nNeedDownload = nTotalSize - nDownloadedSize;
                                    if (nNeedDownload == 0)
                                    {
                                        SendNotification(CmdConstant.HotfixDownCompleted, savePath);
                                    }
                                    else
                                    {
                                        bool bAppend = true;
                                        // -------------临时移除断点续传 CDN 不支持---------------------
                                        //bAppend = false;
                                        //nDownloadedSize = 0;
                                        //nNeedDownload = nTotalSize - nDownloadedSize;
                                        //-------------------------end----------------------------
                                        CoreUtils.uiManager.SetGuideStatus(true);
                                        Alert.CreateAlert(LanguageUtils.getTextFormat(100060, HotfixUtil.FormatFileSize(nNeedDownload))).SetRightButton(() =>
                                        {
                                            CoreUtils.uiManager.SetGuideStatus(false);
                                            CoreUtils.logService.Info($"Start Download HotFix:{strDownloadUrl}", Color.green);
                                            var www = new UnityWebRequest(strDownloadUrl, UnityWebRequest.kHttpVerbGET, new DownloadHandlerFile(savePath, bAppend), null);
                                            www.SetRequestHeader("Range", "bytes=" + nDownloadedSize + "-");
                                            var downloadRequest = www.SendWebRequest();

                                            var timer = Timer.Register(1.0f / Application.targetFrameRate, null, (dt) =>
                                            {
                                                long nDownloadSize = (long)www.downloadedBytes;
                                                SendNotification(CmdConstant.HotfixDownloadProgress, nDownloadSize);

                                            }, true);

                                            downloadRequest.completed += (AsyncOperation asyncOperation2) =>
                                            {
                                                timer.Cancel();
                                                if (uwr.isNetworkError || uwr.isHttpError)
                                                {
                                                    CoreUtils.uiManager.SetGuideStatus(true);
                                                    Alert.CreateAlert(100018).SetRightButton(() =>
                                                    {
                                                        CoreUtils.uiManager.SetGuideStatus(false);
                                                        SendNotification(CmdConstant.HotfixUpteCheck);
                                                    }).Show();
                                                }
                                                else
                                                {
                                                    long nDownloadSize = (long)www.downloadedBytes;
                                                    SendNotification(CmdConstant.HotfixDownloadProgress, nDownloadSize);
                                                    SendNotification(CmdConstant.HotfixDownCompleted, savePath);
                                                }
                                            };

                                            SendNotification(CmdConstant.HotfixStartDownload, nNeedDownload);
                                        }).Show();
                                    }
                                }
                            };
                        }
                        // 无需热更新直接走维护公告
                        else
                        {
                            // 目前还没制作热更新，直接调用检测维护公告
                            SendNotification(CmdConstant.MaintainCheck);
                        }
                    }
                    break;
                case CmdConstant.HotfixDownCompleted:
                    {
                        // 保存文件到本地并解压
                        string savePath = (string)notification.Body;
                        int nHotFixNum = PlayerPrefs.GetInt("HotFixNumber");
                        string md5 = PlayerPrefs.GetString("HotFixMd5");
                        if (!string.IsNullOrEmpty(md5))
                        {
                            var fileMd5 = CoreUtils.GetFileMd5(savePath);
                            if (!fileMd5.Equals(md5))
                            {
                                Debug.LogError($"File Md5 No Compare ! Re Download ! \n{md5}\n{fileMd5}");
                                Task.RunInMainThread(() =>
                                {
                                    File.Delete(savePath);
                                    SendNotification(CmdConstant.HotfixUpteCheck);
                                });
                                return;
                            }
                        }

                        List<string> files = new List<string>();
                        var extractDir = Path.Combine(VersionUtil.HotfixVersionPath, nHotFixNum.ToString());
                        CoreUtils.unZipFileAsync(savePath, extractDir, () =>
                        {
                            Debug.LogError("File Uncompress Error ! Re Download !");
                            File.Delete(savePath);
                            Task.RunInMainThread(() =>
                            {
                                SendNotification(CmdConstant.HotfixUpteCheck);
                            });
                        },
                        (nTotalCount) =>
                        {
                            Task.RunInMainThread(() =>
                            {
                                SendNotification(CmdConstant.HotfixUnCompress, nTotalCount);
                            });
                        },
                        (nFileCount, nTotalCount, filePath) =>
                        {
                            Task.RunInMainThread(() =>
                            {
                                SendNotification(CmdConstant.HotfixUnCompressProgress, nFileCount);

                                var patch_file = filePath;
                                var patchExt = ".patch";
                                if (Path.GetExtension(patch_file).Equals(patchExt))
                                {
                                    var name = filePath.Substring(extractDir.Length + 1, filePath.Length-extractDir.Length-1-patchExt.Length);
                                    var new_file = patch_file.Substring(0, patch_file.Length - patchExt.Length);

                                    var old_file = Path.Combine(Application.temporaryCachePath, "Hotfix", Application.version, name);
                                    //if (!File.Exists(old_file))
                                    {
                                        var dir = Path.GetDirectoryName(old_file);
                                        if(!Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                        Debug.Log("old_file:"+ old_file);
                                        var pkg_file = Path.Combine(Application.streamingAssetsPath, name);
                                        Debug.Log("pkg_file:" + pkg_file);
                                        var data = CoreUtils.assetService.LoadFile(pkg_file, false);
                                        File.WriteAllBytes(old_file, data);
                                    }

                                    Debug.Log("new_file:" + new_file);
                                    lbsdiff.bspatch2(old_file, new_file, patch_file);
                                }

                                if (nFileCount == nTotalCount)
                                {
                                    CoreUtils.logService.Info($"Extract HotFix Finished:{extractDir}", Color.green);
                                    // 文件保存完毕之后，需要保存热更新的版本号
                                    VersionUtil.HotfixNumber = nHotFixNum;
                                    // 删除上一次的热更新目录
                                    if (PlayerPrefs.HasKey("HotFixDir"))
                                    {
                                        Directory.Delete(PlayerPrefs.GetString("HotFixDir"), true);
                                    }

                                    // 保存本次热更新目录
                                    PlayerPrefs.SetString("HotFixDir", extractDir);
                                    PlayerPrefs.Save();
                                    File.Delete(savePath);

                                    Alert.CreateAlert(LanguageUtils.getTextFormat(100025, HotfixUtil.FormatFileSize(1))).SetRightButton(() =>
                                    {
                                        // 直接重启
                                        CoreUtils.RestarGame();
                                    }).Show();
                                }
                            });
                        });
                    }
                    break;
                case CmdConstant.MaintainCheck:
                    {
                        if (SDKBridge.appConfig != null)
                        {
                            var serverConfig = SDKBridge.appConfig.getServerConfig();
                            var iggid = SDKSession.currentSession.getIGGId();
                            for (int i = 0; i < serverConfig.loginBox.whiteList.Count; i++)
                            {
                                var user = serverConfig.loginBox.whiteList[i].userList;
                                if (user.Equals(iggid))
                                {
                                    // 白名单不需要走公告，可以登陆游戏。
                                    SendNotification(CmdConstant.LoginToServer);
                                    //CoreUtils.uiManager.ShowUI(UI.s_InputServerIdView);
                                    return;
                                }
                            }
                            var serverTime = SDKBridge.appConfig.serverTime;
                            DateTime serverDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(serverTime).AddHours(-5);
                            // 维护期间弹出维护公告
                            if (serverConfig.update.isMaintain.state == 1 && string.IsNullOrEmpty(serverConfig.update.serverId) && serverConfig.update.isMaintain.startAt < serverDateTime && serverConfig.update.isMaintain.endAt > serverDateTime)
                            {
                                // 显示维护框
                                CoreUtils.uiManager.ShowUI(UI.s_Maintain, null, MaintainType.Normal);
                                return;
                            }
                        }
                        // 如果读取不到配置，也需要可以登陆游戏。
                        SendNotification(CmdConstant.LoginToServer);
                    }
                    break;
                case CmdConstant.MaintainCheckSingleServer:
                    {
                        var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                        if (SDKBridge.appConfig != null)
                        {
                            var serverConfig = SDKBridge.appConfig.getServerConfig();
                            var iggid = SDKSession.currentSession.getIGGId();
                            for (int i = 0; i < serverConfig.loginBox.whiteList.Count; i++)
                            {
                                var user = serverConfig.loginBox.whiteList[i].userList;
                                if (user.Equals(iggid))
                                {
                                    // 白名单不需要走公告，可以登陆游戏。
                                    if (netProxy != null)
                                    {
                                        netProxy.RedirectGameServer();
                                    }
                                    return;
                                }
                            }
                            var serverTime = SDKBridge.appConfig.serverTime;
                            DateTime serverDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(serverTime).AddHours(-5);
                            // 维护期间弹出维护公告
                            if (serverConfig.update.isMaintain.state == 1 && serverConfig.update.isMaintain.startAt < serverDateTime && serverConfig.update.isMaintain.endAt > serverDateTime)
                            {
                                var serverName = notification.Body as string;
                                if (serverName != null)
                                {
                                    var serverIds = serverConfig.update.serverId.Split(',');

                                    for (int i = 0; i < serverIds.Length; i++)
                                    {
                                        if (serverName.Equals("game" + serverIds[i]))
                                        {
                                            // 显示维护框
                                            CoreUtils.uiManager.ShowUI(UI.s_Maintain, null, MaintainType.NormalSingle);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        if (netProxy != null)
                        {
                            netProxy.RedirectGameServer();
                        }
                    }
                    break;
            }
        }
    }
}

