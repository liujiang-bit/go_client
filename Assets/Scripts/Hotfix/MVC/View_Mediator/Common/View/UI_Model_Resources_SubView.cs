// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Resources_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Resources_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Resources";

        public UI_Model_Resources_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_Resources_ViewBinder;
		[HideInInspector] public Animator m_UI_Model_Resources_Animator;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;
		[HideInInspector] public Animator m_img_icon_Animator;

		[HideInInspector] public LanguageText m_lbl_val_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_val_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_val_Shadow;

		[HideInInspector] public PolygonImage m_img_add_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;



        private void UIFinder()
        {       
			m_UI_Model_Resources_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Model_Resources_Animator = gameObject.GetComponent<Animator>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");
			m_img_icon_Animator = FindUI<Animator>(gameObject.transform ,"img_icon");

			m_lbl_val_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_val");
			m_lbl_val_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_val");
			m_lbl_val_Shadow = FindUI<Shadow>(gameObject.transform ,"lbl_val");

			m_img_add_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_add");
			m_img_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_add");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");


			BindEvent();
        }

        #endregion
    }
}