// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月15日
// Update Time         :    2020年1月15日
// Class Description   :    ResShortMediator
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
    public class ResShortMediator : GameMediator {
        #region Member
        public static string NameMediator = "ResShortMediator";

        private CurrencyProxy m_currencyProxy;
        private long[] m_rss = new long[4];
        private long[] m_needRss = new long[4];
        private UI_Item_ShowResShort_SubView[] m_rssView = new UI_Item_ShowResShort_SubView[4];
        #endregion

        //IMediatorPlug needs
        public ResShortMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ResShortView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                 CmdConstant.UpdateCurrency
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    OnCurrencyUpdate();
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

            m_rssView[0] = view.m_UI_Item_Food;
            m_rssView[1] = view.m_UI_Item_Wood;
            m_rssView[2] = view.m_UI_Item_Stone;
            m_rssView[3] = view.m_UI_Item_Gold;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_UI_Model_btn.m_btn_languageButton_GameButton.onClick.AddListener(OnGetMore);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_ResShort);
        }

        protected override void BindUIData()
        {
            OnCurrencyRefresh();

            for (int i = 0;i< m_needRss.Length;i++)
            {
                if(m_needRss[i]<=0)
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

        private void OnCurrencyUpdate()
        {
            OnCurrencyRefresh();
            bool autoClose = true;
            for (int i = 0; i < m_needRss.Length; i++)
            {
                autoClose &= m_needRss[i] <= 0;
                if (m_needRss[i] > 0)
                {
                    m_rssView[i].m_lbl_count_LanguageText.text = m_needRss[i].ToString("N0");
                }
                else if(m_rssView[i].gameObject.activeSelf)//已补足
                {
                    m_rssView[i].m_lbl_count_LanguageText.text = LanguageUtils.getText(300057);
                }
            }
            if(autoClose)
            {
                CoreUtils.uiManager.CloseUI(UI.s_ResShort);
            }
        }

        private void OnCurrencyRefresh()
        {
            m_needRss[0] = m_currencyProxy.Food - m_rss[0] >= 0 ? 0 : -(m_currencyProxy.Food - m_rss[0]);
            m_needRss[1] = m_currencyProxy.Wood - m_rss[1] >= 0 ? 0 : -(m_currencyProxy.Wood - m_rss[1]);
            m_needRss[2] = m_currencyProxy.Stone - m_rss[2] >= 0 ? 0 : -(m_currencyProxy.Stone - m_rss[2]);
            m_needRss[3] = m_currencyProxy.Gold - m_rss[3] >= 0 ? 0 : -(m_currencyProxy.Gold - m_rss[3]);
        }
       
        #endregion

        private void OnGetMore()
        {
            long[] tmpRss = new long[4];
            for(int i = 0;i< m_needRss.Length;i++)
            {
                if(m_needRss[i]>0)
                {
                    tmpRss[i] = m_rss[i];
                }
            }
            CoreUtils.uiManager.ShowUI(UI.s_AddRes,null, tmpRss);
        }
    }
}