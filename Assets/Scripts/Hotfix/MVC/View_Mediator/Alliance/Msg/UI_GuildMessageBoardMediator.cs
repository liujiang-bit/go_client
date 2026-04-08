// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, May 18, 2020
// Update Time         :    Monday, May 18, 2020
// Class Description   :    UI_GuildMessageBoardMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Client.Utils;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Game {
    public class UI_GuildMessageBoardMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_GuildMessageBoardMediator";


        #endregion

        //IMediatorPlug needs
        public UI_GuildMessageBoardMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_GuildMessageBoardView view;

        private GuildInfoEntity m_guildInfo;
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        
        
        private List<BoardMessageInfo> m_msgs = new List<BoardMessageInfo>();
        private Dictionary<long,BoardMessageInfo> m_msgDic = new Dictionary<long, BoardMessageInfo>();
        
        private Dictionary<long,BoardMessageInfo> m_msgExtDic = new Dictionary<long, BoardMessageInfo>();

        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;

        private long m_selectedMsgIndex;
        private BoardMessageInfo m_replymsg;

        private bool m_canLeaveMsg = false;

        private int m_maxShowReplyNum = 5;


        private int m_replyNoSelfLimit = 5;
        
        
        

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_SendBoardMessage.TagName,
                Guild_GetGuildMessageBoard.TagName,
                Guild_DeleteBoardMessage.TagName,
                Guild_ModifyGuildInfo.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_ModifyGuildInfo.TagName:
                {


                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                    }
                    else
                    {
                        Guild_ModifyGuildInfo.response response =
                            notification.Body as Guild_ModifyGuildInfo.response;


                        if (response.type ==6)
                        {
                            m_canLeaveMsg = response.messageBoardStatus;
                            CheckBtnState();
                        }
                    }

                    
                }
                break;
                case Guild_SendBoardMessage.TagName:
                {
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage) notification.Body;

                        switch ((ErrorCode) error.errorCode)
                        {
                            case ErrorCode.GUILD_REPLY_MESSAGE_INVALID:
                                Tip.CreateTip(730342).Show();
                                break;
                            case ErrorCode.GUILD_NO_OPEN_MESSAGE_BOARD:
                                Tip.CreateTip(730330).Show();
                                break;
                        }

                       
                    }
                    else
                    {
                        Guild_SendBoardMessage.response response =
                            notification.Body as Guild_SendBoardMessage.response;
                    
                        var floorMsg = AddMsg(response.message);

                        if (floorMsg!=null && floorMsg.subFloors.Count>0)
                        {
                            BoardMessageInfo extMsg;
                            if (m_msgExtDic.TryGetValue(floorMsg.replyMessageIndex, out extMsg))
                            {
                                this.ShrinkMsgFloor(extMsg);
                            }
                        }
                        
                        ReList();
                    }

                    
                }
                    break;
                case Guild_GetGuildMessageBoard.TagName:
                {
                    Guild_GetGuildMessageBoard.response response =
                        notification.Body as Guild_GetGuildMessageBoard.response;

                    //Debug.Log(response.messageBoardStatus+"下发"+response.messages.Count);
                    
                    
                    response.messages.Sort(((info, messageInfo) => info.messageIndex.CompareTo(messageInfo.messageIndex) ));
                    
                    response.messages.ForEach((msg) => { AddMsg(msg); });

                    CheckFistList();
                    m_canLeaveMsg = response.messageBoardStatus;
                    view.m_ck_switch_GameToggle.onValueChanged.RemoveAllListeners();
                    CheckBtnState();
                    view.m_ck_switch_GameToggle.onValueChanged.AddListener(onOpenMsg);

                    ReList();
                }
                    break;

                case Guild_DeleteBoardMessage.TagName:
                {
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage) notification.Body;

                        switch ((ErrorCode) error.errorCode)
                        {
                            
                        }

                        Tip.CreateTip(730342).Show();
                    }
                    else
                    {
                        Guild_DeleteBoardMessage.response response =
                            notification.Body as Guild_DeleteBoardMessage.response;

                        DelMsg(response.messageIndex);
                        
                        
                        
                    }
                }
                    break;

                default:
                    break;
            }
        }


        private void CheckFistList()
        {
            List<BoardMessageInfo> extList = new List<BoardMessageInfo>(); 
            m_msgs.ForEach((msg) =>
            {
                if (msg.messageIndex==0 && msg.sendTime>0)
                {
                    extList.Add(msg);
                }
            });
            
            extList.ForEach((msg) => { ShrinkMsgFloor(msg); });
        }

        private BoardMessageInfo AddMsg(BoardMessageInfo newmsg)
        {
            BoardMessageInfo oldMsg;
            BoardMessageInfo floorMsg=null;
            if (newmsg.replyMessageIndex>0)
            {
               

                m_msgDic.TryGetValue(newmsg.replyMessageIndex, out floorMsg);
                for (int i = 0; i < m_msgs.Count; i++)
                {
                    oldMsg = m_msgs[i];
                    int replyCount = 0;
                    if (oldMsg.messageIndex == newmsg.replyMessageIndex &&
                        oldMsg.replyMessageIndex == 0 && oldMsg.subFloors == null )
                    {
                        oldMsg.subFloors = new List<BoardMessageInfo>();

                        oldMsg.subFloors.Add(newmsg);
                        m_msgs.Insert(i + 1, newmsg);
                        break;
                    }
                    else
                    {
                        if (oldMsg.replyMessageIndex == newmsg.replyMessageIndex &&
                            newmsg.sendTime >= oldMsg.sendTime)
                        {
                            m_msgs.Insert(i , newmsg);
                            if (floorMsg != null && !floorMsg.subFloors.Contains(newmsg))
                            {
                                floorMsg.subFloors.Add(newmsg);
                            }
                            break;
                        }
                    }
                }
                
                CheckFlortMsgAdd(floorMsg, newmsg);
            }
            else
            {
                m_msgs.Insert(0,newmsg);
            }

            if (!m_msgDic.ContainsKey(newmsg.messageIndex))
            {
                m_msgDic.Add(newmsg.messageIndex, newmsg);
            }

            return floorMsg;
        }


        private void CheckFlortMsgAdd(BoardMessageInfo floorMsg,BoardMessageInfo newMsg)
        {
            //                //处理下收缩
            if (floorMsg.subFloors.Count>m_maxShowReplyNum)
            {
                BoardMessageInfo extMsg;
                if (!m_msgExtDic.TryGetValue(newMsg.replyMessageIndex,out extMsg))
                {
                    extMsg = new BoardMessageInfo();
                    extMsg.replyMessageIndex = newMsg.replyMessageIndex;
                    extMsg.sendTime = floorMsg.subFloors.Count - m_maxShowReplyNum;
                        
                    m_msgExtDic.Add(newMsg.replyMessageIndex,extMsg);

                    var firstMsg = floorMsg.subFloors[0];

                    int index = m_msgs.IndexOf(firstMsg);
                        
                    m_msgs.Insert(index+1,extMsg);
                }

                if (extMsg.sendTime>0)
                {
                    //extMsg.sendTime = floorMsg.subFloors.Count - m_maxShowReplyNum;
                    
                    ShrinkMsgFloor(extMsg);
                }
            }
            else
            {
                BoardMessageInfo extMsg;
                if (m_msgExtDic.TryGetValue(newMsg.replyMessageIndex, out extMsg))
                {
                    m_msgExtDic.Remove(newMsg.replyMessageIndex);
                    m_msgs.Remove(extMsg);
                }
            }
        }

        private void CheckFloorDel(BoardMessageInfo floorMsg,BoardMessageInfo delMsg)
        {
            BoardMessageInfo extMsg;
            if (m_msgExtDic.TryGetValue(delMsg.replyMessageIndex, out extMsg))
            {
                if (floorMsg.subFloors.Count <= m_maxShowReplyNum)
                {
                
                    m_msgExtDic.Remove(delMsg.replyMessageIndex);
                    m_msgs.Remove(extMsg);
                }
                else
                {

                    if (extMsg.sendTime>0)
                    {
                        this.ShrinkMsgFloor(extMsg);
                    }
                    
                    
                }
            }

            
        }

        private BoardMessageInfo GetSubMsgFloor(long replyMessageIndex)
        {
            BoardMessageInfo floorMsg=null;
            m_msgDic.TryGetValue(replyMessageIndex, out floorMsg);
            return floorMsg;
        }

        private void DelMsg(long msgIndex)
        {
            BoardMessageInfo delMsg;

            if (m_msgDic.TryGetValue(msgIndex, out delMsg))
            {
                if (delMsg.replyMessageIndex>0)//回复类型的
                {
                    //主楼删除
                    BoardMessageInfo floorMsg;
                    m_msgDic.TryGetValue(delMsg.replyMessageIndex, out floorMsg);
                    floorMsg.subFloors.Remove(delMsg);
                    
                    m_msgs.Remove(delMsg);
                    m_msgDic.Remove(msgIndex);
                    
                    CheckFloorDel(floorMsg,delMsg);
                }
                else
                {
                    //删除主楼
                    for (int i = m_msgs.Count-1; i >=0; i--)
                    {
                        BoardMessageInfo msg = m_msgs[i];
                        if (delMsg.messageIndex == msg.messageIndex)
                        {
                            //Debug.Log("删除"+msg.messageIndex+"  "+msg.replyMessageIndex);
                            m_msgs.Remove(msg);
                            m_msgDic.Remove(msg.messageIndex);
                            m_msgExtDic.Remove(msg.messageIndex);
                           
                        }

                        if (delMsg.messageIndex == msg.replyMessageIndex)
                        {
                            //Debug.Log("删除"+msg.messageIndex+"  "+msg.replyMessageIndex);
                            m_msgs.Remove(msg);
                            m_msgDic.Remove(msg.messageIndex);
                        }

                        if (i==2)
                        {
                            CheckFloorDel(delMsg,msg);
                        }
                    }
                }
                
                ReList();
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
            if (view.data is GuildInfoEntity)
            {
                m_guildInfo = view.data as GuildInfoEntity;
            }
            
            if (view.data is GuildInfo)
            {
                var temp = view.data as GuildInfo;
                
                m_guildInfo = new GuildInfoEntity();

                GuildInfoEntity.updateEntity(m_guildInfo, temp);
            }
            
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        private bool isPress;

        private Vector2 m_startPos;

        protected override void BindUIEvent()
        {
            
            view.m_btn_sendMsg_GameButton.onClick.AddListener(onSendMsg);

            view.m_ipt_message_GameInput.onValueChanged.AddListener(onChangeInput);
            
           
            
            view.m_sv_list_ListView.onDragBegin = delegate(PointerEventData data)
            {
                isPress = true;
                m_startPos = data.position;
            };
            
            view.m_sv_list_ListView.onDragEnd = delegate(PointerEventData data)
            {
                if (m_startPos.y<data.pressPosition.y && isPress)
                {
                   
                    //Debug.Log(m_startPos+"下拉"+data.pressPosition);

                    var item = view.m_sv_list_ListView.GetItemByIndex(0);

                    if (item!=null&& m_msgs.Count>0)
                    {
                        var msgdata = m_msgs[0];
                        m_allianceProxy.SendGetGuildMessageBoard(m_guildInfo.guildId,msgdata.messageIndex,1);
                    }
                }
                
                isPress = false;
            };

            view.m_ipt_message_GameInput.characterLimit = m_allianceProxy.Config.allianceMessageCharacterLimit;
        }

        private void onOpenMsg(bool v)
        {
            if (m_guildInfo.guildId != m_playerProxy.CurrentRoleInfo.guildId || m_guildInfo.guildId == m_playerProxy.CurrentRoleInfo.guildId&&m_allianceProxy.CheckIsOffiecOrLeader(m_playerProxy.CurrentRoleInfo.rid)==false)
            {

                if (v!= m_canLeaveMsg)
                {
                    Tip.CreateTip(730136).Show();
                    view.m_ck_switch_GameToggle.isOn = m_canLeaveMsg;
                }
               
                return;
            }

            m_canLeaveMsg = v;
            m_allianceProxy.SendEditAllianceInfo(6,"",null);
            
        }

        private void CheckBtnState()
        {

            view.m_ck_switch_GameToggle.isOn = m_canLeaveMsg;
            //Debug.Log("开光"+m_canLeaveMsg);


            if (view.m_ck_switch_GameToggle.isOn)
            {
                view.m_pl_publish_MakeChildrenGray.Normal();
            }
            else
            {
                view.m_pl_publish_MakeChildrenGray.Gray();
            }

            //view.m_btn_sendMsg_GameButton.interactable = view.m_ck_switch_GameToggle.isOn;
            view.m_ipt_message_GameInput.interactable = view.m_ck_switch_GameToggle.isOn;
            
            
            view.m_img_open_PolygonImage.gameObject.SetActive(m_canLeaveMsg);
            view.m_img_close_PolygonImage.gameObject.SetActive(!m_canLeaveMsg);
                
                
        }

        private void onChangeInput(string text)
        {
            if (m_headStartLen>0)
            {
                if (text.Length<m_headStartLen && !text.EndsWith(":"))
                {
                    ClearInput();
                }
            }
        }

        private void onSendMsg()
        {

            if (view.m_ck_switch_GameToggle.isOn==false)
            {
                Tip.CreateTip(730330).Show();
                return;
            }
            
            //需市政厅等级{0}以上才能使用；（语言包id：730329
            if (SystemOpen.IsSystemOpen(EnumSystemOpen.guild_msg,false))
            {
                if (view.m_ipt_message_GameInput.text.Length>0)
                {
                    
                    string cpMsg = String.Empty;
                    int tip = 0;
                    
                    
                    if (tip==0 && m_headStartLen>0)
                    {
                        cpMsg = view.m_ipt_message_GameInput.text.Substring(m_headStartLen);
                        //if (BannedWord.ChackHasBannedWord(cpMsg))
                        //{
                        //    tip=300128;
                        //}
                    }

                    if (!string.IsNullOrEmpty(m_headText))
                    {
                        cpMsg = LanguageUtils.getTextFormat(730336, m_headText, cpMsg);
                    }
                    else if(m_headStartLen==0)
                    {
                        cpMsg = view.m_ipt_message_GameInput.text;
                    }

                    BannedWord.CheckChatBannedWord(cpMsg);

                    if (cpMsg.Length>0)
                    {
                       
                        if (tip==0 && m_replymsg!=null && m_replymsg.subFloors!=null && m_replymsg.subFloors.Count> m_allianceProxy.Config.allianceMessageTierLimit-1)
                        {
                            tip=730349;
                        }else if (tip==0 && m_replymsg!=null && CheckReplyLimit(m_replymsg))
                        {
                            tip=730348;
                        }

                        if (tip>0)
                        {
                            Tip.CreateTip(tip).Show();
                        }
                        else
                        {
                            m_allianceProxy.SendNewBoardMessage(m_guildInfo.guildId,m_selectedMsgIndex,cpMsg);
                        }
                    }
                    ClearInput();
                }
            }
            else
            {
                var cdata = CoreUtils.dataService.QueryRecord<SystemOpenDefine>((int) EnumSystemOpen.guild_msg);
                Tip.CreateTip(730329, cdata.openLv).Show();
            }
        }

        private bool CheckReplyLimit(BoardMessageInfo msg )
        {
            if (msg.subFloors!=null)
            {
                int count = 0;
                
                msg.subFloors.ForEach((info =>
                {
                    if (info.roleInfo.rid == m_playerProxy.CurrentRoleInfo.rid)
                    {
                        count++;
                    }
                    
                }));

                if (count>=m_allianceProxy.Config.allianceMessageReplyLimit)
                {
                    return true;
                }
            }
            return false;
        }

        private void ClearInput()
        {
            m_headStartLen = 0;
            m_selectedMsgIndex = 0;
            view.m_ipt_message_GameInput.text = "";
            m_headText= String.Empty;
            m_replymsg = null;
        }

        


        private void UpdateInfo()
        {
            view.m_UI_GuildFlag.setData(m_guildInfo.signs);
            view.m_lbl_guildName_LanguageText.text =
                AllianceProxy.FormatGuildName(m_guildInfo.name,m_guildInfo.abbreviationName);
        }

        private void ReList()
        {
            if (m_assetDic.Count>0)
            {
                //Debug.Log("留言条数"+m_msgs.Count);
                view.m_sv_list_ListView.FillContent(m_msgs.Count);
            }
        }

        protected override void BindUIData()
        {
            UpdateInfo();
            
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemSize = GetItemSize;
                funcTab.GetItemPrefabName = GetContactItemPrefabName;
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                
                ReList();
            });
            
            
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);
            
            view.m_ipt_message_GameInput.onEndEdit.AddListener(onEndText);
            
            m_allianceProxy.SendGetGuildMessageBoard(m_guildInfo.guildId,0,1);
        }
        
        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_msgs[item.index];
            int diffHeight = 0;

            m_itemHeights.TryGetValue(data, out diffHeight);
            
            //Debug.Log(item.index+" GetItemSize h:"+diffHeight);


            if (data.replyMessageIndex>0 && data.sendTime>1000)
            {
                return 93f+diffHeight;
            }
            else if (data.messageIndex>0 && data.floorId>0)
            {
                return 93f+diffHeight;
            }

            return 50f;
        }


        private void onEndText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ClearInput();
            }
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMsg);
        }

        private string GetContactItemPrefabName(ListView.ListItem item)
        {
            
            var msg = m_msgs[item.index];
            if (msg.replyMessageIndex>0 && msg.messageIndex>0)
            {
                return view.m_sv_list_ListView.ItemPrefabDataList[1];
            }

            if (msg.messageIndex>0 && msg.sendTime>1000)
            {
                return view.m_sv_list_ListView.ItemPrefabDataList[0];
            }
            
            return view.m_sv_list_ListView.ItemPrefabDataList[2];
        }
        
        private Dictionary<BoardMessageInfo,int> m_itemHeights = new Dictionary<BoardMessageInfo, int>();
        private float m_initItemheight = 0;

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_msgs[scrollItem.index];
            
            bool isSubMsg = data.replyMessageIndex>0;
            
            if (data.replyMessageIndex>0 && data.sendTime>1000)
            {
                UI_Item_GuildMessageSubView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildMessageSubView>(scrollItem.go);
                
                itemView.m_UI_PlayerHead.LoadPlayerIcon(data.roleInfo.headId);
                
                itemView.m_lbl_playerName_LanguageText.text = AllianceProxy.FormatGuildName(data.roleInfo.name,data.roleInfo.guildAbbName,data.roleInfo.rid == m_playerProxy.CurrentRoleInfo.rid?730365:0);
                
                itemView.m_lbl_date_LanguageText.text =
                    ServerTimeModule.Instance.ConverToServerDateTime(data.sendTime).ToString("MM-dd HH:mm");// +" R:  "+data.replyMessageIndex+"  ID"+data.messageIndex;
                
                itemView.m_btn_translation_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_translation_GameButton.onClick.AddListener(() =>
                {
                    ClientUtils.TranslatorSDK(itemView.m_lbl_messageText_LanguageText);
                });
                
                itemView.m_btn_rep_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_rep_GameButton.onClick.AddListener((() =>
                {
                    onMenu(itemView.m_btn_rep_GameButton.transform as RectTransform,data);
                }));
                
                if (m_initItemheight==0)
                {
                    m_initItemheight = itemView.m_btn_rep_PolygonImage.rectTransform.sizeDelta.y;
                }
                
                itemView.m_lbl_messageText_LanguageText.text = "";

                float baseHeight = itemView.m_lbl_messageText_LanguageText.preferredHeight;
                
                itemView.m_lbl_messageText_LanguageText.text = data.content;

                int diffHeight = (int)itemView.m_lbl_messageText_LanguageText.preferredHeight - (int)baseHeight;

                if (itemView.m_lbl_messageText_LanguageText.preferredHeight>30 )
                {
                    int oldHeight = 0;
                    m_itemHeights.TryGetValue(data, out oldHeight);
                    if (oldHeight==0 || oldHeight>0 && oldHeight!= diffHeight)
                    {
                        if (oldHeight==0)
                        {
                            m_itemHeights.Add(data,diffHeight);
                            view.m_sv_list_ListView.RefreshItem(scrollItem.index);
                        }else if (oldHeight>0 && oldHeight!= diffHeight)
                        {
                            m_itemHeights[data] = diffHeight;
                            view.m_sv_list_ListView.RefreshItem(scrollItem.index);
                        }
                    }
                }
                
                itemView.m_btn_rep_PolygonImage.rectTransform.sizeDelta = new Vector2(itemView.m_btn_rep_PolygonImage.rectTransform.sizeDelta.x,m_initItemheight+diffHeight);
                
            }
            else if(data.messageIndex>0 && data.floorId>0)
            {
                UI_Item_GuildMessageView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildMessageView>(scrollItem.go);
                
                itemView.m_UI_PlayerHead.LoadPlayerIcon(data.roleInfo.headId);
                
                itemView.m_lbl_playerName_LanguageText.text = AllianceProxy.FormatGuildName(data.roleInfo.name,data.roleInfo.guildAbbName,data.roleInfo.rid == m_playerProxy.CurrentRoleInfo.rid?730365:0);
               
                itemView.m_lbl_date_LanguageText.text = LanguageUtils.getTextFormat(730334, data.floorId,
                        ServerTimeModule.Instance.ConverToServerDateTime(data.sendTime).ToString("MM-dd HH:mm"))
                    ;
                
                itemView.m_btn_translation_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_translation_GameButton.onClick.AddListener(() =>
                {
                    ClientUtils.TranslatorSDK(itemView.m_lbl_messageText_LanguageText);
                });
                
                itemView.m_btn_rep_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_rep_GameButton.onClick.AddListener((() =>
                {
                    onMenu(itemView.m_btn_rep_GameButton.transform as RectTransform,data);
                }));


                if (m_initItemheight==0)
                {
                    m_initItemheight = itemView.m_btn_rep_PolygonImage.rectTransform.sizeDelta.y;
                }
                
                itemView.m_lbl_messageText_LanguageText.text = "";

                float baseHeight = itemView.m_lbl_messageText_LanguageText.preferredHeight;
                
                itemView.m_lbl_messageText_LanguageText.text = data.content;

                int diffHeight = (int)itemView.m_lbl_messageText_LanguageText.preferredHeight - (int)baseHeight;

                if (itemView.m_lbl_messageText_LanguageText.preferredHeight>30 )
                {
                    int oldHeight = 0;
                    m_itemHeights.TryGetValue(data, out oldHeight);
                    if (oldHeight==0 || oldHeight>0 && oldHeight!= diffHeight)
                    {
                        if (oldHeight==0)
                        {
                            m_itemHeights.Add(data,diffHeight);
                            view.m_sv_list_ListView.RefreshItem(scrollItem.index);
                        }else if (oldHeight>0 && oldHeight!= diffHeight)
                        {
                            m_itemHeights[data] = diffHeight;
                            view.m_sv_list_ListView.RefreshItem(scrollItem.index);
                        }
                    }
                }
                
                itemView.m_btn_rep_PolygonImage.rectTransform.sizeDelta = new Vector2(itemView.m_btn_rep_PolygonImage.rectTransform.sizeDelta.x,m_initItemheight+diffHeight);

            }
            else
            {
                UI_Item_GuildMessageFlexView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildMessageFlexView>(scrollItem.go);

                if (data.sendTime>0)
                {
                    itemView.m_lbl_playerName_LanguageText.text = LanguageUtils.getTextFormat(730350,data.sendTime);
                }
                else
                {
                    itemView.m_lbl_playerName_LanguageText.text = LanguageUtils.getText(730351);
                }

                itemView.m_btn_translation_GameButton.onClick.RemoveAllListeners();

                itemView.m_btn_translation_GameButton.onClick.AddListener(() =>
                {
                    if (data.sendTime >0 )
                    {
                        ExpandMsgFloor(data);
                    }
                    else
                    {
                        ShrinkMsgFloor(data);
                    }
                });
            }
        }

        //扩展
        private void ExpandMsgFloor(BoardMessageInfo extmsg)
        {
            extmsg.sendTime = 0;
            //Debug.Log("扩展"+extmsg.sendTime);
            var floorMsg = GetSubMsgFloor(extmsg.replyMessageIndex);

            floorMsg.subFloors.ForEach((info =>
            {
                if (!m_msgs.Contains(info))
                {
                    AddMsg(info);
                }
            }));

            ReList();
        }

        //收缩
        private void ShrinkMsgFloor(BoardMessageInfo extmsg)
        {
            
            
            var floorMsg = GetSubMsgFloor(extmsg.replyMessageIndex);

            int count = floorMsg.subFloors.Count- m_maxShowReplyNum ;
            int index = 0;
            
            extmsg.sendTime = count;
            //Debug.Log("收缩"+count);
            floorMsg.subFloors.ForEach((info =>
            {
                if (m_msgs.Contains(info) && index<count)
                {
                    m_msgs.Remove(info);
                }

                index++;
            }));
            ReList();
        }




        private HelpTip m_tipView;
        private UI_Pop_GuildMessageMenuView powTipView;

        private int m_headStartLen=0;
        private string m_headText;

        private void onMenu(RectTransform transform,BoardMessageInfo msg)
        {

            
            
            CoreUtils.assetService.Instantiate("UI_Pop_GuildMessageMenu", (obj) =>
            {
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_GuildMessageMenuView>(obj);



                m_tipView = HelpTip.CreateTip(powTipView.gameObject, ClientUtils.GetPreferredSize(powTipView.m_pl_bg_ContentSizeFitter), transform)
                    .SetStyle(HelpTipData.Style.arrowLeft).Show();
                
                
                
                
                powTipView.m_UI_copy.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {

                    string copyString = msg.content;

                    int copyIndex = copyString.LastIndexOf("</b>");
                    if (copyIndex>1)
                    {
                        copyString = copyString.Substring(copyIndex+4);
                    }
                        GUIUtility.systemCopyBuffer = copyString;
                        CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);

                    });
                
                
                powTipView.m_UI_rep.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    bool isSub = msg.replyMessageIndex>0;
                    
                    
                    
                    if (isSub)
                    {
                        var floorMsg = GetSubMsgFloor(msg.replyMessageIndex);
                        m_selectedMsgIndex = msg.messageIndex;
                        m_replymsg = floorMsg;
                        view.m_ipt_message_GameInput.text = string.Format("#NO.{0}@{1}:", floorMsg.floorId, msg.roleInfo.name);
                        m_headText = msg.roleInfo.name;
                    }
                    else
                    {
                        m_selectedMsgIndex = msg.messageIndex;
                        m_replymsg = msg;
                        view.m_ipt_message_GameInput.text = string.Format("#NO.{0}:", msg.floorId);
                    }
                    
                    

                    m_headStartLen = view.m_ipt_message_GameInput.text.IndexOf(':')+1;
                    
                    view.m_ipt_message_GameInput.MoveTextEnd(true);
                    
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                    
                });

                powTipView.m_UI_del.m_btn_languageButton_GameButton.interactable =
                    msg.roleInfo.rid == m_playerProxy.CurrentRoleInfo.rid || m_guildInfo.guildId == m_playerProxy.CurrentRoleInfo.guildId &&
                    m_allianceProxy.CheckIsOffiecOrLeader(m_playerProxy.CurrentRoleInfo.rid);
                
                powTipView.m_UI_del.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    
                    Alert.CreateAlert(730353).SetRightButton(() =>
                    {
                        m_allianceProxy.SendDelBoardMessage(m_guildInfo.guildId,msg.messageIndex);
                    }).SetLeftButton().Show();

                    
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                });
                 
                powTipView.m_UI_report.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    UIHelper.Reporting(msg.roleInfo.rid,msg.roleInfo.name,msg.content,LanguageUtils.getText(300142));
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                });
            });

        }

        #endregion
    }
}