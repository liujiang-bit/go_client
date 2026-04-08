// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Item_MailContact_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using System;

namespace Game {
    public partial class UI_Item_MailContact_SubView : UI_SubView
    {
        private bool m_isInit;
        private AllianceProxy m_allianceProxy;
        public Action<UI_Item_MailContact_SubView, int> SelectCallback;
        private int m_index;

        public void Refresh(GuildMemberInfoEntity info, bool isSelect, int index)
        {
            InitData();
            m_index = index;
            this.m_UI_Model_PlayerHead.LoadPlayerIcon(info.headId, info.headFrameID);
            m_img_select_PolygonImage.gameObject.SetActive(isSelect);

            GuildInfoEntity guideInfo = m_allianceProxy.GetAlliance();
            if (guideInfo != null)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, guideInfo.abbreviationName, info.name);
            } else
            {
                m_lbl_name_LanguageText.text = info.name;
            }
        }

        public void Refresh(WriteAMailData info, bool isSelect, int index)
        {
            InitData();
            m_index = index;
            this.m_UI_Model_PlayerHead.LoadPlayerIcon(info.headId, info.headFrameID);
            m_img_select_PolygonImage.gameObject.SetActive(isSelect);
 
            if (string.IsNullOrEmpty(info.GuildAbbName))
            {
                m_lbl_name_LanguageText.text = info.stableName;
            }
            else
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, info.GuildAbbName, info.stableName);
            }
        }

        private void InitData()
        {
            if (!m_isInit)
            {
                m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                m_btn_select_GameButton.onClick.AddListener(OnSelect);
                m_isInit = true;
            }
        }

        private void OnSelect()
        {
            bool isActive = !m_img_select_PolygonImage.gameObject.activeSelf;
            m_img_select_PolygonImage.gameObject.SetActive(isActive);
            if (SelectCallback != null)
            {
                SelectCallback(this, m_index);
            }
        }
    }
}