using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class CurrencyCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            BuildingResourcesProxy m_buildingRssProxy = AppFacade.GetInstance().RetrieveProxy(BuildingResourcesProxy.ProxyNAME) as BuildingResourcesProxy;
            switch (notification.Name)
            {
                case Build_GetBuildResources.TagName:
                    m_buildingRssProxy.IsCollecting = false;
                    break;
                default: break;
            }
        }
    }
}