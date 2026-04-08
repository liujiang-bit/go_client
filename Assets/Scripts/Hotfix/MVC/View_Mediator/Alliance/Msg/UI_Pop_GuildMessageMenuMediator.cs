// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, May 18, 2020
// Update Time         :    Monday, May 18, 2020
// Class Description   :    UI_Pop_GuildMessageMenuMediator
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
    public class UI_Pop_GuildMessageMenuMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_GuildMessageMenuMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_GuildMessageMenuMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_GuildMessageMenuView view;

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
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}