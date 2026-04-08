// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    ScoutProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using Client;
using Data;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class ScoutProxy : GameProxy
    {
        public enum ScoutState
        {
            Fog = 1 << 12,         //探索迷雾
            Return = 1 << 13,      //探索完成返回
            None = 1 << 14,        //空闲
            Back_City = 1 << 15,   //中途返回
            Scouting = 1 << 25,    //侦察
            Surveing = 1 << 26,    // 调查中
            Scouting_Delete = 1 << 28, //斥候侦查中删除地图斥候
        }

        public enum ScoutType
        {
            OtherPlayerUnit = 1,    //玩家城市、玩家部队、集结部队、玩家采集点
            OtherBuiding = 2,        //联盟建筑、关卡、圣物
            Other = 3,              //迷雾、山洞
        }

        public class ScoutCost
        {
            public int currencyId;
            public int number;
        }

        public class ScoutInfo
        {
            public int id;
            public ScoutState state;
            public int x;   //目的地坐标x
            public int y;   //目的地坐标y
            public Int64 endTime;
            public Int64 startTime;
            public List<pos> posInfos = new List<pos>();
            public int ObjectId;

            public class pos
            {
                public float x;
                public float y;
            }
        }

        #region Member

        public const string ProxyNAME = "ScoutProxy";
        private List<ScoutInfo> mlistSouts = new List<ScoutInfo>();
        private Dictionary<Int64, int> mDicSouts = new Dictionary<long, int>();

        private bool m_isFirstGetInfo = true;

        private PlayerAttributeProxy m_playerAttributeProxy;

        private CityBuildingProxy m_CityBuildingProxy;

        private WorldMapObjectProxy m_worldMapObjectProxy;

        private TroopProxy m_TroopProxy;

        private long m_currSelectScoutIndex;

        #endregion

        // Use this for initialization
        public ScoutProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {

        }

        public override void OnRemove()
        {
            Clear();
        }

        public void Clear()
        {
            mlistSouts.Clear();
            mDicSouts.Clear();
        }

        public void Init()
        {
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_CityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
        }

        public void SetScoutData(INotification notification)
        {
            Role_ScoutsInfo.request scoutsInfo = notification.Body as Role_ScoutsInfo.request;
            if (scoutsInfo != null)
            {
                if (scoutsInfo.HasScoutsQueue)
                {
                    bool isNew = false;
                    foreach (var info in scoutsInfo.scoutsQueue.Values)
                    {
                        if (!info.HasScoutsIndex) continue;
                        ScoutInfo sout = null;
                        if (mDicSouts.ContainsKey(info.scoutsIndex))
                        {
                            sout = mlistSouts[mDicSouts[info.scoutsIndex]];
                        }
                        else
                        {
                            isNew = true;
                            sout = new ScoutInfo();
                            sout.id = (int)info.scoutsIndex;
                        }
                        if (info.HasArrivalTime)
                        {
                            sout.endTime = info.arrivalTime;
                        }
                        if (info.HasStartTime)
                        {
                            sout.startTime = info.startTime;
                        }
                        if (info.HasScoutsStatus)
                        {
                            sout.state = (ScoutState)info.scoutsStatus;
                        }
                        if (info.HasObjectIndex)
                        {
                            sout.ObjectId = (int)info.objectIndex;
                        }
                        //Debug.LogErrorFormat("侦查队列：{0} state:{1} 时间:{2}", info.scoutsIndex, sout.state, info.arrivalTime);
                        sout.posInfos.Clear();
                        if (info.HasScoutsPath)
                        {
                            foreach (var pos in info.scoutsPath)
                            {
                                ScoutInfo.pos posInfo = new ScoutInfo.pos();
                                posInfo.x = pos.x;
                                posInfo.y = pos.y;
                                sout.posInfos.Add(posInfo);
                            }
                            if (sout.posInfos.Count > 1) //路径最后一个点为目的地坐标
                            {
                                sout.x = Mathf.FloorToInt(sout.posInfos[sout.posInfos.Count - 1].x / 100);
                                sout.y = Mathf.FloorToInt(sout.posInfos[sout.posInfos.Count - 1].y / 100);
                            }
                            //Debug.LogErrorFormat("x:{0} y:{1} x2:{2}  y2:{3}", sout.posInfos[0].x, sout.posInfos[0].y, sout.posInfos[1].x, sout.posInfos[1].y);
                        }
                        if (isNew)
                        {
                            mlistSouts.Add(sout);
                            mDicSouts[info.scoutsIndex] = mlistSouts.Count - 1;
                        }
                        if (!m_isFirstGetInfo)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.ScoutQueueUpdate, info.scoutsIndex);
                        }
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnTroopDataChanged);
                }
            }
            m_isFirstGetInfo = false;
        }


        public void SearPos(int id, int x, int y, long targetIndex = 0)
        {
            Map_Scouts.request req = new Map_Scouts.request();
            req.scoutIndex = id;
            req.pos = new PosInfo();
            req.pos.x = x * 100;
            req.pos.y = y * 100;
            if (targetIndex != 0)
            {
                req.targetIndex = targetIndex;
            }
            AppFacade.GetInstance().SendSproto(req);
        }

        public int GetScoutNum()
        {
            return mlistSouts.Count;
        }

        public int GetCanDispatchCount()
        {
            int count = 0;
            foreach (var info in mlistSouts)
            {
                if (info.state == ScoutState.None || info.state == ScoutState.Return || info.state == ScoutState.Back_City)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetActiveScountCount()
        {
            int count = 0;
            foreach (var info in mlistSouts)
            {
                if (info.state != ScoutState.None)
                {
                    count++;
                }
            }
            return count;
        }

        public List<ScoutInfo> GetAllActiveScounts()
        {
            List<ScoutInfo> activeScounts = new List<ScoutInfo>();
            foreach (var scountInfo in mlistSouts)
            {
                if (scountInfo.state != ScoutState.None)
                {
                    activeScounts.Add(scountInfo);
                }
            }
            return activeScounts;
        }

        public int GetSoutView()
        {
            var cityProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            var level = (int)cityProxy.GetMaxLevelofType((long)EnumCityBuildingType.ScoutCamp);
            if (level == 0)
                return 5;
            var scontBuildInfo = CoreUtils.dataService.QueryRecord<BuildingScoutcampDefine>(level);
            return scontBuildInfo.scoutView;
        }

        public ScoutInfo GetScoutInfo(int index)
        {
            if (index >= mlistSouts.Count)
            {
                return null;
            }

            return mlistSouts[index];
        }

        public ScoutInfo GetSoutInfoById(Int64 id)
        {
            return mlistSouts[mDicSouts[id]];
        }

        public int isHaveExploreScout()
        {
            int id = 0;
            mlistSouts.ForEach((info) => {
                if (info.state == ScoutState.Fog)
                {
                    id = info.id;
                    return;
                }
            });
            return id;
        }

        public string GetNameById(int id)
        {
            string name = string.Empty;
            if (id == 1)
            {
                name = LanguageUtils.getText(181156);
            }
            else if (id == 2)
            {
                name = LanguageUtils.getText(181157);
            }
            else if (id == 3)
            {
                name = LanguageUtils.getText(181158);
            }
            return name;
        }

        //获取侦查速度
        public int GetScoutSpeed()
        {
            int speed = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).scoutSpeed;
            return Mathf.FloorToInt(speed * (1 + m_playerAttributeProxy.GetCityAttribute(attrType.scoutSpeedMulti).value));
        }

        public bool isScoutBuildingExit()
        {
            BuildingInfoEntity buildingInfoEntity = m_CityBuildingProxy.GetBuildingInfoByType(28);
            return buildingInfoEntity != null;
        }

        private ScoutType GetScoutType(RssType rssType)
        {
            switch (rssType)
            {
                case RssType.City:
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.Troop:
                case RssType.Scouts:
                    return ScoutType.OtherPlayerUnit;
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                case RssType.CheckPoint:
                case RssType.HolyLand:
                    return ScoutType.OtherBuiding;
                default:
                    break;
            }

            return ScoutType.Other;
        }

        public ScoutCost GetScoutCostDefine(int id)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity == null)
            {
                return null;
            }

            int cityLevel = 0;
            int index = 0;
            ScoutType scoutType = GetScoutType(mapObjectInfoEntity.rssType);
            switch (scoutType)
            {
                case ScoutType.OtherPlayerUnit:
                    cityLevel = (int)mapObjectInfoEntity.cityLevel;
                    break;
                case ScoutType.OtherBuiding:
                    var playProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                    cityLevel = (int)playProxy.GetTownHall();
                    index = 1;
                    break;
                default:
                    break;
            }

            if (cityLevel == 0)
                return null;
            var defines = CoreUtils.dataService.QueryRecords<BuildingScoutcampDefine>();
            foreach (var define in defines)
            {
                if (define.level == cityLevel)
                {
                    var scoutCost = new ScoutCost();
                    if (index == 0)
                    {
                        scoutCost.currencyId = define.costCurrencyType1;
                        scoutCost.number = define.number1;
                    }
                    else
                    {
                        scoutCost.currencyId = define.costCurrencyType2;
                        scoutCost.number = define.number2;
                    }

                    return scoutCost;
                }
            }

            return null;
        }

        //能否侦察
        public void CheckScoutCondition(int id, Action callBack)
        {
            if (!isScoutBuildingExit())
            {
                Tip.CreateTip(LanguageUtils.getText(610024)).Show();
                return;
            }

            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity == null)
            {
                Tip.CreateTip(LanguageUtils.getText(610023)).Show();
                return;
            }

            var cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            BuildingInfoEntity buildingInfo = m_CityBuildingProxy.GetBuildingInfoByType(1);

            switch (mapObjectInfoEntity.rssType)
            {
                case RssType.City:
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.Troop:
                case RssType.Scouts:

                    //城市才有保护器
                    if (mapObjectInfoEntity.rssType == RssType.City)
                    {
                        //敌方城市处于保护盾内
                        if (cityBuffProxy.HasType2Buff(mapObjectInfoEntity.cityRid))
                        {
                            Tip.CreateTip(LanguageUtils.getText(181187)).Show();
                            return;
                        }
                    }

                    //敌方市政厅大于我方特定等级
                    if ((mapObjectInfoEntity.cityLevel - buildingInfo.level) > config.detectedLv)
                    {
                        Tip.CreateTip(LanguageUtils.getText(181161)).Show();
                        return;
                    }
                    break;
                default:
                    //圣物、关卡才有保护器
                    if (mapObjectInfoEntity.rssType == RssType.HolyLand || mapObjectInfoEntity.rssType == RssType.CheckPoint)
                    {
                        UI_Item_IconAndTime_SubView.BuildingState state = (UI_Item_IconAndTime_SubView.BuildingState)mapObjectInfoEntity.holyLandStatus;
                        if (state == UI_Item_IconAndTime_SubView.BuildingState.NotUnlock || state == UI_Item_IconAndTime_SubView.BuildingState.InitProtecting
                                                                        || state == UI_Item_IconAndTime_SubView.BuildingState.Protecting)
                        {
                            Tip.CreateTip(LanguageUtils.getText(730264)).Show();
                            return;
                        }
                    }
                    break;
            }

            //我方市政厅是否大于一定等级
            if (buildingInfo.level >= config.activationWarFare)
            {
                //战争狂热
                if (cityBuffProxy.HasType1Buff())
                {
                    callBack.Invoke();
                }
                else
                {
                    if (m_TroopProxy.GetIsDayShowPanel())
                    {
                        TroopHelp.ShowIsAttackOtherTroop(() =>
                        {
                            callBack.Invoke();
                        }, (isOn) =>
                        {
                            if (isOn)
                            {
                                TroopHelp.SetShowAttackOtherTroopPlayerPrefs(m_TroopProxy.saveKey,
                                    (int)ServerTimeModule.Instance.GetServerTime());
                            }
                        }, TroopBehavior.SpyOn);
                    }
                    else
                    {
                        if (callBack != null)
                        {
                            callBack.Invoke();
                        }
                    }
                }
            }
            else
            {
                callBack.Invoke();
            }

        }

        public static string GetScoutIcon(int id)
        {
            var configCfg = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (configCfg == null) return string.Empty;
            string icon = string.Empty;
            switch (id)
            {
                case 1:
                    icon = configCfg.toScoutsIcon1;
                    break;
                case 2:
                    icon = configCfg.toScoutsIcon2;
                    break;
                case 3:
                    icon = configCfg.toScoutsIcon3;
                    break;
            }
            return icon;
        }

        public static string GetScoutStateIcon(ScoutState state)
        {
            string icon = string.Empty;
            switch (state)
            {
                case ScoutState.None:
                    icon = Hotfix.TroopHelp.GetIcon((long)ArmyStatus.STANBY);
                    break;
                case ScoutState.Fog:
                    icon = Hotfix.TroopHelp.GetIcon((long)ArmyStatus.DISCOVER);
                    break;
                case ScoutState.Surveing:
                case ScoutState.Scouting:
                    icon = Hotfix.TroopHelp.GetIcon((long)ArmyStatus.SCOUNT);
                    break;
                case ScoutState.Back_City:
                case ScoutState.Return:
                    icon = Hotfix.TroopHelp.GetIcon((long)ArmyStatus.RETURN);
                    break;
            }

            return icon;
        }

        public void SetCurrSelectIndex(long scountIndex)
        {
            m_currSelectScoutIndex = scountIndex;
        }

        public long GetCurrSelectIndex()
        {
            return m_currSelectScoutIndex;
        }
    }
}