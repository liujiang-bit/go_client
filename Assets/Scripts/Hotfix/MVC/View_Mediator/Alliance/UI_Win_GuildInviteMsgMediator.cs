// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_Win_GuildInviteMsgMediator
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
    public class UI_Win_GuildInviteMsgMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildInviteMsgMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildInviteMsgMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildInviteMsgView view;
        
        private AllianceProxy m_allianceProxy;

        private GuildInfo m_guildInfo;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.OnShowUI,
                Guild_ApplyJoinGuild.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnShowUI:
                    UIInfo info = notification.Body as UIInfo;

                    if (info!=UI.s_AllianceInviteTip && info != UI.s_fingerInfo)
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_AllianceInviteTip);
                        
                    }
                    
                    break;
                case Guild_ApplyJoinGuild.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        //错误码处理
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        if (error.errorCode == (long)ErrorCode.GUILD_NOT_EXIST)
                        {
                            Tip.CreateTip(730362).Show();
                        }
                        CoreUtils.uiManager.CloseUI(UI.s_AllianceInviteTip);
                        return;
                    }
                    Guild_ApplyJoinGuild.response response = notification.Body as Guild_ApplyJoinGuild.response;


                    if (response.type ==1)
                    {
                        Tip.CreateTip(730345).Show();
                    }
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceInviteTip);

                    
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

        public override void WinClose()
        {
            var md = AppFacade.GetInstance().RetrieveMediator(GlobalAllianceHelpMediator.NameMediator) as GlobalAllianceHelpMediator;
            md.ReTime();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_guildInfo = view.data as GuildInfo;
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_btn_close_GameButton.onClick.AddListener(onClose);
            view.m_UI_jion.m_btn_languageButton_GameButton.onClick.AddListener(onJion);
            
            ReUI(m_guildInfo);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceInviteTip);
        }

        private void onJion()
        {
            m_allianceProxy.SendJionAlliance(m_guildInfo.guildId);
            
            
        }


        private void ReUI(GuildInfo info)
        {
            view.m_lbl_name_LanguageText.text =  LanguageUtils.getTextFormat(300030, info.abbreviationName, info.name);

            var list = m_allianceProxy.Config.allianceRandomManifesto;
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(int.Parse(list[Random.Range(0, list.Count)]));
            
            view.m_UI_GuildFlag.setData(info);

            view.m_UI_PlayerHead.LoadHeadCountry((int)info.leaderHeadId);

        }

        #endregion
    }
}