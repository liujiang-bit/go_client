// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月28日
// Update Time         :    2020年7月28日
// Class Description   :    ItemBagView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemBagView : GameView
    {
		public const string VIEW_NAME = "UI_Item_Bag";

        public ItemBagView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public RectTransform m_pl_new;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item"));
			m_pl_new = FindUI<RectTransform>(vb.transform ,"pl_new");


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}