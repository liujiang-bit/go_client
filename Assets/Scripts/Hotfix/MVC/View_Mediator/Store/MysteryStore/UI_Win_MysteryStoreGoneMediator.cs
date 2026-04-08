// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_Win_MysteryStoreGoneMediator
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
    public class UI_Win_MysteryStoreGoneMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_MysteryStoreGoneMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_MysteryStoreGoneMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_MysteryStoreGoneView view;

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

       
        // private void OnClose()
        // {
        //     CoreUtils.uiManager.CloseUI(UI.s_mysteryStoreGone);
        // }

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
            view.m_btn_Close_GameButton.onClick.AddListener(
                delegate
                {
                    CoreUtils.uiManager.CloseUI(UI.s_mysteryStoreGone);
                });
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}