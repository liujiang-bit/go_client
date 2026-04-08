// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, August 14, 2020
// Update Time         :    Friday, August 14, 2020
// Class Description   :    UI_Item_GuildResearchItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildResearchItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildResearchItem";

        public UI_Item_GuildResearchItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_item1;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_item2;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_item3;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_item4;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_item1 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"item1"));
			m_item2 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"item2"));
			m_item3 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"item3"));
			m_item4 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"item4"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}