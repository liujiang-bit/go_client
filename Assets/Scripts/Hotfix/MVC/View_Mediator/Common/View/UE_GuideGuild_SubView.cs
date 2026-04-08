// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UE_GuideGuild_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UE_GuideGuild_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UE_GuideGuild";

        public UE_GuideGuild_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UE_GuideGuild;
		[HideInInspector] public RectTransform m_pl_size;


        private void UIFinder()
        {       
			m_UE_GuideGuild = gameObject.GetComponent<RectTransform>();
			m_pl_size = FindUI<RectTransform>(gameObject.transform ,"pl_size");

			BindEvent();
        }

        #endregion
    }
}