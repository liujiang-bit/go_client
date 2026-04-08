// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Win_GuildMemRemoveMediator
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
using Hotfix.Utils;

namespace Game {
    public class UI_Win_GuildMemRemoveMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildMemRemoveMediator";
        private AllianceProxy m_allianceProxy;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildMemRemoveMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildMemRemoveView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_GuildNotify.TagName,
                Guild_KickMember.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_GuildNotify.TagName:
                    
                    onClose();
                    break;
                case Guild_KickMember.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        //错误码处理
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        onClose();
                        return;
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
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }

        private int m_selectedRes = 0;

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.setWindowTitle(LanguageUtils.getText(730163));
            view.m_UI_Model_Window_Type2.setCloseHandle(onClose);
            
            view.m_UI_Blue.m_btn_languageButton_GameButton.onClick.AddListener(onOK);
            
            view.m_UI_Red.m_btn_languageButton_GameButton.onClick.AddListener(onClose);
            
            view.m_ck_box1_GameToggle.onValueChanged.AddListener(onR1);
            view.m_ck_box2_GameToggle.onValueChanged.AddListener(onR2);
            view.m_ck_box3_GameToggle.onValueChanged.AddListener(onR3);
            view.m_ck_box4_GameToggle.onValueChanged.AddListener(onR4);
            view.m_ck_box5_GameToggle.onValueChanged.AddListener(onR5);
            view.m_ck_box6_GameToggle.onValueChanged.AddListener(onR6);
        }
        
        private void onR1(bool v)
        {
            if (v)
            {
                m_selectedRes = 1;
            }
        }
        
        private void onR2(bool v)
        {
            if (v)
            {
                m_selectedRes = 2;
            }
        }
        private void onR3(bool v)
        {
            if (v)
            {
                m_selectedRes = 3;
            }
        }
        private void onR4(bool v)
        {
            if (v)
            {
                m_selectedRes = 4;
            }
        }
        private void onR5(bool v)
        {
            if (v)
            {
                m_selectedRes = 5;
            }
        }
        
        private void onR6(bool v)
        {
            if (v)
            {
                m_selectedRes = 6;
            }
        }

        protected override void BindUIData()
        {

        }
        
        
        
        
        private void onOK()
        {

            if (m_selectedRes>0)
            {
                view.m_UI_Blue.m_btn_languageButton_GameButton.interactable = false;
                GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;
              
                m_allianceProxy.SendKickMember(player.rid,m_selectedRes);
                    
            }
           
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMemRemove);
        }
       
        #endregion
    }
}