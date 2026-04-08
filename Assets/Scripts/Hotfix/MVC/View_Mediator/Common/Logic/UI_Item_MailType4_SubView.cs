// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月28日
// Update Time         :    2020年8月28日
// Class Description   :    UI_Item_MailType4_SubView 邮件 探索类报告
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using SprotoType;

namespace Game {
    public partial class UI_Item_MailType4_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private PlayerProxy m_playerProxy;
        private bool m_isInit;
        private Dictionary<string, GameObject> m_assetDic;
        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;
        private bool m_isInitList;

        private List<EmailInfoEntity> m_mailList;

        private int m_assetLoadStatus = 0;//1加载中 2已加载

        private List<int> m_readStatusList;
        private int m_menuSwitchVal = -1;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo, List<EmailInfoEntity> exploreReport)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            //m_exploreReport = exploreReport;
            if (!m_isInit)
            {
                m_readStatusList = new List<int>();
                m_mailList = new List<EmailInfoEntity>();

                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

                m_isInit = true;
            }

            ProcessNewStatus(exploreReport);

            m_mailList.Clear();
            m_mailList.AddRange(exploreReport);

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

            if (m_assetLoadStatus == 1)
            {
                return;
            }
            else if (m_assetLoadStatus == 2)
            {
                RefreshList();
            }
            else
            {
                m_assetLoadStatus = 1;
                ClientUtils.PreLoadRes(gameObject, m_sv_list_view_ListView.ItemPrefabDataList, OnLoadFinish);
            }
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

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            if (gameObject == null)
            {
                return;
            }
            m_assetLoadStatus = 2;
            m_assetDic = asset;
            RefreshList();
        }

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = OnExploreItem;
                m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
            }
            m_sv_list_view_ListView.FillContent(m_mailList.Count);
        }

        private void OnExploreItem(ListView.ListItem item)
        {
            int index = item.index;
            EmailInfoEntity emailInfo = m_mailList[index];
            DiscoverReportInfo report = emailInfo.discoverReport;
            MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)emailInfo.emailId);
            Vector2 pos = PosHelper.ServerPosToLocal(report.pos);
            int x = (int)pos.x;
            int y = (int)pos.y;
            UI_Item_ExploreView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_ExploreView>(item.go);
            itemView.m_img_reddot_PolygonImage.gameObject.SetActive(m_readStatusList[index] == 0);
            itemView.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(emailInfo.sendTime);
            itemView.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), emailInfo.emailContents);
            itemView.m_lbl_linkImageText.m_UI_Model_Link_LanguageText.text = string.Format(LanguageUtils.getText(300032), x, y);
            int rssID = (int)report.mapFixPointId;
            itemView.m_lbl_linkImageText.SetPos(x, y);
            itemView.m_lbl_linkImageText.RegisterPosJumpEvent();
            if (rssID != 0)
            {
                MapFixPointDefine mapPointDefine = CoreUtils.dataService.QueryRecord<MapFixPointDefine>(rssID);
                ResourceGatherTypeDefine define = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(mapPointDefine.type);

                if (define == null)
                {
                    CoreUtils.logService.Error($"服务器下发的采集邮件资源类型有误{emailInfo.emailId}");
                    return;
                }
                ClientUtils.LoadSprite(itemView.m_img_building_PolygonImage, define.icon);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_nameId);
                int villageCaveIndex = Mathf.CeilToInt(rssID / 64f);
                bool explored = false;
                if (m_playerProxy.CurrentRoleInfo != null && m_playerProxy.CurrentRoleInfo.villageCaves != null && m_playerProxy.CurrentRoleInfo.villageCaves.ContainsKey(villageCaveIndex))
                {
                    explored = (ulong)(m_playerProxy.CurrentRoleInfo.villageCaves[villageCaveIndex].rule & (1L << (rssID % 64))) > 0;
                }

                if (explored)
                {
                    itemView.m_btn_explore_GameButton.gameObject.SetActive(false);
                    itemView.m_lbl_explore_LanguageText.gameObject.SetActive(true);
                }
                else
                {
                    itemView.m_lbl_explore_LanguageText.gameObject.SetActive(false);
                    itemView.m_btn_explore_GameButton.gameObject.SetActive(true);
                    itemView.m_btn_explore_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_explore_GameButton.onClick.AddListener(() =>
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_Email);
                        AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                        WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
                        WorldCamera.Instance().ViewTerrainPos(x * 6 + 3, y * 6 + 3, 1000f, null);
                    });
                }
            }
            else
            {
                StrongHoldTypeDefine strongHoldType = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>((int)report.strongHoldType);
                ClientUtils.LoadSprite(itemView.m_img_building_PolygonImage, strongHoldType.icon);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(strongHoldType.l_nameId);
                itemView.m_lbl_explore_LanguageText.gameObject.SetActive(false);
                itemView.m_btn_explore_GameButton.gameObject.SetActive(false);
            }
        }
    }
}