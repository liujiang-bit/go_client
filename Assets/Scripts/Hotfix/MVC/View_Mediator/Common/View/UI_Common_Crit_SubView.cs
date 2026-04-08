// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_Crit_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Common_Crit_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_Crit";

        public UI_Common_Crit_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Common_Crit;
		[HideInInspector] public Animator m_pl_cri_Animator;
		[HideInInspector] public CanvasGroup m_pl_cri_CanvasGroup;
		[HideInInspector] public UIDefaultValue m_pl_cri_UIDefaultValue;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public FontGradient m_lbl_text_FontGradient;
		[HideInInspector] public Shadow m_lbl_text_Shadow;

		[HideInInspector] public LanguageText m_lbl_nub_LanguageText;
		[HideInInspector] public Shadow m_lbl_nub_Shadow;

		[HideInInspector] public PolygonImage m_img_num_PolygonImage;

		[HideInInspector] public Animator m_pl_source_Animator;
		[HideInInspector] public CanvasGroup m_pl_source_CanvasGroup;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_value_LanguageText;
		[HideInInspector] public Shadow m_lbl_value_Shadow;

		[HideInInspector] public LanguageText m_lbl_add_LanguageText;
		[HideInInspector] public Shadow m_lbl_add_Shadow;

		[HideInInspector] public RectTransform m_pl_effect;


        private void UIFinder()
        {       
			m_UI_Common_Crit = gameObject.GetComponent<RectTransform>();
			m_pl_cri_Animator = FindUI<Animator>(gameObject.transform ,"pl_cri");
			m_pl_cri_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_cri");
			m_pl_cri_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_cri");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_cri/lbl_text");
			m_lbl_text_FontGradient = FindUI<FontGradient>(gameObject.transform ,"pl_cri/lbl_text");
			m_lbl_text_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_cri/lbl_text");

			m_lbl_nub_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_cri/lbl_nub");
			m_lbl_nub_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_cri/lbl_nub");

			m_img_num_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_cri/img_num");

			m_pl_source_Animator = FindUI<Animator>(gameObject.transform ,"pl_source");
			m_pl_source_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_source");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_source/img_icon");

			m_lbl_value_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_source/lbl_value");
			m_lbl_value_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_source/lbl_value");

			m_lbl_add_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_source/lbl_add");
			m_lbl_add_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_source/lbl_add");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");

			BindEvent();
        }

        #endregion
    }
}