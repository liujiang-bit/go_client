// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_Alert_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Common_Alert_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_Alert";

        public UI_Common_Alert_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Common_Alert_ViewBinder;
		[HideInInspector] public Animator m_UI_Common_Alert_Animator;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Model_StandardButton_Blue_sure;
		[HideInInspector] public UI_Model_DoubleLineButton_Red2_SubView m_UI_Model_StandardButton_Red;


        private void UIFinder()
        {       
			m_UI_Common_Alert_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Common_Alert_Animator = gameObject.GetComponent<Animator>();

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(gameObject.transform ,"img_bg/lbl_title");

			m_UI_Model_StandardButton_Blue_sure = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/UI_Model_StandardButton_Blue_sure"));
			m_UI_Model_StandardButton_Red = new UI_Model_DoubleLineButton_Red2_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/UI_Model_StandardButton_Red"));

			BindEvent();
        }

        #endregion
    }
}