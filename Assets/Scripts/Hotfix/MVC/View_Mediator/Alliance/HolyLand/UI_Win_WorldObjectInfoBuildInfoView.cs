// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月16日
// Update Time         :    2020年5月16日
// Class Description   :    UI_Win_WorldObjectInfoBuildInfoView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_WorldObjectInfoBuildInfoView : GameView
    {
        public const string VIEW_NAME = "UI_Win_WorldObjectInfoBuildInfo";

        public UI_Win_WorldObjectInfoBuildInfoView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_pl_window;
		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;



        private void UIFinder()
        {
			m_pl_window = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"pl_window"));
			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/sv/v/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/sv/v/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv/v/lbl_desc");


            UI_Win_WorldObjectInfoBuildInfoMediator mt = new UI_Win_WorldObjectInfoBuildInfoMediator(vb.gameObject);
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
