// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月22日
// Update Time         :    2020年1月22日
// Class Description   :    GemShortMediator
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
    public class GemShortMediator : GameMediator {
        #region Member
        public static string NameMediator = "GemShortMediator";


        #endregion

        //IMediatorPlug needs
        public GemShortMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public GemShortView view;

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

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_UI_Model_buy.m_btn_languageButton_GameButton.onClick.AddListener(GoGemShop);
        }

        private void GoGemShop()
        {
            CoreUtils.uiManager.CloseUI(UI.s_GemShort);
            CoreUtils.uiManager.ShowUI(UI.s_Charge,null,(int)EnumRechargeListPageType.ChargeGemShop);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_GemShort);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}