// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildingUpLimit_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_BuildingUpLimit_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildingUpLimit";

        public UI_Item_BuildingUpLimit_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_BuildingUpLimit_ViewBinder;

		[HideInInspector] public RectTransform m_pl_TechIcon;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_StandardButton_Blue;


        private void UIFinder()
        {       
			m_UI_Item_BuildingUpLimit_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_TechIcon = FindUI<RectTransform>(gameObject.transform ,"pl_TechIcon");
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_languageText");

			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_StandardButton_Blue"));

			BindEvent();
        }

        #endregion
    }
}