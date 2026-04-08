// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月23日
// Update Time         :    2020年9月23日
// Class Description   :    UI_Win_BookAddView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_BookAddView : GameView
    {
        public const string VIEW_NAME = "UI_Win_BookAdd";

        public UI_Win_BookAddView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public GridLayoutGroup m_pl_page_GridLayoutGroup;
		[HideInInspector] public ToggleGroup m_pl_page_ToggleGroup;

		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side1;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side2;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side3;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side4;
		[HideInInspector] public RectTransform m_pl_top;
		[HideInInspector] public LanguageText m_lbl_coordinate_LanguageText;

		[HideInInspector] public PolygonImage m_ipt_enter_PolygonImage;
		[HideInInspector] public GameInput m_ipt_enter_GameInput;

		[HideInInspector] public LanguageText m_lbl_choosetype_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_book_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_book_ArabLayoutCompment;

		[HideInInspector] public UI_Item_BookType_SubView m_UI_Item_BookType3;
		[HideInInspector] public UI_Item_BookType_SubView m_UI_Item_BookType2;
		[HideInInspector] public UI_Item_BookType_SubView m_UI_Item_BookType1;
		[HideInInspector] public GridLayoutGroup m_pl_sign_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_sign_ArabLayoutCompment;

		[HideInInspector] public UI_Item_SignType_SubView m_UI_Item_SignType;
		[HideInInspector] public HorizontalLayoutGroup m_pl_btn_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_sure;
		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public BtnAnimation m_btn_delete_BtnAnimation;



        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_pl_page_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_page");
			m_pl_page_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"pl_page");

			m_pl_side1 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(vb.transform ,"pl_page/pl_side1"));
			m_pl_side2 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(vb.transform ,"pl_page/pl_side2"));
			m_pl_side3 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(vb.transform ,"pl_page/pl_side3"));
			m_pl_side4 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(vb.transform ,"pl_page/pl_side4"));
			m_pl_top = FindUI<RectTransform>(vb.transform ,"pl_top");
			m_lbl_coordinate_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_top/lbl_coordinate");

			m_ipt_enter_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_top/ipt_enter");
			m_ipt_enter_GameInput = FindUI<GameInput>(vb.transform ,"pl_top/ipt_enter");

			m_lbl_choosetype_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_top/lbl_choosetype");

			m_pl_book_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_book");
			m_pl_book_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_book");

			m_UI_Item_BookType3 = new UI_Item_BookType_SubView(FindUI<RectTransform>(vb.transform ,"pl_book/UI_Item_BookType3"));
			m_UI_Item_BookType2 = new UI_Item_BookType_SubView(FindUI<RectTransform>(vb.transform ,"pl_book/UI_Item_BookType2"));
			m_UI_Item_BookType1 = new UI_Item_BookType_SubView(FindUI<RectTransform>(vb.transform ,"pl_book/UI_Item_BookType1"));
			m_pl_sign_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_sign");
			m_pl_sign_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_sign");

			m_UI_Item_SignType = new UI_Item_SignType_SubView(FindUI<RectTransform>(vb.transform ,"pl_sign/UI_Item_SignType"));
			m_pl_btn_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"pl_btn");

			m_btn_sure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn/btn_sure"));
			m_btn_delete_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_btn/btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(vb.transform ,"pl_btn/btn_delete");
			m_btn_delete_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_btn/btn_delete");


            UI_Win_BookAddMediator mt = new UI_Win_BookAddMediator(vb.gameObject);
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
            vb.onMenuBackCallback = mt.onMenuBackCallback;
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
