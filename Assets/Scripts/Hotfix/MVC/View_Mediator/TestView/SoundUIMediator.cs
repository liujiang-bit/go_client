// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月13日
// Update Time         :    2019年11月13日
// Class Description   :    SoundUIMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using PureMVC.Interfaces;

namespace Game {
    public class SoundUIMediator : GameMediator {
        #region Member
        public static string NameMediator = "SoundUIMediator";


        #endregion

        //IMediatorPlug needs
        public SoundUIMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public SoundUIView view;

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
            view.m_btn_bgm1_Button.onClick.AddListener(() =>
            {
                CoreUtils.audioService.PlayBgm("bgm_env_day");
            });
            view.m_btn_bgm2_Button.onClick.AddListener(() =>
            {
                CoreUtils.audioService.PlayBgm("bgm_env_night");
            });
            view.m_btn_vfx1_Button.onClick.AddListener(() =>
            {
                CoreUtils.audioService.PlayOneShot("sfx_env_day", null);
            });
            view.m_btn_vfx2_Button.onClick.AddListener(() =>
            {
                CoreUtils.audioService.PlayOneShot("vo_welcome_desc", null);
            });
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}