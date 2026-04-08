// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月3日
// Update Time         :    2020年8月3日
// Class Description   :    UI_IF_ExpeditionFightReadyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_ExpeditionFightReadyView : GameView
    {
        public const string VIEW_NAME = "UI_IF_ExpeditionFightReady";

        public UI_IF_ExpeditionFightReadyView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_ready;
		[HideInInspector] public LanguageText m_lbl_ready_LanguageText;
		[HideInInspector] public Outline m_lbl_ready_Outline;
		[HideInInspector] public Shadow m_lbl_ready_Shadow;

		[HideInInspector] public PolygonImage m_img_ready_PolygonImage;

		[HideInInspector] public RectTransform m_pl_start;
		[HideInInspector] public LanguageText m_lbl_start_LanguageText;
		[HideInInspector] public Outline m_lbl_start_Outline;
		[HideInInspector] public Shadow m_lbl_start_Shadow;

		[HideInInspector] public PolygonImage m_img_start_PolygonImage;



        private void UIFinder()
        {
			m_pl_ready = FindUI<RectTransform>(vb.transform ,"pl_ready");
			m_lbl_ready_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_ready/lbl_ready");
			m_lbl_ready_Outline = FindUI<Outline>(vb.transform ,"pl_ready/lbl_ready");
			m_lbl_ready_Shadow = FindUI<Shadow>(vb.transform ,"pl_ready/lbl_ready");

			m_img_ready_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_ready/img_ready");

			m_pl_start = FindUI<RectTransform>(vb.transform ,"pl_start");
			m_lbl_start_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_start/lbl_start");
			m_lbl_start_Outline = FindUI<Outline>(vb.transform ,"pl_start/lbl_start");
			m_lbl_start_Shadow = FindUI<Shadow>(vb.transform ,"pl_start/lbl_start");

			m_img_start_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_start/img_start");


            UI_IF_ExpeditionFightReadyMediator mt = new UI_IF_ExpeditionFightReadyMediator(vb.gameObject);
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
