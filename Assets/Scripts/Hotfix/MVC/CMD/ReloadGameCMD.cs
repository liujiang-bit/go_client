using Client;
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
        private static SDKServerConfig m_serverConfig;
        public override void Execute(INotification notification)
        {
            CoreUtils.logService.Info($"Start Reload Game:{Time.realtimeSinceStartup}", Color.green);
            CoreUtils.uiManager.CloseAll(true);
            WorldCamera.Instance().ClearViewChange();
            HUDManager.Instance().CloseAll();
            AlertManager.Instance.Clear();
            TipManager.Instance.Clear();

            // 设置 SDK 的
            SDKBridge.shareInstance().ChangeGame(LanguageUtils.GetLanguage());
            // 进入Loading界面
            CoreUtils.uiManager.ShowUI(UI.s_Loading);
        }
    }
}

