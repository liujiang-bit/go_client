// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Tip_WorldObjectLodCaptain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Tip_WorldObjectLodCaptain_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Tip_WorldObjectLodCaptain";

        public UI_Tip_WorldObjectLodCaptain_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public MapElementUI m_UI_Tip_WorldObjectLodCaptain_MapElementUI;
		[HideInInspector] public ViewBinder m_UI_Tip_WorldObjectLodCaptain_ViewBinder;

		[HideInInspector] public PolygonImage m_img_head_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_state_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Tip_WorldObjectLodCaptain_MapElementUI = gameObject.GetComponent<MapElementUI>();
			m_UI_Tip_WorldObjectLodCaptain_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_head");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"img_head/UI_Model_CaptainHead"));
			m_img_state_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_head/img_state");


			BindEvent();
        }

        #endregion
    }
}