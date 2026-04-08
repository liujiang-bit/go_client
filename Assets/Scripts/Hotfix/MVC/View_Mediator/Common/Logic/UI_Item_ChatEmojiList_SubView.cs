// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Item_ChatEmojiList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ChatEmojiList_SubView : UI_SubView
    {
        private List<UI_Item_ChatEmoji_SubView> m_emojiViews = new List<UI_Item_ChatEmoji_SubView>();
        protected override void BindEvent()
        {
            base.BindEvent();
            m_emojiViews.Add(m_UI_Item_ChatEmoji1);
            m_emojiViews.Add(m_UI_Item_ChatEmoji2);
            m_emojiViews.Add(m_UI_Item_ChatEmoji3);
            m_emojiViews.Add(m_UI_Item_ChatEmoji4);
        }

        public void SetInfo(List<ChatEmojiDefine> emojiInfos,UnityAction<int> clickCallback,UnityAction<int,Vector2> longClickCallback,UnityAction<int> releaseAfterLongCallback)
        {
            HideEmojis();
            for (int i = 0; i < emojiInfos.Count; i++)
            {
                m_emojiViews[i].SetInfo(emojiInfos[i],clickCallback,longClickCallback,releaseAfterLongCallback);
                m_emojiViews[i].gameObject.SetActive(true);
            }
        }

        public void HideEmojis()
        {
            foreach (var emojiView in m_emojiViews)
            {
                emojiView.gameObject.SetActive(false);
            }
        }
    }
}