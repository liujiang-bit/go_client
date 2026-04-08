// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Win_LoginMainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_LoginMainView : GameView
    {
        public const string VIEW_NAME = "UI_Win_LoginMain";

        public UI_Win_LoginMainView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public NbUIStrench m_img_bg_NbUIStrench;

		[HideInInspector] public UI_Model_StandardButton_Yellow_big_SubView m_UI_MainLogin;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_OtherLogin;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_NbUIStrench = FindUI<NbUIStrench>(vb.transform ,"img_bg");

			m_UI_MainLogin = new UI_Model_StandardButton_Yellow_big_SubView(FindUI<RectTransform>(vb.transform ,"UI_MainLogin"));
			m_UI_OtherLogin = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"UI_OtherLogin"));

            UI_Win_LoginMainMediator mt = new UI_Win_LoginMainMediator(vb.gameObject);
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
