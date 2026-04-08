// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildingLevelData_four_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_BuildingLevelData_four_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildingLevelData_four";

        public UI_Item_BuildingLevelData_four_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BuildingLevelData_four;
		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name4_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name5_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_BuildingLevelData_four = gameObject.GetComponent<RectTransform>();
			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name1");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name2");

			m_lbl_name3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name3");

			m_lbl_name4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name4");

			m_lbl_name5_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name5");


			BindEvent();
        }

        #endregion
    }
}