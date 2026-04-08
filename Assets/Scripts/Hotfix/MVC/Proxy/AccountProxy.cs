// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    AccountProxy 
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Hotfix;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{

    public class AccountProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "AccountProxy";

        private bool m_isShowBindReddot;

        #endregion

        // Use this for initialization
        public AccountProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" AccountProxy register");

        }

        public override void OnRemove()
        {
            Debug.Log(" AccountProxy remove");
        }

        public void Init()
        {
        
        }

        public void SetBindReddotStatus(bool isShow)
        {
            m_isShowBindReddot = isShow;
            AppFacade.GetInstance().SendNotification(CmdConstant.AccountBindReddotStatus, isShow);
        }

        public bool GetBindReddotStatus()
        {
            return m_isShowBindReddot;
        }
    }
}