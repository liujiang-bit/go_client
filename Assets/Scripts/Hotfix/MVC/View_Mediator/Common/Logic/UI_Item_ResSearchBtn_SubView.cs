// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月17日
// Update Time         :    2020年3月17日
// Class Description   :    UI_Item_ResSearchBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ResSearchBtn_SubView : UI_SubView
    {

        public void SetResSearchInfo(string name)
        {
            m_Txt_name_LanguageText.text = name;
        }
          public void SetGray()
        {
            m_UI_Item_ResSearchBtn_MakeChildrenGray.Gray();
        }
        public void AddClickEvent(UnityAction action)
        {
            m_btn_btn_GameButton.onClick.AddListener(action);
        }
    }
}