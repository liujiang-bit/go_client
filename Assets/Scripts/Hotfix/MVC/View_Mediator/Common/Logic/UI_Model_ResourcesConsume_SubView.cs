// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    UI_Model_ResourcesConsume_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using Data;

namespace Game
{
    public partial class UI_Model_ResourcesConsume_SubView : UI_SubView
    {
        public void SetResourcesConsume(string icon, int value, bool isEnough = true)
        {
            if (m_img_icon_PolygonImage.assetName != icon)
            {
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
            }
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
            m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(value); 
            m_img_icon_PolygonImage.raycastTarget = false;
            m_lbl_languageText_LanguageText.raycastTarget = false;
            if (isEnough)
            {
                ClientUtils.TextSetColor(m_lbl_languageText_LanguageText, RS.ResourcesConsumeDefaultColor);
            }
            else
            {
                m_lbl_languageText_LanguageText.color = Color.red;
            }
        }

        public void SetRes(long num)
        {
            m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(num);
        }

        public void AddClickEvent(UnityAction callback)
        {
            m_btn_btn_GameButton.onClick.AddListener(callback);
        }

        public void SetResourcesConsumeReward(RewardGroupData itemPackageShowDefine)
        {
            string icon = GetIconfromItemPackageShow(itemPackageShowDefine);
            if (m_img_icon_PolygonImage.assetName != icon)
            {
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
            }
            m_lbl_languageText_LanguageText.text = itemPackageShowDefine.number.ToString("N0");
            m_img_icon_PolygonImage.raycastTarget = false;
            m_lbl_languageText_LanguageText.raycastTarget = false;
            AddClickEvent(itemPackageShowDefine);
        }

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="itemPackageShow"></param>
        /// <returns></returns>
        private string GetIconfromItemPackageShow(RewardGroupData itemPackageShow)
        {
            string icon = string.Empty;
            switch ((EnumRewardType)itemPackageShow.RewardType)
            {
                case EnumRewardType.Currency:
                    icon = itemPackageShow.CurrencyData.iconID;
                    break;
                case EnumRewardType.Soldier:
                    icon = itemPackageShow.SoldierData.icon;
                    break;
                case EnumRewardType.Item:
                    icon = itemPackageShow.ItemData.itemIcon;
                    break;
            }
            return icon;
        }
        private void AddClickEvent(RewardGroupData itemPackageShow)
        {
            switch ((EnumRewardType)itemPackageShow.RewardType)
            {
                case EnumRewardType.Currency:
                    m_btn_btn_GameButton.enabled = false;
                    break;
                case EnumRewardType.Soldier:
                    {
                        ArmsDefine armsDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(itemPackageShow.SoldierData.ID);
                        m_btn_btn_GameButton.enabled = true;
                        m_btn_btn_GameButton.onClick.RemoveAllListeners();
                        float offset = m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
                        m_btn_btn_GameButton.onClick.AddListener(() =>
                        {
                            HelpTip.CreateTip(LanguageUtils.getText(armsDefine.l_armsID), m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                        });
                    }
                    break;
                case EnumRewardType.Item:
                    {
                        m_btn_btn_GameButton.enabled = true;
                        m_btn_btn_GameButton.onClick.RemoveAllListeners();
                        float offset = m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
                        m_btn_btn_GameButton.onClick.AddListener(() =>
                          {
                              HelpTip.CreateTip(LanguageUtils.getText(itemPackageShow.ItemData.l_nameID), m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                          });
                    }
                    break;
            }

        }
    }
}