// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月19日
// Update Time         :    2019年11月19日
// Class Description   :    TestUIView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;

namespace Game {
    public class TestUIView : GameView
    {
        public const string VIEW_NAME = "TestUI";

        public TestUIView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Image m_btn_close_Image;
		[HideInInspector] public Button m_btn_close_Button;

		[HideInInspector] public Image m_btn_sound_Image;
		[HideInInspector] public Button m_btn_sound_Button;

		[HideInInspector] public Image m_btn_play_spine_Image;
		[HideInInspector] public Button m_btn_play_spine_Button;



        private void UIFinder()
        {
			m_btn_close_Image = FindUI<Image>(vb.transform ,"btn_close");
			m_btn_close_Button = FindUI<Button>(vb.transform ,"btn_close");

			m_btn_sound_Image = FindUI<Image>(vb.transform ,"Panel/btn_sound");
			m_btn_sound_Button = FindUI<Button>(vb.transform ,"Panel/btn_sound");

			m_btn_play_spine_Image = FindUI<Image>(vb.transform ,"Panel/btn_play_spine");
			m_btn_play_spine_Button = FindUI<Button>(vb.transform ,"Panel/btn_play_spine");


            TestUIMediator mt = new TestUIMediator(vb.gameObject);
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
