// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月9日
// Update Time         :    2019年12月9日
// Class Description   :    ItemListViewChildView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemListViewChildView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ListViewChild";

        public ItemListViewChildView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public Image m_img_select_Image;

		[HideInInspector] public Text m_lbl_info_Text;

		[HideInInspector] public Image m_btn_click_Image;
		[HideInInspector] public Button m_btn_click_Button;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_Image = FindUI<Image>(vb.transform ,"img_bg");

			m_img_select_Image = FindUI<Image>(vb.transform ,"img_bg/img_select");

			m_lbl_info_Text = FindUI<Text>(vb.transform ,"lbl_info");

			m_btn_click_Image = FindUI<Image>(vb.transform ,"btn_click");
			m_btn_click_Button = FindUI<Button>(vb.transform ,"btn_click");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}