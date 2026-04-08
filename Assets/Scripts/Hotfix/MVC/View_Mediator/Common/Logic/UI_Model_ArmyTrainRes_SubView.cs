// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    UI_Model_ArmyTrainRes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_ArmyTrainRes_SubView : UI_SubView
    {
        public void SetCost(string str, bool isEnough)
        {
            m_lbl_resCost_num_LanguageText.text = str;
            if (isEnough)
            {
                m_lbl_resCost_num_LanguageText.color = Color.white;
            }
            else
            {
                m_lbl_resCost_num_LanguageText.color = Color.red;
            }
        }

        public void AddResListener(UnityAction callback)
        {
            m_btn_area_GameButton.onClick.AddListener(callback);
        }
    }
}