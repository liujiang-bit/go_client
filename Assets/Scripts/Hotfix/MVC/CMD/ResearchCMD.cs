using System.Collections;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;


namespace Game
{
    public class ResearchCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            ResearchProxy m_researchProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
            switch (notification.Name)
            {
                case Technology_ResearchTechnology.TagName:
                    {
                        Technology_ResearchTechnology.response res = notification.Body as Technology_ResearchTechnology.response;
                        if(res!=null&&res.HasTechnologyType&&res.HasLevel)
                        {
                           var data = m_researchProxy.GetTechnologyByLevel((int)res.technologyType, 1);
                           Tip.CreateTip(402001, LanguageUtils.getText(data.l_nameID), (int)res.level).Show();
                        }
                    }
                    break;
                default:break;
            }
        }
    }
}

