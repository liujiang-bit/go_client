// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Item_ChargeListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ChargeListItem_SubView : UI_SubView
    {
        protected override void BindEvent()
        {
            m_ck_type_GameToggle.onValueChanged.RemoveAllListeners();
            m_ck_type_GameToggle.onValueChanged.AddListener(OnChanged);
        }

        public void Refresh(int configId, int redPointCount)
        {
            m_CfgId = configId;
            var cfg = CoreUtils.dataService.QueryRecord<Data.RechargeListDefine>(configId);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(cfg.l_pagingID);
            m_img_redpoint_PolygonImage.gameObject.SetActive(redPointCount > 0);
            m_lbl_num_LanguageText.text = redPointCount.ToString();
        }

        private void OnChanged(bool isOn)
        {
            if (isOn)
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSidePage);
                AppFacade.GetInstance().SendNotification(CmdConstant.ChargeListToggleChanged, m_CfgId);
            }
        }

        private int m_CfgId = -1;
    }
}