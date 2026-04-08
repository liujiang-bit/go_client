// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月8日
// Update Time         :    2020年1月8日
// Class Description   :    MonsterProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Data;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public enum ItemType
    {
        None = 0,
        Item,
        Currency
    }

    public class MonsterDataUIData
    {
        public int id;
        public List<ItemType> type = new List<ItemType>(5);
        public List<ItemDefine> itemDefine = new List<ItemDefine>(5);
        public List<CurrencyDefine> currencyDefine = new List<CurrencyDefine>(5);
        public List<int> num = new List<int>();
    }

    public class MonsterProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "MonsterProxy";
        private TroopProxy m_TroopProxy;
        private List<ItemPackageShowDefine> lsItemPackageDefines = new List<ItemPackageShowDefine>();
        private Dictionary<int, MonsterDataUIData> dicMonsterDataUIData;
        private int levelAttackMonster;
        private WorldMapObjectProxy m_worldProxy;

        #endregion

        // Use this for initialization
        public MonsterProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" MonsterProxy register");
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            lsItemPackageDefines = CoreUtils.dataService.QueryRecords<ItemPackageShowDefine>();
            dicMonsterDataUIData = new Dictionary<int, MonsterDataUIData>();                       
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }


        public override void OnRemove()
        {
            Debug.Log(" MonsterProxy remove");
            dicMonsterDataUIData.Clear();
        }

        private void Remove(int id)
        {
            dicMonsterDataUIData.Remove(id);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveMonsterFightHud, id);
        }


        public MonsterDataUIData GetMonsterDataUIData(int id)
        {
            MonsterDataUIData dataUiData;
            if (dicMonsterDataUIData.TryGetValue(id, out dataUiData))
            {
                return dataUiData;
            }

            MapObjectInfoEntity wobj = m_worldProxy.GetWorldMapObjectByobjectId(id);
            dataUiData = new MonsterDataUIData();
            dataUiData.id = id;
            dataUiData.type.Clear();
            dataUiData.itemDefine.Clear();
            dataUiData.currencyDefine.Clear();
            dataUiData.num.Clear();
            foreach (var info in lsItemPackageDefines)
            {
                if (info.group == wobj.monsterDefine.showReward)
                {
                    dataUiData.num.Add(info.number);
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(info.typeData);
                    if (itemDefine != null)
                    {
                        dataUiData.itemDefine.Add(itemDefine);
                        dataUiData.type.Add(ItemType.Item);
                    }
                    else
                    {
                        dataUiData.itemDefine.Add(null);
                    }

                    CurrencyDefine currencyDefine =
                        CoreUtils.dataService.QueryRecord<CurrencyDefine>(info.typeData);
                    if (currencyDefine != null)
                    {
                        dataUiData.currencyDefine.Add(currencyDefine);
                        dataUiData.type.Add(ItemType.Currency);
                    }
                    else
                    {
                        dataUiData.currencyDefine.Add(null);
                    }
                }
            }

            dicMonsterDataUIData.Add(id, dataUiData);

            return dataUiData;
        }

        public float GetMoveSpeed(Vector3 startV3, Vector3 endV3, long time)
        {
            float distance = Vector3.Distance(startV3, endV3);
            long curtime = ServerTimeModule.Instance.GetServerTime();
            long times = time - curtime;
            return distance / times;
        }
    }
}