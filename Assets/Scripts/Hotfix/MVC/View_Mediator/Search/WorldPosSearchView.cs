// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月3日
// Update Time         :    2020年4月3日
// Class Description   :    WorldPosSearchView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class WorldPosSearchView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_WorldPosSearch";

        public WorldPosSearchView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_PosInput_SubView m_UI_Model_PosInput_server;
		[HideInInspector] public UI_Model_PosInput_SubView m_UI_Model_PosInput_X;
		[HideInInspector] public UI_Model_PosInput_SubView m_UI_Model_PosInput_Y;
		[HideInInspector] public PolygonImage m_btn_search_PolygonImage;
		[HideInInspector] public GameButton m_btn_search_GameButton;
		[HideInInspector] public BtnAnimation m_btn_search_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;



        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_UI_Model_PosInput_server = new UI_Model_PosInput_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_PosInput_server"));
			m_UI_Model_PosInput_X = new UI_Model_PosInput_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_PosInput_X"));
			m_UI_Model_PosInput_Y = new UI_Model_PosInput_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_PosInput_Y"));
			m_btn_search_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_search");
			m_btn_search_GameButton = FindUI<GameButton>(vb.transform ,"btn_search");
			m_btn_search_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_search");

			m_img_img_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_search/img_img");


            WorldPosSearchMediator mt = new WorldPosSearchMediator(vb.gameObject);
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
