// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_LC_GuildMemR0View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_LC_GuildMemR0View : GameView
    {
		public const string VIEW_NAME = "UI_LC_GuildMemR0";

        public UI_LC_GuildMemR0View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_GuildMemR0_SubView m_UI_Item_GuildMemR00;
		[HideInInspector] public UI_Item_GuildMemR0_SubView m_UI_Item_GuildMemR01;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_GuildMemR00 = new UI_Item_GuildMemR0_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_GuildMemR00"));
			m_UI_Item_GuildMemR01 = new UI_Item_GuildMemR0_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_GuildMemR01"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}