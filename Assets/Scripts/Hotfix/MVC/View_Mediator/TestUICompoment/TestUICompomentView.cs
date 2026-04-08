// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    TestUICompomentView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class TestUICompomentView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TestUICompoment";

        public TestUICompomentView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public Image m_pl_demo_list_Image;

		[HideInInspector] public PolygonImage m_btn_top_PolygonImage;
		[HideInInspector] public GameButton m_btn_top_GameButton;

		[HideInInspector] public PolygonImage m_btn_bottom_PolygonImage;
		[HideInInspector] public GameButton m_btn_bottom_GameButton;

		[HideInInspector] public PolygonImage m_btn_right_PolygonImage;
		[HideInInspector] public GameButton m_btn_right_GameButton;

		[HideInInspector] public PolygonImage m_btn_left_PolygonImage;
		[HideInInspector] public GameButton m_btn_left_GameButton;

		[HideInInspector] public Image m_ls_top_view_Image;
		[HideInInspector] public ScrollRect m_ls_top_view_ScrollRect;
		[HideInInspector] public ListView m_ls_top_view_ListView;

		[HideInInspector] public Image m_ls_bottom_view_Image;
		[HideInInspector] public ScrollRect m_ls_bottom_view_ScrollRect;
		[HideInInspector] public ListView m_ls_bottom_view_ListView;

		[HideInInspector] public Image m_ls_right_view_Image;
		[HideInInspector] public ScrollRect m_ls_right_view_ScrollRect;
		[HideInInspector] public ListView m_ls_right_view_ListView;

		[HideInInspector] public Image m_ls_left_view_Image;
		[HideInInspector] public ScrollRect m_ls_left_view_ScrollRect;
		[HideInInspector] public ListView m_ls_left_view_ListView;

		[HideInInspector] public ScrollRect m_sv_scroll_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_scroll_view_PolygonImage;
		[HideInInspector] public ScrollView m_sv_scroll_view_ScrollView;

		[HideInInspector] public PolygonImage m_v_scroll_view_PolygonImage;
		[HideInInspector] public Mask m_v_scroll_view_Mask;

		[HideInInspector] public RectTransform m_c_scroll_view;
		[HideInInspector] public LanguageText m_lbl_curr_LanguageText;

		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;

		[HideInInspector] public PolygonImage m_btn_insert_PolygonImage;
		[HideInInspector] public GameButton m_btn_insert_GameButton;

		[HideInInspector] public PolygonImage m_btn_changeHeight_PolygonImage;
		[HideInInspector] public GameButton m_btn_changeHeight_GameButton;

		[HideInInspector] public PolygonImage m_btn_scrollIndex_PolygonImage;
		[HideInInspector] public GameButton m_btn_scrollIndex_GameButton;

		[HideInInspector] public PolygonImage m_btn_switchIndex_PolygonImage;
		[HideInInspector] public GameButton m_btn_switchIndex_GameButton;

		[HideInInspector] public PolygonImage m_btn_centerIndex_PolygonImage;
		[HideInInspector] public GameButton m_btn_centerIndex_GameButton;

		[HideInInspector] public PolygonImage m_btn_refreshIndex_PolygonImage;
		[HideInInspector] public GameButton m_btn_refreshIndex_GameButton;

		[HideInInspector] public PolygonImage m_btn_initList_PolygonImage;
		[HideInInspector] public GameButton m_btn_initList_GameButton;

		[HideInInspector] public PolygonImage m_ipt_index_PolygonImage;
		[HideInInspector] public GameInput m_ipt_index_GameInput;

		[HideInInspector] public Image m_pl_demo_other_Image;

		[HideInInspector] public Image m_btn_animation_Image;
		[HideInInspector] public Button m_btn_animation_Button;
		[HideInInspector] public BtnAnimation m_btn_animation_ButtonAnimation;

		[HideInInspector] public Image m_btn_sd_ani_test_Image;
		[HideInInspector] public Button m_btn_sd_ani_test_Button;

		[HideInInspector] public Slider m_sd_demo_Slider;
		[HideInInspector] public SmoothProgressBar m_sd_demo_SmoothBar;

		[HideInInspector] public Image m_btn_long_mul_Image;
		[HideInInspector] public Button m_btn_long_mul_Button;
		[HideInInspector] public LongPressBtn m_btn_long_mul_LongClickButton;

		[HideInInspector] public Text m_lbl_long_mul_Text;

		[HideInInspector] public Image m_btn_long_onlyone_Image;
		[HideInInspector] public Button m_btn_long_onlyone_Button;
		[HideInInspector] public LongPressBtn m_btn_long_onlyone_LongClickButton;

		[HideInInspector] public Text m_lbl_long_onlyone_Text;

		[HideInInspector] public HrefText m_lbl_linkImageText_LinkImageText;

		[HideInInspector] public Image m_pl_demo_page_Image;

		[HideInInspector] public Image m_page_view_Image;
		[HideInInspector] public ScrollRect m_page_view_ScrollRect;
		[HideInInspector] public PageView m_page_view_PageView;

		[HideInInspector] public PolygonImage m_btn_jumpPage_PolygonImage;
		[HideInInspector] public GameButton m_btn_jumpPage_GameButton;

		[HideInInspector] public PolygonImage m_ipt_languageInputField_PolygonImage;
		[HideInInspector] public GameInput m_ipt_languageInputField_GameInput;

		[HideInInspector] public Image m_btn_close_Image;
		[HideInInspector] public Button m_btn_close_Button;

		[HideInInspector] public Image m_btn_demo_list_Image;
		[HideInInspector] public Button m_btn_demo_list_Button;
		[HideInInspector] public BtnAnimation m_btn_demo_list_ButtonAnimation;

		[HideInInspector] public Image m_btn_demo_other_Image;
		[HideInInspector] public Button m_btn_demo_other_Button;

		[HideInInspector] public Image m_btn_demo_page_Image;
		[HideInInspector] public Button m_btn_demo_page_Button;
		[HideInInspector] public BtnAnimation m_btn_demo_page_ButtonAnimation;



        private void UIFinder()
        {
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_pl_demo_list_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_list");

			m_btn_top_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/topText/btn_top");
			m_btn_top_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/topText/btn_top");

			m_btn_bottom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/bottomText/btn_bottom");
			m_btn_bottom_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/bottomText/btn_bottom");

			m_btn_right_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/rightText/btn_right");
			m_btn_right_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/rightText/btn_right");

			m_btn_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/leftText/btn_left");
			m_btn_left_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/leftText/btn_left");

			m_ls_top_view_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_list/ls_top_view");
			m_ls_top_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_demo_list/ls_top_view");
			m_ls_top_view_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_demo_list/ls_top_view");

			m_ls_bottom_view_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_list/ls_bottom_view");
			m_ls_bottom_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_demo_list/ls_bottom_view");
			m_ls_bottom_view_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_demo_list/ls_bottom_view");

			m_ls_right_view_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_list/ls_right_view");
			m_ls_right_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_demo_list/ls_right_view");
			m_ls_right_view_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_demo_list/ls_right_view");

			m_ls_left_view_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_list/ls_left_view");
			m_ls_left_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_demo_list/ls_left_view");
			m_ls_left_view_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_demo_list/ls_left_view");

			m_sv_scroll_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_demo_list/sv_scroll_view");
			m_sv_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/sv_scroll_view");
			m_sv_scroll_view_ScrollView = FindUI<ScrollView>(vb.transform ,"pl_content/pl_demo_list/sv_scroll_view");

			m_v_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/sv_scroll_view/v_scroll_view");
			m_v_scroll_view_Mask = FindUI<Mask>(vb.transform ,"pl_content/pl_demo_list/sv_scroll_view/v_scroll_view");

			m_c_scroll_view = FindUI<RectTransform>(vb.transform ,"pl_content/pl_demo_list/sv_scroll_view/v_scroll_view/c_scroll_view");
			m_lbl_curr_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_demo_list/lbl_curr");

			m_btn_delete_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_delete");

			m_btn_insert_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_insert");
			m_btn_insert_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_insert");

			m_btn_changeHeight_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_changeHeight");
			m_btn_changeHeight_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_changeHeight");

			m_btn_scrollIndex_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_scrollIndex");
			m_btn_scrollIndex_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_scrollIndex");

			m_btn_switchIndex_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_switchIndex");
			m_btn_switchIndex_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_switchIndex");

			m_btn_centerIndex_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_centerIndex");
			m_btn_centerIndex_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_centerIndex");

			m_btn_refreshIndex_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_refreshIndex");
			m_btn_refreshIndex_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_refreshIndex");

			m_btn_initList_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_initList");
			m_btn_initList_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_list/btn_initList");

			m_ipt_index_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_list/btn_initList/ipt_index");
			m_ipt_index_GameInput = FindUI<GameInput>(vb.transform ,"pl_content/pl_demo_list/btn_initList/ipt_index");

			m_pl_demo_other_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_other");

			m_btn_animation_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_other/btn_animation");
			m_btn_animation_Button = FindUI<Button>(vb.transform ,"pl_content/pl_demo_other/btn_animation");
			m_btn_animation_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_content/pl_demo_other/btn_animation");

			m_btn_sd_ani_test_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_other/btn_sd_ani_test");
			m_btn_sd_ani_test_Button = FindUI<Button>(vb.transform ,"pl_content/pl_demo_other/btn_sd_ani_test");

			m_sd_demo_Slider = FindUI<Slider>(vb.transform ,"pl_content/pl_demo_other/sd_demo");
			m_sd_demo_SmoothBar = FindUI<SmoothProgressBar>(vb.transform ,"pl_content/pl_demo_other/sd_demo");

			m_btn_long_mul_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_other/btn_long_mul");
			m_btn_long_mul_Button = FindUI<Button>(vb.transform ,"pl_content/pl_demo_other/btn_long_mul");
			m_btn_long_mul_LongClickButton = FindUI<LongPressBtn>(vb.transform ,"pl_content/pl_demo_other/btn_long_mul");

			m_lbl_long_mul_Text = FindUI<Text>(vb.transform ,"pl_content/pl_demo_other/btn_long_mul/lbl_long_mul");

			m_btn_long_onlyone_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_other/btn_long_onlyone");
			m_btn_long_onlyone_Button = FindUI<Button>(vb.transform ,"pl_content/pl_demo_other/btn_long_onlyone");
			m_btn_long_onlyone_LongClickButton = FindUI<LongPressBtn>(vb.transform ,"pl_content/pl_demo_other/btn_long_onlyone");

			m_lbl_long_onlyone_Text = FindUI<Text>(vb.transform ,"pl_content/pl_demo_other/btn_long_onlyone/lbl_long_onlyone");

			m_lbl_linkImageText_LinkImageText = FindUI<HrefText>(vb.transform ,"pl_content/pl_demo_other/lbl_linkImageText");

			m_pl_demo_page_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_page");

			m_page_view_Image = FindUI<Image>(vb.transform ,"pl_content/pl_demo_page/page_view");
			m_page_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_demo_page/page_view");
			m_page_view_PageView = FindUI<PageView>(vb.transform ,"pl_content/pl_demo_page/page_view");

			m_btn_jumpPage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_page/btn_jumpPage");
			m_btn_jumpPage_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_demo_page/btn_jumpPage");

			m_ipt_languageInputField_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_demo_page/btn_jumpPage/ipt_languageInputField");
			m_ipt_languageInputField_GameInput = FindUI<GameInput>(vb.transform ,"pl_content/pl_demo_page/btn_jumpPage/ipt_languageInputField");

			m_btn_close_Image = FindUI<Image>(vb.transform ,"btn_close");
			m_btn_close_Button = FindUI<Button>(vb.transform ,"btn_close");

			m_btn_demo_list_Image = FindUI<Image>(vb.transform ,"btn_demo_list");
			m_btn_demo_list_Button = FindUI<Button>(vb.transform ,"btn_demo_list");
			m_btn_demo_list_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_demo_list");

			m_btn_demo_other_Image = FindUI<Image>(vb.transform ,"btn_demo_other");
			m_btn_demo_other_Button = FindUI<Button>(vb.transform ,"btn_demo_other");

			m_btn_demo_page_Image = FindUI<Image>(vb.transform ,"btn_demo_page");
			m_btn_demo_page_Button = FindUI<Button>(vb.transform ,"btn_demo_page");
			m_btn_demo_page_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_demo_page");


            TestUICompomentMediator mt = new TestUICompomentMediator(vb.gameObject);
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
