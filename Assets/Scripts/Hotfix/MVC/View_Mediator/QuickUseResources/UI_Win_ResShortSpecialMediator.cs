// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月24日
// Update Time         :    2020年7月24日
// Class Description   :    UI_Win_ResShortSpecialMediator
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
using Data;

namespace Game {
    
    public class UI_Win_ResShortSpecialMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ResShortSpecialMediator";

        private int type = 0;//1,只显示资源补足，2只显示材料补足，3二者都显示
        private CurrencyProxy m_currencyProxy;
        private BagProxy m_bagProxy;
        private long[] m_rss = new long[6];
        private long[] m_needRss = new long[4];//-1不须补足，0，已补足，>0等待补足
        private int m_needItemNum = 0;
        private UI_Item_ShowResShort_SubView[] m_rssView = new UI_Item_ShowResShort_SubView[4];

        private ItemDefine m_itemDefine;

        private bool m_currencyStockedUp, m_itemStockedUp;
        #endregion

        //IMediatorPlug needs
        public UI_Win_ResShortSpecialMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ResShortSpecialView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                 CmdConstant.UpdateCurrency,
                    CmdConstant.ItemInfoChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    OnCurrencyUpdate();
                    break;
                case CmdConstant.ItemInfoChange:
                    OnItemUpdate();
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
            m_rss = view.data as long[];
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

