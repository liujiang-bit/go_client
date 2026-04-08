// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_accountMgrBinding_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_accountMgrBinding_SubView : UI_SubView
    {
        public void SetBindInfo(bool bHasBind, string accountName = "")
        {
            m_lbl_alreadyBinding_LanguageText.text = accountName;
            m_UI_Binding.gameObject.SetActive(!bHasBind);
        }
        public void AddBindClickEvent(UnityAction action)
        {
            m_UI_Binding.AddClickEvent(action);
        }
    }
}