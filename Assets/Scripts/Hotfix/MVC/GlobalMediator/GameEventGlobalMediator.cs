// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, December 27, 2019
// Update Time         :    Friday, December 27, 2019
// Class Description   :    ServerPushMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine.Events;

namespace Game {

    enum DayNightSwitch
    {
        DAWN,           //黎明
        DAWN_TO_DAY,    //黎明->白天
        DAY,            //白天
        DAY_TO_DUST,    //白天->黄昏
        DUST,           //黄昏
        DUST_NIGHT,     //黄昏->夜晚
        NIGHT,          //夜晚
        AUTO,           //夜晚->黎明
    }
    
    public class GameEventGlobalMediator : GameMediator {
        
        public static string NameMediator = "GameEventGlobalMediator";

        #region 昼夜交替
        private int[] m_lighttingGuideTime = new[] { 2, 2, 2, 2, 2, 2, 2, 2 };
        private int[] m_lighttingTime = new[] {57, 3, 1677, 3, 57, 3, 597, 3};
        private Color[][] lightSetting = new []
        {
            new []{ new Color(0.6470588f, 0.6470588f, 0.6470588f), new Color(0.54509803921569f, 0.30980392156863f, 0.14117647058824f)},
            new []{ new Color(0.53676474f, 0.53676474f, 0.53676474f), new Color(0.7137f, 0.7137f, 0.7137f)},
            new []{ new Color(0.6470588f, 0.6470588f, 0.6470588f),new Color(0.54509803921569f, 0.30980392156863f, 0.14117647058824f)},
            new []{ new Color(0.117647f, 0.494117f, 0.964705f), new Color(0.30196f, 0.411764f, 0.737254f)}
        };

        private float[] m_lightSettingValue = new[] {0.85f, 1f, 0.85f, 0.85f};

        private int m_currentLighttingModeIndex = -1;
        private int m_time = 0;

        private int m_loopTime = 0;
        
        private int[] m_lightStepAccumTime = new int[8];

        private bool m_isForceDayNight = false;


        private bool m_isInit = false;


        private bool m_isDispose = false;

        #endregion

        #region 网络错误处理

        private NetProxy m_netProxy;
        private PlayerProxy m_playerProxy;
        private ChatProxy m_chatProxy;
        
        private bool m_hasShowNetError;
        
        //网络错误提示框
        private Alert m_NetError;


        private bool m_isReConnecting = false;
        private int m_reConnectingCount = 0;

        private int m_serverCloseReason = -1;

        private Timer m_dayNightTimer;

        private GameObject m_applicationFocusNode;

        #endregion


