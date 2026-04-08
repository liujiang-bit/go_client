// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10016_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_10016_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10016";

        public UI_10016_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_shine_PolygonImage;

		[HideInInspector] public PolygonImage m_img_circle_PolygonImage;



        private void UIFinder()
        {       
			m_img_shine_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_shine");

			m_img_circle_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_circle");


			BindEvent();
        }

        #endregion
    }
}