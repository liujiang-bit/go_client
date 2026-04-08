// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    WarWarningCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class WarWarningCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            //Log.Info("Execute WarWarningCMD");
            switch(notification.Name)
            {
                case Role_EarlyWarningInfo.TagName:
                    {
                        Role_EarlyWarningInfo.request request = notification.Body as Role_EarlyWarningInfo.request;
                        if (request == null) return;
                        if(m_warWarningProxy == null)
                        {
                            m_warWarningProxy = AppFacade.GetInstance().RetrieveProxy(WarWarningProxy.ProxyNAME) as WarWarningProxy;
                        }
                        if(m_warWarningProxy != null)
                        {
                            m_warWarningProxy.UpdateWarWarningInfo(request);
                        }
                    }
                    break;
            }           
        }

        private WarWarningProxy m_warWarningProxy = null;
    }
}