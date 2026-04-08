// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 30, 2020
// Update Time         :    Thursday, April 30, 2020
// Class Description   :    UI_Win_GuildInviteMsgView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildInviteMsgView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildInviteMsg";

        public UI_Win_GuildInviteMsgView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_jion;


        private void UIFinder()
        {
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_PlayerHead"));
			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"rect/flag/UI_GuildFlag"));
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_close");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_desc");

			m_UI_jion = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_jion"));

            UI_Win_GuildInviteMsgMediator mt = new UI_Win_GuildInviteMsgMediator(vb.gameObject);
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
