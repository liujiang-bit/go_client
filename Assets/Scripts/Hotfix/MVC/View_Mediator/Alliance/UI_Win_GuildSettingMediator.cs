// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    UI_Win_GuildSettingMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Client.Utils;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_GuildSettingMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildSettingMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildSettingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildSettingView view;

        private int m_joinType = -1;
        private int m_language = 0;
        private bool isCreate;
        private bool isEditMail = false;

        private AllianceProxy _allianceProxy;
        private CurrencyProxy m_crrProxy;
        private PlayerProxy m_playerProxy;
        

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_CreateGuild.TagName,
                Guild_ModifyGuildInfo.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_CreateGuild.TagName:
                    
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;

                        switch ((ErrorCode)error.errorCode)
                        {
                            case ErrorCode.GUILD_ABBNAME_LENGTH_ERROR:
                                Tip.CreateTip(730039).Show();
                                break;
                            case ErrorCode.GUILD_ABBNAME_CHAR_ERROR:
                                Tip.CreateTip(730041).Show();
                                break;
                            case ErrorCode.GUILD_ABBNAME_REPEAT:
                                Tip.CreateTip(730040).Show();
                                break;
                            case ErrorCode.GUILD_ABBNAME_INVALID:
                                Tip.CreateTip(300128).Show();
                                break;
                            case ErrorCode.GUILD_NAME_LENGTH_ERROR:
                                Tip.CreateTip(730042).Show();
                                break;
                            case ErrorCode.GUILD_NAME_INVALID:
                                Tip.CreateTip(300128).Show();
                                break;
                            case ErrorCode.GUILD_NOTICE_LENGTH_ERROR:
                                Tip.CreateTip(730056).Show();
                                break;
                            case ErrorCode.GUILD_NOTICE_INVALID:
                                Tip.CreateTip(300128).Show();
                                break;
                            case ErrorCode.GUILD_ALREADY_IN_GUILD:
                                Tip.CreateTip(730343).Show();
                                break;
                        }

