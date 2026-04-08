// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Warning_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Warning_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Warning";

        public UI_Model_Warning_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public SkeletonGraphic m_UI_Model_Warning_SkeletonGraphic;
		[HideInInspector] public Animator m_UI_Model_Warning_Animator;
		[HideInInspector] public GrayChildrens m_UI_Model_Warning_GrayChildrens;



        private void UIFinder()
        {       
			m_UI_Model_Warning_SkeletonGraphic = gameObject.GetComponent<SkeletonGraphic>();
			m_UI_Model_Warning_Animator = gameObject.GetComponent<Animator>();
			m_UI_Model_Warning_GrayChildrens = gameObject.GetComponent<GrayChildrens>();


			BindEvent();
        }

        #endregion
    }
}