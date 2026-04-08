// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_Item_MailType12_SubView 邮件 联盟留言
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using PureMVC.Interfaces;
using SprotoType;
using Hotfix.Utils;

namespace Game {
    public partial class UI_Item_MailType12_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private bool m_isInitList;
        public Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<EmailInfoEntity> m_leaveMsgList;
        private long m_guildId;
        private long m_msgIndex;

        private int m_isAssetLoadStatus = 0; // 1加载中 2已加载

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    Guild_CheckBoardMessage.TagName,
                    Guild_GetOtherGuildInfo.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_CheckBoardMessage.TagName:   //检查联盟留言信息是否存在                    
                    var response = notification.Body as Guild_CheckBoardMessage.response;
                    if (response.result == false)
                    {
                        m_guildId = 0;
                        m_msgIndex = response.messageIndex;
                        Tip.CreateTip(730342).Show();
                        return;
                    }
                    m_guildId = response.guildId;
                    m_msgIndex = response.messageIndex;
                    Guild_GetOtherGuildInfo.request request = new Guild_GetOtherGuildInfo.request();
                    request.guildId = response.guildId;
                    AppFacade.GetInstance().SendSproto(request);
                    break;
                case Guild_GetOtherGuildInfo.TagName:   //联盟数据
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        m_guildId = 0;
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    var guildResponse = notification.Body as Guild_GetOtherGuildInfo.response;
                    if (guildResponse.guildId == m_guildId)
                    {
                        //消息索引m_msgIndex
                        if (guildResponse.guildInfo != null)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_AllianceMsg, null, guildResponse.guildInfo);
                        }
                    }
                    m_guildId = 0;
                    break;
                default:
                    break;
            }
        }

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo, List<EmailInfoEntity> leaveMsgList)
        {
            if (!m_isInit)
            {
                m_leaveMsgList = new List<EmailInfoEntity>();
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }
            m_leaveMsgList.Clear();
            m_leaveMsgList.AddRange(leaveMsgList);

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

            if (m_isAssetLoadStatus == 1) //加载中
            {
                return;
            }
            if (m_isAssetLoadStatus == 2) //已加载
            {
                RefreshList();
            }
            else
            {
                m_isAssetLoadStatus = 1;
                ClientUtils.PreLoadRes(gameObject, m_sv_list_view_ListView.ItemPrefabDataList, OnLoadFinish);
            }
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_isAssetLoadStatus = 2;
            m_assetDic = asset;
            RefreshList();
        }

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
                m_isInitList = true;
            }
            int showLimit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).emailMessageSave;
            int num = (m_leaveMsgList.Count > showLimit) ? showLimit : m_leaveMsgList.Count;
            m_sv_list_view_ListView.FillContent(num);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            EmailInfoEntity emailInfo = m_leaveMsgList[listItem.index];

            UI_Item_MailType12List_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_MailType12List_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_MailType12List_SubView;
            }
            subView.Refresh(emailInfo);
        }
    }
}