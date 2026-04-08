// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Model_StandardButton_Yellow2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_StandardButton_Yellow2_SubView : UI_SubView
    {
        private DateTime date;
        private Timer m_timer = null;
        public void AddClickEvent(UnityAction call)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(call);
        }
        public void RemoveAllClickEvent()
        {
            m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
        }
        public void ShowEndTime(DateTime endTime)
        {
            m_lbl_line1_LanguageText.gameObject.SetActive(true);
            date = endTime;

            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, () => { }, Update, true, false, m_lbl_line1_LanguageText);
            }
        }

        public void HideEndTime()
        {
            m_lbl_line1_LanguageText.gameObject.SetActive(false);
            if (m_timer != null)
            {
                Timer.Cancel(m_timer);
            }
        }

        private void Update(float t)
        {
            var offset = date - ServerTimeModule.Instance.GetCurrServerDateTime();
            m_lbl_line1_LanguageText.text = offset.ToString("HH:mm:ss");
        }
        public void SetText(string text)
        {
            m_lbl_line1_LanguageText.text = text;
        }
        public void SetIcon(string icon)
        {
            ClientUtils.LoadSprite(m_img_icon2_PolygonImage, icon);
        }
        public void SetNum(string text)
        {
            m_lbl_line2_LanguageText.text = text;

            ClientUtils.UIReLayout(m_btn_languageButton_GameButton);
        }
        
        


        public void SetGray(bool gray)
        {
            GrayChildrens makeChildrenGray=null;
            
            if (makeChildrenGray == null)
            {
                makeChildrenGray= this.gameObject.GetComponent<GrayChildrens>();
            }

            if (gray)
            {             
                makeChildrenGray.Gray();
                m_btn_languageButton_GameButton.interactable = false;
            }
            else
            {
                makeChildrenGray.Normal();
                m_btn_languageButton_GameButton.interactable = true;
            }
        }
    }
}