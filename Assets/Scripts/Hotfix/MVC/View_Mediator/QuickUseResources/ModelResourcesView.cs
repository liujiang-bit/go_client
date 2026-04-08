// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月27日
// Update Time         :    2019年12月27日
// Class Description   :    ModelResourcesView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ModelResourcesView : GameView
    {
		public const string VIEW_NAME = "UI_Model_Resources";

        public ModelResourcesView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_val_LanguageText;

		[HideInInspector] public PolygonImage m_img_add_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_lbl_val_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_val");

			m_img_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_add");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}