// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Item_GuildFeatureBtnsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildFeatureBtnsView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildFeatureBtns";

        public UI_Item_GuildFeatureBtnsView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_bg");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_name");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}