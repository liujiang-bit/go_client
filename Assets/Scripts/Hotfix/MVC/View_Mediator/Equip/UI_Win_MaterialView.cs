// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年11月3日
// Update Time         :    2020年11月3日
// Class Description   :    UI_Win_MaterialView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_MaterialView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Material";

        public UI_Win_MaterialView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_produc_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_produc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_produc_PolygonImage;
		[HideInInspector] public ListView m_sv_produc_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_produc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_produc_PolygonImage;
		[HideInInspector] public Mask m_v_produc_Mask;

		[HideInInspector] public RectTransform m_c_produc;
		[HideInInspector] public ArabLayoutCompment m_pl_mix_ArabLayoutCompment;
		[HideInInspector] public ToggleGroup m_pl_mix_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_material_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_material_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_picture_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_picture_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public ScrollRect m_sv_mix_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_mix_PolygonImage;
		[HideInInspector] public ListView m_sv_mix_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_mix_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_mix_PolygonImage;
		[HideInInspector] public Mask m_v_mix_Mask;

		[HideInInspector] public RectTransform m_c_mix;
		[HideInInspector] public ArabLayoutCompment m_pl_resolve_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_resolve_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_resolve_PolygonImage;
		[HideInInspector] public ListView m_sv_resolve_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_resolve_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_resolve_PolygonImage;
		[HideInInspector] public Mask m_v_resolve_Mask;

		[HideInInspector] public RectTransform m_c_resolve;
		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_right_Animator;
		[HideInInspector] public CanvasGroup m_pl_right_CanvasGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_contentTitle_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_product_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_product_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_other_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_produc1;
		[HideInInspector] public RectTransform m_pl_produc1_effect;
		[HideInInspector] public RectTransform m_pl_queue1;
		[HideInInspector] public UI_Item_BlacksmithQueue_SubView m_UI_BlacksmithQueue1;
		[HideInInspector] public GridLayoutGroup m_pl_queue2_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_queue2_ArabLayoutCompment;

		[HideInInspector] public UI_Item_BlacksmithQueue_SubView m_UI_BlacksmithQueue2;
		[HideInInspector] public UI_Item_BlacksmithQueue_SubView m_UI_BlacksmithQueue3;
		[HideInInspector] public UI_Item_BlacksmithQueue_SubView m_UI_BlacksmithQueue4;
		[HideInInspector] public UI_Item_BlacksmithQueue_SubView m_UI_BlacksmithQueue5;
		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_mix1;
		[HideInInspector] public RectTransform m_pl_mix1_effect1;
		[HideInInspector] public RectTransform m_pl_mix_effect;
		[HideInInspector] public ArabLayoutCompment m_pl_mix2_effect1_ArabLayoutCompment;

		[HideInInspector] public UI_10034_SubView m_UI_10034;
		[HideInInspector] public RectTransform m_pl_mix_set;
		[HideInInspector] public UI_Model_Item_SubView m_img_mix;
		[HideInInspector] public GridLayoutGroup m_pl_mix_GridLayoutGroup;

		[HideInInspector] public UI_Model_Item_SubView m_img_mixItem;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_mix;
		[HideInInspector] public LanguageText m_lbl_mix_quality_LanguageText;

		[HideInInspector] public RectTransform m_pl_resolve1;
		[HideInInspector] public RectTransform m_pl_resolve1_effect;
		[HideInInspector] public GridLayoutGroup m_pl_mix_resolve_GridLayoutGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_break_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_break_GridLayoutGroup;

		[HideInInspector] public UI_Model_Item_SubView m_img_resolveItem;
		[HideInInspector] public UI_Model_Item_SubView m_img_resolve;
		[HideInInspector] public LanguageText m_lbl_resolve_quality_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_resolve;
		[HideInInspector] public ArabLayoutCompment m_pl_quick_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_quick1;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_quick1;
		[HideInInspector] public RectTransform m_pl_quick2;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_quick2;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_quick3;
		[HideInInspector] public ArabLayoutCompment m_pl_tips_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_tips_Animator;
		[HideInInspector] public CanvasGroup m_pl_tips_CanvasGroup;

		[HideInInspector] public ScrollRect m_sv_tips_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_tips_PolygonImage;
		[HideInInspector] public ListView m_sv_tips_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_tips_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_tips_PolygonImage;
		[HideInInspector] public Mask m_v_tips_Mask;

		[HideInInspector] public LanguageText m_lbl_tips_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_tips_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_tips_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left");

			m_pl_produc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_produc");

			m_sv_produc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_left/pl_produc/sv_produc");
			m_sv_produc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/pl_produc/sv_produc");
			m_sv_produc_ListView = FindUI<ListView>(vb.transform ,"rect/pl_left/pl_produc/sv_produc");
			m_sv_produc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_produc/sv_produc");

			m_v_produc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/pl_produc/sv_produc/v_produc");
			m_v_produc_Mask = FindUI<Mask>(vb.transform ,"rect/pl_left/pl_produc/sv_produc/v_produc");

			m_c_produc = FindUI<RectTransform>(vb.transform ,"rect/pl_left/pl_produc/sv_produc/v_produc/c_produc");
			m_pl_mix_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_mix");
			m_pl_mix_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"rect/pl_left/pl_mix");

			m_ck_material_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_left/pl_mix/ck_material");
			m_ck_material_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_mix/ck_material");

			m_ck_picture_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_left/pl_mix/ck_picture");
			m_ck_picture_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_mix/ck_picture");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_left/pl_mix/ck_picture/UI_Common_Redpoint"));
			m_sv_mix_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_left/pl_mix/sv_mix");
			m_sv_mix_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/pl_mix/sv_mix");
			m_sv_mix_ListView = FindUI<ListView>(vb.transform ,"rect/pl_left/pl_mix/sv_mix");
			m_sv_mix_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_mix/sv_mix");

			m_v_mix_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/pl_mix/sv_mix/v_mix");
			m_v_mix_Mask = FindUI<Mask>(vb.transform ,"rect/pl_left/pl_mix/sv_mix/v_mix");

			m_c_mix = FindUI<RectTransform>(vb.transform ,"rect/pl_left/pl_mix/sv_mix/v_mix/c_mix");
			m_pl_resolve_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_resolve");

			m_sv_resolve_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve");
			m_sv_resolve_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve");
			m_sv_resolve_ListView = FindUI<ListView>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve");
			m_sv_resolve_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve");

			m_v_resolve_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve/v_resolve");
			m_v_resolve_Mask = FindUI<Mask>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve/v_resolve");

			m_c_resolve = FindUI<RectTransform>(vb.transform ,"rect/pl_left/pl_resolve/sv_resolve/v_resolve/c_resolve");
			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right");
			m_pl_right_Animator = FindUI<Animator>(vb.transform ,"rect/pl_right");
			m_pl_right_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/pl_right");

			m_pl_contentTitle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_contentTitle");

			m_lbl_product_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/pl_contentTitle/lbl_product");
			m_lbl_product_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_contentTitle/lbl_product");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_right/pl_contentTitle/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_right/pl_contentTitle/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_contentTitle/btn_info");

			m_pl_other_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_other");

			m_pl_produc1 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1");
			m_pl_produc1_effect = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_produc1_effect");
			m_pl_queue1 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue1");
			m_UI_BlacksmithQueue1 = new UI_Item_BlacksmithQueue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue1/UI_BlacksmithQueue1"));
			m_pl_queue2_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue2");
			m_pl_queue2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue2");

			m_UI_BlacksmithQueue2 = new UI_Item_BlacksmithQueue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue2/UI_BlacksmithQueue2"));
			m_UI_BlacksmithQueue3 = new UI_Item_BlacksmithQueue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue2/UI_BlacksmithQueue3"));
			m_UI_BlacksmithQueue4 = new UI_Item_BlacksmithQueue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue2/UI_BlacksmithQueue4"));
			m_UI_BlacksmithQueue5 = new UI_Item_BlacksmithQueue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pl_queue2/UI_BlacksmithQueue5"));
			m_pb_bar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pb_bar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pb_bar/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_other/pl_produc1/pb_bar/lbl_time");

			m_pl_mix1 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1");
			m_pl_mix1_effect1 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix1_effect1");
			m_pl_mix_effect = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix_effect");
			m_pl_mix2_effect1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix2_effect1");

			m_UI_10034 = new UI_10034_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix2_effect1/UI_10034"));
			m_pl_mix_set = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix_set");
			m_img_mix = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix_set/img_mix"));
			m_pl_mix_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix");

			m_img_mixItem = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/pl_mix/img_mixItem"));
			m_btn_mix = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/btn_mix"));
			m_lbl_mix_quality_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/pl_other/pl_mix1/lbl_mix_quality");

			m_pl_resolve1 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1");
			m_pl_resolve1_effect = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/pl_resolve1_effect");
			m_pl_mix_resolve_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/pl_mix_resolve");

			m_pl_break_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/pl_break");
			m_pl_break_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/pl_break");

			m_img_resolveItem = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/pl_break/img_resolveItem"));
			m_img_resolve = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/img_resolve"));
			m_lbl_resolve_quality_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/lbl_resolve_quality");

			m_btn_resolve = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_other/pl_resolve1/btn_resolve"));
			m_pl_quick_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_quick");

			m_pl_quick1 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_quick/pl_quick1");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_right/pl_quick/pl_quick1/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/pl_quick/pl_quick1/img_bg");

			m_btn_quick1 = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_quick/pl_quick1/btn_quick1"));
			m_pl_quick2 = FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_quick/pl_quick2");
			m_btn_quick2 = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_quick/pl_quick2/btn_quick2"));
			m_btn_quick3 = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/pl_quick/pl_quick2/btn_quick3"));
			m_pl_tips_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_tips");
			m_pl_tips_Animator = FindUI<Animator>(vb.transform ,"rect/pl_tips");
			m_pl_tips_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/pl_tips");

			m_sv_tips_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_tips/sv_tips");
			m_sv_tips_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_tips/sv_tips");
			m_sv_tips_ListView = FindUI<ListView>(vb.transform ,"rect/pl_tips/sv_tips");
			m_sv_tips_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_tips/sv_tips");

			m_v_tips_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_tips/sv_tips/v_tips");
			m_v_tips_Mask = FindUI<Mask>(vb.transform ,"rect/pl_tips/sv_tips/v_tips");

			m_lbl_tips_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_tips/sv_tips/v_tips/lbl_tips");
			m_lbl_tips_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_tips/sv_tips/v_tips/lbl_tips");
			m_lbl_tips_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_tips/sv_tips/v_tips/lbl_tips");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_tips/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_tips/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_tips/btn_back");


            UI_Win_MaterialMediator mt = new UI_Win_MaterialMediator(vb.gameObject);
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
