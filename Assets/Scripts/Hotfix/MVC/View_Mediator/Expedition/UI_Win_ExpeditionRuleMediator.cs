// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    UI_Win_ExpeditionRuleMediator
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
using UnityEngine.UI;

namespace Game {
    public class UI_Win_ExpeditionRuleMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ExpeditionRuleMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_ExpeditionRuleMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ExpeditionRuleView view;

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
            view.m_UI_Model_Window_Type4.m_btn_close_GameButton.onClick.AddListener(OnClickClose);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OnClickClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_expeditionRule);
        }
    }
}