// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    CurrencyProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using Data;
using System;
using DG.Tweening;
using UnityEngine.UI;
using SprotoType;

namespace Game {

    /// <summary>
    /// 货币数值跳动参数
    /// </summary>
    public class CurrencyFloatParam
    {
        public EnumCurrencyType currencyType;
        public long number;
        public bool changeColor;
    }

    public class RequireItemParam
    {
        public int ItemId { get; set; }
        public int Num { get; set; }
    }

    public class CurrencyProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "CurrencyProxy";

        public bool firstUpdate = true;

        private Dictionary<string, GameObject> m_assets = new Dictionary<string, GameObject>();

        private BagProxy m_bagProxy = null;
        #endregion

        // Use this for initialization
        public CurrencyProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" CurrencyProxy register");

        }

        private void OnLoadFinish(Dictionary<string,GameObject> asset)
        {
            m_assets = asset;
        }


        public override void OnRemove()
        {
            Debug.Log(" CurrencyProxy remove");
        }

        private Dictionary<int, CurrencyDefine> m_currencyDefines;
        public Dictionary< int,CurrencyDefine> CurrencyDefine
        {
            get 
            {
                if (m_currencyDefines == null)
                {
                    m_currencyDefines = new Dictionary<int, CurrencyDefine>();
                    foreach (var item in CoreUtils.dataService.QueryRecords<CurrencyDefine>())
                    {
                        m_currencyDefines[item.ID] = item;
                    }
                }
                return m_currencyDefines;
            }
        }

        private List<instantPriceDefine> m_instantPriceDefines;
        /// <summary>
        /// 立即完成的价格
        /// </summary>
        public List<instantPriceDefine> InstantPriceDefines
        {
            get
            {
                if(m_instantPriceDefines==null)
                {
                    m_instantPriceDefines = CoreUtils.dataService.QueryRecords<instantPriceDefine>();
                }
                return m_instantPriceDefines;
            }
        }

        public long Food { get; private set; }
        public long FloatFood { get; private set; }
        private Sequence m_foodFloatSequence;

        public long Wood { get; private set; }
        public long FloatWood { get; private set; }
        private Sequence m_woodFloatSequence;

        public long Stone { get; private set; }
        public long FloatStone { get; private set; }
        private Sequence m_stoneFloatSequence;

        public long Gold { get; private set; }
        public long FloatGold { get; private set; }
        private Sequence m_goldFloatSequence;

        public long Gem { get; private set; }
        public long FloatGem { get; private set; }
        private Sequence m_gemFloatSequence;


        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="body"></param>
        public void UpdateCurrency()
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            Food = playerProxy.CurrentRoleInfo.food;
            Wood = playerProxy.CurrentRoleInfo.wood;
            Stone = playerProxy.CurrentRoleInfo.stone;
            Gold = playerProxy.CurrentRoleInfo.gold;
            Gem = playerProxy.CurrentRoleInfo.denar;
            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateCurrency, firstUpdate);
            if (firstUpdate)
            {
                ConfigDefine configDefine = CoreUtils.dataService.QueryRecords<ConfigDefine>()[0];
                ClientUtils.PreLoadRes(CoreUtils.uiManager.GetCanvas().gameObject, new List<string>
                    {
                        configDefine.flyingFoodRes,
                        configDefine.flyingWoodRes,
                        configDefine.flyingStoneRes,
                        configDefine.flyingGlodRes,
                        configDefine.flyingDenarRes
                    }, OnLoadFinish);
                firstUpdate = false;
                FloatFood = Food;
                FloatWood = Wood;
                FloatStone = Stone;
                FloatGold = Gold;
                FloatGem = Gem;
            }
            else
            {
                if( FloatFood != Food)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateFloatCurrency, EnumCurrencyType.food);
                    FloatFood = Food;
                }
                if ( FloatWood != Wood)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateFloatCurrency, EnumCurrencyType.wood);
                    FloatWood = Wood;
                }
                if ( FloatStone != Stone)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateFloatCurrency, EnumCurrencyType.stone);
                    FloatStone = Stone;
                }
                if ( FloatGold != Gold)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateFloatCurrency, EnumCurrencyType.gold);
                    FloatGold = Gold;
                }
                if ( FloatGem != Gem)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateFloatCurrency, EnumCurrencyType.denar);
                    FloatGem = Gem;
                }
            }
        }

        public void DeviceToEachCurrency(EnumCurrencyType type,long currentFloat, long lastFloat)
        {
            switch(type)
            {
                case EnumCurrencyType.food:
                    SetSequence(m_foodFloatSequence, currentFloat, lastFloat, type); break;
                case EnumCurrencyType.wood:
                    SetSequence(m_woodFloatSequence, currentFloat, lastFloat, type); break;
                case EnumCurrencyType.stone:
                    SetSequence(m_stoneFloatSequence, currentFloat, lastFloat, type); break;
                case EnumCurrencyType.gold:
                    SetSequence(m_goldFloatSequence, currentFloat, lastFloat, type); break;
                case EnumCurrencyType.denar:
                    SetSequence(m_gemFloatSequence, currentFloat, lastFloat, type); break;
                default:break;
            }
        }


        /// <summary>
        /// 使用资源道具
        /// </summary>
        /// <param name="ItemID">道具ID</param>
        /// <param name="num">-1的时候是一次性用光</param>
        public void UseCurrencyResources(long itemIndex,int num = 1)
        {
            Item_ItemChangeResource.request req = new Item_ItemChangeResource.request();
            req.itemIndex = itemIndex;
            req.itemNum = num;
            AppFacade.GetInstance().SendSproto(req);
        }

        /// <summary>
        /// 购买并使用资源道具
        /// </summary>
        /// <param name="ItemID"></param>
        public void BuyAndUseCurrencyResources(int ItemID)
        {
            Role_BuyResource.request req = new Role_BuyResource.request();
            req.itemId = ItemID;
            req.itemNum = 1;
            AppFacade.GetInstance().SendSproto(req);
        }


        public bool ShortOfDenar(long cost)
        {
            if(cost>Gem)
            {
                CoreUtils.uiManager.ShowUI(UI.s_GemShort);
                return true;
            }
            return false;
        }

        public string  GeticonIdByType(int type)
        {
            return CurrencyDefine[type].iconID;
        }
        public int GetNameidByType(EnumCurrencyType type)
        {
            return CurrencyDefine[(int)type].l_desID;
        }

        public string GeticonIdByType(EnumCurrencyType type)
        {
           return GeticonIdByType((int)type);
        }

        /// <summary>
        /// 资源不足获取提示
        /// </summary>
        /// <param name="food">需要的食物</param>
        /// <param name="wood">需要的木材</param>
        /// <param name="stone">需要的石头</param>
        /// <param name="gold">需要的金币</param>
        public void LackOfResources(long food = 0, long wood = 0, long stone = 0, long gold = 0, long itemid = 0,long itemnum = 0)
        {
            if (itemid == 0 & itemnum == 0)
            {
                long[] tmpData = new long[4];
                tmpData[0] = food;
                tmpData[1] = wood;
                tmpData[2] = stone;
                tmpData[3] = gold;
                CoreUtils.uiManager.ShowUI(UI.s_ResShort, null, tmpData);
            }
            else
            {
                long[] tmpData = new long[6];
                tmpData[0] = food;
                tmpData[1] = wood;
                tmpData[2] = stone;
                tmpData[3] = gold;
                tmpData[4] = itemid;
                tmpData[5] = itemnum;
                CoreUtils.uiManager.ShowUI(UI.s_resShortSpecial, null, tmpData);
            }
         
        }

        public void LackOfResourcesByType(EnumCurrencyType type, long num)
        {
            switch (type)
            {
                case EnumCurrencyType.food:
                    LackOfResources(num,0,0,0);
                    break;
                case EnumCurrencyType.wood:
                    LackOfResources(0,num,0,0);
                    break;
                case EnumCurrencyType.stone:
                    LackOfResources(0,0,num,0);
                    break;
                case EnumCurrencyType.gold:
                    LackOfResources(0,0,0,num);
                    break;
            }
        }

        /// <summary>
        /// 计算立即完成所需的价格
        /// </summary>
        /// <returns>返回宝石数量</returns>
        public long CaculateImmediatelyFinishPrice(float lefTime = 0,long needFood = 0,long needWood = 0,long needStone = 0,long needGold = 0, 
            List<RequireItemParam> needItemList = null)
        {
            float price = 0;
            if(lefTime>0)
            {
                price += CaculatePrice(0,lefTime);
            }
            if (needFood > 0)
            {
                price += CaculatePrice(100, needFood-Food);
            }
            if (needWood > 0)
            {
                price += CaculatePrice(200,needWood-Wood);
            }
            if (needStone > 0)
            {
                price += CaculatePrice(300, needStone -Stone);
            }
            if (needGold > 0)
            {
                price += CaculatePrice(400,needGold-Gold);
            }
            if(needItemList != null)
            {
                foreach(var param in needItemList)
                {
                    price += GetItemPrice(param);
                }
            }
            return Mathf.FloorToInt(price);
        }

        private float GetItemPrice(RequireItemParam itemParam)
        {
            if (itemParam == null) return 0;
            if(m_bagProxy == null)
            {
                m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            }
            if (m_bagProxy == null) return 0;
            long itemNum = m_bagProxy.GetItemNum(itemParam.ItemId);
            if (itemNum >= itemParam.Num) return 0;
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemParam.ItemId);
            if (itemCfg == null) return 0;
            return itemCfg.shortcutPrice * (itemParam.Num - itemNum);
        }


        #region 私有方法

        private long GetTrueNumByType(int type,long Float)
        {
            long tmpNum;
            switch((EnumCurrencyType)type)
            {
                case EnumCurrencyType.food:
                    tmpNum = FloatFood;
                    FloatFood += Float;
                    break;
                case EnumCurrencyType.wood:
                    tmpNum = FloatWood;
                    FloatWood += Float;
                    break;
                case EnumCurrencyType.stone:
                    tmpNum = FloatStone;
                    FloatStone += Float;
                    break;
                case EnumCurrencyType.gold:
                    tmpNum = FloatGold;
                    FloatGold += Float;
                    break;
                case EnumCurrencyType.denar:
                    tmpNum = FloatGem;
                    FloatGem += Float;
                    break;
                default: return 0;
            }
            return tmpNum;
        }

        private void SetSequence(Sequence sequence,long first,long final, EnumCurrencyType type)
        {
            long diff = final - first;
            if(diff == 0)
            {
                return;
            }
            if (sequence == null)
            {
                sequence = DOTween.Sequence();
            }
            else
            {
                sequence.Complete();
                sequence = DOTween.Sequence();
            }
            bool changeColor = diff > 0 ? false : true;
            long floatNum = diff / 20;
            Queue<CurrencyFloatParam> param = new Queue<CurrencyFloatParam>();
            for(int i = 0;i<19;i++)
            {
                param.Enqueue(new CurrencyFloatParam {changeColor = changeColor,currencyType = type,number = first+floatNum*i });
            }
            param.Enqueue(new CurrencyFloatParam { currencyType = type, number =final ,changeColor = false});

            for(int i = 0;i<20;i++)
            {
                sequence.AppendInterval(0.06f);
                sequence.AppendCallback(() =>
                {

                });
            }
            sequence.Play().SetEase(Ease.OutCirc);
        }

        private string GetCurrencyRes(int id, ConfigDefine configDefine)
        {
            switch ((EnumCurrencyType)id)
            {
                case EnumCurrencyType.food:
                    return configDefine.flyingFoodRes;
                case EnumCurrencyType.wood:
                    return configDefine.flyingWoodRes;
                case EnumCurrencyType.stone:
                    return configDefine.flyingStoneRes;
                case EnumCurrencyType.gold:
                    return configDefine.flyingGlodRes;
                case EnumCurrencyType.denar:
                    return configDefine.flyingDenarRes;
                default:
                    CoreUtils.logService.Error("未添加该飘飞资源");
                    return string.Empty;
            }
        }

        private Vector2 GetUIPos(Vector3 worldPos)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(worldPos), CoreUtils.uiManager.GetUICamera(),out pos);
            return pos;
        }

        private float CaculatePrice(int type, float num)
        {
            float price = 0;
            if(num<=0)
            {
                return price;
            }
            List<instantPriceDefine> tmpDefines = InstantPriceDefines.FindAll((j) =>
            {
                return type ==j.type;
            });

            int i = 0;
            for (; i < tmpDefines.Count - 1; i++)
            {
                if (tmpDefines[i + 1].num > num)
                {
                    break;
                }
            }
            price = tmpDefines[i].price + (num - (tmpDefines[i].num)) * tmpDefines[i].priceAdd;

            return price;
        }
        #endregion
    }
}