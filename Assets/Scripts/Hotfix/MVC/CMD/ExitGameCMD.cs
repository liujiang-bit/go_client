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
    public class ExitGameCMD : GameCmd
    {
        private static IGGServerConfig m_serverConfig;
        public override void Execute(INotification notification)
        {
            Alert.CreateAlert(100046, LanguageUtils.getText(300099)).SetRightButton(null, LanguageUtils.getText(100048)).SetLeftButton(ExitGame, LanguageUtils.getText(100047)).Show();
        }
        private void ExitGame()
        {
            Debug.Log("退出游戏");
            Application.Quit();
        }
    }
}

