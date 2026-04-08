// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainLevelUpOnHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CaptainLevelUpOnHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainLevelUpOnHead";

        public UI_Item_CaptainLevelUpOnHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_CaptainLevelUpOnHead_Animator;

		[HideInInspector] public LanguageText m_lbl_levelup_LanguageText;
		[HideInInspector] public Outline m_lbl_levelup_Outline;
		[HideInInspector] public Shadow m_lbl_levelup_Shadow;

		[HideInInspector] public LanguageText m_lbl_addatt_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_addatt_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_addatt1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_addatt1_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_CaptainLevelUpOnHead_Animator = gameObject.GetComponent<Animator>();

			m_lbl_levelup_LanguageText = FindUI<LanguageText>(gameObject.transform ,"offset/lbl_levelup");
			m_lbl_levelup_Outline = FindUI<Outline>(gameObject.transform ,"offset/lbl_levelup");
			m_lbl_levelup_Shadow = FindUI<Shadow>(gameObject.transform ,"offset/lbl_levelup");

			m_lbl_addatt_LanguageText = FindUI<LanguageText>(gameObject.transform ,"offset/lbl_addatt");
			m_lbl_addatt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"offset/lbl_addatt");

			m_lbl_addatt1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"offset/lbl_addatt1");
			m_lbl_addatt1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"offset/lbl_addatt1");


			BindEvent();
        }

        #endregion
    }
}