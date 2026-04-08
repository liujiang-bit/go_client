// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    RallyTroopsProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Hotfix;
using Skyunion;
using SprotoType;
using UnityEngine;
using System.Text;

namespace Game
{
    public class ItemWarData
    {
        public bool Involveme;//我已经再集结队伍里
        public bool Isme; //我发起的集结，或我被集结
        public DetialType detialType; //1集结，2被集结
        public RallyTroopType rallyTroopType;
        public RallyDetailEntity rallyDetailEntity; //集结
        public RallyedDetailEntity rallyedDetailEntity; //2被集结
        public long rallyRid; //被集结时的集结者rid
    }
    public enum DetialType
    {
        rallyDetail= 1,//联盟集结
        rallyedDetail = 2,//联盟被集结
    }
    public enum RallyTroopType
    {
        ready = 1,//准备中
        wait = 2,//等待中
        March = 3,//行军中，
        battle = 4,//战斗中
    }

    public class ItemWarDetialData
    {
        public ReinforceDetailEntity reinforceDetail; //增援信息
        public JoinRallyDetailEntity joinRallyDetail; //加入集结信息

        public DetialType detialType;
        public RallyTroopType rallyTroopType;
        public int itemType; //0,1,2,3//0，文字，1+号，2item，3士兵信息
        public int datatype;//1,增援信息  ,,1加入信息，2增援信息
        public bool isme; // 我发起的集结信息//
        public bool isCaptain;//队长
        public bool isSelected = false;
        public List<SoldierInfo> subItemData; //士兵信息

    }

    public class RallyTroopsData
    {
        public int mainHeroId;
        public int viceId;
        public Dictionary<long, SoldierInfo> soldierInfo = new Dictionary<long, SoldierInfo>();
        public int targetIndext;
        public int rallyTimes;
        public long joinRid;
        public long armyObjectIndex;
        public long reinforceObjectIndex;
    }

    public class RallyTroopsProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "RallyTroopsProxy";
        private PlayerProxy m_PlayerProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private CityBuildingProxy m_CityBuildingProxy;
        private TroopProxy m_TroopProxy;
        private CityBuffProxy m_CityBuffProxy;
        private ConfigDefine configDefine;
        private AllianceProxy m_allianceProxy;

        private readonly Dictionary<long, RallyDetailEntity> rallyDetailEntityDic =
            new Dictionary<long, RallyDetailEntity>(); //集结信息

        private readonly Dictionary<long, RallyedDetailEntity> rallyedDetailEntityDic =
            new Dictionary<long, RallyedDetailEntity>(); //被集结信息

        private readonly Dictionary<long, Dictionary<long, ReinforceDetailEntity>> rallyedreinforceDetailEntityDic =
            new Dictionary<long, Dictionary<long, ReinforceDetailEntity>>(); //被集结增援信息

        private readonly Dictionary<long, Dictionary<long, RallyDetailEntity>> rallyedrallyDetailEntityDic =
         new Dictionary<long,Dictionary<long, RallyDetailEntity>>(); //被集结者的集结信息

        private readonly Dictionary<long, Dictionary<long, JoinRallyDetailEntity>> joinRallyDetailEntityDic =
            new Dictionary<long, Dictionary<long, JoinRallyDetailEntity>>(); //集结的加入者信息
        private readonly Dictionary<long, Dictionary<long, ReinforceDetailEntity>> rallyReinforceDetailEntityDic =
    new Dictionary<long, Dictionary<long, ReinforceDetailEntity>>(); //集结的增援信息




        #endregion

