using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class EmailCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            EmailProxy m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            switch(notification.Name)
            {
                case Email_GetEmails.TagName:
                    {
                        Debug.Log("邮件 Email_GetEmails");
                        m_emailProxy.EmailReceived = true;
                        m_emailProxy.UpdateEmailVersion();
                    }
                    break;
                case Email_EmailList.TagName:
                    {
                        m_emailProxy.UpdateEmail(notification.Body);
                    }
                    break;
                case Email_TakeEnclosure.TagName:
                    {
                        Email_TakeEnclosure.response res = notification.Body as Email_TakeEnclosure.response;
                        if(res!=null)
                        {
                            if(res.type==2&&res.reward!=null)//指定类型的邮件
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin,null,res.reward);
                            }
                        }
                    }
                    break;
                case Email_CollectEmail.TagName:
                    {
                        Tip.CreateTip(570015).Show();
                    }
                    break;
                case Email_DeleteEmail.TagName:
                    {                      
                        Tip.CreateTip(570012).Show();
                    }
                    break;
                case CmdConstant.SystemHourChange:
                    var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                    if (playerProxy != null)
                    {
                        if (playerProxy.CurrentRoleInfo != null)
                        {
                            playerProxy.CurrentRoleInfo.emailSendCntPerHour = 0;
                        }
                    }
                    break;
                default:break;
            }
        }
    }
}

