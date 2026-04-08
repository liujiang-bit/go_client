// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    UI_Item_ServerListView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_ServerListView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ServerList";

        public UI_Item_ServerListView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_ServerItem_SubView m_UI_Item_ServerItem1;
		[HideInInspector] public UI_Item_ServerItem_SubView m_UI_Item_ServerItem2;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_ServerItem1 = new UI_Item_ServerItem_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ServerItem1"));
			m_UI_Item_ServerItem2 = new UI_Item_ServerItem_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ServerItem2"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}