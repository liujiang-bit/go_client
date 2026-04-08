using Client;
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
    public class ReloadGameCMD : GameCmd
    {
        private static IGGServerConfig m_serverConfig;
        public override void Execute(INotification notification)
        {
            CoreUtils.logService.Info($"Start Reload Game:{Time.realtimeSinceStartup}", Color.green);
            CoreUtils.uiManager.CloseAll(true);
            WorldCamera.Instance().ClearViewChange();
            HUDManager.Instance().CloseAll();
            AlertManager.Instance.Clear();
            TipManager.Instance.Clear();

            // 设置 IGGSDK 的
            IGGSDK.shareInstance().ChangeGame(LanguageUtils.GetLanguage());
            // 进入Loading界面
            CoreUtils.uiManager.ShowUI(UI.s_Loading);
        }
    }
}

