// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 30, 2020
// Update Time         :    Thursday, April 30, 2020
// Class Description   :    UI_Win_GuildAppointView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildAppointView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildAppoint";

        public UI_Win_GuildAppointView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_ok;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_list/v_list_view/c_list_view");
			m_UI_ok = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_ok"));

            UI_Win_GuildAppointMediator mt = new UI_Win_GuildAppointMediator(vb.gameObject);
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
