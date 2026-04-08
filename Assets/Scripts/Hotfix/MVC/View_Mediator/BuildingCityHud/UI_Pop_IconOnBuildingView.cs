// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月25日
// Update Time         :    2020年2月25日
// Class Description   :    UI_Pop_IconOnBuildingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_IconOnBuildingView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_IconOnBuilding";

        public UI_Pop_IconOnBuildingView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_pl_arrow_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_size_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg0_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg1_PolygonImage;

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

			m_img_bg0_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow/pl_size/img_bg0");

			m_img_bg1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/pl_arrow/pl_size/img_bg1");

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