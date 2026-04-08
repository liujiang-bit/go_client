// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    CityGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoTemp;
using Client;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using Client.FSM;
using Data;
using SprotoType;
using Random = UnityEngine.Random;
using UnityEngine.Profiling;

namespace Game
{
    public class SendClientDeviceInfoMedia : GameMediator
    {
        #region Member

        public static string NameMediator = "SendClientDeviceInfoMedia";
        private bool m_bCanReportData = false;
        private float m_netReportTime;
        private float m_reportTick = 30.0f;

        #endregion

        //IMediatorPlug needs
        public SendClientDeviceInfoMedia() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public SendClientDeviceInfoMedia(object viewComponent) : base(NameMediator, null)
        {
            
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Role_RoleLogin.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_RoleLogin.TagName:
                    m_bCanReportData = true;
                    m_netReportTime = UnityEngine.Time.realtimeSinceStartup;
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            this.IsOpenUpdate = true;
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
            if (m_bCanReportData)
            {
                if (UnityEngine.Time.realtimeSinceStartup >= m_netReportTime)
                {
                    ReportData();
                    m_netReportTime = UnityEngine.Time.realtimeSinceStartup + m_reportTick;
                }
            }
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        #endregion

        public static void ReportData()
        {
            var sp = new Role_ReportSelf.request();

            sp.quality = CoreUtils.GetGraphicLevel().ToString();
            sp.memory = $"{Profiler.GetTotalAllocatedMemoryLong() / 1048576f}/{Profiler.GetTotalReservedMemoryLong()/ 1048576f}/{SystemInfo.systemMemorySize}";
            sp.fps = Mathf.CeilToInt(1.0f / Time.deltaTime);
            sp.network = ((int)Application.internetReachability).ToString();
            sp.power = 100;
            sp.chargeStatus = 0;
            sp.volume = 100;


#if UNITY_EDITOR || UNITY_STANDLONE
#elif UNITY_ANDROID

            try
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject audioManager = currentActivity.Call<AndroidJavaObject>("getSystemService", new AndroidJavaObject("java.lang.String", "audio"));
                int volume = audioManager.Call<int> ("getStreamVolume", 3);
                int maxVolume = audioManager.Call<int> ("getStreamMaxVolume", 3);

                sp.volume = 100 * volume / maxVolume;

                AndroidJavaObject intenFilter = new AndroidJavaObject("android.content.IntentFilter", "android.intent.action.BATTERY_CHANGED");
                AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("registerReceiver", null, intenFilter);
                int level = intent.Call<int>("getIntExtra", "level", 0);
                int scale = intent.Call<int>("getIntExtra", "scale", 1);
                int status = intent.Call<int>("getIntExtra", "status", 0);
                sp.power = 100 * level / scale;
                sp.chargeStatus = status;

//int BATTERY_STATUS_CHARGING = 2 充电中
//int BATTERY_STATUS_DISCHARGING = 3  放电中
//int BATTERY_STATUS_NOT_CHARGING = 4 未充电
//int BATTERY_STATUS_FULL = 5 已充满
//int BATTERY_STATUS_UNKNOWN = 1  状态未知
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }
#endif
            CoreUtils.logService.Debug($"quality:{ sp.quality}\nmemory:{sp.memory}\nfps:{sp.fps}\nnetwork:{sp.network}\npower:{sp.power}\nchargeStatus:{sp.chargeStatus}\nvolume:{sp.volume}", Color.green);
            AppFacade.GetInstance().SendSproto(sp);
        }
    }
}