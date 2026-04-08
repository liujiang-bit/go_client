// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Item_EventDateListItemPb_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_EventDateListItemPb_SubView : UI_SubView
    {
        public ActivityItemData ItemData;
        public int Index;
        public Action<UI_Item_EventDateListItemPb_SubView> BtnListener;
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

            m_lbl_title_LanguageText.text = LanguageUtils.getText(data.Define.l_nameID);
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, data.Define.icon);
            m_img_new_PolygonImage.gameObject.SetActive(false);

            RefreshRedpot();
            SetSelectStatus(isSelect);

            m_scheduleData = m_activityProxy.GetActivitySchedule(ItemData.Define.ID);
            RefreshProgress();
            RefreshNewFlag();
        }

        public void RefreshProgress()
        {
            if (m_scheduleData != null)
            {
                ActivityScheduleData scheduleData = m_scheduleData;
                List<ActivityBehaviorData> behaviorList = scheduleData.GetBehaviorList();
                int count = behaviorList.Count;
                if (count < 3)
                {
                    return;
                }
                int total = 0;
                float currProcess = 0f;
                for (int i = 0; i < 3; i++)
                {
                    if (scheduleData.Info.score >= behaviorList[i].Count)
                    {
                        total = total + 1;
                    }
                    else
                    {
                        float score1 = scheduleData.Info.score - behaviorList[i].Condition;
                        float score2 = behaviorList[i].Count - behaviorList[i].Condition;
                        currProcess = score1 / score2;
                        break;
                    }
                }
                float process = (float)total / 3 + currProcess / 3;
                m_pb_rogressBar0_GameSlider.value = process;

                float greenProcess = 0f;
                if (process >= 1)
                {
                    greenProcess = 1f;
                }
                else if (process >= ((float)2 / 3))
                {
                    greenProcess = (float)2 / 3;
                }
                else if (process >= ((float)1 / 3))
                {
                    greenProcess = (float)1 / 3;
                }
                else
                {
                    greenProcess = 0f;
                }
                m_pb_rogressBar_GameSlider.value = greenProcess; //绿色
            }
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
            ActivityScheduleData scheduleData = m_activityProxy.GetActivitySchedule(ItemData.Define.ID);
            if (scheduleData != null)
            {
                int count = scheduleData.GetReddotNum();
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
    }
}