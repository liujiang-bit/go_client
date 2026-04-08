using IGGSDKConstant;
using Newtonsoft.Json;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LoginCommand : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AutoLogin:
                    {
                        if (HotfixUtil.IsShowLoginView())
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_LoginView);
                        }
                        else
                        {
                            //TODO 设置 Session 过期情况, 该步骤在登录之前必接，否则调用自动登录接口出现异常
                            IGGLogin.shareInstance().setLoginDelegate(onSessionExpired);
                            // 自动登陆
                            IGGLogin.shareInstance().AutoLogin(OnAutoLoginFinished);
                        }
                        break;
                    }
                // 账号登陆
                case CmdConstant.LoginAccount:
                    {
                        //TODO 设置 Session 过期情况, 该步骤在登录之前必接，否则调用自动登录接口出现异常
                        IGGLogin.shareInstance().setCheckStateBox(CheckStateBox);
                        IGGLogin.shareInstance().setLoginDelegate(onSessionExpired);
                        IGGLoginType loginType = (IGGLoginType)notification.Body;
                        if (loginType == IGGLoginType.GUEST)
                        {
                            IGGLogin.shareInstance().GuestLogin(OnLoginAccountFinished);
                            return;
                        }
                        else if (loginType == IGGLoginType.IGG_PASSPORT)
                        {
                            IGGLogin.shareInstance().SwitchToIGGPassport(OnLoginAccountFinished);
                            return;
                        }
                        //if (loginType == IGGLoginType.FACEBOOK)
                        //{
                        //    IGGLogin.shareInstance().SwitchToFacebook(OnLoginFinished);
                        //    return;
                        //}
                        //else if (loginType == IGGLoginType.GOOGLE_PLAY)
                        //{
                        //    IGGLogin.shareInstance().SwitchToGooglePlay(OnLoginFinished);
                        //    return;
                        //}
                        //else if (loginType == IGGLoginType.GAMECENTER)
                        //{
                        //    IGGLogin.shareInstance().SwitchToGameCenter(OnLoginFinished);
                        //    return;
                        //}
                        break;
                    }
                // 切换账号
                case CmdConstant.SwitchAccount:
                    {
                        //TODO 设置 Session 过期情况, 该步骤在登录之前必接，否则调用自动登录接口出现异常

                        IGGLogin.shareInstance().setCheckStateBox(CheckStateBox);
                        IGGLogin.shareInstance().setLoginDelegate(onSessionExpiredInGame);
                        IGGLoginType loginType = (IGGLoginType)notification.Body;
                        if (loginType == IGGLoginType.GUEST)
                        {
                            IGGLogin.shareInstance().GuestLogin(OnSwitchLoginFinished);
                            return;
                        }
                        else if (loginType == IGGLoginType.IGG_PASSPORT)
                        {
                            IGGLogin.shareInstance().SwitchToIGGPassport(OnSwitchLoginFinished);
                            return;
                        }
                        break;
                    }
                // 账号切换完成
                case CmdConstant.SwitchAccountFinished:
                    {
                        // 切换成功需要重新走登陆流程
                        IGGSDK.shareInstance().RunInMainThread(() =>
                        {
                            SendNotification(CmdConstant.ReloadGame);
                        });
                    }
                    break;
                // 账号信息
                case CmdConstant.LoadAccountProfile:
                    {
                        IGGLogin.shareInstance().setLoginDelegate(onSessionExpiredInGame);
                        IGGAccountManagementGuideline.shareInstance().loadUserFromServerOrCache((IGGException exception, IGGUserProfile userProfile) =>
                        {
                            if (exception.isNone())
                            {
                                SendNotification(CmdConstant.LoadAccountProfileFinished);
                            }
                        });
                        break;
                    }
                // 账号绑定
                case CmdConstant.BindindAccount:
                    {
                        //TODO 设置 Session 过期情况, 该步骤在登录之前必接，否则调用自动登录接口出现异常
                        IGGLogin.shareInstance().setLoginDelegate(onSessionExpiredInGame);
                        IGGLoginType loginType = (IGGLoginType)notification.Body;
                        if (loginType == IGGLoginType.IGG_PASSPORT)
                        {
                            IGGLogin.shareInstance().BindToIGGPassport(OnBindFinished);
                            return;
                        }
                        break;
                    }
                // 被封号
                case CmdConstant.AccountBan:
                    {
                        CoreUtils.uiManager.SetGuideStatus(true);
                        IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getText(100049), LanguageUtils.getText(100035), LanguageUtils.getText(100036), LanguageUtils.getText(100047), (bool bSure) =>
                        {
                            CoreUtils.uiManager.SetGuideStatus(false);
                            if (bSure)
                            {
                                IGGURLBundle.shareInstance().serviceURL((IGGException ex, string url) =>
                                {
                                    if (ex.isNone())
                                    {
                                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                                    }
                                    SendNotification(CmdConstant.AccountBan);
                                });

                            }
                            else
                            {
                                Application.Quit();
                            }
                        });
                        break;
                    }
            }
        }

         // session 失效需要重新登陆
        private void onSessionExpired(IGGSession expiredSession)
        {
            IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getText(100072), LanguageUtils.getText(100035), LanguageUtils.getText(100036), (bool bSure) =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_AccountLoginMain, null, expiredSession.getLoginType());
            });
        }
        // session 失效需要重新登陆
        private void onSessionExpiredInGame(IGGSession expiredSession)
        {
            IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getText(100072), LanguageUtils.getText(100035), LanguageUtils.getText(100036), (bool bSure) =>
            {
                SendNotification(CmdConstant.ReloadGame);
            });
        }
        private void OnAutoLoginFinished(IGGException exception, IGGSession session)
        {
            var err = OnLoginFinished(exception, session);
            if (exception !=null && exception.isNone())
            {
                Debug.Log("IGGID:" + session.getIGGId());
                IGGPayment.shareInstance().initialize(OnGameItemLoad);
                var gameid = IGGSDK.shareInstance().getGameId();
                //CoreUtils.adService.SetupGameID (gameid);
                //CoreUtils.adService.OnFetchIGGID(session.getIGGId());
                SendNotification(CmdConstant.AutoLoginFinished);
                SendNotification(CmdConstant.AgreementCheck);
            }
            if(!string.IsNullOrEmpty(err))
            {
                CoreUtils.uiManager.SetGuideStatus(true);
                IGGSDKUtils.shareInstance().ShowMsgBox(err, LanguageUtils.getText(100035), LanguageUtils.getText(100036), (bool bSure) =>
                {
                    CoreUtils.uiManager.SetGuideStatus(false);
                    SendNotification(CmdConstant.AutoLogin);
                });
            }
        }
        private void OnLoginAccountFinished(IGGException exception, IGGSession session)
        {
            var err = OnLoginFinished(exception, session);
            if (exception.isNone())
            {
                Debug.Log("IGGID:" + session.getIGGId());
                IGGPayment.shareInstance().initialize(OnGameItemLoad);
                var gameid = IGGSDK.shareInstance().getGameId();
                //CoreUtils.adService.SetupGameID(gameid);
                //CoreUtils.adService.OnFetchIGGID(session.getIGGId());
                SendNotification(CmdConstant.LoginAccountFinished);
                SendNotification(CmdConstant.AgreementCheck);
            }
            if (!string.IsNullOrEmpty(err))
            {
                IGGSDKUtils.shareInstance().ShowToast(err);
            }
        }
        private void OnSwitchLoginFinished(IGGException exception, IGGSession session)
        {
            var err = OnLoginFinished(exception, session);
            if (exception.isNone())
            {
                SendNotification(CmdConstant.SwitchAccountFinished);
            }
            if (!string.IsNullOrEmpty(err))
            {
                IGGSDKUtils.shareInstance().ShowToast(err);
            }
        }
        private string OnLoginFinished(IGGException exception, IGGSession session)
        {
            string errorStr = "";
            if (exception == null || session == null)
            {
                errorStr = LanguageUtils.getTextFormat(100132, "0");
                // 这边需要显示切换账号框
                //IGGSDKUtils.shareInstance().ShowToast("获取Token失败，请重新登陆！");
            }
            else if (exception.isNone())
            {
                IGGPushNotification.shareInstance().uninitialize();
                IGGPushNotification.shareInstance().initialize();

                IGGServerConfig serverConfig;
                if(IGGSDK.appConfig != null)
                {
                    serverConfig = IGGSDK.appConfig.getServerConfig();

                    var loginserver = serverConfig.LoginServer[0];
                    var nexProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                    //nexProxy.SaveLoginInfo(loginserver.host, loginserver.port, session.getIGGId(), session.getAccesskey(), "");
                    nexProxy.SaveLoginInfo("104.233.171.110", 40001, session.getIGGId(), session.getAccesskey(), "");
                }
                else
                {
                    var nexProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                    nexProxy.SaveLoginInfo("loginserver1.oc.igotgames.net", 10000, session.getIGGId(), session.getAccesskey(), "");
                }
            }
            else
            {
                CoreUtils.logService.Info(exception.ToString(), Color.red);
                //if (exception.getCode().Equals("111303"))
                //{
                //    IGGSDKUtils.shareInstance().ShowToast("多次取消登陆，需要到账号中心去登陆GameCenter！");
                //}
                if (exception.getCode().Equals("114103"))
                {
                    // 这个是取消 IGG登陆， 不提示
                }
                else if (exception.getUnderlyingException().getCode().Equals("911"))
                {
                    SendNotification(CmdConstant.AccountBan);
                }
                if (exception.getUnderlyingException().getCode().Equals("4000"))
                {
                    int code = 0;
                    int.TryParse(exception.getCode(), out code);
                    SendNotification(CmdConstant.ReRoleLogin, code);
                }
                else
                {
                    Debug.LogWarning(exception.getUnderlyingException().getCode());
                    errorStr = LanguageUtils.getTextFormat(100132, exception.getCode());
                    //IGGSDKUtils.shareInstance().ShowToast(errorStr);
                }
            }

            return errorStr;
        }

        private void OnBindFinished(IGGException exception, string iggid)
        {
            if (exception != null)
            {
                if (exception.isOccurred() && iggid != "")
                {
                    // 这边对话框可以改掉
                    // 绑定失败。改账号在游戏内已经绑定了IGG ID：{0}
                    IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getTextFormat(100117, iggid), LanguageUtils.getText(100134), LanguageUtils.getText(100036));
                    return;
                }
                if (exception.isOccurred())
                {
                    CoreUtils.logService.Info(exception.ToString(), Color.red);
                    if (exception.getCode().Equals("111405"))
                    {
                        IGGSDKUtils.shareInstance().ShowToast("多次取消登陆，需要到账号中心去登陆GameCenter！");
                    }
                    if (exception.getCode().Equals("114103"))
                    {
                        // 这个是取消 IGG登陆， 不提示
                    }
                    else
                    {
                        IGGSDKUtils.shareInstance().ShowToast(LanguageUtils.getTextFormat(100125, exception.getCode()));
                    }
                    return;
                }
                if (exception.isNone())
                {
                    // 绑定成功
                    IGGSDKUtils.shareInstance().ShowToast(LanguageUtils.getTextFormat(100110));
                    SendNotification(CmdConstant.BindindAccountFinished);
                }
            }
            else if (exception == null && iggid == null)
            {
                IGGSDKUtils.shareInstance().ShowToast(LanguageUtils.getText(100126));
            }
        }
        private void CheckStateBox(string iggid, AccountState accountState, IGGLoginType loginType, IGGLogin.CheckStateReturn callback)
        {
            string strMessage = "";
            if (accountState == AccountState.CreateAccount)
            {
                //strMessage = "没有绑定过IGGID 要以全新IGG ID进入游戏吗？";
                if (loginType == IGGLoginType.GUEST)
                {
                    IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getText(100074), LanguageUtils.getText(100035), LanguageUtils.getText(100036), LanguageUtils.getText(192010), new IGGSDKUtils.MsgBoxReturnListener.Listener(callback));
                }
                else
                {
                    IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getText(100075), LanguageUtils.getText(100035), LanguageUtils.getText(100036), LanguageUtils.getText(192010), new IGGSDKUtils.MsgBoxReturnListener.Listener(callback));
                }
            }
            else if (accountState == AccountState.ChangeAccount)
            {
                // 以后账号无需提示直接确认登陆  以前技术部流程是需要提示的，现在去掉提示
                if (IGGSession.currentSession.isValid() == false)
                {
                    callback(true);
                }
                else
                {
                    // 这边可以改成你们自己的弹窗
                    // 账号不一样或者登陆方式不一样才可以登陆
                    if (loginType == IGGSession.currentSession.getLoginType() && iggid == IGGSession.currentSession.getIGGId())
                    {
                        //IGGSDKUtils.shareInstance().ShowToast("已经是当前账号，无需切换");
                        IGGSDKUtils.shareInstance().ShowToast(LanguageUtils.getText(100120));
                        return;
                    }
                    //strMessage = string.Format("是否以IGGID {0} 进入游戏吗？", iggid);
                    IGGSDKUtils.shareInstance().ShowMsgBox(LanguageUtils.getTextFormat(100078, iggid), LanguageUtils.getText(100035), LanguageUtils.getText(100036), LanguageUtils.getText(192010), new IGGSDKUtils.MsgBoxReturnListener.Listener(callback));
                }
            }
        }

        private void OnGameItemLoad(List<IGGGameItem> gameItems)
        {
            // 这边把商品存起来
            //mLoadItem = true;
            //Debug.Log($"OnGameItemLoad: {gameItems.Count}");
            //for (int i = 0; i < gameItems.Count; i++)
            //{
            //    gameItems[i].getPurchase().setCurrency(Currency.USD);
            //    Debug.Log(gameItems[i].ToString());
            //    var item = Instantiate(mItemTemplate);
            //    item.SetActive(true);
            //    item.transform.parent = mContent;
            //    item.GetComponentInChildren<Text>().text = string.Format("{0}:{1}", gameItems[i].getTitle(), gameItems[i].getPurchase().getFormattedPrice());
            //    string pcid = gameItems[i].getId();
            //    item.GetComponent<Button>().onClick.AddListener(() =>
            //    {
            //        onBuyItem(pcid);
            //    });
            //}
        }
    }
}

