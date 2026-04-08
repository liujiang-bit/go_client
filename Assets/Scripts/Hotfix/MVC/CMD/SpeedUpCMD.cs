using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class SpeedUpCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
           switch(notification.Name)
            {
                case CmdConstant.SpeedUp:
                    CoreUtils.uiManager.ShowUI(UI.s_speedUp,null,notification.Body);
                    break;
                default:break;
            }
        }
    }
}

