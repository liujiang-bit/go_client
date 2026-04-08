using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class RewardGetCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
           switch(notification.Name)
            {
                case CmdConstant.RewardGet:
                    CoreUtils.uiManager.ShowUI(UI.s_rewardGet,null,notification.Body);
                    break;
                default:break;
            }
        }
    }
}

