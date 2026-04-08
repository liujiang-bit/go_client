// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    AllianceProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client;
using Data;
using ILRuntime.Runtime.Debugger.Protocol;
using Skyunion;
using SprotoType;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public enum GuildGiftType
    {
        common = 0,
        uncommon
    }

//# 0 失效
//# 1 建造中
//# 2 正常
//# 3 燃烧中
//# 4 维修中
//# 5 战斗中
//# 6 采集中
    public enum GuildBuildState
    {
        miss = 0,
        building,
        normal,
        fire,
        fix,
        battle,
        collection
    }

    public enum GuildRoot
    {
        mail = 1,
        help,
        exit,
        chat,
        readyMail,
        online,
        createFlag,
        outfire,
        LvUpDown,
        invicePlayer,
        checkInvice,
        removeMember,
        createBuild,
        flag,
        editInfo,
        office,
        removeBuild,
        endGuild
    }

    public class AllianceMemberLevel
    {
        public List<GuildMemberInfoEntity> LevelMembers = new List<GuildMemberInfoEntity>();

        public int prefab_index;

        public bool isSelected = false;

        public int max = 0;

        public int num = 0;

        public AllianceMemberDefine data;

        public List<GuildMemberInfoEntity> subItemsData;
    }


    public class AllianceBuildArmyLevel
    {
        public int prefab_index;

        public bool isSelected = false;

        public bool isHolyLand = false; // 是否是圣地建筑

        public GuildBuildArmyInfoEntity LevelMember;

        public HolyLandArmyInfoEntity holyLandArmyInfoEntity;

        public long armyCount = 0;

        public List<SoldierInfo> subItemData;
    }

    public class ReinforceDetailItemData
    {
        public int prefab_index;

        public bool isSelected = false;

        public ReinforceArmyInfo LevelMember;

        public long armyCount = 0;

        public List<SoldierInfo> subItemData;
        public bool arrived = false; //已到达
    }

    public class AllianceBuildTypeTag
    {
        //固定
        public List<AllianceBuildingTypeDefine> LockList = new List<AllianceBuildingTypeDefine>();

        public Dictionary<long, GuildBuildInfo> TagFlagData;

        public int prefab_index;

        public int subItemRow = 0;

        public bool isSelected = false;

        public string HeadIcon;

        public string HeadTitle;


        public long max = 0;

        public long num = 0;

        public int buildType;

        public List<AllianceBuildingTypeDefine> subItemsData;


        public List<GuildBuildInfo> subFlagData;
    }


    public class AllianceProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "AllianceProxy";


        private ConfigDefine m_config;

        private List<AllianceLanguageSetDefine> m_lanSets;

        private List<AllianceMemberDefine> m_members;

        private List<AllianceMemberJurisdictionDefine> m_memberAccees;

        private Dictionary<int, AllianceMemberJurisdictionDefine> m_accessDic =
            new Dictionary<int, AllianceMemberJurisdictionDefine>();


        private List<AllianceOfficiallyDefine> m_offices;

        private List<AllianceSignDefine> m_sings;

        private Dictionary<int, AllianceSignDefine> m_signDic;

        private List<AllianceSignDefine> m_siginFlagSimple;

        private List<AllianceSignDefine> m_siginFlagSimpleColor;

        private List<AllianceSignDefine> m_siginFlag;

        private List<AllianceSignDefine> m_siginFlagColor;

        private PlayerProxy m_playerProxy;

        private AllianceResarchProxy m_resarchProxy;


        private GuildInfoEntity m_guildInfo;

        private List<GuildApplyInfoEntity> m_guildApplyInfos = new List<GuildApplyInfoEntity>();
        private Dictionary<long, GuildApplyInfoEntity> m_guildApplyDic = new Dictionary<long, GuildApplyInfoEntity>();


        private Dictionary<long, GuildMemberInfoEntity>
            m_guildMemberDic = new Dictionary<long, GuildMemberInfoEntity>();


        private Dictionary<long, long> m_guildMapObjIDTORID = new Dictionary<long, long>();

        private Dictionary<long, GuildOfficerInfoEntity>
            m_officersDic = new Dictionary<long, GuildOfficerInfoEntity>(4);

        private Dictionary<long, GuildOfficerInfoEntity> m_officersIDDic =
            new Dictionary<long, GuildOfficerInfoEntity>(4);

        private List<AllianceMemberLevel> m_guildMember = new List<AllianceMemberLevel>(4);

        private GuildMemberInfoEntity m_guildMaster;

        private Dictionary<long, AllianceMemberLevel> m_guildMemberLvDic = new Dictionary<long, AllianceMemberLevel>(5);

        #endregion

        
        // Use this for initialization
        public AllianceProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" AllianceProxy register");


            m_config = CoreUtils.dataService.QueryRecords<ConfigDefine>()[0];


            InitborderZone();
        //    Test();
        }

        public void Init()
        {
            m_resarchProxy = AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        public override void OnRemove()
        {
            Debug.Log(" AllianceProxy remove");
        }


        public ConfigDefine Config => m_config;


        public List<AllianceLanguageSetDefine> AllianceLanguageSetDefines()
        {
            if (m_lanSets == null)
            {
                m_lanSets = CoreUtils.dataService.QueryRecords<AllianceLanguageSetDefine>();
            }

            return m_lanSets;
        }

        public List<AllianceMemberDefine> AllianceMemberDefines()
        {
            if (m_members == null)
            {
                m_members = CoreUtils.dataService.QueryRecords<AllianceMemberDefine>();
            }

            return m_members;
        }

        public List<AllianceMemberJurisdictionDefine> AllianceMemberJurisdictionDefineDefines()
        {
            if (m_memberAccees == null)
            {
                m_memberAccees = CoreUtils.dataService.QueryRecords<AllianceMemberJurisdictionDefine>();


                for (int i = 0; i < m_memberAccees.Count; i++)
                {
                    var cdata = m_memberAccees[i];
                    m_accessDic.Add(cdata.type, cdata);
                }
            }

            return m_memberAccees;
        }

        public List<AllianceOfficiallyDefine> AllianceOfficiallyDefines()
        {
            if (m_offices == null)
            {
                m_offices = CoreUtils.dataService.QueryRecords<AllianceOfficiallyDefine>();
            }

            return m_offices;
        }

        public List<AllianceSignDefine> AllianceSignDefines()
        {
            if (m_sings == null)
            {
                m_sings = CoreUtils.dataService.QueryRecords<AllianceSignDefine>();
                m_signDic = new Dictionary<int, AllianceSignDefine>(m_sings.Count);

                for (int i = 0; i < m_sings.Count; i++)
                {
                    var s = m_sings[i];
                    m_signDic.Add(s.ID, s);
                }
            }

            return m_sings;
        }


        public List<AllianceSignDefine> SiginFlagSimples()
        {
            if (m_siginFlagSimple == null)
            {
                m_siginFlagSimple = new List<AllianceSignDefine>();

                var list = AllianceSignDefines();
                var len = list.Count;

                for (int i = 0; i < len; i++)
                {
                    var data = list[i];
                    if (data.type == 1)
                    {
                        m_siginFlagSimple.Add(data);
                    }
                }
            }

            return m_siginFlagSimple;
        }

        public List<AllianceSignDefine> SiginFlagSimpleColors()
        {
            if (m_siginFlagSimpleColor == null)
            {
                m_siginFlagSimpleColor = new List<AllianceSignDefine>();

                var list = AllianceSignDefines();
                var len = list.Count;

                for (int i = 0; i < len; i++)
                {
                    var data = list[i];
                    if (data.type == 2)
                    {
                        m_siginFlagSimpleColor.Add(data);
                    }
                }
            }

            return m_siginFlagSimpleColor;
        }

        public List<AllianceSignDefine> SiginFlags()
        {
            if (m_siginFlag == null)
            {
                m_siginFlag = new List<AllianceSignDefine>();

                var list = AllianceSignDefines();
                var len = list.Count;

                for (int i = 0; i < len; i++)
                {
                    var data = list[i];
                    if (data.type == 3)
                    {
                        m_siginFlag.Add(data);
                    }
                }
            }

            return m_siginFlag;
        }

        public List<AllianceSignDefine> SiginFlagColors()
        {
            if (m_siginFlagColor == null)
            {
                m_siginFlagColor = new List<AllianceSignDefine>();

                var list = AllianceSignDefines();
                var len = list.Count;

                for (int i = 0; i < len; i++)
                {
                    var data = list[i];
                    if (data.type == 4)
                    {
                        m_siginFlagColor.Add(data);
                    }
                }
            }

            return m_siginFlagColor;
        }

        // 获取默认的联盟旗帜标识
        public List<long> GetDefaultGuildFlagSign()
        {
            return new List<long>() {101, 201, 301, 401};
        }

        public AllianceSignDefine GetSignByID(int id)
        {
            if (m_signDic.Count == 0)
            {
                var data = SiginFlags();
            }

            if (m_signDic.ContainsKey(id))
            {
                return m_signDic[id];
            }

            return null;
        }

        public bool GetSelfRoot(GuildRoot type)
        {
            var p = m_playerProxy.CurrentRoleInfo.rid;

            var info = getMemberInfo(p);

            if (info != null)
            {
                return GetMemberRoot(info.guildJob, type);
            }

            return false;
        }

        //联盟权限
        public bool GetMemberRoot(long lv, GuildRoot type)
        {
            if (m_accessDic.Count == 0)
            {
                AllianceMemberJurisdictionDefineDefines();
            }

            AllianceMemberJurisdictionDefine root;
            if (m_accessDic.TryGetValue((int) type, out root))
            {
                if (lv == 5)
                {
                    return root.R5 > 0;
                }

                if (lv == 4)
                {
                    return root.R4 > 0;
                }

                if (lv == 3)
                {
                    return root.R3 > 0;
                }

                if (lv == 2)
                {
                    return root.R2 > 0;
                }

                if (lv == 1)
                {
                    return root.R1 > 0;
                }
            }

            return false;
        }

        /// <summary>
        /// 数据清理
        /// </summary>
        public void CleanAll()
        {
            //成员相关
            m_guildApplyInfos.Clear();
            m_guildMemberDic.Clear();
            m_officersDic.Clear();
            m_officersIDDic.Clear();
            m_guildMember.Clear();
            m_guildMemberLvDic.Clear();
            m_guildMapObjIDTORID.Clear();

            m_otherPlayerHelpCount = 0;
            
            m_guildInfo = null;

            m_guildRssFullCount = 0;

            CleanGifts();
            m_guildHolyLandInfos.Clear();
            m_holyLandArmsDic.Clear();
            m_holyLandArmsList.Clear();
            m_myArmyInHolyLandBuild.Clear();
            CleanDepot();
            CleanHelp();
            CleanTerritoryData();
        }

        #region 联盟信息

        public bool HasJionAlliance()
        {
            if (m_playerProxy.CurrentRoleInfo!=null && m_playerProxy.CurrentRoleInfo.guildId > 0)
            {
                return true;
            }

            return false;
        }

        public long GetAllianceId()
        {
            return m_playerProxy.CurrentRoleInfo.guildId;
        }

        public GuildInfoEntity GetAlliance()
        {
            return m_guildInfo;
        }

        //获取联盟简称
        public string GetAbbreviationName()
        {
            if (m_guildInfo != null)
            {
                return m_guildInfo.abbreviationName;
            }

            return "";
        }

        public string GetAllianceName()
        {
            if (m_guildInfo != null)
            {
                return m_guildInfo.name;
            }

            return "-";
        }


        public void UpdateGuildInfo(Guild_GuildInfo.request req)
        {
            if (m_guildInfo == null)
            {
                m_guildInfo = new GuildInfoEntity();
            }

            GuildInfoEntity.updateEntity(m_guildInfo, req.guildInfo);
            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceInfoUpdate, m_guildInfo);
        }

        #endregion

        #region 联盟成员相关

        public void UpdateGuildApplys(Guild_GuildApplys.request request)
        {
            Debug.Log("申请列表");

            if (request.HasDeleteRid)
            {
                GuildApplyInfoEntity infoEntity;
                if (m_guildApplyDic.TryGetValue(request.deleteRid, out infoEntity))
                {
                    m_guildApplyDic.Remove(infoEntity.rid);
                    m_guildApplyInfos.Remove(infoEntity);
                }
            }

            if (request.HasGuildApplys)
            {
                foreach (var apply in request.guildApplys)
                {
                    GuildApplyInfoEntity info;
                    long rid = apply.Value.rid;
                    if (!m_guildApplyDic.TryGetValue(rid, out info))
                    {
                        info = new GuildApplyInfoEntity();

                        m_guildApplyInfos.Add(info);
                        m_guildApplyDic.Add(rid, info);
                    }

                    GuildApplyInfoEntity.updateEntity(info, apply.Value);
                }
            }


            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceApplys, m_guildApplyInfos);
        }


        public GuildMemberInfoEntity getMemberByMapID(long mapObjID)
        {
            long rid = 0;
            if (m_guildMapObjIDTORID.TryGetValue(mapObjID, out rid))
            {
                GuildMemberInfoEntity member;
                if (m_guildMemberDic.TryGetValue(rid,out member))
                {
                    return member;
                }    
            }
            return null;
        }
        


        public void UpdateGuildMembers(Guild_GuildMemberInfo.request request)
        {
            getAllianceMembers();

            bool isSelfJobChange = false;

            if (request.HasGuildMembers)
            {
                foreach (var apply in request.guildMembers)
                {
                    var member = apply.Value;
                    long rid = member.rid;
                    GuildMemberInfoEntity info;

                    bool hasChangeLv = false;

                    long oldLv = 0;

                    if (!m_guildMemberDic.TryGetValue(rid, out info))
                    {
                        info = new GuildMemberInfoEntity();
                        m_guildMemberDic.Add(rid, info);
                        if (apply.Value.cityObjectIndex>0)
                        {
                            m_guildMapObjIDTORID.Add(apply.Value.cityObjectIndex,rid);
                        }
                        AddToMemberLv(member.guildJob, info);
                    }

                    //等级变化时候处理下
                    if (member.HasGuildJob)
                    {
                        if (info.guildJob > 0)
                        {
                            oldLv = info.guildJob;
                        }

                        //判断自己的职位是否发生了变更
                        if (info.guildJob != member.guildJob && member.rid == m_playerProxy.CurrentRoleInfo.rid)
                        {
                            isSelfJobChange = true;
                        }

                        if (member.guildJob != oldLv && oldLv > 0)
                        {
                            RemoveToMemberLv(oldLv, info);
                            AddToMemberLv(member.guildJob, info);
                        }
                    }

                    GuildMemberInfoEntity.updateEntity(info, member);

                    if (member.HasGuildJob)
                    {
                        //服务器没更新guildinfo，需要特殊处理下
                        if (info.guildJob == 5)
                        {
                            m_guildInfo.leaderName = info.name;
                            m_guildInfo.leaderRid = info.rid;
                        }
                    }
                }
            }

            if (request.HasDeleteRid)
            {
                GuildMemberInfoEntity infoEntity;
                if (m_guildMemberDic.TryGetValue(request.deleteRid, out infoEntity))
                {
                    m_guildMemberDic.Remove(infoEntity.rid);
                    m_guildMapObjIDTORID.Remove(infoEntity.cityObjectIndex);
                    RemoveToMemberLv(infoEntity.guildJob, infoEntity);
                }
            }

            if (request.HasGuildOfficers)
            {
                foreach (var valuePair in request.guildOfficers)
                {
                    long officerId = valuePair.Value.officerId;
                    GuildOfficerInfoEntity info;
                    if (!m_officersIDDic.TryGetValue(officerId, out info))
                    {
                        info = new GuildOfficerInfoEntity();

                        m_officersIDDic.Add(officerId, info);
                    }

                    GuildOfficerInfoEntity.updateEntity(info, valuePair.Value);
                }
                m_officersDic.Clear();
                foreach (var kv in m_officersIDDic)
                {
                    if (kv.Value.rid > 0)
                    {
                        m_officersDic.Add(kv.Value.rid, kv.Value);
                    }
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.GuildOfficerInfoChange, getMemberOfficer(m_playerProxy.CurrentRoleInfo.rid));
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceMembers, m_guildApplyInfos);

            if (isSelfJobChange)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildCanCreateCheck);
            }
        }

        public GuildMemberInfoEntity getMemberInfo(long rid)
        {
            GuildMemberInfoEntity info;
            if (!m_guildMemberDic.TryGetValue(rid, out info))
            {
            }

            return info;
        }

        public int GetMemberCount()
        {
            return m_guildMemberDic.Count;
        }

        public Dictionary<long, GuildMemberInfoEntity> GetAllMemberDic()
        {
            return m_guildMemberDic;
        }

        public AllianceOfficiallyDefine getMemberOfficerConfig(long id)
        {
            var info = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>((int) id);

            return info;
        }


        public GuildOfficerInfoEntity getMemberOfficer(long rid)
        {
            GuildOfficerInfoEntity info;
            m_officersDic.TryGetValue(rid, out info);
            return info;
        }

        public GuildOfficerInfoEntity getMemberByOfficerID(long offierID)
        {
            GuildOfficerInfoEntity info;
            m_officersIDDic.TryGetValue(offierID, out info);
            return info;
        }

        public long getMemberByOfficerIDCDEnd(long offierID)
        {
            GuildOfficerInfoEntity info;
            if (m_officersIDDic.TryGetValue(offierID, out info))
            {
                long time = (info.appointTime + 10 * 60) - ServerTimeModule.Instance.GetServerTime();
                if (time > 0)
                {
                    return time;
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }


        public void AddToMemberLv(long guildJob, GuildMemberInfoEntity infoEntity)
        {
            AllianceMemberLevel lv;
            if (m_guildMemberLvDic.TryGetValue(guildJob, out lv))
            {
                lv.LevelMembers.Add(infoEntity);
            }
        }

        public void RemoveToMemberLv(long guildJob, GuildMemberInfoEntity infoEntity)
        {
            AllianceMemberLevel lv;
            if (m_guildMemberLvDic.TryGetValue(guildJob, out lv))
            {
                lv.LevelMembers.Remove(infoEntity);
            }
        }


        public List<AllianceMemberLevel> getAllianceMembers()
        {
            if (m_guildMember.Count == 0)
            {
                var members = AllianceMemberDefines();

                if (members.Count > 0 && members[0].lv == 1)
                {
                    members.Reverse();
                }

                foreach (var member in members)
                {
                    var ml = new AllianceMemberLevel();

                    ml.data = member;
                    ml.num = 0;
                    ml.prefab_index = 0;

                    if (member.lv == 4)
                    {
                        ml.isSelected = true;
                    }

                    if (member.lv < 5)
                    {
                        m_guildMember.Add(ml);
                    }

                    m_guildMemberLvDic.Add(member.lv, ml);
                }
            }

            return m_guildMember;
        }

        public Dictionary<long, AllianceMemberLevel> GetAllianceLvMembers()
        {
            return m_guildMemberLvDic;
        }

        private void RemoveGuildApplys(GuildApplyInfoEntity infoEntity)
        {
            m_guildApplyDic.Remove(infoEntity.rid);
            m_guildApplyInfos.Remove(infoEntity);
        }

        public List<GuildApplyInfoEntity> getGuildApplys()
        {
            return m_guildApplyInfos;
        }

        #endregion

        #region 联盟仓库相关

        private Dictionary<long, GuildCurrencyInfoEntity> m_depotCurrencyDic =
            new Dictionary<long, GuildCurrencyInfoEntity>();

        private List<GuildConsumeRecordInfo> m_depotHistory = new List<GuildConsumeRecordInfo>();


        public void CleanDepot()
        {
            m_depotCurrencyDic.Clear();
            m_depotHistory.Clear();
        }

        public void UpdateGuildDepotInfo(Guild_GuildDepotInfo.request request)
        {
            if (request.HasGuildDepot)
            {
                if (request.guildDepot.HasCurrencies)
                {
                    foreach (var valuePair in request.guildDepot.currencies)
                    {
                        long rid = valuePair.Value.type;
                        GuildCurrencyInfoEntity info;
                        if (!m_depotCurrencyDic.TryGetValue(rid, out info))
                        {
                            info = new GuildCurrencyInfoEntity();
                            m_depotCurrencyDic.Add(rid, info);
                        }

                        GuildCurrencyInfoEntity.updateEntity(info, valuePair.Value);
                    }
                }

                if (request.guildDepot.HasConsumeRecords)
                {
                    m_depotHistory = request.guildDepot.consumeRecords;
                    m_depotHistory.Reverse();
                    Debug.Log("更新仓库历史记录" + m_depotHistory.Count);
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceDepot);
            }
        }

        public long GetDepotCurrency_AlliancePointNum()
        {
            var dic = GetDepotCurrencyInfoEntities();
            if (dic.ContainsKey(107L))
                return dic[107L].num;
            return 0;
        }

        public long GetCurrencyByType(long id)
        {
            if (m_depotCurrencyDic.ContainsKey(id))
            {
                return m_depotCurrencyDic[id].num;
            }
            return 0;
        }

        public Dictionary<long, GuildCurrencyInfoEntity> GetDepotCurrencyInfoEntities()
        {
            return m_depotCurrencyDic;
        }

        public  long GetCurrenc(GuildCurrencyInfoEntity infoEntity)
        {
            float produceSec = (float)infoEntity.produce / 3600;

            long costTime = ServerTimeModule.Instance.GetServerTime() - infoEntity.lastProduceTime;

            long rss = infoEntity.num + (long)(produceSec * costTime);

            if (infoEntity.limit > 0 && rss > infoEntity.limit)
            {
                rss = infoEntity.limit;
            }
            return rss;
        }
        public List<GuildConsumeRecordInfo> GetDepotHistory()
        {
            return m_depotHistory;
        }

        public void SendReqDepotList()
        {
            Guild_GetGuildDepot.request request = new Guild_GetGuildDepot.request();

            AppFacade.GetInstance().SendSproto(request);
        }

        #endregion


        #region 联盟帮助

        private Dictionary<long, GuildRequestHelpInfoEntity> m_reqHelps =
            new Dictionary<long, GuildRequestHelpInfoEntity>();


        private List<GuildRequestHelpInfoEntity> m_reqList = new List<GuildRequestHelpInfoEntity>();

        private int m_otherPlayerHelpCount = 0;


        public void CleanHelp()
        {
            m_reqHelps.Clear();
            m_reqList.Clear();
        }

        public void UpdateHelps(Guild_GuildRequestHelps.request request)
        {
            if (request.HasGuildRequestHelps)
            {
                foreach (var valuePair in request.guildRequestHelps)
                {
                    long index = valuePair.Value.index;
                    long rid = valuePair.Value.rid;
                    GuildRequestHelpInfoEntity info;
                    if (!m_reqHelps.TryGetValue(index, out info))
                    {
                        info = new GuildRequestHelpInfoEntity();
                        m_reqHelps.Add(index, info);

                        if (rid == m_playerProxy.CurrentRoleInfo.rid)
                        {
                            m_reqList.Insert(0, info);
                        }
                        else
                        {
                            m_reqList.Add(info);
                            m_otherPlayerHelpCount++;
                            Debug.Log("帮助增加" + m_otherPlayerHelpCount);
                        }
                    }

                    GuildRequestHelpInfoEntity.updateEntity(info, valuePair.Value);
                }
            }

            if (request.HasDeleteHelpIndexs)
            {
                foreach (var index in request.deleteHelpIndexs)
                {
                    GuildRequestHelpInfoEntity infoEntity;

                    if (m_reqHelps.TryGetValue(index, out infoEntity))
                    {
                        if (infoEntity.rid != m_playerProxy.CurrentRoleInfo.rid)
                        {
                            m_otherPlayerHelpCount--;
                            Debug.Log("帮助减少" + m_otherPlayerHelpCount);
                        }

                        m_reqList.Remove(infoEntity);
                        m_reqHelps.Remove(index);
                    }
                }
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceHelp, m_guildApplyInfos);
        }

        public bool canHelpOhter()
        {
            return m_otherPlayerHelpCount > 0;
        }

        public int RedHelp()
        {
            return m_otherPlayerHelpCount;
        }

        public List<GuildRequestHelpInfoEntity> getHelps()
        {
            return m_reqList;
        }

        public void HelpsRemoveAll()
        {
            int len = m_reqList.Count;

            List<GuildRequestHelpInfoEntity> remove = new List<GuildRequestHelpInfoEntity>();
            for (int i = 0; i < len; i++)
            {
                var data = m_reqList[i];

                if (data.rid != m_playerProxy.CurrentRoleInfo.rid)
                {
                    remove.Add(data);
                }
            }

            len = remove.Count;
            for (int i = 0; i < len; i++)
            {
                var data = remove[i];
                m_reqList.Remove(data);

                m_reqHelps.Remove(data.index);
            }

            m_otherPlayerHelpCount = 0;
//            Debug.Log("帮助减少"+m_otherPlayerHelpCount);
        }

        //发送联盟求助#求助类型 1 建筑建造升级 2 治疗 3 科技升级
        //requestType = 1时，queueIndex为建筑队列索引
        public void SendRequestHelp(long type, long queueIndex)
        {
            if (m_playerProxy.CurrentRoleInfo.buildQueue != null)
            {
                foreach (var item in m_playerProxy.CurrentRoleInfo.buildQueue)
                {
                    QueueInfo queueInfo = item.Value;
                    if (!item.Value.requestGuildHelp && item.Value.finishTime > 0)
                    {
                        Guild_SendRequestHelp.request request = new Guild_SendRequestHelp.request();

                        request.queueIndex = item.Key;
                        request.requestType = 1;
                        AppFacade.GetInstance().SendSproto(request);

                    }
                }
            }
            if (m_playerProxy.CurrentRoleInfo.technologyQueue != null)
            {
                    QueueInfo queueInfo = m_playerProxy.CurrentRoleInfo.technologyQueue;
                    if (!queueInfo.requestGuildHelp && queueInfo.finishTime > 0)
                    {
                        Guild_SendRequestHelp.request request = new Guild_SendRequestHelp.request();

                        request.queueIndex = queueInfo.queueIndex;
                        request.requestType = 3;
                    AppFacade.GetInstance().SendSproto(request);

                }
            }

            if (m_playerProxy.CurrentRoleInfo.treatmentQueue != null)
            {
                QueueInfo queueInfo = m_playerProxy.CurrentRoleInfo.treatmentQueue;
                if (!queueInfo.requestGuildHelp && queueInfo.finishTime > 0 && queueInfo.finishTime > ServerTimeModule.Instance.GetServerTime())
                {
                    Guild_SendRequestHelp.request request = new Guild_SendRequestHelp.request();

                    request.queueIndex = queueInfo.queueIndex;
                    request.requestType = 2;
                    AppFacade.GetInstance().SendSproto(request);
                }
            }
            //已向全体联盟成员发送了帮助请求
            Tip.CreateTip(730001, Tip.TipStyle.AllianceHelp).SetHelpType(Tip.AllianceHelpType.Self).Show();
        }

        //获取联盟求助信息
        public void SendGetRequestHelps()
        {
            Guild_GetGuildRequestHelps.request request = new Guild_GetGuildRequestHelps.request();

            AppFacade.GetInstance().SendSproto(request);
        }


        public void SendHelpGuildMembers()
        {
            Guild_HelpGuildMembers.request request = new Guild_HelpGuildMembers.request();

            AppFacade.GetInstance().SendSproto(request);
        }

        #endregion

        #region 联盟圣地相关

        Dictionary<int, GuildHolyLandInfo> m_guildHolyLandInfos = new Dictionary<int, GuildHolyLandInfo>();
        List<StrongHoldDataDefine> m_strongHoldDataDefines = new List<StrongHoldDataDefine>();

        Dictionary<int,List<int>> m_acceptableZones = new Dictionary<int, List<int>>();//可到达的
        Dictionary<int,List<int>> m_borderZones = new Dictionary<int, List<int>>();//接壤的
        private Dictionary<long, AllianceBuildArmyLevel>
            m_holyLandArmsDic = new Dictionary<long, AllianceBuildArmyLevel>();

        private Dictionary<long, List<AllianceBuildArmyLevel>> m_holyLandArmsList =
            new Dictionary<long, List<AllianceBuildArmyLevel>>();

        private Dictionary<long, int> m_myArmyInHolyLandBuild = new Dictionary<long, int>();

        public Dictionary<int, GuildHolyLandInfo> GetGuildHolyLandInfos()
        {
            return m_guildHolyLandInfos;
        }

        /// <summary>
        /// 是否拥有某个圣地
        /// </summary>
        /// <returns></returns>
        public bool HasGuildHolyLand(int strongHolyId)
        {
            return m_guildHolyLandInfos.ContainsKey(strongHolyId);
        }
        /// <summary>
        /// 省份是联通的
        /// </summary>
        /// <param name="startZone"></param>
        /// <param name="EndZone"></param>
        /// <returns></returns>
        public bool IsAcceptableZone(int startZone, int EndZone)
        {
            bool acceptable = false;
            if (startZone == EndZone)
            {
                acceptable = true;
            }
            else
            {
                if (m_acceptableZones.ContainsKey(startZone))
                {
                    if (m_acceptableZones[startZone].Contains(EndZone))
                    {
                        acceptable = true;
                    }
                }
            }
            return acceptable;
        }
        public void UpdateAcceptableZone()
        {
            ResetAcceptableZoneData();
            foreach (var zone in m_acceptableZones.Keys)
            {
                Acceptable(zone,zone);
            }
        }
        private void Acceptable(int  zone, int zone1)
        {
            List<int> borderZone = m_borderZones[zone1];

            for (int i = 0; i < borderZone.Count; i++)
            {
                int HolyLandId = borderZone[i];
                StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>(HolyLandId);
                if (strongHoldDataDefine != null)
                {
                    if (m_guildHolyLandInfos.ContainsKey(HolyLandId))
                    {
                        int province = 0;
                        if (strongHoldDataDefine.province1 == zone1)
                        {
                            province = strongHoldDataDefine.province2;
                        }
                        else
                        {
                            province = strongHoldDataDefine.province1;
                        }
                        if (!m_acceptableZones[zone].Contains(province)&& province!= zone)
                        {
                            m_acceptableZones[zone].Add(province);
                            
                                Acceptable(zone, province);
                            
                        }
                    }
                }
            }
        }
        private void ResetAcceptableZoneData()
        {
            m_acceptableZones[1] = new List<int>();
            m_acceptableZones[2] = new List<int>();
            m_acceptableZones[3] = new List<int>();
            m_acceptableZones[4] = new List<int>();
            m_acceptableZones[5] = new List<int>();
            m_acceptableZones[6] = new List<int>();
            m_acceptableZones[7] = new List<int>();
            m_acceptableZones[8] = new List<int>();
            m_acceptableZones[9] = new List<int>();
            m_acceptableZones[10] = new List<int>();

        }
        /// <summary>
        /// 初始化接壤区域
        /// </summary>
        private void InitborderZone()
        {
            if (m_strongHoldDataDefines.Count == 0)
            {
                m_strongHoldDataDefines = CoreUtils.dataService.QueryRecords<StrongHoldDataDefine>();
            }
            foreach (var strongHoldDataDefine in m_strongHoldDataDefines)
            {
                if (strongHoldDataDefine.ID > 90000)
                {
                    List<int> province1 = new List<int>();
                    List<int> province2 = new List<int>();
                    if (m_borderZones.TryGetValue(strongHoldDataDefine.province1, out province1))
                    {
                        province1.Add(strongHoldDataDefine.ID);
                    }
                    else
                    {
                        province1 = new List<int>();
                        province1.Add(strongHoldDataDefine.ID);
                        m_borderZones[strongHoldDataDefine.province1] = province1;
                    }
                    if (m_borderZones.TryGetValue(strongHoldDataDefine.province2, out province2))
                    {
                        province2.Add(strongHoldDataDefine.ID);
                    }
                    else
                    {
                        province2 = new List<int>();
                        province2.Add(strongHoldDataDefine.ID);
                        m_borderZones[strongHoldDataDefine.province2] = province2;
                    }
                }
            }
        }
        public void Test()
        {
 
            m_strongHoldDataDefines = CoreUtils.dataService.QueryRecords<StrongHoldDataDefine>();
            for (int i = 0; i < m_strongHoldDataDefines.Count; i++)
            {
                StrongHoldDataDefine strongHoldDataDefine = m_strongHoldDataDefines[i];
                if (strongHoldDataDefine.type > 10000)
                {
                    if (
                      //  strongHoldDataDefine.ID == 90001||
            //    strongHoldDataDefine.ID == 90002||
                strongHoldDataDefine.ID == 90003||
                 strongHoldDataDefine.ID == 90004||
                 strongHoldDataDefine.ID == 90005||
                strongHoldDataDefine.ID == 90015||
                  strongHoldDataDefine.ID == 90016||
                  strongHoldDataDefine.ID == 90017||
                  strongHoldDataDefine.ID == 90018||
                  strongHoldDataDefine.ID == 90041
               )
                    {
                        //    continue;
                        //}
                        GuildHolyLandInfo guildHolyLandInfo = new GuildHolyLandInfo();
                        guildHolyLandInfo.strongHoldId = strongHoldDataDefine.ID;
                        if (!m_guildHolyLandInfos.ContainsKey(strongHoldDataDefine.ID))
                        {
                            m_guildHolyLandInfos.Add(strongHoldDataDefine.ID, guildHolyLandInfo);
                        }
                    }
                }
            }
            UpdateAcceptableZone();
        }

        public void UpdateGuildHolyLandInfo(Guild_GuildHolyLands.request req)
        {
            if (req.HasGuildHolyLands)
            {
                foreach (var dict in req.guildHolyLands)
                {
                    GuildHolyLandInfo info = dict.Value;

                    int strongHoldId = (int) info.strongHoldId;
                    if (!m_guildHolyLandInfos.ContainsKey(strongHoldId))
                    {
                        CoreUtils.logService.Warn("新推送的圣地:[" + info.strongHoldId);
                        m_guildHolyLandInfos.Add(strongHoldId, info);
                    }
                    else
                    {
                        CoreUtils.logService.Warn($"收到了相同的圣地:[{info.strongHoldId}]   确认是要展示相同的圣地吗?");
                    }
                }
            }


            if (req.HasDeleteStrongHoldId && m_guildHolyLandInfos.ContainsKey((int) req.deleteStrongHoldId))
            {
                m_guildHolyLandInfos.Remove((int) req.deleteStrongHoldId);
                CoreUtils.logService.Warn($" 删除圣地  req.deleteStrongHoldId:[{req.deleteStrongHoldId}]");
            }

            UpdateAcceptableZone();
            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceHolyLandUpdate);
        }

        /// <summary>
        /// 更新圣地联盟建筑数据
        /// </summary>
        /// <param name="request"></param>
        public void UpdateHolyLandArmys(Map_HolyLandArmys.request request)
        {
            var buildObjectID = request.objectIndex;

            if (request.HasLeaderBuildArmyIndex) 
            {
                var leaderBuildArmyIndex = request.leaderBuildArmyIndex;
                if (m_objectLeader.ContainsKey(buildObjectID))
                {
                    m_objectLeader[buildObjectID] = leaderBuildArmyIndex;
                }
                else
                {
                    m_objectLeader.Add(buildObjectID, leaderBuildArmyIndex);
                }
            }            

            if (request.HasArmyList)
            {
                foreach (var valuePair in request.armyList)
                {
                    long armyIndex = valuePair.Value.buildArmyIndex;
                    AllianceBuildArmyLevel armyInfo;

                    bool isNew = false;
                    //部队唯一映射
                    if (!m_holyLandArmsDic.TryGetValue(armyIndex, out armyInfo))
                    {
                        armyInfo = new AllianceBuildArmyLevel();
                        armyInfo.holyLandArmyInfoEntity = new HolyLandArmyInfoEntity();

                        armyInfo.prefab_index = 1;
                        armyInfo.isHolyLand = true;

                        m_holyLandArmsDic.Add(armyIndex, armyInfo);
                        isNew = true;
                    }

                    List<AllianceBuildArmyLevel> armysList;

                    //建筑唯一映射
                    if (!m_holyLandArmsList.TryGetValue(buildObjectID, out armysList))
                    {
                        armysList = new List<AllianceBuildArmyLevel>();
                        m_holyLandArmsList.Add(buildObjectID, armysList);
                    }


                    HolyLandArmyInfoEntity.updateEntity(armyInfo.holyLandArmyInfoEntity, valuePair.Value);
                    armyInfo.armyCount = CountSoldiers(armyInfo.holyLandArmyInfoEntity.soldiers);
                    if (isNew)
                    {
                        armysList.Add(armyInfo);

                        armysList.Sort(((level, armyLevel) =>
                            level.holyLandArmyInfoEntity.arrivalTime.CompareTo(armyLevel.holyLandArmyInfoEntity
                                .arrivalTime)));
                       
                        if (armyInfo.holyLandArmyInfoEntity.rid == m_playerProxy.CurrentRoleInfo.rid)
                        {
                            if (!m_myArmyInHolyLandBuild.ContainsKey(buildObjectID))
                            {
                                m_myArmyInHolyLandBuild.Add(buildObjectID, 0);
                            }

                            m_myArmyInHolyLandBuild[buildObjectID]++;
                        }
                    }
                }
            }

            if (request.HasDeleteBuildArmyIndexs)
            {
                var delList = request.deleteBuildArmyIndexs;
                int len = delList.Count;
                for (int i = 0; i < len; i++)
                {
                    var armyIndex = delList[i];
                    AllianceBuildArmyLevel delArmy;

                    if (m_holyLandArmsDic.TryGetValue(armyIndex, out delArmy))
                    {
                        m_holyLandArmsDic.Remove(armyIndex);
                        List<AllianceBuildArmyLevel> armyInfoEntities;
                        if (m_holyLandArmsList.TryGetValue(buildObjectID, out armyInfoEntities))
                        {
                            armyInfoEntities.Remove(delArmy);

                            if (m_myArmyInHolyLandBuild.ContainsKey(buildObjectID))
                            {
                                m_myArmyInHolyLandBuild[buildObjectID]--;
                            }
                        }
                    }
                }
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildArmyUpdate, buildObjectID);
        }

        public List<AllianceBuildArmyLevel> GetBuildArmyInHolyLand(long objectID)
        {
            List<AllianceBuildArmyLevel> armyInfoEntities;
            if (m_holyLandArmsList.TryGetValue(objectID, out armyInfoEntities))
            {
            }

            return armyInfoEntities;
        }

        /// <summary>
        /// 获取我方部队在圣地的数量
        /// </summary>
        /// <param name="buildID"></param>
        /// <returns></returns>
        public int GetMyArmyCountInHolyLand(long buildID)
        {
            if (m_myArmyInHolyLandBuild.ContainsKey(buildID))
            {
                return m_myArmyInHolyLandBuild[buildID];
            }
            
            return 0;
        }

        // 清空圣地信息数据
        public void CleanHolyLandInfos()
        {
            m_guildHolyLandInfos.Clear();
        }

        #endregion


        #region 领地相关 要塞  旗帜  资源中心


        public bool HasGuildBuilding()
        {
            GuildBuildInfoEntity buidInfo1 = GetFortressesByType(1);
            GuildBuildInfoEntity buidInfo2 = GetFortressesByType(2);
            GuildBuildInfoEntity buidInfo3 = GetFortressesByType(12);

            var hasBuilding = buidInfo1 != null && buidInfo1.status == (int) GuildBuildState.building
                              || buidInfo2 != null && buidInfo2.status == (int) GuildBuildState.building ||
                              buidInfo3 != null && buidInfo3.status == (int) GuildBuildState.building;
            if (hasBuilding)
            {
                return true;
            }
            
            GuildBuildInfo buidResInfo1 = GetResCenter().resourceCenter;
            hasBuilding = buidResInfo1 != null && buidResInfo1.status == (int) GuildBuildState.building;
            if (hasBuilding)
            {
                return true;
            }
            
            
            m_flags.flags.Values.ToList().ForEach((build) =>
            {
                if (build.status == (int)GuildBuildState.building)
                {
                    hasBuilding = true;
                }
            });
            
            
            return hasBuilding;
        }


        public static void SetUIFlag(List<long> guildFlagSigns, GameObject go)
        {
            var flagTransform = go.transform.Find("flag");
            if (flagTransform != null)
            {
                var flagSpriteRenderer = flagTransform.gameObject.GetComponent<PolygonImage>();

                var flagDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[0]);
                var flagColorDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[1]);

                ClientUtils.LoadSprite(flagSpriteRenderer, flagDefine.realityIcon);
                ClientUtils.ImageSetColor(flagSpriteRenderer, flagColorDefine.colour);
            }

            var logoTransform = go.transform.Find("logo");

            if (logoTransform != null)
            {
                var logoSpriteRenderer = logoTransform.gameObject.GetComponent<PolygonImage>();

                var logoDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[2]);
                var logoColorDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[3]);

                ClientUtils.LoadSprite(logoSpriteRenderer, logoDefine.realityIcon);
                ClientUtils.ImageSetColor(logoSpriteRenderer, logoColorDefine.colour);
            }
        }

        private List<AllianceBuildTypeTag> m_territoryUIList = new List<AllianceBuildTypeTag>(8);
        private Dictionary<int, AllianceBuildTypeTag> m_territoryUIDic = new Dictionary<int, AllianceBuildTypeTag>(4);

        private Dictionary<long, GuildBuildInfoEntity> m_fortressesBuildsDic =
            new Dictionary<long, GuildBuildInfoEntity>();

        private Dictionary<long, GuildBuildInfoEntity> m_fortressesTypeBuildsDic =
            new Dictionary<long, GuildBuildInfoEntity>();

        private Dictionary<long, RoleTerritoryGainInfoEntity> m_roleTerritoryGainsDic =
            new Dictionary<long, RoleTerritoryGainInfoEntity>();

        private GuildResourceCenterInfoEntity m_resCenter = new GuildResourceCenterInfoEntity();

        private GuildFlagInfoEntity m_flags = new GuildFlagInfoEntity();

        //联盟资源点信息
        private GuildResourcePointInfoEntity m_resPoint = new GuildResourcePointInfoEntity();

        //上次领取领土收益时间
        private long lastTakeGainTime = 0;

        public List<AllianceBuildTypeTag> GetTerritoryUIList()
        {
            if (m_territoryUIList.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    var tag = new AllianceBuildTypeTag();

                    tag.HeadTitle = LanguageUtils.getText(RS.AllianceTerrtriyTagTitle[i]);

                    tag.HeadIcon = RS.AllianceBuildTypeTag[i];

                    if (i == 0)
                    {
                        tag.isSelected = true;
                    }

                    tag.prefab_index = 0;

                    tag.buildType = i;

                    m_territoryUIList.Add(tag);
                    m_territoryUIDic.Add(i, tag);
                }

                var clist = CoreUtils.dataService.QueryRecords<AllianceBuildingTypeDefine>();

                for (int i = 0; i < clist.Count; i++)
                {
                    var cinfo = clist[i];


                    if (cinfo.imgShowIndex < 4)
                    {
                        m_territoryUIList[cinfo.imgShowIndex].LockList.Add(cinfo);
                    }
                }
            }

            var st = m_territoryUIList[0];

            st.num = m_fortressesBuildsDic.Count;
            st.max = 3;

            var res = m_territoryUIList[1];

            res.num = m_resCenter.resourceCenter == null ? 0 : 1;
            res.max = 4;

            var flagTag = m_territoryUIList[2];
            flagTag.num = GetFlagInfoEntity().flagNum;
            flagTag.max = GetFlagInfoEntity().flagLimit;
            flagTag.TagFlagData = m_flags.flags;

            var resTag = m_territoryUIList[3];
            resTag.num = GetResPointCount();

            if (flagTag.TagFlagData == null)
            {
                flagTag.TagFlagData = new Dictionary<long, GuildBuildInfo>();
            }


            return m_territoryUIList;
        }


        public void CleanTerritoryData()
        {
            m_territoryUIList.Clear();

            m_territoryUIDic.Clear();

            m_fortressesBuildsDic.Clear();
            m_fortressesTypeBuildsDic.Clear();

            m_resCenter = new GuildResourceCenterInfoEntity();

            m_flags = new GuildFlagInfoEntity();

            m_resPoint = new GuildResourcePointInfoEntity();

            m_roleTerritoryGainsDic.Clear();
            lastTakeGainTime = 0;
        }

        public void UpdateBuilds(Guild_GuildBuilds.request request)
        {
            Debug.Log("更新联盟建筑" + request.deleteBuildIndex);
            if (request.HasGuildFortresses)
            {
                foreach (var valuePair in request.guildFortresses)
                {
                    long index = valuePair.Value.buildIndex;
                    GuildBuildInfoEntity info;
                    bool isNew = false;
                    if (!m_fortressesBuildsDic.TryGetValue(index, out info))
                    {
                        info = new GuildBuildInfoEntity();
                        m_fortressesBuildsDic.Add(index, info);
                        isNew = true;
                    }

                    GuildBuildInfoEntity.updateEntity(info, valuePair.Value);

                    if (isNew)
                    {
                        //TODO 旗帜重复
                        if (!m_fortressesTypeBuildsDic.ContainsKey(info.type))
                        {
                            m_fortressesTypeBuildsDic.Add(info.type, info);
                        }
                    }
                }
            }

            if (request.HasGuildResourceCenter)
            {
                GuildResourceCenterInfoEntity.updateEntity(m_resCenter, request.guildResourceCenter);
            }

            if (request.HasGuildFlags)
            {
                GuildFlagInfoEntity.updateEntity(m_flags, request.guildFlags);
                Debug.Log(m_flags.flagNum+" 更新旗帜数量"+m_flags.flags.Count+"  "+m_flags.flagLimit);
            }

            if (request.HasGuildResourcePoint)
            {
                GuildResourcePointInfoEntity.updateEntity(m_resPoint, request.guildResourcePoint);
                CheckResRedPoint();
            }

            if (request.HasRoleTerritoryGains)
            {
                foreach (var valuePair in request.roleTerritoryGains)
                {
                    long index = valuePair.Value.type;
                    RoleTerritoryGainInfoEntity info;
                    bool isNew = false;
                    if (!m_roleTerritoryGainsDic.TryGetValue(index, out info))
                    {
                        info = new RoleTerritoryGainInfoEntity();
                        m_roleTerritoryGainsDic.Add(index, info);
                        isNew = true;
                    }

                    RoleTerritoryGainInfoEntity.updateEntity(info, valuePair.Value);
                }
                
                CheckResRedPoint();
            }

            if (request.HasLastTakeGainTime)
            {
                lastTakeGainTime = request.lastTakeGainTime;
            }

            if (request.HasDeleteBuildIndex)
            {
                long index = request.deleteBuildIndex;
                GuildBuildInfoEntity info;
                bool isNew = false;
                if (m_fortressesBuildsDic.TryGetValue(index, out info))
                {
                    m_fortressesBuildsDic.Remove(index);
                    m_fortressesTypeBuildsDic.Remove(info.type);
                    
                }
                else
                {
                    Debug.Log("移除建筑" + index);
                    GuildBuildInfo flaginfo;
                    if (m_flags.flags.TryGetValue(index, out flaginfo))
                    {
                        m_flags.flags.Remove(index);
                    }
                    else if(m_resCenter!=null && (m_resCenter.resourceCenter==null ||  m_resCenter.resourceCenter!=null && m_resCenter.resourceCenter.buildIndex == index))
                    {
                        m_resCenter = new GuildResourceCenterInfoEntity();
                    }
                }
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildUpdate, request.reqType);

            if (request.HasReqType && (request.reqType >0))
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildCanCreateCheck);
            }
        }


        public void CheckResRedPoint()
        {
            m_guildRssFullCount = 0;

            float minTime = float.MaxValue;
            for (int i = 0; i < RS.GuildCurrencyIDs.Length; i++)
            {
                var gaindata = GetTerritoryGainByType(RS.GuildCurrencyIDs[i]);
                var resPointConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(RS.GuildResPointIDs[i]);


                long resCount = 0;

                if (gaindata!=null && gaindata.limit>0)
                {
                    if (gaindata.num>=gaindata.limit && gaindata.limit>0)
                    {

                        m_guildRssFullCount++;
                        break;
                    }
                    else
                    {
                        //点的数量*

                        float baseRssSpeed = resPointConfig.holdPersonSpeed;
                        
                        long time = ServerTimeModule.Instance.GetServerTime() - gaindata.territoryTime;
                        float speed = resPointConfig.holdPersonSpeed * (time / 3600f);

                        float secAllSpeed = 0;

                        switch (i)
                        {
                            case 0:
                                resCount = (long)(m_resPoint.foodPoint * speed);
                                
                                secAllSpeed = (long)(m_resPoint.foodPoint * baseRssSpeed);
                                break;
                            case 1:
                                resCount = (long)(m_resPoint.woodPoint * speed);
                                secAllSpeed = (long)(m_resPoint.woodPoint * baseRssSpeed);
                                break;
                            case 2:
                                resCount = (long)(m_resPoint.stonePoint * speed);
                                secAllSpeed = (long)(m_resPoint.stonePoint * baseRssSpeed);
                                break;
                            case 3:
                                resCount = (long)(m_resPoint.goldPoint * speed);
                                secAllSpeed = (long)(m_resPoint.goldPoint * baseRssSpeed);
                                break;
                        }

                        //剩余多少时间
                        long remainTime = 0;

                        if (secAllSpeed>0)
                        {
                            remainTime = (gaindata.limit - gaindata.num) / (long)secAllSpeed;
                        }
                        
                        if (resCount>gaindata.limit&& gaindata.limit>0)
                        {
                            m_guildRssFullCount++;
                        }
                        else
                        {
                            minTime = Mathf.Min((float)minTime, (float)remainTime);
                        }
                    }
                }
            }

            if (m_guildRssFullCount >= 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceRssRedPoint);
            }

            if (minTime!= float.MaxValue)
            {

                if (m_resResPointTimer!=null)
                {
                    m_resResPointTimer.Cancel();
                    m_resResPointTimer = null;
                }
                
                m_resResPointTimer = Timer.Register(minTime,CheckResRedPoint);
            }
            
        }

        public GuildBuildInfoEntity GetFortressesByType(long type)
        {
            GuildBuildInfoEntity info;
            m_fortressesTypeBuildsDic.TryGetValue(type, out info);
            return info;
        }


        public GuildResourcePointInfoEntity GetResPoint()
        {
            if (m_resPoint == null)
            {
                m_resPoint = new GuildResourcePointInfoEntity();
            }

            return m_resPoint;
        }

        public long GetResPointCount()
        {
            var resPoint = GetResPoint();
            return resPoint.foodPoint + resPoint.woodPoint + resPoint.stonePoint + resPoint.goldPoint;
        }

        public GuildResourceCenterInfoEntity GetResCenter()
        {
            return m_resCenter;
        }

        public GuildBuildInfoEntity GetResCenterByType(int type)
        {
            return null;
        }

        public RoleTerritoryGainInfoEntity GetTerritoryGainByType(long type)
        {
            if (m_roleTerritoryGainsDic.ContainsKey(type))
            {
                return m_roleTerritoryGainsDic[type];
            }

            return null;
        }

        //objType 转为AllianceBuildingTypeDefine ID  12起为要塞
        private int[] typeMap = new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 12, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 1, 1};

        public string GetBuildModle(long type)
        {
            return CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(typeMap[type]).modelId;
        }

        public int GetBuildServerTypeToConfigType(long objType)
        {
            if (objType > typeMap.Length - 1)
            {
                return 0;
            }

            return typeMap[objType];
        }

        public GuildFlagInfoEntity GetFlagInfoEntity()
        {
//            if (m_flags.flagLimit <100)
//            {
//                m_flags.flagLimit = 100;
//            }
            return m_flags;
        }

        //领取联盟领土收益
        public bool SendGetResPoint()
        {
            Guild_TakeGuildTerritoryGain.request request = new Guild_TakeGuildTerritoryGain.request();
            AppFacade.GetInstance().SendSproto(request);
            return false;
        }


        public void SendCreateBuild(int type, float x, float y)
        {
            Guild_CreateGuildBuild.request request = new Guild_CreateGuildBuild.request();

            request.type = type;

            Debug.Log("创建联盟建筑 " + type + " x:" + x + " y:" + y + "  " + m_playerProxy.CurrentRoleInfo.pos.x + "  " +
                      m_playerProxy.CurrentRoleInfo.pos.y);

            request.pos = new PosInfo();
            request.pos.x = (long) x * 100;
            request.pos.y = (long) y * 100;

            AppFacade.GetInstance().SendSproto(request);
        }

        public void SendRemoveBuild(long targetIndex)
        {
            Guild_RemoveGuildBuild.request request = new Guild_RemoveGuildBuild.request();

            request.targetIndex = targetIndex;

            AppFacade.GetInstance().SendSproto(request);
        }

        // # 1 使用代币灭火 2 使用联盟积分灭火
        public void SendRepairBuild(long targetIndex, int type)
        {
            Guild_RepairGuildBuild.request request = new Guild_RepairGuildBuild.request();
            request.targetIndex = targetIndex;
            request.type = type;
            AppFacade.GetInstance().SendSproto(request);
        }


        public void SendGetGuildBuilds(int type)
        {
            Guild_GetGuildBuilds.request request = new Guild_GetGuildBuilds.request();
            request.reqType = type;
            AppFacade.GetInstance().SendSproto(request);
        }

        public void SendCheckBuildCanBuilded(int type, float x, float y)
        {
            Guild_CheckGuildBuildCreate.request request = new Guild_CheckGuildBuildCreate.request();

            request.type = type;

            Debug.Log("检查联盟建筑 " + type + " x:" + x + " y:" + y + "  " + m_playerProxy.CurrentRoleInfo.pos.x + "  " +
                      m_playerProxy.CurrentRoleInfo.pos.y);

            request.pos = new PosInfo();
            request.pos.x = (long) x * 100;
            request.pos.y = (long) y * 100;

            AppFacade.GetInstance().SendSproto(request);
        }

        #endregion

        #region 旗帜辅助

        public static void setFlag(GameObject go, List<long> guildFlagSigns)
        {

            if (go==null)
            {
                return;
            }
            
            var flagTransform = go.transform.Find("alliance/flag");
            if (flagTransform != null)
            {
                var flagSpriteRenderer = flagTransform.gameObject.GetComponent<SpriteRenderer>();

                var flagDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[0]);
                var flagColorDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[1]);


                if (flagDefine!=null && flagColorDefine!=null)
                {
                    ClientUtils.LoadSprite(flagSpriteRenderer, flagDefine.realityIcon,()=>{
                        ClientUtils.ImageSetColor(flagSpriteRenderer, flagColorDefine.colour);
                    });
                }
               
            }

            var logoTransform = go.transform.Find("alliance/logo");

            if (logoTransform != null)
            {
                var logoSpriteRenderer = logoTransform.gameObject.GetComponent<SpriteRenderer>();

                var logoDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[2]);
                var logoColorDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) guildFlagSigns[3]);

                if (logoDefine!=null && logoColorDefine!=null)
                {
                    ClientUtils.LoadSprite(logoSpriteRenderer, logoDefine.realityIcon,()=>{
                        ClientUtils.ImageSetColor(logoSpriteRenderer, logoColorDefine.colour);
                    });
                }
                
            }
        }

        #endregion

        #region 援助采集
        

        private Dictionary<string, AllianceBuildArmyLevel>
            m_buildArmsDic = new Dictionary<string, AllianceBuildArmyLevel>();

        private Dictionary<long, List<AllianceBuildArmyLevel>> m_buildArmsList =
            new Dictionary<long, List<AllianceBuildArmyLevel>>();

        private Dictionary<long, long> m_objectLeader = new Dictionary<long, long>();

        private HashSet<long> m_hasMyArmyInBuild = new HashSet<long>();


        
        
        public long CountSoldiers(Dictionary<long, SoldierInfo> dic)
        {
            long count = 0;

            if (dic != null)
            {
                foreach (var vp in dic)
                {
                    count += vp.Value.num;
                }   
            }

            return count;
        }


        public bool IsBuildArmsLeader(long buildIndex, long armyIndex)
        {
            long army = 0;

            m_objectLeader.TryGetValue(buildIndex, out army);

            return army == armyIndex;
        }

        public void UpdateArmsList(Map_GuildBuildArmys.request request)
        {
            var buildObjectID = request.objectIndex;

            if (request.HasLeaderBuildArmyIndex)
            {
                var leaderBuildArmyIndex = request.leaderBuildArmyIndex;
                if (m_objectLeader.ContainsKey(buildObjectID))
                {
                    m_objectLeader[buildObjectID] = leaderBuildArmyIndex;
                }
                else
                {
                    m_objectLeader.Add(buildObjectID, leaderBuildArmyIndex);
                }
            }            

            if (request.HasArmyList)
            {
                foreach (var valuePair in request.armyList)
                {
                    long buildArmyIndex = valuePair.Value.buildArmyIndex;
                    AllianceBuildArmyLevel armyInfo;

                    bool isNew = false;
                    string key = string.Format("{0}_{1}",buildObjectID , buildArmyIndex);
                    //部队唯一映射
                    if (!m_buildArmsDic.TryGetValue(key, out armyInfo))
                    {
                        armyInfo = new AllianceBuildArmyLevel();
                        armyInfo.LevelMember = new GuildBuildArmyInfoEntity();

                        armyInfo.prefab_index = 1;

                        m_buildArmsDic.Add(key, armyInfo);
                        isNew = true;
                    }

                    List<AllianceBuildArmyLevel> armysList;

                    //建筑唯一映射
                    if (!m_buildArmsList.TryGetValue(buildObjectID, out armysList))
                    {
                        armysList = new List<AllianceBuildArmyLevel>();
                        m_buildArmsList.Add(buildObjectID, armysList);
                    }


                    GuildBuildArmyInfoEntity.updateEntity(armyInfo.LevelMember, valuePair.Value);

                    if (isNew)
                    {
                        armysList.Add(armyInfo);
                        
                        armysList.Sort(((level, armyLevel) =>  level.LevelMember.arrivalTime.CompareTo(armyLevel.LevelMember.arrivalTime)));

                    }
                    
                    if (armyInfo.LevelMember.rid == m_playerProxy.CurrentRoleInfo.rid)
                    {
                        if (!m_hasMyArmyInBuild.Contains(buildObjectID))
                        {
                            //Debug.Log(valuePair.Value.rid+"  armyIndex"+valuePair.Value.armyIndex+ "buildIndex:"+buildArmyIndex +" 添加我的部队到建筑ID:"+buildObjectID);
                            m_hasMyArmyInBuild.Add(buildObjectID);
                        }
                    }
                    
                    armyInfo.armyCount = CountSoldiers(armyInfo.LevelMember.soldiers);

                }
            }

            if (request.HasDeleteBuildArmyIndexs)
            {
                var delList = request.deleteBuildArmyIndexs;
                int len = delList.Count;
                for (int i = 0; i < len; i++)
                {
                    var armyBuildIndex = delList[i];
                    AllianceBuildArmyLevel delArmy;

                    string key = string.Format("{0}_{1}",buildObjectID , armyBuildIndex);
                    
                    if (m_buildArmsDic.TryGetValue(key, out delArmy))
                    {
                        m_buildArmsDic.Remove(key);
                        
                        
                        List<AllianceBuildArmyLevel> armyInfoEntities;
                        if (m_buildArmsList.TryGetValue(buildObjectID, out armyInfoEntities))
                        {
                            armyInfoEntities.Remove(delArmy);

                            if (m_hasMyArmyInBuild.Contains(buildObjectID) && delArmy.LevelMember.rid== m_playerProxy.CurrentRoleInfo.rid)
                            {
                                //Debug.Log(delArmy.LevelMember.rid+" armyIndex: "+delArmy.LevelMember.armyIndex +" buildIndex: "+armyBuildIndex+"移除我的部队从建筑ID："+buildObjectID);
                                m_hasMyArmyInBuild.Remove(buildObjectID);
                            }
                            else
                            {
                               // Debug.Log(delArmy.LevelMember.rid+" armyIndex: "+delArmy.LevelMember.armyIndex +" buildIndex: "+armyBuildIndex+"移除部队从建筑ID："+buildObjectID);

                            }
                        }
                    }
                    else
                    {
                        //Debug.Log("部队不在映射表中"+armyBuildIndex);
                    }
                }
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildArmyUpdate, buildObjectID);
        }

        public List<AllianceBuildArmyLevel> GetBuildArmy(long objectID)
        {
            List<AllianceBuildArmyLevel> armyInfoEntities;
            if (m_buildArmsList.TryGetValue(objectID, out armyInfoEntities))
            {
            }

            return armyInfoEntities;
        }

        public bool HasMyArmyInBuild(long buildID)
        {
            return m_hasMyArmyInBuild.Contains(buildID);
        }


        public void SendMonitorBuildArmy(long objectID)
        {
            Map_GetGuildBuildArmys.request request = new Map_GetGuildBuildArmys.request();


            request.objectIndex = objectID;

            AppFacade.GetInstance().SendSproto(request);
        }

