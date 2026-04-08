// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeRank_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeRank_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeRank";

        public UI_Item_EventTypeRank_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTypeRank;
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public RectTransform m_pl_top;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public ToggleGroup m_pl_ck_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_total_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_total_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_total_LanguageText;

		[HideInInspector] public GameToggle m_ck_single_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_single_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_single_LanguageText;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;

		[HideInInspector] public PolygonImage m_btn_reward_PolygonImage;
		[HideInInspector] public GameButton m_btn_reward_GameButton;

		[HideInInspector] public RectTransform m_pl_playermes;
		[HideInInspector] public PolygonImage m_img_arrow_up_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_up_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_down_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_down_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_rank_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rank_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_Model_GuildFlag;
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

		[HideInInspector] public RectTransform m_pl_stage_sv;
		[HideInInspector] public ScrollRect m_sv_stage_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_stage_list_PolygonImage;
		[HideInInspector] public ListView m_sv_stage_list_ListView;

		[HideInInspector] public RectTransform m_pl_total_sv;
		[HideInInspector] public ScrollRect m_sv_total_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_total_list_PolygonImage;
		[HideInInspector] public ListView m_sv_total_list_ListView;

		[HideInInspector] public PolygonImage m_img_loading_PolygonImage;
		[HideInInspector] public Animation m_img_loading_Animation;
		[HideInInspector] public LayoutElement m_img_loading_LayoutElement;



        private void UIFinder()
        {       
			m_UI_Item_EventTypeRank = gameObject.GetComponent<RectTransform>();
			m_pl_view = FindUI<RectTransform>(gameObject.transform ,"pl_view");
			m_pl_top = FindUI<RectTransform>(gameObject.transform ,"rect/pl_top");
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_top/lbl_title");

			m_pl_ck_ToggleGroup = FindUI<ToggleGroup>(gameObject.transform ,"rect/pl_top/pl_ck");

			m_ck_total_GameToggle = FindUI<GameToggle>(gameObject.transform ,"rect/pl_top/pl_ck/ck_total");
			m_ck_total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_top/pl_ck/ck_total");

			m_lbl_total_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_top/pl_ck/ck_total/lbl_total");

			m_ck_single_GameToggle = FindUI<GameToggle>(gameObject.transform ,"rect/pl_top/pl_ck/ck_single");
			m_ck_single_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_top/pl_ck/ck_single");

			m_lbl_single_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_top/pl_ck/ck_single/lbl_single");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_top/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/pl_top/btn_back");

			m_btn_reward_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_top/btn_reward");
			m_btn_reward_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/pl_top/btn_reward");

			m_pl_playermes = FindUI<RectTransform>(gameObject.transform ,"rect/pl_playermes");
			m_img_arrow_up_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_playermes/img_arrow_up");
			m_img_arrow_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_playermes/img_arrow_up");

			m_img_arrow_down_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_playermes/img_arrow_down");
			m_img_arrow_down_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_playermes/img_arrow_down");

			m_lbl_rank_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_playermes/lbl_rank");
			m_lbl_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_playermes/lbl_rank");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_playermes/UI_PlayerHead"));
			m_UI_Model_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_playermes/UI_Model_GuildFlag"));
			m_lbl_source_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_playermes/lbl_source");
			m_lbl_source_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_playermes/lbl_source");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_playermes/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/pl_playermes/lbl_name");

			m_pl_stage = FindUI<RectTransform>(gameObject.transform ,"rect/pl_stage");
			m_lbl_stageRank0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank0");

			m_lbl_stageRank1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank1");

			m_lbl_stageRank2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank2");

			m_lbl_stageRank3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank3");

			m_lbl_stageRank4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank4");

			m_lbl_stageRank5_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank5");

			m_lbl_stageRank6_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_stage/line2/lbl_stageRank6");

			m_pl_stage_sv = FindUI<RectTransform>(gameObject.transform ,"rect/pl_stage_sv");
			m_sv_stage_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"rect/pl_stage_sv/sv_stage_list");
			m_sv_stage_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_stage_sv/sv_stage_list");
			m_sv_stage_list_ListView = FindUI<ListView>(gameObject.transform ,"rect/pl_stage_sv/sv_stage_list");

			m_pl_total_sv = FindUI<RectTransform>(gameObject.transform ,"rect/pl_total_sv");
			m_sv_total_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"rect/pl_total_sv/sv_total_list");
			m_sv_total_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_total_sv/sv_total_list");
			m_sv_total_list_ListView = FindUI<ListView>(gameObject.transform ,"rect/pl_total_sv/sv_total_list");

			m_img_loading_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_loading");
			m_img_loading_Animation = FindUI<Animation>(gameObject.transform ,"rect/img_loading");
			m_img_loading_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"rect/img_loading");


			BindEvent();
        }

        #endregion
    }
}