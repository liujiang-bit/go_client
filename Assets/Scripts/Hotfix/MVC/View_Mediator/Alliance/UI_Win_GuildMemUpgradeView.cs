// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, April 10, 2020
// Update Time         :    Friday, April 10, 2020
// Class Description   :    UI_Win_GuildMemUpgradeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildMemUpgradeView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildMemUpgrade";

        public UI_Win_GuildMemUpgradeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public ToggleGroup m_pl_group_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_r1_GameToggle;

		[HideInInspector] public GameToggle m_ck_r2_GameToggle;

		[HideInInspector] public GameToggle m_ck_r3_GameToggle;

		[HideInInspector] public GameToggle m_ck_r4_GameToggle;

		[HideInInspector] public GameToggle m_ck_r5_GameToggle;

		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_sure;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_pl_group_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"rect/pl_group");

			m_ck_r1_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/toggle/ck_r1");

			m_ck_r2_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/toggle/ck_r2");

			m_ck_r3_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/toggle/ck_r3");

			m_ck_r4_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/toggle/ck_r4");

			m_ck_r5_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/toggle/ck_r5");

			m_UI_sure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_sure"));

            UI_Win_GuildMemUpgradeMediator mt = new UI_Win_GuildMemUpgradeMediator(vb.gameObject);
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
