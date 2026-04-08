// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月5日
// Update Time         :    2019年12月5日
// Class Description   :    MoveCityProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using Client;
using Data;
using SprotoTemp;
using System;
using System.Linq;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class MoveCityProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "MoveCityProxy";

        private PlayerProxy m_playerProxy;
        private BagProxy m_bagProxy;
        private CityBuildingProxy m_cityBuildingProxy;

        #endregion
        // Use this for initialization
        public MoveCityProxy(string proxyName)
            : base(proxyName)
        {

        }
        public override void OnRegister()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            Debug.Log(" MoveCityProxy register");
        }


        public override void OnRemove()
        {
            Debug.Log(" CityManagerProxy remove");
        }

        public void SendUseItem()
        {
            
        }
    }
}