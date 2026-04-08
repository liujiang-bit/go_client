// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月21日
// Update Time         :    2020年1月21日
// Class Description   :    UI_Item_IconAndTime_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    
    
    public partial class UI_Item_IconAndTime_SubView : UI_SubView
    {
        public enum BuildingState
        {
            NotUnlock = 0,    // 未解锁
            InitProtecting,    // 初始保护中
            InitFighting,    // 初始争夺中
            Fighting,    // 常规争夺中
            Protecting,        // 常规保护中
        }

        private int tips_noOpen = 4100;    // 未开放
        private int tips_protect = 4101;    // 保护中
        private int tips_fighting = 4102;    // 争夺中
        
        private int lan_notOpen = 500789;        // 未开放
        private int lan_noProtectTime = 500780;    // 无保护时间


        private string[] stateIcon = new[]
        {
            "ui_map[img_map_defense]",
            "ui_map[img_map_defense]",
            "ui_map[img_map_war]",
            "ui_map[img_map_war]",
            "ui_map[img_map_defense]",
        };

        private Timer timer;

        public void Refresh(BuildingState state, long time, string icon)
        {
            m_btn_timebg_GameButton.gameObject.SetActive(true);
            if (timer != null)
            {
                Timer.Cancel(timer);
                timer = null;
            }
            
            m_btn_timebg_GameButton.onClick.RemoveAllListeners();
            m_btn_timebg_GameButton.onClick.AddListener(() =>
            {
                int lan = tips_noOpen;
                switch (state)
                {
                    case BuildingState.NotUnlock:
                        lan = tips_noOpen;
                        break;
                    case BuildingState.Protecting:
                    case BuildingState.InitProtecting:
                        lan = tips_protect;
                        break;
                    case BuildingState.InitFighting:
                    case BuildingState.Fighting:
                        lan = tips_fighting;
                        break;
                }
                HelpTip.CreateTip(lan, m_btn_timebg_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
            });
            
            try
            {
                ClientUtils.LoadSprite(m_img_timeIcon_PolygonImage, stateIcon[(int)state]);
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
            }
            catch (Exception e)
            {
                CoreUtils.logService.Warn("[UI_Item_IconAndTime_SubView]   状态有误");
            }

            
            // 刷新状态文字 (初始保护，保护中，争夺中，显示时间)
            if ((state == BuildingState.Protecting || state == BuildingState.InitProtecting || state == BuildingState.Fighting) && time > 0)
            {
                timer = Timer.Register(0.8f, () =>
                {
                    m_lbl_time_LanguageText.text = UIHelper.GetFixedDHMSCounterDown(time);
                }, null, true, false, m_lbl_time_LanguageText);
                m_lbl_time_LanguageText.text = UIHelper.GetFixedDHMSCounterDown(time);    
            }
            else if (state == BuildingState.NotUnlock)
            {
                m_lbl_time_LanguageText.text = LanguageUtils.getText(lan_notOpen);
            }
            else
            {
                m_lbl_time_LanguageText.text = LanguageUtils.getText(lan_noProtectTime);
            }

        }

    }
}