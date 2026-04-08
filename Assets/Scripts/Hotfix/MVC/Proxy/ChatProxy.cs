// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, December 27, 2019
// Update Time         :    Friday, December 27, 2019
// Class Description   :    ChatProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SprotoType;
using U3D.Threading.Tasks;
using Skyunion;
using Sproto;
using Data;
using System;
using System.Text;
using PureMVC.Interfaces;
using System.Text.RegularExpressions;
using Client;
using Hotfix;
using LitJson;
using System.Linq;
using ArabicSupport;

namespace Game {
    #region  本地类
    public class ChatShareData
    {
        public string guildAbbName;//联盟简称
        public Vector2Int vector2Int;//坐标
        public string gameNode;//服务器id
        public int shareType;//0坐标，1城市
        public ChatShareDefine ChatShareDefine;//
        public string cityName;//name
        public long cityRid;//name
        public int resourceBuild;//
        public int strongHold; 
        public int allianceBuild;//
        public int monster;
        public int rune;
        public string URL;
        public string desc;
    }
    public class ContactInfo
    {
        public long rid { get; set; }
        public string name { get; set; }
        public long headID { get; set; }
        public long headFrameID { get; set; }
        public string guildName { get; set; }
        public long guildID { get; set; }
        public long timeStamp { get; set; } = 0;
        public void SetInfo(PushMsgInfo info)
        {
            if (timeStamp > info.timeStamp)
            {
                return;
            }
            rid = info.rid;
            name = info.name;
            headID = info.headId;
            headFrameID = info.headFrameID;
            guildName = info.guildName;
            guildID = info.guildId;
            timeStamp = info.timeStamp;
        }

        public void SetInfo(ChatRoleBrief info)
        {
            rid = info.rid;
            name = info.name;
            headID = info.headId;
            headFrameID = info.headFrameID;
            guildName = info.guildAbbr;
            guildID = info.guildId;
            timeStamp = ServerTimeModule.Instance.GetServerTime();
        }

        public void SetInfo(RoleInfoEntity info)
        {
            rid = info.rid;
            name = info.name;
            headID = info.headId;
            headFrameID = info.headFrameID;
            guildName = info.guildAbbName;
            guildID = info.guildId;
            timeStamp = ServerTimeModule.Instance.GetServerTime();
        }
    }
    //聊天联系人
    public class ChatContact
    {
        //是否置顶
        public bool isTop;
        //联系人类型
        public EnumChatChannel channelType;
        //未读条数(包括免打扰)
        //public int unreadNum;
        //免打扰
        public bool noDisturb;
        public bool delete;
        //陌生人的RID
        public long rid;
        //最后一条消息
        public ChatMsg lastMsg { get; private set; }
        //当前聊天内容列表
        public ChatMsgList ChatMsgList;
        //消息查询状态
        public EnumChatInfoQueryStatus queryStatus { get; private set; }
        
        public int page = 2;
        //红点判定方式
        public EnumChannelRedDotType redDotType;
        //此联系人的最大已读消息唯一索引 唯一标识判定专用
        private long m_maxMsgUniqueIndex;
        public long MaxMsgUniqueIndex
        {
            get { return m_maxMsgUniqueIndex; }
            set { m_maxMsgUniqueIndex = value; }
        }
        //服务器缓存的已读时间戳
        public long serverLastReadMessageTS = 0;
        //已读时间戳
        private long m_lastReadMessageTS = 0;
        public long LastReadMessageTS
        {
            get { return m_lastReadMessageTS; }
            set { m_lastReadMessageTS = value; }
        }
        //消息最大存储数量
        private int m_maxSaveNum;
        
        //未读消息数
        private int m_unreadNum;

        private long m_playerRid = 0;

        public ChatContact(EnumChatChannel _channelType,EnumChannelRedDotType _redDotType = EnumChannelRedDotType.UniqueIndex,long _contactRid =0,int _unreadnum=0,long _lastReadMessageTS=0)
        {
            channelType = _channelType;
            rid = _contactRid;
            rid = _contactRid;
            redDotType = _redDotType;
            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_playerRid = playerProxy.Rid;
            ChatMsgList = new ChatMsgList();
            InitQueryStatus();
            SetMaxMsgUniqueIndexAndDisturb();
            SetChannelInfo();
            InitContact(_unreadnum,_lastReadMessageTS);
        }

        public void InitContact( int _unreadnum = 0, long _lastReadMessageTS = 0)
        {
            ClearData();
            InitQueryStatus();
            m_unreadNum = _unreadnum;
            LastReadMessageTS = GetLocalLastMsgTS();
            //本地缓存的最后阅读时间，防止聊天时关闭游戏最后阅读时间未向服务器更新
            if (LastReadMessageTS < _lastReadMessageTS)
            {
                LastReadMessageTS = _lastReadMessageTS;
            }
            serverLastReadMessageTS = _lastReadMessageTS;
        }

        private int tmpLastPageCount;
        public void PushMsg(ChatMsg info,bool notifyMsg = true)
        {
            if (redDotType== EnumChannelRedDotType.UniqueIndex && ChatMsgList.ChatMsg.Exists((i) => { return i.uniqueIndex == info.uniqueIndex; }))
            {
                return;
            }

            if (lastMsg==null || info.timeStamp >= lastMsg.timeStamp)
            {
                SetLastMsg(info);
            }
            else if(!info.bLocalData)
            {
                tmpLastPageCount++;
                if (tmpLastPageCount >= ChatProxy.ChatPageItemCount)
                {
                    tmpLastPageCount = 0;
                    page++;
                }
            }

            bool isRead = false;
            switch (redDotType)
            {
                case EnumChannelRedDotType.TimeStamp:
                    isRead = info.timeStamp <= LastReadMessageTS;
                    break;
                case EnumChannelRedDotType.UniqueIndex:
                    isRead = info.uniqueIndex <= MaxMsgUniqueIndex;
                    break;
            }
            if (notifyMsg && !isRead && info.rid != m_playerRid)
            {
                m_unreadNum++;

                if (info.mapMarkerTypeID !=0)
                {
                    ChatProxy.AtRedPoint = true;
                }
            }
            ChatMsgList.Add(info, isRead);
            
            //存储数据超出上限
            if (ChatMsgList.ChatMsg.Count > m_maxSaveNum)
            {
                 RemoveFirstMsg();
            }
        }
        public void SetMsgQueryStatus(EnumChatInfoQueryStatus type)
        {
            queryStatus = type;
        }
        
        public void SetLastMsg(ChatMsg msg)
        {
            if (lastMsg != null && lastMsg.timeStamp > msg.timeStamp)
            {
                return;
            }
            lastMsg = msg;
        }
        
        public void SetRead()
        {
            if (m_unreadNum == 0)
            {
                return;
            }
            if (channelType == EnumChatChannel.alliance)
            {
                ChatProxy.AtRedPoint = false;
            }
            m_unreadNum = 0;
            switch (redDotType)
            {
                case EnumChannelRedDotType.TimeStamp:
                    SaveLocalLastMsgTS();
                    ChatMsgList.SetReadByLastReadTime(LastReadMessageTS);
                    break;
                case EnumChannelRedDotType.UniqueIndex:
                    if(lastMsg!=null)
                    {
                        MaxMsgUniqueIndex = lastMsg.uniqueIndex;
                        ChatMsgList.SetRead(MaxMsgUniqueIndex);
                    }
                    break;
            }
        }
        
        //返回是否为未读变已读
        public bool SetRead(ChatMsg msg)
        {
            if (msg.bRead)
            {
                return false;
            }
            m_unreadNum--;
            return ChatMsgList.SetRead(msg);
        }
        
        //是否在聊天窗口隐藏
        public bool IsHide()
        {
            if (channelType == EnumChatChannel.privatechat && lastMsg == null)
            {
                return true;
            }

            return false;
        }

        public int GetUnreadCount()
        {
            return m_unreadNum;
        }

        public void ClearData()
        {
            ChatProxy.AtRedPoint = false;
            m_unreadNum = 0;
            tmpLastPageCount = 0;
            page = 2;
            lastMsg = null;
            ChatMsgList?.ClearData();
        }
        //是否已获取所有数据
        public bool IsReceivedAllMsg()
        {
            if (queryStatus != EnumChatInfoQueryStatus.Queried)
            {
                switch (channelType)
                {
                    case EnumChatChannel.world:
                    case EnumChatChannel.alliance:
                        if (ChatProxy.ChatPageItemCount * (page - 1) > ChatMsgList.ChatMsg.Count)
                        {
                            queryStatus = EnumChatInfoQueryStatus.Queried;
                        }
                        break;
                    case EnumChatChannel.privatechat:
                        //当前消息为一次性发送
                        if (queryStatus == EnumChatInfoQueryStatus.NotQuery)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                        break;
                }
            }
            return queryStatus == EnumChatInfoQueryStatus.Queried;
        }

        public long GetNoDisturbKey()
        {
            long key = (long) channelType;
            if (channelType == EnumChatChannel.privatechat)
            {
                key = rid;
            }

            return key;
        }

