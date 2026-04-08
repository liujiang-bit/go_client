// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTribeKing_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTribeKing_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTribeKing";

        public UI_Item_EventTribeKing_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTribeKing;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public PolygonImage m_img_activitybg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_coming_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_coming_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;
		[HideInInspector] public Outline m_lbl_time_Outline;

		[HideInInspector] public PolygonImage m_btn_rank_PolygonImage;
		[HideInInspector] public GameButton m_btn_rank_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_rank_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_item_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public GridLayoutGroup m_pl_score_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_score_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EventTribeKingScore_SubView m_UI_Item_EventTribeKingScore1;
		[HideInInspector] public UI_Item_EventTribeKingScore_SubView m_UI_Item_EventTribeKingScore2;
		[HideInInspector] public UI_Item_EventTribeKingScore_SubView m_UI_Item_EventTribeKingScore3;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_box_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_box_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_UI_Item_EventTribeKingBox1;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_UI_Item_EventTribeKingBox2;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_UI_Item_EventTribeKingBox3;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_UI_Item_EventTribeKingBox4;
		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

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
			m_UI_Item_EventTribeKing = gameObject.GetComponent<RectTransform>();
			m_pl_mes = FindUI<RectTransform>(gameObject.transform ,"pl_mes");
			m_img_activitybg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_activitybg");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_title");

			m_lbl_coming_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_coming");
			m_lbl_coming_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_coming");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_desc");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/lbl_time");

			m_btn_rank_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_rank");
			m_btn_rank_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_rank");
			m_btn_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_rank");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/btn_rank/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_rank/pl_item");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_rank/pl_item/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_rank/pl_item/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_rank/pl_item/UI_Model_Item3"));
			m_pl_score_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_score");
			m_pl_score_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_score");

			m_UI_Item_EventTribeKingScore1 = new UI_Item_EventTribeKingScore_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_score/UI_Item_EventTribeKingScore1"));
			m_UI_Item_EventTribeKingScore2 = new UI_Item_EventTribeKingScore_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_score/UI_Item_EventTribeKingScore2"));
			m_UI_Item_EventTribeKingScore3 = new UI_Item_EventTribeKingScore_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_score/UI_Item_EventTribeKingScore3"));
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_mes/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pb_rogressBar");

			m_pl_box_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_box");
			m_pl_box_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_box");

			m_UI_Item_EventTribeKingBox1 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box/UI_Item_EventTribeKingBox1"));
			m_UI_Item_EventTribeKingBox2 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box/UI_Item_EventTribeKingBox2"));
			m_UI_Item_EventTribeKingBox3 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box/UI_Item_EventTribeKingBox3"));
			m_UI_Item_EventTribeKingBox4 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box/UI_Item_EventTribeKingBox4"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_info");

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