// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月21日
// Update Time         :    2020年7月21日
// Class Description   :    BattleMainlMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;

namespace Game {
    public class BattleMainlMediator : GameMediator {
        #region Member
        public static string NameMediator = "BattleMainlMediator";

        #endregion

        //IMediatorPlug needs
        public BattleMainlMediator():base(NameMediator, null ) {}

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ExpeditionInfoChange,
            }.ToArray();
        }

       public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ExpeditionInfoChange:
                    {
                        UpdateRedPointNumber();
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
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

        public int CalculateRedPointNumber()
        {
            int num = 0;
            num += GetExpeditionRedPointNumber();
            return num;
        }

        private void UpdateRedPointNumber()
        {
            int num = CalculateRedPointNumber();
            AppFacade.GetInstance().SendNotification(CmdConstant.BattleMainRedPointChanged, num);
        }

        public int GetExpeditionRedPointNumber()
        {
            if(m_playerProxy.CurrentRoleInfo.expeditionInfo == null || m_playerProxy.CurrentRoleInfo.expeditionInfo.Count ==0)
            {
               if (SystemOpen.IsSystemOpen(EnumSystemOpen.war,false))
                {
                    if (SystemOpen.IsSystemOpen(EnumSystemOpen.war_expedition,false))
                    {
                        return 1;
                    }
                }
                return 0;
            }
            int num = 0;
            foreach(var info in m_playerProxy.CurrentRoleInfo.expeditionInfo)
            {
                if(info.Value.star == 3 && !info.Value.reward)
                {
                    num++;
                }
            }
            return num;
        }

        private PlayerProxy m_playerProxy = null;
    }
}