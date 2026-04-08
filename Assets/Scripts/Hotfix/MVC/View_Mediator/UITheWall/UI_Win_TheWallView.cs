// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月29日
// Update Time         :    2020年6月29日
// Class Description   :    UI_Win_TheWallView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_TheWallView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TheWall";

        public UI_Win_TheWallView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public Animator m_pl_rect_Animator;
		[HideInInspector] public RectMask2D m_pl_rect_RectMask2D;

		[HideInInspector] public CanvasGroup m_pl_pagedesc_CanvasGroup;

		[HideInInspector] public ScrollRect m_sv_scroll_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_scroll_view_PolygonImage;
		[HideInInspector] public ScrollView m_sv_scroll_view_ScrollView;

		[HideInInspector] public PolygonImage m_v_scroll_view_PolygonImage;
		[HideInInspector] public Mask m_v_scroll_view_Mask;

		[HideInInspector] public ContentSizeFitter m_c_scroll_view_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_walldesct_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_walldesct_ContentSizeFitter;

		[HideInInspector] public CanvasGroup m_pl_page0_CanvasGroup;

		[HideInInspector] public GridLayoutGroup m_pl_skill1bg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_skill1bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_skill1bg1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill1bg2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill1bg3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill1bg4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill1bg5_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_sklii1_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_sklii1_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill1_1;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill1_2;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill1_3;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill1_4;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill1_5;
		[HideInInspector] public GridLayoutGroup m_pl_skill2bg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_skill2bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_skill2bg1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill2bg2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill2bg3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill2bg4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_skill2bg5_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_sklii2_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_sklii2_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill2_1;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill2_2;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill2_3;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill2_4;
		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_img_skill2_5;
		[HideInInspector] public PolygonImage m_btn_hero_add1_PolygonImage;
		[HideInInspector] public GameButton m_btn_hero_add1_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_hero_add1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_circle_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_CaptainHead0;
		[HideInInspector] public PolygonImage m_btn_hero_add2_PolygonImage;
		[HideInInspector] public GameButton m_btn_hero_add2_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_hero_add2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_CaptainHead1;
		[HideInInspector] public LanguageText m_lbl_mainName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mainName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_subName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_subName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_power_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_powericon_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_wallPoint_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_walltag_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_walltag_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_walldesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_walldesc_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_wallhpbar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_wallhpbar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_Fill_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_wallstate_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_wallstate_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_wallhp_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_wallhp_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_wallicon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_wallicon_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_fire;
		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_repair;
		[HideInInspector] public UI_Item_CaptainList_SubView m_UI_Item_CaptainList;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_rect_Animator = FindUI<Animator>(vb.transform ,"pl_rect");
			m_pl_rect_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_rect");

			m_pl_pagedesc_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_rect/pl_pagedesc");

			m_sv_scroll_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view");
			m_sv_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view");
			m_sv_scroll_view_ScrollView = FindUI<ScrollView>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view");

			m_v_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view/v_scroll_view");
			m_v_scroll_view_Mask = FindUI<Mask>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view/v_scroll_view");

			m_c_scroll_view_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view/v_scroll_view/c_scroll_view");

			m_lbl_walldesct_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view/v_scroll_view/c_scroll_view/lbl_walldesct");
			m_lbl_walldesct_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_rect/pl_pagedesc/sv_scroll_view/v_scroll_view/c_scroll_view/lbl_walldesct");

			m_pl_page0_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_rect/pl_page0");

			m_pl_skill1bg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg");
			m_pl_skill1bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg");

			m_img_skill1bg1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg/img_skill1bg1");

			m_img_skill1bg2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg/img_skill1bg2");

			m_img_skill1bg3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg/img_skill1bg3");

			m_img_skill1bg4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg/img_skill1bg4");

			m_img_skill1bg5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill1bg/img_skill1bg5");

			m_pl_sklii1_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1");
			m_pl_sklii1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1");

			m_img_skill1_1 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1/img_skill1_1"));
			m_img_skill1_2 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1/img_skill1_2"));
			m_img_skill1_3 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1/img_skill1_3"));
			m_img_skill1_4 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1/img_skill1_4"));
			m_img_skill1_5 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii1/img_skill1_5"));
			m_pl_skill2bg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg");
			m_pl_skill2bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg");

			m_img_skill2bg1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg/img_skill2bg1");

			m_img_skill2bg2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg/img_skill2bg2");

			m_img_skill2bg3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg/img_skill2bg3");

			m_img_skill2bg4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg/img_skill2bg4");

			m_img_skill2bg5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_skill2bg/img_skill2bg5");

			m_pl_sklii2_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2");
			m_pl_sklii2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2");

			m_img_skill2_1 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2/img_skill2_1"));
			m_img_skill2_2 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2/img_skill2_2"));
			m_img_skill2_3 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2/img_skill2_3"));
			m_img_skill2_4 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2/img_skill2_4"));
			m_img_skill2_5 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/DoubleSkills/pl_sklii2/img_skill2_5"));
			m_btn_hero_add1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add1");
			m_btn_hero_add1_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add1");
			m_btn_hero_add1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add1");

			m_img_circle_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add1/img_circle");

			m_UI_CaptainHead0 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add1/UI_CaptainHead0"));
			m_btn_hero_add2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add2");
			m_btn_hero_add2_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add2");
			m_btn_hero_add2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add2");

			m_UI_CaptainHead1 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/btn_hero_add2/UI_CaptainHead1"));
			m_lbl_mainName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/lbl_mainName");
			m_lbl_mainName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/lbl_mainName");

			m_lbl_subName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/lbl_subName");
			m_lbl_subName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/top/pl/captain/lbl_subName");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/top/pl/lbl_power");
			m_lbl_power_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_rect/pl_page0/top/pl/lbl_power");

			m_img_powericon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/top/pl/lbl_power/img_powericon");

			m_pl_wallPoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/pl_wallPoint");

			m_lbl_walltag_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_walltag");
			m_lbl_walltag_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_walltag");

			m_lbl_walldesc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_walldesc");
			m_lbl_walldesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_walldesc");

			m_pb_wallhpbar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_rect/pl_page0/wall/data/pb_wallhpbar");
			m_pb_wallhpbar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/pb_wallhpbar");

			m_img_Fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/wall/data/pb_wallhpbar/Fill Area/img_Fill");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/wall/data/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/pl_page0/wall/data/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/btn_info");

			m_lbl_wallstate_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_wallstate");
			m_lbl_wallstate_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_wallstate");

			m_lbl_wallhp_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_wallhp");
			m_lbl_wallhp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/lbl_wallhp");

			m_img_wallicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_page0/wall/data/img_wallicon");
			m_img_wallicon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_page0/wall/data/img_wallicon");

			m_btn_fire = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/wall/data/btns/btn_fire"));
			m_btn_repair = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_page0/wall/data/btns/btn_repair"));
			m_UI_Item_CaptainList = new UI_Item_CaptainList_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_CaptainList"));

            UI_Win_TheWallMediator mt = new UI_Win_TheWallMediator(vb.gameObject);
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
