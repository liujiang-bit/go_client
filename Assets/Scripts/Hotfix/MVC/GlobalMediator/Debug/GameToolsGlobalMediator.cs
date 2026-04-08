// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    GMGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using Client;
using System;
using Hotfix;

namespace Game
{
    public class GameToolsGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "GameToolsGlobalMediator";

        List<KeyCode> m_compareQuque = new List<KeyCode>();
        Stack<KeyCode> m_currentQuque = new Stack<KeyCode>();
        private Vector3 m_lastTouchPos = Vector3.zero;


        #endregion

        public GameToolsGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public GameToolsGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {

            }.ToArray();
        }

        public override void HandleNotification(INotification notificationName)
        {
            switch (notificationName.Name)
            {

                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            this.IsOpenUpdate = true;

            m_compareQuque.Add(KeyCode.UpArrow);
            m_compareQuque.Add(KeyCode.UpArrow);
            m_compareQuque.Add(KeyCode.DownArrow);
            m_compareQuque.Add(KeyCode.DownArrow);
            m_compareQuque.Add(KeyCode.LeftArrow);
            m_compareQuque.Add(KeyCode.LeftArrow);
            m_compareQuque.Add(KeyCode.RightArrow);
            m_compareQuque.Add(KeyCode.RightArrow);
            m_compareQuque.Add(KeyCode.A);
            m_compareQuque.Add(KeyCode.B);
            m_compareQuque.Add(KeyCode.A);
            m_compareQuque.Add(KeyCode.B);
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
#if UNITY_EDITOR || UNITY_STANDLONE || UNITY_STANDALONE_WIN
            if (Input.GetKeyUp(KeyCode.F5))
            {
                CoreUtils.uiManager.ShowUI(UI.s_gameTool);
            }
#endif
        }

        public override void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_lastTouchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var offset = Input.mousePosition - m_lastTouchPos;
                if (offset.magnitude < 50.0f)
                {
                    if (Input.mousePosition.x < Screen.width / 2)
                    {
                        PushCommand(KeyCode.A);
                    }
                    else
                    {
                        PushCommand(KeyCode.B);
                    }
                }
                else
                {
                    var dir = offset.normalized;
                    if (dir.x < -0.5)
                    {
                        PushCommand(KeyCode.LeftArrow);
                    }
                    else if (dir.x > 0.5)
                    {
                        PushCommand(KeyCode.RightArrow);
                    }
                    else if (dir.y > 0.5)
                    {
                        PushCommand(KeyCode.UpArrow);
                    }
                    else if (dir.y < -0.5)
                    {
                        PushCommand(KeyCode.DownArrow);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
        }



        #endregion

        private void PushCommand(KeyCode code)
        {
            //Debug.Log(code);                       
            m_currentQuque.Push(code);
            var lastCode = m_currentQuque.Peek();
            if (lastCode != m_compareQuque[m_currentQuque.Count - 1])
            {
                m_currentQuque.Clear();
            }
            if (m_currentQuque.Count == m_compareQuque.Count)
            {
                CoreUtils.uiManager.ShowUI(UI.s_gameTool);
                m_currentQuque.Clear();
            }
        }
    }
}