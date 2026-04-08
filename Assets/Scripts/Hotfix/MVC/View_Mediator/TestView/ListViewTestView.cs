// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月17日
// Update Time         :    2019年12月17日
// Class Description   :    ListViewTestView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ListViewTestView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ListViewTest";

        public ListViewTestView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;
		[HideInInspector] public Button m_img_bg_Button;

		[HideInInspector] public Image m_img_bg2_Image;

		[HideInInspector] public Image m_btn_listview_Image;
		[HideInInspector] public Button m_btn_listview_Button;

		[HideInInspector] public Image m_img_listview_Image;

		[HideInInspector] public Image m_btn_scrollview_Image;
		[HideInInspector] public Button m_btn_scrollview_Button;

		[HideInInspector] public Image m_img_scrollview_Image;

		[HideInInspector] public Image m_btn_pageview_Image;
		[HideInInspector] public Button m_btn_pageview_Button;

		[HideInInspector] public Image m_img_pageview_Image;

		[HideInInspector] public Image m_sv_listview_Image;
		[HideInInspector] public ScrollRect m_sv_listview_ScrollRect;
		[HideInInspector] public ListView m_sv_listview_ListView;

		[HideInInspector] public Image m_sv_scrollview_Image;
		[HideInInspector] public ScrollRect m_sv_scrollview_ScrollRect;
		[HideInInspector] public ScrollView m_sv_scrollview_ScrollView;

		[HideInInspector] public ScrollRect m_sv_page_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_page_view_PolygonImage;
		[HideInInspector] public PageView m_sv_page_view_PageView;

		[HideInInspector] public PolygonImage m_v_page_view_PolygonImage;
		[HideInInspector] public Mask m_v_page_view_Mask;

		[HideInInspector] public RectTransform m_c_page_view;
		[HideInInspector] public Image m_ipt_field_Image;
		[HideInInspector] public InputField m_ipt_field_InputField;

		[HideInInspector] public Image m_sv_functions_Image;
		[HideInInspector] public ScrollRect m_sv_functions_ScrollRect;

		[HideInInspector] public Image m_v_functions_Image;
		[HideInInspector] public Mask m_v_functions_Mask;

		[HideInInspector] public GridLayoutGroup m_c_functions_GridLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_c_functions_ContentSizeFitter;

		[HideInInspector] public Image m_btn_func0_Image;
		[HideInInspector] public Button m_btn_func0_Button;

		[HideInInspector] public Text m_lbl_fun0_Text;

		[HideInInspector] public Image m_btn_func1_Image;
		[HideInInspector] public Button m_btn_func1_Button;

		[HideInInspector] public Image m_btn_func2_Image;
		[HideInInspector] public Button m_btn_func2_Button;

		[HideInInspector] public Image m_btn_func3_Image;
		[HideInInspector] public Button m_btn_func3_Button;

		[HideInInspector] public Image m_btn_func4_Image;
		[HideInInspector] public Button m_btn_func4_Button;

		[HideInInspector] public Image m_btn_func5_Image;
		[HideInInspector] public Button m_btn_func5_Button;

		[HideInInspector] public Image m_btn_func6_Image;
		[HideInInspector] public Button m_btn_func6_Button;

		[HideInInspector] public Image m_btn_func7_Image;
		[HideInInspector] public Button m_btn_func7_Button;

		[HideInInspector] public Image m_btn_pageJump_Image;
		[HideInInspector] public Button m_btn_pageJump_Button;

		[HideInInspector] public Image m_btn_close_Image;
		[HideInInspector] public Button m_btn_close_Button;



        private void UIFinder()
        {
			m_img_bg_Image = FindUI<Image>(vb.transform ,"img_bg");
			m_img_bg_Button = FindUI<Button>(vb.transform ,"img_bg");

			m_img_bg2_Image = FindUI<Image>(vb.transform ,"img_bg2");

			m_btn_listview_Image = FindUI<Image>(vb.transform ,"btn_listview");
			m_btn_listview_Button = FindUI<Button>(vb.transform ,"btn_listview");

			m_img_listview_Image = FindUI<Image>(vb.transform ,"btn_listview/img_listview");

			m_btn_scrollview_Image = FindUI<Image>(vb.transform ,"btn_scrollview");
			m_btn_scrollview_Button = FindUI<Button>(vb.transform ,"btn_scrollview");

			m_img_scrollview_Image = FindUI<Image>(vb.transform ,"btn_scrollview/img_scrollview");

			m_btn_pageview_Image = FindUI<Image>(vb.transform ,"btn_pageview");
			m_btn_pageview_Button = FindUI<Button>(vb.transform ,"btn_pageview");

			m_img_pageview_Image = FindUI<Image>(vb.transform ,"btn_pageview/img_pageview");

			m_sv_listview_Image = FindUI<Image>(vb.transform ,"sv_listview");
			m_sv_listview_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_listview");
			m_sv_listview_ListView = FindUI<ListView>(vb.transform ,"sv_listview");

			m_sv_scrollview_Image = FindUI<Image>(vb.transform ,"sv_scrollview");
			m_sv_scrollview_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_scrollview");
			m_sv_scrollview_ScrollView = FindUI<ScrollView>(vb.transform ,"sv_scrollview");

			m_sv_page_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_page_view");
			m_sv_page_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_page_view");
			m_sv_page_view_PageView = FindUI<PageView>(vb.transform ,"sv_page_view");

			m_v_page_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_page_view/v_page_view");
			m_v_page_view_Mask = FindUI<Mask>(vb.transform ,"sv_page_view/v_page_view");

			m_c_page_view = FindUI<RectTransform>(vb.transform ,"sv_page_view/v_page_view/c_page_view");
			m_ipt_field_Image = FindUI<Image>(vb.transform ,"ipt_field");
			m_ipt_field_InputField = FindUI<InputField>(vb.transform ,"ipt_field");

			m_sv_functions_Image = FindUI<Image>(vb.transform ,"sv_functions");
			m_sv_functions_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_functions");

			m_v_functions_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions");
			m_v_functions_Mask = FindUI<Mask>(vb.transform ,"sv_functions/v_functions");

			m_c_functions_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"sv_functions/v_functions/c_functions");
			m_c_functions_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"sv_functions/v_functions/c_functions");

			m_btn_func0_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func0");
			m_btn_func0_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func0");

			m_lbl_fun0_Text = FindUI<Text>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func0/lbl_fun0");

			m_btn_func1_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func1");
			m_btn_func1_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func1");

			m_btn_func2_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func2");
			m_btn_func2_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func2");

			m_btn_func3_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func3");
			m_btn_func3_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func3");

			m_btn_func4_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func4");
			m_btn_func4_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func4");

			m_btn_func5_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func5");
			m_btn_func5_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func5");

			m_btn_func6_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func6");
			m_btn_func6_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func6");

			m_btn_func7_Image = FindUI<Image>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func7");
			m_btn_func7_Button = FindUI<Button>(vb.transform ,"sv_functions/v_functions/c_functions/btn_func7");

			m_btn_pageJump_Image = FindUI<Image>(vb.transform ,"btn_pageJump");
			m_btn_pageJump_Button = FindUI<Button>(vb.transform ,"btn_pageJump");

			m_btn_close_Image = FindUI<Image>(vb.transform ,"btn_close");
			m_btn_close_Button = FindUI<Button>(vb.transform ,"btn_close");


            ListViewTestMediator mt = new ListViewTestMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}
