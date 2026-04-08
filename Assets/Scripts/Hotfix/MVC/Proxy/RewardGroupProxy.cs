// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    RewardGroupProxy 奖励组显示规则
// Copyright IGG All rights reserved.
// ===============================================================================

using Client;
using Data;
using Skyunion;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;
using System;

namespace Game {

    public enum EnumRewardType
    {
        /// <summary>
        /// 货币
        /// </summary>
        Currency = 100,
        /// <summary>
        /// 道具
        /// </summary> 
        Item = 200,
        /// <summary>
        /// 士兵
        /// </summary> 
        Soldier = 300,
        /// <summary>
        /// 联盟礼包
        /// </summary> 
        AllianceGift = 600,
        /// <summary>
        /// 统帅
        /// </summary>
        Hero = 400,
    }

    public class RewardCurrencyData
    {
        public int ID;
        public int l_desID;
        public string iconID;
        public CurrencyDefine currencyDefine;
    }

    public class RewardItemData
    {
        public int ID;
        public int l_nameID;
        public int l_tipsID;
        public string itemIcon;
        public int quality;
        public string qualityIcon;
        public int l_topID;
        public int topData;
        public string topFormat;
        public string descFormat;
        public ItemDefine itemDefine;
    }

    public class RewardSoldierData
    {
        public int ID;
        public int l_desID;
        public string icon;
        public ArmsDefine armsDefine;
    }

    public class RewardAllianceGift
    {
        public int ID;
        public string iconImg;
        public int l_desc;
    }
    public class RewardHeroData
    {
        public int ID;
        public string iconImg;
        public HeroDefine HeroDefine;
    }  

    public class RewardGroupData
    {
        public int RewardType;
        public RewardCurrencyData CurrencyData;
        public RewardItemData ItemData;
        public RewardSoldierData SoldierData;
        public RewardAllianceGift AllianceGiftData;
        public RewardHeroData HeroData;
        public int name;
        public Int64 number;
        public int Id;
    }

    public class RewardGroupProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "RewardGroupProxy";

        private PlayerProxy m_playerProxy;

        #endregion

