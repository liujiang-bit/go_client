// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月16日
// Update Time         :    2020年7月16日
// Class Description   :    EventTrackingCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using SprotoType;
using Skyunion;
using System;

namespace Game
{
    public class EventTrackingData
    {
        public EnumEventTracking enumEventTracking;
        public  string value;

        public EventTrackingData(EnumEventTracking enumEventTracking, string value)
        {
            this.enumEventTracking = enumEventTracking;
            this.value = value;
        }
        public EventTrackingData(EnumEventTracking enumEventTracking )
        {
            this.enumEventTracking = enumEventTracking;
            this.value = string.Empty;
        }
    }
    public enum EnumEventTracking
    {
        APP_INSTALL = 0,     // 程序启动的位置进行SDK初始化 只发一次
        SIGN_UP = 1,//在安装后第一次得到 IGGID 时候，发送 SIGN_UP 事件（后面操作不再执行，可在设备端通过缓存判断是否已经执行）只发一次
        SetLanguage = 2,//新玩家首次选择游戏语言时
        CreatingCharacter = 3,//新玩家首次进入创角界面时
        ChooseCivilization = 4,//新玩家首次进入创角界面并点击选择了一个初始文明时
        Character = 5,//新玩家首次点击创建角色按钮且创角角色成功时
        APP_LAUNCH = 6,//新用户创建角色成功进入游戏时，老用户登录成功进入游戏时
        DAY2_RETENTION = 7,//次日留存率：
        IOS_VERSION_AND_ABOVE = 8,//统计的符合 iOS 系统要求的用户。
        ANDROID_VERSION_AND_ABOVE = 9,//统计的符合 Android 系统要求的用户。
        tutorial_begin = 10,//第一步新手引导开始时
        initiated_checkout = 11,//开始充值并触发充值SDK时
        Purchases = 12,//技术部返回购买成功, 并且要上传购买金额
        spent_credits = 13,//当用户在应用中支出虚拟货币（金币、宝石、代币等）时触发。
        earn_virtual_currency = 14,//当用户在应用中购买的虚拟货币（金币、宝石、代币等）到账时触发。
        Purchases099 = 15,// 玩家首次购买首次购买$0.99礼包，技术部返回购买成功时
        Purchases499 = 16,//玩家首次购买首次购买$4.99礼包，技术部返回购买成功时
        Purchases999 = 17,//玩家首次购买首次购买$9.99礼包，技术部返回购买成功时
        Purchases1999 = 18,//玩家首次购买首次购买$19.99礼包，技术部返回购买成功时
        Purchases4999 = 19,//玩家首次购买首次购买$49.99礼包，技术部返回购买成功时
        Purchases9999 = 20, //玩家首次购买首次购买$99.99礼包，技术部返回购买成功时
        join_group = 21,//成功加入联盟时第一次加入联盟，并被同意加入
        tutorial_completion = 22,//玩家首次免费改名成功时
        OpenMall = 23,//玩家首次打开商城界面
        LevelAchieved = 24,//只有在城堡等级提升的时候才需要上报(不要重复上报)非slg游戏根据项目埋入玩家成长等级如：城堡等级 =（5,10,11,13,15,16,19,20,25）上报
        AchievementUnlocked = 25,//无此功能和埋点
        PurchasesID = 26,//玩家每次付费成功时
        Stage1 = 27,//玩家完成主线任务第1章时
        Stage2 = 28,//玩家完成主线任务第2章时
        Stage3 = 29,//玩家完成主线任务第3章时
        Stage4 = 30,//玩家完成主线任务第4章时
        Activity = 31,//玩家首次活跃度达到100时
        WorldChat = 32,//玩家首次世界频道发言
        BarbarianLevel1 = 33,//首次对等级1的野蛮人发起战斗时
        BarbarianLevel2 = 34,//首次对等级1的野蛮人发起战斗时
        BarbarianLevel3 = 35,//首次对等级1的野蛮人发起战斗时
        BarbarianLevel4 = 36,//首次对等级1的野蛮人发起战斗时
        BarbarianLevel5 = 37,//首次对等级1的野蛮人发起战斗时
        BarbarianLevel6 = 38,//首次对等级1的野蛮人发起战斗时
        BarbarianStronghold = 39,//首次发起或参加某个等级的野蛮人城寨战斗时，1~2级
        SoloBattle = 40,//首次单人部队发起会导致PVP战斗的行军时
        RallyBattle = 41,//首次发起或参加会导致PVP战斗的集结时
        reach_castlelv5 = 42,//玩家的城堡升级到5级时触发
        reach_castlelv10 = 43,//玩家的城堡升级到10级时触发
        reach_castlelv13 = 44,//玩家的城堡升级到13级时触发
        reach_castlelv15 = 45,//玩家的城堡升级到15级时触发
        reach_castlelv19 = 46,//玩家的城堡升级到19级时触发
        reach_castlelv22 = 47,//玩家的城堡升级到22级时触发
    }

