// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    EventDateMediator 活动日历主界面
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
using System;

namespace Game {

    public enum EnumActivityItemShowType
    {
        Title = 1,    //标题 
        Activity = 2, //活动
        Date = 3,     //日历
        Hell = 4,     //地狱活动
    }

    public class ActivityItemData
    {
        public EnumActivityItemShowType ShowType;                   //EnumActivityItemShowType
        public string Title;                                        //活动标题
        public ActivityCalendarDefine Define;                       //活动模版数据
        public ActivityTimeInfo Data;                               //活动数据
    }

    public class EventDateMediator : GameMediator {
        #region Member
        public static string NameMediator = "EventDateMediator";

        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        public List<ActivityItemData> m_menuDataList;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private bool m_isInit;

        private float m_menuTitleHeight;
        private float m_menuDataHeight;

        private int m_selectIndex;
        private object m_selectMenuSubView;
        private EnumActivityItemShowType m_selectMenuType;

        private GameObject m_contentNode;
        private object m_contentSubView;
        private string m_contentAssetName;

        private List<ActivityCalendarData> m_activityCalendarList; //活动日期列表
        private bool m_isInitDateList;

        private List<UI_Item_EventDateDateItem_SubView> m_eventDateTitleList;

        private bool m_isJumpHandle;
        private int m_jumpItemIndex;

        #endregion

        //IMediatorPlug needs
        public EventDateMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public EventDateView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                CmdConstant.UpdateActivityReddot,
                CmdConstant.ActivitySwitch,
                CmdConstant.ActivityScheduleUpdate,
                CmdConstant.RefreshActivityNewFlag,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateActivityReddot: //红点刷新 
                    if (!m_isInit)
                    {
                        return;
                    }
                    Int64 activityId = (Int64)notification.Body;
                    RefreshMenuRedpot(activityId);
                    break;
                case CmdConstant.ActivitySwitch:
                    {
                        Int64 acId = (Int64)notification.Body;
                        SwitchActivityById(acId);
                    }
                    break;
                case CmdConstant.ActivityScheduleUpdate:
                    ScheduleUpdate(notification.Body);
                    break;
                case CmdConstant.RefreshActivityNewFlag:
                    {
                        Int64 acId = (Int64)notification.Body;
                        RefreshActivityNewFlag(acId);
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

            view.m_pl_pos.gameObject.SetActive(false);
            view.m_pl_date_ArabLayoutCompment.gameObject.SetActive(false);

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_sv_content_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            //view.m_btn_close_GameButton.onClick.AddListener(Close);
            view.m_btn_mask_GameButton.onClick.AddListener(CloseTip);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            m_menuTitleHeight = m_assetDic["UI_Item_EventDateListLine"].GetComponent<RectTransform>().rect.height;
            m_menuDataHeight = m_assetDic["UI_Item_EventDateListItem"].GetComponent<RectTransform>().rect.height;

            ReadData();

            m_jumpItemIndex = -1;
            m_isJumpHandle = false;
            m_selectIndex = 0;
            if (view.data != null)
            {
                int group = (int)view.data;
                for (int i = 0; i < m_menuDataList.Count; i++)
                {
                    if (m_menuDataList[i].Define != null && m_menuDataList[i].Define.group == group)
                    {
                        m_selectIndex = i;
                        m_jumpItemIndex = i;
                        m_isJumpHandle = true;
                        break;
                    }
                }
            }
            else
            {
                //切换到上次选中的活动
                int findIndex = CheckLastSelectActivity();
                if (findIndex > 0)
                {
                    m_selectIndex = findIndex;
                    m_jumpItemIndex = findIndex;
                    m_isJumpHandle = true;
                }
            }

            InitMenuList();

            RefreshContent(m_menuDataList[m_selectIndex], true);

            m_isInit = true;
        }

        //点击菜单
        private void ClickMenuItem(UI_Item_EventDateListItem_SubView subView)
        {
            if (subView.Index == m_selectIndex)
            {
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.SwitchActivityMenu);

            HideLastSelectMenuSubView();

            subView.SetSelectStatus(true);
            m_selectMenuSubView = subView;
            m_selectMenuType = subView.ItemData.ShowType;
            m_selectIndex = subView.Index;

            if (m_menuDataList[m_selectIndex].Define != null)
            {
                int type = m_menuDataList[m_selectIndex].Define.activityType;
                if (type == 200)
                {
                    ActivityScheduleData scheduleData = m_activityProxy.GetActivitySchedule(m_menuDataList[m_selectIndex].Define.ID);
                    if (scheduleData.ActivityType2 != 5)
                    {
                        scheduleData.ActivityType2 = 5;
                        if (scheduleData.GetReddotNum() > 0)
                        {
                            scheduleData.IsReddotChange = true;
                            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityTotalReddot);
                            subView.RefreshRedpot();
                        }
                    }
                }
            }

