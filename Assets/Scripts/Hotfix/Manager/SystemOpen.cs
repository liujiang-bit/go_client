using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Skyunion;
using Client;

namespace Game
{
    public enum EnumSystemOpen
    {
        captain = 10000,
        alliance = 10001,
        bag = 10002,
        war = 10003,
        mail = 10004,
        war_expedition = 10005,
        war_egypt = 10006,
        war_sunset = 10007,
        war_lostcanyon = 10008,
        war_osiris = 10009,
        rank = 20001,
        charge_pop = 30001,
        guild_msg = 40001,
        guild_mail = 40002,
    }

    public class SystemOpen
    {
        private static Dictionary<int, SystemOpenDefine> m_systemOpenDefines;
        public static Dictionary<int,SystemOpenDefine> SystemOpenDefine
        {
            get
            {
                if(m_systemOpenDefines==null)
                {
                    m_systemOpenDefines = new  Dictionary<int, SystemOpenDefine>();
                    var tmpObj = CoreUtils.dataService.QueryRecords<SystemOpenDefine>();
                    if(tmpObj!=null)
                    {
                        for(int i = 0;i<tmpObj.Count;i++)
                        {
                            m_systemOpenDefines.Add(tmpObj[i].ID,tmpObj[i]);
                        }
                    }
                }
                return m_systemOpenDefines;
            }
        }

        public static HashSet<int> AlreadyAchieve { get; } = new HashSet<int>();

        public static bool IsCanOpenByUiId(int uiID, bool autoPopTip = true)
        {
            OpenUiDefine define = CoreUtils.dataService.QueryRecord<OpenUiDefine>(uiID);
            if (define == null)
            {
                Debug.LogErrorFormat("OpenUiDefine not find:{0}", uiID);
                return false;
            }
            if (!IsExistsByBuildType(define.buildType))
            {
                if (autoPopTip)
                {
                    BuildingTypeConfigDefine define1 = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>(define.buildType);
                    string str = LanguageUtils.getTextFormat(300249, LanguageUtils.getText(define1.l_nameId));
                    Tip.CreateTip(str).SetStyle(Tip.TipStyle.Middle).Show();
                }
                return false;
            }

            bool isOpen = IsSystemOpen(define.systemOpen, autoPopTip);
            if (isOpen)
            {
                if (define.systemOpen == 10001) //如果是进入联盟相关界面 需要判断是否有联盟
                {
                    var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                    if (!allianceProxy.HasJionAlliance())
                    {
                        return false;
                    }
                }
            }
            return isOpen;
        }
        public static bool IsBuildType(int buildType, bool autoPopTip = true)
        {
            if (!IsExistsByBuildType(buildType))
            {
                if (autoPopTip)
                {
                    BuildingTypeConfigDefine define1 = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>(buildType);
                    string str = LanguageUtils.getTextFormat(300249, LanguageUtils.getText( define1.l_nameId));
                    Tip.CreateTip(str).SetStyle(Tip.TipStyle.Middle).Show();
                }
                return false;
            }
            return true;
        }
        public static bool IsActivity(List<int> activityGroups,out int openId ,bool autoPopTip = true)
        {
            bool open = false;
            int tempId = 0;
            openId = 0;
            var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            activityGroups.ForEach((group) => {
                if (activityProxy.IsOpenByGroup(group))
                        {
                    tempId = group;
                    open = true;
                }
            });
            if (!open)
            {
                Tip.CreateTip(128022).SetStyle(Tip.TipStyle.Middle).Show();
            }
            openId = tempId;
            return open;
        }
        public static bool IsPackage(int group, bool autoPopTip = true)
        {
            bool open = false;
            var rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            open = rechargeProxy.isSuperGiftboughtByGroup(group);
            if (!open)
            {
                Tip.CreateTip(128023).SetStyle(Tip.TipStyle.Middle).Show();
            }
            return open;
        }
        public static bool IsRecharge(int pagingType, bool autoPopTip = true)
        {
            bool open = false;
            var rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            var rechargeListcfg = CoreUtils.dataService.QueryRecord<RechargeListDefine>(pagingType);
            if (rechargeListcfg == null) return false;
            open = rechargeProxy.IsShowInToggleList(rechargeListcfg);
            if (!open)
            {
                Tip.CreateTip(128023).SetStyle(Tip.TipStyle.Middle).Show();
            }
            return open;
        }

        public static bool IsSystemOpen(int systemID,bool autoPopTip = true)
        {
            if (systemID == 0)
            {
                return true;
            }
            if(AlreadyAchieve.Contains(systemID))
            {
                return true;
            }
            if(SystemOpenDefine.TryGetValue(systemID,out var define))
            {
                bool open = IsAchieveOpenLv(define.openLv)&&IsAchieveOpenTask(define.openTask)&&IsAchieveOpenSuccess(define.openSuccess);
                if(open)
                {
                    AlreadyAchieve.Add(systemID);
                }
                else if(autoPopTip)
                {
                    string str = LanguageUtils.getTextFormat(define.l_promptID, define.openLv);
                    Tip.CreateTip(str).SetStyle(Tip.TipStyle.Middle).Show();
                }
                return open;
            }
            Debug.LogErrorFormat("SystemID不存在 ：{0}", systemID);
            return false;
        }
        public static void ClearSystemOpen()
        {
            AlreadyAchieve.Clear();
        }

        public static bool IsSystemOpen(EnumSystemOpen systemID, bool autoPopTip = true)
        {
            return IsSystemOpen((int)systemID, autoPopTip);
        }

        public static bool IsSystemClose(EnumSystemOpen systemID, bool autoPopTip = true)
        {
            return !IsSystemOpen((int)systemID,autoPopTip);
        }

        public static bool IsSystemClose(int systemID, bool autoPopTip = true)
        {
            return !IsSystemOpen(systemID, autoPopTip);
        }

        private static bool IsAchieveOpenLv(int value)
        {
            if(value<=0)
            {
                return true;
            }
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if(playerProxy!=null&& playerProxy.CurrentRoleInfo!=null)
            {
                return playerProxy.CurrentRoleInfo.level >= value;
            }

            return false;
        }

        //建筑是否存在
        private static bool IsExistsByBuildType(int buildType)
        {
            if (buildType == 0)
            {
                return true;
            }
            CityBuildingProxy buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            BuildingInfoEntity info = buildingProxy.GetBuildingInfoByType(buildType);
            if (info == null)
            {
                return false;
            }
            if (info.level < 1)
            {
                return false;
            }
            return true;
        }

        private static bool IsAchieveOpenTask(int value)
        {
            if (value <= 0)
            {
                return true;
            }
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy != null && playerProxy.CurrentRoleInfo != null)
            {
                if(playerProxy.CurrentRoleInfo.mainLineTaskId < 0)
                {
                    return true;
                }
                return playerProxy.CurrentRoleInfo.mainLineTaskId > value;
            }

            return false;
        }

        private static bool IsAchieveOpenSuccess(int value)
        {
            if (value <= 0)
            {
                return true;
            }

            return false;
        }
    }
}

