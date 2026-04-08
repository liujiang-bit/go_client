// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月6日
// Update Time         :    2020年5月6日
// Class Description   :    UI_Pop_TroopsCollectBarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_TroopsCollectBarView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_TroopsCollectBar";

        public UI_Pop_TroopsCollectBarView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bar_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_offset = FindUI<RectTransform>(vb.transform ,"pl_offset");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/img_bg");

			m_img_bar_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/img_bar");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}