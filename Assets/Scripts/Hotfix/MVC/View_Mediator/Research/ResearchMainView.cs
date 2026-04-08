// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, February 5, 2020
// Update Time         :    Wednesday, February 5, 2020
// Class Description   :    ResearchMainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ResearchMainView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ResearchMain";

        public ResearchMainView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public ScrollRect m_sv_economy_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_economy_PolygonImage;
		[HideInInspector] public ListView m_sv_economy_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public ScrollRect m_sv_military_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_military_PolygonImage;
		[HideInInspector] public ListView m_sv_military_ListView;

		[HideInInspector] public RectTransform m_pl_speedUp;
		[HideInInspector] public GameSlider m_pb_spBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_spTimeLeft_LanguageText;

		[HideInInspector] public PolygonImage m_img_spbg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_spIcon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_cancelRes_PolygonImage;
		[HideInInspector] public GameButton m_btn_cancelRes_GameButton;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_speedUp;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_speedHelp;
		[HideInInspector] public VerticalLayoutGroup m_pl_side_VerticalLayoutGroup;

		[HideInInspector] public UI_Model_SideToggleGroupBtns_SubView m_UI_SideBtns;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_sv_economy_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_economy");
			m_sv_economy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_economy");
			m_sv_economy_ListView = FindUI<ListView>(vb.transform ,"rect/sv_economy");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_economy/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_economy/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_economy/v_list_view/c_list_view");
			m_sv_military_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_military");
			m_sv_military_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_military");
			m_sv_military_ListView = FindUI<ListView>(vb.transform ,"rect/sv_military");

			m_pl_speedUp = FindUI<RectTransform>(vb.transform ,"rect/pl_speedUp");
			m_pb_spBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/pl_speedUp/pb_spBar");

			m_lbl_spTimeLeft_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_speedUp/pb_spBar/lbl_spTimeLeft");

			m_img_spbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_speedUp/img_spbg");

			m_img_spIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_speedUp/img_spbg/img_spIcon");

			m_btn_cancelRes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_speedUp/img_spbg/btn_cancelRes");
			m_btn_cancelRes_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_speedUp/img_spbg/btn_cancelRes");

			m_UI_speedUp = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_speedUp/UI_speedUp"));
			m_UI_speedHelp = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_speedUp/UI_speedHelp"));
			m_pl_side_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_side");

			m_UI_SideBtns = new UI_Model_SideToggleGroupBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/UI_SideBtns"));

            ResearchMainMediator mt = new ResearchMainMediator(vb.gameObject);
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
