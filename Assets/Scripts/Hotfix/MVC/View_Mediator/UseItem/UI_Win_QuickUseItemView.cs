// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_Win_QuickUseItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_QuickUseItemView : GameView
    {
        public const string VIEW_NAME = "UI_Win_QuickUseItem";

        public UI_Win_QuickUseItemView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_res;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_use;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;



        private void UIFinder()
        {
			m_pl_res = FindUI<RectTransform>(vb.transform ,"pl_res");
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/UI_Model_Item"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_res/lbl_name");

			m_btn_use = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/btn_use"));
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_res/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_res/btn_close");


            UI_Win_QuickUseItemMediator mt = new UI_Win_QuickUseItemMediator(vb.gameObject);
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
