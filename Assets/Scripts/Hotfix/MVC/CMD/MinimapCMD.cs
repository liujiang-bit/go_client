using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class MinimapCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            MinimapProxy minimapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
            switch (notification.Name)
            {
                case Guild_GuildMemberPos.TagName:
                    minimapProxy.UpdateAllianceMap(notification.Body);
                    break;
            }
        }

    }
}

