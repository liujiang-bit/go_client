// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Win_ExChangeActionPointMediator
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
    public class UI_Win_ExChangeActionPointMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ExChangeActionPointMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_ExChangeActionPointMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ExChangeActionPointView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_BuyActionForce.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_BuyActionForce.TagName:
                    {
                        RefreshUI();
                    }
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
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (m_playerProxy == null) return;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (m_bagProxy == null) return;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            if (m_playerAttributeProxy == null) return;
            m_configCfg = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0);
            if (m_configCfg == null) return;
            RefreshUI();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_exchageActionPoint);
            });
            view.m_btn_sure.AddClickEvent(OnClickOKButton);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void RefreshUI()
        {
            var currencyCfg = CoreUtils.dataService.QueryRecord<Data.CurrencyDefine>(104);
            if (currencyCfg == null) return;
            view.m_lbl_item_name1_LanguageText.text = LanguageUtils.getText(currencyCfg.l_desID);
            view.m_UI_Model_Item1.m_lbl_count_LanguageText.text = GetExchangeDenarNum().ToString();

            view.m_lbl_item_name2_LanguageText.text = LanguageUtils.getText(300061);
            view.m_UI_Model_Item2.m_lbl_count_LanguageText.text = m_configCfg.denarChangeEnery2.ToString();
            RefreshProgress();
        }

        private void RefreshProgress()
        {
            long curValue = m_playerProxy.CurrentRoleInfo.actionForce;
            long maxValue = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0).vitalityLimit + (int)m_playerAttributeProxy.GetCityAttribute(Data.attrType.maxVitality).value;
            view.m_pb_rogressBar_GameSlider.value = curValue * 1.0f / maxValue;
            view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300001, ClientUtils.FormatComma(curValue), ClientUtils.FormatComma(maxValue));
        }

        private void OnClickOKButton()
        {
            if(m_playerProxy.CurrentRoleInfo.denar < GetExchangeDenarNum())
            {
                CoreUtils.uiManager.ShowUI(UI.s_GemShort);
            }
            else
            {
                Alert.CreateAlert(LanguageUtils.getTextFormat(100713, GetExchangeDenarNum())).SetLeftButton().SetRightButton(() =>
                {
                    Role_BuyActionForce.request request = new Role_BuyActionForce.request();
                    AppFacade.GetInstance().SendSproto(request);
                }).Show();
            }
        }

        private int GetExchangeDenarNum()
        {
            if(m_playerProxy.CurrentRoleInfo.buyActionForceCount >= m_configCfg.denarChangeEnery1.Count)
            {
                return m_configCfg.denarChangeEnery1[m_configCfg.denarChangeEnery1.Count - 1];
            }
            else
            {
                return m_configCfg.denarChangeEnery1[(int)m_playerProxy.CurrentRoleInfo.buyActionForceCount];
            }
        }

        private PlayerAttributeProxy m_playerAttributeProxy = null;
        private PlayerProxy m_playerProxy = null;
        private BagProxy m_bagProxy = null;
        private Data.ConfigDefine m_configCfg = null;
    }
}