// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10034_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_10034_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10034";

        public UI_10034_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_10034;
		[HideInInspector] public Animator m_UI_10034_Animator;
		[HideInInspector] public PolygonImage m_UI_10034_PolygonImage;



        private void UIFinder()
        {       
			m_UI_10034 = gameObject.GetComponent<RectTransform>();
			m_UI_10034_Animator = FindUI<Animator>(gameObject.transform ,"UI_10034");
			m_UI_10034_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"UI_10034");


			BindEvent();
        }

        #endregion
    }
}