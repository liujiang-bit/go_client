// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月12日
// Update Time         :    2020年8月12日
// Class Description   :    UI_Win_WebViewMediator
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
    public class UI_Win_WebViewMediator : GameMediator {
        public class Param
        {
            public string url;
            public string title;
        }
        #region Member
        public static string NameMediator = "UI_Win_WebViewMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_WebViewMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_WebViewView view;

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

        public override bool onMenuBackCallback()
        {
            CoreUtils.uiManager.CloseUI(UI.s_WebView);
            // 返回true表示自行处理
            return true;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            var param = view.data as Param;
            var webview = view.gameObject.GetComponentInChildren<UniWebView>();
            if (webview != null)
            {
                webview.Load(param.url);
                webview.Show();
                webview.ScreenScale = CoreUtils.getScreenScale();
                //webview.UpdateFrame();
                Debug.Log("Frame:" + webview.Frame);

                webview.OnShouldClose += (UniWebView webView) =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_WebView);
                    return true;
                };
            }

            view.m_bg.setWindowTitle(param.title);

            view.m_bg.AddCloseEvent(()=>
            {
                CoreUtils.uiManager.CloseUI(UI.s_WebView);
            });
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}