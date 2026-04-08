// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Item_MainIFEventCharge_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_MainIFEventCharge_SubView : UI_Item_MainIFEventBtn_SubView
    {
        private bool m_isInit;
        private RechargeProxy m_RechargeProxy;
        private bool m_initProxy;

        //subView事件监听
        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.UpdateRechargeReddot,
                    CmdConstant.CityBuildingLevelUP,
//                    CmdConstant.ActivityScheduleUpdate,
//                    CmdConstant.UpdateActivityTotalReddot,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateRechargeReddot:
                case CmdConstant.CityBuildingLevelUP:
                    {
                        //case CmdConstant.UpdateActivityReddot:
                        //case CmdConstant.ActivityScheduleUpdate:
                        Refresh();
                    }
                    break;
                default:
                    break;
            }
        }

        private void InitProxy()
        {
            if (!m_initProxy)
            {
                m_RechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
                m_initProxy = true;
            }
        }

        public void Refresh()
        {
            var bOpen = SystemOpen.IsSystemOpen(EnumSystemOpen.charge_pop,false);
            if (!bOpen)
            {
                this.gameObject.SetActive(bOpen);
                return;
            }
            
            this.gameObject.SetActive(true);
            if (!m_isInit)
            {
                InitProxy();
                AddBtnListener();
                m_isInit = true;
            }
            UpdateReddot();
            var result = m_RechargeProxy.GetRechargeListItemCfgIds();
            if (result != null && result.Count > 0)
            {
                ClientUtils.LoadSprite(m_btn_eventIcon_PolygonImage , result[0].icon );
            }
        }

        public void UpdateReddot()
        {  
            int reddot = m_RechargeProxy.GetTotalReddot();
            if (reddot > 0)
            {
                m_img_redpoint.gameObject.SetActive(true);
                m_lbl_redpoint_LanguageText.gameObject.SetActive(true);
                m_lbl_redpoint_LanguageText.text = reddot.ToString();
            }
            else
            {
                m_img_redpoint.gameObject.SetActive(false);
            }
        }

        public void AddBtnListener()
        {
            m_btn_eventIcon_GameButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_Charge);
        }
    }
}