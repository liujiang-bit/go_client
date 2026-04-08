// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月18日
// Update Time         :    2020年5月18日
// Class Description   :    UI_Win_MailContactMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using System.Linq;
using log4net.Core;

namespace Game {
    public class WriteEmailItemData
    {
        public int ItemType; //1标题 2目标
        public int DataType; //1最近联系人 2联盟成员
        public int Count;
        public int Level;
        public List<bool> SelectedStatusList;     //0未选中 1选中 
        public List<GuildMemberInfoEntity> MemberDataList;
        public List<WriteAMailData> TargerDataList;
    }

    public class UI_Win_MailContactMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_MailContactMediator";

        private Dictionary<long, WriteAMailData> m_wirteTargets = new Dictionary<long, WriteAMailData>();

        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;
        private EmailProxy m_emailProxy;

        private int m_toggleType = 1;

        private List<WriteEmailItemData> m_contactList;
        private List<WriteEmailItemData> m_allianceList;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private bool m_isInitAllianceList;
        private bool m_isInitContactList;

        private bool m_isInitRefresh;

        #endregion

        //IMediatorPlug needs
        public UI_Win_MailContactMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_MailContactView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {
            //m_allianceProxy.getAllianceMembers();
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;

            view.m_lbl_none_LanguageText.gameObject.SetActive(false);
            view.m_pl_list1.gameObject.SetActive(false);
            view.m_pl_list2.gameObject.SetActive(false);

            m_toggleType = 1;

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list1_ListView.ItemPrefabDataList, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_pl_side1.m_ck_ck_GameToggle.onValueChanged.AddListener(OnLastContact);
            view.m_UI_Model_Window_Type1.m_pl_side2.m_ck_ck_GameToggle.onValueChanged.AddListener(OnAllianceMember);
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_btn_sure.m_btn_languageButton_GameButton.onClick.AddListener(OnSure);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            Refresh();
        }

        private void RefreshAllianceList()
        {
            if (!m_isInitAllianceList)
            {
                ListView.FuncTab funcTab2 = new ListView.FuncTab();
                funcTab2.ItemEnter = OnInitAllianceListViewItem;
                funcTab2.GetItemPrefabName = OnGetAlliancePrefab;
                view.m_sv_list2_ListView.SetInitData(m_assetDic, funcTab2);
                m_isInitAllianceList = true;
            }
            view.m_sv_list2_ListView.FillContent(m_allianceList.Count);
        }

        private void RefreshContactList()
        {
            if (!m_isInitContactList)
            {
                ListView.FuncTab funcTab2 = new ListView.FuncTab();
                funcTab2.ItemEnter = OnInitContactlyListItem;
                funcTab2.GetItemPrefabName = OnGetContactPrefab;
                view.m_sv_list1_ListView.SetInitData(m_assetDic, funcTab2);
                m_isInitContactList = true;
            }
            view.m_sv_list1_ListView.FillContent(m_contactList.Count);
        }

        private void Refresh()
        {
            m_isInitRefresh = true;
            if (m_toggleType == 2) //联盟
            {
                bool isOn = view.m_UI_Model_Window_Type1.m_pl_side2.m_ck_ck_GameToggle.isOn;
                view.m_UI_Model_Window_Type1.m_pl_side2.m_ck_ck_GameToggle.isOn = true;
                if (isOn)
                {
                    SwitchAlliance();
                }
            }
            else
            {
                bool isOn = view.m_UI_Model_Window_Type1.m_pl_side1.m_ck_ck_GameToggle.isOn;
                view.m_UI_Model_Window_Type1.m_pl_side1.m_ck_ck_GameToggle.isOn = true;
                if (isOn)
                {
                    SwitchContact();
                }
            }
        }

        private string OnGetContactPrefab(ListView.ListItem item)
        {
            WriteEmailItemData itemData = m_contactList[item.index];
            if (itemData.ItemType == 1)
            {
                return "UI_Item_MailContactTag";
            }
            else
            {
                return "UI_Item_MailContactList";
            }
        }

        private void OnInitContactlyListItem(ListView.ListItem listItem)
        {
            WriteEmailItemData itemData = m_contactList[listItem.index];

            if (itemData.ItemType == 1)//标题
            {
                UI_Item_MailContactTag_SubView subView = null;
                if (listItem.data != null)
                {
                    subView = listItem.data as UI_Item_MailContactTag_SubView;
                }
                else
                {
                    subView = new UI_Item_MailContactTag_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SelectCallback = ContactTitleSelectCallback;
                }
                subView.Refresh(itemData);
            }
            else//成员
            {
                UI_Item_MailContactList_SubView subView = null;
                if (listItem.data != null)
                {
                    subView = listItem.data as UI_Item_MailContactList_SubView;
                }
                else
                {
                    subView = new UI_Item_MailContactList_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SelectCallback = ContactSelectCallback;
                }
                subView.Refresh(itemData);
            }
        }

        private string OnGetAlliancePrefab(ListView.ListItem item)
        {
            WriteEmailItemData itemData = m_allianceList[item.index];
            if (itemData.ItemType == 1)
            {
                return "UI_Item_MailContactTag";
            }
            else
            {
                return "UI_Item_MailContactList";
            }
        }

