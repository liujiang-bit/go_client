// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月8日
// Update Time         :    2019年11月8日
// Class Description   :    NetProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using Skyunion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Data;
using Sproto;
using SprotoType;
using U3D.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameSocketBase
    {
    }

    public enum ELoginState
    {
        EAuth1, //s1-2
        EAuth2, //s3-7
        EAuth3, //s8-10
        ETokenExpire,
        EAuthError,
        ERedirectionGameServer,
        EGameServerOK,
        EChatAuth,
    }

    public enum AuthError
    {
        BadRequest=400,// challenge failed
        UnAuthorized=401, //unAuthorized by auth_handler
        Forbidden =403, // login_handdler faile
        NotAccpetable=406, // already login
        UserBan=407,
        ToeknExpire=408
    }

    public class RpcTypeExtend
    {
        public const string REQUEST = "REQUEST";
        public const string RESPONSE = "RESPONSE";
        public const string REQUEST_ERROR  = "REQUEST_ERROR";
        public const string RESPONSE_ERROR = "RESPONSE_ERROR";
    }

    public class SprotoSocketAp
    {


        #region  For Networking Viewer
        public bool IsNetworkingViewerEnable { get; set; }
        public Action<ELoginState, int> OnLoginStateChange { get; set; }
        public Action<NetEvent, int> OnNetStateChange { get; set; }
        public Action<SprotoTypeBase, long> OnSendMessage { get; set; }
        public Action<SprotoTypeBase, long> OnReceiveMessage { get; set; }
        public Action<SprotoTypeBase> OnReceiveNotification { get; set; }
        #endregion
        /*
        1.Server->Client : base64(8bytes random challenge)
        2.Client->Server : base64(8bytes handshake client key)
        3.Server: Gen a 8bytes handshake server key
        4.Server->Client : base64(DH - Exchange(server key))
        5.Server / Client secret:= DH - Secret(client key / server key)
        6. Client->Server : base64(HMAC(challenge, secret))
        7. Client->Server : DES(secret, base64(token))  
        8.Server : call auth_handler(token) -> server, uid(A user defined method)
        9.Server : call login_handler(server, uid, secret) ->subid(A user defined method)
        10. Server->Client : 200 base64(subid)
        Error Code:
            400 Bad Request . challenge failed
            401 Unauthorized.unauthorized by auth_handler
            403 Forbidden.login_handler failed
            406 Not Acceptable . already in login(disallow multi login)
        Success:
            200 base64(base64(uid)@base64(gameserver)#base64(subid) base64(connectip)@base64(connectport))
        */

        public const int PackageHeadSize = 2;


        public INetClient NetClient;
        private ILogService logService;

        private NetPackInfo netPackInfo;

        private Action<NetEvent,int> connectEvent;

        private Action<ELoginState,int> authEvent;

        /// <summary>
        /// 授权状态
        /// </summary>
        private ELoginState _eLoginState;

        /// <summary>
        /// 协议层的sesion
        /// </summary>
        private long? m_sproto_session;

        /// <summary>
        /// 游戏层面session，用于重连等等
        /// </summary>
        private int m_game_session;


        private int AddGameSession()
        {
            m_game_session++;
            return m_game_session;
        }

        private int m_game_session_len = 4;
        private int m_game_compressed_len = 1;

        private Dictionary<long?, int> m_dicSessionMapTag = new Dictionary<long?, int>();
        private List<MessageContent> m_listMsg = new List<MessageContent>();

        
        private Dictionary<long,int> m_dicTagMapGameSession = new Dictionary<long, int>();
        
        private Dictionary<int,int> m_needResponeBack = new Dictionary<int, int>();
        private Dictionary<int,int> m_checkResponeBackConfigs = new Dictionary<int, int>();
        
        
        /// <summary>
        /// 包加密解密key
        /// </summary>
        public byte[] des_key;


        private string iggID;
        private string iggSdkToken;
        private string platform;
        private string language = string.Empty;
        private string clientSdkIP = string.Empty;
        private string serverNode = string.Empty;

        /// <summary>
        /// 服务器下发的信息
        /// </summary>
        private int uid;

        private int subid;

        /// <summary>
        /// 服务器域名
        /// </summary>
        private string serverHost;

        /// <summary>
        /// 服务器断开
        /// </summary>
        private int serverPort;

        /// <summary>
        /// 备用ip地址
        /// </summary>
        private string serverIP;

        /// <summary>
        /// 服务器名称
        /// </summary>
        private string serverName;

        public string ServerName => serverName;

        /// <summary>
        /// 是否有角色 0=无角色 1=有角色
        /// </summary> 
        private int hasRole;


        private int connectIndex;

        /// <summary>
        /// 临时变量
        /// </summary>
        private byte[] challenge;

        private byte[] server_key;
        private byte[] client_key;


        private SprotoRpc client;

        private SprotoRpc.RpcRequest clientRequest;

  

        private Timer m_sendHeartTimer;
        private float m_lastHeartSendTime;

        private StreamWriter _streamWriter;

        private bool m_isDebug = false;

        public static SprotoSocketAp CreateInstance(string ip, int port, Action<NetEvent,int> connectEvent,
            Action<ELoginState,int> authEvent)
        {
            var sprotoSocketAp = new SprotoSocketAp();
            INetServcice netService = PluginManager.Instance().FindModule<INetServcice>();

            sprotoSocketAp.connectEvent = connectEvent;
            sprotoSocketAp.authEvent = authEvent;

            sprotoSocketAp.NetClient = netService.CreateClient(sprotoSocketAp.OnNetEvent, sprotoSocketAp.OnReciveEvent,
                sprotoSocketAp.PacketProtocolResolve);

            sprotoSocketAp.serverHost = ip;
            sprotoSocketAp.serverPort = port;

            if (!Application.isMobilePlatform && sprotoSocketAp.m_isDebug)
            {
                // 这边的 DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss_ffff") ios会崩溃
                string fileName = Application.temporaryCachePath + "/" + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss_ffff") + ".log";
                sprotoSocketAp._streamWriter = File.CreateText(fileName);

                Debug.Log("网络日志" + fileName);
            }

            return sprotoSocketAp;
        }

        public SprotoSocketAp()
        {
            logService = PluginManager.Instance().FindModule<ILogService>();

            client = new SprotoRpc(Protocol.Instance);
            clientRequest = client.Attach(Protocol.Instance);

            m_sproto_session = 0;
            m_game_session = 0;
            _eLoginState = ELoginState.EAuth1;
        }


        /// <summary>
        /// 设置授权需要的信息
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="passwd">Passwd.</param>
        /// <param name="gamename">Gamename.</param>
        public void setLoginInfo(string iggID, string iggSdkToken, string platform, string language, string clientaddr,string serverNode)
        {
            this.iggID = iggID;
            this.iggSdkToken = iggSdkToken;
            this.platform = platform;
            this.language = language;
            this.clientSdkIP = clientaddr;
            this.serverNode = serverNode;

            _eLoginState = ELoginState.EAuth1;
            Task.Run(()=>{
                
                Debug.Log("setLoginInfo connect"+serverHost+":"+serverPort);
                NetClient.Connect(serverHost, serverPort);
            });
            
           
        }

        private void OnNetEvent(NetEvent @event,int error)
        {
            logService.Info(_eLoginState+"网络层状态改变 " + @event.ToString()+"  底层错误码:"+error+"  "+clientSdkIP+" :"+serverNode, Color.green);
            if (connectEvent != null)
            {
                Debug.Log("OnNetEvent invoke "+@event);
                connectEvent(@event,error);
            }

            if (@event == NetEvent.ConnectComplete && _eLoginState == ELoginState.ERedirectionGameServer)
            {
                SendRedirectionAuth();
            }
        }

        /// <summary>
        /// 重定向后再次授权验证
        /// 游戏服务器已2个字节包头体长度为协议
        /// </summary>
        private void SendRedirectionAuth()
        {
            Debug.Log("SendRedirectionAuth invoke");
            try
            {
                //username:index:hmac username->base64(uid)@base64(servername)#base64(subid)
                string authToken = string.Format("{0}@{1}#{2}:{3}",
                    Convert.ToBase64String(Encoding.Default.GetBytes(uid.ToString())),
                    Convert.ToBase64String(Encoding.Default.GetBytes(serverName)),
                    Convert.ToBase64String(Encoding.Default.GetBytes((subid).ToString())),
                    (++connectIndex).ToString());
                authToken = string.Format("{0}:{1}", authToken,
                    Convert.ToBase64String(Crypt.hmac64(Crypt.hashkey(authToken), des_key)));
                byte[] tk = Encoding.Default.GetBytes(authToken);

                //重定向不需要发送game session
                Debug.Log("发送重定向校验"+authToken);
                SendPackage(tk, 0 , 0);
            }
            catch (Exception e)
            {
                Tip.CreateTip($"无法连接到服务器{NetEvent.ConnectFail}。").Show();
                AppFacade.GetInstance().SendNotification(CmdConstant.NetEvent, NetEvent.ConnectFail,"0");
            }
           
        }

        /// <summary>
        /// 游戏重连
        /// </summary>
        public void ReConnectGameSever()
        {
            Debug.Log("重连时 gameSesion："+m_game_session);
            OnAuth(ELoginState.ERedirectionGameServer);
            Task.Run(() => { NetClient.Reconnect(); });
            
            ClearMapData();
        }


        private void ClearMapData()
        {
            m_listMsg.Clear();
            m_dicSessionMapTag.Clear();
            m_dicTagMapGameSession.Clear();
        }

        public void Close()
        {

            client = new SprotoRpc(Protocol.Instance);
            clientRequest = client.Attach(Protocol.Instance);
            
            m_game_session = 0;
            m_sproto_session = 0;
            m_lastHeartSendTime = 0;
            
            ClearMapData();
            
            OnAuth(ELoginState.EAuth1);
            NetClient.Disconnect();
            
            if (_streamWriter!=null)
            {
                _streamWriter.Close();
                _streamWriter = null;
            }
        }


        /// <summary>
        /// 写入头
        /// </summary>
        /// <param name="length"></param>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        private void EncodeHeader(int length, byte[] buffer, int start)
        {
            buffer[start + 0] = (byte) ((length >> 8) & 0xff);
            buffer[start + 1] = (byte) (length & 0xff);
        }

        //
        private void EncodeGameSession(int data, byte[] buffer, int start)
        {
            buffer[start + 3] = (byte) data;
            buffer[start + 2] = (byte) ((data >> 8) & 0xff);
            buffer[start + 1] = (byte) ((data >> 16) & 0xff);
            buffer[start + 0] = (byte) ((data >> 24) & 0xff);
        }


        public void SendPackage(Byte[] buffer, int attchSessionLen = 0, int game_session = 0)
        {
            var bufLen = buffer.Length;
            var data = new byte[PackageHeadSize + bufLen + attchSessionLen];

            EncodeHeader(bufLen + attchSessionLen, data, 0);
            if (attchSessionLen > 0)
            {
                EncodeGameSession(game_session, data, data.Length - attchSessionLen);
            }

            Array.Copy(buffer, 0, data, PackageHeadSize, bufLen);
            //Debug.Log("send "+ToHexString(data));
            NetClient.Send(new MemoryStream(data));
        }


        /// <summary>
        /// 发送文本类型换行包
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="start">Start.</param>
        /// <param name="length">Length.</param>
        private void SendHttpLine(Byte[] buffer)
        {
            //Debug.Log("SendHttpLine:"+ToHexString(buffer));
            byte[] _sendBuffer = new byte[buffer.Length + 1];
            buffer.CopyTo(_sendBuffer, 0);
            _sendBuffer[_sendBuffer.Length - 1] = 10;
            NetClient.Send(new MemoryStream(_sendBuffer));
        }


        public long? SendSproto(SprotoTypeBase obj)
        {
            int tag = obj.Tag();
            if (HasSprotoRespone(tag) && des_key!=null)
            {
                m_sproto_session++;
                byte[] reqByte = clientRequest.Invoke(obj, m_sproto_session);

                MessageContent content = new MessageContent();
                content.networkMessage = reqByte;

                m_listMsg.Clear();
                m_listMsg.Add(content);

                GateMessage.request warpReq = new GateMessage.request();
                warpReq.content = m_listMsg;

                m_sproto_session++;
                byte[] warpPackage = clientRequest.Invoke(warpReq, m_sproto_session);

                m_dicSessionMapTag.Add(m_sproto_session, tag);
                if(IsNetworkingViewerEnable)
                {
                    OnSendMessage?.Invoke(obj, m_sproto_session.Value);
                }
                var encodeWarpPackage = CPatch.DesEncodeBuffer(des_key, warpPackage);

                m_sproto_session++;
                
                var gameSession = AddGameSession();
            
                logService.Info(string.Format("发送包 {0} len:{1} time{2} session:{3} game_session：{4} ", obj.ToString(), warpPackage.Length,
                    Time.realtimeSinceStartup, m_sproto_session,gameSession));
                
                SendPackage(encodeWarpPackage, m_game_session_len, gameSession);

            }
            return m_sproto_session;
        }


        #region 网络回包限制


        public void AddSprotoRespineCheckConfig(int tag,int langID)
        {
            if (!m_checkResponeBackConfigs.ContainsKey(tag))
            {
                m_checkResponeBackConfigs.Add(tag,langID);
            }
            else
            {
                Debug.LogError("协议重复"+tag);
            }
        }

        //检查是否有回包
        private bool HasSprotoRespone(int tag)
        {
            
            if (m_checkResponeBackConfigs.ContainsKey(tag))
            {
//                Debug.Log(tag+" 检查未回包 "+m_dicTagMapGameSession.ContainsKey(tag));
                if (m_dicTagMapGameSession.ContainsKey(tag))
                {
                    Tip.CreateTip(m_checkResponeBackConfigs[tag]).Show();
                    return false;
                }
                else
                {
                    m_dicTagMapGameSession.Add(tag,m_game_session++);
                }
            }
            return true;
        }

        //结束回包
        private void EndSprotoRespone(int tag, int gameSession)
        {
            if (m_checkResponeBackConfigs.ContainsKey(tag))
            {
//                Debug.Log(tag+"  结束检查  "+gameSession);
                m_dicTagMapGameSession.Remove(tag);
            }
        }
        

        #endregion

      

        protected long FromBytes(byte[] buffer, int startIndex, int len)
        {
            long ret = 0;
            for (int i=0; i < len; i++)
            {
                ret = unchecked((ret << 8) | buffer[startIndex+i]);
            }
            return ret;
        }
        
        public int ToInt32(byte[] value, int startIndex)
        {
            return unchecked((int)FromBytes(value, startIndex, 4));
        }


    

        private void OnReciveSproto(MemoryStream packt)
        {
            byte[] buffer = packt.ToArray();
            
            //这个只是用来标示回包的game_session
            var game_session = ToInt32(buffer, buffer.Length - m_game_session_len - m_game_compressed_len);
            bool is_compressed = false;
            
            if (m_isDebug && _streamWriter!=null)
            {
                _streamWriter.Write(string.Format("{0} 网络 原始包 {1}  {2}\n",game_session,buffer.Length,System.Convert.ToBase64String(buffer)));
                
                Debug.Log(string.Format("{0} 网络 原始包  {1}  {2}\n",game_session,buffer.Length,ToHexString(buffer)));
            }

            
            if (m_game_compressed_len>0)
            { 
                is_compressed = buffer[buffer.Length - 1] > 0;

                if (is_compressed)
                {
                    
                    byte[] newBuffer = new byte[buffer.Length-m_game_session_len-m_game_compressed_len];
                    
                    Array.Copy(buffer,newBuffer,newBuffer.Length);
                    
                    byte[] decomBuffer = lzip.decompressBuffer(newBuffer);
                    buffer = decomBuffer;

                    if (decomBuffer ==null)
                    {
                        Debug.Log(string.Format("{0}网络 解压缩失败 {1}  {2}\n",game_session,packt.Length,ToHexString(packt.ToArray())));
                        if (m_isDebug && _streamWriter!=null)
                        {
                            _streamWriter.Write(string.Format("{0}解压缩失败 {1}  {2}\n",game_session,packt.Length,ToHexString(packt.ToArray())));
                            
                            
                            _streamWriter.Write(string.Format("{0}解压缩失败 base64 {1}  {2}\n",game_session,packt.Length,System.Convert.ToBase64String(packt.ToArray())));
                        }
                        return;
                    }
                    else
                    {
                        if (m_isDebug && _streamWriter!=null)
                        {
                            _streamWriter.Write(string.Format("{0}解压缩后 base64 {1}  {2}\n",game_session,packt.Length,System.Convert.ToBase64String(packt.ToArray())));
                        }
                    }
                }
            }
            

            if (m_isDebug && _streamWriter!=null)
            {
                _streamWriter.Write(string.Format("{0}解密前 {1}  {2}\n",game_session,buffer.Length,System.Convert.ToBase64String(buffer)));
            }

            int endLen = is_compressed ? 0 : (m_game_session_len + m_game_compressed_len);

            buffer = CPatch.DesDecodeBuffer(des_key, buffer, (uint) (buffer.Length - endLen));

            if (m_isDebug && _streamWriter!=null)
            {
                _streamWriter.Write(
                    string.Format("{0}解密后 {1}  {2}\n",game_session ,buffer.Length, System.Convert.ToBase64String(buffer)));
                
                _streamWriter.Flush();
            }

            try
            {
                SprotoRpc.RpcInfo recvInfo = client.Dispatch(buffer);

                List<MessageContent> msgContents = null;
                Header head = null;

                GateMessage.request gateMsg = null;
                GateMessage.response gateMsgRep = null;

                if (recvInfo.requestObj != null)
                {
                    gateMsg = (GateMessage.request) recvInfo.requestObj;
                    if (gateMsg.HasHead)
                    {
                        head = gateMsg.head;
                    }

                    msgContents = gateMsg.content;
                }
                else if (recvInfo.responseObj != null)
                {
                    gateMsgRep = (GateMessage.response) recvInfo.responseObj;
                    if (gateMsgRep.HasHead)
                    {
                        head = gateMsgRep.head;
                    }

                    msgContents = gateMsgRep.content;
                }
                else
                {
                    return;
                }

                int len = msgContents.Count;

                for (int i = 0; i < len; i++)
                {
                    MessageContent content = msgContents[i];
                    //移除掉需要等待回复的消息,服务器主动推的情况session为空
                    bool bHasSession = true;
                    try
                    {
                        bHasSession = recvInfo.session != null;
                    }
                    catch(Exception e)
                    {
                        bHasSession = false;
                    }
                    if (bHasSession)
                    {
                        int tag = m_dicSessionMapTag[recvInfo.session];


                        //返回错误只有请求服务器返回的时候，服务器主动推的不可能有错误码
                        if (content.HasError)
                        {
                            EndSprotoRespone(m_dicSessionMapTag[recvInfo.session], game_session);
                            string ErrorInfo = GetErrorInfo(content.error.errorCode);
                            logService.Error("回包错误码:" + content.error.errorCode + " : " + ErrorInfo);
                            if (!String.IsNullOrEmpty(ErrorInfo)&&(Application.platform==RuntimePlatform.OSXEditor||Application.platform==RuntimePlatform.WindowsEditor||Application.platform==RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer))
                            {
                                Tip.CreateTip("服务端反馈:"+ErrorInfo).Show();
                            }
                           // MsgResOK(gameSession);
                            //统一飘tips处理
                            //AppFacade.Instance.SendNotification(CmdConstant.NetWorkError, content.error);
                            // 特别处理，需要各个模块监听
                            AppFacade.GetInstance().SendNotification(m_dicSessionMapTag[recvInfo.session].ToString(),
                                content.error, RpcTypeExtend.RESPONSE_ERROR);
                            if(IsNetworkingViewerEnable)
                            {
                                OnReceiveMessage?.Invoke(content.error, recvInfo.session.Value);
                            }
                            continue;
                        }
                    }

                    if (content.networkMessage == null)
                    {
                        logService.Error("error content.networkMessage!!!空包请服务器检查");
                        continue;
                    }

                    try
                    {
                        SprotoRpc.RpcInfo subRecvInfo = client.Dispatch(content.networkMessage);

                        if (subRecvInfo.type == SprotoRpc.RpcType.REQUEST)
                        {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                            if (subRecvInfo.tag!= 30001)
                            {
                                logService.Info(string.Format("服务器请求 {0} Tag:{1}  {2}/{3} packlen:{4} time:{5}",
                                    subRecvInfo.requestObj.ToString(), subRecvInfo.tag.ToString(), i, len, packt.Length, Time.realtimeSinceStartup));
                            }
//#endif
                            AppFacade.GetInstance().SendNotification(subRecvInfo.tag.ToString(), subRecvInfo.requestObj,
                                RpcTypeExtend.REQUEST);
                            //  logService.Info(string.Format("收到包 {0}", subRecvInfo.requestObj));
                            if (IsNetworkingViewerEnable)
                            {
                                OnReceiveNotification?.Invoke(subRecvInfo.requestObj);
                            }
                        }
                        else
                        {

                            EndSprotoRespone(m_dicSessionMapTag[recvInfo.session], game_session);
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                            logService.Info(string.Format("客户端请求回包 {0} session :{1}  len:{2}/{3} packlen:{4} time:{5}  game_session:{6}",
                                subRecvInfo.responseObj.ToString(), m_dicSessionMapTag[recvInfo.session].ToString(), i,
                                len, packt.Length, Time.realtimeSinceStartup,game_session));
//#endif
                            
                            AppFacade.GetInstance().SendNotification(m_dicSessionMapTag[recvInfo.session].ToString(),
                                subRecvInfo.responseObj, RpcTypeExtend.RESPONSE);
                            logService.Info(string.Format("收到包 {0}", subRecvInfo.responseObj.ToString()));
                            if (IsNetworkingViewerEnable)
                            {
                                OnReceiveMessage?.Invoke(subRecvInfo.responseObj, recvInfo.session.Value);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logService.Error(ex.ToString());
                        continue;
                    }
                }
            }
            catch (System.Exception ex)
            {
                logService.Error(ex.ToString());
            }
        }


        public void OnAuth(ELoginState state,int errorCode = 0)
        {
            if (this.authEvent != null && state != _eLoginState)
            {
                _eLoginState = state;
                authEvent(state,errorCode);
            }
        }


        /// <summary>
        /// 收到拆好的包
        /// </summary>
        /// <param name="packt"></param>
        public void OnReciveEvent(MemoryStream packt)
        {
            //logService.Info(packt.Length + " 收到拆好的包 " + _eLoginState + "   " + ToHexString(packt.ToArray()));

            try
            {
                switch (_eLoginState)
                {
                    case ELoginState.EGameServerOK:

                        OnReciveSproto(packt);

                        break;
                    case ELoginState.EAuth1:

                        OnAuth(ELoginState.EAuth2);
                        challenge = packt.ToArray();
                        challenge = Convert.FromBase64String(Encoding.Default.GetString(challenge).Trim('\0'));


                        client_key = Crypt.randomkey();
                        byte[] handshake = Crypt.dhexchange(client_key);
                        handshake = Encoding.Default.GetBytes(Convert.ToBase64String(handshake));

                        SendHttpLine(handshake);
                        break;
                    case ELoginState.EAuth2:

                        OnAuth(ELoginState.EAuth3);

                        server_key = packt.ToArray();
                        server_key = Convert.FromBase64String(Encoding.Default.GetString(server_key).Trim('\0'));
                        des_key = Crypt.dhsecret(server_key, client_key);

                        challenge = Encoding.Default.GetBytes(Convert.ToBase64String(Crypt.hmac64(challenge, des_key)));
                        SendHttpLine(challenge);


                        string strToken = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", iggID, iggSdkToken, platform, language,
                            clientSdkIP,serverNode);
                        byte[] token = Encoding.Default.GetBytes(strToken);
                        token = Encoding.Default.GetBytes(
                            Convert.ToBase64String(CPatch.DesEncodeBuffer(des_key, token)));
                        SendHttpLine(token);

                        break;
                    case ELoginState.EAuth3:

                        byte[] result = packt.ToArray();
                        string resLoginServer = Encoding.Default.GetString(result);
                        logService.Info(string.Format("授权服务器结果: {0} ", resLoginServer));
                        
                        int errorCode = 0; 
                        int.TryParse(resLoginServer.Substring(0,3),out errorCode);
                        if (errorCode == 200)
                        {
                            if (ParseAuthResult(resLoginServer))
                            {
                                OnAuth(ELoginState.ERedirectionGameServer);
                                AppFacade.GetInstance().SendNotification(CmdConstant.MaintainCheckSingleServer, serverName);
                                //RedirectGameServer();
                            }
                            else
                            {
                                OnAuth(ELoginState.EAuthError,errorCode);
                            }
                        }
                        else
                        {
                            OnAuth(ELoginState.EAuthError,errorCode);
                        }

                        break;
                    case ELoginState.ERedirectionGameServer:
                        byte[] recvBuffer = packt.ToArray();
                        string resGameServer = Encoding.Default.GetString(recvBuffer);
                        logService.Info(string.Format("重定向游戏服务器结果: {0} ", resGameServer));
                        int errorCode2 = 0; 
                        int.TryParse(resGameServer.Substring(0,3),out errorCode2);
                        if (errorCode2==200)
                        {
                            OnAuth(ELoginState.EGameServerOK);
                        }
                        else
                        {
                            OnAuth(ELoginState.EAuthError, errorCode2);
                        }

                        break;
                    case ELoginState.EChatAuth:
                        {
                            string chatAuthResult = Encoding.Default.GetString(packt.ToArray());
                            Debug.Log(string.Format("授权服务器结果: {0} ", chatAuthResult));

                            int chatCode = 0;
                            int.TryParse(chatAuthResult.Substring(0, 3), out chatCode);
                            if (chatCode == 200)
                            {
                                OnAuth(ELoginState.EGameServerOK);
                            }
                            else
                            {
                                OnAuth(ELoginState.EAuthError, chatCode);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        /// <summary>
        /// 解析授权后结果
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private bool ParseAuthResult(string res)
        {
            bool isParseOk = false;
            string[] splitStr = res.Split(' ');
            try
            {
                string tmp =
                    Encoding.Default.GetString(Convert.FromBase64String(splitStr[1].Trim('\0')));
                string[] match = tmp.Split('@');
                iggID =
                    Encoding.Default.GetString(
                        Convert.FromBase64String(match[0]));
                subid = Convert.ToInt32(
                    Encoding.Default.GetString(
                        Convert.FromBase64String(match[1])));
                serverHost = Encoding.Default.GetString(
                    Convert.FromBase64String(match[2]));

                serverPort =
                    int.Parse(Encoding.Default.GetString(
                        Convert.FromBase64String(match[3])));
                serverIP = Encoding.Default.GetString(
                    Convert.FromBase64String(match[4]));

                if (match.Length >= 6)
                {
                    serverName = Encoding.Default.GetString(
                        Convert.FromBase64String(match[5]));
                }

                if (match.Length == 7)
                {
                    uid = Convert.ToInt32(
                        Encoding.Default.GetString(
                            Convert.FromBase64String(match[6])));
                }

                isParseOk = true;
                logService.Info(string.Format(
                    "AuthedGameServer IP:{0}:{1}  uid:{2} gamename:{3} subid:{4} serverName:{5}", serverHost,
                    serverPort, uid, platform, subid, serverName));
            }
            catch (Exception e)
            {
                logService.Error(e.Message);
                _eLoginState = ELoginState.EAuthError;

                OnAuth(_eLoginState);
                return false;
            }


            return isParseOk;
        }

        /// <summary>
        /// 授权成功重定向到下发的新游戏服务器去
        /// </summary>
        public void RedirectGameServer()
        {
            NetClient.Connect(serverHost, serverPort);
        }

        public NetPackInfo PacketProtocolResolve(ArraySegment<byte> segmentBytes)
        {
            //logService.Info("收到包 " + _eLoginState);
            if (_eLoginState == ELoginState.EGameServerOK || _eLoginState == ELoginState.ERedirectionGameServer||_eLoginState==ELoginState.EChatAuth)
            {
                return SprotoProtocalResolve(segmentBytes);
            }
            else
            {
                return HttpProtocalResolve(segmentBytes);
            }
        }

        public NetPackInfo HttpProtocalResolve(ArraySegment<byte> segmentBytes)
        {
            netPackInfo = new NetPackInfo();
            netPackInfo.packet_size = segmentBytes.Count - segmentBytes.Offset;

//            Debug.LogFormat("解析长度Offset:{0}  Count:{1} packet_size:{2} {3}", segmentBytes.Offset, segmentBytes.Count,
//                netPackInfo.packet_size, _eLoginState);

            byte[] packetData = new byte[netPackInfo.packet_size];
            int copyOffset = segmentBytes.Offset;
            int copyLength = netPackInfo.packet_size;
            Array.Copy(segmentBytes.Array, copyOffset, packetData, 0, copyLength);

            netPackInfo.content = new MemoryStream(packetData, segmentBytes.Offset, segmentBytes.Count);

            return netPackInfo;
        }

        public NetPackInfo SprotoProtocalResolve(ArraySegment<byte> segmentBytes)
        {
            if (segmentBytes.Count < PackageHeadSize)
            {
                return null;
            }

            int bodyLen = segmentBytes.Array[segmentBytes.Offset] << 8 | segmentBytes.Array[segmentBytes.Offset + 1];
            int packet_size = bodyLen + PackageHeadSize;

            if (segmentBytes.Count < packet_size)
            {
                return null;
            }

            netPackInfo = new NetPackInfo();
            netPackInfo.packet_size = packet_size;

            byte[] packetData = new byte[netPackInfo.packet_size - PackageHeadSize];
            int copyOffset = segmentBytes.Offset + PackageHeadSize;
            int copyLength = netPackInfo.packet_size - PackageHeadSize;
            Array.Copy(segmentBytes.Array, copyOffset, packetData, 0, copyLength);

//            if(packetData[packetData.Length - 1] > 0)
//            {
//                Debug.Log("包有问题，需要做兼容");
//                for(int i = 0; i < packetData.Length-5; i++)
//                {
//                    if(packetData[i]==0&& packetData[i+1] == 0 && packetData[i+2] == 0 && packetData[i+3] == 1 && packetData[i+4] == 0)
//                    {
//                        Debug.Log("包有问题，需要做兼容:返回兼容的包");
//                        byte[] packetData2 = new byte[i+5];
//                        Array.Copy(packetData, 0, packetData2, 0, i + 5);
//                        netPackInfo.content = new MemoryStream(packetData2);
//                        netPackInfo.packet_size = i + 5 + PackageHeadSize;
//                        return netPackInfo;
//                    }
//                }
//            }
//

            netPackInfo.content = new MemoryStream(packetData);

            return netPackInfo;
        }


        private static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                    strB.Append(" ");
                }

                hexString = strB.ToString();
            }

            return hexString;
        }

        public void SetHeartSend()
        {
            SendHeart();
            if (m_sendHeartTimer == null)
            {
                m_sendHeartTimer = Timer.Register(1.0f, UpdateHeart, null, true, true);
            }
        }

        public void UpdateHeart()
        {
            if (Time.realtimeSinceStartup - m_lastHeartSendTime >= 15)
            {
                SendHeart();
            }
        }

        private void SendHeart()
        {
            m_lastHeartSendTime = Time.realtimeSinceStartup;
            Role_Heart.request req = new Role_Heart.request();
            //req.clientTime = ServerTimeModule.Instance.GetTimestamp();
            req.clientTime = ServerTimeModule.Instance.GetTicks();
            req.serverTime = ServerTimeModule.Instance.GetServerTimeMilli();
            SendSproto(req);
        }
        string GetErrorInfo(long code)
        {
            string info = String.Empty;
            switch ((ErrorCode)code)
            {
                case ErrorCode.SUCCESS: info                          = "成功"; break;
                case ErrorCode.SERVER_DUMP: info                      = "系统异常"; break;
                case ErrorCode.CFG_ERROR: info                        = "配置表错误"; break;
                case ErrorCode.PROTOCOL_DELETE: info                  = "协议作废"; break;

                // 角色相关错误
                case ErrorCode.ROLE_CREATE_EXIST: info                         = " 角色已经存在,无法创建"; break;
                case ErrorCode.ROLE_CREATE_FAIL: info                          = "  创建角色失败"; break;
                case ErrorCode.ROLE_NOT_EXIST: info                            = " 角色不存在"; break;
                case ErrorCode.ROLE_ARG_ERROR: info                            = " 角色模块参数错误"; break;
                case ErrorCode.ROLE_HERO_NOT_EXIST: info                       = " 统帅不存在"; break;
                case ErrorCode.ROLE_HERO_NOT_WAIT_STATUS: info                 = " 统帅未处于待命状态"; break;
                case ErrorCode.ROLE_HERO_STAR_NOT_ENOUGH: info                 = " 统帅星级不足"; break;
                // case ErrorCode.ROLE_SOLDIER_NOT_ENOUGH: info                   = " 士兵数量不足"; break;
                case ErrorCode.ROLE_SOLDIER_TOO_MUCH: info                     = " 选择的士兵数量超过部队容量"; break;
                case ErrorCode.ROLE_TROOP_FULL: info                           = " 行军队列已满"; break;
                case ErrorCode.ROLE_FOOD_NOT_ENOUGH: info                      = " 粮食不足"; break;
                case ErrorCode.ROLE_WOOD_NOT_ENOUGH: info                      = " 木材不足"; break;
                case ErrorCode.ROLE_STONE_NOT_ENOUGH: info                     = " 石头不足"; break;
                case ErrorCode.ROLE_GOLD_NOT_ENOUGH: info                      = " 金币不足"; break;
               // case ErrorCode.ROLE_DENAR_NOT_ENOUGH: info                     = " 钻石不足"; break;
                case ErrorCode.ROLE_ARMY_LOCK: info                            = " 该兵种未解锁"; break;
                case ErrorCode.ROLE_ARMY_BUILDING_UPDATE: info                 = " 当前建筑正在升级"; break;
                case ErrorCode.ROLE_ARMY_IN_TRAINING: info                     = " 当前队列正在训练"; break;
                case ErrorCode.ROLE_ARMY_TRAIN_NOT_END: info                   = " 训练未结束无法领取"; break;
                case ErrorCode.ROLE_RSS_CANT_BUY: info                         = " 该资源不能快捷购买"; break;
                case ErrorCode.ROLE_ARMY_NOT_ENOUGH: info                      = " 士兵数量不足"; break;
                case ErrorCode.ROLE_HOSPITAL_NOT_ENOUGH: info                  = " 医院容量不足"; break;
                case ErrorCode.ROLE_TREATMENT_NOT_FINISH: info                 = " 还未治疗完成"; break;
                case ErrorCode.ROLE_TREATMENT_NOT_SOLDIER: info                = " 没有可领取的士兵"; break;
                case ErrorCode.ROLE_ARMY_LEVEL_SAME: info                      = " 无法晋升"; break;
                case ErrorCode.ROLE_HOSPITAL_UPDATE: info                      = "  所有医院都在升级"; break;
                case ErrorCode.ROLE_RESOURCE_NOT_EXIST: info                   = "资源点不存在"; break;
                case ErrorCode.ROLE_MONSTER_NOT_EXIST: info                    = " 怪物不存在"; break;
                case ErrorCode.ROLE_ACTION_NOT_ENOUGH: info                    = " 角色行动力不足"; break;
                case ErrorCode.ROLE_ALREADY_REINFORCE: info                    = " 角色已增援此城堡"; break;
                case ErrorCode.ROLE_REINFORCE_MAX: info                        = "城堡增援超过上限"; break;
                case ErrorCode.ROLE_ALREADY_RALLY: info                        = " 角色已参加此集结"; break;
                case ErrorCode.ROLE_RALLY_MAX: info                            = " 集结超过上限"; break;
                case ErrorCode.ROLE_MARCH_TARGET_NOT_EXIST: info               = "军队行军目标不存在"; break;
                case ErrorCode.ROLE_NOT_SELECT_SOLDIER: info                   = " 角色未选择士兵"; break;
                case ErrorCode.ROLE_SPEED_QUENE_FINISH: info                   = " 加速队列已完成"; break;
               // case ErrorCode.ROLE_RESOURCE_NO_TECHNOLOGY: info               = "前置科技研究未完成"; break;

                // 地图相关错误
//                case ErrorCode.MAP_MARCH_PATH_NOT_FOUND: info         = " 行军路线不存在"; break;
                case ErrorCode.MAP_ARG_ERROR: info                    = " 地图模块参数错误"; break;
                case ErrorCode.MAP_ARMY_NOT_EXIST: info               = " 军队不存在"; break;
                case ErrorCode.MAP_MAX_LEVEL_LIMIT: info              = " 搜索野蛮人超出可刷新等级"; break;
                case ErrorCode.MAP_NOT_FOUND_BARBARIAN: info          = " 未找到合适的野蛮人"; break;
                case ErrorCode.MAP_MONSTER_NOT_EXIST: info            = " 怪物不存在"; break;
                case ErrorCode.MAP_MARCH_ACTION_NOT_ENOUGH: info      = " 角色行军行动力不足"; break;
                case ErrorCode.MAP_MONSTER_LEVEL_TOO_HIGH: info       = " 怪物等级太高"; break;
                case ErrorCode.MAP_RESOURCE_NOT_EXIST: info           = " 采集对象不存在"; break;
                case ErrorCode.MAP_COLLECT_THIS_RESOURCE: info        = " 角色正在采集该资源点"; break;
                case ErrorCode.MAP_ARMY_FAILED_STATUS: info           = " 军队溃败中"; break;
               // case ErrorCode.MAP_MARCH_SAME_TARGET: info            = "  已经向该对象行军"; break;
                case ErrorCode.MAP_ALREADY_STATION: info              = "  军队已经在驻扎中"; break;
                case ErrorCode.MAP_VILLAGE_ALREADY_SCOUT: info        = "  目标村庄已失效"; break;
                case ErrorCode.MAP_VILLAGE_NOT_EXIST: info            = "  目标村庄不存在"; break;
                case ErrorCode.MAP_NOT_VILLAGE: info                  = "  目标不是村庄"; break;
                case ErrorCode.MAP_CITY_DETAIL_SELF: info             = "  无法获取自己的地图内城建筑信息"; break;
                case ErrorCode.MAP_CAVE_ALREADY_SCOUT: info           = "  山洞已探索"; break;
                case ErrorCode.MAP_ATTACK_CITY_LEVEL_ERROR: info      = "  进攻城堡等级不足"; break;
               // case ErrorCode.MAP_ATTACK_SHILED: info                = "  目标处于护盾中无法攻击"; break;
               // case ErrorCode.MAP_ATTACK_GUILD_MEMBER: info          = "  无法攻击同盟玩家"; break;
                case ErrorCode.MAP_REINFORCE_NOT_SELF_GUILD: info     = "  增援的不是自己联盟的建筑"; break;
               // case ErrorCode.MAP_ALREADY_REINFORCE_GUILD: info      = "  只能派遣1支部队建造联盟建筑"; break;
//                case ErrorCode.MAP_GUILD_BUILD_ARMY_LIMIT: info       = "  超过集结部队容量限制"; break;
         //       case ErrorCode.MAP_MOVE_CITY_MARCH_BATTLE: info       = "  执政官，您有部队正在行军或战斗中，无法进行迁城"; break;
            //    case ErrorCode.MAP_MOVE_CITY_STATION: info            = "  执政官，您有部队正在野外扎营中，暂时无法进行迁城"; break;
           //     case ErrorCode.MAP_MOVE_CITY_REINFORCE: info          = "  执政官，在部队集结过程中无法进行迁城"; break;
          //      case ErrorCode.MAP_MOVE_CITY_COLLECT: info            = "  您有部队正在野外进行采集，暂时无法进行迁城。"; break;
          //      case ErrorCode.MAP_MOVE_CITY_DENSEFOG: info           = "  无法迁城至迷雾地区"; break;
            //    case ErrorCode.MAP_MOVE_CITY_NOVICE_ITEM: info        = "  当前不满足新手迁城条件"; break;
            //    case ErrorCode.MAP_MOVE_CITY_ITEM_NOT_ENOUGH: info    = "  迁城道具不足"; break;
            //    case ErrorCode.MAP_MOVE_CITY_NO_GUILD_TERRITORY: info = "  目标点不是联盟领土"; break;

                // 统帅模块相关错误
                case ErrorCode.HERO_OPEN_DAYS_NOT_ENOUGH: info        = " 召唤统帅，所在王国不满足该统帅解锁天数条件"; break;
                case ErrorCode.HERO_SUMMON_ITEM_NOT_ENOUGH: info      = " 召唤统帅所需道具不足"; break;
                case ErrorCode.HERO_ALREADY_EXIST: info               = " 统帅已存在"; break;

                // 任务模块相关错误
                case ErrorCode.TASK_ARG_ERROR: info                   = " 任务模块参数错误"; break;
                case ErrorCode.TASK_ALREADY_HAVE_CHAPTER: info        = " 角色已领取章节"; break;
                case ErrorCode.TASK_FINISH_ALL_CHAPTER: info          = " 角色已完成所有章节"; break;
                case ErrorCode.TASK_NOT_HAVE_CHAPTER: info            = " 角色未领取章节"; break;
                case ErrorCode.TASK_CHAPTER_NOT_TASK: info            = " 章节中无此任务"; break;
                case ErrorCode.TASK_ALREADY_FINISH_CHAPTER_TASK: info = " 角色已完成此章节任务"; break;
                case ErrorCode.TASK_NOT_FINISH_ALL_CHAPTER_TASK: info = " 角色未完成所有章节任务"; break;
                case ErrorCode.TASK_CHAPTER_TASK_NOT_FINISH:   info   = " 章节任务未完成"; break;
                case ErrorCode.TASK_NOT_ACCEPT_MAINLINE:   info       = " 角色未领取该主线任务"; break;
                case ErrorCode.TASK_NOT_FINISH:  info                 = " 任务完成条件不满足"; break;
                case ErrorCode.TASK_SIDE_FINISHED: info               = " 该支线任务已完成"; break;

                // 科技模块相关错误
                case ErrorCode.TECHNOLOGY_COLLAGE_NOT_BUILD: info     = " 学院未建造"; break;
                case ErrorCode.TECHNOLOGY_COLLAGE_UPDATE: info        = " 学院正在升级"; break;
//                case ErrorCode.TECHNOLOGY_RESEARCHING: info           = " 正在研究"; break;
                case ErrorCode.TECHNOLOGY_PRE_LOCK: info              = " 前置科技未解锁"; break;
                case ErrorCode.TECHNOLOGY_CAMPUS_LV_NOT_ENOUGH: info  = " 学院等级不足"; break;
                case ErrorCode.TECHNOLOGY_NOT_AWARD:    info          = "  科技还未领取"; break;
                case ErrorCode.TECHNOLOGY_NOT_RESEARCH:  info         = " 没有在研究的科技"; break;

                // 建筑模块相关错误
                case ErrorCode.BUILDING_LOCK: info                    = " 建筑未解锁"; break;
                case ErrorCode.BUILDING_AREA_ERROR: info              = " 建筑区域错误"; break;
                case ErrorCode.BUILDING_COUNT_MAX: info               = " 建筑数量超出限制"; break;
                case ErrorCode.BUILDING_NOT_FREE_QUEUE: info          = " 没有空闲的建筑队列"; break;
                case ErrorCode.BUILDING_LEVEL_MAX: info               = " 建筑已满级"; break;
                case ErrorCode.BUILDING_TRIAN: info                   = " 建筑正在训练中，无法升级"; break;
                case ErrorCode.BUILDING_RESREACH: info                = " 建筑正在研究中，无法升级"; break;
                case ErrorCode.BUILDING_TREATMENT: info               = " 建筑正在治疗中，无法升级"; break;
                case ErrorCode.BUILDING_CREATEING:   info             = "  建筑还未创建完成，无法收取资源"; break;
                case ErrorCode.BUILDING_NOT_EXIST:info                = "  建筑不存在"; break;

                // 邮件模块相关错误
                case ErrorCode.EMAIL_ARG_ERROR: info                  = " 邮件模块参数错误"; break;
                case ErrorCode.EMAIL_NOT_EXIST: info                  = " 邮件不存在"; break;
                case ErrorCode.EMAIL_NOT_ENCLOSURE: info              = " 邮件无附件奖励"; break;
                case ErrorCode.EMAIL_ALREADY_TAKE_ENCLOSURE: info     = "已领取该邮件附件奖励"; break;
                case ErrorCode.EMAIL_ALREADY_READ: info               = " 邮件已读"; break;
                case ErrorCode.EMAIL_ALREADY_REQUEST: info            = " 已请求过邮件信息"; break;
                case ErrorCode.EMAIL_NOT_TAKE_ENCLOSURE: info         = " 已请求过邮件信息"; break;

                // 道具模块相关错误
                case ErrorCode.ITEM_NOT_ENOUGH: info                       = " 道具不足"; break;
                case ErrorCode.ITEM_ARG_ERROR: info                        = " 道具模块参数错误"; break;
                case ErrorCode.ITEM_NOT_EXIST: info                        = " 道具不存在"; break;
                case ErrorCode.ITEM_NOT_RESOURCE_ITEM: info                = " 不是资源类型道具"; break;

                // 斥候模块相关错误
                case ErrorCode.SCOUTS_NOT_FOUND:                   info    = "  斥候不存在"; break;
                case ErrorCode.SCOUTS_NOT_STANBY:                   info   = "斥候不处于待命状态"; break;
                case ErrorCode.SCOUTS_NOT_POS_DENSEFOG:               info = "目的地范围内没有未开启的迷雾"; break;

                // 集结相关
                case ErrorCode.RALLY_ARG_ERROR:     info = " 集结模块参数错误"; break;
                case ErrorCode.RALLY_ROLE_MAX_LAUNCH:          info = " 角色只能同时发起一个集结"; break;
                case ErrorCode.RALLY_ROLE_NO_GUILD:     info = " 角色未加入联盟,无法发起集结"; break;
                case ErrorCode.RALLY_ROLE_LEVEL_LESS:        info = " 角色市政厅等级不足,无法发起集结"; break;
                case ErrorCode.RALLY_NO_CASTLE_BUILD:       info = " 角色无城堡建筑,无法发起集结"; break;
               // case ErrorCode.RALLY_GUILD_HAD_RALLY:          info = " 联盟已对目标发起了集结"; break;
                case ErrorCode.RALLY_TARGET_LEVEL_LESS:        info = " 目标城市等级过低,无法集结"; break;
                //case ErrorCode.RALLY_NO_PATH_TO_TARGET:        info = " 无路径到目标点,无法集结"; break;
                case ErrorCode.RALLY_ACTION_FORCE_NO_ENOUGH:        info = " 集结行动力不足"; break;
               // case ErrorCode.RALLY_TARGET_IN_SHIELD:         info = " 目标处于护盾内"; break;
                //case ErrorCode.RALLY_TARGET_NOT_BORDER:       info = " 目标未与联盟领地接壤"; break;
                case ErrorCode.RALLY_INVALID_READY_TIME:        info = " 无效的集结时间"; break;
               // case ErrorCode.RALLY_OVER_MAX_MASS_TROOPS:        info = " 超过最大集结容量"; break;
                case ErrorCode.RALLY_JOIN_NO_GUILD:       info = " 加入集结失败,不在联盟中"; break;
                case ErrorCode.RALLY_JOIN_NOT_SAME_GUILD:         info = " 加入集结失败,不处于同一联盟中"; break;
                case ErrorCode.RALLY_ARMY_CANNOT_JOIN:      info = " 部队无法加入集结"; break;
               // case ErrorCode.RALLY_JOIN_NOT_FOUND:       info = " 目标未发起集结,"; break;
                case ErrorCode.RALLY_JOIN_HAD_JOIN:     info = " 已经加入了集结,无法加入"; break;
               // case ErrorCode.RALLY_PATH_NOT_FOUND:       info = " 加入集结失败,未找到路径"; break;
                case ErrorCode.RALLY_CREATE_ARMY_FAIL:        info = " 集结创建部队失败"; break;
                case ErrorCode.RALLY_JOIN_SELF:        info = " 无法加入自己的集结"; break;
                case ErrorCode.RALLY_REPARTRIATION_SELF:        info = " 无法遣返自己的部队"; break;
                case ErrorCode.RALLY_REPARTRIATION_NOT_LEADER:        info = " 非集结队长,无法遣返部队"; break;
                case ErrorCode.RALLY_REPARTRIATION_NOT_IN_TEAM:        info = " 不在集结队伍中,无法遣返"; break;
               // case ErrorCode.RALLY_REPARTRIATION_TEAM_LEAVE:       info = " 集结队伍已出发,无法遣返"; break;
                case ErrorCode.RALLY_DISBAND_NOT_RALLY:     info = " 集结队伍不存在,无法解散"; break;
                case ErrorCode.RALLY_REINFORCE_NOT_RALLY_ARMY:         info = " 增援的不是集结部队"; break;
                case ErrorCode.RALLY_REINFORCE_TARGET_NO_EXIST:        info               = " 增援的目标不存在"; break;
                case ErrorCode.RALLY_HAD_REINFORCE:       info                            = " 已经进行了增援,无法继续增援"; break;
               // case ErrorCode.RALLY_ALLIANCE_CENTER_ARMY_LIMIT:      info                = " 超过联盟中心援助上限"; break;
               // case ErrorCode.RALLY_TARGET_NOT_FOUND:info                                = " 集结目标不存在"; break;
                case ErrorCode.RALLY_REINFORCE_FAIL:    info                              = " 增援目标失败"; break;
                case ErrorCode.RALLY_INVALID_TARGET: info                                 = "无效的集结目标"; break;
               // case ErrorCode.RALLY_JOIN_ON_ARMY_MATCH: info                             = "集结部队已经出发,无法再加入集结"; break;
                case ErrorCode.RALLY_HAD_REINFORCE_TARGET: info                           = "已经增援了此目标,无法增援"; break;
               // case ErrorCode.RALLY_HAD_JOIN_TARGET_REINFORCE:   info                    = "已经加入了此目标,无法增援"; break;
               // case ErrorCode.RALLY_REINFORCE_CITY_FAIL: info                            = "增援城市失败"; break;
                //case ErrorCode.RALLY_REPATRIATION_REINFORCE_FAIL:     info                = "遣返失败"; break;

                // 商栈相关
                case ErrorCode.TRANSPORT_MAX_NUM:     info = " 运输资源超出商栈上限"; break;
                // case ErrorCode.TRANSPORT_TARGET_NOT_BUILDING:      info = " 对方没有商栈建筑"; break;
                case ErrorCode.TRANSPORT_TROOP_FULL: info = " 部队已满"; break;
                case ErrorCode.TRANSPORT_NOT_BUILDING: info = " 没有商栈建筑，无法运输"; break;


                // 远征相关
                case ErrorCode.EXPEDITION_NO_OPEN: info = " 远征系统未开启"; break;
                case ErrorCode.EXPEDITION_PRE_LEVEL_NOT_PASS: info = " 前置关卡未通关"; break;
                case ErrorCode.EXPEDITION_TROOPS_NUM_LESS: info = "  部队数量小于1"; break;
                case ErrorCode.EXPEDITION_TROOPS_FULL: info = "  部队数量超出上限"; break;
                case ErrorCode.EXPEDITION_HERO_NOT_FREE: info = " 远征英雄非空闲状态"; break;
                case ErrorCode.EXPEDITION_SOLDIER_NOT_ENOUGH: info = " 士兵不足"; break;
                case ErrorCode.EXPEDITION_ATTACK_SELF_ARMY: info = "  不能攻击自己的队伍"; break;
                case ErrorCode.EXPEDITION_SOLDIER_TOO_MUCH: info = " 远征士兵太多"; break;
                case ErrorCode.EXPEDITION_ARMY_NOT_EXIST: info = "  队伍不存在"; break;
                case ErrorCode.EXPEDITION_STARTING: info = " 当前已经有一场远征战斗"; break;
            }
            return info;
        }

    }


    public class NetProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "NetProxy";

        public SprotoSocketAp netClient;

        public Action<SprotoSocketAp> OnConnection;

        private ELoginState m_CrrNetState = ELoginState.EAuth1;
        
        
        private string m_userName;
        private string m_password;
        private string m_serverIP;
        private int m_serverPort;

        private string m_serverNode;

        #endregion

        // Use this for initialization
        public NetProxy(string proxyName)
            : base(proxyName)
        {
        }


        public override void OnRegister()
        {
            Debug.Log(" NetProxy register");
        }

        public override void OnRemove()
        {
            Debug.Log(" NetProxy remove");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameOrIggID"></param>
        /// <param name="passwordOrIggSdkToken"></param>
        public void Connection()
        {
            var _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            Debug.LogFormat("链接网络 user:[{0}]   pwd:[{1}] serverIP:「{2}」 serverPort:[{3}] serverNode :{4}", this.MUserName, this.MPassword,this.MServerIp,this.MServerPort,this.MServerNode);
            netClient = SprotoSocketAp.CreateInstance(this.MServerIp, MServerPort, OnNetEvent, OnAuthEvent);

            string ip = IGGSDKConstant.IGGDefault.AppConfigIP;
            if (IGGSDK.appConfig != null)
            {
                Debug.LogFormat("IGGSDK ip0:[{0}]", ip);
                ip = IGGSDK.appConfig.getClientIp();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = "127.0.0.1";
                }
                 Debug.LogFormat("IGGSDK ip1:[{0}]", ip);
            }
            Debug.LogFormat("IGGSDK config is null [{0}]",ip);
            string platform = "3";
#if UNITY_IOS
            platform = "1";
#elif UNITY_ANDROID
            platform = "2";
#endif
            var language = ((int)LanguageUtils.GetLanguage()).ToString();

            netClient.setLoginInfo(this.MUserName, this.MPassword, platform, language, ip, this.MServerNode);

            var responConfig = CoreUtils.dataService.QueryRecords<BacksourcingRestrictDefine>();
            
            responConfig.ForEach((config) =>
            {
                if (config.agreementTag>0)
                {
                    netClient.AddSprotoRespineCheckConfig(config.agreementTag,config.l_descID);
                }
            });
            
            OnConnection?.Invoke(netClient);
        }
        
        public void SaveLoginInfo(string serverIp, int port, string userName ,string passWord,string serverNode)
        {
            m_userName = userName;
            m_password = passWord;
            m_serverIP = serverIp;
            m_serverPort = port;
            m_serverNode = serverNode;
        }
        
        public string MUserName => m_userName;

        public string MPassword
        {
            get => m_password;
        }
        
        public string MServerNode
        {
            get => m_serverNode;
        }

        public string MServerIp => m_serverIP;
        public int MServerPort => m_serverPort;



        public void SendSproto(SprotoTypeBase obj)
        {
            if (netClient == null)
            {
                return;
            }
            netClient.SendSproto(obj);
        }

        //开启心跳包发送
        public void SetHeartSend()
        {
            if (netClient == null)
            {
                return;
            }
            netClient.SetHeartSend();
        }

        public void OnAuthEvent(ELoginState state,int errorCode)
        {
            m_CrrNetState = state;
            AppFacade.GetInstance().SendNotification(CmdConstant.AuthEvent, state,errorCode.ToString());
            if(netClient.IsNetworkingViewerEnable)
            {
                netClient.OnLoginStateChange?.Invoke(state, errorCode);
            }
        }

        private void OnNetEvent(NetEvent @event,int error)
        {
            Task.RunInMainThread(() =>
            {
                
                AppFacade.GetInstance().SendNotification(CmdConstant.NetEvent, @event,error.ToString());
                if (netClient.IsNetworkingViewerEnable)
                {
                    netClient.OnNetStateChange?.Invoke(@event, error);
                }
            });
        }

       

        public ELoginState GetLoginState()
        {
            return m_CrrNetState;
        }

        /// <summary>
        /// 重连游戏服务器
        /// </summary>
        public void ReConnectGameServer()
        {
            Debug.Log("开始重连");
            netClient.ReConnectGameSever();
        }

        public void CloseGameServer()
        {
            Debug.Log("断开链接");
            if (netClient != null)
            {
                netClient.Close();
            }
        }

        // 重定向游戏服务器
        public void RedirectGameServer()
        {
            netClient.RedirectGameServer();
        }
    }
}