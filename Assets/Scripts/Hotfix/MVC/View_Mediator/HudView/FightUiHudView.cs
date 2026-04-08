// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    战斗HudView 基类
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using System;
using Game;
using Hotfix;

namespace Client
{
    public class FightUiHudView
    {
        public HUDUI HuduiView;
        public int TroopId;
        public MapElementUI MapElementUi;
        public bool IsFighting = true;
        public float FadeOutTime;
        protected BattleUIData battleUiData;
        protected TroopProxy m_TroopProxy;
        protected RssProxy m_RssProxy;

        /// <summary>
        /// 攻击目标ID
        /// </summary>
        public int targetId;
        /// <summary>
        /// 站位层
        /// </summary>
        public int stanceLayer;        
        /// <summary>
        /// 站位角度
        /// </summary>
        public float stanceAngle;
        /// <summary>
        /// 最大站位层（为0代表没限制）
        /// </summary>
        protected int stanceMaxLayer = 2;
        /// <summary>
        /// 站位偏移
        /// </summary>
        protected float stanceOffset = 3;

        public FightUiHudView()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_RssProxy= AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
        }

        public virtual void Close()
        {
            if (HuduiView != null)
            {
                ClientUtils.hudManager.CloseSingleHud(ref HuduiView);
            }
        }

        public virtual void FadeOut()
        {
            IsFighting = false;
            FadeOutTime = 0;
            if (MapElementUi != null)
            {
                MapElementUi.SetUIStatus(false, 0);
                MapElementUi.SetUIFadeShow((int)MapElementUI.FadeType.AllFadeOut);
            }
        }

        public virtual void ShowFightStatus()
        {

        }
        

        public virtual void ShowHeadStatus()
        {
            
        }

        public void SetBattleUIData(BattleUIData battleUiData)
        {
            this.battleUiData = battleUiData;
            ShowHeadStatus();
        }
    }
}