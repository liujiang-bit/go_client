// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月28日
// Update Time         :    2020年10月28日
// Class Description   :    UI_Item_GeneralSetting_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_GeneralSetting_SubView : UI_SubView
    {
        public void addCKSwitchEvent(UnityAction<bool> unityAction)
        {
            m_ck_switch_GameToggle.onValueChanged.AddListener(unityAction);
        }

        public void SetToggle(bool open)
        {
            m_ck_switch_GameToggle.isOn = open;
            SetOpen(open);
        }
        public void SetOpen(bool open)
        {
            m_img_open_PolygonImage.gameObject.SetActive(open);
            m_img_close_PolygonImage.gameObject.SetActive(!open);
        }

        public void SetTitle(string title)
        {
            m_lbl_dec1_LanguageText.text = title;
        }

        public void SetDesc2(string desv2)
        {
            m_lbl_dec2_LanguageText.text = desv2;
        }
    }
}