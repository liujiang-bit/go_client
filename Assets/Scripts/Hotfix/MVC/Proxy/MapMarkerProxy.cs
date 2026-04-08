// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月15日
// Update Time         :    2020年9月15日
// Class Description   :    MapMarkerProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sproto;
using Skyunion;
using Data;
using SprotoType;
using Client;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace Game {
    public class MapMarkerProxy : GameProxy {

        #region Member

        public const string ProxyNAME = "MapMarkerProxy";

        private ConfigDefine m_configDefine;

        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;

        private Dictionary<long, MapMarkerInfo> PersonMapMarkerInfoDic = new Dictionary<long, MapMarkerInfo>();
        private Dictionary<long, MapMarkerInfo> GuildMapMarkerInfoDic = new Dictionary<long, MapMarkerInfo>();

        #endregion

        // Use this for initialization
        public MapMarkerProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            
        }

        public override void OnRemove()
        {
            PersonMapMarkerInfoDic.Clear();
            GuildMapMarkerInfoDic.Clear();
        }

        public void Init()
        {
            m_configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            //---------------------------模拟数据---------------------------
            //MapMarkerInfo p_MapMarkerInfo_1 = new MapMarkerInfo();
            //p_MapMarkerInfo_1.markerIndex = 1;
            //p_MapMarkerInfo_1.markerId = 1000;
            //p_MapMarkerInfo_1.description = "特殊";
            //p_MapMarkerInfo_1.gameNode = "5";
            //p_MapMarkerInfo_1.pos = new PosInfo();
            //p_MapMarkerInfo_1.pos.x = 492200;
            //p_MapMarkerInfo_1.pos.y = 23700;
            //p_MapMarkerInfo_1.markerTime = ServerTimeModule.Instance.GetServerTime();
            //p_MapMarkerInfo_1.status = 0;
            //PersonMapMarkerInfoDic.Add(1, p_MapMarkerInfo_1);

            //MapMarkerInfo p_MapMarkerInfo_2 = new MapMarkerInfo();
            //p_MapMarkerInfo_2.markerIndex = 2;
            //p_MapMarkerInfo_2.markerId = 1001;
            //p_MapMarkerInfo_2.description = "朋友";
            //p_MapMarkerInfo_2.gameNode = "5";
            //p_MapMarkerInfo_2.pos = new PosInfo();
            //p_MapMarkerInfo_2.pos.x = 494500;
            //p_MapMarkerInfo_2.pos.y = 23700;
            //p_MapMarkerInfo_2.markerTime = ServerTimeModule.Instance.GetServerTime();
            //p_MapMarkerInfo_2.status = 0;
            //PersonMapMarkerInfoDic.Add(2, p_MapMarkerInfo_2);

            //MapMarkerInfo p_MapMarkerInfo_3 = new MapMarkerInfo();
            //p_MapMarkerInfo_3.markerIndex = 3;
            //p_MapMarkerInfo_3.markerId = 1002;
            //p_MapMarkerInfo_3.description = "敌方";
            //p_MapMarkerInfo_3.gameNode = "5";
            //p_MapMarkerInfo_3.pos = new PosInfo();
            //p_MapMarkerInfo_3.pos.x = 493200;
            //p_MapMarkerInfo_3.pos.y = 25700;
            //p_MapMarkerInfo_3.markerTime = ServerTimeModule.Instance.GetServerTime();
            //p_MapMarkerInfo_3.status = -1;
            //PersonMapMarkerInfoDic.Add(3, p_MapMarkerInfo_3);

            //MapMarkerInfo g_MapMarkerInfo_1 = new MapMarkerInfo();
            //g_MapMarkerInfo_1.markerId = 2000;
            //g_MapMarkerInfo_1.description = "进攻";
            //g_MapMarkerInfo_1.gameNode = "5";
            //g_MapMarkerInfo_1.pos = new PosInfo();
            //g_MapMarkerInfo_1.pos.x = 493200;
            //g_MapMarkerInfo_1.pos.y = 28700;
            //g_MapMarkerInfo_1.markerTime = ServerTimeModule.Instance.GetServerTime();
            //g_MapMarkerInfo_1.status = 0;
            //GuildMapMarkerInfoDic.Add(2000, g_MapMarkerInfo_1);
            //-------------------------------------------------------------
        }

        public void UpdatePersonMapMarkerInfo(Dictionary<long, MapMarkerInfo> mapMarkerInfoDic)
        {
            foreach (var mapMarkerInfo in mapMarkerInfoDic.Values)
            {
                //删除书签
                if (mapMarkerInfo.status == -1)
                {
                    if (PersonMapMarkerInfoDic.ContainsKey(mapMarkerInfo.markerIndex))
                    {
                        PersonMapMarkerInfoDic.Remove(mapMarkerInfo.markerIndex);
                    }
                }
                else
                {
                    //添加书签
                    if (!PersonMapMarkerInfoDic.ContainsKey(mapMarkerInfo.markerIndex))
                    {
                        if (!mapMarkerInfo.HasMarkerIndex)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without MarkerIndex");
                            continue;
                        }
                        if (!mapMarkerInfo.HasMarkerId)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without MarkerId");
                            continue;
                        }
                        if (!mapMarkerInfo.HasDescription)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without Description");
                            continue;
                        }
                        if (!mapMarkerInfo.HasGameNode)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without GameNode");
                            continue;
                        }
                        if (!mapMarkerInfo.HasPos)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without Pos");
                            continue;
                        }
                        if (!mapMarkerInfo.HasMarkerTime)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without MarkerTime");
                            continue;
                        }
                        if (!mapMarkerInfo.HasStatus)
                        {
                            Debug.LogError("PersonMapMarkerInfo Without Status");
                            continue;
                        }
                        PersonMapMarkerInfoDic.Add(mapMarkerInfo.markerIndex, mapMarkerInfo);
                    }
                    //编辑书签
                    else
                    {
                        if (mapMarkerInfo.HasMarkerId) PersonMapMarkerInfoDic[mapMarkerInfo.markerIndex].markerId = mapMarkerInfo.markerId;
                        if (mapMarkerInfo.HasDescription) PersonMapMarkerInfoDic[mapMarkerInfo.markerIndex].description = mapMarkerInfo.description;
                        if (mapMarkerInfo.HasGameNode) PersonMapMarkerInfoDic[mapMarkerInfo.markerIndex].gameNode = mapMarkerInfo.gameNode;
                        if (mapMarkerInfo.HasPos) PersonMapMarkerInfoDic[mapMarkerInfo.markerIndex].pos = mapMarkerInfo.pos;
                        if (mapMarkerInfo.HasMarkerTime) PersonMapMarkerInfoDic[mapMarkerInfo.markerIndex].markerTime = mapMarkerInfo.markerTime;
                        if (mapMarkerInfo.HasStatus) PersonMapMarkerInfoDic[mapMarkerInfo.markerIndex].status = mapMarkerInfo.status;
                    }
                }
            }
        }

        public void UpdateGuildMapMarkerInfo(Dictionary<long, MapMarkerInfo> mapMarkerInfoDic)
        {
            List<long> DeleteMarkerIdList = new List<long>();
            List<long> AddMarkerIdList = new List<long>();
            List<long> EditMarkerIdList = new List<long>();

            foreach (var mapMarkerInfo in mapMarkerInfoDic.Values)
            {
                //删除书签
                if (mapMarkerInfo.status == -1)
                {
                    if (GuildMapMarkerInfoDic.ContainsKey(mapMarkerInfo.markerId))
                    {
                        GuildMapMarkerInfoDic.Remove(mapMarkerInfo.markerId);
                        DeleteMarkerIdList.Add(mapMarkerInfo.markerId);
                    }                    
                }
                else
                {
                    //添加书签
                    if (!GuildMapMarkerInfoDic.ContainsKey(mapMarkerInfo.markerId))
                    {
                        if (!mapMarkerInfo.HasMarkerId)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without MarkerId");
                            continue;
                        }
                        if (!mapMarkerInfo.HasDescription)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without Description");
                            continue;
                        }
                        if (!mapMarkerInfo.HasGameNode)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without GameNode");
                            continue;
                        }
                        if (!mapMarkerInfo.HasPos)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without Pos");
                            continue;
                        }
                        if (!mapMarkerInfo.HasMarkerTime)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without MarkerTime");
                            continue;
                        }
                        if (!mapMarkerInfo.HasStatus)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without Status");
                            continue;
                        }
                        if (!mapMarkerInfo.HasCreateName)
                        {
                            Debug.LogError("GuildMapMarkerInfo Without CreateName");
                            continue;
                        }
                        GuildMapMarkerInfoDic.Add(mapMarkerInfo.markerId, mapMarkerInfo);
                        AddMarkerIdList.Add(mapMarkerInfo.markerId);
                    }
                    //编辑书签
                    else
                    {
                        if (mapMarkerInfo.HasDescription) GuildMapMarkerInfoDic[mapMarkerInfo.markerId].description = mapMarkerInfo.description;
                        if (mapMarkerInfo.HasGameNode) GuildMapMarkerInfoDic[mapMarkerInfo.markerId].gameNode = mapMarkerInfo.gameNode;
                        if (mapMarkerInfo.HasPos) GuildMapMarkerInfoDic[mapMarkerInfo.markerId].pos = mapMarkerInfo.pos;
                        if (mapMarkerInfo.HasMarkerTime) GuildMapMarkerInfoDic[mapMarkerInfo.markerId].markerTime = mapMarkerInfo.markerTime;
                        if (mapMarkerInfo.HasStatus) GuildMapMarkerInfoDic[mapMarkerInfo.markerId].status = mapMarkerInfo.status;
                        if (mapMarkerInfo.HasCreateName) GuildMapMarkerInfoDic[mapMarkerInfo.markerId].createName = mapMarkerInfo.createName;
                        EditMarkerIdList.Add(mapMarkerInfo.markerId);
                    }
                }
            }

            if (DeleteMarkerIdList.Count > 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.GuildMapMarkerInfoDelete, DeleteMarkerIdList);
            }

            if (AddMarkerIdList.Count > 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.GuildMapMarkerInfoAdd, AddMarkerIdList);
            }

            if (EditMarkerIdList.Count > 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.GuildMapMarkerInfoEdit, EditMarkerIdList);
            }
        }

        public Dictionary<long, MapMarkerInfo> GetPersonMapMarkerInfoDic()
        {
            return PersonMapMarkerInfoDic;
        }

        public Dictionary<long, MapMarkerInfo> GetGuildMapMarkerInfoDic()
        {
            return GuildMapMarkerInfoDic;
        }

        public void ClearGuildMapMarkerInfoDic()
        {
            List<long> DeleteMarkerIdList = new List<long>();

            foreach (var guildMapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                DeleteMarkerIdList.Add(guildMapMarkerInfo.markerId);
            }

            GuildMapMarkerInfoDic.Clear();

            if (DeleteMarkerIdList.Count > 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.GuildMapMarkerInfoDelete, DeleteMarkerIdList);
            }
        }

        private List<MapMarkerInfo> GetMapMarkerInfoListById(int id)
        {
            List<MapMarkerInfo> MapMarkerInfoList = new List<MapMarkerInfo>();

            foreach (var mapMarkerInfo in PersonMapMarkerInfoDic.Values)
            {
                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)mapMarkerInfo.markerId);
                if (mapMarkerTypeDefine != null)
                {
                    if (mapMarkerTypeDefine.ID == id)
                    {
                        MapMarkerInfoList.Add(mapMarkerInfo);
                    }
                }
            }

            foreach (var mapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)mapMarkerInfo.markerId);
                if (mapMarkerTypeDefine != null)
                {
                    if (mapMarkerTypeDefine.ID == id)
                    {
                        MapMarkerInfoList.Add(mapMarkerInfo);
                    }
                }
            }

            MapMarkerInfoList.Sort((x, y) =>
            {
                return y.markerTime.CompareTo(x.markerTime);
            });

            return MapMarkerInfoList;
        }

        private List<MapMarkerInfo> GetMapMarkerInfoListByType(int type)
        {
            List<MapMarkerInfo> MapMarkerInfoList = new List<MapMarkerInfo>();

            foreach (var mapMarkerInfo in PersonMapMarkerInfoDic.Values)
            {
                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)mapMarkerInfo.markerId);
                if (mapMarkerTypeDefine != null)
                {
                    if (mapMarkerTypeDefine.type == type)
                    {
                        MapMarkerInfoList.Add(mapMarkerInfo);
                    }
                }
            }

            foreach (var mapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)mapMarkerInfo.markerId);
                if (mapMarkerTypeDefine != null)
                {
                    if (mapMarkerTypeDefine.type == type)
                    {
                        MapMarkerInfoList.Add(mapMarkerInfo);
                    }
                }
            }

            MapMarkerInfoList.Sort((x, y) =>
            {
                return y.markerTime.CompareTo(x.markerTime);
            });

            return MapMarkerInfoList;
        }

        private List<MapMarkerInfo> GetMapMarkerInfoListAll()
        {
            List<MapMarkerInfo> MapMarkerInfoList = new List<MapMarkerInfo>();
            List<MapMarkerInfo> GuildMapMarkerInfoList = new List<MapMarkerInfo>();
            List<MapMarkerInfo> PersonMapMarkerInfoList = new List<MapMarkerInfo>();            

            foreach (var mapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                GuildMapMarkerInfoList.Add(mapMarkerInfo);
            }

            GuildMapMarkerInfoList.Sort((x, y) =>
            {
                return y.markerTime.CompareTo(x.markerTime);
            });

            foreach (var mapMarkerInfo in PersonMapMarkerInfoDic.Values)
            {
                PersonMapMarkerInfoList.Add(mapMarkerInfo);
            }

            PersonMapMarkerInfoList.Sort((x, y) =>
            {
                return y.markerTime.CompareTo(x.markerTime);
            });

            MapMarkerInfoList.AddRange(GuildMapMarkerInfoList);
            MapMarkerInfoList.AddRange(PersonMapMarkerInfoList);

            return MapMarkerInfoList;
        }

        public List<MapMarkerInfo> GetMapMarkerInfoListByPageIndex(int pageIndex)
        {
            List<MapMarkerInfo> MapMarkerInfoList = new List<MapMarkerInfo>();

            //全部
            if (pageIndex == 0)
            {
                MapMarkerInfoList = GetMapMarkerInfoListAll();
            }
            //特殊
            else if (pageIndex == 1)
            {
                MapMarkerInfoList = GetMapMarkerInfoListById(1000);
            }
            //朋友
            else if (pageIndex == 2)
            {
                MapMarkerInfoList = GetMapMarkerInfoListById(1001);
            }
            //敌人
            else if (pageIndex == 3)
            {
                MapMarkerInfoList = GetMapMarkerInfoListById(1002);
            }
            //工会
            else if (pageIndex == 4)
            {
                MapMarkerInfoList = GetMapMarkerInfoListByType(2);
            }
            else
            {
                Debug.LogError("Get MapMarkerInfoList By PageIndex Error.");
            }

            return MapMarkerInfoList;
        }

        public MapMarkerInfo GetPersonMapMarkerInfo(long markerIndex)
        {
            MapMarkerInfo mapMarkerInfo = null;

            PersonMapMarkerInfoDic.TryGetValue(markerIndex, out mapMarkerInfo);

            return mapMarkerInfo;
        }

        public MapMarkerInfo GetGuildMapMarkerInfo(long markerId)
        {
            MapMarkerInfo mapMarkerInfo = null;

            GuildMapMarkerInfoDic.TryGetValue(markerId, out mapMarkerInfo);

            return mapMarkerInfo;
        }

        public bool IsGuildContainsByMarkerId(long markerId)
        {
            foreach (var mapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                if (mapMarkerInfo.markerId == markerId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsGuildContainsByPos(float x, float y, out MapMarkerInfo info)
        {
            foreach (var mapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                if (Math.Abs(mapMarkerInfo.pos.x - x) <= 1 && Math.Abs(mapMarkerInfo.pos.y - y) <= 1)
                {
                    info = mapMarkerInfo;
                    return true;
                }
            }

            info = null;
            return false;
        }

        public bool IsPersonContainsByMarkerId(int markerId)
        {
            foreach (var mapMarkerInfo in PersonMapMarkerInfoDic.Values)
            {
                if (mapMarkerInfo.markerId == markerId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsPersonContainsByPos(float x, float y, out MapMarkerInfo info)
        {
            foreach (var mapMarkerInfo in PersonMapMarkerInfoDic.Values)
            {
                if (Math.Abs(mapMarkerInfo.pos.x - x) <= 1 && Math.Abs(mapMarkerInfo.pos.y - y) <= 1)
                {
                    info = mapMarkerInfo;
                    return true;
                }
            }

            info = null;
            return false;
        }

        public int CalPersonMapMarkerInfoNum()
        {
            return PersonMapMarkerInfoDic.Count;
        }

        public int CalGuildMapMarkerInfoNum()
        {
            return GuildMapMarkerInfoDic.Count;
        }

        public int CalGuildMapMarkerInfoMaxNum()
        {
            int maxNum = 0;

            List<MapMarkerTypeDefine> mapMarkerTypeDefineList = CoreUtils.dataService.QueryRecords<MapMarkerTypeDefine>();
            foreach (var mapMarkerTypeDefine in mapMarkerTypeDefineList)
            {
                if (mapMarkerTypeDefine.type == 2)
                {
                    maxNum++;
                }
            }

            return maxNum;
        }

        public int CalNewGuildMapMarkerInfoNum()
        {
            int newGuildMapMarkerInfoNum = 0;

            foreach (var guildMapMarkerInfo in GuildMapMarkerInfoDic.Values)
            {
                if (guildMapMarkerInfo.status == 0)
                {
                    newGuildMapMarkerInfoNum++;
                }
            }

            return newGuildMapMarkerInfoNum;
        }

        #region Network interaction

        public void AddPersonOrGuildMapMarker(long markerId, string description, float x, float y)
        {
            PosInfo pos = new PosInfo();
            pos.x = (long)x;
            pos.y = (long)y;

            Map_AddMarker.request req = new Map_AddMarker.request();
            req.markerId = markerId;
            req.description = description;
            req.gameNode = m_playerProxy.GetGameNode().ToString("N0");
            req.pos = pos;
            AppFacade.GetInstance().SendSproto(req);
        }

        public void EditPersonMapMarker(long markerIndex, long markerId, string description)
        {
            Map_ModifyMarker.request req = new Map_ModifyMarker.request();
            req.markerIndex = markerIndex;
            req.markerId = markerId;
            req.description = description;
            AppFacade.GetInstance().SendSproto(req);
        }

        public void EditGuildMapMarker(long markerId, string description, float x, float y, long oldMarkerId)
        {
            PosInfo pos = new PosInfo();
            pos.x = (long)x;
            pos.y = (long)y;

            Map_ModifyGuildMarker.request req = new Map_ModifyGuildMarker.request();
            req.markerId = markerId;
            req.description = description;
            req.gameNode = m_playerProxy.GetGameNode().ToString("N0");
            req.pos = pos;
            req.oldMarkerId = oldMarkerId;
            AppFacade.GetInstance().SendSproto(req);
        }

        public void DeleteMapMarker(long markerIndex, long markerId)
        {
            Map_DeleteMarker.request req = new Map_DeleteMarker.request();
            req.markerIndex = markerIndex;
            req.markerId = markerId;
            AppFacade.GetInstance().SendSproto(req);
        }

        public void UpdateMapMarker()
        {
            Map_UpdateGuildMarkerStatus.request req = new Map_UpdateGuildMarkerStatus.request();
            AppFacade.GetInstance().SendSproto(req);
        }

        #endregion

        #region Utility methods

        public void CalMapMarkerProgress(int pageIndex, out int curNum, out int maxNum)
        {
            curNum = 0;
            maxNum = 0;

            //全部
            if (pageIndex == 0)
            {
                curNum = CalPersonMapMarkerInfoNum() + CalGuildMapMarkerInfoNum();
                maxNum = m_configDefine.personMarkerLimit + CalGuildMapMarkerInfoMaxNum();
            }
            //特殊、朋友、敌人
            else if (pageIndex == 1 || pageIndex == 2 || pageIndex == 3)
            {
                curNum = CalPersonMapMarkerInfoNum();
                maxNum = m_configDefine.personMarkerLimit;
            }
            //工会
            else if (pageIndex == 4)
            {
                curNum = CalGuildMapMarkerInfoNum();
                maxNum = CalGuildMapMarkerInfoMaxNum();
            }
            else
            {
                Debug.LogError("Calculate MapMarker Progress pageIndex Error.");
            }
        }

        public string GetDefaultDescription(MapObjectInfoEntity mapObjectInfoEntity)
        {
            string defaultDescription = string.Empty;

            if (mapObjectInfoEntity != null)
            {
                switch (mapObjectInfoEntity.rssType)
                {
                    //玩家城市
                    case RssType.City:
                        if (mapObjectInfoEntity.guildId <= 0 && string.IsNullOrEmpty(mapObjectInfoEntity.guildAbbName))
                        {
                            defaultDescription = mapObjectInfoEntity.cityName;
                        }
                        else
                        {
                            defaultDescription = LanguageUtils.getTextFormat(300030, mapObjectInfoEntity.guildAbbName, mapObjectInfoEntity.cityName);
                        }
                        break;
                    //野外资源田
                    case RssType.Stone:
                    case RssType.Farmland:
                    case RssType.Wood:
                    case RssType.Gold:
                    case RssType.Gem:
                        ResourceGatherTypeDefine resourceGatherTypeDefine1 = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int)mapObjectInfoEntity.resourceId);
                        if (resourceGatherTypeDefine1 != null)
                        {
                            defaultDescription = LanguageUtils.getText(resourceGatherTypeDefine1.l_nameId);
                        }
                        break;
                    //野蛮人要塞
                    case RssType.BarbarianCitadel:
                        MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)mapObjectInfoEntity.monsterId);
                        if (monsterDefine != null)
                        {
                            defaultDescription = LanguageUtils.getText(monsterDefine.l_nameId);
                        }
                        break;
                    //联盟要塞、联盟旗帜、联盟资源中心
                    case RssType.GuildCenter:
                    case RssType.GuildFortress1:
                    case RssType.GuildFortress2:
                    case RssType.GuildFlag:
                    case RssType.GuildFoodResCenter:
                    case RssType.GuildWoodResCenter:
                    case RssType.GuildGoldResCenter:
                    case RssType.GuildGemResCenter:
                        int type1 = m_allianceProxy.GetBuildServerTypeToConfigType(mapObjectInfoEntity.objectType);
                        AllianceBuildingTypeDefine allianceBuildingTypeDefine1 = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type1);
                        if (allianceBuildingTypeDefine1 != null)
                        {
                            if (string.IsNullOrEmpty(mapObjectInfoEntity.guildAbbName))
                            {
                                defaultDescription = LanguageUtils.getText(allianceBuildingTypeDefine1.l_nameId);
                            }
                            else
                            {
                                defaultDescription = LanguageUtils.getTextFormat(300030, mapObjectInfoEntity.guildAbbName, LanguageUtils.getText(allianceBuildingTypeDefine1.l_nameId));
                            }
                        }
                        break;
                    //联盟资源点
                    case RssType.GuildFood:
                    case RssType.GuildWood:
                    case RssType.GuildStone:
                    case RssType.GuildGold:
                        int type2 = m_allianceProxy.GetBuildServerTypeToConfigType(mapObjectInfoEntity.objectType);
                        AllianceBuildingTypeDefine allianceBuildingTypeDefine2 = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type2);
                        if (allianceBuildingTypeDefine2 != null)
                        {
                            defaultDescription = LanguageUtils.getText(allianceBuildingTypeDefine2.l_nameId);
                        }
                        break;
                    //圣地、关卡奇观建筑
                    case RssType.CheckPoint:
                    case RssType.HolyLand:
                    case RssType.Sanctuary:
                    case RssType.Altar:
                    case RssType.Shrine:
                    case RssType.LostTemple:
                    case RssType.Checkpoint_1:
                    case RssType.Checkpoint_2:
                    case RssType.Checkpoint_3:
                        StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)mapObjectInfoEntity.strongHoldId);
                        if (strongHoldDataDefine != null)
                        {
                            StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);
                            if (strongHoldTypeDefine != null)
                            {
                                defaultDescription = LanguageUtils.getText(strongHoldTypeDefine.l_nameId);
                            }
                        }
                        break;
                    //山洞、村庄
                    case RssType.Cave:
                    case RssType.Village:
                        MapFixPointDefine mapFixPointDefine = CoreUtils.dataService.QueryRecord<MapFixPointDefine>((int)mapObjectInfoEntity.resourcePointId);
                        if (mapFixPointDefine != null)
                        {
                            ResourceGatherTypeDefine resourceGatherTypeDefine2 = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(mapFixPointDefine.type);
                            if (resourceGatherTypeDefine2 != null)
                            {
                                defaultDescription = LanguageUtils.getText(resourceGatherTypeDefine2.l_nameId);
                            }
                        }
                        break;
                    default: break;
                }
            }            

            return defaultDescription;
        }

        #endregion
    }
}