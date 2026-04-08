// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月28日
// Update Time         :    2020年7月28日
// Class Description   :    BagView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class BagView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Bag";

        public BagView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type3_SubView m_bg;
		[HideInInspector] public Image m_pl_mes_Image;

		[HideInInspector] public ArabLayoutCompment m_pl_list_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_view_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_pl_detail_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_detail_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_item;
		[HideInInspector] public PolygonImage m_img_item_detailBg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_item_quality_PolygonImage;

		[HideInInspector] public PolygonImage m_img_item_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_desc_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public LanguageText m_lbl_item_name_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_detail_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_detail_PolygonImage;
		[HideInInspector] public ListView m_sv_list_detail_ListView;

		[HideInInspector] public PolygonImage m_v_list_detail_PolygonImage;
		[HideInInspector] public Mask m_v_list_detail_Mask;

		[HideInInspector] public RectTransform m_c_list_detail;
		[HideInInspector] public LanguageText m_lbl_detail_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_detail_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_detail2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_detail2_PolygonImage;
		[HideInInspector] public ListView m_sv_list_detail2_ListView;

		[HideInInspector] public LanguageText m_lbl_detail2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_detail2_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_equip_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_equip_PolygonImage;
		[HideInInspector] public ListView m_sv_equip_ListView;

		[HideInInspector] public RectMask2D m_v_equip_RectMask2D;

		[HideInInspector] public RectTransform m_c_equip;
		[HideInInspector] public UI_Item_EquipAtt_SubView m_UI_Item_EquipAtt;
		[HideInInspector] public ArabLayoutCompment m_pl_input_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_num_PolygonImage;
		[HideInInspector] public GameInput m_ipt_num_GameInput;

		[HideInInspector] public LanguageText m_lbl_ipt_format_LanguageText;

		[HideInInspector] public PolygonImage m_btn_substract_PolygonImage;
		[HideInInspector] public GameButton m_btn_substract_GameButton;
		[HideInInspector] public BtnAnimation m_btn_substract_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_substract_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_substract_normal_PolygonImage;

		[HideInInspector] public PolygonImage m_img_substract_gray_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public BtnAnimation m_btn_add_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_add_normal_PolygonImage;
		[HideInInspector] public GameButton m_img_add_normal_GameButton;
		[HideInInspector] public BtnAnimation m_img_add_normal_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_add_gray_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_max_PolygonImage;
		[HideInInspector] public GameButton m_btn_max_GameButton;
		[HideInInspector] public BtnAnimation m_btn_max_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_max_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_max_LanguageText;

		[HideInInspector] public LanguageText m_lbl_own_num_LanguageText;

		[HideInInspector] public PolygonImage m_btn_operate_PolygonImage;
		[HideInInspector] public GameButton m_btn_operate_GameButton;
		[HideInInspector] public BtnAnimation m_btn_operate_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_operate_LanguageText;
		[HideInInspector] public Shadow m_lbl_operate_Shadow;

		[HideInInspector] public LanguageText m_lbl_no_item_LanguageText;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;
		[HideInInspector] public LayoutElement m_img_line_LayoutElement;

		[HideInInspector] public GameToggle m_ck_res_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_res_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_res_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_res_LanguageText;

		[HideInInspector] public PolygonImage m_img_redpoint1_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num1_LanguageText;

		[HideInInspector] public GameToggle m_ck_speed_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_speed_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_speed_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_speed_LanguageText;

		[HideInInspector] public PolygonImage m_img_redpoint2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num2_LanguageText;

		[HideInInspector] public GameToggle m_ck_gain_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_gain_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_gain_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_gain_LanguageText;

		[HideInInspector] public PolygonImage m_img_redpoint3_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num3_LanguageText;

		[HideInInspector] public GameToggle m_ck_equip_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_equip_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_equip_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_equip_LanguageText;

		[HideInInspector] public PolygonImage m_img_redpoint4_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num4_LanguageText;

		[HideInInspector] public GameToggle m_ck_other_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_other_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_other_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_other_LanguageText;

		[HideInInspector] public PolygonImage m_img_redpoint5_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num5_LanguageText;

		[HideInInspector] public ToggleGroup m_pl_group_ToggleGroup;
		[HideInInspector] public LayoutElement m_pl_group_LayoutElement;



        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_bg = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"bg"));
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");

			m_pl_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_list");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view");
			m_sv_list_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContent/pl_list/sv_list_view/v_list_view/c_list_view");
			m_pl_detail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail");
			m_pl_detail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail");

			m_pl_item = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_item");
			m_img_item_detailBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_item/img_item_detailBg");

			m_img_item_quality_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_item/img_item_detailBg/img_item_quality");

			m_img_item_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_item/img_item_detailBg/img_item");

			m_pl_desc_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_item/img_item_detailBg/pl_desc_bg");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_item/img_item_detailBg/pl_desc_bg/lbl_desc");

			m_lbl_item_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/lbl_item_name");

			m_sv_list_detail_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail");
			m_sv_list_detail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail");
			m_sv_list_detail_ListView = FindUI<ListView>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail");

			m_v_list_detail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail/v_list_detail");
			m_v_list_detail_Mask = FindUI<Mask>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail/v_list_detail");

			m_c_list_detail = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail/v_list_detail/c_list_detail");
			m_lbl_detail_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail/v_list_detail/c_list_detail/lbl_detail");
			m_lbl_detail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail/v_list_detail/c_list_detail/lbl_detail");

			m_sv_list_detail2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail2");
			m_sv_list_detail2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail2");
			m_sv_list_detail2_ListView = FindUI<ListView>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail2");

			m_lbl_detail2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail2/v/c/lbl_detail2");
			m_lbl_detail2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_list_detail2/v/c/lbl_detail2");

			m_sv_equip_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_equip");
			m_sv_equip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_equip");
			m_sv_equip_ListView = FindUI<ListView>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_equip");

			m_v_equip_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_equip/v_equip");

			m_c_equip = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_equip/v_equip/c_equip");
			m_UI_Item_EquipAtt = new UI_Item_EquipAtt_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContent/pl_detail/sv_equip/v_equip/c_equip/UI_Item_EquipAtt"));
			m_pl_input_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input");

			m_ipt_num_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/imgInputBg/ipt_num");
			m_ipt_num_GameInput = FindUI<GameInput>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/imgInputBg/ipt_num");

			m_lbl_ipt_format_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/imgInputBg/ipt_num/lbl_ipt_format");

			m_btn_substract_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_substract");
			m_btn_substract_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_substract");
			m_btn_substract_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_substract");
			m_btn_substract_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_substract");

			m_img_substract_normal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_substract/img_substract_normal");

			m_img_substract_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_substract/img_substract_gray");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add");
			m_btn_add_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add");

			m_img_add_normal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add/img_add_normal");
			m_img_add_normal_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add/img_add_normal");
			m_img_add_normal_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add/img_add_normal");

			m_img_add_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_add/img_add_gray");

			m_btn_max_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_max");
			m_btn_max_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_max");
			m_btn_max_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_max");
			m_btn_max_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_max");

			m_lbl_max_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/pl_input/btn_max/lbl_max");

			m_lbl_own_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/lbl_own_num");

			m_btn_operate_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContent/pl_detail/btn_operate");
			m_btn_operate_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContent/pl_detail/btn_operate");
			m_btn_operate_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContent/pl_detail/btn_operate");

			m_lbl_operate_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/pl_detail/btn_operate/lbl_operate");
			m_lbl_operate_Shadow = FindUI<Shadow>(vb.transform ,"pl_mes/imgContent/pl_detail/btn_operate/lbl_operate");

			m_lbl_no_item_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContent/lbl_no_item");

			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgTopBg/img_line");
			m_img_line_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_mes/imgTopBg/img_line");

			m_ck_res_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_mes/imgTopBg/ck_res");
			m_ck_res_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgTopBg/ck_res");
			m_ck_res_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgTopBg/ck_res");

			m_lbl_res_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_res/lbl_res");

			m_img_redpoint1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgTopBg/ck_res/img_redpoint1");

			m_lbl_num1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_res/img_redpoint1/lbl_num1");

			m_ck_speed_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_mes/imgTopBg/ck_speed");
			m_ck_speed_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgTopBg/ck_speed");
			m_ck_speed_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgTopBg/ck_speed");

			m_lbl_speed_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_speed/lbl_speed");

			m_img_redpoint2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgTopBg/ck_speed/img_redpoint2");

			m_lbl_num2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_speed/img_redpoint2/lbl_num2");

			m_ck_gain_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_mes/imgTopBg/ck_gain");
			m_ck_gain_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgTopBg/ck_gain");
			m_ck_gain_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgTopBg/ck_gain");

			m_lbl_gain_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_gain/lbl_gain");

			m_img_redpoint3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgTopBg/ck_gain/img_redpoint3");

			m_lbl_num3_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_gain/img_redpoint3/lbl_num3");

			m_ck_equip_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_mes/imgTopBg/ck_equip");
			m_ck_equip_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgTopBg/ck_equip");
			m_ck_equip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgTopBg/ck_equip");

			m_lbl_equip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_equip/lbl_equip");

			m_img_redpoint4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgTopBg/ck_equip/img_redpoint4");

			m_lbl_num4_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_equip/img_redpoint4/lbl_num4");

			m_ck_other_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_mes/imgTopBg/ck_other");
			m_ck_other_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgTopBg/ck_other");
			m_ck_other_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgTopBg/ck_other");

			m_lbl_other_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_other/lbl_other");

			m_img_redpoint5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgTopBg/ck_other/img_redpoint5");

			m_lbl_num5_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgTopBg/ck_other/img_redpoint5/lbl_num5");

			m_pl_group_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"pl_mes/imgTopBg/pl_group");
			m_pl_group_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_mes/imgTopBg/pl_group");


            BagMediator mt = new BagMediator(vb.gameObject);
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