        private void InitQueryStatus()
        {
            switch (channelType)
            {
                case EnumChatChannel.alliance:
                case EnumChatChannel.world:
                    queryStatus = EnumChatInfoQueryStatus.Querying;
                    break;
                case EnumChatChannel.privatechat:
                    queryStatus = EnumChatInfoQueryStatus.NotQuery;
                    break;
            }
        }
        //设置频道信息
        private void SetChannelInfo()
        {
            var config = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)channelType);
            m_maxSaveNum = config.saveNum;
        }

        private void RemoveFirstMsg()
        {
            var msg = ChatMsgList.RemoveFirstMsg();
            if (!msg.bRead)
            {
                m_unreadNum--;
            }
        }

        private void SetMaxMsgUniqueIndexAndDisturb()
        {
            //初始化最大消息索引
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy != null && playerProxy.CurrentRoleInfo != null)
            {
                if (playerProxy.CurrentRoleInfo.maxChatUniqueIndex != null)
                {
                    playerProxy.CurrentRoleInfo.maxChatUniqueIndex.TryGetValue((long)channelType, out ChatReadedUniqueIndex value);
                    if (value != null)
                        MaxMsgUniqueIndex = value.uniqueIndex;
                }
                if (playerProxy.CurrentRoleInfo.chatNoDisturbInfo != null)
                {
                    playerProxy.CurrentRoleInfo.chatNoDisturbInfo.TryGetValue(GetNoDisturbKey(), out ChatNoDisturbInfo value);
                    if (value != null)
                        noDisturb = value.chatNoDisturbFlag;
                }
            }
        }
        
        //本地存储最后已读时间
        private void SaveLocalLastMsgTS()
        {
            var time = ServerTimeModule.Instance.GetServerTime();
            m_lastReadMessageTS = time;
            PlayerPrefs.SetInt(channelType.ToString()+m_playerRid.ToString()+rid.ToString(),(int)time);
        }

        //获取本地缓存的最后已读时间
        private long GetLocalLastMsgTS()
        {
            return PlayerPrefs.GetInt(channelType.ToString()+ m_playerRid.ToString() + rid.ToString());
        }
    }

    //聊天信息
    public class ChatMsg
    {
        public long rid { get; private set; }
        public string msg { get; private set; }
        public string mapMarkerTypemsg { get;  set; }
        public int emojiID { get; private set; } = 0;
        public int chatShareID { get; private set; } = 0;
        public int mapMarkerTypeID { get; private set; } = 0;
        public long uniqueIndex { get; set; } = 0;
        public bool bLocalData { get; set; } = false;
        public EnumMsgType msgType { get; set; } = EnumMsgType.Text;
        public bool bRead { get; set; }
        public long timeStamp { get; set; }
        public string Translate { get; set; }
        public ContactInfo contactInfo { get; set; }
        public void SetChatMsg(PushMsgInfo _msg)
        {
            SetRid(_msg.rid);
            msgType = _msg.rid<=0? EnumMsgType.Announcement: EnumMsgType.Text;
            uniqueIndex = _msg.uniqueIndex;
            timeStamp = _msg.timeStamp;
            if (msgType == EnumMsgType.Announcement &&_msg.HasSystemMsg && _msg.systemMsg.HasLanguageId)
            {
                if (_msg.systemMsg.HasArgs && _msg.systemMsg.args.Count > 0)
                {
                    msg = LanguageUtils.getTextFormat((int)_msg.systemMsg.languageId, _msg.systemMsg.args);
                }
                else
                {
                    msg = LanguageUtils.getText((int) _msg.systemMsg.languageId);
                }
            }
            else
            {
                SetMsgAndCheckMsgType(_msg.msg);
                
            }
        }

        public void SetChatMsg(ChatInfo _info)
        {
            //toRid已转为发送者Rid
            SetRid(_info.toRid);
            timeStamp = _info.timeStamp;
            SetMsgAndCheckMsgType(_info.msg);
        }

        public void SetChatMsg(ChatRoleBrief _info)
        {
            SetRid(_info.rid);
            timeStamp = _info.lastMsgTS;
            SetMsgAndCheckMsgType(_info.lastMsg);
        }
        
        public void SetChatMsg(long _rid, string _msg, EnumMsgType _msgTyp, long _timeStamp)
        {
            SetRid(_rid);
            msgType = _msgTyp;
            if (msgType == EnumMsgType.Text)
            {
                SetMsgAndCheckMsgType(_msg);
            }
            else
            {
                msg = _msg;
            }
            timeStamp = _timeStamp;
        }
        public void SetRid(long _rid)
        {
            var chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            rid = _rid;
            contactInfo = chatProxy.GetContactInfo(rid);
            if (rid!=0 && contactInfo == null)
            {
                CoreUtils.logService.Error($"聊天=====对象数据未找到 Rid:{rid}");
                return;
            }
        }

        public ChatTranslateState GetTranslateState()
        {
            if (Translate == null)
            {
                return ChatTranslateState.NoTranslation;
            }else if (Translate.Equals(string.Empty))
            {
                return ChatTranslateState.Translating;
            }

            return ChatTranslateState.Translated;
        }

        private void SetMsgAndCheckMsgType(string _msg)
        {
            if (_msg == null)
            {
                return;
            }
            {
                var match = Regex.Match(_msg, ChatProxy.EmojiPattern);
                if (match.Success)
                {
                    int index = int.Parse(match.Value);
                    if (CoreUtils.dataService.QueryRecord<ChatEmojiDefine>(index) != null)
                    {
                        var emojiConfig = CoreUtils.dataService.QueryRecord<ChatEmojiDefine>(index);
                        emojiID = index;
                        msgType = EnumMsgType.Emoji;
                        msg = LanguageUtils.getText(emojiConfig.l_nameId);
                        return;
                    }
                }
            }
            {
                var match = Regex.Match(_msg, ChatProxy.chatShareIdPattern);
                if (match.Success)
                {
                    int chatShareId = int.Parse(match.Groups[1].Value);
                    if (CoreUtils.dataService.QueryRecord<ChatShareDefine>(chatShareId) != null)
                    {
                        chatShareID = chatShareId;
                        msgType = EnumMsgType.ChatShare;
                        msg = _msg;
                        return;
                    }
                }
            }
            {
                var match = Regex.Match(_msg, ChatProxy.mapMarkerTypePattern);
                if (match.Success)
                {
                    int tempmapMarkerTypeID = int.Parse(match.Groups[1].Value);
                    if (CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>(tempmapMarkerTypeID) != null)
                    {
                        mapMarkerTypeID = tempmapMarkerTypeID;
                        msgType = EnumMsgType.ATUser;
                        msg = _msg;
                        return;
                    }
                }
            }


            msg = _msg;
        }
    }

    public class ChatMsgList
    {
        public List<ChatMsg> ChatMsg
        {
            get;
        }
        private bool bDirty = true;
        private List<ChatMsg> m_chatMsgwithTime;
        public List<ChatMsg> ChatMsgWithTime
        {
            set { m_chatMsgwithTime = value; }
            get
            {
                if(bDirty)
                {
                    bDirty = false;
                    SetTimeStamp();
                }
                return m_chatMsgwithTime;
            }
        }
        public ChatMsgList()
        {
            ChatMsg = new List<ChatMsg>();
            ChatMsgWithTime = new List<ChatMsg>();
        }

        public void Add(ChatMsg msg,bool read = true)
        {
            this.bDirty = true;
            msg.bRead = read;
            if (ChatMsg.Count > 0)
            {
                int count = ChatMsg.Count;
                bool Insert = false;
                for (int i = 0;i< count;i++ )
                {
                    if (ChatMsg[i].timeStamp > msg.timeStamp)
                    {
                        ChatMsg.Insert(i, msg);
                        Insert = true;
                        break;
                    }
                }
                if (!Insert)
                {
                    ChatMsg.Add(msg);
                }
            }
            else
            {
                ChatMsg.Add(msg);
            }
        }

        private void SetTimeStamp()
        {
            m_chatMsgwithTime = new List<ChatMsg>();
            for (int i = ChatMsg.Count-1;i>=0;i--)
            {
                m_chatMsgwithTime.Add(ChatMsg[i]);
                if(i>0 &&ChatMsg[i].timeStamp - ChatMsg[i-1].timeStamp>ChatProxy.TimeStampInterval)
                {
                    m_chatMsgwithTime.Add(new Game.ChatMsg {timeStamp = ChatMsg[i].timeStamp, bRead = true,msgType = EnumMsgType.Time});
                }
            }
            if(ChatMsg.Count>0)
            {
                m_chatMsgwithTime.Add(new Game.ChatMsg { timeStamp = ChatMsg[0].timeStamp, bRead = true ,msgType = EnumMsgType.Time});
            }
        }
        
        //以已读索引方式更新已读信息
        public void SetRead(long MaxMsgUniqueIndex)
        {
            ChatMsg.ForEach((value)=>
            {
                if(value.uniqueIndex<=MaxMsgUniqueIndex)
                {
                    value.bRead = true;
                }
            });
        }
        //以已读时间戳更新已读信息
        public void SetReadByLastReadTime(long lastReadTime)
        {
            ChatMsg.ForEach((value)=>
            {
                if(value.timeStamp<=lastReadTime)
                {
                    value.bRead = true;
                }
            });
        }

        //返回是否为未读变已读
        public bool SetRead(ChatMsg msg)
        {
            if(!msg.bRead)
            {
                msg.bRead = true;
                return true;
            }
            return false;
        }

        public void ClearData()
        {
            ChatMsg.Clear();
            ChatMsgWithTime.Clear();
        }

        public ChatMsg RemoveFirstMsg()
        {
            if (ChatMsg.Count > 0)
            {
                this.bDirty = true;
                var msg = ChatMsg[0];
                ChatMsg.RemoveAt(0);
                return msg;
            }

            return null;
        }

        public void Dispose()
        {
            ClearData();
        }
    }

    public class LocalCacheMsg
    {
        public ContactInfo TargetInfo { get; set; }
        public List<ChatMsg> ChatMsgs { get; set; } = new List<ChatMsg>();
    }

    public enum EnumChatChannel
    {
        world = 1,
        alliance = 2,
        lostland = 3,//失落之地
        privatechat = 100,
        groupchat = 101,
        marquee = 102,//跑马灯
    }

    public enum EnumChatInfoQueryStatus
    {
        NotQuery,
        Querying,
        Queried,
    }

    public enum EnumChannelRedDotType
    {
        TimeStamp,
        UniqueIndex,
    }

    public enum EnumMsgType
    {
        /// <summary>
        /// 纯文本
        /// </summary>
        Text,
        /// <summary>
        /// 表情
        /// </summary>
        Emoji,
        /// <summary>
        /// 时间
        /// </summary>
        Time,
        /// <summary>
        /// 系统通告
        /// </summary>
        Announcement,
        /// <summary>
        /// 提示
        /// </summary>
        Notice,
        /// <summary>
        /// 聊天分享
        /// </summary>
        ChatShare,
        /// <summary>
        /// at用户
        /// </summary>
        ATUser,
    }
    #endregion
    public class ChatProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "ChatProxy";//由于名称的特殊性，把名称的匹配放在最后，
        public const string LastReadMsgTime = "LastReadMsgTime";
        public const string emojiPre = "/e";
        public const string EmojiPattern = "(?<=" + emojiPre + @")\d{1,}";
        public const string chatShareIdPattern = @"(?<=<chatShare=)([1-9][0-9])>";
        public const string guildAbbNamePattern = @"(?<=<guildAbbName=)([A-Za-z0-9]{1,})>";
        public const string coordinatePattern = @"(?<=<coordinateK=)([0-9]{1,},[0-9]{1,},[0-9]{1,})>";
        public const string cityNamePattern = @"(?<=<cityName=)(.*)>";
        public const string resourceBuildPattern = @"(?<=<resourceBuild=)([1-9][0-9]{1,})>";
        public const string strongHoldPattern = @"(?<=<strongHold=)([1-9][0-9]{1,})>";
        public const string allianceBuildPattern = @"(?<=<allianceBuild=)([1-9][0-9]*)>";
        public const string monsterPattern = @"(?<=<monster=)([1-9][0-9]{1,})>";
        public const string runePattern = @"(?<=<rune=)([1-9][0-9]{1,})>";
        public const string cityRidPattern = @"(?<=<cityRid=)([1-9][0-9]{1,})>";
        public const string descPattern = @"(?<=<desc=)(.*)>";
        public const string mapMarkerTypePattern = @"(?<=<mapMarkerType=)([1-9][0-9]{1,})>";//@/全部 

        public static int TimeStampInterval;

        public static int ChatPageItemCount;
        public static bool AtRedPoint = false;//@红点
        //聊天字符限制
        private int channelWordLimit;
        public EnumChatChannel CurrentChannelType;

        //全部联系人
        private List<ChatContact> m_allContact;
        private List<ChatContact> m_allvalidContact = new List<ChatContact>();
        public List<ChatContact> AllContact
        {
            get
            {
                if (m_allContact == null)
                {
                    m_allContact = new List<ChatContact>();
                }
                return m_allContact;
            }
            set
            {
                if (m_allContact == null)
                {
                    m_allContact = new List<ChatContact>();
                }
                m_allContact = value;
            }
        }
        public List<ChatContact> AllvalidContact
        {
            get
            {
                m_allvalidContact.Clear();
                m_allContact.ForEach((Contact) => {
                    if (!Contact.delete)
                    {
                        m_allvalidContact.Add(Contact);
                    }
                });
                return m_allvalidContact;
            }
        }

        //世界频道
        public ChatContact WorldContact;
        Dictionary<EnumChatChannel, float> chatChannelInterval = new Dictionary<EnumChatChannel, float>();//频道发言事件
        //联盟频道
        public ChatContact AllianceContact;
        private bool m_moveDownAfterReciveMsg = false;//TODO:没懂的地方

        //私聊联系人
        public Dictionary<long, ChatContact> PrivateContact
        {
            get;
        } = new Dictionary<long, ChatContact>();
        public List<ChatContact> validPrivateContact = new List<ChatContact>();
        public List<ChatContact> ValidPrivateContact//剔除已经被删除的玩家
        {
            get
            {
                validPrivateContact.Clear();
                PrivateContact.Values.ToList().ForEach((Contact) => {
                    if (!Contact.delete)
                    {
                        validPrivateContact.Add(Contact);
                    }
                });
                return validPrivateContact;
            }
        }
        private Dictionary<long, PushMsgInfo> m_allMsg = new Dictionary<long, PushMsgInfo>();

        public Dictionary<long, PushMsgInfo> AllMsg
        {
            get => m_allMsg;
        }

        //联系人信息
        private Dictionary<long, ContactInfo> m_contactInfos = new Dictionary<long, ContactInfo>();

        //世界频道已读信息的最大唯一ID
        public long WorldMsgUniqueIndex;

        //联盟频道已读信息的最大唯一ID
        public long AllianceMsgUniqueIndex;

        private Dictionary<EnumChatChannel, long> m_msgReadUniqueIndex = new Dictionary<EnumChatChannel, long>();

        //私聊的消息
        public Dictionary<long, ChatMsgList> PrivateChat
        {
            get;
        } = new Dictionary<long, ChatMsgList>();

        //群聊的消息
        public Dictionary<long, ChatMsgList> GroupChat
        {
            get;
        } = new Dictionary<long, ChatMsgList>();

        private Dictionary<long, LocalCacheMsg> LocalMsgs;

        private bool m_hasNewMsg = false;

        public bool HasNewMsg
        {
            get { return m_hasNewMsg; }
            set
            {
                if (m_hasNewMsg && value == false)
                {
                    SaveLastReadMsgTime();
                }
                m_hasNewMsg = value;

            }
        }

        public bool MoveDownAfterReciveMsg { get => m_moveDownAfterReciveMsg; set => m_moveDownAfterReciveMsg = value; }
        public int ChannelWordLimit { get => channelWordLimit; }
        private PlayerProxy m_playerProxy;
        private NetProxy _netProxy;
        private AllianceProxy m_allianceProxy;
        private EmailProxy m_emailProxy;

        #endregion

        // Use this for initialization
        public ChatProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" ChatProxy register");
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            TimeStampInterval = config.timeStamp;
            ChatPageItemCount = config.channelPageLimit;
            channelWordLimit = config.channelWordLimit;
        }

        public override void OnRemove()
        {
            Debug.Log(" ChatProxy remove");
        }


        #region NetService
        private SprotoSocketAp chatClient;

        private ELoginState m_CrrNetState = ELoginState.EAuth1;

        private int connectIndex = 1;

        public void OnGameServerConnect()
        {
            ClearPrivateData();

            //请求私人消息
            QueryPrivateChatList();
        }

        public void OnConnection(Role_RoleLogin.response res)
        {
            _netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;

            ClearChatServerData();
            Debug.LogFormat("链接聊天服务器 user:[{0}]   pwd:[{1}] serverIP:「{2}」 serverPort:{3} serverNode :{4}", _netProxy.MUserName, res.chatSubId, res.chatServerIp, res.chatServerPort, res.chatServerName);

            if (chatClient == null)
            {
                chatClient = SprotoSocketAp.CreateInstance(res.chatServerIp, (int)res.chatServerPort, OnNetEvent,
                    OnAuthEvent);
            }
            chatClient.setLoginInfo(_netProxy.MUserName, res.chatSubId.ToString(), "ios", "cn", "", res.chatServerName);
        }

        private void OnAuthEvent(ELoginState state, int errorCode)
        {
            m_CrrNetState = state;
            AppFacade.GetInstance().SendNotification(CmdConstant.ChatClientAuthEvent, state, errorCode.ToString());

            CoreUtils.logService.Debug(state + "聊天服务器AuthEvent" + errorCode, Color.magenta);
        }

        public void OnNetEvent(NetEvent @event, int error)
        {

            CoreUtils.logService.Debug(@event + "聊天服务器AuthEvent" + error, Color.magenta);
            if (@event == NetEvent.ConnectComplete)
            {
                AuthToChatServer();
            }
            Task.RunInMainThread(() =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ChatClientNetEvent, @event, error.ToString());
            });
        }

        public void AuthToChatServer()
        {
            var _netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;

            string authToken = string.Format("{0}@{1}#{2}:{3}",
                Convert.ToBase64String(Encoding.Default.GetBytes(m_playerProxy.Rid.ToString())),
                Convert.ToBase64String(Encoding.Default.GetBytes(m_playerProxy.GetRoleLoginRes().chatServerName)),
                Convert.ToBase64String(Encoding.Default.GetBytes(m_playerProxy.GetRoleLoginRes().chatSubId.ToString())),
                (++connectIndex).ToString());
            authToken = string.Format("{0}:{1}", authToken,
                Convert.ToBase64String(Crypt.hmac64(Crypt.hashkey(authToken), _netProxy.netClient.des_key)));
            byte[] tk = Encoding.Default.GetBytes(authToken);
            chatClient.OnAuth(ELoginState.EChatAuth);
            CoreUtils.logService.Debug("聊天AuthToChatServer");
            chatClient.SendPackage(tk, 0, 0);
            chatClient.des_key = new byte[_netProxy.netClient.des_key.Length];
            Array.Copy(_netProxy.netClient.des_key, chatClient.des_key, _netProxy.netClient.des_key.Length);
        }

        private Timer reconnectTimer = null;
        public void OnSyncNetEvent(INotification notification)
        {
            NetEvent netEvent = (NetEvent)notification.Body;
            switch (netEvent)
            {
                case NetEvent.ReconnectFail:
                case NetEvent.AuthFail:
                case NetEvent.ConnectFail:
                    CoreUtils.logService.Debug("聊天服务器连接异常:" + netEvent);
                    reconnectTimer = Timer.Register(3f, () =>
                     {
                         OnConnection(m_playerProxy.GetRoleLoginRes());
                     });
                    break;
                case NetEvent.ConnectComplete:
                    if (reconnectTimer != null)
                    {
                        reconnectTimer.Cancel();
                        reconnectTimer = null;
                    }
                    break;
                default: break;
            }
        }

        public void SendSproto(SprotoTypeBase obj)
        {
            if (chatClient == null)
            {
                return;
            }
            chatClient.SendSproto(obj);
        }
        //请求私人消息列表
        public void QueryPrivateChatList()
        {
            Chat_Msg2GSQueryPrivateChatLst.request req = new Chat_Msg2GSQueryPrivateChatLst.request();
            AppFacade.GetInstance().SendSproto(req);
        }
        //请求与某个玩家的详细聊天记录
        public void QueryPrivateChatByRid(long rid)
        {
            if (!PrivateContact.ContainsKey(rid) || PrivateContact[rid].IsReceivedAllMsg())
            {
                return;
            }
            PrivateContact[rid].SetMsgQueryStatus(EnumChatInfoQueryStatus.Querying);
            Chat_Msg2GSQueryPrivateChatByRid.request req = new Chat_Msg2GSQueryPrivateChatByRid.request()
            {
                toRid = rid
            };
            AppFacade.GetInstance().SendSproto(req);
        }


        #endregion

        public void OnUpdateAllianceContact()
        {
            AllianceProxy allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (allianceProxy.HasJionAlliance())
            {
                if (AllianceContact == null)
                {
                    AllianceContact = new ChatContact(EnumChatChannel.alliance);
                    AddNewContact(AllianceContact);
                }
                else if (!AllContact.Contains(AllianceContact))
                {
                    AddNewContact(AllianceContact);
                }
            }
            else
            {
                int index = AllContact.FindIndex(i => i.channelType == EnumChatChannel.alliance);
                if (index >= 0)
                {
                    AllContact.RemoveAt(index);
                    AllianceContact = null;
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatContact);
                }
            }
        }

        /// <summary>
        /// 分享列表
        /// </summary>
        /// <returns></returns>
        public List<ChatContact> GetChatContactsBychatChannel(List<int> chatChannels)
        {
            List<ChatContact> chatContacts = new List<ChatContact>();
            for (int i = 0; i < chatChannels.Count; i++)
            {
                int chatChannel = chatChannels[i];
                if (chatChannel == 1)
                {
                    chatContacts.Add(WorldContact);
                }
                else if (chatChannel == 2)
                {
                    if (m_allianceProxy.HasJionAlliance())
                    {
                        chatContacts.Add(AllianceContact);
                    }

                }
                else if (chatChannel == 100)
                {
                    chatContacts.AddRange(ValidPrivateContact);
                }
            }
            return chatContacts;
        }
        //初始化私聊列表
        public void SetPrivateChatList(Chat_Msg2GSQueryPrivateChatLst.response res)
        {
            UpdateContactInfo(m_playerProxy.CurrentRoleInfo);
            if (res == null)
            {
                return;
            }
            long lastMsgTime = GetLastReadMsgTime();
            foreach (var targetInfo in res.lst)
            {
                if (!targetInfo.HasLastMsg)
                {
                    continue;
                }
                UpdateContactInfo(targetInfo);

                var contact = GetOrCreatePrivateContact(targetInfo.rid, EnumChatInfoQueryStatus.NotQuery);
                contact.InitContact((int)targetInfo.notReadCnt, targetInfo.lastReadTS);
                PrivateContact[targetInfo.rid].SetLastMsg(ChatRoleBriefToChatMsg(targetInfo));
                if (!HasNewMsg && targetInfo.lastMsgTS > lastMsgTime)
                {
                    HasNewMsg = true;
                }
                //本地最后已读时间大于服务器记录时间,请求所有消息,重新计算红点数量
                if (PrivateContact[targetInfo.rid].LastReadMessageTS > targetInfo.lastReadTS)
                {
                    QueryPrivateChatByRid(targetInfo.rid);
                    SendReadTimeStamp(targetInfo.rid);
                }
                if (isDeleteContact(contact))
                {
                    contact.delete = true;
                }
                else
                {
                    contact.delete = false;
                }
            //    Debug.LogError(contact.rid + " " + contact.delete);
            }
            //将本地缓存的聊天信息导入
            LocalMsgs = GetLocalMsg();

            foreach (var localMsg in LocalMsgs)
            {
                var contact = GetOrCreatePrivateContact(localMsg.Key);

                foreach (var msg in localMsg.Value.ChatMsgs)
                {
                    contact.PushMsg(msg, false);
                }

                if (!m_contactInfos.ContainsKey(localMsg.Key))
                {
                    m_contactInfos.Add(localMsg.Key, localMsg.Value.TargetInfo);
                }
            }

            SendNotification(CmdConstant.UpdateChatMsg);
        }
        //设置私聊详细信息
        public void SetPrivateChatInfos(Chat_Msg2GSQueryPrivateChatByRid.response res)
        {
            if (res == null)
            {
                return;
            }
            var rid = res.toRid;
            if (!PrivateContact.ContainsKey(rid))
            {
                return;
            }
            PrivateContact[rid].ClearData();
            PrivateContact[rid].SetMsgQueryStatus(EnumChatInfoQueryStatus.Queried);
            foreach (var chatInfo in res.lst)
            {
                PrivateContact[rid].PushMsg(ChatInfoToChatMsg(rid, chatInfo), false);
            }

            //导入本地缓存数据
            if (LocalMsgs.ContainsKey(rid))
            {
                foreach (var chatMsg in LocalMsgs[rid].ChatMsgs)
                {
                    PrivateContact[rid].PushMsg(chatMsg, false);
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatMsg);
        }
        /// <summary>
        /// 不足10秒间隔
        /// </summary>
        /// <returns></returns>
        public bool IsChatChannelInterval(EnumChatChannel channel, float timeInterval, bool ShowTip = true)
        {
            //间隔限制
            if (chatChannelInterval.ContainsKey((channel)))
            {
                float interval = Time.unscaledTime - chatChannelInterval[channel];
                if (interval < timeInterval)
                {
                    Tip.CreateTip(LanguageUtils.getTextFormat(750003, Mathf.CeilToInt(timeInterval - interval))).SetStyle(Tip.TipStyle.Middle).Show();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 刷新时间间隔
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="timeInterval"></param>
        /// <param name="ShowTip"></param>
        /// <returns></returns>
        public void SetChatChannelInterval(EnumChatChannel channel)
        {
            chatChannelInterval[channel] = Time.unscaledTime;
        }
        //获取联系人信息
        public ContactInfo GetContactInfo(long rid)
        {
            if (m_contactInfos.ContainsKey(rid))
            {
                return m_contactInfos[rid];
            }
            return null;
        }
        //处理系统推送消息
        public void OnProgressMsg(object body)
        {
            Chat_PushMsg.request chatSproto = body as Chat_PushMsg.request;
            if (chatSproto == null || !chatSproto.HasPushMsgInfos)
            {
                return;
            }
            for (int i = 0; i < chatSproto.pushMsgInfos.Count; i++)
            {
                if (chatSproto.pushMsgInfos[i] == null)
                {
                    continue;
                }
                UpdateContactInfo(chatSproto.pushMsgInfos[i]);
                OnDistributeMsg(chatSproto.pushMsgInfos[i]);
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatMsg, chatSproto.pushMsgInfos);
        }
        //检查是否已获取所有数据，未获取完全则向服务请求        
        public bool CheckContactMsgAllReceive(ChatContact contact)
        {
            if (contact.IsReceivedAllMsg())
            {
                return true;
            }

            switch (contact.channelType)
            {
                case EnumChatChannel.alliance:
                case EnumChatChannel.world:
                    QueryAWChannelMsg(contact);
                    break;
                case EnumChatChannel.privatechat:
                    QueryPrivateChatByRid(contact.rid);
                    break;
            }

            return false;
        }

        private void QueryAWChannelMsg(ChatContact contact)
        {
            Chat_GetSaveChatMsg.request req = new Chat_GetSaveChatMsg.request();
            req.page = contact.page;
            req.channelType = (long)contact.channelType;
            SendSproto(req);
        }
        //分发消息至各个频道
        private void OnDistributeMsg(PushMsgInfo req)
        {
            switch (req.channelType)
            {
                case (long)EnumChatChannel.world:
                    OnUpdateContactMsg(req);
                    break;
                case (long)EnumChatChannel.alliance:
                    OnUpdateContactMsg(req);
                    break;
                case (long)EnumChatChannel.lostland:
                    Debug.LogError("没有失落之地的频道");
                    break;
                case (long)EnumChatChannel.privatechat:
                    OnPrivateChatMsgRecived(req);
                    HasNewMsg = true;
                    break;
                case (long)EnumChatChannel.groupchat:
                    break;
                case (long)EnumChatChannel.marquee:
                    Debug.LogError("没有跑马灯的频道");
                    break;
                default:
                    Debug.LogError("没有该频道消息： " + req.channelType);
                    break;
            }
        }

        #region 联系人信息
        //更新或添加联系人信息
        public void UpdateContactInfo(PushMsgInfo msgInfo)
        {
            var contact = GetContactInfo(msgInfo.rid);
            if (contact == null)
            {
                m_contactInfos.Add(msgInfo.rid, new ContactInfo());
            }
            contact = m_contactInfos[msgInfo.rid];
            contact.SetInfo(msgInfo);
        }
        //更新或添加联系人信息
        public void UpdateContactInfo(ChatRoleBrief roleInfo)
        {
            var contact = GetContactInfo(roleInfo.rid);
            if (contact == null)
            {
                m_contactInfos.Add(roleInfo.rid, new ContactInfo());
            }
            contact = m_contactInfos[roleInfo.rid];
            contact.SetInfo(roleInfo);
        }
        //更新或添加联系人信息
        public void UpdateContactInfo(RoleInfoEntity roleInfoEntity)
        {
            var contact = GetContactInfo(roleInfoEntity.rid);
            if (contact == null)
            {
                m_contactInfos.Add(roleInfoEntity.rid, new ContactInfo());
            }
            contact = m_contactInfos[roleInfoEntity.rid];
            contact.SetInfo(roleInfoEntity);
        }
        //开启与目标角色的聊天界面(临时聊天,未发消息前不记录在私聊列表)
        public void BeginChat(RoleInfoEntity info)
        {
            UpdateContactInfo(info);
            ChatContact contact = GetOrCreatePrivateContact(info.rid);
            CoreUtils.uiManager.ShowUI(UI.s_chat, null, contact);
        }
        //获取聊天数据
        public ChatContact GetChatContact(EnumChatChannel channel, long rid = 0)
        {
            var contact = AllContact.Find(x => x.channelType == channel && x.rid == rid);
            return contact;
        }
        //获取私人聊天数据，未找到则创建
        public ChatContact GetOrCreatePrivateContact(long rid, EnumChatInfoQueryStatus queryStatus = EnumChatInfoQueryStatus.Queried)
        {
            var contact = GetChatContact(EnumChatChannel.privatechat, rid);
            if (contact == null)
            {
                contact = new ChatContact(EnumChatChannel.privatechat, EnumChannelRedDotType.TimeStamp, rid);
                contact.SetMsgQueryStatus(queryStatus);
                PrivateContact.Add(rid, contact);
                AddNewContact(contact);
            }
            return contact;
        }
        #endregion

        #region 聊天表情

        public Dictionary<int, List<ChatEmojiDefine>> GetEmojiGroup()
        {
            var group = new Dictionary<int, List<ChatEmojiDefine>>();
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            var emojiConfigs = CoreUtils.dataService.QueryRecords<ChatEmojiDefine>();
            for (int i = 1; i <= config.chatEmojiGroupNum; i++)
            {
                group.Add(i, new List<ChatEmojiDefine>());
            }

            foreach (var emojiConfig in emojiConfigs)
            {
                if (emojiConfig.@group <= config.chatEmojiGroupNum)
                {
                    group[emojiConfig.@group].Add(emojiConfig);
                }
            }

            return group;
        }

        public string GetEmojiStr(int emojiID)
        {
            return emojiPre + emojiID.ToString();
        }

        #endregion

        /// <summary>
        /// 聊天对象排序
        /// </summary>
        /// <param name="chatContact1"></param>
        /// <param name="chatContact2"></param>
        /// <returns></returns>
        public int SortContact(ChatContact chatContact1, ChatContact chatContact2)
        {
            if (chatContact1.IsHide())
            {
                return 1;
            }
            else if (chatContact2.IsHide())
            {
                return -1;
            }

            if (chatContact1.isTop == chatContact2.isTop)
            {
                    if (chatContact1.lastMsg == null && chatContact2.lastMsg == null)
                    {
                        return 0;
                    }
                    else if (chatContact1.lastMsg == null)
                    {
                        return 1;
                    }
                    else if (chatContact2.lastMsg == null)
                    {
                        return -1;
                    }
                    else
                    {
                        return chatContact2.lastMsg.timeStamp.CompareTo(chatContact1.lastMsg.timeStamp);
                    }
            }
            else if (chatContact1.isTop && !chatContact2.isTop)
            {
                return -1;
            }
            else if (!chatContact1.isTop && chatContact2.isTop)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #region 置顶,取消置顶，删除
        List<long> topList = new List<long>();//聊天置顶列表
        List<long> untopList = new List<long>();//聊天取消置顶列表
        Dictionary<long,long> deleteDic = new Dictionary<long, long>();//聊天删除列表
        public void LoadTopList()
        {
            topList.Clear();
            string key = string.Format("ChatTopList{0}", m_playerProxy.CurrentRoleInfo.rid);
            string s_topList = PlayerPrefs.GetString(key, "");

            string[] strArr = s_topList.Split('|');
            for (int i = 0; i < strArr.Length; i++)
            {
                long rid = 0;
                if (long.TryParse(strArr[i], out rid))
                {
                    if (!topList.Contains(rid))
                    {
                        topList.Add(rid);
                    }
                }
            }
        }
        public void LoadUnTopList()
        {
            untopList.Clear();
            string key = string.Format("ChatUntopList{0}", m_playerProxy.CurrentRoleInfo.rid);
            string s_untopList = PlayerPrefs.GetString(key, "");

            string[] strArr = s_untopList.Split('|');
            for (int i = 0; i < strArr.Length; i++)
            {
                long rid = 0;
                if (long.TryParse(strArr[i], out rid))
                {
                    if (!untopList.Contains(rid))
                    {
                        untopList.Add(rid);
                    }
                }
            }
        }
        public void InitTopChatContact(ChatContact chatContact)
        {
            switch (chatContact.channelType)
            {
                case EnumChatChannel.privatechat:
                    {
                        if (topList.Contains(chatContact.rid))
                        {
                            chatContact.isTop = true;
                        }
                    }
                    break;
                case EnumChatChannel.world:
                case EnumChatChannel.alliance:
                case EnumChatChannel.lostland:
                    if (topList.Contains((long)chatContact.channelType))
                    {
                        chatContact.isTop = true;
                    }
                    else
                    {
                        if (!untopList.Contains((long)chatContact.channelType))
                        {
                            chatContact.isTop = true;
                        }
                    }
                    break;
                default:
                    if (topList.Contains((long)chatContact.channelType))
                    {
                        chatContact.isTop = true;
                    }

                    break;
            }
        }
        public void LoadDeleteList()
        {
            deleteDic.Clear();
            string key = string.Format("ChatDeleteList{0}", m_playerProxy.CurrentRoleInfo.rid);
            string s_deleteDic = PlayerPrefs.GetString(key, "");

            string[] strArr = s_deleteDic.Split(',','|');
            for (int i = 0; i < strArr.Length; i+=2)
            {
                long rid = 0;
                long timestart = 0;
                if (long.TryParse(strArr[i], out rid))
                {
                    if (long.TryParse(strArr[i+1], out timestart))
                    {
                            deleteDic[rid] = timestart;
                    }  
                }
            }
      //      Debug.LogError(s_deleteDic);
        }
        public bool isDeleteContact(ChatContact chatContact)
        {
            bool delete = false;
            if (deleteDic.ContainsKey(chatContact.rid))
            {
                if (chatContact.lastMsg != null)
                {
                    if (deleteDic[chatContact.rid] >= chatContact.lastMsg.timeStamp)
                    {
                        delete = true;
                    }
                }
            }
            return delete;
        }
        public void SaveDeleteDic()
        {
            string key = string.Format("ChatDeleteList{0}", m_playerProxy.CurrentRoleInfo.rid);
            StringBuilder s_deleteDic = new StringBuilder();
            foreach (var delete in deleteDic)
            {
                s_deleteDic.Append(delete.Key + "|"+ delete.Value + ",");
            }
            PlayerPrefs.SetString(key, s_deleteDic.ToString());
        }
        public void SaveUntopList()
        {
            string key = string.Format("ChatUntopList{0}", m_playerProxy.CurrentRoleInfo.rid);
            StringBuilder s_untopList = new StringBuilder();
            for (int i = 0; i < untopList.Count; i++)
            {
                long rid = 0;
                s_untopList.Append(topList[i] + "|");
            }
            PlayerPrefs.SetString(key, s_untopList.ToString());
        }
        public void SaveTopList()
        {
            string key = string.Format("ChatTopList{0}", m_playerProxy.CurrentRoleInfo.rid);
            StringBuilder s_topList = new StringBuilder();
            for (int i = 0; i < topList.Count; i++)
            {
                long rid = 0;
                s_topList.Append(topList[i] + "|");
            }
              PlayerPrefs.SetString(key, s_topList.ToString());
        }
        public void TopContact(ChatContact chatContact)
        {
            chatContact.isTop = true;
            switch (chatContact.channelType)
            {
                case EnumChatChannel.privatechat:
                    {
                        if (!topList.Contains(chatContact.rid))
                        {
                            topList.Add(chatContact.rid);
                        }
                        if (untopList.Contains(chatContact.rid))
                        {
                            untopList.Remove(chatContact.rid);
                        }
                    }
                    break;
                 default:
                    if (!topList.Contains((long)chatContact.channelType))
                    {
                        topList.Add((long)chatContact.channelType);
                    }
                    if (untopList.Contains((long)chatContact.channelType))
                    {
                        untopList.Remove((long)chatContact.channelType);
                    }
                    break;
            }
            SaveTopList();
            SaveUntopList();
        }
        public void UnTopContact(ChatContact chatContact)
        {
            chatContact.isTop = false;
            switch (chatContact.channelType)
            {
                case EnumChatChannel.privatechat:
                    {
                        if (topList.Contains(chatContact.rid))
                        {
                            topList.Remove(chatContact.rid);
                        }
                        if (!untopList.Contains(chatContact.rid))
                        {
                            untopList.Add(chatContact.rid);
                        }
                    }
                    break;
                default:
                    {
                        if (topList.Contains((long)chatContact.channelType))
                        {
                            topList.Remove((long)chatContact.channelType);
                        }
                        if (!untopList.Contains(chatContact.rid))
                        {
                            untopList.Add(chatContact.rid);
                        }
                    }
                    break;
            }
            SaveTopList();
            SaveUntopList();
        }

        #endregion
        //在线推送私聊消息的处理
        public int OnPrivateChatMsgRecived(PushMsgInfo req)
        {
            var chatMsg = PushMsgInfoToChatMsg(req);
            var rid = req.toRid;
            if (rid == m_playerProxy.Rid)
            {
                rid = req.rid;
            }
            if (PrivateContact.ContainsKey(rid))
            {
                PrivateContact[rid].PushMsg(chatMsg);
            }
            else
            {
                var contact = AllContact.Find(x => x.rid == rid);
                if (contact == null)
                {
                    PrivateContact.Add(rid,
                        new ChatContact(EnumChatChannel.privatechat, EnumChannelRedDotType.TimeStamp, rid));
                    AddNewContact(PrivateContact[rid]);
                }
                PrivateContact[rid].PushMsg(chatMsg);
            }
            if (isDeleteContact(PrivateContact[rid]))
            {
                PrivateContact[rid].delete = true;
            }
            else
            {
                PrivateContact[rid].delete = false;
            }
            return 0;
        }


        //更新联盟与世界聊天信息
        public void OnUpdateContactMsg(PushMsgInfo req)
        {
            switch (req.channelType)
            {
                case (long)EnumChatChannel.world:
                    InitWorldContact();
                    WorldContact.PushMsg(PushMsgInfoToChatMsg(req));
                    break;
                case (long)EnumChatChannel.alliance:
                    InitAllianceContact();
                    AllianceContact.PushMsg(PushMsgInfoToChatMsg(req));
                    break;
                default:
                    Debug.LogError("暂未添加其他频道");
                    break;
            }

        }
        // 添加新联系人
        public void AddNewContact(ChatContact contact)
        {
            if (!AllContact.Contains(contact))
            {
   //             Debug.LogError(contact.channelType);
                InitTopChatContact(contact);
                    AllContact.Add(contact);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatContact);
            }
        }
        public void RemoveContact(ChatContact contact)
        {
            switch (contact.channelType)
            {
                case EnumChatChannel.alliance:
                    contact.ClearData();
                    break;
                case EnumChatChannel.privatechat:
                    contact.delete = true;
                    AllContact.Remove(contact);
                    PrivateContact.Remove(contact.rid);
                    if (contact.lastMsg != null)
                    {
                        deleteDic[contact.rid] = contact.lastMsg.timeStamp;
                        SaveDeleteDic();
                    }
                    break;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.DeleteChatContact);
        }

        //发送免打扰信息
        public void SendChatNoDisturb(ChatContact contact)
        {
            //发送免打扰协议
            Chat_SendChatMsgNoDisturb.request req = new Chat_SendChatMsgNoDisturb.request();
            req.chatNoDisturbInfo = new Dictionary<long, ChatNoDisturbInfo>();
            req.chatNoDisturbInfo.Add(contact.GetNoDisturbKey(), new ChatNoDisturbInfo { channelType = (long)contact.channelType, chatNoDisturbFlag = contact.noDisturb });
            AppFacade.GetInstance().SendSproto(req);
        }

        //发送已读记录
        public void SendReadRecord(ChatContact contact)
        {
            if (contact.rid == 0)
            {
                //联盟与世界频道记录
                OnSendReadUniqueIndex(contact);
            }
            else
            {
                //私聊记录
                SendReadTimeStamp(contact.rid);
            }
        }

        //发送已读的索引
        public void OnSendReadUniqueIndex(ChatContact contact)
        {
            if (!m_msgReadUniqueIndex.ContainsKey(contact.channelType) ||
                m_msgReadUniqueIndex[contact.channelType] >= contact.MaxMsgUniqueIndex)
            {
                return;
            }
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy.GetRoleLoginRes() != null)
            {
                Chat_SendMaxUniqueIndex.request req = new Chat_SendMaxUniqueIndex.request();
                req.chatReadInfo = new Dictionary<long, ChatReadedUniqueIndex>();
                    AllContact.ForEach((tempContact) => {
                        if (tempContact.rid == 0)
                        {
                            ChatReadedUniqueIndex tmp = new ChatReadedUniqueIndex();
                            tmp.channelType = (long)tempContact.channelType;
                            tmp.uniqueIndex = tempContact.MaxMsgUniqueIndex;
                            req.chatReadInfo.Add(tmp.channelType, tmp);
                        }
                    });
                AppFacade.GetInstance().SendSproto(req);
                m_msgReadUniqueIndex[contact.channelType] = contact.MaxMsgUniqueIndex;
            }
        }
        //发送当前已读时间戳
        public void SendReadTimeStamp(long rid)
        {
            if (!PrivateContact.ContainsKey(rid) || PrivateContact[rid].lastMsg == null || PrivateContact[rid].serverLastReadMessageTS >= PrivateContact[rid].lastMsg.timeStamp)
            {
                return;
            }

            var curTime = ServerTimeModule.Instance.GetServerTime();
            Chat_Msg2GSReadPrivateChat.request req = new Chat_Msg2GSReadPrivateChat.request()
            {
                toRid = rid,
                tsLastRead = curTime,
            };
            AppFacade.GetInstance().SendSproto(req);
            PrivateContact[rid].serverLastReadMessageTS = curTime;
        }

        public void InitChatChannel()
        {
            InitWorldContact();
            InitAllianceContact();
            AddNewContact(WorldContact);
            AllianceProxy allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (allianceProxy.HasJionAlliance() )
            {
                AddNewContact(AllianceContact);
            }
        }

        //记录玩家发送失败的消息,储存在本地数据
        public void SaveLocalErrorMsg(ChatContact contact, string errorMsg, string msg, long timeStamp)
        {
            if (!LocalMsgs.ContainsKey(contact.rid)) {
                LocalMsgs.Add(contact.rid, new LocalCacheMsg());
                LocalMsgs[contact.rid].TargetInfo = GetContactInfo(contact.rid);
            }
            var errorChatMsg = new ChatMsg();
            var myChatMsg = new ChatMsg();
            errorChatMsg.SetChatMsg(0, errorMsg, EnumMsgType.Notice, timeStamp);
            errorChatMsg.bLocalData = true;
            //本地存储的消息只能角色自身发送
            myChatMsg.SetChatMsg(m_playerProxy.Rid, msg, EnumMsgType.Text, timeStamp);
            myChatMsg.bLocalData = true;

            contact.PushMsg(errorChatMsg, false);
            contact.PushMsg(myChatMsg, false);

            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateChatMsg);

            LocalMsgs[contact.rid].ChatMsgs.Add(errorChatMsg);
            LocalMsgs[contact.rid].ChatMsgs.Add(myChatMsg);
            string info = JsonMapper.ToJson(LocalMsgs);
            PlayerPrefs.SetString(m_playerProxy.Rid.ToString() + "LocalChatMsg", info);
        }
        //读取本地缓存的发送失败数据
        private Dictionary<long, LocalCacheMsg> GetLocalMsg()
        {
            string info = PlayerPrefs.GetString(m_playerProxy.Rid.ToString() + "LocalChatMsg");
            if (string.IsNullOrEmpty(info))
            {
                return new Dictionary<long, LocalCacheMsg>();
            }
            else
            {
                var cache = JsonMapper.ToObject<Dictionary<string, LocalCacheMsg>>(info);
                var msgs = new Dictionary<long, LocalCacheMsg>();
                foreach (var msg in cache)
                {
                    msgs.Add(long.Parse(msg.Key), msg.Value);
                }
                return msgs;
            }
        }
        #region 发送相关
        public void SendMsgMapMarkerType(string msg, int MapMarkerType, ChatContact m_currentContact )
        {
                //替换脏字
              Client.Utils.BannedWord.CheckChatBannedWord(msg);
            StringBuilder sb = new StringBuilder();
            string atuser = string.Format(s_mapMarkerType, MapMarkerType);
            string desc = string.Format(s_desc, msg);
            sb.Append(atuser).Append(desc);
            OnSendMsgSproto(sb.ToString(), m_currentContact,-1);
        }

        /// <summary>
        /// 字符限制
        /// </summary>
        /// <param name="str"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private bool CheckChannelWordLimit(string content,  bool autoPopTip = true)
        {
            bool pass = true;
            if (content.Length > channelWordLimit)
            {
                pass = false;
                if (autoPopTip)
                {
                    Tip.CreateTip(750005).SetStyle(Tip.TipStyle.Middle).Show();
                }
            }
            return pass;
        }
        /// <summary>
        /// 禁言
        /// </summary>
        /// <returns></returns>
        public bool CheckSilence(bool autoPopTip = true)
        {
            bool pass = true;
            if (m_playerProxy.CurrentRoleInfo.silence == -1) //永久禁言
            {
                pass = false;
                if (autoPopTip)
                {
                    Alert.CreateAlert(100302, LanguageUtils.getText(730184)).SetRightButton(null, LanguageUtils.getText(730060)).Show();
                }
            }
            else
            {
                //限时禁言
                if (m_playerProxy.CurrentRoleInfo.silence > 0 && m_playerProxy.CurrentRoleInfo.silence > ServerTimeModule.Instance.GetServerTime())
                {
                    pass = false;
                    if (autoPopTip)
                    {
                        DateTime time = ServerTimeModule.Instance.ConverToServerDateTime(m_playerProxy.CurrentRoleInfo.silence);
                        string str = LanguageUtils.getTextFormat(100301, time.ToString("yyyy/MM/dd HH:mm:ss"));
                        Alert.CreateAlert(str, LanguageUtils.getText(730184)).SetRightButton(null, LanguageUtils.getText(730060)).Show();
                    }
                }
            }
            return pass;
        }
        //发言间隔
        public bool CheckChannelInterva(ChatContact m_currentContact, bool autoPopTip = true)
        {
            bool pass = true;
            EnumChatChannel channel = m_currentContact.channelType;
            ChatChannelDefine define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)m_currentContact.channelType);
            float tempValue = 0;
            //间隔限制
            if (ContainsKeyChatChannelInterva(channel, out tempValue))
            {
                float interval = Time.unscaledTime - tempValue;
                if (interval < define.timeInterval)
                {
                    if (autoPopTip)
                    {
                        Tip.CreateTip(LanguageUtils.getTextFormat(750003, Mathf.CeilToInt(define.timeInterval - interval))).SetStyle(Tip.TipStyle.Middle).Show();
                    }
                    pass = false;
                }
            }
            return pass;
        }  
        //等级限制
        public bool CheckLvLimit(string msg, ChatContact m_currentContact, bool autoPopTip = true)
        {
            bool pass = true;
            EnumChatChannel channel = m_currentContact.channelType;
            ChatChannelDefine define = CoreUtils.dataService.QueryRecord<ChatChannelDefine>((int)m_currentContact.channelType);

            if (m_playerProxy.CurrentRoleInfo.level < define.lvLimit)
            {
                pass = false;
                if (m_currentContact.channelType == EnumChatChannel.privatechat)
                {
                    RefreshChatChannelInterval(channel, Time.unscaledTime);
                    MoveDownAfterReciveMsg = true;
                    var time = ServerTimeModule.Instance.GetServerTime();
                    SaveLocalErrorMsg(m_currentContact, LanguageUtils.getTextFormat(750002, define.lvLimit), msg, time);
                }
                else
                {
                    if (autoPopTip)
                    {
                        Tip.CreateTip(LanguageUtils.getTextFormat(750001, define.lvLimit)).SetStyle(Tip.TipStyle.Middle).Show();
                    }
                }
            }
            return pass;
        }
        public bool CheckAliance(ChatContact chatContact ,bool autoPopTip = true)
        {
            bool pass = true;
            if (chatContact != null)
            {
                if (chatContact.channelType == EnumChatChannel.alliance)
                {
                    if (!m_allianceProxy.HasJionAlliance())
                    {
                        pass = false;
                        if (autoPopTip)
                        {
                            Tip.CreateTip(LanguageUtils.getText(733389)).SetStyle(Tip.TipStyle.Middle).Show();
                        }
                    }
                }
            }
            return pass;
        }

        public bool CheckMemberJurisdiction(ChatContact chatContact, bool autoPopTip = true)
        {
            bool pass = true;
            if (chatContact != null)
            {
                if (chatContact.channelType == EnumChatChannel.alliance)
                {
                        if (!m_allianceProxy.GetSelfRoot(GuildRoot.chat))
                        {
                            pass = false;
                            if (autoPopTip)
                            {
                                Tip.CreateTip(LanguageUtils.getText(730136)).SetStyle(Tip.TipStyle.Middle).Show();
                            }
                        }           
                    }
            }
            return pass;
        }
        public bool CheckMyMsgCount(string msg, ChatContact m_currentContact, EnumMsgType msgType, bool autoPopTip = true)
        {
            bool pass = true;
            int myMsgCount = 0;
            myMsgCount = MyMsgCount(m_currentContact,  msgType, msg);
            if (myMsgCount >= 3)
            {
                pass = false;
                if (autoPopTip)
                {
                    Tip.CreateTip(750004).SetStyle(Tip.TipStyle.Middle).Show();
                }
            }
            return pass;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m_currentContact"></param>
        /// <param name="action"></param>
        public void SendMsg(string msg, ChatContact m_currentContact, EnumMsgType enumMsgType, Action<string> action = null)
        {
            string lbl_input = "";
            if (string.IsNullOrEmpty(msg) || msg.Trim() == "")
            {
                return;
            }
            if (!CheckAliance(m_currentContact))
            {
                return;
            }
            if (!CheckMemberJurisdiction(m_currentContact))
            {
                return;
            }
            
            if (!CheckSilence())
            {
                return;
            }

            if (!CheckChannelInterva(m_currentContact))
            {
                return;
            }

            if (!CheckLvLimit(msg, m_currentContact))
            {
                return;
            }
            if (!CheckMyMsgCount(msg, m_currentContact, EnumMsgType.Text))
            {
                return;
            }
      
            if (enumMsgType == EnumMsgType.Text)
            { 
                //字符限制
                if (msg.Length > ChannelWordLimit)
                {
                    Tip.CreateTip(750005).SetStyle(Tip.TipStyle.Middle).Show();
                    return;
                }
                action?.Invoke(string.Empty);
                //替换脏字
                Client.Utils.BannedWord.CheckChatBannedWord(msg);
            }
            if (enumMsgType == EnumMsgType.ATUser)
            {
                StringBuilder sb = new StringBuilder();
                string atuser = string.Format(s_mapMarkerType, 0);
                string desc = string.Format(s_desc, msg);
                sb.Append(atuser).Append(desc);
                OnSendMsgSproto(sb.ToString(), m_currentContact);
            }
            else
            {
                OnSendMsgSproto(msg, m_currentContact);
            }


        }


        public void OnSendMsgSproto(string content, ChatContact chatContact, long  notifyRid = 0)
        {
            bool personal = false;
            if (chatContact.rid > 0)
            {
                personal = true;
            }
            MoveDownAfterReciveMsg = true;
            if (personal)
            {
                Chat_SendPrivateMsg.request msg = new Chat_SendPrivateMsg.request();
                msg.toRid = chatContact.rid;
                msg.msgContent = content;
                msg.gameNode = m_playerProxy.GetRoleLoginRes().chatServerName;
                AppFacade.GetInstance().SendSproto(msg);
            }
            else
            {
                Chat_SendMsg.request msg = new Chat_SendMsg.request();
                msg.channelType = (long)chatContact.channelType;
                msg.msgContent = content;
                if (notifyRid != 0)
                {
                msg.notifyRid = notifyRid;
                }
                SendSproto(msg);
                if (chatContact.channelType == EnumChatChannel.world)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.WorldChat));
                }
            }

            EnumChatChannel channel = chatContact.channelType;
            SetChatChannelInterval(channel);
        }

        /// <summary>
        /// 类型为emoji的传emojiid 同一条消息不能发送超出3
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private int MyMsgCount(ChatContact m_currentContact ,EnumMsgType msgType, string msg)
        {
            int count = 0;
            for (int i = m_currentContact.ChatMsgList.ChatMsg.Count - 1; i >= 0; i--)
            {
                if (count >= 3)
                {
                    break;
                }
                if (m_currentContact.ChatMsgList.ChatMsg[i].rid == m_playerProxy.Rid && m_currentContact.ChatMsgList.ChatMsg[i].msgType == msgType)
                {
                    if (m_currentContact.ChatMsgList.ChatMsg[i].msg != null && string.Equals(msg, m_currentContact.ChatMsgList.ChatMsg[i].msg))
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

        #endregion

        private void InitWorldContact()
        {
            if(WorldContact==null)
            {
                WorldContact = new ChatContact(EnumChatChannel.world);
                m_msgReadUniqueIndex.Add(EnumChatChannel.world,WorldContact.MaxMsgUniqueIndex);
            }
        }

        private void InitAllianceContact()
        {
            if (AllianceContact == null)
            {
                AllianceContact = new ChatContact(EnumChatChannel.alliance);
                m_msgReadUniqueIndex[EnumChatChannel.alliance] = AllianceContact.MaxMsgUniqueIndex;
            }
        }

        private ChatMsg ChatRoleBriefToChatMsg(ChatRoleBrief info)
        {
            if (string.IsNullOrEmpty(info.lastMsg))
            {
                return null;
            }
            var chatMsg = new ChatMsg();
            chatMsg.SetChatMsg(info);
            return chatMsg;
        }

        private ChatMsg ChatInfoToChatMsg(long targetRid,ChatInfo info)
        {
            var chatMsg = new ChatMsg();
            //toRid字段置为发送方Rid
            if (targetRid != info.toRid)
            {
                info.toRid = targetRid;
            }
            else
            {
                info.toRid = m_playerProxy.Rid;
            }
            chatMsg.SetChatMsg(info);
            return chatMsg;
        }

        private ChatMsg PushMsgInfoToChatMsg(PushMsgInfo info)
        {
            var chatMsg = new ChatMsg();
            chatMsg.SetChatMsg(info);
            return chatMsg;
        }
        //读取最后进入聊天界面的时间
        private long GetLastReadMsgTime()
        {
            return PlayerPrefs.GetInt(LastReadMsgTime + m_playerProxy.Rid.ToString());
        }
        //保存最后进入聊天界面的时间
        private void SaveLastReadMsgTime()
        {
            PlayerPrefs.SetInt(LastReadMsgTime + m_playerProxy.Rid.ToString(),(int)ServerTimeModule.Instance.GetServerTime());
        }
        #region 聊天间隔
        /// <summary>
        /// 刷新聊天间隔
        /// </summary>
        /// <param name=""></param>
        public void RefreshChatChannelInterval(EnumChatChannel channel ,float unscaledTime)
        {
            chatChannelInterval[channel] = unscaledTime;
        }
        public bool ContainsKeyChatChannelInterva(EnumChatChannel channel, out float unscaledTime)
        {
            return chatChannelInterval.TryGetValue(channel, out unscaledTime);
        }
        #endregion
        private void ClearPrivateData()
        {
            foreach (var chatContact in PrivateContact.Values)
            {
                chatContact.ClearData();
            }
        }

        private void ClearChatServerData()
        {
            WorldContact?.ClearData();
            AllianceContact?.ClearData();
        }
        #region AT解析
        public const string s_mapMarkerType = @"<mapMarkerType={0}>";
        public bool ConvertStringToChatviewMapMarkerType(ChatMsg msg, out string lbl_dec)
        {
            bool convert = true;
            int mapMarkerType = 0;
            string desc = "";
            lbl_dec = "";
            {
                var match = Regex.Match(msg.msg, mapMarkerTypePattern);
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value,out mapMarkerType);
                }
            }
            {
                var match = Regex.Match(msg.msg, descPattern);
                if (match.Success)
                {
                    desc = match.Groups[1].Value;
                }
            }

            string arg0 = "", arg1 = "";
            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>(mapMarkerType);
            if (mapMarkerTypeDefine != null)
            {
                convert = true;
                if (string.IsNullOrEmpty(desc))
                {
                    lbl_dec = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage,"  ");
                    msg.mapMarkerTypemsg = " ";
                }
                else
                {
                    lbl_dec = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, desc);
                    msg.mapMarkerTypemsg = desc;
                }
            }
            else
            {
                convert = false;
            }
            return convert;
        }
        /// <summary>
        /// 联系人列表显示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lbl_dec"></param>
        /// <returns></returns>
        public bool ConvertStringToChatListviewMapMarkerType(ChatMsg msg, out string lbl_dec)
        {
            bool convert = true;
            int mapMarkerType = 0;
            string desc = "";
            lbl_dec = "";
            {
                var match = Regex.Match(msg.msg, mapMarkerTypePattern);
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out mapMarkerType);
                }
            }
            {
                var match = Regex.Match(msg.msg, descPattern);
                if (match.Success)
                {
                    desc = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>(mapMarkerType);
            if (mapMarkerTypeDefine != null)
            {
                convert = true;
                if (string.IsNullOrEmpty(desc))
                {
                    lbl_dec = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, "  ");
                }
                else
                {
                    lbl_dec  = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, desc);
                }
            }
            else
            {
                convert = false;
            }
            return convert;
        }
        public bool ConvertStringToMainViewMapMarkerType(ChatMsg msg, out string lbl_dec)
        {
            bool convert = true;
            int mapMarkerType = 0;
            string desc = "";
            lbl_dec = "";
            {
                var match = Regex.Match(msg.msg, mapMarkerTypePattern);
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out mapMarkerType);
                }
            }
            {
                var match = Regex.Match(msg.msg, descPattern);
                if (match.Success)
                {
                    desc = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>(mapMarkerType);
            if (mapMarkerTypeDefine != null)
            {
                convert = true;
                if (string.IsNullOrEmpty(desc))
                {
                    lbl_dec = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, "  ");//
                }
                else
                {
                    lbl_dec = LanguageUtils.getTextFormat(mapMarkerTypeDefine.chatMessage, desc);
                }
             //   lbl_dec = "缺 " + lbl_dec;
            }
            else
            {
                convert = false;
            }
            return convert;
        }
        #endregion
        #region 分享解析
        public const string s_chatShareId = @"<chatShare={0}>";
        public const string s_guildAbbName = @"<guildAbbName={0}>";
        public const string s_coordinate = @"<coordinateK={0},{1},{2}>";
        public const string s_cityName= @"<cityName={0}>";
        public const string s_resourceBuild = @"<resourceBuild={0}>";
        public const string s_strongHold = @"<strongHold={0}>";
        public const string s_allianceBuild = @"<allianceBuild={0}>";
        public const string s_monster = @"<monster={0}>";
        public const string s_cityRid = @"<cityRid={0}>";
        public const string s_desc = @"<desc={0}>";
        public const string s_rune = @"<rune={0}>";
        
        private string ConvertContactToString10( ChatShareData chatShareData, out string msgContent  )
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string guildAbbName = string.Format(s_guildAbbName, chatShareData.guildAbbName);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string cityName = string.Format(s_cityName, chatShareData.cityName);
            sb.Append(chatShareId).Append(guildAbbName).Append(coordinate).Append(cityName);
            string arg0 = "",shareName = "";
            if (string.IsNullOrEmpty(chatShareData.guildAbbName))
            {
                arg0 = chatShareData.cityName;
            }
            else
            {
                arg0 = LanguageUtils.getTextFormat(300030, chatShareData.guildAbbName, chatShareData.cityName);
            }
            shareName = arg0;

            lbl_share = ConvertlblShare(langStr, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString11(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string resourceBuild = string.Format(s_resourceBuild, chatShareData.resourceBuild);
            sb.Append(chatShareId).Append(coordinate).Append(resourceBuild);
            ResourceGatherTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(chatShareData.resourceBuild);
            if (resourceGatherTypeDefine != null)
            {
                string arg0 = "", shareName = "";
                arg0 = chatShareData.resourceBuild.ToString();
                shareName = m_emailProxy.FormatResourceBuildName(arg0);
                lbl_share = ConvertlblShare(langStr, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            }
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string  ConvertlblShare(string langStr, string shareName, string arg0, string gameNode, int x, int y)
        {
            string arg1 = string.Format("{0},{1},{2}", gameNode, x, y);

            string output  = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            if (ArabicFixer.IsRtl(shareName) && !LanguageUtils.IsArabic())
            {
                output = string.Format("({3}:Y,{2}:X,{1}#) {0}", shareName, gameNode, x, y);
            }
            else if (ArabicFixer.IsRtl(shareName) && LanguageUtils.IsArabic())
            {
                output = string.Format("({3}:Y,{2}:X,{1}#) {0}", shareName, gameNode, x, y);
            }
            return output;
        }
        private string ConvertContactToString12(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string guildAbbName = string.Format(s_guildAbbName, chatShareData.guildAbbName);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string strongHold = string.Format(s_strongHold, chatShareData.strongHold);
            sb.Append(chatShareId).Append(guildAbbName).Append(coordinate).Append(strongHold);

            StrongHoldDataDefine dataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)chatShareData.strongHold);

            if (dataDefine != null)
            {
                StrongHoldTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(dataDefine.type);
                if (resourceGatherTypeDefine != null)
                {
                    string arg0 = "", shareName = "";
                    if (string.IsNullOrEmpty(chatShareData.guildAbbName))
                    {
                        arg0 = chatShareData.strongHold.ToString();
                    }
                    else
                    {
                        arg0 = string.Format("{0},{1}", chatShareData.guildAbbName, chatShareData.strongHold);
                    }
                    shareName = m_emailProxy.FormatStrongHold(arg0);
                    lbl_share = ConvertlblShare(langStr, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
                }
            }
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString13(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string guildAbbName = string.Format(s_guildAbbName, chatShareData.guildAbbName);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string allianceBuild = string.Format(s_allianceBuild, chatShareData.allianceBuild);
            sb.Append(chatShareId).Append(guildAbbName).Append(coordinate).Append(allianceBuild);

            AllianceBuildingTypeDefine buildingDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)chatShareData.allianceBuild);

            if (buildingDefine != null)
            {
                string arg0 = "", shareName = "";
                if (string.IsNullOrEmpty(chatShareData.guildAbbName))
                {
                    arg0 = chatShareData.allianceBuild.ToString();
                }
                else
                {
                    arg0 = string.Format("{0},{1}", chatShareData.guildAbbName, chatShareData.allianceBuild);
                }
                shareName = m_emailProxy.FormatAllianceBuildName (arg0);
                lbl_share = ConvertlblShare(langStr, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            }
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString14(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string monster = string.Format(s_monster, chatShareData.monster);
            sb.Append(chatShareId).Append(coordinate).Append(monster);

            MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>(chatShareData.monster);
            if (monsterDefine != null)
            {
                string arg0 = "", shareName = "";
                arg0 = chatShareData.monster.ToString();
                shareName = m_emailProxy.FormatMonsterName (arg0);
                lbl_share = ConvertlblShare(langStr, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            }
            msgContent = sb.ToString(); 
            return lbl_share;
        }
        private string ConvertContactToString15(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string rune = string.Format(s_rune, chatShareData.rune);
            sb.Append(chatShareId).Append(coordinate).Append(rune);

            MapItemTypeDefine mapItemTypeDefine = CoreUtils.dataService.QueryRecord<MapItemTypeDefine>(chatShareData.rune);
            if (mapItemTypeDefine != null)
            {
                string arg0 = "", shareName = "";
                arg0 = chatShareData.rune.ToString();
                shareName = m_emailProxy.FormatMapItemTypeName(arg0);
                lbl_share = ConvertlblShare(langStr, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            }
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString20(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string guildAbbName = string.Format(s_guildAbbName, chatShareData.guildAbbName);
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string cityName = string.Format(s_cityName, chatShareData.cityName);
            string cityRid = string.Format(s_cityRid, chatShareData.cityRid);
            sb.Append(chatShareId).Append(guildAbbName).Append(coordinate).Append(cityRid).Append(cityName);
            msgContent = sb.ToString(); 
            return lbl_share;
        }
        private string ConvertContactToString30(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString31(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            msgContent = sb.ToString(); 
            return lbl_share;
        }
        private string ConvertContactToString32(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString40(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            msgContent = sb.ToString();
            return lbl_share;
        }
        private string ConvertContactToString99(ChatShareData chatShareData, out string msgContent)
        {
            string lbl_share = "";
            string langStr = LanguageUtils.getText(chatShareData.ChatShareDefine.l_desID);
            StringBuilder sb = new StringBuilder();
            string chatShareId = string.Format(s_chatShareId, chatShareData.ChatShareDefine.ID);
            string coordinate = string.Format(s_coordinate, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            string desc = string.Format(s_desc, chatShareData.desc);
            sb.Append(chatShareId).Append(coordinate).Append(desc);

            string arg0 = "", arg1 = "",shareName ="";
            arg0 = chatShareData.desc;
            shareName = chatShareData.desc;
            arg1 = LanguageUtils.getTextFormat(300279, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y); 
            lbl_share = string.Format(langStr, arg0, arg1);
            lbl_share = ConvertlblShare(lbl_share, shareName, arg0, chatShareData.gameNode, chatShareData.vector2Int.x, chatShareData.vector2Int.y);
            msgContent = sb.ToString();
            return lbl_share;
        }
        public string ConvertStringToChatview10(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            string guildAbbName = "";
            coordinate = "";
            string cityName = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, cityNamePattern);
                if (match.Success)
                {
                    cityName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg0 = cityName;
            }
            else
            {
                arg0 = LanguageUtils.getTextFormat(300030, guildAbbName, cityName);
            }
            arg1 = coordinate;
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            return lbl_dec;
        }
        public string ConvertStringToChatview11(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
             coordinate = "";
            string resourceBuild = "";
            {
                var match = Regex.Match(msg.msg, resourceBuildPattern);
                if (match.Success)
                {
                    resourceBuild = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            arg0 = resourceBuild;
            arg1 = coordinate;
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            return lbl_dec;
        }
        public string ConvertStringToChatview12(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            string guildAbbName = "";
            coordinate = "";
            string strongHold = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, strongHoldPattern);
                if (match.Success)
                {
                    strongHold = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg0 = strongHold;
            }
            else
            {
                arg0 = String.Format("{0},{1}", guildAbbName, strongHold);
            }
            arg1 = coordinate;
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            return lbl_dec;
        }
        public string ConvertStringToChatview13(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            string guildAbbName = "";
             coordinate = "";
            string allianceBuild = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, allianceBuildPattern);
                if (match.Success)
                {
                    allianceBuild = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg0 = allianceBuild;
            }
            else
            {
                arg0 = String.Format("{0},{1}", guildAbbName, allianceBuild);
            }
            arg1 = coordinate;
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            return lbl_dec;
        }
        public string ConvertStringToChatview14(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
             coordinate = "";
            string monster = "";
            {
                var match = Regex.Match(msg.msg, monsterPattern);
                if (match.Success)
                {
                    monster = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            arg0 = monster;
            arg1 = coordinate;
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            return lbl_dec;
        }
        public string ConvertStringToChatview15(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            coordinate = "";
            string rune = "";
            {
                var match = Regex.Match(msg.msg, runePattern);
                if (match.Success)
                {
                    rune = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            arg0 = rune;
            arg1 = coordinate;
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1 });
            return lbl_dec;
        }
        public string ConvertStringToChatview20(ChatMsg msg, string langStr, out string coordinate)
        {
            string guildAbbName = "";
            string lbl_dec = "";
             coordinate = "";
            string cityName = "";
            string cityRid = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, cityRidPattern);
                if (match.Success)
                {
                    cityRid = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, cityNamePattern);
                if (match.Success)
                {
                    cityName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg0 = cityName;
            }
            else
            {
                arg0 = LanguageUtils.getTextFormat(300030, guildAbbName, cityName);
            }
            lbl_dec = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0 });
            return lbl_dec;
        }
        public string ConvertStringToChatview30(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            coordinate = "";
            return lbl_dec;
        }
        public string ConvertStringToChatview31(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            coordinate = "";
            return lbl_dec;
        }
        public string ConvertStringToChatview32(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            coordinate = "";
            return lbl_dec;
        }
        public string ConvertStringToChatview40(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
            coordinate = "";
            return lbl_dec;
        }
        public string ConvertStringToChatview99(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_dec = "";
             coordinate = "";
            string desc = "";
            {
                var match = Regex.Match(msg.msg, descPattern);
                if (match.Success)
                {
                    desc = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "";
            arg0 = desc;
            string[] strList = coordinate.Split(',');

            if (strList.Length == 3)
            {
                int jumpK = int.Parse(strList[0]);
                int jumpX = int.Parse(strList[1]);
                int jumpY = int.Parse(strList[2]);
                arg1 = LanguageUtils.getTextFormat(300279, jumpK, jumpX, jumpY);
            }
            lbl_dec = string.Format(langStr, arg0, arg1);
            return lbl_dec;
        }
        public string ConvertStringToMainView10(ChatMsg msg, string langStr, out string coordinate)
        {
            string guildAbbName = "";
            string lbl_linkmes = "";
             coordinate = "";
            string cityName = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, cityNamePattern);
                if (match.Success)
                {
                    cityName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;

                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            arg0 = coordinate;
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg1 = cityName;
            }
            else
            {
                arg1 = LanguageUtils.getTextFormat(300030, guildAbbName, cityName);
            }
            arg2 = coordinate;
            lbl_linkmes = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1, arg2 });
            if (ArabicFixer.IsRtl(msg.msg) && !LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    lbl_linkmes = string.Format("({3}:Y,{2}:X,{1}#) <href=0><color=#1fc4ec>{0}</color></href>", LanguageUtils.getTextFormat(300030, guildAbbName, cityName), jumpK, jumpX, jumpY);
                }
            }
            else if (ArabicFixer.IsRtl(msg.msg) && LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#)", LanguageUtils.getTextFormat(300030, guildAbbName, cityName), jumpK, jumpX, jumpY);
                }
            }

            return lbl_linkmes;
        }
        public string ConvertStringToMainView11(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
             coordinate = "";
            string resourceBuild = "";
            {
                var match = Regex.Match(msg.msg, resourceBuildPattern);
                if (match.Success)
                {
                    resourceBuild = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            arg0 = coordinate;
            arg1 = resourceBuild;
            arg2 = coordinate;
            lbl_linkmes = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1, arg2 });
            if (LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                   string name =   m_emailProxy.FormatResourceBuildName(resourceBuild);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#) ", name, jumpK, jumpX, jumpY);
                }
            }
            return lbl_linkmes;
        }
        public string ConvertStringToMainView12(ChatMsg msg, string langStr, out string coordinate)
        {
            string guildAbbName = "";
            string lbl_linkmes = "";
             coordinate = "";
            string strongHold = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, strongHoldPattern);
                if (match.Success)
                {
                    strongHold = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            arg0 = coordinate;
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg1 = strongHold;
            }
            else
            {
                arg1 = String.Format("{0},{1}", guildAbbName, strongHold);
            }
            arg2 = coordinate;
            lbl_linkmes = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1, arg2 });
            if (LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    string name = m_emailProxy.FormatStrongHold(strongHold);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#) ", name, jumpK, jumpX, jumpY);
                }
            }
            return lbl_linkmes;
        }
        public string ConvertStringToMainView13(ChatMsg msg, string langStr, out string coordinate)
        {
            string guildAbbName = "";
            string lbl_linkmes = "";
             coordinate = "";
            string allianceBuild = "";
            {
                var match = Regex.Match(msg.msg, guildAbbNamePattern);
                if (match.Success)
                {
                    guildAbbName = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, allianceBuildPattern);
                if (match.Success)
                {
                    allianceBuild = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            arg0 = coordinate;
            if (string.IsNullOrEmpty(guildAbbName))
            {
                arg1 = allianceBuild;
            }
            else
            {
                arg1 = String.Format("{0},{1}", guildAbbName, allianceBuild);
            }
            arg2 = coordinate;
            lbl_linkmes = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1, arg2 });
            if (LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    string name = m_emailProxy.FormatAllianceBuildName(allianceBuild);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#) ", name, jumpK, jumpX, jumpY);
                }
            }
            return lbl_linkmes;
        }
        public string ConvertStringToMainView14(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
             coordinate = "";
            string monster = "";
            {
                var match = Regex.Match(msg.msg, monsterPattern);
                if (match.Success)
                {
                    monster = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            arg0 = coordinate;
            arg1 = monster;
            arg2 = coordinate;
            lbl_linkmes = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1, arg2 });
            if (LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    string name = m_emailProxy.FormatMonsterName(monster);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#) ", name, jumpK, jumpX, jumpY);
                }
            }
            return lbl_linkmes;
        }
        public string ConvertStringToMainView15(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
             coordinate = "";
            string rune = "";
            {
                var match = Regex.Match(msg.msg, runePattern);
                if (match.Success)
                {
                    rune = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            arg0 = coordinate;
            arg1 = rune;
            arg2 = coordinate;
            lbl_linkmes = m_emailProxy.OnTextFormat(langStr, new List<string> { arg0, arg1, arg2 });
            if (LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    string name = m_emailProxy.FormatMapItemTypeName(rune);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#) ", name, jumpK, jumpX, jumpY);
                }
            }
            return lbl_linkmes;
        }
        public string ConvertStringToMainView20(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
        coordinate = "";
            return lbl_linkmes;
        }
        public string ConvertStringToMainView30(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
        coordinate = "";
            return lbl_linkmes;
        }
        public string ConvertStringToMainView31(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
    coordinate = "";
            return lbl_linkmes;
        }
        public string ConvertStringToMainView32(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
    coordinate = "";
    return lbl_linkmes;
        }
        public string ConvertStringToMainView40(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
    coordinate = "";
    return lbl_linkmes;
        }
        public string ConvertStringToMainView99(ChatMsg msg, string langStr, out string coordinate)
        {
            string lbl_linkmes = "";
             coordinate = "";
            string desc = "";
            {
                var match = Regex.Match(msg.msg, descPattern);
                if (match.Success)
                {
                    desc = match.Groups[1].Value;
                }
            }
            {
                var match = Regex.Match(msg.msg, coordinatePattern);
                if (match.Success)
                {
                    coordinate = match.Groups[1].Value;
                }
            }
            string arg0 = "", arg1 = "", arg2 = "";
            string[] strList = coordinate.Split(',');

            if (strList.Length == 3)
            {
                int jumpK = int.Parse(strList[0]);
                int jumpX = int.Parse(strList[1]);
                int jumpY = int.Parse(strList[2]);
                arg2 = LanguageUtils.getTextFormat(300279, jumpK, jumpX, jumpY);
            }
            arg0 = coordinate;
            arg1 = desc;
            lbl_linkmes = string.Format(langStr, arg0, arg1, arg2);
            if (ArabicFixer.IsRtl(msg.msg) && !LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    lbl_linkmes = string.Format("({3}:Y,{2}:X,{1}#) <href=0><color=#1fc4ec>{0}</color></href>", desc, jumpK, jumpX, jumpY);
                }
            }
            else if (ArabicFixer.IsRtl(msg.msg) && LanguageUtils.IsArabic())
            {
                string[] strArr = coordinate.Split(',');
                if (strArr.Length == 3)
                {
                    int jumpK = int.Parse(strArr[0]);
                    int jumpX = int.Parse(strArr[1]);
                    int jumpY = int.Parse(strArr[2]);
                    lbl_linkmes = string.Format("<href=0><color=#1fc4ec>{0}</color></href> ({3}:Y,{2}:X,{1}#)", desc, jumpK, jumpX, jumpY);
                }
            }
            return lbl_linkmes;
        }
        /// <summary>
        /// 地图对象转服务器消息
        /// </summary>
        /// <param name="chatContact"></param>
        /// <param name="chatShareData"></param>
        /// <param name="msgContent"></param>
        /// <param name="lbl_share"></param>
        /// <returns></returns>
        public bool ConvertContactToString(ChatContact chatContact, ChatShareData chatShareData, out string msgContent, out string lbl_share)
        {
            bool convert = true;
            msgContent = "";
            lbl_share = "";
            switch (chatShareData.ChatShareDefine.ID)
            {
                case 10:
                    {
                        lbl_share = ConvertContactToString10(chatShareData, out msgContent);
                    }
                    break;
                case 11:
                    {
                        lbl_share = ConvertContactToString11(chatShareData, out msgContent);
                    }
                    break;
                case 12:
                    {

                        lbl_share = ConvertContactToString12(chatShareData, out msgContent);

                    }
                    break;
                case 13:
                    {
                        lbl_share = ConvertContactToString13(chatShareData, out msgContent);

                    }
                    break;
                case 14:
                    {
                        lbl_share = ConvertContactToString14(chatShareData, out msgContent);

                    }
                    break;
                case 15:
                    {
                        lbl_share = ConvertContactToString15(chatShareData, out msgContent);

                    }
                    break;
                case 20:
                    {
                        lbl_share = ConvertContactToString20(chatShareData, out msgContent);

                    }
                    break;
                case 99:
                    {
                        lbl_share = ConvertContactToString99(chatShareData, out msgContent);
                    }
                    break;
                case 30:
                case 31:
                case 32:
                case 40:
                default:
                    convert = false;
                    Debug.LogError("未处理的数据类型");
                    break;
            }
            return convert;
        }

        /// <summary>
        /// 聊天信息在聊天界面显示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lbl_dec"></param>
        /// <param name="lbl_title"></param>
        /// <param name="icon"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool ConvertStringToChatview(ChatMsg msg, out string lbl_dec, out string lbl_title, out string icon, out string color, out string outCoordinate)
        {
            bool convert = true;
            lbl_dec = "";
            lbl_title = "";
            icon = "";
            color = "";
            outCoordinate = "";
            StringBuilder sb = new StringBuilder();

            int ChatShareDefineId = msg.chatShareID;
            ChatShareDefine chatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(ChatShareDefineId);
            if (chatShareDefine == null)
            {
                convert = false;
                return convert;
            }
            icon = chatShareDefine.iconID;
            string langStr = LanguageUtils.getText(chatShareDefine.l_desID);
            lbl_title = LanguageUtils.getText(chatShareDefine.l_titleID);
            color = (chatShareDefine.color);
            switch (chatShareDefine.ID)
            {
                case 10:

                    lbl_dec = ConvertStringToChatview10(msg, langStr, out outCoordinate);

                    break;
                case 11:
                    lbl_dec = ConvertStringToChatview11(msg, langStr, out outCoordinate);

                    break;
                case 12:
                    lbl_dec = ConvertStringToChatview12(msg, langStr, out outCoordinate);

                    break;
                case 13:
                    lbl_dec = ConvertStringToChatview13(msg, langStr, out outCoordinate);

                    break;
                case 14:
                    lbl_dec = ConvertStringToChatview14(msg, langStr, out outCoordinate);

                    break;
                case 15:
                    lbl_dec = ConvertStringToChatview15(msg, langStr, out outCoordinate);

                    break;
                case 20:
                    lbl_dec = ConvertStringToChatview20(msg, langStr, out outCoordinate);

                    break;
                case 30:
                case 31:
                case 32:
                case 40:
                case 99:
                    lbl_dec = ConvertStringToChatview99(msg, langStr, out outCoordinate);
                    break;
                default:
                    convert = false;
                    Debug.LogError("未处理的数据类型");
                    break;
            }
            return convert;
        }
        /// <summary>
        /// 聊天界面在主界面显示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lbl_dec"></param>
        /// <param name="lbl_title"></param>
        /// <param name="icon"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool ConvertStringToMainView(ChatMsg msg, out string lbl_linkmes,out string coordinate)
        {
            EmailProxy emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            bool convert = true;
            lbl_linkmes = "";
            coordinate = "";
            StringBuilder sb = new StringBuilder();

            int ChatShareDefineId = msg.chatShareID;
            ChatShareDefine chatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(ChatShareDefineId);
            if (chatShareDefine == null)
            {
                convert = false;
                return convert;
            }
            string langStr = LanguageUtils.getText(chatShareDefine.chatShow);
            switch (chatShareDefine.ID)
            {
                case 10:
                    {
                        lbl_linkmes = ConvertStringToMainView10(msg, langStr, out coordinate);
                    }
                    break;
                case 11:
                    {
                        lbl_linkmes = ConvertStringToMainView11(msg, langStr, out coordinate);

                    }
                    break;
                case 12:
                    {
                        lbl_linkmes = ConvertStringToMainView12(msg, langStr, out coordinate);

                    }
                    break;
                case 13:
                    {
                        lbl_linkmes = ConvertStringToMainView13(msg, langStr, out coordinate);

                    }
                    break;
                case 14:
                        {
                        lbl_linkmes = ConvertStringToMainView14(msg, langStr, out coordinate);

                    }
                    break;
                case 15:
                    {
                        lbl_linkmes = ConvertStringToMainView15(msg, langStr, out coordinate);

                    }
                    break;
                case 99:
                    {
                        lbl_linkmes = ConvertStringToMainView99(msg, langStr, out coordinate);

                    }
                    break;
                case 20:
                case 30:
                case 31:
                case 32:
                case 40:

                default:
                    convert = false;
                    Debug.LogError("未处理的数据类型");
                    break;
            }
            return convert;
        }
        public int SetChatShareData(MapObjectInfoEntity mapObjectInfo,  ChatShareData chatShareData)
        {
            int chatShareID = 0;
            switch (mapObjectInfo.rssType )
            {
                case RssType.City:
                    {
                        if (chatShareData.shareType == 0)
                        {
                            chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(10);
                        }
                        else if (chatShareData.shareType == 1)
                        {
                            chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(20);
                            chatShareData.cityRid = mapObjectInfo.cityRid;
                        }
                        chatShareData.cityName = mapObjectInfo.cityName;
                    }
                    break;
                case RssType.Stone://石料
                case RssType.Farmland://农田
                case RssType.Wood://木材
                case RssType.Gold: //金矿
                case RssType.Gem: //宝石
                case RssType.Village: //村庄
                case RssType.Cave: //山洞
                    {
                        chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(11);
                        chatShareData.resourceBuild = (int)mapObjectInfo.resourceGatherTypeDefine.ID;
                    }
                    break;
                case RssType.CheckPoint:
                case RssType.HolyLand:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1://关卡1
                case RssType.Checkpoint_2://关卡2
                case RssType.Checkpoint_3://关卡3
                    {
                        chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(12);
                        chatShareData.strongHold = (int)mapObjectInfo.strongHoldId;
                    }
                    break;
                case RssType.GuildCenter:
                case RssType.GuildFortress1: //联盟要塞1
               case RssType.GuildFortress2://联盟要塞2
                case RssType.GuildFlag: //15 联盟旗帜
                case RssType.GuildFood://16 联盟农田
                case RssType.GuildWood://联盟伐木场
                case RssType.GuildStone: //联盟石矿床
                case RssType.GuildGold://联盟金矿床
                case RssType.GuildFoodResCenter://20 联盟谷仓  资源中心
                case RssType.GuildWoodResCenter://联盟木料场
                case RssType.GuildGoldResCenter: //联盟石材厂
                case RssType.GuildGemResCenter://23联盟铸币场
                    {
                        chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(13);
                        chatShareData.allianceBuild = m_allianceProxy.GetBuildServerTypeToConfigType(mapObjectInfo.objectType);
                    }
                    break;
                case RssType.Monster:
                case RssType.BarbarianCitadel:
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                case RssType.Guardian: //圣地守护者
                    {
                        chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(14);
                        chatShareData.monster = (int)mapObjectInfo.monsterId;
                    }
                    break;
                case RssType.Rune:  //符文
                    {
                        chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(15);
                        chatShareData.rune = (int)mapObjectInfo.runeId;
                    }
                    break;
                case RssType.None:
                case RssType.Troop:  //军队
                case RssType.Transport: //运输车
                case RssType.Scouts: //斥候W
                case RssType.Expedition:  //远征内对象 



                    {
                        Debug.LogError("未处理的类型");
                    }
                    break;
            }
            return chatShareID;
        }
        #endregion
    }
}