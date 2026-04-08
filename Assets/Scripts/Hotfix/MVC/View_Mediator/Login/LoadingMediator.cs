// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, February 10, 2020
// Update Time         :    Monday, February 10, 2020
// Class Description   :    LoadingMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class LoadingMediator : GameMediator {
        #region Member
        public static string NameMediator = "LoadingMediator";

        private long m_hotfixTotalSize = 0;
        private long m_unCompressTotalCount = 0;

        #endregion

        //IMediatorPlug needs
        public LoadingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public LoadingView view;

        private int mapObjPro = -1;
        private int buildObjPro = 0;


        
        public static float PRO_NETEVENT = 0.15f;
        public static float PRO_AUTHEVENT = 0.16f;
        public static float PRO_ROLEINFO = 0.19f;
        public static float PRO_MAPINFO = 0.20f;
        public static float PRO_MAP_SPET = 0.8f / 80;
        
        
        
       

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.LoadAppConfig, // 请求配置
                CmdConstant.PackageUpdateCheck, // 检测商店更新
                CmdConstant.AutoLogin, // 账号登陆
                CmdConstant.AgreementCheck, // 用户协议检测
                CmdConstant.HotfixUpteCheck, // 热更新检测 
                CmdConstant.HotfixStartDownload,
                CmdConstant.HotfixDownloadProgress,
                CmdConstant.HotfixUnCompress,
                CmdConstant.HotfixUnCompressProgress,
                CmdConstant.MaintainCheck, // 维护公告检测
                CmdConstant.LoginToServer, // 开始连接服务器（本界面处理）
                CmdConstant.NetEvent, // 账号服务器连接成功 NetEvent.ConnectComplete
                CmdConstant.AuthEvent, // 账号服务器校验EAuth1 EAuth2 EAuth3 ERedirectionGameServer EGameServerOK
                Role_GetRoleList.TagName, // 获取完角色信息
                Role_RoleLogin.TagName, // 登陆完毕
                Map_ObjectInfo.TagName,
                CmdConstant.FirstEnterCityStartEndter
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                //-----------------------以下是登陆流程相关----------------------------
                case CmdConstant.LoadAppConfig: // 请求配置
                    {
                        SetProgress(0.0f, true);
                        SetProgress(0.05f);
                    }
                    break;
                case CmdConstant.PackageUpdateCheck: // 检测商店更新
                    {
                        SetProgress(0.1f);
                    }
                    break;
                case CmdConstant.AutoLogin: // 账号登陆
                    {
                        SetProgress(0.15f);
                    }
                    break;
                case CmdConstant.AgreementCheck: // 用户协议检测
                    {
                        SetProgress(0.2f);
                    }
                    break;
                case CmdConstant.HotfixUpteCheck: // 热更新检测 
                    {
                        SetProgress(0.25f);
                    }
                    break;
                case CmdConstant.MaintainCheck: // 维护公告检测
                    {
                        SetProgress(0.3f);
                    }
                    break;
                case CmdConstant.LoginToServer: // 开始连接服务器（本界面处理）
                    {
                        SetProgress(0.35f);
                        PreLoadRes();
                    }
                    break;
                case CmdConstant.NetEvent: // 账号服务器连接成功 NetEvent.ConnectComplete
                    {
                        NetEvent netEvent = (NetEvent)notification.Body;

                        switch (netEvent)
                        {
                            case NetEvent.ConnectComplete:
                                //SetProgress(0.35f, true);
                                //SetProgress(0.4f);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case CmdConstant.AuthEvent: // 账号服务器校验EAuth1 EAuth2 EAuth3 ERedirectionGameServer EGameServerOK
                    {
                        ELoginState state = (ELoginState)notification.Body;
                        Debug.LogFormat("HandleNotification AuthEvent loadingMediator [{0}]",state);
                        switch(state)
                        {
                            case ELoginState.EAuth1:
                                {
                                    SetProgress(0.45f);
                                }
                                break;
                            case ELoginState.EAuth2:
                                {
                                    SetProgress(0.5f);
                                }
                                break;
                            case ELoginState.EAuth3:
                                {
                                    SetProgress(0.55f);
                                }
                                break;
                            case ELoginState.ERedirectionGameServer:
                                {
                                    SetProgress(0.6f);
                                }
                                break;
                            case ELoginState.EGameServerOK:
                                {
                                    //校验成功
                                    var _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                                    _playerProxy.GetRoleList();
                                    SetProgress(0.6f, true);
                                    SetProgress(0.65f);
                                }
                                break;
                        }
                    }
                    break;
                case Role_GetRoleList.TagName: // 获取完角色信息
                    {
                        SetProgress(0.8f);

                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            if (error.errorCode == (long)ErrorCode.ROLE_BAN)
                            {
                                SendNotification(CmdConstant.AccountBan);
                                return;
                            }
                        }
                        else
                        {
                            //RoleInfoHelp.DeleteNewCreateRole();
                            Role_GetRoleList.response response = notification.Body as Role_GetRoleList.response;
                            var m_RoleInfoProxy = AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
                            m_RoleInfoProxy?.UpdateRoleInfoData(notification);
                            if (!RoleInfoHelp.GetIsNewCreateRole())
                            {
                                if (response.HasRoleInfos && response.roleInfos.Count > 0)
                                {
                                    var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                                    int serverId = RoleInfoHelp.GetServerId(netProxy?.netClient.ServerName);
                                    List<RoleInfo> lsRoleInfo = new List<RoleInfo>();
                                    foreach (var info in response.roleInfos.Values)
                                    {
                                        int gameId = RoleInfoHelp.GetServerId(info.gameNode);
                                        if (gameId == serverId)
                                        {
                                            lsRoleInfo.Add(info);
                                        }
                                    }

                                    lsRoleInfo.Sort(delegate(RoleInfo info, RoleInfo roleInfo)
                                    {
                                        if (info.lastLoginTime > roleInfo.lastLoginTime)
                                        {
                                            return -1;
                                        }

                                        if (info.lastLoginTime < roleInfo.lastLoginTime)
                                        {
                                            return 1;
                                        }

                                        return 0;
                                    });

                                    if (lsRoleInfo.Count > 0)
                                    {
                                        long rid = lsRoleInfo[0].rid;
                                        var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                                        playerProxy?.LoginRole(rid);
                                    }
                                    else
                                    {
                                        OnShowCreateCharacterPanel();
                                    }
                                }
                                else
                                {
                                    OnShowCreateCharacterPanel();
                                }
                            }
                            else
                            {
                                OnShowCreateCharacterPanel();
                            }
                        }
                    }
                    break;
                case Role_RoleLogin.TagName: // 登陆完毕
                    {
                        SetProgress(0.9f);
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            Debug.LogError($"RoleLoginError:{error.errorCode}");
                            SendNotification(CmdConstant.ReRoleLogin, (int)error.errorCode);
                        }
                        else
                        {
                            var loginResponse = notification.Body as Role_RoleLogin.response;
                            if (loginResponse != null)
                            {
                                ServerTimeModule.Instance.SetServerZone(loginResponse.timezone);
                            }
                            var net = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                            net.SetHeartSend();
                            Debug.Log("迷雾切换到主城");
                            AppFacade.GetInstance().SendNotification(CmdConstant.SwitchMainCityCmd);
                        }
                    }
                    break;
                case Map_ObjectInfo.TagName:
                    {
                        SetProgress(1.0f);
                    }
                    break;
                case CmdConstant.FirstEnterCityStartEndter:
                    {
                        SetProgress(1.0f);
                        //var timer1 = Timer.Register(0.3f, () =>
                        //{
                        //    CoreUtils.uiManager.CloseUI(UI.s_Loading);
                        //});
                        CoreUtils.uiManager.ShowUI(UI.s_mainInterface, () => {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MainViewFirstInitEnd);
                            if (notification.Body != null)
                            {
                                CoreUtils.uiManager.CloseUI(notification.Body as UIInfo);
                            }
                        });
                    }
                    break;
                    //-----------------------以下是热更新下载相关----------------------------
                case CmdConstant.HotfixStartDownload:
                    {
                        m_hotfixTotalSize = (long)notification.Body;
                        SetProgress(0.0f, true);
                        view.m_lbl_doing_LanguageText.text = LanguageUtils.getTextFormat(100062, HotfixUtil.FormatFileSize(0), HotfixUtil.FormatFileSize(m_hotfixTotalSize));
                    }
                    break;
                case CmdConstant.HotfixDownloadProgress:
                    {
                        var nSize = (long)notification.Body;
                        SetProgress((float)nSize/ (float)m_hotfixTotalSize, true);
                        view.m_lbl_doing_LanguageText.text = LanguageUtils.getTextFormat(100062, HotfixUtil.FormatFileSize(nSize), HotfixUtil.FormatFileSize(m_hotfixTotalSize));
                    }
                    break;
                case CmdConstant.HotfixUnCompress:
                    {
                        m_unCompressTotalCount = (long)notification.Body;
                        SetProgress(0.0f, true);
                        view.m_lbl_doing_LanguageText.text = LanguageUtils.getTextFormat(100063, 0, m_unCompressTotalCount);
                    }
                    break;
                case CmdConstant.HotfixUnCompressProgress:
                    {
                        var nCount = (long)notification.Body;
                        SetProgress((float)nCount / (float)m_unCompressTotalCount, true);
                        view.m_lbl_doing_LanguageText.text = LanguageUtils.getTextFormat(100063, nCount, m_unCompressTotalCount);
                    }
                    break;
                    //-----------------------以上是热更新下载相关----------------------------
            }
        }
        
        
        private void OnShowCreateCharacterPanel()
        {
            CoreUtils.audioService.StopByName(RS.SoundLogin);
            CoreUtils.logService.Info("没有角色开始创建角色");
            CoreUtils.uiManager.ShowUI(UI.s_CreateCharacter);
        }
        

        private void PreLoadRes()
        {
            var preConfigs = CoreUtils.dataService.QueryRecords<LoadingPreloadDefine>();
            var preLoadTotalCount = preConfigs.Count;
            int preLoadedCount = 0;
            preLoadTotalCount = 0; // 暂时不预加载
            if (preLoadTotalCount == 0)
            {
                StartConnectServer();
                return;
            }
            for (int i = 0; i < preLoadTotalCount; i++)
            {
                var loadingPreloadDefine = preConfigs[i];
                CoreUtils.assetService.LoadAssetAsync<GameObject>(loadingPreloadDefine.res, (asset =>
                {
                    preLoadedCount++;
                    if (preLoadedCount >= preLoadTotalCount)
                    {
                        StartConnectServer();
                    }
                }));
            }
        }


        private void StartConnectServer()
        {
            Timer.Register(0.1f, () =>
            {
                // 开始连接服务器
                var nexProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                nexProxy.Connection();
            });
        }

        private void LoadingTip()
        {
            
            int allWeight = 0;
            List<LoadTipsDefine> tipsDefines = CoreUtils.dataService.QueryRecords<LoadTipsDefine>();
            foreach (LoadTipsDefine define in tipsDefines)
            {
                allWeight += define.chance;
            }

            int tipID = 0;
            float random =Random.Range(0f, 1f);

            for (int i = tipsDefines.Count - 1; i >= 0; i--)
            {
                LoadTipsDefine matchingDefine = tipsDefines[i];
                float weight = (float)tipsDefines[i].chance / allWeight * i;
                if (random >= weight)
                {
                    tipID = matchingDefine.l_tipId;
                    break;
                }
            }
            if (tipID != 0)
            {
                view.m_lbl_Tip_LanguageText.text = LanguageUtils.getText(tipID);
            }
           
            
        }

        private float m_crrPro = 0.0f;

        private void AlertWarn(string tip){
            Alert.CreateAlert(tip, LanguageUtils.getText(100035)).SetRightButton(goBack,LanguageUtils.getText(100036)).Show();
        }

        private void goBack()
        {
            Login();
        }


        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus()
        {
            WorldCamera.Instance().SetCanZoom(false);
        }

        public override void WinClose(){
            if (!GuideProxy.IsGuideing)
            {
                WorldCamera.Instance().SetCanZoom(true);
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            if (view.m_pb_rogressBar_GameSlider.value < m_crrPro)
            {
                view.m_pb_rogressBar_GameSlider.value = view.m_pb_rogressBar_GameSlider.value + 0.01f;
                if(view.m_pb_rogressBar_GameSlider.value > m_crrPro)
                {
                    view.m_pb_rogressBar_GameSlider.value = m_crrPro;
                }
                view.m_lbl_doing_LanguageText.text = LanguageUtils.getTextFormat(100034, (int)(view.m_pb_rogressBar_GameSlider.value*100));
            }            
        }        

        protected override void InitData()
        {
            IsOpenUpdate = true;
            var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            if (netProxy != null)
            {
                netProxy.CloseGameServer();
            }

            // 移除掉一开始设置的默认背景图片
            var defaultBg = GameObject.Find("UIRoot/Container/DefaultBg");
            if (defaultBg != null)
            {
                GameObject.Destroy(defaultBg.gameObject);
            }
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            Login();
            ClientUtils.LoadSprite(view.m_img_logo_PolygonImage, HotfixUtil.getLanguageImage(1000), false, () =>
            {
                 Timer.Register(0.01f, () =>
                 {
                     CoreUtils.audioService.StopByName(RS.SoundCityDay);
                     CoreUtils.audioService.StopByName(RS.SoundCityNight);
                    // 播放背景音乐
                    CoreUtils.audioService.PlayBgm(RS.SoundLogin);
                    // 重置MVC数据
                    SendNotification(CmdConstant.ResetSceneCmd);
                     if(HotfixUtil.IsShowLoginView())
                     {
                         SendNotification(CmdConstant.AutoLogin);
                     }
                     else
                     {
                         // 发起配置读取请求
                         SendNotification(CmdConstant.LoadAppConfig);
                     }
                 }, null, false, false, view.m_lbl_Tip_Outline);
            });

            view.m_btn_service_GameButton.onClick.AddListener(() =>
            {
                IGGURLBundle.shareInstance().serviceURL((exception, url) =>
                {
                    if (exception.isNone())
                    {
                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                    }
                });
            });
        }

        #endregion
        private void Login()
        {
            view.m_lbl_version_LanguageText.gameObject.SetActive(true);
            view.m_lbl_version_LanguageText.text = LanguageUtils.getTextFormat(100007, VersionUtil.GetVersionStr());
            LoadingTip();
            SetProgress(0.0f, true);
        }

        private void SetProgress(float value, bool updateJust = false)
        {
            if (updateJust)
            {
                view.m_lbl_doing_LanguageText.text = LanguageUtils.getTextFormat(100034, (int)(value * 100));
                view.m_pb_rogressBar_GameSlider.value = value;
            }
            m_crrPro = value;
        }
    }
}