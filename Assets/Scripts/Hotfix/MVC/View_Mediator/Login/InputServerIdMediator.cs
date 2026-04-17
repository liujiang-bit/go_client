// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月7日
// Update Time         :    2020年9月7日
// Class Description   :    InputServerIdMediator
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

namespace Game {
    public class InputServerIdMediator : GameMediator {
        #region Member
        public static string NameMediator = "InputServerIdMediator";


        #endregion

        //IMediatorPlug needs
        public InputServerIdMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public InputServerIdView view;

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
            view.m_ipt_serverGameNode_GameInput.text = PlayerPrefs.GetString("ServerID", "");
        }

        protected override void BindUIEvent()
        {
            view.m_btn_login_Button.onClick.AddListener(OnLogin);
        }

        protected override void BindUIData()
        {

        }

        #endregion
        //登录处理
        private void OnLogin()
        {
            view.m_btn_login_Button.enabled = false;
            if (view.m_ipt_serverGameNode_GameInput.text.Length > 0)
            {
                var serverId = view.m_ipt_serverGameNode_GameInput.text;
                int nServerId = 1;
                if (!string.IsNullOrEmpty(serverId) && int.TryParse(serverId, out nServerId))
                {
                    PlayerPrefs.SetString("ServerID", serverId);
                    SDKServerConfig serverConfig;
                    var session = SDKSession.currentSession;
                    if (SDKBridge.appConfig != null)
                    {
                        serverConfig = SDKBridge.appConfig.getServerConfig();

                        var loginserver = serverConfig.LoginServer[0];
                        var nexProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                        nexProxy.SaveLoginInfo(loginserver.host, loginserver.port, session.getIGGId(), session.getAccesskey(), "game"+serverId);
                    }
                    else
                    {
                        var nexProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                        nexProxy.SaveLoginInfo("loginserver1.oc.igotgames.net", 10000, session.getIGGId(), session.getAccesskey(), "game"+serverId);
                    }
                    CoreUtils.uiManager.CloseUI(UI.s_InputServerIdView);
                    AppFacade.GetInstance().SendNotification(CmdConstant.LoginToServer);
                }
            }
            else
            {
                PlayerPrefs.SetString("ServerID", "");
                CoreUtils.uiManager.CloseUI(UI.s_InputServerIdView);
                AppFacade.GetInstance().SendNotification(CmdConstant.LoginToServer);
            }
        }
    }
}