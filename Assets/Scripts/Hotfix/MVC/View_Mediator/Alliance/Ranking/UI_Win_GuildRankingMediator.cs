// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月2日
// Update Time         :    2020年6月2日
// Class Description   :    UI_Win_GuildRankingMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public enum AllianceRankingType
    {
        Power = 300,
        Kill = 301,
        Tech = 302,
        Building = 303,
        Help = 304,
        Support = 305,
    }
    public class UI_Win_GuildRankingMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildRankingMediator";

        private List<UI_Item_GuildRankingBtn_SubView> m_rankingBtns=new List<UI_Item_GuildRankingBtn_SubView>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;
        private List<Rank_QueryRank.response.RankInfo> m_rankingInfos;
        private Rank_QueryRank.response m_curQueryRankRes;
        private Timer m_resetTimer;
        private int m_curQueryCount = 100; // 当前查询数量
        private int m_queryAccum = 100; // 每次累加查询数量
        private bool m_isAllRankingReceived = false;
        private bool m_isQueringRefreshRanking = false;
        private bool m_isOpenDetail = false;
        private LeaderboardDefine m_curLeaderboardDefine;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildRankingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildRankingView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Rank_QueryRank.TagName,
                Rank_ShowRankFirst.TagName,
                Role_GetRoleInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Rank_ShowRankFirst.TagName:
                    Rank_ShowRankFirst.response res1 = notification.Body as Rank_ShowRankFirst.response;
                    SetRankingBtnInfo(res1);
                    break;
                case Rank_QueryRank.TagName:
                    m_curQueryRankRes = notification.Body as Rank_QueryRank.response;
                    QueryRefreshRankingRes(m_curQueryRankRes);
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            ClearResetTimer();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            ClientUtils.PreLoadRes(view.gameObject,view.m_sv_list_ListView.ItemPrefabDataList,LoadItemFinish);
            
            m_rankingBtns = new List<UI_Item_GuildRankingBtn_SubView>()
            {
                view.m_UI_Builder,
                view.m_UI_Help,
                view.m_UI_Killer,
                view.m_UI_Person,
                view.m_UI_Resource,
                view.m_UI_Technology,
            };
            DisableRankingBtn();

        }

        protected override void BindUIEvent()
        {

            view.m_UI_Model_Window_Type3.AddCloseEvent(OnClickClose);
            
            view.m_btn_help_GameButton.onClick.AddListener(OnClickTip);
        }

        protected override void BindUIData()
        {
            RefreshResetTime();
            QueryShowRankFirst();
            ShowDetail(false);
        }
       
        #endregion

        #region 排行榜入口

        //刷新排行榜入口按钮数据
        private void SetRankingBtnInfo(Rank_ShowRankFirst.response res)
        {
            var infos = res.rankInfo;
            DisableRankingBtn();
            if (infos == null)
            {
                return;
            }

            int i = 0;
            foreach (var info in infos)
            {
                var cfg = CoreUtils.dataService.QueryRecord<LeaderboardDefine>((int)info.Key);
                if (cfg.target != 3)
                {
                    continue;
                }
                m_rankingBtns[i].SetBtnInfo(info.Value,OnClickRankingBtn);
                m_rankingBtns[i].gameObject.SetActive(true);
                i++;
            }
        }
        //关闭所有排行榜入口按钮
        private void DisableRankingBtn()
        {
            foreach (var btnSubView in m_rankingBtns)
            {
                btnSubView.gameObject.SetActive(false);
            }
        }
        
        //刷新重置时间
        private void RefreshResetTime()
        {
            ClearResetTimer();

            long resetTime = ServerTimeModule.Instance.GetNextSundayTime() + ServerTimeModule.Instance.GetServerTime();
            UpdateTime(resetTime);
            m_resetTimer = Timer.Register(1, () =>
            {
                UpdateTime(resetTime);
            },null,true);
        }

        private void UpdateTime(long resetTime)
        {
            long leftTime = resetTime - ServerTimeModule.Instance.GetServerTime();
            if (leftTime <= 0)
            {
                leftTime = 0;
                ClearResetTimer();
                QueryShowRankFirst();
            }
            view.m_lbl_lifeTime_LanguageText.text = LanguageUtils.getTextFormat(730201, ClientUtils.FormatCountDown((int)leftTime));
        }

        private void ClearResetTimer()
        {
            if (m_resetTimer != null)
            {
                m_resetTimer.Cancel();
                m_resetTimer = null;
            }
        }

        private void OnClickRankingBtn(int rankingType)
        {
            m_curLeaderboardDefine = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(rankingType);
            ShowDetail(true);
            m_curQueryCount = Mathf.Min(m_queryAccum, m_curLeaderboardDefine.showLimit + 1);
            QueryRefreshRanking(m_curLeaderboardDefine);
        }

        private void ShowDetail(bool isShow)
        {
            if (m_curLeaderboardDefine != null &&
                (m_curLeaderboardDefine.ID == (int)AllianceRankingType.Tech || m_curLeaderboardDefine.ID == (int)AllianceRankingType.Building))
            {
                view.m_btn_help_GameButton.gameObject.SetActive(true);
            }
            else
            {
                view.m_btn_help_GameButton.gameObject.SetActive(false);
            }
            view.m_pl_Detail.gameObject.SetActive(isShow);
            view.m_pl_Total_GridLayoutGroup.gameObject.SetActive(!isShow);
            m_isOpenDetail = isShow;
        }

        private void OnClickClose()
        {
            if (m_isOpenDetail)
            {
                ShowDetail(false);
                return;
            }
            CoreUtils.uiManager.CloseUI(UI.s_AllianceRanking);
        }

        #endregion

        #region 排行榜详情

        private void LoadItemFinish(Dictionary<string,IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            RefreshDetailListView();
        }

        private void ListViewItemByIndex(ListView.ListItem item)
        {
            UI_Item_GuildRankingItem_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_GuildRankingItem_SubView;
            }
            else
            {
                itemView = new UI_Item_GuildRankingItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            
            itemView.SetInfo(m_rankingInfos[item.index]);
            
            if (!m_isAllRankingReceived && (item.index + 60 <= m_curLeaderboardDefine.showLimit) && item.index + 60 > m_curQueryCount) // 检测是否需要再次请求数据
            {
                m_curQueryCount += m_queryAccum;
                m_curQueryCount = Mathf.Min(m_curQueryCount, m_curLeaderboardDefine.showLimit + 1);
                
                QueryRefreshRanking(m_curLeaderboardDefine);
            }
        }
        
        private void QueryRefreshRankingRes(Rank_QueryRank.response res)
        {
            m_isQueringRefreshRanking = false;
            m_isAllRankingReceived = res.rankList.Count < m_curQueryCount;
            m_rankingInfos?.Clear();
            m_rankingInfos = res.rankList;
            RefreshTopPlayerInfo(res);
            RefreshDetailListView();
        }
        
        private void RefreshDetailListView()
        {
            if (m_assetDic.Count <= 0 || m_rankingInfos==null)
            {
                return;
            }
            
            view.m_lbl_noOne_LanguageText.gameObject.SetActive(m_rankingInfos.Count <= 0);
            view.m_sv_list_ListView.FillContent(m_rankingInfos.Count);
        }

        //刷新玩家信息
        private void RefreshTopPlayerInfo(Rank_QueryRank.response res)
        {
            int scoreTypeNameID = 0;
            if (m_curLeaderboardDefine.typeNameID != null && m_curLeaderboardDefine.typeNameID.Count > 0)
            {
                scoreTypeNameID = m_curLeaderboardDefine.typeNameID[0];
            }
            view.m_lbl_scoreType_LanguageText.text = LanguageUtils.getText(scoreTypeNameID);
            view.m_lbl_playerScoreType_LanguageText.text = LanguageUtils.getText(scoreTypeNameID);
            view.m_lbl_playerPow_LanguageText.text = ClientUtils.FormatComma(res.score);
            view.m_lbl_playerName_LanguageText.text = m_playerProxy.CurrentRoleInfo.name;
            view.m_UI_PlayerHead.LoadPlayerIcon(m_playerProxy.CurrentRoleInfo.headId,m_playerProxy.CurrentRoleInfo.headFrameID);

            if (res.selfRank <= 3 && res.selfRank > 0)
            {
                ClientUtils.LoadSprite(view.m_img_rank_PolygonImage, RS.RankingTop3IconName[res.selfRank - 1]);
                view.m_img_rank_PolygonImage.gameObject.SetActive(true);
                view.m_lbl_ranking_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                view.m_lbl_ranking_LanguageText.text = res.selfRank.ToString();
                view.m_img_rank_PolygonImage.gameObject.SetActive(false);
                view.m_lbl_ranking_LanguageText.gameObject.SetActive(true);
            }

            var officerInfo = m_allianceProxy.getMemberOfficer(m_playerProxy.Rid);
            if (officerInfo != null)
            {
                var officerCfg = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>((int)officerInfo.officerId);
                view.m_lbl_playerJob_LanguageText.text = LanguageUtils.getText(officerCfg.l_officiallyID);
            }
            else
            {
                view.m_lbl_playerJob_LanguageText.text = "-";
            }
        }

        private void OnClickTip()
        {
            int tipID = 0;
            switch (m_curLeaderboardDefine.ID)
            {
                case (int)AllianceRankingType.Tech:
                    tipID = 4003;
                    break;
                case (int)AllianceRankingType.Building:
                    tipID = 4005;
                    break;
            }
            
            HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(tipID);
            if (define == null)
            {
                return;
            }
            HelpTip.CreateTip(string.Format(LanguageUtils.getText(define.l_typeID) , LanguageUtils.getText(define.l_data1)), view.m_img_help_PolygonImage.rectTransform)
                .SetAutoFilter(true)
                .SetOffset(view.m_img_help_PolygonImage.rectTransform.rect.width/2)
                .Show();
        }

        #endregion

        #region 消息

        //请求排行榜入口按钮数据
        private void QueryShowRankFirst()
        {
            Rank_ShowRankFirst.request req = new Rank_ShowRankFirst.request()
            {
                type = 2
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        private void QueryRefreshRanking(LeaderboardDefine define)
        {
            if (m_isQueringRefreshRanking)
            {
                return;
            }

            m_isQueringRefreshRanking = true;
            Rank_QueryRank.request req = new Rank_QueryRank.request();
            req.type = define.ID;
            req.num = m_curQueryCount;
            
            AppFacade.GetInstance().SendSproto(req);
        }

        #endregion

        #region 数据处理
        

        #endregion
    }
}