// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月9日
// Update Time         :    2019年12月9日
// Class Description   :    ItemListViewParentView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemListViewParentView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ListViewParent";

        public ItemListViewParentView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public Image m_img_select_Image;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_Image = FindUI<Image>(vb.transform ,"img_bg");

			m_img_select_Image = FindUI<Image>(vb.transform ,"img_bg/img_select");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}