// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月29日
// Update Time         :    2020年10月29日
// Class Description   :    UI_Tip_WorldObjectLodArmyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Tip_WorldObjectLodArmyView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldObjectLodArmy";

        public UI_Tip_WorldObjectLodArmyView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_head_PolygonImage;

		[HideInInspector] public PolygonImage m_img_choose_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_state_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_head_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_head");

			m_img_choose_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_head/img_choose");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"img_head/UI_Model_CaptainHead"));
			m_img_state_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_head/img_state");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}