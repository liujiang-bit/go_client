// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildingLevelData_two_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_BuildingLevelData_two_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildingLevelData_two";

        public UI_Item_BuildingLevelData_two_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BuildingLevelData_two;
		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name3_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_BuildingLevelData_two = gameObject.GetComponent<RectTransform>();
			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name1");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name2");

			m_lbl_name3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name3");


			BindEvent();
        }

        #endregion
    }
}