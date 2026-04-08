// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_PlayerHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_PlayerHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_PlayerHead";

        public UI_Model_PlayerHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Model_PlayerHead_PolygonImage;
		[HideInInspector] public ViewBinder m_UI_Model_PlayerHead_ViewBinder;

		[HideInInspector] public PolygonImage m_img_circle0_PolygonImage;

		[HideInInspector] public PolygonImage m_img_circle_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_circle_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_PlayerHead_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Model_PlayerHead_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_circle0_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_circle0");

			m_img_circle_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_circle");
			m_img_circle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_circle");


			BindEvent();
        }

        #endregion
    }
}