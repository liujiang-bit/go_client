// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, 03 November 2020
// Update Time         :    Tuesday, 03 November 2020
// Class Description   :    GameToolView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class GameToolView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GameTool";

        public GameToolView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Image m_pl_mes_Image;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_rid_LanguageText;

		[HideInInspector] public PolygonImage m_ls_template_PolygonImage;
		[HideInInspector] public GameButton m_ls_template_GameButton;
		[HideInInspector] public BtnAnimation m_ls_template_BtnAnimation;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public PolygonImage m_img_test_PolygonImage;



        private void UIFinder()
        {
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/img_polygonImage");

			m_lbl_rid_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/img_polygonImage/lbl_rid");

			m_ls_template_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/Scroll View/Viewport/Content/ls_template");
			m_ls_template_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/Scroll View/Viewport/Content/ls_template");
			m_ls_template_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/Scroll View/Viewport/Content/ls_template");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/btn_close");

			m_img_test_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/img_test");


            GameToolMediator mt = new GameToolMediator(vb.gameObject);
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
