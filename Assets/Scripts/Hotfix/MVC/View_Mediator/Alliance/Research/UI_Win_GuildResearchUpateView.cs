// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月10日
// Update Time         :    2020年7月10日
// Class Description   :    UI_Win_GuildResearchUpateView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildResearchUpateView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildResearchUpate";

        public UI_Win_GuildResearchUpateView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_win;
		[HideInInspector] public Animator m_img_update_Animator;
		[HideInInspector] public CanvasGroup m_img_update_CanvasGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_more_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_more_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;

		[HideInInspector] public GameButton m_btn_mark_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_mark_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_mark_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_mark_ArabLayoutCompment;
		[HideInInspector] public GrayChildrens m_img_mark_MakeChildrenGray;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_leveLow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_leveLow_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_leveLow_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_up_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_up_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_leveHight_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_leveHight_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_upPlane_ArabLayoutCompment;

		[HideInInspector] public UI_Model_AttrList_SubView m_UI_Model_AttrList;
		[HideInInspector] public LanguageText m_lbl_max_LanguageText;

		[HideInInspector] public RectTransform m_pl_unlock;
		[HideInInspector] public LanguageText m_lbl_unlock_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_go;
		[HideInInspector] public PolygonImage m_pl_needPreResearch_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_preResearchList_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_preResearchList_PolygonImage;
		[HideInInspector] public ListView m_sv_preResearchList_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public RectTransform m_pl_condition;
		[HideInInspector] public PolygonImage m_img_needPay_PolygonImage;

		[HideInInspector] public UI_Model_ResCost_SubView m_UI_Model_ResCost;
		[HideInInspector] public PolygonImage m_pl_limit_PolygonImage;

		[HideInInspector] public UI_Item_BuildingUpLimit_SubView m_UI_BuildingUpLimit;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_btn_research;
		[HideInInspector] public PolygonImage m_pl_conduct_PolygonImage;

		[HideInInspector] public GameSlider m_pb_spBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_spTimeLeft_LanguageText;

		[HideInInspector] public PolygonImage m_img_spbg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_spIcon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_cancel_PolygonImage;
		[HideInInspector] public GameButton m_btn_cancel_GameButton;

		[HideInInspector] public RectTransform m_pl_donate;
		[HideInInspector] public GameSlider m_pb_donateBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_donateBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_pro_LanguageText;

		[HideInInspector] public PolygonImage m_btn_icon_PolygonImage;
		[HideInInspector] public GameButton m_btn_icon_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_help_PolygonImage;
		[HideInInspector] public GameButton m_btn_help_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_help_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_img_curGuild;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_img_curCredit;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_donate_gem;
		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_donate_res;
		[HideInInspector] public LanguageText m_lbl_chance_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_chance_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_effect_pos;
		[HideInInspector] public Animator m_pl_sv_level_Animator;
		[HideInInspector] public CanvasGroup m_pl_sv_level_CanvasGroup;
		[HideInInspector] public VerticalLayoutGroup m_pl_sv_level_VerticalLayoutGroup;

		[HideInInspector] public RectTransform m_pl_tilte;
		[HideInInspector] public ScrollRect m_sv_levelData_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_levelData_PolygonImage;
		[HideInInspector] public ListView m_sv_levelData_ListView;



        private void UIFinder()
        {
			m_UI_win = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_win"));
			m_img_update_Animator = FindUI<Animator>(vb.transform ,"rect/img_update");
			m_img_update_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/img_update");

			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_left/img_icon");

			m_btn_more_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_left/btn_more");
			m_btn_more_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_left/btn_more");
			m_btn_more_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left/btn_more");

			m_pb_bar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/img_update/pl_left/pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left/pb_bar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_left/pb_bar/lbl_barText");

			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_left/lbl_des");

			m_btn_mark_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_left/btn_mark");
			m_btn_mark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left/btn_mark");

			m_img_mark_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_left/btn_mark/img_mark");
			m_img_mark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_left/btn_mark/img_mark");
			m_img_mark_MakeChildrenGray = FindUI<GrayChildrens>(vb.transform ,"rect/img_update/pl_left/btn_mark/img_mark");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right");

			m_lbl_leveLow_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow");
			m_lbl_leveLow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow");
			m_lbl_leveLow_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow");

			m_img_up_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/img_up");
			m_img_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/img_up");

			m_lbl_leveHight_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/lbl_leveHight");
			m_lbl_leveHight_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/lbl_leveLow/lbl_leveHight");

			m_pl_upPlane_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_upPlane");

			m_UI_Model_AttrList = new UI_Model_AttrList_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/UI_Model_AttrList"));
			m_lbl_max_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/lbl_max");

			m_pl_unlock = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_unlock");
			m_lbl_unlock_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/pl_unlock/lbl_unlock");

			m_btn_go = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_unlock/btn_go"));
			m_pl_needPreResearch_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch");

			m_sv_preResearchList_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch/sv_preResearchList");
			m_sv_preResearchList_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch/sv_preResearchList");
			m_sv_preResearchList_ListView = FindUI<ListView>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch/sv_preResearchList");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch/sv_preResearchList/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch/sv_preResearchList/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_needPreResearch/sv_preResearchList/v_list_view/c_list_view");
			m_pl_condition = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition");
			m_img_needPay_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/img_needPay");

			m_UI_Model_ResCost = new UI_Model_ResCost_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/img_needPay/UI_Model_ResCost"));
			m_pl_limit_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_limit");

			m_UI_BuildingUpLimit = new UI_Item_BuildingUpLimit_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/pl_limit/UI_BuildingUpLimit"));
			m_btn_research = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_condition/btn_research"));
			m_pl_conduct_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_conduct");

			m_pb_spBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/img_update/pl_right/pl_conduct/pb_spBar");

			m_lbl_spTimeLeft_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/pl_conduct/pb_spBar/lbl_spTimeLeft");

			m_img_spbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_conduct/img_spbg");

			m_img_spIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_conduct/img_spbg/img_spIcon");

			m_btn_cancel_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_conduct/btn_cancel");
			m_btn_cancel_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_right/pl_conduct/btn_cancel");

			m_pl_donate = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_donate");
			m_pb_donateBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/img_update/pl_right/pl_donate/pb_donateBar");
			m_pb_donateBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_donate/pb_donateBar");

			m_lbl_pro_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/pl_donate/pb_donateBar/lbl_pro");

			m_btn_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_icon");
			m_btn_icon_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_icon");
			m_btn_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_icon");

			m_btn_help_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_help");
			m_btn_help_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_help");
			m_btn_help_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_help");

			m_img_curGuild = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_donate/img_curGuild"));
			m_img_curCredit = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_donate/img_curCredit"));
			m_btn_donate_gem = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_donate_gem"));
			m_btn_donate_res = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_donate/btn_donate_res"));
			m_lbl_chance_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/pl_donate/lbl_chance");
			m_lbl_chance_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_donate/lbl_chance");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_update/pl_right/pl_donate/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_update/pl_right/pl_donate/lbl_time");

			m_pl_effect_pos = FindUI<RectTransform>(vb.transform ,"rect/img_update/pl_right/pl_donate/pl_effect_pos");
			m_pl_sv_level_Animator = FindUI<Animator>(vb.transform ,"rect/pl_sv_level");
			m_pl_sv_level_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/pl_sv_level");
			m_pl_sv_level_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/pl_sv_level");

			m_pl_tilte = FindUI<RectTransform>(vb.transform ,"rect/pl_sv_level/pl_tilte");
			m_sv_levelData_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_sv_level/sv_levelData");
			m_sv_levelData_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_sv_level/sv_levelData");
			m_sv_levelData_ListView = FindUI<ListView>(vb.transform ,"rect/pl_sv_level/sv_levelData");


            UI_Win_GuildResearchUpateMediator mt = new UI_Win_GuildResearchUpateMediator(vb.gameObject);
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
