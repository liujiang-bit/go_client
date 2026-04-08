using IGGSDKConstant;
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
        private static IGGServerConfig m_serverConfig;
        public override void Execute(INotification notification)
        {
            if(HotfixUtil.IsDebugServerConfig())
            {
                IGGSDK.shareInstance().initialize("server_config_test", OnConfigLoaded, OnConfigLoadedbackup);
            }
            else
            {
                IGGSDK.shareInstance().initialize("server_config", OnConfigLoaded, OnConfigLoadedbackup);
            }
        }
        private void OnConfigLoaded(IGGPrimaryAppConfig config, IGGEasternStandardTime serverTime)
        {
            config.serverTime = serverTime.getTimestamp();
            // 检测整包更新
            SendNotification(CmdConstant.PackageUpdateCheck);
        }
        private void OnConfigLoadedbackup(IGGPrimaryAppConfigBackup config, IGGEasternStandardTime serverTime)
        {
            if(!config.isNull())
            {
                OnConfigLoaded(config.getAppconfig(), serverTime);
            }
            else
            {
                SendNotification(CmdConstant.AutoLogin);
            }
        }

    }
}

