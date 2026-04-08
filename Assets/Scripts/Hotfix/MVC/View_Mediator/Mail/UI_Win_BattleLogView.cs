// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月3日
// Update Time         :    2020年7月3日
// Class Description   :    UI_Win_BattleLogView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_BattleLogView : GameView
    {
        public const string VIEW_NAME = "UI_Win_BattleLog";

        public UI_Win_BattleLogView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public PolygonImage m_img_Bg_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_img_Bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_Bg");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"img_Bg/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_Bg/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"img_Bg/sv_list");


            UI_Win_BattleLogMediator mt = new UI_Win_BattleLogMediator(vb.gameObject);
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
