// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月8日
// Update Time         :    2020年1月8日
// Class Description   :    UI_Item_BuildingUpLimitView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_BuildingUpLimitView : GameView
    {
		public const string VIEW_NAME = "UI_Item_BuildingUpLimit";

        public UI_Item_BuildingUpLimitView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_StandardButton_Blue;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_languageText");

			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_StandardButton_Blue"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}