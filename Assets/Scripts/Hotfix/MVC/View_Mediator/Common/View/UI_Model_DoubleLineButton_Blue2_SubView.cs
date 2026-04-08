// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_DoubleLineButton_Blue2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_DoubleLineButton_Blue2_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_DoubleLineButton_Blue2";

        public UI_Model_DoubleLineButton_Blue2_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GrayChildrens m_UI_Model_DoubleLineButton_Blue2_GrayChildrens;

		[HideInInspector] public PolygonImage m_btn_languageButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_languageButton_GameButton;
		[HideInInspector] public VerticalLayoutGroup m_btn_languageButton_VerticalLayoutGroup;
		[HideInInspector] public BtnAnimation m_btn_languageButton_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public LayoutElement m_img_img_LayoutElement;

		[HideInInspector] public PolygonImage m_img_forbid_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_Text_LanguageText;
		[HideInInspector] public Shadow m_lbl_Text_Shadow;

		[HideInInspector] public HorizontalLayoutGroup m_pl_line2_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_icon2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_line2_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_line2_ContentSizeFitter;
		[HideInInspector] public Shadow m_lbl_line2_Shadow;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_DoubleLineButton_Blue2_GrayChildrens = gameObject.GetComponent<GrayChildrens>();

			m_btn_languageButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_languageButton");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_img");
			m_img_img_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"btn_languageButton/img_img");

			m_img_forbid_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_img/img_forbid");

			m_lbl_Text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_languageButton/lbl_Text");
			m_lbl_Text_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_languageButton/lbl_Text");

			m_pl_line2_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"btn_languageButton/pl_line2");

			m_img_icon2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/pl_line2/img_icon2");

			m_lbl_line2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_languageButton/pl_line2/lbl_line2");
			m_lbl_line2_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"btn_languageButton/pl_line2/lbl_line2");
			m_lbl_line2_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_languageButton/pl_line2/lbl_line2");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_redpoint");


			BindEvent();
        }

        #endregion
    }
}