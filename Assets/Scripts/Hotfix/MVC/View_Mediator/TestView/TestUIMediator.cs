// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月4日
// Update Time         :    2019年11月4日
// Class Description   :    TestUIMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;

namespace Game {
    public class TestUIMediator : GameMediator {
        #region Member
        public static string NameMediator = "TestUIMediator";


        #endregion

        //IMediatorPlug needs
        public TestUIMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public TestUIView view;

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
            view.m_btn_sound_Button.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_soundUIInfo);
            });
        }

        protected override void BindUIData()
        {
        }
       
        #endregion
    }
}