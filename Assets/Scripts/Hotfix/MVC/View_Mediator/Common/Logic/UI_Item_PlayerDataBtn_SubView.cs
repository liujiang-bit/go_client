// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    UI_Item_PlayerDataBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_PlayerDataBtn_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction call)
        {
            m_btn_btn_GameButton.onClick.AddListener(call);
        }

        public void SetAgreement(SDKAgreement agreement)
        {
            // SDK 协议展示功能已移除（Stub）
            // 后续可接入自定义协议管理
        }
    }
}