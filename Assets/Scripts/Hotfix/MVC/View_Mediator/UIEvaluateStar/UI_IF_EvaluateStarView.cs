// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_IF_EvaluateStarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_EvaluateStarView : GameView
    {
        public const string VIEW_NAME = "UI_IF_EvaluateStar";

        public UI_IF_EvaluateStarView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Later;
		[HideInInspector] public UI_Model_StandardButton_Yellow_big_SubView m_UI_Great;


        private void UIFinder()
        {
			m_UI_Later = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"flag/UI_Later"));
			m_UI_Great = new UI_Model_StandardButton_Yellow_big_SubView(FindUI<RectTransform>(vb.transform ,"flag/UI_Great"));

            UI_IF_EvaluateStarMediator mt = new UI_IF_EvaluateStarMediator(vb.gameObject);
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
