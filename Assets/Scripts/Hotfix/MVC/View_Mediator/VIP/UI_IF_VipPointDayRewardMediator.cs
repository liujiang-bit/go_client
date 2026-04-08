// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_IF_VipPointDayRewardMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_IF_VipPointDayRewardMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_VipPointDayRewardMediator";

        private PlayerProxy m_playerProxy;
        #endregion

        //IMediatorPlug needs
        public UI_IF_VipPointDayRewardMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_VipPointDayRewardView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void OnRemove()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideCheck, (int)EnumFuncGuide.ReceiveVipBox);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            RefreshInfo(m_playerProxy.CurrentRoleInfo);
            view.m_pl_close_PolygonImage.gameObject.SetActive(true);
            Timer.Register(1.5f, () => { view.m_pl_close_PolygonImage.gameObject.SetActive(false); }, null, false, false, view.vb);
        }
       
        #endregion

        private void RefreshInfo(RoleInfoEntity info)
        {
            int todayReward ;
            int tomorrowReward;
            GetVIPReward(info,out todayReward,out tomorrowReward);

            view.m_lbl_todayVal_LanguageText.text = todayReward.ToString();
            view.m_lbl_tomorrowVal_LanguageText.text = tomorrowReward.ToString();
            view.m_lbl_loginDay_LanguageText.text = info.continuousLoginDay.ToString();
        }


        private void GetVIPReward(RoleInfoEntity info,out int todayReward, out int tomorrowReward)
        {
            todayReward = -1;
            tomorrowReward = -1;
            var vipDayPointConfig = m_playerProxy.GetVipDayPointInfo(info.continuousLoginDay);
            if (vipDayPointConfig == null)
            {
                return;
            }
            todayReward = vipDayPointConfig.point;
            
            vipDayPointConfig = m_playerProxy.GetVipDayPointInfo(info.continuousLoginDay + 1);
            tomorrowReward = vipDayPointConfig.point;
        }
    }
}