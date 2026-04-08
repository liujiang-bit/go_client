// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月6日
// Update Time         :    2019年12月6日
// Class Description   :    ItemListTestView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemListTestView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ListTest";

        public ItemListTestView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Text m_lbl_text_Text;

		[HideInInspector] public Image m_btn_test_Image;
		[HideInInspector] public Button m_btn_test_Button;

		[HideInInspector] public Image m_btn_print_Image;
		[HideInInspector] public Button m_btn_print_Button;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_text_Text = FindUI<Text>(vb.transform ,"lbl_text");

			m_btn_test_Image = FindUI<Image>(vb.transform ,"btn_test");
			m_btn_test_Button = FindUI<Button>(vb.transform ,"btn_test");

			m_btn_print_Image = FindUI<Image>(vb.transform ,"btn_print");
			m_btn_print_Button = FindUI<Button>(vb.transform ,"btn_print");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}