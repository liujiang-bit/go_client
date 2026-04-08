// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10079_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_10079_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10079";

        public UI_10079_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 


        private void UIFinder()
        {       

			BindEvent();
        }

        #endregion
    }
}