//                        
                    }
                    else
                    {
                        Guild_CreateGuild.response response = notification.Body as Guild_CreateGuild.response;

                        if (response.guildId>0)
                        {
                            onClose();

                            Tip.CreateTip(730074).Show();
                        
                            CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                        }
                    }

                   
                    break;
                case Guild_ModifyGuildInfo.TagName:
                {
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        
                    }
                    else
                    {
                        Guild_ModifyGuildInfo.response re = notification.Body as Guild_ModifyGuildInfo.response;

                        if (re!=null && re.type == 4)
                        {
                            onClose();
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
            _allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_crrProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        protected override void BindUIEvent()
        {

            isCreate = _allianceProxy.HasJionAlliance() == false;
            
            view.m_btn_change.gameObject.SetActive(!isCreate);
            
            view.m_btn_create.gameObject.SetActive(isCreate);
            
            view.m_btn_edit_GameButton.gameObject.SetActive(!isCreate);
            
            view.m_btn_left_GameButton.onClick.AddListener(NextType);
            view.m_btn_right_GameButton.onClick.AddListener(NextType);
            
            view.m_btn_language_GameButton.onClick.AddListener(onSelectLan);
            
            view.m_UI_BlueSelect.m_btn_languageButton_GameButton.onClick.AddListener(onSelectFlag);
            
            view.m_btn_edit_GameButton.onClick.AddListener(onWelcomeEdit);
            
            view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(onClose);
            
            //view.m_lbl_curLanguage_LanguageText.text = LanguageUtils.getText(300129);

            int language = (int)LanguageUtils.GetLanguage();
            if((language != (int)SystemLanguage.Arabic && language != (int)SystemLanguage.Turkish) || CoreUtils.dataService.QueryRecord<AllianceLanguageSetDefine>(language) == null)
            {
                language = 0;
            }
            setLan(language);

//            view.m_ipt_desc_GameInput.onEndEdit.AddListener(onDesChange);

            view.m_btn_sample_GameButton.onClick.AddListener(onSimpleName);
            view.m_btn_rename_GameButton.onClick.AddListener(onName);

            view.m_ipt_desc_GameInput.characterLimit = _allianceProxy.Config.allianceNoticeNum;
            
            if (isCreate==false)
            {
                var info = _allianceProxy.GetAlliance();
                
                BindAlliace(info);
                
                view.m_UI_GuildFlag.setData(info);
                    
                view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730188));
                view.m_lbl_title_LanguageText.text = LanguageUtils.getText(730223);
                
                view.m_btn_change.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730181);
                
                view.m_btn_change.m_btn_languageButton_GameButton.onClick.AddListener(onCreate);
                
                RefreshMailBtnRedDot();

//                view.m_btn_change.m_lbl_line2_LanguageText.text = _allianceProxy.Config.allianceEstablishCost.ToString();
            }
            else
            {
                NextType();
                
                view.m_UI_GuildFlag.RandomFlag();

                view.m_lbl_title_LanguageText.text = LanguageUtils.getText(730045);
                
                view.m_UI_BlueSelect.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730054);
                view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730044));
                view.m_btn_create.m_lbl_line1_LanguageText.text = LanguageUtils.getText(730055);

                view.m_btn_create.m_lbl_line2_LanguageText.text = _allianceProxy.Config.allianceEstablishCost.ToString();
                
                ClientUtils.UIReLayout(view.m_btn_create.m_btn_languageButton_GameButton);
                
                view.m_btn_create.m_btn_languageButton_GameButton.onClick.AddListener(onCreate);
            }
            
           
            
        }

        private void onName()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceNameEdit,null,1+"|"+isCreate);
        }


        private void onSimpleName()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceNameEdit,null,2+"|"+isCreate);
        }

        private void onDesChange(string text)
        {
//            if (text.Length>= _allianceProxy.Config.allianceNoticeNum)
//            {
//                Tip.CreateTip(730056, _allianceProxy.Config.allianceNoticeNum).Show();
//                view.m_ipt_desc_GameInput.text = text.Substring(0, _allianceProxy.Config.allianceNoticeNum);
//            }
        }

        private void BindAlliace(GuildInfoEntity infoEntity)
        {
            view.m_lbl_name_LanguageText.text = infoEntity.name;
            view.m_ipt_desc_GameInput.text = infoEntity.notice;
            view.m_lbl_sample_LanguageText.text = infoEntity.abbreviationName;
            
            setLan((int)infoEntity.languageId);

            if (infoEntity.needExamine)
            {
                m_joinType = 1;
            }
            else
            {
                m_joinType = 0;
            }

            SetType();
        }


        public void SetName(int type, string name)
        {
            if (type == 1)
            {
                view.m_lbl_name_LanguageText.text = name;
            }
            else
            {
                view.m_lbl_sample_LanguageText.text = name;
            }
        }

        public void setLan(int lang)
        {
            m_language = lang;

            
            var lanConfig =CoreUtils.dataService
                .QueryRecord<AllianceLanguageSetDefine>(m_language);

            if (lanConfig != null)
            {
                view.m_lbl_curLanguage_LanguageText.text = LanguageUtils.getText(lanConfig.l_languageID);
            }
            else
            {
                Debug.Log("AllianceLanguageSetDefine: null "+lang);
            }


        }

        private void RefreshMailBtnRedDot()
        {
            int value = PlayerPrefs.GetInt(string.Format($"{m_playerProxy.Rid}{_allianceProxy.GetAlliance().guildId}_GuildWelcome"));
            if (value == 0)
            {
                view.m_img_redpoint_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                view.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                isEditMail = true;
            }
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceCreateWin);
        }

        private void onWelcomeEdit()
        {
            if (!isEditMail)
            {
                PlayerPrefs.SetInt(string.Format($"{m_playerProxy.Rid}{_allianceProxy.GetAlliance().guildId}_GuildWelcome"), 1);
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceSettingWelcomeMailChange,1);
                RefreshMailBtnRedDot();
                isEditMail = true;
            }

            CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcomeEdit);
        }

        private void onCreate()
        {
//            if (view.m_ipt_desc_GameInput.text.Length>= _allianceProxy.Config.allianceNoticeNum)
//            {
//                Tip.CreateTip(730056, _allianceProxy.Config.allianceNoticeNum).Show();
//                return;
//            }

            if (BannedWord.ChackHasBannedWord(view.m_ipt_desc_GameInput.text))
            {

                Tip.CreateTip(300128).Show();
                return;
            }

            if (view.m_lbl_name_LanguageText.text.Length == 0)
            {
                Tip.CreateTip(730221,
                    _allianceProxy.Config.allianceNameMin, _allianceProxy.Config.allianceNameMax).Show();
                return;
            }
            
            if (view.m_lbl_sample_LanguageText.text.Length == 0)
            {
                Tip.CreateTip(730039,
                    _allianceProxy.Config.allianceAbbreviationMin, _allianceProxy.Config.allianceAbbreviationMax).Show();
                return;
            }

            if (isCreate)
            {
                if (!m_crrProxy.ShortOfDenar(_allianceProxy.Config.allianceEstablishCost))
                {
                    Guild_CreateGuild.request req = new Guild_CreateGuild.request();

                    req.name = view.m_lbl_name_LanguageText.BaseText.Trim();
                    req.notice = view.m_ipt_desc_GameInput.text;
                    req.abbreviationName = view.m_lbl_sample_LanguageText.BaseText;
                    req.needExamine = m_joinType == 1;
                    req.languageId = m_language;
                    req.signs = view.m_UI_GuildFlag.GetSigns();
                    _allianceProxy.SendCreateAlliance(req);

                }
                else
                {
                    CoreUtils.uiManager.ShowUI(UI.s_GemShort);
                }
            }
            else
            {
                _allianceProxy.SendEditAllianceInfo(4,view.m_ipt_desc_GameInput.text,null,m_joinType == 1,m_language);
            }


        }

        private void NextType()
        {
            m_joinType++;

            if (m_joinType>=2)
            {
                m_joinType = 0;
            }

            SetType();
        }

        private void SetType()
        {
            if (m_joinType == 0 )
            {
                view.m_lbl_mid_LanguageText.text = LanguageUtils.getText(730050);
            }
            else
            {
                view.m_lbl_mid_LanguageText.text = LanguageUtils.getText(730051);
            }
        }


        private void onSelectLan()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceLan,null,m_language);
        }

        private void onSelectFlag()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceFlag,null,view.m_UI_GuildFlag);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}