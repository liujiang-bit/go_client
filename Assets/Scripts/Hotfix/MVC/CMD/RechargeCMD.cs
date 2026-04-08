// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    RechargeInfoCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;

namespace Game {
    public class RechargeCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            //Log.Info("Execute RechargeInfoCMD");
            switch (notification.Name)
            {
                case Recharge_RechargeInfo.TagName:
                    var noti = notification.Body as Recharge_RechargeInfo.request;
                    Tip.CreateTip(LanguageUtils.getTextFormat(300237,LanguageUtils.getText(124034) + "X" + noti.denar) ).Show();
                    break;
            }
        }
    }
}