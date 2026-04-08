// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_HeadStar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_HeadStar_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_HeadStar";

        public UI_Model_HeadStar_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_HeadStar;
		[HideInInspector] public PolygonImage m_img_stardark_PolygonImage;

		[HideInInspector] public PolygonImage m_img_star_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_HeadStar = gameObject.GetComponent<RectTransform>();
			m_img_stardark_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_stardark");

			m_img_star_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_star");


			BindEvent();
        }

        #endregion
    }
}