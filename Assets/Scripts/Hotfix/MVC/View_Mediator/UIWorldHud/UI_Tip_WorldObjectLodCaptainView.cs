// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月19日
// Update Time         :    2020年3月19日
// Class Description   :    UI_Tip_WorldObjectLodCaptainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_WorldObjectLodCaptainView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldObjectLodCaptain";

        public UI_Tip_WorldObjectLodCaptainView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_head_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_state_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_head_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_head");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"img_head/UI_Model_CaptainHead"));
			m_img_state_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_head/img_state");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}