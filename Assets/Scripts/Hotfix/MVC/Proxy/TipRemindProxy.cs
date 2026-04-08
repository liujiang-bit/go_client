// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    tip提醒通用判断
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
    public class TipRemindProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "TipRemindProxy";

        public const string TavernSummonRemind = "TavernSummonRemind";              //白银宝箱提醒
        public const string TavernGoldSummonRemind = "TavernGoldSummonRemind";      //黄金宝箱提醒
        public const string StoreBuyRemind = "StoreBuyRemind";
        public const string AddStarExpSureItemRemind = "AddStarExpSureItemRemind";
        public const string ExchangeActivityRemind = "ExchangeActivityRemind";
        public const string MysteryStoreBuyRemind = "MysteryStoreBuyRemind";
        public const string MysteryStoreRefreshRemind = "MysteryStoreRefreshRemind";
        public const string GrowingFuncRemind = "GrowingFuncRemind";
        public const string ExpeiditonStoreResetRemind = "ExpeiditonStoreResetRemind";
        public const string ActivityLuckyDrawCostRemind = "ActivityLuckyDrawCostRemind";

        public const string ActivityCalendarReddotTotal = "ActivityCalendarReddotTotal";
        public const string LastSelectActivityId = "LastSelectActivityId";

        #endregion

        // Use this for initialization
        public TipRemindProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" TipRemindProxy register");
        }

        public override void OnRemove()
        {
            Debug.Log(" TipRemindProxy remove");
        }

        public static bool IsShowRemind(string key)
        {
            bool isShowRemind = false;
            int times = PlayerPrefs.GetInt(key);
            if (times > 0)
            {
                DateTime time1 = ServerTimeModule.Instance.GetCurrServerDateTime();
                DateTime time2 = ServerTimeModule.Instance.ConverToServerDateTime(times);
                if (time1.Month != time2.Month || time1.Day != time2.Day)
                {
                    isShowRemind = true;
                }
            }
            else
            {
                isShowRemind = true;
            }
            return isShowRemind;
        }

        public static void SaveRemind(string key)
        {
            PlayerPrefs.SetInt(key, (int)ServerTimeModule.Instance.GetServerTime());
        }
    }
}