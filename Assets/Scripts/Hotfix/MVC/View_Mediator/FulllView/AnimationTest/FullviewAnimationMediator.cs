// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月11日
// Update Time         :    2019年12月11日
// Class Description   :    FullviewAnimationMediator
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
    public class FullviewAnimationMediator : GameMediator {
        #region Member
        public static string NameMediator = "FullviewAnimationMediator";

        private bool fullviewZoomed = false;
        #endregion

        //IMediatorPlug needs
        public FullviewAnimationMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public FullviewAnimationView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ReturnToFullView,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ReturnToFullView:
                    {
                        if(fullviewZoomed)
                        {
                            fullviewZoomed = false;
                            view.m_img_bg_Animator.Play("FullviewZoomIn");
                        }
                    }
                    break;
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
            view.m_btn_taskzoom_GameButton.onClick.AddListener(TaskZoom);
            view.m_btn_queuezoom_GameButton.onClick.AddListener(QueueZoom);
            view.m_btn_menu_GameButton.onClick.AddListener(OnMenu);
            view.m_btn_world_GameButton.onClick.AddListener(QueueZoom);
        }

        protected override void BindUIData()
        {

        }

        #endregion


        bool taskZoomed = false;
        private void TaskZoom()
        {
            if(!taskZoomed)
            {
                taskZoomed = true;
                view.m_pl_task_Animator.Play("TaskZoomIn");
            }
            else
            {
                taskZoomed = false;
                view.m_pl_task_Animator.Play("TaskZoomOut");
            }
        }


        private void QueueZoom()
        {
#if UNITY_EDITOR || UNITY_STANDLONE
            CoreUtils.uiManager.ShowUI(UI.s_listView);
#endif
            fullviewZoomed = true;
            view.m_img_bg_Animator.Play("FullviewZoomOut");
        }

        bool menuZoomed = false;
        private void OnMenu()
        {
            if (!menuZoomed)
            {
                menuZoomed = true;
                view.m_pl_menumask_Animator.Play("MenuZoomIn");
            }
            else
            {
                menuZoomed = false;
                view.m_pl_menumask_Animator.Play("MenuZoomOut");
            }
        }



    }
}