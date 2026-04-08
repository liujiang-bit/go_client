// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    ScoutProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game {
    public class EffectinfoProxy : GameProxy {
    
        #region Member
        public const string ProxyNAME = "EffectinfoProxy";
        private List<EffectInfoDefine> effectInfoDefineList = new List<EffectInfoDefine>();

        #endregion

        // Use this for initialization
        public EffectinfoProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
//            if (effectInfoDefineList.Count == 0)
//            {
//                effectInfoDefineList  = CoreUtils.dataService.QueryRecords<EffectInfoDefine>();
//            }

        }

        public override void OnRemove()
        {

        }

        public EffectInfoDefine GetEffectInfoDefineByName(string name)
        {
            EffectInfoDefine effectInfoDefine = null;
            for (int i = 0; i < effectInfoDefineList.Count; i++)
            {
                if (string.Equals(effectInfoDefineList[i].effectID, name))
                {
                    effectInfoDefine = effectInfoDefineList[i];
                    break;
                }
            }
            return effectInfoDefine;
        }

    }
}