// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Win_PlayerChangeSureMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {

    public enum RoleInfoPanelType
    {
        None=0,
        Login, //老角色登录
        Create //新建
    }


    public class RoleInfoPanelData
    {
        public RoleInfoPanelType Type= RoleInfoPanelType.None;
        public int ServerId;
        public RoleInfoEntity m_RoleInfoEntity;
        public ServerListTypeDefine m_ServerListTypeDefine;

        public RoleInfoPanelData(RoleInfoPanelType type, int id)
        {
            this.Type = type;
            this.ServerId = id;
        }
    }


    public class UI_Win_PlayerChangeSureMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_PlayerChangeSureMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_PlayerChangeSureMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_PlayerChangeSureView view;
        private RoleInfoPanelData m_RoleInfoPanelData;
        private RoleInfoProxy m_RoleInfoProxy;
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
            m_RoleInfoProxy= AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            m_RoleInfoPanelData = view.data as RoleInfoPanelData;
            SetPanelData();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_PlayerChangeSure);
            });
            
            view.m_btn_cancel.AddBtnCancelClick(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_PlayerChangeSure);
            });
                        
            view.m_btn_sure.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                if (m_RoleInfoPanelData.Type == RoleInfoPanelType.Login)
                { 
                   // Debug.LogError("登录"+m_RoleInfoPanelData.m_RoleInfoEntity.rid);
                    m_RoleInfoProxy.SendRoleSetLastServerAndRole(m_RoleInfoPanelData.m_RoleInfoEntity.gameNode,m_RoleInfoPanelData.m_RoleInfoEntity.rid);
                }else if (m_RoleInfoPanelData.Type == RoleInfoPanelType.Create)
                {
                    if (m_RoleInfoProxy.GetIsCreateRole(m_RoleInfoPanelData.m_ServerListTypeDefine.ID))
                    {
                        string name = string.Format("{0}{1}", "game", m_RoleInfoPanelData.m_ServerListTypeDefine.severId);
                        m_RoleInfoProxy.SendRoleSetLastServerAndRole(name,0);
                       // Debug.LogError("创建角色"+name);
                    }
                    else
                    {
                        Tip.CreateTip(100520).Show(); 
                        ClosePanel();
                    }
                }
            });
        }

        protected override void BindUIData()
        {

        }

        private void SetPanelData()
        {
            if (m_RoleInfoPanelData == null)
            {
                return;
            }

            view.m_pl_view1.gameObject.SetActive(m_RoleInfoPanelData.Type == RoleInfoPanelType.Create);
            view.m_pl_view2.gameObject.SetActive(m_RoleInfoPanelData.Type == RoleInfoPanelType.Login);

            if (m_RoleInfoPanelData.Type == RoleInfoPanelType.Login)
            {
                view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(100523);
                view.m_lbl_kingdomNum_LanguageText.text = RoleInfoHelp.GetServerIdDes(m_RoleInfoPanelData.m_RoleInfoEntity.gameNode);
                view.m_lbl_kingdomName_LanguageText.text = RoleInfoHelp.GetServerNameId(m_RoleInfoPanelData.m_RoleInfoEntity.gameNode);

                string name = m_RoleInfoPanelData.m_RoleInfoEntity.name;
                if (!string.IsNullOrEmpty(m_RoleInfoPanelData.m_RoleInfoEntity.guildAbbName))
                {
                    name = string.Format("[{0}]{1}", m_RoleInfoPanelData.m_RoleInfoEntity.guildAbbName, m_RoleInfoPanelData.m_RoleInfoEntity.name);
                }

                view.m_lbl_playerName_LanguageText.text = name;
                view.m_UI_Model_PlayerHead.LoadPlayerIcon(m_RoleInfoPanelData.m_RoleInfoEntity.headId,m_RoleInfoPanelData.m_RoleInfoEntity.headFrameID);

            }else if (m_RoleInfoPanelData.Type == RoleInfoPanelType.Create)
            {
                view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(100522);
                view.m_lbl_kingdomNum_LanguageText.text = LanguageUtils.getTextFormat(100525, m_RoleInfoPanelData.m_ServerListTypeDefine.ID);
                if (m_RoleInfoPanelData.m_ServerListTypeDefine.serverNameId > 0)
                {                
                    view.m_lbl_kingdomName_LanguageText.text = LanguageUtils.getText(m_RoleInfoPanelData.m_ServerListTypeDefine.serverNameId);  
                }

            }
        }

        private void ClosePanel()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerChangeSure);
        }

        #endregion
    }
}