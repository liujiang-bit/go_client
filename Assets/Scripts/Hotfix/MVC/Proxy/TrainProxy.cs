// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    TrainProxy 训练部队
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
    public class TrainProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "TrainProxy";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private SoldierProxy m_soldierProxy;

        public static int OpenTrainViewType;

        #endregion

        // Use this for initialization
        public TrainProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" TrainProxy register");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
        }

        public override void OnRemove()
        {
            Debug.Log(" TrainProxy remove");
        }

        //获取时代名称
        public static string GetAgeStr(int type)
        {
            CityAgeSizeDefine define = CoreUtils.dataService.QueryRecord<CityAgeSizeDefine>(type);
            if (define != null)
            {
                return LanguageUtils.getText(define.l_nameId);
            }
            return "";
        }

        public static string GetBuildingName(int type)
        {
            //todo
            if (type == (int)EnumCityBuildingType.Barracks)
            {
                return "兵营";
            }
            else if (type == (int)EnumCityBuildingType.Stable)
            {
                return "马厩";
            }
            else if (type == (int)EnumCityBuildingType.ArcheryRange)
            {
                return "靶场";
            }
            else if (type == (int)EnumCityBuildingType.SiegeWorkshop)
            {
                return "攻城武器厂";
            }
            return "";
        }

        //正在训练中
        public bool IsTraining()
        {
            var trainQueue = m_playerProxy.GetTrainQueue();
            if (trainQueue != null)
            {
                foreach (var data in trainQueue)
                {
                    if (data.Value.buildingIndex>0 && data.Value.finishTime > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //获取训练信息
        public QueueInfo GetTrainInfo(Int64 buildingIndex)
        {
            var trainQueue = m_playerProxy.GetTrainQueue();
            if (trainQueue != null)
            {
                foreach (var data in trainQueue)
                {
                    if (data.Value.buildingIndex == buildingIndex && data.Value.finishTime > 0)
                    {
                        return data.Value;
                    }
                }
            }
            return null;
        }

        //获取训练建筑最高可训练兵种
        public ArmsDefine GetMaxUnlockSoldier(Int64 buildingIndex)
        {
            BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
            if (info == null)
            {
                return null;
            }
            int armyType = 0;
            if (info.type == (int)EnumCityBuildingType.Barracks)
            {
                armyType = (int)EnumSoldierType.Infantry;
            }
            else if (info.type == (int)EnumCityBuildingType.Stable)
            {
                armyType = (int)EnumSoldierType.Cavalry;
            }
            else if (info.type == (int)EnumCityBuildingType.ArcheryRange)
            {
                armyType = (int)EnumSoldierType.Bowmen;
            }
            else if (info.type == (int)EnumCityBuildingType.SiegeWorkshop)
            {
                armyType = (int)EnumSoldierType.SiegeEngines;
            }
            else
            {
                Debug.LogFormat("异常类型：{0}", info.type);
                return null;
            }
            ArmsDefine findDefine = null;

            for (int i = 0; i < 5; i++)
            {
                int id = m_soldierProxy.GetTemplateId(armyType, i + 1);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (define != null)
                {
                    if (m_soldierProxy.IsUnlock(define))
                    {
                        findDefine = define;
                    }
                }
            }
            return findDefine;
        }

        public string GetMaxSoldier()
        {
            int   armyType = (int)EnumSoldierType.Bowmen;
            ArmsDefine findDefine = null;

            for (int i = 0; i < 5; i++)
            {
                int id = m_soldierProxy.GetTemplateId(armyType, i + 1);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (define != null)
                {
                    if (m_soldierProxy.IsUnlock(define))
                    {
                        findDefine = define;
                    }
                }
            }
            if (findDefine == null)
            {
                return "";
            }
            return findDefine.armsModel;
        }

        public ArmsDefine GetArmsDefine(int type, int level)
        {
            int id = m_soldierProxy.GetTemplateId(type, level);
            ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
            return define;
        }
    }
}