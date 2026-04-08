// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月27日
// Update Time         :    2020年5月27日
// Class Description   :    UI_Win_GuildStoreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildStoreView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildStore";

        public UI_Win_GuildStoreView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_bg;
		[HideInInspector] public Image m_pl_mes_Image;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_title1;
		[HideInInspector] public PolygonImage m_btn_info1_PolygonImage;
		[HideInInspector] public GameButton m_btn_info1_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_record1_PolygonImage;
		[HideInInspector] public GameButton m_btn_record1_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_record1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num1_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_num1_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_num1_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_title2;
		[HideInInspector] public PolygonImage m_btn_info2_PolygonImage;
		[HideInInspector] public GameButton m_btn_info2_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_record2_PolygonImage;
		[HideInInspector] public GameButton m_btn_record2_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_record2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num2_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_num2_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_num2_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_left_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

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

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_buy;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_stock;


        private void UIFinder()
        {
			m_bg = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"bg"));
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right");

			m_pl_title1 = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_title1");
			m_btn_info1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pl_title1/btn_info1");
			m_btn_info1_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_right/pl_title1/btn_info1");
			m_btn_info1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_title1/btn_info1");

			m_btn_record1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pl_title1/btn_record1");
			m_btn_record1_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_right/pl_title1/btn_record1");
			m_btn_record1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_title1/btn_record1");

			m_lbl_num1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/pl_title1/lbl_num1");
			m_lbl_num1_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/pl_right/pl_title1/lbl_num1");
			m_lbl_num1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_title1/lbl_num1");

			m_pl_title2 = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right/pl_title2");
			m_btn_info2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pl_title2/btn_info2");
			m_btn_info2_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_right/pl_title2/btn_info2");
			m_btn_info2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_title2/btn_info2");

			m_btn_record2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pl_title2/btn_record2");
			m_btn_record2_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_right/pl_title2/btn_record2");
			m_btn_record2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_title2/btn_record2");

			m_lbl_num2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/pl_title2/lbl_num2");
			m_lbl_num2_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/pl_right/pl_title2/lbl_num2");
			m_lbl_num2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/pl_title2/lbl_num2");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/pl_right/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_mes/pl_right/sv_list");
			m_sv_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_right/sv_list");

			m_pl_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left");
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left");

			m_pl_item = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/pl_item");
			m_img_item_detailBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_item/img_item_detailBg");

			m_img_item_quality_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_item/img_item_detailBg/img_item_quality");

			m_img_item_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_item/img_item_detailBg/img_item");

			m_pl_desc_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_item/img_item_detailBg/pl_desc_bg");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/pl_item/img_item_detailBg/pl_desc_bg/lbl_desc");

			m_lbl_item_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/lbl_item_name");

			m_sv_list_detail_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/pl_left/sv_list_detail");
			m_sv_list_detail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/sv_list_detail");
			m_sv_list_detail_ListView = FindUI<ListView>(vb.transform ,"pl_mes/pl_left/sv_list_detail");

			m_v_list_detail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/sv_list_detail/v_list_detail");
			m_v_list_detail_Mask = FindUI<Mask>(vb.transform ,"pl_mes/pl_left/sv_list_detail/v_list_detail");

			m_c_list_detail = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/sv_list_detail/v_list_detail/c_list_detail");
			m_lbl_detail_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/sv_list_detail/v_list_detail/c_list_detail/lbl_detail");
			m_lbl_detail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/sv_list_detail/v_list_detail/c_list_detail/lbl_detail");

			m_pl_input_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_input");

			m_ipt_num_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/imgInputBg/ipt_num");
			m_ipt_num_GameInput = FindUI<GameInput>(vb.transform ,"pl_mes/pl_left/pl_input/imgInputBg/ipt_num");

			m_lbl_ipt_format_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/pl_input/imgInputBg/ipt_num/lbl_ipt_format");

			m_btn_substract_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/btn_substract");
			m_btn_substract_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_left/pl_input/btn_substract");
			m_btn_substract_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/pl_left/pl_input/btn_substract");
			m_btn_substract_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_input/btn_substract");

			m_img_substract_normal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/btn_substract/img_substract_normal");

			m_img_substract_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/btn_substract/img_substract_gray");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add");
			m_btn_add_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add");

			m_img_add_normal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add/img_add_normal");
			m_img_add_normal_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add/img_add_normal");
			m_img_add_normal_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add/img_add_normal");

			m_img_add_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_left/pl_input/btn_add/img_add_gray");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_left/lbl_num");

			m_btn_buy = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/btn_buy"));
			m_btn_stock = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_left/btn_stock"));

            UI_Win_GuildStoreMediator mt = new UI_Win_GuildStoreMediator(vb.gameObject);
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
