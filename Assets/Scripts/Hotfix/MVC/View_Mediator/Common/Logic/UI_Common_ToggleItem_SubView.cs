// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_Common_ToggleItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Common_ToggleItem_SubView : UI_SubView
    {
        public void SetInfo(bool isOn,int index,string txt, UnityAction<bool,int> callback)
        {
            m_lbl_label_LanguageText.text = txt;
            m_UI_Common_ToggleItem_GameToggle.onValueChanged.RemoveAllListeners();
            m_UI_Common_ToggleItem_GameToggle.onValueChanged.AddListener((bValue)=>
            {
                callback?.Invoke(bValue,index);
            });
            m_UI_Common_ToggleItem_GameToggle.SetIsOnWithoutNotify(isOn);
        }
    }
}