// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月1日
// Update Time         :    2020年7月1日
// Class Description   :    UI_Win_ExpeditionRuleView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ExpeditionRuleView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ExpeditionRule";

        public UI_Win_ExpeditionRuleView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_content_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_pl_view = FindUI<RectTransform>(vb.transform ,"pl_view");
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_view/lbl_title");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_view/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_view/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_view/sv_list");

			m_lbl_content_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_view/sv_list/v/c/lbl_content");
			m_lbl_content_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_view/sv_list/v/c/lbl_content");
			m_lbl_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_view/sv_list/v/c/lbl_content");


            UI_Win_ExpeditionRuleMediator mt = new UI_Win_ExpeditionRuleMediator(vb.gameObject);
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
