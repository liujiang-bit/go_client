// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Model_CaptainBar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_CaptainBar_SubView : UI_SubView
    {
        int m_nMaxExp;
        public void SetExp(int exp, int maxExp)
        {
            m_nMaxExp = maxExp;
            SetExp(exp);
        }
        public void ShowBtn(bool bShow)
        {
            m_btn_add_GameButton.gameObject.SetActive(bShow);
        }
        public void SetExp(int exp)
        {
            if (m_nMaxExp == 0)
            {
                m_pb_rogressBar_GameSlider.value = 1;
                m_lbl_value_LanguageText.enabled = false;
            }
            else
            {
                m_pb_rogressBar_GameSlider.value = (float)exp / (float)m_nMaxExp;
                m_lbl_value_LanguageText.enabled = true;
                m_lbl_value_LanguageText.text = string.Format(LanguageUtils.getText(300001), ClientUtils.FormatComma(exp), ClientUtils.FormatComma(m_nMaxExp));
            }
        }
        public void AddClickEvent(UnityAction call)
        {
            m_btn_add_GameButton.onClick.AddListener(call);
        }
        public void Enable(bool enable)
        {

        }
    }
}