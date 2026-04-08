// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ArmyDetailsColunm_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_ArmyDetailsColunm_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ArmyDetailsColunm";

        public UI_Model_ArmyDetailsColunm_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public HorizontalLayoutGroup m_UI_Model_ArmyDetailsColunm_HorizontalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_col1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col4_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col5_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_ArmyDetailsColunm_HorizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();

			m_lbl_col1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_col1");

			m_lbl_col2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_col2");

			m_lbl_col3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_col3");

			m_lbl_col4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_col4");

			m_lbl_col5_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_col5");


			BindEvent();
        }

        #endregion
    }
}