// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Text_DamageDead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Text_DamageDead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Text_DamageDead";

        public UI_Text_DamageDead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Text_DamageDead_ViewBinder;
		[HideInInspector] public MapElementUI m_UI_Text_DamageDead_MapElementUI;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public Outline m_lbl_text_Outline;
		[HideInInspector] public Shadow m_lbl_text_Shadow;

		[HideInInspector] public LanguageText m_lbl_ordinaryAtk_LanguageText;
		[HideInInspector] public Animator m_lbl_ordinaryAtk_Animator;

		[HideInInspector] public RectTransform m_pl_skillAtkOffset;
		[HideInInspector] public RectTransform m_pl_buffOffset;
		[HideInInspector] public LanguageText m_lbl_deBuff_LanguageText;
		[HideInInspector] public Outline m_lbl_deBuff_Outline;
		[HideInInspector] public Shadow m_lbl_deBuff_Shadow;
		[HideInInspector] public ContentSizeFitter m_lbl_deBuff_ContentSizeFitter;
		[HideInInspector] public Animator m_lbl_deBuff_Animator;
		[HideInInspector] public CanvasGroup m_lbl_deBuff_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_buff_LanguageText;
		[HideInInspector] public Outline m_lbl_buff_Outline;
		[HideInInspector] public Shadow m_lbl_buff_Shadow;
		[HideInInspector] public ContentSizeFitter m_lbl_buff_ContentSizeFitter;
		[HideInInspector] public Animator m_lbl_buff_Animator;
		[HideInInspector] public CanvasGroup m_lbl_buff_CanvasGroup;

		[HideInInspector] public RectTransform m_pl_atkbyatkOffset;
		[HideInInspector] public LanguageText m_lbl_atkbyatk_LanguageText;
		[HideInInspector] public Outline m_lbl_atkbyatk_Outline;
		[HideInInspector] public Animator m_lbl_atkbyatk_Animator;

		[HideInInspector] public RectTransform m_pl_routOffset;
		[HideInInspector] public LanguageText m_lbl_rout_LanguageText;
		[HideInInspector] public Outline m_lbl_rout_Outline;
		[HideInInspector] public Shadow m_lbl_rout_Shadow;
		[HideInInspector] public Animator m_lbl_rout_Animator;

		[HideInInspector] public LanguageText m_lbl_fail_LanguageText;
		[HideInInspector] public Outline m_lbl_fail_Outline;
		[HideInInspector] public Shadow m_lbl_fail_Shadow;
		[HideInInspector] public Animator m_lbl_fail_Animator;



        private void UIFinder()
        {       
			m_UI_Text_DamageDead_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Text_DamageDead_MapElementUI = gameObject.GetComponent<MapElementUI>();

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_text");
			m_lbl_text_Outline = FindUI<Outline>(gameObject.transform ,"lbl_text");
			m_lbl_text_Shadow = FindUI<Shadow>(gameObject.transform ,"lbl_text");

			m_lbl_ordinaryAtk_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_ordinaryAtk");
			m_lbl_ordinaryAtk_Animator = FindUI<Animator>(gameObject.transform ,"lbl_ordinaryAtk");

			m_pl_skillAtkOffset = FindUI<RectTransform>(gameObject.transform ,"pl_skillAtkOffset");
			m_pl_buffOffset = FindUI<RectTransform>(gameObject.transform ,"pl_buffOffset");
			m_lbl_deBuff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_Outline = FindUI<Outline>(gameObject.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_Animator = FindUI<Animator>(gameObject.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_buffOffset/lbl_deBuff");

			m_lbl_buff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_Outline = FindUI<Outline>(gameObject.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_Animator = FindUI<Animator>(gameObject.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_buffOffset/lbl_buff");

			m_pl_atkbyatkOffset = FindUI<RectTransform>(gameObject.transform ,"pl_atkbyatkOffset");
			m_lbl_atkbyatk_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_atkbyatkOffset/lbl_atkbyatk");
			m_lbl_atkbyatk_Outline = FindUI<Outline>(gameObject.transform ,"pl_atkbyatkOffset/lbl_atkbyatk");
			m_lbl_atkbyatk_Animator = FindUI<Animator>(gameObject.transform ,"pl_atkbyatkOffset/lbl_atkbyatk");

			m_pl_routOffset = FindUI<RectTransform>(gameObject.transform ,"pl_routOffset");
			m_lbl_rout_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_routOffset/lbl_rout");
			m_lbl_rout_Outline = FindUI<Outline>(gameObject.transform ,"pl_routOffset/lbl_rout");
			m_lbl_rout_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_routOffset/lbl_rout");
			m_lbl_rout_Animator = FindUI<Animator>(gameObject.transform ,"pl_routOffset/lbl_rout");

			m_lbl_fail_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_routOffset/lbl_fail");
			m_lbl_fail_Outline = FindUI<Outline>(gameObject.transform ,"pl_routOffset/lbl_fail");
			m_lbl_fail_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_routOffset/lbl_fail");
			m_lbl_fail_Animator = FindUI<Animator>(gameObject.transform ,"pl_routOffset/lbl_fail");


			BindEvent();
        }

        #endregion
    }
}