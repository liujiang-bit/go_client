// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, May 29, 2020
// Update Time         :    Friday, May 29, 2020
// Class Description   :    UI_Item_GuildMessageFlexView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildMessageFlexView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildMessageFlex";

        public UI_Item_GuildMessageFlexView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GameButton m_btn_translation_GameButton;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_translation_GameButton = FindUI<GameButton>(vb.transform ,"btn_translation");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_translation/lbl_playerName");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}