// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月12日
// Update Time         :    2020年6月12日
// Class Description   :    UI_Item_MailType7_SubView 邮件 联盟领土报告
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailType7_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private Dictionary<long, UI_Model_GuildRes_SubView> m_resDic;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;

                m_resDic = new Dictionary<long, UI_Model_GuildRes_SubView>();
                m_resDic[(long)EnumCurrencyType.leaguePoints] = m_pl_res1;
                m_resDic[(long)EnumCurrencyType.allianceFood] = m_pl_res2;
                m_resDic[(long)EnumCurrencyType.allianceWood] = m_pl_res3;
                m_resDic[(long)EnumCurrencyType.allianceStone] = m_pl_res4;
                m_resDic[(long)EnumCurrencyType.allianceGold] = m_pl_res5;
                m_isInit = true;

            }

            m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), mailInfo.titleContents);
            m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), mailInfo.subTitleContents);
            m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(mailInfo.sendTime);
            if (!string.IsNullOrEmpty(mailDefine.poster))
            {
                ClientUtils.LoadSprite(m_UI_Item_MailTitle.m_img_bg_PolygonImage, mailDefine.poster);
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }

            m_lbl_mes_LinkImageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), mailInfo.emailContents);
            m_lbl_mes_LinkImageText.onHrefClick.RemoveAllListeners();
            m_lbl_mes_LinkImageText.onHrefClick.AddListener((str) => {
                str = m_emailProxy.CoordinateReverse(str, m_lbl_mes_LinkImageText.isArabicText);
                m_emailProxy.CoordinateJump(str);
            });
            m_lbl_mes_LinkImageText.gameObject.SetActive(true);

            if (mailDefine.ID == 300010) //建造成功
            {
                m_pl_node1.gameObject.SetActive(true);
                m_lbl_build_LanguageText.text = LanguageUtils.getText(570074);

                if (mailInfo.guildEmail != null)
                {
                    m_UI_PlayerHead.LoadPlayerIcon(mailInfo.guildEmail.roleHeadId, mailInfo.guildEmail.roleHeadFrameId);
                    m_lbl_name_LanguageText.text = mailInfo.guildEmail.roleName;
                }
            }
            else if (mailDefine.ID == 300011) //建造失败
            {
                m_pl_node1.gameObject.SetActive(true);
                m_lbl_build_LanguageText.text = LanguageUtils.getText(570074);

                if (mailInfo.guildEmail != null)
                {
                    m_UI_PlayerHead.LoadPlayerIcon(mailInfo.guildEmail.roleHeadId, mailInfo.guildEmail.roleHeadFrameId);
                    m_lbl_name_LanguageText.text = mailInfo.guildEmail.roleName;
                }
            }
            else if (mailDefine.ID == 300012) //建筑拆除
            {
                m_pl_node1.gameObject.SetActive(true);
                m_lbl_build_LanguageText.text = LanguageUtils.getText(570097);

                if (mailInfo.guildEmail != null)
                {
                    m_UI_PlayerHead.LoadPlayerIcon(mailInfo.guildEmail.roleHeadId, mailInfo.guildEmail.roleHeadFrameId);
                    m_lbl_name_LanguageText.text = mailInfo.guildEmail.roleName;
                }
            }
            else if (mailDefine.ID == 300013) //建筑摧毁
            {
                m_pl_node1.gameObject.SetActive(false);
                m_pl_node2.gameObject.SetActive(false);
            }
            else if (mailDefine.ID == 300014) //进攻方
            {
                m_pl_node1.gameObject.SetActive(true);
                m_lbl_build_LanguageText.text = LanguageUtils.getText(570098);

                if (mailInfo.guildEmail != null)
                {
                    m_UI_PlayerHead.LoadPlayerIcon(mailInfo.guildEmail.roleHeadId, mailInfo.guildEmail.roleHeadFrameId);
                    m_lbl_name_LanguageText.text = string.IsNullOrEmpty(mailInfo.guildEmail.guildAbbName) ? mailInfo.guildEmail.roleName :
                        LanguageUtils.getTextFormat(300030, mailInfo.guildEmail.guildAbbName, mailInfo.guildEmail.roleName);
                }
            }

            if (mailInfo.guildEmail != null && mailInfo.guildEmail.buildCostGuildCurrencies != null)
            {
                int count = mailInfo.guildEmail.buildCostGuildCurrencies.Count;
                if (count > 0)
                {
                    m_pl_node2.gameObject.SetActive(true);
                    foreach (var data in m_resDic)
                    {
                        data.Value.gameObject.SetActive(false);
                    }
                    for (int i = 0; i < count; i++)
                    {
                        SprotoType.CurrencyInfo currencyInfo = mailInfo.guildEmail.buildCostGuildCurrencies[i];
                        if (m_resDic.ContainsKey(currencyInfo.type))
                        {
                            m_resDic[currencyInfo.type].gameObject.SetActive(true);
                            m_resDic[currencyInfo.type].m_lbl_resnum_LanguageText.text = ClientUtils.CurrencyFormat(currencyInfo.num);
                        }
                    }
                    return;
                }
            }

            m_pl_node2.gameObject.SetActive(false);
            foreach (var data in m_resDic)
            {
                data.Value.gameObject.SetActive(false);
            }
        }
    }
}