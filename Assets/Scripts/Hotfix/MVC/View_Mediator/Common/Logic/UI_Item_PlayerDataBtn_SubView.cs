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

        public void SetAgreement(IGGAgreement agreement)
        {
            m_lbl_Text_LanguageText.text = agreement.localizedName();
            var url = agreement.url();
            AddClickEvent(() =>
            {
                HotfixUtil.OpenBrowser(url, agreement.localizedName());
            });
        }
    }
}