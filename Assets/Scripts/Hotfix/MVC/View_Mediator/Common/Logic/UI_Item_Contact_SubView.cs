// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月15日
// Update Time         :    2020年4月15日
// Class Description   :    UI_Item_Contact_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game {
    public partial class UI_Item_Contact_SubView : UI_SubView
    {
        private UnityAction<bool> onShowOptionBar;
        private Vector2 m_mousePos;
        private bool m_isShowLeft = false;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_sv_contact_ListView.onDragBegin += OnDragBegin;
            m_sv_contact_ListView.onDragEnd += OnDragEnd;
        }

        public void SetInfo(bool showOptionBar,UnityAction<bool> showOptionBarCallback)
        {
            onShowOptionBar = showOptionBarCallback;
            ShowLeft(showOptionBar);
        }

        private void ShowLeft(bool isShow)
        {
            if (m_isShowLeft == isShow)
            {
                return;
            }
            m_isShowLeft = isShow;
            onShowOptionBar?.Invoke(isShow);
            var dis = m_pl_left.rect.width* (isShow?1:-1);
            m_sv_contact_ListView.SetContainerPos(0);
            m_pl_filter.anchoredPosition +=new Vector2(dis,0);
        }

        private void OnDragBegin(PointerEventData data)
        {
            m_mousePos = data.position;
        }

        private void OnDragEnd(PointerEventData data)
        {
            var moveDistance = data.position.x - m_mousePos.x;
            moveDistance *= m_isShowLeft ? -1 : 1;
            if (moveDistance * 2 > m_pl_left.rect.width)
            {
                ShowLeft(!m_isShowLeft);
            }
        }
    }
}