            m_rssView[0] = view.m_UI_Item_Food;
            m_rssView[1] = view.m_UI_Item_Wood;
            m_rssView[2] = view.m_UI_Item_Stone;
            m_rssView[3] = view.m_UI_Item_Gold;
            if (m_rss[4] != 0)
            {
                int itemNum = (int)(m_bagProxy.GetItemNum((int)m_rss[4]) - m_rss[5]);
                m_needItemNum = itemNum > 0 ? 0 : -itemNum;
                m_itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_rss[4]);
            }
            OnCurrencyRefresh();
            
            if (m_needRss[0]+ m_needRss[1] + m_needRss[2] + m_needRss[3] != 0)
            {
                type = 1;
                m_currencyStockedUp = false;
                m_itemStockedUp = true;
            }
            if (m_rss[4] != 0 && m_needItemNum != 0)
            {
                m_itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_rss[4]);
                if (type ==1 )
                {
                    type = 3;
                    m_currencyStockedUp = false;
                    m_itemStockedUp = false;
                }
                else
                {
                    type = 2;
                    m_currencyStockedUp = true;
                    m_itemStockedUp = false;
                }
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_UI_Model_btn.m_btn_languageButton_GameButton.onClick.AddListener(OnGetMore);
            view.m_btn_bug.AddClickEvent(OnBuyBtnClick);
        }

        protected override void BindUIData()
        {
            if (type == 1 || type == 3)
            {
                for (int i = 0; i < m_needRss.Length; i++)
                {
                    if (m_needRss[i] <=0)
                    {
                        m_rssView[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_rssView[i].gameObject.SetActive(true);
                        m_rssView[i].m_lbl_count_LanguageText.text = m_needRss[i].ToString("N0");
                    }
                }
            }
            if (type == 2 || type == 3)
            {
                view.m_UI_Model_Item.Refresh(m_itemDefine,string.Empty,false,true,false);
                view.m_lbl_item_name_LanguageText.text = LanguageUtils.getText(m_itemDefine.l_nameID);
                view.m_lbl_item_count_LanguageText.text = LanguageUtils.getTextFormat(145048, m_needItemNum.ToString("N0"));
                view.m_btn_bug.SetNum((m_itemDefine.shortcutPrice* m_needItemNum).ToString("N0"));
                view.m_lbl_item_Stockedup_LanguageText.gameObject.SetActive(false);
            }

            if (type == 1)
            {
                view.m_pl_res_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_pl_item_ArabLayoutCompment.gameObject.SetActive(false);
            }
            else if (type == 2)
            {
                view.m_pl_res_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_pl_item_ArabLayoutCompment.gameObject.SetActive(true);
            }
            else if (type == 3)
            {
                view.m_pl_res_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_pl_item_ArabLayoutCompment.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("not find type ");
                view.m_pl_res_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_pl_item_ArabLayoutCompment.gameObject.SetActive(false);
            }
        }

        #endregion
        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_resShortSpecial);
        }
        public void OnBuyBtnClick()
        {
            OnBuyBtnClick((int)m_rss[4], m_needItemNum, m_itemDefine.shortcutPrice);
        }
        public void OnBuyBtnClick(int itemid, int itemnum, int shortcutPrice)
        {
            Alert.CreateAlert(LanguageUtils.getTextFormat(300072, (shortcutPrice * itemnum).ToString("N0"))).SetRightButton(() =>
             {
                 if (!m_currencyProxy.ShortOfDenar(shortcutPrice * itemnum))
                 {
                     UIHelper.DenarCostRemain(shortcutPrice * itemnum, () =>
                     {
                         Role_BuyResource.request req = new Role_BuyResource.request();
                         req.itemId = itemid;
                         req.itemNum = itemnum;
                         AppFacade.GetInstance().SendSproto(req);
                     });
                 }
             });
        }
        private void OnItemUpdate()
        {
            bool autoClose = true;
            if (m_rss[4] == 0)
            {
                return;
            }

            int itemNum = (int)(m_bagProxy.GetItemNum(m_itemDefine.ID )- m_rss[5]);
            m_needItemNum = itemNum > 0 ? 0 : -itemNum;
            m_itemStockedUp = m_needItemNum==0;
            if (!m_itemStockedUp)
            {
                view.m_lbl_item_Stockedup_LanguageText.gameObject.SetActive(false);
                view.m_btn_bug.gameObject.SetActive(true);
                view.m_lbl_item_count_LanguageText.gameObject.SetActive(true);

            }
            else //已补足
            {
                view.m_lbl_item_Stockedup_LanguageText.gameObject.SetActive(true);
                view.m_btn_bug.gameObject.SetActive(false);
                view.m_lbl_item_count_LanguageText.gameObject.SetActive(false);
            }
            view.m_lbl_item_count_LanguageText.text = LanguageUtils.getTextFormat(145048, m_needItemNum.ToString("N0"));
            view.m_btn_bug.SetNum((m_itemDefine.shortcutPrice * m_needItemNum).ToString("N0"));
            autoClose = m_itemStockedUp & m_currencyStockedUp;
            if (autoClose)
            {
                CoreUtils.uiManager.CloseUI(UI.s_resShortSpecial);
            }
        }
        private void OnCurrencyUpdate()
        {
            OnCurrencyRefresh();
            bool autoClose = true;
            m_currencyStockedUp = true;
            for (int i = 0; i < m_needRss.Length; i++)
            {
                m_currencyStockedUp &= m_needRss[i] <= 0;
                if (m_needRss[i] > 0)
                {
                    m_rssView[i].m_lbl_count_LanguageText.text = m_needRss[i].ToString("N0");
                }
                else if (m_rssView[i].gameObject.activeSelf)//已补足
                {
                    m_rssView[i].m_lbl_count_LanguageText.text = LanguageUtils.getText(300057);
                }
            }
            autoClose = m_itemStockedUp & m_currencyStockedUp;
            if (autoClose)
            {
                CoreUtils.uiManager.CloseUI(UI.s_resShortSpecial);
            }
        }
        private void OnCurrencyRefresh()
        {
            m_needRss[0] = m_currencyProxy.Food - m_rss[0] >= 0 ? 0 : -(m_currencyProxy.Food - m_rss[0]);
            m_needRss[1] = m_currencyProxy.Wood - m_rss[1] >= 0 ? 0 : -(m_currencyProxy.Wood - m_rss[1]);
            m_needRss[2] = m_currencyProxy.Stone - m_rss[2] >= 0 ? 0 : -(m_currencyProxy.Stone - m_rss[2]);
            m_needRss[3] = m_currencyProxy.Gold - m_rss[3] >= 0 ? 0 : -(m_currencyProxy.Gold - m_rss[3]);
 
        }

        private void OnGetMore()
        {
            long[] tmpRss = new long[4];
            for (int i = 0; i < m_needRss.Length; i++)
            {
                if (m_needRss[i] > 0)
                {
                    tmpRss[i] = m_rss[i];
                }
            }
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, tmpRss);
        }
    }
}