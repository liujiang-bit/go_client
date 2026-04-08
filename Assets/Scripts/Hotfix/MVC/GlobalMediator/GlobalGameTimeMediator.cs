// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    GlobalGameTimeMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoTemp;
using Client;
using System;
using System.Text;
using Client.FSM;
using Data;
using SprotoType;
using Random = UnityEngine.Random;

namespace Game
{

    public class GlobalGameTimeMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "GlobalGameTimeMediator";

        public Timer m_timer;
        public DateTime m_lastDateTime;

        public int m_systemDayTime;

        #endregion

        //IMediatorPlug needs
        public GlobalGameTimeMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public GlobalGameTimeMediator(object viewComponent) : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.GameTimeInit,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {

                case CmdConstant.GameTimeInit:
                    Init();
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {

        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        public override void Update()
        {

        }

        public override void LateUpdate()
        {
            
        }

        public override void FixedUpdate()
        {

        }

        public override void OnRemove()
        {
            CancelTimer();
        }

        #endregion

        private void Init()
        {
            m_lastDateTime = ServerTimeModule.Instance.GetCurrServerDateTime();
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_systemDayTime = config.systemDayTime;
            if (m_timer == null)
            {
                 m_timer = Timer.Register(1.0f, CheckTime, null, true, true);
            }
        }

        private void CheckTime()
        {
            DateTime currDateTime = ServerTimeModule.Instance.GetCurrServerDateTime();

            if (currDateTime.Hour != m_lastDateTime.Hour)
            {
                if (currDateTime.Hour == m_systemDayTime)
                {
                    if ((currDateTime.Hour - m_lastDateTime.Hour) == 1)
                    {
                        Debug.Log("跨天了");
                        m_lastDateTime = currDateTime;
                        AppFacade.GetInstance().SendNotification(CmdConstant.SystemDayTimeChange);
                        return;
                    }
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.SystemHourChange);
            }
            if (currDateTime.Day != m_lastDateTime.Day)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.SystemDayChange);
            }
            m_lastDateTime = currDateTime;
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }
    }
}