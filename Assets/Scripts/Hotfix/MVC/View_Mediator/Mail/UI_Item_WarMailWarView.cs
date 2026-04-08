// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月19日
// Update Time         :    2020年3月19日
// Class Description   :    UI_Item_WarMailWarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarMailWarView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarMailWar";

        public UI_Item_WarMailWarView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_position;
		[HideInInspector] public LanguageText m_lbl_date_LanguageText;

		[HideInInspector] public UI_Model_Link_SubView m_lbl_position;
		[HideInInspector] public GridLayoutGroup m_pl_rect_GridLayoutGroup;

		[HideInInspector] public UI_Item_MailWarPersonInfo_SubView m_UI_Item_MailWarPersonInfo_left;
		[HideInInspector] public RectTransform m_UI_Item_MailWarPersonInfo_self;
		[HideInInspector] public VerticalLayoutGroup m_layout_self_VerticalLayoutGroup;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_self_LanguageText;

		[HideInInspector] public UI_Model_Link_SubView m_lbl_position_self;
		[HideInInspector] public LanguageText m_lbl_power_self_LanguageText;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_percent_LanguageText;

		[HideInInspector] public GridLayoutGroup m_captain_self_GridLayoutGroup;

		[HideInInspector] public RectTransform m_UI_Item_WarMailCaptain1_self;
		[HideInInspector] public RectTransform m_pl_key1_self;
		[HideInInspector] public LanguageText m_lbl_key1_self_LanguageText;

		[HideInInspector] public PolygonImage m_img_key1_self_PolygonImage;

		[HideInInspector] public PolygonImage m_icon_key1_self_PolygonImage;

		[HideInInspector] public RectTransform m_pl_exp1_self;
		[HideInInspector] public LanguageText m_lbl_name1_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name1_self_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead1_self;
		[HideInInspector] public PolygonImage m_img_exp1_self_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_exp1_self_LanguageText;

		[HideInInspector] public RectTransform m_pl_empty1_self;
		[HideInInspector] public LanguageText m_lbl_empty1_self_LanguageText;

		[HideInInspector] public PolygonImage m_img_empty1_self_PolygonImage;

		[HideInInspector] public RectTransform m_UI_Item_WarMailCaptain2_self;
		[HideInInspector] public RectTransform m_pl_key2_self;
		[HideInInspector] public LanguageText m_lbl_key2_self_LanguageText;

		[HideInInspector] public PolygonImage m_img_key2_self_PolygonImage;

		[HideInInspector] public PolygonImage m_icon_key2_self_PolygonImage;

		[HideInInspector] public RectTransform m_pl_exp2_self;
		[HideInInspector] public LanguageText m_lbl_name2_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name2_self_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead2_self;
		[HideInInspector] public PolygonImage m_img_exp2_self_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_exp2_self_LanguageText;

		[HideInInspector] public RectTransform m_pl_empty2_self;
		[HideInInspector] public LanguageText m_lbl_empty2_self_LanguageText;

		[HideInInspector] public PolygonImage m_img_empty2_self_PolygonImage;

		[HideInInspector] public PolygonImage m_img_lineL_PolygonImage;
		[HideInInspector] public LayoutElement m_img_lineL_LayoutElement;

		[HideInInspector] public RectTransform m_pl_detail_self;
		[HideInInspector] public GridLayoutGroup m_pl_data_self_GridLayoutGroup;

		[HideInInspector] public RectTransform m_pl_total_self;
		[HideInInspector] public LanguageText m_lbl_title_total_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_total_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_total_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_total_self_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_more_self_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_self_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_more_self_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_reatment_self;
		[HideInInspector] public LanguageText m_lbl_title_reatment_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_reatment_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_reatment_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_reatment_self_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_dead_self;
		[HideInInspector] public LanguageText m_lbl_title_dead_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_dead_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_dead_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_dead_self_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_heart_self;
		[HideInInspector] public LanguageText m_lbl_title_heart_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_heart_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_heart_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_heart_self_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_Question_heart_self_PolygonImage;
		[HideInInspector] public GameButton m_btn_Question_heart_self_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_Question_heart_self_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public RectTransform m_pl_littlehurt_self;
		[HideInInspector] public LanguageText m_lbl_title_littlehurt_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_littlehurt_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_littlehurt_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_littlehurt_self_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_Question_ittlehurt_self_PolygonImage;
		[HideInInspector] public GameButton m_btn_Question_ittlehurt_self_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_Question_ittlehurt_self_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_last_self;
		[HideInInspector] public LanguageText m_lbl_title_last_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_last_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_last_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_last_self_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_arrow_self;
		[HideInInspector] public LanguageText m_lbl_title_arrow_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_arrow_self_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_arrow_self_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_arrow_self_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_defultL_self;
		[HideInInspector] public PolygonImage m_img_defult_self_PolygonImage;

		[HideInInspector] public HorizontalLayoutGroup m_pl_btn_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_btn_buff;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_btn_log;
		[HideInInspector] public RectTransform m_pl_light;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_position = FindUI<RectTransform>(vb.transform ,"pl_position");
			m_lbl_date_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_position/lbl_date");

			m_lbl_position = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"pl_position/lbl_position"));
			m_pl_rect_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect");

			m_UI_Item_MailWarPersonInfo_left = new UI_Item_MailWarPersonInfo_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_left"));
			m_UI_Item_MailWarPersonInfo_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self");
			m_layout_self_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/player/UI_Model_PlayerHead"));
			m_lbl_name_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/player/lbl_name_self");

			m_lbl_position_self = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/player/lbl_position_self"));
			m_lbl_power_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/player/lbl_power_self");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/player/pb_rogressBar");

			m_lbl_percent_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/player/pb_rogressBar/lbl_percent");

			m_captain_self_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self");

			m_UI_Item_WarMailCaptain1_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self");
			m_pl_key1_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_key1_self");
			m_lbl_key1_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_key1_self/lbl_key1_self");

			m_img_key1_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_key1_self/img_key1_self");

			m_icon_key1_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_key1_self/icon_key1_self");

			m_pl_exp1_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_exp1_self");
			m_lbl_name1_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_exp1_self/lbl_name1_self");
			m_lbl_name1_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_exp1_self/lbl_name1_self");

			m_UI_Model_CaptainHead1_self = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_exp1_self/UI_Model_CaptainHead1_self"));
			m_img_exp1_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_exp1_self/img_exp1_self");

			m_lbl_exp1_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_exp1_self/img_exp1_self/lbl_exp1_self");

			m_pl_empty1_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_empty1_self");
			m_lbl_empty1_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_empty1_self/lbl_empty1_self");

			m_img_empty1_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain1_self/pl_empty1_self/img_empty1_self");

			m_UI_Item_WarMailCaptain2_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self");
			m_pl_key2_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_key2_self");
			m_lbl_key2_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_key2_self/lbl_key2_self");

			m_img_key2_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_key2_self/img_key2_self");

			m_icon_key2_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_key2_self/icon_key2_self");

			m_pl_exp2_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_exp2_self");
			m_lbl_name2_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_exp2_self/lbl_name2_self");
			m_lbl_name2_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_exp2_self/lbl_name2_self");

			m_UI_Model_CaptainHead2_self = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_exp2_self/UI_Model_CaptainHead2_self"));
			m_img_exp2_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_exp2_self/img_exp2_self");

			m_lbl_exp2_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_exp2_self/img_exp2_self/lbl_exp2_self");

			m_pl_empty2_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_empty2_self");
			m_lbl_empty2_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_empty2_self/lbl_empty2_self");

			m_img_empty2_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/UI_Item_WarMailCaptain2_self/pl_empty2_self/img_empty2_self");

			m_img_lineL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/img_lineL");
			m_img_lineL_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/captain_self/img_lineL");

			m_pl_detail_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self");
			m_pl_data_self_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self");

			m_pl_total_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self");
			m_lbl_title_total_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/lbl_title_total_self");
			m_lbl_title_total_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/lbl_title_total_self");

			m_lbl_val_total_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/lbl_val_total_self");
			m_lbl_val_total_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/lbl_val_total_self");

			m_btn_more_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/btn_more_self");
			m_btn_more_self_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/btn_more_self");
			m_btn_more_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_total_self/btn_more_self");

			m_pl_reatment_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_reatment_self");
			m_lbl_title_reatment_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_reatment_self/lbl_title_reatment_self");
			m_lbl_title_reatment_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_reatment_self/lbl_title_reatment_self");

			m_lbl_val_reatment_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_reatment_self/lbl_val_reatment_self");
			m_lbl_val_reatment_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_reatment_self/lbl_val_reatment_self");

			m_pl_dead_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_dead_self");
			m_lbl_title_dead_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_dead_self/lbl_title_dead_self");
			m_lbl_title_dead_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_dead_self/lbl_title_dead_self");

			m_lbl_val_dead_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_dead_self/lbl_val_dead_self");
			m_lbl_val_dead_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_dead_self/lbl_val_dead_self");

			m_pl_heart_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self");
			m_lbl_title_heart_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/lbl_title_heart_self");
			m_lbl_title_heart_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/lbl_title_heart_self");

			m_lbl_val_heart_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/lbl_val_heart_self");
			m_lbl_val_heart_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/lbl_val_heart_self");

			m_btn_Question_heart_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/btn_Question_heart_self");
			m_btn_Question_heart_self_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/btn_Question_heart_self");
			m_btn_Question_heart_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/btn_Question_heart_self");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_heart_self/btn_Question_heart_self/img_icon");

			m_pl_littlehurt_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self");
			m_lbl_title_littlehurt_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/lbl_title_littlehurt_self");
			m_lbl_title_littlehurt_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/lbl_title_littlehurt_self");

			m_lbl_val_littlehurt_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/lbl_val_littlehurt_self");
			m_lbl_val_littlehurt_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/lbl_val_littlehurt_self");

			m_btn_Question_ittlehurt_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/btn_Question_ittlehurt_self");
			m_btn_Question_ittlehurt_self_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/btn_Question_ittlehurt_self");
			m_btn_Question_ittlehurt_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_littlehurt_self/btn_Question_ittlehurt_self");

			m_pl_last_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_last_self");
			m_lbl_title_last_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_last_self/lbl_title_last_self");
			m_lbl_title_last_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_last_self/lbl_title_last_self");

			m_lbl_val_last_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_last_self/lbl_val_last_self");
			m_lbl_val_last_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_last_self/lbl_val_last_self");

			m_pl_arrow_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_arrow_self");
			m_lbl_title_arrow_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_arrow_self/lbl_title_arrow_self");
			m_lbl_title_arrow_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_arrow_self/lbl_title_arrow_self");

			m_lbl_val_arrow_self_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_arrow_self/lbl_val_arrow_self");
			m_lbl_val_arrow_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_detail_self/pl_data_self/pl_arrow_self/lbl_val_arrow_self");

			m_pl_defultL_self = FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_defultL_self");
			m_img_defult_self_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/UI_Item_MailWarPersonInfo_self/layout_self/pl_defultL_self/img_defult_self");

			m_pl_btn_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"pl_btn");

			m_UI_btn_buff = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn/UI_btn_buff"));
			m_UI_btn_log = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn/UI_btn_log"));
			m_pl_light = FindUI<RectTransform>(vb.transform ,"pl_light");


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}