// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    UI_Win_CivilizationChangeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_CivilizationChangeView : GameView
    {
        public const string VIEW_NAME = "UI_Win_CivilizationChange";

        public UI_Win_CivilizationChangeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public ScrollRect m_sv_mes_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_mes_PolygonImage;
		[HideInInspector] public ListView m_sv_mes_ListView;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_mes_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_sure;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_sv_mes_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/sv_mes");
			m_sv_mes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/sv_mes");
			m_sv_mes_ListView = FindUI<ListView>(vb.transform ,"pl_mes/sv_mes");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/sv_mes/v/c/lbl_mes");
			m_lbl_mes_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/sv_mes/v/c/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/sv_mes/v/c/lbl_mes");

			m_btn_sure = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/btn_sure"));

            UI_Win_CivilizationChangeMediator mt = new UI_Win_CivilizationChangeMediator(vb.gameObject);
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

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_eventDate);
        }

    }
}
