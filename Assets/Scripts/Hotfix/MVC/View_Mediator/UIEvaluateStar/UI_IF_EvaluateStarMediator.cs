// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_IF_EvaluateStarMediator
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
    public class UI_IF_EvaluateStarMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_EvaluateStarMediator";

        private SDKStandardAppRating m_starndardAppRating;

        #endregion

        //IMediatorPlug needs
        public UI_IF_EvaluateStarMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_EvaluateStarView view;

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
            m_starndardAppRating = view.data as SDKStandardAppRating;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Later.AddClickEvent(()=>
            {
                CoreUtils.uiManager.CloseUI(UI.s_EvaluateStar);
            });
            view.m_UI_Great.AddClickEvent(() =>
            {
                m_starndardAppRating.like((SDKException ex)=>
                {
                });
                CoreUtils.uiManager.CloseUI(UI.s_EvaluateStar);
            });
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}