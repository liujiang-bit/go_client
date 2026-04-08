// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月27日
// Update Time         :    2020年7月27日
// Class Description   :    UI_Pop_TextOnTavernView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_TextOnTavernView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_TextOnTavern";

        public UI_Pop_TextOnTavernView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_pl_size_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public PolygonImage m_pl_arrow_PolygonImage;

		[HideInInspector] public GameButton m_btn_click_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;



        private void UIFinder()
        {
			m_pl_offset = FindUI<RectTransform>(vb.transform ,"pl_offset");
			m_pl_size_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_size");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_offset/pl_size/lbl_languageText");

			m_pl_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow");

			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"pl_offset/btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_offset/btn_click");


            UI_Pop_TextOnTavernMediator mt = new UI_Pop_TextOnTavernMediator(vb.gameObject);
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
