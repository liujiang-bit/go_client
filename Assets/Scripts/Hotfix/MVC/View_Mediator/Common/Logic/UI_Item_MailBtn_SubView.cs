// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月3日
// Update Time         :    2020年8月3日
// Class Description   :    UI_Item_MailBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using System;

namespace Game {
    public partial class UI_Item_MailBtn_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private PlayerProxy m_playerProxy;
        public Action<UI_Item_MailBtn_SubView> BtnClickEvent;
        private bool m_isInit;
        private int m_index;

        public void Refresh(EmailInfoEntity emailInfo, int index, long selectIndex)
        {
            m_index = index;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

                m_btn_click_GameButton.onClick.AddListener(OnEmailClick);
                m_isInit = true;
            }
            MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)emailInfo.emailId);

            bool selected = selectIndex == emailInfo.emailIndex;
            m_img_bgActive_PolygonImage.gameObject.SetActive(selected);
            m_img_redpot_PolygonImage.gameObject.SetActive(emailInfo.status == 0);
            string title = "";
            if (emailInfo.senderInfo != null)
            {
                //个人邮件
                title = GetPersonalTilte(mailDefine, emailInfo);
            }
            else
            {
                title = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), emailInfo.titleContents);
            }
            ClientUtils.FormatBeyondText(m_lbl_title_LanguageText, title);

            string subTitle = "";
            if (mailDefine.group == 2)
            {
                subTitle = m_emailProxy.FormatBattleReportSubTitle(mailDefine.ID, LanguageUtils.getText(mailDefine.l_subheadingID), emailInfo.reportSubTile);
            }
            else
            {
                subTitle = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), emailInfo.subTitleContents);
            }
            ClientUtils.FormatBeyondText(m_lbl_desc_LanguageText, subTitle);

            m_lbl_time_LanguageText.text = UIHelper.FormatEmailSendTime(emailInfo.sendTime);
            m_lbl_time_LanguageText.gameObject.SetActive(true);

            //颜色控制
            if (selected)
            {
                ClientUtils.TextSetColor(m_lbl_title_LanguageText, "#000000");
                ClientUtils.TextSetColor(m_lbl_desc_LanguageText, "#000000");
            }
            else
            {
                ClientUtils.TextSetColor(m_lbl_title_LanguageText, "#E2D19B");
                ClientUtils.TextSetColor(m_lbl_desc_LanguageText, "#FFFFFF");
            }

            //设置头像
            if (mailDefine.type == 4 || mailDefine.type == 5)
            {
                m_img_icon_PolygonImage.gameObject.SetActive(false);
                m_UI_Model_PlayerHead.gameObject.SetActive(true);
                m_UI_Model_PlayerHead.LoadPlayerIcon(emailInfo.senderInfo.headId, emailInfo.senderInfo.headFrameID);
                //PlayerHeadDefine headDefine = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)emailInfo.senderInfo.headId);
                //if (headDefine != null)
                //{
                //    ClientUtils.LoadSprite(m_img_icon_PolygonImage, headDefine.icon);
                //}
                //else
                //{
                //    ClientUtils.LoadSprite(m_img_icon_PolygonImage, RS.PlayerDefaultHeadIcon);
                //}
            }
            else
            {
                m_img_icon_PolygonImage.gameObject.SetActive(true);
                m_UI_Model_PlayerHead.gameObject.SetActive(false);
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, mailDefine.icon);
            }

            if (emailInfo.takeEnclosure == false && emailInfo.rewards != null)
            {
                m_img_bag_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_img_bag_PolygonImage.gameObject.SetActive(false);
            }
        }

        private void OnEmailClick()
        {
            if (BtnClickEvent != null)
            {
                BtnClickEvent(this);
            }
        }

        public int GetIndex()
        {
            return m_index;
        }

        private string GetPersonalTilte(MailDefine mailDefine, EmailInfoEntity emailInfo)
        {
            string str = "";
            SenderInfo senderData = emailInfo.senderInfo;
            if (senderData.rid == m_playerProxy.CurrentRoleInfo.rid)
            {
                str = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), emailInfo.titleContents);
            }
            else
            {
                str = string.IsNullOrEmpty(senderData.guildAbbr) ? senderData.name : LanguageUtils.getTextFormat(300030, senderData.guildAbbr, senderData.name);
            }
            return str;
        }
    }
}