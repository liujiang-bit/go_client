// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainData_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainData_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainData";

        public UI_Item_CaptainData_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public CanvasGroup m_UI_Item_CaptainData_CanvasGroup;
		[HideInInspector] public Animator m_UI_Item_CaptainData_Animator;
		[HideInInspector] public UIDefaultValue m_UI_Item_CaptainData_UIDefaultValue;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_CaptainData_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_titleimg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_titleimg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_title_PolygonImage;
		[HideInInspector] public GameButton m_btn_title_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_title_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_talent_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_talent_GridLayoutGroup;

		[HideInInspector] public UI_Model_CaptainTalent_SubView m_UI_Model_CaptainTalent1;
		[HideInInspector] public UI_Model_CaptainTalent_SubView m_UI_Model_CaptainTalent2;
		[HideInInspector] public UI_Model_CaptainTalent_SubView m_UI_Model_CaptainTalent3;
		[HideInInspector] public ArabLayoutCompment m_pl_stars_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_stars_GridLayoutGroup;

		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar1;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar2;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar3;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar4;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar5;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar6;
		[HideInInspector] public ArabLayoutCompment m_pl_starUp_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_starUp_effect_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_starUp_PolygonImage;
		[HideInInspector] public GameButton m_btn_starUp_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_starUp_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_forbid_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_exp_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainBar_SubView m_UI_Model_CaptainExpBar;
		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_armyCount_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_arnyCountInfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_arnyCountInfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_arnyCountInfo_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;

		[HideInInspector] public HorizontalLayoutGroup m_pl_featureBtn_HorizontalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_featureBtn_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_StandardButton_gift;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_StandardButton_skillup;
		[HideInInspector] public UI_Item_SkillInSummon_SubView m_pl_skill;
		[HideInInspector] public ArabLayoutCompment m_pl_summon_ArabLayoutCompment;

		[HideInInspector] public UI_Item_SkillInSummon_SubView m_UI_Item_SkillInSummon;
		[HideInInspector] public UI_Model_CaptainBar_SubView m_UI_Model_CaptainHeadBar;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_StandardButton_Blue;
		[HideInInspector] public PolygonImage m_img_sumIconframe_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_sumIconframe_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_sumIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_sumIcon_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_CapIFAnime_Left_SubView m_UI_Tag_CapIFAnime_Left;
		[HideInInspector] public UI_Item_EquipUse_SubView m_UI_Item_EquipUse;


        private void UIFinder()
        {       
			m_UI_Item_CaptainData_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
			m_UI_Item_CaptainData_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_CaptainData_UIDefaultValue = gameObject.GetComponent<UIDefaultValue>();
			m_UI_Item_CaptainData_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_info");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_pl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_title/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title/lbl_title");

			m_img_titleimg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_title/img_titleimg");
			m_img_titleimg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title/img_titleimg");

			m_btn_title_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_title/btn_title");
			m_btn_title_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_title/btn_title");
			m_btn_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title/btn_title");

			m_pl_talent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talent");
			m_pl_talent_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_talent");

			m_UI_Model_CaptainTalent1 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talent/UI_Model_CaptainTalent1"));
			m_UI_Model_CaptainTalent2 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talent/UI_Model_CaptainTalent2"));
			m_UI_Model_CaptainTalent3 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_talent/UI_Model_CaptainTalent3"));
			m_pl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stars");
			m_pl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_stars");

			m_UI_Model_CaptainStar1 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar1"));
			m_UI_Model_CaptainStar2 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar2"));
			m_UI_Model_CaptainStar3 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar3"));
			m_UI_Model_CaptainStar4 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar4"));
			m_UI_Model_CaptainStar5 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar5"));
			m_UI_Model_CaptainStar6 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar6"));
			m_pl_starUp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_starUp");

			m_pl_starUp_effect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_starUp/pl_starUp_effect");

			m_btn_starUp_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_starUp/btn_starUp");
			m_btn_starUp_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_starUp/btn_starUp");
			m_btn_starUp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_starUp/btn_starUp");

			m_img_forbid_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_starUp/btn_starUp/img_forbid");

			m_pl_exp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_exp");

			m_UI_Model_CaptainExpBar = new UI_Model_CaptainBar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_exp/UI_Model_CaptainExpBar"));
			m_lbl_level_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_exp/lbl_level");
			m_lbl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_exp/lbl_level");

			m_pl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_armyCount");

			m_btn_arnyCountInfo_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_armyCount/btn_arnyCountInfo");
			m_btn_arnyCountInfo_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_armyCount/btn_arnyCountInfo");
			m_btn_arnyCountInfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_armyCount/btn_arnyCountInfo");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_armyCount/lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_armyCount/lbl_armyCount");

			m_pl_featureBtn_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_featureBtn");
			m_pl_featureBtn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_featureBtn");

			m_UI_Model_StandardButton_gift = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_featureBtn/UI_Model_StandardButton_gift"));
			m_UI_Model_StandardButton_skillup = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_featureBtn/UI_Model_StandardButton_skillup"));
			m_pl_skill = new UI_Item_SkillInSummon_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill"));
			m_pl_summon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_summon");

			m_UI_Item_SkillInSummon = new UI_Item_SkillInSummon_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_summon/UI_Item_SkillInSummon"));
			m_UI_Model_CaptainHeadBar = new UI_Model_CaptainBar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_summon/UI_Model_CaptainHeadBar"));
			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_summon/UI_Model_StandardButton_Blue"));
			m_img_sumIconframe_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_summon/img_sumIconframe");
			m_img_sumIconframe_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_summon/img_sumIconframe");

			m_img_sumIcon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_summon/img_sumIcon");
			m_img_sumIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_summon/img_sumIcon");

			m_UI_Tag_CapIFAnime_Left = new UI_Tag_CapIFAnime_Left_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_CapIFAnime_Left"));
			m_UI_Item_EquipUse = new UI_Item_EquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_EquipUse"));

			BindEvent();
        }

        #endregion
    }
}