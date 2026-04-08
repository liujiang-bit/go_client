// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月30日
// Update Time         :    2020年9月30日
// Class Description   :    UI_Win_GuideAnimView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_GuideAnimView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuideAnim";

        public UI_Win_GuideAnimView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_title_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuideAnim_SubView m_UI_Model_GuideAnim;


        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/lbl_title");
			m_lbl_title_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/lbl_title");

			m_UI_Model_GuideAnim = new UI_Model_GuideAnim_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/UI_Model_GuideAnim"));

            UI_Win_GuideAnimMediator mt = new UI_Win_GuideAnimMediator(vb.gameObject);
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
