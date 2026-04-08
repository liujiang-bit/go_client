// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    UI_Win_GuildWelcomeMediator
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
    public class UI_Win_GuildWelcomeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildWelcomeMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildWelcomeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildWelcomeView view;

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
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AllianceJoinIntro);
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
            view.m_btn_join.m_btn_languageButton_GameButton.onClick.AddListener(onJoin);
            view.m_btn_creat.m_btn_languageButton_GameButton.onClick.AddListener(onCreate);
            view.m_UI_Model_Window_Type2.setWindowTitle(LanguageUtils.getText(730021));

            view.m_btn_join.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730026);
            
            view.m_btn_creat.m_lbl_text_LanguageText.text = LanguageUtils.getText(730027);
            
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(onClose);
        }


        private void onJoin()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceJionList);
            CoreUtils.uiManager.CloseUI(UI.s_AllianceWelcome);
        }
        
        private void onCreate()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceCreateWin,null,true);
            CoreUtils.uiManager.CloseUI(UI.s_AllianceWelcome);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceWelcome);
        }


        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}