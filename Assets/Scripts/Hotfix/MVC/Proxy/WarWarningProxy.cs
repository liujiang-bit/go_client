// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    WarWarningProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SprotoType;

namespace Game {

    public enum WarWarningType
    {
        None,
        Scout,
        War,
        Reinforce,
        Transport,
    }


    public class WarWarningProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "WarWarningProxy";



        #endregion

        // Use this for initialization
        public WarWarningProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" WarWarningProxy register");   
        }


        public override void OnRemove()
        {
            Debug.Log(" WarWarningProxy remove");
            m_dictWarningInfo.Clear();
        }

        public void UpdateWarWarningInfo(Role_EarlyWarningInfo.request request)
        {
            foreach(var info in request.earlyWarningInfo)
            {
                if (info.Value.HasIsDelete && info.Value.isDelete)
                {
                    if(m_dictWarningInfo.ContainsKey(info.Key))
                    {
                        m_dictWarningInfo.Remove(info.Key);
                    }
                }
                else
                {
                    EarlyWarningInfoEntity entity = null;
                    if(!m_dictWarningInfo.TryGetValue(info.Key, out entity))
                    {
                        entity = new EarlyWarningInfoEntity();
                        m_dictWarningInfo.Add(info.Key, entity);
                    }
                    EarlyWarningInfoEntity.updateEntity(entity, info.Value);
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.WarWarningInfoChanged);
        }

        public void Clear()
        {
            m_dictWarningInfo.Clear();
        }

        public List<EarlyWarningInfoEntity> GetWarWarningInfoList()
        {
            List<EarlyWarningInfoEntity> warningList = new List<EarlyWarningInfoEntity>();
            warningList.AddRange(m_dictWarningInfo.Values);
            return warningList;
        }

        public EarlyWarningInfoEntity GetLastNotIgnoreWarning()
        {
            if (m_dictWarningInfo.Count == 0) return null;
            EarlyWarningInfoEntity lastInfo = null;
            foreach (var info in m_dictWarningInfo)
            {
                if(!info.Value.isShield && (lastInfo == null || info.Key > lastInfo.earlyWarningIndex))
                {
                    lastInfo = info.Value;
                }
            }
            return lastInfo;
        }
        public EarlyWarningInfoEntity GetLastWarning()
        {
            if (m_dictWarningInfo.Count == 0) return null;
            EarlyWarningInfoEntity lastInfo = null;
            foreach (var info in m_dictWarningInfo)
            {
                if (lastInfo == null || info.Key > lastInfo.earlyWarningIndex)
                {
                    lastInfo = info.Value;
                }
            }
            return lastInfo;
        }
        
        public static long GetSoldierCount(EarlyWarningInfoEntity info)
        {
            long count = 0;
            foreach (var soldier in info.attackSoldiers)
            {
                count += soldier.Value.num;
            }
            return count;
        }

        public static string GetWarningSkinName(WarWarningType warningType, bool isRally)
        {
            string iconName = string.Empty;
            switch (warningType)
            {
                case WarWarningType.Scout:
                    iconName = "btn_mmu_1020_4";
                    break;
                case WarWarningType.War:
                    if(isRally)
                    {
                        iconName = "btn_mmu_1020_1";
                    }
                    else
                    {
                        iconName = "btn_mmu_1020_3";
                    }
                    break;
                case WarWarningType.Reinforce:
                    iconName = "btn_mmu_1020_2";
                    break;
                case WarWarningType.Transport:
                    iconName = "btn_mmu_1020_2";
                    break;
            }
            return iconName;
        }

        private Dictionary<long, EarlyWarningInfoEntity> m_dictWarningInfo = new Dictionary<long, EarlyWarningInfoEntity>();
    }
}