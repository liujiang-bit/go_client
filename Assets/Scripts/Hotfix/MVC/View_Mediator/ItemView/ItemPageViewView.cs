// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月16日
// Update Time         :    2019年12月16日
// Class Description   :    ItemPageViewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemPageViewView : GameView
    {
		public const string VIEW_NAME = "UI_Item_PageView";

        public ItemPageViewView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public Text m_lbl_text_Text;

		[HideInInspector] public Image m_btn_test_Image;
		[HideInInspector] public Button m_btn_test_Button;

		[HideInInspector] public Image m_btn_print_Image;
		[HideInInspector] public Button m_btn_print_Button;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public Image m_pl_view1_Image;

		[HideInInspector] public Image m_pl_view2_Image;

		[HideInInspector] public Image m_pl_view3_Image;

		[HideInInspector] public Image m_pl_view4_Image;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_Image = FindUI<Image>(vb.transform ,"img_bg");

			m_lbl_text_Text = FindUI<Text>(vb.transform ,"lbl_text");

			m_btn_test_Image = FindUI<Image>(vb.transform ,"btn_test");
			m_btn_test_Button = FindUI<Button>(vb.transform ,"btn_test");

			m_btn_print_Image = FindUI<Image>(vb.transform ,"btn_print");
			m_btn_print_Button = FindUI<Button>(vb.transform ,"btn_print");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"sv_list_view/v_list_view/c_list_view");
			m_pl_view1_Image = FindUI<Image>(vb.transform ,"sv_list_view/v_list_view/c_list_view/pl_view1");

			m_pl_view2_Image = FindUI<Image>(vb.transform ,"sv_list_view/v_list_view/c_list_view/pl_view2");

			m_pl_view3_Image = FindUI<Image>(vb.transform ,"sv_list_view/v_list_view/c_list_view/pl_view3");

			m_pl_view4_Image = FindUI<Image>(vb.transform ,"sv_list_view/v_list_view/c_list_view/pl_view4");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}