// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatEmojiList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatEmojiList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatEmojiList";

        public UI_Item_ChatEmojiList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_ChatEmojiList_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ChatEmojiList_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ChatEmoji_SubView m_UI_Item_ChatEmoji1;
		[HideInInspector] public UI_Item_ChatEmoji_SubView m_UI_Item_ChatEmoji2;
		[HideInInspector] public UI_Item_ChatEmoji_SubView m_UI_Item_ChatEmoji3;
		[HideInInspector] public UI_Item_ChatEmoji_SubView m_UI_Item_ChatEmoji4;


        private void UIFinder()
        {       
			m_UI_Item_ChatEmojiList_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_ChatEmojiList_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_ChatEmoji1 = new UI_Item_ChatEmoji_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChatEmoji1"));
			m_UI_Item_ChatEmoji2 = new UI_Item_ChatEmoji_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChatEmoji2"));
			m_UI_Item_ChatEmoji3 = new UI_Item_ChatEmoji_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChatEmoji3"));
			m_UI_Item_ChatEmoji4 = new UI_Item_ChatEmoji_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChatEmoji4"));

			BindEvent();
        }

        #endregion
    }
}