// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_PowerItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_PowerItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_PowerItem";

        public UI_Model_PowerItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_PowerItem;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_value_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_value_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_PowerItem = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_lbl_value_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_value");
			m_lbl_value_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_value");


			BindEvent();
        }

        #endregion
    }
}