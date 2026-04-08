// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Item_ChargeItemListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ChargeItemListItem_SubView : UI_SubView
    {
        private RewardGroupData m_rewardInfo;
        public void InitData(RewardGroupData rewardInfo)
        {
            m_rewardInfo = rewardInfo;
        }

        public void RefreshUI()
        {
            if (m_rewardInfo == null) return;
            m_lbl_name_LanguageText.text = LanguageUtils.getText(m_rewardInfo.name);
            m_lbl_num_LanguageText.text = m_rewardInfo.number.ToString();
            m_UI_Model_RewardGet.SetScale(m_UI_Model_RewardGet.gameObject.transform.localScale.x);
            m_UI_Model_RewardGet.RefreshByGroup(m_rewardInfo,1);
        }

        public void RemoveListener()
        {
            m_UI_Model_RewardGet.m_UI_Model_Item.RemoveBtnAllListener();
        }
    }
}