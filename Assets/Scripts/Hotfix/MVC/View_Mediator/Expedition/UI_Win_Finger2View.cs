// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月11日
// Update Time         :    2020年7月11日
// Class Description   :    UI_Win_Finger2View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_Finger2View : GameView
    {
        public const string VIEW_NAME = "UI_Win_Finger2";

        public UI_Win_Finger2View () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public RectTransform m_pl_arrow;
		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;



        private void UIFinder()
        {
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_pl_arrow = FindUI<RectTransform>(vb.transform ,"pl_content/pl_arrow");
			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_arrow/img_arrow");


            UI_Win_Finger2Mediator mt = new UI_Win_Finger2Mediator(vb.gameObject);
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
