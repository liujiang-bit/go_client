// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildingLevelData_three_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_BuildingLevelData_three_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildingLevelData_three";

        public UI_Item_BuildingLevelData_three_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_BuildingLevelData_three_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name4_LanguageText;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_BuildingLevelData_three_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name1");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name2");

			m_lbl_name3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name3");

			m_lbl_name4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name4");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");


			BindEvent();
        }

        #endregion
    }
}