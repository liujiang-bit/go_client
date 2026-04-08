// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_MailGetNew_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Pop_MailGetNew_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_MailGetNew";

        public UI_Pop_MailGetNew_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Pop_MailGetNew_ArabLayoutCompment;
		[HideInInspector] public ViewBinder m_UI_Pop_MailGetNew_ViewBinder;
		[HideInInspector] public CanvasGroup m_UI_Pop_MailGetNew_CanvasGroup;
		[HideInInspector] public Animator m_UI_Pop_MailGetNew_Animator;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_click_PolygonImage;
		[HideInInspector] public GameButton m_btn_click_GameButton;



        private void UIFinder()
        {       
			m_UI_Pop_MailGetNew_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_Pop_MailGetNew_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Pop_MailGetNew_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
			m_UI_Pop_MailGetNew_Animator = gameObject.GetComponent<Animator>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"offset/bg/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"offset/bg/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"offset/bg/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"offset/bg/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"offset/bg/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"offset/bg/lbl_desc");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"offset/bg/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"offset/bg/img_arrow");

			m_btn_click_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"offset/bg/btn_click");
			m_btn_click_GameButton = FindUI<GameButton>(gameObject.transform ,"offset/bg/btn_click");


			BindEvent();
        }

        #endregion
    }
}