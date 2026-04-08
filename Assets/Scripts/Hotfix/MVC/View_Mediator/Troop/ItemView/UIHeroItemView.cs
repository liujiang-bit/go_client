// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    UIHeroItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UIHeroItemView : GameView
    {
		public const string VIEW_NAME = "UIHeroItem";

        public UIHeroItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_item_lef;
		[HideInInspector] public UI_Item_CaptainHead_SubView m_UI_Item_CaptainHead;
		[HideInInspector] public RectTransform m_item_right;
		[HideInInspector] public UI_Item_CaptainHead_SubView m_UI_Item_CaptainHead1;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_item_lef = FindUI<RectTransform>(vb.transform ,"item_lef");
			m_UI_Item_CaptainHead = new UI_Item_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"item_lef/UI_Item_CaptainHead"));
			m_item_right = FindUI<RectTransform>(vb.transform ,"item_right");
			m_UI_Item_CaptainHead1 = new UI_Item_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"item_right/itemRightView/UI_Item_CaptainHead1"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}