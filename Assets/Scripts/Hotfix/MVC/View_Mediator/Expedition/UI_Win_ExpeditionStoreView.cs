// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Saturday, 24 October 2020
// Update Time         :    Saturday, 24 October 2020
// Class Description   :    UI_Win_ExpeditionStoreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_ExpeditionStoreView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ExpeditionStore";

        public UI_Win_ExpeditionStoreView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_coin_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_coin_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_coin_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_UI_Item_coin_GridLayoutGroup;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public LayoutElement m_img_frame_LayoutElement;

		[HideInInspector] public UI_Model_Resources_SubView m_pl_gem;
		[HideInInspector] public UI_Model_Resources_SubView m_pl_conquerorMedal;
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public ArabLayoutCompment m_pl_char_ArabLayoutCompment;

		[HideInInspector] public UI_Effect_BuildShow_SubView m_UI_Effect_BuildShow;
		[HideInInspector] public SkeletonGraphic m_pl_horeSpineBefore_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_horeSpineBefore_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_horeSpineBefore_Animator;

		[HideInInspector] public SkeletonGraphic m_pl_horeSpineAfter_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_horeSpineAfter_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_horeSpineAfter_Animator;

		[HideInInspector] public PolygonImage m_btn_left_PolygonImage;
		[HideInInspector] public GameButton m_btn_left_GameButton;

		[HideInInspector] public PolygonImage m_btn_right_PolygonImage;
		[HideInInspector] public GameButton m_btn_right_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;

		[HideInInspector] public PolygonImage m_bl_et_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_bl_et_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_et_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_et_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_mes_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_item_effect_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_effect_ArabLayoutCompment;
		[HideInInspector] public LayoutElement m_pl_item_effect_LayoutElement;

		[HideInInspector] public RectTransform m_pl_item_effect1;
		[HideInInspector] public RectTransform m_pl_item_effect2;
		[HideInInspector] public RectTransform m_pl_item_effect3;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_commodity1;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_commodity2;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_commodity3;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_storeItem1;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_storeItem2;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_storeItem3;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_storeItem4;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_storeItem5;
		[HideInInspector] public UI_Item_ExpeditionStoreItem_SubView m_pl_storeItem6;
		[HideInInspector] public LanguageText m_lbl_refreshNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_refreshNum_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_refresh_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_reflash_PolygonImage;
		[HideInInspector] public GameButton m_btn_reflash_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_reflash_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_Resources;
		[HideInInspector] public LanguageText m_lbl_free_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_free_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Item_coin_Animator = FindUI<Animator>(vb.transform ,"UI_Item_coin");
			m_UI_Item_coin_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"UI_Item_coin");
			m_UI_Item_coin_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Item_coin");
			m_UI_Item_coin_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"UI_Item_coin");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Item_coin/img_frame");
			m_img_frame_LayoutElement = FindUI<LayoutElement>(vb.transform ,"UI_Item_coin/img_frame");

			m_pl_gem = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_coin/pl_gem"));
			m_pl_conquerorMedal = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_coin/pl_conquerorMedal"));
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char");

			m_UI_Effect_BuildShow = new UI_Effect_BuildShow_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_char/UI_Effect_BuildShow"));
			m_pl_horeSpineBefore_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"rect/pl_char/pl_horeSpineBefore");
			m_pl_horeSpineBefore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char/pl_horeSpineBefore");
			m_pl_horeSpineBefore_Animator = FindUI<Animator>(vb.transform ,"rect/pl_char/pl_horeSpineBefore");

			m_pl_horeSpineAfter_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"rect/pl_char/pl_horeSpineAfter");
			m_pl_horeSpineAfter_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char/pl_horeSpineAfter");
			m_pl_horeSpineAfter_Animator = FindUI<Animator>(vb.transform ,"rect/pl_char/pl_horeSpineAfter");

			m_btn_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_char/btn_left");
			m_btn_left_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_char/btn_left");

			m_btn_right_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_char/btn_right");
			m_btn_right_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_char/btn_right");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_char/lbl_name");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_char/lbl_time");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_char/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_char/btn_info");

			m_bl_et_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_char/bl_et");
			m_bl_et_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char/bl_et");

			m_lbl_et_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_char/lbl_et");
			m_lbl_et_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char/lbl_et");

			m_pl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes");

			m_pl_item_effect_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_mes/top/pl_item_effect");
			m_pl_item_effect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/top/pl_item_effect");
			m_pl_item_effect_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_mes/top/pl_item_effect");

			m_pl_item_effect1 = FindUI<RectTransform>(vb.transform ,"rect/pl_mes/top/pl_item_effect/pl_item_effect1");
			m_pl_item_effect2 = FindUI<RectTransform>(vb.transform ,"rect/pl_mes/top/pl_item_effect/pl_item_effect2");
			m_pl_item_effect3 = FindUI<RectTransform>(vb.transform ,"rect/pl_mes/top/pl_item_effect/pl_item_effect3");
			m_pl_commodity1 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/top/pl_commodity1"));
			m_pl_commodity2 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/top/pl_commodity2"));
			m_pl_commodity3 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/top/pl_commodity3"));
			m_pl_storeItem1 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/down/pl_storeItem1"));
			m_pl_storeItem2 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/down/pl_storeItem2"));
			m_pl_storeItem3 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/down/pl_storeItem3"));
			m_pl_storeItem4 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/down/pl_storeItem4"));
			m_pl_storeItem5 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/down/pl_storeItem5"));
			m_pl_storeItem6 = new UI_Item_ExpeditionStoreItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/down/pl_storeItem6"));
			m_lbl_refreshNum_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_mes/lbl_refreshNum");
			m_lbl_refreshNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/lbl_refreshNum");

			m_pl_refresh_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/pl_refresh");

			m_btn_reflash_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_mes/pl_refresh/btn_reflash");
			m_btn_reflash_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_mes/pl_refresh/btn_reflash");
			m_btn_reflash_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/pl_refresh/btn_reflash");

			m_UI_Model_Resources = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_refresh/UI_Model_Resources"));
			m_lbl_free_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_mes/pl_refresh/lbl_free");
			m_lbl_free_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/pl_refresh/lbl_free");


            UI_Win_ExpeditionStoreMediator mt = new UI_Win_ExpeditionStoreMediator(vb.gameObject);
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
