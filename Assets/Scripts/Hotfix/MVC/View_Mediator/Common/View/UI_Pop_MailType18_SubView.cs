// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_MailType18_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Pop_MailType18_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_MailType18";

        public UI_Pop_MailType18_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Pop_MailType18;
		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;
		[HideInInspector] public ArabLayoutCompment m_pl_pos_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_mail;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_btn_remove;


        private void UIFinder()
        {       
			m_UI_Pop_MailType18 = gameObject.GetComponent<RectTransform>();
			m_pl_pos_Animator = FindUI<Animator>(gameObject.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_pos");
			m_pl_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_btn_mail = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_pos/btn_mail"));
			m_btn_remove = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_pos/btn_remove"));

			BindEvent();
        }

        #endregion
    }
}