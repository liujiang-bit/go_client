// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月11日
// Update Time         :    2020年6月11日
// Class Description   :    UI_Item_MailType6_SubView 邮件 加入联盟
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using PureMVC.Interfaces;
using System;
using Hotfix.Utils;

namespace Game {
    public partial class UI_Item_MailType6_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private long m_guildId;
        private EmailInfoEntity m_emailInfo;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    Guild_GetOtherGuildInfo.TagName,
                    Guild_ReplyGuildInvite.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_ReplyGuildInvite.TagName: //加入联盟 错误码提醒
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    var inviteResponse = notification.Body as Guild_ReplyGuildInvite.response;
                    if (inviteResponse == null)
                    {
                        return;
                    }
                    if (m_emailInfo != null && m_emailInfo.emailIndex == inviteResponse.emailIndex)
                    {
                        if (inviteResponse.result == true)//同意加入联盟 则关闭邮件界面
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_Email);
                        }
                    }
                    break;
                case Guild_GetOtherGuildInfo.TagName:   //联盟数据
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    var response = notification.Body as Guild_GetOtherGuildInfo.response;
                    if (response.guildId == m_guildId)
                    {
                        if (response.guildInfo != null)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo, null, response.guildInfo);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }
            m_emailInfo = mailInfo;

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

            //旗帜
            m_UI_GuildFlag.setData(mailInfo.guildEmail.signs);

            m_lbl_mes_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), mailInfo.emailContents);

            //查看联盟信息
            m_btn_info_GameButton.onClick.RemoveAllListeners();
            m_btn_info_GameButton.onClick.AddListener(()=> {
                m_guildId = mailInfo.guildEmail.guildId;
                Guild_GetOtherGuildInfo.request request = new Guild_GetOtherGuildInfo.request();
                request.guildId = m_guildId;
                AppFacade.GetInstance().SendSproto(request);
            });

            if (mailInfo.guildEmail.inviteStatus == 0)//未处理
            {
                m_lbl_join_LanguageText.gameObject.SetActive(false);
                m_lbl_reject_LanguageText.gameObject.SetActive(false);
                m_btn_join.m_btn_languageButton_GameButton.gameObject.SetActive(true);
                m_btn_reject.m_btn_languageButton_GameButton.gameObject.SetActive(true);
                //加入
                m_btn_join.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                m_btn_join.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                    Guild_ReplyGuildInvite.request request = new Guild_ReplyGuildInvite.request();
                    request.emailIndex = mailInfo.emailIndex;
                    request.result = true;
                    AppFacade.GetInstance().SendSproto(request);
                });

                //拒绝
                m_btn_reject.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                m_btn_reject.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                    Guild_ReplyGuildInvite.request request = new Guild_ReplyGuildInvite.request();
                    request.emailIndex = mailInfo.emailIndex;
                    request.result = false;
                    AppFacade.GetInstance().SendSproto(request);
                });
            }
            else if (mailInfo.guildEmail.inviteStatus == 1) //已同意
            {
                m_btn_join.m_btn_languageButton_GameButton.gameObject.SetActive(false);
                m_btn_reject.m_btn_languageButton_GameButton.gameObject.SetActive(false);
                m_lbl_join_LanguageText.gameObject.SetActive(true);
                m_lbl_reject_LanguageText.gameObject.SetActive(false);
            }
            else if (mailInfo.guildEmail.inviteStatus == 2) //已拒绝
            {
                m_btn_join.m_btn_languageButton_GameButton.gameObject.SetActive(false);
                m_btn_reject.m_btn_languageButton_GameButton.gameObject.SetActive(false);
                m_lbl_join_LanguageText.gameObject.SetActive(false);
                m_lbl_reject_LanguageText.gameObject.SetActive(true);
            }
        }
    }
}