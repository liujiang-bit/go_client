// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventStrongerPlayer2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventStrongerPlayer2_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventStrongerPlayer2";

        public UI_Item_EventStrongerPlayer2_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_EventStrongerPlayer2_Animator;
		[HideInInspector] public RectMask2D m_UI_Item_EventStrongerPlayer2_RectMask2D;

		[HideInInspector] public CanvasGroup m_pl_mes_CanvasGroup;

		[HideInInspector] public RectTransform m_pl_top;
		[HideInInspector] public PolygonImage m_img_top_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_top_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_title_Shadow;
		[HideInInspector] public Outline m_lbl_title_Outline;

		[HideInInspector] public LanguageText m_lbl_lifeTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lifeTime_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_lifeTime_Shadow;
		[HideInInspector] public Outline m_lbl_lifeTime_Outline;

		[HideInInspector] public LanguageText m_lbl_lifeDay_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lifeDay_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_lifeDay_Shadow;
		[HideInInspector] public Outline m_lbl_lifeDay_Outline;

		[HideInInspector] public PolygonImage m_btn_rank_PolygonImage;
		[HideInInspector] public GameButton m_btn_rank_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_rank_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_item_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_ck_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_ck_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public LayoutElement m_img_bg_LayoutElement;

		[HideInInspector] public UI_Item_EventStrongerPlayerCK_SubView m_ck_day1;
		[HideInInspector] public UI_Item_EventStrongerPlayerCK_SubView m_ck_day2;
		[HideInInspector] public UI_Item_EventStrongerPlayerCK_SubView m_ck_day3;
		[HideInInspector] public UI_Item_EventStrongerPlayerCK_SubView m_ck_day4;
		[HideInInspector] public UI_Item_EventStrongerPlayerCK_SubView m_ck_day5;
		[HideInInspector] public RectTransform m_pl_rank;
		[HideInInspector] public LanguageText m_lbl_myRank_LanguageText;

		[HideInInspector] public LanguageText m_lbl_myScore_LanguageText;

		[HideInInspector] public PolygonImage m_btn_help_PolygonImage;
		[HideInInspector] public GameButton m_btn_help_GameButton;

		[HideInInspector] public PolygonImage m_btn_question_PolygonImage;
		[HideInInspector] public GameButton m_btn_question_GameButton;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public RectTransform m_pl_result;
		[HideInInspector] public RectTransform m_pl_playermes;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_source_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_source_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_stage;
		[HideInInspector] public LanguageText m_lbl_stageRank0_LanguageText;

		[HideInInspector] public LanguageText m_lbl_stageRank1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_stageRank2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_stageRank3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_stageRank4_LanguageText;

		[HideInInspector] public LanguageText m_lbl_stageRank5_LanguageText;

		[HideInInspector] public LanguageText m_lbl_stageRank6_LanguageText;

		[HideInInspector] public HrefText m_lbl_result_LinkImageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_result_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_pos_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_offset_Animator;

		[HideInInspector] public PolygonImage m_img_pos_bg_PolygonImage;
		[HideInInspector] public ContentSizeFitter m_img_pos_bg_ContentSizeFitter;
		[HideInInspector] public HorizontalLayoutGroup m_img_pos_bg_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;
		[HideInInspector] public LayoutElement m_img_arrowSideR_LayoutElement;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;
		[HideInInspector] public LayoutElement m_img_arrowSideButtom_LayoutElement;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;
		[HideInInspector] public LayoutElement m_img_arrowSideTop_LayoutElement;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;
		[HideInInspector] public LayoutElement m_img_arrowSideL_LayoutElement;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_info_CanvasGroup;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_info_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_info_PolygonImage;
		[HideInInspector] public ListView m_sv_info_ListView;

		[HideInInspector] public PolygonImage m_v_info_PolygonImage;
		[HideInInspector] public Mask m_v_info_Mask;

		[HideInInspector] public ContentSizeFitter m_c_info_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_c_info_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_info_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_info_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_info_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventStrongerPlayer2_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_EventStrongerPlayer2_RectMask2D = gameObject.GetComponent<RectMask2D>();

			m_pl_mes_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_mes");

			m_pl_top = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top");
			m_img_top_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/img_top");
			m_img_top_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/img_top");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_mes/pl_top/lbl_title");
			m_lbl_title_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_title");

			m_lbl_lifeTime_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");
			m_lbl_lifeTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");
			m_lbl_lifeTime_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");
			m_lbl_lifeTime_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");

			m_lbl_lifeDay_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");
			m_lbl_lifeDay_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");
			m_lbl_lifeDay_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");
			m_lbl_lifeDay_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");

			m_btn_rank_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/btn_rank");
			m_btn_rank_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_top/btn_rank");
			m_btn_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_rank");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item/UI_Model_Item3"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_top/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_info");

			m_pl_ck_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_ck");
			m_pl_ck_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_ck");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_ck/img_bg");
			m_img_bg_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_ck/img_bg");

			m_ck_day1 = new UI_Item_EventStrongerPlayerCK_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_ck/ck_day1"));
			m_ck_day2 = new UI_Item_EventStrongerPlayerCK_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_ck/ck_day2"));
			m_ck_day3 = new UI_Item_EventStrongerPlayerCK_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_ck/ck_day3"));
			m_ck_day4 = new UI_Item_EventStrongerPlayerCK_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_ck/ck_day4"));
			m_ck_day5 = new UI_Item_EventStrongerPlayerCK_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_ck/ck_day5"));
			m_pl_rank = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rank");
			m_lbl_myRank_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_rank/lbl_myRank");

			m_lbl_myScore_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_rank/lbl_myScore");

			m_btn_help_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_rank/btn_help");
			m_btn_help_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_rank/btn_help");

			m_btn_question_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_rank/btn_help/btn_question");
			m_btn_question_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_rank/btn_help/btn_question");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mes/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_mes/sv_list");

			m_pl_result = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_result");
			m_pl_playermes = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_result/pl_playermes");
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_result/pl_playermes/UI_PlayerHead"));
			m_lbl_source_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_playermes/lbl_source");
			m_lbl_source_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_result/pl_playermes/lbl_source");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_playermes/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_result/pl_playermes/lbl_name");

			m_pl_stage = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_result/pl_stage");
			m_lbl_stageRank0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank0");

			m_lbl_stageRank1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank1");

			m_lbl_stageRank2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank2");

			m_lbl_stageRank3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank3");

			m_lbl_stageRank4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank4");

			m_lbl_stageRank5_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank5");

			m_lbl_stageRank6_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_result/pl_stage/line2/lbl_stageRank6");

			m_lbl_result_LinkImageText = FindUI<HrefText>(gameObject.transform ,"pl_mes/pl_result/lbl_result");
			m_lbl_result_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_result/lbl_result");

			m_pl_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_pos");

			m_pl_offset_Animator = FindUI<Animator>(gameObject.transform ,"pl_mes/pl_pos/pl_offset");

			m_img_pos_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg");
			m_img_pos_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg");
			m_img_pos_bg_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideR");
			m_img_arrowSideR_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideButtom");
			m_img_arrowSideButtom_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideTop");
			m_img_arrowSideTop_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideL");
			m_img_arrowSideL_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/img_arrowSideL");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_pos/pl_offset/img_pos_bg/lbl_time");

			m_pl_info_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_info");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_info/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_info/btn_back");

			m_sv_info_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_info/sv_info");
			m_sv_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/sv_info");
			m_sv_info_ListView = FindUI<ListView>(gameObject.transform ,"pl_info/sv_info");

			m_v_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/sv_info/v_info");
			m_v_info_Mask = FindUI<Mask>(gameObject.transform ,"pl_info/sv_info/v_info");

			m_c_info_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_info/sv_info/v_info/c_info");
			m_c_info_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_info/sv_info/v_info/c_info");

			m_lbl_info_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");


			BindEvent();
        }

        #endregion
    }
}