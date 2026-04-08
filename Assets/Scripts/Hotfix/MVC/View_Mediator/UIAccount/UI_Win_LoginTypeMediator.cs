// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Win_LoginTypeMediator
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
    public class UI_Win_LoginTypeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_LoginTypeMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_LoginTypeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_LoginTypeView view;

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
                        CoreUtils.uiManager.CloseUI(UI.s_AccountLoginOther);
                    }
                    break;
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
                SendNotification(CmdConstant.LoginAccount, IGGSDKConstant.IGGLoginType.IGG_PASSPORT);
            });
            view.m_UI_Machine.AddClickEvent(() =>
            {
                SendNotification(CmdConstant.LoginAccount, IGGSDKConstant.IGGLoginType.GUEST);
            });
            view.m_UI_Model_Window_TypeMid.setCloseHandle(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_AccountLoginOther);
            });
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}