        private void OnInitAllianceListViewItem(ListView.ListItem listItem)
        {
            WriteEmailItemData itemData = m_allianceList[listItem.index];

            if (itemData.ItemType == 1)//标题
            {
                UI_Item_MailContactTag_SubView subView = null;
                if (listItem.data != null)
                {
                    subView = listItem.data as UI_Item_MailContactTag_SubView;
                }
                else
                {
                    subView = new UI_Item_MailContactTag_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SelectCallback = ContactTitleSelectCallback;
                }
                subView.Refresh(itemData);
            }
            else//成员
            {
                UI_Item_MailContactList_SubView subView = null;
                if (listItem.data != null)
                {
                    subView = listItem.data as UI_Item_MailContactList_SubView;
                }
                else
                {
                    subView = new UI_Item_MailContactList_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SelectCallback = ContactSelectCallback;
                }
                subView.Refresh(itemData);
            }
        }

        private void ContactTitleSelectCallback(UI_Item_MailContactTag_SubView subView)
        {
            WriteEmailItemData itemData = subView.GetItemData();
            bool isSelect = subView.m_img_select_PolygonImage.gameObject.activeSelf;

            if (itemData.DataType == 1)
            {
                for (int i = 0; i < m_contactList.Count; i++)
                {
                    if (m_contactList[i] != itemData)
                    {
                        for (int k = 0; k < m_contactList[i].SelectedStatusList.Count; k++)
                        {
                            m_contactList[i].SelectedStatusList[k] = isSelect;
                        }
                    }
                }
                view.m_sv_list1_ListView.ForceRefresh();
            }
            else
            {
                for (int i = 0; i < m_allianceList.Count; i++)
                {
                    if (m_allianceList[i].Level == itemData.Level && m_allianceList[i] != itemData)
                    {
                        for (int k = 0; k < m_allianceList[i].SelectedStatusList.Count; k++)
                        {
                            m_allianceList[i].SelectedStatusList[k] = isSelect;
                        }
                    }
                }
                view.m_sv_list2_ListView.ForceRefresh();
            }
        }

        private void ContactSelectCallback(UI_Item_MailContactList_SubView parentView, UI_Item_MailContact_SubView childSubView)
        {

        }

        private void OnLastContact(bool value)
        {
            if (!m_isInitRefresh)
            {
                return;
            }
            //Debug.LogError("switch OnLastContact:"+value);
            if(value)
            {
                m_toggleType = 1;
                SwitchContact();
            }
        }

        private void OnAllianceMember(bool value)
        {
            if (!m_isInitRefresh)
            {
                return;
            }
            //Debug.LogError("switch OnAllianceMember:" + value);
            if (value)
            {
                m_toggleType = 2;
                SwitchAlliance();
            }
        }

        private void SwitchAlliance()
        {
            view.m_pl_list1.gameObject.SetActive(false);
            view.m_pl_list2.gameObject.SetActive(true);
            if (m_allianceProxy.HasJionAlliance())
            {
                ReadAllianceData();
                if (m_allianceList.Count == 0)
                {
                    view.m_lbl_none_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_none_LanguageText.text = LanguageUtils.getText(570090);
                }
                else
                {
                    view.m_lbl_none_LanguageText.gameObject.SetActive(false);
                    RefreshAllianceList();
                }
            }
            else
            {

                view.m_lbl_none_LanguageText.gameObject.SetActive(true);
                view.m_lbl_none_LanguageText.text = LanguageUtils.getText(570091);
            }
        }

        private void SwitchContact()
        {
            view.m_pl_list2.gameObject.SetActive(false);
            ReadContactData();
            if (m_contactList.Count == 0)
            {
                view.m_pl_list1.gameObject.SetActive(false);
                view.m_lbl_none_LanguageText.gameObject.SetActive(true);
                view.m_lbl_none_LanguageText.text = LanguageUtils.getText(570092);
            }
            else
            {
                view.m_pl_list1.gameObject.SetActive(true);
                view.m_lbl_none_LanguageText.gameObject.SetActive(false);
                RefreshContactList();
            }
        }

        #region 数据获取

        //读取最近联系人
        private void ReadContactData()
        {
            if (m_contactList != null)
            {
                return;
            }
            List<WriteAMailData> localList = m_emailProxy.GetContactInfo();
            m_contactList = new List<WriteEmailItemData>();
            int count = localList.Count;
            if (localList.Count < 1)
            {
                return;
            }
            WriteEmailItemData itemData = new WriteEmailItemData();
            itemData.ItemType = 1;
            itemData.DataType = 1;
            itemData.Count = count;
            itemData.Level = 0;
            itemData.SelectedStatusList = new List<bool>();
            itemData.SelectedStatusList.Add(false);
            m_contactList.Add(itemData);

            int k = 0;
            while (k < count)
            {
                WriteEmailItemData itemData2 = new WriteEmailItemData();
                itemData2.ItemType = 2;
                itemData2.DataType = 1;
                itemData2.Level = 0;
                itemData2.SelectedStatusList = new List<bool>();
                itemData2.TargerDataList = new List<WriteAMailData>();
                itemData2.TargerDataList.Add(localList[k]);
                itemData2.SelectedStatusList.Add(false);
                if (k + 1 < count)
                {
                    itemData2.TargerDataList.Add(localList[k + 1]);
                    itemData2.SelectedStatusList.Add(false);
                }
                m_contactList.Add(itemData2);
                k = k + 2;
            }
            ClientUtils.Print(m_contactList);
        }

