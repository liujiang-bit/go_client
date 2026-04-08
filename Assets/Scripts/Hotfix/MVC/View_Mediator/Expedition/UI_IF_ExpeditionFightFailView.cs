// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月3日
// Update Time         :    2020年9月3日
// Class Description   :    UI_IF_ExpeditionFightFailView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_ExpeditionFightFailView : GameView
    {
        public const string VIEW_NAME = "UI_IF_ExpeditionFightFail";

        public UI_IF_ExpeditionFightFailView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public UI_Item_ExpeditionFightTask_SubView m_UI_Item_ExpeditionFightTask;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_back;
		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_btn_try;


        private void UIFinder()
        {
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_title");

			m_UI_Item_ExpeditionFightTask = new UI_Item_ExpeditionFightTask_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Item_ExpeditionFightTask"));
			m_btn_back = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_back"));
			m_btn_try = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_try"));

            UI_IF_ExpeditionFightFailMediator mt = new UI_IF_ExpeditionFightFailMediator(vb.gameObject);
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
