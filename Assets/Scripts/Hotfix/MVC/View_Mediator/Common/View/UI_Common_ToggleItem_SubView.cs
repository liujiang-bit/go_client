// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_ToggleItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Common_ToggleItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_ToggleItem";

        public UI_Common_ToggleItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_UI_Common_ToggleItem_GameToggle;

		[HideInInspector] public LanguageText m_lbl_label_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_label_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Common_ToggleItem_GameToggle = gameObject.GetComponent<GameToggle>();

			m_lbl_label_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_label");
			m_lbl_label_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_label");


			BindEvent();
        }

        #endregion
    }
}