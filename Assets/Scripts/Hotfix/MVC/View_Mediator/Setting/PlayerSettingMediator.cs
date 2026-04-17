// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    PlayerSettingMediator
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
using System;

namespace Game {
    public class PlayerSettingMediator : GameMediator {
        #region Member
        public static string NameMediator = "PlayerSettingMediator";

        private SDKAgreementSigningFile m_signingFile;
        private SDKAssignedAgreements m_assignedAgreements;
        private List<UI_Item_PlayerDataBtn_SubView> m_agreementBtnList = new List<UI_Item_PlayerDataBtn_SubView>();
        private RoleInfoProxy m_RoleInfoProxy;

        #endregion

        //IMediatorPlug needs
        public PlayerSettingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public PlayerSettingView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AccountBindReddotStatus,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AccountBindReddotStatus:
                    RefreshPlayerSettingReddot();
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
            m_RoleInfoProxy= AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            //view.m_UI_Model_Link.gameObject.SetActive(true);
            view.m_lbl_version_LanguageText.text = LanguageUtils.getTextFormat(100007, VersionUtil.GetVersionStr());

            RefreshTime();
            Timer.Register(1f,()=> {
                RefreshTime();
            }, null, true, false, view.m_lbl_date_LanguageText);

            m_agreementBtnList.Add(view.m_UI_Item_PlayerDataBtn10);
            m_agreementBtnList.Add(view.m_UI_Item_PlayerDataBtn12);
            for (int i = 0; i < m_agreementBtnList.Count; i++)
            {
                m_agreementBtnList[i].gameObject.SetActive(false);
            }
            if (!HotfixUtil.IsShowLoginView())
            {
                SDKBridge.shareInstance().getAgreementSigning().getAssignedAgreements(OnSDKAssignedAgreementsLoad);
            }
            m_RoleInfoProxy?.SendRoleInfo();
        }

        private void RefreshTime()
        {
            view.m_lbl_date_LanguageText.text = UIHelper.GetAstTimeFormat(ServerTimeModule.Instance.GetServerTime());
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(onClose);
            view.m_UI_Item_PlayerDataBtn.AddClickEvent(onLanguageClick);
            view.m_UI_Item_PlayerDataBtn3.AddClickEvent(OnGeneralSettingClick);
            view.m_UI_Item_PlayerDataBtn4.AddClickEvent(OnClickNoticeSetting);
            view.m_UI_Item_PlayerDataBtn7.AddClickEvent(OnCustomerService);
            view.m_UI_Item_PlayerDataBtn6.AddClickEvent(OnAccountClick);
            view.m_UI_Item_PlayerDataBtn8.AddClickEvent(OnClickCommunity);
            
            view.m_UI_Item_PlayerDataBtn5.AddClickEvent(OnClickRoleMgr);
            //view.m_UI_Model_Link.AddClickEvent(OnViewAgreement);

            RefreshPlayerSettingReddot();
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_Setting);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void RefreshPlayerSettingReddot()
        {
            var accountProxy = AppFacade.GetInstance().RetrieveProxy(AccountProxy.ProxyNAME) as AccountProxy;
            view.m_UI_Item_PlayerDataBtn6.m_UI_Common_Redpoint.gameObject.SetActive(accountProxy.GetBindReddotStatus());
        }

        private void onLanguageClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            CoreUtils.uiManager.ShowUI(UI.s_Pop_LanguageSet, null, LanguageSetMediator.OpenType.Setting);
        }

        private void OnGeneralSettingClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_generalSetting);
        }

        public void OnCustomerService()
        {
            CoreUtils.uiManager.ShowUI(UI.s_helper);
        }
        private void OnAccountClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AccountCenter);
        }
        //private void OnViewAgreement()
        //{
        //    IGGSDK.shareInstance().getAgreementSigning().requestAssignedAgreements(OnSDKAssignedAgreementsLoad);
        //}
        void OnSDKAssignedAgreementsLoad(SDKException exception, SDKAssignedAgreements agreements)
        {
            if (view.gameObject == null)
            {
                return;
            }
            if (exception == null || agreements == null)
            {
                return;
            }
            if (exception.isNone())
            {
                //CoreUtils.uiManager.ShowUI(UI.s_Agreement, null, agreements);

                List<SDKAgreement> agrees = agreements.getAgreements();
                if (agrees == null)
                {
                    Debug.Log("OnSDKAssignedAgreementsLoad return null");
                    return;
                }
                Debug.LogFormat("OnSDKAssignedAgreementsLoad count:{0}", agrees.Count);
                if (agrees.Count > 0)
                {
                    int count = m_agreementBtnList.Count;
                    for (int i = 0; i < agrees.Count; i++)
                    {
                        if (i < count)
                        {
                            m_agreementBtnList[i].gameObject.SetActive(true);
                            m_agreementBtnList[i].SetAgreement(agrees[i]);
                        }
                    }
                }
            }
            else
            {
                SDKUtils.ShowToast(LanguageUtils.getTextFormat(100125, exception.getCode()));
            }
        }

        private void OnClickCommunity()
        {
            CoreUtils.uiManager.ShowUI(UI.s_community);
        }

        private void OnClickNoticeSetting()
        {
            CoreUtils.uiManager.ShowUI(UI.s_NoticeSetting);
        }

        private void OnClickRoleMgr()
        {

            CoreUtils.uiManager.ShowUI(UI.s_PlayerManage);
        }
    }
}