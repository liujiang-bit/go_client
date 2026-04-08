// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月11日
// Update Time         :    2020年7月11日
// Class Description   :    UI_Win_Finger2Mediator
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

    public class Finger2ViewData
    {
        public Vector3 BeginWorldPos { get; set; }
        public Vector3 EndWorldPos { get; set; }
    }

    public class UI_Win_Finger2Mediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_Finger2Mediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_Finger2Mediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_Finger2View view;

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
            if(m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_data = view.data as Finger2ViewData;
            m_timer = Timer.Register(0, null, MoveFinder, true);
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void MoveFinder(float deltaTime)
        {
            m_moveTime += Time.deltaTime;
            if (m_moveTime >= m_totalMoveTime) m_moveTime = 0;
            Vector3 curWorldPos = Vector3.Lerp(m_data.BeginWorldPos, m_data.EndWorldPos, m_moveTime / m_totalMoveTime);
            Vector2 screenPos = WorldCamera.Instance().GetCamera().WorldToScreenPoint(curWorldPos);
            Vector2 uiPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.m_pl_arrow, screenPos, CoreUtils.uiManager.GetUICamera(), out uiPos);
            view.m_img_arrow_PolygonImage.rectTransform.anchoredPosition = uiPos;
        }

        private float m_moveTime = 0;
        private float m_totalMoveTime = 1f;
        private Finger2ViewData m_data;
        private Timer m_timer = null;
    }
}