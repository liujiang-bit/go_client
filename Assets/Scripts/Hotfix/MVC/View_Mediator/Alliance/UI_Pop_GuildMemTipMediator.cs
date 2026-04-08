// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Pop_GuildMemTipMediator
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
    public class UI_Pop_GuildMemTipMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_GuildMemTipMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_GuildMemTipMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_GuildMemTipView view;

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
//            view.m_UI_mail.m_btn_languageButton_GameButton.onClick.AddListener(onMail);
//            view.m_UI_info.m_btn_languageButton_GameButton.onClick.AddListener(onPlayerInfo);
//            view.m_UI_help.m_btn_languageButton_GameButton.onClick.AddListener(onHelp);
//            view.m_UI_res_help.m_btn_languageButton_GameButton.onClick.AddListener(onResHelp);
//            view.m_UI_remove.m_btn_languageButton_GameButton.onClick.AddListener(onRemovePlayer);
//            view.m_UI_plus.m_btn_languageButton_GameButton.onClick.AddListener(onUpdateOffice);
            
        }

        private void onMail()
        {
            
        }
        private void onPlayerInfo()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerInfo);
        }
        private void onHelp()
        {
            
        }
        private void onResHelp()
        {
            
        }
        private void onRemovePlayer()
        {
           
            
            GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;
            Alert.CreateAlert(730164, LanguageUtils.getText(730163))
                .SetLeftButton(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceMemRemove);
                    
                },LanguageUtils.getText(730154)).SetRightButton(null,LanguageUtils.getText(730155)).Show();
        }
        private void onUpdateOffice()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceOffice);
        }
        

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}