// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Item_ChatEmojiMenu_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ChatEmojiMenu_SubView : UI_SubView
    {
        public void SetInfo(EmojiMenuInfo info,bool isSelect,UnityAction<int> clickCallback)
        {
            ClientUtils.LoadSpine(m_spine_emoji_SkeletonGraphic,info.EmojiKey);
            m_img_select_PolygonImage.gameObject.SetActive(isSelect);
            
            m_btn_emoji_GameButton.onClick.RemoveAllListeners();
            m_btn_emoji_GameButton.onClick.AddListener(() =>
            {
                clickCallback?.Invoke(info.Group);
            });
        }
    }
}