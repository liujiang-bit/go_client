// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Win_MonumentView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_MonumentView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Monument";

        public UI_Win_MonumentView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;
		[HideInInspector] public BtnAnimation m_btn_close_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_close_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;



        private void UIFinder()
        {
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_title");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_close");
			m_btn_close_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/btn_close");
			m_btn_close_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/btn_close");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list_view");


            UI_Win_MonumentMediator mt = new UI_Win_MonumentMediator(vb.gameObject);
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