    public class EventTrackingCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            PlayerProxy m_playerProxy = Facade.RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            EventTrackingData eventTrackingData = notification.Body as EventTrackingData;
            if (eventTrackingData == null)
            {
                return;
            }
            EnumEventTracking enumEventTracking = eventTrackingData.enumEventTracking;
            switch (enumEventTracking)
            {
                case EnumEventTracking.SetLanguage:
                case EnumEventTracking.CreatingCharacter:
                case EnumEventTracking.ChooseCivilization:
                case EnumEventTracking.Character:
                case EnumEventTracking.tutorial_begin:
                    {
                        if (PlayerPrefs.HasKey(enumEventTracking.ToString()))
                            return;
                        PlayerPrefs.SetString(enumEventTracking.ToString(), "1");
                        PlayerPrefs.Save();
                        SendEvent(enumEventTracking, eventTrackingData.value);
                        break;
                    }
                case EnumEventTracking.earn_virtual_currency:
                case EnumEventTracking.Purchases1999:
                case EnumEventTracking.Purchases499:
                case EnumEventTracking.Purchases4999:
                case EnumEventTracking.Purchases999:
                case EnumEventTracking.Purchases9999:
                case EnumEventTracking.Purchases099:
                case EnumEventTracking.join_group:
                case EnumEventTracking.tutorial_completion:
                case EnumEventTracking.OpenMall:
                case EnumEventTracking.Stage1:
                case EnumEventTracking.Stage2:
                case EnumEventTracking.Stage3:
                case EnumEventTracking.Stage4:
                case EnumEventTracking.Activity:
                case EnumEventTracking.WorldChat:
                case EnumEventTracking.BarbarianLevel1:
                case EnumEventTracking.BarbarianLevel2:
                case EnumEventTracking.BarbarianLevel3:
                case EnumEventTracking.BarbarianLevel4:
                case EnumEventTracking.BarbarianLevel5:
                case EnumEventTracking.BarbarianLevel6:
                case EnumEventTracking.BarbarianStronghold:
                case EnumEventTracking.SoloBattle:
                case EnumEventTracking.RallyBattle:
                case EnumEventTracking.reach_castlelv5:
                case EnumEventTracking.reach_castlelv10:
                case EnumEventTracking.reach_castlelv13:
                case EnumEventTracking.reach_castlelv15:
                case EnumEventTracking.reach_castlelv19:
                case EnumEventTracking.reach_castlelv22:
                    {
                        if ((m_playerProxy.CurrentRoleInfo.eventTrancking & (long)Math.Pow(2, (int)enumEventTracking)) == 0)
                        {
                            SendEvent(enumEventTracking, eventTrackingData.value);
                            long eventTracking = m_playerProxy.CurrentRoleInfo.eventTrancking + (long)Math.Pow(2, (int)enumEventTracking);
                            m_playerProxy.CurrentRoleInfo.eventTrancking = eventTracking;
                            SendSproto(eventTracking);
                        }
                        break;
                    }
                case EnumEventTracking.Purchases:
                case EnumEventTracking.initiated_checkout:
                case EnumEventTracking.spent_credits:
                case EnumEventTracking.LevelAchieved:
                case EnumEventTracking.PurchasesID:
                    {
                        SendEvent(enumEventTracking, eventTrackingData.value);
                        if ((m_playerProxy.CurrentRoleInfo.eventTrancking & (long)Math.Pow(2, (int)enumEventTracking)) == 0)
                        {
                            long eventTracking = m_playerProxy.CurrentRoleInfo.eventTrancking + (long)Math.Pow(2, (int)enumEventTracking);
                            m_playerProxy.CurrentRoleInfo.eventTrancking = eventTracking;
                            SendSproto(eventTracking);
                        }
                        break;
                    }

                case EnumEventTracking.APP_INSTALL:
                case EnumEventTracking.SIGN_UP:
                case EnumEventTracking.APP_LAUNCH:
                case EnumEventTracking.DAY2_RETENTION:
                case EnumEventTracking.IOS_VERSION_AND_ABOVE:
                case EnumEventTracking.ANDROID_VERSION_AND_ABOVE:
                case EnumEventTracking.AchievementUnlocked:
                    { 
                        Debug.LogErrorFormat("不能处理的数据类型");
                    }
                    break;
                default:
                    break;
            }
        }
        private void SendEvent(EnumEventTracking eventTracking, string value)
        {
            string key = ConvertToEventKey(eventTracking);
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //CoreUtils.adService.SendEvent(key, value);
            }
          //  Debug.LogError("AD "  + eventTracking.ToString() +  " "+ value);
        }
        private void SendSproto(long eventTracking)
        {
            Role_UpdateEventTrancking.request request = new Role_UpdateEventTrancking.request();
            request.eventTrancking = eventTracking;
            AppFacade.GetInstance().SendSproto(request);
          //  Debug.LogError("SendSproto " + eventTracking.ToString());
        }
        /// <summary>
        ///  枚举转字符串
        /// </summary>
        public string ConvertToEventKey(EnumEventTracking enumEventTracking)
        {
            string eventKey = string.Empty;
            switch (enumEventTracking)
            {
                case EnumEventTracking.Activity:
                    eventKey = "Activity";
                    break;
                case EnumEventTracking.AchievementUnlocked:
                    eventKey = "Achievements Unlocked";
                    break;
                case EnumEventTracking.initiated_checkout:
                    eventKey = "Initiated Checkout";
                    break;
                case EnumEventTracking.LevelAchieved:
                    eventKey = "Levels Achieved";
                    break;
                case EnumEventTracking.Purchases:
                    eventKey = "Purchases";
                    break;
                case EnumEventTracking.Purchases1999:
                    eventKey = "Purchases1999";
                    break;
                case EnumEventTracking.Purchases499:
                    eventKey = "Purchases499";
                    break;
                case EnumEventTracking.Purchases4999:
                    eventKey = "Purchases4999";
                    break;
                case EnumEventTracking.Purchases999:
                    eventKey = "Purchases999";
                    break;
                case EnumEventTracking.Purchases9999:
                    eventKey = "Purchases9999";
                    break;
                case EnumEventTracking.Purchases099:
                    eventKey = "Purchases099";
                    break;
                case EnumEventTracking.spent_credits:
                    eventKey = "Spent Credits";
                    break;
                case EnumEventTracking.tutorial_completion:
                    eventKey = "Tutorials Completed";
                    break;
                case EnumEventTracking.ANDROID_VERSION_AND_ABOVE:
                    eventKey = " ";
                    break;
                case EnumEventTracking.BarbarianLevel1:
                    eventKey = "Barbarian 1";
                    break;
                case EnumEventTracking.BarbarianLevel2:
                    eventKey = "Barbarian 2";
                    break;
                case EnumEventTracking.BarbarianLevel3:
                    eventKey = "Barbarian 3";
                    break;
                case EnumEventTracking.BarbarianLevel4:
                    eventKey = "Barbarian 4";
                    break;
                case EnumEventTracking.BarbarianLevel5:
                    eventKey = "Barbarian 5";
                    break;
                case EnumEventTracking.BarbarianLevel6:
                    eventKey = "Barbarian 6";
                    break;
                case EnumEventTracking.BarbarianStronghold:
                    eventKey = "Barbarian Stronghold";
                    break;
                case EnumEventTracking.Character:
                    eventKey = "Character";
                    break;
                case EnumEventTracking.ChooseCivilization:
                    eventKey = "Choose Civilization";
                    break;
                case EnumEventTracking.CreatingCharacter:
                    eventKey = "Creating Character";
                    break;
                case EnumEventTracking.DAY2_RETENTION:
                    eventKey = "DAY2_RETENTION";
                    break;
                case EnumEventTracking.earn_virtual_currency:
                    eventKey = "earn_virtual_currency";
                    break;
                case EnumEventTracking.APP_LAUNCH:
                    eventKey = "App Launches";
                    break;
                case EnumEventTracking.APP_INSTALL:
                    eventKey = "App Installs";
                    break;
                case EnumEventTracking.IOS_VERSION_AND_ABOVE:
                    eventKey = " ";
                    break;
                case EnumEventTracking.join_group:
                    eventKey = "join_group";
                    break;
                case EnumEventTracking.OpenMall:
                    eventKey = "Open Mall";
                    break;
                case EnumEventTracking.PurchasesID:
                    eventKey = "PurchasesID";
                    break;
                case EnumEventTracking.RallyBattle:
                    eventKey = "Rally Battle";
                    break;
                case EnumEventTracking.reach_castlelv10:
                    eventKey = "reach_castlelv10";
                    break;
                case EnumEventTracking.reach_castlelv13:
                    eventKey = "reach_castlelv13";
                    break;
                case EnumEventTracking.reach_castlelv15:
                    eventKey = "reach_castlelv15";
                    break;
                case EnumEventTracking.reach_castlelv19:
                    eventKey = "reach_castlelv19";
                    break;
                case EnumEventTracking.reach_castlelv22:
                    eventKey = "reach_castlelv22";
                    break;
                case EnumEventTracking.reach_castlelv5:
                    eventKey = "reach_castlelv5";
                    break;
                case EnumEventTracking.SetLanguage:
                    eventKey = "Set Language";
                    break;
                case EnumEventTracking.SIGN_UP:
                    eventKey = "SIGN_UP";
                    break;
                case EnumEventTracking.SoloBattle:
                    eventKey = "Solo Battle";
                    break;
                case EnumEventTracking.Stage1:
                    eventKey = "Stage1";
                    break;
                case EnumEventTracking.Stage2:
                    eventKey = "Stage2";
                    break;
                case EnumEventTracking.Stage3:
                    eventKey = "Stage3";
                    break;
                case EnumEventTracking.Stage4:
                    eventKey = "Stage4";
                    break;
                case EnumEventTracking.tutorial_begin:
                    eventKey = "tutorial_begin";
                    break;
                case EnumEventTracking.WorldChat:
                    eventKey = "World Chat";
                    break;
                default:
                    break;
            }
            return eventKey;
        }

    }
}