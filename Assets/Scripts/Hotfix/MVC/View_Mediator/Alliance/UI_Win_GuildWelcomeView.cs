// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, June 17, 2020
// Update Time         :    Wednesday, June 17, 2020
// Class Description   :    UI_Win_GuildWelcomeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildWelcomeView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildWelcome";

        public UI_Win_GuildWelcomeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_btn_creat;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_join;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_bg");

			m_btn_creat = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_creat"));
			m_btn_join = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_join"));

            UI_Win_GuildWelcomeMediator mt = new UI_Win_GuildWelcomeMediator(vb.gameObject);
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