        // Use this for initialization
        public RewardGroupProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" RewardGroupProxy register");
        }


        public override void OnRemove()
        {
            Debug.Log(" RewardGroupProxy remove");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        //获取奖励组数据
        public List<RewardGroupData> GetRewardDataByGroup(int groupId, bool sort = false, bool isCivilization = true)
        {
            int civilization = -1;
            if (isCivilization)
            {
                civilization = (int)m_playerProxy.GetCivilization();
            }
            List<RewardGroupData> list = new List<RewardGroupData>();
            ItemPackageShowDefine rewardDefine = null;
            int initId = groupId * 1000;
            for (int i = 1; i < 1000; i++)
            {
                rewardDefine = CoreUtils.dataService.QueryRecord<ItemPackageShowDefine>(initId + i);
                if (rewardDefine == null)
                {
                    break;
                }
                //文明限制
                if (civilization >0 && rewardDefine.civilization_limit != null && rewardDefine.civilization_limit.Count > 0)
                {
                    if (!rewardDefine.civilization_limit.Contains(civilization))
                    {
                        continue;
                    }
                }

                RewardGroupData data = ConvertRewardGroupData(rewardDefine.type, rewardDefine.typeData, rewardDefine.number);
                data.RewardType = rewardDefine.type;
                data.Id = rewardDefine.ID;
                list.Add(data);
            }
            if (sort)
            {
                list.Sort(delegate (RewardGroupData x, RewardGroupData y)
                {
                    int re = x.RewardType.CompareTo(y.RewardType);
                    if (re == 0)
                    {
                        if (y.RewardType == (int)EnumRewardType.Currency)
                        {
                            re = x.CurrencyData.ID.CompareTo(y.CurrencyData.ID);
                        }
                    }
                    return re;
                });
            }
            return list;
        }

        //获取奖励组数据
        public List<RewardGroupData> GetChoiceRewardDataByGroup(int groupId, bool sort = false)
        {
            List<RewardGroupData> list = new List<RewardGroupData>();
            ItemRewardChoiceDefine rewardDefine = null;
            int initId = groupId * 1000;
            for (int i = 1; i < 1000; i++)
            {
                rewardDefine = CoreUtils.dataService.QueryRecord<ItemRewardChoiceDefine>(initId + i);
                if (rewardDefine == null)
                {
                    break;
                }
             
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Item, rewardDefine.item, rewardDefine.num);
                data.RewardType = (int)EnumRewardType.Item;
                data.Id = rewardDefine.ID;
                list.Add(data);
            }
            if (sort)
            {
                list.Sort(delegate (RewardGroupData x, RewardGroupData y)
                {
                    int re = x.RewardType.CompareTo(y.RewardType);
                    if (re == 0)
                    {
                        if (y.RewardType == (int)EnumRewardType.Currency)
                        {
                            re = x.CurrencyData.ID.CompareTo(y.CurrencyData.ID);
                        }
                    }
                    return re;
                });
            }
            return list;
        }

        //转换数据
        public RewardGroupData ConvertRewardGroupData(int type, int id, Int64 num)
        {
            RewardGroupData data = new RewardGroupData();
            data.RewardType = type;
            data.number = num;
            if (type == (int)EnumRewardType.Currency) //货币
            {
                RewardCurrencyData currencyData = new RewardCurrencyData();
                CurrencyDefine define = CoreUtils.dataService.QueryRecord<CurrencyDefine>(id);
                if (define != null)
                {
                    currencyData.ID = id;
                    currencyData.l_desID = define.l_desID;
                    currencyData.iconID = define.iconID;
                    currencyData.currencyDefine = define;
                    data.name = currencyData.l_desID;
                }
                else
                {
                    Debug.LogFormat("CurrencyDefine not find:{0}", id);
                }
                data.CurrencyData = currencyData;
            }
            else if (type == (int)EnumRewardType.Item) //道具
            {
                RewardItemData itemData = new RewardItemData();
                ItemDefine define = CoreUtils.dataService.QueryRecord<ItemDefine>(id);
                if (define != null)
                {
                    itemData.ID = id;
                    itemData.l_nameID = define.l_nameID;
                    itemData.l_tipsID = define.l_tipsID;
                    itemData.itemIcon = define.itemIcon;
                    itemData.quality = define.quality;
                    itemData.l_topID = define.l_topID;
                    itemData.topData = define.topData;
                    itemData.topFormat = LanguageUtils.getTextFormat(define.l_topID, ClientUtils.FormatComma(define.topData));
                    itemData.descFormat = LanguageUtils.getTextFormat (define.l_desID, ClientUtils.FormatComma(define.desData1), ClientUtils.FormatComma(define.desData2));
                  
                    itemData.itemDefine = define;
                    data.name = define.l_nameID;
                    num = (define.quality - 1);
                    if (num > -1 && num < RS.ItemQualityBg.Length)
                    {
                        itemData.qualityIcon = RS.ItemQualityBg[num];
                    }
                    else
                    {
                        itemData.qualityIcon = "";
                    }
                }
                else
                {
                    Debug.LogFormat("ItemDefine not find:{0}", id);
                }
                data.ItemData = itemData;
            }
            else if (type == (int)EnumRewardType.Soldier)//士兵
            {
                RewardSoldierData soldierData = new RewardSoldierData();
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (define != null)
                {
                    soldierData.ID = id;
                    soldierData.l_desID = define.l_desID;
                    soldierData.icon = define.icon;
                    soldierData.armsDefine = define;
                    data.name = define.l_armsID;
                }
                else
                {
                    Debug.LogFormat("ArmsDefine not find:{0}", id);
                }
                data.SoldierData = soldierData;
            }
            else if (type == (int)EnumRewardType.AllianceGift) //联盟礼包
            {
                RewardAllianceGift allianceGift = new RewardAllianceGift();
                AllianceGiftTypeDefine define = CoreUtils.dataService.QueryRecord<AllianceGiftTypeDefine>(id);
                if (define != null)
                {
                    allianceGift.ID = id;
                    allianceGift.l_desc = define.l_desc;
                    allianceGift.iconImg = define.iconImg;
                    data.name = define.l_nameId;
                }
                data.AllianceGiftData = allianceGift;
            }
            else if (type == (int)EnumRewardType.Hero)//英雄
            {
                RewardHeroData heroData = new RewardHeroData();
                HeroDefine define = CoreUtils.dataService.QueryRecord<HeroDefine>(id);
                if (define != null)
                {
                    heroData.ID = id;
                    heroData.HeroDefine = define;
                    heroData.iconImg = define.heroIcon;
                }
                data.HeroData = heroData;
            }

            return data;
        }

        /// <summary>
        /// 获取奖励组数据 仅处理士兵与道具其余待处理
        /// </summary>
        /// <param name="rewardInfo"></param>
        /// <returns></returns>
        public List<RewardGroupData> GetRewardDataByRewardInfo(RewardInfo rewardInfo)
        {
            List<RewardGroupData> list = new List<RewardGroupData>();

            if (rewardInfo.HasFood && rewardInfo.food > 0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.food, rewardInfo.food);
                list.Add(data);
            }

            if (rewardInfo.HasWood && rewardInfo.wood > 0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.wood, rewardInfo.wood);
                list.Add(data);
            }

            if (rewardInfo.HasStone && rewardInfo.stone > 0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.stone, rewardInfo.stone);
                list.Add(data);
            }

            if (rewardInfo.HasGold && rewardInfo.gold > 0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.gold, rewardInfo.gold);
                list.Add(data);
            }

            if (rewardInfo.HasDenar && rewardInfo.denar > 0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.denar, rewardInfo.denar);
                list.Add(data);
            }

            if (rewardInfo.HasItems && rewardInfo.items!=null)
            {
                foreach (var item in rewardInfo.items)
                {
                    RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Item, (int)item.itemId, (int)item.itemNum);
                    list.Add(data);
                }
            }

            if (rewardInfo.HasSoldiers && rewardInfo.soldiers!=null)
            {
                foreach (var soldier in rewardInfo.soldiers)
                {
                    RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Soldier, (int)soldier.id, (int)soldier.num);
                    list.Add(data);
                }
            }
            if (rewardInfo.HasHeros && rewardInfo.heros!=null)
            {
                foreach (var hero in rewardInfo.heros)
                {
                    RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Hero, (int)hero.heroId, (int)hero.num);
                    list.Add(data);
                }
            }
            if(rewardInfo.HasExpeditionCoin && rewardInfo.expeditionCoin>0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.conquerorMedal, rewardInfo.expeditionCoin);
                list.Add(data);
            }

            if (rewardInfo.HasGuildPoint && rewardInfo.guildPoint>0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.individualPoints, rewardInfo.guildPoint);
                list.Add(data);
            }

            if (rewardInfo.HasVip && rewardInfo.vip>0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.vipPoint, rewardInfo.vip);
                list.Add(data);
            }

            if (rewardInfo.HasLeaguePoints && rewardInfo.leaguePoints>0)
            {
                RewardGroupData data = ConvertRewardGroupData((int)EnumRewardType.Currency, (int)EnumCurrencyType.leaguePoints, rewardInfo.leaguePoints);
                list.Add(data);
            }

            //Todo:其余资源处理

            return list;
        }

        public List<Data.ItemPackageDefine> GetRewardByGroupId(int groupId)
        {
            List<Data.ItemPackageDefine> rewards = new List<ItemPackageDefine>();
            int initId = groupId * 1000;
            for (int i = 1; i < 1000; i++)
            {
                var cfg = CoreUtils.dataService.QueryRecord<ItemPackageDefine>(initId + i);
                if (cfg == null)
                {
                    break;
                }
                rewards.Add(cfg);
            }            
            return rewards;
        }
    }
}