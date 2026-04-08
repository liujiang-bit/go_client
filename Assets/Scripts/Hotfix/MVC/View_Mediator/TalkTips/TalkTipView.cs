// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月12日
// Update Time         :    2020年8月12日
// Class Description   :    TalkTipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class TalkTipView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_TalkTip";

        public TalkTipView () 
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

		[HideInInspector] public PolygonImage m_img_arrowSideBR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideBL_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_PopAnime_Talk_SubView m_UI_Tag_PopAnime_Talk;


        private void UIFinder()
        {
			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideBR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideBR");

			m_img_arrowSideBL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideBL");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/lbl_text");

			m_UI_Tag_PopAnime_Talk = new UI_Tag_PopAnime_Talk_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/UI_Tag_PopAnime_Talk"));

            TalkTipMediator mt = new TalkTipMediator(vb.gameObject);
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
