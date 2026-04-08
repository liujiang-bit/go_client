// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月8日
// Update Time         :    2020年9月8日
// Class Description   :    UI_Win_RankingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_RankingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Ranking";

        public UI_Win_RankingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public GridLayoutGroup m_pl_Total_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_Total_ArabLayoutCompment;

		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Guild0;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Guild1;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Guild2;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person0;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person1;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person2;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Other0;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Other1;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Other2;
		[HideInInspector] public RectTransform m_pl_Detail;
		[HideInInspector] public PolygonImage m_img_title_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_itemTitle0_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemTitle0_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemTitle1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemTitle1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_Blet_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_Blet_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_bletContent;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public LanguageText m_lbl_ranking_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_ranking_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_rank_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_guildName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_guildName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerPow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerPow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowUp_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowUp_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowDown_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowDown_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_noOne_LanguageText;

		[HideInInspector] public UI_Common_Spin_SubView m_UI_Common_Spin;


        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_Total_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_Total");
			m_pl_Total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Total");

			m_UI_Guild0 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Guild0"));
			m_UI_Guild1 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Guild1"));
			m_UI_Guild2 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Guild2"));
			m_UI_Person0 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Person0"));
			m_UI_Person1 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Person1"));
			m_UI_Person2 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Person2"));
			m_UI_Other0 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Other0"));
			m_UI_Other1 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Other1"));
			m_UI_Other2 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Other2"));
			m_pl_Detail = FindUI<RectTransform>(vb.transform ,"pl_Detail");
			m_img_title_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/img_title_bg");

			m_lbl_itemTitle0_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/img_title_bg/lbl_itemTitle0");
			m_lbl_itemTitle0_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/img_title_bg/lbl_itemTitle0");

			m_lbl_itemTitle1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/img_title_bg/lbl_itemTitle1");
			m_lbl_itemTitle1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/img_title_bg/lbl_itemTitle1");

			m_img_Blet_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/img_Blet");
			m_img_Blet_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/img_Blet");

			m_pl_bletContent = FindUI<RectTransform>(vb.transform ,"pl_Detail/top/pl_bletContent");
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_Detail/top/pl_bletContent/UI_PlayerHead"));
			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"pl_Detail/top/pl_bletContent/UI_GuildFlag"));
			m_lbl_ranking_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_ranking");
			m_lbl_ranking_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_ranking");

			m_img_rank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/pl_bletContent/img_rank");
			m_img_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/img_rank");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_playerName");
			m_lbl_playerName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_playerName");

			m_lbl_guildName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_guildName");
			m_lbl_guildName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_guildName");

			m_lbl_playerPow_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_playerPow");
			m_lbl_playerPow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/lbl_playerPow");

			m_img_arrowUp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/pl_bletContent/img_arrowUp");
			m_img_arrowUp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/img_arrowUp");

			m_img_arrowDown_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/pl_bletContent/img_arrowDown");
			m_img_arrowDown_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/pl_bletContent/img_arrowDown");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_Detail/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_Detail/sv_list");

			m_lbl_noOne_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/lbl_noOne");

			m_UI_Common_Spin = new UI_Common_Spin_SubView(FindUI<RectTransform>(vb.transform ,"UI_Common_Spin"));

            UI_Win_RankingMediator mt = new UI_Win_RankingMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
            vb.onMenuBackCallback = mt.onMenuBackCallback;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}
