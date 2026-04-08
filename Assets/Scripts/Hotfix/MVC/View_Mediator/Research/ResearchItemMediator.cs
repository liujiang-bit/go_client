// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 2, 2020
// Update Time         :    Thursday, January 2, 2020
// Class Description   :    ResearchItemMediator
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
    public class ResearchItemMediator : GameMediator {
        #region Member
        public static string NameMediator = "ResearchItemMediator";


        #endregion

        //IMediatorPlug needs
        public ResearchItemMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ResearchItemView view;

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