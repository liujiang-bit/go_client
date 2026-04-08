// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    InputGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;
using Skyunion;
using System;

namespace Game {
    public struct Touche3DData
    {
        public int x;
        public int y;
        public string parentName;
        public string colliderName;


        public Touche3DData(int arg1, int arg2, string arg3, string arg4) : this()
        {
            this.x = arg1;
            this.y = arg2;
            this.parentName = arg3;
            this.colliderName = arg4;
        }
    }
    public class InputGlobalMediator : GameMediator {
        #region Member
        public static string NameMediator = "InputGlobalMediator";

        #endregion

        //IMediatorPlug needs
        public InputGlobalMediator():base(NameMediator, null ) {}

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

        protected override void InitData()
        {
            CoreUtils.inputManager.SetTouch3DEvent(OnTouche3DBegin, OnTouche3D, OnTouche3DEnd, OnTouche3DReleaseOutside);
        }

        private void OnTouche3DReleaseOutside(int arg1, int arg2, string arg3, string arg4)
        {
            AppFacade.GetInstance().SendNotification( CmdConstant.OnTouche3DReleaseOutside, new Touche3DData(arg1, arg2, arg3, arg4));
        }

        private void OnTouche3DEnd(int arg1, int arg2, string arg3, string arg4)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnTouche3DEnd, new Touche3DData(arg1, arg2, arg3, arg4));
        }

        private void OnTouche3D(int arg1, int arg2, string arg3, string arg4)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnTouche3D, new Touche3DData(arg1, arg2, arg3, arg4));
        }

        private void OnTouche3DBegin(int arg1, int arg2, string arg3, string arg4)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnTouche3DBegin, new Touche3DData(arg1, arg2, arg3, arg4));
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

        #endregion
    }
}