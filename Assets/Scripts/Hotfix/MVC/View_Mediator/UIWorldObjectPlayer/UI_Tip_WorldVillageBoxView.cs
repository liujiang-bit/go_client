// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月26日
// Update Time         :    2020年3月26日
// Class Description   :    UI_Tip_WorldVillageBoxView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_WorldVillageBoxView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldVillageBox";

        public UI_Tip_WorldVillageBoxView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_pl_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}