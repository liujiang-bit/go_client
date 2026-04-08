// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月19日
// Update Time         :    2020年6月19日
// Class Description   :    UI_Item_MailType17_SubView 邮件 被侦查内容
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
    public partial class UI_Item_MailType17_SubView : UI_SubView
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
            m_assetDic = asset;
            m_prefabLoadStatus = 3;

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
            m_sv_list_view_ListView.FillContent(m_mailList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            EmailInfoEntity emailInfo = m_mailList[listItem.index];

            UI_Item_MailBeScout_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_MailBeScout_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_MailBeScout_SubView;
            }
            ScoutReportInfo scoutInfo = emailInfo.scoutReport;

            //ClientUtils.Print(emailInfo);

            //时间
            subView.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(emailInfo.sendTime);

            //坐标
            if (scoutInfo.pos != null)
            {
                subView.m_lbl_linkImageText.m_UI_Model_Link_LanguageText.text = PosHelper.FormatServerPos(scoutInfo.pos);
                subView.m_lbl_linkImageText.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_lbl_linkImageText.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    if (scoutInfo != null && scoutInfo.pos != null)
                    {
                        Vector3 worldPos = PosHelper.ServerPosToClient(scoutInfo.pos);
                        CoreUtils.uiManager.CloseUI(UI.s_Email);
                        WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
                        WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.z, 1000f, null);
                    }
                });
            }

            //内容
            MailDefine emailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)emailInfo.emailId);
            if (emailDefine != null)
            {
                subView.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(emailDefine.l_mesID), emailInfo.emailContents);
            }

            //名称
            subView.m_lbl_playername.m_UI_Model_Link_LanguageText.text = string.IsNullOrEmpty(scoutInfo.guildAbbName) ? scoutInfo.scoutRole.name :
                                                   LanguageUtils.getTextFormat(300030, scoutInfo.guildAbbName, scoutInfo.scoutRole.name);
            subView.m_lbl_playername.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            subView.m_lbl_playername.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                if (scoutInfo != null && scoutInfo.pos != null)
                {
                    if (scoutInfo.scoutRole != null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo, null, scoutInfo.scoutRole.rid);
                    }
                }
            });

            //头像
            if (scoutInfo.scoutRole != null)
            {
                subView.m_UI_PlayerHead.LoadPlayerIcon(scoutInfo.scoutRole.headId, scoutInfo.scoutRole.headFrameID);
            }


            //红点
            subView.m_img_reddot_PolygonImage.gameObject.SetActive(m_readStatusList[listItem.index] == 0);

            //ClientUtils.Print(scoutInfo);

            //标题
            if (scoutInfo.targetType == 1)//城市
            {
                subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(570083);
            }
            else if (scoutInfo.targetType == 5) //联盟建筑
            {
                AllianceBuildingTypeDefine allianceBuild = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)scoutInfo.objectTypeId);
                if (allianceBuild != null)
                {
                    subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(allianceBuild.l_nameId);
                }
            }
            else if (scoutInfo.targetType == 2 || scoutInfo.targetType == 3) //玩家部队 集结部队
            {
                subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(300065);
            }
            else if (scoutInfo.targetType == 6 || scoutInfo.targetType == 7) //关卡 圣地
            {
                StrongHoldDataDefine strongHoldData = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)scoutInfo.objectTypeId);
                if (strongHoldData != null)
                {
                    StrongHoldTypeDefine strongHold = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldData.type);
                    if (strongHold != null)
                    {
                        subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(strongHold.l_nameId);
                    }
                }
            }
        }
    }
}