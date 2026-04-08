// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_IF_ExpeditionFightFailMediator
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

    public class ExpeditionFightFailViewData
    {
        public Data.ExpeditionDefine ExpeditionCfg { get; set; }

        public ExpeditionFightResult Result { get; set; }
    }


    public class UI_IF_ExpeditionFightFailMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_ExpeditionFightFailMediator";


        #endregion

        //IMediatorPlug needs
        public UI_IF_ExpeditionFightFailMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_ExpeditionFightFailView view;

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
            m_viewData = view.data as ExpeditionFightFailViewData;
            RefreshUI();
        }

        protected override void BindUIEvent()
        {
            view.m_btn_back.AddClickEvent(OnClickBack);
            view.m_btn_try.AddClickEvent(OnClickRetry);
            CoreUtils.audioService.PlayOneShot("Sound_Ui_Defeated");
        }

        protected override void BindUIData()
        {

        }

        public override bool onMenuBackCallback()
        {
            OnClickBack();
            return true;
        }

        #endregion

        private void RefreshUI()
        {
            view.m_UI_Item_ExpeditionFightTask.Show(m_viewData.ExpeditionCfg);
            int languageId = 0;
            switch(m_viewData.Result)
            {
                case ExpeditionFightResult.TimeoutFail:
                    languageId = 805024;
                    break;
                case ExpeditionFightResult.FightFail:
                    languageId = 805027;
                    break;
            }
            view.m_lbl_title_LanguageText.text = LanguageUtils.getText(languageId);
        }

        private void OnClickRetry()
        {
            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightFail);
            AppFacade.GetInstance().SendNotification(CmdConstant.RetryExpedtionFight);
        }

        private void OnClickBack()
        {
            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightFail);
            AppFacade.GetInstance().SendNotification(CmdConstant.ExitExpeditionMap);
        }

        private ExpeditionFightFailViewData m_viewData = null;
    }
}