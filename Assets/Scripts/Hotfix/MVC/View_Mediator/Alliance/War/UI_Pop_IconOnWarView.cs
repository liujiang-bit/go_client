// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, July 30, 2020
// Update Time         :    Thursday, July 30, 2020
// Class Description   :    UI_Pop_IconOnWarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_IconOnWarView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_IconOnWar";

        public UI_Pop_IconOnWarView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_pl_arrow_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_size_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public GameButton m_btn_click_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_offset = FindUI<RectTransform>(vb.transform ,"pl_offset");
			m_pl_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow");

			m_pl_size_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow/pl_size");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow/pl_size/img_icon");

			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"pl_offset/btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_offset/btn_click");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}