// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Win_ItemExchangeMediator
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

    public class ItemExchangeViewData
    {
        public HeroProxy.Hero Hero { get; set; }
    }

    public class UI_Win_ItemExchangeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ItemExchangeMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_ItemExchangeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ItemExchangeView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Hero_ExchangeHeroItem.TagName,

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Hero_ExchangeHeroItem.TagName:
                    {
                        Hero_ExchangeHeroItem.response response = notification.Body as Hero_ExchangeHeroItem.response;
                        if (response == null) return;
                        OnExchangeItemSuccess();
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
            if(!CoreUtils.uiManager.ExistUI(UI.s_captainItemSource))
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.SetHeroSkillLineSort, 6000);
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_curExchangeCount = 0;
            ItemExchangeViewData data = view.data as ItemExchangeViewData;
            if (data == null || data.Hero == null) return;
            m_hero = data.Hero;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (m_bagProxy == null) return;
            RefreshUI();
        }

        protected override void BindUIEvent()
        {
            view.m_btn_sure.AddClickEvent(OnClickedOK);
            view.m_btn_quick_GameButton.onClick.AddListener(OnClickedQuickMore);
            view.m_btn_add_GameButton.onClick.AddListener(OnClickedMore);
            view.m_btn_substract_GameButton.onClick.AddListener(OnClickedLess);
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(OnClickedClose);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OnClickedClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_itemExchange);
        }

        private void OnClickedOK()
        {
            if (m_curExchangeCount == 0) return;
            Hero_ExchangeHeroItem.request request = new Hero_ExchangeHeroItem.request()
            {
                heroId = m_hero.config.ID,
                itemNum = m_curExchangeCount,
            };
            AppFacade.GetInstance().SendSproto(request);
        }

        private void OnClickedMore()
        {
            long itemCount = m_bagProxy.GetItemNum(m_hero.config.exchange);
            if (m_curExchangeCount >= itemCount) return;
            m_curExchangeCount++;
            RefreshUI();
        }

        private void OnClickedLess()
        {
            if (m_curExchangeCount == 0) return;
            m_curExchangeCount--;
            RefreshUI();
        }

        private void OnClickedQuickMore()
        {
            m_curExchangeCount = m_curExchangeCount + m_curQuickExchangeCount;
            RefreshUI();
        }

        private void RefreshUI()
        {            
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_hero.config.getItem);
            if (itemCfg == null) return;
            view.m_UI_herolvup_item.Refresh(itemCfg, 0, false);
            m_curCostItemCount = m_hero.GetSkillLevelUpCostItemNum();
            long itemCount = m_bagProxy.GetItemNum(itemCfg.ID);
            if(m_curExchangeCount == 0)
            {
                view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(145056, LanguageUtils.getText(itemCfg.l_nameID), 
                    ClientUtils.FormatComma(itemCount), ClientUtils.FormatComma(m_curCostItemCount));
            }
            else
            {
                view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(166030, LanguageUtils.getText(itemCfg.l_nameID), 
                    ClientUtils.FormatComma(itemCount), ClientUtils.FormatComma(m_curExchangeCount), ClientUtils.FormatComma(m_curCostItemCount));
            }
            view.m_pb_rogressBar_GameSlider.value = Mathf.Min(1, (itemCount + m_curExchangeCount) * 1.0f / m_curCostItemCount);
            if(itemCount + m_curExchangeCount > m_curCostItemCount)
            {
                view.m_lbl_mes_LanguageText.text = LanguageUtils.getText(166029);
            }
            else
            {
                view.m_lbl_mes_LanguageText.text = LanguageUtils.getTextFormat(166028, Mathf.Max(0, m_curCostItemCount - itemCount - m_curExchangeCount));
            }            
            view.m_UI_Model_Item1.Refresh(itemCfg, itemCount + m_curExchangeCount, false);
            view.m_UI_Model_Item1.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(itemCount + m_curExchangeCount);
            view.m_lbl_item_name1_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);

            RefreshExchangeUI();
        }

        private void RefreshExchangeUI()
        {
            int exchageItemId = m_hero.config.exchange;
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(exchageItemId);
            if (itemCfg == null) return;
            long itemCount = m_bagProxy.GetItemNum(itemCfg.ID);
            view.m_UI_Model_Item2.Refresh(itemCfg, itemCount - m_curExchangeCount, false);
            view.m_UI_Model_Item2.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(itemCount - m_curExchangeCount);
            view.m_lbl_item_name2_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
            long getItemCount = m_bagProxy.GetItemNum(m_hero.config.getItem);
            bool isQuickBtnVisible = getItemCount + m_curExchangeCount < m_curCostItemCount && m_curExchangeCount < itemCount;
            view.m_btn_quick_GameButton.gameObject.SetActive(isQuickBtnVisible);
            if(isQuickBtnVisible)
            {
                m_curQuickExchangeCount = Mathf.Min(m_curCostItemCount - (int)getItemCount - m_curExchangeCount, (int)itemCount - m_curExchangeCount);
                view.m_lbl_quick_num_LanguageText.text = $"+{ClientUtils.FormatComma(m_curQuickExchangeCount)}";
            }
            view.m_lbl_changeNum_LanguageText.text = ClientUtils.FormatComma(m_curExchangeCount);

            view.m_img_add_gray_PolygonImage.gameObject.SetActive(m_curExchangeCount >= itemCount || itemCount == 0);
            view.m_img_substract_gray_PolygonImage.gameObject.SetActive(m_curExchangeCount == 0);

            view.m_btn_sure.m_img_forbid_PolygonImage.gameObject.SetActive(m_curExchangeCount == 0);
        }

        private void OnExchangeItemSuccess()
        {
            m_curExchangeCount = 0;
            RefreshUI();
            AppFacade.GetInstance().SendNotification(CmdConstant.SkillUpSourceRefresh);
        }

        private BagProxy m_bagProxy = null;
        private HeroProxy.Hero m_hero = null;
        private int m_curExchangeCount = 0;
        private int m_curCostItemCount = 0;
        private int m_curQuickExchangeCount = 0;
    }
}