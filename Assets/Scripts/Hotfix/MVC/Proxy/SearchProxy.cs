// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    SearchProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using Data;
using Skyunion;
using UnityEngine;
using System;
using Client;

namespace Game
{
    public class SearchData
    {
        public int id;
        public string name;
        public string iconPath;
        public string des;
    }


    public enum SearchType
    {
        None = -1,

        /// <summary>
        /// 野蛮人
        /// </summary>
        Barbarian = 0,

        /// <summary>
        /// 农田
        /// </summary>
        Farmland = 1,

        /// <summary>
        /// "伐木场"
        /// </summary>
        Mill = 2,

        /// <summary>
        /// "石矿床"
        /// </summary>
        Stone = 3,

        /// <summary>
        /// 金矿床
        /// </summary>
        Gold = 4
    }


    public class SearchProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "SearchProxy";
        public static int searchCount = 5;
        public static int MaxBarbarianLevel = 25;
        public  static int MaxCurrLevel = 6;
        private Dictionary<int, SearchData> dicSearch;
        public SearchType searchType = SearchType.Barbarian;//暂存搜索类型
        public int currBarbarianLevel = 1;//暂存野蛮人等级
        public int currCurrLevel = 1;//暂存资源田等级
        #endregion

        // Use this for initialization
        public SearchProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" SearchProxy register");
            dicSearch = new Dictionary<int, SearchData>(5);
            Init();
        }


        public override void OnRemove()
        {
            Debug.Log(" SearchProxy remove");
            dicSearch.Clear();
        }

        public void SetValue()
        {
            int level = 0;
            int openTime = 0;
            int zone = 0;
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            DateTime currServerDateTime = ServerTimeModule.Instance.GetCurrServerDateTime();
            DateTime openDatatime = ServerTimeModule.Instance.ConverToServerDateTime(playerProxy.GetRoleLoginRes().openTime);

         //   Debug.LogError(currServerDateTime.ToString() + openDatatime.ToString());
            openTime = ClientUtils.TimeSpanDays(openDatatime, currServerDateTime);
            if (openTime <= 0)
            {
                openTime = 1;
            }
            zone = MapManager.Instance().GetMapZoneLevel(new Vector2( playerProxy.CurrentRoleInfo.pos.x/100, playerProxy.CurrentRoleInfo.pos.y/100));
            List<MonsterRefreshLevelDefine>  monsterRefreshLevelDefine = CoreUtils.dataService.QueryRecords<MonsterRefreshLevelDefine>();
            for (int i = 0; i < monsterRefreshLevelDefine.Count; i++)
            {
                if (monsterRefreshLevelDefine[i].zoneLevel == zone)
                {
                    if (monsterRefreshLevelDefine[i].monsterType == 1)
                    {
                        if (monsterRefreshLevelDefine[i].serverLevelMin <= openTime && monsterRefreshLevelDefine[i].serverLevelMax >= openTime)
                        {
                            if (monsterRefreshLevelDefine[i].monsterLevel > level)
                            {
                                level = monsterRefreshLevelDefine[i].monsterLevel;
                            }
                        }
                    }
                }
            }
            MaxBarbarianLevel = level % 1000;
           // Debug.LogError(openTime + "   " +  MaxBarbarianLevel);
        }

        private void Init()
        {
            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            for (int i = 0; i < 5; i++)
            {
                SearchData data = new SearchData();
                data.id = i;

                if (configDefine != null)
                {
                    switch (i)
                    {
                        case 0:
                            data.iconPath = configDefine.searchMonsterIcon;
                            data.des = LanguageUtils.getText(500302);
                            data.name = LanguageUtils.getText(500200);
                            break;
                        case 1:
                            data.iconPath = configDefine.searchFoodIcon;
                            data.des = LanguageUtils.getText(500303);
                            data.name = LanguageUtils.getText(500000);
                            break;
                        case 2:
                            data.iconPath = configDefine.searchWoodIcon;
                            data.des = LanguageUtils.getText(500304);
                            data.name = LanguageUtils.getText(500001);
                            break;
                        case  3:
                            data.iconPath = configDefine.searchStoneIcon;
                            data.des = LanguageUtils.getText(500305);
                            data.name = LanguageUtils.getText(500002);
                            break;
                        case 4:
                            data.iconPath = configDefine.searchGoldIcon;
                            data.des = LanguageUtils.getText(500306);
                            data.name = LanguageUtils.getText(500003);
                            break;
                                                  
                    }
                }

                dicSearch.Add(data.id, data);
            }
            MaxCurrLevel = configDefine.resourceGatherPointLevelMax;
        }

        public SearchData GetSearchData(int id)
        {
            SearchData data;
            if (dicSearch.TryGetValue(id, out data))
            {
                return data;
            }

            return null;
        }
        public SearchData GetSearchData(SearchType searchType)
        {
            SearchData data;
            if (dicSearch.TryGetValue((int)searchType, out data))
            {
                return data;
            }
            return null;
        }

        public string GetNameByType(SearchType searchType)
        {
            string name = string.Empty;
            SearchData data;
            if (dicSearch.TryGetValue((int)searchType, out data))
            {
                return data.name;
            }
            return name;
        }
    }
}