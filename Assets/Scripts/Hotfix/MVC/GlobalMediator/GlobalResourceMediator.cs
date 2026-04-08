using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class GlobalResourceMediator : GameMediator
    {
        public static string NameMediator = "GlobalResourceMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private BuildingResourcesProxy m_buildingRssProxy;
        private CurrencyProxy m_currencyProxy;
        private HUDUI m_tavernTipHud;

        public Dictionary<GameObject, HUDUI> m_foodHuds = new Dictionary<GameObject, HUDUI>();
        public Dictionary<GameObject, HUDUI> m_woodHuds = new Dictionary<GameObject, HUDUI>();
        public Dictionary<GameObject, HUDUI> m_stoneHuds = new Dictionary<GameObject, HUDUI>();
        public Dictionary<GameObject, HUDUI> m_goldHuds = new Dictionary<GameObject, HUDUI>();
        private long m_selectBuildID = 0;
        //IMediatorPlug needs
        public GlobalResourceMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalResourceMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdateBuildingResourcesHud,
                CmdConstant.GuideForceShowResCollect,
                Role_RoleLogin.TagName,
                CmdConstant.UpdateBuildingTavernHud,
                CmdConstant.BuildSelected,
                CmdConstant.BuildSelectReset,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch(notification.Name)
            {
                case CmdConstant.UpdateBuildingResourcesHud:
                    UpdateResourcesBuilding(notification.Body);
                    break;
                case CmdConstant.GuideForceShowResCollect:
                    OnGuideForceShowResCollect(notification.Body);
                    break;
                case Role_RoleLogin.TagName:
                    BuildingResourcesProxy rssProxy = AppFacade.GetInstance().RetrieveProxy(BuildingResourcesProxy.ProxyNAME) as BuildingResourcesProxy;
                    rssProxy.OnUpdateRss();
                    break;
                case CmdConstant.BuildSelected:
                    {
                        m_selectBuildID = (long)notification.Body;
                        UpdateTavernSummonTip();
                    }
                    break;
                case CmdConstant.BuildSelectReset:
                    {
                        m_selectBuildID = 0;
                        UpdateTavernSummonTip();
                    }
                    break;
                case CmdConstant.UpdateBuildingTavernHud://统帅厅免费招募刷新
                    UpdateTavernSummonTip();
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

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_buildingRssProxy = AppFacade.GetInstance().RetrieveProxy(BuildingResourcesProxy.ProxyNAME) as BuildingResourcesProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void UpdateTavernSummonTip()
        {
            //酒馆数据
            CityBuildingProxy m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Tavern);
            if (info == null)
            {
                //Debug.LogError("没找到酒馆数据");
                return;
            }
            GameObject go = CityObjData.GeBuildTipTargetGameObject(info.buildingIndex);
            if (!go)
            {
                //Debug.LogError("建筑GameObject为空？");
                return;
            }

            bool isShowHudUI = false;
            bool isShowHudUI1 = !(info.buildingIndex == m_selectBuildID);
            long serverTime = ServerTimeModule.Instance.GetServerTime();
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy.CurrentRoleInfo.addGoldFreeAddTime < serverTime)
            {
                isShowHudUI = true;
            }

            if (playerProxy.CurrentRoleInfo.silverFreeCount > 0)
            {
                if (playerProxy.CurrentRoleInfo.openNextSilverTime < serverTime)
                {
                    isShowHudUI = true;
                }
            }
            if (isShowHudUI&& isShowHudUI1)
            {
                if (m_tavernTipHud == null)
                {
                    m_tavernTipHud = HUDUI.Register(UI_Pop_TextOnTavernView.VIEW_NAME, typeof(UI_Pop_TextOnTavernView), HUDLayer.city, go).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 100f)).SetInitCallback((ui) =>
                    {
                    }).Show();
                }
            }
            else
            {
                if (m_tavernTipHud != null)
                {
                    m_tavernTipHud.Close();
                    m_tavernTipHud = null;
                }
            }
        }

        private void UpdateResourcesBuilding(object body)
        {
            int type = (int)body;
            if ((type & 0x01) != 0)
            {
                OnRssHud(m_buildingRssProxy.FoodBuilding, m_buildingRssProxy.FoodRss, EnumCityBuildingType.Farm, m_foodHuds);
            }
            if ((type & 0x02) != 0)
            {
                OnRssHud(m_buildingRssProxy.WoodBuilding, m_buildingRssProxy.WoodRss, EnumCityBuildingType.Sawmill, m_woodHuds);
            }
            if ((type & 0x04) != 0)
            {
                OnRssHud(m_buildingRssProxy.StoneBuilding, m_buildingRssProxy.StoneRss, EnumCityBuildingType.Quarry, m_stoneHuds);
            }
            if ((type & 0x08) != 0)
            {
                OnRssHud(m_buildingRssProxy.GoldBuilding, m_buildingRssProxy.GoldRss, EnumCityBuildingType.SilverMine, m_goldHuds);
            }
        }


        //资源收集
        private void OnRssHud(List<BuildingInfoEntity> buildings, Dictionary<long, float> rssDic, EnumCityBuildingType type, Dictionary<GameObject, HUDUI> m_huds)
        {
            if (m_guideForceShowResCollectHud != null && !m_guideForceShowResCollectHud.bDispose)
            {
                return;
            }
            foreach (var element in buildings)
            {
                GameObject go = CityObjData.GeBuildTipTargetGameObject(element.buildingIndex);
                if (go ==null)
                {
                    continue;
                }
                if (!go)
                {
                    Debug.LogError("建筑GameObject为空？");
                }
                BuildingResourcesProduceDefine define = m_buildingRssProxy.BuildingRssDefine.Find((i) => { return i.type == element.type && i.level == element.level; });
                if (!rssDic.ContainsKey(element.buildingIndex) || define == null)
                {
                    continue;
                }
                int gatherMin = define.gatherMin;
                int gatherMax = define.gatherMax;
                if (rssDic[element.buildingIndex] >= gatherMin)
                {
                    if (m_huds.ContainsKey(go) && m_huds[go] != null)
                    {
                        if (!m_huds[go].assetLoadFinish)
                        {
                            continue;
                        }
                    }
                    if (!m_huds.ContainsKey(go) || m_huds[go] == null || m_huds[go].uiObj == null)
                    {
                        m_huds[go] = HUDUI.Register(UI_Pop_IconOnBuildingView.VIEW_NAME, typeof(UI_Pop_IconOnBuildingView), HUDLayer.city, go).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 25f)).SetData(element).SetInitCallback((ui) =>
                        {
                            UI_Pop_IconOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnBuildingView>(ui.uiObj);
                            //变色
                            bool fullRss = rssDic[element.buildingIndex] >= gatherMax;
                            if (hudView.m_img_bg0_PolygonImage.gameObject.activeSelf == fullRss)
                            {
                                hudView.m_img_bg0_PolygonImage.gameObject.SetActive(!fullRss);
                            }
                            if (hudView.m_img_bg1_PolygonImage.gameObject.activeSelf != fullRss)
                            {
                                hudView.m_img_bg1_PolygonImage.gameObject.SetActive(fullRss);
                            }
                            SetImgByBuildingType(hudView.m_img_icon_PolygonImage, type);
                            hudView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                            hudView.m_btn_click_GameButton.onClick.AddListener(() =>
                            {
                                OnHarvestRss(type);
                            });
                        }).Show();
                    }
                    else if (m_huds[go].uiObj != null && m_huds[go].uiObj.activeSelf)
                    {
                        UI_Pop_IconOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnBuildingView>(m_huds[go].uiObj);
                        //变色
                        bool fullRss = rssDic[element.buildingIndex] >= gatherMax;
                        if (hudView.m_img_bg0_PolygonImage.gameObject.activeSelf == fullRss)
                        {
                            hudView.m_img_bg0_PolygonImage.gameObject.SetActive(!fullRss);
                        }
                        if (hudView.m_img_bg1_PolygonImage.gameObject.activeSelf != fullRss)
                        {
                            hudView.m_img_bg1_PolygonImage.gameObject.SetActive(fullRss);
                        }
                    }
                }
                else if (m_huds.ContainsKey(go) && m_huds[go] != null)
                {
                    m_huds[go].Close();
                }
            }
        }
        public HUDUI m_guideForceShowResCollectHud;
        //新手引导中的资源收集
        private void OnGuideForceShowResCollect(object body)
        {
            BuildingInfoEntity info = body as BuildingInfoEntity;
            if (info == null)
            {
                Debug.LogError("GuideForceShowResCollect的参数错误");
                return;
            }

            GameObject building = CityObjData.GeBuildTipTargetGameObject(info.buildingIndex);
            if (!building)
            {
                Debug.LogError("不存在该建筑");
                return;
            }

            m_guideForceShowResCollectHud = HUDUI.Register(UI_Pop_IconOnBuildingView.VIEW_NAME, typeof(UI_Pop_IconOnBuildingView), HUDLayer.city, building).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 25f)).SetData(info).SetInitCallback((ui) =>
            {
                UI_Pop_IconOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnBuildingView>(ui.uiObj);
                SetImgByBuildingType(hudView.m_img_icon_PolygonImage, (EnumCityBuildingType)info.type);
                hudView.m_btn_click_GameButton.onClick.AddListener(() =>
                {
                    var req = new Build_GetBuildResources.request
                    {
                        buildingIndexs = new List<long>()
                    };
                   // CoreUtils.audioService.PlayOneShot(GetRssSound(GetCurrencyByBuildingType((EnumCityBuildingType)info.type)));
                    req.buildingIndexs.Add(info.buildingIndex);
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyUICurrency((int)GetCurrencyByBuildingType((EnumCityBuildingType)info.type), 99999, ui.uiObj.transform.position);
                    AppFacade.GetInstance().SendSproto(req);
                    m_guideForceShowResCollectHud.Close();
                    long num = m_buildingRssProxy.GetCurrencyNum(info);
                    if (num != 0)
                    {
                        m_buildingRssProxy.FoodRss[info.buildingIndex] = 0;
                    }
                });
            }).Show();
        }

        public void OnHarvestRss(EnumCityBuildingType type)
        {
            var req = new Build_GetBuildResources.request
            {
                buildingIndexs = new List<long>()
            };
            if (m_guideForceShowResCollectHud != null && !m_guideForceShowResCollectHud.bDispose)
            {
                BuildingInfoEntity info = m_guideForceShowResCollectHud.data as BuildingInfoEntity;
                if (info != null)
                {
                   // CoreUtils.audioService.PlayOneShot(GetRssSound(GetCurrencyByBuildingType((EnumCityBuildingType)info.type)));
                    req.buildingIndexs.Add(info.buildingIndex);
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyUICurrency((int)GetCurrencyByBuildingType((EnumCityBuildingType)info.type), 99999, m_guideForceShowResCollectHud.uiObj.transform.position);
                    AppFacade.GetInstance().SendSproto(req);
                    m_guideForceShowResCollectHud.Close();
                    long num = m_buildingRssProxy.GetCurrencyNum(info);
                    if (num != 0)
                    {
                        m_buildingRssProxy.FoodRss[info.buildingIndex] = 0;
                    }
                }
                return;
            }
          //  CoreUtils.audioService.PlayOneShot(GetRssSound(GetCurrencyByBuildingType(type)));
            foreach (var item in GetHudUIByType(type))
            {
                if (item.Value != null && item.Value.uiObj != null && item.Value.uiObj.activeSelf)
                {
                    var tmpEntity = item.Value.data as BuildingInfoEntity;
                    if (tmpEntity != null)
                    {
                        req.buildingIndexs.Add(tmpEntity.buildingIndex);
                        long num = m_buildingRssProxy.GetCurrencyNum(tmpEntity);
                        OnHarvestHud(item.Key, num);
                        GetRssDicByType(type)[tmpEntity.buildingIndex] = 0;
                        m_buildingRssProxy.IsCollecting = true;
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyUICurrency((int)GetCurrencyByBuildingType(type), num, item.Value.uiObj.transform.position,()=>
                        {
                            m_buildingRssProxy.IsCollecting = false;
                        });
                        item.Value.Close();
                    }
                }
            }
            m_buildingRssProxy.Harvest(type);
            AppFacade.GetInstance().SendSproto(req);
        }

        private Dictionary<GameObject, HUDUI> GetHudUIByType(EnumCityBuildingType type)
        {
            switch (type)
            {
                case EnumCityBuildingType.Farm: return m_foodHuds;
                case EnumCityBuildingType.Sawmill: return m_woodHuds;
                case EnumCityBuildingType.Quarry: return m_stoneHuds;
                case EnumCityBuildingType.SilverMine: return m_goldHuds;
                default: return m_foodHuds;
            }
        }

        public Dictionary<long, float> GetRssDicByType(EnumCityBuildingType type)
        {
            switch (type)
            {
                case EnumCityBuildingType.Farm: return m_buildingRssProxy.FoodRss;
                case EnumCityBuildingType.Sawmill: return m_buildingRssProxy.WoodRss;
                case EnumCityBuildingType.Quarry: return m_buildingRssProxy.StoneRss;
                case EnumCityBuildingType.SilverMine: return m_buildingRssProxy.GoldRss;
                default: return m_buildingRssProxy.FoodRss;
            }
        }

        private void OnHarvestHud(GameObject go, long num)
        {
            HUDUI.Register("UI_Hud_HarvestNum", typeof(UI_Hud_HarvestNumView), HUDLayer.city, go).SetInitCallback((ui) =>
            {
                UI_Hud_HarvestNumView itemView = MonoHelper.AddHotFixViewComponent<UI_Hud_HarvestNumView>(ui.uiObj);
                itemView.m_lbl_languageText_LanguageText.text = num.ToString("N0");
                itemView.m_lbl_languageText_LanguageText.rectTransform.DOLocalMoveY(90, 1).OnComplete(() =>
                {
                    ui.Close();
                });
            }).Show();
        }

        private EnumCurrencyType GetCurrencyByBuildingType(EnumCityBuildingType type)
        {
            switch (type)
            {
                case EnumCityBuildingType.Farm: return EnumCurrencyType.food;
                case EnumCityBuildingType.Sawmill: return EnumCurrencyType.wood;
                case EnumCityBuildingType.Quarry: return EnumCurrencyType.stone;
                case EnumCityBuildingType.SilverMine: return EnumCurrencyType.gold;
                default: return EnumCurrencyType.food;
            }
        }

        private void SetImgByBuildingType(PolygonImage img, EnumCityBuildingType type)
        {
            EnumCurrencyType cType;
            switch (type)
            {
                case EnumCityBuildingType.Farm: cType = EnumCurrencyType.food; break;
                case EnumCityBuildingType.Sawmill: cType = EnumCurrencyType.wood; break;
                case EnumCityBuildingType.Quarry: cType = EnumCurrencyType.stone; break;
                case EnumCityBuildingType.SilverMine: cType = EnumCurrencyType.gold; break;
                default: cType = EnumCurrencyType.food; break;
            }
            ClientUtils.LoadSprite(img, m_currencyProxy.GeticonIdByType(cType));
        }
    }
}

