// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月29日
// Update Time         :    2020年4月29日
// Class Description   :    UI_Item_EventTypeRank_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using UnityEngine.Events;
using Data;
using System.Collections.Generic;
using SprotoType;
using PureMVC.Interfaces;

namespace Game {
    public enum EventRankSourceType
    {
        /// <summary>
        /// 通用条件达成类活动
        /// </summary>
        CeneralCondition = 1,
        /// <summary>
        /// 最强执政官
        /// </summary>
        StrongerConsul = 2,
        /// <summary>
        /// 战斗号角
        /// </summary>
        Warhorn = 3,
        /// <summary>
        /// 部落之王
        /// </summary>
        TribalKing = 4,
    }

    public class RankItemData
    {
        public int ShowType;
        public Rank_QueryRank.response.RankInfo RankData;
        public Activity_GetRank.response.RankList.RankInfo StageRankData;
        public int Stage;
        public ActivityKillTypeDefine Define;
        public int Status;
    }

    public partial class UI_Item_EventTypeRank_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;

        private bool m_isInit;
        private Int64 m_activityId;
        private ActivityCalendarDefine m_activityDefine;
        private int m_leaderboardId;
        private int m_leaderboardAllianceId;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private bool m_isInitStageList = false;
        private bool m_isInitTotalList = false;
        private bool m_assetIsLoadFinish = false;

        private int m_requestNum = 100;
        private int m_requestIndex;
        private bool m_isRequesting;

        private int m_sourceType; //1通用条件达成类活动 2最强执政官
        private int m_ckType;   //1总排名 2阶段排名
        private float m_menuTitleHeight = 0;
        private float m_menuDataHeight = 0;

        private Rank_QueryRank.response m_totalRankResponse;
        private Activity_GetRank.response m_stageRankResponse;
        private bool m_isRequestingStage;

        private List<RankItemData> m_totalRankDatalist;
        private List<RankItemData> m_stageRankDataList;
        private List<RankItemData> m_dataList;

        private ActivityScheduleData m_scheduleData;
        private int m_currStage;
        private int m_currLevel;

        private int m_comparisonLimit;

        private Rank_QueryRank.response m_allianceRankResponse;
        private Rank_QueryRank.response m_personalRankResponse;
        private List<RankItemData> m_alliancRankDatalist;
        private List<RankItemData> m_personalRankdDatalist;
        private int m_requestAllianceNum = 0;
        private int m_comparisonAllianceLimit;

        private Timer m_loadingTimer;

