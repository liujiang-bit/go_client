// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_alliance_1002_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_alliance_1002_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_alliance_1002";

        public UI_alliance_1002_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_alliance_1002_ViewBinder;

		[HideInInspector] public PolygonImage m_city_build_1_PolygonImage;



        private void UIFinder()
        {       
			m_UI_alliance_1002_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_city_build_1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"city_build_1");


			BindEvent();
        }

        #endregion
    }
}