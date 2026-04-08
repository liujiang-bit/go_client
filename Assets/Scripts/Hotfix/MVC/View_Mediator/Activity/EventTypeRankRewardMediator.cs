// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    EventTypeRankRewardMediator 通用条件类活动 排行榜奖励界面
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
    public class EventTypeRankParam
    {
        public int Type;
        public int ActivityId;
        public int SubId;
        public int RankType;//1总排名 2阶段排名
    }

    public class EventTypeRankRewardMediator : GameMediator {
        #region Member
        public static string NameMediator = "EventTypeRankRewardMediator";

        private int m_activityId;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<ActivityRankingTypeDefine> m_dataList = new List<ActivityRankingTypeDefine>();

        private List<ActivityRankingTypeDefine> m_allianceRankList;
        private List<ActivityRankingTypeDefine> m_totalRankDataList;
        private List<ActivityRankingTypeDefine> m_stageRankDataList;
        private int m_ckType = 0; //1总排名 2阶段排名
        private int m_subTypeId = 0;

        private List<float> m_itemHeightList = new List<float>();
        private float m_itemHeight;

        private int m_leaderboardId;
        private int m_alliacneLeaderboardId;

        #endregion

        //IMediatorPlug needs
        public EventTypeRankRewardMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public EventTypeRankRewardView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
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
            List<EvolutionRankRewardDefine> evolutionRankRewardDefineList = view.data as List<EvolutionRankRewardDefine>;
            if (evolutionRankRewardDefineList != null)    // 纪念碑奖励排行榜处理
            {
                view.m_lbl_desc_LanguageText.gameObject.SetActive(false);
                view.m_pl_rankGroup_ToggleGroup.gameObject.SetActive(false);

                float addHeight = view.m_lbl_desc_LanguageText.GetComponent<RectTransform>().rect.height + 
                                  view.m_pl_rankGroup_ToggleGroup.GetComponent<RectTransform>().rect.height;
                view.m_list_node.sizeDelta = new Vector2(view.m_list_node.sizeDelta.x, view.m_list_node.sizeDelta.y+addHeight);

                for (int i = 0; i < evolutionRankRewardDefineList.Count; i++)
                {
                    EvolutionRankRewardDefine evolutionRankRewardDefine = evolutionRankRewardDefineList[i];
                    ActivityRankingTypeDefine activityRankingTypeDefine = new ActivityRankingTypeDefine();

                    activityRankingTypeDefine.ID = evolutionRankRewardDefine.ID;
                    activityRankingTypeDefine.activityType = evolutionRankRewardDefine.type;
                    activityRankingTypeDefine.targetMax = evolutionRankRewardDefine.rankMax;
                    activityRankingTypeDefine.targetMin = evolutionRankRewardDefine.rankMin;
                    activityRankingTypeDefine.itemPackage = evolutionRankRewardDefine.rewardShow;
                    m_dataList.Add(activityRankingTypeDefine);
                }
            }
            else
            {
                EventTypeRankParam param = view.data as EventTypeRankParam;
                m_activityId = param.ActivityId;

                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>(m_activityId);

                if (param.Type == 1)
                {
                    LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(define.leaderboard);
                    view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);

                    view.m_pl_rankGroup_ToggleGroup.gameObject.SetActive(false);
                    ReadTotalRankData();
                    m_dataList = m_totalRankDataList;
                }
                else if (param.Type == 2)
                {
                    LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(define.leaderboard);
                    view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);

                    view.m_pl_rankGroup_ToggleGroup.gameObject.SetActive(true);

                    view.m_lbl_total_LanguageText.text = LanguageUtils.getText(762205);
                    view.m_lbl_single_LanguageText.text = LanguageUtils.getText(762204);

                    m_ckType = param.RankType;
                    m_subTypeId = param.SubId;
                    if (m_ckType == 1)
                    {
                        view.m_ck_total_GameToggle.isOn = true;
                        view.m_ck_single_GameToggle.isOn = false;

                        ReadTotalRankData();
                        m_dataList = m_totalRankDataList;
                    }
                    else
                    {
                        view.m_ck_total_GameToggle.isOn = false;
                        view.m_ck_single_GameToggle.isOn = true;

                        ReadStageRankData();
                        m_dataList = m_stageRankDataList;
                    }

                    view.m_ck_total_GameToggle.onValueChanged.AddListener(CkTotal);
                    view.m_ck_single_GameToggle.onValueChanged.AddListener(CkStage);
                }
                else if (param.Type == 3)
                {
                    LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(define.leaderboard);
                    view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);

                    view.m_pl_rankGroup_ToggleGroup.gameObject.SetActive(false);
                    ActivityInfernalDefine define2 = CoreUtils.dataService.QueryRecord<ActivityInfernalDefine>(param.SubId);
                    m_dataList = GetRankData(define2.rewardRank);
                }
                else if (param.Type == 4)
                {
                    m_leaderboardId = define.leaderboard;
                    m_alliacneLeaderboardId = define.allianceleaderboard;

                    view.m_pl_rankGroup_ToggleGroup.gameObject.SetActive(true);
                    view.m_lbl_total_LanguageText.text = LanguageUtils.getText(762250);
                    view.m_lbl_single_LanguageText.text = LanguageUtils.getText(762249);

                    m_ckType = param.RankType;

                    ActivityIntegralTypeDefine define2 = CoreUtils.dataService.QueryRecord<ActivityIntegralTypeDefine>(param.SubId);
                    m_subTypeId = define2.leaderboardType;

                    if (m_ckType == 1) //联盟排名奖励
                    {
                        LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(m_alliacneLeaderboardId);
                        view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);

                        view.m_ck_total_GameToggle.isOn = true;
                        view.m_ck_single_GameToggle.isOn = false;

                        ReadAllianceRankData();
                        m_dataList = m_allianceRankList;
                    }
                    else //个人排名奖励
                    {
                        LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(m_leaderboardId);
                        view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);

                        view.m_ck_total_GameToggle.isOn = false;
                        view.m_ck_single_GameToggle.isOn = true;

                        ReadTotalRankData();
                        m_dataList = m_totalRankDataList;
                    }

                    view.m_ck_total_GameToggle.onValueChanged.AddListener(CkAlliance);
                    view.m_ck_single_GameToggle.onValueChanged.AddListener(CkPersonal);
                }
                else if (param.Type == 5)
                {
                    view.m_pl_rankGroup_ToggleGroup.gameObject.SetActive(false);

                    LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(define.allianceleaderboard);
                    view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);

                    ActivityIntegralTypeDefine define2 = CoreUtils.dataService.QueryRecord<ActivityIntegralTypeDefine>(param.SubId);
                    m_subTypeId = define2.leaderboardType;

                    ReadAllianceRankData();
                    m_dataList = m_allianceRankList;
                }
            }

            SetHeightDataList(m_dataList.Count);

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {

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
            m_itemHeight = m_assetDic["UI_Item_EventTypeRankReward"].GetComponent<RectTransform>().rect.height;
            RefreshList();
        }

        public void RefreshList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ItemByIndex;
            functab.GetItemSize = OnGetItemSize;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_ListView.FillContent(m_dataList.Count);
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            if(m_itemHeightList[listItem.index] == -1)
            {
                return m_itemHeight;
            }
            return m_itemHeightList[listItem.index];
        }

        private void ItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_EventTypeRankReward_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_EventTypeRankReward_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_EventTypeRankReward_SubView;
            }
            subView.Refresh(m_dataList[listItem.index]);
            float itemHeight = subView.GetHeight();
            if (m_itemHeightList[listItem.index] != itemHeight)
            {
                m_itemHeightList[listItem.index] = itemHeight;
                view.m_sv_list_ListView.RefreshItem(listItem.index);
            }
        }

        private void CkTotal(bool isCk)
        {
            if (isCk)
            {
                if (m_ckType == 1)
                {
                    return;
                }
                m_ckType = 1;
                if (m_totalRankDataList == null)
                {
                    ReadTotalRankData();
                }
                m_dataList = m_totalRankDataList;
                SetHeightDataList(m_dataList.Count);
                RefreshList();
            }
        }

        private void CkStage(bool isCk)
        {
            if (isCk)
            {
                if (m_ckType == 2)
                {
                    return;
                }
                m_ckType = 2;
                if (m_stageRankDataList == null)
                {
                    ReadStageRankData();
                }
                m_dataList = m_stageRankDataList;
                SetHeightDataList(m_dataList.Count);
                RefreshList();
            }
        }

        private void CkAlliance(bool isCk)
        {
            if (isCk)
            {
                if (m_ckType == 1)
                {
                    return;
                }
                m_ckType = 1;
                ChangeDesc(m_alliacneLeaderboardId);
                if (m_allianceRankList == null)
                {
                    ReadAllianceRankData();
                }
                m_dataList = m_allianceRankList;
                SetHeightDataList(m_dataList.Count);
                RefreshList();
            }
        }

        private void CkPersonal(bool isCk)
        {
            if (isCk)
            {
                if (m_ckType == 2)
                {
                    return;
                }
                m_ckType = 2;
                ChangeDesc(m_leaderboardId);
                if (m_totalRankDataList == null)
                {
                    ReadTotalRankData();
                }
                m_dataList = m_totalRankDataList;
                SetHeightDataList(m_dataList.Count);
                RefreshList();
            }
        }

        private void ChangeDesc(int leaderboarId)
        {
            LeaderboardDefine lDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(leaderboarId);
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lDefine.l_desID);
        }

        private void ReadTotalRankData()
        {
            m_totalRankDataList = new List<ActivityRankingTypeDefine>();
            int id = m_activityId * 1000;
            for (int i = 1; i < 500; i++)
            {
                ActivityRankingTypeDefine rankDefine = CoreUtils.dataService.QueryRecord<ActivityRankingTypeDefine>(id + i);
                if (rankDefine == null)
                {
                    break;
                }
                else
                {
                    m_totalRankDataList.Add(rankDefine);
                }
            }
        }

        private void ReadStageRankData()
        {
            m_stageRankDataList = new List<ActivityRankingTypeDefine>();
            int id = m_subTypeId * 1000;
            for (int i = 1; i < 500; i++)
            {
                ActivityRankingTypeDefine rankDefine = CoreUtils.dataService.QueryRecord<ActivityRankingTypeDefine>(id + i);
                if (rankDefine == null)
                {
                    break;
                }
                else
                {
                    m_stageRankDataList.Add(rankDefine);
                }
            }
        }

        private void ReadAllianceRankData()
        {
            m_allianceRankList = new List<ActivityRankingTypeDefine>();
            int id = m_subTypeId * 1000;
            for (int i = 1; i < 500; i++)
            {
                ActivityRankingTypeDefine rankDefine = CoreUtils.dataService.QueryRecord<ActivityRankingTypeDefine>(id + i);
                if (rankDefine == null)
                {
                    break;
                }
                else
                {
                    m_allianceRankList.Add(rankDefine);
                }
            }
        }

        private List<ActivityRankingTypeDefine> GetRankData(int baseId)
        {
            List<ActivityRankingTypeDefine> list = new List<ActivityRankingTypeDefine>();
            int id = baseId * 1000;
            for (int i = 1; i < 500; i++)
            {
                ActivityRankingTypeDefine rankDefine = CoreUtils.dataService.QueryRecord<ActivityRankingTypeDefine>(id + i);
                if (rankDefine == null)
                {
                    break;
                }
                else
                {
                    list.Add(rankDefine);
                }
            }
            return list;
        }

        private void SetHeightDataList(int count)
        {
            m_itemHeightList.Clear();
            for (int i = 0; i < count; i++)
            {
                m_itemHeightList.Add(-1);
            }
        }
    }
}