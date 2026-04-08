// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Item_HelperView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_HelperView : GameView
    {
		public const string VIEW_NAME = "UI_Item_Helper";

        public UI_Item_HelperView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public PolygonImage m_img_arrowDown_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowUp_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_title");

			m_img_arrowDown_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrowDown");

			m_img_arrowUp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrowUp");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}