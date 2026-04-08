// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月31日
// Update Time         :    2020年8月31日
// Class Description   :    UI_Win_QuestView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_QuestView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Quest";

        public UI_Win_QuestView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public ScrollRect m_sv_questMain_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_questMain_PolygonImage;
		[HideInInspector] public ListView m_sv_questMain_ListView;

		[HideInInspector] public RectTransform m_pl_daily;
		[HideInInspector] public PolygonImage m_img_apicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_aptext_LanguageText;

		[HideInInspector] public PolygonImage m_img_timeicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_timetext_LanguageText;

		[HideInInspector] public GameSlider m_pb_ap_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_ap_ArabLayoutCompment;
		[HideInInspector] public SmoothProgressBar m_pb_ap_SmoothBar;

		[HideInInspector] public UI_Item_QuestBox_SubView m_UI_Item_QuestBox20;
		[HideInInspector] public UI_Item_QuestBox_SubView m_UI_Item_QuestBox40;
		[HideInInspector] public UI_Item_QuestBox_SubView m_UI_Item_QuestBox60;
		[HideInInspector] public UI_Item_QuestBox_SubView m_UI_Item_QuestBox80;
		[HideInInspector] public UI_Item_QuestBox_SubView m_UI_Item_QuestBox100;
		[HideInInspector] public ScrollRect m_sv_questDaily_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_questDaily_PolygonImage;
		[HideInInspector] public ListView m_sv_questDaily_ListView;

		[HideInInspector] public RectTransform m_pl_chapter;
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_sp_PolygonImage;
		[HideInInspector] public Mask m_pl_sp_Mask;

		[HideInInspector] public SkeletonGraphic m_img_char2_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_img_char2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_char_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_progress_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_progress_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_UI_Model_btn;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_view_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_chapterQuest_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_chapterQuest_PolygonImage;
		[HideInInspector] public ListView m_sv_chapterQuest_ListView;

		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_1;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_2;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_3;


        private void UIFinder()
        {
			m_UI_Model_Window = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_sv_questMain_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_questMain");
			m_sv_questMain_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_questMain");
			m_sv_questMain_ListView = FindUI<ListView>(vb.transform ,"rect/sv_questMain");

			m_pl_daily = FindUI<RectTransform>(vb.transform ,"rect/pl_daily");
			m_img_apicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_daily/plboxs/apval/img_apicon");

			m_lbl_aptext_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_daily/plboxs/apval/lbl_aptext");

			m_img_timeicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_daily/plboxs/timeVal/img_timeicon");

			m_lbl_timetext_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_daily/plboxs/timeVal/lbl_timetext");

			m_pb_ap_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/pl_daily/plboxs/pb_ap");
			m_pb_ap_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_daily/plboxs/pb_ap");
			m_pb_ap_SmoothBar = FindUI<SmoothProgressBar>(vb.transform ,"rect/pl_daily/plboxs/pb_ap");

			m_UI_Item_QuestBox20 = new UI_Item_QuestBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_daily/plboxs/pb_ap/boxs/UI_Item_QuestBox20"));
			m_UI_Item_QuestBox40 = new UI_Item_QuestBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_daily/plboxs/pb_ap/boxs/UI_Item_QuestBox40"));
			m_UI_Item_QuestBox60 = new UI_Item_QuestBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_daily/plboxs/pb_ap/boxs/UI_Item_QuestBox60"));
			m_UI_Item_QuestBox80 = new UI_Item_QuestBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_daily/plboxs/pb_ap/boxs/UI_Item_QuestBox80"));
			m_UI_Item_QuestBox100 = new UI_Item_QuestBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_daily/plboxs/pb_ap/boxs/UI_Item_QuestBox100"));
			m_sv_questDaily_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_daily/sv_questDaily");
			m_sv_questDaily_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_daily/sv_questDaily");
			m_sv_questDaily_ListView = FindUI<ListView>(vb.transform ,"rect/pl_daily/sv_questDaily");

			m_pl_chapter = FindUI<RectTransform>(vb.transform ,"rect/pl_chapter");
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_chapter/character/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_chapter/character/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_chapter/character/lbl_text");

			m_pl_sp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_chapter/character/pl_sp");
			m_pl_sp_Mask = FindUI<Mask>(vb.transform ,"rect/pl_chapter/character/pl_sp");

			m_img_char2_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"rect/pl_chapter/character/pl_sp/img_char2");
			m_img_char2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_chapter/character/pl_sp/img_char2");

			m_img_char_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_chapter/character/img_char");
			m_img_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_chapter/character/img_char");

			m_lbl_progress_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/lbl_progress");
			m_lbl_progress_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/lbl_progress");

			m_UI_Model_btn = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/UI_Model_btn"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/items/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/items/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/items/sv_list_view");
			m_sv_list_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_chapter/quest/title/chapReward/items/sv_list_view");

			m_sv_chapterQuest_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_chapter/quest/sv_chapterQuest");
			m_sv_chapterQuest_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_chapter/quest/sv_chapterQuest");
			m_sv_chapterQuest_ListView = FindUI<ListView>(vb.transform ,"rect/pl_chapter/quest/sv_chapterQuest");

			m_UI_Model_PageButton_1 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"page/UI_Model_PageButton_1"));
			m_UI_Model_PageButton_2 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"page/UI_Model_PageButton_2"));
			m_UI_Model_PageButton_3 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"page/UI_Model_PageButton_3"));

            UI_Win_QuestMediator mt = new UI_Win_QuestMediator(vb.gameObject);
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
