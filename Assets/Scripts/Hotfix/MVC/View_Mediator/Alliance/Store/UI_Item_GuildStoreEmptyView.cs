// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Item_GuildStoreEmptyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildStoreEmptyView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildStoreEmpty";

        public UI_Item_GuildStoreEmptyView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_Text_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_Text_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_Text");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}