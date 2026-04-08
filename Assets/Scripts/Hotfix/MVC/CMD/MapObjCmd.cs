using PureMVC.Interfaces;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class MapObjCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case Map_ObjectInfo.TagName:
                    {
                        Map_ObjectInfo.request mapItemInfo = notification.Body as Map_ObjectInfo.request;
                        if (mapItemInfo == null)
                        {
                            return;
                        }
                        var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                        if(playerProxy.CurrentRoleInfo == null)
                        {
                            //var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                            //netProxy.CloseGameServer();
                            return;
                        }

                        var worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                        if (worldMapObjectProxy == null)
                            break;
                        worldMapObjectProxy.UpdateMapObject(mapItemInfo);
                        if ((RssType)mapItemInfo.mapObjectInfo.objectType == RssType.Troop ||
                            (RssType)mapItemInfo.mapObjectInfo.objectType == RssType.Scouts ||
                            (RssType)mapItemInfo.mapObjectInfo.objectType == RssType.Transport ||
                            (RssType)mapItemInfo.mapObjectInfo.objectType == RssType.Expedition)
                        {
                            var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                            if (troopProxy == null)
                            {
                                break;
                            }
                            troopProxy.Create(notification);
                        }
                    }
                    break;

                case Map_ObjectDelete.TagName:
                    {
                        Map_ObjectDelete.request del = notification.Body as Map_ObjectDelete.request;
                        if (del == null)
                        {
                            return;
                        }
                        var worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                        if (worldMapObjectProxy == null)
                            break;
                        worldMapObjectProxy.DelMapObject(del.objectId);
                    }
                    break;                
            }
        }
    }
}