// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Item_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Item";

        public UI_Model_Item_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_Item_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_animButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_animButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_animButton_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_quality_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_quality_GrayChildrens;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_icon_GrayChildrens;

		[HideInInspector] public PolygonImage m_pl_desc_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public Outline m_lbl_desc_Outline;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public Shadow m_lbl_count_Shadow;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_Item_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_animButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton");
			m_btn_animButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_animButton");
			m_btn_animButton_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_animButton");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/img_bg");

			m_img_quality_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/img_quality");
			m_img_quality_GrayChildrens = FindUI<GrayChildrens>(gameObject.transform ,"btn_animButton/img_quality");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/img_icon");
			m_img_icon_GrayChildrens = FindUI<GrayChildrens>(gameObject.transform ,"btn_animButton/img_icon");

			m_pl_desc_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/pl_desc_bg");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/pl_desc_bg/lbl_desc");
			m_lbl_desc_Outline = FindUI<Outline>(gameObject.transform ,"btn_animButton/pl_desc_bg/lbl_desc");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/lbl_count");
			m_lbl_count_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_animButton/lbl_count");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/img_select");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/img_redpoint");


			BindEvent();
        }

        #endregion
    }
}