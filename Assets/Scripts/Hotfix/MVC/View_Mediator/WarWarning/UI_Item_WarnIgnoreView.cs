// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_WarnIgnoreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarnIgnoreView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarnIgnore";

        public UI_Item_WarnIgnoreView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_warn_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_warn_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_btn;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_warn_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_warn");
			m_lbl_warn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_warn");

			m_UI_Model_btn = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_btn"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}