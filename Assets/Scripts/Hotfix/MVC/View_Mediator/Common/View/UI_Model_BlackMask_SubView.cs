// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_BlackMask_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_BlackMask_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_BlackMask";

        public UI_Model_BlackMask_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_BlackMask;


        private void UIFinder()
        {       
			m_UI_Model_BlackMask = gameObject.GetComponent<RectTransform>();

			BindEvent();
        }

        #endregion
    }
}