        // Use this for initialization
        public RallyTroopsProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" RallyTroopsProxy register");     
        }

        public void Init()
        {
    
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_CityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_CityBuffProxy= AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_allianceProxy= AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
        }


        public override void OnRemove()
        {
            Debug.Log(" RallyTroopsProxy remove");
        }

        public void Set(Rally_RallyBattleInfo.request info)
        {
            if (m_PlayerProxy.CurrentRoleInfo!=null&& m_PlayerProxy.CurrentRoleInfo.guildId == 0)
            {
                Debug.Log("没有联盟的时候收到了联盟信息");
                return;
            }
            bool RallyTroopChange = false;//集结部队数量发生了变化
            if (info.rallyDetails != null)
            {
                foreach (var infoRallyDetail in info.rallyDetails.Values)
                {
                    RallyDetailEntity detailEntity = null;
                    if (!rallyDetailEntityDic.TryGetValue(infoRallyDetail.rallyRid, out detailEntity))
                    {
                        detailEntity = new RallyDetailEntity();
                        rallyDetailEntityDic[infoRallyDetail.rallyRid] = detailEntity;
                        RallyTroopChange = true;
                        m_showGuildCityHud = true;
                    }

                    //  Debug.LogErrorFormat("rallyRid:{0},,{1}", infoRallyDetail.rallyRid, infoRallyDetail.rallyObjectIndex);
                    RallyDetailEntity.updateEntity(detailEntity, infoRallyDetail);
                    if (detailEntity.rallyTargetDetail == null)
                    {
                        rallyDetailEntityDic.Remove(infoRallyDetail.rallyRid);
                        if (joinRallyDetailEntityDic.ContainsKey(infoRallyDetail.rallyRid))
                        {
                            joinRallyDetailEntityDic.Remove(infoRallyDetail.rallyRid);
                        }
                        if (rallyReinforceDetailEntityDic.ContainsKey(infoRallyDetail.rallyRid))
                        {
                            rallyReinforceDetailEntityDic.Remove(infoRallyDetail.rallyRid);
                        }
                        Debug.Log("服务器下发信息不全没收到集结目标");
                        continue;
                    }
                        if (detailEntity.rallyDelete || infoRallyDetail.rallyRid == 0 )
                    {
                        //    Debug.LogErrorFormat("rallyDelete");
                        rallyDetailEntityDic.Remove(infoRallyDetail.rallyRid);
                        if (joinRallyDetailEntityDic.ContainsKey(infoRallyDetail.rallyRid))
                        {
                            joinRallyDetailEntityDic.Remove(infoRallyDetail.rallyRid);
                        }
                        if (rallyReinforceDetailEntityDic.ContainsKey(infoRallyDetail.rallyRid))
                        {
                            rallyReinforceDetailEntityDic.Remove(infoRallyDetail.rallyRid);
                        }
                        RallyTroopChange = true;
                        continue;
                    }

                    if (!rallyReinforceDetailEntityDic.ContainsKey(infoRallyDetail.rallyRid))
                    {
                        rallyReinforceDetailEntityDic[infoRallyDetail.rallyRid] =
                            new Dictionary<long, ReinforceDetailEntity>();
                    }

                    if (infoRallyDetail.rallyReinforceDetail != null)
                    {
                        foreach (var reinforceDetailinfo in infoRallyDetail.rallyReinforceDetail.Values)
                        {
                            //  Debug.LogErrorFormat("reinforceDetailinfoRid:{0}", reinforceDetailinfo.reinforceRid);
                            ReinforceDetailEntity reinforceDetailEntity = null;
                            if (!rallyReinforceDetailEntityDic[infoRallyDetail.rallyRid]
                                .TryGetValue(reinforceDetailinfo.reinforceRid, out reinforceDetailEntity))
                            {
                                reinforceDetailEntity = new ReinforceDetailEntity();
                                rallyReinforceDetailEntityDic[infoRallyDetail.rallyRid][reinforceDetailinfo.reinforceRid] =
                                    reinforceDetailEntity;
                                RallyTroopChange = true;

                               
                            }

                            ReinforceDetailEntity.updateEntity(reinforceDetailEntity, reinforceDetailinfo);
                            if (reinforceDetailEntity.reinforceDelete)
                            {
                                RallyTroopChange = true;
                                rallyReinforceDetailEntityDic[infoRallyDetail.rallyRid].Remove(reinforceDetailEntity.reinforceRid);
                            }
                        }
                    }
                    if (!joinRallyDetailEntityDic.ContainsKey(infoRallyDetail.rallyRid))
                    {
                        joinRallyDetailEntityDic[infoRallyDetail.rallyRid] =
                            new Dictionary<long, JoinRallyDetailEntity>();
                    }

                    if (infoRallyDetail.rallyJoinDetail != null)
                    {
                        foreach (var joinRallyDetailInfo in infoRallyDetail.rallyJoinDetail.Values)
                        {
                            // Debug.LogErrorFormat("joinRallyDetailInfoRid:{0}", joinRallyDetailInfo.joinRid);
                            JoinRallyDetailEntity joinRallyDetail = null;
                            if (!joinRallyDetailEntityDic[infoRallyDetail.rallyRid]
                                .TryGetValue(joinRallyDetailInfo.joinRid, out joinRallyDetail))
                            {
                                joinRallyDetail = new JoinRallyDetailEntity();
                                joinRallyDetailEntityDic[infoRallyDetail.rallyRid][joinRallyDetailInfo.joinRid] =
                                    joinRallyDetail;
                                RallyTroopChange = true;
                            }

                            JoinRallyDetailEntity.updateEntity(joinRallyDetail, joinRallyDetailInfo);
                            if (joinRallyDetail.joinDelete)
                            {
                                RallyTroopChange = true;
                                //  Debug.LogErrorFormat("joinDelete");
                                joinRallyDetailEntityDic[infoRallyDetail.rallyRid].Remove(joinRallyDetail.joinRid);
                            }
                        }
                    }
                    SendEventTranckingByRallyDetail(detailEntity);
                }
            }
//--------------------------------------------------------------------------------------------------------------------
            if (info.rallyedDetail != null)
            {
                foreach (var rallyedDetail in info.rallyedDetail.Values)
                {
                     // Debug.LogErrorFormat("rallyedIndex:{0}", inforallyedDetail.rallyedIndex);
                    RallyedDetailEntity rallyedDetailEntity = new RallyedDetailEntity();
                    if (!rallyedDetailEntityDic.TryGetValue(rallyedDetail.rallyedIndex, out rallyedDetailEntity))
                    {
                        rallyedDetailEntity = new RallyedDetailEntity();
                        rallyedDetailEntityDic.Add(rallyedDetail.rallyedIndex, rallyedDetailEntity);
                    }

                    RallyedDetailEntity.updateEntity(rallyedDetailEntity, rallyedDetail);
                    //-----------------------------------------------------------------增援
                    if (!rallyedreinforceDetailEntityDic.ContainsKey(rallyedDetail.rallyedIndex))
                    {
                        rallyedreinforceDetailEntityDic[rallyedDetail.rallyedIndex] =
                            new Dictionary<long, ReinforceDetailEntity>();
                    }
                    if (rallyedDetail.reinforceDetail != null)
                    {
                        int count = rallyedDetail.reinforceDetail.Count;
                      for ( int i = 0;i< count; i++) 
                        {
                            var reinforce = rallyedDetail.reinforceDetail[i];
                          //  Debug.LogErrorFormat("reinforceRid:{0},,,,{1}", reinforce.reinforceRid,reinforce.armyIndex);
                            long key = reinforce.reinforceRid*10+reinforce.armyIndex;
                            if (reinforce.armyIndex == 0)
                            {
                                Debug.LogErrorFormat("没有收到增援部队下标");
                            }
                              ReinforceDetailEntity reinforceDetail = null;

                            if (!rallyedreinforceDetailEntityDic[rallyedDetail.rallyedIndex]
                                .TryGetValue(key, out reinforceDetail))
                            {
                                reinforceDetail = new ReinforceDetailEntity();
                                rallyedreinforceDetailEntityDic[rallyedDetail.rallyedIndex]
                                    .Add(key, reinforceDetail);
                                RallyTroopChange = true;
                            }

                            ReinforceDetailEntity.updateEntity(reinforceDetail, reinforce);
                            if (reinforceDetail.reinforceDelete)
                            {
                                 // Debug.LogErrorFormat("reinforceDelete");
                                RallyTroopChange = true;
                                rallyedreinforceDetailEntityDic[rallyedDetail.rallyedIndex].Remove(key); 
                            }

                        }
                    }
                    //————————————————————————————————被集结
                    if (!rallyedrallyDetailEntityDic.ContainsKey(rallyedDetail.rallyedIndex))
                    {
                        rallyedrallyDetailEntityDic[rallyedDetail.rallyedIndex] = new Dictionary<long, RallyDetailEntity>();
                    }
                    if (rallyedDetail.rallyDetail != null)
                    {
                        foreach (var rallyDetail in rallyedDetail.rallyDetail.Values)
                        {
                            //  Debug.LogErrorFormat("rallyedIndexrallyRid{0}", rallyDetail.rallyRid);
                            RallyDetailEntity detailEntity = null;
                            if (!rallyedrallyDetailEntityDic[rallyedDetail.rallyedIndex].TryGetValue(rallyDetail.rallyRid, out detailEntity))
                            {
                                RallyTroopChange = true;
                                detailEntity = new RallyDetailEntity();
                                rallyedrallyDetailEntityDic[rallyedDetail.rallyedIndex]
                                    .Add(rallyDetail.rallyRid, detailEntity);
                            }
                            RallyDetailEntity.updateEntity(detailEntity, rallyDetail);
                            if (detailEntity.rallyDelete)
                            {
                                RallyTroopChange = true;
                                rallyedrallyDetailEntityDic[rallyedDetail.rallyedIndex].Remove(rallyDetail.rallyRid);
                             
                            }
                            if (rallyedrallyDetailEntityDic[rallyedDetail.rallyedIndex].Count == 0)
                            {
                                RallyTroopChange = true;
                                rallyedrallyDetailEntityDic.Remove(rallyedDetail.rallyedIndex);
                            }
                        }
                    }
                }
            }
            if (RallyTroopChange)
            {
                if (rallyDetailEntityDic.Count==0)
                {
                    ClearGuildCityHud();
                }
                 rallyedCount = 0;
                rallyedrallyDetailEntityDic.Values.ToList().ForEach((rallyed)=> {
                    rallyedCount += rallyed.Count;
                });
                m_hasReadRally = rallyDetailEntityDic.Count + rallyedCount == 0;
                AppFacade.GetInstance().SendNotification(CmdConstant.RallyTroopChange,rallyDetailEntityDic.Count+ rallyedCount);
            }
        }

        //是否阅读过红点
        private bool m_hasReadRally = false;

        private bool m_showGuildCityHud = false;
        private int rallyedCount = 0;//被集结条数

        //集结红点数量
        public int GetRallyRedPoint()
        {
            if (m_PlayerProxy.CurrentRoleInfo.guildId<=0)
            {
                return 0;
            }
            return rallyDetailEntityDic.Count + rallyedCount;
        }

        public int GetMyGuildRallyCount()
        {
            return rallyDetailEntityDic.Count;
        }

        //设置红点已读
        // public void SetReadedRallyRedPoint()
        // {
        //     m_hasReadRally = true;
        // }

        public bool ShowGuildCityHudState()
        {
            return m_showGuildCityHud;
        }

        public void ClearGuildCityHud()
        {
            m_showGuildCityHud = false;
            AppFacade.GetInstance().SendNotification(CmdConstant.ChangeGuildCityHud,m_showGuildCityHud);
        }

        /// <summary>
        /// 退出联盟清理数据
        /// </summary>
        public void allianceEixt()
        {
            Clear();
        }
        public RallyDetailEntity GetRallyDetailEntity(long rallyRid)
        {
            RallyDetailEntity rallyDetail;
            if (rallyDetailEntityDic.TryGetValue(rallyRid, out rallyDetail))
            {
                return rallyDetail;
            }

            return null;
        }

        public RallyDetailEntity GetRallyDetailEntityByObjectId(long objectId)
        {
            foreach (var info in rallyDetailEntityDic.Values)
            {
                if (info.rallyObjectIndex == objectId)
                {
                    return info;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过军队获取集结信息
        /// </summary>
        /// <param name="rallyRid"></param>
        /// <returns></returns>
        public long GetRallyDetailEntityByarmIndex(long armIndex)
        {
            foreach (var temp in joinRallyDetailEntityDic)
            {
                Dictionary<long, JoinRallyDetailEntity> keyValuePairs = temp.Value;
                foreach (var temp2 in keyValuePairs)
                {
                    JoinRallyDetailEntity joinRallyDetailEntity = temp2.Value;
                    if (joinRallyDetailEntity.joinRid == m_PlayerProxy.CurrentRoleInfo.rid)
                    {
                        if (joinRallyDetailEntity.joinArmyIndex == armIndex)
                        {
                            return temp.Key;
                        }
                    }
                }
            }

            return 0;
        }

        public JoinRallyDetailEntity GetRallyDetailEntityByArmIndex(long armIndex)
        {
            foreach (var temp in joinRallyDetailEntityDic)
            {
                Dictionary<long, JoinRallyDetailEntity> keyValuePairs = temp.Value;
                foreach (var temp2 in keyValuePairs)
                {
                    JoinRallyDetailEntity joinRallyDetailEntity = temp2.Value;
                    if (joinRallyDetailEntity.joinRid == m_PlayerProxy.CurrentRoleInfo.rid)
                    {
                        if (joinRallyDetailEntity.joinArmyIndex == armIndex)
                        {
                            return temp2.Value;
                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 部队是集结部队，切我是队长
        /// </summary>
        /// <param name="armIndex"></param>
        /// <returns></returns>
        public bool IsCaptainByarmIndex(long armIndex)
        {
            foreach (var temp in joinRallyDetailEntityDic)
            {
                if (temp.Key == m_PlayerProxy.CurrentRoleInfo.rid)
                { 
                    Dictionary<long, JoinRallyDetailEntity> keyValuePairs = temp.Value;
                    foreach (var temp2 in keyValuePairs)
                    {
                        JoinRallyDetailEntity joinRallyDetailEntity = temp2.Value;
                        if (joinRallyDetailEntity.joinRid == m_PlayerProxy.CurrentRoleInfo.rid)
                        {
                            if (joinRallyDetailEntity.joinArmyIndex == armIndex)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 是否正在被集结
        /// </summary>
        /// <returns></returns>
        public bool IsrallyedIndex(long rallyedIndex)
        {
            if (rallyedrallyDetailEntityDic.ContainsKey(rallyedIndex))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///被集结时的集结者信息
        /// </summary>
        /// <param name="被集结目标"></param>
        /// <param name="集结发起"></param>
        /// <returns></returns>
        public RallyDetailEntity GetRallyDetailEntity(long rallyedIndex,long rallyRid)
        {
            Dictionary<long, RallyDetailEntity> rallyDetailEntitys;
            if (rallyedrallyDetailEntityDic.TryGetValue(rallyedIndex, out rallyDetailEntitys))
            {
                RallyDetailEntity data;
                 if (rallyDetailEntitys.TryGetValue(rallyRid, out data))
                    {
                        return data;
                    }
            }

            return null;
        }
        /// <summary>
        ///  被集结信息
        /// </summary>
        /// <param name="被集结目标"></param>
        /// <returns></returns>
        public RallyedDetailEntity GetRallyedDetailEntity(long rallyedIndex)
        {
            RallyedDetailEntity rallyedDetail;
            if (rallyedDetailEntityDic.TryGetValue(rallyedIndex, out rallyedDetail))
            {
                return rallyedDetail;
            }

            return null;
        }

        /// <summary>
        /// 被集结的增援信息
        /// </summary>
        /// <param name="被集结目标"></param>
        /// <returns></returns>
        public Dictionary<long, ReinforceDetailEntity> GetRallyedReinforceDetailEntity(long rllyerIndex)
        {
            Dictionary<long, ReinforceDetailEntity> reinforceDetail;
            if (rallyedreinforceDetailEntityDic.TryGetValue(rllyerIndex, out reinforceDetail))
            {
                return reinforceDetail;
            }
            return null;
        }
        /// <summary>
        /// 增援信息
        /// </summary>
        /// <param name="集结发起"></param>
        /// <param name="增援角色rid"></param>
        /// <returns></returns>
        public ReinforceDetailEntity GetRallyReinforceDetailEntity(long rallyRid, long reinforceRid)
        {
            Dictionary<long, ReinforceDetailEntity> reinforceDetail;
            if (rallyReinforceDetailEntityDic.TryGetValue(rallyRid, out reinforceDetail))
            {
                ReinforceDetailEntity data;
                if (reinforceDetail.TryGetValue(reinforceRid, out data))
                {
                    return data;
                }
            }

            return null;
        }
        /// <summary>
        /// 加入集结信息
        /// </summary>
        /// <param name="集结发起"></param>
        /// <param name="加入集结"></param>
        /// <returns></returns>
        public JoinRallyDetailEntity GetJoinRallyDetailEntity(long rallyRid, long joninRid)
        {
            Dictionary<long, JoinRallyDetailEntity> joinRallyDetail;
            if (joinRallyDetailEntityDic.TryGetValue(rallyRid, out joinRallyDetail))
            {
                JoinRallyDetailEntity data;
                if (joinRallyDetail.TryGetValue(joninRid, out data))
                {
                    return data;
                }
            }

            return null;
        }
        /// <summary>
        /// 被集结时获取增援部队数量
        /// </summary>
        /// <param name="rallyedIndex"></param>
        /// <returns></returns>
        public long GetReinforceCount(long rallyedIndex)
        {
            long count = 0;
            Dictionary<long, ReinforceDetailEntity> ReinforceDetailEntitys = GetRallyedReinforceDetailEntity(rallyedIndex);
            if (ReinforceDetailEntitys == null || ReinforceDetailEntitys.Count == 0)
            {

            }
            else
            {
                ReinforceDetailEntitys.Values.ToList().ForEach((reinforceDetailEntity) => {
                    MapObjectInfoEntity mapObjectInfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(rallyedIndex);
                    int leftTimeArrival = (int)(reinforceDetailEntity.arrivalTime - ServerTimeModule.Instance.GetServerTime());
                    if (leftTimeArrival >= 0)
                    {

                    }
                    else
                    {
                        count += m_allianceProxy.CountSoldiers(reinforceDetailEntity.soldiers);

                    }
                });
            }
            return count;
        }


        public List<ItemWarData> GetItemWarDataList()
        {
            List<ItemWarData> itemWarDatas = new List<ItemWarData>();
            List<RallyDetailEntity> rallyDetailEntityList = new List<RallyDetailEntity>();
            {
                rallyDetailEntityList = rallyDetailEntityDic.Values.ToList();
                rallyDetailEntityList.ForEach((rallyDetailEntity) =>
                {
                    ItemWarData itemWarData = new ItemWarData();
                    itemWarData.detialType = DetialType.rallyDetail;
                    itemWarData.rallyDetailEntity = rallyDetailEntity;
                    itemWarData.Isme = rallyDetailEntity.rallyRid == m_PlayerProxy.CurrentRoleInfo.rid;
                    itemWarData.rallyRid = rallyDetailEntity.rallyRid;
                    itemWarData.Involveme = GetJoinRallyDetailEntity(rallyDetailEntity.rallyRid, m_PlayerProxy.CurrentRoleInfo.rid) != null;
                    itemWarDatas.Add(itemWarData);
                });
            }
            rallyedDetailEntityDic.Values.ToList().ForEach((RallyedDetailEntity) =>
            {
            Dictionary<long, RallyDetailEntity> temp = null;
                if (rallyedrallyDetailEntityDic.TryGetValue(RallyedDetailEntity.rallyedIndex, out temp))
                {
                    rallyDetailEntityList = temp.Values.ToList();
                    rallyDetailEntityList.ForEach((rallyDetailEntity) =>
                {
                    ItemWarData itemWarData = new ItemWarData();
                    itemWarData.detialType = DetialType.rallyedDetail;
                    itemWarData.rallyedDetailEntity = RallyedDetailEntity;
                    itemWarData.rallyDetailEntity = rallyDetailEntity;
                    itemWarData.rallyRid = rallyDetailEntity.rallyRid;
                    itemWarData.Isme = RallyedDetailEntity.rallyedIndex == m_CityBuildingProxy.MyCityObjData.mapObjectExtEntity.objectId;
                    itemWarDatas.Add(itemWarData);
                });

            }
         
            });
            //TODO:排序
            //   itemWarDatas.com
            return itemWarDatas;
        }

        /// <summary>
        /// 战争详情显示集结信息
        /// </summary>
        /// <param name="rid">集结发起</param>
        /// <returns></returns>
        public List<ItemWarDetialData> getItemWarDetialDataListType1(long rid)
        {
            List<ItemWarDetialData> itemWarDetialDatas = new List<ItemWarDetialData>();

            RallyDetailEntity rallyDetailEntity = new RallyDetailEntity();
            if (rallyDetailEntityDic.TryGetValue(rid, out rallyDetailEntity))
            {
                long leftTimeReady = rallyDetailEntity.rallyReadyTime - ServerTimeModule.Instance.GetServerTime(); //准备中
                long leftTimeWait = rallyDetailEntity.rallyWaitTime - ServerTimeModule.Instance.GetServerTime(); //等待中
                long leftTimeMarch = rallyDetailEntity.rallyMarchTime - ServerTimeModule.Instance.GetServerTime(); //行军中
                Dictionary<long, JoinRallyDetailEntity> keyValuePairs = new Dictionary<long, JoinRallyDetailEntity>();
                Dictionary<long, ReinforceDetailEntity> keyValuePairs1 = new Dictionary<long, ReinforceDetailEntity>();
                if (!IsRallyTroopAttackMe(rid))
                {
                    if (rallyDetailEntity.rallyArmyCountMax <= rallyDetailEntity.rallyArmyCount)
                    {
                       
                    }
                    else
                    {
                        if (leftTimeReady > 0)
                        {
                            ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                            itemWarDetialData.itemType = 1;
                            itemWarDetialData.detialType = DetialType.rallyDetail;
                            itemWarDetialData.rallyTroopType = RallyTroopType.ready;
                            itemWarDetialDatas.Add(itemWarDetialData);
                        }
                        else if (leftTimeWait > 0)
                        {
                            ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                            itemWarDetialData.itemType = 0;
                            itemWarDetialData.detialType = DetialType.rallyDetail;
                            itemWarDetialData.rallyTroopType = RallyTroopType.wait;
                            itemWarDetialDatas.Add(itemWarDetialData);
                        }
                        else if (leftTimeMarch > 0)
                        {
                            ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                            itemWarDetialData.itemType = 1;
                            itemWarDetialData.detialType = DetialType.rallyDetail;
                            itemWarDetialData.rallyTroopType = RallyTroopType.March;
                            itemWarDetialDatas.Add(itemWarDetialData);
                        }
                        else
                        {
                            ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                            itemWarDetialData.itemType = 1;
                            itemWarDetialData.detialType = DetialType.rallyDetail;
                            itemWarDetialData.rallyTroopType = RallyTroopType.battle;
                            itemWarDetialDatas.Add(itemWarDetialData);
                        }
                    }
                       
                }
                if (joinRallyDetailEntityDic.TryGetValue(rid, out keyValuePairs))
                {
                    List<JoinRallyDetailEntity> temp = keyValuePairs.Values.ToList();
                    temp.ForEach((JoinRallyDetailEntity) =>
                    {
                        ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                        itemWarDetialData.itemType = 2;
                        itemWarDetialData.datatype = 1;
                        itemWarDetialData.detialType = DetialType.rallyDetail;
                        itemWarDetialData.joinRallyDetail = JoinRallyDetailEntity;
                        itemWarDetialData.isSelected = false;
                        itemWarDetialDatas.Add(itemWarDetialData);
                        if (JoinRallyDetailEntity.joinRid == rallyDetailEntity.rallyRid)
                        {
                            itemWarDetialData.isCaptain = true;
                        }
                        else
                        {
                            itemWarDetialData.isCaptain = false;
                        }
                        if (JoinRallyDetailEntity.joinRid == m_PlayerProxy.CurrentRoleInfo.rid)
                        {
                            itemWarDetialData.isme = true;
                        }
                        else
                        {
                            itemWarDetialData.isme = false;
                        }
                    });
                }
                if (rallyReinforceDetailEntityDic.TryGetValue(rid, out keyValuePairs1))
                {
                    List<ReinforceDetailEntity> temp = keyValuePairs1.Values.ToList();
                    temp.ForEach((reinforceDetailEntity) =>
                    {
                        ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                        itemWarDetialData.itemType = 2;
                        itemWarDetialData.datatype = 2;
                        itemWarDetialData.detialType = DetialType.rallyDetail;
                        itemWarDetialData.reinforceDetail = reinforceDetailEntity;
                        itemWarDetialDatas.Add(itemWarDetialData);
                        if (reinforceDetailEntity.reinforceRid == rallyDetailEntity.rallyRid)
                        {
                            itemWarDetialData.isCaptain = true;
                            Debug.LogError("isCaptain error");
                        }
                        else
                        {
                            itemWarDetialData.isCaptain = false;
                        }
                        if (reinforceDetailEntity.reinforceRid == m_PlayerProxy.CurrentRoleInfo.rid)
                        {
                            itemWarDetialData.isme = true;
                        }
                        else
                        {
                            itemWarDetialData.isme = false;
                        }
                    });
                }

            }
            itemWarDetialDatas.Sort(Compare);

            return itemWarDetialDatas;
        }
        public int Compare(ItemWarDetialData x, ItemWarDetialData y)
        {
            if (x.isCaptain)
            {
                return -1;
            }
            else
            {
                return 1;
            }
            return 0;
        }

        private void addsoldierInfos(ItemWarDetialData tag)
        { 
        List<SoldierInfo> soldierInfos = new List<SoldierInfo>();

            if (tag.detialType == DetialType.rallyDetail)
            {

                soldierInfos = tag.joinRallyDetail.joinSoldiers.Values.ToList();
            }
            else if (tag.detialType == DetialType.rallyedDetail)
            {
                if (tag.datatype == 1)

                {
                    soldierInfos = tag.reinforceDetail.soldiers.Values.ToList();
                }
            }
//            soldierInfos.Sort((SoldierInfo x, SoldierInfo y) =>
//            {
//                int re = 0;
//re = ((int) y.level).CompareTo((int) x.level);
//                if (re == 0)
//                {
//                    return x.type.CompareTo(y.type);
//                }
//                return re;
//            });
//            int len = soldierInfos.Count;
//            for (int i = 0; i<len; i = i + 4)
//            {
//                ItemWarDetialData itemWarDetialData = new ItemWarDetialData();

//itemWarDetialData.itemType = 3; //兵种
//                itemWarDetialData.joinRallyDetail = tag.joinRallyDetail;
//                itemWarDetialData.subItemData = new List<SoldierInfo>();
//                itemWarDetialData.detialType = tag.detialType;

//                for (int j = i; j<i + 4; j++)
//                {
//                    if (j<len)
//                    {
//                        itemWarDetialData.subItemData.Add(soldierInfos[j]);
//                    }
//                }

//                m_itemWarDetialDatas.Insert(index + 1, itemWarDetialData);
//                index++;
  //          }
    }

        /// <summary>
        /// 战争详情显示增援信息
        /// </summary>
        /// <returns></returns>
        public List<ItemWarDetialData> getItemWarDetialDataListType2(long rallyedIndex)
        {
            List<ItemWarDetialData> itemWarDetialDatas = new List<ItemWarDetialData>();
            Dictionary<long, ReinforceDetailEntity> reinforceDetailEntits = null;

            if (rallyedDetailEntityDic.ContainsKey(rallyedIndex))
            {
                if (rallyedDetailEntityDic[rallyedIndex].rallyedReinforceMax == 0)
                {

                    ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                    itemWarDetialData.itemType = 0;
                    itemWarDetialData.detialType = DetialType.rallyedDetail;
                    itemWarDetialDatas.Add(itemWarDetialData);
                }
                else
                {

                    if (rallyedreinforceDetailEntityDic.TryGetValue(rallyedIndex, out reinforceDetailEntits))
                    {
                        MapObjectInfoEntity mapObjectInfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(rallyedIndex);
                        if (mapObjectInfo != null && mapObjectInfo.cityRid != m_PlayerProxy.CurrentRoleInfo.rid)
                        {
                            if (GetReinforceCount(rallyedIndex) >= rallyedDetailEntityDic[rallyedIndex].rallyedReinforceMax)
                            {
                                ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                                itemWarDetialData.itemType = 0;
                                itemWarDetialData.detialType = DetialType.rallyedDetail;
                                itemWarDetialDatas.Add(itemWarDetialData);
                            }
                            else
                            {
                                ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                                itemWarDetialData.itemType = 1;
                                itemWarDetialData.detialType = DetialType.rallyedDetail;
                                itemWarDetialDatas.Add(itemWarDetialData);
                            }
                        }
                        else if (mapObjectInfo == null)
                        {
                            if (GetReinforceCount(rallyedIndex) < rallyedDetailEntityDic[rallyedIndex].rallyedReinforceMax)
                            {
                                ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                                itemWarDetialData.itemType = 1;
                                itemWarDetialData.detialType = DetialType.rallyedDetail;
                                itemWarDetialDatas.Add(itemWarDetialData);
                            }
                        }
                        List<ReinforceDetailEntity> reinforceDetailEntityList = reinforceDetailEntits.Values.ToList();
                        reinforceDetailEntityList.ForEach((reinforceDetailEntity) =>
                        {
                            ItemWarDetialData itemWarDetialData = new ItemWarDetialData();
                            itemWarDetialData.itemType = 2;
                            itemWarDetialData.datatype = 1;
                            itemWarDetialData.detialType = DetialType.rallyedDetail;
                            itemWarDetialData.reinforceDetail = reinforceDetailEntity;
                            itemWarDetialDatas.Add(itemWarDetialData);
                        });
                    }
                }
            }
            return itemWarDetialDatas;
        }
        /// <summary>
        /// 加入或增援
        /// </summary>
        /// <param name=""></param>
        public void SendJoinOrReinforce(ItemWarData itemWarData)
        {
            long leftTimeReady = itemWarData.rallyDetailEntity.rallyReadyTime - ServerTimeModule.Instance.GetServerTime(); //准备中
            long leftTimeWait = itemWarData.rallyDetailEntity.rallyWaitTime - ServerTimeModule.Instance.GetServerTime(); //等待中
            long leftTimeMarch = itemWarData.rallyDetailEntity.rallyMarchTime - ServerTimeModule.Instance.GetServerTime(); //行军中
            Dictionary<long, JoinRallyDetailEntity> keyValuePairs = new Dictionary<long, JoinRallyDetailEntity>();
            long  objectId = 0;
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(itemWarData.rallyDetailEntity.rallyObjectIndex);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.objectType == (int)RssType.BarbarianCitadel)
                {
                    objectId = itemWarData.rallyDetailEntity.rallyTargetDetail.rallyTargetMonsterId;
                }
                else
                {
                    objectId = mapObjectInfoEntity.objectId;
                }

            }

            long rallyArmyCount = 0;//当前兵力
            if (leftTimeReady > 0)
            {
                FightHelper.Instance.JoinRally((int)objectId,
                                               itemWarData.rallyDetailEntity.rallyRid,
                                               itemWarData.rallyDetailEntity.rallyPos.x,
                                               itemWarData.rallyDetailEntity.rallyPos.y,
                                               itemWarData.rallyDetailEntity.rallyArmyCountMax - itemWarData.rallyDetailEntity.rallyArmyCount,
                                               itemWarData.rallyDetailEntity.rallyReadyTime - itemWarData.rallyDetailEntity.rallyStartTime,
                                               itemWarData.rallyDetailEntity.rallyTargetDetail.rallyTargetMonsterId);
            }
            else if (leftTimeWait > 0)
            {
            }
            else if (leftTimeMarch > 0)
            {
                if (mapObjectInfoEntity != null)
                {
                    Vector2 pos = GetRallyTroopPos(mapObjectInfoEntity.objectId, itemWarData);
                    FightHelper.Instance.Reinfore((int)objectId, (int)itemWarData.rallyDetailEntity.rallyRid, itemWarData.rallyDetailEntity.rallyObjectIndex, pos.x, pos.y, false, itemWarData.rallyDetailEntity.rallyArmyCountMax - itemWarData.rallyDetailEntity.rallyArmyCount);
                }
                else
                {
                    Vector2 pos = GetRallyTroopMovePos(itemWarData);
                    FightHelper.Instance.Reinfore((int)objectId, (int)itemWarData.rallyDetailEntity.rallyRid, itemWarData.rallyDetailEntity.rallyObjectIndex, pos.x, pos.y);
                }

            }
            else //战斗中
            {
                if (mapObjectInfoEntity != null)
                {
                    Vector2 pos = GetRallyTroopPos(mapObjectInfoEntity.objectId, itemWarData);
                    //  Debug.LogErrorFormat("{0}", itemWarData.rallyDetailEntity.rallyArmyCountMax - itemWarData.rallyDetailEntity.rallyArmyCount);
                    FightHelper.Instance.Reinfore((int)objectId, (int)itemWarData.rallyDetailEntity.rallyRid, itemWarData.rallyDetailEntity.rallyObjectIndex, pos.x, pos.y, false, itemWarData.rallyDetailEntity.rallyArmyCountMax - itemWarData.rallyDetailEntity.rallyArmyCount);
                }
                else
                {
                    Vector2 pos = GetRallyTroopMovePos(itemWarData);
                    //    Debug.LogErrorFormat("{0}", itemWarData.rallyDetailEntity.rallyArmyCountMax - itemWarData.rallyDetailEntity.rallyArmyCount);
                    FightHelper.Instance.Reinfore((int)objectId, (int)itemWarData.rallyDetailEntity.rallyRid, itemWarData.rallyDetailEntity.rallyObjectIndex, pos.x, pos.y);
                }
            }
        }

        #region 集结检测


        public bool isRally(long targetRid, int objectId, Action callback=null)
        {
            bool show = isAddGuild();
            if (!show)
            {
                return false;
            }

            bool show1 = isHaveCityGuild();

            if (!show1)
            {
                return false;
            }

//            bool show2 = CheckAttackOtherCity();
//            if (!show2)
//            {
//                return false;
//            }

            bool show3 = IsRallyTroopHaveMe();
            if (!show3)
            {
                return false;
            }

            bool show4 = IsPlayTroopNum();
            if (!show4)
            {
                return false;
            }

       

//            bool show6 = m_CityBuffProxy.HasType2Buff(targetRid);
//            if (!show6)
//            {
//                Tip.CreateTip(730264).Show();
//                return false;
//            }

            bool show7 = HasRallyed(targetRid);
            if (show7)
            {
                Tip.CreateTip(730261).Show();
                return false;
            }

            bool show8 = IsRallyTargetCityCheck(targetRid);
            if (!show8)
            {
                string des = LanguageUtils.getTextFormat(730262,configDefine.rallyCityMinLevel);
                Tip.CreateTip(des).Show();
                return false;
            }

//            bool show9 = IsProvince(targetRid);
//            if (!show9)
//            {
//                Tip.CreateTip(730263).Show();
//                return false;
//            }
            bool show5 = IsWasFever(objectId, OpenPanelType.CreateRally,callback);
            if (!show5)
            {
                return false;
            }

            return true;
        }
        
   

        //判断发起集结玩家是否加入了联盟
        public bool isAddGuild()
        {
            bool isShow = m_PlayerProxy.CurrentRoleInfo.guildId == 0;
            if (isShow)
            {
                Tip.CreateTip(730259).Show();
                return false;
            }

            return true;
        }

        //判断发起集结玩家城市内是否拥有“城堡”建筑

        public bool isHaveCityGuild()
        {
            BuildingInfoEntity buildingInfoEntity = m_CityBuildingProxy.GetBuildingInfoByType(17);
            if (buildingInfoEntity == null)
            {
                Tip.CreateTip(730260).Show();
                return false;
            }

            return true;
        }
        //判断发起集结玩家市政厅建筑等级是否>=X级

        public bool CheckAttackOtherCity()
        {
            bool isShow = false;
            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (configDefine != null)
            {
                isShow= m_PlayerProxy.CurrentRoleInfo.level < configDefine.attackCityLevel;
                if (isShow)
                {
                    string des = LanguageUtils.getTextFormat(181186, configDefine.attackCityLevel);
                    Tip.CreateTip(des).Show();
                    return false;
                }
            }        
            return true;
        }

        //玩家自己是否已经存在一个已经发起的集结部队

        public bool IsRallyTroopHaveMe()
        {
            bool isShow = GetTroopHaveRally(m_PlayerProxy.CurrentRoleInfo.rid, m_PlayerProxy.CurrentRoleInfo.rid);
            if (isShow)
            {
                Tip.CreateTip(730328).Show();
                return false;
            }

            return true;
        }

        //玩家当前已存在的部队数量是否小于行军队列上限？
        public bool IsPlayTroopNum()
        {
            bool isShow = m_TroopProxy.GetIsCityCreateTroop();
            if (!isShow)
            {
                Tip.CreateTip(730265).Show();
                return false;
            }

            return true;
        }

        //发起集结玩家是否存在战争狂热状态
        public bool IsWasFever(int objectId, OpenPanelType type= OpenPanelType.CreateRally,Action callBack= null)
        {

            MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(objectId);
            if (infoEntity != null && (infoEntity.objectType == (int) RssType.Monster ||
                                       infoEntity.objectType == (int) RssType.Guardian ||
                                       infoEntity.objectType == (int) RssType.BarbarianCitadel ||
                                       infoEntity.objectType == (int) RssType.SummonAttackMonster ||
                                       infoEntity.objectType == (int) RssType.SummonConcentrateMonster))
            {
                return true;
            }

            bool isShow = m_TroopProxy.IsWarFever();
            if (isShow)
            {

                TroopBehavior troopBehavior = TroopBehavior.Play;
                if (type == OpenPanelType.Common)
                {
                    troopBehavior = TroopBehavior.Attack;
                }

                if (m_TroopProxy.GetIsDayShowPanel())
                {
                    switch (infoEntity.rssType)
                    {
                        case RssType.CheckPoint:
                        case RssType.Checkpoint_1:
                        case RssType.Checkpoint_2:
                        case RssType.Checkpoint_3:
                        case RssType.HolyLand:
                        case RssType.Sanctuary:
                        case RssType.Altar:
                        case RssType.Shrine:
                        case RssType.LostTemple:
                            if(infoEntity.guildId == 0)
                            {
                                if (callBack == null)
                                {
                                    OpenPanelData openPanelData = new OpenPanelData(objectId, type);
                                    FightHelper.Instance.OpenCreateArmyPanel(openPanelData);
                                }
                                else
                                {
                                    callBack.Invoke();
                                }
                                return true;
                            }
                            break;
                    }
                    TroopHelp.ShowIsAttackOtherTroop(() =>
                    {
                        if (callBack == null)
                        {                           
                            OpenPanelData openPanelData= new OpenPanelData(objectId,type);
                            FightHelper.Instance.OpenCreateArmyPanel(openPanelData);
                        }
                        else
                        {
                            callBack.Invoke();
                        }


                    }, (isOn) =>
                    {
                        if (isOn)
                        {
                            TroopHelp.SetShowAttackOtherTroopPlayerPrefs(m_TroopProxy.saveKey,
                                (int) ServerTimeModule.Instance.GetServerTime());
                        }
                    },troopBehavior);
                    return false;
                }

                return true;
            }

            return true;
        }
        /// <summary>
        /// 我方目标是否被集结
        /// </summary>
        /// <param name="rallyedIndex"></param>
        /// <returns></returns>
        public bool HasRallyed(long rallyedIndex)
        {
            if (!rallyedrallyDetailEntityDic.ContainsKey(rallyedIndex))
            {
                return false;
            }
            if (rallyedrallyDetailEntityDic[rallyedIndex].Count == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 已经被自己联盟集结
        /// </summary>
        /// <returns></returns>
        public bool HasRally(long rid )
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByRid(rid);
            if (mapObjectInfoEntity != null)
            {
                string name = mapObjectInfoEntity.cityName;
            foreach (var rallyDetail in rallyDetailEntityDic.Values)
                {
                    if (rallyDetail.rallyTargetDetail.rallyTargetType == (long)RssType.City)
                    {
                        if (rallyDetail.rallyTargetDetail.rallyTargetName == name)
                        {
                            return true;
                        }
                    }
                }
            }
          
            return false;
        }
        
        #region 集结攻击城市判断

        //目标城市市政厅是否>=X级，X配置在config的rallyCityMinLevel字段
        public bool IsRallyTargetCityCheck(long id)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByRid(id);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.objectType == (int) RssType.City)
                {
                    if (configDefine != null)
                    {
                        return mapObjectInfoEntity.cityLevel >= configDefine.rallyCityMinLevel|| mapObjectInfoEntity.guildId!=0;
                    }
                }
            }

            return true;
        }
        
        //被集结的目标城市所在区域是否与发起集结的玩家城市所在区域为同一个区域

        public bool IsProvince(long id)
        {            
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByRid(id);
            if (mapObjectInfoEntity != null)
            {
                Vector2 v2= new Vector2(mapObjectInfoEntity.objectPos.x/100f,mapObjectInfoEntity.objectPos.y/100f);
                return m_TroopProxy.IsProvince(v2);
            }

            return false;
        }

        #endregion

        #region 集结攻击联盟建筑判断

        

        #endregion
        
        #region 集结目标为关卡时

        

        #endregion
        
        #region 集结目标为圣物时

        

        #endregion
        
        #region 集结目标为其他集结部队（反集结）

        

        #endregion

        #region 加入集结判断

        public bool IsJoinRallyCheck(long rid,bool isShow=true)
        {
            bool show4 = IsPlayTroopNum();
            if (!show4)
            {
                return true;
            }

            bool show= IsRallyTroopAttackMe(rid);      
            if (show)
            {
                if (isShow)
                {              
                    Tip.CreateTip(730278).Show();
                }

                return true;
            }
            return false;
        }
        
        #endregion

        #endregion
        /// <summary>
        /// 是否有有自己的部队存在这个集结部队里面
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public bool IsRallyTroopAttackMe(long rid)
        {           
            JoinRallyDetailEntity rallyDetail =GetJoinRallyDetailEntity(rid, m_PlayerProxy.CurrentRoleInfo.rid);
            ReinforceDetailEntity reinforceDetailEntity = GetRallyReinforceDetailEntity(rid, m_PlayerProxy.CurrentRoleInfo.rid);
            bool isShow = false;
            bool isShow1 = false;
            if (rallyDetail != null)
            {
                isShow = true;
                Debug.LogWarning("有自己的部队存在这个集结部队里面2222");
            }
            if (reinforceDetailEntity != null)
            {
                isShow = true;
                Debug.LogWarning("有自己的部队存在这个集结部队里面111");
            }

            return isShow|| isShow1;
        }


        public bool GetTroopHaveRally(long rallyRid, long troopRid)
        {
            RallyDetailEntity rallyDetail = GetRallyDetailEntity(rallyRid);
            if (rallyDetail != null)
            {
                JoinRallyDetailEntity joinRallyDetail = GetJoinRallyDetailEntity(rallyRid, troopRid);
                if (joinRallyDetail != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetRallyArmyCountMax(long rallyRid)
        {
            RallyDetailEntity rallyDetail = GetRallyDetailEntity(rallyRid);
            if (rallyDetail != null)
            {
                return rallyDetail.rallyArmyCount >= rallyDetail.rallyArmyCountMax;
            }

            return false;
        }
        
        //集结部队是否属于我的联盟

        public bool isRallyTroopHaveGuid(long rid)
        {
            GuildMemberInfoEntity guildMemberInfoEntity=   m_allianceProxy.getMemberInfo(rid);
            if (guildMemberInfoEntity != null)
            {
                return true;
            }
            return false;
        }
        #region 集结PVP战斗，发起或参与1级野蛮人城寨

        private void SendEventTranckingByRallyDetail(RallyDetailEntity detailEntity)
        {
            if (detailEntity.rallyTargetDetail == null)
            {
                Debug.Log("服务器下发的信息不全");
                return;
            }
            if (detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.BarbarianCitadel)
            {
                if ((m_PlayerProxy.CurrentRoleInfo.eventTrancking & (long)Math.Pow(2, (int)EnumEventTracking.BarbarianStronghold)) != 0)
                {
                    return;
                }
                var monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)detailEntity.rallyTargetDetail.rallyTargetMonsterId);
                if (monsterDefine != null)
                {
                    if (monsterDefine.type == 2)
                    {
                        if (monsterDefine.level == 1 || monsterDefine.level == 2)
                        {
                            if (IsRallyTroopAttackMe(detailEntity.rallyRid))
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.BarbarianStronghold));
                            }
                        }
                    }
                }
            }
            else if (detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.City 
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Troop 
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.CheckPoint
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Checkpoint_1
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Checkpoint_2
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Checkpoint_3
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.HolyLand
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Sanctuary
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Altar
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.Shrine
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.LostTemple
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.GuildCenter
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.GuildFortress1
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.GuildFortress2
                || detailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.GuildFlag
                )
            {
                if ((m_PlayerProxy.CurrentRoleInfo.eventTrancking & (long)Math.Pow(2, (int)EnumEventTracking.RallyBattle)) != 0)
                {
                    return;
                }
                if (IsRallyTroopAttackMe(detailEntity.rallyRid))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.RallyBattle));
                }
            }
        }
        #endregion
        #region 协议

        //发起集结
        public void SendLaunchRally(RallyTroopsData data)
        {
            if (data == null)
            {
                return;
            }

            Rally_LaunchRally.request req = new Rally_LaunchRally.request();
            req.mainHeroId = data.mainHeroId;
            req.deputyHeroId = data.viceId;
            req.soldiers = new Dictionary<long, SoldierInfo>();
            foreach (var soldier in data.soldierInfo)
            {
                if (soldier.Value.num != 0)
                {
                    req.soldiers.Add(soldier.Key, soldier.Value);
                }   
            }

            req.targetIndex = data.targetIndext;
            req.rallyTimes = data.rallyTimes;
            AppFacade.GetInstance().SendSproto(req);
        }

        //加入集群
        public void SendJoinRally(RallyTroopsData data)
        {
            if (data == null)
            {
                return;
            }

            Rally_JoinRally.request req = new Rally_JoinRally.request();
            req.mainHeroId = data.mainHeroId;
            req.deputyHeroId = data.viceId;
            req.soldiers = new Dictionary<long, SoldierInfo>();
            foreach (var soldier in data.soldierInfo)
            {
                if (soldier.Value.num != 0)
                {
                    req.soldiers.Add(soldier.Key, soldier.Value);
                }
            }

            req.joinRid = data.joinRid;
            AppFacade.GetInstance().SendSproto(req);
        }


        //加入集结 。已经在城外的部队
        public void SendJoinRallyByHaveTroop(long rid,long armyIndex)
        {
            Rally_JoinRally.request req = new Rally_JoinRally.request();
            req.joinRid = rid;
            if (armyIndex != 0)
            {            
                req.armyIndex = armyIndex;
            }
            AppFacade.GetInstance().SendSproto(req);
        }


        //遣返集结部队
        public void SendReturnRally(long rid,string name)
        {
            Alert.CreateAlert(LanguageUtils.getTextFormat(730299,name))
                     .SetRightButton(null, LanguageUtils.getText(300013))
                     .SetLeftButton(() =>
                     {
                         Rally_RepatriationRally.request req = new Rally_RepatriationRally.request();
                         req.repatriationRid = rid;
                         AppFacade.GetInstance().SendSproto(req);
                     }, LanguageUtils.getText(300014))
                     .Show();
        }

        //解散集结部队
        public void SendDisbandRally()
        {
            Rally_DisbandRally.request req = new Rally_DisbandRally.request();
            AppFacade.GetInstance().SendSproto(req);
        }

        //增援部队
        public void SendReinforeRally(RallyTroopsData data)
        {
            if (data == null)
            {
                return;
            }

            Rally_ReinforceRally.request req = new Rally_ReinforceRally.request();
            req.reinforceObjectIndex = data.reinforceObjectIndex;
            req.mainHeroId = data.mainHeroId;
            req.deputyHeroId = data.viceId;
            req.soldiers = new Dictionary<long, SoldierInfo>();
            foreach (var soldier in data.soldierInfo)
            {
                req.soldiers.Add(soldier.Key, soldier.Value);
            }

           // req.armyIndex = data.armyObjectIndex;
            AppFacade.GetInstance().SendSproto(req);
        }

        //增援部队。 部队在城市外面
        public void SendReinforeRallyByArmyIndex(long reinforceObjectIndex, long armyObjectIndex, List<int> armyObjectIndexList = null)
        {
            Rally_ReinforceRally.request req = new Rally_ReinforceRally.request();
            req.reinforceObjectIndex = reinforceObjectIndex;

            List<long> armyIndexList = new List<long>();
            if (armyObjectIndex != 0)
            {
                armyIndexList.Add(armyObjectIndex);
            }            
            if (armyObjectIndexList != null)
            {
                foreach (var armyIndex in armyObjectIndexList)
                {
                    if (!armyIndexList.Contains(armyIndex))
                    {
                        armyIndexList.Add(armyIndex);
                    }                    
                }
            }
            req.armyIndexs = armyIndexList;

            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append("大世界行军 - 类型 ：增援");
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

            AppFacade.GetInstance().SendSproto(req);
        }


        #region 获取集结部队的位置
        public Vector2 GetRallyTroopMovePos(ItemWarData itemWarData)
        {
            Vector2 defPos= new Vector2(itemWarData.rallyDetailEntity.rallyPos.x,itemWarData.rallyDetailEntity.rallyPos.y);
            long startTimes = itemWarData.rallyDetailEntity.rallyWaitTime;
            long endTimes= itemWarData.rallyDetailEntity.rallyMarchTime;
            List<Vector2> path= new List<Vector2>();
            foreach (var info in itemWarData.rallyDetailEntity.rallyPath)
            {
                Vector2 v= new Vector2(info.x/100f,info.y/100);
                path.Add(v);
            }
            
            TroopHelp.CalcPosAndIndex(out var pos , out var index,0, defPos,path,startTimes,
                endTimes);
            Vector2 p= new Vector2(pos.x*100,pos.y*100);
            return p;
        }
        
        private Vector2 GetRallyTroopPos(long objectId,ItemWarData itemWarData)
        {
            float x = itemWarData.rallyDetailEntity.rallyPos.x;
            float y = itemWarData.rallyDetailEntity.rallyPos.y;
            Vector2 v2= new Vector2(x,y);
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)objectId);
            if (armyData != null)
            {
                if (armyData.go != null)
                {
                    v2= new Vector2(armyData.go.transform.position.x*100,armyData.go.transform.position.z*100);
                }
            }
            return v2;
        }


        public Vector2 GetRallyTroopPos(long objectId)
        {
            RallyDetailEntity rallyDetailEntity= this.GetRallyDetailEntityByObjectId(objectId);
            if (rallyDetailEntity != null)
            {
                ItemWarData itemWarData= new ItemWarData();
                itemWarData.rallyDetailEntity = rallyDetailEntity;
                return GetRallyTroopMovePos(itemWarData);
            }
            return Vector2.zero;
        }

        #endregion
    
        #endregion
        public void Clear()
        {
            rallyDetailEntityDic.Clear();
            rallyedDetailEntityDic.Clear();
            rallyedreinforceDetailEntityDic.Clear();
            rallyedrallyDetailEntityDic.Clear();
            joinRallyDetailEntityDic.Clear();
            rallyReinforceDetailEntityDic.Clear();
            rallyedCount = 0;
        }
    }
}