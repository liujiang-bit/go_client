// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Pop_FightVictoryMediator
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
    public class UI_Pop_FightVictoryMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_FightVictoryMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_FightVictoryMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_FightVictoryView view;

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
            string name = (string)view.data;
            view.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(181189, name);
            CoreUtils.audioService.PlayOneShot("Sound_Ui_Victory");
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