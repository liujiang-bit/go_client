// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeStartServer_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeStartServer_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeStartServer";

        public UI_Item_EventTypeStartServer_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_EventTypeStartServer_Animator;

		[HideInInspector] public CanvasGroup m_pl_con_CanvasGroup;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_life_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_life_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_val_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_box_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_box_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_num_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_num_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_boxicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_gettime_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_get;
		[HideInInspector] public PolygonImage m_img_box_red_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_box_red_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public UI_Model_EventTypeStartServerBtn_SubView m_btn_day1;
		[HideInInspector] public UI_Model_EventTypeStartServerBtn_SubView m_btn_day2;
		[HideInInspector] public UI_Model_EventTypeStartServerBtn_SubView m_btn_day3;
		[HideInInspector] public UI_Model_EventTypeStartServerBtn_SubView m_btn_day4;
		[HideInInspector] public UI_Model_EventTypeStartServerBtn_SubView m_btn_day5;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Model_EventTypeStartServerCk2_SubView m_ck_event1;
		[HideInInspector] public UI_Model_EventTypeStartServerCk2_SubView m_ck_event2;
		[HideInInspector] public UI_Model_EventTypeStartServerCk2_SubView m_ck_event3;
		[HideInInspector] public CanvasGroup m_pl_desc_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_eventDesc_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_eventDesc_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_eventDesc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventTypeStartServer_Animator = gameObject.GetComponent<Animator>();

			m_pl_con_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_con");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/img_bg");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_con/top/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/lbl_name");

			m_lbl_life_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_con/top/lbl_life");
			m_lbl_life_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/lbl_life");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_con/top/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/pb_rogressBar");

			m_lbl_val_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_con/top/pb_rogressBar/lbl_val");
			m_lbl_val_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/pb_rogressBar/lbl_val");

			m_btn_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/top/btn_box");
			m_btn_box_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_con/top/btn_box");
			m_btn_box_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/btn_box");

			m_img_num_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/top/btn_box/img_num");
			m_img_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/btn_box/img_num");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_con/top/btn_box/img_num/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/btn_box/img_num/lbl_num");

			m_img_boxicon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/top/btn_box/img_boxicon");

			m_lbl_gettime_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_con/top/btn_box/lbl_gettime");

			m_btn_get = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/top/btn_box/btn_get"));
			m_img_box_red_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/top/btn_box/img_box_red");
			m_img_box_red_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/btn_box/img_box_red");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/top/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_con/top/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_con/top/btn_info");

			m_btn_day1 = new UI_Model_EventTypeStartServerBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleday/btn_day1"));
			m_btn_day2 = new UI_Model_EventTypeStartServerBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleday/btn_day2"));
			m_btn_day3 = new UI_Model_EventTypeStartServerBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleday/btn_day3"));
			m_btn_day4 = new UI_Model_EventTypeStartServerBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleday/btn_day4"));
			m_btn_day5 = new UI_Model_EventTypeStartServerBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleday/btn_day5"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_con/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_con/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_con/sv_list");

			m_ck_event1 = new UI_Model_EventTypeStartServerCk2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleEvent/ck_event1"));
			m_ck_event2 = new UI_Model_EventTypeStartServerCk2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleEvent/ck_event2"));
			m_ck_event3 = new UI_Model_EventTypeStartServerCk2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_con/toggleEvent/ck_event3"));
			m_pl_desc_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_desc");

			m_lbl_eventDesc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_desc/sv/v/c/lbl_eventDesc");
			m_lbl_eventDesc_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_desc/sv/v/c/lbl_eventDesc");
			m_lbl_eventDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_desc/sv/v/c/lbl_eventDesc");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_desc/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_desc/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_desc/btn_back");


			BindEvent();
        }

        #endregion
    }
}