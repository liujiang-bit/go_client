// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    AllianceBuildCheckGlobalMediator 可创建联盟建筑条件检测触发
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class AllianceBuildCheckGlobalMediator : GameMediator
    {
        public static string NameMediator = "AllianceBuildCheckGlobalMediator";

        private Timer m_timer;

        private int m_checkTime;

        private List<List<AllianceBuildingTypeDefine>> m_territoryList;

        private AllianceProxy m_allianceProxy;

        private PlayerProxy m_playerProxy;

        private int m_fixedVal = 10000;

        private bool m_init;

        //IMediatorPlug needs
        public AllianceBuildCheckGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public AllianceBuildCheckGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AllianceBuildCanCreateCheck,
                CmdConstant.AllianceEixtEx,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch (notification.Name)
            {
                case CmdConstant.AllianceBuildCanCreateCheck:
                    Check();
                    break;
                case CmdConstant.AllianceEixtEx: //退出联盟
                    Check();
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
            CancelTimer();
        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            InitData2();
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void InitData2()
        {
            if (m_territoryList == null)
            {
                m_territoryList = new List<List<AllianceBuildingTypeDefine>>();
                for (int i = 0; i < 4; i++)
                {
                    m_territoryList.Add(new List<AllianceBuildingTypeDefine>());
                }
                var clist = CoreUtils.dataService.QueryRecords<AllianceBuildingTypeDefine>();
                for (int i = 0; i < clist.Count; i++)
                {
                    var cinfo = clist[i];
                    if (cinfo.imgShowIndex < 4)
                    {
                        m_territoryList[cinfo.imgShowIndex].Add(cinfo);
                    }
                }
            }
        }

        private void Init()
        {
            if (m_init)
            {
                return;
            }
            m_init = true;

            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_checkTime = config.allianceBuildCheckTime;
            m_timer = Timer.Register(m_checkTime, RequestAllianceInfo, null, true, true);
        }

        private void Check()
        {
            Init();
            CheckCanBuild();
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void RequestAllianceInfo()
        {
            if (m_allianceProxy != null && m_allianceProxy.HasJionAlliance())
            {
                m_allianceProxy.SendGetGuildBuilds(2);
            }
        }
 
        private void CheckCanBuild()
        {
            //主界面是否存在
            MainInterfaceMediator mainMediator = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            if (mainMediator == null)
            {
                return;
            }

            //是否有联盟
            if (!m_allianceProxy.HasJionAlliance())
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildCanCreateFlag, false);
                return;
            }
            //是否有创建权限
            if (m_allianceProxy.GetSelfRoot(GuildRoot.createBuild) == false)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildCanCreateFlag, false);
                return;
            }

            bool isCanBuild = false;
            //判断联盟要塞是否有可创建的
            var fortressList = m_territoryList[0];
            for (int i = 0; i < fortressList.Count; i++)
            {
                isCanBuild = m_allianceProxy.IsCanBuild(fortressList[i]);
                if (isCanBuild)
                {
                    break;
                }
            }

            //判断联盟资源中心是否有可创建的
            if (!isCanBuild)
            {
                var resCenterList = m_territoryList[1];
                for (int i = 0; i < resCenterList.Count; i++)
                {
                    isCanBuild = m_allianceProxy.IsCanBuild(resCenterList[i]);
                    if (isCanBuild)
                    {
                        break;
                    }
                }
            }

            //判断联盟旗帜是否有可创建的
            if (!isCanBuild)
            {
                var flagList = m_territoryList[2];
                if (flagList.Count >0)
                {
                    isCanBuild = m_allianceProxy.IsCanBuild(flagList[0]);
                }
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildCanCreateFlag, isCanBuild);
        }

    }
}

