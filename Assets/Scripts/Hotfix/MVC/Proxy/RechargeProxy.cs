// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    RechargeProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Data;
using IGGSDKConstant;
using Skyunion;
using SprotoType;
using UnityEngine;
using UnityEngine.Networking;

namespace Game {
    public class RechargeProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "RechargeProxy";

        private PlayerProxy m_PlayerProxy;

        private Dictionary<int, int> m_gemMalls = new Dictionary<int, int>();//宝石商城
        private List<int> m_purchases099s = new List<int>();//$0.99礼包
        private List<int> m_purchases499s = new List<int>();//4.99礼包
        private List<int> m_purchases999s = new List<int>();//9.99礼包
        private List<int> m_purchases1999s = new List<int>();//19.99礼包
        private List<int> m_purchases4999s = new List<int>();//49.99礼包
        private List<int> m_purchases9999s = new List<int>();//99.99礼包
        #endregion

        // Use this for initialization
        public RechargeProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" RechargeProxy register");
        }


        public override void OnRemove()
        {
            Debug.Log(" RechargeProxy remove");
        }

        public void Init()
        {
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            SetDenarsStoreList();
        }
        public int GetTotalReddot()
        {
            //TODO 实现宝石商店红点逻辑
            int count = 0;
            count += GetGrowingReddotCount() + GetRiseRoadReddotCount() + GetCitySupplyReddotCount() + GetDayCheapReddotCount();
            return count;
        }
        /// <summary>
        /// 保存一些要用到的数据
        /// </summary>
        public void SetDenarsStoreList()
        {
            var list = CoreUtils.dataService.QueryRecords<PriceDefine>();
            list.ForEach((priceDefine) => {
                if (priceDefine.rechargeType == 4)
                {
                    m_gemMalls.Add(priceDefine.rechargeID, priceDefine.rechargeTypeID);
                }
                if (priceDefine.price == 0.99f)
                {
                    m_purchases099s.Add(priceDefine.rechargeID);
                }
                else if (priceDefine.price == 4.99f)
                {
                    m_purchases499s.Add(priceDefine.rechargeID);
                }
                else if (priceDefine.price == 9.99f)
                {
                    m_purchases999s.Add(priceDefine.rechargeID);
                }
                else if (priceDefine.price == 19.99f)
                {
                    m_purchases1999s.Add(priceDefine.rechargeID);
                }
                else if (priceDefine.price == 49.99f)
                {
                    m_purchases4999s.Add(priceDefine.rechargeID);
                }
                else if (priceDefine.price == 99.99f)
                {
                    m_purchases9999s.Add(priceDefine.rechargeID);
                }
            });
        }
        public int GetDayCheapReddotCount()
        {
            if (m_PlayerProxy == null) return 0;
            int count = IsFreeGiftGot() ? 0 : 1;
            return count;
        }

        public int GetRiseRoadReddotCount()
        {
            if (m_PlayerProxy == null) return 0;
            int count = IsCanCollectCurRiseRoadReward() ? 1 : 0;
            return count;
        }
        
        public int GetCitySupplyReddotCount()
        {
            if (m_PlayerProxy == null) return 0;
            int count = 0;
            foreach (var data in m_PlayerProxy.CurrentRoleInfo.supply)
            {
                if (data.Value.expiredTime > ServerTimeModule.Instance.GetServerTime() && data.Value.award == false)
                {
                    count++;
                }
            }
            return count;
        }
        
        public int GetGrowingReddotCount()
        {
            if (m_PlayerProxy == null) return 0;
            int count = 0;
            if(m_PlayerProxy.CurrentRoleInfo.growthFund)
            {
                var cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                if (cityBuildingProxy == null) return 0;
                var buildingInfo = cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.TownCenter));
                if (buildingInfo == null) return 0;
                var rechargeFundDatas = CoreUtils.dataService.QueryRecords<Data.RechargeFundDefine>();
                foreach (var data in rechargeFundDatas)
                {
                    if (m_PlayerProxy.CurrentRoleInfo.growthFundReward.Contains(data.ID))
                    {
                        continue;
                    }
                    else if (buildingInfo.level >= data.needLv)
                    {
                        count++;
                    }
                }
                count = count > 0 ? 1 : 0;
               
            }
            else
            {
                count = TipRemindProxy.IsShowRemind("GrowingFuncRemind") ? 1 : 0;
            }
            return count;
        }
        /**
         * 是否再开放时间内
         */
        public bool IsInDurationTime(RechargeListDefine cfgData)
        {
            //判断在活动那个开放时间内
            if (cfgData.timeType.CompareTo("-1") == 0)
            {
                return true;
            }
            else
            {
                var arr_str = cfgData.timeType.Split('|');
                if (arr_str.Length == 3)
                {
                    int year, month, day;
                    if (int.TryParse(arr_str[0], out year) && int.TryParse(arr_str[1], out month) &&
                        int.TryParse(arr_str[2], out day))
                    {
                        var designOpenDate = new DateTime(year,month,day);
                        var designCloseDate = designOpenDate.AddSeconds(cfgData.durationTime);
                        var curDate = ServerTimeModule.Instance.GetCurrServerDateTime() ;
                        if (curDate > designOpenDate && curDate < designCloseDate)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /**
         * 是否完成前置
         */
        public bool IsFinishPrepositon(RechargeListDefine cfgData)
        {
            if (cfgData.preposition == 0)
                return true;
            else
            {
                // if(!Enum.IsDefined(typeof(EnumRechargeListPageType),  cfgData.pagingType))
                // {
                //     return false;
                // }
                switch ((EnumRechargeListPageType)cfgData.pagingType)
                {
                    case EnumRechargeListPageType.ChargeRiseRoad:
                        return IsFirstRechargeDone();
                        break;
                    default:
                        break;
                }
            }
            return false;
        }
        public bool IsShowInToggleList(RechargeListDefine cfg)
        {
            if (!IsInDurationTime(cfg))
            {
                return false;
            }
            if (!IsFinishPrepositon(cfg))
            {
                return false;
            }
            if(!IsRechargePageEnable(cfg))
            {
                return false;
            }
            return true;
        }

        private bool IsRechargePageEnable(RechargeListDefine cfg)
        {          
            bool ret = true;
            switch ((EnumRechargeListPageType)cfg.pagingType)
            {
                case EnumRechargeListPageType.ChargeGrowing:
                    {
                        ret = !IsGrowingFundAllClaimed();
                    }
                    break;
                case EnumRechargeListPageType.ChargeDayCheap:
                {
                    ret = !IsDayCheapFinish();
                }
                    break;
                
                case EnumRechargeListPageType.ChargeFirst:
                    ret = !IsFirstRechargeDone();
                    break;
                case EnumRechargeListPageType.ChargeRiseRoad:
                    ret = IsFirstRechargeDone() && TryGetCurRiseRoadCfg(out var data);
                    break;
            }
            return ret;
        }

        public bool IsGrowingFundAllClaimed()
        {
            if (!m_PlayerProxy.CurrentRoleInfo.growthFund) return false;
            var allFundCfgs = CoreUtils.dataService.QueryRecords<RechargeFundDefine>();
            if (allFundCfgs.Count == 0) return true;
            if (m_PlayerProxy.CurrentRoleInfo.growthFundReward.Count != allFundCfgs.Count) return false;
            return true;
        }

        /*
         * 获取城市补给站商品信息
         */
        public Supply GetSupplyInfoById(int priceId)
        {
            if (m_PlayerProxy == null) return null;

            if (m_PlayerProxy.CurrentRoleInfo.supply != null)
            {
                if (m_PlayerProxy.CurrentRoleInfo.supply.ContainsKey(priceId))
                {
                    return m_PlayerProxy.CurrentRoleInfo.supply[priceId];
                }
            }

            return null;
        }

        /*
         * 获取每日特惠礼包奖励Id
         */
        public int GetDayCheapGiftRewardId(RechargeDailySpecialDefine specialDefine)
        {
            for (int i = 0; i < specialDefine.heroLimit.Count; i++)
            {
                var heroLimit = specialDefine.heroLimit[i];
                HeroProxy heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                var hero = heroProxy.GetHeroByID(heroLimit);
                if (!hero.IsAwakening())
                {
                    return specialDefine.itemPackage[i];
                }
            }

            return -1;
        }

        /*
         * 每日特惠活动是否结束
         */
        public bool IsDayCheapFinish()
        {
            var cfgs = CoreUtils.dataService.QueryRecords<RechargeDailySpecialDefine>();
            foreach (var cfg in cfgs)
            {
                if (GetDayCheapGiftRewardId(cfg) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /*
         *  每日特惠免费礼包是否已被领取
         */
        public bool IsFreeGiftGot()
        {
            if (m_PlayerProxy == null) return false;

            return m_PlayerProxy.CurrentRoleInfo.freeDaily;
        }

        /*
         * 每日特惠礼包是否已购买过
         */
        public bool isDailyGiftBought(int priceId)
        {
            if (m_PlayerProxy == null) return false;

            return m_PlayerProxy.CurrentRoleInfo.dailyPackage.Contains(priceId);
        }

        /*
         * 超值礼包是否已购买
         */
        public bool isSuperGiftbought(int group, int id)
        {
            if (m_PlayerProxy == null || m_PlayerProxy.CurrentRoleInfo.rechargeSale == null) return false;
            if (m_PlayerProxy.CurrentRoleInfo.rechargeSale.ContainsKey(group))
            {
                if (m_PlayerProxy.CurrentRoleInfo.rechargeSale[group].ids.Contains(id))
                {
                    return true;
                }
            }

            return false;
        }
        /*
         * 超值礼包是否已购买
         */
        public bool isSuperGiftboughtByGroup(int group)
        {
            if (m_PlayerProxy == null || m_PlayerProxy.CurrentRoleInfo.rechargeSale == null) return false;
            if (m_PlayerProxy.CurrentRoleInfo.rechargeSale.ContainsKey(group))
            {
                    return true;
            }
            return false;
        }


        /*
         * 超值礼包是否可显示
         */
        public bool isSuperGiftCanShow(Data.RechargeSaleDefine rechargeSale,bool isLast)
        {
            if (rechargeSale == null) return false;
            bool isActive = true;
            switch (rechargeSale.giftType)
            {
                case 1://终身限购
                    break;
                case 2://定时开启
                    var date = rechargeSale.data1.Split('|');
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
                    DateTime openTime = new DateTime(int.Parse(date[0]),int.Parse(date[1]),int.Parse(date[2])); 
                    long openTimeSecs = (int)(openTime - dtStart).TotalSeconds;
                    long serverTime = ServerTimeModule.Instance.GetServerTime();
                    if (openTimeSecs > serverTime)
                    {
                        isActive = false;
                    }
                    else if (rechargeSale.data2 > 0 && (rechargeSale.data2 + openTimeSecs) <= ServerTimeModule.Instance.GetServerTime())
                    {
                        isActive = false;
                    }
                    break;
                case 3: //天重置
                case 4: //周重置
                case 5: //月重置
                    break;
                case 6: //活动开启
                    ActivityProxy activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                    ActivityTimeInfo activityInfo1 = activityProxy.GetActivityById(int.Parse(rechargeSale.data1));
                    ActivityTimeInfo activityInfo2 = activityProxy.GetActivityById(rechargeSale.data2);
                    if (ServerTimeModule.Instance.GetServerTime() < activityInfo1.startTime || ServerTimeModule.Instance.GetServerTime() >= activityInfo2.endTime)
                    {
                        isActive = false;
                    }

                    break;
                default:
                    break;
            }

            if (isActive == false)
                return false;

            if (!isSuperGiftbought(rechargeSale.group, rechargeSale.price))
            {
                return true;
            }
            else if (isLast == true)
            {
                return true;
            }

            return false;
        }


        public List<RechargeListDefine> GetRechargeListItemCfgIds()
        {
            var ItemCfg = new List<RechargeListDefine>();
            var cfgs = CoreUtils.dataService.QueryRecords<Data.RechargeListDefine>();
            foreach (var cfg in cfgs)
            {
                if (IsShowInToggleList(cfg))
                    ItemCfg.Add(cfg);
            }
            ItemCfg.Sort((a,b) => { return a.sort.CompareTo(b.sort); });
            return ItemCfg;
        }

        /**
         * 首充-是否首次充值
         */
        public bool IsFirstRechargeDone()
        {
            // var ret = false;
            // var recharge = m_PlayerProxy.CurrentRoleInfo.recharge;
            // foreach (var v in recharge)
            // {
            //     if (v.Value.HasId && v.Value.HasCount && v.Value.count > 0)
            //     {
            //         ret = true;
            //     }
            // }
            return m_PlayerProxy.CurrentRoleInfo.rechargeFirst;
        }
        
        /**
         * 宝石商店-是否首次充值单项加赠
         */
        public bool IsFirstAdd(int id)
        {
            //是否首次充值加赠
            var recharge = m_PlayerProxy.CurrentRoleInfo.recharge;
            foreach (var v in recharge)
            {
                if (v.Value.HasId)
                {
                    Debug.Log("v.Value"+ v.Value.id+" v.Value.count"+ v.Value.count);
                    if (v.Value.id == id)
                    {
                        return v.Value.count <= 0;
                    }
                }
            }
            return true;
        }
        /**
         * 获得宝石商店价格
         * 后期考虑挤出通用或更名
         */
        public bool TryGetGemShopSDKPrice(ref string Price)
        {
            //SDK是否返回价格
            var items = IGGPayment.shareInstance().GetIGGGameItems();
            foreach (var v in items)
            {
                if (v.getId().CompareTo(Price) == 0)
                {
                    Price = v.getShopCurrencyPrice();
                    return true;
                }
            }
            return false;
        }

        public string GetPriceString(int priceId)
        {
            string strPrice = string.Empty;
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(priceId);
            if (priceCfg != null)
            {
                var gameItems = IGGPayment.shareInstance().GetIGGGameItems();
                if (gameItems != null)
                {
                    IGGGameItem funGameItem = null;
                    foreach (var gameItem in gameItems)
                    {
                        if (gameItem.getId() == priceCfg.rechargeID.ToString())
                        {
                            funGameItem = gameItem;
                            break;
                        }
                    }

                    if (funGameItem != null && funGameItem.getPurchase() != null)
                    {
                        strPrice = funGameItem.getShopCurrencyPrice();
                    }
                }

                if (string.IsNullOrEmpty(strPrice))
                {
                    strPrice = $"${priceCfg.price}";
                }
            }

            return strPrice;
        }

        class BuyInfo
        {
            public string account;
            public string serverName;
            public string time;
            public string pcid;
            public string price;
            public string rid;
        }

        long lastBuyTime = 0;
        public void CallSdkBuyByPcid(PriceDefine priceDefine, string pcid, string price, IGGPaymentPayload payload = null)
        {

            string key = PlayerProxy.signKey;

            long curTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (curTime - lastBuyTime < 1000)
            {
                Tip.CreateTip("操作频繁，请等待");
                return;
            }
            lastBuyTime = curTime;
            //发起购买
            Debug.Log("发起购买" + pcid);

            MD5 md5Hash = MD5.Create();
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            BuyInfo buyInfo = new BuyInfo();
            WWWForm form = new WWWForm();
            buyInfo.account = IGGSDKConstant.IGGDefault.IGGID;
            form.AddField("account", buyInfo.account);

            buyInfo.time = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            form.AddField("time", buyInfo.time);

            buyInfo.pcid = pcid;
            form.AddField("pcid", buyInfo.pcid);

            buyInfo.price = (priceDefine.price + 0.01f).ToString();
            form.AddField("price", buyInfo.price);

            buyInfo.serverName = PlayerProxy.curServerImformation.sid;
            form.AddField("servername", buyInfo.serverName);

            buyInfo.rid = playerProxy.CurrentRoleInfo.rid.ToString();
            form.AddField("rid", buyInfo.rid);

            string hasher = $"{buyInfo.time}:{buyInfo.pcid}:{key}:{buyInfo.price}:{buyInfo.account}:{key}:{buyInfo.rid}:{buyInfo.serverName}";
            byte[] result = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hasher));
            string resultMD5 = BitConverter.ToString(result).Replace("-", "").ToLower();
            form.AddField("sign", resultMD5);

            UnityWebRequest request = new UnityWebRequest($"http://{PlayerProxy.curServerImformation.host}:88/api/pay.php", UnityWebRequest.kHttpVerbPOST);

            request.uploadHandler = new UploadHandlerRaw(form.data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            var sendRequest = request.SendWebRequest();
            sendRequest.completed += (op) =>
            {
                if (!request.isNetworkError && !request.isHttpError)
                {
                    Debug.Log(request.responseCode);
                    Debug.Log(request.downloadHandler.text);
                    switch (request.downloadHandler.text)
                    {
                        case "OK":
                            var isBuyItemWork = IGGPayment.shareInstance().buyItem(pcid, (IGGException ex, bool bIsUserCancle) =>
                            {
                                if (ex.isNone())
                                {
                                    if (bIsUserCancle)
                                    {
                                        Debug.Log("发起购买-用户取消");
                                        return;
                                    }
                                    int tempPcid = 0;

                                    string curency = "USD";
                                    var gameitem = IGGPayment.shareInstance().GetGameItem(pcid);
                                    if (gameitem != null)
                                    {
                                        price = gameitem.getShopPrice();
                                        curency = gameitem.getShopCurrencyCode();
                                    }
                                    AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases, $"{curency}|{price}"));
                                    AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.PurchasesID, pcid));
                                    if (int.TryParse(pcid, out tempPcid))
                                    {
                                        Debug.Log("发起购买-成功" + tempPcid);
                                        Tip.CreateTip(priceDefine.l_succeededID).Show();
                                        if (m_gemMalls.ContainsKey(tempPcid))
                                        {
                                            RechargeGemMallDefine rechargeGemMallDefine = CoreUtils.dataService.QueryRecord<RechargeGemMallDefine>(m_gemMalls[tempPcid]);
                                            if (rechargeGemMallDefine != null)
                                            {
                                                AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.earn_virtual_currency, rechargeGemMallDefine.denarNum.ToString()));
                                            }
                                        }
                                        if (m_purchases099s.Contains(tempPcid))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases099));
                                        }
                                        else if (m_purchases499s.Contains(tempPcid))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases499));
                                        }
                                        else if (m_purchases999s.Contains(tempPcid))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases999));
                                        }
                                        else if (m_purchases1999s.Contains(tempPcid))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases1999));
                                        }
                                        else if (m_purchases4999s.Contains(tempPcid))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases4999));
                                        }
                                        else if (m_purchases9999s.Contains(tempPcid))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Purchases9999));
                                        }
                                    }
                                }
                                else
                                {
                                    if (bIsUserCancle)
                                    {
                                        Debug.Log("发起购买-用户取消");
                                        return;
                                    }
                                    Debug.Log(ex.ToString());
                                    Tip.CreateTip(300292).Show();
                                }
                            }, payload);
                            if (!isBuyItemWork)
                            {
                                int limitType = (int)IGGPayment.shareInstance().getPurchaseLimit();
                                if (limitType != (int)IGGPaymentPurchaseLimitation.IGGPaymentPurchaseLimitationNone)
                                {
                                    Tip.CreateTip(300246).Show();
                                }
                                else
                                {
                                    Debug.Log("发起购买-SDK未工作");
                                    Tip.CreateTip(300292).Show();
                                }
                            }
                            else
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.initiated_checkout));
                            }
                            break;
                        default:
                            Tip.CreateTip(request.downloadHandler.text).Show();
                            break;
                    }
                }
                else
                {
                    Tip.CreateTip($"无法更新服务器列表，请检查网络和服务器状态。错误代码{request.responseCode}").Show();
                }
            };
        }
        /**
         * 获得当前成长之路的配置
         */
        public bool TryGetCurRiseRoadCfg(out RechargeFirstDefine ret)
        {
            var lstCollected = m_PlayerProxy.CurrentRoleInfo.riseRoadPackage;
            var lstCfgRise = CoreUtils.dataService.QueryRecords<RechargeFirstDefine>();
            foreach (var v in lstCfgRise)
            {
                if (v.type == 2 && !lstCollected.Contains(v.ID))
                {
                    ret = v;
                    return true;
                }
            }
            ret = null;
            return false;
        }
        /**
         * 当前成长之路是否可以领取
         */
        public bool IsCanCollectCurRiseRoadReward()
        {
            var curRiseValue = m_PlayerProxy.CurrentRoleInfo.riseRoad;
            if (TryGetCurRiseRoadCfg(out var cfgData))
            {
                if (cfgData.needDenar <= curRiseValue)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetJumpToPageType(int jumpType , ref int pageType)
        {
            var lstAllJumpCfg = CoreUtils.dataService.QueryRecords<Data.JumpTypeDefine>();
            var jumpTypeDefines = lstAllJumpCfg.FindAll(cfg => cfg.group == jumpType);
            foreach (var v in jumpTypeDefines)
            {
                switch ((EnumRechargeListPageViewType)v.typeData1)
                {
                    case EnumRechargeListPageViewType.ChargeFirst:
                        pageType = (int)EnumRechargeListPageType.ChargeFirst;
                        return true;
                    case EnumRechargeListPageViewType.ChargeRiseRoad:
                        pageType = (int)EnumRechargeListPageType.ChargeRiseRoad;
                        return true;
                    case EnumRechargeListPageViewType.ChargeSuperGift:
                        pageType = (int)EnumRechargeListPageType.ChargeSuperGift;
                        return true;
                    case EnumRechargeListPageViewType.ChargeCitySupply:
                        pageType = (int)EnumRechargeListPageType.ChargeCitySupply;
                        return true;
                    case EnumRechargeListPageViewType.ChargeGrowing:
                        pageType = (int)EnumRechargeListPageType.ChargeGrowing;
                        return true;
                    case EnumRechargeListPageViewType.ChargeDayCheap:
                        pageType = (int)EnumRechargeListPageType.ChargeDayCheap;
                        return true;
                    case EnumRechargeListPageViewType.ChargeGemShop:
                        pageType = (int)EnumRechargeListPageType.ChargeGemShop;
                        return true;
                }
            }
            return false;
        }
    }
}