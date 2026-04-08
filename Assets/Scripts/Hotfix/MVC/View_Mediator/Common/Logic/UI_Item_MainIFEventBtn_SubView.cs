// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_MainIFEventBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_MainIFEventBtn_SubView : UI_SubView
    {

        protected long m_uId;
        protected string m_icon;
        protected string m_iconBorder;
        protected long m_endTime;
        protected Timer m_timer;
        protected Action m_onComplete;
        protected Action m_onClick;

        public virtual void InitData(long uid,string icon,string iconBorder,Action onClick)
        {
            m_uId = uid;
            m_icon = icon;
            m_iconBorder = iconBorder;
            m_onClick = onClick;
            AddBtnListener();
        }

        public virtual void Refresh()
        {
            if (!string.IsNullOrEmpty(m_icon))
                ClientUtils.LoadSprite(m_btn_eventIcon_PolygonImage , m_icon);

            if (!string.IsNullOrEmpty(m_iconBorder))
            {
                m_img_frame_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(m_img_frame_PolygonImage, m_iconBorder);
            }
        }

        public virtual void UpdateReddot(bool isShow,int reddotCount)
        {
            if (isShow)
            {
                m_img_redpoint.gameObject.SetActive(true);
                m_lbl_redpoint_LanguageText.gameObject.SetActive(true);
                m_lbl_redpoint_LanguageText.text = reddotCount > 0 ? reddotCount.ToString() : "1";
            }
            else
            {
                m_img_redpoint.gameObject.SetActive(false);
            }
        }

        public void SetTimer(long endTime,Action onComplete = null)
        {
            long deltTime = endTime - ServerTimeModule.Instance.GetServerTime();
            if (deltTime > 0)
            {
                m_img_timebg_PolygonImage.gameObject.SetActive(true);
                m_onComplete = onComplete;
                m_endTime = endTime;
                m_lbl_time_LanguageText.text = GetCountDownStr(deltTime);
                if (m_timer != null)
                {
                    m_timer.Cancel();
                }
                m_timer = Timer.Register(1, onUpdate, null, true);
            }
            else
            {
                m_img_timebg_PolygonImage.gameObject.SetActive(false);
                if (m_onComplete != null)
                {
                    m_onComplete.Invoke();
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.RemoveLimitTimePackage,m_uId);
            }
        }

        private void onUpdate()
        {
            long deltTime = m_endTime - ServerTimeModule.Instance.GetServerTime();
            if (deltTime > 0)
            {
                if (m_lbl_time_LanguageText)
                    m_lbl_time_LanguageText.text = GetCountDownStr(deltTime);
                else
                {
                    if (m_timer != null)
                    {
                        m_timer.Cancel();
                        m_timer = null;
                    }
                }
            }
            else
            {
                if (m_onComplete != null)
                {
                    m_onComplete.Invoke();
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.RemoveLimitTimePackage,m_uId);
            }
        }

        public void Destroy(bool isNotisfy = true)
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
            
            CoreUtils.assetService.Destroy(gameObject);
        }

        protected void AddBtnListener()
        {
            m_btn_eventIcon_GameButton.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            if (m_onClick != null)
            {
                m_onClick.Invoke();
            }
        }
        
        private string GetCountDownStr(long deltTime)
        {
            long day = deltTime / 86400;
            long hour = (deltTime % 86400) / 3600;
            long min = ((deltTime % 86400) % 3600) / 60;
            long sec = ((deltTime % 86400) % 3600) % 60;
            
            if (day > 0)
            {
                return LanguageUtils.getTextFormat(800109, day.ToString("00"), "" + hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00"));
            }
            else
            {
                return "" + hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
            }
        }
    }
}