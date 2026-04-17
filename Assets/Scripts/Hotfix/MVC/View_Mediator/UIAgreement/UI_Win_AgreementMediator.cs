// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Win_AgreementMediator
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
    public class UI_Win_AgreementMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_AgreementMediator";

        SDKAgreementSigningFile m_signingFile;
        SDKAssignedAgreements m_assignedAgreements;

        #endregion

        //IMediatorPlug needs
        public UI_Win_AgreementMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_AgreementView view;

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

        public override void WinFocus()
        {
            // 登陆的协议列表不可以返回关闭
            if (m_signingFile != null)
            {
            }
        }

        public override void WinClose()
        {
            // 登陆的协议列表不可以返回关闭
            if (m_signingFile != null)
            {
            }
        }
        public override bool onMenuBackCallback()
        {
            if (m_signingFile != null)
            {
                SendNotification(CmdConstant.ExitGame);
                return true;
            }
            return false;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_signingFile = view.data as SDKAgreementSigningFile;
            m_assignedAgreements = view.data as SDKAssignedAgreements;
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            List<SDKAgreement> agrees = null;
            if (m_signingFile != null)
            {
                view.m_lbl_Content_LanguageText.text = m_signingFile.getLocalizedCaption();
                view.m_UI_btnOK.m_lbl_Text_LanguageText.text = m_signingFile.getLocalizedActionSign();
                view.m_lbl_title_LanguageText.text = m_signingFile.getLocalizedTitle();

                view.m_UI_btnOK.AddClickEvent(() =>
                {
                    var signing = SDKBridge.shareInstance().getAgreementSigning();
                    signing.sign(m_signingFile, (SDKException ex) =>
                    {
                        if (ex.isNone())
                        {
                            SendNotification(CmdConstant.HotfixUpteCheck);
                            CoreUtils.uiManager.CloseUI(UI.s_Agreement);
                        }
                    });
                });
                agrees = m_signingFile.getAgreements();
            }
            else
            {
                agrees = m_assignedAgreements.getAgreements();
                view.m_UI_btnOK.AddClickEvent(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_Agreement);
                });
            }

            while(agrees.Count > view.m_pl_links_GridLayoutGroup.transform.childCount)
            {
                Object.Instantiate(view.m_UI_Item_AgreementLink.gameObject, view.m_pl_links_GridLayoutGroup.transform);
            }
            if(agrees.Count == 0)
            {
                GameObject.DestroyImmediate(view.m_UI_Item_AgreementLink.gameObject);
            }
            for(int i = 0; i < view.m_pl_links_GridLayoutGroup.transform.childCount; i++)
            {
                var link = new UI_Item_AgreementLink_SubView(view.m_pl_links_GridLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>());
                link.SetAgreement(agrees[i]);
            }
        }
       
        #endregion
    }
}