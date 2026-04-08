// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 29 October 2020
// Update Time         :    Thursday, 29 October 2020
// Class Description   :    UI_Win_RewardGetView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_RewardGetView : GameView
    {
        public const string VIEW_NAME = "UI_Win_RewardGet";

        public UI_Win_RewardGetView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_view_Animator;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public UI_Item_GetReward_SubView m_UI_Item_GetReward;
		[HideInInspector] public PolygonImage m_btn_close_1_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_1_GameButton;



        private void UIFinder()
        {
			m_pl_view_Animator = FindUI<Animator>(vb.transform ,"pl_view");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_view/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"pl_view/lbl_title");

			m_UI_Item_GetReward = new UI_Item_GetReward_SubView(FindUI<RectTransform>(vb.transform ,"reward/UI_Item_GetReward"));
			m_btn_close_1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_close_1");
			m_btn_close_1_GameButton = FindUI<GameButton>(vb.transform ,"btn_close_1");


            UI_Win_RewardGetMediator mt = new UI_Win_RewardGetMediator(vb.gameObject);
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
