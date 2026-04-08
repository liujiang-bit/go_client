// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainTalent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainTalent_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainTalent";

        public UI_Item_CaptainTalent_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_CaptainTalent_ArabLayoutCompment;
		[HideInInspector] public Animator m_UI_Item_CaptainTalent_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_CaptainTalent_CanvasGroup;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_img_talent_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_talentPoint_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_reset;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public UI_Item_TalentTop_SubView m_UI_Item_TalentTop2;
		[HideInInspector] public UI_Item_TalentTop_SubView m_UI_Item_TalentTop1;
		[HideInInspector] public UI_Item_TalentTop_SubView m_UI_Item_TalentTop3;
		[HideInInspector] public ScrollRect m_sv_talentList_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_talentList_PolygonImage;
		[HideInInspector] public ListView m_sv_talentList_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_talentList_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_talentPage_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_title_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_talentPages_GridLayoutGroup;

		[HideInInspector] public UI_Item_TalentPage_SubView m_UI_TalentPage0;
		[HideInInspector] public UI_Item_TalentPage_SubView m_UI_TalentPage1;
		[HideInInspector] public UI_Item_TalentPage_SubView m_UI_TalentPage2;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_change;
		[HideInInspector] public GameSlider m_pb_line_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_line_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_pointbg_GridLayoutGroup;

		[HideInInspector] public GridLayoutGroup m_pl_point_GridLayoutGroup;

		[HideInInspector] public PolygonImage m_img_point0_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point5_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point6_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point7_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point8_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point9_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point10_PolygonImage;

		[HideInInspector] public UI_Item_TalentSpecialPop_SubView m_UI_Item_TalentSpecialPop;
		[HideInInspector] public UI_Item_TalentSkillPop_SubView m_UI_Item_TalentSkillPop;


        private void UIFinder()
        {       
			m_UI_Item_CaptainTalent_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_Item_CaptainTalent_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_CaptainTalent_CanvasGroup = gameObject.GetComponent<CanvasGroup>();

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_polygonImage");

			m_img_talent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_talent");

			m_lbl_talentPoint_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_talent/lbl_talentPoint");

			m_btn_reset = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"img_talent/btn_reset"));
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_left");

			m_UI_Item_TalentTop2 = new UI_Item_TalentTop_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/talentTotals/UI_Item_TalentTop2"));
			m_UI_Item_TalentTop1 = new UI_Item_TalentTop_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/talentTotals/UI_Item_TalentTop1"));
			m_UI_Item_TalentTop3 = new UI_Item_TalentTop_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/talentTotals/UI_Item_TalentTop3"));
			m_sv_talentList_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_left/sv_talentList");
			m_sv_talentList_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_left/sv_talentList");
			m_sv_talentList_ListView = FindUI<ListView>(gameObject.transform ,"pl_left/sv_talentList");
			m_sv_talentList_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_left/sv_talentList");

			m_pl_talentPage_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talentPage");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_talentPage/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talentPage/img_bg");

			m_img_title_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_talentPage/img_title");
			m_img_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talentPage/img_title");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_talentPage/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_talentPage/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talentPage/btn_back");

			m_pl_talentPages_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_talentPage/pl_talentPages");

			m_UI_TalentPage0 = new UI_Item_TalentPage_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talentPage/pl_talentPages/UI_TalentPage0"));
			m_UI_TalentPage1 = new UI_Item_TalentPage_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talentPage/pl_talentPages/UI_TalentPage1"));
			m_UI_TalentPage2 = new UI_Item_TalentPage_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talentPage/pl_talentPages/UI_TalentPage2"));
			m_btn_change = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talentPage/btn_change"));
			m_pb_line_GameSlider = FindUI<GameSlider>(gameObject.transform ,"bar/pb_line");
			m_pb_line_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bar/pb_line");

			m_pl_pointbg_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"bar/pl_pointbg");

			m_pl_point_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"bar/pl_point");

			m_img_point0_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point0");

			m_img_point1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point1");

			m_img_point2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point2");

			m_img_point3_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point3");

			m_img_point4_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point4");

			m_img_point5_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point5");

			m_img_point6_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point6");

			m_img_point7_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point7");

			m_img_point8_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point8");

			m_img_point9_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point9");

			m_img_point10_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pl_point/img_point10");

			m_UI_Item_TalentSpecialPop = new UI_Item_TalentSpecialPop_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TalentSpecialPop"));
			m_UI_Item_TalentSkillPop = new UI_Item_TalentSkillPop_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TalentSkillPop"));

			BindEvent();
        }

        #endregion
    }
}