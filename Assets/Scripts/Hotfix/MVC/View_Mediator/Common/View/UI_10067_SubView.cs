// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10067_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_10067_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10067";

        public UI_10067_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public LevelDetailScale m_UI_10067_LevelDetailScale;



        private void UIFinder()
        {       
			m_UI_10067_LevelDetailScale = gameObject.GetComponent<LevelDetailScale>();


			BindEvent();
        }

        #endregion
    }
}