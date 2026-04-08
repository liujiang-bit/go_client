// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, February 18, 2020
// Update Time         :    Tuesday, February 18, 2020
// Class Description   :    ReConnectMediator
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
    public class ReConnectMediator : GameMediator {
        #region Member
        public static string NameMediator = "ReConnectMediator";


        #endregion

        //IMediatorPlug needs
        public ReConnectMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ReConnectView view;

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

        public override void OnRemove()
        {
            base.OnRemove();
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