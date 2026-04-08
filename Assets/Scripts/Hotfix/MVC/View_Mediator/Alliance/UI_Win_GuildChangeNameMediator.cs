// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    UI_Win_GuildChangeNameMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Client.Utils;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_GuildChangeNameMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildChangeNameMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildChangeNameMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildChangeNameView view;
        
        private AllianceProxy m_allianceProxy;

        private UI_Win_GuildSettingMediator _mediator;
        
        private CurrencyProxy m_crrProxy;

        private int m_type;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_CheckGuildName.TagName,
                Guild_ModifyGuildInfo.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_CheckGuildName.TagName:
                {
                    Guild_CheckGuildName.response response = notification.Body as Guild_CheckGuildName.response;

                    if (response.type == m_type)
                    {
                        if (response.result == false)
                        {
                            hasError = true;

                            if (m_type == 1)
                            {
                                view.m_lbl_error_LanguageText.text = LanguageUtils.getText(730043);
                            }
                            else
                            {
                                view.m_lbl_error_LanguageText.text = LanguageUtils.getText(730040);
                            }

                            checkButtom();
                        }
                    }
                }

                    break;
                case Guild_ModifyGuildInfo.TagName:
                {
                    if (notification.Body!=null)
                    {
                        Guild_ModifyGuildInfo.response response = notification.Body as Guild_ModifyGuildInfo.response;
                        if (response!=null && response.HasType && response.type >0)
                        {
                            onOk();
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

        private bool m_isCreateMode = false;
        private ConfigDefine m_config;
        private int m_changeCost;

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            _mediator =
                AppFacade.GetInstance().RetrieveMediator(UI_Win_GuildSettingMediator.NameMediator) as
                    UI_Win_GuildSettingMediator;
            
            string data = (string)view.data;
            string[] datasp = data.Split('|');
            m_type = int.Parse(datasp[0]);

            m_isCreateMode = m_allianceProxy.HasJionAlliance()==false;

            m_config = m_allianceProxy.Config;

            var allInfo = m_allianceProxy.GetAlliance();
            
            m_crrProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            if (m_type==1)
            {
                if (m_isCreateMode ==false)
                {
                    view.m_ipt_name_GameInput.text = allInfo.name;
                    m_changeCost = m_config.allianceAbbreviationAmend;
                    view.m_btn_change.m_lbl_line2_LanguageText.text = m_config.allianceAbbreviationAmend.ToString();
                    ClientUtils.UIReLayout(view.m_btn_change.m_btn_languageButton_GameButton);
                }
            }
            else
            {
                if (m_isCreateMode ==false)
                {
                    view.m_ipt_name_GameInput.text = allInfo.abbreviationName;
                    m_changeCost = m_config.allianceNameAmend;
                    view.m_btn_change.m_lbl_line2_LanguageText.text = m_config.allianceNameAmend.ToString();
                    ClientUtils.UIReLayout(view.m_btn_change.m_btn_languageButton_GameButton);
                }
            }


            onTextChange(view.m_ipt_name_GameInput.text);
        }

        protected override void BindUIEvent()
        {
            view.m_ipt_name_GameInput.onValueChanged.AddListener(onTextChange);
            
            view.m_ipt_name_GameInput.onEndEdit.AddListener(onEndEdit);
            
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(onClose);
            
            view.m_UI_BlueSure.m_btn_languageButton_GameButton.onClick.AddListener(onOk);
            view.m_btn_change.m_btn_languageButton_GameButton.onClick.AddListener(onChangeName);
            
            
            view.m_UI_BlueSure.gameObject.SetActive(m_isCreateMode);
            view.m_btn_change.gameObject.SetActive(!m_isCreateMode);
        }

        private void onEndEdit(string text)
        {
            if (hasError == false)
            {
                m_allianceProxy.SendCheckRename((long)m_type,text);
            }
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceNameEdit);
        }

        protected override void BindUIData()
        {
            if (m_type == 1)
            {
                view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(730046);
                view.m_lbl_des_LanguageText.text = LanguageUtils.getTextFormat(730047,m_allianceProxy.Config.allianceNameMin,m_allianceProxy.Config.allianceNameMax);
                view.m_ipt_name_GameInput.characterLimit = m_allianceProxy.Config.allianceNameMax;
            }
            else
            {
                view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(730034);
                view.m_lbl_des_LanguageText.text = LanguageUtils.getTextFormat(730037,m_allianceProxy.Config.allianceAbbreviationMin,m_allianceProxy.Config.allianceAbbreviationMax);

                view.m_ipt_name_GameInput.characterLimit = m_allianceProxy.Config.allianceAbbreviationMax;
            }
            
            onTextChange(view.m_ipt_name_GameInput.text);
        }

        private void onChangeName()
        {
            if (BannedWord.ChackHasBannedWord(view.m_ipt_name_GameInput.text))
            {
                Tip.CreateTip(300128).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }

            if (!m_crrProxy.ShortOfDenar(m_changeCost))
            {
                m_allianceProxy.SendEditAllianceInfo(m_type==1?2:1,view.m_ipt_name_GameInput.text,null);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_GemShort);
            }
        }

        private void onOk()
        {

            if (BannedWord.ChackHasBannedWord(view.m_ipt_name_GameInput.text))
            {
                Tip.CreateTip(300128).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            _mediator.SetName(m_type,view.m_ipt_name_GameInput.text);
            CoreUtils.uiManager.CloseUI(UI.s_AllianceNameEdit);
        }

        private bool hasError;
        private int rem = 0;
        
        private Regex reg = new Regex(@"[^a-zA-Z0-9_~!@#&=:<>/\^\-]+");//
        
        private Regex regFullname = new Regex(@"[^\w_~!@#&=:<>/\^\-\ ]+");//

        private void onTextChange(string text)
        {
            
            Debug.Log(text.Length+" "+text);
            rem = 0;
            hasError = false;
            if (m_type == 1)
            {
                rem = m_allianceProxy.Config.allianceNameMax - text.Length;
                
                if ( regFullname.IsMatch(text))
                {
                    view.m_lbl_error_LanguageText.text = LanguageUtils.getTextFormat(730221,
                        m_allianceProxy.Config.allianceNameMin, m_allianceProxy.Config.allianceNameMax);
                    hasError = true;
                }

                if (!hasError && text.Length < m_allianceProxy.Config.allianceNameMin ||
                    text.Length > m_allianceProxy.Config.allianceNameMax)
                {
                    view.m_lbl_error_LanguageText.text = LanguageUtils.getTextFormat(730042,
                        m_allianceProxy.Config.allianceNameMin, m_allianceProxy.Config.allianceNameMax);
                    hasError = true;
                }

                if (!hasError && BannedWord.ChackHasBannedWord(text))
                {
                    view.m_lbl_error_LanguageText.text = LanguageUtils.getText(300128);
                    hasError = true;
                }

                if (rem >= 0)
                {
                    view.m_lbl_textCount_LanguageText.text = LanguageUtils.getTextFormat(730035, rem);
                }
            }
            else
            {
                rem = m_allianceProxy.Config.allianceAbbreviationMax - text.Length;

                if (reg.IsMatch(text))
                {
                    view.m_lbl_error_LanguageText.text = LanguageUtils.getTextFormat(730041,
                        m_allianceProxy.Config.allianceAbbreviationMin, m_allianceProxy.Config.allianceAbbreviationMax);
                    hasError = true;
                }
                
                if (!hasError && text.Length < m_allianceProxy.Config.allianceAbbreviationMin ||
                    text.Length > m_allianceProxy.Config.allianceAbbreviationMax)
                {
                    view.m_lbl_error_LanguageText.text = LanguageUtils.getTextFormat(730039,
                        m_allianceProxy.Config.allianceAbbreviationMin, m_allianceProxy.Config.allianceAbbreviationMax);
                    hasError = true;
                }

                if (rem >= 0)
                {
                    view.m_lbl_textCount_LanguageText.text = LanguageUtils.getTextFormat(730035, rem);
                }
                
            }
            
            checkButtom();
        }


        private void checkButtom()
        {
            
            

            if (m_isCreateMode==false)
            {
                var allInfo = m_allianceProxy.GetAlliance();
                if (m_type == 1)
                {
                    view.m_btn_change.m_btn_languageButton_GameButton.interactable =
                        view.m_ipt_name_GameInput.text !=allInfo .name;
                }
                else
                {
                    view.m_btn_change.m_btn_languageButton_GameButton.interactable =
                        view.m_ipt_name_GameInput.text != allInfo.abbreviationName;
                }
                view.m_btn_change.m_btn_languageButton_GameButton.interactable = rem>=0 && hasError==false;
                
            }
            else
            {
                view.m_UI_BlueSure.m_btn_languageButton_GameButton.interactable = rem>=0 && hasError==false;
            }


            view.m_lbl_error_LanguageText.gameObject.SetActive(hasError);

            
            view.m_img_redMark_PolygonImage.gameObject.SetActive(hasError && view.m_ipt_name_GameInput.text.Length>0);
            view.m_img_greenMark_PolygonImage.gameObject.SetActive(!hasError&& view.m_ipt_name_GameInput.text.Length>0);
        }

        #endregion
    }
}