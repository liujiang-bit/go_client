// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Win_GuildMemUpgradeMediator
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
    public class UI_Win_GuildMemUpgradeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildMemUpgradeMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildMemUpgradeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildMemUpgradeView view;

        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_ModifyMemberLevel.TagName,
                Guild_GuildMemberInfo.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_ModifyMemberLevel.TagName:
                case Guild_GuildMemberInfo.TagName:
                    
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMemUpgrade);
                    
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
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        private int m_selectedLv = 0;

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(730179));
            view.m_UI_Model_Window_TypeMid.setCloseHandle(onClose);
            
            view.m_UI_sure.m_btn_languageButton_GameButton.onClick.AddListener(onOK);
            
            
            view.m_ck_r1_GameToggle.onValueChanged.AddListener(onR1);
            view.m_ck_r2_GameToggle.onValueChanged.AddListener(onR2);
            view.m_ck_r3_GameToggle.onValueChanged.AddListener(onR3);
            view.m_ck_r4_GameToggle.onValueChanged.AddListener(onR4);
            view.m_ck_r5_GameToggle.onValueChanged.AddListener(onR5);
            
            view.m_ck_r5_GameToggle.gameObject.SetActive(m_allianceProxy.GetSelfRoot(GuildRoot.endGuild));
            
            GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;

            m_selectedLv =(int)player.guildJob;


            switch (m_selectedLv)
            {
                case 1:
                    view.m_ck_r1_GameToggle.isOn = true;
                    break;
                case 2:
                    view.m_ck_r2_GameToggle.isOn = true;
                    break;
                case 3:
                    view.m_ck_r3_GameToggle.isOn = true;
                    break;
                case 4:
                    view.m_ck_r4_GameToggle.isOn = true;
                    break;
                case 5:
                    view.m_ck_r5_GameToggle.isOn = true;
                    break;
                
            }
        }

        private void checkBtns()
        {
//            GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;
//            view.m_UI_sure.m_btn_languageButton_GameButton.interactable = player.guildJob != m_selectedLv;
        }

        private void onR1(bool v)
        {
            if (v)
            {
                m_selectedLv = 1;
//                checkBtns();
            }
        }
        
        private void onR2(bool v)
        {
            if (v)
            {
                m_selectedLv = 2;
//                checkBtns();
            }
        }
        private void onR3(bool v)
        {
            if (v)
            {
                m_selectedLv = 3;
//                checkBtns();
            }
        }
        private void onR4(bool v)
        {
            if (v)
            {
                m_selectedLv = 4;
//                checkBtns();
            }
        }
        private void onR5(bool v)
        {
            if (v)
            {
                m_selectedLv = 5;
//                checkBtns();
            }
        }
      

        private void onOK()
        {

            GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;

            if (player.guildJob == m_selectedLv)
            {
                Tip.CreateTip(730326, "R"+m_selectedLv).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            
            if (m_selectedLv == 5)
            {
                MinimapProxy minimapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
                if (!minimapProxy.MemberPos.ContainsKey(player.rid))
                {
                    Tip.CreateTip(184032, Tip.TipStyle.Middle).Show();
                    return;
                }
                Alert.CreateAlert(LanguageUtils.getTextFormat(730150,player.name), LanguageUtils.getText(730185)).
                    SetLeftButton(SendServer,LanguageUtils.getText(730154)).SetRightButton(null,LanguageUtils.getText(730155)).Show();
            }
            else
            {
                SendServer();
            }

           
        }

        private void SendServer()
        {

            var self = m_allianceProxy.getMemberInfo(m_playerProxy.CurrentRoleInfo.rid);
            GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;


            if (m_selectedLv == 4)
            {
                var members = m_allianceProxy.getAllianceMembers();

                if (members.Count>0 && members[0].LevelMembers.Count == 8)
                {
                     Tip.CreateTip(730217).Show();
                     return;
                }
            }
            
            
            if (self.guildJob<=4 && self.guildJob<=m_selectedLv)
            {
                Tip.CreateTip(730149).Show();
            }
            else
            
            {
                m_allianceProxy.SendMemberLvevelChange(player.rid,m_selectedLv);
            }

            
            
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMemUpgrade);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}