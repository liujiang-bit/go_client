// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Win_AccountMgrMediator
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
using IGGSDKConstant;

namespace Game {
    public class UI_Win_AccountMgrMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_AccountMgrMediator";
        private bool m_bHasLoadUserProfile = false;

        private AccountProxy m_accountProxy;

        #endregion

        //IMediatorPlug needs
        public UI_Win_AccountMgrMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_AccountMgrView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.BindindAccountFinished,
                CmdConstant.LoadAccountProfileFinished,
                CmdConstant.AccountBindReddotStatus
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.BindindAccountFinished:
                    {
                        UpdateAccountInfo();
                        m_accountProxy.SetBindReddotStatus(false);
                    }
                    break;
                case CmdConstant.LoadAccountProfileFinished:
                    {
                        m_bHasLoadUserProfile = true;
                        UpdateAccountInfo();
                    }
                    break;
                case CmdConstant.AccountBindReddotStatus:
                    RefreshIGGBindReddot();
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
            if (m_accountProxy.GetBindReddotStatus())
            {
                m_accountProxy.SetBindReddotStatus(false);
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_bHasLoadUserProfile = false;
            m_accountProxy = AppFacade.GetInstance().RetrieveProxy(AccountProxy.ProxyNAME) as AccountProxy;
            RefreshIGGBindReddot();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_BindingIGG.AddBindClickEvent(()=>
            {
                SendNotification(CmdConstant.BindindAccount, IGGLoginType.IGG_PASSPORT);
            });

            view.m_UI_Change.AddClickEvent(()=>
            {
                CoreUtils.uiManager.ShowUI(UI.s_AccountSwitch);
            });
            view.m_UI_Model_Window_Type2.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_AccountCenter);
            });
        }

        protected override void BindUIData()
        {
            UpdateAccountInfo();
            SendNotification(CmdConstant.LoadAccountProfile);
        }

        #endregion

        private void UpdateAccountInfo()
        {
            if (!m_bHasLoadUserProfile)
            {
                view.m_lbl_accountID_LanguageText.text = LanguageUtils.getTextFormat(100102, IGGSession.currentSession.getIGGId());
                var loginType = IGGSession.currentSession.getLoginType();
                if (loginType == IGGSDKConstant.IGGLoginType.IGG_PASSPORT)
                {
                    view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, LanguageUtils.getText(100131));
                }
                else if (loginType == IGGSDKConstant.IGGLoginType.GUEST)
                {
                    view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, LanguageUtils.getText(100127));
                }
                else
                {
                    view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, loginType.ToString());
                }
                view.m_lbl_safeLevel_LanguageText.transform.parent.gameObject.SetActive(false);
                view.m_UI_BindingMachine.gameObject.SetActive(false);
                view.m_UI_BindingIGG.gameObject.SetActive(false);
                return;
            }
            else
            {
                IGGUserProfile userProfile = IGGAccountManagementGuideline.shareInstance().getUserProfile();
                view.m_lbl_accountID_LanguageText.text = LanguageUtils.getTextFormat(100102, userProfile.getIGGID());
                var loginType = IGGSession.currentSession.getLoginType();
                //view.m_lbl_logName_LanguageText.text = userProfile.getLoginType();
                if (loginType == IGGSDKConstant.IGGLoginType.IGG_PASSPORT)
                {
                    view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, LanguageUtils.getText(100131));
                }
                else if (loginType == IGGSDKConstant.IGGLoginType.GUEST)
                {
                    view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, LanguageUtils.getText(100127));
                }
                else
                {
                    view.m_lbl_logName_LanguageText.text = LanguageUtils.getTextFormat(100103, loginType.ToString());
                }
                view.m_UI_BindingMachine.gameObject.SetActive(true);
                view.m_UI_BindingIGG.gameObject.SetActive(true);

                bool bShowSafe = true;
                var profile = userProfile.getBindingProfile(IGGLoginType.GUEST);

                if (userProfile.getDeviceBindState() == GuestBindState.BIND_CURRENT_DEVICE)
                {
                    view.m_UI_BindingMachine.SetBindInfo(true, LanguageUtils.getText(100106));
                }
                else if (userProfile.getDeviceBindState() == GuestBindState.BIND_NO_CURRENT_DEVICE)
                {
                    view.m_UI_BindingMachine.SetBindInfo(true, LanguageUtils.getText(100107));
                }
                else
                {
                    view.m_UI_BindingMachine.SetBindInfo(true, LanguageUtils.getText(570029));
                }
                profile = userProfile.getBindingProfile(IGGLoginType.IGG_PASSPORT);
                if (profile != null && profile.isHasBound())
                {
                    view.m_UI_BindingIGG.SetBindInfo(true, profile.getDisplayName());
                    bShowSafe = false;
                }
                else
                {
                    view.m_UI_BindingIGG.SetBindInfo(false);
                }

                view.m_lbl_safeLevel_LanguageText.transform.parent.gameObject.SetActive(bShowSafe);
            }
        }

        private void RefreshIGGBindReddot()
        {
            view.m_UI_BindingIGG.m_UI_Binding.m_img_redpoint_PolygonImage.gameObject.SetActive(m_accountProxy.GetBindReddotStatus());
        }
    }
}