// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Item_HelperItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_HelperItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_HelperItem";

        public UI_Item_HelperItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_title");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}