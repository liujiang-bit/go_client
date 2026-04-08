// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月30日
// Update Time         :    2020年3月30日
// Class Description   :    UI_Pop_TextOnBuildingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_TextOnBuildingView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_TextOnBuilding";

        public UI_Pop_TextOnBuildingView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_pl_size_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public PolygonImage m_pl_arrow_PolygonImage;

		[HideInInspector] public GameButton m_btn_click_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_offset = FindUI<RectTransform>(vb.transform ,"pl_offset");
			m_pl_size_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_size");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_offset/pl_size/lbl_languageText");

			m_pl_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow");

			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"pl_offset/btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_offset/btn_click");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}