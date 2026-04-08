// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildResearchSubItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildResearchSubItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildResearchSubItem";

        public UI_Item_GuildResearchSubItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_GuildResearchSubItem_ViewBinder;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_line_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_line2_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_line2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_research_PolygonImage;
		[HideInInspector] public GameButton m_btn_research_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_research_ArabLayoutCompment;
		[HideInInspector] public GrayChildrens m_btn_research_MakeChildrenGray;

		[HideInInspector] public PolygonImage m_img_initiativeSkill_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_initiativeSkill_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_skillFrame_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_skillFrame_ArabLayoutCompment;

		[HideInInspector] public UI_10014_SubView m_pl_effect;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_mark_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_GuildResearchSubItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_line");
			m_img_line_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_line");

			m_img_line2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_line2");
			m_img_line2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_line2");

			m_btn_research_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research");
			m_btn_research_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_research");
			m_btn_research_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_research");
			m_btn_research_MakeChildrenGray = FindUI<GrayChildrens>(gameObject.transform ,"btn_research");

			m_img_initiativeSkill_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research/img_initiativeSkill");
			m_img_initiativeSkill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_research/img_initiativeSkill");

			m_img_skillFrame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research/img_skillFrame");
			m_img_skillFrame_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_research/img_skillFrame");

			m_pl_effect = new UI_10014_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_research/img_skillFrame/pl_effect"));
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_research/img_skillFrame/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_research/img_skillFrame/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_pb_bar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pb_bar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pb_bar/lbl_barText");

			m_pl_mark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mark");


			BindEvent();
        }

        #endregion
    }
}