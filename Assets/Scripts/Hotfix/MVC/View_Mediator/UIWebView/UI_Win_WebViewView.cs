// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月27日
// Update Time         :    2020年8月27日
// Class Description   :    UI_Win_WebViewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_WebViewView : GameView
    {
        public const string VIEW_NAME = "UI_Win_WebView";

        public UI_Win_WebViewView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_bg;
		[HideInInspector] public Image m_pl_mes_Image;



        private void UIFinder()
        {
			m_bg = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"bg"));
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");


            UI_Win_WebViewMediator mt = new UI_Win_WebViewMediator(vb.gameObject);
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
