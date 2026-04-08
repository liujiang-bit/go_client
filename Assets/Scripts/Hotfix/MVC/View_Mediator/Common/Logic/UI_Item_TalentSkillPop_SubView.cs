// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_Item_TalentSkillPop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_TalentSkillPop_SubView : UI_SubView
    {
        private UnityAction m_clickCall;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_closeButton_GameButton.onClick.AddListener(OnClickClose);
            m_btn_leveup.m_btn_languageButton_GameButton.onClick.AddListener(OnLevelUp);
        }

        public void SetInfo(HeroTalentGainTreeDefine treeDefine,int state,UnityAction clickCall)
        {
            m_UI_Item_TalentSkillPop_Animator.gameObject.SetActive(true);
            m_btn_closeButton_GameButton.gameObject.SetActive(true);
            m_pl_pos.gameObject.SetActive(true);
            m_clickCall = clickCall;
            if (treeDefine != null)
            {
                m_lbl_skillname_LanguageText.text = LanguageUtils.getText(treeDefine.nameID);
                m_lbl_mes_LanguageText.text = LanguageUtils.getText(treeDefine.descID);
                m_lbl_skilllv_LanguageText.text = treeDefine.level + "/10";
                if (state == 0)
                {
                    m_btn_leveup.gameObject.SetActive(true);
                }
                else
                {
                    m_btn_leveup.gameObject.SetActive(false);
                }
            }
        }

        private void OnClickClose()
        {
            m_UI_Item_TalentSkillPop_Animator.gameObject.SetActive(false);
            m_btn_closeButton_GameButton.gameObject.SetActive(false);
            m_pl_pos.gameObject.SetActive(false);
        }

        private void OnLevelUp()
        {
            if (m_clickCall != null)
            {
                m_clickCall.Invoke();
                OnClickClose();
            }
        }

    }
}