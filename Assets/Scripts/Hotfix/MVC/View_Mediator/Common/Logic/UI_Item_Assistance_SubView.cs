// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月15日
// Update Time         :    2020年6月15日
// Class Description   :    UI_Item_Assistance_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;

namespace Game {
    public partial class UI_Item_Assistance_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private Dictionary<long, UI_Item_AssistanceRes_SubView> m_resDic;

        public void Refresh(EmailInfoEntity emailInfo)
        {
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_resDic = new Dictionary<long,UI_Item_AssistanceRes_SubView>();
                m_resDic[(long)EnumCurrencyType.food] = m_UI_Item_AssistanceRes1;
                m_resDic[(long)EnumCurrencyType.wood] = m_UI_Item_AssistanceRes2;
                m_resDic[(long)EnumCurrencyType.stone] = m_UI_Item_AssistanceRes3;
                m_resDic[(long)EnumCurrencyType.gold] = m_UI_Item_AssistanceRes4;
                m_isInit = true;
            }
            m_img_reddot_PolygonImage.gameObject.SetActive(emailInfo.status == 0);
            m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(emailInfo.sendTime);
            if (emailInfo.guildEmail == null )
            {
                return;
            }
            m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(570076, emailInfo.guildEmail.roleName);


            if (emailInfo.guildEmail.transportResource != null && emailInfo.guildEmail.transportResource.Count > 0)
            {
                foreach (var data in m_resDic)
                {
                    data.Value.gameObject.SetActive(false);
                }
                List<CurrencyInfo> infoList = emailInfo.guildEmail.transportResource;
                for (int i = 0; i < infoList.Count; i++)
                {
                    if (m_resDic.ContainsKey(infoList[i].type) && infoList[i].num > 0)
                    {
                        m_resDic[infoList[i].type].gameObject.SetActive(true);
                        m_resDic[infoList[i].type].m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(infoList[i].num);
                    }
                }
                return;
            }
            foreach (var data in m_resDic)
            {
                data.Value.gameObject.SetActive(false);
            }
        }
    }
}