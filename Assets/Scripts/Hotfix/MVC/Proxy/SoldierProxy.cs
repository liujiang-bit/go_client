// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    SoldierProxy
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
    public class SoldiersData
    {
        public int Id { get; set; }
        public ArmsDefine ArmysCfg { get; set; }
        public SoldierInfo ServerInfo { get; set; }
        public int Number { get; set; }
    }

    public class SoldierProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "SoldierProxy";

        private PlayerProxy m_playerProxy;
        private TroopProxy m_troopProxy;

        private Dictionary<int, List<int>> m_specialArmyDic = new Dictionary<int, List<int>>();
        private bool m_isReadSpecial;

        #endregion

        // Use this for initialization
        public SoldierProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" SoldierProxy register");
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
        }

        public override void OnRemove()
        {
            Debug.Log(" SoldierProxy remove");
        }

        //获取部队数据 包括在家 外出
        public Dictionary<Int64, Int64> GetAllSoldierByType(Int64 type)
        {
            Dictionary<Int64, Int64> dic = new Dictionary<Int64, Int64>();
            Dictionary<Int64, Int64> inDic = GetInSoldierByType(type);
            Dictionary<Int64, Int64> outDic = GetOutSoldierByType(type);
            foreach (var data in inDic)
            {
                if (dic.ContainsKey(data.Key))
                {
                    dic[data.Key] = dic[data.Key] + data.Value;
                }
                else
                {
                    dic[data.Key] = data.Value;
                }
            }
            foreach (var data in outDic)
            {
                if (dic.ContainsKey(data.Key))
                {
                    dic[data.Key] = dic[data.Key] + data.Value;
                }
                else
                {
                    dic[data.Key] = data.Value;
                }
            }
            return dic;
        }

        //获取外出部队
        public Dictionary<Int64, Int64> GetOutSoldierByType(Int64 type)
        {
            Dictionary<Int64, Int64> dic = new Dictionary<Int64, Int64>();
            var armies = m_troopProxy.GetArmys();
            if (armies != null)
            {
                foreach(var data in armies)
                {
                    if (data.soldiers != null)
                    {
                        foreach (var soldier in data.soldiers.Values)
                        {
                            if (soldier.type == type)
                            {
                                if (dic.ContainsKey(soldier.level))
                                {
                                    dic[soldier.level] = dic[soldier.level] + soldier.num;
                                }
                                else
                                {
                                    dic[soldier.level] = soldier.num;
                                }
                            }
                        }
                    }
                }
            }
            return dic;
        }

        //获取在家部队
        public Dictionary<Int64, Int64> GetInSoldierByType(Int64 type)
        {
            Dictionary<Int64, Int64> dic = new Dictionary<Int64, Int64>();
            var soldiers = m_playerProxy.GetInArmyInfo();
            if (soldiers != null)
            {
                foreach (var data in soldiers)
                {
                    if (data.Value.type == type)
                    {
                        if (dic.ContainsKey(data.Value.level))
                        {
                            dic[data.Value.level] = dic[data.Value.level] + data.Value.num;
                        }
                        else
                        {
                            dic[data.Value.level] = data.Value.num;
                        }
                    }
                }
            }
            return dic;
        }
        //获取士兵 城内，城外，训练中 0，所有士兵
        public long GetSoldierByType(Int64 type)
        {
            Int64 Soldier = 0;
            int tempId = 0;
            ArmsDefine tDefine = null;

            if (type == 0)
            {
                //待命士兵
                var soldiers = m_playerProxy.GetInArmyInfo();
                if (soldiers != null)
                {
                    foreach (var data in soldiers)
                    {
                        if (data.Value.num > 0)
                        {
                            Soldier = Soldier + data.Value.num;
                        }
                    }
                }
            }
            else
            {
                //待命士兵
                var soldiers = m_playerProxy.GetInArmyInfo();
                if (soldiers != null)
                {
                    foreach (var data in soldiers)
                    {
                        if (data.Value.num > 0 && type == (int)data.Value.type)
                        {

                            Soldier = Soldier + data.Value.num;
                        }
                    }
                }
            }
            if (type == 0)
            {
                var trainQueue = m_playerProxy.GetTrainQueue();
                if (trainQueue != null)
                {
                    foreach (var data in trainQueue)
                    {
                        if (data.Value.finishTime > 0 && data.Value.armyNum > 0)
                        {
                            if (data.Value.oldArmyLevel > 0) //正在晋升
                            {
                                Soldier = Soldier + data.Value.armyNum;
                            }
                        }
                    }
                }
            }
            else
            {
                var trainQueue = m_playerProxy.GetTrainQueue();
                if (trainQueue != null)
                {
                    foreach (var data in trainQueue)
                    {
                        if (data.Value.finishTime > 0 && data.Value.armyNum > 0)
                        {
                            if (data.Value.oldArmyLevel > 0 && type == (int)data.Value.armyType) //正在晋升
                            {
                                Soldier = Soldier + data.Value.armyNum;
                            }
                        }
                    }
                }
            }

            if (type == 0)
            {
                var outQueue = m_troopProxy.GetArmys();
                if (outQueue != null)
                {
                    foreach (var data in outQueue)
                    {
                        if (data.mainHeroId > 0)
                        {
                            if (data.soldiers != null)
                            {
                                foreach (var data2 in data.soldiers.Values)
                                {

                                    Soldier = Soldier + data2.num;
                                }
                            }
                            if (data.minorSoldiers != null)
                            {
                                foreach (var data2 in data.minorSoldiers.Values)
                                {
                                    Soldier = Soldier + data2.num;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var outQueue = m_troopProxy.GetArmys();
                if (outQueue != null)
                {
                    foreach (var data in outQueue)
                    {
                        if (data.mainHeroId > 0)
                        {
                            if (data.soldiers != null)
                            {
                                foreach (var data2 in data.soldiers.Values)
                                {
                                    if (type == (int)data2.type)
                                    {
                                        Soldier = Soldier + data2.num;
                                    }
                                }
                            }
                            if (data.minorSoldiers != null)
                            {
                                foreach (var data2 in data.minorSoldiers.Values)
                                {
                                    if (type == (int)data2.type)
                                    {
                                        Soldier = Soldier + data2.num;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //正在出征的士兵
            return Soldier;
        }

        public void SetSpecialArmy(bool isReset = false)
        {
            if (m_isReadSpecial)
            {
                if (!isReset)
                {
                    return;
                }
            }
            m_isReadSpecial = true;
            Int64 clId = m_playerProxy.GetCivilization();
            CivilizationDefine clDefine = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)clId);
            List<int> specialArmList = null;
            if (clDefine == null)
            {
                Debug.LogErrorFormat("not find Civilization:{0}", clId);
            }
            else
            {
                specialArmList = clDefine.featureArms;
            }
            if (specialArmList != null && specialArmList.Count > 0)
            {
                for (int i = 0; i < specialArmList.Count; i++)
                {
                    ArmsDefine tDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(specialArmList[i]);
                    if(tDefine == null)
                    {
                        Debug.LogErrorFormat("ArmsDefine not find:{0}", specialArmList[i]);
                        return;
                    }
                    List<int> tempList;
                    m_specialArmyDic.TryGetValue(tDefine.armsType, out tempList);
                    if (tempList == null)
                    {
                        tempList = new List<int>();
                        m_specialArmyDic[tDefine.armsType] = tempList;
                        for (int j = 0; j < 5; j++)
                        {
                            tempList.Add(GetId(tDefine.armsType, j + 1));
                        }
                    }
                    tempList[tDefine.armsLv - 1] = tDefine.ID;
                }
            }
        }

        //士兵是否解锁
        public bool IsUnlock(ArmsDefine armDefine)
        {
            if (armDefine.studyID < 1)
            {
                return true;
            }
            if (m_playerProxy.IsTechnologyUnlock(armDefine.studyID))
            {
                return true;
            }
            return false;
        }

        //id
        private int GetId(int type, int level)
        {
            if (level > 5 || level < 1)
            {
                Debug.LogErrorFormat("士兵等级异常：{0} type:{1}", level, type);
            }
            return (10000 + type * 100 + level);
        }

        //获取模版id
        public int GetTemplateId(int type, int level)
        {
            if(!m_isReadSpecial)
            {
                SetSpecialArmy();
            }
            if (m_specialArmyDic.ContainsKey(type))
            {
                if(level>5)
                {
                    Debug.LogErrorFormat("士兵等级异常：{0} type:{1}", level, type);
                    return 0;
                }
                return m_specialArmyDic[type][level - 1];
            }
            else
            {
                return GetId(type, level);
            }
        }
        public static string GetArmyHeadIcon(int id)
        {
            ArmsDefine armDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
            if (armDefine == null)
            {
                return "";
            }
            return GetArmyHeadIcon(armDefine);
        }

        public static string GetArmyHeadIcon(ArmsDefine armDefine)
        {
            return armDefine.icon;
        }

        //领取训练士兵
        public void RequestReceiveSoldier(Int64 type)
        {
            var sp = new Role_AwardArmy.request();
            sp.type = type;
            if (GuideManager.Instance.IsGuideSoldierGet)
            {
                Debug.Log("引导领取 特殊处理");
                sp.guide = true;
            }
            AppFacade.GetInstance().SendSproto(sp);
        }

        // 获取部队总战力
        public Int64 GetTroopsTotalPower()
        {
            Int64 power = 0;
            int tempId = 0;
            ArmsDefine tDefine = null;

            //待命士兵战力
            var soldiers = m_playerProxy.GetInArmyInfo();
            if (soldiers != null)
            {
                foreach (var data in soldiers)
                {
                    if (data.Value.num > 0)
                    {
                        tempId = GetTemplateId((int)data.Value.type, (int)data.Value.level);
                        tDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                        if (tDefine != null)
                        {
                            power = power + data.Value.num * tDefine.militaryCapability;
                        }
                    }
                }
            }
            //正在晋升中的士兵战力
            var trainQueue = m_playerProxy.GetTrainQueue();
            if (trainQueue != null)
            {
                foreach (var data in trainQueue)
                {
                    if (data.Value.finishTime > 0 && data.Value.armyNum >0)
                    {
                        if (data.Value.oldArmyLevel > 0) //正在晋升
                        {
                            tempId = GetTemplateId((int)data.Value.armyType, (int)data.Value.oldArmyLevel);
                            tDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                            if (tDefine != null)
                            {
                                power = power + data.Value.armyNum * tDefine.militaryCapability;
                            }
                        }
                    }
                }
            }

            //正在出征的士兵战力
            var outQueue = m_troopProxy.GetArmys();
            if (outQueue != null)
            {
                foreach (var data in outQueue)
                {
                    if (data.mainHeroId > 0)
                    {
                        if (data.soldiers != null)
                        {
                            foreach (var data2 in data.soldiers.Values)
                            {
                                tempId = GetTemplateId((int)data2.type, (int)data2.level);
                                tDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                                if (tDefine != null)
                                {
                                    power = power + data2.num * tDefine.militaryCapability;
                                }
                            }
                        }
                    }
                }
            }
            return power;
        }

        public static void SortSoldierDataByAttackType(List<SoldiersData> datas, TroopAttackType type)
        {
            switch (type)
            {
                case TroopAttackType.Attack:
                    datas.Sort((SoldiersData data1, SoldiersData data2) =>
                    {
                        int re = data1.ServerInfo.type.CompareTo(data2.ServerInfo.type);
                        if (re == 0)
                        {
                            re = data2.ServerInfo.level.CompareTo(data1.ServerInfo.level);
                        }
                        return re;
                    });
                    break;
                case TroopAttackType.Collect:
                    datas.Sort((SoldiersData data1, SoldiersData data2) =>
                    {
                        int re = data1.ServerInfo.type.CompareTo(data2.ServerInfo.type);
                        if (re == 0)
                        {
                            re = data2.ServerInfo.level.CompareTo(data1.ServerInfo.level);
                        }
                        else
                        {
                            if (data1.ServerInfo.type == 4)
                            {
                                re = -1;
                            }
                            else if (data2.ServerInfo.type == 4)
                            {
                                re = 1;
                            }
                        }
                        return re;
                    });
                    break;
            }
        }      
    }
}