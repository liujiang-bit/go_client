// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Item_GuildStoreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildStoreView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildStore";

        public UI_Item_GuildStoreView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item4;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item4"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}