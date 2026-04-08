// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    GuideProxy 新手引导
// Copyright IGG All rights reserved.
// ===============================================================================

using Data;
using Skyunion;
using SprotoType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumNewbieGuide
{
    CreateFarm   = 1,       //创建农场
    TrainSoldier = 2,       //训练士兵
    UpgradeWall  = 3,       //升级城墙
    CreateTavern = 4,       //创建酒馆
    OpenTavernBox = 5,      //开启酒馆宝箱
    SearchMonster = 6,      //搜索怪物
    AttackMonster = 7,      //攻击怪物
    GetSecondHero = 8,      //获得第二统帅
    UpgradeCity = 9,        //升级市政厅
    CreateScoutCamp = 10,   //创建斥候营地
    ExploreFog = 11,        //探索迷雾
    ChapterTaskReward = 12, //第一章节奖励领取
}

namespace Game {
    public class GuideProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "GuideProxy";

        private PlayerProxy m_playerProxy;
        private TaskProxy m_taskProxy;
        private CityBuildingProxy m_cityBuildingProxy;

        public static bool IsOpenGuide = false;
        public static bool IsLightingGuideControl = false;      //是否是lighting引导控制阶段
        public static bool IsGuideing = false;                  //是否正在引导中

        public static bool IsTestMonsterAttackVillage = false;

        private bool m_isChangeCityPos;
        private PosInfo m_roleCityPos;
        private List<Vector2Int> m_needHideFogList;

        private Dictionary<int, List<int>> m_stageDataDic;

        #endregion

