// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Common_ScrollMessageView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Common_ScrollMessageView : GameView
    {
        public const string VIEW_NAME = "UI_Common_ScrollMessage";

        public UI_Common_ScrollMessageView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_offset_Animator;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public RectMask2D m_img_bg_RectMask2D;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_content_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_start_PolygonImage;
		[HideInInspector] public SortingGroup m_img_start_SortingGroup;

		[HideInInspector] public PolygonImage m_img_end_PolygonImage;
		[HideInInspector] public SortingGroup m_img_end_SortingGroup;



        private void UIFinder()
        {
			m_pl_offset_Animator = FindUI<Animator>(vb.transform ,"pl_offset");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/img_bg");
			m_img_bg_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_offset/img_bg");

			m_lbl_content_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_offset/img_bg/lbl_content");
			m_lbl_content_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_offset/img_bg/lbl_content");

			m_img_start_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/img_start");
			m_img_start_SortingGroup = FindUI<SortingGroup>(vb.transform ,"pl_offset/img_start");

			m_img_end_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/img_end");
			m_img_end_SortingGroup = FindUI<SortingGroup>(vb.transform ,"pl_offset/img_end");


            UI_Common_ScrollMessageMediator mt = new UI_Common_ScrollMessageMediator(vb.gameObject);
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
