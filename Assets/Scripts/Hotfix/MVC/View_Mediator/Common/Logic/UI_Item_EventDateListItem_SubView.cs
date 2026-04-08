// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    UI_Item_EventDateListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using System;

namespace Game {
    public partial class UI_Item_EventDateListItem_SubView : UI_SubView
    {
        public ActivityItemData ItemData;
        public int Index;
        public Action<UI_Item_EventDateListItem_SubView> BtnListener;
        public ActivityProxy m_activityProxy;
        public bool m_isInit;
        private ActivityScheduleData m_scheduleData;

        public void AddBtnListener()
        {
            m_btn_click_GameButton.onClick.AddListener(Click);
        }

        public void Click()
        {
            if (BtnListener != null)
            {
                BtnListener(this);
            }
        }

        public void RefreshItem(int index, ActivityItemData data, bool isSelect)
        {
            if (!m_isInit)
            {
                m_isInit = true;
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            }
            Index = index;
            ItemData = data;
            if (data.ShowType == EnumActivityItemShowType.Date) //活动日历
            {
                m_lbl_title_LanguageText.text = LanguageUtils.getText(762166);
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, RS.EventCalendarIcon);

                m_img_new_PolygonImage.gameObject.SetActive(false);
                m_img_redpot_PolygonImage.gameObject.SetActive(false);

                RefreshCalendarNewFlag();
            }
            else
            {
                m_lbl_title_LanguageText.text = LanguageUtils.getText(data.Define.l_nameID);
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, data.Define.icon);

                m_img_new_PolygonImage.gameObject.SetActive(false);

                m_scheduleData = m_activityProxy.GetActivitySchedule(ItemData.Define.ID);
                RefreshRedpot();
                RefreshNewFlag();
            }

            SetSelectStatus(isSelect);
        }

        public void SetSelectStatus(bool isSelect)
        {
            if (isSelect)
            {
                ClientUtils.TextSetColor(m_lbl_title_LanguageText, "#6a6758");
            }
            else
            {
                ClientUtils.TextSetColor(m_lbl_title_LanguageText, "#ffffff");
            }
            m_img_bgActive_PolygonImage.gameObject.SetActive(isSelect);
        }

        public void RefreshRedpot()
        { 
            if (m_scheduleData != null)
            {
                int count = m_scheduleData.GetReddotNum();
                if (count > 0)
                {
                    m_img_redpot_PolygonImage.gameObject.SetActive(true);
                    m_img_redpot_LanguageText.text = count.ToString();
                    return;
                }
            }
            m_img_redpot_PolygonImage.gameObject.SetActive(false);
        }

        public void RefreshNewFlag()
        {
            if (m_scheduleData != null)
            {
                m_img_new_PolygonImage.gameObject.SetActive(m_scheduleData.IsNewActivity());
            }
            else
            {
                m_img_new_PolygonImage.gameObject.SetActive(false);
            }
        }

        private void RefreshCalendarNewFlag()
        {
            if (TipRemindProxy.IsShowRemind(TipRemindProxy.ActivityCalendarReddotTotal))
            {
                if (m_activityProxy.GetCalendarReddot() > 0)
                {
                    m_img_new_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    m_img_new_PolygonImage.gameObject.SetActive(false);
                }
            }
            else
            {
                m_img_new_PolygonImage.gameObject.SetActive(false);
            }
        }
    }
}