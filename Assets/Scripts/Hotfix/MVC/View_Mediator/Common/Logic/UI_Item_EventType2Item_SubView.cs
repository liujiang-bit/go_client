// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月23日
// Update Time         :    2020年4月23日
// Class Description   :    UI_Item_EventType2Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using System;
using SprotoType;

namespace Game {
    public partial class UI_Item_EventType2Item_SubView : UI_SubView
    {
        private bool m_isInit;
        private RewardGroupProxy m_rewardGroupProxy;
        private ActivityProxy m_activityProxy;
        private List<UI_Model_RewardGet_SubView> m_itemViewList = new List<UI_Model_RewardGet_SubView>();
        private int m_acEventId;
        private Int64 m_activityId;
        private bool m_isRequesting;
        private bool m_isCanReceive;    //是否可领取
        public Action<UI_Item_EventType2Item_SubView> BtnClickCallback;
        private ActivityScheduleData m_scheduleData;
        private int m_index;

        //刷新开服活动行为
        public void Refresh1(int acEventId, ActivityScheduleData scheduleData, bool isExpire)
        {
            m_acEventId = acEventId;
            m_activityId = scheduleData.ActivityId;
            m_scheduleData = scheduleData;

            InitData();

            ActivityDaysTypeDefine define = m_activityProxy.GetDayTypeDefine(acEventId);
            PlayerBehaviorDataDefine behaviorDefine = CoreUtils.dataService.QueryRecord<PlayerBehaviorDataDefine>(define.playerBehavior);

            //奖励组物品
            RefreshRewardGroup(define.itemPackage);

            //行为描述
            RefreshBehaviorDesc(behaviorDefine, define.data0, define.data1, define.data2);

            //刷新按钮和进度
            RefreshProcessAndBtn(behaviorDefine, define.data0, define.data3, isExpire);
        }

        //刷新通用条件达成类活动行为
        public void Refresh2(int acEventId, ActivityScheduleData scheduleData, bool isExpire = false)
        {
            m_acEventId = acEventId;
            m_activityId = scheduleData.ActivityId;
            m_scheduleData = scheduleData;

            InitData();

            ActivityTargetTypeDefine define = m_activityProxy.GetTargetTypeDefine(acEventId);
            PlayerBehaviorDataDefine behaviorDefine = CoreUtils.dataService.QueryRecord<PlayerBehaviorDataDefine>(define.playerBehavior);

            //奖励组物品
            RefreshRewardGroup(define.itemPackage);

            //行为描述
            RefreshBehaviorDesc(behaviorDefine, define.data0, define.data1, define.data2);

            //刷新按钮和进度
            RefreshProcessAndBtn(behaviorDefine, define.data0, define.data3, isExpire);
        }

        //刷新最强执政官行为
        public void Refresh3(ActivityBehaviorData behaviorData, ActivityScheduleData scheduleData)
        {
            m_acEventId = behaviorData.Id;
            m_activityId = scheduleData.ActivityId;
            m_scheduleData = scheduleData;
            InitData3();

            int index = behaviorData.PlayerBehavior - 1;
            m_index = behaviorData.PlayerBehavior;
            ActivityKillTypeDefine skillTypeDefine = CoreUtils.dataService.QueryRecord<ActivityKillTypeDefine>(behaviorData.Id);
            int num = skillTypeDefine.standard[index];
            m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(skillTypeDefine.l_desID, ClientUtils.FormatComma(num));

            RefreshRewardGroup(skillTypeDefine.itemPackage[index]);

            int process = Mathf.FloorToInt((float)scheduleData.Info.score / behaviorData.Count*100);
            process = (process > 100) ? 100 : process;
            //string str = LanguageUtils.getTextFormat(300102, process);
            //str = LanguageUtils.getTextFormat(762203, str);
            m_lbl_process_LanguageText.text = LanguageUtils.getTextFormat(762236, process);

            //按钮
            if (scheduleData.Info.score >= behaviorData.Count)
            {
                if (m_scheduleData.IsReceive2(behaviorData.PlayerBehavior))
                {
                    m_isCanReceive = false;
                    m_btn_get.gameObject.SetActive(false);
                    m_btn_go.gameObject.SetActive(false);
                    m_lbl_already_LanguageText.gameObject.SetActive(true);
                }
                else
                {
                    m_isCanReceive = true;
                    m_btn_get.gameObject.SetActive(true);
                    m_btn_go.gameObject.SetActive(false);
                    m_lbl_already_LanguageText.gameObject.SetActive(false);
                }
            } else
            {
                m_isCanReceive = false;
                m_lbl_already_LanguageText.gameObject.SetActive(false);
                m_btn_get.gameObject.SetActive(false);
                m_btn_go.gameObject.SetActive(true);
            }
        }

