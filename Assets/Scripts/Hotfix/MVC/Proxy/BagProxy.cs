// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    BagProxy 背包
// Copyright IGG All rights reserved.
// ===============================================================================

using PureMVC.Interfaces;
using SprotoType;
using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Skyunion;
using UnityEngine;
using Client;
using LitJson;

namespace Game {
    public enum BagItemType
    {
        Resource = 1,
        Speedup =2,
        Boost =3,
        Equipment = 4,
        Other = 5,
        Icon = 6,
    }

    public enum EquipItemType
    {
        Material =1,
        Equip =2,
        Drawing =3,
        //归于材料分类
        DrawingMaterial =4,
    }
    
    public enum EquipSortType
    {
        HighQuality,
        LowQuality,
        worn,
        Exclusive,
    }

    public enum EquipSubType
    {
        Head = 1,
        BreastPlate = 2,
        Weapon = 3,
        Gloves = 4,
        Pants = 5,
        accessories1 = 6,
        accessories2 = 7,
        shoes = 8,
    }

    public class ItemInfo
    {
        public long ItemIndex { get; set; }
        public int ItemID { get; set; }
        public long ItemNum { get; set; }
        public int ItemType { get; set; }
        public long HeroID { get; set; } = 0;

        public ItemInfo(int type, ItemInfoEntity info)
        {
            ItemID =(int)info.itemId;
            ItemType = type;
            ItemIndex = info.itemIndex;
            ItemNum = info.overlay;
            HeroID = info.heroId;
        }

        public ItemInfo(int itemId,long itemNum)
        {
            ItemID =itemId;
            ItemNum = itemNum;
        }
        
    }

    public class EquipItemInfo : ItemInfo
    {
        public int Exclusive { get; set; }
        public int Order { get; set; }
        public int Group { get; set; }

        public EquipItemInfo(int type, ItemInfoEntity info) : base(type,info)
        {
            var equipDefine = CoreUtils.dataService.QueryRecord<EquipDefine>(ItemID);
            
            Exclusive = (int)info.exclusive;
            if (equipDefine == null)
            {
                CoreUtils.logService.Error($"背包道具====装备道具:{info.itemId} 未在装备表中配置,请检查！！！");
                Order = 1;
                Group = 1;
                return;
            }
            Order = equipDefine.order;
            Group = equipDefine.group;
        }
    }

    public class MaterialItem : ItemInfo
    {
        //若为图纸,配置信息为空
        public EquipMaterialDefine MaterialDefine;
        public EquipItemType MaterialType;
        public MaterialItem(int type,int materialType, ItemInfoEntity info) : base(type, info)
        {
            SetItemID(ItemID);
            MaterialType = (EquipItemType)materialType;
        }

        public MaterialItem(int itemID,int itemNum) : base(itemID,itemNum)
        {
            
        }

        public void SetItemID(int itemID)
        {
            ItemID = itemID;
            MaterialDefine = CoreUtils.dataService.QueryRecord<EquipMaterialDefine>(itemID);
        }
    }



    public class BagProxy : GameProxy {
    
        #region Member
        public const string ProxyNAME = "BagProxy";

        private PlayerProxy m_playerProxy;
        /// <summary>
        /// 背包道具
        /// </summary>
        public Dictionary<Int64, ItemInfoEntity> Items = new Dictionary<Int64, ItemInfoEntity>();
        private List<ItemInfoEntity> m_itemList = new List<ItemInfoEntity>();
        private Dictionary<Int64, int> m_keyMap = new Dictionary<long, int>();
        public List<Int64> ItemChangeList = new List<Int64>();
        public List<Int64> ItemIdChangeList = new List<Int64>();
        private bool m_isFirstGetItemInfo = true;               //是否首次获取item信息

        private Dictionary<long,ItemInfo> m_materialItemInfos = new Dictionary<long, ItemInfo>();  //装备材料
        private Dictionary<long,ItemInfo> m_equipItemInfos = new Dictionary<long, ItemInfo>();    //装备
        private Dictionary<Int64, int> m_itemNumMap = new Dictionary<long, int>();

        private Dictionary<int, Dictionary<long, long>> m_reddotRecord = new Dictionary<int, Dictionary<long, long>>(); //红点记录
        private Dictionary<int, long> m_reddotTotalDic = new Dictionary<int, long>(); //红点统计

