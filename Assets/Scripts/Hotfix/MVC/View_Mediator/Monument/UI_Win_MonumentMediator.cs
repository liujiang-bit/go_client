// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Win_MonumentMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game
{
    public class UI_Win_MonumentMediator : GameMediator
    {
        
        enum EventState
        {
            NotOpen, // 未开启
            Opend, //已开启
            Doing, // 进行中
            Closed, // 已关闭
        }

        enum RewardAcceptState
        {
            CannotAccept, // 不可领取
            CanAccept, // 可领取
            Accepted, // 已领取
        }

        enum ReceiveRewardState
        {
            CanReceive, // 可领取
            Received, // 已领取
            CannotReceive, // 未达到领取条件
        }

        class MonumentProject_Item
        {
            public List<UI_Model_Item_SubView> items = new List<UI_Model_Item_SubView>();
            public List<UI_Model_MonumentItem_SubView> monumentItems = new List<UI_Model_MonumentItem_SubView>();
            public Timer timer;
            // 可领奖特效
            public GameObject effect;
        }

        Color canReceiveColor = new Color(102.0f / 255.0f, 205.0f / 255.0f, 0);
        Color receivedColor = new Color(169.0f / 255.0f, 169.0f / 255.0f, 169.0f / 255.0f);
        Color cannotReceiveColor = new Color(205.0f / 255.0f, 0, 0);
        
        #region 多语言ID

        public int lan_getRewardType1 = 183001;
        public int lan_getRewardType2 = 183002;
        public int lan_getRewardType3 = 183003;
        public int lan_getRewardType4 = 183004;
        
        public int lan_getRewardTypeStatus1 = 183008;
        public int lan_getRewardTypeStatus2 = 183009;
        public int lan_getRewardTypeStatus3 = 183009;
        public int lan_getRewardTypeStatus4 = 183010;

        public int lan_rewardCanReceive = 183007; // 奖励可领取
        public int lan_rewardReceived = 762150; // 已领取
        public int lan_showAlliance = 183011; // 查看联盟

        public int lan_notOpen = 183012; // 暂未开启
        public int lan_surplusTime = 762019; // 剩余时间
        public int lan_progress = 730290; // 进度多语言

        public int lan_finishAt = 183013; // 目标达成于

        public int lan_chapterFinish = 183014; // 章节已结束

        #endregion

        #region Member

        public static string NameMediator = "UI_Win_MonumentMediator";

        private List<EvolutionMileStoneDefine> m_evolutionMileStoneDefine = new List<EvolutionMileStoneDefine>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private Dictionary<int, Role_GetMonument.response.MonumentList> monumentLists =
            new Dictionary<int, Role_GetMonument.response.MonumentList>();

        private Dictionary<int, List<int>> m_groupItems = new Dictionary<int, List<int>>();

        private RewardGroupProxy m_rewardGroupProxy;
        private RectTransform[] m_curRewardRectTransforms;

        private MonumentProject_Item rewardingItem;

        private float m_eventTimeDownRefresh;
        private int m_curEventRefreshIndex = -1;
        private bool m_wantDelaySendGetMonumentReq;
        private float m_wantDelaySendGetMonumentReqTimer;

        private bool m_isInitedListView;
        
        #endregion

        //IMediatorPlug needs
        public UI_Win_MonumentMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_MonumentView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Role_GetMonument.TagName,
                Role_GetMonumentReward.TagName,
                Monument_End.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_GetMonument.TagName:
                    Role_GetMonument.response res1 = notification.Body as Role_GetMonument.response;
                    QueryGetMonumentEndRes(res1);
                    break;
                case Role_GetMonumentReward.TagName:
                    Role_GetMonumentReward.response res = notification.Body as Role_GetMonumentReward.response;
                    QueryGetMonumentRes(res);
                    break;
                case Monument_End.TagName:
                    // 事件结束，重新查询纪念碑事件
                    SendGetMonumentReq();
                    break;
                default:
                    break;
            }
        }


        #region UI template method

        public override void OpenAniEnd()
        {
        }

        public override void WinFocus()
        {
            PlayerProxy.IsMonumentflag = false;
        }

        public override void WinClose()
        {
        }

        public override void PrewarmComplete()
        {
        }

        protected override void InitData()
        {
            this.IsOpenUpdate = true;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

            List<EvolutionMileStoneDefine> defines = CoreUtils.dataService.QueryRecords<EvolutionMileStoneDefine>();
            m_evolutionMileStoneDefine.AddRange(defines);
            m_evolutionMileStoneDefine.Sort((a, b) => { return a.order - b.order; });

            view.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_monument);
            });
            
            SendGetMonumentReq();
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
        }

        #endregion

        private void SendGetMonumentReq()
        {
            Role_GetMonument.request req = new Role_GetMonument.request();
            AppFacade.GetInstance().SendSproto(req);
        }

        /// <summary>
        /// 获取纪念碑信息回包
        /// </summary>
        private void QueryGetMonumentEndRes(Role_GetMonument.response res)
        {
            monumentLists.Clear();
            foreach (var list in res.monumentList)
            {
                monumentLists.Add((int) list.Value.id, list.Value);
            }

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        /// <summary>
        /// 领取纪念碑奖励回包
        /// </summary>
        private void QueryGetMonumentRes(Role_GetMonumentReward.response res)
        {
            RewardInfo rewardInfo = res.rewardInfo;
            
            // 领奖飘飞效果
            MonumentProject_Item tmp = rewardingItem;
            rewardingItem = null;
            List<RewardGroupData> groupDatas = m_rewardGroupProxy.GetRewardDataByRewardInfo(rewardInfo);
            UIHelper.FlyRewardEffect(GetRewardItemRectTransforms(tmp, groupDatas), rewardInfo);
            SendGetMonumentReq();
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            InitListView();
        }

        private void InitListView()
        {
            if (m_isInitedListView)
            {
                view.m_sv_list_view_ListView.FillContent(m_evolutionMileStoneDefine.Count);
                view.m_sv_list_view_ListView.ForceRefresh();
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ListViewItemByIndex;
                functab.ItemRemove = ListViewOnItemRemove; 
                view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
                view.m_sv_list_view_ListView.FillContent(m_evolutionMileStoneDefine.Count);
                view.m_sv_list_view_ListView.ForceRefresh();
                ListViewMoveToUnlockItem();
                m_isInitedListView = true;
            }
        }

        void ListViewMoveToUnlockItem()
        {
            if (monumentLists.Count > 3)
            {
                view.m_sv_list_view_ListView.MovePanelToItemIndex(monumentLists.Count - 2);
            }  
        }

        void ListViewOnItemRemove(ListView.ListItem listItem)
        {
            MonumentProject_Item monumentProjectItem = listItem.data as MonumentProject_Item;
            if (monumentProjectItem != null && monumentProjectItem.timer != null)
            {
                Timer.Cancel(monumentProjectItem.timer);
            }
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_MonumentProjectView projectView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_MonumentProjectView>(listItem.go);
            if (!listItem.isInit)
            {
                listItem.isInit = true;
                MonumentProject_Item monumentProjectItem = new MonumentProject_Item();
                monumentProjectItem.items.Add(projectView.m_UI_Model_Item);
                monumentProjectItem.monumentItems.Add(projectView.m_UI_MonumentItem);
                projectView.data = monumentProjectItem;
            }

            if (listItem.index < 0 || listItem.index >= m_evolutionMileStoneDefine.Count)
                return;

            EvolutionMileStoneDefine define = m_evolutionMileStoneDefine[listItem.index];
            Role_GetMonument.response.MonumentList monumentList = null;
            if (monumentLists.ContainsKey(define.order))
            {
                monumentList = monumentLists[define.order];
            }

            RefreshProjectView(projectView, define, monumentList, listItem.index);
        }

        /// <summary>
        /// 刷新单个事件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="define"></param>
        private void RefreshProjectView(UI_Item_MonumentProjectView item, EvolutionMileStoneDefine define,
            Role_GetMonument.response.MonumentList monumentList, int index)
        {
            ClientUtils.LoadSprite(item.m_img_img_PolygonImage, define.backImage);
            item.m_lbl_title_LanguageText.text = LanguageUtils.getText(define.l_nameId);
            string desc = LanguageUtils.getText(define.l_desc);
            desc = desc.Replace("{p1}", define.param1.ToString("N0"));
            desc = desc.Replace("{p2}", define.param2.ToString("N0"));
            desc = desc.Replace("{r1}", define.require.ToString("N0"));

            item.m_lbl_tips_LanguageText.text = desc;

            EventState curState = EventState.NotOpen;
            
            MonumentProject_Item monumentProjectItem = item.data as MonumentProject_Item;
            if (monumentProjectItem.timer != null)
            {
                Timer.Cancel(monumentProjectItem.timer);
                monumentProjectItem.timer = null;
            }

            if (monumentList != null && monumentList.HasFinishTime)
            {
                if (ServerTimeModule.Instance.GetServerTime() + 3 >= monumentList.finishTime)
                {
                    curState = EventState.Closed;
                }
                else
                {
                    curState = EventState.Doing;
                }
            }

            RefreshProjectView_Reward(item, define, curState, monumentList, monumentProjectItem);
            RefreshProjectView_DetailDesc(item, define, curState, monumentList);
            RefreshProjectView_Progress(item, define, curState, monumentList, monumentProjectItem, index);
        }

        /// <summary>
        /// 事件奖励区
        /// </summary>
        /// <param name="item"></param>
        /// <param name="define"></param>
        private void RefreshProjectView_Reward(UI_Item_MonumentProjectView item, EvolutionMileStoneDefine define,
            EventState curState, Role_GetMonument.response.MonumentList monumentList, MonumentProject_Item monumentProjectItem)
        {
            item.m_lbl_awardText_LanguageText.text = LanguageUtils.getText(GetRewardTypeLan(define.getRewardType));

            //奖励图标
            RefreshProjectView_RewardItems(item, define, curState);
            RefreshRewardStatuTxt(item, define, curState, monumentList, monumentProjectItem);

            item.m_btn_info_GameButton.onClick.RemoveAllListeners();
            item.m_btn_info_GameButton.gameObject.SetActive(define.recordRankReward != 0 ? true : false);
            if (define.recordRankReward != 0)
            {
                item.m_btn_info_GameButton.onClick.AddListener(() =>
                {

                    List<EvolutionRankRewardDefine> evolutionRankRewardDefines =
                        CoreUtils.dataService.QueryRecords<EvolutionRankRewardDefine>();

                    List<EvolutionRankRewardDefine> rankRewardDefines = new List<EvolutionRankRewardDefine>();
                    for (int i = 0; i < evolutionRankRewardDefines.Count; i++)
                    {
                        if (evolutionRankRewardDefines[i].type == define.recordRankReward)
                        {
                            rankRewardDefines.Add(evolutionRankRewardDefines[i]);
                        }
                    }
                    
                    CoreUtils.uiManager.ShowUI(UI.s_eventTypeRankReward, null, rankRewardDefines);
                });
            }
        }
        
        

        /// <summary>
        /// 显示奖励领取提示
        /// </summary>
        private void RefreshRewardStatuTxt(UI_Item_MonumentProjectView item, EvolutionMileStoneDefine define,
            EventState curState, Role_GetMonument.response.MonumentList monumentList, MonumentProject_Item monumentProjectItem)
        {
            if (monumentProjectItem.effect != null)
            {
                CoreUtils.assetService.Destroy(monumentProjectItem.effect);
                monumentProjectItem.effect = null;
            }
            
            item.m_btn_receiveReward_GameButton.onClick.RemoveAllListeners();
            item.m_btn_receiveReward_GameButton.gameObject.SetActive(false);
            
            item.m_img_reddot_PolygonImage.gameObject.SetActive(false);
            
            if (curState == EventState.NotOpen || curState == EventState.Doing)
            {
                item.m_lbl_status_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                item.m_lbl_status_LanguageText.gameObject.SetActive(true);
                Color statusColor = receivedColor;
                ReceiveRewardState curReceiveState = ReceiveRewardState.Received;

                if (monumentList.canReward) // 可领奖
                {
                    curReceiveState = monumentList.reward ? ReceiveRewardState.Received : ReceiveRewardState.CanReceive;
                }
                else
                {
                    curReceiveState = ReceiveRewardState.CannotReceive;
                }

                int rewardStatuId = lan_rewardReceived;

                switch (curReceiveState)
                {
                    case ReceiveRewardState.CanReceive:
                        statusColor = canReceiveColor;
                        rewardStatuId = lan_rewardCanReceive;

                        item.m_btn_receiveReward_GameButton.gameObject.SetActive(true);
                        item.m_img_reddot_PolygonImage.gameObject.SetActive(true);
                        // 奖励可领取时特效等处理        (还未处理)
                        CoreUtils.assetService.Instantiate("UI_10034", (obj) =>
                        {
                            if (monumentProjectItem.effect != null)
                            {
                                CoreUtils.assetService.Destroy(monumentProjectItem.effect);
                                monumentProjectItem.effect = null;
                            }

                            monumentProjectItem.effect = obj;
                            Transform objTr = obj.transform;
                            objTr.transform.SetParent(item.m_btn_receiveReward_GameButton.transform);
                            objTr.localPosition = Vector3.zero;
                            objTr.localScale = Vector3.one;
                        });
                        
                        item.m_btn_receiveReward_GameButton.onClick.AddListener(() =>
                        {
                            if (rewardingItem == null)
                            {
                                rewardingItem = monumentProjectItem;
                                Role_GetMonumentReward.request req = new Role_GetMonumentReward.request();
                                req.id = monumentList.id;
                                AppFacade.GetInstance().SendSproto(req);
                            }
                            
                        });
                        break;
                    case ReceiveRewardState.Received:
                        statusColor = receivedColor;
                        rewardStatuId = lan_rewardReceived;
                        break;
                    case ReceiveRewardState.CannotReceive:
                        statusColor = cannotReceiveColor;
                        rewardStatuId = GetRewardTypeStatusLan(define.getRewardType);
                        break;
                }

                item.m_lbl_status_LanguageText.text = LanguageUtils.getText(rewardStatuId);
                item.m_lbl_status_LanguageText.color = statusColor;
            }
        }

        private RectTransform[] GetRewardItemRectTransforms(MonumentProject_Item monumentProjectItem, List<RewardGroupData> groupDatas)
        {
            if (groupDatas.Count <= 0)
            {
                CoreUtils.logService.Error("[纪念碑]  该事件的奖励， 服务器没有下发任何奖励!");
            }
            
            var items = monumentProjectItem.items;
            List<RectTransform> rectTransforms = new List<RectTransform>();

            for (int i = 0; i < groupDatas.Count; i++)
            {
                RewardGroupData groupData = groupDatas[i];

                for (int itemIndex = 0; itemIndex < items.Count; itemIndex++)
                {
                    if (items[itemIndex].gameObject.activeSelf && groupData.ItemData.itemIcon.Equals(items[itemIndex].m_img_icon_PolygonImage.assetName))
                    {
                        rectTransforms.Add(items[itemIndex].gameObject.GetComponent<RectTransform>());
                        break;
                    }
                }
            }

            if (rectTransforms.Count != groupDatas.Count)    // 服务器下发的奖励与事件展示的不符，不管展示顺序了
            {
                rectTransforms.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].gameObject.activeSelf)
                    {
                        rectTransforms.Add(items[i].gameObject.GetComponent<RectTransform>());    
                    }
                }
            }
            
            return rectTransforms.ToArray();
        }

        /// <summary>
        /// 通过getRewardTyp获取不同的文字
        /// </summary>
        /// <param name="getRewardTyp"></param>
        /// <returns></returns>
        private int GetRewardTypeLan(int getRewardTyp)
        {
            int value = lan_getRewardType1;

            switch (getRewardTyp)
            {
                case 1:
                    value = lan_getRewardType1;
                    break;
                case 2:
                    value = lan_getRewardType2;
                    break;
                case 3:
                    value = lan_getRewardType3;
                    break;
                case 4:
                    value = lan_getRewardType4;
                    break;
            }

            return value;
        }
        
        private int GetRewardTypeStatusLan(int getRewardTyp)
        {
            int value = lan_getRewardTypeStatus1;

            switch (getRewardTyp)
            {
                case 1:
                    value = lan_getRewardTypeStatus1;
                    break;
                case 2:
                    value = lan_getRewardTypeStatus2;
                    break;
                case 3:
                    value = lan_getRewardTypeStatus3;
                    break;
                case 4:
                    value = lan_getRewardTypeStatus4;
                    break;
            }

            return value;
        }

        /// <summary>
        /// 刷新奖励图标
        /// </summary>
        /// <param name="item"></param>
        /// <param name="define"></param>
        private void RefreshProjectView_RewardItems(UI_Item_MonumentProjectView item, EvolutionMileStoneDefine define,
            EventState curState)
        {
            bool isGray = curState == EventState.NotOpen ? true : false;


            MonumentProject_Item monumentProjectItem = item.data as MonumentProject_Item;
            List<UI_Model_Item_SubView> items = monumentProjectItem.items;
            List<UI_Model_MonumentItem_SubView> monumentItems = monumentProjectItem.monumentItems;

            for (int i = 0; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < monumentItems.Count; i++)
            {
                monumentItems[i].gameObject.SetActive(false);
            }

            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(define.rewardShow);
            int count = groupDataList.Count;
            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    GameObject obj = CoreUtils.assetService.Instantiate(item.m_UI_Model_Item.gameObject);
                    items.Add(new UI_Model_Item_SubView(obj.GetComponent<RectTransform>()));
                    obj.transform.SetParent(item.m_UI_Model_Item.gameObject.transform.parent);
                    obj.transform.localScale = Vector3.one;
                }

                UI_Model_Item_SubView subViewItm = items[i];
                subViewItm.gameObject.SetActive(true);
                subViewItm.RefreshByGroup(groupDataList[i], 3);
                subViewItm.SetGray(isGray);
                subViewItm.gameObject.transform.SetAsLastSibling();
            }

            string[] fogArr = new[] {define.fogOpenImg1, define.fogOpenImg2};
            int[] fogDescArr = new[] {define.fogOpenDesc1, define.fogOpenDesc2};
            int[] fogOpenFlagArr = new[] {define.fogOpenFlag1, define.fogOpenFlag2};

            for (int i = 0; i < fogArr.Length; i++)
            {
                if (string.IsNullOrEmpty(fogArr[i]))
                {
                    continue;
                }

                if (i >= monumentItems.Count)
                {
                    GameObject obj = CoreUtils.assetService.Instantiate(item.m_UI_MonumentItem.gameObject);
                    monumentItems.Add(new UI_Model_MonumentItem_SubView(obj.GetComponent<RectTransform>()));
                    obj.transform.SetParent(item.m_UI_Model_Item.gameObject.transform.parent);
                    obj.transform.localScale = Vector3.one;
                }

                UI_Model_MonumentItem_SubView subViewItm = monumentItems[i];
                subViewItm.gameObject.SetActive(true);
                subViewItm.SetGray(isGray);
                subViewItm.Refresh(fogDescArr[i], fogOpenFlagArr[i], fogArr[i]);
                subViewItm.gameObject.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// 事件详细描述区
        /// </summary>
        /// <param name="item"></param>
        /// <param name="define"></param>
        private void RefreshProjectView_DetailDesc(UI_Item_MonumentProjectView item, EvolutionMileStoneDefine define,
            EventState curState, Role_GetMonument.response.MonumentList monumentList)
        {
            item.m_btn_check01_GameButton.onClick.RemoveAllListeners();
            item.m_btn_check01_GameButton.gameObject.SetActive(false);
            item.m_lbl_desc_LanguageText.gameObject.SetActive(true);

            if (curState == EventState.NotOpen)
            {
                item.m_lbl_desc_LanguageText.text = LanguageUtils.getText(define.contributeP0);
                return;
            }

            bool showAllianceEnable = define.allianceRankDesc != 0 ? true : false;

            // 事件领取提示区
            string curStateInfo = "";
            switch (curState)
            {
                case EventState.Doing:
                    curStateInfo =  LanguageUtils.getTextFormat(define.contributeP0, monumentList.count);
                    break;
                case EventState.Closed: // 事件已关闭需根据不同状态处理
                    item.m_lbl_desc_LanguageText.gameObject.SetActive(!showAllianceEnable);
                    bool personRequireFinish = monumentList.count >= define.personRequire ? true : false;
                    
                    item.m_btn_check01_GameButton.gameObject.SetActive(showAllianceEnable);
                    item.m_btn_check01_GameButton.onClick.AddListener(() =>
                    {
                        ShowMonumentAllianceRank(monumentList, define.allianceRankDesc);
                    });

                    if (personRequireFinish) // 个人推荐是否完成
                    {
                        curStateInfo = LanguageUtils.getTextFormat(define.contributeP1, monumentList.count);
                    }
                    else
                    {
                        curStateInfo = LanguageUtils.getTextFormat(define.contributeP2, monumentList.count);
                    }

                    break;
            }

            item.m_lbl_desc_LanguageText.text = curStateInfo;
        }

        /// <summary>
        /// 显示纪念碑排行
        /// </summary>
        private void ShowMonumentAllianceRank(Role_GetMonument.response.MonumentList monumentList, int allianceRankDesc)
        {
            CoreUtils.uiManager.ShowUI(UI.s_monumentAlliance, null, new object[] {monumentList, allianceRankDesc});
        }

        /// <summary>
        /// 事件进度
        /// </summary>
        /// <param name="item"></param>
        /// <param name="define"></param>
        private void RefreshProjectView_Progress(UI_Item_MonumentProjectView item, EvolutionMileStoneDefine define,
            EventState curState, Role_GetMonument.response.MonumentList monumentList, MonumentProject_Item monumentProjectItem, int index)
        {
            item.m_lbl_milestone01_LanguageText.gameObject.SetActive(false);
            item.m_lbl_milestone02_LanguageText.gameObject.SetActive(false);
            item.m_pb_milestoneBar_GameSlider.gameObject.SetActive(false);

            if (curState == EventState.NotOpen) // 未开启，直接显示文本，返回
            {
                item.m_lbl_milestone01_LanguageText.gameObject.SetActive(true);
                item.m_lbl_milestone01_LanguageText.text = LanguageUtils.getText(lan_notOpen);
                return;
            }

            bool isFinish = false;
            bool isGlobalServerEvent = define.gobalFlag == 1 ? true : false;
            if (isGlobalServerEvent)
            {
                isFinish = monumentList.serverCount >= define.require;
            }
            else
            {
                isFinish = monumentList.count >= define.require;
                
                if (!monumentList.canReward) // 如果奖励不可领的话，事件就不算完成。 与策划确认设计如此
                {
                    isFinish = false;
                }
            }

            item.m_lbl_milestone02_LanguageText.color = Color.black;
            switch (curState)
            {
                case EventState.Doing:
                    item.m_lbl_milestone01_LanguageText.gameObject.SetActive(true);
                    item.m_pb_milestoneBar_GameSlider.gameObject.SetActive(true);


                    int progressCount = (int) monumentList.count;
                    int requireCount = define.require;
                    if (isGlobalServerEvent) // 是全服奖励
                    {
                        progressCount = (int) monumentList.serverCount;
                        requireCount = define.require;
                    }
                    
                    item.m_pb_milestoneBar_GameSlider.value = ((float) progressCount / requireCount);
                    

                    monumentProjectItem.timer = Timer.Register(0.7f, () =>
                    {
                        int finishTime2 = (int) monumentList.finishTime;
                        int curTime2 = (int) ServerTimeModule.Instance.GetServerTime();
                        item.m_lbl_milestone01_LanguageText.text = LanguageUtils.getTextFormat(lan_surplusTime,
                            ClientUtils.FormatTime(finishTime2 - curTime2));
                    },null, true, false, item.m_lbl_milestone01_LanguageText);
                    
                    int finishTime = (int) monumentList.finishTime;
                    int curTime = (int) ServerTimeModule.Instance.GetServerTime();
                    item.m_lbl_milestone01_LanguageText.text = LanguageUtils.getTextFormat(lan_surplusTime,
                        ClientUtils.FormatTime(finishTime - curTime)); 
                    
                    item.m_lbl_process_LanguageText.text =
                        LanguageUtils.getTextFormat(lan_progress, progressCount.ToString("N0"), requireCount.ToString("N0"));
                    break;
                case EventState.Closed:

                    if (isFinish) // 事件已达成
                    {
                        string finishTimeStr = ServerTimeModule.Instance.ConverToServerDateTime(monumentList.finishTime)
                            .ToString("MM/dd HH:mm");

                        item.m_lbl_milestone02_LanguageText.gameObject.SetActive(true);
                        item.m_lbl_milestone02_LanguageText.text =
                            LanguageUtils.getTextFormat(lan_finishAt, finishTimeStr);
                        item.m_lbl_milestone02_LanguageText.color = new Color(255/255.0f,253/255.0f,44/255.0f);
                    }
                    else // 事件未达成
                    {
                        item.m_lbl_milestone01_LanguageText.gameObject.SetActive(true);
                        item.m_lbl_milestone01_LanguageText.text = LanguageUtils.getText(lan_chapterFinish);
                        
                        if (define.getRewardType == 1)
                        {
                            item.m_lbl_milestone02_LanguageText.gameObject.SetActive(true);
                            item.m_lbl_milestone02_LanguageText.text =
                                LanguageUtils.getTextFormat(lan_progress, monumentList.serverCount.ToString("N0"), define.require.ToString("N0"));
                        }
                        else if (define.getRewardType == 2)
                        {
                            item.m_lbl_milestone02_LanguageText.gameObject.SetActive(true);
                            item.m_lbl_milestone02_LanguageText.text =
                                LanguageUtils.getTextFormat(lan_progress, monumentList.count.ToString("N0"), define.require.ToString("N0"));
                        }
                    }

                    break;
            }
        }

        private EventState GetEventState(Role_GetMonument.response.MonumentList monumentList)
        {
            EventState result = EventState.NotOpen;

            if (monumentList != null && monumentList.HasFinishTime)
            {
                if (ServerTimeModule.Instance.GetServerTime() > monumentList.finishTime)
                {
                    result = EventState.Closed;
                }
                else
                {
                    result = EventState.Doing;
                }
            }
            
            return result;
        }

    }
}