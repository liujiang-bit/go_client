// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10035_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_10035_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10035";

        public UI_10035_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_10035;
		[HideInInspector] public PolygonImage m_img_star003_2_PolygonImage;
		[HideInInspector] public DeemoRotate m_img_star003_2_DeemoRotate;

		[HideInInspector] public PolygonImage m_img_star003_1_PolygonImage;
		[HideInInspector] public DeemoRotate m_img_star003_1_DeemoRotate;



        private void UIFinder()
        {       
			m_UI_10035 = gameObject.GetComponent<RectTransform>();
			m_img_star003_2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_star003_2");
			m_img_star003_2_DeemoRotate = FindUI<DeemoRotate>(gameObject.transform ,"img_star003_2");

			m_img_star003_1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_star003_1");
			m_img_star003_1_DeemoRotate = FindUI<DeemoRotate>(gameObject.transform ,"img_star003_1");


			BindEvent();
        }

        #endregion
    }
}