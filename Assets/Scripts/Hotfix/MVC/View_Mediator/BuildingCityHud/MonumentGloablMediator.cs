using System;
using System.Collections.Generic;
using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
 
    public class MonumentGloablMediator : GameMediator
    {
        #region Member
        
        
        public static string NameMediator = "MonumentGloablMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private bool monumentCanReward;
        private HUDUI monumentHUDUI;

        private long m_selectBuildID = 0;
        #endregion

        public MonumentGloablMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public MonumentGloablMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Monument_RewardNodify.TagName,
                Role_GetMonument.TagName,
                CmdConstant.CityBuildingDone,   
                CmdConstant.BuildSelected,
                CmdConstant.BuildSelectReset,
                 Role_RoleLogin.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_RoleLogin.TagName:
                    {
                        ChechMonumenFlag();
                    }
            break;
                case CmdConstant.CityBuildingDone:
                    CheckCreateIconOnMonumentHUD();
                    break;
                case Monument_RewardNodify.TagName:
                    Monument_RewardNodify.request req = notification.Body as Monument_RewardNodify.request;
                    if (req != null)
                    {
                        QueryMonumentRewardNotifyRes(req);
                    }
                    
                    break;
                case Role_GetMonument.TagName:
                    Role_GetMonument.response res1 = notification.Body as Role_GetMonument.response;
                    QueryGetMonumentEndRes(res1);
                    break;
                case CmdConstant.BuildSelected:
                    {
                        m_selectBuildID = (long)notification.Body;
                        CheckCreateIconOnMonumentHUD();
                    }
                    break;
                case CmdConstant.BuildSelectReset:
                    {
                        m_selectBuildID = 0;
                        CheckCreateIconOnMonumentHUD();
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_cityBuildingProxy =   AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy =   AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        public override void OnRemove()
        {

        }

        public override void Update()
        {

        }

        public override void LateUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }
        #endregion

        void QueryMonumentRewardNotifyRes(Monument_RewardNodify.request req)
        {
            if (req.HasCanReward)
            {
                monumentCanReward = req.canReward;
            }

            CheckCreateIconOnMonumentHUD();
        }
        private void ChechMonumenFlag()
        {
            DateTime now = ServerTimeModule.Instance.GetCurrServerDateTime();
            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            string kry = m_playerProxy.Rid.ToString() + "MonumenTime";
            string strLast = PlayerPrefs.GetString("kry", "0");
            if (strLast == "0")
            {
                PlayerPrefs.SetString("kry", now.ToString(culture));
                PlayerPrefs.Save();
                PlayerProxy.IsMonumentflag = true;
                return;
            }
            DateTime last = DateTime.Parse(strLast, culture);
            // 当日登陆
            if (now.Year == last.Year && now.DayOfYear == last.DayOfYear)
            {
            }
            // 次日登陆
            else
            {
                PlayerProxy.IsMonumentflag = true;
            }
            PlayerPrefs.SetString("kry", now.ToString(culture));
            PlayerPrefs.Save();
        }

        private void QueryGetMonumentEndRes(Role_GetMonument.response res)
        {
            monumentCanReward = false;
            if (res != null)
            {
                foreach (var list in res.monumentList)
                {
                    var value = list.Value;
                    if (value.HasCanReward && value.HasReward)
                    {
                        if (value.canReward && !value.reward)    // 可领取，且没有领取
                        {
                            monumentCanReward = true;
                            break;
                        }
                    }
                }
            }

            CheckCreateIconOnMonumentHUD();
        }

        void CheckCreateIconOnMonumentHUD()
        {
            if (monumentHUDUI == null)
            {
                BuldingObjData buildingObjData = m_cityBuildingProxy.GetBuldingObjDataByType(EnumCityBuildingType.Monument);
                if (buildingObjData != null && buildingObjData.gameObject != null)
                {
                    monumentHUDUI = HUDUI.Register(UI_Pop_IconOnMonumentView.VIEW_NAME, typeof(UI_Pop_IconOnMonumentView), HUDLayer.city, buildingObjData.gameObject);
                    monumentHUDUI.SetScaleAutoAnchor(true);
                    monumentHUDUI.SetCameraLodDist(0, 270f);
                    monumentHUDUI.SetInitCallback(OnMonumentHUDUICreate);
                    ClientUtils.hudManager.ShowHud(monumentHUDUI);
                }
            }
            else
            {
                RefreshIconOnMonument();
            }
        }
       

        void OnMonumentHUDUICreate(HUDUI hud)
        {
            RefreshIconOnMonument();
        }

        void RefreshIconOnMonument()
        {
            if (monumentHUDUI.targetObj == null || monumentHUDUI.uiObj == null)
            {
                return;
            }
            
            UI_Pop_IconOnMonumentView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnMonumentView>(monumentHUDUI.uiObj);
            if (m_selectBuildID == m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Monument).buildingIndex)
            {
                view.m_pl_offset.gameObject.SetActive(false);
                return;
            }
            view.m_pl_offset.gameObject.SetActive(monumentCanReward);
            if (PlayerProxy.IsMonumentflag)
            {
                view.m_pl_offset.gameObject.SetActive(true);
            }
            view.m_btn_click_GameButton.onClick.RemoveAllListeners();
            view.m_btn_click_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_monument);
            });
        }
        
    }
}