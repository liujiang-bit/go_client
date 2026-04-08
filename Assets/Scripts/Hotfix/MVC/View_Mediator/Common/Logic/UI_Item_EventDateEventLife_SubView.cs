// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月17日
// Update Time         :    2020年4月17日
// Class Description   :    UI_Item_EventDateEventLife_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using Data;
using System;

namespace Game {
    public partial class UI_Item_EventDateEventLife_SubView : UI_SubView
    {
        public ActivityCalendarData ActivityInfo;
        private float m_unitWidth = 97f;
        private RectTransform m_btnRect;
        private bool m_init;
        private RectTransform m_imgIconRect;
        public Action<UI_Item_EventDateEventLife_SubView> BtnListener;
        public ActivityProxy m_activityProxy;

        public void AddBtnListener()
        {
            m_btn_mes_GameButton.onClick.AddListener(OnClick);
        }

        public ActivityCalendarData GetActivityInfo()
        {
            return ActivityInfo;
        }

        public void OnClick()
        {
            if (BtnListener != null)
            {
                BtnListener(this);
            }
        }

        public void RefreshItem(ActivityCalendarData itemData)
        {
            if (!m_init)
            {
                m_btnRect = m_btn_mes_GameButton.gameObject.transform.GetComponent<RectTransform>();
                m_imgIconRect = m_img_icon_PolygonImage.GetComponent<RectTransform>();
                m_init = true;
                m_btnRect.anchorMin = new Vector2(0, 0.5f);
                m_btnRect.anchorMax = new Vector2(0, 0.5f);
                m_btnRect.pivot = new Vector2(0, 0.5f);
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            }
            ActivityInfo = itemData;
            ActivityTimeInfo activityInfo = itemData.ActivityInfo;
            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)activityInfo.activityId);

            ClientUtils.LoadSprite(m_img_bg_PolygonImage, GetProgerssBar(define.progressBar));
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, define.icon);

            ActivityScheduleData scheduleData = m_activityProxy.GetActivitySchedule(activityInfo.activityId);
            if (scheduleData != null && scheduleData.Info.isNew)
            {
                m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
            }

            //设置按钮位置大小
            int width = itemData.EndWeekDay - itemData.StartWeekDay + 1;
            float startX = 0;
            //if (LanguageUtils.IsArabic())
            //{
            startX = itemData.StartWeekDay * m_unitWidth;
            //}
            //else
            //{
            //startX = -itemData.StartWeekDay * m_unitWidth;
            //}
            float btnWidth = width * m_unitWidth;         
            m_btnRect.anchoredPosition = new Vector2(startX, m_btnRect.anchoredPosition.y);
            m_btnRect.sizeDelta = new Vector2(btnWidth, m_btnRect.rect.height);

            //设置文本
            m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_nameID);
            float textShowWitdh = btnWidth - (m_imgIconRect.rect.width+10f);
            if (textShowWitdh < m_lbl_name_LanguageText.preferredWidth)
            {
                bool isBreak = false;
                string formatText = m_lbl_name_LanguageText.text;
                int size = ClientUtils.SubStringGetTotalIndex(formatText);
                while (size > 0)
                {
                    size = size - 1;
                    formatText = ClientUtils.SubStringUTF8(formatText, 1, size);
                    m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300250, formatText);
                    if (m_lbl_name_LanguageText.preferredWidth <= textShowWitdh)
                    {
                        isBreak = true;
                        break;
                    }
                }
                if (!isBreak)
                {
                    m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300250, "");
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_img_bg_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        private string GetProgerssBar(int type)
        {
            if (type < RS.ActivityDateProgressBar.Length)
            {
                return RS.ActivityDateProgressBar[type];
            }
            return RS.ActivityDateProgressBar[0];
        }
    }
}