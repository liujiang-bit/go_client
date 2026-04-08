// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    UI_IF_BattleMainMediator
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
    public class UI_IF_BattleMainMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_BattleMainMediator";


        #endregion

        //IMediatorPlug needs
        public UI_IF_BattleMainMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_BattleMainView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ExpeditionInfoChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ExpeditionInfoChange:
                    OnExpeditionInfoChanged((int)notification.Body);
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
            if(CoreUtils.audioService.GetCurBgmName() != ExpeditionProxy.UIBgm)
            {
                CoreUtils.audioService.PlayBgm(ExpeditionProxy.UIBgm);
            }
            RefreshBattleMainRedPoint();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Interface.AddClickEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_battleMain);
            });
            view.m_UI_Item_BattleMainMenu1.m_btn_menu_GameButton.onClick.AddListener(OpenExpedition);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OpenExpedition()
        {
            if (SystemOpen.IsSystemClose(EnumSystemOpen.war_expedition))
            {
                Data.SystemOpenDefine cfg = CoreUtils.dataService.QueryRecord<Data.SystemOpenDefine>(10005);
                if(cfg != null)
                {
                    Tip.CreateTip(805001, cfg.openLv);
                }
                return;
            }
            CoreUtils.uiManager.ShowUI(UI.s_expeditionFight);
            CoreUtils.uiManager.CloseUI(UI.s_battleMain);
        }

        private void CloseUI()
        {
            CoreUtils.uiManager.CloseUI(UI.s_battleMain);
        }

        private void RefreshBattleMainRedPoint()
        {
            var battleMain = AppFacade.GetInstance().RetrieveMediator(BattleMainlMediator.NameMediator) as BattleMainlMediator;
            int num = battleMain.GetExpeditionRedPointNumber();
            OnExpeditionInfoChanged(num);
        }

        private void OnExpeditionInfoChanged(int num)
        {
            view.m_UI_Item_BattleMainMenu1.m_img_redpot_PolygonImage.gameObject.SetActive(num > 0);
            view.m_UI_Item_BattleMainMenu1.m_lbl_redCount_LanguageText.text = num.ToString();
        }
    }
}