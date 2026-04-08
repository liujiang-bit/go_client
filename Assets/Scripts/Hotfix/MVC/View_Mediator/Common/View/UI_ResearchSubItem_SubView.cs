// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_ResearchSubItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_ResearchSubItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_ResearchSubItem";

        public UI_ResearchSubItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_ResearchSubItem_ViewBinder;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;

		[HideInInspector] public PolygonImage m_img_line2_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_research_PolygonImage;
		[HideInInspector] public GameButton m_btn_research_GameButton;

		[HideInInspector] public PolygonImage m_img_iconBg_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_iconBg_MakeChildrenGray;

		[HideInInspector] public UI_10014_SubView m_pl_effect;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public GameSlider m_pb_bar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;



        private void UIFinder()
        {       
			m_UI_ResearchSubItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_line");

			m_img_line2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_line2");

			m_btn_research_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research");
			m_btn_research_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_research");

			m_img_iconBg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research/img_iconBg");
			m_img_iconBg_MakeChildrenGray = FindUI<GrayChildrens>(gameObject.transform ,"btn_research/img_iconBg");

			m_pl_effect = new UI_10014_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_research/img_iconBg/pl_effect"));
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research/img_iconBg/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_pb_bar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pb_bar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pb_bar/lbl_barText");


			BindEvent();
        }

        #endregion
    }
}