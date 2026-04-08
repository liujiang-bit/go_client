using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    public enum RoleNofityOperateType
    {
        CollectRuneFinish  = 1,
        CompulsoryMoveCity = 2,//强制迁城
        BattleLose         = 3,//战损补偿通知
    }


    public class RoleNotify : GameCmd
    {      
        public override void Execute(INotification notification)
        {           

            switch (notification.Name)
            {
                case Role_RoleNotify.TagName:
                    {
                        Debug.LogError("Role_RoleNotify");
                        Role_RoleNotify.request notify = notification.Body as Role_RoleNotify.request;
                        if (notify == null) return;
                        Debug.LogErrorFormat("Role_RoleNotify:{0}", (RoleNofityOperateType)notify.notifyOperate);
                        switch ((RoleNofityOperateType)notify.notifyOperate)
                        {
                            case RoleNofityOperateType.CollectRuneFinish:
                                {                                   
                                    AppFacade.GetInstance().SendNotification(CmdConstant.CollectRuneFinish, notify);
                                }
                                break;
                            case RoleNofityOperateType.CompulsoryMoveCity:
                                {
                                  //  Alert.CreateAlert(770106, LanguageUtils.getText(730120)).SetLeftButton(null, LanguageUtils.getText(192009)).Show();
                                }
                                break;
                            case RoleNofityOperateType.BattleLose: //战损补偿
                                {
                                    var emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;

                                    if (notify.stringArg != null && notify.stringArg.Count > 0)
                                    {
                                        string str = "";
                                        if (string.IsNullOrEmpty(notify.stringArg[0]))
                                        {
                                            str = LanguageUtils.getText(555032);
                                        }
                                        else
                                        {
                                            str = emailProxy.OnTextFormat(LanguageUtils.getText(555028), new List<string> { notify.stringArg[0] });
                                        }
                                        Alert.CreateAlert(str).SetRightButton(() =>
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.ShowChapterDiaglog, 2000);
                                        }).Show();
                                    }
                                }
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}