// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Win_AccountChangeMediator
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

namespace Game {
    public class UI_Win_AccountChangeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_AccountChangeMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_AccountChangeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_AccountChangeView view;

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
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Igg.AddClickEvent(() =>
            {
                SendNotification(CmdConstant.SwitchAccount, LoginType.GOOGLE_PLAY);
            });
            view.m_UI_Machine.AddClickEvent(() =>
            {
                SendNotification(CmdConstant.SwitchAccount, LoginType.GUEST);
            });

            view.m_UI_Model_Window_Type2.AddCloseEvent(()=>
            {
                CoreUtils.uiManager.CloseUI(UI.s_AccountSwitch);
            });
            view.m_UI_Model_Window_Type2.AddBackEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_AccountSwitch);
            });

            var loginType = SDKSession.currentSession.getLoginType();
            view.m_UI_Machine.SetEnabled(loginType != LoginType.GUEST);
        }

        protected override void BindUIData()
        {
            view.m_lbl_accountID_LanguageText.text = LanguageUtils.getTextFormat(100102, SDKSession.currentSession.getIGGId());
            var loginType = SDKSession.currentSession.getLoginType();
            if (loginType == LoginType.GOOGLE_PLAY)
            {
                view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, LanguageUtils.getText(100131));
            }
            else if (loginType == LoginType.GUEST)
            {
                view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, LanguageUtils.getText(100127));
            }
            else
            {
                view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, loginType.ToString());
            }
        }
       
        #endregion
    }
}