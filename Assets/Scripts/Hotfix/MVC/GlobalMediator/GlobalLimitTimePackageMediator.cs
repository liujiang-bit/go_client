// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    GlobalLimitTimePackageMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;
using Skyunion;

namespace Game {
    public class GlobalLimitTimePackageMediator : GameMediator {
        #region Member
        public static string NameMediator = "GlobalLimitTimePackageMediator";
        
        private List<LimitTimePackage> m_limitPackageList = new List<LimitTimePackage>();
        private bool m_IsPackageShowing = false;
        private bool m_isInCity;
        private bool m_IsDuringAge = false;
        private bool m_IsInDialog = false;

        #endregion

        //IMediatorPlug needs
        public GlobalLimitTimePackageMediator():base(NameMediator, null ) {}

        public void SetPackageState(bool isShowing)
        {
            m_IsPackageShowing = isShowing;
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.LimitTimePackageChange,
                CmdConstant.EnterCityShow,
                CmdConstant.ExitCityHide,
                CmdConstant.LimitTimePackageState,
                CmdConstant.AgeStart,
                CmdConstant.AgeEnd,
                CmdConstant.ShowNPCDiaglog,
                CmdConstant.OnNPCDiaglogEnd,
                CmdConstant.ShowChapterDiaglog,
                CmdConstant.OnChapterDiaglogEnd
            }.ToArray();
        }

       public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.LimitTimePackageChange:
                {
                    var packageInfos = notification.Body as Dictionary<long,LimitTimePackage>;
                    foreach (var KeyValue in packageInfos)
                    {
                        var packageInfo = KeyValue.Value;
                        //  -1 表示过期或已购买
                        if (packageInfo.id == -1)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.RemoveLimitTimePackage,packageInfo.index);
                            ClearPopState(packageInfo.index);
                        } else if (packageInfo != null && packageInfo.expiredTime > 0 &&
                            packageInfo.expiredTime > ServerTimeModule.Instance.GetServerTime())
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.NewLimitTimePackage,packageInfo);
                            if (isCanPop(packageInfo.index))
                            {
                                InsertLimitPackage(packageInfo);
                            }
                        }
                    }
                    
                }
                    break;
                case CmdConstant.LimitTimePackageState:
                {
                    var isOpenning = (bool)notification.Body;
                    SetPackageState(isOpenning);
                }
                    break;
                case CmdConstant.EnterCityShow:
                {
                    m_isInCity = true;
                }
                    break;
                case CmdConstant.ExitCityHide:
                {
                    m_isInCity = false;
                }
                    break;
                case CmdConstant.AgeStart:
                {
                    m_IsDuringAge = true;
                }
                    break;
                case CmdConstant.AgeEnd:
                {
                    m_IsDuringAge = false;
                }
                    break;
                case CmdConstant.ShowNPCDiaglog:
                {
                    m_IsInDialog = true;
                }
                    break;
                case CmdConstant.OnNPCDiaglogEnd:
                {
                    m_IsInDialog = false;
                }
                    break;
                case CmdConstant.ShowChapterDiaglog:
                {
                    m_IsInDialog = true;
                }
                    break;
                case CmdConstant.OnChapterDiaglogEnd:
                {
                    m_IsInDialog = false;
                }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {

        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        public override void Update()
        {
            if (m_IsPackageShowing || m_limitPackageList.Count < 1 || GuideProxy.IsGuideing || !m_isInCity || m_IsDuringAge || m_IsInDialog ||
                CoreUtils.uiManager.LayerCount(UILayer.WindowLayer) > 0) return;

            if (CoreUtils.uiManager.ExistUI(UI.s_ChargePop))
            {
                return;
            }

            var packageInfo = m_limitPackageList[0];
            CoreUtils.uiManager.ShowUI(UI.s_GiftLimit, null, packageInfo);
            m_limitPackageList.RemoveAt(0);
            SavePopState(packageInfo.index,(int)packageInfo.expiredTime);
            m_IsPackageShowing = true;
        }

        public override void LateUpdate()
        {
            
        }

        public override void FixedUpdate()
        {

        }

        #endregion

        private void InsertLimitPackage(LimitTimePackage packageInfo)
        {
            m_limitPackageList.Insert(m_limitPackageList.Count,packageInfo);
        }

        public void SavePopState(long index,int expiredTime)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/" + index;
            PlayerPrefs.SetInt(key, expiredTime);
        }

        public void ClearPopState(long index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/" + index;
            PlayerPrefs.DeleteKey(key);
        }

        public bool isCanPop(long index)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            string key = playerProxy.CurrentRoleInfo.rid + "/" + index;
            int value = PlayerPrefs.GetInt(key);
            if (value <= 0) 
                return true;
            else if (value <= ServerTimeModule.Instance.GetServerTime())
            {
                ClearPopState(index);
                return true;
            }

            return false;
        }
    }
}