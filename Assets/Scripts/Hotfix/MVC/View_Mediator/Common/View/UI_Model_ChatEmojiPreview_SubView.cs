// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ChatEmojiPreview_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_ChatEmojiPreview_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ChatEmojiPreview";

        public UI_Model_ChatEmojiPreview_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_ChatEmojiPreview;
		[HideInInspector] public SkeletonGraphic m_spin_preview_SkeletonGraphic;



        private void UIFinder()
        {       
			m_UI_Model_ChatEmojiPreview = gameObject.GetComponent<RectTransform>();
			m_spin_preview_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"bg/spin_preview");


			BindEvent();
        }

        #endregion
    }
}