// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月20日
// Update Time         :    2020年7月20日
// Class Description   :    StrongerPlayerRankView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class StrongerPlayerRankView : GameView
    {
        public const string VIEW_NAME = "UI_Win_StrongerPlayerRank";

        public StrongerPlayerRankView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public LanguageText m_lbl_no_rank_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;



        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_lbl_no_rank_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_no_rank");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_close");


            StrongerPlayerRankMediator mt = new StrongerPlayerRankMediator(vb.gameObject);
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
