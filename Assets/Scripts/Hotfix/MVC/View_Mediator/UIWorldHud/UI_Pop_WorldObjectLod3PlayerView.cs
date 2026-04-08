// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月18日
// Update Time         :    2020年3月18日
// Class Description   :    UI_Pop_WorldObjectLod3PlayerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_WorldObjectLod3PlayerView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_WorldObjectLod3Player";

        public UI_Pop_WorldObjectLod3PlayerView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_nohead_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_nohead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_nohead");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_PlayerHead"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}