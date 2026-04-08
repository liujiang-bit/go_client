// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_Win_GuildViewMediator
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
    public class UI_Win_GuildViewMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildViewMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildViewMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildViewView view;
        
        private long m_guildID;

        private GuildInfo m_guildInfo;
        
       
        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_GetGuildInfo.TagName,
                Guild_GetOtherGuildInfo.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_GetGuildInfo.TagName:
//                    GuildInfo info = 
//                    ReUI();
                    break;
                
                case Guild_GetOtherGuildInfo.TagName:

                    Guild_GetOtherGuildInfo.response response =
                        notification.Body as Guild_GetOtherGuildInfo.response;
                    m_guildInfo = response.guildInfo;
                    ReUI(m_guildInfo);
                    break;
                default:
                    break;
            }
        }

        private void ReUI(GuildInfo info)
        {
            m_guildInfo = info;
            m_guildID = info.guildId;
            view.m_lbl_guildName_LanguageText.text = LanguageUtils.getTextFormat(300030, info.abbreviationName, info.name); 

            view.m_lbl_power_LanguageText.text =
                LanguageUtils.getTextFormat(300233, ClientUtils.FormatComma(info.power));
            
            view.m_lbl_master_LanguageText.text = LanguageUtils.getTextFormat(730077, info.leaderName);

            view.m_lbl_terrtroy_LanguageText.text = LanguageUtils.getTextFormat(730078, info.territory);

            var strLevel = LanguageUtils.getTextFormat(180306, info.giftLevel);
            view.m_lbl_gift_LanguageText.text = LanguageUtils.getTextFormat(730079, strLevel);

            view.m_lbl_member_LanguageText.text = LanguageUtils.getTextFormat(730080, info.memberNum, info.memberLimit);

            view.m_lbl_desc_LanguageText.text = info.notice;
            
            view.m_UI_flag.setData(info);
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
            
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730021));
            
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);
            
            view.m_UI_btn_menber.m_btn_languageButton_GameButton.onClick.AddListener(onMember);
            view.m_UI_btn_comment.m_btn_languageButton_GameButton.onClick.AddListener(onComment);
            view.m_UI_btn_mail.m_btn_languageButton_GameButton.onClick.AddListener(onMail);
            
            view.m_btn_trans_GameButton.onClick.AddListener(onTrans);

            if (view.data is GuildInfo)
            {
                ReUI(view.data as GuildInfo);
            }else if (view.data is long)
            {
                SendGetGuildInfo((long) view.data);
            }else if(view.data is int)
            {
                SendGetGuildInfo((long) view.data);
            }
        }
        
        private void SendGetGuildInfo(long guildID)
        {
            m_guildID = guildID;
            Guild_GetOtherGuildInfo.request request = new Guild_GetOtherGuildInfo.request();

            request.guildId = guildID;

            AppFacade.GetInstance().SendSproto(request);
        }
        

        private void onTrans()
        {
            if (view.m_lbl_desc_LanguageText.text.Length>0)
            {
                ClientUtils.TranslatorSDK(view.m_lbl_desc_LanguageText);
            }
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceInfo);
        }

        private void onMember()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceMember,null,m_guildInfo);
        }

        private void onComment()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceMsg,null,m_guildInfo);
        }

        private void onMail()
        {
            WriteAMailData mailData = new WriteAMailData();

            mailData.stableName = m_guildInfo.leaderName;
            mailData.stableRid = m_guildInfo.leaderRid;
                   
            CoreUtils.uiManager.ShowUI(UI.s_writeAMail,null,mailData);
        }

        #endregion
    }
}