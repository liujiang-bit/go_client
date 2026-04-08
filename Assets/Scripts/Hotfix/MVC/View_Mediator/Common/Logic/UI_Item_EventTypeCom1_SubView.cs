// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    UI_Item_EventTypeCom1_SubView 兑换类活动
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;
using System;
using PureMVC.Interfaces;
using Data;

namespace Game {
    public partial class UI_Item_EventTypeCom1_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private BagProxy m_bagProxy;

        private bool m_isInit;
        private bool m_assetIsLoadFinish;
        private bool m_isInitList;

        private ActivityItemData m_menuData;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Dictionary<Int64, RectTransform> m_exchangeRecord = new Dictionary<Int64, RectTransform>();

        private Timer m_timer;
        private Int64 m_endTime;

        private ActivityScheduleData m_scheduleData;
        private ActivityScheduleData m_preScheduleData;
        private List<ActivityBehaviorData> m_behaviorList;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.ActivityExchangeRefresh,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ActivityExchangeRefresh://兑换
                    ExchangeProcess(notification.Body);
                    break;
                default:
                    break;
            }
        }

        public void Init(ActivityItemData menuData)
        {
            m_menuData = menuData;
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

                ReadData();

                m_UI_Model_EventType.m_btn_rank_GameButton.gameObject.SetActive(false);

                m_UI_Model_EventType.Refresh(m_menuData.Define);

                m_UI_Model_EventType.m_ck_showExchange_GameToggle.isOn = m_activityProxy.IsExchangeRemind();
                m_UI_Model_EventType.m_ck_showExchange_GameToggle.gameObject.SetActive(true);
                m_UI_Model_EventType.m_ck_showExchange_GameToggle.onValueChanged.AddListener(OnExchange);

                //预加载列表预设
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(m_UI_Model_EventType.m_sv_list_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);
                m_isInit = true;
            }
            else
            {
                ReadData();
                if (m_assetIsLoadFinish)
                {
                    Refresh();
                }
            }
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            gameObject.SetActive(true);
            m_assetIsLoadFinish = true;
            Refresh();
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);
            m_preScheduleData = m_scheduleData.GetScheduleData();
            List<ActivityBehaviorData> dataList = m_preScheduleData.GetBehaviorList();

            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            int vipLevel = (int)playerProxy.GetVipLevel();

            //排序
            dataList.Sort(delegate (ActivityBehaviorData x, ActivityBehaviorData y)
            {
                int re = IsEnoughExchange(x, vipLevel).CompareTo(IsEnoughExchange(y, vipLevel)); 
                if (re == 0)
                {
                    re = x.Id.CompareTo(y.Id);
                }
                return re;
            });
            m_behaviorList = dataList;
        }

        public int IsEnoughExchange(ActivityBehaviorData behaviorData, int vipLevel)
        { 
            if (vipLevel < behaviorData.PlayerBehavior)
            {
                return 2;
            }

            if (m_scheduleData.Info.exchange != null && m_scheduleData.Info.exchange.ContainsKey(behaviorData.Id))
            {
                if (m_scheduleData.Info.exchange[behaviorData.Id].count >= behaviorData.Count)
                {
                    return 2;
                }
            }

            ActivityConversionTypeDefine define = m_activityProxy.GetConversionTypeDefine(behaviorData.Id);
            if (define.conversionItem != null && define.num != null)
            {
                if (define.conversionItem.Count == define.num.Count)
                {
                    int total = define.conversionItem.Count;
                    int enoughCount = 0;
                    for (int k = 0; k < total; k++)
                    {
                        Int64 itemCount = m_bagProxy.GetItemNum(define.conversionItem[k]);
                        if (itemCount >= define.num[k])
                        {
                            enoughCount = enoughCount + 1;
                        }
                    }
                    if (enoughCount >= total)
                    {
                        return 1;
                    }
                }
            }
            return 2;
        }

        private void Refresh()
        {
            m_UI_Model_EventType.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);
            ClientUtils.TextSetColor(m_UI_Model_EventType.m_lbl_title_LanguageText, m_menuData.Define.taglineColour);
            m_UI_Model_EventType.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);
            ClientUtils.TextSetColor(m_UI_Model_EventType.m_lbl_name_LanguageText, m_menuData.Define.nameColour);
            ClientUtils.LoadSprite(m_UI_Model_EventType.m_img_top_PolygonImage, m_menuData.Define.background);

            ActivityTimeInfo activityInfo = m_menuData.Data;
            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
            DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
            m_UI_Model_EventType.m_lbl_lifeDay_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

            m_UI_Model_EventType.m_lbl_reset_LanguageText.text = LanguageUtils.getText(762021);

            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (activityInfo.endTime > serverTime)
            {
                m_endTime = activityInfo.endTime;
                UpdateTime();
                StartTimer();
            }
            else
            {
                CancelTimer();
            }

            RefreshList();
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_UI_Model_EventType.m_sv_list_ListView);
            }
        }

        //取消定时器
        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void UpdateTime()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 diffTime = m_endTime - serverTime;
            if (diffTime < 0)
            {
                //倒计时结束 关闭界面
                CancelTimer();
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
            }
            else
            {
                m_UI_Model_EventType.m_lbl_lifeTime_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        public void RefreshList()
        {
            if (m_isInitList)
            {
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                m_UI_Model_EventType.m_sv_list_ListView.SetInitData(m_assetDic, functab);
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
                m_isInitList = true;
            }
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            var behaviorData = m_behaviorList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_EventType1Item_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickCallback = ExchangeBtnRecord;
                subView.Refresh(behaviorData, m_preScheduleData);
            }
            else
            {
                var subView = listItem.data as UI_Item_EventType1Item_SubView;
                subView.Refresh(behaviorData, m_preScheduleData);
            }
        }

        private void OnExchange(bool isRemind)
        {
            //0提醒 1不提醒
            int num = 0;
            if (!isRemind)
            {
                num = 2;
            }
            m_activityProxy.SetExchangeRemindStatus(num, true);
        }

        private void ExchangeProcess(object body)
        {
            var result = body as Activity_Exchange.response;
            RectTransform rectTrans = null;
            if (result.id > 0)
            {
                m_exchangeRecord.TryGetValue(result.id, out rectTrans);
                m_exchangeRecord.Remove(result.id);
            }
            if (rectTrans == null)
            {
                return;
            }
            //飘飞特效
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (result.rewardInfo.items != null)
            {
                for (int i = 0; i < result.rewardInfo.items.Count; i++)
                {
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)result.rewardInfo.items[i].itemId);
                    if (itemDefine != null)
                    {
                        string str = LanguageUtils.getTextFormat(700025,
                                                                 LanguageUtils.getText(itemDefine.l_nameID),
                                                                 ClientUtils.FormatComma(result.rewardInfo.items[i].itemNum));
                        Tip.CreateTip(str).Show();
                    }

                    mt.FlyItemEffect((int)result.rewardInfo.items[i].itemId,
                                     (int)result.rewardInfo.items[i].itemNum,
                                     rectTrans);
                }
            }
            if (m_isInitList)
            {
                m_UI_Model_EventType.m_sv_list_ListView.ForceRefresh();
            }
        }

        //行为事件奖励兑换按钮记录
        private void ExchangeBtnRecord(UI_Item_EventType1Item_SubView subView)
        {
            int eventId = subView.GetEventId();
            m_exchangeRecord[eventId] = subView.m_btn_exchange.m_root_RectTransform;
        }
    }
}