        //读取联盟数据
        private void ReadAllianceData()
        {
            if (m_allianceList != null)
            {
                return;
            }
            m_allianceList = new List<WriteEmailItemData>();

            Dictionary<long, AllianceMemberLevel> alliancedic = m_allianceProxy.GetAllianceLvMembers();
            Dictionary<long, List<GuildMemberInfoEntity>> tempDic = new Dictionary<long, List<GuildMemberInfoEntity>>();
            for (int i = 5; i > 0; i--)
            {
                int index = (i > 4) ? 4 : i;
                if (!tempDic.ContainsKey(index))
                {
                    tempDic[index] = new List<GuildMemberInfoEntity>();
                }
                if (alliancedic.ContainsKey(i))
                {
                    if (alliancedic[i].LevelMembers != null)
                    {
                        for (int k = 0; k < alliancedic[i].LevelMembers.Count; k++)
                        {
                            if (alliancedic[i].LevelMembers[k].rid != m_playerProxy.CurrentRoleInfo.rid)
                            {
                                tempDic[index].Add(alliancedic[i].LevelMembers[k]);
                            }
                        }
                    }
                }
            }

            for (int i = 4; i > 0; i--)
            {
                int count = tempDic.ContainsKey(i) ? tempDic[i].Count : 0;

                WriteEmailItemData itemData = new WriteEmailItemData();
                itemData.ItemType = 1;
                itemData.DataType = 2;
                itemData.Count = count;
                itemData.Level = i;
                itemData.SelectedStatusList = new List<bool>();
                itemData.SelectedStatusList.Add(false);
                m_allianceList.Add(itemData);

                int k = 0;
                while (k < count)
                {
                    WriteEmailItemData itemData2 = new WriteEmailItemData();
                    itemData2.ItemType = 2;
                    itemData2.DataType = 2;
                    itemData2.Level = i;
                    itemData2.SelectedStatusList = new List<bool>();
                    itemData2.MemberDataList = new List<GuildMemberInfoEntity>();
                    itemData2.MemberDataList.Add(tempDic[i][k]);
                    itemData2.SelectedStatusList.Add(false);
                    if (k + 1 < count)
                    {
                        itemData2.MemberDataList.Add(tempDic[i][k + 1]);
                        itemData2.SelectedStatusList.Add(false);
                    }
                    m_allianceList.Add(itemData2);
                    k = k + 2;
                }
            }
        }
        #endregion

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_mailContactList);
        }
 
        //确认
        private void OnSure()
        {
            Dictionary<long, bool> tDic = new Dictionary<long, bool>();
            List<WriteAMailData> dataList = new List<WriteAMailData>();
            if (m_contactList != null)
            {
                for (int i = 0; i < m_contactList.Count; i++)
                {
                    if (m_contactList[i].ItemType == 2)
                    {
                        for (int k = 0; k < m_contactList[i].SelectedStatusList.Count; k++)
                        {
                            if (m_contactList[i].SelectedStatusList[k])
                            {
                                WriteAMailData data = new WriteAMailData();
                                data.stableRid = m_contactList[i].TargerDataList[k].stableRid;
                                data.stableName = m_contactList[i].TargerDataList[k].stableName;
                                data.GuildAbbName = m_contactList[i].TargerDataList[k].GuildAbbName;
                                data.headId = (int)m_contactList[i].TargerDataList[k].headId;
                                data.headFrameID = (int)m_contactList[i].TargerDataList[k].headFrameID;

                                dataList.Add(data);
                                tDic[data.stableRid] = true;
                            }
                        }
                    }
                }
            }
            if (m_allianceList != null)
            {
                for (int i = 0; i < m_allianceList.Count; i++)
                {
                    if (m_allianceList[i].ItemType == 2)
                    {
                        for (int k = 0; k < m_allianceList[i].SelectedStatusList.Count; k++)
                        {
                            if (m_allianceList[i].SelectedStatusList[k] && !tDic.ContainsKey(m_allianceList[i].MemberDataList[k].rid))
                            {
                                WriteAMailData data = new WriteAMailData();
                                data.stableRid = m_allianceList[i].MemberDataList[k].rid;
                                data.stableName = m_allianceList[i].MemberDataList[k].name;
                                data.GuildAbbName = m_allianceProxy.GetAbbreviationName();
                                data.headId = (int)m_allianceList[i].MemberDataList[k].headId;
                                data.headFrameID = (int)m_allianceList[i].MemberDataList[k].headFrameID;
                                dataList.Add(data);
                            }
                        }
                    }
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.OnSelectEmailTarget, dataList);
            OnClose();
        }
    }
}