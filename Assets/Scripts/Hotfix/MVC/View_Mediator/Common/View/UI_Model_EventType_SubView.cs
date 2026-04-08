// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_EventType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_EventType_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_EventType";

        public UI_Model_EventType_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectMask2D m_UI_Model_EventType_RectMask2D;
		[HideInInspector] public Animator m_UI_Model_EventType_Animator;

		[HideInInspector] public CanvasGroup m_pl_mes_CanvasGroup;

		[HideInInspector] public RectTransform m_pl_top;
		[HideInInspector] public PolygonImage m_img_top_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_top_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;
		[HideInInspector] public Outline m_lbl_title_Outline;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_name_Shadow;
		[HideInInspector] public Outline m_lbl_name_Outline;

		[HideInInspector] public LanguageText m_lbl_lifeDay_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lifeDay_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_lifeDay_Shadow;
		[HideInInspector] public Outline m_lbl_lifeDay_Outline;

		[HideInInspector] public LanguageText m_lbl_lifeTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lifeTime_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_lifeTime_Shadow;
		[HideInInspector] public Outline m_lbl_lifeTime_Outline;

		[HideInInspector] public PolygonImage m_btn_reset_PolygonImage;
		[HideInInspector] public GameButton m_btn_reset_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_reset_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_reset_LanguageText;

		[HideInInspector] public PolygonImage m_btn_rank_PolygonImage;
		[HideInInspector] public GameButton m_btn_rank_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_rank_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_item_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public GameToggle m_ck_showExchange_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_showExchange_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

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
			m_UI_Model_EventType_RectMask2D = gameObject.GetComponent<RectMask2D>();
			m_UI_Model_EventType_Animator = gameObject.GetComponent<Animator>();

			m_pl_mes_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_mes");

			m_pl_top = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top");
			m_img_top_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/img_top");
			m_img_top_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/img_top");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_title");
			m_lbl_title_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_title");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_mes/pl_top/lbl_name");
			m_lbl_name_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_name");

			m_lbl_lifeDay_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");
			m_lbl_lifeDay_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");
			m_lbl_lifeDay_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");
			m_lbl_lifeDay_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeDay");

			m_lbl_lifeTime_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");
			m_lbl_lifeTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");
			m_lbl_lifeTime_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");
			m_lbl_lifeTime_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/pl_top/lbl_lifeTime");

			m_btn_reset_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/btn_reset");
			m_btn_reset_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_top/btn_reset");
			m_btn_reset_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_reset");

			m_lbl_reset_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_top/btn_reset/lbl_reset");

			m_btn_rank_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/btn_rank");
			m_btn_rank_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_top/btn_rank");
			m_btn_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_rank");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_top/btn_rank/pl_item/UI_Model_Item3"));
			m_ck_showExchange_GameToggle = FindUI<GameToggle>(gameObject.transform ,"pl_mes/pl_top/ck_showExchange");
			m_ck_showExchange_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/ck_showExchange");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_top/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_top/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_top/btn_info");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mes/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_mes/sv_list");

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