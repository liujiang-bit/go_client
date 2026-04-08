// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月23日
// Update Time         :    2020年3月23日
// Class Description   :    UI_Tip_WorldCaveMarkView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_WorldCaveMarkView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldCaveMark";

        public UI_Tip_WorldCaveMarkView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}