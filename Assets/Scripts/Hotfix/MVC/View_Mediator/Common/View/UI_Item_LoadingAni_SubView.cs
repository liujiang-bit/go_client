// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_LoadingAni_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_LoadingAni_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_LoadingAni";

        public UI_Item_LoadingAni_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_LoadingAni_Animator;
		[HideInInspector] public PolygonImage m_UI_Item_LoadingAni_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_LoadingAni_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_LoadingAni_PolygonImage = gameObject.GetComponent<PolygonImage>();


			BindEvent();
        }

        #endregion
    }
}