// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Win_ServerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_ServerView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Server";

        public UI_Win_ServerView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public ScrollRect m_sv_list1_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list1_PolygonImage;
		[HideInInspector] public ListView m_sv_list1_ListView;

		[HideInInspector] public ScrollRect m_sv_list2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list2_PolygonImage;
		[HideInInspector] public ListView m_sv_list2_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_sv_list1_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/sv_list1");
			m_sv_list1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/sv_list1");
			m_sv_list1_ListView = FindUI<ListView>(vb.transform ,"pl_mes/sv_list1");

			m_sv_list2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/sv_list2");
			m_sv_list2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/sv_list2");
			m_sv_list2_ListView = FindUI<ListView>(vb.transform ,"pl_mes/sv_list2");


            UI_Win_ServerMediator mt = new UI_Win_ServerMediator(vb.gameObject);
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
