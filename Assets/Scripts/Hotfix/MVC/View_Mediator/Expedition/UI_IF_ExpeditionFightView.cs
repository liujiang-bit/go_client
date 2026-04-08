// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_IF_ExpeditionFightView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_IF_ExpeditionFightView : GameView
    {
        public const string VIEW_NAME = "UI_IF_ExpeditionFight";

        public UI_IF_ExpeditionFightView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_btn_rank_PolygonImage;
		[HideInInspector] public GameButton m_btn_rank_GameButton;

		[HideInInspector] public PolygonImage m_btn_rule_PolygonImage;
		[HideInInspector] public GameButton m_btn_rule_GameButton;

		[HideInInspector] public PolygonImage m_btn_shop_PolygonImage;
		[HideInInspector] public GameButton m_btn_shop_GameButton;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public PolygonImage m_btn_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_box_GameButton;

		[HideInInspector] public PolygonImage m_img_box_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_box_GrayChildrens;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_UI_Model_AnimationBox;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public Outline m_lbl_time_Outline;

		[HideInInspector] public Animator m_pl_buttom_Animator;
		[HideInInspector] public CanvasGroup m_pl_buttom_CanvasGroup;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_challage;
		[HideInInspector] public GridLayoutGroup m_pl_challageNumbg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_challageNumbg_ArabLayoutCompment;
		[HideInInspector] public GameButton m_pl_challageNumbg_GameButton;
		[HideInInspector] public PolygonImage m_pl_challageNumbg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blackman0_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blackman1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blackman2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blackman3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blackman4_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_challageNum_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_challageNum_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_blueman4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blueman3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blueman2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blueman1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blueman0_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_reward_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_reward1_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_firstBg_PolygonImage;
		[HideInInspector] public LayoutElement m_img_firstBg_LayoutElement;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet3;
		[HideInInspector] public PolygonImage m_img_firsticon_PolygonImage;
		[HideInInspector] public LayoutElement m_img_firsticon_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_img_firsticon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_rewardget1_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_rewardget1_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_lbl_rewardget1_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_reward2_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet4;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet5;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet6;
		[HideInInspector] public LanguageText m_lbl_rewardget2_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_rewardget2_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_lbl_rewardget2_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_level_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_starbg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_starbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_starNormal1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starNormal2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starNormal3_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_star_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_star_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_starHighlight1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starHighlight2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starHighlight3_PolygonImage;

		[HideInInspector] public Animator m_pl_coin_Animator;
		[HideInInspector] public CanvasGroup m_pl_coin_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_coin_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_coin_GridLayoutGroup;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public LayoutElement m_img_frame_LayoutElement;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_medal;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_gem;
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;


        private void UIFinder()
        {
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view");
			m_btn_rank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_rank");
			m_btn_rank_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_rank");

			m_btn_rule_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_rule");
			m_btn_rule_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_rule");

			m_btn_shop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_shop");
			m_btn_shop_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_shop");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_shop/UI_Common_Redpoint"));
			m_btn_box_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_box");
			m_btn_box_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_box");

			m_img_box_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_box/img_box");
			m_img_box_GrayChildrens = FindUI<GrayChildrens>(vb.transform ,"rect/btn_box/img_box");

			m_UI_Model_AnimationBox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_box/UI_Model_AnimationBox"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/btn_box/lbl_time");
			m_lbl_time_Outline = FindUI<Outline>(vb.transform ,"rect/btn_box/lbl_time");

			m_pl_buttom_Animator = FindUI<Animator>(vb.transform ,"rect/pl_buttom");
			m_pl_buttom_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/pl_buttom");

			m_btn_challage = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/btn_challage"));
			m_pl_challageNumbg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_challageNumbg");
			m_pl_challageNumbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_challageNumbg");
			m_pl_challageNumbg_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_buttom/pl_challageNumbg");
			m_pl_challageNumbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNumbg");

			m_img_blackman0_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNumbg/img_blackman0");

			m_img_blackman1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNumbg/img_blackman1");

			m_img_blackman2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNumbg/img_blackman2");

			m_img_blackman3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNumbg/img_blackman3");

			m_img_blackman4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNumbg/img_blackman4");

			m_pl_challageNum_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_challageNum");
			m_pl_challageNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_challageNum");

			m_img_blueman4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNum/img_blueman4");

			m_img_blueman3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNum/img_blueman3");

			m_img_blueman2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNum/img_blueman2");

			m_img_blueman1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNum/img_blueman1");

			m_img_blueman0_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_challageNum/img_blueman0");

			m_pl_reward_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_reward");
			m_pl_reward_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_reward");

			m_pl_reward1_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1");
			m_pl_reward1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1");

			m_img_firstBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/img_firstBg");
			m_img_firstBg_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/img_firstBg");

			m_UI_Model_RewardGet1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/UI_Model_RewardGet1"));
			m_UI_Model_RewardGet2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/UI_Model_RewardGet2"));
			m_UI_Model_RewardGet3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/UI_Model_RewardGet3"));
			m_img_firsticon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/img_firsticon");
			m_img_firsticon_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/img_firsticon");
			m_img_firsticon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/img_firsticon");

			m_lbl_rewardget1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/lbl_rewardget1");
			m_lbl_rewardget1_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/lbl_rewardget1");
			m_lbl_rewardget1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward1/lbl_rewardget1");

			m_pl_reward2_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2");
			m_pl_reward2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2");

			m_UI_Model_RewardGet4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2/UI_Model_RewardGet4"));
			m_UI_Model_RewardGet5 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2/UI_Model_RewardGet5"));
			m_UI_Model_RewardGet6 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2/UI_Model_RewardGet6"));
			m_lbl_rewardget2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2/lbl_rewardget2");
			m_lbl_rewardget2_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2/lbl_rewardget2");
			m_lbl_rewardget2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_reward/pl_reward2/lbl_rewardget2");

			m_pl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_level");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_buttom/pl_level/lbl_level");
			m_lbl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_level/lbl_level");

			m_pl_starbg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_level/pl_starbg");
			m_pl_starbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_level/pl_starbg");

			m_img_starNormal1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_level/pl_starbg/img_starNormal1");

			m_img_starNormal2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_level/pl_starbg/img_starNormal2");

			m_img_starNormal3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_level/pl_starbg/img_starNormal3");

			m_pl_star_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_buttom/pl_level/pl_star");
			m_pl_star_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_buttom/pl_level/pl_star");

			m_img_starHighlight1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_level/pl_star/img_starHighlight1");

			m_img_starHighlight2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_level/pl_star/img_starHighlight2");

			m_img_starHighlight3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_buttom/pl_level/pl_star/img_starHighlight3");

			m_pl_coin_Animator = FindUI<Animator>(vb.transform ,"pl_coin");
			m_pl_coin_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_coin");
			m_pl_coin_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_coin");
			m_pl_coin_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_coin");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_coin/img_frame");
			m_img_frame_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_coin/img_frame");

			m_UI_medal = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"pl_coin/UI_medal"));
			m_UI_gem = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"pl_coin/UI_gem"));
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));

            UI_IF_ExpeditionFightMediator mt = new UI_IF_ExpeditionFightMediator(vb.gameObject);
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
