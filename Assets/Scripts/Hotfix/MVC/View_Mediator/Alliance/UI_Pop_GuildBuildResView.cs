// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 22, 2020
// Update Time         :    Wednesday, April 22, 2020
// Class Description   :    UI_Pop_GuildBuildResView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_GuildBuildResView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_GuildBuildRes";

        public UI_Pop_GuildBuildResView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_res_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res1;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res2;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res3;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res4;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res5;


        private void UIFinder()
        {
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_title");

			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_res");
			m_pl_res_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_res");

			m_pl_res1 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_res/pl_res1"));
			m_pl_res2 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_res/pl_res2"));
			m_pl_res3 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_res/pl_res3"));
			m_pl_res4 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_res/pl_res4"));
			m_pl_res5 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_res/pl_res5"));

            UI_Pop_GuildBuildResMediator mt = new UI_Pop_GuildBuildResMediator(vb.gameObject);
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
