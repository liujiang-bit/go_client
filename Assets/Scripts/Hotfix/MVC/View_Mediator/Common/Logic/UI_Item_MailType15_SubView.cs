// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    UI_Item_MailType15_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_MailType15_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }
            m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), mailInfo.titleContents);
            m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), mailInfo.subTitleContents);
            m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(mailInfo.sendTime);
            m_lbl_Content_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), mailInfo.emailContents);
            if (!string.IsNullOrEmpty(mailDefine.poster))
            {
                ClientUtils.LoadSprite(m_UI_Item_MailTitle.m_img_bg_PolygonImage, mailDefine.poster);
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }

            if (mailInfo.guildEmail != null && mailInfo.guildEmail.strongHoldId > 0)
            {
                StrongHoldDataDefine dataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)mailInfo.guildEmail.strongHoldId);
                if (dataDefine == null)
                {
                    return;
                }
                StrongHoldTypeDefine typeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(dataDefine.type);
                ClientUtils.LoadSprite(m_icon_build_PolygonImage, typeDefine.iconImg);

                if (dataDefine.type == 10001 || dataDefine.type == 10002 || dataDefine.type == 10003)
                {
                    m_icon_buff1_PolygonImage.gameObject.SetActive(false);
                    m_icon_buff2_PolygonImage.gameObject.SetActive(false);
                    m_lbl_mes_LanguageText.gameObject.SetActive(true);
                }
                else
                {
                    m_lbl_mes_LanguageText.gameObject.SetActive(false);
                    if (typeDefine.buffData1 > 0)
                    {
                        m_icon_buff1_PolygonImage.gameObject.SetActive(true);
                        CityBuffDefine buffData = CoreUtils.dataService.QueryRecord<CityBuffDefine>(typeDefine.buffData1);
                        if (buffData != null)
                        {
                            ClientUtils.LoadSprite(m_icon_buff1_PolygonImage, buffData.icon);
                            m_lbl_buffatt1_LanguageText.text = ClientUtils.SafeFormat(LanguageUtils.getText(buffData.tag), buffData.tagData);
                        }
                    }
                    else
                    {
                        m_icon_buff1_PolygonImage.gameObject.SetActive(false);
                    }

                    if (typeDefine.buffData2 > 0)
                    {
                        m_icon_buff2_PolygonImage.gameObject.SetActive(true);
                        CityBuffDefine buffData = CoreUtils.dataService.QueryRecord<CityBuffDefine>(typeDefine.buffData2);
                        if (buffData != null)
                        {
                            ClientUtils.LoadSprite(m_icon_buff2_PolygonImage, buffData.icon);
                            m_lbl_buffatt2_LanguageText.text = ClientUtils.SafeFormat(LanguageUtils.getText(buffData.tag), buffData.tagData);
                        }
                    }
                    else
                    {
                        m_icon_buff2_PolygonImage.gameObject.SetActive(false);
                    }
                }

                Vector2 pos = new Vector2((int)dataDefine.posX/6,(int)dataDefine.posY/6);
                m_UI_Model_Link.m_UI_Model_Link_LanguageText.text = LanguageUtils.getTextFormat(300032, pos.x, pos.y);
                m_UI_Model_Link.SetPos((int)pos.x, (int)pos.y);
                m_UI_Model_Link.RegisterPosJumpEvent();
            }
        }
    }
}