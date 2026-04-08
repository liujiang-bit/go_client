using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class NPCDialogCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch(notification.Name)
            {
                case CmdConstant.ShowNPCDiaglog:
                    CoreUtils.uiManager.ShowUI(UI.s_NPCDialog, null, notification.Body);
                    break;
                case CmdConstant.ShowChapterDiaglog:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_ChapterDialog, null, notification.Body);
                    }
                    break;
                default:break;
            }
        }
    }
}

