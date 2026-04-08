// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_awaken_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_awaken_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_awaken";

        public UI_Item_awaken_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_awaken;
		[HideInInspector] public PolygonImage m_img_awakenbg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_awakenlight_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_awaken = gameObject.GetComponent<RectTransform>();
			m_img_awakenbg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_awakenbg");

			m_img_awakenlight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_awakenlight");


			BindEvent();
        }

        #endregion
    }
}