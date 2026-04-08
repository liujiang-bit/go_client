// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    ScrollMessageCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class ScrollMessageCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            //Log.Info("Execute ScrollMessageCMD");
            switch (notification.Name)
            {
                case Chat_MarqueeNotify.TagName:
                    var msg = notification.Body as Chat_MarqueeNotify.request;
                    ScrollMessageProxy proxy = AppFacade.GetInstance().RetrieveProxy(ScrollMessageProxy.ProxyNAME) as ScrollMessageProxy;
                    proxy.Enqueue(msg);
                    break;
            }

        }
    }
}