// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月4日
// Update Time         :    2020年9月4日
// Class Description   :    UI_Item_MailType19_SubView 活动邮件
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
    public class MailActivityItemData
    {
        public int ItemType;
        public RoleList RoleData;
    }

    public partial class UI_Item_MailType19_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;

        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;

        private bool m_isInitList;
        private int m_prefabLoadStatus;  //1未加载 2加载中 3已加载
        private Dictionary<string, GameObject> m_assetDic;
        private List<MailActivityItemData> m_itemList;
        private Dictionary<int, float> m_itemHeightDic = new Dictionary<int, float>();

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            if (mailInfo.roleList == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            if (!m_isInit)
            {
                m_prefabLoadStatus = 1;
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_itemList = new List<MailActivityItemData>();
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

            ReadListData();

            //显示列表
            if (m_prefabLoadStatus == 1)
            {
                m_prefabLoadStatus = 2;
                ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, OnLoadFinish);
            }
            else if (m_prefabLoadStatus == 3)
            {
                RefreshList();
            }
        }

        private void ReadListData()
        {
            m_itemList.Clear();

            MailActivityItemData itemData1 = new MailActivityItemData();
            itemData1.ItemType = 1;
            m_itemList.Add(itemData1);

            for (int i = 0; i < m_emailInfo.roleList.Count; i++)
            {
                MailActivityItemData itemData = new MailActivityItemData();
                itemData.ItemType = 2;
                itemData.RoleData = m_emailInfo.roleList[i];
                m_itemList.Add(itemData);
            }
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;
            m_itemHeightDic[1] = m_assetDic["UI_Item_MailType19Text"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[2] = m_assetDic["UI_Item_MailType19List"].GetComponent<RectTransform>().rect.height;
            m_prefabLoadStatus = 3;

            RefreshList();
        }

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                funcTab.GetItemPrefabName = GetItemPrefabName;
                m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                m_isInitList = true;
            }
            m_sv_list_ListView.FillContent(m_itemList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            MailActivityItemData itemData = m_itemList[listItem.index];
            if (itemData.ItemType == 1)
            {
                UI_Item_MailType19Text_SubView subView = null;
                if (listItem.data == null)
                {
                    subView = new UI_Item_MailType19Text_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                }
                else
                {
                    subView = listItem.data as UI_Item_MailType19Text_SubView;
                }
                subView.m_lbl_mes_link_HrefText.onHrefClick.RemoveAllListeners();
                subView.m_lbl_mes_link_HrefText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_emailDefine.l_mesID), m_emailInfo.emailContents);
                subView.m_lbl_mes_link_HrefText.onHrefClick.AddListener((str) =>
                {
                    str = m_emailProxy.CoordinateReverse(str, subView.m_lbl_mes_link_HrefText.isArabicText);
                    m_emailProxy.CoordinateJump(str);
                });
            }
            else
            {
                UI_Item_MailType19List_SubView subView = null;
                if (listItem.data == null)
                {
                    subView = new UI_Item_MailType19List_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                }
                else
                {
                    subView = listItem.data as UI_Item_MailType19List_SubView;
                }
                subView.Refresh(itemData.RoleData);
            }
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            int itemType = m_itemList[listItem.index].ItemType;
            return m_itemHeightDic[itemType];
        }

        private string GetItemPrefabName(ListView.ListItem listItem)
        {
            int itemType = m_itemList[listItem.index].ItemType;
            string name = "";
            if (itemType == 1)
            {
                name = "UI_Item_MailType19Text";
            }
            else
            {
                name = "UI_Item_MailType19List";
            }
            return name;
        }
    }
}