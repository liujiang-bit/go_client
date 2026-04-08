// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_Spin_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Common_Spin_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_Spin";

        public UI_Common_Spin_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Common_Spin_ViewBinder;



        private void UIFinder()
        {       
			m_UI_Common_Spin_ViewBinder = gameObject.GetComponent<ViewBinder>();


			BindEvent();
        }

        #endregion
    }
}