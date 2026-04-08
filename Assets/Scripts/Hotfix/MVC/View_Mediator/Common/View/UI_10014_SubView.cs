// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10014_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_10014_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10014";

        public UI_10014_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_10014_Animator;
		[HideInInspector] public PolygonImage m_UI_10014_PolygonImage;



        private void UIFinder()
        {       
			m_UI_10014_Animator = gameObject.GetComponent<Animator>();
			m_UI_10014_PolygonImage = gameObject.GetComponent<PolygonImage>();


			BindEvent();
        }

        #endregion
    }
}