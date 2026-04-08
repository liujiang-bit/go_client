// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月26日
// Update Time         :    2019年12月26日
// Class Description   :    HeroProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game {
    public class HeroProxy : GameProxy {

        public enum SortType
        {
            None,
            Rare,
            Star,
            Level,
            Power,
            Recomend
        }
        public class Hero
        {
            public HeroInfoEntity data { get; private set; }
            public HeroDefine config { get; private set; }
            public HeroLevelDefine levelConfig { get; private set; }
            public ItemDefine itemConfig { get; private set; }
            public int itemCount
            {
                get
                {
                    var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    return (int)bagProxy.GetItemNum(config.getItem);
                }
            }
            public int power { get; private set; }
            public int level
            {
                get { return data != null ? (int)data.level : 1; }
            }
            public int exp
            {
                get { return data != null ? (int)data.exp : 0; }
            }
            public int star
            {
                get { return data != null ? (int)data.star : config.initStar; }
            }
            public int starExp
            {
                get { return data != null ? (int)data.starExp : 0; }
            }
            public int levelScore
            {
                get
                {
                    return levelConfig.score;
                }
            }
            public int baseScore
            {
                get
                {
                    return config.score;
                }
            }
            public int skillScore
            {
                get
                {
                    int power = 0;
                    if (data != null)
                    {
                        foreach (var skill in data.skills)
                        {
                            var effect = CoreUtils.dataService.QueryRecord<HeroSkillEffectDefine>((int)(skill.skillId * 1000 + skill.skillLevel));
                            if (effect == null)
                            {
                                Debug.LogError($"HeroSkillEffectDefine Not Found {skill.skillId * 1000 + skill.skillLevel}");
                                continue;
                            }
                            power += effect.score;
                        }
                    }
                    else
                    {
                        foreach (var skillID in config.skill)
                        {
                            var skill = CoreUtils.dataService.QueryRecord<HeroSkillDefine>((int)(skillID));
                            if(skill == null)
                            {
                                Debug.LogError($"HeroSkillDefine Not Found {skillID}");
                                Debug.LogError(skillID.ToString());
                                continue;
                            }
                            if(skill.open == 1)
                            {
                                var effect = CoreUtils.dataService.QueryRecord<HeroSkillEffectDefine>(skill.ID*1000+1);
                                if (effect == null)
                                {
                                    Debug.LogError($"HeroSkillEffectDefine Not Found {skill.ID * 1000 + 1}");
                                    continue;
                                }
                                power += effect.score;
                            }
                        }
                    }
                    return power;
                }
            }
            
            public int talentScore
            {
                get
                {
                    int score = 0;
                    Dictionary<int,int> treeIdMap = new Dictionary<int,int>();
                    Dictionary<int,int> masteryIdMap = new Dictionary<int,int>();
                    var talentTrees = GetTalentTreesByIndex(talentIndex);
                    if (talentTrees != null && talentTrees.talentTree != null)
                    {
                        foreach (var talent in talentTrees.talentTree)
                        {
                            var talentData = CoreUtils.dataService.QueryRecord<HeroTalentGainTreeDefine>((int)talent);
                            score += talentData.score;
                            if(treeIdMap.ContainsKey(talentData.gainTree))
                            {
                                treeIdMap[talentData.gainTree] += 1;
                            }
                            else
                            {
                                treeIdMap[talentData.gainTree] = 1;
                            }
                        }
                        
                        foreach (var talentId in config.talent)
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
                                    score += masteryDefine.score;
                                }
                            }
                        }
                    }

                    return score;
                }
            }

            public int talentIndex
            {
                get { return data !=null?(int) data.talentIndex:0; }
            }

            public Hero(HeroDefine config)
            {
                this.config = config;
                power = config.score;
                levelConfig = CoreUtils.dataService.QueryRecord<HeroLevelDefine>((int)level + config.rare * 10000 - 1);
                itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>((int)config.getItem);
                UpdatePower();
            }
            public void UpdatePower()
            {
                power = baseScore;
                power += levelScore;
                power += skillScore;
                if (data != null)
                {
                    power += talentScore;
                }
            }
            public bool CanSummon()
            {
                return data == null && itemCount >= config.getItemNum;
            }
            public void UpdateData(HeroInfoEntity data)
            {
                this.data = data;
                levelConfig = CoreUtils.dataService.QueryRecord<HeroLevelDefine>((int)data.level + config.rare * 10000 - 1);
                UpdatePower();
            }
            
            public int GetSkillAllLevel()
            {
                int allLevel = 0;
                if(data != null)
                {
                    foreach (var skillInfo in data.skills)
                    {
                        allLevel += (int)skillInfo.skillLevel;
                    }
                }                
                return allLevel;
            }
            
            public bool IsAllUnlockSkillMax()
            {
                if (data == null) return false;
                foreach (var skillInfo in data.skills)
                {
                    if (skillInfo.skillLevel > 0 && skillInfo.skillLevel < 5)
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool IsCanUpSkill()
            {
                if (data == null) return false;
                if (IsAllUnlockSkillMax()) return false;
                if (IsAllSkillMax()) return false;
                var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(config.getItem);
                if (itemCfg == null) return false;
                BagProxy bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                long itemCount = bagProxy.GetItemNum(itemCfg.ID);
                int costItemCount = GetSkillLevelUpCostItemNum();
                return itemCount >= costItemCount;
            }

            private int m_nTotalTalenPoint = 0;
            public int GetTotalTalentPoint()
            {
                if (m_nTotalTalenPoint > 0)
                    return m_nTotalTalenPoint;
                int point = 0;
                var levelDatas = CoreUtils.dataService.QueryRecords<HeroLevelDefine>();
                for (int i =0; i < levelDatas.Count; i++)
                {
                    var levelData = levelDatas[i];
                    if (levelData.rareGroup == config.rare && levelData.lv <= data.level && levelData.starEffectData > 0)
                        point += levelData.starEffectData;
                }

                var starDatas = CoreUtils.dataService.QueryRecords<HeroStarDefine>();
                for(int i = 0; i < starDatas.Count; i++)
                {
                    var starData = starDatas[i];
                    if (starData.ID <= data.star && starData.starEffectData > 0)
                        point += starData.starEffectData;
                }
                m_nTotalTalenPoint = point;
                return m_nTotalTalenPoint;
            }

            public int GetCurPageRemainPoint(int index)
            {
                int remainPoint = 0;
                HeroInfo.TalentTrees talentTrees = GetTalentTreesByIndex(index);
                int usePoint = 0;
                if (talentTrees != null)
                {
                    usePoint = talentTrees.talentTree.Count();
                }
                remainPoint = GetTotalTalentPoint() - usePoint;
                return remainPoint;
            }

            public HeroInfo.TalentTrees GetTalentTreesByIndex(int index)
            {
                if (data == null || data.talentTrees == null || !data.talentTrees.ContainsKey(index)) return null;
                return data.talentTrees[index];
            }

            public int GetTalentMasteryPointByIndex(int index,int gainTree)
            {
                int masteryPoint = 0;
                HeroInfo.TalentTrees talentTrees = GetTalentTreesByIndex(index);
                if (talentTrees != null)
                {
                    foreach (var talent in talentTrees.talentTree)
                    {
                        var talentData = CoreUtils.dataService.QueryRecord<HeroTalentGainTreeDefine>((int)talent);
                        if (talentData.gainTree == gainTree)
                        {
                            masteryPoint++;
                        }
                    }
                }

                return masteryPoint;
            }

            public int GetLevelByIndex(int index)
            {
                HeroInfo.TalentTrees talentTrees = GetTalentTreesByIndex(index);
                if (talentTrees != null)
                {
                    return talentTrees.talentTree.Count();
                }

                return 0;
            }

            public List<HeroTalentDefine> GetTalentDefines()
            {
                List<HeroTalentDefine> talentDefineList = new List<HeroTalentDefine>();
                var talentDatas = config.talent;
                for (int i = 0; i < talentDatas.Count; i++)
                {
                    var talentDefine = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(talentDatas[i]);
                    talentDefineList.Insert(talentDefineList.Count,talentDefine);
                }

                return talentDefineList;
            }

            public List<int> GetActiveTalentCountByIndex(int index)
            {
                List<int> counts = new List<int>{0,0,0};
                var talentDatas = GetTalentDefines();
                var talentTrees = GetTalentTreesByIndex(index);
                if (talentTrees != null)
                {
                    foreach (var talent in talentTrees.talentTree)
                    {
                        var talentData = CoreUtils.dataService.QueryRecord<HeroTalentGainTreeDefine>((int)talent);
                        if (talentData.gainTree == int.Parse(talentDatas[0].gainTree))
                        {
                            counts[0]++;
                        }
                        if (talentData.gainTree == int.Parse(talentDatas[1].gainTree))
                        {
                            counts[1]++;
                        }
                        if (talentData.gainTree == int.Parse(talentDatas[2].gainTree))
                        {
                            counts[2]++;
                        }
                    }   
                }

                return counts;
            }

            public int GetHeroEquipByType(int type)
            {
                if (data == null) return 0;
                    
                switch ((EquipSubType)type)
                {
                    case EquipSubType.Head:
                        return (int)data.head;
                    case EquipSubType.BreastPlate:
                        return (int)data.breastPlate;
                    case EquipSubType.Weapon:
                        return (int)data.weapon;
                    case EquipSubType.Gloves:
                        return (int)data.gloves;
                    case EquipSubType.Pants:
                        return (int)data.pants;
                    case EquipSubType.accessories1:
                        return (int)data.accessories1;
                    case EquipSubType.accessories2:
                        return (int)data.accessories2;
                    case EquipSubType.shoes:
                        return (int) data.shoes;
                    default:
                        return 0;
                }
            }

            public bool IsAllSkillMax()
            {
                return GetSkillAllLevel() >= 20;
            }

            public int GetSkillCount()
            {
                if(data == null)
                {
                    return 0;
                }
                return data.skills.Count;
            }
            public int GetSkillLevelUpCostItemNum()
            {
                int id = 100 + GetSkillAllLevel() - GetSkillCount();
                var record = CoreUtils.dataService.QueryRecord<Data.HeroSkillLevelDefine>(id);
                if (record == null) return 0;
                int costItem = 0;
                switch ((EnumRareType)config.rare)
                {
                    case EnumRareType.White:
                        {
                            costItem = record.costItem1;
                        }
                        break;
                    case EnumRareType.Green:
                        {
                            costItem = record.costItem2;
                        }
                        break;
                    case EnumRareType.Blue:
                        {
                            costItem = record.costItem3;
                        }
                        break;
                    case EnumRareType.Purple:
                        {
                            costItem = record.costItem4;
                        }
                        break;
                    case EnumRareType.Orange:
                        {
                            costItem = record.costItem5;
                        }
                        break;
                }
                return costItem;
            }

            public bool IsAwakening()
            {
                if (IsAllSkillMax() && config.skill.Count >= 5)
                {
                    return true;
                }

                return false;
            }

            public bool IsLevelLimitByStar()
            {
                var starConfig = CoreUtils.dataService.QueryRecord<HeroStarDefine>((int)data.star);
                if (starConfig == null) return true;
                return data.level >= starConfig.starLimit;
            }

            public bool IsMaxLevel()
            {
                if (levelConfig == null) return true;
                return levelConfig.exp == 0;
            }

            public bool IsStarMaxLevel()
            {
                return star >= config.star;
            }
            
        }
        #region Member
        public const string ProxyNAME = "HeroProxy";
        Dictionary<long, Hero> mHeroDic = new Dictionary<long, Hero>();
        private bool mFirstGetData = true;
        private bool mHeroDirty = true;
        private SortType m_heroSortType = SortType.None;
        private List<Hero> m_ownHero = new List<Hero>();
        private List<Hero> m_summonHero = new List<Hero>();
        private List<Hero> m_noSummonHero = new List<Hero>();
        #endregion

        // Use this for initialization
        public HeroProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            //AnalysisHero();
            //{
            //    var hero = GetHeroByID(1013);
            //    var data = new HeroInfoEntity();
            //    data.exp = 1500000;
            //    data.heroId = hero.config.ID;
            //    data.level = 45;
            //    data.savageKillNum = 123;
            //    data.skills = new List<SprotoType.SkillInfo>();
            //    data.soldierKillNum = 456;
            //    data.star = 4;
            //    data.starExp = 60000;
            //    data.summonTime = DateTime.Now.ToFileTime();
            //    hero.UpdateData(data);
            //}
            //{
            //    var hero = GetHeroByID(2007);
            //    var data = new HeroInfoEntity();
            //    data.exp = 1500000;
            //    data.heroId = hero.config.ID;
            //    data.level = 43;
            //    data.savageKillNum = 123;
            //    data.skills = new List<SprotoType.SkillInfo>();
            //    data.soldierKillNum = 456;
            //    data.star = 5;
            //    data.starExp = 60000;
            //    data.summonTime = DateTime.Now.ToFileTime();
            //    hero.UpdateData(data);
            //}
        }


        public override void OnRemove()
        {
            mHeroDic.Clear();
        }

        public void UpdateHeroInfo(INotification notification)
        {
            Hero_HeroInfo.request heroInfos = notification.Body as Hero_HeroInfo.request;

            if (heroInfos.HasHeroInfo)
            {
                foreach(var data in heroInfos.heroInfo)
                {
                    var hero = GetHeroByID(data.Key);
                    var entity = hero.data;
                    bool bNewHero = false;
                    if (entity == null)
                    {
                        entity = new HeroInfoEntity();
                        bNewHero = true;
                    }
                    if(!mFirstGetData && !bNewHero)
                    {
                        if (hero.level < data.Value.level)
                        {
                            CoreUtils.audioService.PlayOneShot(RS.SoundUiCaptainLvUp);
                            AppFacade.GetInstance().SendNotification(CmdConstant.FightUpdateHeroLevel, data.Value.heroId);
                        }
                    }
                    HeroInfoEntity.updateEntity(entity, data.Value);
                    hero.UpdateData(entity);
                    if(bNewHero && !mFirstGetData && !heroInfos.HasNoShow)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.GetNewHero, data.Key);
                    }
                    else
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateHero, entity);
                    }
                }
            }

            mHeroDirty = true;
            mFirstGetData = false;
        }

        public void SummonHero(int id)
        {
            Hero_SummonHero.request req = new Hero_SummonHero.request();
            req.heroId = id;
            AppFacade.GetInstance().SendSproto(req);
        }


        public void AnalysisHero()
        {
            if (mHeroDic.Count > 0)
                return;

            mHeroDic.Clear();
            var heros = CoreUtils.dataService.QueryRecords<HeroDefine>();
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            for (int i = 0; i < heros.Count; i++)
            {
                var hero = new Hero(heros[i]);
                hero.UpdatePower();
                mHeroDic.Add(hero.config.ID, hero);
            }
        }

        public Hero GetHeroByID(long heroId)
        {
            AnalysisHero();
            Hero hero = null;
            mHeroDic.TryGetValue(heroId, out hero);
            return hero;
        }

        public long GetHeroTotalPower()
        {
            AnalysisHero();
            long power = 0;
            foreach (var hero in mHeroDic.Values)
            {
                if (hero.config.listDisplay == 1)
                    continue;
                if (hero.data != null)
                {
                    power += hero.power;
                }
            }
            return power;
        }

        public int CompareHeroByStar(Hero x, Hero y)
        {
            long xScore = x.data != null ? 1 : (x.itemCount < x.config.getItemNum ? 0 : 2);
            long yScore = y.data != null ? 1 : (y.itemCount < y.config.getItemNum ? 0 : 2);

            if (xScore > yScore) return 1;
            if (xScore < yScore) return -1;

            if (x.data != null)
            {
                if (x.data.star < y.data.star) return -1;
                if (x.data.star > y.data.star) return 1;

                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.data.level < y.data.level) return -1;
                if (x.data.level > y.data.level) return 1;

                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;

                if (x.data.summonTime < y.data.summonTime) return -1;
                if (x.data.summonTime > y.data.summonTime) return 1;
            }
            else
            {
                if (x.config.initStar < y.config.initStar) return -1;
                if (x.config.initStar > y.config.initStar) return 1;

                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;
            }

            return 0;
        }
        public int CompareHeroByLevel(Hero x, Hero y)
        {
            long xScore = x.data != null ? 1 : (x.itemCount < x.config.getItemNum ? 0 : 2);
            long yScore = y.data != null ? 1 : (y.itemCount < y.config.getItemNum ? 0 : 2);

            if (xScore > yScore) return 1;
            if (xScore < yScore) return -1;

            if (x.data != null)
            {
                if (x.data.level < y.data.level) return -1;
                if (x.data.level > y.data.level) return 1;

                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.data.star < y.data.star) return -1;
                if (x.data.star > y.data.star) return 1;

                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;

                if (x.data.summonTime < y.data.summonTime) return -1;
                if (x.data.summonTime > y.data.summonTime) return 1;
            }
            else
            {
                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.config.initStar < y.config.initStar) return -1;
                if (x.config.initStar > y.config.initStar) return 1;

                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;
            }

            return 0;
        }
        public int CompareHeroByPower(Hero x, Hero y)
        {
            long xScore = x.data != null ? 1 : (x.itemCount < x.config.getItemNum ? 0 : 2);
            long yScore = y.data != null ? 1 : (y.itemCount < y.config.getItemNum ? 0 : 2);

            if (xScore > yScore) return 1;
            if (xScore < yScore) return -1;

            if (x.data != null)
            {
                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;

                if (x.data.level < y.data.level) return -1;
                if (x.data.level > y.data.level) return 1;

                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.data.star < y.data.star) return -1;
                if (x.data.star > y.data.star) return 1;

                if (x.data.summonTime < y.data.summonTime) return -1;
                if (x.data.summonTime > y.data.summonTime) return 1;
            }
            else
            {
                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;

                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.config.initStar < y.config.initStar) return -1;
                if (x.config.initStar > y.config.initStar) return 1;
            }

            return 0;
        }

        public int CompareHeroByRare(Hero x, Hero y)
        {
            long xScore = x.data != null ? 1 : (x.itemCount < x.config.getItemNum ? 0 : 2);
            long yScore = y.data != null ? 1 : (y.itemCount < y.config.getItemNum ? 0 : 2);

            if (xScore > yScore) return 1;
            if (xScore < yScore) return -1;

            if (x.data != null)
            {
                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.data.star < y.data.star) return -1;
                if (x.data.star > y.data.star) return 1;

                if (x.data.level < y.data.level) return -1;
                if (x.data.level > y.data.level) return 1;

                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;

                if (x.data.summonTime < y.data.summonTime) return -1;
                if (x.data.summonTime > y.data.summonTime) return 1;
            }
            else
            {
                if (x.config.rare < y.config.rare) return -1;
                if (x.config.rare > y.config.rare) return 1;

                if (x.config.initStar < y.config.initStar) return -1;
                if (x.config.initStar > y.config.initStar) return 1;

                if (x.power < y.power) return -1;
                if (x.power > y.power) return 1;
            }

            return 0;
        }

        public void SortHeros(List<Hero> heros, SortType type)
        {
            switch (type)
            {
                case SortType.Rare:
                    heros.Sort(CompareHeroByRare);
                    break;
                case SortType.Star:
                    heros.Sort(CompareHeroByStar);
                    break;
                case SortType.Level:
                    heros.Sort(CompareHeroByLevel);
                    break;
                case SortType.Power:
                    heros.Sort(CompareHeroByPower);
                    break; 
            }

            heros.Reverse();
        }

        public void GetHerosBySort(out List<Hero> own, out List<Hero> summon, out List<Hero> nosummon, SortType type)
        {
            if(type == m_heroSortType && mHeroDirty == false)
            {
                own = m_ownHero;
                summon = m_summonHero;
                nosummon = m_noSummonHero;
                return;
            }
            mHeroDirty = false;
            m_heroSortType = type;
            AnalysisHero();
            var heros = mHeroDic.Values.ToList();
            switch (type)
            {
                case SortType.Rare:
                    heros.Sort(CompareHeroByRare);
                    break;
                case SortType.Star:
                    heros.Sort(CompareHeroByStar);
                    break;
                case SortType.Level:
                    heros.Sort(CompareHeroByLevel);
                    break;
                case SortType.Power:
                    heros.Sort(CompareHeroByPower);
                    break;
            }
            heros.Reverse();

            own = m_ownHero = own = new List<Hero>();
            summon = m_summonHero = new List<Hero>();
            nosummon = m_noSummonHero = new List<Hero>();

            for (int i = 0; i < heros.Count; i++)
            {
                var hero = heros[i];
                if (hero.config.listDisplay == 1)
                    continue;
                if(hero.data != null)
                {
                    own.Add(hero);
                }
                else
                {
                    if(hero.itemCount < hero.config.getItemNum)
                    {
                        nosummon.Add(hero);
                    }
                    else
                    {
                        summon.Add(hero);
                    }
                }
            }
        }

        public List<Hero> GetSummonerHeros()
        {
            List<Hero> heros = new List<Hero>();
            foreach(var kv in mHeroDic)
            {
                if(kv.Value.data != null)
                {
                    heros.Add(kv.Value);
                }
            }
            return heros;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCanSummonerHeroCount()
        {
            int count = 0;
            foreach (var kv in mHeroDic)
            {
                HeroProxy.Hero hero = kv.Value;
                if(hero.config.listDisplay == 1)
                {
                    continue;
                 }
                if (hero.data != null)
                {
                    if (hero.IsCanUpSkill())
                    {
                        count++;
                       // Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "升技能");
                    }
                    var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(hero.star);
                    if (heroStarCfg != null)
                    {
                        if (hero.level == heroStarCfg.starLimit&& !hero.IsStarMaxLevel())
                        {
                            count++;
                          //  Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "升星");

                        }
                    }
                    int talentPoint = hero.GetCurPageRemainPoint(hero.talentIndex);
                    if (talentPoint > 0)
                    {
                        count++;
                      //  Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "升天赋");

                    }
                }
                else
                {
                    if (kv.Value.config.getItem > 0 && kv.Value.itemCount >= kv.Value.config.getItemNum)
                    {
                        count++;
                        //Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "召唤");
                    }
                }
        
            }
            return count;
        }
        /// <summary>
        /// 当统帅有技能可升级时，红点数量+1
        /// </summary>
        /// <returns></returns>
        public int GetCanSkillUpHeroCount()
        {
            int count = 0;
            foreach (var kv in mHeroDic)
            {
                HeroProxy.Hero hero = kv.Value;
                if (hero.config.listDisplay == 1)
                {
                    continue;
                }
                if (hero.data != null)
                {
                    if (hero.IsCanUpSkill())
                    {
                        count++;
                        // Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "升技能");
                    }
                    var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(hero.star);
                    if (heroStarCfg != null)
                    {
                        if (hero.level == heroStarCfg.starLimit && !hero.IsStarMaxLevel())
                        {
                            count++;
                            //  Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "升星");

                        }
                    }
                    int talentPoint = hero.GetCurPageRemainPoint(hero.talentIndex);
                    if (talentPoint > 0)
                    {
                        count++;
                        //  Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "升天赋");

                    }
                }
                else
                {
                    if (kv.Value.config.getItem > 0 && kv.Value.itemCount >= kv.Value.config.getItemNum)
                    {
                        count++;
                        //Debug.LogError(LanguageUtils.getText(hero.config.l_nameID) + "召唤");
                    }
                }

            }
            return count;
        }
        /// <summary>
        /// 当统帅可升星时，红点数量+1
        /// </summary>
        /// <returns></returns>
        public int GetHeroStarCount()
        {
            int count = 0;
            return count;
        }
        /// <summary>
        /// 当统帅有天赋点未分配时，红点数量+1
        /// </summary>
        /// <returns></returns>
        public int GetCantalentPointHeroCCount()
        {
            int count = 0;
            return count;
        }
    }
}