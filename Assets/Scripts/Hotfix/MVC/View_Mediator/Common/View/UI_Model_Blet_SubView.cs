// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Blet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_Blet_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Blet";

        public UI_Model_Blet_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Blet;
		[HideInInspector] public PolygonImage m_img_Blet0_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_Blet = gameObject.GetComponent<RectTransform>();
			m_img_Blet0_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_Blet0");


			BindEvent();
        }

        #endregion
    }
}