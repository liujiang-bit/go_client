// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_HorizontalButton_MiniBlue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_HorizontalButton_MiniBlue_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_HorizontalButton_MiniBlue";

        public UI_Model_HorizontalButton_MiniBlue_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_HorizontalButton_MiniBlue;
		[HideInInspector] public PolygonImage m_btn_languageButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_languageButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_languageButton_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_img_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_forbid_PolygonImage;
		[HideInInspector] public UIDefaultValue m_img_forbid_UIDefaultValue;

		[HideInInspector] public LanguageText m_lbl_Text_LanguageText;
		[HideInInspector] public Shadow m_lbl_Text_Shadow;
		[HideInInspector] public ContentSizeFitter m_lbl_Text_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_HorizontalButton_MiniBlue = gameObject.GetComponent<RectTransform>();
			m_btn_languageButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_languageButton");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_img");
			m_img_img_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_languageButton/img_img");

			m_img_forbid_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_img/img_forbid");
			m_img_forbid_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"btn_languageButton/img_img/img_forbid");

			m_lbl_Text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_languageButton/lbl_Text");
			m_lbl_Text_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_languageButton/lbl_Text");
			m_lbl_Text_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"btn_languageButton/lbl_Text");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/lbl_Text/img_icon");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_redpoint");


			BindEvent();
        }

        #endregion
    }
}