// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年11月2日
// Update Time         :    2020年11月2日
// Class Description   :    UI_Pop_ArmySelectView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Pop_ArmySelectView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_ArmySelect";

        public UI_Pop_ArmySelectView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public VerticalLayoutGroup m_pl_layout_VerticalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_layout_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ArmyQueueNew_SubView m_UI_Item_ArmyQueueNew;
		[HideInInspector] public ScrollRect m_list_Troops_ScrollRect;
		[HideInInspector] public PolygonImage m_list_Troops_PolygonImage;
		[HideInInspector] public ListView m_list_Troops_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public GameToggle m_ck_selectAll_GameToggle;
		[HideInInspector] public LayoutElement m_ck_selectAll_LayoutElement;

		[HideInInspector] public ArabLayoutCompment m_pl_NewArmy_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_NewArmy_Animator;
		[HideInInspector] public UIDefaultValue m_pl_NewArmy_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_mist_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_mist_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideR_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Model_StandardButton_Blue;
		[HideInInspector] public UI_Tag_PopAnime_QueueShow_SubView m_UI_Tag_PopAnime_QueueShow;
		[HideInInspector] public ArabLayoutCompment m_pl_Go_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_Go_Animator;
		[HideInInspector] public UIDefaultValue m_pl_Go_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_go_arrowSideR_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_go_arrowSideR_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_pos_Animator;

		[HideInInspector] public RectTransform m_pl_page1;
		[HideInInspector] public VerticalLayoutGroup m_img_mist_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public LanguageText m_lbl_weight_LanguageText;

		[HideInInspector] public PolygonImage m_img_CostAp_PolygonImage;
		[HideInInspector] public VerticalLayoutGroup m_img_CostAp_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_countCostAp_LanguageText;

		[HideInInspector] public PolygonImage m_btn_ApQuestion_PolygonImage;
		[HideInInspector] public GameButton m_btn_ApQuestion_GameButton;

		[HideInInspector] public LanguageText m_lbl_valCostAp_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_valCostAp_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_pl_warnTip_PolygonImage;
		[HideInInspector] public LayoutElement m_pl_warnTip_LayoutElement;

		[HideInInspector] public LanguageText m_lbl_warnTip_LanguageText;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;

		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_UI_Model_StandardButton_Yellow;
		[HideInInspector] public ScrollRect m_sv_page2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_page2_PolygonImage;
		[HideInInspector] public ListView m_sv_page2_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_page2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_page2_PolygonImage;
		[HideInInspector] public Mask m_v_page2_Mask;

		[HideInInspector] public RectTransform m_c_page2;
		[HideInInspector] public RectTransform m_pl_armyInfo;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_stars_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_stars_ArabLayoutCompment;

		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar1;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar2;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar3;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar4;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar5;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar6;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill1;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill2;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill3;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill4;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill5;
		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_allarmys_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_allarmys_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_infoback_PolygonImage;
		[HideInInspector] public GameButton m_btn_infoback_GameButton;

		[HideInInspector] public ArabLayoutCompment m_pl_Dead_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_Dead_Animator;
		[HideInInspector] public UIDefaultValue m_pl_Dead_UIDefaultValue;

		[HideInInspector] public UI_Pop_BuildUpGo_SubView m_UI_Pop_BuildUpGo;


        private void UIFinder()
        {
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_title");

			m_pl_layout_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"bg/pl_layout");
			m_pl_layout_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/pl_layout");

			m_UI_Item_ArmyQueueNew = new UI_Item_ArmyQueueNew_SubView(FindUI<RectTransform>(vb.transform ,"bg/pl_layout/UI_Item_ArmyQueueNew"));
			m_list_Troops_ScrollRect = FindUI<ScrollRect>(vb.transform ,"bg/pl_layout/list_Troops");
			m_list_Troops_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/pl_layout/list_Troops");
			m_list_Troops_ListView = FindUI<ListView>(vb.transform ,"bg/pl_layout/list_Troops");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/pl_layout/list_Troops/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"bg/pl_layout/list_Troops/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"bg/pl_layout/list_Troops/v_list_view/c_list_view");
			m_ck_selectAll_GameToggle = FindUI<GameToggle>(vb.transform ,"bg/ck_selectAll");
			m_ck_selectAll_LayoutElement = FindUI<LayoutElement>(vb.transform ,"bg/ck_selectAll");

			m_pl_NewArmy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_NewArmy");
			m_pl_NewArmy_Animator = FindUI<Animator>(vb.transform ,"pl_NewArmy");
			m_pl_NewArmy_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_NewArmy");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_NewArmy/img_bg");

			m_img_mist_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_NewArmy/img_bg/img_mist");
			m_img_mist_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_NewArmy/img_bg/img_mist");

			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_NewArmy/img_bg/img_mist/lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_NewArmy/img_bg/img_mist/lbl_state");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_NewArmy/img_bg/img_arrowSideR");
			m_img_arrowSideR_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_NewArmy/img_bg/img_arrowSideR");

			m_UI_Model_StandardButton_Blue = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_NewArmy/img_bg/UI_Model_StandardButton_Blue"));
			m_UI_Tag_PopAnime_QueueShow = new UI_Tag_PopAnime_QueueShow_SubView(FindUI<RectTransform>(vb.transform ,"pl_NewArmy/UI_Tag_PopAnime_QueueShow"));
			m_pl_Go_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Go");
			m_pl_Go_Animator = FindUI<Animator>(vb.transform ,"pl_Go");
			m_pl_Go_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_Go");

			m_img_go_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/img_go_arrowSideR");
			m_img_go_arrowSideR_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Go/img_bg/img_go_arrowSideR");

			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"pl_Go/img_bg/mask/pl_pos");

			m_pl_page1 = FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1");
			m_img_mist_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_mist");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_mist/lbl_count");

			m_lbl_weight_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_mist/lbl_weight");

			m_img_CostAp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp");
			m_img_CostAp_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp");

			m_lbl_countCostAp_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/lbl_countCostAp");

			m_btn_ApQuestion_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/title/btn_ApQuestion");
			m_btn_ApQuestion_GameButton = FindUI<GameButton>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/title/btn_ApQuestion");

			m_lbl_valCostAp_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/lbl_valCostAp");
			m_lbl_valCostAp_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/lbl_valCostAp");

			m_pl_warnTip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/pl_warnTip");
			m_pl_warnTip_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/pl_warnTip");

			m_lbl_warnTip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/img_CostAp/pl_warnTip/lbl_warnTip");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/btn_info");

			m_UI_Model_StandardButton_Yellow = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/pl_page1/UI_Model_StandardButton_Yellow"));
			m_sv_page2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2");
			m_sv_page2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2");
			m_sv_page2_ListView = FindUI<ListView>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2");
			m_sv_page2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2");

			m_v_page2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2");
			m_v_page2_Mask = FindUI<Mask>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2");

			m_c_page2 = FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2");
			m_pl_armyInfo = FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo");
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/lbl_name");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/lbl_level");
			m_lbl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/lbl_level");

			m_pl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars");
			m_pl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars");

			m_UI_Model_HeadStar1 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars/UI_Model_HeadStar1"));
			m_UI_Model_HeadStar2 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars/UI_Model_HeadStar2"));
			m_UI_Model_HeadStar3 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars/UI_Model_HeadStar3"));
			m_UI_Model_HeadStar4 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars/UI_Model_HeadStar4"));
			m_UI_Model_HeadStar5 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars/UI_Model_HeadStar5"));
			m_UI_Model_HeadStar6 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectlevel/pl_stars/UI_Model_HeadStar6"));
			m_UI_Item_CaptainSkill1 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectSkill/UI_Item_CaptainSkill1"));
			m_UI_Item_CaptainSkill2 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectSkill/UI_Item_CaptainSkill2"));
			m_UI_Item_CaptainSkill3 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectSkill/UI_Item_CaptainSkill3"));
			m_UI_Item_CaptainSkill4 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectSkill/UI_Item_CaptainSkill4"));
			m_UI_Item_CaptainSkill5 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/bg/rectSkill/UI_Item_CaptainSkill5"));
			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/lbl_armyCount");

			m_pl_allarmys_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/pl_allarmys");
			m_pl_allarmys_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/v_page2/c_page2/pl_armyInfo/pl_allarmys");

			m_btn_infoback_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/btn_infoback");
			m_btn_infoback_GameButton = FindUI<GameButton>(vb.transform ,"pl_Go/img_bg/mask/pl_pos/sv_page2/btn_infoback");

			m_pl_Dead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Dead");
			m_pl_Dead_Animator = FindUI<Animator>(vb.transform ,"pl_Dead");
			m_pl_Dead_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_Dead");

			m_UI_Pop_BuildUpGo = new UI_Pop_BuildUpGo_SubView(FindUI<RectTransform>(vb.transform ,"UI_Pop_BuildUpGo"));

            UI_Pop_ArmySelectMediator mt = new UI_Pop_ArmySelectMediator(vb.gameObject);
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
