// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月12日
// Update Time         :    2020年6月12日
// Class Description   :    UI_Item_MailType8_SubView 邮件 联盟捐献
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
    public class AllianceMemberDonateInfo
    {
        public List<RoleDonateInfo> DonateInfoList;
        public float Height;
        public AllianceDonateRankingDefine DonateRankingDefine;
    }

    public partial class UI_Item_MailType8_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private bool m_isInitList;
        private int m_prefabLoadStatus = 1; //1未加载 2加载中 3已加载
        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;
        private Dictionary<string, GameObject> m_assetDic;
        private List<AllianceMemberDonateInfo> m_donateList;
        private int m_type;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_donateList = new List<AllianceMemberDonateInfo>();
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

            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (mailDefine.ID == 300015) //每日捐献
            {
                m_type = 1;
                m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(550178), new List<string> { ClientUtils.FormatComma(config.AllianceDailyUpperLimit) });
            }
            else if (mailDefine.ID == 300016)//每周捐献
            {
                m_type = 2;
                m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(570096), new List<string> { ClientUtils.FormatComma(config.AllianceWeeklyUpperLimit) });
            }

            //处理数据
            ProcessData();

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

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                funcTab.GetItemSize = OnGetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;
                m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
                m_isInitList = true;
            }
            m_sv_list_view_ListView.RefreshAndRestPos(0);
            m_sv_list_view_ListView.FillContent(m_donateList.Count);
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            if (m_donateList[listItem.index].Height == -1000)
            {
                return 150;
            }
            return m_donateList[listItem.index].Height;
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            return "UI_Item_MailRank";
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            AllianceMemberDonateInfo DonateInfo = m_donateList[listItem.index];

            UI_Item_MailRank_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_MailRank_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_MailRank_SubView;
            }

            subView.Refresh(DonateInfo);

            if (m_donateList[listItem.index].Height == -1000)
            {
                m_donateList[listItem.index].Height = listItem.go.GetComponent<RectTransform>().rect.height;
                m_sv_list_view_ListView.RefreshItem(listItem.index);
            }
        }

        //处理数据
        private void ProcessData()
        {
            m_donateList.Clear();

            List<AllianceDonateRankingDefine> list = GetDonateRankingTempData(m_type);
            if (list.Count > 0)
            {
                List<AllianceMemberDonateInfo> donateList = new List<AllianceMemberDonateInfo>();
                for (int i = 0; i < list.Count; i++)
                {
                    AllianceMemberDonateInfo info = new AllianceMemberDonateInfo();
                    info.Height = -1000;
                    info.DonateRankingDefine = list[i];
                    info.DonateInfoList = new List<RoleDonateInfo>();
                    m_donateList.Add(info);
                }
            }

            if (m_emailInfo.guildEmail != null && m_emailInfo.guildEmail.roleDonates != null)
            {
                int count = m_emailInfo.guildEmail.roleDonates.Count;
                if (count < 1)
                {
                    return;
                }
                List<RoleDonateInfo> roleDonateList = m_emailInfo.guildEmail.roleDonates;
                if (list.Count > 0)
                {
                    int index = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        int min = list[i].targetMin;
                        int max = list[i].targetMax;
                        for (int j = min; j <= max; j++)
                        {
                            index = j - 1;
                            if (index < count)
                            {
                                m_donateList[i].DonateInfoList.Add(roleDonateList[index]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private List<AllianceDonateRankingDefine> GetDonateRankingTempData(int type)
        {
            List<AllianceDonateRankingDefine> list = new List<AllianceDonateRankingDefine>();
            int id = type * 1000;
            for (int i = 0; i < 500; i++)
            {
                AllianceDonateRankingDefine donateDefine = CoreUtils.dataService.QueryRecord<AllianceDonateRankingDefine>(id+i);
                if (donateDefine == null)
                {
                    break;
                }
                else
                {
                    list.Add(donateDefine);
                }
            }
            return list;
        }
    }
}