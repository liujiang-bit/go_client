// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月9日
// Update Time         :    2020年7月9日
// Class Description   :    UI_Tip_ComBuildStateView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_ComBuildStateView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_ComBuildState";

        public UI_Tip_ComBuildStateView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_lbl_icon_PolygonImage;

		[HideInInspector] public RectTransform m_pl_distance;
		[HideInInspector] public LanguageText m_lbl_distance_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_frame");

			m_lbl_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/lbl_icon");

			m_pl_distance = FindUI<RectTransform>(vb.transform ,"img_bg/pl_distance");
			m_lbl_distance_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/pl_distance/lbl_distance");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}