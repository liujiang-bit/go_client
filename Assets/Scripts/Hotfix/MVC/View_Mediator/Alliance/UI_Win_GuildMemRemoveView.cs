// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, April 10, 2020
// Update Time         :    Friday, April 10, 2020
// Class Description   :    UI_Win_GuildMemRemoveView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildMemRemoveView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildMemRemove";

        public UI_Win_GuildMemRemoveView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public GridLayoutGroup m_pl_cks_GridLayoutGroup;
		[HideInInspector] public ToggleGroup m_pl_cks_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_box1_GameToggle;

		[HideInInspector] public GameToggle m_ck_box2_GameToggle;

		[HideInInspector] public GameToggle m_ck_box3_GameToggle;

		[HideInInspector] public GameToggle m_ck_box4_GameToggle;

		[HideInInspector] public GameToggle m_ck_box5_GameToggle;

		[HideInInspector] public GameToggle m_ck_box6_GameToggle;

		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_Blue;
		[HideInInspector] public UI_Model_StandardButton_Red_SubView m_UI_Red;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_cks_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_cks");
			m_pl_cks_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"rect/pl_cks");

			m_ck_box1_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_cks/ck_box1");

			m_ck_box2_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_cks/ck_box2");

			m_ck_box3_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_cks/ck_box3");

			m_ck_box4_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_cks/ck_box4");

			m_ck_box5_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_cks/ck_box5");

			m_ck_box6_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_cks/ck_box6");

			m_UI_Blue = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/UI_Blue"));
			m_UI_Red = new UI_Model_StandardButton_Red_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/UI_Red"));

            UI_Win_GuildMemRemoveMediator mt = new UI_Win_GuildMemRemoveMediator(vb.gameObject);
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
