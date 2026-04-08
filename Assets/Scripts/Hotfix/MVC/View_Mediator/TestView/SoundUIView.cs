// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月13日
// Update Time         :    2019年11月13日
// Class Description   :    SoundUIView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;

namespace Game {
    public class SoundUIView : GameView
    {
        public const string VIEW_NAME = "SoundUI";

        public SoundUIView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Image m_btn_close_Image;
		[HideInInspector] public Button m_btn_close_Button;

		[HideInInspector] public Image m_btn_bgm1_Image;
		[HideInInspector] public Button m_btn_bgm1_Button;

		[HideInInspector] public Image m_btn_bgm2_Image;
		[HideInInspector] public Button m_btn_bgm2_Button;

		[HideInInspector] public Image m_btn_vfx1_Image;
		[HideInInspector] public Button m_btn_vfx1_Button;

		[HideInInspector] public Image m_btn_vfx2_Image;
		[HideInInspector] public Button m_btn_vfx2_Button;



        private void UIFinder()
        {
			m_btn_close_Image = FindUI<Image>(vb.transform ,"btn_close");
			m_btn_close_Button = FindUI<Button>(vb.transform ,"btn_close");

			m_btn_bgm1_Image = FindUI<Image>(vb.transform ,"btn_bgm1");
			m_btn_bgm1_Button = FindUI<Button>(vb.transform ,"btn_bgm1");

			m_btn_bgm2_Image = FindUI<Image>(vb.transform ,"btn_bgm2");
			m_btn_bgm2_Button = FindUI<Button>(vb.transform ,"btn_bgm2");

			m_btn_vfx1_Image = FindUI<Image>(vb.transform ,"btn_vfx1");
			m_btn_vfx1_Button = FindUI<Button>(vb.transform ,"btn_vfx1");

			m_btn_vfx2_Image = FindUI<Image>(vb.transform ,"btn_vfx2");
			m_btn_vfx2_Button = FindUI<Button>(vb.transform ,"btn_vfx2");


            SoundUIMediator mt = new SoundUIMediator(vb.gameObject);
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
