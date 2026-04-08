// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Tag_ClickAnimeMsg_btn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Tag_ClickAnimeMsg_btn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Tag_ClickAnimeMsg_btn";

        public UI_Tag_ClickAnimeMsg_btn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Tag_ClickAnimeMsg_btn;


        private void UIFinder()
        {       
			m_UI_Tag_ClickAnimeMsg_btn = gameObject.GetComponent<RectTransform>();

			BindEvent();
        }

        #endregion
    }
}