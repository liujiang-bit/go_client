// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, 30 September 2020
// Update Time         :    Wednesday, 30 September 2020
// Class Description   :    UI_Pop_GuideAnimView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Pop_GuideAnimView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_GuideAnim";

        public UI_Pop_GuideAnimView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_title_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuideAnim_SubView m_UI_Model_GuideAnim;


        private void UIFinder()
        {
			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/lbl_title");
			m_lbl_title_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/lbl_title");

			m_UI_Model_GuideAnim = new UI_Model_GuideAnim_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/UI_Model_GuideAnim"));

            UI_Pop_GuideAnimMediator mt = new UI_Pop_GuideAnimMediator(vb.gameObject);
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
