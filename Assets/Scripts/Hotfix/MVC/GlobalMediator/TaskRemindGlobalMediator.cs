// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    TaskRemindGlobalMediator 主线任务定时提醒功能
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class TaskRemindGlobalMediator : GameMediator
    {
        public static string NameMediator = "TaskRemindGlobalMediator";

        private float m_recordTime;

        private Timer m_timer;

        private int m_remindTaskTime;

        //IMediatorPlug needs
        public TaskRemindGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public TaskRemindGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){

                CmdConstant.OnApplicationFocus,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch(notification.Name)
            {
                case CmdConstant.OnApplicationFocus:
                    bool isFocus = (bool)notification.Body;
                    if (!isFocus)
                    {
                        //Debug.LogError("重置时间");
                        //重置时间
                        m_recordTime = Time.realtimeSinceStartup;
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void OnRemove()
        {
            base.OnRemove();
            CancelTimer();
            CoreUtils.inputManager.RemoveTouchEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_remindTaskTime = config.remindActivityTime;
            //Debug.LogError("m_remindTa skTime："+ m_remindTaskTime);
            m_timer = Timer.Register(10.0f, CheckTime, null, true, true);
            m_recordTime = Time.realtimeSinceStartup;
        }

        protected override void BindUIEvent()
        {
            CoreUtils.inputManager.AddTouchEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        public void OnTouchBegan(int x, int y)
        {
            m_recordTime = Time.realtimeSinceStartup;
        }

        public void OnTouchMoved(int x, int y)
        {
            m_recordTime = Time.realtimeSinceStartup;
        }

        public void OnTouchEnded(int x, int y)
        {
            m_recordTime = Time.realtimeSinceStartup;
        }

        public static bool IsTouchBegin()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
            return false;
        }

        private void CheckTime()
        {
            if (Time.realtimeSinceStartup - m_recordTime > m_remindTaskTime)
            {
                m_recordTime = Time.realtimeSinceStartup;

                //判断任务侧边栏是否存在
                MainInterfaceMediator mainMediator = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
                if (mainMediator == null)
                {
                    return;
                }
                if (!mainMediator.GetTaskMenuShowStatus())
                {
                    return;
                }
                if (mainMediator.isTaskAniOn)
                {
                    return;
                }

                //判断是否正在伺候跟随
                PVPGlobalMediator pvpMediator = AppFacade.GetInstance().RetrieveMediator(PVPGlobalMediator.NameMediator) as PVPGlobalMediator;
                if (pvpMediator == null)
                {
                    return;
                }
                if (pvpMediator.IsCameraFollow())
                {
                    return;
                }

                //是否正在引导中
                if (GuideProxy.IsGuideing)
                {
                    return;
                }

                if (CoreUtils.uiManager.ExistUI(UI.s_fingerInfo))
                {
                    return;
                }

                if (CoreUtils.uiManager.ExistUI(UI.s_funcGuide))
                {
                    UIInfo uiInfo = CoreUtils.uiManager.GetUI(99001);
                    if (uiInfo != null && uiInfo.uiObj != null && uiInfo.uiObj.activeSelf)
                    {
                        return;
                    }
                }

                //是否有弹出窗口
                if (CoreUtils.uiManager.IsHasPopView())
                {
                    return;
                }

                var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                if (playerProxy == null || playerProxy.CurrentRoleInfo == null)
                {
                    return;
                }
                if (playerProxy.CurrentRoleInfo.mainLineTaskId > 0 || playerProxy.CurrentRoleInfo.chapterId > 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.TaskGuideRemind, 1);
                }
            }
        }
    }
}

