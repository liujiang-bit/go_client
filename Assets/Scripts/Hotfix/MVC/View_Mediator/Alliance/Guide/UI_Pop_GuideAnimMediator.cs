// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月28日
// Update Time         :    2020年9月28日
// Class Description   :    UI_Pop_GuideAnimMediator
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
    public class UI_Pop_GuideAnimMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_GuideAnimMediator";

        public bool m_isCloseAnim;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_GuideAnimMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_GuideAnimView view;

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
            if (IsTouchEnd())
            {
                CloseAnim();
            }
        }        

        protected override void InitData()
        {
            IsOpenUpdate = true;
            view.m_UI_Model_GuideAnim.Play();
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        #endregion

        public static bool IsTouchEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                return true;
            }
            return false;
        }

        private void CloseAnim()
        {
            if (m_isCloseAnim)
            {
                return;
            }
            m_isCloseAnim = true;
            Timer.Register(0f, () =>
            {
                if (view.gameObject == null)
                {
                    return;
                }
                CoreUtils.uiManager.CloseUI(UI.s_AlliancePopGuideAnim, true, true);
            });

        }
        
    }
}