// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月7日
// Update Time         :    2020年1月7日
// Class Description   :    ItemBuildingLevelDataSixView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemBuildingLevelDataSixView : GameView
    {
		public const string VIEW_NAME = "UI_Item_BuildingLevelData_Six";

        public ItemBuildingLevelDataSixView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name4_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name5_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name6_LanguageText;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_name1_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name1");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name2");

			m_lbl_name3_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name3");

			m_lbl_name4_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name4");

			m_lbl_name5_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name5");

			m_lbl_name6_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name6");

			m_img_select_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_select");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}