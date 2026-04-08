// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, 20 October 2020
// Update Time         :    Tuesday, 20 October 2020
// Class Description   :    EventDateView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class EventDateView : GameView
    {
        public const string VIEW_NAME = "UI_Win_EventDate";

        public EventDateView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type5_SubView m_UI_Model_Window_Type5;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_date_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem6;
		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem5;
		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem4;
		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem3;
		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem2;
		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem1;
		[HideInInspector] public UI_Item_EventDateDateItem_SubView m_UI_DateItem0;
		[HideInInspector] public ScrollRect m_sv_content_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_content_PolygonImage;
		[HideInInspector] public ListView m_sv_content_ListView;

		[HideInInspector] public ArabLayoutCompment m_pl_event_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_mask_PolygonImage;
		[HideInInspector] public GameButton m_btn_mask_GameButton;

		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public Animator m_pl_offset_Animator;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_name_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_time_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_reward_ArabLayoutCompment;
		[HideInInspector] public HorizontalLayoutGroup m_pl_reward_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item5;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_go;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public PolygonImage m_btn_full_mask_PolygonImage;
		[HideInInspector] public GameButton m_btn_full_mask_GameButton;



        private void UIFinder()
        {
			m_UI_Model_Window_Type5 = new UI_Model_Window_Type5_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type5"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/listbg/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/listbg/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/listbg/sv_list");
			m_sv_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/listbg/sv_list");

			m_pl_date_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_date");

			m_UI_DateItem6 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem6"));
			m_UI_DateItem5 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem5"));
			m_UI_DateItem4 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem4"));
			m_UI_DateItem3 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem3"));
			m_UI_DateItem2 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem2"));
			m_UI_DateItem1 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem1"));
			m_UI_DateItem0 = new UI_Item_EventDateDateItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_date/date/UI_DateItem0"));
			m_sv_content_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_date/sv_content");
			m_sv_content_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_date/sv_content");
			m_sv_content_ListView = FindUI<ListView>(vb.transform ,"rect/pl_date/sv_content");

			m_pl_event_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_event");

			m_btn_mask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_mask");
			m_btn_mask_GameButton = FindUI<GameButton>(vb.transform ,"btn_mask");

			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_offset_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_offset");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_offset/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_offset/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_offset/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_offset/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_offset/img_bg/img_arrowSideL");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_offset/rect/lbl_name");
			m_lbl_name_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_offset/rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_offset/rect/lbl_name");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_offset/rect/lbl_name/img_icon");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_offset/rect/lbl_time");
			m_lbl_time_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_offset/rect/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_offset/rect/lbl_time");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_offset/rect/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_offset/rect/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_offset/rect/lbl_desc");

			m_pl_reward_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward");
			m_pl_reward_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward");

			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/rect/pl_reward/UI_Model_Item5"));
			m_btn_go = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/rect/btn_go"));
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"btn_close");

			m_btn_full_mask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_full_mask");
			m_btn_full_mask_GameButton = FindUI<GameButton>(vb.transform ,"btn_full_mask");


            EventDateMediator mt = new EventDateMediator(vb.gameObject);
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
