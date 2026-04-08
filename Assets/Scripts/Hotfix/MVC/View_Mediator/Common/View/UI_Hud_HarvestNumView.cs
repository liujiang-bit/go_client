// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月19日
// Update Time         :    2020年1月19日
// Class Description   :    UI_Hud_HarvestNumView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Hud_HarvestNumView : GameView
    {
		public const string VIEW_NAME = "UI_Hud_HarvestNum";

        public UI_Hud_HarvestNumView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_languageText");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}