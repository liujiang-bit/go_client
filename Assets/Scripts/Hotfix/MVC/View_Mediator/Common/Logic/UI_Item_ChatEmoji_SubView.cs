// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Item_ChatEmoji_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game {
    public partial class UI_Item_ChatEmoji_SubView : UI_SubView
    {
        public void SetInfo(ChatEmojiDefine info, UnityAction<int> clickCallback, UnityAction<int,Vector2> longClickCallback,UnityAction<int> releaseAfterLongCallback)
        {
            ClientUtils.LoadSpine(m_spine_emoji_SkeletonGraphic,info.spine);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(info.l_nameId);
            m_btn_emoji_LongClickButton.action = () =>
            {
                var world = m_UI_Item_ChatEmoji.position;
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(world), CoreUtils.uiManager.GetUICamera(), out pos);

                pos.y += m_UI_Item_ChatEmoji.rect.height / 2;
                longClickCallback?.Invoke(info.ID,pos);
            };
            m_btn_emoji_LongClickButton.releaseAction = () =>
            {
                releaseAfterLongCallback?.Invoke(info.ID);
            };
            m_btn_emoji_UIClickListener.onPointerDown = (PointerEventData data) =>
            {
                m_img_select_PolygonImage.gameObject.SetActive(true);
            };
            m_btn_emoji_UIClickListener.onPointerUp = (PointerEventData data) =>
            {
                m_img_select_PolygonImage.gameObject.SetActive(false);
            };
            m_btn_emoji_UIClickListener.onPointerClick = (PointerEventData data) =>
            {
                clickCallback?.Invoke(info.ID);
            };
        }
    }
}