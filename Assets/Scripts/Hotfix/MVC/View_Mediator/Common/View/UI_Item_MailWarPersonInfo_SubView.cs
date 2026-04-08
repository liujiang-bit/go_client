// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarPersonInfo_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarPersonInfo_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarPersonInfo";

        public UI_Item_MailWarPersonInfo_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailWarPersonInfo;
		[HideInInspector] public RectTransform m_pl_player;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHeadL;
		[HideInInspector] public LanguageText m_lbl_nameL_LanguageText;

		[HideInInspector] public UI_Model_Link_SubView m_lbl_positionL;
		[HideInInspector] public LanguageText m_lbl_powerL_LanguageText;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_percent_LanguageText;

		[HideInInspector] public PolygonImage m_img_monsterhead_PolygonImage;

		[HideInInspector] public PolygonImage m_img_fra_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_fra_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_monster_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_monster_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_captain_GridLayoutGroup;

		[HideInInspector] public RectTransform m_UI_Item_WarMailCaptain1;
		[HideInInspector] public RectTransform m_pl_key1;
		[HideInInspector] public LanguageText m_lbl_key1_LanguageText;

		[HideInInspector] public PolygonImage m_img_key1_PolygonImage;

		[HideInInspector] public PolygonImage m_icon_key1_PolygonImage;

		[HideInInspector] public RectTransform m_pl_exp1;
		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name1_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead1;
		[HideInInspector] public PolygonImage m_img_exp1_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_exp1_LanguageText;

		[HideInInspector] public RectTransform m_pl_empty1;
		[HideInInspector] public LanguageText m_lbl_empty1_LanguageText;

		[HideInInspector] public PolygonImage m_img_empty1_PolygonImage;

		[HideInInspector] public RectTransform m_UI_Item_WarMailCaptain2;
		[HideInInspector] public RectTransform m_pl_key2;
		[HideInInspector] public LanguageText m_lbl_key2_LanguageText;

		[HideInInspector] public PolygonImage m_img_key2_PolygonImage;

		[HideInInspector] public PolygonImage m_icon_key2_PolygonImage;

		[HideInInspector] public RectTransform m_pl_exp2;
		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead2;
		[HideInInspector] public PolygonImage m_img_exp2_PolygonImage;

		[HideInInspector] public RectTransform m_pl_empty2;
		[HideInInspector] public LanguageText m_lbl_empty2_LanguageText;

		[HideInInspector] public PolygonImage m_img_empty2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_lineL_PolygonImage;
		[HideInInspector] public LayoutElement m_img_lineL_LayoutElement;

		[HideInInspector] public RectTransform m_pl_detailL;
		[HideInInspector] public RectTransform m_pl_Specail;
		[HideInInspector] public LanguageText m_lbl_specTitleL_LanguageText;

		[HideInInspector] public LanguageText m_lbl_specvalL_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_data_GridLayoutGroup;

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

		[HideInInspector] public RectTransform m_pl_defultL;
		[HideInInspector] public PolygonImage m_img_defult_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_MailWarPersonInfo = gameObject.GetComponent<RectTransform>();
			m_pl_player = FindUI<RectTransform>(gameObject.transform ,"layout/pl_player");
			m_UI_Model_PlayerHeadL = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"layout/pl_player/UI_Model_PlayerHeadL"));
			m_lbl_nameL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_player/lbl_nameL");

			m_lbl_positionL = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"layout/pl_player/lbl_positionL"));
			m_lbl_powerL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_player/lbl_powerL");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"layout/pl_player/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_player/pb_rogressBar");

			m_lbl_percent_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_player/pb_rogressBar/lbl_percent");

			m_img_monsterhead_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_player/img_monsterhead");

			m_img_fra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_player/img_fra");
			m_img_fra_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_player/img_fra");

			m_img_monster_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_player/img_fra/img_monster");
			m_img_monster_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_player/img_fra/img_monster");

			m_pl_captain_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"layout/pl_captain");

			m_UI_Item_WarMailCaptain1 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1");
			m_pl_key1 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_key1");
			m_lbl_key1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_key1/lbl_key1");

			m_img_key1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_key1/img_key1");

			m_icon_key1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_key1/icon_key1");

			m_pl_exp1 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_exp1");
			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_exp1/lbl_name1");
			m_lbl_name1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_exp1/lbl_name1");

			m_UI_Model_CaptainHead1 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_exp1/UI_Model_CaptainHead1"));
			m_img_exp1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_exp1/img_exp1");

			m_lbl_exp1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_exp1/img_exp1/lbl_exp1");

			m_pl_empty1 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_empty1");
			m_lbl_empty1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_empty1/lbl_empty1");

			m_img_empty1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain1/pl_empty1/img_empty1");

			m_UI_Item_WarMailCaptain2 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2");
			m_pl_key2 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_key2");
			m_lbl_key2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_key2/lbl_key2");

			m_img_key2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_key2/img_key2");

			m_icon_key2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_key2/icon_key2");

			m_pl_exp2 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_exp2");
			m_lbl_name2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_exp2/lbl_name2");
			m_lbl_name2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_exp2/lbl_name2");

			m_UI_Model_CaptainHead2 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_exp2/UI_Model_CaptainHead2"));
			m_img_exp2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_exp2/img_exp2");

			m_pl_empty2 = FindUI<RectTransform>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_empty2");
			m_lbl_empty2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_empty2/lbl_empty2");

			m_img_empty2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/UI_Item_WarMailCaptain2/pl_empty2/img_empty2");

			m_img_lineL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_captain/img_lineL");
			m_img_lineL_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"layout/pl_captain/img_lineL");

			m_pl_detailL = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL");
			m_pl_Specail = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_Specail");
			m_lbl_specTitleL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_Specail/lbl_specTitleL");

			m_lbl_specvalL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_Specail/lbl_specvalL");

			m_pl_data_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"layout/pl_detailL/pl_data");

			m_pl_total_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self");
			m_lbl_title_total_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/lbl_title_total_self");
			m_lbl_title_total_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/lbl_title_total_self");

			m_lbl_val_total_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/lbl_val_total_self");
			m_lbl_val_total_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/lbl_val_total_self");

			m_btn_more_self_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/btn_more_self");
			m_btn_more_self_GameButton = FindUI<GameButton>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/btn_more_self");
			m_btn_more_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_total_self/btn_more_self");

			m_pl_reatment_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_reatment_self");
			m_lbl_title_reatment_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_reatment_self/lbl_title_reatment_self");
			m_lbl_title_reatment_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_reatment_self/lbl_title_reatment_self");

			m_lbl_val_reatment_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_reatment_self/lbl_val_reatment_self");
			m_lbl_val_reatment_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_reatment_self/lbl_val_reatment_self");

			m_pl_dead_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_dead_self");
			m_lbl_title_dead_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_dead_self/lbl_title_dead_self");
			m_lbl_title_dead_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_dead_self/lbl_title_dead_self");

			m_lbl_val_dead_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_dead_self/lbl_val_dead_self");
			m_lbl_val_dead_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_dead_self/lbl_val_dead_self");

			m_pl_heart_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self");
			m_lbl_title_heart_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/lbl_title_heart_self");
			m_lbl_title_heart_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/lbl_title_heart_self");

			m_lbl_val_heart_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/lbl_val_heart_self");
			m_lbl_val_heart_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/lbl_val_heart_self");

			m_btn_Question_heart_self_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/btn_Question_heart_self");
			m_btn_Question_heart_self_GameButton = FindUI<GameButton>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/btn_Question_heart_self");
			m_btn_Question_heart_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/btn_Question_heart_self");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_heart_self/btn_Question_heart_self/img_icon");

			m_pl_littlehurt_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self");
			m_lbl_title_littlehurt_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/lbl_title_littlehurt_self");
			m_lbl_title_littlehurt_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/lbl_title_littlehurt_self");

			m_lbl_val_littlehurt_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/lbl_val_littlehurt_self");
			m_lbl_val_littlehurt_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/lbl_val_littlehurt_self");

			m_btn_Question_ittlehurt_self_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/btn_Question_ittlehurt_self");
			m_btn_Question_ittlehurt_self_GameButton = FindUI<GameButton>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/btn_Question_ittlehurt_self");
			m_btn_Question_ittlehurt_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_littlehurt_self/btn_Question_ittlehurt_self");

			m_pl_last_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_last_self");
			m_lbl_title_last_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_last_self/lbl_title_last_self");
			m_lbl_title_last_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_last_self/lbl_title_last_self");

			m_lbl_val_last_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_last_self/lbl_val_last_self");
			m_lbl_val_last_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_last_self/lbl_val_last_self");

			m_pl_arrow_self = FindUI<RectTransform>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_arrow_self");
			m_lbl_title_arrow_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_arrow_self/lbl_title_arrow_self");
			m_lbl_title_arrow_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_arrow_self/lbl_title_arrow_self");

			m_lbl_val_arrow_self_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_arrow_self/lbl_val_arrow_self");
			m_lbl_val_arrow_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"layout/pl_detailL/pl_data/pl_arrow_self/lbl_val_arrow_self");

			m_pl_defultL = FindUI<RectTransform>(gameObject.transform ,"layout/pl_defultL");
			m_img_defult_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"layout/pl_defultL/img_defult");


			BindEvent();
        }

        #endregion
    }
}