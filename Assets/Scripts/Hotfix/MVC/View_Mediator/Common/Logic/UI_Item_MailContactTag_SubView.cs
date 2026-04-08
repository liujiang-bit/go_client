// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Item_MailContactTag_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_MailContactTag_SubView : UI_SubView
    {
        private WriteEmailItemData m_itemData;
        private bool m_isInit;
        public Action<UI_Item_MailContactTag_SubView> SelectCallback;

        public void Refresh(WriteEmailItemData itemData)
        {
            if (!m_isInit)
            {
                m_isInit = true;
                m_btn_select_GameButton.onClick.AddListener(OnSelect);
            }
            m_itemData = itemData;
            if (itemData.DataType == 1)
            {
                m_lbl_titleName_LanguageText.text = LanguageUtils.getText(570088);
            }
            else
            {
                if (itemData.Level == 4)
                {
                    m_lbl_titleName_LanguageText.text = LanguageUtils.getTextFormat(570089, itemData.Level);
                }
                else
                {
                    m_lbl_titleName_LanguageText.text = LanguageUtils.getTextFormat(180306, itemData.Level);
                }
            }

            m_lbl_count_LanguageText.text = itemData.Count.ToString();
            m_img_select_PolygonImage.gameObject.SetActive(itemData.SelectedStatusList[0] == true);
        }

        public void OnSelect()
        {
            bool isActive = !m_img_select_PolygonImage.gameObject.activeSelf;
            m_img_select_PolygonImage.gameObject.SetActive(isActive);
            m_itemData.SelectedStatusList[0] = isActive;
            if (SelectCallback != null)
            {
                SelectCallback(this);
            }
        }

        public WriteEmailItemData GetItemData()
        {
            return m_itemData;
        }
    }
}