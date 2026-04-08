// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    StoreProxy 商店
// Copyright IGG All rights reserved.
// ===============================================================================

using Data;
using Skyunion;
using SprotoType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class StoreProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "StoreProxy";

        public bool IsStoreBubbleShowed { get; set; } = false;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy currencyProxy;
        private GlobalEffectMediator globalEffectMediator;
        private ConfigDefine configDefine;
        private long mysteryStoreLeaveTimes;
        
        
        private Dictionary<int, List<int>> m_cacheShopData;
        private Dictionary<int, bool> m_showTypeDic;
        private List<int> mysteryStoreItemList;
        
        #endregion

        // Use this for initialization
        public StoreProxy(string proxyName)
            : base(proxyName)
        {
        }
        
        public override void OnRegister()
        {
            Debug.Log(" StoreProxy register");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            currencyProxy = AppFacade.GetInstance().RetrieveProxy(Game.CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_showTypeDic = new Dictionary<int, bool>();
            m_showTypeDic[1] = true;
            m_showTypeDic[2] = true;
            m_showTypeDic[3] = true;
            m_showTypeDic[5] = true;
            configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            mysteryStoreItemList = new List<int>();
        }

        public override void OnRemove()
        {
            Debug.Log(" StoreProxy remove");
        }

        public Dictionary<int, List<int>> GetShopData()
        {
            if (m_cacheShopData == null)
            {
                m_cacheShopData = new Dictionary<int, List<int>>();
                List<ItemDefine> defineList = CoreUtils.dataService.QueryRecords<ItemDefine>();
                defineList.ForEach((define) => {
                    if (define.shopPrice > 0 && m_showTypeDic.ContainsKey(define.type))
                    {
                        if (!m_cacheShopData.ContainsKey(define.type))
                        {
                            m_cacheShopData[define.type] = new List<int>();
                        }
                        m_cacheShopData[define.type].Add(define.ID);
                    }
                });
            }
            return m_cacheShopData;
        }

        public GlobalEffectMediator GetGlobalEffectMediator()
        {
            if (globalEffectMediator == null)
            {
                globalEffectMediator = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            }

            return globalEffectMediator;
        }
        
        #region 神秘商人相关功能

        #region 获得数据

        //获得神秘商店商品_神秘商人
        public List<MysteryStoreItemGroupData> GetMysteryStoreGoodses()
        {
            List<MysteryStoreItemGroupData> datas = new List<MysteryStoreItemGroupData>();
            mysteryStoreItemList = new List<int>();
            if (CheckIsOpen_MysteryStore())
            {
                var originalData = m_playerProxy.CurrentRoleInfo.mysteryStore.mysteryStoreGoods;
                foreach (var value in originalData)
                {
                    mysteryStoreItemList.Add((int)value.Value.id);
                    var data = new MysteryStoreItemData(value.Value);
                    MysteryStoreItemGroupData.AddItemData(datas,data);
                }
            }

            MysteryStoreItemGroupData.SortClassList(datas);
            return datas;
        }
        //当前刷新次数_神秘商人
        public int GetCurRefreshCount_MysteryStore()
        {
            if (m_playerProxy.HasMysteryStoreInfo())
            {
                return (int)m_playerProxy.CurrentRoleInfo.mysteryStore.refreshCount;
            }
            return 0;
        }

        //最大刷新次数_神秘商人
        public int GetMaxRefreshCount_MysteryStore()
        {
            return configDefine.mysteryStoreRefresh;
        }

        

        //获得当前刷新价格
        public int GetCurRefreshCost_MysteryStore()
        {
            int curNum = GetCurRefreshCount_MysteryStore();
            if (curNum < configDefine.mysteryStoreRefreshPrice.Count)
            {
                return configDefine.mysteryStoreRefreshPrice[curNum];
            }
            return configDefine.mysteryStoreRefreshPrice[configDefine.mysteryStoreRefreshPrice.Count - 1];
        }

        //获得神秘商人剩余离开时间
        public long GetLeaveTime_MysteryStore()
        {
            //
            var num = mysteryStoreLeaveTimes - ServerTimeModule.Instance.GetServerTime(); 
            return num > 0 ? num : 0;
        }
        #endregion

        #region OnEventFunction--各种事件监听
        //当商店信息刷新_神秘商人
        public void OnMysteryStoreRefresh()
        {
            RefreshLeaveTime();
            bool newMysteryStoreState = CheckIsOpen_MysteryStore();
            if (newMysteryStoreState && !IsStoreBubbleShowed)
            {
                //神秘商人出现
                AppFacade.GetInstance().SendNotification(CmdConstant.OnMysteryStoreOpen);
            }
            else if(!newMysteryStoreState)
            {
                //神秘商人消失
                AppFacade.GetInstance().SendNotification(CmdConstant.OnMysteryStoreClose);
                AppFacade.GetInstance().SendNotification(CmdConstant.HideMysteryStoreBubble);
                IsStoreBubbleShowed = false;
            }
        }
        

        #endregion

        #region 逻辑行为相关--购买、刷新等
 
        //购买神秘商人道具_神秘商人
        public bool BuyItem_MysteryStore(MysteryStoreItemData itemData,Action fCallback = null)
        {
            if (!CheckIsOpen_MysteryStore())
            {
                CoreUtils.logService.Error("wwz===========错误，神秘商人未出现，不可购买");
                return false;
            }

            if (!HasItemByItemId_MysteryStore(itemData.id))
            {
                CoreUtils.logService.Error("wwz===========错误，神秘商人没出售该物品，不可购买");
                return false;
            }
            
            if (!HasEnoughCost_MysteryStore(itemData.costType,itemData.costNum))
            {
                // CoreUtils.logService.Error("wwz===========转资源不足或宝石不足功能");
                return false;
            }
            GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
            if (itemData.costType == (int)EnumCurrencyType.denar && isRemind)
            {
                //需要提醒
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                string currencyiconId = currencyProxy.GeticonIdByType(104);
                Data.SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                if (settingPersonalityDefine != null)
                {
                    string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                    string str = LanguageUtils.getTextFormat(300072, itemData.costNum);
                    Alert.CreateAlert(str).
                        SetLeftButton().
                        SetRightButton(null, LanguageUtils.getText(100036)).
                        SetCurrencyRemind((isBool) =>
                        {
                            if (isBool)
                            {
                                generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                            }
                        //发送购买协议
                        SendByRequest_MysteryStore(itemData.id);
                            fCallback?.Invoke();
                        },
                            itemData.costNum, s_remind, currencyiconId).//价格
                        Show();
                }
            }
            else
            {
                //不需要提醒
                //发送购买协议
                SendByRequest_MysteryStore(itemData.id);
                fCallback?.Invoke();
            }

            return true;//购买成功，界面应该开始等待协议回来
        }
        
        //刷新道具列表_神秘商人
        public bool RefreshItemList_MysteryStore()
        {
            if (!CheckIsOpen_MysteryStore())
            {
                CoreUtils.logService.Error("wwz===========错误，神秘商人未出现，不可刷新");
                return false;
            }

            if (HasFreeRefreshCount_MysteryStore())
            {
                CoreUtils.logService.Info("wwz===========免费刷新神秘商店");
                //发送协议
                SendRefreshRequest_MysteryStore();
                return true;
            }
            
            if (!HasRefreshCount_MysteryStore())
            {
                return false;
            }

            int costNum = GetCurRefreshCost_MysteryStore();
            if (currencyProxy.ShortOfDenar(costNum))
            {
                // CoreUtils.logService.Error("wwz===========转资源不足或宝石不足功能");
                return false;
            }
            GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
            if (isRemind)
            {
                //需要提醒
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                string currencyiconId = currencyProxy.GeticonIdByType(104);
                Data.SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                if (settingPersonalityDefine != null)
                {
                    string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                    string str = LanguageUtils.getTextFormat(300072, costNum);
                    Alert.CreateAlert(str).
                        SetLeftButton().
                        SetRightButton(null, LanguageUtils.getText(100036)).
                        SetCurrencyRemind((isBool) =>
                        {
                            if (isBool)
                            {
                                generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                            }
                        //发送协议
                        SendRefreshRequest_MysteryStore();
                        },
                            costNum, s_remind, currencyiconId).//价格
                        Show();
                }
            }
            else
            {
                //不需要提醒
                //发送购买协议
                SendRefreshRequest_MysteryStore();
            }

            return true;
        }

        //打开神秘商店_神秘商人
        public bool OpenMysteryStore()
        {
            if (CheckIsOpen_MysteryStore())
            {
                CoreUtils.uiManager.ShowUI(UI.s_mysteryStore);
                AppFacade.GetInstance().SendNotification(CmdConstant.HideMysteryStoreBubble);
                return true;
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_mysteryStoreGone);
                return false;
            }
            // CoreUtils.uiManager.ShowUI(UI.s_mysteryStore);
        }
        

        //播放道具飞行动画
        public void PlayItemFlyAnim_MysteryStore(int itemId,RectTransform rect)
        {
            GetGlobalEffectMediator().FlyItemEffect(itemId, 1,rect);
        }

        #endregion

        #region 判断方法相关

        //检查是否有神秘商店数据_神秘商人
        public bool CheckIsOpen_MysteryStore()
        {
            if (m_playerProxy.HasMysteryStoreInfo() && m_playerProxy.CurrentRoleInfo.mysteryStore.mysteryStoreGoods != null)
            {
                return true;
            }
            return false;
        }
        //有对应道具_神秘商人
        public bool HasItemByItemId_MysteryStore(int itemGuid)
        {
            return mysteryStoreItemList.Contains(itemGuid);
        }
        //道具未售罄_神秘商人
        public bool HasCountByItemId_MysteryStore()
        {
            return false;
        }
        //需要消耗的资源足够_神秘商人
        public bool HasEnoughCost_MysteryStore(int sourceType,int costNum)
        {
            if (m_playerProxy.GetResNumByType(sourceType) < costNum)
            {
                switch ((EnumCurrencyType)sourceType)
                {
                    case EnumCurrencyType.food:
                    case EnumCurrencyType.wood:
                    case EnumCurrencyType.stone:
                    case EnumCurrencyType.gold:
                        {
                            long food = sourceType == (int)EnumCurrencyType.food ? costNum : 0;
                            long wood = sourceType == (int)EnumCurrencyType.wood ? costNum : 0;
                            long stone = sourceType == (int)EnumCurrencyType.stone ? costNum : 0;
                            long gold = sourceType == (int)EnumCurrencyType.gold ? costNum : 0;
                            currencyProxy.LackOfResources(food, wood, stone, gold);
                        }
                        break;
                    case EnumCurrencyType.denar:
                        {
                            currencyProxy.ShortOfDenar(costNum);
                        }
                        break;
                }
                return false;
            }
            return true;
        }
        //需要确认弹出_神秘商人
        public bool NeedBuyTips_MysteryStore()
        {
            return false;
        }
        
        //还可以继续刷新_神秘商人
        public bool HasRefreshCount_MysteryStore()
        {
            if (HasFreeRefreshCount_MysteryStore() || HasCostDiamondRefreshCount_MysteryStore())
            {
                return true;
            }
            return false;
        }

        public bool HasCostDiamondRefreshCount_MysteryStore()
        {
            return GetCurRefreshCount_MysteryStore() < GetMaxRefreshCount_MysteryStore();
        }
        
        //有免费刷新
        public bool HasFreeRefreshCount_MysteryStore()
        {
            if (m_playerProxy.HasMysteryStoreInfo())
            {
                return !m_playerProxy.CurrentRoleInfo.mysteryStore.freeRefresh;
            }
            return false;
        }
        #endregion
        

        #region 神秘商店协议相关
        private void SendByRequest_MysteryStore(int nById)
        {
            Shop_BuyPostItem.request req = new Shop_BuyPostItem.request();
            req.id = nById;
            AppFacade.GetInstance().SendSproto(req);
        }

        private void SendRefreshRequest_MysteryStore()
        {
            Shop_RefreshPostItem.request req = new Shop_RefreshPostItem.request();
            AppFacade.GetInstance().SendSproto(req);
        }
        #endregion

        private void RefreshLeaveTime()
        {
            mysteryStoreLeaveTimes = 0;
            if (m_playerProxy.HasMysteryStoreInfo())
            {
                mysteryStoreLeaveTimes = m_playerProxy.CurrentRoleInfo.mysteryStore.leaveTime;
            }
        }
        
        #endregion
        
    }

    //神秘商店道具类
    public class MysteryStoreItemData
    {
        public int id;
        public int itemTypeId;
        public int groupType;
        public long num;
        public int discount;
        public bool isBuy;
        public int costType;
        public int costNum;
        public int originalCostNum;

        public MysteryStoreItemData(MysteryStore.MysteryStoreGoods data)
        {
            id = (int)data.id;
            num = data.num;
            discount = (int) data.discount;
            isBuy = data.isBuy;
            var config = CoreUtils.dataService.QueryRecord<MysteryStoreDefine>(id);
            itemTypeId = config.item;
            costType = config.type;
            originalCostNum = config.price;
            costNum = (int)data.price;
            groupType = config.@group;
        }
        
    }

    //为了处理服务器下发的数据是无序的问题，封装的类
    public class MysteryStoreItemGroupData
    {
        private int groupType;
        private List<MysteryStoreItemData> items;

        public MysteryStoreItemGroupData(int groupType)
        {
            this.groupType = groupType;
            items = new List<MysteryStoreItemData>();
        }
        
        public bool IsGroupType(int groupType)
        {
            return this.groupType == groupType;
        }

        public bool HasValue()
        {
            return items != null && items.Count > 0;
        }
        
        public void AddItemData(MysteryStoreItemData data)
        {
            if (items == null)
            {
                items = new List<MysteryStoreItemData>();
            }
            items.Add(data);
        }

        public void SortItemDatas()
        {
            if (HasValue())
            {
                items.Sort(delegate(MysteryStoreItemData data, MysteryStoreItemData itemData)
                {
                    return data.id < itemData.id ? -1 : 1;
                });
            }
        }

        public List<MysteryStoreItemData> GetItems()
        {
            return items;
        }

        #region StaticFunc

        public static void AddItemData(List<MysteryStoreItemGroupData> lists,MysteryStoreItemData data)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                if (lists[i].IsGroupType(data.groupType))
                {
                    lists[i].AddItemData(data);
                    return;
                }
            }
            //没成功添加
            var newList = new MysteryStoreItemGroupData(data.groupType);
            newList.AddItemData(data);
            lists.Add(newList);
        }

        public static void SortClassList(List<MysteryStoreItemGroupData> lists)
        {
            lists.Sort(delegate(MysteryStoreItemGroupData data, MysteryStoreItemGroupData groupData)
            {
                return data.groupType < groupData.groupType ? -1 : 1;
            });
            for (int i = 0; i < lists.Count; i++)
            {
                lists[i].SortItemDatas();
            }
        }
        //得到所有数据的第几个Item,index从0开始
        public static MysteryStoreItemData GetItemByIndex(List<MysteryStoreItemGroupData> lists ,int index)
        {
            MysteryStoreItemData data = null;
            int curIndex = 0;
            for (int i = 0; i < lists.Count; i++)
            {
                var items = lists[i].GetItems();
                for (int j = 0; j < items.Count; j++)
                {
                    if (curIndex == index)
                    {
                        return items[i];
                    }

                    curIndex++;
                }
            }

            return null;
        }
        

        #endregion
        // public override int GetHashCode()
        // {
        //     return groupType;
        // }
        //
        // public override bool Equals(object obj)
        // {
        //     if (obj.GetType() == typeof(MysteryStoreItemGroupData))
        //     {
        //         return groupType == ((obj) as MysteryStoreItemGroupData).groupType;
        //     }
        //     else
        //     {
        //         return base.Equals(obj);
        //     }
        // }
    }
    
}