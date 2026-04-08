// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, July 22, 2020
// Update Time         :    Wednesday, July 22, 2020
// Class Description   :    UI_Pop_GuildMapBuildTipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_GuildMapBuildTipView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_GuildMapBuildTip";

        public UI_Pop_GuildMapBuildTipView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public Animator m_img_bg_Animator;
		[HideInInspector] public UIDefaultValue m_img_bg_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_position_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_am_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_am_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_spy;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_btn_mass;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_btn_atk;
		[HideInInspector] public GridLayoutGroup m_pl_self_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_self_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_addhelp;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_seeInfo;
		[HideInInspector] public RectTransform m_pl_create;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_create;
		[HideInInspector] public UI_Common_PopFun_SubView m_UI_Common_PopFun;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_colPro_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_remainDayTime_LanguageText;

		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line1;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line2;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line3;
		[HideInInspector] public UI_Item_IconAndTime_SubView m_UI_Item_IconAndTime;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_WorldObjInfoTPlayer1_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_UI_Item_line4;
		[HideInInspector] public PolygonImage m_btn_more4_PolygonImage;
		[HideInInspector] public GameButton m_btn_more4_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_more4_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_content4_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_content4_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_UI_Item_line5;
		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;
		[HideInInspector] public GameButton m_img_cur_GameButton;
		[HideInInspector] public ArabLayoutCompment m_img_cur_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_content5_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_content5_ArabLayoutCompment;

		[HideInInspector] public UI_Item_IconAndTime_SubView m_UI_Item_IconAndTime1;
		[HideInInspector] public PolygonImage m_btn_descinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_descinfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descinfo_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_description_CanvasGroup;
		[HideInInspector] public Animator m_pl_description_Animator;

		[HideInInspector] public PolygonImage m_btn_descBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_descBack_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descBack_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_descBack_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;



        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");
			m_img_bg_Animator = FindUI<Animator>(vb.transform ,"pl_pos/img_bg");
			m_img_bg_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_lbl_position_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/standard/lbl_position");

			m_pl_am_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_am");
			m_pl_am_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_am");

			m_btn_spy = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_am/btn_spy"));
			m_btn_mass = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_am/btn_mass"));
			m_btn_atk = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_am/btn_atk"));
			m_pl_self_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_self");
			m_pl_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_self");

			m_btn_addhelp = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_self/btn_addhelp"));
			m_btn_seeInfo = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_self/btn_seeInfo"));
			m_pl_create = FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_create");
			m_btn_create = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/btns/pl_create/btn_create"));
			m_UI_Common_PopFun = new UI_Common_PopFun_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/standard/UI_Common_PopFun"));
			m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar");

			m_img_fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar/Fill Area/img_fill");

			m_lbl_colPro_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar/lbl_colPro");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar/img_icon");

			m_lbl_remainDayTime_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/pb_rogressBar/lbl_remainDayTime");

			m_UI_Item_line1 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/textLayer/UI_Item_line1"));
			m_UI_Item_line2 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/textLayer/UI_Item_line2"));
			m_UI_Item_line3 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/textLayer/UI_Item_line3"));
			m_UI_Item_IconAndTime = new UI_Item_IconAndTime_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/UI_Item_IconAndTime"));
			m_UI_Item_WorldObjInfoTPlayer1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1");

			m_UI_Item_line4 = FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line4");
			m_btn_more4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line4/btn_more4");
			m_btn_more4_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line4/btn_more4");
			m_btn_more4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line4/btn_more4");

			m_lbl_content4_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line4/lbl_content4");
			m_lbl_content4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line4/lbl_content4");

			m_UI_Item_line5 = FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line5");
			m_img_cur_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line5/img_cur");
			m_img_cur_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line5/img_cur");
			m_img_cur_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line5/img_cur");

			m_lbl_content5_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line5/lbl_content5");
			m_lbl_content5_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/textLayer1/UI_Item_line5/lbl_content5");

			m_UI_Item_IconAndTime1 = new UI_Item_IconAndTime_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer1/UI_Item_IconAndTime1"));
			m_btn_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/btn_descinfo");
			m_btn_descinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/btn_descinfo");
			m_btn_descinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/SpecInfo/btn_descinfo");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/normalInfo/lbl_name");

			m_pl_description_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/img_bg/pl_description");
			m_pl_description_Animator = FindUI<Animator>(vb.transform ,"pl_pos/img_bg/pl_description");

			m_btn_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/pl_description/btn_descBack");
			m_btn_descBack_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/pl_description/btn_descBack");
			m_btn_descBack_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/pl_description/btn_descBack");

			m_img_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/pl_description/btn_descBack/img_descBack");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/img_bg/pl_description/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/pl_description/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_pos/img_bg/pl_description/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/img_bg/pl_description/sv_desc/v/c/lbl_desc");


            UI_Pop_GuildMapBuildTipMediator mt = new UI_Pop_GuildMapBuildTipMediator(vb.gameObject);
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
