// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月6日
// Update Time         :    2020年5月6日
// Class Description   :    UI_Item_EventType1Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using SprotoType;
using System;

namespace Game {
    public partial class UI_Item_EventType1Item_SubView : UI_SubView
    {
        private PlayerProxy m_playerProxy;
        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;
        private BagProxy m_bagProxy;

        private bool m_isInit;
        private List<UI_Model_RewardGet_SubView> m_beforeList;
        private List<UI_Model_RewardGet_SubView> m_afterList;

        private ActivityBehaviorData m_behaviorData;
        private ActivityScheduleData m_scheduleData;

        public Action<UI_Item_EventType1Item_SubView> BtnClickCallback;

        private bool m_isRequesting;
        private bool m_isCanExchange;

        public void Refresh(ActivityBehaviorData behaviorData, ActivityScheduleData scheduleData)
        {
            m_behaviorData = behaviorData;
            m_scheduleData = scheduleData;
            m_isRequesting = false;
            m_isCanExchange = false;
            if (!m_isInit)
            {
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

                float scale = m_UI_ItemBefore1.m_root_RectTransform.localScale.x;
                m_beforeList = new List<UI_Model_RewardGet_SubView>();
                m_beforeList.Add(m_UI_ItemBefore1);
                m_beforeList.Add(m_UI_ItemBefore2);
                for (int i = 0; i < m_beforeList.Count; i++)
                {
                    m_beforeList[i].SetScale(scale);
                }

                m_afterList = new List<UI_Model_RewardGet_SubView>();
                m_afterList.Add(m_UI_ItemAfter1);
                m_afterList.Add(m_UI_ItemAfter2);
                for (int i = 0; i < m_afterList.Count; i++)
                {
                    m_afterList[i].SetScale(scale);
                }

                m_btn_exchange.AddClickEvent(OnExchange);

                m_isInit = true;
            }
            m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(300130, behaviorData.PlayerBehavior);
            int canExChangeNum= behaviorData.Count; ;
            if (scheduleData.Info.exchange.ContainsKey(behaviorData.Id))
            {
                canExChangeNum = behaviorData.Count - (int)scheduleData.Info.exchange[behaviorData.Id].count;
            }
            m_lbl_times_LanguageText.text = LanguageUtils.getTextFormat(300133, canExChangeNum);

            ActivityConversionTypeDefine define = m_activityProxy.GetConversionTypeDefine(behaviorData.Id);
            if (define.conversionItem != null)
            {
                int count = define.conversionItem.Count;
                int enoughNum = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (i >= count)
                    {
                        m_beforeList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_beforeList[i].gameObject.SetActive(true);
                        ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(define.conversionItem[i]);
                        m_beforeList[i].RefreshItem(itemDefine, 0, false);

                        int itemNum = (int)m_bagProxy.GetItemNum(itemDefine.ID);
                        //m_beforeList[i].m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(false);
                        if (itemNum >= define.num[i])
                        {
                            enoughNum = enoughNum + 1;
                            m_beforeList[i].m_UI_Model_Item.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(762118, itemNum, define.num[i]);
                            m_beforeList[i].m_UI_Model_Item.m_img_icon_GrayChildrens.Normal();
                            m_beforeList[i].m_UI_Model_Item.m_img_quality_GrayChildrens.Normal();
                            //m_beforeList[i].m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(true);
                        }
                        else
                        {
                            m_beforeList[i].m_UI_Model_Item.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(762119, itemNum, define.num[i]);
                            m_beforeList[i].m_UI_Model_Item.m_img_icon_GrayChildrens.Gray();
                            m_beforeList[i].m_UI_Model_Item.m_img_quality_GrayChildrens.Gray();
                            //m_beforeList[i].m_UI_Model_Item.m_img_bg_PolygonImage.gameObject.SetActive(true);
                        }
                    }
                }
                if ((enoughNum >= count) && (m_playerProxy.GetVipLevel() >= behaviorData.PlayerBehavior) && (canExChangeNum > 0))
                {
                    m_btn_exchange.SetGray(false);
                    m_isCanExchange = true;
                }
                else
                {
                    m_btn_exchange.SetGray(true);
                }
            }
            List<RewardGroupData> groupList = m_rewardGroupProxy.GetRewardDataByGroup(define.itemPackage);
            int count2 = groupList.Count;
            for (int i = 0; i < 2; i++)
            {
                if (i >= count2)
                {
                    m_afterList[i].gameObject.SetActive(false);
                }
                else
                {
                    m_afterList[i].gameObject.SetActive(true);
                    m_afterList[i].RefreshByGroup(groupList[i], 3);
                    //m_afterList[i].m_lbl_count_LanguageText.text = groupList[i].number.ToString();
                }
            }
        }

        private void OnExchange()
        {
            if (!m_isCanExchange)
            {
                return;
            }
            if (m_isRequesting)
            {
                return;
            }
            m_isRequesting = true;
            Timer.Register(0.5f, () => {
                m_isRequesting = false;
            });
            var sp = new Activity_Exchange.request();
            sp.activityId = m_scheduleData.ActivityId;
            sp.id = m_behaviorData.Id;
            AppFacade.GetInstance().SendSproto(sp);
            Debug.LogFormat("activityId:{0} acEventId:{1}", m_scheduleData.ActivityId, m_behaviorData.Id);
            if (BtnClickCallback != null)
            {
                BtnClickCallback(this);
            }
        }

        public int GetEventId()
        {
            return m_behaviorData.Id;
        }
    }
}