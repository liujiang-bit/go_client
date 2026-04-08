// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 29 October 2020
// Update Time         :    Thursday, 29 October 2020
// Class Description   :    BuildingMenuView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class BuildingMenuView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_BuildingMenu";

        public BuildingMenuView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UIDefaultValue m_pl_Title_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_levelBg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public LanguageText m_lbl_buildingName_LanguageText;

		[HideInInspector] public UI_Item_CMDBtns_SubView m_UI_Item_FeatureBtns;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_Title_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_Title");

			m_img_levelBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Title/img_levelBg");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Title/img_levelBg/lbl_level");

			m_lbl_buildingName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Title/flag/lbl_buildingName");

			m_UI_Item_FeatureBtns = new UI_Item_CMDBtns_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_FeatureBtns"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}