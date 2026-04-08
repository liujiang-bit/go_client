// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月11日
// Update Time         :    2020年8月11日
// Class Description   :    UI_Win_VipStoreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_VipStoreView : GameView
    {
        public const string VIEW_NAME = "UI_Win_VipStore";

        public UI_Win_VipStoreView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public PolygonImage m_img_vipBg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_vipLevel0_LanguageText;
		[HideInInspector] public Outline m_lbl_vipLevel0_Outline;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public LanguageText m_lbl_need_LanguageText;

		[HideInInspector] public GameSlider m_pb_vipBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_vipBar_ArabLayoutCompment;
		[HideInInspector] public SmoothProgressBar m_pb_vipBar_SmoothBar;

		[HideInInspector] public PolygonImage m_btn_vipBarAdd_PolygonImage;
		[HideInInspector] public GameButton m_btn_vipBarAdd_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_vipBarAdd_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_barText_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lastTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lastTime_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Pop_UseAddItemInWin_SubView m_UI_Pop_UseAddItemInWin;
		[HideInInspector] public UI_Pop_UseItemInPop_SubView m_UI_Pop_UseItemInPop;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_img_vipBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/img_vipBg");

			m_lbl_vipLevel0_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/img_vipBg/lbl_vipLevel0");
			m_lbl_vipLevel0_Outline = FindUI<Outline>(vb.transform ,"rect/right/img_vipBg/lbl_vipLevel0");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/lbl_level");

			m_lbl_need_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/lbl_need");

			m_pb_vipBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/right/bar/pb_vipBar");
			m_pb_vipBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/bar/pb_vipBar");
			m_pb_vipBar_SmoothBar = FindUI<SmoothProgressBar>(vb.transform ,"rect/right/bar/pb_vipBar");

			m_btn_vipBarAdd_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/bar/btn_vipBarAdd");
			m_btn_vipBarAdd_GameButton = FindUI<GameButton>(vb.transform ,"rect/right/bar/btn_vipBarAdd");
			m_btn_vipBarAdd_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/bar/btn_vipBarAdd");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/bar/lbl_barText");
			m_lbl_barText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/bar/lbl_barText");

			m_lbl_lastTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_lastTime");
			m_lbl_lastTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/lbl_lastTime");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/left/sv_list");

			m_UI_Pop_UseAddItemInWin = new UI_Pop_UseAddItemInWin_SubView(FindUI<RectTransform>(vb.transform ,"Pops/UI_Pop_UseAddItemInWin"));
			m_UI_Pop_UseItemInPop = new UI_Pop_UseItemInPop_SubView(FindUI<RectTransform>(vb.transform ,"Pops/UI_Pop_UseItemInPop"));

            UI_Win_VipStoreMediator mt = new UI_Win_VipStoreMediator(vb.gameObject);
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
