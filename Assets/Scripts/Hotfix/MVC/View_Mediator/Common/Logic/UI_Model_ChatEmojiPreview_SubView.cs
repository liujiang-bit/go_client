// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Model_ChatEmojiPreview_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Model_ChatEmojiPreview_SubView : UI_SubView
    {
        public void SetInfo(int emojiID,Vector2 position)
        {
            var emojiConfig = CoreUtils.dataService.QueryRecord<ChatEmojiDefine>(emojiID);
            position.y += m_UI_Model_ChatEmojiPreview.rect.height / 2;
            m_UI_Model_ChatEmojiPreview.anchoredPosition = position;
            ClientUtils.LoadSpine(m_spin_preview_SkeletonGraphic,emojiConfig.spine);
        }
    }
}