// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月19日
// Update Time         :    2020年3月19日
// Class Description   :    UI_Item_SoldierHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using Data;

namespace Game {
    public partial class UI_Item_SoldierHead_SubView : UI_SubView
    {
        private bool m_isInit;
        private bool m_isRegisterEvent;
        private int m_soldierId;

        public long soldierId;
        public void SetSoldierInfo(string icon)
        {           
            ClientUtils.LoadSprite(m_img_head_PolygonImage, icon);
            m_lbl_count_LanguageText.gameObject.SetActive(false);
        }
        public void SetSoldierInfo(long num )
        {
            m_lbl_count_LanguageText.text = num.ToString("N0");
        }
        public void SetSoldierInfo(string icon, int num)
        {
            m_lbl_count_LanguageText.gameObject.SetActive(true);
            ClientUtils.LoadSprite(m_img_head_PolygonImage, icon);
            m_lbl_count_LanguageText.text = num.ToString("N0");
        }

        public void Refresh(SoldierInfo soldierInfo, bool isRegisterEvent = false)
        {
            if (!m_isInit)
            {
                m_btn_head_GameButton.onClick.AddListener(ClickHead);
                m_isInit = true;
            }
            m_isRegisterEvent = isRegisterEvent;
            m_soldierId = (int)soldierInfo.id;
            ArmsDefine armDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(m_soldierId);
            ClientUtils.LoadSprite(m_img_head_PolygonImage, armDefine.icon);
            m_lbl_count_LanguageText.text = soldierInfo.num.ToString("N0");
        }

        private void ClickHead()
        {
            if (!m_isRegisterEvent)
            {
                return;
            }
        }

        public void HeadBtnAddOnClick(int soldierID)
        {
            m_btn_head_GameButton.onClick.RemoveAllListeners();
            m_btn_head_GameButton.onClick.AddListener(() =>
            {
                if (soldierID != 0)
                {
                    ArmsDefine armDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(soldierID);
                    HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(2100);
                    var tipStr = LanguageUtils.getTextFormat(define.l_typeID, LanguageUtils.getText(armDefine.l_typeName), LanguageUtils.getText(armDefine.l_armsID));
                    HelpTip.CreateTip(tipStr, m_btn_head_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetWidth(define.width).Show();
                }
                else
                {
                    HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(7000);
                    if (define != null)
                    {
                        var data1 = LanguageUtils.getTextFormat(define.l_typeID, LanguageUtils.getText(define.l_data1), LanguageUtils.getText(define.l_data2));
                        HelpTip.CreateTip(7000, m_btn_head_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetWidth(define.width).Show();
                    }
                }
            });
        }
    }
}