// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Win_ChatMediator
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
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

namespace Game {
    public enum EnumScrollRectDragingState
    {
        None,
        Ondraging,
        OnLoading,
    }

    public enum ChatTranslateState
    {
        NoTranslation,
        Translating,
        Translated
    }

    public class EmojiMenuInfo
    {
        public int Group { get; set; }
        public string EmojiKey { get; set; }
    }

    public class EmojiListInfo
    {
        public List<ChatEmojiDefine> Infos { get; set; } = new List<ChatEmojiDefine>();
    }

    public class UI_Win_ChatMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_ChatMediator";

        ChatProxy m_chatProxy;
        PlayerProxy _playerProxy;
        AllianceProxy _allianceProxy;

        private Dictionary<string, GameObject> m_assetDic;
        private bool assetReady = false;

        private SDKTranslator m_translator;

        //联系人列表
        private List<ChatContact> m_contacts;
        //当前的联系人
        private ChatContact m_currentContact;
        //当前联系人的索引
        private int m_currentContactIndex;
        //当前对话信息列表
        private ChatMsgList m_currentChatMsgList;
        private EmojiMenuInfo m_currentEmojiMenu;
        private bool m_inChatLast = false;

        private Dictionary<int, List<EmojiListInfo>> m_emojiGroupInfos=new Dictionary<int, List<EmojiListInfo>>();
        private List<EmojiMenuInfo> m_emojiMenuInfo = new List<EmojiMenuInfo>();


        private const string m_UI_Item_ChatSelf = "UI_Item_ChatSelf";
        private const string m_UI_Item_ChatOther = "UI_Item_ChatOther";
        private const string m_UI_Item_ChatTime = "UI_Item_ChatTime";
        private const string m_UI_Item_Contact = "UI_Item_Contact";
        private const string m_UI_Item_ChatSelfEmoji = "UI_Item_ChatSelfEmoji";
        private const string m_UI_Item_ChatOtherEmoji = "UI_Item_ChatOtherEmoji";
        private const string UI_Item_ChatGMPlacard = "UI_Item_ChatGMPlacard";
        private const string m_UI_Item_ChatSystemNotice = "UI_Item_ChatMes";
        private const string m_UI_Item_ChatSelfShare = "UI_Item_ChatSelfShare";
        private const string m_UI_Item_ChatOtherShare = "UI_Item_ChatOtherShare";

        private Color m_contactNameColor;

        //正在获取聊天分页信息
        private EnumScrollRectDragingState m_chatListScrollRectLoading;


        private EnumChatChannel m_viewDataChannel = EnumChatChannel.world;
        private bool UseViewData;
        private ChatContact m_viewDataContact;
        #endregion

