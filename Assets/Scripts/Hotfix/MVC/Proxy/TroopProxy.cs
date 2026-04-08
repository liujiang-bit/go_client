// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    TroopProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client;
using Data;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;
using System.Text;

namespace Game
{
    public class TroopPosInfo
    {
        public long x;
        public long y;
    }

    public class TroopDataObject
    {
        private SoldierProxy m_soldierProxy;
        private TroopProxy m_troopProxy;
        private RssProxy m_rssProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        public int id;

        public ArmyInfoEntity ArmyData;
        private Int64 m_soldierNum; //总兵数
        private Int64 m_totalWeight; //总负载
        private float m_speed; //当前行军速度

        private Int64 m_resWeight; //单个资源负载
        private float m_collectSpeed; //采集速度
        private Int64 m_useWeight; //已使用的负载 
        private float m_beforeWeight; //变速前已采集的负载
        private Int64 m_beforeTime; //变速前已采集的时间

        private attrType[] attrTypeArray = { attrType.infantryMoveSpeedMulti,
                                             attrType.cavalryMoveSpeedMulti,
                                             attrType.bowmenMoveSpeedMulti,
                                             attrType.siegeCarMoveSpeedMulti };

        public void Init(ArmyInfoEntity armyInfo)
        {
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_rssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;

            Update(armyInfo);
        }

        public void Update(ArmyInfoEntity armyInfo)
        {
            this.ArmyData = armyInfo;
            this.id = (int) armyInfo.armyIndex;

            //更新部队速度时需要额外附加战斗动态buff加成
            //由于战斗模块没有维护buff加成相关数据
            //联盟领地行军速度加成也包含在buff中
            //此做法并不优雅，乃权宜之计
            Dictionary<attrType, int> buffAttributeDic = new Dictionary<attrType, int>();
            MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(armyInfo.objectIndex);
            if (infoEntity != null)
            {
                foreach (var battleBuff in infoEntity.battleBuff)
                {
                    SkillStatusDefine skillStatus = CoreUtils.dataService.QueryRecord<SkillStatusDefine>((int)battleBuff.buffId);
                    if (skillStatus != null)
                    {
                        foreach (var aType in attrTypeArray)
                        {
                            if (skillStatus.attrType.Contains(aType.ToString()))
                            {
                                if (!buffAttributeDic.ContainsKey(aType))
                                {
                                    buffAttributeDic.Add(aType, 0); 
                                }

                                buffAttributeDic[aType] += skillStatus.attrNumber[skillStatus.attrType.IndexOf(aType.ToString())];
                            }
                        }
                    }
                }
            }

            //总负载
            Int64 totalWeight = 0;
            //士兵总数
            Int64 totalSoldier = 0;
            //速度
            float speed = -1;
            if (armyInfo.soldiers != null)
            {
                var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                var attributes = playerAttributeProxy.GetTroopAttribute(armyInfo.mainHeroId, armyInfo.deputyHeroId);
                List<float> lsSpeed = new List<float>();

                foreach (var soldier in armyInfo.soldiers.Values)
                {
                    totalSoldier = totalSoldier + soldier.num;
                    ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)soldier.id);
                    if (config != null)
                    {
                        totalWeight += (soldier.num * config.capacity);

                        float sppedBonus = attributes[(int)attrType.marchSpeedMulti - 1].origvalue;

                        switch (config.armsType)
                        {
                            case 1:
                                sppedBonus += attributes[(int) attrType.infantryMoveSpeedMulti - 1].origvalue;
                                if (buffAttributeDic.ContainsKey(attrType.infantryMoveSpeedMulti))
                                {
                                    sppedBonus += buffAttributeDic[attrType.infantryMoveSpeedMulti];
                                }
                                break;
                            case 2:
                                sppedBonus += attributes[(int) attrType.cavalryMoveSpeedMulti - 1].origvalue;
                                if (buffAttributeDic.ContainsKey(attrType.cavalryMoveSpeedMulti))
                                {
                                    sppedBonus += buffAttributeDic[attrType.cavalryMoveSpeedMulti];
                                }
                                break;
                            case 3:
                                sppedBonus += attributes[(int) attrType.bowmenMoveSpeedMulti - 1].origvalue;
                                if (buffAttributeDic.ContainsKey(attrType.bowmenMoveSpeedMulti))
                                {
                                    sppedBonus += buffAttributeDic[attrType.bowmenMoveSpeedMulti];
                                }
                                break;
                            case 4:
                                sppedBonus += attributes[(int) attrType.siegeCarMoveSpeedMulti - 1].origvalue;
                                if (buffAttributeDic.ContainsKey(attrType.siegeCarMoveSpeedMulti))
                                {
                                    sppedBonus += buffAttributeDic[attrType.siegeCarMoveSpeedMulti];
                                }
                                break;
                        }

                        lsSpeed.Add((config.speed * (1000 + sppedBonus) / 1000));
                    }

                }
                speed = lsSpeed.Count>0 ? lsSpeed.Min():0;           
                if (speed < 0)
                {
                    speed = 0;
                }

                //负载加成
                float multi = m_troopProxy.GetTroopsSpaceMulti(armyInfo.mainHeroId,armyInfo.deputyHeroId);
                totalWeight = (Int64) Mathf.Floor(totalWeight * multi);
            }

