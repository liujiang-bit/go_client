// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月26日
// Update Time         :    2020年3月26日
// Class Description   :    UI_Win_GainEffectView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GainEffectView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GainEffect";

        public UI_Win_GainEffectView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public ScrollRect m_sv_list_view2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view2_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view2_ListView;

		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_Side1;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_Side2;


        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/content/sv_list_view");

			m_sv_list_view2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/content/sv_list_view2");
			m_sv_list_view2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/content/sv_list_view2");
			m_sv_list_view2_ListView = FindUI<ListView>(vb.transform ,"rect/content/sv_list_view2");

			m_UI_Model_PageButton_Side1 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"rect/page/UI_Model_PageButton_Side1"));
			m_UI_Model_PageButton_Side2 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"rect/page/UI_Model_PageButton_Side2"));

            UI_Win_GainEffectMediator mt = new UI_Win_GainEffectMediator(vb.gameObject);
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
