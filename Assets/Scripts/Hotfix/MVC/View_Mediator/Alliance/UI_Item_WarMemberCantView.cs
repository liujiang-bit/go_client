// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月1日
// Update Time         :    2020年6月1日
// Class Description   :    UI_Item_WarMemberCantView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarMemberCantView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarMemberCant";

        public UI_Item_WarMemberCantView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_text");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}