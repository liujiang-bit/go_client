// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, December 24, 2019
// Update Time         :    Tuesday, December 24, 2019
// Class Description   :    LoginMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game {
    public class LoginServerConfirmMediator : GameMediator {
        #region Member
        public static string NameMediator = "LoginServerConfirmMediator";

        private PlayerProxy _playerProxy;
        private string _lastLoginName;
        private NetProxy m_netProxy;
        
        #endregion

        //IMediatorPlug needs
        public LoginServerConfirmMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public LoginServerConfirm view;

        private string m_serverIP;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AuthEvent,
                Role_GetRoleList.TagName,
                Role_RoleLogin.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            
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
            _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            
            m_netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                        
            view.m_btn_login_Button.enabled = true;
            _lastLoginName = PlayerPrefs.GetString("lastLoginName", RmdName());
            view.m_ipt_username_GameInput.text = _lastLoginName;
            view.m_btn_login_Button.enabled = true;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_login_Button.onClick.AddListener(OnLogin);
            view.m_btn_rmdname_GameButton.onClick.AddListener(OnRmdName);
            view.m_dd_serverip_Dropdown.onValueChanged.AddListener(  onSelectedServer);

            int serverIndex = PlayerPrefs.GetInt("serverindex", 0);
            
            view.m_dd_serverip_Dropdown.value = serverIndex;
            onSelectedServer(serverIndex);
        }

        private void onSelectedServer(int change)
        {
            m_serverIP = view.m_dd_serverip_Dropdown.options[change].text;
            
            PlayerPrefs.SetInt("serverindex",change);
            Debug.Log("服务器ip index:"+change+"  "+m_serverIP);
        }

        protected override void BindUIData()
        {
//            CoreUtils.audioService.SetMusicVolume(0);
//            CoreUtils.audioService.SetSfxVolume(0);
        }
       
        #endregion

        private string RmdName()
        {
            return "ID" + Random.Range(1, 2000000).ToString();
        }
        
        //随机名字
        private void OnRmdName()
        {
            _lastLoginName = RmdName();
            view.m_ipt_username_GameInput.text = _lastLoginName;
        }

        //登录处理
        private void OnLogin()
        {
            view.m_btn_login_Button.enabled = false;
            
            if (view.m_ipt_username_GameInput.text.Length>0)
            {
                view.m_lbl_error_LanguageText.text = "";
                
                _lastLoginName = view.m_ipt_username_GameInput.text;
                PlayerPrefs.SetString("lastLoginName",_lastLoginName);

                string ip = "127.0.0.1";
                string pwd = view.m_ipt_password_GameInput.text;
                var pwds = pwd.Split('|');
                if(pwds.Length == 2)
                {
                    ip = pwds[0];
                    pwd = pwds[1];
                }
                IGGSDKConstant.IGGDefault.AppConfigIP = ip;
                IGGSDKConstant.IGGDefault.IGGID = _lastLoginName;
                IGGSDKConstant.IGGDefault.Token = pwd;
                m_netProxy.SaveLoginInfo(m_serverIP, 10000, _lastLoginName, pwd, view.m_ipt_serverGameNode_GameInput.text);
                CoreUtils.uiManager.CloseUI(UI.s_LoginView);
                // m_netProxy.Connection();
                AppFacade.GetInstance().SendNotification(CmdConstant.LoginToServer);
            }
            else
            {
                Tip.CreateTip("请输入账号").Show();
            }
        }
    }
}