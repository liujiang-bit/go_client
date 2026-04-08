// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, 04 November 2020
// Update Time         :    Wednesday, 04 November 2020
// Class Description   :    UI_Win_EquipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_EquipView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Equip";

        public UI_Win_EquipView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_view_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_equiptype1_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_equiptype1_ArabLayoutCompment;
		[HideInInspector] public ToggleGroup m_pl_equiptype1_ToggleGroup;

		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType0;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType1;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType2;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType3;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType4;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType5;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType6;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType7;
		[HideInInspector] public UI_Item_EquipType_SubView m_UI_Item_EquipType8;
		[HideInInspector] public PolygonImage m_btn_arr_PolygonImage;
		[HideInInspector] public GameButton m_btn_arr_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_arr_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_arrType_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_arrType_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_empty_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_empty_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_listType_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_listType_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Toggle_SubView m_UI_Common_Toggle;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_dec_ArabLayoutCompment;
		[HideInInspector] public CanvasGroup m_pl_dec_CanvasGroup;
		[HideInInspector] public Animator m_pl_dec_Animator;

		[HideInInspector] public ScrollRect m_sv_des_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_des_PolygonImage;
		[HideInInspector] public ListView m_sv_des_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_des_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_describe_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_describe_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_describe_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public BtnAnimation m_btn_back_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_att_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_att_Animator;
		[HideInInspector] public CanvasGroup m_pl_att_CanvasGroup;

		[HideInInspector] public UI_Item_EquipAtt_SubView m_UI_Item_EquipAtt;
		[HideInInspector] public RectTransform m_pl_ues;
		[HideInInspector] public LanguageText m_lbl_use_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_use_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_btn_dec_PolygonImage;
		[HideInInspector] public GameButton m_btn_dec_GameButton;
		[HideInInspector] public BtnAnimation m_btn_dec_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_dec_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_mid;
		[HideInInspector] public RectTransform m_pl_equipmix_effect;
		[HideInInspector] public RectTransform m_pl_equipicon;
		[HideInInspector] public PolygonImage m_img_euqip_PolygonImage;
		[HideInInspector] public Animation m_img_euqip_Animation;

		[HideInInspector] public LanguageText m_lbl_materialTitle_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_materialQueue_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_materialQueue_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EquipMaterialList_SubView m_UI_Item_EquipMaterialList;
		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_make;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_quick;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_resolve;
		[HideInInspector] public ArabLayoutCompment m_pl_top_ArabLayoutCompment;
		[HideInInspector] public ToggleGroup m_pl_top_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_make_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_make_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_make_LanguageText;
		[HideInInspector] public Shadow m_lbl_make_Shadow;

		[HideInInspector] public GameToggle m_ck_resolve_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_resolve_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_resolve_LanguageText;
		[HideInInspector] public Shadow m_lbl_resolve_Shadow;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_Gold;
		[HideInInspector] public UI_Model_Interface_SubView m_btn_close;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_pl_effect = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_effect");
			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/pl_right/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"pl_mes/pl_right/sv_list_view");
			m_sv_list_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/sv_list_view");

			m_pl_equiptype1_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_mes/pl_right/pl_equiptype1");
			m_pl_equiptype1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_equiptype1");
			m_pl_equiptype1_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"pl_mes/pl_right/pl_equiptype1");

			m_UI_Item_EquipType0 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType0"));
			m_UI_Item_EquipType1 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType1"));
			m_UI_Item_EquipType2 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType2"));
			m_UI_Item_EquipType3 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType3"));
			m_UI_Item_EquipType4 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType4"));
			m_UI_Item_EquipType5 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType5"));
			m_UI_Item_EquipType6 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType6"));
			m_UI_Item_EquipType7 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType7"));
			m_UI_Item_EquipType8 = new UI_Item_EquipType_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_equiptype1/UI_Item_EquipType8"));
			m_btn_arr_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/btn_arr");
			m_btn_arr_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_right/btn_arr");
			m_btn_arr_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/btn_arr");

			m_lbl_arrType_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/btn_arr/lbl_arrType");
			m_lbl_arrType_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/btn_arr/lbl_arrType");

			m_lbl_empty_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_empty");
			m_lbl_empty_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/lbl_empty");

			m_lbl_listType_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_listType");
			m_lbl_listType_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/lbl_listType");

			m_UI_Common_Toggle = new UI_Common_Toggle_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/UI_Common_Toggle"));
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left");

			m_pl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_dec");
			m_pl_dec_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_mes/pl_left/pl_dec");
			m_pl_dec_Animator = FindUI<Animator>(vb.transform ,"pl_mes/pl_left/pl_dec");

			m_sv_des_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des");
			m_sv_des_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des");
			m_sv_des_ListView = FindUI<ListView>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des");
			m_sv_des_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des");

			m_lbl_describe_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des/v/c/lbl_describe");
			m_lbl_describe_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des/v/c/lbl_describe");
			m_lbl_describe_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/pl_left/pl_dec/sv_des/v/c/lbl_describe");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_dec/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_left/pl_dec/btn_back");
			m_btn_back_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/pl_left/pl_dec/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_dec/btn_back");

			m_pl_att_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_att");
			m_pl_att_Animator = FindUI<Animator>(vb.transform ,"pl_mes/pl_left/pl_att");
			m_pl_att_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_mes/pl_left/pl_att");

			m_UI_Item_EquipAtt = new UI_Item_EquipAtt_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/pl_att/sv/v/c/UI_Item_EquipAtt"));
			m_pl_ues = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/pl_att/pl_ues");
			m_lbl_use_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/pl_att/pl_ues/lbl_use");
			m_lbl_use_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_att/pl_ues/lbl_use");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/pl_att/pl_ues/UI_Model_CaptainHead"));
			m_btn_dec_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_att/btn_dec");
			m_btn_dec_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_left/pl_att/btn_dec");
			m_btn_dec_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/pl_left/pl_att/btn_dec");
			m_btn_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_att/btn_dec");

			m_pl_mid = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid");
			m_pl_equipmix_effect = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid/pl_equipmix_effect");
			m_pl_equipicon = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid/pl_equipicon");
			m_img_euqip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_mid/pl_equipicon/img_euqip");
			m_img_euqip_Animation = FindUI<Animation>(vb.transform ,"pl_mes/pl_mid/pl_equipicon/img_euqip");

			m_lbl_materialTitle_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_mid/lbl_materialTitle");

			m_pl_materialQueue_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_mes/pl_mid/pl_materialQueue");
			m_pl_materialQueue_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_mid/pl_materialQueue");

			m_UI_Item_EquipMaterialList = new UI_Item_EquipMaterialList_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid/pl_materialQueue/UI_Item_EquipMaterialList"));
			m_btn_make = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid/btn_make"));
			m_btn_quick = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid/btn_quick"));
			m_btn_resolve = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_mid/btn_resolve"));
			m_pl_top_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_top");
			m_pl_top_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"pl_top");

			m_ck_make_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_top/ck_make");
			m_ck_make_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_top/ck_make");

			m_lbl_make_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_top/ck_make/lbl_make");
			m_lbl_make_Shadow = FindUI<Shadow>(vb.transform ,"pl_top/ck_make/lbl_make");

			m_ck_resolve_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_top/ck_resolve");
			m_ck_resolve_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_top/ck_resolve");

			m_lbl_resolve_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_top/ck_resolve/lbl_resolve");
			m_lbl_resolve_Shadow = FindUI<Shadow>(vb.transform ,"pl_top/ck_resolve/lbl_resolve");

			m_UI_Gold = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"pl_top/UI_Gold"));
			m_btn_close = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"btn_close"));

            UI_Win_EquipMediator mt = new UI_Win_EquipMediator(vb.gameObject);
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
