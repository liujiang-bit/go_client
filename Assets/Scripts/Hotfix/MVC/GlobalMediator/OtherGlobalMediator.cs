// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    OtherGlobalMediator 
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using Client;
using Data;
using IGGSDKConstant;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class OtherGlobalMediator : GameMediator
    {
        public static string NameMediator = "OtherGlobalMediator";

        private float m_recordTime;

        private Timer m_timer;

        private int m_remindTaskTime;

        private bool m_isSendRequestProfile;

        private bool m_dispose;

        //IMediatorPlug needs
        public OtherGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public OtherGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AccountBindReddotCheck,
                CmdConstant.LoadAccountProfileFinished,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch (notification.Name)
            {
                case CmdConstant.AccountBindReddotCheck:
                    Check();
                    break;
                case CmdConstant.LoadAccountProfileFinished:
                    Check2();
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void OnRemove()
        {
            base.OnRemove();
            m_dispose = true;
        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
    
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void Check()
        {
            if (m_dispose)
            {
                return;
            }
            //Debug.LogError("开始检测帐号绑定");//todo

            //一天只能执行一次
            int times = PlayerPrefs.GetInt("AccountBindCheckTime");
            bool isCheck = false;
            if (times > 0)
            {
                DateTime currDateTime = ServerTimeModule.Instance.GetCurrServerDateTime();
                DateTime lastTime = ServerTimeModule.Instance.ConverToServerDateTime(times);
                if (currDateTime.Day != lastTime.Day)
                {
                    isCheck = true;
                }
            }
            else
            {
                isCheck = true;
            }

            if (!isCheck)
            {
                //Debug.LogError("下一次检测时间还未到");//todo
                return;
            }

            //判断等级是否满足
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy.CurrentRoleInfo.level < config.accountBindLevel)
            {
                //Debug.LogError("等级不足 不需要检查");//todo
                return;
            }

            PlayerPrefs.SetInt("AccountBindCheckTime", (int)ServerTimeModule.Instance.GetServerTime());
            m_isSendRequestProfile = true;
            AppFacade.GetInstance().SendNotification(CmdConstant.LoadAccountProfile);
        }

        private void Check2()
        {
            if (m_dispose)
            {
                return;
            }
            if (!m_isSendRequestProfile)
            {
                //Debug.LogError("未请求account信息");//todo
                return;
            }
            m_isSendRequestProfile = false;

            IGGUserProfile userProfile = IGGAccountManagementGuideline.shareInstance().getUserProfile();
            if (userProfile == null)
            {
                Debug.LogError("userProfile is null");
                return;
            }
            var profile = userProfile.getBindingProfile(IGGLoginType.IGG_PASSPORT);
            if (profile == null)
            {
                Debug.Log("profile is null");
            }
            else
            {
                //Debug.LogErrorFormat("profile bound status:{0}", profile.isHasBound());//todo
            }
            var accountProxy = AppFacade.GetInstance().RetrieveProxy(AccountProxy.ProxyNAME) as AccountProxy;
            if (profile != null && profile.isHasBound()) //已绑定
            {
                //Debug.LogError("已绑定");//todo
                accountProxy.SetBindReddotStatus(false);
            }
            else//未绑定
            {
                //Debug.LogError("未绑定");//todo
                accountProxy.SetBindReddotStatus(true);
            }
        }
    }
}

