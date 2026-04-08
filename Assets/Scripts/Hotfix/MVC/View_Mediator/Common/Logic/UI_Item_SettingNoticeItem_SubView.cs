// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月4日
// Update Time         :    2020年6月4日
// Class Description   :    UI_Item_SettingNoticeItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_SettingNoticeItem_SubView : UI_SubView
    {
        public void SetInfo(PushSetting info,UnityAction<int> callBack)
        {
            var pushCfg = CoreUtils.dataService.QueryRecord<PushMessageGroupDefine>((int)info.id);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(pushCfg.l_title);
            m_lbl_des_LanguageText.text = LanguageUtils.getText(pushCfg.l_desc);
            m_ck_switch_GameToggle.isOn = info.open==1;
            m_ck_switch_GameToggle.RemoveAllClickListener();
            m_ck_switch_GameToggle.interactable = true;
            m_ck_switch_GameToggle.AddListener(() =>
            {
                m_ck_switch_GameToggle.interactable = false;
                callBack?.Invoke((int)info.id);
            });
        }
    }
}