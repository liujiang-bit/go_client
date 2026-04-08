// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月27日
// Update Time         :    2020年5月27日
// Class Description   :    UI_Win_ArmyShowView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ArmyShowView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ArmyShow";

        public UI_Win_ArmyShowView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public RectMask2D m_pl_Rect_RectMask2D;

		[HideInInspector] public Animator m_pl_blet_Animator;
		[HideInInspector] public CanvasGroup m_pl_blet_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_blet_LanguageText;

		[HideInInspector] public PolygonImage m_btn_blet_PolygonImage;
		[HideInInspector] public GameButton m_btn_blet_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_blet_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_armys_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_armys_PolygonImage;
		[HideInInspector] public ListView m_sv_armys_ListView;
		[HideInInspector] public Animator m_sv_armys_Animator;
		[HideInInspector] public CanvasGroup m_sv_armys_CanvasGroup;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;
		[HideInInspector] public Animator m_sv_desc_Animator;
		[HideInInspector] public CanvasGroup m_sv_desc_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_show_PolygonImage;
		[HideInInspector] public Animator m_pl_show_Animator;
		[HideInInspector] public CanvasGroup m_pl_show_CanvasGroup;

		[HideInInspector] public RectTransform m_pl_army;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadWithLevel_sub_show;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadWithLevel_main_show;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_transport_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_woker_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_woker_MakeChildrenGray;
		[HideInInspector] public ArabLayoutCompment m_img_woker_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count2_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Item_ArmyShowLayOut_SubView m_UI_Item_ArmyShowLayOut;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_Rect_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_Rect");

			m_pl_blet_Animator = FindUI<Animator>(vb.transform ,"pl_Rect/pl_blet");
			m_pl_blet_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_Rect/pl_blet");

			m_lbl_blet_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Rect/pl_blet/lbl_blet");

			m_btn_blet_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/pl_blet/btn_blet");
			m_btn_blet_GameButton = FindUI<GameButton>(vb.transform ,"pl_Rect/pl_blet/btn_blet");
			m_btn_blet_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_blet/btn_blet");

			m_sv_armys_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_Rect/sv_armys");
			m_sv_armys_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/sv_armys");
			m_sv_armys_ListView = FindUI<ListView>(vb.transform ,"pl_Rect/sv_armys");
			m_sv_armys_Animator = FindUI<Animator>(vb.transform ,"pl_Rect/sv_armys");
			m_sv_armys_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_Rect/sv_armys");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_Rect/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_Rect/sv_desc");
			m_sv_desc_Animator = FindUI<Animator>(vb.transform ,"pl_Rect/sv_desc");
			m_sv_desc_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_Rect/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Rect/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/sv_desc/v/c/lbl_desc");

			m_pl_show_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/pl_show");
			m_pl_show_Animator = FindUI<Animator>(vb.transform ,"pl_Rect/pl_show");
			m_pl_show_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_Rect/pl_show");

			m_pl_army = FindUI<RectTransform>(vb.transform ,"pl_Rect/pl_show/top/pl_army");
			m_UI_Model_CaptainHeadWithLevel_sub_show = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_Rect/pl_show/top/pl_army/UI_Model_CaptainHeadWithLevel_sub_show"));
			m_UI_Model_CaptainHeadWithLevel_main_show = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_Rect/pl_show/top/pl_army/UI_Model_CaptainHeadWithLevel_main_show"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Rect/pl_show/top/pl_army/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_show/top/pl_army/lbl_name");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Rect/pl_show/top/pl_army/lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_show/top/pl_army/lbl_armyCount");

			m_pl_transport_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_show/top/pl_transport");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/lbl_name2");
			m_lbl_name2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/lbl_name2");

			m_img_woker_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/img_woker");
			m_img_woker_MakeChildrenGray = FindUI<GrayChildrens>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/img_woker");
			m_img_woker_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/img_woker");

			m_img_char_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/img_woker/img_char");

			m_lbl_count2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/lbl_count2");
			m_lbl_count2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Rect/pl_show/top/pl_transport/lbl_count2");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_Rect/pl_show/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Rect/pl_show/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_Rect/pl_show/sv_list");

			m_UI_Item_ArmyShowLayOut = new UI_Item_ArmyShowLayOut_SubView(FindUI<RectTransform>(vb.transform ,"pl_Rect/pl_show/sv_list/v/c/UI_Item_ArmyShowLayOut"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));

            UI_Win_ArmyShowMediator mt = new UI_Win_ArmyShowMediator(vb.gameObject);
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
