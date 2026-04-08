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
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using LitJson;
using System.Text;
using static IGGServerConfig;
using System.Security.Cryptography;
using System;
using System.Net;

namespace Game
{
    [System.Serializable]
    class LoginInfo
    {
        public string account;
        public string password;
    }

    public class LoginMediator : GameMediator
    {
        string host = "192.168.252.126";
        #region Member
        public static string NameMediator = "LoginMediator";

        private PlayerProxy _playerProxy;
        private string _lastLoginName;
        private string _lastLoginPassword;
        private NetProxy m_netProxy;

        #endregion
        //IMediatorPlug needs
        public LoginMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public LoginView view;

        private string m_serverIP;
        private int m_serverPort;

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

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

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
            _lastLoginPassword = PlayerPrefs.GetString("lastLoginPassword", "");
            view.m_ipt_username_GameInput.text = _lastLoginName;
            view.m_ipt_password_GameInput.text = _lastLoginPassword;
            view.m_btn_login_Button.enabled = true;

            view.m_dd_serverip_Dropdown.options.Clear();

            view.m_btn_login_Button.enabled = false;

            MD5 md5Hash = MD5.Create();

            string key = PlayerProxy.signKey;
            var time = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            WWWForm form = new WWWForm();
            form.AddField("requestserverlist", time);
            string hasher = $"request:{time}:server:{key}:list";
            byte[] result = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hasher));
            string resultMD5 = BitConverter.ToString(result).Replace("-", "").ToLower();
            form.AddField("sign", resultMD5);

            UnityWebRequest request = new UnityWebRequest($"http://{host}:88/api/lists.php", UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(form.data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            var sendRequest = request.SendWebRequest();
            sendRequest.completed += (op) =>
            {
                view.m_btn_login_Button.enabled = true;

                if (!request.isNetworkError && !request.isHttpError)
                {
                    Debug.Log(request.responseCode);
                    Debug.Log(request.downloadHandler.text);

                    var text = request.downloadHandler.text;

                    switch(text)
                    {
                        case "服务器列表获取失败":
                            break;
                        default:
                            JsonData jsonData = JsonMapper.ToObject(text);
                            //sp.ip = jsonData["cip"].ToString();
                            //sp.area = $"{sp.area}:{jsonData["cname"].ToString()}";
                            PlayerProxy.serverImformations = JsonMapper.ToObject < List < ServerImformation >> (text).ToArray();
                            foreach (var server in PlayerProxy.serverImformations)
                            {
                                Dropdown.OptionData od = new Dropdown.OptionData(server.servername);
                                if (String.IsNullOrEmpty( server.host))
                                    server.host = host;
                                view.m_dd_serverip_Dropdown.options.Add(od);
                            }
                            view.m_dd_serverip_Dropdown.RefreshShownValue();
                            view.m_dd_serverip_Dropdown.value = PlayerPrefs.GetInt("serverindex", 0);
                            onSelectedServer(view.m_dd_serverip_Dropdown.value);
                            break;
                    }
                }
                else
                {
                    Tip.CreateTip($"无法更新服务器列表，请检查网络和服务器状态。错误代码{request.responseCode}，3秒后重试").Show();
                }
            };
        }

        protected override void BindUIEvent()
        {
            view.m_btn_login_Button.onClick.AddListener(OnLogin);
            view.m_btn_rmdname_GameButton.onClick.AddListener(OnRmdName);
            view.m_dd_serverip_Dropdown.onValueChanged.AddListener(onSelectedServer);
            //view.m_dd_serverip_Dropdown.OnPointerClick(() => CoreUtils.uiManager.ShowUI(UI.s_LoginServerSelect));
            int serverIndex = PlayerPrefs.GetInt("serverindex", 0);

            view.m_dd_serverip_Dropdown.value = serverIndex;
            onSelectedServer(serverIndex);
        }

        private void onSelectedServer(int change)
        {
            if (PlayerProxy.serverImformations == null)
                return;
            ServerImformation si = PlayerProxy.serverImformations[change];
            PlayerProxy.curServerImformation = si;
            m_serverIP = si.host;
            m_serverPort = si.port;
            //m_serverIP = "103.145.22.185";
            //m_serverPort = 40001;
            PlayerPrefs.SetInt("serverindex", change);
            PlayerPrefs.SetString("servername", si.servername);
            Debug.Log("服务器ip index:" + change + " IP:" + m_serverIP + " PORT:" + m_serverPort);
        }

        protected override void BindUIData()
        {
            //            CoreUtils.audioService.SetMusicVolume(0);
            //            CoreUtils.audioService.SetSfxVolume(0);
        }

#endregion

        private string RmdName()
        {
            return "ID" + UnityEngine.Random.Range(1, 2000000).ToString();
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
            if (view.m_ipt_username_GameInput.text.Length == 0)
            {
                Tip.CreateTip("请输入账号").Show();
                return;
            }

            view.m_btn_login_Button.enabled = false;

            view.m_lbl_error_LanguageText.text = "";

            _lastLoginName = view.m_ipt_username_GameInput.text;
            _lastLoginPassword = view.m_ipt_password_GameInput.text;
            PlayerPrefs.SetString("lastLoginName", _lastLoginName);
            PlayerPrefs.SetString("lastLoginPassword", _lastLoginPassword);
            string ip = "127.0.0.1";

            //var pwds = pwd.Split('|');
            //if (pwds.Length == 2)
            //{
            //    ip = pwds[0];
            //    pwd = pwds[1];
            //}
            IGGSDKConstant.IGGDefault.AppConfigIP = ip;
            IGGSDKConstant.IGGDefault.IGGID = _lastLoginName;
            IGGSDKConstant.IGGDefault.Token = _lastLoginPassword;

            string key = PlayerProxy.signKey;
            LoginInfo loginInfo= new LoginInfo();
            loginInfo.account = _lastLoginName;
            loginInfo.password = _lastLoginPassword;

            WWWForm form = new WWWForm();
            form.AddField("account", _lastLoginName);
            form.AddField("password", _lastLoginPassword);

            UnityWebRequest request = new UnityWebRequest($"http://{host}:88/api/login.php", UnityWebRequest.kHttpVerbPOST);

            request.uploadHandler = new UploadHandlerRaw(form.data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            var sendRequest = request.SendWebRequest();
            sendRequest.completed += (op) => 
            {
                view.m_btn_login_Button.enabled = true;

                if (!request.isNetworkError && !request.isHttpError)
                {
                    Debug.Log(request.responseCode);
                    Debug.Log(request.downloadHandler.text);
                    switch(request.downloadHandler.text)
                    {
                        case "请输入用户名或密码":
                            Tip.CreateTip("未正确输入账号或者密码").Show();
                            break;
                        case "请前去网页注册,可联系客服获取网页注册地址":
                            Tip.CreateTip("请前去网页注册,可联系客服获取网页注册地址").Show();
                            break;
                        case "用户名或密码错误":
                            Tip.CreateTip("用户名或密码错误").Show();
                            break;
                        default:
                            string hasher = $"{_lastLoginName}:{_lastLoginPassword}:{key}";
                            MD5 md5Hash = MD5.Create();
                            byte[] result = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hasher));
                            string resultMD5 = BitConverter.ToString(result).Replace("-", "");
                            resultMD5 = resultMD5.ToLower();
                            Debug.Log("resultMD5:" + resultMD5);
                            Debug.Log("downloadHandler:" + request.downloadHandler.text);
                            // if (resultMD5.CompareTo(request.downloadHandler.text) == 0)
                            // {
                                m_netProxy.SaveLoginInfo(m_serverIP, m_serverPort, _lastLoginName, _lastLoginPassword, view.m_ipt_serverGameNode_GameInput.text);
                                CoreUtils.uiManager.CloseUI(UI.s_LoginView);
                                // m_netProxy.Connection();
                                AppFacade.GetInstance().SendNotification(CmdConstant.LoginToServer);
                            // }
                            break;
                    }
                }
                else
                {
                    Tip.CreateTip($"无法连接到服务器{m_serverIP}。错误代码{request.responseCode}").Show();
                }
            };
        }
    }
}