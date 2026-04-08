// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    UI_Win_ChoseChatMediator
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
using Data;
using System;

namespace Game {
    public class UI_Win_ChoseChatMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ChoseChatMediator";

        private ChatProxy m_chatProxy;
        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;
        private int type = 0;//0,选择界面，1确认界面

        private List<ChatContact> m_contacts;   //联系人列表
                                                //当前的联系人
        private ChatContact m_currentContact;
        //当前对话信息列表
        private ChatMsgList m_currentChatMsgList;
        private bool m_moveDownAfterReciveMsg = false;

        private Dictionary<string, GameObject> m_assetDic;
        private bool m_assetReady = false;
        bool isContactViewInited = false;
        private Color m_contactNameColor;
        //聊天字符限制
        private int channelWordLimit;
        private ChatShareData m_chatShareData;
        #endregion

        //IMediatorPlug needs
        public UI_Win_ChoseChatMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ChoseChatView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
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

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_chatShareData = view.data as ChatShareData;
            if (m_chatShareData == null)
            {
                Debug.LogError("传参异常");
            }
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            type = 0;
            m_contactNameColor = ClientUtils.StringToHtmlColor("#E2D19B");
            channelWordLimit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).channelWordLimit;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(() => { CoreUtils.uiManager.CloseUI(UI.s_choseChat); });
            view.m_btn_cancel.AddClickEvent(OnBtnCancelClick);
            view.m_btn_sure.AddClickEvent(OnBtnSureClick);
           /// view.m_ipt_text_GameInput.onValueChanged.AddListener(OnInputMsgChanged);
        }

        protected override void BindUIData()
        {
            var prefabsName = new List<string>();
            RefreshView();
            prefabsName.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabsName, OnLoadAssetFinished);
        }

        #endregion
        #region 按钮事件
        private void OnBtnCancelClick()
        {
            type = 0;
            RefreshView();
            RefreshListView();
        }


        private void OnBtnSureClick()
        {
            if (!m_chatProxy.CheckAliance(m_currentContact))
            {
                return;
            }
            if (!m_chatProxy.CheckMemberJurisdiction(m_currentContact))
            {
                return;
            }
            if (!m_chatProxy.CheckSilence())
            {
                return;
            }
            if (!m_chatProxy.CheckChannelInterva(m_currentContact))
            {
                return;
            }
            if (!m_chatProxy.CheckLvLimit(msgContentTarget, m_currentContact))
            {
                return;
            }
            if (!m_chatProxy.CheckMyMsgCount(msgContentTarget, m_currentContact, EnumMsgType.ChatShare))
            {
                return;
            }
            m_chatProxy.OnSendMsgSproto(msgContentTarget, m_currentContact);
            CoreUtils.uiManager.CloseUI(UI.s_choseChat);
            CoreUtils.uiManager.ShowUI(UI.s_chat,null, m_currentContact);
        }

        //private void OnInputMsgChanged(string msg)
        //{
        //    if (msg.Length > channelWordLimit)
        //    {
        //        view.m_ipt_text_GameInput.text = msg.Substring(0, channelWordLimit);
        //    }
        //}
        #endregion

        private void OnLoadAssetFinished(Dictionary<string, GameObject> assets)
        {
            m_assetDic = assets;
            m_assetReady = true;
            m_contacts = m_chatProxy.GetChatContactsBychatChannel(m_chatShareData.ChatShareDefine.chatChannel);
            m_contacts.Sort(m_chatProxy.SortContact);
            InitListView();
            view.m_sv_list_ListView.FillContent(m_contacts.Count);
        }

        private string GetContactItemPrefabName(ListView.ListItem item)
        {
            return "UI_Item_ChoseChat";
        }
        private void InitContactListItem(ListView.ListItem item)
        {
            int index = item.index;
            ChatContact contact = m_contacts[index];

            UI_Item_ChoseChat_SubView itemView = null;
            if (item.data == null)
            {
                itemView = new UI_Item_ChoseChat_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            else
            {
                itemView = item.data as UI_Item_ChoseChat_SubView;
            }
            itemView.gameObject.SetActive(true);
            ChatChannelDefine define;

            switch (contact.channelType)
            {
                case EnumChatChannel.world:
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(750011);
                    define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)EnumChatChannel.world);
                    itemView.m_UI_Model_PlayerHead.LoadHeadImg(define.icon);
                    itemView.m_UI_Model_PlayerHead.HideFrameImg();
                    break;
                case EnumChatChannel.alliance:
                    itemView.m_lbl_name_LanguageText.text = m_allianceProxy.GetAlliance()?.name;
                    define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)EnumChatChannel.alliance);
                    itemView.m_UI_Model_PlayerHead.LoadHeadImg(define.icon);
                    itemView.m_UI_Model_PlayerHead.HideFrameImg();
                    break;
                case EnumChatChannel.privatechat:
                    var contactInfo = m_chatProxy.GetContactInfo(contact.rid);
                    itemView.m_lbl_name_LanguageText.text = contactInfo.name;
                    //隐藏临时聊天
                    itemView.gameObject.SetActive(!contact.IsHide());
                    itemView.m_UI_Model_PlayerHead.ShowFrameImg();
                    itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(contactInfo.headID, contactInfo.headFrameID);
                    break;
                default: break;
            }
            itemView.m_btn_choose_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_choose_GameButton.onClick.AddListener(() =>
            {
                SetCurrentContact(contact, true);
            });

        }

        private void SetCurrentContact(ChatContact chatContact, bool moveToDown = false)
        {
            type = 1;
            m_currentContact = chatContact;
            m_currentChatMsgList = m_currentContact.ChatMsgList;
            RefreshView();
            RefreshShareContentView();
        }
        //联系人操作栏显示控制
        private Dictionary<string, bool> m_optionBarStatusDic = new Dictionary<string, bool>();
        private bool GetOptionBarStatus(ChatContact contact)
        {
            var key = contact.channelType.ToString() + contact.rid.ToString();
            if (m_optionBarStatusDic.ContainsKey(key))
            {
                return m_optionBarStatusDic[key];
            }
            else
            {
                m_optionBarStatusDic.Add(key, false);
                return false;
            }
        }
        private void RefreshView()
        {
            if (type == 0)
            {
                view.m_pl_share.gameObject.SetActive(false);
            }
            else
            {
                view.m_pl_share.gameObject.SetActive(true);
            }
        }
        private void InitListView()
        {
            if (isContactViewInited)
            {
                return;
            }
            isContactViewInited = true;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = InitContactListItem;
            funcTab.GetItemPrefabName = GetContactItemPrefabName;
            view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
        }
        private void RefreshListView()
        {
            m_contacts = m_chatProxy.GetChatContactsBychatChannel(m_chatShareData.ChatShareDefine.chatChannel);
            m_contacts.Sort(m_chatProxy.SortContact);
            InitListView();
            view.m_sv_list_ListView.FillContent(m_contacts.Count);
        }
        private string msgContentTarget;
        private void RefreshShareContentView()
        {
            ChatChannelDefine define;
            switch (m_currentContact.channelType)
            {
                case EnumChatChannel.world:
                    view.m_UI_Item_ChoseContact.SetNane(LanguageUtils.getText(750011));
                    define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)EnumChatChannel.world);
                    view.m_UI_Item_ChoseContact.m_UI_Model_PlayerHead.LoadHeadImg(define.icon);
                    view.m_UI_Item_ChoseContact.m_UI_Model_PlayerHead.HideFrameImg();
                    break;
                case EnumChatChannel.alliance:
                    view.m_UI_Item_ChoseContact.SetNane(m_allianceProxy.GetAlliance()?.name);
                    define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)EnumChatChannel.alliance);
                    view.m_UI_Item_ChoseContact.m_UI_Model_PlayerHead.LoadHeadImg(define.icon);
                    view.m_UI_Item_ChoseContact.m_UI_Model_PlayerHead.HideFrameImg();
                    break;
                case EnumChatChannel.privatechat:
                    var contactInfo = m_chatProxy.GetContactInfo(m_currentContact.rid);
                    view.m_UI_Item_ChoseContact.SetNane(contactInfo.name);
                    view.m_UI_Item_ChoseContact.m_UI_Model_PlayerHead.ShowFrameImg();
                    view.m_UI_Item_ChoseContact.m_UI_Model_PlayerHead.LoadPlayerIcon(contactInfo.headID, contactInfo.headFrameID);
                    break;
                default: break;
            }
            string lbl_share = "";
            if (m_chatProxy.ConvertContactToString(m_currentContact, m_chatShareData, out msgContentTarget, out lbl_share))
            {
                view.m_lbl_share_LanguageText.text = lbl_share;
            }
            else
            {
                view.m_lbl_share_LanguageText.text = "";

            }
        }

    }
}