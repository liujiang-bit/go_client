// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月12日
// Update Time         :    2020年3月12日
// Class Description   :    UI_Item_ItemSize65View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ItemSize65View : GameView
    {
		public const string VIEW_NAME = "UI_Item_ItemSize65";

        public UI_Item_ItemSize65View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}