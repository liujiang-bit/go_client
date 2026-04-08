// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, April 17, 2020
// Update Time         :    Friday, April 17, 2020
// Class Description   :    UI_Win_GuildDissolutionMediator
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
    public class UI_Win_GuildDissolutionMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildDissolutionMediator";

        private AllianceProxy m_allianceProxy;
        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildDissolutionMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildDissolutionView view;

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
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.setCloseHandle(onClose);
            
            view.m_btn_dissolution.m_btn_languageButton_GameButton.onClick.AddListener(onOK);
            view.m_btn_dissolution.m_btn_languageButton_GameButton.interactable = false;
            view.m_btn_cancel.m_btn_languageButton_GameButton.onClick.AddListener(onClose);
            
            view.m_ipt_mes_GameInput.onEndEdit.AddListener(onEditEnd);
            view.m_ipt_mes_GameInput.onValueChanged.AddListener(onEditEnd);

            view.m_ipt_mes_GameInput.characterLimit = "DELETE".Length;
        }

        private void onEditEnd(string text)
        {
            
            view.m_btn_dissolution.m_btn_languageButton_GameButton.interactable = view.m_ipt_mes_GameInput.text.Trim()=="DELETE";
            
        }

        private void onOK()
        {
            if (view.m_ipt_mes_GameInput.text.Trim()=="DELETE")
            {
                m_allianceProxy.SendExitGuild(2);
                onClose();
            }
            
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceExitConfirm);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}