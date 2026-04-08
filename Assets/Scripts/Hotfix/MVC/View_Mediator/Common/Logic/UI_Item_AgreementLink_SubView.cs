// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Item_AgreementLink_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_AgreementLink_SubView : UI_SubView
    {
        public void SetAgreement(IGGAgreement agreement)
        {
            m_UI_Model_Link.SetLinkText(agreement.localizedName());
            var url = agreement.url();
            m_UI_Model_Link.AddClickEvent(()=>
            {
                HotfixUtil.OpenBrowser(url, agreement.localizedName());
            });
        }
    }
}