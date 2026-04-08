// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainEquip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainEquip_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainEquip";

        public UI_Item_CaptainEquip_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_CaptainEquip_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_char_PolygonImage;
		[HideInInspector] public GameButton m_btn_char_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_char_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_CapIFAnime_Char_SubView m_UI_Tag_CapIFAnime_Char;
		[HideInInspector] public Image m_pl_talkPos_Image;
		[HideInInspector] public ArabLayoutCompment m_pl_talkPos_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_top_Animator;
		[HideInInspector] public CanvasGroup m_pl_top_CanvasGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_title_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_heroname_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_heroname_ArabLayoutCompment;

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
		[HideInInspector] public CanvasGroup m_pl_stars_CanvasGroup;
		[HideInInspector] public Animator m_pl_stars_Animator;

		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar1;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar2;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar3;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar4;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar5;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar6;
		[HideInInspector] public UI_Item_EquipUse_SubView m_UI_Item_EquipUse;
		[HideInInspector] public RectTransform m_pl_mask1;
		[HideInInspector] public PolygonImage m_btn_closeButton1_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton1_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_closeButton1_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_equip_mes1_CanvasGroup;
		[HideInInspector] public Animator m_pl_equip_mes1_Animator;
		[HideInInspector] public ArabLayoutCompment m_pl_equip_mes1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideL_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Equip_SubView m_UI_Model_Equip1;
		[HideInInspector] public LanguageText m_lbl_lv1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lv1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name1_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list1_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list1_PolygonImage;
		[HideInInspector] public ListView m_sv_list1_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list1_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EquipAtt_SubView m_UI_Item_EquipAtt1;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_drop;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_change;
		[HideInInspector] public RectTransform m_pl_mask;
		[HideInInspector] public PolygonImage m_btn_closeButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_closeButton_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_equip_list_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_equip_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_equip_list_PolygonImage;
		[HideInInspector] public ListView m_sv_equip_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_equip_list_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_equip_list_PolygonImage;
		[HideInInspector] public Mask m_v_equip_list_Mask;
		[HideInInspector] public ArabLayoutCompment m_v_equip_list_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_c_equip_list_ArabLayoutCompment;

		[HideInInspector] public ViewBinder m_pl_mask2_ViewBinder;
		[HideInInspector] public ArabLayoutCompment m_pl_mask2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_closeButton2_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton2_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_closeButton2_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_equip_mes2_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_equip_mes2_Animator;
		[HideInInspector] public CanvasGroup m_pl_equip_mes2_CanvasGroup;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Equip_SubView m_UI_Model_Equip2;
		[HideInInspector] public LanguageText m_lbl_lv2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lv2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name2_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list2_PolygonImage;
		[HideInInspector] public ListView m_sv_list2_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list2_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EquipAtt_SubView m_UI_Item_EquipAtt2;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_use;


        private void UIFinder()
        {       
			m_UI_Item_CaptainEquip_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_char_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_char");
			m_btn_char_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_char");
			m_btn_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_char");

			m_UI_Tag_CapIFAnime_Char = new UI_Tag_CapIFAnime_Char_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_char/UI_Tag_CapIFAnime_Char"));
			m_pl_talkPos_Image = FindUI<Image>(gameObject.transform ,"btn_char/pl_talkPos");
			m_pl_talkPos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_char/pl_talkPos");

			m_pl_top_Animator = FindUI<Animator>(gameObject.transform ,"pl_top");
			m_pl_top_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_top");

			m_pl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_top/pl_title");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_top/pl_title/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_top/pl_title/lbl_title");
			m_lbl_title_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_top/pl_title/lbl_title");

			m_lbl_heroname_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_top/pl_title/lbl_title/lbl_heroname");
			m_lbl_heroname_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_top/pl_title/lbl_title/lbl_heroname");

			m_img_titleimg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_top/pl_title/img_titleimg");
			m_img_titleimg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_top/pl_title/img_titleimg");

			m_btn_title_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_top/pl_title/btn_title");
			m_btn_title_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_top/pl_title/btn_title");
			m_btn_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_top/pl_title/btn_title");

			m_pl_talent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_top/pl_talent");
			m_pl_talent_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_top/pl_talent");

			m_UI_Model_CaptainTalent1 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_top/pl_talent/UI_Model_CaptainTalent1"));
			m_UI_Model_CaptainTalent2 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_top/pl_talent/UI_Model_CaptainTalent2"));
			m_UI_Model_CaptainTalent3 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_top/pl_talent/UI_Model_CaptainTalent3"));
			m_pl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stars");
			m_pl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_stars");
			m_pl_stars_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_stars");
			m_pl_stars_Animator = FindUI<Animator>(gameObject.transform ,"pl_stars");

			m_UI_Model_CaptainStar1 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar1"));
			m_UI_Model_CaptainStar2 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar2"));
			m_UI_Model_CaptainStar3 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar3"));
			m_UI_Model_CaptainStar4 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar4"));
			m_UI_Model_CaptainStar5 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar5"));
			m_UI_Model_CaptainStar6 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_CaptainStar6"));
			m_UI_Item_EquipUse = new UI_Item_EquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_EquipUse"));
			m_pl_mask1 = FindUI<RectTransform>(gameObject.transform ,"pl_mask1");
			m_btn_closeButton1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask1/btn_closeButton1");
			m_btn_closeButton1_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mask1/btn_closeButton1");
			m_btn_closeButton1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask1/btn_closeButton1");

			m_pl_equip_mes1_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_mask1/pl_equip_mes1");
			m_pl_equip_mes1_Animator = FindUI<Animator>(gameObject.transform ,"pl_mask1/pl_equip_mes1");
			m_pl_equip_mes1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask1/pl_equip_mes1");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask1/pl_equip_mes1/img_arrowSideL");
			m_img_arrowSideL_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask1/pl_equip_mes1/img_arrowSideL");

			m_UI_Model_Equip1 = new UI_Model_Equip_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask1/pl_equip_mes1/UI_Model_Equip1"));
			m_lbl_lv1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mask1/pl_equip_mes1/lbl_lv1");
			m_lbl_lv1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask1/pl_equip_mes1/lbl_lv1");

			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mask1/pl_equip_mes1/lbl_name1");
			m_lbl_name1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask1/pl_equip_mes1/lbl_name1");

			m_sv_list1_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mask1/pl_equip_mes1/sv_list1");
			m_sv_list1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask1/pl_equip_mes1/sv_list1");
			m_sv_list1_ListView = FindUI<ListView>(gameObject.transform ,"pl_mask1/pl_equip_mes1/sv_list1");
			m_sv_list1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask1/pl_equip_mes1/sv_list1");

			m_UI_Item_EquipAtt1 = new UI_Item_EquipAtt_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask1/pl_equip_mes1/sv_list1/v/c/UI_Item_EquipAtt1"));
			m_btn_drop = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask1/pl_equip_mes1/btn_drop"));
			m_btn_change = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask1/pl_equip_mes1/btn_change"));
			m_pl_mask = FindUI<RectTransform>(gameObject.transform ,"pl_mask");
			m_btn_closeButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask/btn_closeButton");
			m_btn_closeButton_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mask/btn_closeButton");
			m_btn_closeButton_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask/btn_closeButton");

			m_pl_equip_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask/pl_equip_list");

			m_sv_equip_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list");
			m_sv_equip_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list");
			m_sv_equip_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list");
			m_sv_equip_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list");

			m_v_equip_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list/v_equip_list");
			m_v_equip_list_Mask = FindUI<Mask>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list/v_equip_list");
			m_v_equip_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list/v_equip_list");

			m_c_equip_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask/pl_equip_list/sv_equip_list/v_equip_list/c_equip_list");

			m_pl_mask2_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"pl_mask2");
			m_pl_mask2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2");

			m_btn_closeButton2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask2/btn_closeButton2");
			m_btn_closeButton2_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mask2/btn_closeButton2");
			m_btn_closeButton2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2/btn_closeButton2");

			m_pl_equip_mes2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2/pl_equip_mes2");
			m_pl_equip_mes2_Animator = FindUI<Animator>(gameObject.transform ,"pl_mask2/pl_equip_mes2");
			m_pl_equip_mes2_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_mask2/pl_equip_mes2");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask2/pl_equip_mes2/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2/pl_equip_mes2/img_bg");

			m_UI_Model_Equip2 = new UI_Model_Equip_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/UI_Model_Equip2"));
			m_lbl_lv2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/lbl_lv2");
			m_lbl_lv2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/lbl_lv2");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/lbl_name2");
			m_lbl_name2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/lbl_name2");

			m_sv_list2_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/sv_list2");
			m_sv_list2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/sv_list2");
			m_sv_list2_ListView = FindUI<ListView>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/sv_list2");
			m_sv_list2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/sv_list2");

			m_UI_Item_EquipAtt2 = new UI_Item_EquipAtt_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/sv_list2/v/c/UI_Item_EquipAtt2"));
			m_btn_use = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mask2/pl_equip_mes2/content/btn_use"));

			BindEvent();
        }

        #endregion
    }
}