        // Use this for initialization
        public GuideProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" GuideProxy register");
        }

        public override void OnRemove()
        {
            Debug.Log(" GuideProxy remove");
            if (m_stageDataDic != null)
            {
                m_stageDataDic.Clear();
                m_stageDataDic = null;
            }
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
        }

        public Vector2 GetGuideCityPos()
        {
            return new Vector2(1140 * 600, 518 * 600);
        }

        public void SetCityPos(PosInfo info)
        {
            m_roleCityPos = info;
            m_isChangeCityPos = true;
        }

        public void SetCityPosStatus(bool isBool)
        {
            m_isChangeCityPos = isBool;
        }

        public PosInfo GetCityPos()
        {
            return m_roleCityPos;
        }

        public bool IsChangeCityPos()
        {
            return m_isChangeCityPos;
        }

        public void SetNeedHideFogList(List<Vector2Int> list)
        {
            m_needHideFogList = list;
        }

        public List<Vector2Int> GetNeedHideFogList()
        {
            return m_needHideFogList;
        }

        public Dictionary<int, List<int>> GetStageDataDic()
        {
            InitStageDataDic();
            return m_stageDataDic;
        }

        private void InitStageDataDic()
        {
            if (m_stageDataDic == null)
            {
                m_stageDataDic = new Dictionary<int, List<int>>();
                List<GuideDefine> list = CoreUtils.dataService.QueryRecords<GuideDefine>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!m_stageDataDic.ContainsKey(list[i].stage))
                    {
                        m_stageDataDic[list[i].stage] = new List<int>();
                    }
                    m_stageDataDic[list[i].stage].Add(list[i].ID);
                }
            }
        }

        //获取下一个需要引导阶段
        public int GetNeedNextGuideStage(int currStage)
        {
            InitStageDataDic();

            //找出满足条件且尚未引导的阶段
            int findStage = -1;
            foreach (var data in m_stageDataDic)
            {
                //判断是否已引导
                if (data.Key > currStage && !IsCompletedByStage(data.Key))
                {
                    findStage = data.Key;
                }
                if (findStage > -1)
                {
                    break;
                }
            }
            return findStage;
        }

        public Int64 ConvertStage(int stage)
        {
            int num = stage + 1;
            return (Int64)(Math.Pow(2, ((int)num - 2)));
        }

        //是否有已完成的记录
        private bool IsHasCompletedRecord(int stage)
        {
            if (!GuideProxy.IsOpenGuide)
            {
                return true;
            }
            Int64 stage2 = ConvertStage(stage);
            if ((m_playerProxy.CurrentRoleInfo.noviceGuideStep & stage2) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IdEqual(int id, int stage, int step)
        {
            int val = GetId(stage, step);
            if (id == val)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetId(int stage, int step)
        {
            int val = stage * 100 + step;
            return val;
        }

        //是否已完成打怪引导
        public bool IsCompleteAttackMonsterGuide()
        {
            return IsCompletedByStage((int)EnumNewbieGuide.AttackMonster);
        }

        //引导是否已完成
        public bool IsCompletedByStage(int stage)
        {
            if (!GuideProxy.IsOpenGuide)
            {
                return true;
            }
            if (IsHasCompletedRecord(stage))
            {
                return true;
            }
            if (stage == (int)EnumNewbieGuide.CreateFarm)
            {
                //判断下是否有农场建筑
                BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Farm));
                if (buildingInfoEntity != null)
                {
                    RequestRecordGuideStage(stage);
                    return true;
                }
                return false;
            }
            else if (stage == (int)EnumNewbieGuide.TrainSoldier)
            {
                bool firstStageIsCompleted = IsCompletedByStage((int)EnumNewbieGuide.CreateFarm);
                if (!firstStageIsCompleted)
                {
                    return false;
                }
                ConfigDefine define = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                int initNum = 0;
                if (define != null && define.initialArmsNum != null)
                {
                    for (int i = 0; i < define.initialArmsNum.Count; i++)
                    {
                        initNum = initNum + define.initialArmsNum[i];
                    }
                }
                var soldiers = m_playerProxy.GetInArmyInfo();
                if (soldiers != null)
                {
                    foreach (var data in soldiers)
                    {
                        if (data.Value.num > initNum)
                        {
                            RequestRecordGuideStage(stage);
                            return true;
                        }
                    }
                }
            }
            else if (stage == (int)EnumNewbieGuide.UpgradeWall)
            {
                BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.CityWall));
                if (buildingInfoEntity != null)
                {
                    if (buildingInfoEntity.level > 1)
                    {
                        RequestRecordGuideStage(stage);
                        return true;
                    }
                    else
                    {
                        //判断下城墙建筑是否在升级
                        Dictionary<Int64, QueueInfo> buildQueue = m_playerProxy.GetBuildQueue();
                        if (buildQueue != null)
                        {
                            foreach (var data in buildQueue)
                            {
                                if (data.Value.buildingIndex == buildingInfoEntity.buildingIndex)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (stage == (int)EnumNewbieGuide.CreateTavern)
            {
                BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Tavern));
                if (buildingInfoEntity != null)
                {
                    RequestRecordGuideStage(stage);
                    return true;
                }
                return false;
            }
            else if (stage == (int)EnumNewbieGuide.OpenTavernBox)
            {
                Int64 civilization = m_playerProxy.GetCivilization();
                CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)civilization);
                if (define != null)
                {
                    bool isFind = false;
                    var m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                    List<HeroProxy.Hero> ownList = null;
                    List<HeroProxy.Hero> summonList = null;
                    List<HeroProxy.Hero> onsummonList = null;
                    m_heroProxy.GetHerosBySort(out ownList, out summonList, out onsummonList, HeroProxy.SortType.None);
                    if (ownList != null && ownList.Count > 0)
                    {
                        for (int i = 0; i < ownList.Count; i++)
                        {
                            if (ownList[i].data.heroId == define.initialHero)
                            {
                                isFind = true;
                                break;
                            }
                        }
                    }
                    if (isFind)
                    {
                        RequestRecordGuideStage(stage);
                        return true;
                    }
                }
            }
            else
            {
                if (stage == (int)EnumNewbieGuide.AttackMonster || stage == (int)EnumNewbieGuide.GetSecondHero)
                {
                    bool isFind = false;
                    var m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                    List<HeroProxy.Hero> ownList = null;
                    List<HeroProxy.Hero> summonList = null;
                    List<HeroProxy.Hero> onsummonList = null;
                    m_heroProxy.GetHerosBySort(out ownList, out summonList, out onsummonList, HeroProxy.SortType.None);
                    int guideHero = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).guideHero;
                    if (ownList != null && ownList.Count > 0)
                    {
                        for (int i = 0; i < ownList.Count; i++)
                        {
                            if (ownList[i].data.heroId == guideHero)
                            {
                                isFind = true;
                                break;
                            }
                        }
                    }
                    if (isFind)
                    {
                        RequestRecordGuideStage(stage);
                        return true;
                    }
                }
                if (stage == (int)EnumNewbieGuide.UpgradeCity)//升级市政厅
                {
                    if (m_playerProxy.GetTownHall() >= 2)
                    {
                        if (m_taskProxy.GetTaskState(101001) == TaskState.received) //是否已领取过奖励
                        {
                            RequestRecordGuideStage(stage);
                            return true;
                        }
                    }
                }
                else if (stage == (int)EnumNewbieGuide.CreateScoutCamp) // 建造斥候营地
                {
                    BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.ScoutCamp));
                    if (buildingInfoEntity != null)
                    {
                        if (m_taskProxy.GetTaskState(101002) == TaskState.received)
                        {
                            RequestRecordGuideStage(stage);
                            return true;
                        }
                    }
                }
                else if (stage == (int)EnumNewbieGuide.ExploreFog)// 探索迷雾
                {
                    if (m_taskProxy.GetTaskState(101003) == TaskState.received) //是否已领取过奖励
                    {
                        RequestRecordGuideStage(stage);
                        return true;
                    }
                }
                else if (stage == (int)EnumNewbieGuide.ChapterTaskReward) // 章节奖励领取
                {
                    if (m_playerProxy.CurrentRoleInfo.chapterId > 1) //如果已经进行到第2章节 则表示引导已结束
                    {
                        RequestRecordGuideStage(stage);
                        return true;
                    }
                }
            }

            return false;
        }

        //请求记录引导数据
        public void RequestRecordGuideStage(int stage, int step = 0)
        {
            if (IsTestMonsterAttackVillage)
            {
                return;
            }
            m_playerProxy.CurrentRoleInfo.noviceGuideStep = m_playerProxy.CurrentRoleInfo.noviceGuideStep | ConvertStage(stage);
            Debug.LogFormat("保存引导阶段:{0} 当前记录：{1} 转换：{2},,,{3}", stage, m_playerProxy.CurrentRoleInfo.noviceGuideStep, ConvertStage(stage), step);
            var sp = new Role_NoviceGuideStep.request();
            sp.noviceGuideStep = m_playerProxy.CurrentRoleInfo.noviceGuideStep;
            sp.noviceGuideDetailStep = step;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //请求记录每一步步骤
        public void RequestRecordGuideStep(int stage, int step, int guideId)
        {
            if (IsTestMonsterAttackVillage)
            {
                return;
            }
            Debug.LogFormat("保存引导阶段:{0} 当前记录：{1} 转换：{2}", stage, m_playerProxy.CurrentRoleInfo.noviceGuideStep, ConvertStage(stage));
            var sp = new Role_NoviceGuideStep.request();
            sp.noviceGuideStep = 0;
            sp.noviceGuideDetailStep = 0;
            sp.guideId = guideId;
            AppFacade.GetInstance().SendSproto(sp); 
            if (stage == 11 && step == 2)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.tutorial_completion));
            }
        }

    }
}