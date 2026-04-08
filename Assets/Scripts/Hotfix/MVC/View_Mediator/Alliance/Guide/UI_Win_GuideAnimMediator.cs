// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年9月28日
// Update Time         :    2020年9月28日
// Class Description   :    UI_Win_GuideAnimMediator 联盟建筑 引导界面
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
    public class UI_Win_GuideAnimMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuideAnimMediator";

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuideAnimMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuideAnimView view;

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

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type4.m_btn_close_GameButton.onClick.AddListener(Close);
            view.m_UI_Model_GuideAnim.Play();
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceGuideHelp);
        }
    }
}