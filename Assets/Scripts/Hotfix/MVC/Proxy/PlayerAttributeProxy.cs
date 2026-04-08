// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    TrainProxy 训练部队
// Copyright IGG All rights reserved.
// ===============================================================================

using Data;
using Skyunion;
using SprotoType;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {
    // 所有属性的获取，需要判断是否为 null 因为 null 表示没有该属性
    public class PlayerAttributeProxy : GameProxy
    {
        public class AttributeValue
        {
            public AttributeValue(AttrInfoDefine define, long value)
            {
                this.define = define;
                this.origvalue = value;
            }

            public float value
            {
                get
                {
                    if(define.valueType == 0)
                    {
                        return origvalue;
                    }
                    if (define.valueType == 2)
                    {
                        return origvalue / 1000.0f;
                    }
                    Debug.LogWarningFormat("不能处理的类型，{0}", define.ID);
                    return origvalue;
                }
                private set { }
            }
            public float perfValue
            {
                get
                {
                    return value * 100;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_decimal">true 添加两位小数</param>
            /// <returns></returns>
            public string GetShowValue()
            {
                if (define.valueType == 0)
                {
                    return  value.ToString("N0");
                }
                if (define.valueType == 2)
                {
                        return (value*100).ToString("0.##");
                }
                Debug.LogWarningFormat("不能处理的类型，{0}", define.ID);
                return (origvalue>0?"+":"")+origvalue.ToString("N0");
            }

            public AttrInfoDefine define;
            public long origvalue;
        }

        // 城市属性加层相关
        Dictionary<long, List<AttributeValue>>[] m_listCityAttributeSource;
        // 城市属性汇总
        AttributeValue[] m_cityAttributeTotal;
        // 城市属性来源汇总
        AttributeValue[][] m_cityAttributeSourceTotal;

        // 英雄属性汇总
        Dictionary<long, AttributeValue[]> m_heroAttributes;
        // 英雄属性来源汇总
        Dictionary<long, AttributeValue[][]> m_heroAttributeSourceTotal;

        // 部队属性相关
        Dictionary<long, AttributeValue[]> m_troopAttributes;

        // 属性配置表
        List<AttrInfoDefine> m_arrtibuteConfigs;

        private int m_attrTypeCount;
        private int m_sourceAttrCount;

        #region Member
        public const string ProxyNAME = "PlayerAttributeProxy";
        #endregion
        // Use this for initialization
        public PlayerAttributeProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" PlayerAttributeProxy register");
            int typeCount = Enum.GetValues(typeof(attrType)).Length - 1;
            m_attrTypeCount = typeCount;
            m_sourceAttrCount = Enum.GetValues(typeof(EnumSourceAttr)).Length;

            m_listCityAttributeSource = new Dictionary<long, List<AttributeValue>>[m_attrTypeCount];
            m_cityAttributeTotal = new AttributeValue[m_attrTypeCount];
            m_heroAttributes = new Dictionary<long, AttributeValue[]>();
            m_troopAttributes = new Dictionary<long, AttributeValue[]>();
            m_arrtibuteConfigs = CoreUtils.dataService.QueryRecords<AttrInfoDefine>();
            m_cityAttributeSourceTotal = new AttributeValue[m_sourceAttrCount][];

            for (int i = 0; i < m_cityAttributeSourceTotal.Length; i++)
            {
                m_cityAttributeSourceTotal[i] = new AttributeValue[m_attrTypeCount];
            }

            m_heroAttributeSourceTotal = new Dictionary<long, AttributeValue[][]>();
        }

        public override void OnRemove()
        {
            Debug.Log(" PlayerAttributeProxy remove");
        }
        //--------------------------------------------------------------------

        // 获取当前属性的来源， 外部自己根据显示规则归类。
        public AttributeValue GetCityAttributes(attrType type, EnumSourceAttr source)
        {
            var attribute = m_cityAttributeSourceTotal[(int)source - 1][(int)type - 1];
            //if(attribute == null)
            //{
            //    m_cityAttributeSourceTotal[(int)source - 1][(int)type - 1] = attribute = new AttributeValue(m_arrtibuteConfigs[(int)type-1], 0);
            //}
            return attribute;
        }

        // 获取城市属性的统一接口
        public AttributeValue GetCityAttribute(attrType type)
        {
            var attribute = m_cityAttributeTotal[(int)type - 1];
            if (attribute == null)
            {
                m_cityAttributeTotal[(int)type - 1] = attribute = new AttributeValue(m_arrtibuteConfigs[(int)type - 1], 0);
            }
            return attribute;
        }

        #region 英雄属性
        // 获取统帅属性的统一接口
        public AttributeValue[] GetHeroAttribute(long captionId)
        {
            AttributeValue [] heroAttribute;
            if (!m_heroAttributes.TryGetValue(captionId, out heroAttribute))
            {
                heroAttribute = new AttributeValue[Enum.GetValues(typeof(attrType)).Length - 1];
                for(int i = 0; i < m_arrtibuteConfigs.Count; i++)
                {
                    var config = m_arrtibuteConfigs[i];
                    heroAttribute[(int)config.ID-1] = new AttributeValue(config, 0);
                }
                BuildHeroAttribute(heroAttribute, captionId);
                m_heroAttributes.Add(captionId, heroAttribute);
            }
            return heroAttribute;
        }
        // 获取统帅属性的统一接口
        public AttributeValue GetHeroAttribute(long captionId, attrType type)
        {
            var attribute = GetHeroAttribute(captionId);
            if(attribute != null)
            {
                return attribute[(int)type-1];
            }
            return new AttributeValue(m_arrtibuteConfigs[(int)type-1], 0);
        }

        //根据来源获取英雄的属性
        public AttributeValue GetHeroSourceAttribute(long heroId, attrType type, EnumSourceAttr source)
        {
            if (!m_heroAttributes.ContainsKey(heroId))
            {
                GetHeroAttribute(heroId);
            }
            var arr = GetHeroSourceAttrArr(heroId);
            var attribute = arr[(int)source - 1][(int)type - 1];
            if (attribute == null)
            {
                attribute = new AttributeValue(m_arrtibuteConfigs[(int)type - 1], 0);
                arr[(int)source - 1][(int)type - 1] = attribute;
            }
            return attribute;
        }
        // 获取英雄某个来源对应的属性列表
        private AttributeValue[] GetHeroSourceAttrList(long heroId, EnumSourceAttr source)
        {
            var arr = GetHeroSourceAttrArr(heroId);
            return arr[(int)source - 1];
        }

        private AttributeValue[][] GetHeroSourceAttrArr(long heroId)
        {
            if (m_heroAttributeSourceTotal.ContainsKey(heroId))
            {
                return m_heroAttributeSourceTotal[heroId];
            }
            else
            {
                var arr = new AttributeValue[m_sourceAttrCount][];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = new AttributeValue[m_attrTypeCount];
                }
                m_heroAttributeSourceTotal[heroId] = arr;
                return arr;
            }
        }

        #endregion

        // 获取部队属性的统一接口
        public AttributeValue [] GetTroopAttribute(long troopId)
        {
            AttributeValue[] troopAttribute;
            if (!m_heroAttributes.TryGetValue(troopId, out troopAttribute))
            {
                var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                var troop = troopProxy.GetArmyByIndex(troopId);

                troopAttribute = new AttributeValue[Enum.GetValues(typeof(attrType)).Length - 1];

                BuildTroopAttribute(troopAttribute, troop.mainHeroId, troop.deputyHeroId);

                m_heroAttributes.Add(troopId, troopAttribute);
            }
            return troopAttribute;
        }

        // 获取部队属性的统一接口
        public AttributeValue GetTroopAttribute(long troopId, attrType type)
        {
            var attribute = GetTroopAttribute(troopId);
            if (attribute != null)
            {
                return attribute[(int)type - 1];
            }
            return null;
        }
        // 获取部队属性的统一接口
        public AttributeValue GetTroopAttribute(long mainHeroId, long deputyHeroId, attrType type)
        {
            var attribute = GetTroopAttribute(mainHeroId, deputyHeroId);
            if (attribute != null)
            {
                return attribute[(int)type - 1];
            }
            return null;
        }
        // 获取部队属性的统一接口
        public AttributeValue[] GetTroopAttribute(long mainHeroId, long deputyHeroId)
        {
            AttributeValue[] troopAttribute;
            troopAttribute = new AttributeValue[Enum.GetValues(typeof(attrType)).Length - 1];
            BuildTroopAttribute(troopAttribute, mainHeroId, deputyHeroId);
            return troopAttribute;
        }

        //获取副将技能属性
        public AttributeValue[] GetAttributebuteBySkill(long captionId)
        {          
            AttributeValue [] heroAttribute;
            if (!m_heroAttributes.TryGetValue(captionId, out heroAttribute))
            {
                heroAttribute = new AttributeValue[Enum.GetValues(typeof(attrType)).Length - 1];
                for(int i = 0; i < m_arrtibuteConfigs.Count; i++)
                {
                    var config = m_arrtibuteConfigs[i];
                    heroAttribute[(int)config.ID-1] = new AttributeValue(config, 0);
                }
                BuildHeroAttributeBySkill(heroAttribute, captionId);
                m_heroAttributes.Add(captionId, heroAttribute);
            }
            return heroAttribute;
        }

        /// <summary>
        /// 联盟头衔
        /// </summary>
        /// <param name="techs"></param>
        public void UpdateGuildOfficerInfo(GuildOfficerInfoEntity guildOfficerInfoEntity)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.GuildOfficerInfo, 0);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.GuildOfficerInfo);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
            if (guildOfficerInfoEntity != null)
            {
                var config = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>((int)guildOfficerInfoEntity.officerId);

                if (config != null && config.add != null)
                {
                    for (int i = 0; i < config.addAtt.Count; i++)
                    {
                        var type = config.addAtt[i];
                        AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, type, config.addData[i]);
                    }
                }
            }
            else
            {
                
            }
        }
        /// <summary>
        /// 联盟头衔
        /// </summary>
        /// <param name="techs"></param>
        public void UpdateGuildOfficerInfo( )
        {
            var attributes = GetAttrbutes(EnumSourceAttr.GuildOfficerInfo, 0);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.GuildOfficerInfo);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
        }
        // 科技变更都要同步这个接口 不管是删除增加还是更新
        public void UpdateTechInfo(Dictionary<long, TechnologyInfo> techs)
        {
            var listTechs = techs.Values.ToList();

            listTechs.ForEach((tech) =>
            {
                UpdateTechInfo(tech.technologyType, tech.level);
            });
        }
        // 建筑变更都要同步这个接口 不管是删除增加还是更新
        public void UpdateCityBuildInfo(List<BuildingInfoEntity> buildingInfoEntities)
        {
            buildingInfoEntities.ForEach((build) =>
            {
                UpdateCityBuildInfo(build.buildingIndex, build.type, build.level);
            });
        }
        // 文明更新接口， 一般是登陆时直接调用
        public void UpdateCivilization(long id)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.CivilizationBuff, 0);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.CivilizationBuff);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
            var config = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)id);

            if (config != null && config.civilizationAddNew != null)
            {
                for (int i = 0; i < config.civilizationAddNew.Count; i++)
                {
                    var type = config.civilizationAddNew[i];
                    AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, type, config.civilizationAddData[i]);
                }
            }
        }
        // 目前还没有此功能，等有了再添加规则
        // 联盟科技变更都要同步这个接口 不管是删除增加还是更新
        public void UpdateAllianceTechInfo(Dictionary<long, GuildTechnologyInfo> techs)
        {
            var listTechs = techs.Values.ToList();

            listTechs.ForEach((tech) =>
            {
                UpdateGuildTechInfo(tech.type, tech.level);
            });
        }
        public void UpdateAllianceTechInfo()
        {
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.AliStudy);
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal);
            int id = (int)EnumSourceAttr.AliStudy - 1;
            if (m_listCityAttributeSource[id] != null)
            {
                m_listCityAttributeSource[id].Clear();
            }
        }
        // 目前还没有此功能，等有了再添加规则
        // Buf状态变更都要同步这个接口 不管是删除增加还是更新
        public void UpdateCityBuf(CityBuff cityBuff)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.CityBuff, cityBuff.id);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.CityBuff);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
            var cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.id);
            if (cityBuff.expiredTime == -2)
                return;
            if (cityBuffDefine.attrNew != null)
            {
                for (int i = 0; i < cityBuffDefine.attrNew.Count; i++)
                {
                    var type = cityBuffDefine.attrNew[i];
                    AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, type, cityBuffDefine.attrData[i]);
                }
            }
        }
        // 目前还没有此功能，等有了再添加规则
        // VIP更新接口， 一般是登陆时直接调用
        public void UpdateVip(long lvl)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.Vip, 0);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.Vip);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
            var vipAttCfgs = CoreUtils.dataService.QueryRecords<VipAttDefine>();
            vipAttCfgs = vipAttCfgs.FindAll(x => x.levelGroup == lvl);
            foreach (var vipAttCfg in vipAttCfgs)
            {
                var type = vipAttCfg.attNew;
                if (type == attrType.None)
                {
                    continue;
                }
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, type, (long)vipAttCfg.add);
            }
        }

        public void RemoveHolyland(Dictionary<int, GuildHolyLandInfo> holyLandInfos)
        {
            foreach (var dict in holyLandInfos)
            {
                int holylandId = dict.Key;
                var attributes = GetAttrbutes(EnumSourceAttr.HolylandBuff, holylandId);
                var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.HolylandBuff);
                // 需要调用一下移除才知道哪些变化了
                RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
            }
        }


        /// <summary>
        /// 更新圣地属性
        /// </summary>
        public void UpdateHolyland(Dictionary<int, GuildHolyLandInfo> holyLandInfos)
        {
            foreach (var dict in holyLandInfos)
            {
                int holylandId = dict.Key;
                
                var attributes = GetAttrbutes(EnumSourceAttr.HolylandBuff, holylandId);
                var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.HolylandBuff);
                // 需要调用一下移除才知道哪些变化了
                RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
                StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>(holylandId);
                StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);
                UpdateHolylandCityBuff(attributes, cityAttributeSourceTotal, strongHoldTypeDefine.buffData1);
                UpdateHolylandCityBuff(attributes, cityAttributeSourceTotal, strongHoldTypeDefine.buffData2);
                UpdateHolylandCityBuff(attributes, cityAttributeSourceTotal, strongHoldTypeDefine.buffData3);
            }
        }

        // 计算圣地buff属性
        private void UpdateHolylandCityBuff(List<AttributeValue> attributes, AttributeValue[] cityAttributeSourceTotal, int buffId)
        {
            if (buffId <= 0)
            {
                return;
            }
            
            CityBuffDefine cityBuffDefine =
                CoreUtils.dataService.QueryRecord<CityBuffDefine>(buffId);
            if (cityBuffDefine.attrNew != null)
            {
                for (int i = 0; i < cityBuffDefine.attrNew.Count; i++)
                {
                    var type = cityBuffDefine.attrNew[i];
                    AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, type, cityBuffDefine.attrData[i]);
                }
            }
        }

        // 新增属性
        private void AddAttribute(AttributeValue [] totalAttributes, AttributeValue[] sourceTotalAttributes, List<AttributeValue> list, attrType type, long value)
        {
            var attribute = new AttributeValue(m_arrtibuteConfigs[(int)type - 1], value);
            list.Add(attribute);

            var attributeTotal = totalAttributes[(int)type - 1];
            if (attributeTotal == null)
            {
                attributeTotal = new AttributeValue(attribute.define, attribute.origvalue);
                totalAttributes[(int)type - 1] = attributeTotal;
            }
            else
            {
                attributeTotal.origvalue += value;
            }
            var sourceAttributeTotal = sourceTotalAttributes[(int)type - 1];
            if (sourceAttributeTotal == null)
            {
                sourceAttributeTotal = new AttributeValue(attribute.define, attribute.origvalue);
                sourceTotalAttributes[(int)type - 1] = sourceAttributeTotal;
            }
            else
            {
                sourceAttributeTotal.origvalue += value;
            }
        }
        // 移除属性
        private void RemoveAttribute(AttributeValue[] totalAttributes, AttributeValue[] sourceTotalAttributes, List<AttributeValue> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                var attribute = list[i];
                var attributeTotal = totalAttributes[(int)attribute.define.ID - 1];
                if (attributeTotal != null)
                {
                    attributeTotal.origvalue -= attribute.origvalue;
                }
                var sourceAttributeTotal = sourceTotalAttributes[(int)attribute.define.ID - 1];
                if (sourceAttributeTotal != null)
                {
                    sourceAttributeTotal.origvalue -= attribute.origvalue;
                }
            }
            list.Clear();
        }
        private void RemoveAttribute(AttributeValue[] totalAttributes, AttributeValue[] sourceTotalAttributes)
        {
            for (int i = 0; i < sourceTotalAttributes.Length; i++)
            {
                var attribute = sourceTotalAttributes[i]; 
                var sourceAttributeTotal = totalAttributes[i];
                if (attribute!=null&& sourceAttributeTotal != null)
                { 
                    sourceAttributeTotal.origvalue -= attribute.origvalue;
                    attribute.origvalue = 0;
                }
            }
        }
        // 获取一个属性来源的列表
        private List<AttributeValue> GetAttrbutes(EnumSourceAttr source, long id)
        {
            var dic = m_listCityAttributeSource[(int)source-1];
            if(dic == null)
            {
                dic = new Dictionary<long, List<AttributeValue>>();
                m_listCityAttributeSource[(int)source - 1] = dic;
            }
            List<AttributeValue> list;
            if (!dic.TryGetValue(id, out list))
            {
                list = new List<AttributeValue>();
                dic.Add(id, list);
            }
            return list;
        }
        // 获取一个属性来源的列表
        private AttributeValue[] GetSourceAttrbutes(EnumSourceAttr source)
        {
            return m_cityAttributeSourceTotal[(int)source - 1];
        }

        // 刷新建筑相关的属性
        private void UpdateCityBuildInfo(long id, long type, long level)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.Build, id);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.Build);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes);

            // 如果等级为 -1 表示建筑被移除了
            if (level == -1 || level == 0)
                return;

            if (type == (long)EnumCityBuildingType.TownCenter)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingTownCenterDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.troopsCapacity, config.troopsCapacity);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.troopsDispatchNumber, config.troopsDispatchNumber);
            }
            else if (type == (long)EnumCityBuildingType.CityWall)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingCityWallDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.wallDurableMax, config.wallDurableMax);
            }
            else if (type == (long)EnumCityBuildingType.GuardTower)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.warningTowerAttack, config.warningTowerAttack);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.warningTowerHpMax, config.warningTowerHpMax);
            }
            else if (type == (long)EnumCityBuildingType.Barracks)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingBarracksDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.infantryTrainNumber, config.infantryTrainNumber);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.infantryAttackMulti, config.infantryAttackMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.cavalryAttackMulti, config.cavalryAttackMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.bowmenAttackMulti, config.bowmenAttackMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.siegeCarAttackMulti, config.siegeCarAttackMulti);
            }
            else if (type == (long)EnumCityBuildingType.ArcheryRange)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingArcheryrangeDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.bowmenTrainNumber, config.bowmenTrainNumber);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.infantryHpMaxMulti, config.infantryHpMaxMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.cavalryHpMaxMulti, config.cavalryHpMaxMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.bowmenHpMaxMulti, config.bowmenHpMaxMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.siegeCarHpMaxMulti, config.siegeCarHpMaxMulti);
            }
            else if (type == (long)EnumCityBuildingType.Stable)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingStableDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.cavalryTrainNumber, config.cavalryTrainNumber);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.infantryDefenseMulti, config.infantryDefenseMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.cavalryDefenseMulti, config.cavalryDefenseMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.bowmenDefenseMulti, config.bowmenDefenseMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.siegeCarDefenseMulti, config.siegeCarDefenseMulti);
            }
            else if (type == (long)EnumCityBuildingType.SiegeWorkshop)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingSiegeWorkshopDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.siegeCarTrainNumber, config.siegeCarTrainNumber);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.troopsSpaceMulti, config.troopsSpaceMulti);
            }
            else if (type == (long)EnumCityBuildingType.Academy)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingCampusDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.researchSpeedMulti, config.researchSpeedMulti);
            }
            else if (type == (long)EnumCityBuildingType.Hospital)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingHospitalDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.infantryHpMaxMulti, config.infantryHpMaxMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.cavalryHpMaxMulti, config.cavalryHpMaxMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.bowmenHpMaxMulti, config.bowmenHpMaxMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.siegeCarHpMaxMulti, config.siegeCarHpMaxMulti);
            }
            else if (type == (long)EnumCityBuildingType.Castel)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingCastleDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.massTroopsCapacity, config.massTroopsCapacity);
            }
            else if (type == (long)EnumCityBuildingType.TradingPost)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingFreightDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.transportSpeedMulti, config.transportSpeedMulti);
            }
            else if (type == (long)EnumCityBuildingType.ScoutCamp)
            {
                var config = CoreUtils.dataService.QueryRecord<BuildingScoutcampDefine>((int)level);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.scoutSpeedMulti, config.scoutSpeedMulti);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.scoutNumber, config.scoutNumber);
                AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes, attrType.scoutView, config.scoutView);
            }
        }

        // 刷新科技相关的属性
        private void UpdateTechInfo(long type, long level)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.Study, type);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.Study);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal,  attributes);
            var id = type / 100 * 100000 + type * 100 + level;
            var config = CoreUtils.dataService.QueryRecord<StudyDefine>((int)id);
            if (config != null)
            {
                if (config.buffTypeNew != null)
                {
                    for (int i = 0; i < config.buffTypeNew.Count; i++)
                    {
                        var attr_type = config.buffTypeNew[i];
                        AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, attr_type, config.buffData[i]);
                    }
                }
            }
        }
        // 刷新联盟科技相关的属性
        private void UpdateGuildTechInfo(long type, long level)
        {
            var attributes = GetAttrbutes(EnumSourceAttr.AliStudy, type);
            var cityAttributeSourceTotal = GetSourceAttrbutes(EnumSourceAttr.AliStudy);
            // 需要调用一下移除才知道哪些变化了
            RemoveAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes);
            var id = type / 100 * 100000 + type * 100 + level;
            var config = CoreUtils.dataService.QueryRecord<AllianceStudyDefine>((int)id);
            if (config != null)
            {
                if (config.buffTypeNew != null)
                {
                    for (int i = 0; i < config.buffTypeNew.Count; i++)
                    {
                        var attr_type = config.buffTypeNew[i];
                        AddAttribute(m_cityAttributeTotal, cityAttributeSourceTotal, attributes, attr_type, config.buffData[i]);
                    }
                }
            }
            else
            {
               // Debug.LogErrorFormat("not find id = {0}AllianceStudyDefine",id);
            }
       
        }

        public void UpdateHeroInfo(HeroInfoEntity heroInfoEntity)
        {
            // 直接移除掉，后续获取的时候会重新计算
            if(m_heroAttributes.ContainsKey(heroInfoEntity.heroId))
            {
                m_heroAttributes.Remove(heroInfoEntity.heroId);
            }
            if (m_heroAttributeSourceTotal.ContainsKey(heroInfoEntity.heroId))
            {
                m_heroAttributeSourceTotal.Remove(heroInfoEntity.heroId);
            }
        }
        public void UpdateTroop(int troopId)
        {
            // 直接移除掉，后续获取的时候会重新计算
            if (m_troopAttributes.ContainsKey(troopId))
            {
                m_troopAttributes.Remove(troopId);
            }
        }

        private void AddHeroSourceAttr(AttributeValue[] sourceAttrList, attrType type, long value)
        {
            int index = (int)type - 1;
            var heroAttrSkillSource = sourceAttrList[(int)type - 1];
            if (heroAttrSkillSource == null)
            {
                var attribute = new AttributeValue(m_arrtibuteConfigs[index], value);
                heroAttrSkillSource = new AttributeValue(attribute.define, attribute.origvalue);
                sourceAttrList[index] = heroAttrSkillSource;
            }
            else
            {
                heroAttrSkillSource.origvalue += value;
            }            
        }

        //只算技能的 部队副将要用
        public void BuildHeroAttributeBySkill(AttributeValue [] heroAttribute, long id)
        {
            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            var hero = heroProxy.GetHeroByID(id);
            if (hero == null)
            {
                return;
            }
            // 部队容量
            heroAttribute[(int)attrType.troopsCapacity - 1].origvalue = hero.levelConfig.soldiers;
            if (hero.data != null)
            {
                var skills = hero.data.skills;
                var heroAttributeSkillSourceList = GetHeroSourceAttrList(id, EnumSourceAttr.HeroSkill);
                for (int i = 0; i < skills.Count; i++)
                {
                    var skill = skills[i];
                    long effectId = skill.skillId * 1000 + skill.skillLevel;
                    var effect = CoreUtils.dataService.QueryRecord<HeroSkillEffectDefine>((int)effectId);
                    if (effect == null)
                        continue;
                    for(int j = 0; j < effect.attrType.Count; j++)
                    {
                        if (effect.attrTypeNew[j] != attrType.None)
                        {
                            heroAttribute[(int)effect.attrTypeNew[j] - 1].origvalue += effect.attrNumber[j];
                            AddHeroSourceAttr(heroAttributeSkillSourceList, effect.attrTypeNew[j], effect.attrNumber[j]);
                        }
                    }
                }

            }

        }


        public void BuildHeroAttribute(AttributeValue [] heroAttribute, long id)
        {
            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            var hero = heroProxy.GetHeroByID(id);
            if (hero == null)
            {
                return;
            }

            // 后续需要加上 技能 天赋 装备的 属性加层
            // 部队容量
            heroAttribute[(int)attrType.troopsCapacity - 1].origvalue = hero.levelConfig.soldiers;
            
            if(hero.data != null)
            {
                var skills = hero.data.skills;
                var heroAttributeSkillSourceList = GetHeroSourceAttrList(id, EnumSourceAttr.HeroSkill);
                for (int i = 0; i < skills.Count; i++)
                {
                    var skill = skills[i];
                    long effectId = skill.skillId * 1000 + skill.skillLevel;
                    var effect = CoreUtils.dataService.QueryRecord<HeroSkillEffectDefine>((int)effectId);
                    if (effect == null)
                        continue;
                    for(int j = 0; j < effect.attrType.Count; j++)
                    {
                        if (effect.attrTypeNew[j] != attrType.None)
                        {
                            heroAttribute[(int)effect.attrTypeNew[j] - 1].origvalue += effect.attrNumber[j];
                            AddHeroSourceAttr(heroAttributeSkillSourceList, effect.attrTypeNew[j], effect.attrNumber[j]);
                        }
                    }
                }
                
                //装备属性加成
                Dictionary<int,int> composeMap = new Dictionary<int, int>();
                
                Array equipSubTypeList = Enum.GetValues(typeof(EquipSubType));
                for(int i = 0; i < equipSubTypeList.Length; i++)
                {
                    int equipIndex = hero.GetHeroEquipByType((int)equipSubTypeList.GetValue(i));
                    if (equipIndex <= 0) continue;
                    
                    var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    var heroEquipInfo = bagProxy.GetEquipItemInfo(equipIndex);
                    if (heroEquipInfo == null) continue;
                    var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(heroEquipInfo.ItemID);
                    float plus = -1;
                    if (heroEquipInfo.Exclusive != 0)
                    {
                        foreach (var talent in hero.config.talent)
                        {
                            var talentCfg = CoreUtils.dataService.QueryRecord<HeroTalentDefine>(talent);
                            if (talentCfg.type == heroEquipInfo.Exclusive)
                            {
                                var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                                plus = config.equipTalentPromote;
                                break;
                            }
                        }
                    }
                    
                    for (int j = 0; j < equipCfg.att.Count && j <equipCfg.attAddEx.Count; j++)
                    {
                        var attrCfg = CoreUtils.dataService.QueryRecord<EquipAttDefine>(equipCfg.att[j]);
                        int attrValue = equipCfg.attAddEx[j];
                        if (plus > 0)
                        {
                            attrValue += Mathf.RoundToInt(plus/10 * attrValue * 2) * 5;
                        }

                        heroAttribute[(int) attrCfg.attNew - 1].origvalue += (int)attrValue;
                    }

                    if (equipCfg.compose > 0)
                    {
                        if (composeMap.ContainsKey(equipCfg.compose))
                        {
                            composeMap[equipCfg.compose]++;
                        }
                        else
                        {
                            composeMap[equipCfg.compose] = 1;
                        }
                    }
                }

                //装备套装属性加成
                foreach (var keyValue in composeMap)
                {
                    var composeCfg = CoreUtils.dataService.QueryRecord<EquipComposeDefine>(keyValue.Key);
                    List<int> attrIdList = new List<int>();
                    List<int> attrAddList = new List<int>();
                    if (composeCfg.compose8 != null&& composeCfg.compose8.Count>0&&keyValue.Value>=8)
                    {
                        attrIdList = composeCfg.compose8;
                        attrAddList = composeCfg.compose8AddEx;
                    }
                    else if (composeCfg.compose6 != null && composeCfg.compose6.Count > 0 && keyValue.Value >= 6)
                    {
                        attrIdList = composeCfg.compose6;
                        attrAddList = composeCfg.compose6AddEx;
                    }
                    else if (composeCfg.compose4 != null && composeCfg.compose4.Count > 0 && keyValue.Value >= 4)
                    {
                        attrIdList = composeCfg.compose4;
                        attrAddList = composeCfg.compose4AddEx;
                    }
                    else if (composeCfg.compose2 != null && composeCfg.compose2.Count > 0 && keyValue.Value >= 2)
                    {
                        attrIdList = composeCfg.compose2;
                        attrAddList = composeCfg.compose2AddEx;
                    }
                    
                    for (int i = 0; i < attrIdList.Count; i++)
                    {
                        var attrCfg = CoreUtils.dataService.QueryRecord<EquipAttDefine>(attrIdList[i]);
                        heroAttribute[(int) attrCfg.attNew - 1].origvalue += (int) attrAddList[i];
                    }
                }
                
                //天赋属性加成
                Dictionary<int,int> treeIdMap = new Dictionary<int,int>();
                Dictionary<int,int> masteryIdMap = new Dictionary<int,int>();
                var talentTrees = hero.GetTalentTreesByIndex(hero.talentIndex);
                if (talentTrees != null && talentTrees.talentTree != null)
                {
                    var heroAttributeTalentSourceList = GetHeroSourceAttrList(id, EnumSourceAttr.HeroTalent);
                    foreach (var talent in talentTrees.talentTree)
                    {
                        var talentData = CoreUtils.dataService.QueryRecord<HeroTalentGainTreeDefine>((int)talent);
                        for(int i = 0; i < talentData.attrTypeNew.Count; i++)
                        {
                            if (talentData.attrTypeNew[i] != attrType.None)
                            {
                                heroAttribute[(int)talentData.attrTypeNew[i] - 1].origvalue += talentData.attrNumber[i];
                                AddHeroSourceAttr(heroAttributeTalentSourceList, talentData.attrTypeNew[i], talentData.attrNumber[i]);
                            }
                        }
                        if(treeIdMap.ContainsKey(talentData.gainTree))
                        {
                            treeIdMap[talentData.gainTree] += 1;
                        }
                        else
                        {
                            treeIdMap[talentData.gainTree] = 1;
                        }
                    }
                    
                    foreach (var talentId in hero.config.talent)
                    {
                        var talentDefine = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(talentId);
                        if(treeIdMap.ContainsKey(int.Parse(talentDefine.gainTree)))
                        {
                            masteryIdMap[talentDefine.masteryGroupID] = treeIdMap[int.Parse(talentDefine.gainTree)];
                        }
                        else
                        {
                            masteryIdMap[talentDefine.masteryGroupID] = 0;
                        }
                    }
                    
                    var masteryDefines = CoreUtils.dataService.QueryRecords<Data.HeroTalentMasteryDefine>();
                    foreach (var masteryDefine in masteryDefines)
                    {
                        if (masteryIdMap.ContainsKey(masteryDefine.group))
                        {
                            if (masteryDefine.needTalentPoint <= masteryIdMap[masteryDefine.group])
                            {
                                for (int i = 0; i < masteryDefine.attrTypeNew.Count; i++)
                                {
                                    if (masteryDefine.attrTypeNew[i] != attrType.None)
                                    {
                                        heroAttribute[(int) masteryDefine.attrTypeNew[i] - 1].origvalue += masteryDefine.attrNumber[i];
                                        AddHeroSourceAttr(heroAttributeTalentSourceList, masteryDefine.attrTypeNew[i], masteryDefine.attrNumber[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void BuildTroopAttribute(AttributeValue[] heroAttribute, long mainHeroId, long deputyHeroId)
        {
            var mainHeroAttributes = GetHeroAttribute(mainHeroId);
            var deputyHeroAttributes = GetHeroSourceAttrList(deputyHeroId, EnumSourceAttr.HeroSkill); // 改成覆盖属性来源
            var cityAttribute = m_cityAttributeTotal;

            for(int i = 0; i < heroAttribute.Length; i++)
            {
                var attributes = new AttributeValue(m_arrtibuteConfigs[i], 0);
                if(mainHeroAttributes[i] != null)
                {
                    attributes.origvalue += mainHeroAttributes[i].origvalue;
                }
                if (cityAttribute[i] != null)
                {
                    attributes.origvalue += cityAttribute[i].origvalue;
                }
                if (deputyHeroAttributes[i] != null)
                {
                    attributes.origvalue += deputyHeroAttributes[i].origvalue;
                }

                heroAttribute[i] = attributes;
            }
           
        }
    }
}