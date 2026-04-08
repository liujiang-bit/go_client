// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月23日
// Update Time         :    2020年2月23日
// Class Description   :    UI_Item_WorldObjInfoTextView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WorldObjInfoTextView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoText";

        public UI_Item_WorldObjInfoTextView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_title");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}