        private int m_requestType;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    Rank_QueryRank.TagName,
                    Activity_GetRank.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Rank_QueryRank.TagName:   //总排行数据 
                    ReceiveRankData(notification.Body);
                    break;
                case Activity_GetRank.TagName: //阶段排名数据
                    ReceiveStageData(notification.Body);
                    break;
                default:
                    break;
            }
        }

        public void Refresh(Int64 activityId, int sourceType, int ckType = 0)
        {
            CancelLoading();
            m_activityId = activityId;
            m_sourceType = sourceType;
            m_ckType = ckType;
            m_pl_playermes.gameObject.SetActive(false);
            m_pl_total_sv.gameObject.SetActive(false);
            m_pl_stage_sv.gameObject.SetActive(false);

            if (m_sourceType == (int)EventRankSourceType.CeneralCondition)//通用条件达成类活动
            {
                m_lbl_title_LanguageText.gameObject.SetActive(true);
                m_pl_ck_ToggleGroup.gameObject.SetActive(false);
                m_pl_stage.gameObject.SetActive(false);
            }
            else if (m_sourceType == (int)EventRankSourceType.StrongerConsul)//最强执政官 进行中
            {
                m_lbl_title_LanguageText.gameObject.SetActive(false);
                m_pl_stage.gameObject.SetActive(false);
                m_pl_ck_ToggleGroup.gameObject.SetActive(false);
            }
            else if (m_sourceType == (int)EventRankSourceType.Warhorn)//战斗号角
            {
                m_lbl_title_LanguageText.gameObject.SetActive(false);
                m_pl_stage.gameObject.SetActive(false);
                m_pl_ck_ToggleGroup.gameObject.SetActive(false);
            }
            else if (m_sourceType == (int)EventRankSourceType.TribalKing) //部落之王
            {
                m_lbl_title_LanguageText.gameObject.SetActive(true);
                m_lbl_title_LanguageText.text = LanguageUtils.getText(300063);
                m_pl_stage.gameObject.SetActive(false);
                m_pl_ck_ToggleGroup.gameObject.SetActive(false);
            }

            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_activityId);
                m_activityDefine = define;
                m_leaderboardId = define.leaderboard;
                m_leaderboardAllianceId = define.allianceleaderboard;
                Debug.LogFormat("leaderboardId:{0}", m_leaderboardId);

                if (m_sourceType == (int)EventRankSourceType.Warhorn)//战斗号角
                {
                    LeaderboardDefine define1 = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(m_leaderboardId);
                    m_requestNum = define1.showLimit;
                    m_comparisonLimit = define1.comparisonLimit;

                    LeaderboardDefine define2 = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(m_leaderboardAllianceId);
                    m_requestAllianceNum = define2.showLimit;
                    m_comparisonAllianceLimit = define2.comparisonLimit;
                }
                else if (m_sourceType == (int)EventRankSourceType.TribalKing)
                {
                    LeaderboardDefine define1 = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(m_leaderboardAllianceId);
                    m_requestNum = define1.showLimit;
                    m_comparisonLimit = define1.comparisonLimit;

                    m_comparisonAllianceLimit = define1.comparisonLimit;
                }
                else
                {
                    LeaderboardDefine define1 = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(m_leaderboardId);
                    m_requestNum = define1.showLimit;
                    m_comparisonLimit = define1.comparisonLimit;
                }

                m_btn_reward_GameButton.onClick.AddListener(OnRankReward);

                //预加载列表预设
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(m_sv_stage_list_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);

                m_isInit = true;
            }
            else
            {
                if (m_assetIsLoadFinish)
                {
                    m_totalRankDatalist = null;
                    m_stageRankDataList = null;
                    m_alliancRankDatalist = null;
                    m_personalRankdDatalist = null;
                    if (m_dataList == null)
                    {
                        m_dataList = new List<RankItemData>();
                    }
                    m_dataList.Clear();

                    //刷新界面
                    RefreshContent();
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

            m_menuTitleHeight = m_assetDic["UI_Item_EventTypeRankTitle"].GetComponent<RectTransform>().rect.height;
            m_menuDataHeight = m_assetDic["UI_Item_EventTypeRankItem"].GetComponent<RectTransform>().rect.height;

            m_assetIsLoadFinish = true;

            RefreshContent();
        }

        private void RefreshContent()
        {
            if (m_sourceType == (int)EventRankSourceType.StrongerConsul) //最强执政官 排行榜
            {
                ReadScheduleData();

                m_ck_total_GameToggle.onValueChanged.RemoveAllListeners();
                m_ck_single_GameToggle.onValueChanged.RemoveAllListeners();

                m_lbl_total_LanguageText.text = LanguageUtils.getText(762205);
                m_lbl_single_LanguageText.text = LanguageUtils.getText(762204);

                if (m_ckType == 1) //总排名
                {
                    m_ck_total_GameToggle.isOn = true;
                    m_ck_single_GameToggle.isOn = false;
                    SetTotalColorSelectStatus(true);
                    SetSingleColorSelectStatus(false);
                    m_ck_total_GameToggle.onValueChanged.AddListener(OnTotalRank);
                    m_ck_single_GameToggle.onValueChanged.AddListener(OnStageRank);

                    RequestRankData(0);
                }
                else //阶段排名
                {
                    m_ck_total_GameToggle.isOn = false;
                    m_ck_single_GameToggle.isOn = true;
                    SetTotalColorSelectStatus(false);
                    SetSingleColorSelectStatus(true);
                    m_ck_total_GameToggle.onValueChanged.AddListener(OnTotalRank);
                    m_ck_single_GameToggle.onValueChanged.AddListener(OnStageRank);

                    ReqeustStageData();
                }
            }
            else if (m_sourceType == (int)EventRankSourceType.Warhorn) //战斗号角
            {
                m_scheduleData = m_activityProxy.GetActivitySchedule(m_activityId);

                m_ck_total_GameToggle.onValueChanged.RemoveAllListeners();
                m_ck_single_GameToggle.onValueChanged.RemoveAllListeners();

                m_lbl_total_LanguageText.text = LanguageUtils.getText(762250);//联盟排名
                m_lbl_single_LanguageText.text = LanguageUtils.getText(762249);//个人排名

                if (m_ckType == 1) //联盟排名
                {
                    m_ck_total_GameToggle.isOn = true;
                    m_ck_single_GameToggle.isOn = false;
                    SetTotalColorSelectStatus(true);
                    SetSingleColorSelectStatus(false);
                    m_ck_total_GameToggle.onValueChanged.AddListener(OnAllianceRank);
                    m_ck_single_GameToggle.onValueChanged.AddListener(OnPersonalRank);

                    RequestAllianceRank();
                }
                else //个人排名
                {
                    m_ck_total_GameToggle.isOn = false;
                    m_ck_single_GameToggle.isOn = true;
                    SetTotalColorSelectStatus(false);
                    SetSingleColorSelectStatus(true);
                    m_ck_total_GameToggle.onValueChanged.AddListener(OnAllianceRank);
                    m_ck_single_GameToggle.onValueChanged.AddListener(OnPersonalRank);

                    RequestPersonalRank();
                }

            }
            else if (m_sourceType == (int)EventRankSourceType.TribalKing) //部落之王
            {
                m_scheduleData = m_activityProxy.GetActivitySchedule(m_activityId);
                RequestTribalKingRank();
            }
            else
            {
                RequestRankData(0);
            }
        }

        private void ReadScheduleData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_activityId);

            if (m_scheduleData == null)
            {
                Debug.LogErrorFormat("not find ActivitySchedule activityId:{0}", m_activityId);
                return;
            }
            m_currLevel = (int)m_scheduleData.Info.level;
            m_currStage = (int)m_scheduleData.Info.stage;
            if (m_currStage == 0)
            {
                m_currStage = 5;
            }
        }

        #region 刷新总排名

        private void RefreshTotalRankContent()
        {
            CancelLoading();
            m_pl_playermes.gameObject.SetActive(true);
            if (m_sourceType == 2)
            {
                m_pl_ck_ToggleGroup.gameObject.SetActive(true);
            }
            m_pl_total_sv.gameObject.SetActive(true);
            RefreshTotalRankTitle();
            RefreshTotalList();
        }

        private void RefreshTotalRankTitle()
        {
            m_UI_PlayerHead.LoadPlayerIcon();

            Rank_QueryRank.response result = m_totalRankResponse;
            string str = m_allianceProxy.GetName();
            m_lbl_name_LanguageText.text = str;

            if (result.selfRank == result.selfOldRank)
            {
                m_img_arrow_up_PolygonImage.gameObject.SetActive(false);
                m_img_arrow_down_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                bool isShow = (result.selfRank > result.selfOldRank) ? false : true;
                m_img_arrow_up_PolygonImage.gameObject.SetActive(isShow);
                m_img_arrow_down_PolygonImage.gameObject.SetActive(!isShow);
            }

            m_lbl_source_LanguageText.text = (result.score > 0) ? ClientUtils.FormatComma(result.score) : "-";
            m_lbl_rank_LanguageText.text = (result.selfRank > 0) ? ActivityProxy.RankNumFormat(result.selfRank, m_comparisonLimit) : "-";
        }

        private void RefreshTotalList()
        {
            if (m_isInitTotalList)
            {
                m_sv_total_list_ListView.FillContent(m_dataList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                functab.GetItemPrefabName = OnGetItemPrefabName;
                functab.GetItemSize = OnGetItemSize;
                m_sv_total_list_ListView.SetInitData(m_assetDic, functab);
                m_sv_total_list_ListView.FillContent(m_dataList.Count);
                m_isInitTotalList = true;
            }
        }

        #endregion

        #region 刷新阶段排名

        private void RefreshStageRankContent()
        {
            CancelLoading();
            m_pl_playermes.gameObject.SetActive(true);
            m_pl_stage.gameObject.SetActive(true);
            m_pl_ck_ToggleGroup.gameObject.SetActive(true);
            m_pl_stage_sv.gameObject.SetActive(true);
            RefreshStageRankTitle();
            RefreshStageList();
        }

        private void RefreshStageRankTitle()
        {
            m_UI_PlayerHead.LoadPlayerIcon();

            Activity_GetRank.response result = m_stageRankResponse;
            string str = m_allianceProxy.GetName();
            m_lbl_name_LanguageText.text = str;

            //阶段
            m_img_arrow_up_PolygonImage.gameObject.SetActive(false);
            m_img_arrow_down_PolygonImage.gameObject.SetActive(false);
            m_lbl_rank_LanguageText.text = "";

            if (result.selfRank == null)
            {
                return;
            }

            List<LanguageText> textList = new List<LanguageText>();
            textList.Add(m_lbl_stageRank1_LanguageText);
            textList.Add(m_lbl_stageRank2_LanguageText);
            textList.Add(m_lbl_stageRank3_LanguageText);
            textList.Add(m_lbl_stageRank4_LanguageText);
            textList.Add(m_lbl_stageRank5_LanguageText);

            for (int i = 1; i < 6; i++)
            {
                if (result.selfRank.ContainsKey(i))
                {
                    bool isShowLimit = (result.selfRank[i].score > 0 && result.selfRank[i].rank == 0) ? true : false;
                    textList[i - 1].text = ActivityProxy.RankNumFormat(result.selfRank[i].rank, m_comparisonLimit, isShowLimit);
                }
                else
                {
                    textList[i - 1].text = "-";
                }
            }

            Int64 score = 0;
            if (result.selfRank.ContainsKey(m_currStage))
            {
                score = result.selfRank[m_currStage].score;
            }
            m_lbl_source_LanguageText.text = ClientUtils.FormatComma(score);
        }

        private void RefreshStageList()
        {
            if (m_isInitStageList)
            {
                m_sv_stage_list_ListView.FillContent(m_dataList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                functab.GetItemPrefabName = OnGetItemPrefabName;
                functab.GetItemSize = OnGetItemSize;
                m_sv_stage_list_ListView.SetInitData(m_assetDic, functab);
                m_sv_stage_list_ListView.FillContent(m_dataList.Count);
                m_isInitStageList = true;
            }
        }

        #endregion

        #region 刷新联盟排名 个人排名

        //刷新联盟排名
        private void RefreshAllianceRankContent()
        {
            CancelLoading();
            m_pl_playermes.gameObject.SetActive(true);
            m_pl_ck_ToggleGroup.gameObject.SetActive(true);
            m_pl_total_sv.gameObject.SetActive(true);
            RefreshAllianceRankTitle();
            RefreshTotalList();
        }

        private void RefreshAllianceRankTitle()
        {
            m_UI_PlayerHead.gameObject.SetActive(false);
            m_UI_Model_GuildFlag.gameObject.SetActive(true);

            Rank_QueryRank.response result = m_allianceRankResponse;

            //旗帜
            string str = "";
            if (m_allianceProxy.HasJionAlliance())
            {
                m_UI_Model_GuildFlag.m_img_flagIcon_PolygonImage.gameObject.SetActive(true);
                m_UI_Model_GuildFlag.m_img_flag_noali_PolygonImage.gameObject.SetActive(false);
                var info = m_allianceProxy.GetAlliance();
                m_UI_Model_GuildFlag.setData(info);
                str = LanguageUtils.getTextFormat(300030, m_allianceProxy.GetAbbreviationName(), m_allianceProxy.GetAllianceName());
            }
            else
            {
                m_UI_Model_GuildFlag.m_img_flagIcon_PolygonImage.gameObject.SetActive(false);
                m_UI_Model_GuildFlag.m_img_flag_noali_PolygonImage.gameObject.SetActive(true);
                str = "-";
            }

            m_lbl_name_LanguageText.text = str;

            if (result.selfRank == result.selfOldRank)
            {
                m_img_arrow_up_PolygonImage.gameObject.SetActive(false);
                m_img_arrow_down_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                bool isShow = (result.selfRank > result.selfOldRank) ? false : true;
                m_img_arrow_up_PolygonImage.gameObject.SetActive(isShow);
                m_img_arrow_down_PolygonImage.gameObject.SetActive(!isShow);
            }

            m_lbl_source_LanguageText.text = (result.score > 0) ? ClientUtils.FormatComma(result.score) : "-";
            m_lbl_rank_LanguageText.text = (result.selfRank > 0) ? ActivityProxy.RankNumFormat(result.selfRank, m_comparisonAllianceLimit) : "-";
        }

        //刷新个人排名
        private void RefreshPersonalRankContent()
        {
            CancelLoading();
            m_pl_playermes.gameObject.SetActive(true);
            m_pl_ck_ToggleGroup.gameObject.SetActive(true);
            m_pl_total_sv.gameObject.SetActive(true);
            RefreshPersonalRankTitle();
            RefreshTotalList();
        }

        private void RefreshPersonalRankTitle()
        {
            m_UI_PlayerHead.gameObject.SetActive(true);
            m_UI_Model_GuildFlag.gameObject.SetActive(false);

            m_UI_PlayerHead.LoadPlayerIcon();

            Rank_QueryRank.response result = m_personalRankResponse;
            string str = m_allianceProxy.GetName();
            m_lbl_name_LanguageText.text = str;

            if (result.selfRank == result.selfOldRank)
            {
                m_img_arrow_up_PolygonImage.gameObject.SetActive(false);
                m_img_arrow_down_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                bool isShow = (result.selfRank > result.selfOldRank) ? false : true;
                m_img_arrow_up_PolygonImage.gameObject.SetActive(isShow);
                m_img_arrow_down_PolygonImage.gameObject.SetActive(!isShow);
            }

            m_lbl_source_LanguageText.text = (result.score > 0) ? ClientUtils.FormatComma(result.score) : "-";
            m_lbl_rank_LanguageText.text = (result.selfRank > 0) ? ActivityProxy.RankNumFormat(result.selfRank, m_comparisonLimit) : "-";
        }

        //刷新部落之王排名
        private void RefreshTribalKingRankContent()
        {
            m_pl_playermes.gameObject.SetActive(true);
            m_pl_total_sv.gameObject.SetActive(true);
            RefreshAllianceRankTitle();
            RefreshTotalList();
        }

        #endregion

        #region listItem

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            if (m_dataList[listItem.index].ShowType == 2)
            {
                return "UI_Item_EventTypeRankTitle";
            }
            else
            {
                return "UI_Item_EventTypeRankItem";
            }
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            if (m_dataList[listItem.index].ShowType == 2)
            {
                return m_menuTitleHeight;
            }
            else
            {
                return m_menuDataHeight;
            }
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            if (listItem.index >= m_dataList.Count)
            {
                return;
            }
            var rankData = m_dataList[listItem.index];
            if (rankData.ShowType == 2)//阶段排名标题
            {
                if (listItem.data == null)
                {
                    var subView = new UI_Item_EventTypeRankTitle_SubView(listItem.go.GetComponent<RectTransform>());
                    if (rankData.Define != null)
                    {
                        subView.m_lbl_stageName_LanguageText.text = LanguageUtils.getTextFormat(762234, rankData.Stage, LanguageUtils.getText(rankData.Define.l_nameID));
                    }
                }
                else
                {
                    var subView = listItem.data as UI_Item_EventTypeRankTitle_SubView;
                    if (rankData.Define != null)
                    {
                        subView.m_lbl_stageName_LanguageText.text = LanguageUtils.getTextFormat(762234, rankData.Stage, LanguageUtils.getText(rankData.Define.l_nameID));
                    }
                }
            }
            else
            {
                if (listItem.data == null)
                {
                    var subView = new UI_Item_EventTypeRankItem_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.Refresh(rankData);
                }
                else
                {
                    var subView = listItem.data as UI_Item_EventTypeRankItem_SubView;
                    subView.Refresh(rankData);
                }
            }
        }

        #endregion

        #region 排行榜奖励界面

        //排行榜奖励界面
        private void OnRankReward()
        {
            EventTypeRankParam param = new EventTypeRankParam();

            if (m_sourceType == (int)EventRankSourceType.CeneralCondition)
            {
                param.ActivityId = (int)m_activityId;
                param.Type = 1;
            }
            else if (m_sourceType == (int)EventRankSourceType.StrongerConsul)
            {
                param.ActivityId = (int)m_activityId;
                param.Type = 2;
                param.SubId = m_activityProxy.GetStageDefine((int)m_activityId, m_currStage, m_currLevel).subtypeID;
                param.RankType = m_ckType;
            }
            else if (m_sourceType == (int)EventRankSourceType.Warhorn)
            {
                param.ActivityId = (int)m_scheduleData.PreActivityId;
                param.Type = 4;
                param.SubId = param.ActivityId * 10000 + (-1 * 100) + 1;
                param.RankType = m_ckType;
            }
            else if (m_sourceType == (int)EventRankSourceType.TribalKing)
            {
                param.ActivityId = (int)m_scheduleData.PreActivityId;
                param.Type = 5;
                param.SubId = param.ActivityId * 10000 + (-1 * 100) + 1;
            }
            CoreUtils.uiManager.ShowUI(UI.s_eventTypeRankReward, null, param);
        }

        public void AddBackListener(UnityAction callback)
        {
            m_btn_back_GameButton.onClick.AddListener(callback);
        }

        #endregion

        #region 复选框事件

        //联盟排名
        private void OnAllianceRank(bool isCk)
        {
            SetTotalColorSelectStatus(isCk);
            if (isCk)
            {
                if (m_ckType == 1)
                {
                    return;
                }
                m_ckType = 1;
                m_pl_playermes.gameObject.SetActive(false);
                //判断是否请求过数据
                if (m_alliancRankDatalist == null)
                {
                    RequestAllianceRank();
                }
                else
                {
                    m_dataList = m_alliancRankDatalist;
                    RefreshAllianceRankContent();
                }
            }
        }

        //个人排名
        private void OnPersonalRank(bool isCk)
        {
            SetSingleColorSelectStatus(isCk);
            if (isCk)
            {
                if (m_ckType == 2)
                {
                    return;
                }
                m_ckType = 2;
                m_pl_playermes.gameObject.SetActive(false);
                //判断是否请求过数据
                if (m_personalRankdDatalist == null)
                {
                    RequestPersonalRank();
                }
                else
                {
                    m_dataList = m_personalRankdDatalist;
                    RefreshPersonalRankContent();
                }
            }
        }

        //总排名
        private void OnTotalRank(bool isCk)
        {
            SetTotalColorSelectStatus(isCk);
            if (isCk)
            {
                if (m_ckType == 1)
                {
                    return;
                }
                m_ckType = 1;
                m_pl_stage_sv.gameObject.SetActive(false);
                m_pl_stage.gameObject.SetActive(false);
                m_pl_playermes.gameObject.SetActive(false);
                //判断是否请求过数据
                if (m_totalRankDatalist == null)
                {
                    RequestRankData(0);
                }
                else
                {
                    m_dataList = m_totalRankDatalist;
                    RefreshTotalRankContent();
                }
            }
        }

        //阶段排名
        private void OnStageRank(bool isCk)
        {
            SetSingleColorSelectStatus(isCk);
            if (isCk)
            {
                if (m_ckType == 2)
                {
                    return;
                }
                m_ckType = 2;
                m_pl_total_sv.gameObject.SetActive(false);
                m_pl_playermes.gameObject.SetActive(false);
                //判断是否请求数据
                if (m_stageRankDataList == null)
                {
                    ReqeustStageData();
                }
                else
                {
                    m_dataList = m_stageRankDataList;
                    RefreshStageRankContent();
                }
            }
        }

        #endregion

        #region 菊花loading

        private void StartTimer()
        {
            CancelTimer();
            m_loadingTimer = Timer.Register(0.2f, ShowLoading);
        }

        private void CancelTimer()
        {
            if (m_loadingTimer != null)
            {
                m_loadingTimer.Cancel();
                m_loadingTimer = null;
            }
        }

        private void CancelLoading()
        {
            CancelTimer();
            HideLoading();
        }

        private void ShowLoading()
        {
            if (gameObject == null)
            {
                return;
            }
            m_img_loading_PolygonImage.gameObject.SetActive(true);    
        }

        private void HideLoading()
        {
            m_img_loading_PolygonImage.gameObject.SetActive(false);
        }

        #endregion

        #region 请求数据 回包数据

        //请求联盟排名数据
        private void RequestAllianceRank()
        {
            StartTimer();
            m_requestType = m_leaderboardAllianceId;

            m_isRequesting = true;
            var sp = new Rank_QueryRank.request();
            sp.type = m_leaderboardAllianceId;
            sp.num = m_requestAllianceNum;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //请求个人排名数据
        private void RequestPersonalRank()
        {
            StartTimer();

            m_isRequesting = true;
            m_requestType = m_leaderboardId;

            var sp = new Rank_QueryRank.request();
            sp.type = m_leaderboardId;
            sp.num = m_requestNum;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //请求部落之王 排名数据
        private void RequestTribalKingRank()
        {
            StartTimer();

            m_isRequesting = true;
            m_requestType = m_leaderboardAllianceId;

            var sp = new Rank_QueryRank.request();
            sp.type = m_leaderboardAllianceId;
            sp.num = m_requestAllianceNum;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //请求总排名数据
        private void RequestRankData(int num)
        {
            StartTimer();

            num = num + m_requestNum;
            m_isRequesting = true;
            m_requestType = m_leaderboardId;

            var sp = new Rank_QueryRank.request();
            sp.type = m_leaderboardId;
            sp.num = num;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //请求阶段排名数据
        private void ReqeustStageData()
        {
            if (m_isRequestingStage)
            {
                return;
            }
            StartTimer();

            m_isRequestingStage = true;
            var sp = new Activity_GetRank.request();
            sp.type = m_activityDefine.activityType;
            sp.activityId = m_activityId;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //总排名数据
        private void ReceiveRankData(object body)
        {
            if (gameObject == null)
            {
                return;
            }
            var response = body as Rank_QueryRank.response;
            if (m_requestType != response.type)
            {
                Debug.LogFormat("requestType:{0} type:{1} 不一致", m_requestType, response.type);
                return;
            }
            CancelLoading();

            int status = 0;
            if (m_sourceType == (int)EventRankSourceType.Warhorn) //战斗号角
            {
                if (m_ckType == 1)
                {
                    status = 1;
                }
            }
            else if (m_sourceType == (int)EventRankSourceType.TribalKing)//部落之王
            {
                status = 1;
            }


            List<RankItemData> list = new List<RankItemData>();
            if (response.rankList != null)
            {
                for (int i = 0; i < response.rankList.Count; i++)
                {
                    RankItemData data = new RankItemData();
                    data.ShowType = 1;
                    data.RankData = response.rankList[i];
                    data.Status = status;
                    list.Add(data);
                }
            }
            Debug.LogFormat("data count:{0}", list.Count);
            //ClientUtils.Print(response);

            m_dataList = list;

            if (m_sourceType == (int)EventRankSourceType.Warhorn) //战斗号角
            {
                if (m_ckType == 1)//联盟排名数据
                {
                    m_allianceRankResponse = response;

                    m_alliancRankDatalist = list;
                    RefreshAllianceRankContent();
                }
                else//个人排名数据
                {
                    m_personalRankResponse = response;

                    m_personalRankdDatalist = list;
                    RefreshPersonalRankContent();
                }
            }
            else if (m_sourceType == (int)EventRankSourceType.TribalKing)//部落之王
            {
                m_allianceRankResponse = response;
                m_alliancRankDatalist = list;
                RefreshTribalKingRankContent();
            }
            else
            {
                m_totalRankResponse = response;
                m_totalRankDatalist = list;
                RefreshTotalRankContent();
            }

            m_isRequesting = false;
        }

        //阶段排名数据
        private void ReceiveStageData(object body)
        {
            if (gameObject == null)
            {
                return;
            }
            CancelLoading();
            var response = body as Activity_GetRank.response;
            if (m_stageRankDataList == null)
            {
                m_stageRankDataList = new List<RankItemData>();
            }
            if (response.rankList != null)
            {
                List<Activity_GetRank.response.RankList> tempList = new List<Activity_GetRank.response.RankList>();
                foreach (var data in response.rankList)
                {
                    tempList.Add(data.Value);
                }
                if (tempList.Count > 0)
                {
                    tempList.Sort((Activity_GetRank.response.RankList x, Activity_GetRank.response.RankList y) => {
                        int re = y.index.CompareTo(x.index);
                        return re;
                    });
                }
                for (int i = 0; i < tempList.Count; i++)
                {
                    RankItemData itemData = new RankItemData();
                    itemData.ShowType = 2;
                    itemData.Stage = (int)tempList[i].index;
                    itemData.Define = m_activityProxy.GetStageDefine((int)m_activityId, itemData.Stage, m_currLevel);
                    m_stageRankDataList.Add(itemData);

                    if (tempList[i].ranks != null)
                    {
                        List<Activity_GetRank.response.RankList.RankInfo> rankList = tempList[i].ranks;
                        for (int k = 0; k < rankList.Count; k++)
                        {
                            RankItemData itemData1 = new RankItemData();
                            itemData1.ShowType = 3;
                            itemData1.StageRankData = rankList[k];
                            m_stageRankDataList.Add(itemData1);
                        }
                    }
                }
            }
            m_dataList = m_stageRankDataList;
            m_stageRankResponse = response;
            //ClientUtils.Print(response);
            Debug.LogFormat("data count:{0}", m_stageRankDataList.Count);
            RefreshStageRankContent();

            m_isRequestingStage = false;
        }

        #endregion

        #region 其他

        private void SetTotalColorSelectStatus(bool isSelect)
        {
            if (isSelect)
            {
                ClientUtils.TextSetColor(m_lbl_total_LanguageText, "#ffffff");
            }
            else
            {
                ClientUtils.TextSetColor(m_lbl_total_LanguageText, "#a49d92");
            }
        }

        private void SetSingleColorSelectStatus(bool isSelect)
        {
            if (isSelect)
            {
                ClientUtils.TextSetColor(m_lbl_single_LanguageText, "#ffffff");
            }
            else
            {
                ClientUtils.TextSetColor(m_lbl_single_LanguageText, "#a49d92");
            }
        }

        #endregion
    }
}