        #endregion

        // Use this for initialization
        public BagProxy(string proxyName)
            : base(proxyName)
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME)as PlayerProxy;
        }
        
        public override void OnRegister()
        {
            Debug.Log(" BagProxy register");

            for (int i = 1; i <= 5; i++)
            {
                m_reddotRecord[i] = new Dictionary<long, long>();
                m_reddotTotalDic[i] = 0;
            }
        }


        public override void OnRemove()
        {
            Debug.Log(" BagProxy remove");
        }

        //更新item信息
        public void UpdateItemInfo(INotification notification)
        {
            Item_ItemInfo.request itemInfos = notification.Body as Item_ItemInfo.request;

            if (itemInfos == null || itemInfos.itemInfo == null)
            {
                return;
            }
            bool bIsFistGetItem = IsFirstGetItem();
            ItemChangeList.Clear();
            ItemIdChangeList.Clear();

            bool bSavePrefab = false;
            bool isNewItem;
            long itemId = 0;
            bool isReddotChange = false;
            foreach (var data in itemInfos.itemInfo)
            {
                isNewItem = false;
                ItemInfoEntity itemData;
                Items.TryGetValue(data.Key, out itemData);
                if (itemData == null)
                {
                    itemId = data.Value.itemId;
                    itemData = new ItemInfoEntity();
                    Items[data.Key] = itemData;
                    m_itemList.Add(itemData);
                    m_keyMap[data.Key] = m_itemList.Count - 1;
                    if (data.Value.itemId > 0)
                    {
                        m_itemNumMap[data.Value.itemId] = -1;
                        ItemIdChangeList.Add(data.Value.itemId);
                    }
                    isNewItem = true;
                }
                else
                {
                    itemId = (data.Value.itemId > 0) ? data.Value.itemId : itemData.itemId;
                    if (itemId > 0)
                    {
                        m_itemNumMap[itemId] = -1;
                        ItemIdChangeList.Add(itemId);
                    }
                }

                //红点记录
                if (ReddotRecord(itemId, isNewItem, data.Value, itemData))
                {
                    isReddotChange = true;
                }

                ItemChangeList.Add(data.Key);
                if(SaveNewItemToLocal(data.Value.itemIndex))
                {
                    bSavePrefab = true;
                }
                // 首次登陆获得，全部标记成旧的  或者道具已经被穿上了也是一样
                if(bIsFistGetItem || data.Value.heroId != 0)
                {
                    if (SetLocalItemToOld(data.Value.itemIndex))
                    {
                        bSavePrefab = true;
                    }
                }
                HashSet<string> changeAttr = ItemInfoEntity.updateEntity(itemData, data.Value, true);
                if (ItemSort(itemData))
                {
                    bSavePrefab = true;
                }
            }
            if (m_isFirstGetItemInfo)
            {
                m_isFirstGetItemInfo = false;
            }
            else
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ItemInfoChange);
            }
            if (isReddotChange)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ItemReddotChange, 1);
            }
            if (bIsFistGetItem)
            {
                SetFirstGetItem(false);
                bSavePrefab = true;
            }
            if(bSavePrefab)
            {
                HotfixUtil.InvokOncePerfOneFrame("PlayerPrefs.Save()", () =>
                {
                    PlayerPrefs.Save();
                });
            }
        }

        public long GetItemNum(int itemID)
        {
            if (!m_itemNumMap.ContainsKey(itemID))
            {
                return 0;
            }
            if (m_itemNumMap[itemID] > -1)
            {
                return m_itemNumMap[itemID];
            }
            Int64 tempNum = 0;
            m_itemList.ForEach((itemInfo)=> {
                if (itemInfo.itemId == itemID)
                {
                    tempNum += itemInfo.overlay;
                }
            });
            m_itemNumMap[itemID] = (int)tempNum;
            return tempNum;
        }

        private Int64 m_tempIndex;
        public long GetItemIndex(int itemID)
        {
            m_tempIndex = 0;
            m_itemList.ForEach((itemInfo) => {
                if (itemInfo.itemId == itemID && itemInfo.overlay > 0)
                {
                    m_tempIndex = itemInfo.itemIndex;
                    return;
                }
            });
            return m_tempIndex;
        }

        public int GetItemTypeById(long itemId)
        {
            return (int)itemId / 100000000;
        }
        
        //道具分类
        private bool ItemSort(ItemInfoEntity itemInfo)
        {
            var cfg = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemInfo.itemId);
            if (cfg == null)
            {
                CoreUtils.logService.Warn($"========找不到对应道具数据:{itemInfo.itemId}");
                return false;
            }
            var itemInfos = GetItemInfoLstByType((BagItemType) cfg.type,  cfg.typeGroup);
            if (itemInfos == null)
            {
                return false;
            }
            if (itemInfos.ContainsKey(itemInfo.itemIndex))
            {
                return UpdateItemInfo(itemInfos,itemInfo);
            }
            else
            {
                itemInfos.Add(itemInfo.itemIndex,GetItemInfo(itemInfo));
            }
            return false;
        }
        //更新道具信息
        private bool UpdateItemInfo(Dictionary<long, ItemInfo> itemInfos, ItemInfoEntity info)
        {
            if (info.overlay <= 0)
            {
                itemInfos.Remove(info.itemIndex);
                ClearLocalItem(info.itemIndex);
                return true;
            }
            else
            {
                itemInfos[info.itemIndex].ItemNum = info.overlay;
                itemInfos[info.itemIndex].HeroID = info.heroId;
                return false;
            }
        }
        //根据道具类型获取道具列表
        public Dictionary<long, ItemInfo> GetItemInfoLstByType(BagItemType type, int subType = 0)
        {
            switch (type)
            {
                case BagItemType.Equipment:
                    switch (subType)
                    {
                        case (int)EquipItemType.Material:
                        case (int)EquipItemType.DrawingMaterial:
                        case (int) EquipItemType.Drawing:
                            return m_materialItemInfos;
                        case (int)EquipItemType.Equip:
                            return m_equipItemInfos;
                    }
                    break;
            }
            return null;
        }

        //将服务器下发的道具信息转为对应道具类
        private ItemInfo GetItemInfo(ItemInfoEntity infoEntity)
        {
            var cfg = CoreUtils.dataService.QueryRecord<ItemDefine>((int)infoEntity.itemId);
            switch (cfg.type)
            {
                case (int)BagItemType.Equipment:
                    switch (cfg.typeGroup)
                    {
                        case (int)EquipItemType.Material:
                        case (int) EquipItemType.DrawingMaterial:
                        case (int) EquipItemType.Drawing:
                            return new MaterialItem(cfg.type,cfg.typeGroup,infoEntity);
                        case (int)EquipItemType.Equip:
                            return new EquipItemInfo(cfg.type,infoEntity);
                    }
                    break;
            }
            return new ItemInfo(cfg.type,infoEntity);
        }

        private bool IsFirstGetItem()
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = $"{playerProxy.CurrentRoleInfo.rid}_HasItemUseCheck";
            return PlayerPrefs.GetInt(key, 1) == 1;
        }
        public void SetFirstGetItem(bool bFirstGetItem)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = $"{playerProxy.CurrentRoleInfo.rid}_HasItemUseCheck";
            if (bFirstGetItem)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            else
            {
                PlayerPrefs.SetInt(key, 0);
            }
        }

        private bool SaveNewItemToLocal(long index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/itemIndex:" + index;
            int value = PlayerPrefs.GetInt(key, 0);
            if (value == 0)
            {
                PlayerPrefs.SetInt(key, 1);
                return true;
            }
            return false;
        }

        public void ClearLocalItem(long index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/itemIndex:" + index;
            PlayerPrefs.DeleteKey(key);
        }

        public bool SetLocalItemToOld(long index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/itemIndex:" + index;
            int value = PlayerPrefs.GetInt(key);
            if (value > 0)
            {
                PlayerPrefs.SetInt(key, -1);
                return true;
            }
            return false;
        }

        public bool isNewItem(long index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/itemIndex:" + index;
            int value = PlayerPrefs.GetInt(key);
            if (value > 0)  
                return true;

            return false;
        }

        #region 材料

        public Dictionary<long, MaterialItem> GetMaterialItems()
        {
            var items = new Dictionary<long, MaterialItem>();
            var itemInfos = GetItemInfoLstByType(BagItemType.Equipment, (int) EquipItemType.Material);
            foreach (var itemInfo in itemInfos)
            {
                items.Add(itemInfo.Key,itemInfo.Value as MaterialItem);
            }

            return items;
        }
        
        public void ProduceItem(int ID)
        {
            Build_ProduceMaterial.request req = new Build_ProduceMaterial.request()
            {
                itemId =ID
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        public void ResolveMaterial(int itemID,long count,bool isMax)
        {
            Build_MaterialDecomposition.request req = new Build_MaterialDecomposition.request()
            {
                itemId = itemID,
                count = count,
                max = isMax,
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        public void MixMaterial(int itemID,long count,bool isMax)
        {
            Build_MaterialSynthesis.request req = new Build_MaterialSynthesis.request()
            {
                itemId = itemID,
                count = count,
                max = isMax,
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        public void CancelProduceItem(int index)
        {
            Build_CancelProduceMaterial.request req = new Build_CancelProduceMaterial.request()
            {
                index = index
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        //材料是否可以快速制造
        public bool MaterialCanBeMake(int itemID, long itemNum)
        {
            Dictionary<int,long > materials = new Dictionary<int, long>();
            MakeMaterial(ref materials, itemID, itemNum);
            return materials!=null;
        }

        //获取材料快速制造所需材料列表
        public void MakeMaterial(ref Dictionary<int, long> materials,int itemID, long itemNum)
        {
            if (materials == null)
            {
                return;
            }

            long curNum = 0;
            long lackNum = 0;
            EquipMaterialDefine cfg = null;

            do
            {
                curNum = GetItemNum(itemID);
                lackNum = itemNum - curNum;

                if (lackNum <= 0)
                {
                    materials.Add(itemID, itemNum);
                    return;
                }
                else if (curNum > 0)
                {
                    materials.Add(itemID, curNum);
                }

                var materialCfg = CoreUtils.dataService.QueryRecord<EquipMaterialDefine>(itemID);

                if (materialCfg == null)
                {
                    //所需为完整图纸
                    var materialCfgs = CoreUtils.dataService.QueryRecords<EquipMaterialDefine>();
                    cfg = materialCfgs.Find(x => x.mix == itemID);
                }
                else
                {
                    //寻找下位材料
                    cfg = CoreUtils.dataService.QueryRecord<EquipMaterialDefine>(materialCfg.split);
                }

                //下位材料为空
                if (cfg == null)
                {
                    materials.Clear();
                    materials = null;
                    return;
                }
                else
                {
                    itemID = cfg.itemID;
                    itemNum = cfg.mixCostNum * lackNum;
                }
            }while(true);
        }

        public long GetCanMixDrawingMaterialNum()
        {
            long mixNum = 0;
            var materialItems = GetMaterialItems();
            foreach (var materialItem in materialItems)
            {
                if (materialItem.Value.MaterialDefine !=null && materialItem.Value.MaterialDefine.mix != 0 && materialItem.Value.MaterialType == EquipItemType.DrawingMaterial)
                {
                    mixNum += materialItem.Value.ItemNum / materialItem.Value.MaterialDefine.mixCostNum;
                }
            }

            return mixNum;
        }
        #endregion

        #region 装备

        public void CheckMakeEquip(int itemID)
        {
            Build_CheckMakeEquip.request req = new Build_CheckMakeEquip.request()
            {
                itemId = itemID
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        public void MakeEquipment(int itemID, int exclusive)
        {
            Build_MakeEquipment.request req = new Build_MakeEquipment.request()
            {
                itemId = itemID,
                exclusive = exclusive
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        public void ResolveEquipment(long index)
        {
            Build_DecompositionEquipment.request req = new Build_DecompositionEquipment.request()
            {
                itemIndex = index,
            };
            AppFacade.GetInstance().SendSproto(req);
        }
        
        public EquipItemInfo GetEquipItemInfo(long index)
        {
            if (m_equipItemInfos.ContainsKey(index))
            {
                return  m_equipItemInfos[index] as EquipItemInfo;
            }
            CoreUtils.logService.Warn($"找不到装备Index：{index}");
            return null;
        }

        public List<EquipItemInfo> GetEquipItems()
        {
            var items = new List<EquipItemInfo>();
            var itemInfos = GetItemInfoLstByType(BagItemType.Equipment, (int) EquipItemType.Equip);
            foreach (var itemInfo in itemInfos)
            {
                items.Add(itemInfo.Value as EquipItemInfo);
            }

            return items;
        }

        public List<EquipItemInfo> GetEquipItemsBySubType(int subType)
        {
            var items = new List<EquipItemInfo>();
            var itemInfos = GetEquipItems();
            foreach (var itemInfo in itemInfos)
            {
                var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemInfo.ItemID);
                if (itemDefine.subType == subType)
                {
                    items.Add(itemInfo);
                    
                }
            }

            return items;
        }

        public List<EquipItemInfo> GetUsableEquipItemsBySubType(int subType)
        {
            var items = new List<EquipItemInfo>();
            var itemInfos = GetEquipItemsBySubType(subType);
            foreach (var itemInfo in itemInfos)
            {
                if (itemInfo.HeroID <= 0)
                {
                    items.Add(itemInfo);
                    
                }
            }

            return items;
        }

        public List<EquipItemInfo> GetEquipItemsBySortType(int groupID,EquipSortType type)
        {
            var items = new List<EquipItemInfo>();
            
            return items;
        }
        
        public int GetNewEquipCountBySubtype(int subType)
        {
            int count = 0;
            var itemInfos = GetEquipItemsBySubType(subType);
            foreach (var itemInfo in itemInfos)
            {
                if (isNewItem(itemInfo.ItemIndex))
                {
                    count++;

                }
            }

            return count;
        }

        public int GetRegionRedPointCount()
        {
            int count = 0;
            var heroEquipTypes =  CoreUtils.dataService.QueryRecord<ConfigDefine>(0).heroEquipType;
            foreach (var subType in heroEquipTypes)
            {
                var itemInfos = GetEquipItemsBySubType(subType);
                foreach (var itemInfo in itemInfos)
                {
                    if (isNewItem(itemInfo.ItemIndex))
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }

        public bool HasEquipCanBeForged()
        {
            var equipCfgs = CoreUtils.dataService.QueryRecords<EquipDefine>();
            foreach (var equipCfg in equipCfgs)
            {
                if (!ShowForgeEquipRedDot(equipCfg.itemID))
                {
                    continue;
                }
                var materials = equipCfg.makeMaterial;
                bool canForge = true;
                for (int i = 0; i < materials.Count; i++)
                {
                    if (GetItemNum(materials[i]) < equipCfg.makeMaterialNum[i])
                    {
                        canForge = false;
                        break;
                    }
                }
                if (canForge)
                {
                    return true;
                }
            }

            return false;
        }

        private List<int> m_forgeEquipIgnoreRedDotLst = new List<int>();

        public void GetForgeEquipIgnoreRedDotInfo()
        {
            string info = PlayerPrefs.GetString(m_playerProxy.Rid.ToString() + "forgeEquipIgnoreRedDot");
            if (string.IsNullOrEmpty(info))
            {
                m_forgeEquipIgnoreRedDotLst = new List<int>();
            }
            else
            {
                m_forgeEquipIgnoreRedDotLst = JsonMapper.ToObject<List<int>>(info);
            }
        }

        public void ClearForgeEquipRedDot()
        {
            m_forgeEquipIgnoreRedDotLst.Clear();
            PlayerPrefs.SetString(m_playerProxy.Rid.ToString() + "forgeEquipIgnoreRedDot", "");
        }
        public bool ShowForgeEquipRedDot(int equipID)
        {
            if (m_forgeEquipIgnoreRedDotLst.Contains(equipID))
            {
                return false;
            }
            return true;
        }
        public void AddForgeEquipIgnoreRedDot(int equipID)
        {
            if (m_forgeEquipIgnoreRedDotLst.Contains(equipID))
            {
                return;
            }
            m_forgeEquipIgnoreRedDotLst.Add(equipID);
            string info = JsonMapper.ToJson(m_forgeEquipIgnoreRedDotLst);
            PlayerPrefs.SetString(m_playerProxy.Rid.ToString()+"forgeEquipIgnoreRedDot", info);
            SendNotification(CmdConstant.EquipForgeRedDotChange);
        }
        
        #endregion

        #region 背包红点统计

        //红点记录
        private bool ReddotRecord(long itemId, bool isNewItem, SprotoType.ItemInfo data, ItemInfoEntity itemData)
        {
            bool isChange = false;
            if (!m_isFirstGetItemInfo)
            {
                int type = GetItemTypeById(itemId);

                var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemId);
                if (itemDefine == null)
                {
                    return false;
                }
                if (itemDefine.redDotPrompt != 1)
                {
                    return false;
                }

                if (isNewItem)
                {
                    if (m_reddotRecord.ContainsKey(type)) //新获得物品
                    {
                        m_reddotRecord[type][data.itemIndex] = data.overlay;
                        isChange = true;
                    }
                }
                else
                {
                    if (m_reddotRecord.ContainsKey(type))
                    {
                        if (data.overlay < 1) //物品移除了
                        {
                            //如果有红点记录 则移除
                            if (m_reddotRecord[type].ContainsKey(data.itemIndex))
                            {
                                m_reddotRecord[type].Remove(data.itemIndex);
                                isChange = true;
                            }
                        }
                        else if (data.overlay > itemData.overlay) //物品增加了
                        {
                            isChange = true;
                            long overlay = (itemData.overlay < 0) ? 0 : itemData.overlay;
                            if (m_reddotRecord[type].ContainsKey(data.itemIndex))
                            {
                                m_reddotRecord[type][data.itemIndex] = m_reddotRecord[type][data.itemIndex] + (data.overlay - overlay);
                            }
                            else
                            {
                                m_reddotRecord[type][data.itemIndex] = data.overlay - overlay;
                            }
                        }
                        else if (data.overlay < itemData.overlay) //物品减少了
                        {
                            isChange = true;
                            //如果有红点记录 则移除
                            if (m_reddotRecord[type].ContainsKey(data.itemIndex))
                            {
                                m_reddotRecord[type].Remove(data.itemIndex);
                            }
                        }
                    }
                }
                if (m_reddotTotalDic.ContainsKey(type))
                {
                    m_reddotTotalDic[type] = -1;
                }
            }
            return isChange;
        }

        //获取背包红点总数
        public long GetBagReddotTotal()
        {
            long num = 0;
            foreach (var data in m_reddotTotalDic)
            {
                num = GetBagReddotNumByType(data.Key) + num;
            }
            return num;
        }

        //获取某个类型的红点数量
        public long GetBagReddotNumByType(int type)
        {
            if (m_reddotTotalDic[type] > -1)
            {
                return m_reddotTotalDic[type];
            }
            long num = 0;
            Dictionary<long, long> recordDic = m_reddotRecord[type];
            foreach (var data in recordDic)
            {
                num = data.Value + num;
            }
            return num;
        }

        //清除所有红点记录
        public void ClearAllReddotRecord()
        {
            foreach (var data in m_reddotRecord)
            {
                data.Value.Clear();
                m_reddotTotalDic[data.Key] = 0;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ItemReddotChange, 0);
        }

        //清除对应类型的红点记录
        public void ClearReddotRecordByType(int type)
        {
            if (m_reddotRecord.ContainsKey(type))
            {
                m_reddotRecord[type].Clear();
                m_reddotTotalDic[type] = 0;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ItemReddotChange, 0);
        }

        //清除物品索引对应的红点记录
        public void ClearReddotRecordByIndex(long index, long itemId)
        {
            int type = GetItemTypeById(itemId);
            if (m_reddotRecord.ContainsKey(type))
            {
                if (m_reddotRecord[type].ContainsKey(index))
                {
                    long total = m_reddotTotalDic[type];
                    m_reddotTotalDic[type] = total - m_reddotRecord[type][index];
                    m_reddotRecord[type][index] = 0;
                    AppFacade.GetInstance().SendNotification(CmdConstant.ItemReddotChange, 0);
                } 
            }
        }

        public bool IsShowReddot(long index, long itemId)
        {
            int type = GetItemTypeById(itemId);
            if (m_reddotRecord.ContainsKey(type))
            {
                if (m_reddotRecord[type].ContainsKey(index) && m_reddotRecord[type][index] >0)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}