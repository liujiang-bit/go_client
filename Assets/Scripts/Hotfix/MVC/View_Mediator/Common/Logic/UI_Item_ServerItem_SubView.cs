// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    UI_Item_ServerItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_ServerItem_SubView : UI_SubView
    {
        private RoleInfoProxy m_RoleInfoProxy;
        private Dictionary<int, Timer> dicTimes = new Dictionary<int, Timer>();
        
        public void SetData(ServerListTypeDefine data)
        {
            this.gameObject.SetActive(data!=null);
            if (data == null)
            {
                return;
            }

            if (m_RoleInfoProxy == null)
            {
                m_RoleInfoProxy= AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            }

            if (data.serverNameId > 0)
            {         
                m_lbl_kingdomName_LanguageText.text = LanguageUtils.getText(data.serverNameId);    
            }

            m_lbl_kingdomNum_LanguageText.text = LanguageUtils.getTextFormat(100525, data.ID);

            if (m_RoleInfoProxy != null)
            {
                int haveNum = m_RoleInfoProxy.IsHaveRole(data.severId);
                m_img_icon_PolygonImage.gameObject.SetActive(haveNum>0);
                m_lbl_num_LanguageText.text = haveNum.ToString();
            }
            
            DateTime dateTime=   Convert.ToDateTime(data.serverTime);    
           // long serverTimes =  RoleInfoHelp.ToUnixTimestamp(dateTime);
           // TimeSpan time =ServerTimeModule.Instance.GetCurrServerDateTime() -dateTime;
            bool isShow = false;
            Clear();
            OnTimes(dateTime, isShow);          
            Timer timer=Timer.Register(1f, () =>
            {
                if (m_lbl_time_LanguageText != null)
                {
                    OnTimes(dateTime,isShow);
                }
                else
                {
                    Clear();
                }
            }, null, true);
            
            if (!dicTimes.ContainsKey(data.ID))
            {
                dicTimes[data.ID] = timer;
            }
                        
            m_btn_kingdom_GameButton.onClick.RemoveAllListeners();
            m_btn_kingdom_GameButton.onClick.AddListener(() =>
            {
                Tip.CreateTip(100045).Show();
            });
        }

        private void OnTimes(DateTime serverTime, bool isShow)
        {
            TimeSpan time = ServerTimeModule.Instance.GetCurrServerDateTime() -serverTime;;
            isShow = false;
            if (time.Seconds > 0)
            {                
                var m_formatTimeSpan = new TimeSpan(0, 0, (int) m_RoleInfoProxy?.NewseverTimeLimit);
                isShow = time.Days <= m_formatTimeSpan.Days;
                m_lbl_time_LanguageText.text = isShow ? RoleInfoHelp.FormatCountDown(time) : string.Empty;
            }

            m_pl_view_HorizontalLayoutGroup.gameObject.SetActive(time.Seconds > 0);
            m_UI_Common_Redpoint.gameObject.SetActive(time.Seconds > 0 && isShow);
        }

        private void Clear()
        {
            foreach (var time in dicTimes.Values)
            {
                time.Cancel();
            }
            dicTimes.Clear();
        }

    }
}