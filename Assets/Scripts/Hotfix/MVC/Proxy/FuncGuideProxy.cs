// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    FuncGuideProxy 功能介绍引导
// Copyright IGG All rights reserved.
// ===============================================================================

using Data;
using Skyunion;
using SprotoType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class FuncGuideProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "FuncGuideProxy";

        private PlayerProxy m_playerProxy;

        public static bool IsGuideing = false;                  //是否正在引导中

        #endregion

        // Use this for initialization
        public FuncGuideProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" FuncGuideProxy register");
        }

        public override void OnRemove()
        {
            Debug.Log(" FuncGuideProxy remove");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        //是否有已完成的记录
        private bool IsHasCompletedRecord(int stage)
        {
            Int64 stage2 = ConvertStage(stage);
            if (m_playerProxy.CurrentRoleInfo != null && (m_playerProxy.CurrentRoleInfo.noviceGuideStepEx & stage2) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsCompletedByStage(int stage)
        {
            if (IsHasCompletedRecord(stage))
            {
                return true;
            }
            return false;
        }

        public Int64 ConvertStage(int stage)
        {
            int num = stage + 1;
            return (Int64)(Math.Pow(2, ((int)num - 2)));
        }

        //请求记录引导数据
        public void RequestRecordGuideStage(int stage)
        {
            m_playerProxy.CurrentRoleInfo.noviceGuideStepEx = m_playerProxy.CurrentRoleInfo.noviceGuideStepEx | ConvertStage(stage);
            Debug.LogFormat("保存功能引导阶段:{0} 当前记录：{1} 转换：{2}", stage, m_playerProxy.CurrentRoleInfo.noviceGuideStepEx, ConvertStage(stage));
            var sp = new Role_NoviceGuideStepEx.request();
            sp.noviceGuideStepEx = m_playerProxy.CurrentRoleInfo.noviceGuideStepEx;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //请求记录功能引导id
        public void RequestRecordGuideId(int stage, int guideId)
        {
            Debug.LogFormat("保存功能引导id:{0} ", guideId);
            var sp = new Role_NoviceGuideStepEx.request();
            sp.noviceGuideStepEx = m_playerProxy.CurrentRoleInfo.noviceGuideStepEx;
            sp.guideId = guideId;
            AppFacade.GetInstance().SendSproto(sp);
        }
    }
}