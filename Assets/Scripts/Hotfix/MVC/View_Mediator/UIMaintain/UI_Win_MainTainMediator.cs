// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月13日
// Update Time         :    2020年5月13日
// Class Description   :    UI_Win_MainTainMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using System;

namespace Game {
    public class UI_Win_MainTainMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_MainTainMediator";
        private Timer m_Timer1 = null;
        private Timer m_Timer2 = null;
        #endregion

        //IMediatorPlug needs
        public UI_Win_MainTainMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_MainTainView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
        }

        public override void WinClose(){
            m_Timer1?.Cancel();
            m_Timer2?.Cancel();
        }

        public override bool onMenuBackCallback()
        {
            SendNotification(CmdConstant.ExitGame);
            return true;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            MaintainType type = (MaintainType)view.data;

            view.m_UI_Model_Window_TypeMid.SetCloseVisible(false);

            var serverConfig = IGGSDK.appConfig.getServerConfig();
            if (type == MaintainType.ForceUpdate)
            {
                view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(100056));
                if (serverConfig.messages != null)
                {
                    view.m_lbl_languageText_LanguageText.text = serverConfig.messages.content.forceUpdate;
                }

                view.m_lbl_Tip_LanguageText.transform.parent.gameObject.SetActive(false);

                view.m_UI_reflash.SetText(LanguageUtils.getText(100059));
                view.m_UI_reflash.AddClickEvent(() =>
                {
                    Application.Quit();
                });

                view.m_UI_facebook.SetText(LanguageUtils.getText(100058));
                view.m_UI_facebook.AddClickEvent(() =>
                {
#if UNITY_ANDROID
                    IGGSDKUtils.shareInstance().OpenBrowser(serverConfig.update.googleUrl);
#else
                    IGGSDKUtils.shareInstance().OpenBrowser(serverConfig.update.appleUrl);
#endif
                });
            }
            else if (type == MaintainType.OptionalUpdate)
            {
                view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(100056));
                if (serverConfig.messages != null)
                {
                    view.m_lbl_languageText_LanguageText.text = serverConfig.messages.content.update;
                }

                view.m_lbl_Tip_LanguageText.transform.parent.gameObject.SetActive(false);

                view.m_UI_reflash.SetText(LanguageUtils.getText(100057));
                view.m_UI_reflash.AddClickEvent(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_Maintain);
                    SendNotification(CmdConstant.AutoLogin);
                });

                view.m_UI_facebook.SetText(LanguageUtils.getText(100058));
                view.m_UI_facebook.AddClickEvent(() =>
                {
#if UNITY_ANDROID
                    IGGSDKUtils.shareInstance().OpenBrowser(serverConfig.update.googleUrl);
#else
                    IGGSDKUtils.shareInstance().OpenBrowser(serverConfig.update.appleUrl);
#endif
                });
            }
            else if (type == MaintainType.Normal || type == MaintainType.NormalSingle)
            {
                view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(100050));
                if (serverConfig.messages != null)
                {
                    view.m_lbl_languageText_LanguageText.text = serverConfig.messages.content.maintain;
                }
                bool bEnter = false;
                var serverTime = IGGSDK.appConfig.serverTime;
                DateTime serverDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(serverTime).AddHours(-5);
                
                if (serverConfig.update.isMaintain.endAt <= serverDateTime)
                {
                    view.m_UI_reflash.SetText(LanguageUtils.getText(100054));
                    bEnter = true;
                }
                else
                {
                    view.m_UI_reflash.SetGray(true);
                    view.m_UI_reflash.SetText(LanguageUtils.getText(100053));
                    var refreshTime = Time.realtimeSinceStartup + CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0).maintainRefreshTime;
                    m_Timer2 = Timer.Register(1.0f, null, (float time) =>
                    {
                        if (refreshTime < Time.realtimeSinceStartup)
                        {
                            view.m_UI_reflash.SetText(LanguageUtils.getText(100053));
                            view.m_UI_reflash.SetGray(false);
                            m_Timer2.Cancel();
                        }
                        else
                        {
                            view.m_UI_reflash.SetText(LanguageUtils.getTextFormat(100138, (int)(refreshTime - Time.realtimeSinceStartup)));
                        }
                    }, true, view.m_lbl_languageText_LanguageText);
                }

                m_Timer1 = Timer.Register(1.0f, null, (float time) =>
                {
                    serverTime = IGGSDK.appConfig.serverTime;
                    serverDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(serverTime).AddHours(-5);
                    if (serverConfig.update.isMaintain.endAt <= serverDateTime)
                    {
                        view.m_UI_reflash.SetText(LanguageUtils.getText(100054));
                        view.m_lbl_Tip_LanguageText.text = LanguageUtils.getTextFormat(100052, "00:00:00");
                        bEnter = true;
                        m_Timer2?.Cancel();
                        view.m_UI_reflash.SetGray(false);
                    }
                    else
                    {
                        var offset = serverConfig.update.isMaintain.endAt - serverDateTime;
                        if (offset.Days > 0)
                        {
                            view.m_lbl_Tip_LanguageText.text = LanguageUtils.getTextFormat(100052, LanguageUtils.getTextFormat(300090, offset.Days.ToString("00"), offset.Hours.ToString("00"), offset.Minutes.ToString("00"), offset.Seconds.ToString("00")));
                        }
                        else
                        {
                            view.m_lbl_Tip_LanguageText.text = LanguageUtils.getTextFormat(100052, offset.ToString("hh\\:mm\\:ss"));
                        }
                    }

                }, true, view.m_lbl_languageText_LanguageText);

                view.m_UI_reflash.AddClickEvent(() =>
                {
                    //if(bEnter)
                    //{
                    //    CoreUtils.uiManager.CloseUI(UI.s_Maintain);
                    //    //if (type == MaintainType.NormalSingle)
                    //    //{
                    //    //    var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                    //    //    if (netProxy != null)
                    //    //    {
                    //    //        netProxy.RedirectGameServer();
                    //    //    }
                    //    //}
                    //    //else
                    //    {
                    //        SendNotification(CmdConstant.LoginToServer);
                    //    }
                    //}
                    //else
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_Maintain);
                        SendNotification(CmdConstant.LoadAppConfig);
                    }
                });

                view.m_UI_facebook.SetText(LanguageUtils.getText(100055));
                view.m_UI_facebook.AddClickEvent(() =>
                {
                    IGGSDKUtils.shareInstance().OpenBrowser(HotfixUtil.getLanguageLink(1));
                });
            }
        }

        protected override void BindUIEvent()
        {
            view.m_btn_service_GameButton.onClick.AddListener(OnServiceEvent);
        }

        protected override void BindUIData()
        {

        }
       
#endregion

        private void OnServiceEvent()
        {
            //暂时只有问题提交
            IGGURLBundle.shareInstance().serviceURL((exception, url) =>
            {
                if (exception.isNone())
                {
                    IGGSDKUtils.shareInstance().OpenBrowser(url);
                }
            });
        }
    }
}