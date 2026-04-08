// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月1日
// Update Time         :    2020年7月1日
// Class Description   :    UI_Win_GameToolViewSettingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GameToolViewSettingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GameToolViewSetting";

        public UI_Win_GameToolViewSettingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Image m_pl_mes_Image;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public GridLayoutGroup m_glg_content_GridLayoutGroup;

		[HideInInspector] public Image m_btn_confirm_Image;
		[HideInInspector] public Button m_btn_confirm_Button;

		[HideInInspector] public Toggle m_ck_viewtype_Toggle;



        private void UIFinder()
        {
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/btn_close");

			m_glg_content_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_mes/glg_content");

			m_btn_confirm_Image = FindUI<Image>(vb.transform ,"pl_mes/btn_confirm");
			m_btn_confirm_Button = FindUI<Button>(vb.transform ,"pl_mes/btn_confirm");

			m_ck_viewtype_Toggle = FindUI<Toggle>(vb.transform ,"pl_mes/ck_viewtype");


            UI_Win_GameToolViewSettingMediator mt = new UI_Win_GameToolViewSettingMediator(vb.gameObject);
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
