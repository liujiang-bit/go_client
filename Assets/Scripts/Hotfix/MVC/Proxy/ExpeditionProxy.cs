// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    ExpeditionProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using SprotoType;
using Skyunion;
using Hotfix;
using System.Text;

namespace Game {
    
    public enum ExpeditionFightStatus
    {
        None,
        PrepareNormal,
        PreparePreview,
        WatingForStart,
        Fightting,
        WatingForFinish,
    }

    public enum ExpeditionLevelType
    {
        Normal = 1,
        Boss = 2,
        Rally = 3,
        Garrison = 4,
    }

    public enum ExpeditionFightResult
    {
        TimeoutFail =0,
        FightFail = 1,
        Win = 2,
    }

    public class ExpeditionMosnterTroopData
    {
        public int Index { get; set; }  //Starting from 1
        public MonsterDefine MonsterCfg { get; set; }
        public MonsterTroopsDefine TroopsCfg { get; set; }        
        public Dictionary<long, SoldierInfo> Soldiers { get; set; }
        public Vector2 BornPosisiton { get; set; }
        public Vector2 Forward { get; set; }
    }

    public class ExpeditionPlayerTroopData
    {
        public int Index { get; set; } //Starting from 1
        public int MainHeroId { get; set; }
        public int DeputyHeroId { get; set; }
        public Dictionary<long, SoldierInfo> Soldiers { get; set; }
        public Vector2 BornPosisiton { get; set; }
        public Vector2 Forward { get; set; }
    }


    public class ExpeditionProxy : GameProxy
    {
        


        #region Member
        public const string ProxyNAME = "ExpeditionProxy";

        public int LastSelectedLevelId { get; set; }
        public ExpeditionDefine ExpeditionCfg { get; private set; }
        public ExpeditionBattleDefine ExpeditionBattleCfg { get; private set; }
        public int MaxExpeditionLevel { get; set; }
        public ExpeditionFightStatus ExpeditionStatus { get; set; } = ExpeditionFightStatus.None;
        public long ExpeditionFightEndTime { get; private set; }
        public const string UIBgm = "Bgm_Expedition";
        public const string BattleBgm = "Bgm_Battle1";
        #endregion