        //IMediatorPlug needs
        public GameEventGlobalMediator( ):base(NameMediator, null ) {}

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.DayNightTimeTick,
                CmdConstant.NetEvent,
                System_KickConnect.TagName,
                CmdConstant.AuthEvent,
                CmdConstant.TestReLogin,
                CmdConstant.ReRoleLogin,
                CmdConstant.SwitchDayNight,
                CmdConstant.DayNightInit,
                Role_RoleLogin.TagName,
            }.ToArray();
        }

       public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.DayNightInit:
                    DayNightInit();
                    break;
                case CmdConstant.DayNightTimeTick:
                    DayNightNtf();
                    break;
                    
                case CmdConstant.NetEvent:
                    ServerNetEvent((NetEvent)notification.Body,notification.Type);
                    break;
                
                case System_KickConnect.TagName:
                    System_KickConnect.request req = notification.Body as System_KickConnect.request;

                    ServerClose(req);
                    break;
                case CmdConstant.AuthEvent:
                    ELoginState state = (ELoginState) notification.Body;
                    Debug.LogFormat("HandleNotification AuthEvent [{0}]",state);
                    switch (state)
                    {
                        case ELoginState.EAuthError:
                            //登录出错
                            int errorCode = 0;
                            int.TryParse(notification.Type, out errorCode);
                            if (errorCode == (int)AuthError.UserBan)
                            {
                                SendNotification(CmdConstant.AccountBan);
                            }
                            else if (errorCode == (int)AuthError.ToeknExpire)
                            {
                                ShowErrorAlert(100072, GoLogin);
                            }
                            else
                            {
                                // 策划要求 100015 修改成 100028
                                ShowErrorAlert(100028, GoLogin, int.Parse(notification.Type));
                            }
                            break;
                        case ELoginState.EGameServerOK:
                            EndReConnecting();
                            //重连后业务
                            if (m_serverCloseReason > 0)
                            {
                                m_serverCloseReason = 0;
                                m_playerProxy.LoginRole(m_playerProxy.CurrentRoleInfo.rid);
                            }
                            break;
                    }
                    break;
                case CmdConstant.TestReLogin:
                    // 策划要求 100015 修改成 100028
                    ShowErrorAlert(100028, GoLogin, 0);
                    break;
                case CmdConstant.ReRoleLogin:
                    // 策划要求 100015 修改成 100028
                    ShowErrorAlert(100028, GoLogin, (int)notification.Body);
                    break;
                case CmdConstant.SwitchDayNight://切换白天夜晚
                    int switchId = (int)notification.Body;
                    ForceSwitchDayNight(switchId);
                    break;
                case Role_RoleLogin.TagName:
                    m_playerProxy.SaveRoleLoginRes(notification.Body as Role_RoleLogin.response);
                    break;
                default:
                    break;
            }
           
        }

        #region UI template method          

        protected override void InitData()
        {
            m_netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;

            m_applicationFocusNode = new GameObject("OnApplicationFocusNode");
            var handler = m_applicationFocusNode.AddComponent<Client.OnGuiHandler>();
            handler.SetApplicationFocus(OnApplicationFocus);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnApplicationFocus, hasFocus);
        }

        protected override void BindUIEvent()
        {
            CoreUtils.uiManager.SetExitGame(onExitGame);
        }

        private void onExitGame()
        {
            SendNotification(CmdConstant.ExitGame);
        }


        protected override void BindUIData()
        {

        }

        public override void Update()
        {

        }

        public override void LateUpdate()
        {
            
        }

        public override void FixedUpdate()
        {

        }

        public override void OnRemove()
        {
            base.OnRemove();
            m_isDispose = true;
            if (m_dayNightTimer != null)
            {
                m_dayNightTimer.Cancel();
                m_dayNightTimer = null;
            }
            if (m_applicationFocusNode != null)
            {
                GameObject.Destroy(m_applicationFocusNode);
                m_applicationFocusNode = null;
            }
        }

        #endregion


        #region 网络断线相关处理

        //服务器主动下发断线处理
        public void ServerClose(System_KickConnect.request req )
        {
            Debug.Log("服务器主动断线 原因:"+req.reason);
            
            m_serverCloseReason = (int)req.reason;
            switch (req.reason)
            {
                case 1:
                    //心跳超时会被服务器踢出
                    Debug.Log("心跳超时会被服务器踢出");
                    TryReConnecting();
                    break;
                case 2:
                    //服务器即将关闭 服务器关闭
                    Debug.Log("服务器即将关闭 服务器关闭");
                    ShowErrorAlert(100033,GoLogin);
                    break;
                case 3: 
                    //被顶号 您的账号在其他设备登陆
                    ShowErrorAlert(100030,ExitGame); 
                    break;
                case 4:
                    //账号被封禁
                    SendNotification(CmdConstant.AccountBan);
                    break;
                case 5:
                    //sdk accesstoken失效
                    SDKSession.invalidateCurrentSession();
                    ShowErrorAlert(100072,GoLogin);
                    break;
                case 6: // 移民踢出
                    ShowErrorAlert(100072, GoLogin);
                    break;
                case 7:
                    //运营踢出
                    ShowErrorAlert(100304, GoLogin);
                    break;
            }
        }

        //本地检测到断线处理
        public void ServerNetEvent(NetEvent @event,string errStr)
        {
            CoreUtils.logService.Info(m_netProxy.GetLoginState()+"游戏层接收到网络状态改变处理" + @event+"  errorCode:"+errStr+"  "+Application.internetReachability);
            
            int errorCode = int.Parse(errStr);
            switch (@event)
            {
                
                case NetEvent.ConnectFail:
                    switch (@m_netProxy.GetLoginState() )
                    {
                        case ELoginState.EAuth1:
                            if (NetworkReachability.NotReachable == Application.internetReachability)
                            {
                                //本地没有网络  100028 网路不通畅，请尝试重新连接，错误码
                                ShowErrorAlert(100044,TryReConnecting,105);
                            }
                            else
                            {
                                if (errorCode > 0 )
                                {
                                    ShowErrorAlert(100028,TryReConnecting,errorCode);
                                }
                                else
                                {
                                    //账号服务器关闭
                                    ShowErrorAlert(100031,GoLogin);
                                }
                            }
                            break;
                        case ELoginState.EAuth2:
                        case ELoginState.EAuth3:
                            //登录的时候验证时候没网络需要重新链接
                            TryReConnecting();
                            break;
                        case ELoginState.ERedirectionGameServer:
                            Debug.Log("无法连接游戏服务器ERedirectionGameServer");
                            if (NetworkReachability.NotReachable == Application.internetReachability)
                            {
                                //本地没有网络  100028 网路不通畅，请尝试重新连接，错误码
                                ShowErrorAlert(100044,TryReConnecting,206);
                            }
                            else
                            {
                                if (errorCode > 0 )
                                {
                                    ShowErrorAlert(100028,TryReConnecting,errorCode);
                                }
                                else
                                {
                                    //账号服务器关闭
                                    ShowErrorAlert(100031,GoLogin);
                                }
                            }
                            break;
                        default:
                            Debug.Log(@event+" 链接状态未处理 "+@m_netProxy.GetLoginState());
                            break;
                    }
                    
                    break;
                case NetEvent.DisconnectedComplete:
                    switch (@m_netProxy.GetLoginState())
                    {
                        case ELoginState.EGameServerOK:

                            if (m_serverCloseReason < 2)
                            {
                                if (errorCode == 0 && m_serverCloseReason ==-1)
                                {
                                    m_serverCloseReason = 100;//主动网络被关闭
                                }

                                if (errorCode == 0 && m_serverCloseReason == 0){
                                    m_serverCloseReason = 101;//后台超时
                                }

                                Debug.Log(m_serverCloseReason+"游戏服务器断开连接 网路不通畅，请尝试重新连接"+errorCode);
                                TryReConnecting();
                            }
                            else
                            {
                                Debug.Log("游戏服务器关闭不处理 原因"+m_serverCloseReason);
                            }

                            break;
                        case ELoginState.EAuth2:
                        case ELoginState.EAuth3:
//                            m_netProxy.CloseGameServer();//
//                            m_netProxy.Connection();
                            
                            ShowErrorAlert(100028,TryReConnecting,999);
                            
                            break;
                    }
                    break;
                case NetEvent.ReconnectFail:
                    TryReConnecting();
                    break;
            }
        }


        private long m_disconnecTime = 0;

        private void TryReConnecting()
        {
            CloseNetError();
            
            if (NetworkReachability.NotReachable == Application.internetReachability)
            {
                Debug.Log("没有网络需要重连");
                ShowErrorAlert(100044,TryReConnecting);
            }
            else
            {
                Debug.Log("有网络,3秒后自动尝试重连 :"+m_reConnectingCount);
                if (StartReConnecting())
                {
                    if (m_reConnectingCount == 1)
                    {
                        m_disconnecTime = DateTime.Now.Ticks;
                        Debug.Log("发送系统重连事件"+m_disconnecTime/60*10000000L);
                        AppFacade.GetInstance().SendNotification(CmdConstant.NetWorkReconnecting);

                        if (!GuideProxy.IsGuideing)
                        {
                            CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                            CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                            CoreUtils.uiManager.CloseLayerUI(UILayer.WindowMenuLayer);
                            CoreUtils.uiManager.CloseUI(UI.s_funcGuide);
                        }  
                    }
                    
                    
                    Timer.Register(3, () =>
                    {
                        if (@m_netProxy.GetLoginState() == ELoginState.EAuth1 ||
                            @m_netProxy.GetLoginState() == ELoginState.EAuth2
                        )
                        {
                            Debug.Log("在登录界面网络层重连");
                            m_netProxy.Connection();
                        }
                        else if (@m_netProxy.GetLoginState() == ELoginState.ERedirectionGameServer)
                        {
                            Debug.Log(m_disconnecTime+" 在游戏内 重载游戏 "+(DateTime.Now.Ticks-m_disconnecTime)/60*10000000L);

                            if (DateTime.Now.Ticks-m_disconnecTime>15*60*10000000L)
                            {
                                 SendNotification(CmdConstant.ReloadGame);
                            }
                            else
                            {
                                m_netProxy.ReConnectGameServer();
                            }

                        }
                        else
                        {
                            Debug.Log("重连服务器中");
                            m_netProxy.ReConnectGameServer();
                        }
                    });
                }
                else
                {
                    //网路不通畅，请尝试重新连接
                    ShowErrorAlert(100032,GoLogin);
                }
            }
        }

        //开始转菊花
        private void EndReConnecting()
        {
            if (m_isReConnecting)
            {
                m_isReConnecting = false;
                m_reConnectingCount = 0;
                CoreUtils.uiManager.CloseUI(UI.s_reConnecting);
                Debug.Log("结束重连状态");
            }
        }

        private bool StartReConnecting()
        {
            if (m_isReConnecting==false)
            {
                m_isReConnecting = true;
                CoreUtils.uiManager.ShowUI(UI.s_reConnecting);
            }
            m_reConnectingCount++;
            return m_reConnectingCount < 4;
        }

      
        //确认返回登录
        private void GoLogin()
        {
            CloseNetError();
            SendNotification(CmdConstant.ReloadGame);
        }
        
        //退出游戏
        private void ExitGame()
        {
            Debug.Log("退出游戏");
            Application.Quit();
        }

        private void CloseNetError()
        {
            m_hasShowNetError = false;
            if (m_NetError!=null)
            {
                m_NetError.DestroySelf();
                m_NetError = null;
            }
        }


        private void ShowErrorAlert(int errorLanID,UnityAction okCallBack,int subError=0)
        {
            EndReConnecting();
            
            if (m_hasShowNetError)
            {
                Debug.Log("已经显示网络状态了 "+errorLanID);
                return;
            }

            if (subError>0)
            {
                m_NetError = Alert.CreateAlert(LanguageUtils.getTextFormat(errorLanID,subError),LanguageUtils.getText(100035)).SetLeftButton(okCallBack,LanguageUtils.getText(100036)).Show();
                m_NetError.CanAndroidBack(false);
            }
            else
            {
                m_NetError = Alert.CreateAlert(errorLanID,LanguageUtils.getText(100035)).SetLeftButton(okCallBack,LanguageUtils.getText(100036)).Show();
                m_NetError.CanAndroidBack(false);
            }

            m_hasShowNetError = true;
        }
        
        #endregion

        #region 昼夜交替

         //是否白天
        public bool IsDay()
        {
            return m_currentLighttingModeIndex < 5;
        }

        public void DayNightNtf()
        {
            if (m_dayNightTimer == null)
            {
                m_dayNightTimer = Timer.Register(2, () =>
                {
                    if (GuideProxy.IsLightingGuideControl)
                    {
                        return;
                    }
                    CalcDayNight();
                }, null, true);
            }
            if (GuideProxy.IsLightingGuideControl)
            {
                return;
            }
            CalcDayNight();
        }

        public void DayNightInit()
        {
            if (m_isInit == false)
            {
                m_isInit = true;

                for (int i = 0; i < m_lighttingTime.Length; i++)
                {
                    m_lightStepAccumTime[i] = m_loopTime;
                    m_loopTime += m_lighttingTime[i];
                }
                m_currentLighttingModeIndex = -1;
            }
        }

        public int GetCurrLightIndex()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 mTime = serverTime % m_loopTime;

            int index = 0; 
            for (int i = m_lightStepAccumTime.Length - 1; i >= 0; i--)
            {
                if (m_lightStepAccumTime[i] <= mTime)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public void CalcDayNight(int forceIndex = -1)
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            
            if (!m_isInit || serverTime<1)
            {
                return;
            }

            int index = 0;
            int passedTime = 0;

            Int64 mTime = serverTime % m_loopTime;
            bool isForce = false;

            if (GuideProxy.IsLightingGuideControl)
            {
                index = forceIndex;
                passedTime = 0;
                isForce = true;
            }
            else
            {
                for (int i = m_lightStepAccumTime.Length - 1; i >= 0; i--)
                {
                    if (m_lightStepAccumTime[i] <= mTime)
                    {
                        index = i;
                        passedTime = (int)mTime - m_lightStepAccumTime[i];
                        break;
                    }
                }
                isForce = forceIndex > -1;
                if (m_isForceDayNight && !isForce)
                {
                    return;
                }
                if (isForce && forceIndex != (int)DayNightSwitch.AUTO)
                {
                    index = forceIndex;
                }
            }
 
            int oldIndex = m_currentLighttingModeIndex;
            if (m_currentLighttingModeIndex != index || isForce)
            {
                m_currentLighttingModeIndex = index;
                if (m_currentLighttingModeIndex%2 == 1 || oldIndex == -1 || isForce)
                {
                    int light_index = (int)Mathf.Ceil((m_currentLighttingModeIndex+1) / 2f);
                    int next_light_index = (int)Mathf.Ceil((m_currentLighttingModeIndex+2) / 2f);

                    if (lightSetting.Length < next_light_index)
                    {
                        next_light_index = 1;
                    }
                    
                    var org_ambient_color = lightSetting[light_index-1][0];
                    var org_direction_color = lightSetting[light_index-1][1];
                    var org_direction_intensity = m_lightSettingValue[light_index-1];
                    var new_ambient_color = lightSetting[next_light_index-1][0];
                    var new_direction_color = lightSetting[next_light_index-1][1];
                    var new_direction_intensity = m_lightSettingValue[next_light_index-1];
                    
                    LightManager.Instance().UpdateLighting(
                        org_ambient_color,
                        org_direction_color,
                        org_direction_intensity,
                        new_ambient_color,
                        new_direction_color,
                        new_direction_intensity,
                        m_lighttingTime[m_currentLighttingModeIndex],
                        passedTime
                        );
                    
                    Debug.Log(IsDay()+" 昼夜交替变色 index:"+ m_currentLighttingModeIndex+" light_index:"+light_index+"  next_light_index:"+next_light_index);
                }

                if (m_currentLighttingModeIndex % 2 !=1 || oldIndex == -1)
                {
                    if (IsDay())
                    {
                        LightManager.Instance().SetIsNight(false);
                    }
                    else
                    {
                        LightManager.Instance().SetIsNight(true);
                    }
                }
                
                Debug.Log(IsDay()+" 昼夜交替 index:"+ m_currentLighttingModeIndex+" passedTime:"+passedTime+"  轮询内:"+mTime+"  服务器时间:"+serverTime+"  循环时间:"+m_loopTime+"  "+ServerTimeModule.Instance.GetServerTime());
                
                AppFacade.GetInstance().SendNotification(CmdConstant.DayNightChange,IsDay());
            }
        }


        public void ForceSwitchDayNight(int forceIndex)
        {
            //if (forceIndex == (int)DayNightSwitch.DAWN || forceIndex == (int)DayNightSwitch.DAY || forceIndex == (int)DayNightSwitch.DUST || forceIndex == (int)DayNightSwitch.NIGHT)
            //{
            //    m_isForceDayNight = true;
            //}
            //else
            //{
            //    m_isForceDayNight = false;
            //    forceIndex = (int)DayNightSwitch.AUTO;
            //}
            //GuideProxy.IsLightingGuideControl = true;
            CalcDayNight(forceIndex);
        }

        #endregion
        
       
    }
}