// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, 12 October 2020
// Update Time         :    Monday, 12 October 2020
// Class Description   :    LoadingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class LoadingView : GameView
    {
        public const string VIEW_NAME = "UI_IF_Loading";

        public LoadingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public NbUIStrench m_img_bg_NbUIStrench;

		[HideInInspector] public PolygonImage m_img_logo_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_tip_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_Tip_LanguageText;
		[HideInInspector] public Shadow m_lbl_Tip_Shadow;
		[HideInInspector] public Outline m_lbl_Tip_Outline;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_doing_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_view_ArabLayoutCompment;

		[HideInInspector] public UI_Item_LoadingAni_SubView m_UI_Item_LoadingAni;
		[HideInInspector] public LanguageText m_lbl_version_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_version_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_service_PolygonImage;
		[HideInInspector] public GameButton m_btn_service_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_service_ArabLayoutCompment;



        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_NbUIStrench = FindUI<NbUIStrench>(vb.transform ,"img_bg");

			m_img_logo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_logo");

			m_pl_tip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_tip");

			m_lbl_Tip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_tip/lbl_Tip");
			m_lbl_Tip_Shadow = FindUI<Shadow>(vb.transform ,"pl_tip/lbl_Tip");
			m_lbl_Tip_Outline = FindUI<Outline>(vb.transform ,"pl_tip/lbl_Tip");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pb_rogressBar");

			m_lbl_doing_LanguageText = FindUI<LanguageText>(vb.transform ,"pb_rogressBar/lbl_doing");

			m_pl_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pb_rogressBar/pl_view");

			m_UI_Item_LoadingAni = new UI_Item_LoadingAni_SubView(FindUI<RectTransform>(vb.transform ,"pb_rogressBar/pl_view/UI_Item_LoadingAni"));
			m_lbl_version_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_version");
			m_lbl_version_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_version");

			m_btn_service_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_service");
			m_btn_service_GameButton = FindUI<GameButton>(vb.transform ,"btn_service");
			m_btn_service_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_service");


            LoadingMediator mt = new LoadingMediator(vb.gameObject);
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