            RefreshContent(subView.ItemData, true);
        }

        private void ClickMenuItem2(UI_Item_EventDateListItemPb_SubView subView)
        {
            if (subView.Index == m_selectIndex)
            {
                return;
            }
            HideLastSelectMenuSubView();

            subView.SetSelectStatus(true);
            m_selectMenuSubView = subView;
            m_selectMenuType = subView.ItemData.ShowType;
            m_selectIndex = subView.Index;

            RefreshContent(subView.ItemData, false);
        }

        private void HideLastSelectMenuSubView()
        { 
            if(m_selectMenuSubView == null)
            {
                return;
            }
            if (m_selectMenuSubView is UI_Item_EventDateListItem_SubView)
            {
                var sView = m_selectMenuSubView as UI_Item_EventDateListItem_SubView;
                sView.SetSelectStatus(false);
                return;
            }
            if (m_selectMenuSubView is UI_Item_EventDateListItemPb_SubView)
            {
                var sView = m_selectMenuSubView as UI_Item_EventDateListItemPb_SubView;
                sView.SetSelectStatus(false);
            }
        }

        private void RefreshContent(ActivityItemData itemData, bool isForceRefresh)
        {
            if (itemData.Define != null)
            {
                Debug.LogFormat("activityId:{0}", itemData.Define.ID);
            }
            if (itemData.ShowType == EnumActivityItemShowType.Date)
            {
                RefershCalendarContent(itemData, isForceRefresh);
                m_activityProxy.SetLastSelectActivityId(0);
            }
            else
            {
                RefreshActivityContent(itemData);
                m_activityProxy.SetLastSelectActivityId(itemData.Define.ID);
            }
        }

        #region 菜单刷新

