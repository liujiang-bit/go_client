// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, April 17, 2020
// Update Time         :    Friday, April 17, 2020
// Class Description   :    UI_Win_GuildDissolutionView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildDissolutionView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildDissolution";

        public UI_Win_GuildDissolutionView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public PolygonImage m_ipt_mes_PolygonImage;
		[HideInInspector] public GameInput m_ipt_mes_GameInput;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_des_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_cancel;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_dissolution;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_ipt_mes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_mes");
			m_ipt_mes_GameInput = FindUI<GameInput>(vb.transform ,"ipt_mes");

			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_des");
			m_lbl_des_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_des");

			m_btn_cancel = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"btn_cancel"));
			m_btn_dissolution = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"btn_dissolution"));

            UI_Win_GuildDissolutionMediator mt = new UI_Win_GuildDissolutionMediator(vb.gameObject);
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
