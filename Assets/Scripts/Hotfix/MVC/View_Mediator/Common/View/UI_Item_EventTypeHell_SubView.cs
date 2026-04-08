// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeHell_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeHell_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeHell";

        public UI_Item_EventTypeHell_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_EventTypeHell_Animator;
		[HideInInspector] public RectMask2D m_UI_Item_EventTypeHell_RectMask2D;

		[HideInInspector] public CanvasGroup m_pl_mes_CanvasGroup;

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

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_time_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public LanguageText m_lbl_score_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_score_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_score_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_score_val_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_score_val_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_difference_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_difference_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_difference_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_difference_val_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_difference_val_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_mes1;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EventBtnSource_SubView m_btn_source1;
		[HideInInspector] public UI_Item_EventBtnSource_SubView m_btn_source2;
		[HideInInspector] public UI_Item_EventBtnSource_SubView m_btn_source3;
		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_right_GridLayoutGroup;

		[HideInInspector] public GameSlider m_pb_rogressBar0_GameSlider;
		[HideInInspector] public LayoutElement m_pb_rogressBar0_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar0_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_handle_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_handle_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public LayoutElement m_pb_rogressBar_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EventHell_SubView m_UI_Item_EventHell1;
		[HideInInspector] public UI_Item_EventHell_SubView m_UI_Item_EventHell2;
		[HideInInspector] public UI_Item_EventHell_SubView m_UI_Item_EventHell3;
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

		[HideInInspector] public UI_Tip_BoxReward_SubView m_UI_Tip_BoxReward;


        private void UIFinder()
        {       
			m_UI_Item_EventTypeHell_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_EventTypeHell_RectMask2D = gameObject.GetComponent<RectMask2D>();

			m_pl_mes_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_mes");

			m_btn_rank_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_rank");
			m_btn_rank_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_rank");
			m_btn_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_rank");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/btn_rank/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_rank/pl_item");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_rank/pl_item/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_rank/pl_item/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_rank/pl_item/UI_Model_Item3"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_info");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_title");

			m_img_time_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_time");
			m_img_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/img_time");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/img_time/lbl_time");

			m_lbl_score_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_score");
			m_lbl_score_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_score");
			m_lbl_score_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_score");

			m_lbl_score_val_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_score/lbl_score_val");
			m_lbl_score_val_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_score/lbl_score_val");

			m_lbl_difference_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_difference");
			m_lbl_difference_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_difference");
			m_lbl_difference_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_difference");

			m_lbl_difference_val_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_difference/lbl_difference_val");
			m_lbl_difference_val_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_difference/lbl_difference_val");

			m_pl_mes1 = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1");
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_mes1/pl_left");

			m_btn_source1 = new UI_Item_EventBtnSource_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1/pl_left/btn_source1"));
			m_btn_source2 = new UI_Item_EventBtnSource_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1/pl_left/btn_source2"));
			m_btn_source3 = new UI_Item_EventBtnSource_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1/pl_left/btn_source3"));
			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_mes1/pl_right");
			m_pl_right_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_mes1/pl_right");

			m_pb_rogressBar0_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar0");
			m_pb_rogressBar0_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar0");
			m_pb_rogressBar0_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar0");

			m_img_handle_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar0/Handle Slide Area/Handle/img_handle");
			m_img_handle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar0/Handle Slide Area/Handle/img_handle");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar");
			m_pb_rogressBar_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/pb_rogressBar");

			m_UI_Item_EventHell1 = new UI_Item_EventHell_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/UI_Item_EventHell1"));
			m_UI_Item_EventHell2 = new UI_Item_EventHell_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/UI_Item_EventHell2"));
			m_UI_Item_EventHell3 = new UI_Item_EventHell_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_mes1/pl_right/UI_Item_EventHell3"));
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

			m_UI_Tip_BoxReward = new UI_Tip_BoxReward_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tip_BoxReward"));

			BindEvent();
        }

        #endregion
    }
}