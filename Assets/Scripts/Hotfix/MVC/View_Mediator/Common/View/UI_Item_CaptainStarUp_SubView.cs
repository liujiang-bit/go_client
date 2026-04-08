// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainStarUp_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainStarUp_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainStarUp";

        public UI_Item_CaptainStarUp_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_CaptainStarUp_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_CaptainStarUp_CanvasGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;
		[HideInInspector] public UIDefaultValue m_pl_left_UIDefaultValue;
		[HideInInspector] public CanvasGroup m_pl_left_CanvasGroup;
		[HideInInspector] public Animator m_pl_left_Animator;

		[HideInInspector] public ScrollRect m_sv_listAddStar_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_listAddStar_PolygonImage;
		[HideInInspector] public ListView m_sv_listAddStar_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_listAddStar_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_culture;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_auto;
		[HideInInspector] public GridLayoutGroup m_lbl_stars_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_lbl_stars_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar0;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar1;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar2;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar3;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar4;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar5;
		[HideInInspector] public PolygonImage m_img_exp_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_exp_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideTop_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_exp_LanguageText;

		[HideInInspector] public LanguageText m_lbl_lucky_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lucky_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_lucky_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_luckyNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_luckyNum_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_starExp_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_starExp_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_starExp_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_starExpNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_starExpNum_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_stardesc_PolygonImage;
		[HideInInspector] public GameButton m_btn_stardesc_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_stardesc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_preview_PolygonImage;
		[HideInInspector] public GameButton m_btn_preview_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_preview_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_table_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_table_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_item_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainStarUpItem_SubView m_img_item1;
		[HideInInspector] public UI_Item_CaptainStarUpItem_SubView m_img_item2;
		[HideInInspector] public UI_Item_CaptainStarUpItem_SubView m_img_item3;
		[HideInInspector] public UI_Item_CaptainStarUpItem_SubView m_img_item4;
		[HideInInspector] public UI_Item_CaptainStarUpItem_SubView m_img_item5;
		[HideInInspector] public UI_Item_CaptainStarUpItem_SubView m_img_item6;
		[HideInInspector] public UI_Common_Crit_SubView m_UI_Common_Crit;
		[HideInInspector] public ArabLayoutCompment m_pl_stardesc_ArabLayoutCompment;
		[HideInInspector] public UIDefaultValue m_pl_stardesc_UIDefaultValue;
		[HideInInspector] public Animator m_pl_stardesc_Animator;
		[HideInInspector] public CanvasGroup m_pl_stardesc_CanvasGroup;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public Image m_pl_starpreview_Image;
		[HideInInspector] public Button m_pl_starpreview_Button;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public UI_Item_AddStarPreview_SubView m_UI_Item_AddStarPreview0;
		[HideInInspector] public UI_Item_AddStarPreview_SubView m_UI_Item_AddStarPreview1;
		[HideInInspector] public UI_Item_AddStarPreview_SubView m_UI_Item_AddStarPreview2;
		[HideInInspector] public UI_Item_AddStarPreview_SubView m_UI_Item_AddStarPreview3;
		[HideInInspector] public UI_Item_AddStarPreview_SubView m_UI_Item_AddStarPreview4;


        private void UIFinder()
        {       
			m_UI_Item_CaptainStarUp_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_CaptainStarUp_CanvasGroup = gameObject.GetComponent<CanvasGroup>();

			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_left");
			m_pl_left_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_left");
			m_pl_left_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_left");
			m_pl_left_Animator = FindUI<Animator>(gameObject.transform ,"pl_left");

			m_sv_listAddStar_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_left/sv_listAddStar");
			m_sv_listAddStar_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_left/sv_listAddStar");
			m_sv_listAddStar_ListView = FindUI<ListView>(gameObject.transform ,"pl_left/sv_listAddStar");
			m_sv_listAddStar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_left/sv_listAddStar");

			m_UI_culture = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/btn/UI_culture"));
			m_btn_auto = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/btn/btn_auto"));
			m_lbl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"right/top/lbl_stars");
			m_lbl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/lbl_stars");

			m_UI_CaptainStar0 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"right/top/lbl_stars/UI_CaptainStar0"));
			m_UI_CaptainStar1 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"right/top/lbl_stars/UI_CaptainStar1"));
			m_UI_CaptainStar2 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"right/top/lbl_stars/UI_CaptainStar2"));
			m_UI_CaptainStar3 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"right/top/lbl_stars/UI_CaptainStar3"));
			m_UI_CaptainStar4 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"right/top/lbl_stars/UI_CaptainStar4"));
			m_UI_CaptainStar5 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"right/top/lbl_stars/UI_CaptainStar5"));
			m_img_exp_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"right/top/img_exp");
			m_img_exp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/img_exp");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"right/top/img_exp/img_arrowSideTop");
			m_img_arrowSideTop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/img_exp/img_arrowSideTop");

			m_lbl_exp_LanguageText = FindUI<LanguageText>(gameObject.transform ,"right/top/img_exp/lbl_exp");

			m_lbl_lucky_LanguageText = FindUI<LanguageText>(gameObject.transform ,"right/top/lbl_lucky");
			m_lbl_lucky_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/lbl_lucky");
			m_lbl_lucky_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"right/top/lbl_lucky");

			m_lbl_luckyNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"right/top/lbl_lucky/lbl_luckyNum");
			m_lbl_luckyNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/lbl_lucky/lbl_luckyNum");

			m_lbl_starExp_LanguageText = FindUI<LanguageText>(gameObject.transform ,"right/top/lbl_starExp");
			m_lbl_starExp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/lbl_starExp");
			m_lbl_starExp_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"right/top/lbl_starExp");

			m_lbl_starExpNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"right/top/lbl_starExp/lbl_starExpNum");
			m_lbl_starExpNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/top/lbl_starExp/lbl_starExpNum");

			m_btn_stardesc_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"right/btn_stardesc");
			m_btn_stardesc_GameButton = FindUI<GameButton>(gameObject.transform ,"right/btn_stardesc");
			m_btn_stardesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/btn_stardesc");

			m_btn_preview_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"right/btn_preview");
			m_btn_preview_GameButton = FindUI<GameButton>(gameObject.transform ,"right/btn_preview");
			m_btn_preview_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/btn_preview");

			m_pl_table_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"right/pl_table");
			m_pl_table_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/pl_table");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"right/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"right/pl_item");

			m_img_item1 = new UI_Item_CaptainStarUpItem_SubView(FindUI<RectTransform>(gameObject.transform ,"right/pl_item/img_item1"));
			m_img_item2 = new UI_Item_CaptainStarUpItem_SubView(FindUI<RectTransform>(gameObject.transform ,"right/pl_item/img_item2"));
			m_img_item3 = new UI_Item_CaptainStarUpItem_SubView(FindUI<RectTransform>(gameObject.transform ,"right/pl_item/img_item3"));
			m_img_item4 = new UI_Item_CaptainStarUpItem_SubView(FindUI<RectTransform>(gameObject.transform ,"right/pl_item/img_item4"));
			m_img_item5 = new UI_Item_CaptainStarUpItem_SubView(FindUI<RectTransform>(gameObject.transform ,"right/pl_item/img_item5"));
			m_img_item6 = new UI_Item_CaptainStarUpItem_SubView(FindUI<RectTransform>(gameObject.transform ,"right/pl_item/img_item6"));
			m_UI_Common_Crit = new UI_Common_Crit_SubView(FindUI<RectTransform>(gameObject.transform ,"right/UI_Common_Crit"));
			m_pl_stardesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stardesc");
			m_pl_stardesc_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_stardesc");
			m_pl_stardesc_Animator = FindUI<Animator>(gameObject.transform ,"pl_stardesc");
			m_pl_stardesc_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_stardesc");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_stardesc/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_stardesc/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(gameObject.transform ,"pl_stardesc/sv_desc");
			m_sv_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stardesc/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_stardesc/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_stardesc/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stardesc/sv_desc/v/c/lbl_desc");

			m_pl_starpreview_Image = FindUI<Image>(gameObject.transform ,"pl_starpreview");
			m_pl_starpreview_Button = FindUI<Button>(gameObject.transform ,"pl_starpreview");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_starpreview/content/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_starpreview/content/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_starpreview/content/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_starpreview/content/img_bg/img_arrowSideButtom");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_starpreview/content/img_bg/img_arrowSideL");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_starpreview/content/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_starpreview/content/lbl_text");

			m_UI_Item_AddStarPreview0 = new UI_Item_AddStarPreview_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_starpreview/content/stars/UI_Item_AddStarPreview0"));
			m_UI_Item_AddStarPreview1 = new UI_Item_AddStarPreview_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_starpreview/content/stars/UI_Item_AddStarPreview1"));
			m_UI_Item_AddStarPreview2 = new UI_Item_AddStarPreview_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_starpreview/content/stars/UI_Item_AddStarPreview2"));
			m_UI_Item_AddStarPreview3 = new UI_Item_AddStarPreview_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_starpreview/content/stars/UI_Item_AddStarPreview3"));
			m_UI_Item_AddStarPreview4 = new UI_Item_AddStarPreview_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_starpreview/content/stars/UI_Item_AddStarPreview4"));

			BindEvent();
        }

        #endregion
    }
}