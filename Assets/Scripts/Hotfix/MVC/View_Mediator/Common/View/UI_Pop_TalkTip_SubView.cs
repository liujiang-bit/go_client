// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_TalkTip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Pop_TalkTip_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_TalkTip";

        public UI_Pop_TalkTip_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Pop_TalkTip_ViewBinder;

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
			m_UI_Pop_TalkTip_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_pos_Animator = FindUI<Animator>(gameObject.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideBR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideBR");

			m_img_arrowSideBL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideBL");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/img_bg/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/img_bg/lbl_text");

			m_UI_Tag_PopAnime_Talk = new UI_Tag_PopAnime_Talk_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_pos/UI_Tag_PopAnime_Talk"));

			BindEvent();
        }

        #endregion
    }
}