        //IMediatorPlug needs
        public UI_Win_ChatMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
            IsOpenUpdate = true;
        }


        public UI_Win_ChatView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.UpdateChatContact,
                CmdConstant.UpdateChatMsg,
                CmdConstant.OnShowUI,
                CmdConstant.DeleteChatContact,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.DeleteChatContact:
                    m_contacts = m_chatProxy.AllvalidContact;
                    if (m_contacts.Count > 0)
                    {
                        SetCurrentContact(m_contacts[0], true);
                    }
                //    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatContact);                    
                    break;
                case CmdConstant.UpdateChatContact:
                    {
                        RefreshContactList();
                    }
                    break;
                case CmdConstant.UpdateChatMsg:
                    {
                        if (!assetReady)
                        {
                            return;
                        }
                        m_chatProxy.HasNewMsg = false;
                        List<PushMsgInfo> msgs = notification.Body as List<PushMsgInfo>;
                        m_chatProxy.MoveDownAfterReciveMsg = CheckMoveToDown(msgs);
                        SetCurrentChatMsgList(m_chatProxy.MoveDownAfterReciveMsg);
                        m_chatProxy.MoveDownAfterReciveMsg = false;
                        RefreshContactList();
                    }
                    break;
                case CmdConstant.OnShowUI:
                {
                    CheckChatClose(notification.Body as UIInfo);
                }
                    break;
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {
            SendCurContactReadRecord();
        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {
            if (!isEmojiMenuPanelHide && view.m_ipt_chat_GameInput.isFocused)
            {
                OnHideEmojiMenu();
            }
            
            if (chatTipView!=null && chatTipView.gameObject.activeSelf && Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> result = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, result);
                if (result.Count > 0)
                {
                    for (int i = result.Count - 1; i >= 0; i--)
                    {
                        if (result[i].gameObject.transform.IsChildOf(chatTipView.gameObject.transform))
                        {
                            return;
                        }
                    }
                    ClosePopMenu();
                }
            }
        }

        protected override void InitData()
        {
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            _allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_chatProxy.HasNewMsg = false;
            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatRedDot);
            m_translator = GameHelper.GetTranslator();

            m_contactNameColor = ClientUtils.StringToHtmlColor("#E2D19B");

            if (view.data is EnumChatChannel)
            {
                m_viewDataChannel = (EnumChatChannel)view.data;
                UseViewData = true;
            }
            else if(view.data is ChatContact)
            {
                m_viewDataContact = view.data as ChatContact;
            }
            var prefabsName = new List<string>()
            {

            };
            prefabsName.AddRange(view.m_sv_emoji_ListView.ItemPrefabDataList);
            prefabsName.AddRange(view.m_sv_chat_ListView.ItemPrefabDataList);
            prefabsName.AddRange(view.m_sv_emojimenu_ListView.ItemPrefabDataList);
            prefabsName.AddRange(view.m_sv_contact_ListView.ItemPrefabDataList); 
            prefabsName.AddRange(view.m_sv_playerhead_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,prefabsName, OnLoadAssetFinished);

            emojiOffset =new Vector2(0, view.m_pl_emoji.rect.height);

            m_contentLocalPos = view.m_pl_content.localPosition;
        }

        private void OnLoadAssetFinished(Dictionary<string,GameObject> assets)
        {

             m_assetDic = assets;
            InitView();
            assetReady = true;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);

            view.m_btn_chatshrink_GameButton.onClick.AddListener(OnChatShrink);

            view.m_btn_emoji_GameButton.onClick.AddListener(OnShowEmojiPanel);

            view.m_btn_emojimask_GameButton.onClick.AddListener(OnEmojiMask);

            view.m_btn_contactmenu_GameButton.onClick.AddListener(OnContactMenu);
            view.m_btn_contactmenumask_GameButton.onClick.AddListener(OnContactMenuMask);
            
            view.m_ipt_chat_GameInput.onValueChanged.AddListener(OnInputMsgChanged);
        }
        
        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_chat);
        }

        protected override void BindUIData()
        {
        }

        #endregion

        #region 初始化整个界面

        private void InitView()
        {
            m_contacts = m_chatProxy.AllvalidContact;
            m_contacts.Sort(m_chatProxy.SortContact);
            if(UseViewData)
            {
                m_currentContactIndex = m_contacts.FindIndex((i) => { return i.channelType==m_viewDataChannel; });
                SetCurrentContact(m_contacts[m_currentContactIndex],true);
            }
            else if (m_viewDataContact != null)
            {
                SetCurrentContact(m_viewDataContact,true);
            }
            else
            {
                SetCurrentContact(m_contacts[0],true);
            }

            m_chatProxy.CurrentChannelType = m_currentContact.channelType;
        }


        #endregion

        #region 动画

        //联系人列表是否隐藏
        bool isContactListPanelHide = false;
        bool isEmojiMenuPanelHide = true;
        private Vector3 m_contentLocalPos;
        private void OnPopContactList()
        {
            if (!isContactListPanelHide)
            {
                return;
            }
            isContactListPanelHide = false;
            view.m_pl_content.DOLocalMoveX(m_contentLocalPos.x, 0.2f).SetEase(Ease.InCubic);
        }

        private void OnHideContactList()
        {
            if(isContactListPanelHide)
            {
                return;
            }
            isContactListPanelHide = true;
            view.m_pl_content.DOLocalMoveX(m_contentLocalPos.x-view.m_pl_contact.sizeDelta.x, 0.2f).SetEase(Ease.InCubic);
        }

        Vector2 emojiOffset;
        private void OnHideEmojiMenu()
        {
            if(isEmojiMenuPanelHide)
            {
                return;
            }
            isEmojiMenuPanelHide = true;
            view.m_sv_chat_PolygonImage.rectTransform.sizeDelta += emojiOffset;
            view.m_img_chat_PolygonImage.rectTransform.sizeDelta += emojiOffset;
            view.m_pl_down.anchoredPosition -= emojiOffset;
            view.m_pl_emoji.anchoredPosition -= emojiOffset;

            view.m_btn_emojimask_GameButton.gameObject.SetActive(false);
        }

        private void OnPopEmojiMenu()
        {
            
            if (!isEmojiMenuPanelHide)
            {
                return;
            }
            isEmojiMenuPanelHide = false;
            view.m_sv_chat_PolygonImage.rectTransform.sizeDelta -= emojiOffset;
            view.m_img_chat_PolygonImage.rectTransform.sizeDelta -= emojiOffset;

            view.m_pl_down.anchoredPosition += emojiOffset;
            view.m_pl_emoji.anchoredPosition += emojiOffset;
            view.m_btn_emojimask_GameButton.gameObject.SetActive(true);
            RefreshEmoji();
        }

        private void OnChatShrink()
        {
            if(isContactListPanelHide)
            {
                OnPopContactList();
            }
            else
            {
                OnHideContactList();
            }
        }

        private void OnShowEmojiPanel()
        {
            if(isEmojiMenuPanelHide)
            {
                OnPopEmojiMenu();
            }
            else
            {
                OnHideEmojiMenu();
            }
        }

        private void OnEmojiMask()
        {
            OnHideEmojiMenu();
        }

        #endregion

        #region 联系人菜单

        private void OnContactMenu()
        {
            view.m_pl_contactmenu.gameObject.SetActive(true);
        }

        private void OnContactMenuMask()
        {
            view.m_pl_contactmenu.gameObject.SetActive(false);
        }
        #endregion

        #region 消息列表

        bool isContactViewInited;

        private void InitContactView()
        {
            if(isContactViewInited)
            {
                return;
            }
            isContactViewInited = true;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = InitContactListItem;
            funcTab.GetItemPrefabName = GetContactItemPrefabName;
            view.m_sv_contact_ListView.SetInitData(m_assetDic, funcTab);
        }

        private void RefreshContactList()
        {
            m_contacts = m_chatProxy.AllvalidContact;
            InitContactView();
            m_contacts.Sort(m_chatProxy.SortContact);
            view.m_sv_contact_ListView.FillContent(m_contacts.Count);
            m_currentContactIndex = m_contacts.FindIndex(x => x == m_currentContact);
            OnTotalUnreadNum();
        }

        private string GetContactItemPrefabName(ListView.ListItem item)
        {
            return m_UI_Item_Contact;
        }

        private void SetCurrentContact(ChatContact chatContact, bool moveToDown = false)
        {
            m_currentContactIndex = m_contacts.FindIndex(x => x == chatContact);
            m_currentContact = chatContact;
            m_currentContact.SetRead();
            SendCurContactReadRecord();
            RefreshContactList();
            SetCurrentChatMsgList(moveToDown);
            ResetSetting();
            AppFacade.GetInstance().SendNotification(CmdConstant.RefreshMainChatPoint);
        }
        
        //向服务器发送当前聊天页面的阅读记录
        private void SendCurContactReadRecord()
        {
            if (m_currentContact != null)
            {
                m_chatProxy.SendReadRecord(m_currentContact);
            }
        }


        private void InitContactListItem(ListView.ListItem item)
        {
            int index = item.index;
            ChatContact contact = m_contacts[index];

            UI_Item_Contact_SubView itemView = null;
            if (item.data == null)
            {
                itemView = new UI_Item_Contact_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            else
            {
                itemView = item.data as UI_Item_Contact_SubView;
            }
            itemView.SetInfo(GetOptionBarStatus(contact), (isShow) =>
            {
                SetOptionBarStatus(contact,isShow);
            });
            itemView.m_btn_top_GameButton.gameObject.SetActive(!contact.isTop);
            itemView.m_btn_untop_GameButton.gameObject.SetActive(contact.isTop);
            itemView.gameObject.SetActive(true);
            itemView.m_sv_contact_ListView.SetParent(view.m_sv_contact_ScrollRect);
            ChatChannelDefine define;

            switch (contact.channelType)
            {
                case EnumChatChannel.world:
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(750011);
                    itemView.m_btn_delete_GameButton.gameObject.SetActive(false);
                    define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)EnumChatChannel.world);
                    itemView.m_UI_Model_PlayerHead.LoadHeadImg(define.icon);
                    itemView.m_UI_Model_PlayerHead.HideFrameImg();
                    break;
                case EnumChatChannel.alliance:
                    itemView.m_lbl_name_LanguageText.text = _allianceProxy.GetAlliance()?.name;
                    itemView.m_btn_delete_GameButton.gameObject.SetActive(true);
                    define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)EnumChatChannel.alliance);
                    itemView.m_UI_Model_PlayerHead.LoadHeadImg(define.icon);
                    itemView.m_UI_Model_PlayerHead.HideFrameImg();
                    break;
                case EnumChatChannel.privatechat:
                    var contactInfo = m_chatProxy.GetContactInfo(contact.rid);
                    itemView.m_lbl_name_LanguageText.text = contactInfo.name;
                    itemView.m_btn_delete_GameButton.gameObject.SetActive(true);
                    //隐藏临时聊天
                    itemView.gameObject.SetActive(!contact.IsHide());
                    itemView.m_UI_Model_PlayerHead.ShowFrameImg();
                    itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(contactInfo.headID,contactInfo.headFrameID);
                    break;
                default:break;
            }

            if(contact.lastMsg!=null)
            {
                itemView.m_lbl_time_LanguageText.text = ConvetToTimeString(contact.lastMsg.timeStamp);
                string lbl_content = contact.lastMsg.msg;
                switch (contact.lastMsg.msgType)
                {
                    case EnumMsgType.ATUser:
                        m_chatProxy.ConvertStringToChatListviewMapMarkerType(contact.lastMsg, out lbl_content);
                        itemView.m_lbl_content_LanguageText.text = lbl_content;
                        break;
                    case EnumMsgType.ChatShare:
                        ChatShareDefine chatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(contact.lastMsg.chatShareID);
                        if (chatShareDefine != null)
                        {
                            lbl_content = LanguageUtils.getTextFormat(300278, LanguageUtils.getText(chatShareDefine.l_titleID));
                        }
                        UIHelper.SetTextWithEllipsis(itemView.m_lbl_content_LanguageText, lbl_content, "...");
                        break;
                    default:
                        UIHelper.SetTextWithEllipsis(itemView.m_lbl_content_LanguageText, lbl_content, "...");
                        break;
                }
  
            }
            else
            {
                itemView.m_lbl_time_LanguageText.text = string.Empty;
                itemView.m_lbl_content_LanguageText.text = string.Empty;
            }

            //红点
            if(contact.GetUnreadCount() > 0)
            {
                itemView.m_img_shrinkreddot_PolygonImage.gameObject.SetActive(true);
                itemView.m_lbl_shrinkreddot_LanguageText.text = ShowOverlayMsgCount(contact.GetUnreadCount());
            }
            else
            {
                itemView.m_img_shrinkreddot_PolygonImage.gameObject.SetActive(false);
            }

            if (contact == m_currentContact)
            {
                itemView.m_img_select_PolygonImage.gameObject.SetActive(true);
                itemView.m_lbl_name_LanguageText.color = Color.black;
                itemView.m_lbl_content_LanguageText.color = Color.black;
            }
            else
            {
                itemView.m_lbl_name_LanguageText.color = m_contactNameColor;
                itemView.m_lbl_content_LanguageText.color = Color.white;

                itemView.m_img_select_PolygonImage.gameObject.SetActive(false);
            }


            itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_click_GameButton.onClick.AddListener(()=>
            {
                if(contact!= m_currentContact)
                {
                    m_chatListScrollRectLoading = EnumScrollRectDragingState.None;
                }
                if(m_currentContactIndex!=index&& m_currentContact!=contact)
                {
                    SetCurrentContact(contact,true);

                }


            });

            itemView.m_btn_top_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_top_GameButton.onClick.AddListener(()=>
            {
                m_chatProxy.TopContact(contact);
                SetOptionBarStatus(contact, false);
                RefreshContactList();
            });
            itemView.m_btn_untop_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_untop_GameButton.onClick.AddListener(() =>
            {
                m_chatProxy.UnTopContact(contact);
                SetOptionBarStatus(contact, false);
                RefreshContactList();
            });

            itemView.m_btn_delete_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_delete_GameButton.onClick.AddListener(()=>
            {
                m_chatProxy.RemoveContact(contact);
                SetOptionBarStatus(contact, false);
                RefreshContactList();
            });
        }


        private void OnTotalUnreadNum()
        {
            int totalNum = 0;
            for(int i = 0;i<m_contacts.Count;i++)
            {
                totalNum += m_contacts[i].noDisturb ? 0 : m_contacts[i].GetUnreadCount();
            }

            view.m_img_shrinkreddot_PolygonImage.gameObject.SetActive(totalNum>0);
            view.m_lbl_shrinkreddot_LanguageText.text = ShowOverlayMsgCount(totalNum);
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
                m_optionBarStatusDic.Add(key,false);
                return false;
            }
        }

        private void SetOptionBarStatus(ChatContact contact,bool value)
        {
            var key = contact.channelType.ToString() + contact.rid.ToString();
            if (m_optionBarStatusDic.ContainsKey(key))
            {
                m_optionBarStatusDic[key] = value;
            }
            else
            {
                m_optionBarStatusDic.Add(key,value);
            }
        }
        #endregion

        #region 对话界面

        private void SetCurrentChatMsgList(bool moveToDown = false)
        {
            m_currentChatMsgList = m_currentContact.ChatMsgList;
            RefreshChatPage(moveToDown);
        }


        bool isChatPageInited;
        private void InitChatPage()
        {
            if(isChatPageInited)
            {
                return;
            }
            isChatPageInited = true;

            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = InitChatListItem;
            funcTab.ItemRemove = ChatListItemRemove;
            funcTab.GetItemPrefabName = OnGetChatItemPrefabNmae;
            view.m_sv_chat_ListView.SetInitData(m_assetDic, funcTab);

            chatListOffset = view.m_sv_chat_PolygonImage.rectTransform.rect.height* 1.2f;
            view.m_sv_chat_ListView.onDragEnd = OnChatListDragEnd;
            view.m_sv_chat_ScrollRect.onValueChanged.RemoveAllListeners();
            view.m_sv_chat_ScrollRect.onValueChanged.AddListener(OnChatScrollRectValueChanged);
            view.m_ipt_chat_GameInput.onValueChanged.AddListener(OnChatInputTextChange);
            view.m_btn_send_GameButton.onClick.AddListener(OnPressSendBtn);

            view.m_btn_currentchatunread_GameButton.onClick.RemoveAllListeners();
            view.m_btn_currentchatunread_GameButton.onClick.AddListener(() =>
            {
                m_currentContact.SetRead();
                view.m_sv_chat_ListView.ScrollPanelToItemIndex(0);
                OnSetUnreadPage();
            });
        }

        private void OnChatScrollRectValueChanged(Vector2 value)
        {
            if(value.y<0)
            {
                value.y = 0;
                view.m_sv_chat_ScrollRect.normalizedPosition = value;
            }
            view.m_sv_chat_ListView.ShowContentAt(view.m_sv_chat_ListView.GetContainerPos());
        }


        private float chatListOffset;

        private void OnChatListDragEnd(UnityEngine.EventSystems.PointerEventData data)
        {
            if (m_chatListScrollRectLoading == EnumScrollRectDragingState.None &&view.m_c_chat.rect.height+ view.m_c_chat.anchoredPosition.y < chatListOffset)
            {
                m_chatListScrollRectLoading = EnumScrollRectDragingState.Ondraging;
                OnNextPage();
            }
        }

        private long m_pageCurrentMsgUniqueIndex;
        private void OnNextPage()
        {
            if(!m_chatProxy.CheckContactMsgAllReceive(m_currentContact))
            {
                if(m_pageCurrentMsgUniqueIndex == 0&&m_currentContact.ChatMsgList.ChatMsg.Count>0)
                {
                    m_pageCurrentMsgUniqueIndex = m_currentContact.ChatMsgList.ChatMsg[0].uniqueIndex;
                }                
            };

            m_chatListScrollRectLoading = EnumScrollRectDragingState.None;
        }

        private void RefreshChatPage(bool moveToDown = false)
        {
            InitChatPage();
            OnContactSetting();
            
            //刷新私人频道时检查是否获取过聊天数据
            if (m_currentContact.channelType == EnumChatChannel.privatechat && !m_chatProxy.CheckContactMsgAllReceive(m_currentContact))
            {
                m_chatProxy.MoveDownAfterReciveMsg = true;
                OnSetUnreadPage();
                return;
            }
            view.m_sv_chat_ListView.FillContent(m_currentChatMsgList.ChatMsgWithTime.Count);
            if (moveToDown)
            {
                m_currentContact.SetRead();
                view.m_sv_chat_ScrollRect.velocity = Vector2.zero;
                view.m_sv_chat_ListView.MovePanelToItemIndex(0);
            }
            else if (m_pageCurrentMsgUniqueIndex != 0)
            {
                int itemIndex = m_currentContact.ChatMsgList.ChatMsgWithTime.FindIndex(i => i.msg != null && i.uniqueIndex == m_pageCurrentMsgUniqueIndex);
                m_pageCurrentMsgUniqueIndex = 0;
                if (itemIndex > 0)
                {
                    view.m_sv_chat_ScrollRect.velocity = Vector2.zero;
                    view.m_sv_chat_ListView.SetContainerPos(-view.m_sv_chat_ListView.GetItemByIndex(itemIndex - 1).startPos);
                }
            }

            if (m_currentContact.channelType == EnumChatChannel.privatechat)
            {
                view.m_lbl_name_LanguageText.gameObject.SetActive(true);
                var contactInfo = m_chatProxy.GetContactInfo(m_currentContact.rid);
                view.m_lbl_name_LanguageText.text = contactInfo.name;
            }
            else
            {
                view.m_lbl_name_LanguageText.gameObject.SetActive(true);
                if (m_currentContact.channelType == EnumChatChannel.world)
                {
                    view.m_lbl_name_LanguageText.text = LanguageUtils.getText(750011);
                }
                else if (m_currentContact.channelType == EnumChatChannel.alliance)
                {
                    view.m_lbl_name_LanguageText.text = _allianceProxy.GetAlliance()?.name;
                }
                else
                {
                    view.m_lbl_name_LanguageText.text = "";
                }
            }
            
            //更新未读信息按钮
            OnSetUnreadPage();
        }

        float chatBubbleSize = 460f;
        float chatBubbleSpaceWidth = 43f;
        float chatBubbleSpaceHeight = 36;
        float originChatItemNameHeight = 30f;
        float chatTranslateSpace = 20f;

        private string OnGetChatItemPrefabNmae(ListView.ListItem item)
        {
            int index = item.index;
            ChatMsg msg = m_currentChatMsgList.ChatMsgWithTime[index];
            if(msg.msgType== EnumMsgType.Time)
            {
                return m_UI_Item_ChatTime;
            }
            else if(msg.msgType == EnumMsgType.Text)
            {
                if(msg.rid==_playerProxy.CurrentRoleInfo.rid){
                    return m_UI_Item_ChatSelf;
                }
                else
                {
                    return m_UI_Item_ChatOther;
                }
            }
            else if (msg.msgType == EnumMsgType.Announcement)
            {
                return UI_Item_ChatGMPlacard;
            }else if (msg.msgType == EnumMsgType.Notice)
            {
                return m_UI_Item_ChatSystemNotice;
            }
            else if (msg.msgType == EnumMsgType.Emoji)
            {
                if(msg.rid==_playerProxy.CurrentRoleInfo.rid){
                    return m_UI_Item_ChatSelfEmoji;
                }
                else
                {
                    return m_UI_Item_ChatOtherEmoji;
                }
            }
            else if (msg.msgType == EnumMsgType.ChatShare)
            {
                if (msg.rid == _playerProxy.CurrentRoleInfo.rid)
                {
                    return m_UI_Item_ChatSelfShare;
                }
                else
                {
                    return m_UI_Item_ChatOtherShare;
                }
            }
            else if (msg.msgType == EnumMsgType.ATUser)
            {
                if (msg.rid == _playerProxy.CurrentRoleInfo.rid)
                {
                    return m_UI_Item_ChatSelf;
                }
                else
                {
                    return m_UI_Item_ChatOther;
                }
            }

            return m_UI_Item_ChatOther;
        }
        
        private void InitChatListItem(ListView.ListItem item)
        {
            int index = item.index;
            ChatMsg msg = m_currentChatMsgList.ChatMsgWithTime[index];
            if (!m_inChatLast)
            {
                m_inChatLast = index == 0;
            }
            if (m_currentContact.SetRead(msg))
            {
                //更新未读信息按钮
                OnSetUnreadPage();
            }
            switch (item.prefabName)
            {
                case m_UI_Item_ChatTime:
                    {
                        UI_Item_ChatTimeView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ChatTimeView>(item.go);
                        itemView.m_lbl_time_LanguageText.text = ConvetToTimeString(msg.timeStamp);
                        float size = itemView.m_pl_time.rect.height;
                        view.m_sv_chat_ListView.UpdateItemSize(index,size);
                    }
                    break;
                case m_UI_Item_ChatSelf:
                    {
                         SetItemChatSelfData(item, msg, index);
                    }
                    break;
                case m_UI_Item_ChatOther:
                    {
                         SetItemChatOtherData(item, msg, index);
                    }
                    break;
                case m_UI_Item_ChatSelfEmoji:
                    {
                        SetItemChatSelfEmoji(item, msg, index);
                    }
                    break;
                case m_UI_Item_ChatOtherEmoji:
                    {
                        SetItemChatOtherEmoji(item, msg, index);
                    }
                    break;
                case UI_Item_ChatGMPlacard:
                    {
                        SetItemSystemMsg(item, msg, index);
                    }
                    break;
                case m_UI_Item_ChatSystemNotice:
                    {
                        SetItemSystemNoticeMsg(item,msg,index);
                    }
                    break;
                case m_UI_Item_ChatSelfShare:
                    {
                        SetItemChatSelfShare(item, msg, index);
                    }
                    break;
                case m_UI_Item_ChatOtherShare:
                    {
                        SetItemChatOtherShare(item, msg, index);
                    }
                    break;
                default:break;
            }
        }

        private void ChatListItemRemove(ListView.ListItem item)
        {
            if (m_inChatLast && item.index == 0)
            {
                m_inChatLast = false;
            }
        }
        private void OnSetUnreadPage()
        {
            int unreadCount = m_currentContact.GetUnreadCount();
            if(unreadCount>0)
            {
                if (!view.m_btn_currentchatunread_GameButton.gameObject.activeSelf)
                {
                    view.m_btn_currentchatunread_GameButton.gameObject.SetActive(true);
                }
                view.m_lbl_currentchatunread_LanguageText.text = LanguageUtils.getTextFormat(750006, ShowOverlayMsgCount(unreadCount));
            }
            else if(view.m_btn_currentchatunread_GameButton.gameObject.activeSelf)
            {
                view.m_btn_currentchatunread_GameButton.gameObject.SetActive(false);
                view.m_sv_contact_ListView.ForceRefresh();
                OnTotalUnreadNum();
            }
        }

        private void SetItemChatSelfData(ListView.ListItem item,ChatMsg msg, int index)
        {
            UI_Item_ChatSelf_SubView  itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatSelf_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatSelf_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            itemView.SetName(msg.contactInfo.guildName, msg.contactInfo.name);
            if (msg.msgType == EnumMsgType.ATUser)
            {
                string lbl_content = "";
                m_chatProxy.ConvertStringToChatviewMapMarkerType(msg, out lbl_content);
                itemView.m_lbl_content_LanguageText.text = lbl_content;
            }
            else
            {
                itemView.m_lbl_content_LanguageText.text = msg.msg;
            }
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(msg.contactInfo.headID,msg.contactInfo.headFrameID);
            var preferredWidth = itemView.m_lbl_content_LanguageText.preferredWidth;
            itemView.m_lbl_content_LanguageText.rectTransform.sizeDelta = Vector2.zero;
            itemView.m_lbl_content_ContentSizeFitter.SetLayoutVertical();
            //自适应布局和大小
            if(preferredWidth< chatBubbleSize)
            {
                UIHelper.AlignRightWhenIsArabic(itemView.m_lbl_content_LanguageText,preferredWidth);
                itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(preferredWidth+ chatBubbleSpaceWidth, itemView.m_lbl_content_LanguageText.preferredHeight+ chatBubbleSpaceHeight);
            }
            else
            {
                UIHelper.AlignRightWhenIsArabic(itemView.m_lbl_content_LanguageText,chatBubbleSize);
                itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(chatBubbleSize + chatBubbleSpaceWidth, itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight);
            }
            float itemSize = itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta.y + 10;
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
            itemView.m_root_RectTransform.sizeDelta = new Vector2(itemView.m_root_RectTransform.sizeDelta.x,itemSize);
        }

        private void SetItemChatOtherData(ListView.ListItem item, ChatMsg msg , int index)
        {
            UI_Item_ChatOther_SubView  itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatOther_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatOther_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            if (!string.IsNullOrEmpty(msg.contactInfo.guildName))
            {
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, msg.contactInfo.guildName, msg.contactInfo.name);
            }
            else
            {
                itemView.m_lbl_name_LanguageText.text =  msg.contactInfo.name;
            }

            if (msg.msgType == EnumMsgType.ATUser)
            {
                string lbl_content = "";
                m_chatProxy.ConvertStringToChatviewMapMarkerType(msg, out lbl_content);
                itemView.m_lbl_content_LanguageText.text = lbl_content;
            }
            else
            {
                itemView.m_lbl_content_LanguageText.text = msg.msg;
            }
            float preferredWidth = itemView.m_lbl_content_LanguageText.preferredWidth;
            if (preferredWidth < chatBubbleSize)
            {
                UIHelper.AlignRightWhenIsArabic(itemView.m_lbl_content_LanguageText,preferredWidth);
            }
            else
            {
                UIHelper.AlignRightWhenIsArabic(itemView.m_lbl_content_LanguageText,chatBubbleSize);
            }
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(msg.contactInfo.headID,msg.contactInfo.headFrameID);
            itemView.m_btn_head_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_head_GameButton.onClick.AddListener(() =>
            {
                OnOpenPrivateChat(msg.rid);
            });
            ChatTranslateState translateState = msg.GetTranslateState();
            itemView.m_btn_chatFunction_LongClickButton.action = null;
            itemView.m_btn_chatFunction_LongClickButton.action+=() =>
            {
                ShowPopMenu(msg,itemView.m_btn_chatFunction_PolygonImage.rectTransform);
            };

            float itemSize = 0f;
            switch (translateState)
            {
                case ChatTranslateState.NoTranslation:
                    {
                        itemView.m_UI_Common_Spin.gameObject.SetActive(false);
                        itemView.m_img_line_PolygonImage.gameObject.SetActive(false);
                        itemView.m_lbl_tanslatetext_LanguageText.gameObject.SetActive(false);
                        itemView.m_img_alreadyTrans_PolygonImage.gameObject.SetActive(false);
                        itemView.m_btn_tanslate_GameButton.gameObject.SetActive(true);
                        itemView.m_btn_tanslate_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_tanslate_GameButton.onClick.AddListener(() =>
                        {
                            msg.Translate = string.Empty;
                            switch (msg.msgType)
                            {
                                case EnumMsgType.ATUser:
                                    m_translator.translateText(new SDKTranslationSource(msg.mapMarkerTypemsg), (value1) => {
                                        msg.Translate = value1.getByIndex(0).getText();
                                        view.m_sv_chat_ListView.ForceRefresh();
                                    }, (value2, value3) => {
                                        msg.Translate = msg.mapMarkerTypemsg;
                                        if (view.gameObject != null)
                                        {
                                            view.m_sv_chat_ListView.ForceRefresh();
                                        }
                                        Debug.LogWarning("翻译失败");
                                    });
                                    break;
                                case EnumMsgType.Text:
                                    m_translator.translateText(new SDKTranslationSource(msg.msg), (value1) => {
                                        msg.Translate = value1.getByIndex(0).getText();
                                        view.m_sv_chat_ListView.ForceRefresh();
                                    }, (value2, value3) => {
                                        msg.Translate = msg.msg;
                                        if (view.gameObject != null)
                                        {
                                            view.m_sv_chat_ListView.ForceRefresh();
                                        }
                                        Debug.LogWarning("翻译失败");
                                    });
                                    break;
                            }
                     
                            view.m_sv_chat_ListView.ForceRefresh();
                        });
                        itemView.m_lbl_content_ContentSizeFitter.SetLayoutVertical();
                        if (preferredWidth < chatBubbleSize)
                        {
                            itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(preferredWidth + chatBubbleSpaceWidth, itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight);
                        }
                        else
                        {
                            itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(chatBubbleSize + chatBubbleSpaceWidth, itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight);
                        }
                        itemSize = originChatItemNameHeight + itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta.y;
                    }
                    break;
                case ChatTranslateState.Translating:
                    {
                        itemView.m_UI_Common_Spin.gameObject.SetActive(true);
                        itemView.m_img_line_PolygonImage.gameObject.SetActive(false);
                        itemView.m_lbl_tanslatetext_LanguageText.gameObject.SetActive(false);
                        itemView.m_img_alreadyTrans_PolygonImage.gameObject.SetActive(true);
                        itemView.m_lbl_alreadyTrans_LanguageText.text = LanguageUtils.getText(750035);
                        itemView.m_btn_tanslate_GameButton.gameObject.SetActive(false);
                        itemView.m_lbl_content_ContentSizeFitter.SetLayoutVertical();
                        if (preferredWidth < chatBubbleSize)
                        {
                            itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(preferredWidth + chatBubbleSpaceWidth, itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight);
                        }
                        else
                        {
                            itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(chatBubbleSize + chatBubbleSpaceWidth, itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight);
                        }
                        itemSize = originChatItemNameHeight + itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta.y + itemView.m_img_alreadyTrans_PolygonImage.rectTransform.sizeDelta.y;
                    }
                    break;
                case ChatTranslateState.Translated:
                    {
                        itemView.m_UI_Common_Spin.gameObject.SetActive(false);
                        itemView.m_img_line_PolygonImage.gameObject.SetActive(true);
                        itemView.m_lbl_tanslatetext_LanguageText.gameObject.SetActive(true);
                        itemView.m_img_alreadyTrans_PolygonImage.gameObject.SetActive(true);
                        itemView.m_lbl_alreadyTrans_LanguageText.text = LanguageUtils.getText(750008);
                        itemView.m_btn_tanslate_GameButton.gameObject.SetActive(false);

                        if (msg.msgType == EnumMsgType.ATUser)
                        {
                            string lbl_content = "";
                            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>(msg.mapMarkerTypeID);
                            if (mapMarkerTypeDefine!=null)
                            {
                                if (string.IsNullOrEmpty(msg.Translate))
                                {
                                    lbl_content = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, "  ");
                                }
                                else
                                {
                                    lbl_content = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, msg.Translate);
                                }
                            }
                            itemView.m_lbl_tanslatetext_LanguageText.text = lbl_content;
                        }
                        else
                        {
                            itemView.m_lbl_tanslatetext_LanguageText.text = msg.Translate;
                        }
                        float preferredWidth1 = itemView.m_lbl_content_LanguageText.preferredWidth;
                        float preferredWidth2 = itemView.m_lbl_tanslatetext_LanguageText.preferredWidth;
                        preferredWidth = preferredWidth1 > preferredWidth2 ? preferredWidth1 : preferredWidth2;
                        var lineSize = itemView.m_img_line_PolygonImage.rectTransform.sizeDelta;
                        itemView.m_lbl_tanslatetext_LanguageText.rectTransform.sizeDelta = Vector2.zero;
                        itemView.m_lbl_tanslatetext_ContentSizeFitter.SetLayoutVertical();
                        itemView.m_lbl_content_ContentSizeFitter.SetLayoutVertical();
                        itemView.m_pl_view_VerticalLayoutGroup.SetLayoutVertical();
                        if (preferredWidth < chatBubbleSize)
                        {
                            UIHelper.AlignRightWhenIsArabic(itemView.m_lbl_tanslatetext_LanguageText,preferredWidth);
                            itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(preferredWidth + chatBubbleSpaceWidth,chatTranslateSpace + lineSize.y + itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight+itemView.m_lbl_tanslatetext_LanguageText.preferredHeight);
                        }
                        else
                        {
                            UIHelper.AlignRightWhenIsArabic(itemView.m_lbl_tanslatetext_LanguageText,chatBubbleSize);
                            itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta = new Vector2(chatBubbleSize + chatBubbleSpaceWidth,chatTranslateSpace + lineSize.y + itemView.m_lbl_content_LanguageText.preferredHeight + chatBubbleSpaceHeight + itemView.m_lbl_tanslatetext_LanguageText.preferredHeight);
                        }
                        itemView.m_img_line_PolygonImage.rectTransform.sizeDelta = new Vector2(itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta.x-chatBubbleSpaceWidth, lineSize.y);
                        itemSize = originChatItemNameHeight + itemView.m_img_chatbg_PolygonImage.rectTransform.sizeDelta.y + itemView.m_img_alreadyTrans_PolygonImage.rectTransform.sizeDelta.y
                            + itemView.m_lbl_alreadyTrans_LanguageText.preferredHeight;
                    }
                    break;
                default:break;
            }
            ClientUtils.UIReLayout(itemView.m_pl_view_VerticalLayoutGroup.gameObject);
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
            itemView.m_root_RectTransform.sizeDelta = new Vector2(itemView.m_root_RectTransform.sizeDelta.x,itemSize);
        }

        private void SetItemChatSelfEmoji(ListView.ListItem item, ChatMsg msg, int index)
        {
            UI_Item_ChatSelfEmoji_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatSelfEmoji_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatSelfEmoji_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            if (!string.IsNullOrEmpty(msg.contactInfo.guildName))
            {
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, msg.contactInfo.guildName, msg.contactInfo.name);
            }
            else
            {
                itemView.m_lbl_name_LanguageText.text =  msg.contactInfo.name;
            }
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(msg.contactInfo.headID, msg.contactInfo.headFrameID);
            var emojiCfg = CoreUtils.dataService.QueryRecord<ChatEmojiDefine>(msg.emojiID);
            ClientUtils.LoadSpine(itemView.m_spine_emoji_SkeletonGraphic,emojiCfg.spine);
            
            float itemSize = itemView.m_root_RectTransform.rect.height;
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
        }

        private void SetItemChatOtherEmoji(ListView.ListItem item, ChatMsg msg, int index)
        {
            UI_Item_ChatOtherEmoji_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatOtherEmoji_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatOtherEmoji_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            if (!string.IsNullOrEmpty(msg.contactInfo.guildName))
            {
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, msg.contactInfo.guildName, msg.contactInfo.name);
            }
            else
            {
                itemView.m_lbl_name_LanguageText.text =  msg.contactInfo.name;
            }
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(msg.contactInfo.headID, msg.contactInfo.headFrameID);
            itemView.m_btn_head_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_head_GameButton.onClick.AddListener(() =>
            {
                OnOpenPrivateChat(msg.rid);
            });
            var emojiCfg = CoreUtils.dataService.QueryRecord<ChatEmojiDefine>(msg.emojiID);
            ClientUtils.LoadSpine(itemView.m_spine_emoji_SkeletonGraphic,emojiCfg.spine);
            
            float itemSize = itemView.m_root_RectTransform.rect.height;
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
        }

        private void SetItemSystemMsg(ListView.ListItem item, ChatMsg msg, int index)
        {
            UI_Item_ChatGMPlacard_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatGMPlacard_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatGMPlacard_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            itemView.SetInfo(msg);
            
            float itemSize = itemView.GetHeight();
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
        }

        private void SetItemSystemNoticeMsg(ListView.ListItem item, ChatMsg msg, int index)
        {
            UI_Item_ChatMes_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatMes_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatMes_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            itemView.m_lbl_text_LanguageText.text = msg.msg;
            float itemSize = itemView.m_root_RectTransform.rect.height;
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
        }
        private void SetItemChatSelfShare(ListView.ListItem item, ChatMsg msg, int index)
        {
            string icon = "";
            string lbl_title = "";
            string lbl_dec = "";
            string color = "";//分享坐标默认颜色
            string coordinate = "";
            UI_Item_ChatSelfShare_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatSelfShare_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatSelfShare_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            if (!string.IsNullOrEmpty(msg.contactInfo.guildName))
            {
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, msg.contactInfo.guildName, msg.contactInfo.name);
            }
            else
            {
                itemView.m_lbl_name_LanguageText.text =  msg.contactInfo.name;
            }
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(msg.contactInfo.headID, msg.contactInfo.headFrameID);
            if (m_chatProxy.ConvertStringToChatview(msg,out lbl_dec,out lbl_title,out icon, out color, out coordinate))
            {
                itemView.m_UI_Item_ChatShare.SetDec(lbl_dec);
                itemView.m_UI_Item_ChatShare.SetTitle(lbl_title);
                itemView.m_UI_Item_ChatShare.SetColor(color);
                itemView.m_UI_Item_ChatShare.SetIcon(icon);
            }
            float itemSize = itemView.m_root_RectTransform.rect.height;
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
            itemView.m_UI_Item_ChatShare.AddEvent(() => {
                AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, coordinate);
                CoreUtils.uiManager.CloseUI(UI.s_chat);
            });
        }
        private void SetItemChatOtherShare(ListView.ListItem item, ChatMsg msg, int index)
        {
            string icon = "";
            string lbl_title = "";
            string lbl_dec = "";
            string color = "";//分享坐标默认颜色
            string coordinate = "";
            UI_Item_ChatOtherShare_SubView  itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_ChatOtherShare_SubView;
            }
            else
            {
                itemView = new UI_Item_ChatOtherShare_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            if (!string.IsNullOrEmpty(msg.contactInfo.guildName))
            {
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, msg.contactInfo.guildName, msg.contactInfo.name);
            }
            else
            {
                itemView.m_lbl_name_LanguageText.text =  msg.contactInfo.name;
            }
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(msg.contactInfo.headID, msg.contactInfo.headFrameID);
            itemView.AddEvent(() =>
            {
                OnOpenPrivateChat(msg.rid);
            });
            if (m_chatProxy.ConvertStringToChatview(msg, out lbl_dec, out lbl_title, out icon, out color, out coordinate))
            {
                itemView.m_UI_Item_ChatShare.SetDec(lbl_dec);
                itemView.m_UI_Item_ChatShare.SetTitle(lbl_title);
                itemView.m_UI_Item_ChatShare.SetColor(color);
                itemView.m_UI_Item_ChatShare.SetIcon(icon);
            }
            float itemSize = itemView.m_root_RectTransform.rect.height;
            view.m_sv_chat_ListView.UpdateItemSize(index, itemSize);
            itemView.m_UI_Item_ChatShare.AddEvent(() => {
                AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, coordinate);
                CoreUtils.uiManager.CloseUI(UI.s_chat);
            });
        }

        private void OnOpenPrivateChat(long rid)
        {
            var contact = m_chatProxy.GetOrCreatePrivateContact(rid);
            SetCurrentContact(contact,true);
        }
        
        private void OnChatInputTextChange(string text)
        {

        }
        private void OnPressSendBtn()
        {
            m_chatProxy.SendMsg(view.m_ipt_chat_GameInput.text, m_currentContact,EnumMsgType.Text ,(lbl)=> {
                view.m_ipt_chat_GameInput.text = lbl;
            });
        }
        #region 待移动到通用的代码
