// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    UI_Win_GuildWelcomeEditMediator
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
using Data;

namespace Game {
    public class UI_Win_GuildWelcomeEditMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildWelcomeEditMediator";


        private AllianceProxy m_allianceProxy;
        private bool m_isRequest;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildWelcomeEditMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildWelcomeEditView view;

      

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_GetGuildInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_GetGuildInfo.TagName:
                    Guild_GetGuildInfo.response response = notification.Body as Guild_GetGuildInfo.response;
                    if (response.type == 2 && m_isRequest)
                    {
                        view.gameObject.SetActive(true);
                        m_isRequest = false;
                        GuildInfoEntity info = m_allianceProxy.GetAlliance();
                        if (info != null)
                        {
                            if (info.welcomeEmailFlag) //修改联盟邮件
                            {
                                view.m_ipt_inputField_GameInput.text = info.welcomeEmail;
                            }
                            else
                            {
                                MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>(300000);
                                if (mailDefine != null)
                                {
                                    view.m_ipt_inputField_GameInput.text = LanguageUtils.getText(550163);
                                }
                            }
                        }
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

        private string m_preData;

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(onClose);
            view.m_btn_sure.m_btn_languageButton_GameButton.onClick.AddListener(onOK);

            m_preData = view.m_ipt_inputField_GameInput.text;
            
            view.m_ipt_inputField_GameInput.onValueChanged.AddListener(onTextChange);
            view.m_ipt_inputField_GameInput.characterLimit = 2000;

            view.m_ipt_inputField_GameInput.text = "";
            m_isRequest = true;
            Guild_GetGuildInfo.request request = new Guild_GetGuildInfo.request();
            request.type = 2;
            AppFacade.GetInstance().SendSproto(request);

            view.gameObject.SetActive(false);
        }

        private void onTextChange(string text)
        {
            view.m_lbl_wordsCount_LanguageText.text = LanguageUtils.getTextFormat(181104, text.Length, 2000);
        }

        private void onOK()
        {
            if (m_preData!=view.m_ipt_inputField_GameInput.text )
            {
                m_allianceProxy.SendEditAllianceInfo(3,view.m_ipt_inputField_GameInput.text,null);
            }
            
            
            onClose();
        }


        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceWelcomeEdit);
        }




        protected override void BindUIData()
        {
            
        }
       
        #endregion
    }
}