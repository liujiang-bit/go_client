// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月17日
// Update Time         :    2020年9月17日
// Class Description   :    UI_Item_PlayerManageView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_PlayerManageView : GameView
    {
		public const string VIEW_NAME = "UI_Item_PlayerManage";

        public UI_Item_PlayerManageView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_PlayerManageItem_SubView m_UI_Item_PlayerManageList1;
		[HideInInspector] public UI_Item_PlayerManageItem_SubView m_UI_Item_PlayerManageList2;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_PlayerManageList1 = new UI_Item_PlayerManageItem_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_PlayerManageList1"));
			m_UI_Item_PlayerManageList2 = new UI_Item_PlayerManageItem_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_PlayerManageList2"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}