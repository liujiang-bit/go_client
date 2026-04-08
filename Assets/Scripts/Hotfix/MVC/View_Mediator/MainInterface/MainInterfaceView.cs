// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, 04 November 2020
// Update Time         :    Wednesday, 04 November 2020
// Class Description   :    MainInterfaceView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class MainInterfaceView : GameView
    {
        public const string VIEW_NAME = "UI_IF_MainInterface";

        public MainInterfaceView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_age_ArabLayoutCompment;

		[HideInInspector] public Animator m_UI_Item_CurAge_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_CurAge_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_CurAge_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_age_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_age_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_events_set_ArabLayoutCompment;

		[HideInInspector] public Animator m_UI_Item_Events_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_Events_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_Events_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_UI_Item_Events_GridLayoutGroup;

		[HideInInspector] public UI_Item_MainIFEventCharge_SubView m_UI_Item_MainIFEventCharge;
		[HideInInspector] public UI_Item_MainIFEventActivity_SubView m_UI_Item_MainIFEventActivity;
		[HideInInspector] public UI_Tag_MainIFAnime_Right_SubView m_UI_Tag_MainIFAnime_Right;
		[HideInInspector] public UI_Item_NewRoleBtn_SubView m_UI_Item_NewRoleBtn;
		[HideInInspector] public ArabLayoutCompment m_pl_task_set_ArabLayoutCompment;

		[HideInInspector] public Animator m_UI_Item_HotTask_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_HotTask_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_HotTask_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_tasks_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_tasks_PolygonImage;
		[HideInInspector] public ListView m_sv_list_tasks_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public Animator m_c_list_view_Animator;

		[HideInInspector] public PolygonImage m_btn_blue_PolygonImage;
		[HideInInspector] public GameButton m_btn_blue_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_blue_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_task_PolygonImage;
		[HideInInspector] public GameButton m_btn_task_GameButton;
		[HideInInspector] public BtnAnimation m_btn_task_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_task_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_taskredpot_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_taskreddotcount_LanguageText;
		[HideInInspector] public Shadow m_lbl_taskreddotcount_Shadow;

		[HideInInspector] public ArabLayoutCompment m_pl_head_set_ArabLayoutCompment;

		[HideInInspector] public UI_Item_PlayerPowerInfo_SubView m_UI_Item_PlayerPowerInfo;
		[HideInInspector] public Animator m_UI_Item_ChatSearchBuild_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_ChatSearchBuild_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ChatSearchBuild_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_black_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_black_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_line_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_dialog_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_dialog_PolygonImage;
		[HideInInspector] public ListView m_sv_list_dialog_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_dialog_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_btn_chat_PolygonImage;
		[HideInInspector] public GameButton m_btn_chat_GameButton;
		[HideInInspector] public BtnAnimation m_btn_chat_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_chat_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_img_redpoint_chat;
		[HideInInspector] public UI_Tag_MainIFAnime_Left_SubView m_UI_Tag_MainIFAnime_Left;
		[HideInInspector] public PolygonImage m_img_chat1di_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_chat1di_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_chat2di_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_chat2di_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_chat1_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_chat1_ArabLayoutCompment;
		[HideInInspector] public Animator m_img_chat1_Animator;

		[HideInInspector] public PolygonImage m_img_chat2_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_chat2_ArabLayoutCompment;
		[HideInInspector] public Animator m_img_chat2_Animator;

		[HideInInspector] public Animator m_UI_Item_FlagWarn_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_FlagWarn_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_FlagWarn_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_blueSingle_PolygonImage;
		[HideInInspector] public GameButton m_btn_blueSingle_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_blueSingle_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_redThree_PolygonImage;
		[HideInInspector] public GameButton m_btn_redThree_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_redThree_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_redDouble_PolygonImage;
		[HideInInspector] public GameButton m_btn_redDouble_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_redDouble_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_queue_set_ArabLayoutCompment;

		[HideInInspector] public Animator m_UI_Item_Queuebg_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_Queuebg_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_Queuebg_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_queueGroup_CanvasGroup;
		[HideInInspector] public PolygonImage m_pl_queueGroup_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_queueButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_queueButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_queueButton_BtnAnimation;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_img_symbol_PolygonImage;

		[HideInInspector] public Animator m_UI_Item_Position_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_Position_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_Position_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_posBtn_PolygonImage;
		[HideInInspector] public GameButton m_btn_posBtn_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_posBtn_ArabLayoutCompment;
		[HideInInspector] public BtnAnimation m_btn_posBtn_BtnAnimation;

		[HideInInspector] public ArabLayoutCompment m_pl_position_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_position_GridLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_position_server_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_position_server_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_server_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_server_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_positionX_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_positionX_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_X_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_X_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_positionY_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_positionY_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_Y_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_Y_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_search_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_search_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_collect_PolygonImage;
		[HideInInspector] public GameButton m_btn_collect_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_collect_ArabLayoutCompment;
		[HideInInspector] public BtnAnimation m_btn_collect_BtnAnimation;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint_collect;
		[HideInInspector] public ArabLayoutCompment m_pl_map_set_ArabLayoutCompment;

		[HideInInspector] public UI_Item_Map_SubView m_UI_Item_Map;
		[HideInInspector] public ArabLayoutCompment m_pl_LodMenu_set_ArabLayoutCompment;

		[HideInInspector] public UI_Item_LodMenu_SubView m_UI_Item_LodMenu;
		[HideInInspector] public ArabLayoutCompment m_pl_word_set_ArabLayoutCompment;

		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_world;
		[HideInInspector] public ArabLayoutCompment m_pl_resources_set_ArabLayoutCompment;

		[HideInInspector] public UI_Item_PlayerResources_SubView m_UI_Item_PlayerResources;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_mail_ArabLayoutCompment;
		[HideInInspector] public CanvasGroup m_UI_Item_mail_CanvasGroup;
		[HideInInspector] public Animator m_UI_Item_mail_Animator;

		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_mail;
		[HideInInspector] public Animator m_pl_MapAndCity_Animator;
		[HideInInspector] public ArabLayoutCompment m_pl_MapAndCity_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_map_PolygonImage;
		[HideInInspector] public GameButton m_btn_map_GameButton;
		[HideInInspector] public BtnAnimation m_btn_map_BtnAnimation;
		[HideInInspector] public Animator m_btn_map_Animator;
		[HideInInspector] public ArabLayoutCompment m_btn_map_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_ClickAnime_StandardButton_SubView m_UI_Tag_ClickAnimeButton;
		[HideInInspector] public PolygonImage m_btn_city_PolygonImage;
		[HideInInspector] public GameButton m_btn_city_GameButton;
		[HideInInspector] public BtnAnimation m_btn_city_BtnAnimation;
		[HideInInspector] public Animator m_btn_city_Animator;
		[HideInInspector] public ArabLayoutCompment m_btn_city_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_right_set_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_midmenu_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_midmenu_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_UI_Item_warn_ArabLayoutCompment;
		[HideInInspector] public CanvasGroup m_UI_Item_warn_CanvasGroup;

		[HideInInspector] public PolygonImage m_btn_warn_PolygonImage;
		[HideInInspector] public GameButton m_btn_warn_GameButton;
		[HideInInspector] public BtnAnimation m_btn_warn_BtnAnimation;

		[HideInInspector] public UI_Model_Warning_SubView m_UI_Model_Warning;
		[HideInInspector] public UI_Item_AliGuide_SubView m_UI_Item_AliGuide;
		[HideInInspector] public ArabLayoutCompment m_pl_menu_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_UI_Item_Features_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_Features_ArabLayoutCompment;
		[HideInInspector] public Animator m_UI_Item_Features_Animator;

		[HideInInspector] public Animator m_pl_rect_Animator;
		[HideInInspector] public ArabLayoutCompment m_pl_rect_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_btnsbg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_btnsbg_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_img_featuremask_ArabLayoutCompment;
		[HideInInspector] public RectMask2D m_img_featuremask_RectMask2D;

		[HideInInspector] public GridLayoutGroup m_pl_btns_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_btns_ArabLayoutCompment;

		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_Captain;
		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_Item;
		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_Guild;
		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_War;
		[HideInInspector] public UI_Model_FeatureBtn_SubView m_UI_Model_more;
		[HideInInspector] public ArabLayoutCompment m_pl_simple_ode_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_languageCheckBox_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_languageCheckBox_ArabLayoutCompment;

		[HideInInspector] public UI_Pop_MailGetNew_SubView m_UI_Pop_MailGetNew;
		[HideInInspector] public ArabLayoutCompment m_pl_MapAndCity_set_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_BuildAndSearch_Animator;
		[HideInInspector] public ArabLayoutCompment m_pl_BuildAndSearch_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_search_PolygonImage;
		[HideInInspector] public GameButton m_btn_search_GameButton;
		[HideInInspector] public BtnAnimation m_btn_search_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_search_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_build_PolygonImage;
		[HideInInspector] public GameButton m_btn_build_GameButton;
		[HideInInspector] public BtnAnimation m_btn_build_BtnAnimation;
		[HideInInspector] public CanvasGroup m_btn_build_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_btn_build_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {
			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_polygonImage");

			m_pl_age_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_age");

			m_UI_Item_CurAge_Animator = FindUI<Animator>(vb.transform ,"pl_age/UI_Item_CurAge");
			m_UI_Item_CurAge_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_age/UI_Item_CurAge");
			m_UI_Item_CurAge_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_age/UI_Item_CurAge");

			m_lbl_age_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_age/UI_Item_CurAge/lbl_age");
			m_lbl_age_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_age/UI_Item_CurAge/lbl_age");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_age/UI_Item_CurAge/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_age/UI_Item_CurAge/lbl_time");

			m_pl_events_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_events_set");

			m_UI_Item_Events_Animator = FindUI<Animator>(vb.transform ,"pl_events_set/UI_Item_Events");
			m_UI_Item_Events_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_events_set/UI_Item_Events");
			m_UI_Item_Events_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_events_set/UI_Item_Events");
			m_UI_Item_Events_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_events_set/UI_Item_Events");

			m_UI_Item_MainIFEventCharge = new UI_Item_MainIFEventCharge_SubView(FindUI<RectTransform>(vb.transform ,"pl_events_set/UI_Item_Events/UI_Item_MainIFEventCharge"));
			m_UI_Item_MainIFEventActivity = new UI_Item_MainIFEventActivity_SubView(FindUI<RectTransform>(vb.transform ,"pl_events_set/UI_Item_Events/UI_Item_MainIFEventActivity"));
			m_UI_Tag_MainIFAnime_Right = new UI_Tag_MainIFAnime_Right_SubView(FindUI<RectTransform>(vb.transform ,"pl_events_set/UI_Item_Events/UI_Tag_MainIFAnime_Right"));
			m_UI_Item_NewRoleBtn = new UI_Item_NewRoleBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_events_set/UI_Item_Events/UI_Item_NewRoleBtn"));
			m_pl_task_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_task_set");

			m_UI_Item_HotTask_Animator = FindUI<Animator>(vb.transform ,"pl_task_set/UI_Item_HotTask");
			m_UI_Item_HotTask_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_task_set/UI_Item_HotTask");
			m_UI_Item_HotTask_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_task_set/UI_Item_HotTask");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg");

			m_sv_list_tasks_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/sv_list_tasks");
			m_sv_list_tasks_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/sv_list_tasks");
			m_sv_list_tasks_ListView = FindUI<ListView>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/sv_list_tasks");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/sv_list_tasks/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/sv_list_tasks/v_list_view");

			m_c_list_view_Animator = FindUI<Animator>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/sv_list_tasks/v_list_view/c_list_view");

			m_btn_blue_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/btn_blue");
			m_btn_blue_GameButton = FindUI<GameButton>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/btn_blue");
			m_btn_blue_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_task_set/UI_Item_HotTask/img_bg/btn_blue");

			m_btn_task_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task");
			m_btn_task_GameButton = FindUI<GameButton>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task");
			m_btn_task_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task");
			m_btn_task_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task");

			m_img_taskredpot_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task/img_taskredpot");

			m_lbl_taskreddotcount_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task/img_taskredpot/lbl_taskreddotcount");
			m_lbl_taskreddotcount_Shadow = FindUI<Shadow>(vb.transform ,"pl_task_set/UI_Item_HotTask/btn_task/img_taskredpot/lbl_taskreddotcount");

			m_pl_head_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_head_set");

			m_UI_Item_PlayerPowerInfo = new UI_Item_PlayerPowerInfo_SubView(FindUI<RectTransform>(vb.transform ,"pl_head_set/UI_Item_PlayerPowerInfo"));
			m_UI_Item_ChatSearchBuild_Animator = FindUI<Animator>(vb.transform ,"UI_Item_ChatSearchBuild");
			m_UI_Item_ChatSearchBuild_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"UI_Item_ChatSearchBuild");
			m_UI_Item_ChatSearchBuild_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild");

			m_img_black_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/img_black");
			m_img_black_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/img_black");

			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/img_line");
			m_img_line_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/img_line");

			m_sv_list_dialog_ScrollRect = FindUI<ScrollRect>(vb.transform ,"UI_Item_ChatSearchBuild/sv_list_dialog");
			m_sv_list_dialog_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/sv_list_dialog");
			m_sv_list_dialog_ListView = FindUI<ListView>(vb.transform ,"UI_Item_ChatSearchBuild/sv_list_dialog");
			m_sv_list_dialog_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/sv_list_dialog");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"UI_Item_ChatSearchBuild/sv_list_dialog/v_list_view/c_list_view");
			m_btn_chat_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/btn_chat");
			m_btn_chat_GameButton = FindUI<GameButton>(vb.transform ,"UI_Item_ChatSearchBuild/btn_chat");
			m_btn_chat_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"UI_Item_ChatSearchBuild/btn_chat");
			m_btn_chat_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/btn_chat");

			m_img_redpoint_chat = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ChatSearchBuild/btn_chat/img_redpoint_chat"));
			m_UI_Tag_MainIFAnime_Left = new UI_Tag_MainIFAnime_Left_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ChatSearchBuild/UI_Tag_MainIFAnime_Left"));
			m_img_chat1di_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat1di");
			m_img_chat1di_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat1di");

			m_img_chat2di_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat2di");
			m_img_chat2di_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat2di");

			m_img_chat1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat1");
			m_img_chat1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat1");
			m_img_chat1_Animator = FindUI<Animator>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat1");

			m_img_chat2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat2");
			m_img_chat2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat2");
			m_img_chat2_Animator = FindUI<Animator>(vb.transform ,"UI_Item_ChatSearchBuild/img_chat2");

			m_UI_Item_FlagWarn_Animator = FindUI<Animator>(vb.transform ,"UI_Item_FlagWarn");
			m_UI_Item_FlagWarn_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"UI_Item_FlagWarn");
			m_UI_Item_FlagWarn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_FlagWarn");

			m_btn_blueSingle_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_FlagWarn/btn_blueSingle");
			m_btn_blueSingle_GameButton = FindUI<GameButton>(vb.transform ,"UI_Item_FlagWarn/btn_blueSingle");
			m_btn_blueSingle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_FlagWarn/btn_blueSingle");

			m_btn_redThree_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_FlagWarn/btn_redThree");
			m_btn_redThree_GameButton = FindUI<GameButton>(vb.transform ,"UI_Item_FlagWarn/btn_redThree");
			m_btn_redThree_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_FlagWarn/btn_redThree");

			m_btn_redDouble_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_FlagWarn/btn_redDouble");
			m_btn_redDouble_GameButton = FindUI<GameButton>(vb.transform ,"UI_Item_FlagWarn/btn_redDouble");
			m_btn_redDouble_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_FlagWarn/btn_redDouble");

			m_pl_queue_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_queue_set");

			m_UI_Item_Queuebg_Animator = FindUI<Animator>(vb.transform ,"pl_queue_set/UI_Item_Queuebg");
			m_UI_Item_Queuebg_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_queue_set/UI_Item_Queuebg");
			m_UI_Item_Queuebg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_queue_set/UI_Item_Queuebg");

			m_pl_queueGroup_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup");
			m_pl_queueGroup_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup");

			m_btn_queueButton_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/btn_queueButton");
			m_btn_queueButton_GameButton = FindUI<GameButton>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/btn_queueButton");
			m_btn_queueButton_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/btn_queueButton");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/btn_queueButton/lbl_count");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/sv_list_view");

			m_img_symbol_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_queue_set/UI_Item_Queuebg/pl_queueGroup/img_symbol");

			m_UI_Item_Position_Animator = FindUI<Animator>(vb.transform ,"UI_Item_Position");
			m_UI_Item_Position_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"UI_Item_Position");
			m_UI_Item_Position_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position");

			m_btn_posBtn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_Position/btn_posBtn");
			m_btn_posBtn_GameButton = FindUI<GameButton>(vb.transform ,"UI_Item_Position/btn_posBtn");
			m_btn_posBtn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn");
			m_btn_posBtn_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"UI_Item_Position/btn_posBtn");

			m_pl_position_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position");
			m_pl_position_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position");

			m_lbl_position_server_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_position_server");
			m_lbl_position_server_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_position_server");

			m_lbl_server_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_position_server/lbl_server");
			m_lbl_server_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_position_server/lbl_server");

			m_lbl_positionX_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionX");
			m_lbl_positionX_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionX");

			m_lbl_X_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionX/lbl_X");
			m_lbl_X_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionX/lbl_X");

			m_lbl_positionY_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionY");
			m_lbl_positionY_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionY");

			m_lbl_Y_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionY/lbl_Y");
			m_lbl_Y_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/pl_position/lbl_positionY/lbl_Y");

			m_img_search_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_Position/btn_posBtn/img_search");
			m_img_search_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_posBtn/img_search");

			m_btn_collect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_Position/btn_collect");
			m_btn_collect_GameButton = FindUI<GameButton>(vb.transform ,"UI_Item_Position/btn_collect");
			m_btn_collect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_Position/btn_collect");
			m_btn_collect_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"UI_Item_Position/btn_collect");

			m_UI_Common_Redpoint_collect = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_Position/btn_collect/UI_Common_Redpoint_collect"));
			m_pl_map_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_map_set");

			m_UI_Item_Map = new UI_Item_Map_SubView(FindUI<RectTransform>(vb.transform ,"pl_map_set/UI_Item_Map"));
			m_pl_LodMenu_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_LodMenu_set");

			m_UI_Item_LodMenu = new UI_Item_LodMenu_SubView(FindUI<RectTransform>(vb.transform ,"pl_LodMenu_set/UI_Item_LodMenu"));
			m_pl_word_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_word_set");

			m_UI_Model_world = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_word_set/UI_Model_world"));
			m_pl_resources_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_resources_set");

			m_UI_Item_PlayerResources = new UI_Item_PlayerResources_SubView(FindUI<RectTransform>(vb.transform ,"pl_resources_set/UI_Item_PlayerResources"));
			m_UI_Item_mail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_mail");
			m_UI_Item_mail_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"UI_Item_mail");
			m_UI_Item_mail_Animator = FindUI<Animator>(vb.transform ,"UI_Item_mail");

			m_UI_Model_mail = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_mail/UI_Model_mail"));
			m_pl_MapAndCity_Animator = FindUI<Animator>(vb.transform ,"pl_MapAndCity");
			m_pl_MapAndCity_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity");

			m_btn_map_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_MapAndCity/btn_map");
			m_btn_map_GameButton = FindUI<GameButton>(vb.transform ,"pl_MapAndCity/btn_map");
			m_btn_map_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_MapAndCity/btn_map");
			m_btn_map_Animator = FindUI<Animator>(vb.transform ,"pl_MapAndCity/btn_map");
			m_btn_map_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity/btn_map");

			m_UI_Tag_ClickAnimeButton = new UI_Tag_ClickAnime_StandardButton_SubView(FindUI<RectTransform>(vb.transform ,"pl_MapAndCity/btn_map/UI_Tag_ClickAnimeButton"));
			m_btn_city_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_MapAndCity/btn_city");
			m_btn_city_GameButton = FindUI<GameButton>(vb.transform ,"pl_MapAndCity/btn_city");
			m_btn_city_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_MapAndCity/btn_city");
			m_btn_city_Animator = FindUI<Animator>(vb.transform ,"pl_MapAndCity/btn_city");
			m_btn_city_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity/btn_city");

			m_pl_right_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_right_set");

			m_pl_midmenu_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_right_set/pl_midmenu");
			m_pl_midmenu_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_right_set/pl_midmenu");

			m_UI_Item_warn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_warn");
			m_UI_Item_warn_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_warn");

			m_btn_warn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_warn/btn_warn");
			m_btn_warn_GameButton = FindUI<GameButton>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_warn/btn_warn");
			m_btn_warn_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_warn/btn_warn");

			m_UI_Model_Warning = new UI_Model_Warning_SubView(FindUI<RectTransform>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_warn/btn_warn/UI_Model_Warning"));
			m_UI_Item_AliGuide = new UI_Item_AliGuide_SubView(FindUI<RectTransform>(vb.transform ,"pl_right_set/pl_midmenu/UI_Item_AliGuide"));
			m_pl_menu_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_menu");

			m_UI_Item_Features_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_menu/UI_Item_Features");
			m_UI_Item_Features_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_menu/UI_Item_Features");
			m_UI_Item_Features_Animator = FindUI<Animator>(vb.transform ,"pl_menu/UI_Item_Features");

			m_pl_rect_Animator = FindUI<Animator>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect");
			m_pl_rect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect");

			m_img_btnsbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_btnsbg");
			m_img_btnsbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_btnsbg");

			m_img_featuremask_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask");
			m_img_featuremask_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask");

			m_pl_btns_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask/pl_btns");
			m_pl_btns_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask/pl_btns");

			m_UI_Model_Captain = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask/pl_btns/UI_Model_Captain"));
			m_UI_Model_Item = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask/pl_btns/UI_Model_Item"));
			m_UI_Model_Guild = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask/pl_btns/UI_Model_Guild"));
			m_UI_Model_War = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/img_featuremask/pl_btns/UI_Model_War"));
			m_UI_Model_more = new UI_Model_FeatureBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_menu/UI_Item_Features/pl_rect/UI_Model_more"));
			m_pl_simple_ode_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_simple_ode");

			m_ck_languageCheckBox_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_simple_ode/ck_languageCheckBox");
			m_ck_languageCheckBox_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_simple_ode/ck_languageCheckBox");

			m_UI_Pop_MailGetNew = new UI_Pop_MailGetNew_SubView(FindUI<RectTransform>(vb.transform ,"UI_Pop_MailGetNew"));
			m_pl_MapAndCity_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity_set");

			m_pl_BuildAndSearch_Animator = FindUI<Animator>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch");
			m_pl_BuildAndSearch_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch");

			m_btn_search_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_search");
			m_btn_search_GameButton = FindUI<GameButton>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_search");
			m_btn_search_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_search");
			m_btn_search_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_search");

			m_btn_build_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_build");
			m_btn_build_GameButton = FindUI<GameButton>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_build");
			m_btn_build_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_build");
			m_btn_build_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_build");
			m_btn_build_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_build");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"pl_MapAndCity_set/pl_BuildAndSearch/btn_build/UI_Common_Redpoint"));

            MainInterfaceMediator mt = new MainInterfaceMediator(vb.gameObject);
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