//        public List<>

        #endregion


        #region 礼物


        private long m_giftPoint;

        private long m_keyPoint;
        
        private Dictionary<long,GuildTreasureInfoEntity> m_treasures = new Dictionary<long, GuildTreasureInfoEntity>();
        
        private Dictionary<long,GuildGiftInfoEntity> m_gifts = new Dictionary<long, GuildGiftInfoEntity>();


        private List<GuildGiftInfoEntity> m_giftCommons = new List<GuildGiftInfoEntity>();
        
        private List<GuildGiftInfoEntity> m_giftUnCommons = new List<GuildGiftInfoEntity>();

        private int m_giftRedCommon = 0;
        private int m_giftRedUnCommon = 0;


        public List<GuildGiftInfoEntity> MGiftCommons => m_giftCommons;

        public List<GuildGiftInfoEntity> MGiftUnCommons => m_giftUnCommons;

        public long GiftPoint => m_giftPoint;

        public long KeyPoint => m_keyPoint;


        public long TreasureCount => m_treasures.Count;

        private float m_minExpiredGiftTime = long.MaxValue;
        private Timer m_minExpiredTimer = null;

        private float m_guildRssFullCount = 0;
        private Timer m_resResPointTimer = null;


        public int GetRedPointRss()
        {
            return m_guildRssFullCount > 0 ? 1 : 0;
        }

        public Dictionary<long, GuildTreasureInfoEntity> MTreasures => m_treasures;

        public void CleanGifts()
        {
            m_treasures.Clear();
            m_gifts.Clear();
            m_giftCommons.Clear();
            m_giftUnCommons.Clear();
            m_giftRedCommon = 0;
            m_giftRedUnCommon = 0;
            m_giftPoint = 0;
            m_keyPoint = 0;

            if (m_minExpiredTimer!=null)
            {
                m_minExpiredTimer.Cancel();
                m_minExpiredTimer = null;
            }
            
        }

        public void SendGetAllGift()
        {
            SendGift(2);
        }

        public void SendGetGift(long giftID)
        {
            SendGift(3,giftID);
        }

        public void SendGetBestGift()
        {
            SendGift(1);
        }

        //# 1 领取珍藏 2 一键领取普通礼物 3 领取指定礼物
        // giftIndex 1 : integer                           # type = 3时 giftIndex为礼物索引
        private void SendGift(int type,long giftIndex=0)
        {
            Guild_TakeGuildGift.request request = new  Guild_TakeGuildGift.request();
            request.type = type;
            request.giftIndex = giftIndex;
            
            AppFacade.GetInstance().SendSproto(request);
        } 

        public void SendGiftDelAll()
        {
            Guild_CleanGiftRecord.request request = new Guild_CleanGiftRecord.request();
            
            AppFacade.GetInstance().SendSproto(request);

        }


        public void UpdateGifts(Guild_GuildGifts.request request)
        {
            Debug.Log("联盟礼物");
            //ClientUtils.Print(request);
            if (request.HasGiftPoint)
            {
                m_giftPoint = request.giftPoint;
            }

            if (request.HasKeyPoint)
            {
                m_keyPoint = request.keyPoint;
            }

            if (request.HasGifts)
            {
                foreach (var kv in request.gifts)
                {
                    var gift = kv.Value;
                    GuildGiftInfoEntity infoEntity;
                    if (!m_gifts.TryGetValue(gift.giftIndex, out infoEntity))
                    {
                        infoEntity = new GuildGiftInfoEntity();
                        m_gifts.Add(gift.giftIndex,infoEntity);
                    }

                    long giftID = gift.giftId > 0 ? gift.giftId : infoEntity.giftId;
                    
                    if (giftID>=2000000)
                    {
                        if (gift.HasStatus && (gift.status == 1 && infoEntity.status ==0 ||gift.status == 2 && infoEntity.status ==0))
                        {
                            m_giftCommons.Insert(0,infoEntity);
                        }
                    }
                    else
                    {
                        if (gift.HasStatus && (gift.status == 1 && infoEntity.status ==0||gift.status == 2 && infoEntity.status ==0))
                        {
                          m_giftUnCommons.Insert(0,infoEntity);
                        }
                    }
                    GuildGiftInfoEntity.updateEntity(infoEntity, gift);
                }
            }
            
            if (request.HasTreasures)
            {
                foreach (var kv in request.treasures)
                {
                    var treasure = kv.Value;
                    GuildTreasureInfoEntity infoEntity;
                    if (!m_treasures.TryGetValue(treasure.giftIndex, out infoEntity))
                    {
                        infoEntity = new GuildTreasureInfoEntity();
                        m_treasures.Add(treasure.giftIndex,infoEntity);
                    }

                    GuildTreasureInfoEntity.updateEntity(infoEntity, treasure);
                }
            }


            if (request.HasDeleteGiftIndexs)
            {
                foreach (var index in request.deleteGiftIndexs)
                {
                    GuildGiftInfoEntity infoEntity;
                    if (m_gifts.TryGetValue(index, out infoEntity))
                    {
                        m_gifts.Remove(index);
                        if (infoEntity.giftId>=2000000)
                        {
                            m_giftCommons.Remove(infoEntity);
                        }
                        else
                        {
                            m_giftUnCommons.Remove(infoEntity);
                        }
                    }
                    else
                    {
                        m_treasures.Remove(index);
                    }
                }
            }


            CheckNextExpiredGiftTime();
            
            AppFacade.GetInstance().SendNotification(GuildGiftInfoEntity.GuildGiftInfoChange);
        }

        public void CheckNextExpiredGiftTime()
        {
            int giftRedCommon = 0;
            if (m_giftCommons.Count>0)
            {
                m_giftCommons.ForEach((gift) =>
                {
                    var rewardConfig = CoreUtils.dataService.QueryRecord<AllianceGiftRewardDefine>((int) gift.giftId);
                    var giftConfig = CoreUtils.dataService.QueryRecord<AllianceGiftTypeDefine>(rewardConfig.giftType);


                    long passTime = (giftConfig.lastTime + gift.sendTime) - ServerTimeModule.Instance.GetServerTime();

                    if (passTime>0)
                    {
                        m_minExpiredGiftTime = Mathf.Min(passTime, m_minExpiredGiftTime);

                        if (gift.status == 1)
                        {
                             giftRedCommon++;
                        }
                    }
                    
                });
            }

            int giftRedUnCommon = 0;
            if (m_giftUnCommons.Count>0)
            {
                m_giftUnCommons.ForEach((gift) =>
                {
                    var rewardConfig = CoreUtils.dataService.QueryRecord<AllianceGiftRewardDefine>((int) gift.giftId);
                    var giftConfig = CoreUtils.dataService.QueryRecord<AllianceGiftTypeDefine>(rewardConfig.giftType);


                    long passTime = (giftConfig.lastTime + gift.sendTime) - ServerTimeModule.Instance.GetServerTime();

                    if (passTime>0)
                    {
                        m_minExpiredGiftTime = Mathf.Min(passTime, m_minExpiredGiftTime);
                        if (gift.status == 1)
                        {
                            giftRedUnCommon++;
                        }
                    }
                });
            }


            if (giftRedCommon!= m_giftRedCommon ||giftRedUnCommon!=m_giftRedUnCommon )
            {
                m_giftRedCommon = giftRedCommon;
                m_giftRedUnCommon = giftRedUnCommon;
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceGiftRedPoint);
            }

            if (m_minExpiredGiftTime>0 && m_minExpiredGiftTime< long.MaxValue)
            {

                if (m_minExpiredTimer!=null)
                {
                    m_minExpiredTimer.Cancel();
                    
                }
                
                m_minExpiredTimer = Timer.Register(m_minExpiredGiftTime, CheckNextExpiredGiftTime);
            }
        }

        public int GetGiftRedCommon()
        {
            return m_giftRedCommon;
        }
        
        public int GetGiftRedUnCommon()
        {
            return m_giftRedUnCommon;
        }
        
        public int GetGiftRed()
        {
            return m_giftRedCommon+m_giftRedUnCommon;
        }


        #endregion


        #region 联盟红点

        public int AllRedPoint()
        {

            if (m_playerProxy.CurrentRoleInfo.guildId <=0)
            {
                return 0;
            }
            
            return GetGiftRed() + RedHelp()+(int)GetStudyDoRedCount()+GetRedPointRss();
        }

        #endregion


        #region 留言板

        //# 1 获取比messageIndex更新记录 2 获取比messageIndex更旧记录
        public void SendGetGuildMessageBoard(long guildID, long lastMsgIndex, long type)
        {
            Guild_GetGuildMessageBoard.request request = new Guild_GetGuildMessageBoard.request();

            request.guildId = guildID;
            request.messageIndex = lastMsgIndex;
            request.type = type;

            AppFacade.GetInstance().SendSproto(request);
        }

        public void SendNewBoardMessage(long guildID, long replyMsgIndex, string msg)
        {
            Guild_SendBoardMessage.request request = new Guild_SendBoardMessage.request();

            request.guildId = guildID;
            request.replyMessageIndex = replyMsgIndex;
            request.content = msg;

            Debug.Log(guildID + " 发送留言 " + replyMsgIndex + "  " + msg);

            AppFacade.GetInstance().SendSproto(request);
        }

        public void SendDelBoardMessage(long guildID, long msgIndex)
        {
            Guild_DeleteBoardMessage.request request = new Guild_DeleteBoardMessage.request();

            request.guildId = guildID;
            request.messageIndex = msgIndex;

            AppFacade.GetInstance().SendSproto(request);
        }

        public bool CheckIsR45(long rid)
        {
            var member = getMemberInfo(rid);

            if (member == null)
            {
                return false;
            }

            return member.guildJob >= 4;
        }

        public bool CheckIsOffiecOrLeader(long rid)
        {
            if (GetAlliance().leaderRid == rid)
            {
                return true;
            }

            if (getMemberOfficer(rid)!=null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 联盟商店物品
        private Dictionary<long,long> m_StoreItems = new Dictionary<long, long>();

        public void SetStoreItems(Guild_ShopQuery.response res)
        {
            foreach (var v in res.lst)
            {
                if (m_StoreItems.ContainsKey(v.idItemType))
                {
                    m_StoreItems[v.idItemType] = v.nCount;
                }
                else
                {
                    m_StoreItems.Add(v.idItemType,v.nCount);
                }
            }
        }

        public long GetStoreItemCountByItemType(long type)
        {
            if (m_StoreItems.ContainsKey(type))
            {
                return m_StoreItems[type];
            }
            return 0; 
        }
        #endregion

        #region 联盟科技


        private Timer m_timeStudy;

        public long GetStudyDoRedCount()
        {
            if (m_playerProxy.CurrentRoleInfo.guildId==0)
            {
                return 0;
            }
            long count = (ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)/Config.AllianceStudyGiftCD;
            int totalCount = Config.AllianceStudyGiftTime;
            if (count>totalCount)
            {
                if (m_timeStudy!=null)
                {
                    m_timeStudy.Cancel();
                    m_timeStudy = null;
                }
                
                long remainTime = (ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)/Config.AllianceStudyGiftCD;
                
                m_timeStudy = Timer.Register(remainTime,TimeStudyDo);
                
                count = totalCount;
            }

            
            return count;
        }
        
        private void TimeStudyDo()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceStudyDonateRedCount);
        }

        #endregion

        #region 发送包

        //1 修改简称 2 修改名称 3 设置欢迎邮件 4 设置联盟公告、入盟要求和语言信息 5 设置联盟标识
        public void SendCreateAlliance(Guild_CreateGuild.request createGuild)
        {
            AppFacade.GetInstance().SendSproto(createGuild);
        }

        public void SendEditAllianceInfo(int type, string newValue, List<long> signs, bool needExamine = false,
            int langID = 0)
        {
            Guild_ModifyGuildInfo.request request = new Guild_ModifyGuildInfo.request();

            request.type = type;


            switch (type)
            {
                case 1:
                case 2:
                case 3:
                    request.newValue = newValue;

                    break;
                case 5:

                    request.newSigns = signs;
                    break;
                case 4:
                    request.newValue = newValue;
                    request.needExamine = needExamine;
                    request.languageId = langID;
                    break;
            }

            AppFacade.GetInstance().SendSproto(request);
        }


        //检查联盟名词
        public void SendCheckRename(long type, string name)
        {
            Guild_CheckGuildName.request request = new Guild_CheckGuildName.request();

            request.type = type;
            request.value = name;

            AppFacade.GetInstance().SendSproto(request);
        }


        public void SearchAlliance(int type, string key = "")
        {
            Guild_SearchGuild.request req = new Guild_SearchGuild.request();
            req.type = type;
            req.keyName = key;

            AppFacade.GetInstance().SendSproto(req);
        }

        //申请加入联盟
        public void SendJionAlliance(long id)
        {
            Guild_ApplyJoinGuild.request request = new Guild_ApplyJoinGuild.request();

            request.guildId = id;

            AppFacade.GetInstance().SendSproto(request);
        }

        //取消入盟申请
        public void CancelJionAlliance(long gid)
        {
            Guild_CancelGuildApply.request request = new Guild_CancelGuildApply.request();

            request.guildId = gid;

            AppFacade.GetInstance().SendSproto(request);
        }


        //#获取联盟申请信息
        public void SendAllianceInfo(int reqType)
        {
            Guild_GetGuildInfo.request request = new Guild_GetGuildInfo.request();
            request.reqType = reqType;
            AppFacade.GetInstance().SendSproto(request);
        }

        //审批入盟申请
        public void SendApplyPlayer(GuildApplyInfoEntity infoEntity, bool res)
        {
            Guild_ExamineGuildApply.request request = new Guild_ExamineGuildApply.request();
            request.applyRid = infoEntity.rid;
            request.result = res;
            RemoveGuildApplys(infoEntity);
            AppFacade.GetInstance().SendSproto(request);
        }

        //邀请其他玩家加入联盟
        public void SendInvitePlayer(long rid)
        {
            Guild_InviteGuild.request request = new Guild_InviteGuild.request();

            request.invitedRid = rid;

            AppFacade.GetInstance().SendSproto(request);
        }


        //搜索其他玩家
        public void SendSearchPlayers(string name = "")
        {
            Role_QueryRoleByParam.request request = new Role_QueryRoleByParam.request();

            request.param = name;
            AppFacade.GetInstance().SendSproto(request);
        }

        //获取最新联盟成员信息
        public void SendReMember()
        {
            Guild_GetGuildMembers.request request = new Guild_GetGuildMembers.request();


            AppFacade.GetInstance().SendSproto(request);
        }

        //联盟成员升降级(包括盟主转让)
        public void SendMemberLvevelChange(long rid, int lv)
        {
            Guild_ModifyMemberLevel.request request = new Guild_ModifyMemberLevel.request();

            request.memberRid = rid;
            request.newGuildJob = lv;

            AppFacade.GetInstance().SendSproto(request);
        }

        //踢人
        public void SendKickMember(long rid, int res)
        {
            Guild_KickMember.request request = new Guild_KickMember.request();
            request.reasonId = res;

            request.memberRid = rid;

            AppFacade.GetInstance().SendSproto(request);
        }

        //1 退出联盟 2 解散联盟
        public void SendExitGuild(int res)
        {
            Guild_ExitGuild.request request = new Guild_ExitGuild.request();

            request.type = res;

            AppFacade.GetInstance().SendSproto(request);
        }

        //任命官员
        public void SendAppointOfficial(long rid, int officeID)
        {
            Guild_AppointOfficer.request request = new Guild_AppointOfficer.request();

            request.memberRid = rid;
            request.officerId = officeID;

            AppFacade.GetInstance().SendSproto(request);
        }

        #endregion

        public string GetName()
        {
            string str = "";
            bool isHasAlliance = HasJionAlliance();
            if (isHasAlliance)
            {
                str = LanguageUtils.getTextFormat(300030, GetAlliance().abbreviationName,
                    m_playerProxy.CurrentRoleInfo.name);
            }
            else
            {
                str = m_playerProxy.CurrentRoleInfo.name;
            }

            return str;
        }

        public static string FormatGuildName(string playerName, string guildName,int cID=0)
        {
            if (string.IsNullOrEmpty(guildName))
            {
                return playerName;
            }

            return LanguageUtils.getTextFormat(cID>0?cID:300030, guildName, playerName);
        }

        //资源是否足够
        public bool IsResEnough(int id)
        { 
            var defineData = CoreUtils.dataService.QueryRecord<AllianceBuildingDataDefine>(id);
            if (defineData != null)
            {
                bool isEnough = (defineData.food > 0) ? 
                                 GetCurrencyByType((long)EnumCurrencyType.allianceFood) >= GetAttrMulti(defineData.food) : true;
                if (!isEnough)
                {
                    return false;
                }
                isEnough = (defineData.wood > 0) ? 
                            GetCurrencyByType((long)EnumCurrencyType.allianceWood) >= GetAttrMulti(defineData.wood) : true;
                if (!isEnough)
                {
                    return false;
                }
                isEnough = (defineData.stone > 0) ? 
                            GetCurrencyByType((long)EnumCurrencyType.allianceStone) >= GetAttrMulti(defineData.stone) : true;
                if (!isEnough)
                {
                    return false;
                }
                isEnough = (defineData.coin > 0) ? 
                            GetCurrencyByType((long)EnumCurrencyType.allianceGold) >= GetAttrMulti(defineData.coin) : true;
                if (!isEnough)
                {
                    return false;
                }
                isEnough = (defineData.fund > 0) ? GetCurrencyByType((long)EnumCurrencyType.leaguePoints) >= GetAttrMulti(defineData.fund) : true;
                return isEnough;
            }
            else
            {
                Debug.LogErrorFormat("AllianceBuildingDataDefine not find:{0}", id);
                return false;
            }
        }

        public long GetAttrMulti(int val)
        { 
            long cost = m_resarchProxy.GetAttrMulti(allianceAttrType.allianceBuildingCostMulti, val);
            return cost;
        }

        public bool IsCanBuild(AllianceBuildingTypeDefine define, bool isJudgeRoot = false)
        {
            if (isJudgeRoot)
            {
                if (!GetSelfRoot(GuildRoot.createBuild))
                {
                    return false;
                }
            }

            if (define.imgShowIndex == 0) //联盟要塞
            {
                var buidInfo = GetFortressesByType(define.type);
                bool hasPreBuild = false;
                if (define.preBuilding1 > 0)
                {
                    GuildBuildInfoEntity preBuildInfo = GetFortressesByType(define.preBuilding1);
                    hasPreBuild = preBuildInfo == null;
                }

                bool canBuild = GetAlliance().memberNum >= define.playerNum &&
                                GetAlliance().power >= define.alliancePower && buidInfo == null &&
                                !hasPreBuild;
                if (canBuild == true)
                {
                    //判断资源是否足够
                    int id = define.type * 10000 + 1;
                    if (IsResEnough(id))
                    {
                        return true;
                    }
                }
            }
            else if (define.imgShowIndex == 1)//联盟资源中心
            {
                GuildBuildInfo buidInfo = GetResCenter().resourceCenter;
                bool canBuild = GetAlliance().memberNum >= define.playerNum &&
                                GetFlagInfoEntity().flagNum >= define.preNum1 && buidInfo == null;
                if (canBuild == true)
                {
                    //判断资源是否足够
                    int id = define.type * 10000 + 1;
                    if (IsResEnough(id))
                    {
                        return true;
                    }
                }
            }
            else if (define.imgShowIndex == 2) //联盟旗帜
            {
                var flag = GetFlagInfoEntity();
                if (flag != null && flag.flagNum < flag.flagLimit)
                {
                    bool hasPreBuild = false;
                    if (define.preBuilding1 > 0)
                    {
                        GuildBuildInfoEntity preBuildInfo = GetFortressesByType(define.preBuilding1);
                        hasPreBuild = preBuildInfo == null;
                    }

                    if (hasPreBuild)
                    {
                        return false;
                    }

                    //判断资源是否足够
                    int id = define.type * 10000 + 1 + (int)flag.flagNum;
                    if (IsResEnough(id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}