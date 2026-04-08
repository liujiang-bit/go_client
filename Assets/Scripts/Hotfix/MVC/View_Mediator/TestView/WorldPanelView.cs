// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月9日
// Update Time         :    2019年12月9日
// Class Description   :    WorldPanelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class WorldPanelView : GameView
    {
        public const string VIEW_NAME = "WorldPanel";

        public WorldPanelView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Image m_m_btn_home_Image;
		[HideInInspector] public Button m_m_btn_home_Button;

		[HideInInspector] public Text m_m_WorldPos_Text;

		[HideInInspector] public Image m_m_btn_test1_Image;
		[HideInInspector] public Button m_m_btn_test1_Button;



        private void UIFinder()
        {
			m_m_btn_home_Image = FindUI<Image>(vb.transform ,"m_btn_home");
			m_m_btn_home_Button = FindUI<Button>(vb.transform ,"m_btn_home");

			m_m_WorldPos_Text = FindUI<Text>(vb.transform ,"m_WorldPos");

			m_m_btn_test1_Image = FindUI<Image>(vb.transform ,"m_btn_test1");
			m_m_btn_test1_Button = FindUI<Button>(vb.transform ,"m_btn_test1");


            WorldPanelMediator mt = new WorldPanelMediator(vb.gameObject);
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
