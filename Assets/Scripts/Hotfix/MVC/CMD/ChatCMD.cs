using Client;
using Hotfix.Utils;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class ChatCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            ChatProxy chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            switch (notification.Name)
            {
                case SprotoType.Chat_PushMsg.TagName:
                    {
                        chatProxy.OnProgressMsg(notification.Body);
                    }
                    break;
                case CmdConstant.ChatClientNetEvent:
                    {
                        chatProxy.OnSyncNetEvent(notification);
                    }
                    break;
                case CmdConstant.OnCityLoadFinished:
                    {
                        chatProxy.InitChatChannel();
                    }
                    break;
                case Guild_CreateGuild.TagName:
                case Guild_ApplyJoinGuild.TagName:
                case CmdConstant.AllianceEixt:
                    chatProxy.OnUpdateAllianceContact();
                    break;
                case Chat_Msg2GSQueryPrivateChatLst.TagName:
                    chatProxy.SetPrivateChatList(notification.Body as Chat_Msg2GSQueryPrivateChatLst.response);
                    break;
                case Chat_Msg2GSQueryPrivateChatByRid.TagName:
                    chatProxy.SetPrivateChatInfos(notification.Body as Chat_Msg2GSQueryPrivateChatByRid.response);
                    break;
                case Chat_SendMsg.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    break;
                default:break;
            }
        }

    }
}

