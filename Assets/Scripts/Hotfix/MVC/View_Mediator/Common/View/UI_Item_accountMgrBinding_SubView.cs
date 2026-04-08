// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_accountMgrBinding_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_accountMgrBinding_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_accountMgrBinding";

        public UI_Item_accountMgrBinding_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_accountMgrBinding;
		[HideInInspector] public LanguageText m_lbl_buindingName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buindingName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_alreadyBinding_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_alreadyBinding_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Binding;


        private void UIFinder()
        {       
			m_UI_Item_accountMgrBinding = gameObject.GetComponent<RectTransform>();
			m_lbl_buindingName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_buindingName");
			m_lbl_buindingName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_buindingName");

			m_lbl_alreadyBinding_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_alreadyBinding");
			m_lbl_alreadyBinding_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_alreadyBinding");

			m_UI_Binding = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Binding"));

			BindEvent();
        }

        #endregion
    }
}