        // Use this for initialization
        public ExpeditionProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" ExpeditionProxy register");   
        }


        public override void OnRemove()
        {
            Debug.Log(" ExpeditionProxy remove");
        }

        public Vector2 ExpeditionPosToWorldPos(float x, float y)
        {
            return new Vector2(x + m_mapMinPos.x, y + m_mapMinPos.y);
        }

        public Vector2 WorldPosToExpeditionPos(float x, float y)
        {
            return new Vector2(x - m_mapMinPos.x, y - m_mapMinPos.y);
        }

        public void SetMapRange(int xMin, int xMax, int yMin, int yMax)
        {
            m_mapMinPos = new Vector2(xMin, yMin);
            m_mapMaxPos = new Vector2(xMax, yMax);
        }        

        public void InitData(ExpeditionDefine cfg, ExpeditionBattleDefine battleCfg)
        {
            if (cfg == null || battleCfg == null) return;
            ExpeditionCfg = cfg;
            ExpeditionBattleCfg = battleCfg;
            InitMonsterTroopData();
        }

        public void ClearAllData()
        {
            ExpeditionCfg = null;
            ExpeditionBattleCfg = null;
            m_expeditionMonsterTroopData.Clear();
            m_expeditionPlayerTroopData.Clear();
            ExpeditionStatus = ExpeditionFightStatus.None;
        }

        private void InitMonsterTroopData()
        {
            m_expeditionMonsterTroopData.Clear();
            switch((ExpeditionLevelType)ExpeditionCfg.type)
            {
                case ExpeditionLevelType.Normal:
                    {
                        if (ExpeditionBattleCfg.monster1ID != 0)
                        {
                            m_expeditionMonsterTroopData.Add(1, CreateMosnterTroopData(1, ExpeditionBattleCfg.monster1ID, ExpeditionBattleCfg.monster1BornInfo[0],
                                ExpeditionBattleCfg.monster1BornInfo[1], ExpeditionBattleCfg.monster1BornInfo[2]));
                        }
                        if (ExpeditionBattleCfg.monster2ID != 0)
                        {
                            m_expeditionMonsterTroopData.Add(2, CreateMosnterTroopData(2, ExpeditionBattleCfg.monster2ID, ExpeditionBattleCfg.monster2BornInfo[0],
                                ExpeditionBattleCfg.monster2BornInfo[1], ExpeditionBattleCfg.monster2BornInfo[2]));
                        }
                        if (ExpeditionBattleCfg.monster3ID != 0)
                        {
                            m_expeditionMonsterTroopData.Add(3, CreateMosnterTroopData(3, ExpeditionBattleCfg.monster3ID, ExpeditionBattleCfg.monster3BornInfo[0],
                                ExpeditionBattleCfg.monster3BornInfo[1], ExpeditionBattleCfg.monster3BornInfo[2]));
                        }
                        if (ExpeditionBattleCfg.monster4ID != 0)
                        {
                            m_expeditionMonsterTroopData.Add(4, CreateMosnterTroopData(4, ExpeditionBattleCfg.monster4ID, ExpeditionBattleCfg.monster4BornInfo[0],
                                ExpeditionBattleCfg.monster4BornInfo[1], ExpeditionBattleCfg.monster4BornInfo[2]));
                        }
                        if (ExpeditionBattleCfg.monster5ID != 0)
                        {
                            m_expeditionMonsterTroopData.Add(5, CreateMosnterTroopData(5, ExpeditionBattleCfg.monster5ID, ExpeditionBattleCfg.monster5BornInfo[0],
                                ExpeditionBattleCfg.monster5BornInfo[1], ExpeditionBattleCfg.monster5BornInfo[2]));
                        }
                    }
                    break;
                case ExpeditionLevelType.Boss:
                    m_expeditionMonsterTroopData.Add(1, CreateMosnterTroopData(1, ExpeditionBattleCfg.bossID, ExpeditionBattleCfg.bossBornInfo[0],
                               ExpeditionBattleCfg.bossBornInfo[1], ExpeditionBattleCfg.bossBornInfo[2]));
                    break;
            }           
        }

        private ExpeditionMosnterTroopData CreateMosnterTroopData(int index, int monsterId, int x, int y, int z)
        {
            var monsterCfg = CoreUtils.dataService.QueryRecord<Data.MonsterDefine>(monsterId);
            if (monsterCfg == null) return null;
            var monsterTroopCfg = CoreUtils.dataService.QueryRecord<Data.MonsterTroopsDefine>(monsterCfg.monsterTroopsId);
            if (monsterTroopCfg == null) return null;
            ExpeditionMosnterTroopData data = new ExpeditionMosnterTroopData();
            data.MonsterCfg = monsterCfg;
            data.TroopsCfg = monsterTroopCfg;
            data.BornPosisiton = ExpeditionPosToWorldPos(x, y);
            data.Forward = GetForwardByNum(z);
            Dictionary<long, SoldierInfo> Soldiers = new Dictionary<long, SoldierInfo>();
            var allMonsterTroopsAttr = CoreUtils.dataService.QueryRecords<MonsterTroopsAttrDefine>();
            foreach(var attr in allMonsterTroopsAttr)
            {
                if (attr.group != monsterTroopCfg.troopsId) continue;
                var armyCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>(attr.armType);
                if (armyCfg == null) continue;
                SoldierInfo soldierInfo = new SoldierInfo()
                {
                    id = attr.armType,
                    type = armyCfg.armsType,
                    level = armyCfg.armsLv,
                    num = attr.armNum,
                };
                Soldiers.Add(attr.armType, soldierInfo);
            }
            data.Soldiers = Soldiers;
            data.Index = index;
            return data;
        }

        private Vector2 GetForwardByNum(int value)
        {
            return TroopHelp.Rotated(Vector2.right, value);
        }

        //Starting from 1
        public ExpeditionMosnterTroopData GetMonsterTroopData(int monsterIndex)
        {
            ExpeditionMosnterTroopData data = null;
            m_expeditionMonsterTroopData.TryGetValue(monsterIndex, out data);
            return data;
        }

        public List<ExpeditionMosnterTroopData> GetAllMonsterTroopData()
        {
            return new List<ExpeditionMosnterTroopData>(m_expeditionMonsterTroopData.Values);
        }

        public int GetMonsterTroopCount()
        {
            return m_expeditionMonsterTroopData.Count;
        }

        public void AddPlayerTroop(int index, int mainHeroId, int deputyHeroId, Dictionary<long, SoldierInfo> soldierInfo, bool isFromCache = false)
        {
            if (m_expeditionPlayerTroopData.ContainsKey(index))
            {
                m_expeditionPlayerTroopData.Remove(index);
            }

            int x =0, y = 0, z = 0;
            GetPlayerTroopInitParam(index, ref x, ref y, ref z);
            ExpeditionPlayerTroopData data = new ExpeditionPlayerTroopData()
            {
                Index = index,
                MainHeroId = mainHeroId,
                DeputyHeroId = deputyHeroId,
                Soldiers = soldierInfo,
                BornPosisiton = ExpeditionPosToWorldPos(x, y),
                Forward = GetForwardByNum(z),
            };
            m_expeditionPlayerTroopData.Add(index, data);
            if(!isFromCache)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.CreateExpeditionTroop, index);
            }
        }

        public void RemovePlayerTroop(int index)
        {
            if(m_expeditionPlayerTroopData.ContainsKey(index))
            {
                m_expeditionPlayerTroopData.Remove(index);
            }
        }

        public List<ExpeditionPlayerTroopData> GetAllPlayerTroopData()
        {
            return new List<ExpeditionPlayerTroopData>(m_expeditionPlayerTroopData.Values);
        }

        public ExpeditionPlayerTroopData GetPlayerTroopData(int index)
        {
            ExpeditionPlayerTroopData data = null;
            m_expeditionPlayerTroopData.TryGetValue(index, out data);
            return data;
        }

        public int GetPlayerTroopCount()
        {
            return m_expeditionPlayerTroopData.Count;
        }

        public void GetPlayerTroopInitParam(int index, ref int x, ref int y, ref int z)
        {
            List<int> bornInfo = null;
            switch (index)
            {
                case 1:
                    bornInfo = ExpeditionBattleCfg.playerBornInfo1;
                    break;
                case 2:
                    bornInfo = ExpeditionBattleCfg.playerBornInfo2;
                    break;
                case 3:
                    bornInfo = ExpeditionBattleCfg.playerBornInfo3;
                    break;
                case 4:
                    bornInfo = ExpeditionBattleCfg.playerBornInfo4;
                    break;
                case 5:
                    bornInfo = ExpeditionBattleCfg.playerBornInfo5;
                    break;
            }
            if (bornInfo == null || bornInfo.Count < 3) return;
            x = bornInfo[0];
            y = bornInfo[1];
            z = bornInfo[2];
        }

        public List<HeroProxy.Hero> GetAvailableHeros(int index)
        {
            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            List<HeroProxy.Hero> heros = heroProxy.GetSummonerHeros();
            List<HeroProxy.Hero> available = new List<HeroProxy.Hero>();
            foreach (var hero in heros)
            {
                if (IsHeroInScene(hero.config.ID, index)) continue;
                available.Add(hero);
            }
            return available;
        }

        public List<SoldiersData> GetAvailableSoldiers(int index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy == null) return null;
            List<SoldiersData> soldiers = new List<SoldiersData>();
            foreach (var soldierInfo in playerProxy.CurrentRoleInfo.historySoldiers)
            {
                int num = (int)soldierInfo.Value.num - GetSoldierNumInScene((int)soldierInfo.Value.id, index);
                if (num <= 0) continue;
                soldiers.Add(new SoldiersData()
                {
                    Id = (int)soldierInfo.Value.id,
                    ArmysCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)soldierInfo.Value.id),
                    ServerInfo = soldierInfo.Value,
                    Number = num,
                });
            }
            return soldiers;
        }

        public Dictionary<int, int> GetReadySoldiers()
        {
            Dictionary<int, int> readySoldiers = new Dictionary<int, int>();
            foreach (var playerTroopData in m_expeditionPlayerTroopData)
            {
                foreach (var kv in playerTroopData.Value.Soldiers)
                {
                    int soldierId = (int)kv.Key;
                    if(readySoldiers.ContainsKey(soldierId))
                    {
                        readySoldiers[soldierId] += (int)kv.Value.num;
                    }
                    else
                    {
                        readySoldiers[soldierId] = (int)kv.Value.num;
                    }
                }
            }
            return readySoldiers;
        }

        private int GetSoldierNumInScene(int soldierId, int troopIndex)
        {
            int count = 0;
            foreach (var playerTroopData in m_expeditionPlayerTroopData)
            {
                if (playerTroopData.Value.Index != troopIndex && playerTroopData.Value.Soldiers.ContainsKey(soldierId))
                {
                    count += (int)playerTroopData.Value.Soldiers[soldierId].num;
                }
            }
            return count;
        }

        private bool IsHeroInScene(int heroId, int troopIndex)
        {
            foreach(var playerTroopData in m_expeditionPlayerTroopData)
            {
                if(playerTroopData.Value.Index != troopIndex && (playerTroopData.Value.MainHeroId == heroId ||
                    playerTroopData.Value.DeputyHeroId == heroId))
                {
                    return true;
                }
            }
            return false;
        }

        public void TroopMarchToSpace(int objectId, Vector3 worldPos, List<int> objectIdList = null)
        {
            Vector2 expeditionPos = WorldPosToExpeditionPos(worldPos.x, worldPos.z);

            List<long> objIdList = new List<long>();
            if (objectId != 0)
            {
                objIdList.Add(objectId);
            }            
            if (objectIdList != null)
            {
                foreach (var objId in objectIdList)
                {
                    if (!objIdList.Contains(objId))
                    {
                        objIdList.Add(objId);
                    }                    
                }
            }

            Expedition_March.request request = new Expedition_March.request()
            {
                targetType = 0,
                targetArg = new MarchTargetArg()
                {
                    pos = new PosInfo()
                    {
                        x = (int)(expeditionPos.x * 100),
                        y = (int)(expeditionPos.y * 100),
                    }
                },
                objectIndexs = objIdList
            };

            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append("远征行军 - 类型 ：");
                sb.Append("空地");
                sb.Append("\t部队id列表 ：");
                foreach (var id in objIdList)
                {
                    sb.Append(" ");
                    sb.Append(id);
                }
                Color color;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                CoreUtils.logService.Debug(sb.ToString(), color);
            }

            AppFacade.GetInstance().SendSproto(request);

            WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(objectId);
        }

        public void TroopMarchToEnemy(int objectId, int enemyObjectId, List<int> objectIdList = null)
        {
            List<long> objIdList = new List<long>();
            if (objectId != 0)
            {
                objIdList.Add(objectId);
            }            
            if (objectIdList != null)
            {
                foreach (var objId in objectIdList)
                {
                    if (!objIdList.Contains(objId))
                    {
                        objIdList.Add(objId);
                    }                    
                }
            }

            Expedition_March.request request = new Expedition_March.request()
            {
                targetType = 1,
                targetArg = new MarchTargetArg()
                {
                    targetObjectIndex = enemyObjectId,
                },
                objectIndexs = objIdList
            };

            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append("远征行军 - 类型 ：");
                sb.Append("攻击");
                sb.Append("\t部队id列表 ：");
                foreach (var id in objIdList)
                {
                    sb.Append(" ");
                    sb.Append(id);
                }
                Color color;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                CoreUtils.logService.Debug(sb.ToString(), color);
            }

            AppFacade.GetInstance().SendSproto(request);

            WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(objectId);
        }

        public void TroopStation(int objectId, List<int> objectIdList = null)
        {
            List<long> objIdList = new List<long>();
            if (objectId != 0)
            {
                objIdList.Add(objectId);
            }
            if (objectIdList != null)
            {
                foreach (var objId in objectIdList)
                {
                    if (!objIdList.Contains(objId))
                    {
                        objIdList.Add(objId);
                    }
                }
            }

            Expedition_March.request request = new Expedition_March.request()
            {
                objectIndex = objectId,
                targetType = 7,
                objectIndexs = objIdList,
            };

            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append("远征行军 - 类型 ：");
                sb.Append("驻扎");
                sb.Append("\t部队id列表 ：");
                foreach (var id in objIdList)
                {
                    sb.Append(" ");
                    sb.Append(id);
                }
                Color color;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                CoreUtils.logService.Debug(sb.ToString(), color);
            }

            AppFacade.GetInstance().SendSproto(request);
        }

        public void SetFightEndTime(long endTime)
        {
            ExpeditionFightEndTime = endTime;
        }

        public void CachePlayerTroopData(List<ExpeditionPlayerTroopData> playTroopDatas)
        {
            ClearPlayerTroopDataCache();
            m_playerTroopDataCache.AddRange(playTroopDatas);
        }

        public List<ExpeditionPlayerTroopData> GetPlayeyTroopDataCache()
        {
            return new List<ExpeditionPlayerTroopData>(m_playerTroopDataCache);
        }

        public void ClearPlayerTroopDataCache()
        {
            m_playerTroopDataCache.Clear();
        }

        private Vector2 m_mapMinPos = Vector2.zero;
        private Vector2 m_mapMaxPos = Vector2.zero;

        private Dictionary<int, ExpeditionMosnterTroopData> m_expeditionMonsterTroopData = new Dictionary<int, ExpeditionMosnterTroopData>();
        private Dictionary<int, ExpeditionPlayerTroopData> m_expeditionPlayerTroopData = new Dictionary<int, ExpeditionPlayerTroopData>();
        private List<ExpeditionPlayerTroopData> m_playerTroopDataCache = new List<ExpeditionPlayerTroopData>();
    }
}