// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月29日
// Update Time         :    2020年2月29日
// Class Description   :    UE_TrainSoldierFlyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UE_TrainSoldierFlyView : GameView
    {
		public const string VIEW_NAME = "UE_TrainSoldierFly";

        public UE_TrainSoldierFlyView () 
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