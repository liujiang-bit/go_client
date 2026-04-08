// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, July 14, 2020
// Update Time         :    Tuesday, July 14, 2020
// Class Description   :    UI_LC_GuildMemR4View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_LC_GuildMemR4View : GameView
    {
		public const string VIEW_NAME = "UI_LC_GuildMemR4";

        public UI_LC_GuildMemR4View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_GuildMemR4_SubView m_UI_Item_GuildMemR40;
		[HideInInspector] public UI_Item_GuildMemR4_SubView m_UI_Item_GuildMemR41;
		[HideInInspector] public UI_Item_GuildMemR4_SubView m_UI_Item_GuildMemR42;
		[HideInInspector] public UI_Item_GuildMemR4_SubView m_UI_Item_GuildMemR43;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_GuildMemR40 = new UI_Item_GuildMemR4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_GuildMemR40"));
			m_UI_Item_GuildMemR41 = new UI_Item_GuildMemR4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_GuildMemR41"));
			m_UI_Item_GuildMemR42 = new UI_Item_GuildMemR4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_GuildMemR42"));
			m_UI_Item_GuildMemR43 = new UI_Item_GuildMemR4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_GuildMemR43"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}