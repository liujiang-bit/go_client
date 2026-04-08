// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Item_PlayerNewCharView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_PlayerNewCharView : GameView
    {
		public const string VIEW_NAME = "UI_Item_PlayerNewChar";

        public UI_Item_PlayerNewCharView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_PlayerNewCharItem_SubView m_UI_Item_PlayerNewCharItem1;
		[HideInInspector] public UI_Item_PlayerNewCharItem_SubView m_UI_Item_PlayerNewCharItem2;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_PlayerNewCharItem1 = new UI_Item_PlayerNewCharItem_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_PlayerNewCharItem1"));
			m_UI_Item_PlayerNewCharItem2 = new UI_Item_PlayerNewCharItem_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_PlayerNewCharItem2"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}