        //初始化菜单列表
        private void InitMenuList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = MenuItemByIndex;
            functab.GetItemPrefabName = OnGetItemPrefabName;
            functab.GetItemSize = OnGetItemSize;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_ListView.FillContent(m_menuDataList.Count);
            if (m_isJumpHandle && m_jumpItemIndex >=0)
            {
                m_isJumpHandle = false;
                view.m_sv_list_ListView.ScrollList2IdxCenter(m_jumpItemIndex);
            }
        }

        private void RefreshMenuList()
        {
            view.m_sv_list_ListView.FillContent(m_menuDataList.Count);
        }

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            if (m_menuDataList[listItem.index].ShowType == EnumActivityItemShowType.Title)
            {
                return "UI_Item_EventDateListLine";
            }
            else if (m_menuDataList[listItem.index].ShowType == EnumActivityItemShowType.Hell)
            {
                return "UI_Item_EventDateListItemPb";
            }
            else
            {
                return "UI_Item_EventDateListItem";
            }
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            if (m_menuDataList[listItem.index].ShowType == EnumActivityItemShowType.Title)
            {
                return m_menuTitleHeight;
            }
            else
            {
                return m_menuDataHeight;
            }
        }

        //刷新菜单item
        private void MenuItemByIndex(ListView.ListItem listItem)
        {
            var itemData = m_menuDataList[listItem.index];
            if (itemData.ShowType == EnumActivityItemShowType.Title)
            {
                if (listItem.data == null)
                {
                    var subView = new UI_Item_EventDateListLine_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.RefreshItem(itemData);
                }
                else
                {
                    var subView = listItem.data as UI_Item_EventDateListLine_SubView;
                    subView.RefreshItem(itemData);
                }
            }
            else if (itemData.ShowType == EnumActivityItemShowType.Hell) //地狱活动
            {
                UI_Item_EventDateListItemPb_SubView subView;
                if (listItem.data == null)
                {
                    subView = new UI_Item_EventDateListItemPb_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.BtnListener = ClickMenuItem2;
                    subView.AddBtnListener();
                }
                else
                {
                    subView = listItem.data as UI_Item_EventDateListItemPb_SubView;
                }
                if (m_selectIndex == listItem.index)
                {
                    m_selectMenuSubView = subView;
                    m_selectMenuType = itemData.ShowType;
                    subView.RefreshItem(listItem.index, itemData, true);
                }
                else
                {
                    subView.RefreshItem(listItem.index, itemData, false);
                }
            }
            else //其它活动
            {
                UI_Item_EventDateListItem_SubView subView;
                if (listItem.data == null)
                {
                    subView = new UI_Item_EventDateListItem_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.BtnListener = ClickMenuItem;
                    subView.AddBtnListener();
                }
                else
                {
                    subView = listItem.data as UI_Item_EventDateListItem_SubView;
                }
                if (m_selectIndex == listItem.index)
                {
                    m_selectMenuSubView = subView;
                    m_selectMenuType = itemData.ShowType;
                    subView.RefreshItem(listItem.index, itemData, true);
                }
                else
                {
                    subView.RefreshItem(listItem.index, itemData, false);
                }
            }
        }

        //刷新菜单红点
        private void RefreshMenuRedpot(Int64 activityId)
        {
            if (m_menuDataList != null)
            {
                int findIndex = -1;
                for (int i = 0; i < m_menuDataList.Count; i++)
                {
                    if (m_menuDataList[i].Define != null && m_menuDataList[i].Define.ID == activityId)
                    {
                        findIndex = i;
                        break;
                    }
                }
                if (findIndex >= 0)
                {
                    ListView.ListItem listItem = view.m_sv_list_ListView.GetItemByIndex(findIndex);
                    if (listItem == null || listItem.data == null)
                    {
                        return;
                    }
                    if (!listItem.go.activeSelf)
                    {
                        return;
                    }
                    var itemData = m_menuDataList[findIndex];
                    if (itemData.ShowType == EnumActivityItemShowType.Activity)
                    {
                        UI_Item_EventDateListItem_SubView subView = listItem.data as UI_Item_EventDateListItem_SubView;
                        subView.RefreshRedpot();
                    }
                    else if (itemData.ShowType == EnumActivityItemShowType.Hell)
                    {
                        UI_Item_EventDateListItemPb_SubView subView = listItem.data as UI_Item_EventDateListItemPb_SubView;
                        subView.RefreshRedpot();
                        subView.RefreshProgress();
                    }
                }
            }
        }

        #endregion

        #region 刷新日历内容

        private void RefershCalendarContent(ActivityItemData itemData, bool isForceRefresh)
        {
            m_contentAssetName = "";
            view.m_pl_date_ArabLayoutCompment.gameObject.SetActive(true);
            view.m_pl_event_ArabLayoutCompment.gameObject.SetActive(false);

            if (isForceRefresh)
            {
                RefreshDateTitle();
                RefershCalendarList();
            }

            if (TipRemindProxy.IsShowRemind(TipRemindProxy.ActivityCalendarReddotTotal))
            {
                TipRemindProxy.SaveRemind(TipRemindProxy.ActivityCalendarReddotTotal);
                Int64 id = -1;
                AppFacade.GetInstance().SendNotification(CmdConstant.RefreshActivityNewFlag, id);
            }
        }

        //刷新日历标题
        private void RefreshDateTitle()
        {
            if (m_eventDateTitleList == null)
            {
                m_eventDateTitleList = new List<UI_Item_EventDateDateItem_SubView>();
                m_eventDateTitleList.Add(view.m_UI_DateItem0);
                m_eventDateTitleList.Add(view.m_UI_DateItem1);
                m_eventDateTitleList.Add(view.m_UI_DateItem2);
                m_eventDateTitleList.Add(view.m_UI_DateItem3);
                m_eventDateTitleList.Add(view.m_UI_DateItem4);
                m_eventDateTitleList.Add(view.m_UI_DateItem5);
                m_eventDateTitleList.Add(view.m_UI_DateItem6);
            }
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 startTime = m_activityProxy.GetWeekStartTime();
            DateTime currTime = ServerTimeModule.Instance.ConverToServerDateTime(serverTime);
            long showTime = 0;
            int count = m_eventDateTitleList.Count;
            for (int i = 0; i < count; i++)
            {
                showTime = startTime + i * 86400;
                DateTime time = ServerTimeModule.Instance.ConverToServerDateTime(showTime);
                m_eventDateTitleList[i].SetHighLight(currTime.Day == time.Day);
                m_eventDateTitleList[i].m_lbl_day_LanguageText.text = LanguageUtils.getTextFormat(762010, time.Month, time.Day);
            }
        }

        private void RefershCalendarList()
        {
            if (m_isInitDateList)
            {
                view.m_sv_content_ListView.FillContent(m_activityCalendarList.Count);
            }
            else
            {
                m_isInitDateList = true;
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemDateProgressRefresh;
                view.m_sv_content_ListView.SetInitData(m_assetDic, functab);
                view.m_sv_content_ListView.FillContent(m_activityCalendarList.Count);
            }
        }

        //刷新日历进度item
        private void ItemDateProgressRefresh(ListView.ListItem listItem)
        {
            var itemData = m_activityCalendarList[listItem.index];
            UI_Item_EventDateEventLife_SubView subView;
            if (listItem.data == null)
            {
                subView = new UI_Item_EventDateEventLife_SubView(listItem.go.GetComponent<RectTransform>());
                subView.BtnListener = ClickDateProgress;
                subView.AddBtnListener();
            }
            else
            {
                subView = listItem.data as UI_Item_EventDateEventLife_SubView;
            }
            subView.RefreshItem(itemData);
        }

        private void ClickDateProgress(UI_Item_EventDateEventLife_SubView subView)
        {
            view.m_btn_mask_GameButton.gameObject.SetActive(true);
            view.m_pl_pos.gameObject.SetActive(true);
            ShowDateTip(subView);
        }

        private void CloseTip()
        {
            view.m_btn_mask_GameButton.gameObject.SetActive(false);
            view.m_pl_pos.gameObject.SetActive(false);
        }

        #endregion

        #region 日历tip信息

        private void ShowDateTip(UI_Item_EventDateEventLife_SubView subView)
        {
            ActivityCalendarData activityInfo = subView.GetActivityInfo();
            ActivityTimeInfo activityData = activityInfo.ActivityInfo;
            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)activityData.activityId);

            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_nameID);
            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, define.icon);
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(define.l_sketchID);


            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityData.startTime);
            DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityData.endTime);
            //Debug.LogErrorFormat("活动id:{0} startTime:{1} endTime:{2} {3} {4}", 
            //                     activityData.activityId, activityData.startTime, activityData.endTime,
            //                     startTime, endTime);
            view.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762012, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(define.itemPackage);
            List<UI_Model_Item_SubView> subViewList = new List<UI_Model_Item_SubView>();
            subViewList.Add(view.m_UI_Model_Item1);
            subViewList.Add(view.m_UI_Model_Item2);
            subViewList.Add(view.m_UI_Model_Item3);
            subViewList.Add(view.m_UI_Model_Item4);
            subViewList.Add(view.m_UI_Model_Item5);
            int count = groupDataList.Count;
            for (int i = 0; i < 5; i++)
            {
                if (i < count)
                {
                    subViewList[i].gameObject.SetActive(true);

                    if (groupDataList[i].ItemData != null)
                    {
                        subViewList[i].RefreshByGroup(groupDataList[i], 3);
                    }
                }
                else
                {
                    subViewList[i].gameObject.SetActive(false);
                }
            }

            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();

            if (serverTime >= activityData.startTime && serverTime <= activityData.endTime)
            {
                view.m_btn_go.gameObject.SetActive(true);
            }
            else
            {
                view.m_btn_go.gameObject.SetActive(false);
            }
            view.m_btn_go.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            view.m_btn_go.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                OnGo(activityInfo);
            });

            ChangeTipPos(subView);
        }

        private void ChangeTipPos(UI_Item_EventDateEventLife_SubView subView)
        {
            Vector2 localPos;
            Vector3 pos = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(subView.m_img_icon_PolygonImage.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.m_pl_pos.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform viewRect = view.gameObject.GetComponent<RectTransform>();

            var rect = view.m_pl_offset_Animator.transform.GetComponent<RectTransform>().rect;

            Transform targetTrans = view.m_pl_offset_Animator.transform;

            float diffNum = 20f;

            view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(false);
            view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(false);
            view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(false);
            view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(false);

            // 左
            if (localPos.x < viewRect.rect.width / 2)
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y = localPos.y - (rect.height / 2) - diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideTop_PolygonImage.transform.localPosition = new Vector2(offset,
                                                                           view.m_img_arrowSideTop_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                                                   view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x + (rect.width / 2) + diffNum;
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                }
            }
            // 右
            else
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.y - (viewRect.rect.height - rect.height / 2);
                        view.m_img_arrowSideR_PolygonImage.transform.localPosition = new Vector2(view.m_img_arrowSideR_PolygonImage.transform.localPosition.x,
                                                                                                 offset);
                        view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);

                        localPos.x = localPos.x - (rect.width / 2) - diffNum;
                        localPos.y = viewRect.rect.height - rect.height / 2;
                    }
                    else
                    {
                        localPos.y = localPos.y - (rect.height / 2) - diffNum;
                        view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                    }
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.x - (viewRect.rect.width - rect.width / 2);
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                                                                                                       view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x - (rect.width / 2) - diffNum;
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                }
            }

            targetTrans.localPosition = localPos;
        }

        private void SwitchActivityById(Int64 activityId)
        {
            Debug.LogError("SwitchActivityById:" + activityId);
            ReadData();
            RefreshMenuList();
            RefershCalendarContent(m_menuDataList[0], true);
            int lastIndex = FindIndexById(activityId);

            if (lastIndex >= 0)
            {
                JumpToMenuByIndex(lastIndex);
            }
        }

        //前往
        private void OnGo(ActivityCalendarData activityInfo)
        {
            CloseTip();

            ActivityTimeInfo activityData = activityInfo.ActivityInfo;
            Int64 activityId = activityData.activityId;
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();

            if (serverTime >= activityData.startTime && serverTime <= activityData.endTime) //在活动区间
            {
                int index = FindIndexById(activityId);
                if (index >= 0)
                {
                    JumpToMenuByIndex(index);
                    return;
                }
            }

            //左侧没找到对应活动 重新读取数据
            ReadData();
            RefreshMenuList();
            RefershCalendarContent(m_menuDataList[0], true);
            int lastIndex = FindIndexById(activityId);

            //重新查找左侧列表是否有该活动
            if (lastIndex >= 0)
            {
                JumpToMenuByIndex(lastIndex);
            }
        }

        //查找左侧列表是否有该活动
        private int FindIndexById(Int64 activityId)
        {
            int index = -1;
            for (int i = 0; i < m_menuDataList.Count; i++)
            {
                if (m_menuDataList[i].Data != null && m_menuDataList[i].Data.activityId == activityId)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void JumpToMenuByIndex(int index)
        {
            view.m_sv_list_ListView.ScrollList2IdxCenter(index);
            ListView.ListItem listItem = view.m_sv_list_ListView.GetItemByIndex(index);
            if (listItem != null)
            {
                UI_Item_EventDateListItem_SubView subView = listItem.data as UI_Item_EventDateListItem_SubView;
                ClickMenuItem(subView);
            }
        }

        #endregion

        #region 刷新活动内容

        //刷新活动内容
        private void RefreshActivityContent(ActivityItemData itemData)
        {
            view.m_pl_date_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_event_ArabLayoutCompment.gameObject.SetActive(true);

            if (m_contentNode != null)
            {
                CoreUtils.assetService.Destroy(m_contentNode);
                m_contentNode = null;
            }
            m_contentSubView = null;
            m_contentAssetName = GetContentAssetName(itemData.Define.activityType);
            CoreUtils.assetService.Instantiate(m_contentAssetName, ContentPrefabLoadFinish);
        }

        private void ContentPrefabLoadFinish(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            string str = obj.name.Replace("(Clone)", "");
            if (m_contentAssetName != str)
            {
                CoreUtils.assetService.Destroy(obj);
                return;
            }
            if (m_contentNode != null && m_contentNode.name == obj.name)
            {
                CoreUtils.assetService.Destroy(obj);
                return;
            }
            obj.transform.SetParent(view.m_pl_event_ArabLayoutCompment.gameObject.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.gameObject.SetActive(false);
            RectTransform rectTrans = obj.GetComponent<RectTransform>();
            rectTrans.offsetMin = Vector2.zero;
            rectTrans.offsetMax = Vector2.zero;
            m_contentNode = obj;

            int type = m_menuDataList[m_selectIndex].Define.activityType;
            if (type == 100) //开服活动 预显示
            {
                var subView = new UI_Item_EventTypeStartServer_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            if (type == 101)//开服活动 正式开启
            {
                var subView = new UI_Item_EventTypeStartServer_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 200) //通用兑换类活动
            {
                var subView = new UI_Item_EventTypeCom1_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 300) //最强执政官 预告
            {
                var subView = new UI_Item_EventStrongerPlayer1_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 301 || type == 302) //最强执政官 进行中和结束
            {
                var subView = new UI_Item_EventStrongerPlayer2_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 303 || type == 304 || type ==305 || type == 306)//战斗号角&部落之王
            {
                var subView = new UI_Item_EventTribeKing_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 400) //通用条件达成类活动 基础达标类型活动界面
            {
                var subView = new UI_Item_EventTypeCom2_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 401) //通用条件达成类活动 每日重置达标类型
            {
                var subView = new UI_Item_EventTypeCom2_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 402 || type == 403 || type == 500 || type == 501 || type == 502) // 通用条件达成类活动 达标+排行界面 掉落类活动
            {
                var subView = new UI_Item_EventTypeCom3_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 600)//幸运大转盘
            {
                var subView = new UI_Item_EventTurntable_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 700) //洛哈的试炼
            {
                var subView = new UI_Item_EventNarmer_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }
            else if (type == 800)//地狱活动
            {
                var subView = new UI_Item_EventTypeHell_SubView(rectTrans);
                m_contentSubView = subView;
                subView.Init(m_menuDataList[m_selectIndex]);
            }

            //是否请求清除新活动标志
            ActivityScheduleData scheduleData = m_activityProxy.GetActivitySchedule(m_menuDataList[m_selectIndex].Define.ID);
            if (scheduleData != null && scheduleData.IsNewActivity())
            {
                //请求数据 todo  
                var sp = new Activity_ClickActivity.request();
                sp.activityId = m_menuDataList[m_selectIndex].Define.ID;
                AppFacade.GetInstance().SendSproto(sp);
            } 
        }

        private string GetContentAssetName(int type)
        {
            Debug.LogFormat("type:{0}",type);
            string assetName = "";
            if (type == 100) //开服活动
            {
                assetName = "UI_Item_EventTypeStartServer";
            }
            if (type == 101)
            {
                assetName = "UI_Item_EventTypeStartServer";
            }
            else if (type == 200)
            {
                assetName = "UI_Item_EventTypeCom1";
            }
            else if (type == 300)
            {
                assetName = "UI_Item_EventStrongerPlayer1";
            }
            else if (type == 301 || type == 302)
            {
                assetName = "UI_Item_EventStrongerPlayer2";
            }
            else if (type == 303 || type == 304 || type == 305 || type == 306)//战斗号角&部落之王
            {
                assetName = "UI_Item_EventTribeKing";
            }
            else if (type == 400)
            {
                assetName = "UI_Item_EventTypeCom2";
            }
            else if (type == 401)
            {
                assetName = "UI_Item_EventTypeCom2";
            }
            else if (type == 402 || type == 403 || type == 500 || type == 501 || type == 502)
            {
                assetName = "UI_Item_EventTypeCom3";
            }
            else if (type == 600)
            {
                assetName = "UI_Item_EventTurntable";
            }
            else if (type == 700)
            {
                assetName = "UI_Item_EventNarmer";
            }
            else if (type == 800)
            {
                assetName = "UI_Item_EventTypeHell";
            }
            return assetName;
        }
        #endregion

        //更新活动行为进度 红点
        private void ScheduleUpdate(object body)
        {
            //刷新列表中包含 被选中的菜单内容
            if (m_menuDataList != null && m_selectIndex < m_menuDataList.Count)
            {
                if (m_menuDataList[m_selectIndex].Define == null)
                {
                    return;
                }
                List<Int64> list = body as List<Int64>;
                if (list.Count > 0)
                {
                    int acId = m_menuDataList[m_selectIndex].Define.ID;
                    int activityType = m_menuDataList[m_selectIndex].Define.activityType;
                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (list[i] == acId)
                        {
                            if (activityType == 401) //每日重置
                            {
                                //刷新菜单红点
                                RefreshMenuRedpot(acId);
                                //刷新内容
                                if (m_contentSubView != null)
                                {
                                    UI_Item_EventTypeCom2_SubView subView = m_contentSubView as UI_Item_EventTypeCom2_SubView;
                                    subView.RefreshSchedule();
                                }
                                break;
                            }
                            else if (activityType == 301) //最强执政官 进行中
                            {
                                //刷新菜单红点
                                RefreshMenuRedpot(acId);
                                //刷新内容
                                if (m_contentSubView != null)
                                {
                                    UI_Item_EventStrongerPlayer2_SubView subView = m_contentSubView as UI_Item_EventStrongerPlayer2_SubView;
                                    subView.RefreshView();
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        //读取数据
        private void ReadData()
        {
            //读取菜单数据列表
            ReadMenuData();

            //读取日历列表数据
            ReadCalendarData();
        }

        private void ReadCalendarData()
        {
            m_activityCalendarList = m_activityProxy.GetActivityDateList();
        }

        private void ReadMenuData()
        {
            if (m_menuDataList == null)
            {
                m_menuDataList = new List<ActivityItemData>();
            }
            else
            {
                m_menuDataList.Clear();
            }

            //活动日历
            ActivityItemData item1 = new ActivityItemData();
            item1.ShowType = EnumActivityItemShowType.Date;
            item1.Title = LanguageUtils.getText(762166);
            m_menuDataList.Add(item1);

            List<ActivityItemData> hotMenuDataList = new List<ActivityItemData>();
            List<ActivityItemData> normalMenuDataList = new List<ActivityItemData>();

            List<ActivityTimeInfo> dataList = m_activityProxy.GetActivityMenuList();
            bool isHasHot = false;
            bool isHasNomal = false;
            for (int i = 0; i < dataList.Count; i++)
            {
                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)dataList[i].activityId);
                if (define == null)
                {
                    continue;
                }
                if (define.page == 1) //热门活动
                {
                    if (!isHasHot)
                    {
                        isHasHot = true;
                        ActivityItemData item2 = new ActivityItemData();
                        item2.ShowType = EnumActivityItemShowType.Title;
                        item2.Title = LanguageUtils.getText(762001);
                        hotMenuDataList.Add(item2);
                    }
                    ActivityItemData item3 = new ActivityItemData();
                    if (define.activityType == 800)
                    {
                        item3.ShowType = EnumActivityItemShowType.Hell;
                    }
                    else
                    {
                        item3.ShowType = EnumActivityItemShowType.Activity;
                    }
                    item3.Define = define;
                    item3.Data = dataList[i];
                    hotMenuDataList.Add(item3);
                }
                else if (define.page == 2) //日常活动
                {
                    if (!isHasNomal)
                    {
                        isHasNomal = true;
                        ActivityItemData item2 = new ActivityItemData();
                        item2.ShowType = EnumActivityItemShowType.Title;
                        item2.Title = LanguageUtils.getText(762002);
                        normalMenuDataList.Add(item2);
                    }
                    ActivityItemData item3 = new ActivityItemData();
                    if (define.activityType == 800)
                    {
                        item3.ShowType = EnumActivityItemShowType.Hell;
                    }
                    else
                    {
                        item3.ShowType = EnumActivityItemShowType.Activity;
                    }
                    item3.Define = define;
                    item3.Data = dataList[i];
                    normalMenuDataList.Add(item3);
                }
            }
            if (hotMenuDataList.Count > 0)
            {
                m_menuDataList.AddRange(hotMenuDataList);
            }
            if (normalMenuDataList.Count > 0)
            {
                m_menuDataList.AddRange(normalMenuDataList);
            }
        }

        public object GetContentSubview()
        {
            return m_contentSubView;
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_eventDate);
        }

        //刷新活动new标识
        private void RefreshActivityNewFlag(Int64 activityId)
        {
            if (activityId == -1)
            {
                view.m_sv_list_ListView.RefreshItem(0);
            }
            else if(activityId >=0)
            {
                int lastIndex = FindIndexById(activityId);
                view.m_sv_list_ListView.RefreshItem(lastIndex);
            }
        }

        private int CheckLastSelectActivity()
        {
            int findIndex = -1;
            int activityId = m_activityProxy.GetLastSelectActivityId();
            if (activityId < 0)
            {
                findIndex = -1;

            }
            else if (activityId == 0)
            {
                findIndex = 0;
            }
            else
            {
                findIndex = FindIndexById(activityId);
            }
            if (findIndex < 0)
            {
                for (int i = 0; i < m_menuDataList.Count; i++)
                {
                    if (m_menuDataList[i].Define != null && m_menuDataList[i].Define.page == 1)
                    {
                        findIndex = i;
                        break;
                    }
                }
                if (findIndex < 0)
                {
                    findIndex = 0;
                }
            }
            return findIndex;
        }
    }
}