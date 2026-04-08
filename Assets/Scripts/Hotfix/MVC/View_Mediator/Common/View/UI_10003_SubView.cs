// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10003_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_10003_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10003";

        public UI_10003_SubView (RectTransform transform) 
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