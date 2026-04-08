// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    GlobalCityMenuHudMediator
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
using Data;
using DG.Tweening;

namespace Game
{
    public class GlobalWorldHudMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "GlobalWorldHudMediator";

        #endregion

        //IMediatorPlug needs
        public GlobalWorldHudMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalWorldHudMediator(object viewComponent) : base(NameMediator, null) { }

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

        public override void Update()
        {

        }

        protected override void InitData()
        {
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