// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月23日
// Update Time         :    2020年7月23日
// Class Description   :    UI_Win_GuildRankingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildRankingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildRanking";

        public UI_Win_GuildRankingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public GridLayoutGroup m_pl_Total_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_Total_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lifeTime_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_lifeTime_LayoutElement;

		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Killer;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Resource;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Technology;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Builder;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Help;
		[HideInInspector] public RectTransform m_pl_Detail;
		[HideInInspector] public LanguageText m_lbl_scoreType_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_scoreType_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_ranking_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_ranking_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_rank_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerJob_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerJob_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerPow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerPow_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerScoreType_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerScoreType_ArabLayoutCompment;

		[HideInInspector] public GameButton m_btn_help_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_help_Empty4Raycast;
		[HideInInspector] public ArabLayoutCompment m_btn_help_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_help_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_noOne_LanguageText;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_Total_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_Total");
			m_pl_Total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Total");

			m_lbl_lifeTime_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Total/lbl_lifeTime");
			m_lbl_lifeTime_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_Total/lbl_lifeTime");

			m_UI_Killer = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Killer"));
			m_UI_Person = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Person"));
			m_UI_Resource = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Resource"));
			m_UI_Technology = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Technology"));
			m_UI_Builder = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Builder"));
			m_UI_Help = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_Total/UI_Help"));
			m_pl_Detail = FindUI<RectTransform>(vb.transform ,"pl_Detail");
			m_lbl_scoreType_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/lbl_scoreType");
			m_lbl_scoreType_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/lbl_scoreType");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_Detail/top/bletContent/UI_PlayerHead"));
			m_lbl_ranking_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/bletContent/lbl_ranking");
			m_lbl_ranking_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/lbl_ranking");

			m_img_rank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/bletContent/img_rank");
			m_img_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/img_rank");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerName");
			m_lbl_playerName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerName");

			m_lbl_playerJob_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerJob");
			m_lbl_playerJob_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerJob");

			m_lbl_playerPow_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerPow");
			m_lbl_playerPow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerPow");

			m_lbl_playerScoreType_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerScoreType");
			m_lbl_playerScoreType_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/lbl_playerScoreType");

			m_btn_help_GameButton = FindUI<GameButton>(vb.transform ,"pl_Detail/top/bletContent/btn_help");
			m_btn_help_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_Detail/top/bletContent/btn_help");
			m_btn_help_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Detail/top/bletContent/btn_help");

			m_img_help_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/top/bletContent/btn_help/img_help");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_Detail/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Detail/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_Detail/sv_list");

			m_lbl_noOne_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Detail/lbl_noOne");


            UI_Win_GuildRankingMediator mt = new UI_Win_GuildRankingMediator(vb.gameObject);
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
