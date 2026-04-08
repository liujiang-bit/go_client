// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeFirst_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ChargeFirst_SubView : UI_SubView
    {

        protected override void BindEvent()
        {
            base.BindEvent();
            var rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            var cfgData = CoreUtils.dataService.QueryRecord<Data.RechargeFirstDefine>(1001);
            var cfgPrice = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(cfgData.price);
            
            var vipData = CoreUtils.dataService.QueryRecord<Data.VipDefine>(cfgData.vipSpecialBox);
            
            m_btn_buy.m_lbl_Text_LanguageText.text = rechargeProxy.GetPriceString(cfgData.price);
            m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                var isFirst = rechargeProxy.IsFirstRechargeDone();
                if (!isFirst)
                {
                    rechargeProxy.CallSdkBuyByPcid(cfgPrice,cfgPrice.rechargeID.ToString(), cfgPrice.price.ToString("N2"));
                }
                else
                {
                    Debug.LogError("Had First Recharge! Why this view shown?");
                }
            } );
            
            m_btn_bigBox_GameButton.onClick.AddListener(() =>
            {
                if (!m_UI_Tip_BoxReward.gameObject.activeSelf)
                {
                    m_UI_Tip_BoxReward.gameObject.SetActive(true);
                    m_UI_Tip_BoxReward.IsShow = true;
                }

            });
            
            var rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> groupDataList = rewardGroupProxy.GetRewardDataByGroup(cfgData.itemPackage);
            if (groupDataList.Count > 0 && groupDataList[0].ItemData != null && groupDataList[0].ItemData.itemDefine != null)
            {
                m_UI_Model_Item1.RefreshByGroup(groupDataList[0], 3);
                m_UI_Model_Item1.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Model_Item1.gameObject.SetActive(false);
            }
            if (groupDataList.Count > 1 && groupDataList[1].ItemData != null && groupDataList[1].ItemData.itemDefine != null)
            {
                m_UI_Model_Item2.RefreshByGroup(groupDataList[1], 3);
                m_UI_Model_Item2.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Model_Item2.gameObject.SetActive(false);
            }
            if (groupDataList.Count > 2 && groupDataList[2].ItemData != null && groupDataList[2].ItemData.itemDefine != null)
            {
                m_UI_Model_Item3.RefreshByGroup(groupDataList[2], 3);
                m_UI_Model_Item3.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Model_Item3.gameObject.SetActive(false);
            }
            if (groupDataList.Count > 3 && groupDataList[3].ItemData != null && groupDataList[3].ItemData.itemDefine != null)
            {
                m_UI_Model_Item4.RefreshByGroup(groupDataList[3], 3);
                m_UI_Model_Item4.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Model_Item4.gameObject.SetActive(false);
            }
            if (groupDataList.Count > 4 && groupDataList[4].ItemData != null && groupDataList[4].ItemData.itemDefine != null)
            {
                m_UI_Model_Item5.RefreshByGroup(groupDataList[4], 3);
                m_UI_Model_Item5.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Model_Item5.gameObject.SetActive(false);
            }
            
            
            var heroShow = cfgData.heroShow;
            if (heroShow != 0)
            {
                var hero = CoreUtils.dataService.QueryRecord<Data.HeroDefine>(heroShow);
                ClientUtils.LoadSpine(m_spin_char_SkeletonGraphic, hero.heroModel);
            }
            
            m_UI_Tip_BoxReward.RefreshInfo(vipData.specialBox);

        }
    }
}