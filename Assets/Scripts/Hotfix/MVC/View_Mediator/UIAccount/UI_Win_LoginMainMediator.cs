// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Win_LoginMainMediator
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
    public class UI_Win_LoginMainMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_LoginMainMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_LoginMainMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_LoginMainView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.LoginAccountFinished
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.LoginAccountFinished:
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_AccountLoginMain);
                    }
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus()
        {
        }

        public override void WinClose()
        {
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

        }

        protected override void BindUIEvent()
        {
            view.m_UI_MainLogin.AddClickEvent(() =>
            {
                SendNotification(CmdConstant.LoginAccount, view.data);
            });
            view.m_UI_OtherLogin.AddClickEvent(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_AccountLoginOther);
            });
        }

        protected override void BindUIData()
        {
            var loginType = (LoginType)view.data;
            if (loginType == LoginType.GOOGLE_PLAY)
            {
                view.m_UI_MainLogin.SetText(LanguageUtils.getText(100131));
            }
            else if (loginType == LoginType.GUEST)
            {
                view.m_UI_MainLogin.SetText(LanguageUtils.getText(100127));
            }
            else
            {
                view.m_UI_MainLogin.SetText(loginType.ToString());
            }

        }

        #endregion
    }
}