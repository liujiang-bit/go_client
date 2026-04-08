// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, April 20, 2020
// Update Time         :    Monday, April 20, 2020
// Class Description   :    UI_Win_ReinforceMediator
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
    public class UI_Win_ReinforceMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ReinforceMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_ReinforceMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ReinforceView view;

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