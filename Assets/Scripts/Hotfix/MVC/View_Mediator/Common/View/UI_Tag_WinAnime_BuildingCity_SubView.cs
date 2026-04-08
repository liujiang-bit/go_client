// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Tag_WinAnime_BuildingCity_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Tag_WinAnime_BuildingCity_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Tag_WinAnime_BuildingCity";

        public UI_Tag_WinAnime_BuildingCity_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Tag_WinAnime_BuildingCity;


        private void UIFinder()
        {       
			m_UI_Tag_WinAnime_BuildingCity = gameObject.GetComponent<RectTransform>();

			BindEvent();
        }

        #endregion
    }
}