        private void InitData()
        {
            if (!m_isInit)
            {
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;

                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                m_itemViewList.Add(m_UI_Model_Item4);
                m_itemViewList.Add(m_UI_Model_Item5);
                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(0.5f);
                }

                m_btn_get.m_btn_languageButton_GameButton.onClick.AddListener(ClickReceive);

                m_isInit = true;
            }
        }

        private void InitData3()
        {
            if (!m_isInit)
            {
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;

                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                m_itemViewList.Add(m_UI_Model_Item4);
                m_itemViewList.Add(m_UI_Model_Item5);
                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(0.5f);
                }

                m_btn_get.m_btn_languageButton_GameButton.onClick.AddListener(ClickReceive2);
                m_btn_go.m_btn_languageButton_GameButton.onClick.AddListener(OnGo);

                m_isInit = true;
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < 5; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(groupDataList[i], 3);
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
        }

        //刷新行为描述
        private void RefreshBehaviorDesc(PlayerBehaviorDataDefine behaviorDefine, int data0, int data1, int data2)
        {
            string str = "";
            int playerBehavior = behaviorDefine.ID;
            if (playerBehavior == 5001 || playerBehavior == 5002 || playerBehavior == 5003 || playerBehavior == 5006)
            {
                //这几种比较特殊 需要获取建筑名称
                BuildingTypeConfigDefine define2 = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>(data1);
                if (define2 != null)
                {
                    str = LanguageUtils.getTextFormat(behaviorDefine.l_behaviorID, ClientUtils.FormatComma(data0), LanguageUtils.getText(define2.l_nameId), data2);
                }
            }
            else
            {
                str = LanguageUtils.getTextFormat(behaviorDefine.l_behaviorID, ClientUtils.FormatComma(data0), data1);
            }
            m_lbl_title_LanguageText.text = str;
        }

        //刷新进度和按钮显示
        private void RefreshProcessAndBtn(PlayerBehaviorDataDefine behaviorDefine, int data0, int data3, bool isExpire)
        {
            //进度
            int val = m_scheduleData.GetBehaviorSchedule(behaviorDefine.ID, data3);
            val = (val > data0) ? data0 : val;
            bool isEnough = (val >= data0) ? true : false;
            if (isEnough)
            {
                //进度已满
                m_lbl_process_LanguageText.text = LanguageUtils.getTextFormat(762147, ClientUtils.FormatComma(val), ClientUtils.FormatComma(data0));
            }
            else
            {
                //进度未满
                m_lbl_process_LanguageText.text = LanguageUtils.getTextFormat(762148, ClientUtils.FormatComma(val), ClientUtils.FormatComma(data0));
            }

            //领取状态
            if (m_scheduleData.IsReceive(m_acEventId))
            {
                //已领取
                m_isCanReceive = false;
                m_btn_get.gameObject.SetActive(false);
                m_btn_go.gameObject.SetActive(false);
                m_lbl_already_LanguageText.gameObject.SetActive(true);
            }
            else
            {
                m_isCanReceive = isEnough;
                if (isExpire) //活动是否过期
                {
                    m_isCanReceive = false;
                }
                //未领取或不可领取
                m_btn_go.gameObject.SetActive(false);
                m_btn_get.gameObject.SetActive(true);
                m_btn_get.m_img_forbid_PolygonImage.gameObject.SetActive(!m_isCanReceive);
                m_lbl_already_LanguageText.gameObject.SetActive(false);
            }
        }

        public int GetEventId()
        {
            return m_acEventId;
        }

        public int GetIndex()
        {
            return m_index;
        }

        //点击领取
        public void ClickReceive()
        {
            if (!m_isCanReceive)
            {
                return;
            }
            if (m_isRequesting)
            {
                return;
            }
            Send((int)m_activityId, m_acEventId, -1);
        }

        //点击领取
        public void ClickReceive2()
        {
            if (!m_isCanReceive)
            {
                return;
            }
            if (m_isRequesting)
            {
                return;
            }
            Send((int)m_activityId, m_acEventId, m_index);
        }

        //领取发包
        private void Send(int activityId, int id, int index)
        {
            m_isRequesting = true;
            Timer.Register(1f, () => {
                m_isRequesting = false;
            });
            var sp = new Activity_ReceiveReward.request();
            sp.activityId = activityId;
            sp.id = id;
            if (index > -1)
            {
                sp.index = index;
            }
            AppFacade.GetInstance().SendSproto(sp);
            Debug.LogFormat("activityId:{0} acEventId:{1} index:{2}", m_activityId, m_acEventId, index);
            if (BtnClickCallback != null)
            {
                BtnClickCallback(this);
            }
        }

        //前往
        public void OnGo()
        {
            ActivityKillTypeDefine skillTypeDefine = CoreUtils.dataService.QueryRecord<ActivityKillTypeDefine>(m_acEventId);
            m_activityProxy.GoJump(skillTypeDefine.jumpType);
        }
    }
}