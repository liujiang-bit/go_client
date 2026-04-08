// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CaptainHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainHead";

        public UI_Item_CaptainHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainHead;
		[HideInInspector] public PolygonImage m_img_selected_PolygonImage;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_headDark_PolygonImage;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public PolygonImage m_img_level_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_stars_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_stars_ArabLayoutCompment;

		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar1;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar2;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar3;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar4;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar5;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar6;
		[HideInInspector] public GameButton m_btn_select_GameButton;
		[HideInInspector] public PolygonImage m_btn_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_state_PolygonImage;

		[HideInInspector] public VerticalLayoutGroup m_pl_summon_VerticalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_summon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_summon_LanguageText;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public Shadow m_lbl_count_Shadow;

		[HideInInspector] public PolygonImage m_img_state_defense_PolygonImage;

		[HideInInspector] public PolygonImage m_img_promote_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_CaptainHead = gameObject.GetComponent<RectTransform>();
			m_img_selected_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_selected");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_img_headDark_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_headDark");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_ClickAnimeMsg_btn"));
			m_img_level_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_level");

			m_lbl_lv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_level/lbl_lv");

			m_pl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_stars");
			m_pl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stars");

			m_UI_Model_HeadStar1 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar1"));
			m_UI_Model_HeadStar2 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar2"));
			m_UI_Model_HeadStar3 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar3"));
			m_UI_Model_HeadStar4 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar4"));
			m_UI_Model_HeadStar5 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar5"));
			m_UI_Model_HeadStar6 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar6"));
			m_btn_select_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_select");
			m_btn_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_select");

			m_img_state_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_state");

			m_pl_summon_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_summon");

			m_img_summon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_summon/img_summon");

			m_lbl_summon_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_summon/img_summon/lbl_summon");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_summon/lbl_count");
			m_lbl_count_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_summon/lbl_count");

			m_img_state_defense_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_state_defense");

			m_img_promote_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_promote");


			BindEvent();
        }

        #endregion
    }
}