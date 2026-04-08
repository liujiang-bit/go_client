// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月23日
// Update Time         :    2020年3月23日
// Class Description   :    UI_Win_ScoutView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ScoutView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Scout";

        public UI_Win_ScoutView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public LanguageText m_lbl_Desc_LanguageText;
		[HideInInspector] public CanvasGroup m_lbl_Desc_CanvasGroup;
		[HideInInspector] public Animator m_lbl_Desc_Animator;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;
		[HideInInspector] public Animator m_sv_list_view_Animator;
		[HideInInspector] public CanvasGroup m_sv_list_view_CanvasGroup;

		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_lbl_Desc_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/lbl_Desc");
			m_lbl_Desc_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/lbl_Desc");
			m_lbl_Desc_Animator = FindUI<Animator>(vb.transform ,"Rect/lbl_Desc");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"Rect/sv_list_view");
			m_sv_list_view_Animator = FindUI<Animator>(vb.transform ,"Rect/sv_list_view");
			m_sv_list_view_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/sv_list_view");

			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));

            UI_Win_ScoutMediator mt = new UI_Win_ScoutMediator(vb.gameObject);
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
