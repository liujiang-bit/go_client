// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_FeatureBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_FeatureBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_FeatureBtn";

        public UI_Model_FeatureBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_FeatureBtn;
		[HideInInspector] public UI_Tag_ClickAnime_StandardButton_SubView m_UI_Tag_ClickAnimeButton;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;
		[HideInInspector] public BtnAnimation m_btn_btn_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_hide_PolygonImage;

		[HideInInspector] public PolygonImage m_img_namebg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Shadow m_lbl_name_Shadow;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_FeatureBtn = gameObject.GetComponent<RectTransform>();
			m_UI_Tag_ClickAnimeButton = new UI_Tag_ClickAnime_StandardButton_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_ClickAnimeButton"));
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");
			m_btn_btn_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_btn");

			m_img_hide_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_hide");

			m_img_namebg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_namebg");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/img_namebg/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_btn/img_namebg/lbl_name");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_redpoint");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/img_redpoint/lbl_count");


			BindEvent();
        }

        #endregion
    }
}