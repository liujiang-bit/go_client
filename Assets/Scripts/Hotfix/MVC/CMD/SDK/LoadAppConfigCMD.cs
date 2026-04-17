using Newtonsoft.Json;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LoadAppConfigCMD : GameCmd
    {
        private static SDKServerConfig m_serverConfig;
        public override void Execute(INotification notification)
        {
            if(HotfixUtil.IsDebugServerConfig())
            {
                SDKBridge.shareInstance().initialize("server_config_test", OnConfigLoaded, OnConfigLoadedbackup);
            }
            else
            {
                SDKBridge.shareInstance().initialize("server_config", OnConfigLoaded, OnConfigLoadedbackup);
            }
        }
        private void OnConfigLoaded(SDKAppConfig config, long serverTime)
        {
            config.serverTime = serverTime;
            // 检测整包更新
            SendNotification(CmdConstant.PackageUpdateCheck);
        }
        private void OnConfigLoadedbackup(SDKAppConfig config, long serverTime)
        {
            if(!config.isNull())
            {
                OnConfigLoaded(config, serverTime);
            }
            else
            {
                SendNotification(CmdConstant.AutoLogin);
            }
        }

    }
}

