// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Model_EventType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Model_EventType_SubView : UI_SubView
    {
        private ActivityCalendarDefine m_activityDefine;
        private bool m_isInit;
        public void Refresh(ActivityCalendarDefine define)
        {
            m_activityDefine = define;
            if (!m_isInit)
            {
                m_btn_info_GameButton.onClick.AddListener(OnInfo);
                m_btn_back_GameButton.onClick.AddListener(OnBack);
                m_isInit = true;
            }
        }

        private void OnInfo()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(true);
            m_pl_mes_CanvasGroup.gameObject.SetActive(false);

            m_lbl_info_LanguageText.text = LanguageUtils.getText(m_activityDefine.l_ruleID);
        }

        private void OnBack()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(false);
            m_pl_mes_CanvasGroup.gameObject.SetActive(true);
        }
    }
}