// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 29 October 2020
// Update Time         :    Thursday, 29 October 2020
// Class Description   :    GeneralSettingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class GeneralSettingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GeneralSetting";

        public GeneralSettingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public ScrollRect m_sv_scroll_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_scroll_view_PolygonImage;
		[HideInInspector] public ListView m_sv_scroll_view_ListView;

		[HideInInspector] public PolygonImage m_v_scroll_view_PolygonImage;
		[HideInInspector] public Mask m_v_scroll_view_Mask;

		[HideInInspector] public RectTransform m_c_scroll_view;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_sv_scroll_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_scroll_view");
			m_sv_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_scroll_view");
			m_sv_scroll_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_scroll_view");

			m_v_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_scroll_view/v_scroll_view");
			m_v_scroll_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_scroll_view/v_scroll_view");

			m_c_scroll_view = FindUI<RectTransform>(vb.transform ,"rect/sv_scroll_view/v_scroll_view/c_scroll_view");

            GeneralSettingMediator mt = new GeneralSettingMediator(vb.gameObject);
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
