// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, March 17, 2020
// Update Time         :    Tuesday, March 17, 2020
// Class Description   :    ResearchUpateView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ResearchUpateView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ResearchUpate";

        public ResearchUpateView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public Animator m_img_update_Animator;
		[HideInInspector] public CanvasGroup m_img_update_CanvasGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_more_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_GameButton;

		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_leveLow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_leveLow_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_leveLow_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_up_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_up_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_leveHight_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_leveHight_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_upPlane_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_unlock_LanguageText;

		[HideInInspector] public UI_Model_AttrList_SubView m_UI_Model_AttrList;
		[HideInInspector] public RectTransform m_pl_condition;
		[HideInInspector] public PolygonImage m_img_needPay_PolygonImage;

		[HideInInspector] public UI_Model_ResCost_SubView m_UI_Model_ResCost;
		[HideInInspector] public PolygonImage m_pl_needPreResearch_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_preResearchList_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_preResearchList_PolygonImage;
		[HideInInspector] public ListView m_sv_preResearchList_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_pl_BuildingUpLimit_PolygonImage;

		[HideInInspector] public UI_Item_BuildingUpLimit_SubView m_UI_BuildingUpLimit;
		[HideInInspector] public RectTransform m_pl_btns;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_long_SubView m_btn_research;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_long_SubView m_btn_researchNow;
		[HideInInspector] public PolygonImage m_pl_speedUp_PolygonImage;

		[HideInInspector] public GameSlider m_pb_spBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_spTimeLeft_LanguageText;

		[HideInInspector] public PolygonImage m_img_spbg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_spIcon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_cancel_PolygonImage;
		[HideInInspector] public GameButton m_btn_cancel_GameButton;

		[HideInInspector] public PolygonImage m_btn_speedUp_PolygonImage;
		[HideInInspector] public GameButton m_btn_speedUp_GameButton;

		[HideInInspector] public PolygonImage m_btn_speedHelp_PolygonImage;
		[HideInInspector] public GameButton m_btn_speedHelp_GameButton;

		[HideInInspector] public Animator m_pl_power_Animator;
		[HideInInspector] public CanvasGroup m_pl_power_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_subDes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_subDes_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_power_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_power_PolygonImage;
		[HideInInspector] public ListView m_sv_power_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_img_update_Animator = FindUI<Animator>(vb.transform ,"rect/img_update");
			m_img_update_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/img_update");

			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_left/img_icon");

			m_btn_more_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_left/btn_more");
			m_btn_more_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_left/btn_more");

			m_pb_bar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/img_update/pl_left/pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left/pb_bar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_left/pb_bar/lbl_barText");

			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_left/lbl_des");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right");

			m_lbl_leveLow_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow");
			m_lbl_leveLow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow");
			m_lbl_leveLow_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow");

			m_img_up_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/img_up");
			m_img_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/img_up");

			m_lbl_leveHight_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/lbl_leveHight");
			m_lbl_leveHight_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/lbl_leveHight");

			m_pl_upPlane_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_upPlane");

			m_lbl_unlock_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/lbl_unlock");

			m_UI_Model_AttrList = new UI_Model_AttrList_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/UI_Model_AttrList"));
			m_pl_condition = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition");
			m_img_needPay_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/img_needPay");

			m_UI_Model_ResCost = new UI_Model_ResCost_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/img_needPay/UI_Model_ResCost"));
			m_pl_needPreResearch_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch");

			m_sv_preResearchList_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch/sv_preResearchList");
			m_sv_preResearchList_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch/sv_preResearchList");
			m_sv_preResearchList_ListView = FindUI<ListView>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch/sv_preResearchList");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch/sv_preResearchList/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch/sv_preResearchList/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_needPreResearch/sv_preResearchList/v_list_view/c_list_view");
			m_pl_BuildingUpLimit_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_BuildingUpLimit");

			m_UI_BuildingUpLimit = new UI_Item_BuildingUpLimit_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_BuildingUpLimit/UI_BuildingUpLimit"));
			m_pl_btns = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_btns");
			m_btn_research = new UI_Model_DoubleLineButton_Blue_long_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_btns/btn_research"));
			m_btn_researchNow = new UI_Model_DoubleLineButton_Yellow_long_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_btns/btn_researchNow"));
			m_pl_speedUp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_speedUp");

			m_pb_spBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/pb_spBar");

			m_lbl_spTimeLeft_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/pb_spBar/lbl_spTimeLeft");

			m_img_spbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/img_spbg");

			m_img_spIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/img_spbg/img_spIcon");

			m_btn_cancel_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/btn_cancel");
			m_btn_cancel_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/btn_cancel");

			m_btn_speedUp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/btn_speedUp");
			m_btn_speedUp_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/btn_speedUp");

			m_btn_speedHelp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/btn_speedHelp");
			m_btn_speedHelp_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_right/pl_speedUp/btn_speedHelp");

			m_pl_power_Animator = FindUI<Animator>(vb.transform ,"rect/pl_power");
			m_pl_power_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/pl_power");

			m_lbl_subDes_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_power/lbl_subDes");
			m_lbl_subDes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_power/lbl_subDes");

			m_sv_power_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_power/sv_power");
			m_sv_power_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_power/sv_power");
			m_sv_power_ListView = FindUI<ListView>(vb.transform ,"rect/pl_power/sv_power");


            ResearchUpateMediator mt = new ResearchUpateMediator(vb.gameObject);
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
