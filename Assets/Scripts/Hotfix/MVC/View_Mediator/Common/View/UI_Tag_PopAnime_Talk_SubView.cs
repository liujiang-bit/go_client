// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Tag_PopAnime_Talk_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Tag_PopAnime_Talk_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Tag_PopAnime_Talk";

        public UI_Tag_PopAnime_Talk_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Tag_PopAnime_Talk;


        private void UIFinder()
        {       
			m_UI_Tag_PopAnime_Talk = gameObject.GetComponent<RectTransform>();

			BindEvent();
        }

        #endregion
    }
}