            this.m_soldierNum = totalSoldier;
            this.m_totalWeight = totalWeight;
            this.m_speed = speed;
        }

        //获取总负载
        public Int64 GetTotalWeight()
        {
            return m_totalWeight;
        }

        //获取士兵总数
        public long GetSoldierNum()
        {
            return m_soldierNum;
        }

        public long GetSoldierPower()
        {
            long sum = 0;
            foreach (var soldier in ArmyData.soldiers.Values)
            {
                int id = m_soldierProxy.GetTemplateId((int)soldier.type, (int)soldier.level);
                ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (config != null)
                {
                    sum += (soldier.num * config.militaryCapability);
                }
            }


            return sum;
        }

        //获取行军时间
        public int GetMarchTime(int rssId, Vector2 targetPos)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId((int)ArmyData.armyIndex);
            if (armyData != null)
            {
                //为了与服务端行军时间一致
                //需要扣除目标半径（如果是集结部队并且在视野范围外，需要扣除集结部队半径，目前需求还未明确）
                //需要扣除自身半径
                float radius = TroopHelp.GetRssRadius(rssId);
                float difValue = radius * 100 + armyData.armyRadius * 100;

                Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId);
                if (formation != null)
                {
                    Vector2 pos = new Vector2((int)Mathf.Floor(formation.gameObject.transform.position.x) * 100,
                        (int)Mathf.Floor(formation.gameObject.transform.position.z) * 100);
                    int times = (int)((TroopHelp.GetDistance(pos, targetPos) - difValue) / m_speed);
                    times = times > 0 ? times : 0;

                    return times;
                }

                if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING) ||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.RALLY_WAIT) ||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.GARRISONING) ||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.STATIONING))
                {
                    Vector2 pos = new Vector2((int)Mathf.Floor(armyData.Pos.x) * 100,
                        (int)Mathf.Floor(armyData.Pos.y) * 100);
                    int times = (int)((TroopHelp.GetDistance(pos, targetPos) - difValue) / m_speed);
                    times = times > 0 ? times : 0;

                    return times;
                }
                else
                {
                    Vector2 startPos = armyData.GetMovePos();
                    startPos.Set(startPos.x * 100, startPos.y * 100);
                    int times = (int)((TroopHelp.GetDistance(startPos, targetPos) - difValue) / m_speed);
                    times = times > 0 ? times : 0;
                    return times;
                }
            }

            return 0;
        }

        //计算采集数据
        public void CalCollectData()
        {
            //已使用负载
            Int64 usedWeight = 0;
            if (ArmyData.resourceLoads != null)
            {
                for (int i = 0; i < ArmyData.resourceLoads.Count; i++)
                {
                    int resType = ArmyInfoHelp.Instance.GetResType(ArmyData.resourceLoads[i]);
                    if (resType > 0)
                    {
                        usedWeight = usedWeight + m_rssProxy.GetResWeight(resType) * ArmyData.resourceLoads[i].load;
                    }
                }
            }

            this.m_useWeight = usedWeight;

            //正在采集的资源
            this.m_resWeight = 0;
            this.m_collectSpeed = 0;
            this.m_beforeWeight = 0;
            this.m_beforeTime = 0;
            if (ArmyData.collectResource != null)
            {
                int resType = ArmyInfoHelp.Instance.GetResType(ArmyData.collectResource);
                if (resType > 0)
                {
                    this.m_resWeight = m_rssProxy.GetResWeight(resType);

                    //计算变速前已采集负载
                    if (ArmyData.collectResource.collectSpeeds != null)
                    {
                        for (int i = 0; i < ArmyData.collectResource.collectSpeeds.Count; i++)
                        {
                            float collectVal = ArmyData.collectResource.collectSpeeds[i].collectTime *
                                               ArmyData.collectResource.collectSpeeds[i].collectSpeed / 10000;
                            this.m_beforeWeight = m_beforeWeight + collectVal * m_resWeight;
                            this.m_beforeTime = m_beforeTime + ArmyData.collectResource.collectSpeeds[i].collectTime;
                        }
                    }

                    //每秒采集速度
                    this.m_collectSpeed = (float) ArmyData.collectResource.collectSpeed / 10000;
                }
            }

            this.m_collectSpeed = (m_collectSpeed == 0) ? 1 : m_collectSpeed;
        }

        //获取已采集的负载
        public Int64 GetCollectWeight()
        {
            if (ArmyData.collectResource != null)
            {
                Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
                Int64 costTime = serverTime - ArmyData.collectResource.startTime - m_beforeTime;
                Int64 num = (Int64) Mathf.Floor(m_useWeight + costTime * m_collectSpeed * m_resWeight + m_beforeWeight);
                num = num > m_totalWeight ? m_totalWeight : num;
                return num;
            }
            else
            {
                return m_useWeight;
            }
        }
    }


    public class TroopProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "TroopProxy";
        private IData m_HeroSave;
        private List<AttrInfoDefine> m_attrInfoDefine;
        private int curHeroType = 0;
        private HeroProxy m_HeroProxy;
        private PlayerProxy m_PlayerProxy;
        private CityBuffProxy m_CityBuffProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private SoldierProxy m_soldierProxy;
        private CityBuildingProxy m_CityBuildingProxy;
        private RssProxy m_RssProxy;
        private Dictionary<int, bool> mWarHeroDic = new Dictionary<int, bool>(); //已出征英雄
        public bool SituStation; //存储驻扎状态
        private MapViewLevel m_viewLevel;
        private SquareHelper m_SquareHelper;
        private int cfgWarFeverLevel;
        private ConfigDefine configDefine;
        private Dictionary<long, ArmyInfoEntity> m_armyInfos = new Dictionary<long, ArmyInfoEntity>();
        private Dictionary<long, TransportInfoEntity> m_transprotInfos = new Dictionary<long, TransportInfoEntity>();

        #endregion

        public TroopProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" TroopProxy register");
        }

        public void Init()
        {
            InitCfgData();
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;

            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_CityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_CityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_RssProxy= AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_SquareHelper = SquareHelper.Instance;
            m_SquareHelper.InitUnitPrefabDict();
            m_HeroSave = new TroopSave();

            configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (configDefine != null)
            {
                cfgWarFeverLevel = configDefine.activationWarFare;
            }
        }

        public override void OnRemove()
        {
            Debug.Log(" TroopProxy remove");
            if (m_HeroSave != null)
            {
                m_HeroSave.Clear();
            }

            Clear();
        }

        public void Clear()
        {
            m_armyInfos.Clear();
            m_transprotInfos.Clear();
        }

        #region Army
        public ArmyInfoEntity GetArmyByIndex(long id)
        {
            ArmyInfoEntity info = null;
            m_armyInfos.TryGetValue(id, out info);
            return info;
        }

        public void UpdateArmyInfo(Dictionary<long, ArmyInfo> ArmyInfoDic)
        {
            foreach (var data in ArmyInfoDic)
            {
                if(data.Value.HasMainHeroId && data.Value.mainHeroId == 0)
                {
                    if(m_armyInfos.ContainsKey(data.Key))
                    {
                        m_armyInfos.Remove(data.Key);
                    }
                }
                else
                {
                    ArmyInfoEntity armyInfo = null;
                    if (!m_armyInfos.TryGetValue(data.Key, out armyInfo))
                    {
                        armyInfo = new ArmyInfoEntity();
                        m_armyInfos[data.Key] = armyInfo;
                    }
                    ArmyInfoEntity.updateEntity(armyInfo, data.Value);                 
                }              
            }
            UpdateTroopQueue();
        }

        public List<ArmyInfoEntity> GetArmys()
        {
            List<ArmyInfoEntity> armyList = new List<ArmyInfoEntity>();
            armyList.AddRange(m_armyInfos.Values);
            return armyList;
        }
        

        public int GetArmyCount()
        {
            return m_armyInfos.Count;
        }

        public int GetArmyIndex(int mapObjectId)
        {
            foreach (var info in GetArmys())
            {
                if (info.targetArg.targetObjectIndex == mapObjectId)
                {
                    return (int)info.armyIndex;
                }
            }
            return 0;
        }

        public bool IsHaveArmyIndex(long mapObjectId, int armyId)
        {
            List<long> lsId= new List<long>();
            foreach (var armyInfo in GetArmys())
            {
                if (armyInfo.status == (long)ArmyStatus.GARRISONING ||
                    armyInfo.status == (long)ArmyStatus.COLLECTING)
                {
                    if (armyInfo.targetArg.targetObjectIndex == mapObjectId)
                    {
                        lsId.Add(armyInfo.armyIndex);
                    }
                }
            }

            foreach (var id in lsId)
            {
                if (id == armyId)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Transport
        public void UpdateTransportInfo(INotification notification)
        {
            Transport_TransportList.request transportInfo = notification.Body as Transport_TransportList.request;
            if (transportInfo == null)
            {
                return;
            }
            foreach (var info in transportInfo.transportInfo)
            {
                if(info.Value.targetObjectIndex == -1)
                {
                    if (m_transprotInfos.ContainsKey(info.Key))
                    {
                        m_transprotInfos.Remove(info.Key);
                    }
                }
                else
                {
                    TransportInfoEntity data = null;
                    if (!m_transprotInfos.TryGetValue(info.Key, out data))
                    {
                        data = new TransportInfoEntity();
                        m_transprotInfos.Add(info.Key, data);
                    }
                    TransportInfoEntity.updateEntity(data, info.Value);
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.OnTroopDataChanged);
        }
        
        public List<TransportInfoEntity> GetAllTransportInfos()
        {
            List<TransportInfoEntity> infos = new List<TransportInfoEntity>();
            infos.AddRange(m_transprotInfos.Values);
            return infos;
        }

        public TransportInfoEntity GetTransportInfo(int id)
        {
            TransportInfoEntity info = null;
            m_transprotInfos.TryGetValue(id, out info);
            return info;
        }
        #endregion


        public int GetAllTroopCount()
        {
            return m_armyInfos.Count + m_transprotInfos.Count;
        }

        
        #region 静态部队阵型配置

        private void InitCfgData()
        {
            InitCfgRowData();
            InitCfgRowWidthData();
            InitCfgWardData();
            InitSqareOffset();
            InitCfgNumberBySum();
            InitPrefabs();
        }

        private void InitPrefabs()
        {
            List<ArmsDefine> lsArms = CoreUtils.dataService.QueryRecords<ArmsDefine>();
            foreach (var armsDefine in lsArms)
            {
                if (string.IsNullOrEmpty(armsDefine.armsModel))
                    continue;

                Matrix_Prefab prefab = new Matrix_Prefab();
                prefab.id = armsDefine.ID;
                prefab.name = armsDefine.armsModel;
                TroopsDatas.Instance.InitPrefabs(prefab);
            }

            List<HeroDefine> lsHeroS = CoreUtils.dataService.QueryRecords<HeroDefine>();
            foreach (var heroDefine in lsHeroS)
            {
                Matrix_Prefab prefab = new Matrix_Prefab();
                prefab.id = heroDefine.ID;
                prefab.name = heroDefine.heroAction;
                TroopsDatas.Instance.InitPrefabs(prefab);
            }

            ConfigDefine configScout = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            for (int i = 1; i < 4; i++)
            {
                Matrix_Prefab prefab = new Matrix_Prefab();
                prefab.id = i;
                switch (i)
                {
                    case 1:
                        prefab.name = configScout.toScoutsModel1;
                        break;
                    case 2:
                        prefab.name = configScout.toScoutsModel2;
                        break;
                    case 3:
                        prefab.name = configScout.toScoutsModel3;
                        break;
                }

                TroopsDatas.Instance.InitPrefabs(prefab);
            }
            
            Matrix_Prefab prefabtransport = new Matrix_Prefab();
            prefabtransport.id = 10;
            prefabtransport.name = configScout.transportModel;
            TroopsDatas.Instance.InitPrefabs(prefabtransport);

        }

        private void InitCfgRowData()
        {
            List<SquareMaxNumberDefine> ls = CoreUtils.dataService.QueryRecords<SquareMaxNumberDefine>();
            foreach (var item in ls)
            {
                TroopsDatas.Instance.InitCfgRowData((Troops.ENMU_MATRIX_TYPE) item.group, item.type, item.num);
            }
        }

        private void InitCfgRowWidthData()
        {
            List<SquareRowWidthDefine> ls = CoreUtils.dataService.QueryRecords<SquareRowWidthDefine>();
            foreach (var item in ls)
            {
                TroopsDatas.Instance.InitCfgRowWidthData((Troops.ENMU_MATRIX_TYPE) item.group, item.type,
                    item.RowWidth);
            }
        }

        private void InitCfgWardData()
        {
            List<SquareSpacingDefine> ls = CoreUtils.dataService.QueryRecords<SquareSpacingDefine>();
            foreach (var item in ls)
            {
                switch (item.towards)
                {
                    case 1:
                        TroopsDatas.Instance.InitCfgForwardSpacingData((Troops.ENMU_MATRIX_TYPE) item.group,
                            item.type, item.spacing);
                        break;
                    case 2:
                        TroopsDatas.Instance.InitCfgBackwardSpacingData((Troops.ENMU_MATRIX_TYPE) item.group,
                            item.type, item.spacing);

                        break;
                }
            }
        }

        private void InitSqareOffset()
        {
            List<SquareOffsetDefine> ls = CoreUtils.dataService.QueryRecords<SquareOffsetDefine>();
            foreach (var item in ls)
            {
                TroopsDatas.Instance.InitSquareOffset((Troops.ENMU_MATRIX_TYPE) item.group, item.type,
                    item.offsetX, item.offsetZ);
            }
        }

        private void InitCfgNumberBySum()
        {
            List<SquareNumberBySumDefine> ls = CoreUtils.dataService.QueryRecords<SquareNumberBySumDefine>();
            foreach (var item in ls)
            {
                TroopsDatas.Instance.InitNumberBySumData((Troops.ENMU_MATRIX_TYPE) item.group, item.type,
                    item.rangeMin, item.rangeMax, item.num);
            }
        }

        #endregion


        /// <summary>
        /// 目标名
        /// </summary>
        /// <param name="targetType"> #目的地类型(0 空地 1 攻击 2 增援 3 集结 4 采集 5 </param>
        /// <param name="MarchTargetArg">#目标参数</param>
        /// <returns></returns>
        public string GetTargetNameBytargetType(long targetType, MarchTargetArg MarchTargetArg)
        {
            string targetName = "";
            switch ((TroopAttackType) (targetType))
            {
                case TroopAttackType.Attack:
                {
                    MapObjectInfoEntity MapObject =
                        m_worldMapObjectProxy.GetWorldMapObjectByobjectId(MarchTargetArg.targetObjectIndex);
                    if (MapObject != null)
                    {
                        MonsterDefine monsterDefine =
                            CoreUtils.dataService.QueryRecord<MonsterDefine>((int) MapObject.monsterId);
                        if (monsterDefine != null)
                        {
                            targetName = LanguageUtils.getText(monsterDefine.l_nameId);
                        }
                    }
                }
                    break;
            }

            return targetName;
        }

        public SquareHelper GetSquareHelper()
        {
            return m_SquareHelper;
        }

        public void UpdateTroopQueue()
        {
            UpdateWarHero();
            AppFacade.GetInstance().SendNotification(CmdConstant.OnTroopDataChanged);
        }

        public IData CreateIDataFactory(IDataType t, int type = (int) IDataType.None)
        {
            if (t == IDataType.HeroSave)
            {
                return m_HeroSave;
            }
            return null;
        }

        public void Create(INotification notification)
        {
            Map_ObjectInfo.request mapItemInfo = notification.Body as Map_ObjectInfo.request;
            if (mapItemInfo != null)
            {
                int id = (int) mapItemInfo.mapObjectInfo.objectId;
                if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .IsContainTroop(id))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapLogicObjectChange, mapItemInfo); 
                    Debug.LogWarning("map已经存在当前id的部队了，服务器还通知创建" + id);
                    return;
                }

                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .CreateTroopData(notification);
            }
        }

        public string GetMonsterSoldiersDes(int heroId, int viceId, Dictionary<Int64, SoldierInfo> soldiers)
        {
            string des =
                m_SquareHelper.GetMapCreateTroopDes(heroId, viceId, soldiers, Troops.ENMU_MATRIX_TYPE.BARBARIAN);
            return des;
        }

        public static long GetFightingCount(Dictionary<int, int> selectedSoldiers)
        {
            long power = 0;
            foreach(var soldier in selectedSoldiers)
            {
                var armysCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>(soldier.Key);
                if (armysCfg == null) continue;
                power += armysCfg.militaryCapability * soldier.Value;             
            }
            return power;
        }

        public long GetArmySum(Dictionary<int, int> selectedSoldiers)
        {
            long sum = 0;
            foreach (var soldiers in selectedSoldiers)
            {
                sum += soldiers.Value;
            }
            return sum;
        }

        //获取负载
        public long GetArmyWeight(Dictionary<int, int> selectedSoldiers,long mainHeroId = 0, long deputyHeroId = 0,RssType type= RssType.None)
        {
            long sum = 0;
            foreach (var soldier in selectedSoldiers)
            {
                var armysCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>(soldier.Key);
                if (armysCfg == null) continue;
                sum += armysCfg.capacity * soldier.Value;
            }
            float multi = GetTroopsSpaceMulti(mainHeroId,deputyHeroId);
            int weight = ArmyInfoHelp.Instance.GetRssWeight(type);
            sum = (long) Mathf.Floor((sum * multi));
            long sum1 = sum / weight;
            return sum1;
        }

        //获取队伍负载加成
        public float GetTroopsSpaceMulti(long mainHeroId = 0, long deputyHeroId = 0)
        {

            return 1 + m_playerAttributeProxy.GetTroopAttribute(mainHeroId,deputyHeroId,attrType.troopsSpaceMulti).value;
        }

        //获取部队速度
        public float GetArmySpeed(Dictionary<int, int> selectedSoldiers,long mainHeroId = 0, long deputyHeroId = 0)
        {
            var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            float speed = -1;

            var attributes=  playerAttributeProxy.GetTroopAttribute(mainHeroId, deputyHeroId);
            
            List<float> lsSpeed= new List<float>();
            foreach (var soldier in selectedSoldiers)
            {
                if (soldier.Value == 0)
                    continue;

                var armysCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>(soldier.Key);
                if (armysCfg == null) continue;

                float sppedBonus = attributes[(int)attrType.marchSpeedMulti - 1].origvalue;

                switch (armysCfg.armsType)
                {
                    case 1:
                        sppedBonus += attributes[(int) attrType.infantryMoveSpeedMulti - 1].origvalue;
                        break;
                    case 2:
                        sppedBonus += attributes[(int) attrType.cavalryMoveSpeedMulti - 1].origvalue;
                        break;
                    case 3:
                        sppedBonus += attributes[(int) attrType.bowmenMoveSpeedMulti - 1].origvalue;
                        break;
                    case 4:
                        sppedBonus += attributes[(int) attrType.siegeCarMoveSpeedMulti - 1].origvalue;
                        break;
                }
                    
                lsSpeed.Add((armysCfg.speed * (1000 + sppedBonus) / 1000));
            }
            speed = lsSpeed.Count>0 ? lsSpeed.Min():0;           
            if (speed < 0)
            {
                speed = 0;
            }
            //获取联盟领地行军速度加成
            float guildSpeed = 0f;
            if (m_worldMapObjectProxy.IsPosInGuildArea(m_PlayerProxy.CurrentRoleInfo.pos.x/100f, m_PlayerProxy.CurrentRoleInfo.pos.y/100f, m_PlayerProxy.CurrentRoleInfo.guildId))
            {
                guildSpeed=attributes[(int) attrType.allTerrMoveSpeedMulti - 1].origvalue;
            }

            float heroMulti = (1000 + guildSpeed) / 1000;
            return speed * heroMulti;
        }

        public long GetHeroPower(int heroId, int viceId)
        {
            HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(heroId);
            HeroProxy.Hero heroVice = m_HeroProxy.GetHeroByID(viceId);
            long sum = 0;
            if (hero != null)
            {
                sum += hero.power;
            }

            if (heroVice != null)
            {
                sum += heroVice.power;
            }

            return sum;
        }


        public long GetTroopsPower()
        {
            long sum = 0;
            foreach(var armyInfo in m_armyInfos)
            {
                foreach (var soldier in armyInfo.Value.soldiers.Values)
                {
                    int id = m_soldierProxy.GetTemplateId((int)soldier.type, (int)soldier.level);
                    ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                    if (config != null)
                    {
                        sum += (soldier.num * config.militaryCapability);
                    }
                }

                sum += GetHeroPower((int)armyInfo.Value.mainHeroId, (int)armyInfo.Value.deputyHeroId);
            }

            return sum;
        }

        public long GetTroopPowerByArmyData(ArmyData armyData)
        {
            long sum = 0;
            foreach (var soldier in armyData.soldiers.Values)
            {
                int id = m_soldierProxy.GetTemplateId((int)soldier.type, (int)soldier.level);
                ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (config != null)
                {
                    sum += (soldier.num * config.militaryCapability);
                }
            }

            return sum;
        }

        public bool IsPlayOwnTroop(int troopId)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(troopId);
            if (armyData != null)
            {
                if (armyData.isGuide)
                {
                    return false;
                }
                return armyData.isPlayerHave;
            }            

            return false;
        }

        public bool IsScoutTroop(int id)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(id);
            if (armyData != null)
            {
                return armyData.troopType == RssType.Scouts;
            }

            return false;
        }

        public TroopAttackType GetTroopAttackType(int data)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(data);
            if (armyData != null)
            {
                return TroopAttackType.Attack;
            }

            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(data);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.rssType == RssType.Monster||
                    mapObjectInfoEntity.rssType== RssType.Guardian||
                    mapObjectInfoEntity.rssType== RssType.BarbarianCitadel||
                    mapObjectInfoEntity.rssType == RssType.SummonAttackMonster ||
                    mapObjectInfoEntity.rssType == RssType.SummonConcentrateMonster ||
                    mapObjectInfoEntity.rssType== RssType.CheckPoint||
                    mapObjectInfoEntity.rssType== RssType.HolyLand)
                {
                    return TroopAttackType.Attack;
                }

                if (TroopHelp.GetIsGuildType(mapObjectInfoEntity.rssType))
                {
                    bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 &&
                                   m_PlayerProxy.CurrentRoleInfo.guildId == mapObjectInfoEntity.guildId;

                    if (isGuild)
                    {
                        if (mapObjectInfoEntity.guildBuildStatus == (long) GuildBuildState.building)
                        {
                            return TroopAttackType.Reinforce;
                        }

                        if (TroopHelp.IsTouchGuildRss(mapObjectInfoEntity.rssType))
                        {
                            return TroopAttackType.Collect;
                        }
                    }

                    if (mapObjectInfoEntity.guildBuildStatus == (long) GuildBuildState.normal)
                    {
                        if (TroopHelp.IsCollectGuildType(mapObjectInfoEntity.rssType))
                        {
                            return TroopAttackType.Collect;
                        }

                        if (TroopHelp.IsAttackGuildType(mapObjectInfoEntity.rssType))
                        {
                           return  TroopAttackType.Attack;
                        }
                    }
                    return TroopAttackType.Attack;
                }

                if (mapObjectInfoEntity.rssType == RssType.City||mapObjectInfoEntity.collectRid != 0)
                {
                    return TroopAttackType.Attack;
                }

                return TroopAttackType.Collect;
            }

            return TroopAttackType.Space;
        }

        private Vector2 troopMoveSpacePos = Vector2.zero;

        public void SetTroopMoveSpacePos(Vector2 v2)
        {
            troopMoveSpacePos = v2;
        }

        public Vector2 GetTroopMoveSpacePos()
        {
            return troopMoveSpacePos;
        }


        public bool GetIsCityCreateTroop()
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return true;
            int num = GetTroopDispatchNum();
            return m_armyInfos.Count < num;
        }


        //获取部队可派遣队列上限
        public int GetTroopDispatchNum()
        {
            CityBuildingProxy m_cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            BuildingInfoEntity buildingInfo =
                m_cityBuildingProxy.GetBuildingInfoByType((int) EnumCityBuildingType.TownCenter);
            if (buildingInfo != null)
            {
                BuildingTownCenterDefine define =
                    CoreUtils.dataService.QueryRecord<BuildingTownCenterDefine>((int) buildingInfo.level);
                if (define != null)
                {
                    return define.troopsDispatchNumber;
                }
            }

            return 0;
        }

        //部队容量
        public int GetHeroTroopsCapacity(long heroId, long viceId)
        {
            var playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            if (playerAttributeProxy != null)
            {
                if (heroId == 0)
                {
                    return 0;
                }

                var troopCap = playerAttributeProxy.GetTroopAttribute(heroId, viceId, attrType.troopsCapacity).origvalue;
                var troopCapMutil = playerAttributeProxy.GetTroopAttribute(heroId, viceId, attrType.troopsCapacityMulti).value;
                var troopCapTotal =
                    Mathf.FloorToInt(troopCap * (1 + troopCapMutil));

                return troopCapTotal;
            }

            return 0;
        }

        //集结部队容量
        public int GetRallyTroopsCapacity(long heroId, long viceId)
        {
            int num = 0;
            var playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            if (playerAttributeProxy != null)
            {
                PlayerAttributeProxy.AttributeValue attributeValue =
                    playerAttributeProxy.GetTroopAttribute(heroId, viceId, attrType.massTroopsCapacity);
                float troopCapacity=0;
                if (attributeValue != null)
                {
                     troopCapacity = attributeValue.origvalue;
                }

                float troopCapacity1=0;
                PlayerAttributeProxy.AttributeValue attributeValue1 =
                    playerAttributeProxy.GetTroopAttribute(heroId, viceId, attrType.massTroopsCapacityMulti);
                if (attributeValue1 != null)
                {
                    troopCapacity1 = attributeValue1.origvalue;
                }

                num = Mathf.FloorToInt(troopCapacity * (1 + troopCapacity1 / 1000f));
            }
            return num;
        }

        //关卡或圣地容量 = 关卡或圣地驻守部队容量上限 * （1 + 集结部队容量百分比 / 1000）
        public int GetStrongHoldTroopsCapacity(long heroId, long viceId, int strongHoldId)
        {
            int strongHoldTroopsCapacity = 0;

            StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>(strongHoldId);
            if (strongHoldDataDefine != null)
            {
                StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);
                if (strongHoldTypeDefine != null)
                {
                    var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

                    float rallyTroopsCapacityMulti = 0;
                    
                    PlayerAttributeProxy.AttributeValue attributeValue = playerAttributeProxy.GetTroopAttribute(heroId, viceId, attrType.massTroopsCapacityMulti);
                    if (attributeValue != null)
                    {
                        rallyTroopsCapacityMulti = attributeValue.origvalue;
                    }

                    strongHoldTroopsCapacity = Mathf.FloorToInt(strongHoldTypeDefine.armyCntLimit * (1 + rallyTroopsCapacityMulti / 1000f));
                }
            }

            return strongHoldTroopsCapacity;
        }

        //联盟建筑容量 = 联盟建筑驻守部队容量上限 * （1 + 集结部队容量百分比 / 1000）
        public int GetGuildTroopsCapacity(long heroId, long viceId, RssType rssType)
        {
            int guildTroopsCapacity = 0;

            var m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            var GuildConfigType = m_allianceProxy.GetBuildServerTypeToConfigType((long)rssType);

            AllianceBuildingTypeDefine allianceBuildingTypeDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(GuildConfigType);
            if (allianceBuildingTypeDefine != null)
            {
                var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

                float rallyTroopsCapacityMulti = 0;

                PlayerAttributeProxy.AttributeValue attributeValue = playerAttributeProxy.GetTroopAttribute(heroId, viceId, attrType.massTroopsCapacityMulti);
                if (attributeValue != null)
                {
                    rallyTroopsCapacityMulti = attributeValue.origvalue;
                }

                guildTroopsCapacity = Mathf.FloorToInt(allianceBuildingTypeDefine.armyCntLimit * (1 + rallyTroopsCapacityMulti / 1000f));
            }

            return guildTroopsCapacity;
        }

        //更新出征英雄
        public void UpdateWarHero()
        {
            mWarHeroDic.Clear();
            foreach (var data in m_armyInfos)
            {
                if (data.Value.mainHeroId > 0)
                {
                    mWarHeroDic[(int)data.Value.mainHeroId] = true;
                    if (data.Value.deputyHeroId > 0)
                    {
                        mWarHeroDic[(int)data.Value.deputyHeroId] = true;
                    }
                }
            }
        }

        //英雄是否已出征
        public bool IsWarByHero(int heroId)
        {
            if (mWarHeroDic.ContainsKey(heroId))
            {
                return true;
            }

            return false;
        }

        #region 行军检测

        //是否在省内
        public bool IsProvince(Vector2 v2)
        {
            string targetName = MapManager.Instance().GetMapProvinceName(v2);
            CityObjData cityObjData = m_CityBuildingProxy.GetCityObjData(m_PlayerProxy.CurrentRoleInfo.rid);
            string cityName = string.Empty;
            if (cityObjData != null)
            {
                cityName = cityObjData.ProvinceName;
            }

            return String.Equals(targetName, cityName);
        }

        //是否战争狂热状态
        public bool IsWarFever()
        {          
            return !m_CityBuffProxy.HasType1Buff()&& m_PlayerProxy.CurrentRoleInfo.level>=cfgWarFeverLevel;
        }

        [IFix.Patch]
        private bool CheckWarFever(int id, TroopAttackType targetType, int targetId, TroopPosInfo posInfo, List<int> idList = null, bool isSituStation = false)
        {
            if (IsWarFever())
            {
                if (GetIsDayShowPanel())
                {
                    List<int> tempIdList = new List<int>();
                    if (idList != null)
                    {
                        foreach (var tempId in idList)
                        {
                            tempIdList.Add(tempId);
                        }
                    }                    
                    TroopHelp.ShowIsAttackOtherTroop(() =>
                    {
                        OnMapMarCh(id, targetType, targetId, posInfo, tempIdList, isSituStation);

                    }, (isOn) =>
                    {
                        if (isOn)
                        {
                            TroopHelp.SetShowAttackOtherTroopPlayerPrefs(saveKey,
                                (int)ServerTimeModule.Instance.GetServerTime());
                        }
                    }, TroopBehavior.Attack);
                    return true;
                }
            }

            return false;
        }

        public bool GetIsDayShowPanel()
        {
            return TroopHelp.IsShowAttackOtherTroopView(saveKey);
        }

        //当前
        public bool CheckAttackOtherCity(int id)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.objectType == (int) RssType.City)
                {
                    ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                    if (configDefine != null)
                    {
                        bool isShow= m_PlayerProxy.CurrentRoleInfo.level < configDefine.attackCityLevel;
                        if (isShow)
                        {                            
                            string des = LanguageUtils.getTextFormat(181186, configDefine.attackCityLevel);
                            Tip.CreateTip(des).Show();
                        }

                        return isShow;
                    }
                }
            }

            return false;
        }

        private bool CheckAttackCityBuff(int id)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.rssType == RssType.City)
                {
                    return m_CityBuffProxy.HasType2Buff(mapObjectInfoEntity.cityRid);
                }
            }

            return false;
        }

        public bool CheckAttackMyBuff()
        {
            bool isShow = m_CityBuffProxy.HasType2Buff(m_PlayerProxy.CurrentRoleInfo.rid);
            return isShow;
        }
        #endregion

        #region 协议

        /// <summary>
        /// 通知服务器创建部队
        /// </summary>
        /// <param name="主将ID"></param>
        /// <param name="副将ID，0或空为无副将"></param>
        /// <param name="士兵信息"></param>
        /// <param name="目的地类型(0 空地 1 攻击 2 增援 3 集结 4 采集)"></param>
        /// <param name="targetArg"></param>
        public void SendCreateTroopServer(int heroId, int viceId, List<TroopsCell> lsSoldiersData, int targetType,
            int targetId, Vector2Int posInfo, bool isSituStation=false)
        {
                   
            Role_CreateArmy.request req = new Role_CreateArmy.request();
            req.mainHeroId = heroId;
            req.deputyHeroId = viceId;
            req.soldiers = new Dictionary<long, SoldierInfo>();
            //创建新部队
            CoreUtils.audioService.PlayOneShot("Sound_Ui_CreateTroops");
            HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>(heroId);
            if (hero != null)
            {
                CoreUtils.audioService.PlayOneShot(hero.voiceMove);
            }

            foreach (var unit in lsSoldiersData)
            {
                if (unit.unitCount != 0)
                {
                    SoldierInfo soldierInfo = new SoldierInfo();
                    soldierInfo.id = unit.unitserverId;
                    soldierInfo.type = unit.unitType;
                    soldierInfo.level = unit.unitLevel;
                    soldierInfo.num = unit.unitCount;
                    req.soldiers.Add(soldierInfo.id, soldierInfo);
                }
            }

            req.targetType = targetType;
            req.targetArg = new MarchTargetArg();
            if (targetId != 0)
            {              
                req.targetArg.targetObjectIndex = targetId; 
            }

            req.targetArg.pos = new PosInfo();
            req.targetArg.pos.x = posInfo.x;
            req.targetArg.pos.y = posInfo.y;
            if ((TroopAttackType) targetType == TroopAttackType.Attack ||
                (TroopAttackType) targetType == TroopAttackType.Collect
            )
            {
                isSituStation = SituStation;            
                req.isSituStation = isSituStation;
            }

            AppFacade.GetInstance().SendSproto(req);
            SendEventTrancking(targetId);
        }

        public string saveKey = "SetShowAttackOtherTroopPlayerPrefs";

        public void InitSaveKey()
        {
            saveKey = string.Format("{0}{1}", saveKey,  m_PlayerProxy.CurrentRoleInfo.rid);
        }

        /// <summary>
        /// 自由行军
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetType"></param>
        /// <param name="targetId"></param>
        /// <param name="posInfo"></param>
        public void TroopMapMarCh(int id, TroopAttackType targetType, int targetId, TroopPosInfo posInfo, List<int> idList = null, bool isSituStation = false, bool isCheckWar = true)
        {
            //攻击行军判断
            if (targetType== TroopAttackType.Attack)
            {
                bool isCheck = isCheckWar;
                MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(targetId);
                if (infoEntity != null)
                {
                    if (infoEntity.rssType == RssType.Monster ||
                        infoEntity.rssType == RssType.Guardian ||
                        infoEntity.rssType == RssType.SummonAttackMonster ||
                        infoEntity.rssType == RssType.SummonConcentrateMonster)
                    {
                        isCheck = false;
                    }
                }

                if (isCheck)
                {
                    bool isState = CheckTroopAttack(targetId, () => {                        
                        OnMapMarChs(id, targetType, targetId, posInfo, idList, isSituStation);
                    });
                    if (!isState)
                    {
                        return;
                    }
                    if (CheckWarFever(id, targetType, targetId, posInfo, idList, isSituStation))
                    {                       
                        return;
                    }
                }
            }

            //侦查行军判断
            if (targetType== TroopAttackType.Scouts)
            {
                if (CheckWarFever(id, targetType, targetId, posInfo, idList, isSituStation))
                {
                    return;
                }
            }

            OnMapMarChs(id, targetType, targetId, posInfo, idList, isSituStation);
        }
        

        private void OnMapMarChs(int id, TroopAttackType targetType, int targetId, TroopPosInfo posInfo, List<int> idList = null, bool isSituStation = false)
        {      
            OnMapMarCh(id, targetType, targetId, posInfo, idList, isSituStation);
        }


        public bool CheckTroopAttack(int targetId ,Action callBack)
        {
            //攻击其他玩家城市判断
            if (CheckAttackOtherCity(targetId))
            {
                string des = LanguageUtils.getTextFormat(181186, configDefine.attackCityLevel);
                Tip.CreateTip(des).Show();
                return false;
            }

            if (CheckAttackCityBuff(targetId))
            {
                Tip.CreateTip(181187).Show();
                return false;
            }

            if (CheckAttackMyBuff())
            {
                if (!TroopHelp.IsShowCityBuffCheck(targetId))
                {
                    TroopHelp.ShowCheckCityBuffPanel(() =>
                    {
                        if (callBack != null)
                        {
                            callBack.Invoke();
                        }
                    });
                    return false;
                }
            }
            
            return true;
        }

        private void OnMapMarCh(int id, TroopAttackType targetType, int targetId, TroopPosInfo posInfo, List<int> idList = null, bool isSituStation = false)
        {
            //斥候回城
            if (targetType == TroopAttackType.ScoutsBack)
            {
                Map_ScoutsBack.request rep1 = new Map_ScoutsBack.request();
                rep1.objectIndex = id;
                AppFacade.GetInstance().SendSproto(rep1);
                return;
            }

            //运输车回城
            if (targetType == TroopAttackType.TransportBack)
            {
                Transport_TransportBack.request rep1 = new Transport_TransportBack.request();
                rep1.objectIndex = id;
                AppFacade.GetInstance().SendSproto(rep1);
                return;
            }

            bool isCollect = GetIsCollectRssItem(targetId);
            if (!isCollect)
            {
                return;
            }

            bool isMapMarCh = CheckTroopMapMarch(id, targetType, targetId);
            if (!isMapMarCh)
            {
                return;
            }

            Map_March.request rep = new Map_March.request();

            List<long> armyIndexList = new List<long>();
            if (id != 0)
            {
                armyIndexList.Add(id);
            }            
            if (idList != null)
            {
                foreach (var armyIndex in idList)
                {
                    if (!armyIndexList.Contains(armyIndex))
                    {
                        armyIndexList.Add(armyIndex);
                    }                    
                }
            }
            rep.armyIndexs = armyIndexList;

            rep.targetType = (int) targetType;

            rep.targetArg = new MarchTargetArg();
            if (targetId != 0)
            {
                rep.targetArg.targetObjectIndex = targetId;
            }

            if (posInfo != null)
            {
                rep.targetArg.pos = new PosInfo();
                rep.targetArg.pos.x = posInfo.x;
                rep.targetArg.pos.y = posInfo.y;
            }

            if ( targetType == TroopAttackType.Attack ||
                 targetType == TroopAttackType.Collect
            )
            {               
                rep.isSituStation = SituStation;
            }

            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append("大世界行军 - 类型 ：");
                sb.Append(targetType.ToString());
                sb.Append("\t部队索引列表 ：");
                foreach (var armyIndex in armyIndexList) 
                {
                    sb.Append(" ");
                    sb.Append(armyIndex);
                }
                Color color;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                CoreUtils.logService.Debug(sb.ToString(), color);
            }

            AppFacade.GetInstance().SendSproto(rep);

            SendEventTrancking(targetId);
        }

        private void SendEventTrancking(long targetId)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(targetId);
            if (mapObjectInfoEntity != null)
            {
                RssType resType = (RssType)mapObjectInfoEntity.objectType;
                switch (resType)
                {
                    case RssType.Monster:
                        {
                            if (mapObjectInfoEntity.monsterDefine != null)
                            {
                                if (mapObjectInfoEntity.monsterDefine.type == 1)
                                {
                                    if (mapObjectInfoEntity.monsterDefine.level == 1)
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianLevel1));
                                    }
                                    else if (mapObjectInfoEntity.monsterDefine.level == 2)
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianLevel2));
                                    }
                                    else if (mapObjectInfoEntity.monsterDefine.level == 3)
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianLevel3));
                                    }
                                    else if (mapObjectInfoEntity.monsterDefine.level == 4)
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianLevel4));
                                    }
                                    else if (mapObjectInfoEntity.monsterDefine.level == 5)
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianLevel5));
                                    }
                                    else if (mapObjectInfoEntity.monsterDefine.level == 6)
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianLevel6));
                                    }
                                }
                            }
                        }
                        break;
                    case RssType.City:
                    case RssType.Troop:
                    case RssType.GuildCenter:
                    case RssType.GuildFortress1:
                    case RssType.GuildFortress2:
                    case RssType.GuildFlag:
                    case RssType.CheckPoint:
                    case RssType.HolyLand:
                    case RssType.Sanctuary :
                    case RssType.Altar :
                    case RssType.Shrine:
                    case RssType.LostTemple:
                    case RssType.Checkpoint_1:
                    case RssType.Checkpoint_2:
                    case RssType.Checkpoint_3:
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.SoloBattle));
                        }
                        break;
                }
            }
        }


        public bool GetIsCollectRssItem(int targetId)
        {
            MapObjectInfoEntity rssData = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(targetId);
            if (rssData != null)
            {
                if (rssData.resourceGatherTypeDefine != null)
                {
                    if (rssData.resourceGatherTypeDefine.scienceReq > 0 &&
                        !m_PlayerProxy.IsTechnologyUnlockByType(rssData.resourceGatherTypeDefine.scienceReq))
                    {
                        StudyDefine define =
                            CoreUtils.dataService.QueryRecord<StudyDefine>((int) rssData.resourceGatherTypeDefine
                                .scienceLevReq);
                        if (define == null)
                        {
                            Debug.LogErrorFormat("StudyDefine not find {0}",
                                rssData.resourceGatherTypeDefine.scienceLevReq);
                            return false;
                        }

                        Tip.CreateTip(LanguageUtils.getTextFormat(500101,
                            LanguageUtils.getText(define.l_nameID),
                            LanguageUtils.getText(rssData.resourceGatherTypeDefine.l_nameId))).Show();
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckTroopMapMarch(int id, TroopAttackType targetType, int targetId)
        {
            ArmyInfoEntity armyInfo = null;
            if(m_armyInfos.TryGetValue(id, out armyInfo))
            {         
                if (TroopHelp.IsHaveState(armyInfo.status,ArmyStatus.FAILED_MARCH))
                {
                    Debug.LogWarning("溃败状态无法操作");
                    return false;
                }
                  
                var hero = m_HeroProxy.GetHeroByID(armyInfo.mainHeroId);
                //播放音效
                if (hero != null && hero.config != null)
                {
                    CoreUtils.audioService.PlayOneShot(hero.config.voiceMove);
                }
            }
            return true;
        }

        public List<HeroProxy.Hero> GetAvailableHeros()
        {
            List<HeroProxy.Hero> heros = m_HeroProxy.GetSummonerHeros();
            List<HeroProxy.Hero> available = new List<HeroProxy.Hero>();
            foreach(var hero in heros)
            {
                if (IsWarByHero(hero.config.ID)) continue;
                available.Add(hero);
            }
            return available;
        }

        public List<SoldiersData> GetAvailableSoldiers()
        {
            List<SoldiersData> soldiers = new List<SoldiersData>();
            foreach(var soldierInfo in m_PlayerProxy.CurrentRoleInfo.soldiers)
            {
                if (soldierInfo.Value.num <= 0) continue;
                soldiers.Add(new SoldiersData()
                {
                    Id = (int)soldierInfo.Value.id,
                    ArmysCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)soldierInfo.Value.id),
                    ServerInfo = soldierInfo.Value,
                    Number = (int)soldierInfo.Value.num,
                });
            }
            return soldiers;
        }

        #endregion
    }
    
}