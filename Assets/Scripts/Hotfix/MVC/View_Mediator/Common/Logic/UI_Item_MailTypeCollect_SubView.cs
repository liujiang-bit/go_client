// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年11月2日
// Update Time         :    2020年11月2日
// Class Description   :    UI_Item_MailTypeCollect_SubView 采集报告
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Item_MailTypeCollect_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;

        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;
        private List<EmailInfoEntity> m_mailList;

        private bool m_isInitList;
        private int m_prefabLoadStatus = 1; //1未加载 2加载中 3已加载
        private Dictionary<string, GameObject> m_assetDic;

        private List<int> m_readStatusList;
        private int m_menuSwitchVal = -1;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo, List<EmailInfoEntity> mailList)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            if (!m_isInit)
            {
                m_readStatusList = new List<int>();
                m_mailList = new List<EmailInfoEntity>();
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }

            ProcessNewStatus(mailList);

            m_mailList.Clear();
            m_mailList.AddRange(mailList);

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

            //显示列表
            if (m_prefabLoadStatus == 1)
            {
                m_prefabLoadStatus = 2;
                ClientUtils.PreLoadRes(gameObject, m_sv_list_view_ListView.ItemPrefabDataList, OnLoadFinish);
            }
            else if (m_prefabLoadStatus == 3)
            {
                RefreshList();
            }
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;
            m_prefabLoadStatus = 3;
            RefreshList();
        }

        private void ProcessNewStatus(List<EmailInfoEntity> mailList)
        {
            int status = 1;
            if (m_mailList != null && mailList.Count == m_mailList.Count)
            {
                for (int i = 0; i < mailList.Count; i++)
                {
                    if (mailList[i].emailIndex != m_mailList[i].emailIndex)
                    {
                        status = 2;
                        break;
                    }
                }
            }
            else
            {
                status = 2;
            }
            if (status == 1)
            {
                if (m_menuSwitchVal != m_emailProxy.MenuSwitchVal)
                {
                    status = 2;
                }
            }

            if (status > 1)
            {
                m_menuSwitchVal = m_emailProxy.MenuSwitchVal;
                m_readStatusList.Clear();
                for (int i = 0; i < mailList.Count; i++)
                {
                    m_readStatusList.Add((int)mailList[i].status);
                }
            }
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
            m_sv_list_view_ListView.FillContent(m_mailList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            EmailInfoEntity emailInfo = m_mailList[listItem.index];
            if (emailInfo == null)
            {
                Debug.LogError("OnCollectionItem emailInfo null");
                return;
            }
            if (emailInfo.resourceCollectReport == null)
            {
                Debug.LogErrorFormat("OnCollectionItem resourceCollectReport null index:{0} emailId:{1}", emailInfo.emailIndex, emailInfo.emailId);
                return;
            }
            CollectReport collect = emailInfo.resourceCollectReport;
            if (collect.pos == null)
            {
                Debug.LogErrorFormat("OnCollectionItem pos null index:{0} emailId:{1}", emailInfo.emailIndex, emailInfo.emailId);
                return;
            }
            UI_Item_CollectReportView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_CollectReportView>(listItem.go);
            string icon = "";
            int nameId;
            if (collect.type == 1) //资源田
            {
                ResourceGatherTypeDefine define = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int)collect.resourceTypeId);
                if (define == null)
                {
                    CoreUtils.logService.Error($"服务器下发的采集邮件资源类型有误{collect.resourceTypeId}");
                    return;
                }
                icon = define.icon;
                nameId = define.l_nameId;
            }
            else //联盟资源中心
            {
                AllianceBuildingTypeDefine define = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)collect.resourceTypeId);
                if (define == null)
                {
                    CoreUtils.logService.Error($"服务器下发的采集邮件资源类型有误{collect.resourceTypeId}");
                    return;
                }
                icon = define.icon;
                nameId = define.l_nameId;
            }

            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, icon);
            itemView.m_lbl_languageText_LanguageText.text = collect.resource.ToString("N0");

            itemView.m_lbl_add_LanguageText.text = collect.extraResource > 0 ? string.Concat("+", collect.extraResource.ToString("N0")) : string.Empty;

            itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(nameId);
            Vector2 pos = PosHelper.ServerPosToLocal(collect.pos);
            itemView.m_lbl_linkImageText.m_UI_Model_Link_LanguageText.text = string.Format(LanguageUtils.getText(300032), pos.x, pos.y);
            itemView.m_lbl_linkImageText.SetPos((int)pos.x, (int)pos.y);
            itemView.m_lbl_linkImageText.RegisterPosJumpEvent();
            itemView.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(emailInfo.sendTime);
            itemView.m_img_reddot_PolygonImage.gameObject.SetActive(m_readStatusList[listItem.index] == 0);

        }
    }
}