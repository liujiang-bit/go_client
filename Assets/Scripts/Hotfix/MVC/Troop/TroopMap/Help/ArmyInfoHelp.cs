using System;
using Data;
using Game;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public  class ArmyInfoHelp : TSingleton<ArmyInfoHelp>
    {
        private TroopProxy m_TroopProxy;
        private SoldierProxy m_soldierProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private RssProxy m_RssProxy;
        private WorldMapObjectProxy m_worldProxy;

        
        public  void InitProxy()
        {
            m_TroopProxy=   AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_playerAttributeProxy=  AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_RssProxy= AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_worldProxy= AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }


        public  long GetArmyCollectResidueTime(int armyIndex)
        {
            ArmyInfoEntity armyInfo = m_TroopProxy.GetArmyByIndex(armyIndex);
            if (armyInfo == null)
            {
                return 0;
            }
            
            if (armyInfo.collectResource == null)
            {
                return 0;
            }
            
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            return armyInfo.collectResource.endTime-serverTime;
        }


        public long GetArmyCollectMaxTime(int armyIndex)
        {
            ArmyInfoEntity armyInfo = m_TroopProxy.GetArmyByIndex(armyIndex);
            if (armyInfo == null)
            {
                return 0;
            }
            
            if (armyInfo.collectResource == null)
            {
                return 0;
            }

            return armyInfo.collectResource.endTime - armyInfo.collectResource.startTime;
        }

        public long GetArmyCollectCurValue(int armyIndex)
        {
            ArmyInfoEntity armyInfo = m_TroopProxy.GetArmyByIndex(armyIndex);
            if (armyInfo == null)
            {
                return 0;
            }

            if (armyInfo.collectResource == null)
            {
                return 0;
            }

            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 countTime = armyInfo.collectResource.endTime - armyInfo.collectResource.startTime;
            if (countTime > 0)
            {
                return (serverTime-armyInfo.collectResource.startTime)/countTime;
            }
            return 0;
        }


        public   long GetArmyCollectNum(int armyIndex)
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            ArmyInfoEntity armyInfo = m_TroopProxy.GetArmyByIndex(armyIndex);
            if (armyInfo == null)
            {
                return 0;
            }
            
            if (armyInfo.collectResource==null)
            {
                return 0;
            }

            long collectNum = armyInfo.collectResource.collectNum + armyInfo.collectResource.collectSpeed *
                              (serverTime - armyInfo.collectResource.lastSpeedChangeTime) / 10000;
            return collectNum;
        }


        public  float GetArmyWeight(int armyIndex)
        {             
            long armyWeight = 0;
            //总负载
            ArmyInfoEntity armyInfo = m_TroopProxy.GetArmyByIndex(armyIndex);
            if (armyInfo == null)
            {
                return armyWeight;
            }

            if (armyInfo.collectResource == null)
            {
                return armyWeight;
            }

            if (armyInfo.targetArg == null)
            {
                return armyWeight;
            }


            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(armyInfo.targetArg.targetObjectIndex);
            if (infoEntity == null)
            {
                return 0;
            }
            
            int m_currResWeight=0; //当前资源负载            
            float m_collectBeforeWeight = 0;

            m_currResWeight = GetAllianceBuildingResWeight((RssType) armyInfo.collectResource.guildBuildType);
            m_currResWeight = m_currResWeight == 0 ? 1 : m_currResWeight;
            if (armyInfo.collectResource.collectSpeeds != null)
            {
                for (int i = 0; i < armyInfo.collectResource.collectSpeeds.Count; i++)
                {
                    float collectVal = (float) armyInfo.collectResource.collectSpeeds[i].collectSpeed / 10000 *
                                       armyInfo.collectResource.collectSpeeds[i].collectTime;
                    m_collectBeforeWeight = m_collectBeforeWeight + m_currResWeight * collectVal;
                }
            }


            if (armyInfo.soldiers != null)
            {
                foreach (var soldier in armyInfo.soldiers.Values)
                {
                    int tempId = m_soldierProxy.GetTemplateId((int) soldier.type, (int) soldier.level);
                    ArmsDefine define2 = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                    armyWeight = armyWeight + define2.capacity * soldier.num;
                }
            }
            
            float multi =(1 + m_playerAttributeProxy.GetTroopAttribute(armyInfo.mainHeroId,armyInfo.deputyHeroId,attrType.troopsSpaceMulti).value);
            long totalWeight = (int) Mathf.Floor(armyWeight * multi);
            
            //已使用负载
            float usedWeight = 0;
            if (armyInfo.resourceLoads != null)
            {
                for (int i = 0; i < armyInfo.resourceLoads.Count; i++)
                {
                    int resType =GetResType(armyInfo.resourceLoads[i]);
                    if (resType > 0)
                    {
                        usedWeight = usedWeight +
                                     m_RssProxy.GetResWeight(resType) * armyInfo.resourceLoads[i].load;
                    }
                }
            }
            //剩余总负载
            float mResidueTotalWeight = totalWeight - usedWeight - m_collectBeforeWeight;
            return mResidueTotalWeight;
        }

        private int GetAllianceBuildingResWeight(RssType type)
        {
            int num = 0;
            switch (type)
            {
                case RssType.GuildFoodResCenter:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).foodRaito;
                    break;
                case  RssType.GuildWoodResCenter:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).woodRaito;
                    break;
                case RssType.GuildGoldResCenter:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).stoneRaito;
                    break;
                case RssType.GuildGemResCenter:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).goldRaito;
                    break;

            }
            
            return num;
        }

        public int GetRssWeight(RssType type)
        {
            int num = 1;
            switch (type)
            {
                case RssType.Farmland:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).foodRaito;
                    break;
                case RssType.Wood:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).woodRaito;
                    break;
                case RssType.Stone:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).stoneRaito;
                    break;
                case RssType.Gold:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).goldRaito;
                    break;
                case RssType.Gem:
                    num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).diamonRaito;
                    break;                   
            }

            return num;
        }
        
        public int GetResType(ResourceCollectInfo collectInfo)
        {
            int resType = 0;
            if (collectInfo.resourceId > 0)
            {
                ResourceGatherTypeDefine define = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int)collectInfo.resourceTypeId);
                if (define != null)
                {
                    resType = define.type;
                }
            }
            else
            {
                AllianceBuildingTypeDefine define = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)collectInfo.guildBuildType);
                if (define != null)
                {
                    resType = define.gatherType;
                }
            }
            return resType;
        }
    }
}