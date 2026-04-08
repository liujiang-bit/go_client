// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_Win_MysteryStoreGoneView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_MysteryStoreGoneView : GameView
    {
        public const string VIEW_NAME = "UI_Win_MysteryStoreGone";

        public UI_Win_MysteryStoreGoneView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_Close_PolygonImage;
		[HideInInspector] public GameButton m_btn_Close_GameButton;

		[HideInInspector] public SkeletonGraphic m_pl_char_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_char_ArabLayoutCompment;



        private void UIFinder()
        {
			m_btn_Close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_Close");
			m_btn_Close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_Close");

			m_pl_char_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"rect/pl_char");
			m_pl_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char");


            UI_Win_MysteryStoreGoneMediator mt = new UI_Win_MysteryStoreGoneMediator(vb.gameObject);
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
