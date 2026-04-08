// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    UI_Win_RankingMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public enum RankingListType
    {
        Activity = 1, // 活动排行
        Base, // 基础排行
        Guild, // 联盟排行
    }

    public enum RankingTarget
    {
        Player = 1, // 玩家
        Guild // 联盟
    }

    public enum RankingTypeValue
    {
        GuildTotalPower = 1,
        GuildTotalKill = 2,
        GuildTotalFlag = 3,
        PlayerPower = 4,
        PlayerKill = 5,
        TownLv = 6,
        CollectCount = 9
    }

    public class RankingViewData
    {
         public LeaderboardDefine TargetLeaderboard { get; set; }
    }

    public class UI_Win_RankingMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Win_RankingMediator";

        public static string[] RankingTop3IconName = new string[]
            {"ui_activity[img_activity_rank1]", "ui_activity[img_activity_rank2]", "ui_activity[img_activity_rank3]"};
        public static string[] RankingTopTitle = new string[] { "img_activity_title1", "img_activity_title2", "img_activity_title3", "img_activity_title4"};


        public List<LeaderboardDefine> m_leaderboardDefines = new List<LeaderboardDefine>();
        private List<LeaderboardDefine> m_baseLeaderboardDefines = new List<LeaderboardDefine>();

        Dictionary<int, Rank_ShowRankFirst.response.RankInfo> rankInfos =
            new Dictionary<int, Rank_ShowRankFirst.response.RankInfo>();


        private List<UI_Item_GuildRankingBtn_SubView> m_entryBtns = new List<UI_Item_GuildRankingBtn_SubView>();

        private LeaderboardDefine m_curLeaderboardDefine;
        private int m_curQueryCount = 100; // 当前查询数量
        private int m_queryAccum = 100; // 每次累加查询数量
        private bool m_isQueringRefreshRanking;
        private bool m_isAllRankingReceived = false;
        private bool m_isLoadingPrefab = false;
        private string crossBar = "";
        private bool m_isOpenDetail;
        private Timer m_loadTimer;
        private bool m_isLoadFinish = false;

        private WorldMapObjectProxy worldMapObjectProxy;
        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;

        private List<Rank_QueryRank.response.RankInfo> rankListViewList = new List<Rank_QueryRank.response.RankInfo>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Rank_QueryRank.response m_curQueryRankRes;
        private int m_curSelfRank;

        #endregion

        //IMediatorPlug needs
        public UI_Win_RankingMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_RankingView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Rank_QueryRank.TagName,
                Rank_ShowRankFirst.TagName,
                Role_GetRoleInfo.TagName,
                Guild_GetOtherGuildInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Rank_ShowRankFirst.TagName:
                    Rank_ShowRankFirst.response res1 = notification.Body as Rank_ShowRankFirst.response;
                    QueryShowRankFirstRes(res1);
                    break;
                case Rank_QueryRank.TagName:
                    m_curQueryRankRes = notification.Body as Rank_QueryRank.response;
                    QueryRefreshRankingRes(m_curQueryRankRes);
                    break;
                case Role_GetRoleInfo.TagName:
                    Role_GetRoleInfo.response resRoleInfo = notification.Body as Role_GetRoleInfo.response;
                    QueryGetRoleInfoRes(resRoleInfo);
                    break;
                case Guild_GetOtherGuildInfo.TagName:
                    Guild_GetOtherGuildInfo.response req = notification.Body as Guild_GetOtherGuildInfo.response;
                    QueryGetOtherGuildInfoRes(req);
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
        }

        public override void WinClose()
        {
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            
            
            crossBar = LanguageUtils.getText(300232);

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_leaderboardDefines = CoreUtils.dataService.QueryRecords<LeaderboardDefine>();

            view.m_pl_Total_GridLayoutGroup.gameObject.SetActive(false);
            view.m_pl_Detail.gameObject.SetActive(false);

            view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(() => { onClose(); });
            
            view.m_UI_Common_Spin.gameObject.SetActive(false);

            LeaderboardDefine targetLeaderboard = null;
            if (view.data != null)
            {
                RankingViewData viewData = view.data as RankingViewData;
                if(viewData != null)
                {
                    targetLeaderboard = viewData.TargetLeaderboard;
                }
            }          

            if(targetLeaderboard == null)
            {
                BindEntryBtns();
                DataProcess();
                QueryShowRankFirst();
            }          
            else
            {
                OnClickEntry(targetLeaderboard);
            }
        }

        protected override void BindUIEvent()
        {
            for (int i = 0; i < m_entryBtns.Count; i++)
            {
                if (i < m_baseLeaderboardDefines.Count)
                {
                    BindEntryBtnClick(m_entryBtns[i].m_btn_btn_GameButton, m_baseLeaderboardDefines[i]);
                }
            }
        }

        protected override void BindUIData()
        {
        }

        #endregion

        #region 排行榜入口相关

        private void QueryShowRankFirst()
        {
            Rank_ShowRankFirst.request req = new Rank_ShowRankFirst.request()
            {
                type = 1
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        private void BindEntryBtnClick(GameButton btn, LeaderboardDefine define)
        {
            btn.onClick.AddListener(() => { OnClickEntry(define); });
        }

        void QueryShowRankFirstRes(Rank_ShowRankFirst.response res)
        {
            view.m_pl_Total_GridLayoutGroup.gameObject.SetActive(true);
            rankInfos.Clear();

            foreach (var dict in res.rankInfo)
            {
                Rank_ShowRankFirst.response.RankInfo rankInfo = dict.Value;
                rankInfos.Add((int) rankInfo.type, rankInfo);
            }

            // 入口按钮状态刷新
            for (int i = 0; i < m_entryBtns.Count; i++)
            {
                if (i < m_baseLeaderboardDefines.Count)
                {
                    m_entryBtns[i].gameObject.SetActive(true);
                    LeaderboardDefine define = m_baseLeaderboardDefines[i];
                    Rank_ShowRankFirst.response.RankInfo rankInfo = null;
                    if (rankInfos.ContainsKey(define.ID))
                        rankInfo = rankInfos[define.ID];

                    RefreshEntryBtn(m_entryBtns[i], define, rankInfo);
                }
                else
                {
                    m_entryBtns[i].gameObject.SetActive(false);
                }
            }
        }

        private void BindEntryBtns()
        {
            m_entryBtns.Add(view.m_UI_Guild0);
            m_entryBtns.Add(view.m_UI_Guild1);
            m_entryBtns.Add(view.m_UI_Guild2);
            m_entryBtns.Add(view.m_UI_Person0);
            m_entryBtns.Add(view.m_UI_Person1);
            m_entryBtns.Add(view.m_UI_Person2);
            m_entryBtns.Add(view.m_UI_Other0);
            m_entryBtns.Add(view.m_UI_Other1);
            m_entryBtns.Add(view.m_UI_Other2);
        }

        private void DataProcess()
        {
            m_baseLeaderboardDefines.Clear();

            for (int i = 0; i < m_leaderboardDefines.Count; i++)
            {
                if (m_leaderboardDefines[i].list == (int) RankingListType.Base)
                {
                    m_baseLeaderboardDefines.Add(m_leaderboardDefines[i]);
                }
            }

            m_baseLeaderboardDefines.Sort((a, b) => { return a.ID - b.ID; });
        }

        // 刷新入口按钮
        private void RefreshEntryBtn(UI_Item_GuildRankingBtn_SubView subView, LeaderboardDefine define,
            Rank_ShowRankFirst.response.RankInfo rankInfo)
        {
            if (rankInfo == null)
            {
                CoreUtils.logService.Warn($"排行榜 刷新入口按钮    客户端配置的类型，服务器不包含   type:[{define.type}]");
                subView.m_lbl_mid_LanguageText.text = crossBar;
                subView.m_lbl_name_LanguageText.text = crossBar;
            }
            else
            {
                string name = "";
                string abbreviationName = string.IsNullOrEmpty(rankInfo.abbreviationName)
                    ? crossBar
                    : rankInfo.abbreviationName;

                switch ((RankingTarget) define.target)
                {
                    case RankingTarget.Player:
                        subView.m_lbl_mid_LanguageText.text = rankInfo.name;
                        subView.m_lbl_name_LanguageText.text =
                            string.IsNullOrEmpty(rankInfo.guildName) ? crossBar : LanguageUtils.getTextFormat(300030, abbreviationName, rankInfo.guildName);
                        break;
                    case RankingTarget.Guild:
                        subView.m_lbl_name_LanguageText.text = rankInfo.leaderName;
                        subView.m_lbl_mid_LanguageText.text = LanguageUtils.getTextFormat(300030, abbreviationName, rankInfo.guildName);
                        break;
                    default:

                        CoreUtils.logService.Warn($" 排行榜  刷新入口按钮  位置的目标类型:[{define.target}]");
                        break;
                }
            }

            subView.m_lbl_title_LanguageText.text = LanguageUtils.getText(define.nameID);

            if (string.IsNullOrEmpty(define.icon))
            {
                subView.m_img_icon_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                subView.m_img_icon_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(subView.m_img_icon_PolygonImage, define.icon);
            }

            if (string.IsNullOrEmpty(define.flag))
            {
                subView.m_img_flag_PolygonImage.enabled = false;
            }
            else
            {
                subView.m_img_flag_PolygonImage.enabled = true;
                ClientUtils.LoadSprite(subView.m_img_flag_PolygonImage, define.flag);
            }
        }

        private void OnClickEntry(LeaderboardDefine define)
        {
            m_curLeaderboardDefine = define;
            if (m_assetDic.Count == 0 && !m_isLoadingPrefab)
            {
                m_isLoadingPrefab = true;
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
                return;
            }
            RefreshDetailPanel();
        }

        private void RefreshDetailPanel()
        {
            m_isAllRankingReceived = false;
            m_curQueryCount = Mathf.Min(m_queryAccum, m_curLeaderboardDefine.showLimit + 1);
            m_isOpenDetail = true;
            view.m_sv_list_ListView.RefreshAndRestPos();
            QueryRefreshRanking(m_curLeaderboardDefine);
        }

        #endregion

        #region 排行界面

        /// <summary>
        /// 请求刷新排行榜
        /// </summary>
        /// <param name="define">刷新指定排行榜数据</param>
        void QueryRefreshRanking(LeaderboardDefine define)
        {
            if (m_isQueringRefreshRanking)
                return;

            view.m_pl_Total_GridLayoutGroup.gameObject.SetActive(false);
            view.m_pl_Detail.gameObject.SetActive(true);
            view.m_pl_bletContent.gameObject.SetActive(false);
            view.m_img_title_bg_PolygonImage.gameObject.SetActive(false);

            view.m_lbl_noOne_LanguageText.gameObject.SetActive(false);
            view.m_sv_list_ListView.gameObject.SetActive(false);          
            
            if (m_loadTimer != null)
            {
                m_loadTimer.Cancel();
                m_loadTimer = null;
            }

            m_isLoadFinish = false;
            m_loadTimer = Timer.Register(0.2f, () =>
            {
                if (!m_isLoadFinish)
                {
                    view.m_UI_Common_Spin.gameObject.SetActive(true);
                }
            });
            
            m_isQueringRefreshRanking = true;
            Rank_QueryRank.request req = new Rank_QueryRank.request();
            req.type = define.ID;
            req.num = m_curQueryCount;
            
            AppFacade.GetInstance().SendSproto(req);
        }

        void QueryRefreshRankingRes(Rank_QueryRank.response res)
        {
            m_isQueringRefreshRanking = false;
            if (m_curLeaderboardDefine == null)
            {
                CoreUtils.logService.Error($"配表错误！ 请检查该类型配置是否存在");
                return;    
            }

            if (res == null)
            {
                CoreUtils.logService.Error($"排行榜 服务器下发协议等于空");
                return;
            }

            m_curSelfRank = (int)(res.HasSelfRank ? res.selfRank : 0);
            
            bool showNoOne = false;
            if (res.rankList.Count <= 0)
            {
                showNoOne = true;
            }
            RefreshTopSelf(res);
            RefreshListView(res);
            view.m_lbl_noOne_LanguageText.gameObject.SetActive(showNoOne);
            view.m_sv_list_ListView.gameObject.SetActive(!showNoOne);
        }

        /// <summary>
        /// 刷新顶部自己的数据
        /// </summary>
        /// <param name="res"></param>
        void RefreshTopSelf(Rank_QueryRank.response res)
        {
            view.m_pl_bletContent.gameObject.SetActive(true);
            view.m_img_title_bg_PolygonImage.gameObject.SetActive(true);
            bool hasAlliance = m_allianceProxy.HasJionAlliance();
            string roleName;

            view.m_UI_PlayerHead.gameObject.SetActive(false);
            view.m_UI_GuildFlag.gameObject.SetActive(false);

            string powValue = ClientUtils.FormatComma(m_curQueryRankRes.score);

            RefreshBlet(m_curSelfRank);
            RefreshPowerUpOrDown(view.m_img_arrowUp_PolygonImage, view.m_img_arrowDown_PolygonImage, m_curSelfRank, m_curSelfRank);
            
//            CoreUtils.logService.Error($"[刷新顶部自己的数据]  powValue:[{powValue}]  selfRank:[{res.selfRank}]  defineId:[{m_curLeaderboardDefine.ID}]   ");
            
            try
            {
//                CoreUtils.logService.Error($"[刷新顶部自己的数据]  typeNameID[0]:[{m_curLeaderboardDefine.typeNameID[0]}] ");   
                view.m_lbl_playerPow_LanguageText.text = LanguageUtils.getTextFormat(m_curLeaderboardDefine.typeNameID[1], powValue);
                view.m_lbl_itemTitle1_LanguageText.text = LanguageUtils.getText(m_curLeaderboardDefine.typeNameID[0]);
            }
            catch (Exception e)
            {
                CoreUtils.logService.Warn("排行版  刷新顶部自己的数据  策划配置错误  战力与战力: ");
            }

            GuildInfoEntity guildInfoEntity = m_allianceProxy.GetAlliance();
            string guildAbbName = guildInfoEntity != null ? guildInfoEntity.abbreviationName : "";
            switch ((RankingTarget) m_curLeaderboardDefine.target)
            {
                case RankingTarget.Player:
                    view.m_UI_PlayerHead.gameObject.SetActive(true);

                    string selfRankInfo = m_curSelfRank.ToString();
                    if (m_curSelfRank > m_curLeaderboardDefine.showLimit)
                    {
                        selfRankInfo = LanguageUtils.getTextFormat(785020, m_curLeaderboardDefine.showLimit);
                    }
                    
                    // 排名
                    view.m_lbl_ranking_LanguageText.text = selfRankInfo;

                    if (m_curSelfRank <= 3 && m_curSelfRank > 0)
                    {
                        view.m_img_rank_PolygonImage.enabled = true;
                        ClientUtils.LoadSprite(view.m_img_rank_PolygonImage, RankingTop3IconName[m_curSelfRank - 1]);
                    }
                    else
                    {
                        view.m_img_rank_PolygonImage.enabled = false;
                    }


                    // 角色头像 
                    view.m_UI_PlayerHead.LoadPlayerIcon();

                    // 角色名称
                    roleName = hasAlliance
                        ? LanguageUtils.getTextFormat(300030, guildAbbName, m_playerProxy.CurrentRoleInfo.name)
                        : m_playerProxy.CurrentRoleInfo.name;
                    view.m_lbl_playerName_LanguageText.text = roleName;

                    // 联盟名称
                    string allianceName =
                        hasAlliance ? m_allianceProxy.GetAllianceName() : crossBar;
                    view.m_lbl_guildName_LanguageText.text = allianceName;
                    break;
                case RankingTarget.Guild:
                    view.m_UI_GuildFlag.gameObject.SetActive(true);
                    
                    string selfRankInfo_guild = hasAlliance ? m_curSelfRank.ToString() : crossBar;
                    if (hasAlliance && m_curSelfRank > m_curLeaderboardDefine.showLimit)
                    {
                        selfRankInfo_guild = LanguageUtils.getTextFormat(785020, m_curLeaderboardDefine.showLimit);
                    }
                    
                    // 排名
                    view.m_lbl_ranking_LanguageText.text = selfRankInfo_guild;

                    if (m_curSelfRank <= 3 && m_curSelfRank > 0)
                    {
                        view.m_img_rank_PolygonImage.enabled = true;
                        ClientUtils.LoadSprite(view.m_img_rank_PolygonImage, RankingTop3IconName[m_curSelfRank - 1]);
                    }
                    else
                    {
                        view.m_img_rank_PolygonImage.enabled = false;
                    }


                    // 设置联盟图标
                    view.m_UI_GuildFlag.setData(m_allianceProxy.GetAlliance());

                    // 角色名称
                    roleName = hasAlliance
                        ? LanguageUtils.getTextFormat(300030, guildAbbName, m_allianceProxy.GetAllianceName())
                        : crossBar;
                    view.m_lbl_playerName_LanguageText.text = roleName;

                    // 盟主名称
                    view.m_lbl_guildName_LanguageText.text =
                        hasAlliance ? LanguageUtils.getTextFormat(785002, m_allianceProxy.GetAlliance().leaderName) : crossBar;
                    break;
                default:
                    CoreUtils.logService.Warn($"未知的排行榜对象  :[{m_curLeaderboardDefine}] ");
                    break;
            }
        }

        /// <summary>
        /// 排行数据变更，刷新
        /// </summary>
        /// <param name="res"></param>
        void RefreshListView(Rank_QueryRank.response res)
        {
            m_isAllRankingReceived = res.rankList.Count < m_curQueryCount;
            rankListViewList.Clear();
            rankListViewList.AddRange(res.rankList);
            view.m_sv_list_ListView.FillContent(rankListViewList.Count);
            m_isLoadFinish = true;
            view.m_UI_Common_Spin.gameObject.SetActive(false);
            if (m_loadTimer != null)
            {
                m_loadTimer.Cancel();
            }
            m_loadTimer = null;
//            view.m_sv_list_ListView.ForceRefresh();
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            InitRankViewList();
            RefreshDetailPanel();
        }

        /// <summary>
        /// 初始化排行榜列表
        /// </summary>
        private void InitRankViewList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            if (!listItem.isInit)
            {
                listItem.isInit = true;
            }

            UI_Item_RankingItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_RankingItemView>(listItem.go);
            Rank_QueryRank.response.RankInfo rankInfo = rankListViewList[listItem.index];

            RefreshRankingItemView(itemView, rankInfo);

            if (!m_isAllRankingReceived && (listItem.index + 60 <= m_curLeaderboardDefine.showLimit) && listItem.index + 60 > m_curQueryCount) // 检测是否需要再次请求数据
            {
                m_curQueryCount += m_queryAccum;
                m_curQueryCount = Mathf.Min(m_curQueryCount, m_curLeaderboardDefine.showLimit + 1);
                
                QueryRefreshRanking(m_curLeaderboardDefine);
            }
        }

        /// <summary>
        /// 刷新单条滑动列表数据
        /// </summary>
        /// <param name="itemView"></param>
        /// <param name="rankInfo"></param>
        private void RefreshRankingItemView(UI_Item_RankingItemView itemView, Rank_QueryRank.response.RankInfo rankInfo)
        {
            // 排名up or down 处理
            itemView.m_img_arrowUp_PolygonImage.gameObject.SetActive(false);
            itemView.m_img_arrowDown_PolygonImage.gameObject.SetActive(false);
            
            int selfRank = (int) rankInfo.index;
            int selfOldRank = (int) rankInfo.oldRank;
            RefreshPowerUpOrDown(itemView.m_img_arrowUp_PolygonImage, itemView.m_img_arrowDown_PolygonImage, selfRank, selfOldRank);

            string rankInfoTxt = ClientUtils.FormatComma(rankInfo.index);
            if (rankInfo.index > m_curLeaderboardDefine.showLimit)
            {
                rankInfoTxt = LanguageUtils.getTextFormat(785020, m_curLeaderboardDefine.showLimit);
            }
            
            itemView.m_lbl_rank_LanguageText.text = rankInfoTxt;
            itemView.m_lbl_power_LanguageText.text = ClientUtils.FormatComma(rankInfo.score);
            if (rankInfo.index <= 3 && rankInfo.index > 0)
            {
                itemView.m_img_rank_PolygonImage.enabled = true;
                ClientUtils.LoadSprite(itemView.m_img_rank_PolygonImage, RankingTop3IconName[rankInfo.index - 1]);
            }
            else
            {
                itemView.m_img_rank_PolygonImage.enabled = false;
            }

            itemView.m_UI_PlayerHead.gameObject.SetActive(false);
            itemView.m_UI_GuildFlag.gameObject.SetActive(false);

            itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
            {
                if (rankInfo.index != m_curSelfRank)
                {
                    if (m_curLeaderboardDefine.target == (int) RankingTarget.Player)
                    {
                        Role_GetRoleInfo.request req = new Role_GetRoleInfo.request();
                        req.queryRid = rankInfo.rid;
                        AppFacade.GetInstance().SendSproto(req);
                    }
                    else if (m_curLeaderboardDefine.target == (int) RankingTarget.Guild) // 对象为联盟处理
                    {
                        Guild_GetOtherGuildInfo.request request = new Guild_GetOtherGuildInfo.request();
                        request.guildId = rankInfo.guildId;
                        AppFacade.GetInstance().SendSproto(request);
                    }
                }
            });

            try
            {
                string abbreviationName = string.IsNullOrEmpty(rankInfo.abbreviationName)
                    ? crossBar
                    : rankInfo.abbreviationName;
                bool hasGuild = !string.IsNullOrEmpty(rankInfo.guildName);
                string guildName = hasGuild ? rankInfo.guildName : crossBar;
                string leaderName = string.IsNullOrEmpty(rankInfo.leaderName) ? crossBar : rankInfo.leaderName;

                if (m_curLeaderboardDefine.target == (int) RankingTarget.Player) // 对象为玩家处理
                {
                    itemView.m_lbl_name_LanguageText.text = hasGuild ? LanguageUtils.getTextFormat(300030, abbreviationName, rankInfo.name) : rankInfo.name;
                    itemView.m_lbl_guildName_LanguageText.text = guildName;
                    itemView.m_UI_PlayerHead.gameObject.SetActive(true);
                    itemView.m_UI_PlayerHead.LoadPlayerIcon(rankInfo.headId, rankInfo.headFrameID);
                }
                else if (m_curLeaderboardDefine.target == (int) RankingTarget.Guild) // 对象为联盟处理
                {
                    itemView.m_lbl_name_LanguageText.text =
                        LanguageUtils.getTextFormat(300030, abbreviationName, guildName);
                    itemView.m_lbl_guildName_LanguageText.text = LanguageUtils.getTextFormat(785002, leaderName);

                    List<int> signList = new List<int>();
                    for (int i = 0; i < rankInfo.signs.Count; i++)
                        signList.Add((int) rankInfo.signs[i]);
                    itemView.m_UI_GuildFlag.gameObject.SetActive(true);
                    itemView.m_UI_GuildFlag.setData(signList);
                }
            }
            catch (Exception e)
            {
                CoreUtils.logService.Warn($"排行榜刷新单条数据错误   \nerr:[{e}]");
            }
        }

        private string GetSelfRankValueByType(int type)
        {
            int value = 0;

            switch ((RankingTypeValue) type)
            {
                case RankingTypeValue.GuildTotalPower:
                    if (m_allianceProxy.HasJionAlliance())
                    {
                        value = (int) m_allianceProxy.GetAlliance().power;
                    }
                    break;
                case RankingTypeValue.GuildTotalKill:    // 联盟击杀功能还未有
//                    if (m_allianceProxy.HasJionAlliance())
//                        value = (int)m_allianceProxy.GetAlliance();
                    break;
                case RankingTypeValue.GuildTotalFlag:
                    if (m_allianceProxy.HasJionAlliance())
                        value = (int) m_allianceProxy.GetAlliance().territory;
                    break;
                case RankingTypeValue.PlayerPower:
                    value = (int) m_playerProxy.Power();
                    break;
                case RankingTypeValue.PlayerKill:
                    value = (int) m_playerProxy.killCount();
                    break;
                case RankingTypeValue.TownLv:
                    value = (int) m_playerProxy.GetTownHall();
                    break;
                case RankingTypeValue.CollectCount:
                    value = (int) m_playerProxy.ResourceCollection();
                    break;
            }
            
            return ClientUtils.FormatComma(value);
        }

        private void RefreshPowerUpOrDown(PolygonImage up, PolygonImage down, int curRank, int oldRank)
        {
            // 排名up or down 处理
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
            if (curRank < oldRank)
            {
                up.gameObject.SetActive(true);
            }
            else if (curRank > oldRank)
            {
                down.gameObject.SetActive(true);
            }
        }

        private void RefreshBlet(int rankValue)
        {
            string bletName = RankingTopTitle[3];
            if (rankValue <= 3 && rankValue > 0)
            {
                bletName = RankingTopTitle[rankValue - 1];
            }
            
            ClientUtils.LoadSprite(view.m_img_Blet_PolygonImage, bletName);
        }

        #endregion

        #region 查询其他个人or联盟信息

        private void QueryGetRoleInfoRes(Role_GetRoleInfo.response res)
        {
            CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo, null, res.roleInfo);
        }
        
        private void QueryGetOtherGuildInfoRes(Guild_GetOtherGuildInfo.response res)
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo, null, res.guildInfo);
        }

        #endregion

        private void onClose()
        {
            if (m_isOpenDetail && view.data == null)
            {
                m_isOpenDetail = false;
                view.m_pl_Total_GridLayoutGroup.gameObject.SetActive(true);
                view.m_pl_Detail.gameObject.SetActive(false);
                view.m_UI_Common_Spin.gameObject.SetActive(false);
                if (m_loadTimer != null)
                {
                    m_loadTimer.Cancel();
                }
                m_loadTimer = null;
            }
            else
            {
                CoreUtils.uiManager.CloseUI(UI.s_ranking);
            }
        }
    }
}