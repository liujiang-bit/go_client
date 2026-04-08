// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月21日
// Update Time         :    2020年4月21日
// Class Description   :    UI_Model_PlayerHeadView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Model_PlayerHeadView : GameView
    {
		public const string VIEW_NAME = "UI_Model_PlayerHead";

        public UI_Model_PlayerHeadView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_circle_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_circle_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_circle");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}