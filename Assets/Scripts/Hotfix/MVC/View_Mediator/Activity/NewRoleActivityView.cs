// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, 12 October 2020
// Update Time         :    Monday, 12 October 2020
// Class Description   :    NewRoleActivityView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class NewRoleActivityView : GameView
    {
        public const string VIEW_NAME = "UI_Win_NewRoleActivity";

        public NewRoleActivityView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_reset_PolygonImage;
		[HideInInspector] public GameButton m_btn_reset_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_reset_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_reset_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_list_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;
		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_right_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public LanguageText m_lbl_actcur_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_actcur_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_received_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_received_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_rewardsinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_rewardsinfo_GameButton;
		[HideInInspector] public BtnAnimation m_btn_rewardsinfo_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_rewardsinfo_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_info_CanvasGroup;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_info_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_info_PolygonImage;
		[HideInInspector] public ListView m_sv_info_ListView;

		[HideInInspector] public PolygonImage m_v_info_PolygonImage;
		[HideInInspector] public Mask m_v_info_Mask;

		[HideInInspector] public ContentSizeFitter m_c_info_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_c_info_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_info_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_info_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_info_ArabLayoutCompment;

		[HideInInspector] public ViewBinder m_UI_Tip_reward_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_closeButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton_GameButton;

		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;
		[HideInInspector] public ArabLayoutCompment m_pl_pos_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public RectMask2D m_img_bg_RectMask2D;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideR_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_tips_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_tips_PolygonImage;
		[HideInInspector] public ListView m_sv_tips_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_tips_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_tips_PolygonImage;
		[HideInInspector] public Mask m_v_tips_Mask;

		[HideInInspector] public RectTransform m_c_tips;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;
		[HideInInspector] public BtnAnimation m_btn_close_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_close_ArabLayoutCompment;



        private void UIFinder()
        {
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"pl_mes/lbl_title");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/btn_info");

			m_btn_reset_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/btn_reset");
			m_btn_reset_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/btn_reset");
			m_btn_reset_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/btn_reset");

			m_lbl_reset_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/btn_reset/lbl_reset");

			m_pl_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_list");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/pl_list/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_list/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_mes/pl_list/sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_list/sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(vb.transform ,"pl_mes/pl_list/sv_list/v_list");

			m_c_list = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_list/sv_list/v_list/c_list");
			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right");

			m_img_right_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/img_right_bg");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_time");

			m_lbl_actcur_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_actcur");
			m_lbl_actcur_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/lbl_actcur");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/UI_Model_Item"));
			m_lbl_received_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_received");
			m_lbl_received_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/lbl_received");

			m_btn_rewardsinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/btn_rewardsinfo");
			m_btn_rewardsinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_right/btn_rewardsinfo");
			m_btn_rewardsinfo_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/pl_right/btn_rewardsinfo");
			m_btn_rewardsinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/btn_rewardsinfo");

			m_pl_info_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_info");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_info/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"pl_info/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_info/btn_back");

			m_sv_info_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_info/sv_info");
			m_sv_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_info/sv_info");
			m_sv_info_ListView = FindUI<ListView>(vb.transform ,"pl_info/sv_info");

			m_v_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_info/sv_info/v_info");
			m_v_info_Mask = FindUI<Mask>(vb.transform ,"pl_info/sv_info/v_info");

			m_c_info_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_info/sv_info/v_info/c_info");
			m_c_info_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_info/sv_info/v_info/c_info");

			m_lbl_info_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");

			m_UI_Tip_reward_ViewBinder = FindUI<ViewBinder>(vb.transform ,"UI_Tip_reward");

			m_btn_closeButton_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/btn_closeButton");
			m_btn_closeButton_GameButton = FindUI<GameButton>(vb.transform ,"UI_Tip_reward/btn_closeButton");

			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"UI_Tip_reward/pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"UI_Tip_reward/pl_pos");
			m_pl_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Tip_reward/pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg");
			m_img_bg_RectMask2D = FindUI<RectMask2D>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/img_arrowSideR");
			m_img_arrowSideR_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/img_arrowSideL");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/img_arrowSideTop");

			m_sv_tips_ScrollRect = FindUI<ScrollRect>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips");
			m_sv_tips_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips");
			m_sv_tips_ListView = FindUI<ListView>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips");
			m_sv_tips_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips");

			m_v_tips_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips/v_tips");
			m_v_tips_Mask = FindUI<Mask>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips/v_tips");

			m_c_tips = FindUI<RectTransform>(vb.transform ,"UI_Tip_reward/pl_pos/img_bg/sv_tips/v_tips/c_tips");
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"btn_close");
			m_btn_close_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_close");
			m_btn_close_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_close");


            NewRoleActivityMediator mt = new NewRoleActivityMediator(vb.gameObject);
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
