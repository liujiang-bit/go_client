// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainSkillUp_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainSkillUp_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainSkillUp";

        public UI_Item_CaptainSkillUp_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_CaptainSkillUp_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_CaptainSkillUp_CanvasGroup;

		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill1;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill2;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill3;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill4;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill5;
		[HideInInspector] public PolygonImage m_img_cover_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_awaken_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_awaken_ArabLayoutCompment;

		[HideInInspector] public UI_Item_awaken_SubView m_UI_Item_awaken1;
		[HideInInspector] public UI_Item_awaken_SubView m_UI_Item_awaken2;
		[HideInInspector] public UI_Item_awaken_SubView m_UI_Item_awaken3;
		[HideInInspector] public UI_Item_awaken_SubView m_UI_Item_awaken4;
		[HideInInspector] public RectTransform m_pl_skill_lvup;
		[HideInInspector] public UI_Model_Item_SubView m_UI_herolvup_item;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;
		[HideInInspector] public BtnAnimation m_btn_add_BtnAnimation;
		[HideInInspector] public GrayChildrens m_btn_add_GrayChildrens;

		[HideInInspector] public LanguageText m_lbl_itemNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemNum_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_change_PolygonImage;
		[HideInInspector] public GameButton m_btn_change_GameButton;
		[HideInInspector] public BtnAnimation m_btn_change_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_currency_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_currency_iconNum_LanguageText;

		[HideInInspector] public LanguageText m_lbl_change_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_btn_lvup;
		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public BtnAnimation m_btn_back_BtnAnimation;

		[HideInInspector] public UI_Tag_ClickAnime_StandardButton_SubView m_UI_Tag_ClickAnime_StandardButton;
		[HideInInspector] public SortingGroup m_pl_effect_SortingGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_effect_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_effect_line1;
		[HideInInspector] public RectTransform m_pl_effect_line2;
		[HideInInspector] public RectTransform m_pl_effect_line3;
		[HideInInspector] public RectTransform m_pl_effect_line4;


        private void UIFinder()
        {       
			m_UI_Item_CaptainSkillUp_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_CaptainSkillUp_CanvasGroup = gameObject.GetComponent<CanvasGroup>();

			m_UI_Item_CaptainSkill1 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill1"));
			m_UI_Item_CaptainSkill2 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill2"));
			m_UI_Item_CaptainSkill3 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill3"));
			m_UI_Item_CaptainSkill4 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill4"));
			m_UI_Item_CaptainSkill5 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill5"));
			m_img_cover_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"UI_Item_CaptainSkill5/img_cover");

			m_pl_awaken_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"UI_Item_CaptainSkill5/pl_awaken");
			m_pl_awaken_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"UI_Item_CaptainSkill5/pl_awaken");

			m_UI_Item_awaken1 = new UI_Item_awaken_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill5/pl_awaken/UI_Item_awaken1"));
			m_UI_Item_awaken2 = new UI_Item_awaken_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill5/pl_awaken/UI_Item_awaken2"));
			m_UI_Item_awaken3 = new UI_Item_awaken_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill5/pl_awaken/UI_Item_awaken3"));
			m_UI_Item_awaken4 = new UI_Item_awaken_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainSkill5/pl_awaken/UI_Item_awaken4"));
			m_pl_skill_lvup = FindUI<RectTransform>(gameObject.transform ,"pl_skill_lvup");
			m_UI_herolvup_item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill_lvup/UI_herolvup_item"));
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_skill_lvup/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_skill_lvup/pb_rogressBar");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_skill_lvup/pb_rogressBar/lbl_name");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_skill_lvup/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_skill_lvup/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_skill_lvup/btn_add");
			m_btn_add_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"pl_skill_lvup/btn_add");
			m_btn_add_GrayChildrens = FindUI<GrayChildrens>(gameObject.transform ,"pl_skill_lvup/btn_add");

			m_lbl_itemNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_skill_lvup/lbl_itemNum");
			m_lbl_itemNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_skill_lvup/lbl_itemNum");

			m_btn_change_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_skill_lvup/btn_change");
			m_btn_change_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_skill_lvup/btn_change");
			m_btn_change_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"pl_skill_lvup/btn_change");

			m_img_currency_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_skill_lvup/btn_change/img_currency_icon");

			m_lbl_currency_iconNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_skill_lvup/btn_change/lbl_currency_iconNum");

			m_lbl_change_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_skill_lvup/btn_change/lbl_change");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_skill_lvup/lbl_desc");

			m_UI_btn_lvup = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill_lvup/UI_btn_lvup"));
			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_back");
			m_btn_back_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_back");

			m_UI_Tag_ClickAnime_StandardButton = new UI_Tag_ClickAnime_StandardButton_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_back/UI_Tag_ClickAnime_StandardButton"));
			m_pl_effect_SortingGroup = FindUI<SortingGroup>(gameObject.transform ,"pl_effect");
			m_pl_effect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_effect");

			m_pl_effect_line1 = FindUI<RectTransform>(gameObject.transform ,"pl_effect/pl_effect_line1");
			m_pl_effect_line2 = FindUI<RectTransform>(gameObject.transform ,"pl_effect/pl_effect_line2");
			m_pl_effect_line3 = FindUI<RectTransform>(gameObject.transform ,"pl_effect/pl_effect_line3");
			m_pl_effect_line4 = FindUI<RectTransform>(gameObject.transform ,"pl_effect/pl_effect_line4");

			BindEvent();
        }

        #endregion
    }
}