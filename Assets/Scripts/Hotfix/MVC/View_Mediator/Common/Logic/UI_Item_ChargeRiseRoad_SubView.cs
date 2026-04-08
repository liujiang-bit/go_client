// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeRiseRoad_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Item_ChargeRiseRoad_SubView : UI_SubView
    {
        private RechargeProxy m_rechargeProxy;
        private PlayerProxy m_PlayerProxy;
        private RewardGroupProxy m_rewardGroupProxy;
        private RechargeFirstDefine m_cfgData;
        private List<RewardGroupData> m_groupDataList;
        private List<RectTransform> m_groupData_Tr_List;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            m_groupData_Tr_List = new List<RectTransform>();
            m_groupData_Tr_List.Add(m_UI_Model_Item1.gameObject.GetComponent<RectTransform>());
            m_groupData_Tr_List.Add(m_UI_Model_Item2.gameObject.GetComponent<RectTransform>());
            m_groupData_Tr_List.Add(m_UI_Model_Item3.gameObject.GetComponent<RectTransform>());
            m_groupData_Tr_List.Add(m_UI_Model_Item4.gameObject.GetComponent<RectTransform>());
            m_groupData_Tr_List.Add(m_UI_Model_Item5.gameObject.GetComponent<RectTransform>());
            
            Refresh();
            
            m_btn_charge.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                if (m_cfgData != null)
                {
                    var pageType = (int)EnumRechargeListPageType.ChargeGemShop;
                    m_rechargeProxy.TryGetJumpToPageType(m_cfgData.jumpType,ref pageType);
                    AppFacade.GetInstance().SendNotification(CmdConstant.JumpToChargeListByPageType, pageType);
                }
                else
                {
                    Debug.Log("m_cfgData == null");
                }
            });
            m_btn_get.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                Recharge_AwardRisePackage.request rq = new Recharge_AwardRisePackage.request();
                rq.id = m_cfgData.ID;
                AppFacade.GetInstance().SendSproto(rq);
                if (m_groupDataList != null && m_groupData_Tr_List != null)
                {
                    UIHelper.FlyRewardEffect(m_groupData_Tr_List.ToArray() , m_groupDataList);
                }
            });
            
        }

        public void Refresh()
        {
            if (m_rechargeProxy.TryGetCurRiseRoadCfg(out var cfgData))
            {
                m_cfgData = cfgData;
                
                ClientUtils.LoadSprite(m_img_bg_PolygonImage,m_cfgData.background);
                
                m_lbl_mes2_LanguageText.text = LanguageUtils.getTextFormat(800052, ClientUtils.FormatComma((long)cfgData.giftValue));
                var heroShow = cfgData.heroShow;
                if (heroShow != 0)
                {
                    var hero = CoreUtils.dataService.QueryRecord<HeroDefine>(heroShow);
                    ClientUtils.LoadSpine(m_spin_char_SkeletonGraphic, hero.heroModel , () =>
                    {
                        m_spin_char_SkeletonGraphic.gameObject.SetActive(true);
                    });
                }
                else
                {
                    m_spin_char_SkeletonGraphic.gameObject.SetActive(false);
                }

                var remain = Mathf.Max(0, cfgData.needDenar - m_PlayerProxy.CurrentRoleInfo.riseRoad);
                m_lbl_mes1_LanguageText.text = LanguageUtils.getTextFormat(800050 ,ClientUtils.FormatComma((long)remain));
                m_lbl_mes1_LanguageText.gameObject.SetActive(remain > 0);
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(cfgData.itemPackage);
                if (m_groupDataList.Count > 0 && m_groupDataList[0].ItemData != null && m_groupDataList[0].ItemData.itemDefine != null)
                {
                    m_UI_Model_Item1.Refresh(m_groupDataList[0].ItemData.itemDefine, m_groupDataList[0].number, false, true);
                    m_UI_Model_Item1.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_Item1.gameObject.SetActive(false);
                }
                if (m_groupDataList.Count > 1 && m_groupDataList[1].ItemData != null && m_groupDataList[1].ItemData.itemDefine != null)
                {
                    m_UI_Model_Item2.Refresh(m_groupDataList[1].ItemData.itemDefine, m_groupDataList[1].number, false, true);
                    m_UI_Model_Item2.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_Item2.gameObject.SetActive(false);
                }
                if (m_groupDataList.Count > 2 && m_groupDataList[2].ItemData != null && m_groupDataList[2].ItemData.itemDefine != null)
                {
                    m_UI_Model_Item3.Refresh(m_groupDataList[2].ItemData.itemDefine, m_groupDataList[2].number, false, true);
                    m_UI_Model_Item3.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_Item3.gameObject.SetActive(false);
                }
                if (m_groupDataList.Count > 3 && m_groupDataList[3].ItemData != null && m_groupDataList[3].ItemData.itemDefine != null)
                {
                    m_UI_Model_Item4.Refresh(m_groupDataList[3].ItemData.itemDefine, m_groupDataList[3].number, false, true);
                    m_UI_Model_Item4.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_Item4.gameObject.SetActive(false);
                }
                if (m_groupDataList.Count > 4 && m_groupDataList[4].ItemData != null && m_groupDataList[4].ItemData.itemDefine != null)
                {
                    m_UI_Model_Item5.Refresh(m_groupDataList[4].ItemData.itemDefine, m_groupDataList[4].number, false, true);
                    m_UI_Model_Item5.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_Item5.gameObject.SetActive(false);
                }

                var IsCanCollect = m_rechargeProxy.IsCanCollectCurRiseRoadReward();
                m_btn_get.gameObject.SetActive(IsCanCollect);
                m_btn_charge.gameObject.SetActive(!IsCanCollect);
            }
        }
    }
}