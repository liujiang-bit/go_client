// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Pop_FightVictoryView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_FightVictoryView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_FightVictory";

        public UI_Pop_FightVictoryView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public Outline m_lbl_text_Outline;
		[HideInInspector] public Shadow m_lbl_text_Shadow;



        private void UIFinder()
        {
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_text");
			m_lbl_text_Outline = FindUI<Outline>(vb.transform ,"lbl_text");
			m_lbl_text_Shadow = FindUI<Shadow>(vb.transform ,"lbl_text");


            UI_Pop_FightVictoryMediator mt = new UI_Pop_FightVictoryMediator(vb.gameObject);
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
