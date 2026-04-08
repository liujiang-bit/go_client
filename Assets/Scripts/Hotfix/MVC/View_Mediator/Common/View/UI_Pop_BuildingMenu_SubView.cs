// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_BuildingMenu_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Pop_BuildingMenu_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_BuildingMenu";

        public UI_Pop_BuildingMenu_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Pop_BuildingMenu_ViewBinder;

		[HideInInspector] public RectTransform m_pl_Title;
		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public LanguageText m_lbl_buildingName_LanguageText;

		[HideInInspector] public UI_Item_CMDBtns_SubView m_UI_Item_FeatureBtns;


        private void UIFinder()
        {       
			m_UI_Pop_BuildingMenu_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_Title = FindUI<RectTransform>(gameObject.transform ,"pl_Title");
			m_lbl_level_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Title/levelBg/lbl_level");

			m_lbl_buildingName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Title/flag/lbl_buildingName");

			m_UI_Item_FeatureBtns = new UI_Item_CMDBtns_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_FeatureBtns"));

			BindEvent();
        }

        #endregion
    }
}