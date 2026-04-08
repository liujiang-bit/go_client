// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月27日
// Update Time         :    2020年10月27日
// Class Description   :    UI_Item_MailType18List_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using SprotoType;

namespace Game {
    public partial class UI_Item_MailType18List_SubView : UI_SubView
    {
        private InactiveMembersInfo m_itemData;
        private bool m_isInit;
        public Action<UI_Item_MailType18List_SubView> BtnClickEvent;

        public void Refresh(InactiveMembersInfo itemInfo)
        {
            m_itemData = itemInfo;

            if (!m_isInit)
            {
                m_btn_bg_GameButton.onClick.AddListener(OnClick);
                m_isInit = true;
            }

            m_UI_Model_PlayerHead.LoadPlayerIcon(itemInfo.headId, itemInfo.headFrameID);

            m_lbl_name_LanguageText.text = itemInfo.name;

            //几天前
            long diffTime = ServerTimeModule.Instance.GetServerTime() - itemInfo.lastLogoutTime;
            TimeSpan timeSpan = new TimeSpan(0, 0, (int)diffTime);
            int day = 0;
            if (timeSpan.Days > 0)
            {
                day = timeSpan.Days;
            }
            else
            {
                day = 1;
            }
            m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(570105, day);
        }

        public void OnClick()
        {
            if (BtnClickEvent != null)
            {
                BtnClickEvent(this);
            }
        }

        public InactiveMembersInfo GetItemData()
        {
            return m_itemData; 
        }
    }
}