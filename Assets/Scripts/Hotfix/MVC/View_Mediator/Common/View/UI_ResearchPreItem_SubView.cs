// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_ResearchPreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_ResearchPreItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_ResearchPreItem";

        public UI_ResearchPreItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_ResearchPreItem_ViewBinder;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_jump_PolygonImage;
		[HideInInspector] public GameButton m_btn_jump_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;



        private void UIFinder()
        {       
			m_UI_ResearchPreItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_btn_jump_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_jump");
			m_btn_jump_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_jump");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_lbl_lv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_lv");


			BindEvent();
        }

        #endregion
    }
}