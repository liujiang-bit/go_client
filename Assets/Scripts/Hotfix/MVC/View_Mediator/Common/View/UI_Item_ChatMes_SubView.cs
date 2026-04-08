// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatMes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatMes_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatMes";

        public UI_Item_ChatMes_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChatMes_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChatMes_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_text");


			BindEvent();
        }

        #endregion
    }
}