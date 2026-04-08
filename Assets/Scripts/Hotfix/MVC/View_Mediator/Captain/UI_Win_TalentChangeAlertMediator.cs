// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Win_TalentChangeAlertMediator
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
using UnityEngine.Events;

namespace Game {

    public class TalentChangeData
    {
        public string context;
        public UnityAction confirmCall;

        public TalentChangeData(string context,UnityAction confirmCall)
        {
            this.context = context;
            this.confirmCall = confirmCall;
        }
    }

    public class UI_Win_TalentChangeAlertMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_TalentChangeAlertMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_TalentChangeAlertMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_TalentChangeAlertView view;

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
            view.m_lbl_title_LanguageText.text = LanguageUtils.getText(128015);
            if (view.data != null)
            {
                TalentChangeData data = view.data as TalentChangeData;
                view.m_lbl_text_LanguageText.text = data.context;
            }
            
            int resetItemId = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).talentResetItemID;
            ItemDefine itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(resetItemId);
            BagProxy bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            long itemCount = bagProxy.GetItemNum(resetItemId);

            if (itemCount <= 0)
            {
                view.m_img_frame_PolygonImage.enabled = false;
                ItemDefine DenarConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(101090010);
                ClientUtils.LoadSprite(view.m_img_icon2_PolygonImage, DenarConfig.itemIcon);
                view.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(itemConfig.shortcutPrice).ToString();
                view.m_lbl_have_LanguageText.text = LanguageUtils.getTextFormat(760024, ClientUtils.FormatComma(playerProxy.CurrentRoleInfo.denar));
                view.m_lbl_Text_LanguageText.text = LanguageUtils.getText(300097);
                Timer.Register(0.1f, () => { view.m_lbl_line2_LanguageText.UpdateLanguage(); });

            }
            else
            {
                view.m_img_frame_PolygonImage.enabled = true;
                ClientUtils.LoadSprite(view.m_img_icon2_PolygonImage, itemConfig.itemIcon);
                view.m_lbl_line2_LanguageText.text = LanguageUtils.getText(128021);
                view.m_lbl_have_LanguageText.text = LanguageUtils.getTextFormat(760024, ClientUtils.FormatComma(itemCount));
                view.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730038);
            }
        }

        protected override void BindUIEvent()
        {
            view.m_btn_languageButton_GameButton.onClick.AddListener(OnConfirmEvent);
            view.m_btn_cancel.m_btn_languageButton_GameButton.onClick.AddListener(CloseUI);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OnConfirmEvent()
        {
            if (view.data != null)
            {
                TalentChangeData data = view.data as TalentChangeData;
                if (data.confirmCall != null)
                {
                    view.m_btn_languageButton_GameButton.interactable = false;
                    data.confirmCall.Invoke();
                    CloseUI();
                }
            }
        }

        private void CloseUI()
        {
            CoreUtils.uiManager.CloseUI(UI.s_talentChangeAlert);
        }
    }
}