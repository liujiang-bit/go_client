// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月10日
// Update Time         :    2020年8月10日
// Class Description   :    UI_IF_TaskChapterView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_TaskChapterView : GameView
    {
        public const string VIEW_NAME = "UI_IF_TaskChapter";

        public UI_IF_TaskChapterView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_view_Animator;

		[HideInInspector] public PolygonImage m_img_chapter_PolygonImage;
		[HideInInspector] public UIMaterial m_img_chapter_UIMaterial;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public Outline m_lbl_text_Outline;



        private void UIFinder()
        {
			m_pl_view_Animator = FindUI<Animator>(vb.transform ,"pl_view");

			m_img_chapter_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_view/img_chapter");
			m_img_chapter_UIMaterial = FindUI<UIMaterial>(vb.transform ,"pl_view/img_chapter");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_view/lbl_text");
			m_lbl_text_Outline = FindUI<Outline>(vb.transform ,"pl_view/lbl_text");


            UI_IF_TaskChapterMediator mt = new UI_IF_TaskChapterMediator(vb.gameObject);
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
