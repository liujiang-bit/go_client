// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, February 28, 2020
// Update Time         :    Friday, February 28, 2020
// Class Description   :    UE_ResFly_researchView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UE_ResFly_researchView : GameView
    {
		public const string VIEW_NAME = "UE_ResFly_research";

        public UE_ResFly_researchView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_rare_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_rare_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_rare");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}