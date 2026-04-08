// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_TroopsState_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Common_TroopsState_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_TroopsState";

        public UI_Common_TroopsState_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public SkeletonGraphic m_UI_Common_TroopsState_SkeletonGraphic;
		[HideInInspector] public Animator m_UI_Common_TroopsState_Animator;



        private void UIFinder()
        {       
			m_UI_Common_TroopsState_SkeletonGraphic = gameObject.GetComponent<SkeletonGraphic>();
			m_UI_Common_TroopsState_Animator = gameObject.GetComponent<Animator>();


			BindEvent();
        }

        #endregion
    }
}