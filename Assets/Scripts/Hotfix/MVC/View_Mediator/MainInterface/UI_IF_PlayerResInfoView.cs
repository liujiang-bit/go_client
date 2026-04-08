// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年11月3日
// Update Time         :    2020年11月3日
// Class Description   :    UI_IF_PlayerResInfoView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_IF_PlayerResInfoView : GameView
    {
        public const string VIEW_NAME = "UI_IF_PlayerResInfo";

        public UI_IF_PlayerResInfoView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Item_PlayerPowerInfo_SubView m_UI_Item_PlayerPowerInfo;
		[HideInInspector] public ArabLayoutCompment m_pl_resources_set_ArabLayoutCompment;

		[HideInInspector] public UI_Item_PlayerResources_SubView m_UI_Item_PlayerResources;


        private void UIFinder()
        {
			m_UI_Item_PlayerPowerInfo = new UI_Item_PlayerPowerInfo_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_PlayerPowerInfo"));
			m_pl_resources_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_resources_set");

			m_UI_Item_PlayerResources = new UI_Item_PlayerResources_SubView(FindUI<RectTransform>(vb.transform ,"pl_resources_set/UI_Item_PlayerResources"));

            UI_IF_PlayerResInfoMediator mt = new UI_IF_PlayerResInfoMediator(vb.gameObject);
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