/*

        private void SendMsg( string msg)
        {
            if (string.IsNullOrEmpty(msg) || msg.Trim() == "")
            {
                return;
            }

            //禁言判断
            //_playerProxy.CurrentRoleInfo.silence = ServerTimeModule.Instance.GetServerTime() + 36222;
            if (_playerProxy.CurrentRoleInfo.silence == -1) //永久禁言
            {
                Alert.CreateAlert(100302, LanguageUtils.getText(730184)).SetRightButton(null, LanguageUtils.getText(730060)).Show();
                return;
            }
            else
            {
                //限时禁言
                if (_playerProxy.CurrentRoleInfo.silence > 0 && _playerProxy.CurrentRoleInfo.silence > ServerTimeModule.Instance.GetServerTime())
                {
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    DateTime TranslateDate = startTime.AddSeconds(_playerProxy.CurrentRoleInfo.silence);
                    string str = LanguageUtils.getTextFormat(100301, TranslateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    Alert.CreateAlert(str, LanguageUtils.getText(730184)).SetRightButton(null, LanguageUtils.getText(730060)).Show();
                    return;
                }
            }

            EnumChatChannel channel = m_currentContact.channelType;
            ChatChannelDefine define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)m_currentContact.channelType);

            //间隔限制
            if (_chatProxy.IsChatChannelInterval(channel, define.timeInterval, true))
            {
                return;
            }
            
            //等级限制
            if (_playerProxy.CurrentRoleInfo.level < define.lvLimit)
            {
                if(m_currentContact.channelType==EnumChatChannel.privatechat)
                {
                    _chatProxy.SetChatChannelInterval(channel);
                    // Tip.CreateTip(LanguageUtils.getTextFormat(750002, define.lvLimit)).SetStyle(Tip.TipStyle.Middle).Show();
                    _chatProxy.MoveDownAfterReciveMsg = true;
                    var time = ServerTimeModule.Instance.GetServerTime();
                    _chatProxy.SaveLocalErrorMsg(m_currentContact,LanguageUtils.getTextFormat(750002, define.lvLimit),msg,time);
                    view.m_ipt_chat_GameInput.text = string.Empty;
                }
                else
                {
                    Tip.CreateTip(LanguageUtils.getTextFormat(750001, define.lvLimit)).SetStyle(Tip.TipStyle.Middle).Show();
                }
                return;
            }
            
            int myMsgCount = 0;
            myMsgCount =  MyMsgCount( msg);
            if (myMsgCount >= 3)
            {
                Tip.CreateTip(750004).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }

            OnSendMsgSproto(msg,m_currentContact);
        }
        /// <summary>
        /// 类型为emoji的传emojiid
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private int MyMsgCount(EnumMsgType msgType, string msg)
        {
            int count = 0;
            for (int i = m_currentChatMsgList.ChatMsg.Count - 1; i >= 0; i--)
            {
                if (count >= 3)
                {
                    break;
                }
                if (m_currentChatMsgList.ChatMsg[i].rid == _playerProxy.Rid&& m_currentChatMsgList.ChatMsg[i].msgType == msgType)
                {
                        if (m_currentChatMsgList.ChatMsg[i].msg != null && string.Equals(msg, m_currentChatMsgList.ChatMsg[i].msg))
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                }
            }
            return count;
        }


        private void OnSendMsgSproto(string content,ChatContact chatContact)
        {
            //字符限制
            if(content.Length>channelWordLimit)
            {
                Tip.CreateTip(750005).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            
            view.m_ipt_chat_GameInput.text = string.Empty;
            Client.Utils.BannedWord.CheckChatBannedWord(content);
            bool personal = false;
            if(chatContact.rid>0)
            {
                personal = true;
            }
            _chatProxy.MoveDownAfterReciveMsg = true;
            if(personal)
            {
                Chat_SendPrivateMsg.request msg = new Chat_SendPrivateMsg.request();
                msg.toRid = chatContact.rid;
                msg.msgContent = content;
                msg.gameNode = _playerProxy.GetRoleLoginRes().chatServerName;
                AppFacade.GetInstance().SendSproto(msg);
            }
            else
            {
                Chat_SendMsg.request msg = new Chat_SendMsg.request();
                msg.channelType = (long)chatContact.channelType;
                msg.msgContent = content;
                _chatProxy.SendSproto(msg);
                if (chatContact.channelType == EnumChatChannel.world)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.WorldChat));
                }
            }

            EnumChatChannel channel = m_currentContact.channelType;
            _chatProxy.SetChatChannelInterval(channel);
        }
*/
        #endregion
        private bool CheckMoveToDown(List<PushMsgInfo> msgs)
        {
            if (m_chatProxy.MoveDownAfterReciveMsg)
            {
                return true;
            }

            if (m_inChatLast && msgs != null && msgs.Count == 1 )
            {
                var msg = msgs[0];
                if (msg.channelType == (long) m_currentContact.channelType)
                {
                    if (m_currentContact.channelType != EnumChatChannel.privatechat)
                    {
                        return true;
                    }
                    else if(msg.rid == m_currentContact.rid || msg.toRid == m_currentContact.rid)
                    {
                        return true;
                    }
                }
            }
            return false;
            
        }
        
        private UI_Pop_ChatOtherView chatTipView;
        private void ShowPopMenu(ChatMsg chatMsg, RectTransform transform)
        {
            if (chatTipView != null)
            {
                chatTipView.gameObject.SetActive(true);
                SetPopMenuInfo(chatMsg,transform);
                return;
            }
            CoreUtils.assetService.Instantiate("UI_Pop_ChatOther", (obj) =>
            {
                chatTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_ChatOtherView>(obj);
                obj.transform.SetParent(view.gameObject.transform);
                obj.transform.localScale= Vector3.one;
                SetPopMenuInfo(chatMsg,transform);
            });
        }

        private void ClosePopMenu()
        {
            chatTipView.gameObject.SetActive(false);
        }

        private void SetPopMenuInfo(ChatMsg chatMsg, RectTransform transform)
        {
            chatTipView.m_btn_report_GameButton.gameObject.SetActive(chatMsg.rid != _playerProxy.Rid);
            ClientUtils.UIReLayout(chatTipView.m_img_bg_ContentSizeFitter.gameObject);
            chatTipView.m_img_bg_PolygonImage.transform.position = transform.position;
            chatTipView.m_img_bg_PolygonImage.rectTransform.anchoredPosition += new Vector2(0,transform.rect.height/2);
            chatTipView.m_btn_copy_GameButton.onClick.RemoveAllListeners();
            chatTipView.m_btn_copy_GameButton.onClick.AddListener(() =>
            {
                TextEditor te = new TextEditor();
                te.content = new GUIContent(chatMsg.msg);
                te.SelectAll();
                te.Copy();                
                Tip.CreateTip(750036).Show();
                ClosePopMenu();
            });
            chatTipView.m_btn_report_GameButton.onClick.RemoveAllListeners();
            chatTipView.m_btn_report_GameButton.onClick.AddListener(() =>
            {
                UIHelper.Reporting(chatMsg.rid,chatMsg.contactInfo.name,chatMsg.msg,LanguageUtils.getText(300142));
                ClosePopMenu();
            });
        }
        
        #endregion

        #region 聊天表情

        private bool m_emojiIsInited = false;
        private void InitEmojiInfo()
        {
            if (m_emojiIsInited)
            {
                return;
            }
            m_emojiIsInited = true;

            InitEmojiGroupInfo();
            //
            InitEmojiMenuInfo();
            
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = InitEmojiListItem;
            view.m_sv_emoji_ListView.SetInitData(m_assetDic, funcTab);
            
            ListView.FuncTab emojiMenuFuncTab = new ListView.FuncTab();
            emojiMenuFuncTab.ItemEnter = InitEmojiMenuItem;
            view.m_sv_emojimenu_ListView.SetInitData(m_assetDic,emojiMenuFuncTab);
        }

        private void InitEmojiGroupInfo()
        {
            var groupInfos = m_chatProxy.GetEmojiGroup();
            m_emojiGroupInfos.Clear();
            foreach (var groupInfo in groupInfos)
            {
                m_emojiGroupInfos.Add(groupInfo.Key,new List<EmojiListInfo>());
                for (int i = 0; i < groupInfo.Value.Count; i += m_emojiCountPerList)
                {
                    var info = new EmojiListInfo();
                    m_emojiGroupInfos[groupInfo.Key].Add(info);
                    for (int j = i; j < i + m_emojiCountPerList && j < groupInfo.Value.Count; j++)
                    {
                        info.Infos.Add(groupInfo.Value[j]);
                    }
                }
            }
        }

        private void InitEmojiMenuInfo()
        {
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            var info = typeof(ConfigDefine);
            var pattern = @"(?<=chatEmojiGroup)\d{1,}";
            foreach (var field in info.GetFields())
            {
                var match = Regex.Match(field.Name, pattern);
                if (match.Success)
                {
                    var key = field.GetValue(config);
                    if (key == null || !(key is string))
                    {
                        Debug.LogWarning($"s_config配置错误!====key:{field.Name}");
                        continue;
                    }

                    var groupID = Int32.Parse(match.Value);
                    if (config.chatEmojiGroupNum < groupID)
                    {
                        continue;
                    }
                    if (!m_emojiGroupInfos.ContainsKey(groupID))
                    {
                        Debug.LogWarning($"s_config配置错误!====key:{field.Name},s_chatEmoji不存在groupID：{groupID}");
                        continue;
                    }
                    m_emojiMenuInfo.Add(new EmojiMenuInfo(){ Group = groupID,EmojiKey = key as string});
                }
            }

            if (m_emojiMenuInfo.Count == 0)
            {
                Debug.LogWarning($"s_config配置错误!====聊天表情未配置！！！");
                return;
            }
            m_currentEmojiMenu = m_emojiMenuInfo[0];
        }

        private void RefreshEmoji()
        {
            InitEmojiInfo();
            SetCurrentEmojiGroup(m_currentEmojiMenu);
        }

        private void SetCurrentEmojiGroup(EmojiMenuInfo info)
        {
            if (m_assetDic.Count == 0)
            {
                return;
            }
            m_currentEmojiMenu = info;
            if (m_emojiGroupInfos.ContainsKey(info.Group))
            {
                view.m_sv_emoji_ListView.FillContent(m_emojiGroupInfos[info.Group].Count);
                view.m_sv_emoji_ListView.MovePanelToItemIndex(0);
            }
            else
            {
                view.m_sv_emoji_ListView.FillContent(0);
            }
            view.m_sv_emojimenu_ListView.FillContent(m_emojiMenuInfo.Count);
        }
        

        private int m_emojiCountPerList = 4;
        private void InitEmojiListItem(ListView.ListItem item)
        {
            UI_Item_ChatEmojiList_SubView itemView = null; 
            if (item.data == null)
            {
                itemView = new UI_Item_ChatEmojiList_SubView(item.go.GetComponent<RectTransform>());
            }
            else
            {
                itemView = item.data as UI_Item_ChatEmojiList_SubView;
            }
            itemView.SetInfo(m_emojiGroupInfos[m_currentEmojiMenu.Group][item.index].Infos,OnClickEmojiItem,OnLongClickEmojiItem,OnReleaseLongClick);
        }

        private void InitEmojiMenuItem(ListView.ListItem item)
        {
            UI_Item_ChatEmojiMenu_SubView itemView = null;
            if (item.data == null)
            {
                itemView = new UI_Item_ChatEmojiMenu_SubView(item.go.GetComponent<RectTransform>());
            }
            else
            {
                itemView = item.data as UI_Item_ChatEmojiMenu_SubView;
            }
            
            itemView.SetInfo(m_emojiMenuInfo[item.index],m_currentEmojiMenu == m_emojiMenuInfo[item.index],OnClickEmojiMenuItem);
        }

        private void OnClickEmojiItem(int emojiID)
        {
            m_chatProxy.SendMsg(m_chatProxy.GetEmojiStr(emojiID),m_currentContact ,EnumMsgType.Emoji);
        }

        private void OnLongClickEmojiItem(int emojiID,Vector2 position)
        {
            view.m_UI_Model_ChatEmojiPreview.gameObject.SetActive(true);
            view.m_UI_Model_ChatEmojiPreview.SetInfo(emojiID,position);
        }

        private void OnReleaseLongClick(int emojiID)
        {
            view.m_UI_Model_ChatEmojiPreview.gameObject.SetActive(false);
        }

        private void OnClickEmojiMenuItem(int groupID)
        {
            var emojiGroupInfo = m_emojiMenuInfo.Find(x => x.Group == groupID);
            if (emojiGroupInfo.Group == m_currentEmojiMenu.Group)
            {
                return;
            }
            SetCurrentEmojiGroup(emojiGroupInfo);
        }
        

        #endregion
        
        #region 消息设置界面      

        public class ChatGroupMember
        {
            public long rid;
            public long headID;
            public long headFrameID;
        }

        private List<ChatGroupMember> m_chatGroupMember = new List<ChatGroupMember>();
        private void OnContactSetting()
        {

            switch (m_currentContact.channelType)
            {
                case EnumChatChannel.world:
                    {
                        view.m_btn_chatsetting_GameButton.gameObject.SetActive(false);
                        view.m_btn_chatundisturb_GameButton.gameObject.SetActive(true);
                        view.m_img_chatdisturb_PolygonImage.gameObject.SetActive(m_currentContact.noDisturb);
                        view.m_img_chatundisturb_PolygonImage.gameObject.SetActive(!m_currentContact.noDisturb);
                        view.m_btn_chatundisturb_GameButton.onClick.RemoveAllListeners();
                        view.m_btn_chatundisturb_GameButton.onClick.AddListener(()=>
                        {
                            m_currentContact.noDisturb = !m_currentContact.noDisturb;
                            view.m_img_chatdisturb_PolygonImage.gameObject.SetActive(m_currentContact.noDisturb);
                            view.m_img_chatundisturb_PolygonImage.gameObject.SetActive(!m_currentContact.noDisturb);
                            m_chatProxy.SendChatNoDisturb(m_currentContact);
                            OnTotalUnreadNum();
                        });
                    }
                    break;
                case EnumChatChannel.alliance:
                    {
                        view.m_btn_chatsetting_GameButton.gameObject.SetActive(true);
                        view.m_btn_chatundisturb_GameButton.gameObject.SetActive(false);
                        view.m_btn_chatsetting_GameButton.onClick.RemoveAllListeners();
                        view.m_btn_chatsetting_GameButton.onClick.AddListener(()=>
                        {
                            RefreshContactDetails(EnumChatChannel.alliance);
                        });
                    }
                    break;
                case EnumChatChannel.privatechat:
                    {
                        view.m_btn_chatsetting_GameButton.gameObject.SetActive(false);
                        view.m_btn_chatundisturb_GameButton.gameObject.SetActive(false);
                        view.m_btn_chatsetting_GameButton.onClick.RemoveAllListeners();
                    }
                    break;
                default:break;
            }
            m_chatProxy.CurrentChannelType = m_currentContact.channelType;
        }

        private void RefreshContactDetails(EnumChatChannel chatChannel)
        {
            switch(chatChannel)
            {

                case EnumChatChannel.alliance:
                    {
                        view.m_pl_chat.gameObject.SetActive(false);
                        view.m_pl_detail.gameObject.SetActive(true);
                        view.m_pl_groupname.gameObject.SetActive(false);
                        view.m_sv_playerhead_PolygonImage.gameObject.SetActive(true);
                        view.m_pl_addmember.gameObject.SetActive(false);
                        view.m_pl_deletemember.gameObject.SetActive(false);
                        view.m_pl_blockthis.gameObject.SetActive(false);
                        view.m_btn_cancel.gameObject.SetActive(false);
                        view.m_btn_detailDelete.gameObject.SetActive(false);
                        view.m_btn_detailMsg.gameObject.SetActive(false);
                        view.m_pl_nodisturb.gameObject.SetActive(true);
                        view.m_ck_nodisturb_GameToggle.RemoveAllClickListener();
                        view.m_ck_nodisturb_GameToggle.isOn = m_chatProxy.AllianceContact.noDisturb;
                        view.m_ck_nodisturb_GameToggle.AddListener(()=>
                        {
                            var isOn = view.m_ck_nodisturb_GameToggle.isOn;
                            m_currentContact.noDisturb = isOn;
                            //发送免打扰协议
                            Chat_SendChatMsgNoDisturb.request req = new Chat_SendChatMsgNoDisturb.request();
                            req.chatNoDisturbInfo = new Dictionary<long, ChatNoDisturbInfo>();
                            req.chatNoDisturbInfo.Add((long)m_currentContact.channelType, new ChatNoDisturbInfo { channelType = (long)m_currentContact.channelType, chatNoDisturbFlag = m_currentContact.noDisturb });
                            AppFacade.GetInstance().SendSproto(req);
                            OnTotalUnreadNum();
                        });
                        view.m_btn_detailfunc.SetText(LanguageUtils.getText(750017));
                        view.m_btn_detailfunc.gameObject.SetActive(true);
                        view.m_btn_detailfunc.RemoveAllClickEvent();

                        view.m_btn_detailfunc.AddClickEvent(()=>
                        {
                            if (_allianceProxy.HasJionAlliance())
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                            }
                            else
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                            }
                            CoreUtils.uiManager.CloseUI(UI.s_chat);
                        });
                        view.m_btn_returnfromdetail_GameButton.onClick.RemoveAllListeners();
                        view.m_btn_returnfromdetail_GameButton.onClick.AddListener(()=>
                        {
                            ResetSetting();
                        });

                        m_chatGroupMember.Clear();
                        _allianceProxy.GetAllMemberDic().Values.ToList().ForEach((member) =>
                        {
                            if (_allianceProxy.CheckIsR45(member.rid))
                            {
                                m_chatGroupMember.Add(new ChatGroupMember {rid = member.rid, headID = member.headId});
                            }
                        });
                        InitChatMemberList();
                    }
                    break;
                default:break;
            }
        }

        private bool isInitChatMemberList;
        private void InitChatMemberList()
        {
            if(!isInitChatMemberList)
            {
                isInitChatMemberList = true;
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = InitMemberListItem;
                view.m_sv_playerhead_ListView.SetInitData(m_assetDic, funcTab);
            }
            view.m_sv_playerhead_ListView.FillContent(m_chatGroupMember.Count);
        }

        private void InitMemberListItem(ListView.ListItem item)
        {
            ChatGroupMember member = m_chatGroupMember[item.index];
            UI_Item_PlayerHeadSelectView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_PlayerHeadSelectView>(item.go);

            itemView.m_img_using_PolygonImage.gameObject.SetActive(false);
            itemView.m_img_lock_PolygonImage.gameObject.SetActive(false);
            itemView.m_img_selet_PolygonImage.gameObject.SetActive(false);
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(member.headID,member.headFrameID);

            view.m_sv_playerhead_ListView.UpdateItemSize(item.index,126);
        }

        private void ResetSetting()
        {
            view.m_pl_detail.gameObject.SetActive(false);
            view.m_pl_chat.gameObject.SetActive(true);
        }

        #endregion


        #region Tools

        //聊天通用时间格式显示
        private string ConvetToTimeString(long timeStamp)
        {
            DateTime current = ServerTimeModule.Instance.GetCurrServerDateTime();
            DateTime target = ServerTimeModule.Instance.ConverToServerDateTime(timeStamp);
            if(current.Date==target.Date)
            {
                return LanguageUtils.getTextFormat(300089, AddFirstZero(target.Hour), AddFirstZero(target.Minute));
            }
            return LanguageUtils.getTextFormat(300002, target.Year, target.Month, target.Day, AddFirstZero(target.Hour), AddFirstZero(target.Minute));
        }

        private string AddFirstZero(int num)
        {
            return num <= 9 ? $"0{num}" : num.ToString();
        }
        
        private string ShowOverlayMsgCount(int count)
        {
            return UIHelper.NumerBeyondFormat(count);
        }

        private string ShowOverlayMsgCount(long count)
        {
            return UIHelper.NumerBeyondFormat((int)count);
        }

        private void CheckChatClose(UIInfo uiInfo)
        {
            if (uiInfo.info.layer == UILayer.WindowLayer)
            {
                CoreUtils.uiManager.CloseUI(UI.s_chat);
            }
        }
        
        private void OnInputMsgChanged(string msg)
        {
            if (msg.Length > m_chatProxy.ChannelWordLimit)
            {
                view.m_ipt_chat_GameInput.text = msg.Substring(0, m_chatProxy.ChannelWordLimit);
            }
        }
        #endregion
    }
}