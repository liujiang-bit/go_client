// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ItemEffect_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ItemEffect_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ItemEffect";

        public UI_Item_ItemEffect_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ItemEffect;


        private void UIFinder()
        {       
			m_UI_Item_ItemEffect = gameObject.GetComponent<RectTransform>();

			BindEvent();
        }

        #endregion
    }
}