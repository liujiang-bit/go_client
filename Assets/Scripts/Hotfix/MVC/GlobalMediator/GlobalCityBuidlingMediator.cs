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
using System.Linq;
using ExcelDataReader.Log;

namespace Game
{
    public class GlobalCityBuidlingMediator : GameMediator
    {
        public static string NameMediator = "GlobalCityBuidlingMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private BuildingResourcesProxy m_buildingRssProxy;
        private CurrencyProxy m_currencyProxy;
        private PlayerProxy m_playerProxy;
        private ResearchProxy m_researchProxy;

        private Dictionary<long, Timer> m_timer = new Dictionary<long, Timer>();

        private Dictionary<long, HUDUI> m_buildBarHuds = new Dictionary<long, HUDUI>();//建造升级进度条
        private HUDUI m_serviceHud;//维修
        private HUDUI m_extinguishingHud;//灭火
        private bool loading = false;
        private bool loading2 = false;
        private HUDUI m_builderHutHud;//工人小屋hud
        private bool m_builderHutHudLoading;//工人小屋hud
        private long m_selectBuildID = 0;

        //IMediatorPlug needs
        public GlobalCityBuidlingMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalCityBuidlingMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Build_BuildingInfo.TagName,
                CmdConstant.CityBuildingDone,
                CmdConstant.buildQueueChange,
                CmdConstant.CityBuildingStart,
                CmdConstant.RemoveBuildHud,
                CmdConstant.CityFireStateChange,
                CmdConstant.BuildSelected,
                CmdConstant.BuildSelectReset,
                  CmdConstant.CityBuildingLevelUPStart,
                CmdConstant.CityBuildingLevelUPCancel,
                 CmdConstant.CityBuildingLevelUP,
                 CmdConstant.buildQueueChange,
                 CmdConstant.BuidingMenuOpen
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch (notification.Name)
            {
                case CmdConstant.CityBuildingDone:
                    {
                        UpdateBuildBar();
                        UpdateFireHud();
                        UpdateBuildHud();
                        BuildShelf();
                        break;
                    }
                case CmdConstant.BuidingMenuOpen:
                    {
                        long index = (long)notification.Body;
                        BuldingObjData buldingObjData = m_cityBuildingProxy.GetBuldingObjDataByIndex(index);

                        if (buldingObjData != null&& buldingObjData.type == EnumCityBuildingType.GuardTower)
                        {
                            UpdateBuildBar();
                        }
                    }

                    break;
                case CmdConstant.RemoveBuildHud:
                    {
                        foreach (var value in m_buildBarHuds.Values)
                        {
                            value.Close();
                        }
                    }
                    break;
                case CmdConstant.BuildSelected:
                    {
                        m_selectBuildID = (long)notification.Body;
                        UpdateFireHud();
                        UpdateBuildHud();
                    }
                    break;
                case CmdConstant.BuildSelectReset:
                    {
                        m_selectBuildID = 0;
                        UpdateFireHud();
                        UpdateBuildHud();
                    }
                    break;
                case CmdConstant.CityBuildingStart:
                    {
                        long index = (long)notification.Body;
                        BuildShelf(index); 
                        UpdateBuildBar();
                    }
                    break;
                case CmdConstant.CityBuildingLevelUPStart:
               case CmdConstant.CityBuildingLevelUPCancel:
                    {
                        long index = (long)notification.Body;
                        BuildShelf(index);
                        break;
                    }
                case CmdConstant.CityBuildingLevelUP:
                    {
                        long index = (long)notification.Body;
                        BuildShelf(index);
                        CreateTip(index);
                    }
                    break;
                case CmdConstant.buildQueueChange:
                    UpdateBuildBar();
                    UpdateBuildHud();
                    BuildShelf();
                    break;
                case CmdConstant.CityFireStateChange:
                    {
                        UpdateFireHud( );
                    }          
                    break;
                default:
                    break;
            }
        }
        private bool buildShelf1Loading = false;
        private bool buildShelf2Loading = false;
        /// <summary>
        /// 脚手架
        /// </summary>
        private void BuildShelf()
        {
            Dictionary<long, QueueInfo> Queues = m_playerProxy.CurrentRoleInfo.buildQueue;
            foreach (var element in Queues.Values)
            {
                BuildShelf(element.buildingIndex);
            }
        }
        private void BuildShelf(long buildingIndex)
        {
            BuldingObjData buldingObjData = m_cityBuildingProxy.GetBuldingObjDataByIndex(buildingIndex);

            if (buldingObjData == null)
            {
                return;
            }
            Transform go = buldingObjData.transform_shelf;
            if (go == null)
            {
                //  Debug.LogError("建造完成后建筑为空或建筑Index不正确："+element.buildingIndex);
                return;
            }
            if (m_cityBuildingProxy.IsUpgrading(buildingIndex))
            {
                if (go.transform.Find(RS.BuildScaffold[buldingObjData.buildingTypeConfigDefine.length - 1]) == null)
                {
                    ClientUtils.UIAddEffect(RS.BuildScaffold[buldingObjData.buildingTypeConfigDefine.length - 1], go, (temp) =>
                    {
                        if (m_cityBuildingProxy.IsUpgrading(buildingIndex))
                        {
                            temp.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            Animation[] animations = temp.GetComponentsInChildren<Animation>();
                            animations.ToList().ForEach((animation) =>
                            {
                                animation.enabled = true;
                            });
                        }
                        else
                        {
                            CoreUtils.assetService.Destroy(temp);
                        }
                    });
                    Timer.Register(2, () =>
                    {
                        if (go == null)
                        {
                            return;
                        }
                        Transform transform = go.transform.Find(RS.BuildScaffold[buldingObjData.buildingTypeConfigDefine.length - 1]);
                        if (transform != null)
                        {
                            Animation[] animations = transform.GetComponentsInChildren<Animation>();
                            animations.ToList().ForEach((animation) =>
                            {
                                animation.enabled = false;
                            });
                            Transform transform1 = transform.Find("build_3003");
                            if (transform1 != null)
                            {
                                transform1.gameObject.SetActive(false);
                            }
                        }

                    }, null, false, false);
                }
            }
            else
            {
                Transform transform = go.transform.Find(RS.BuildScaffold[buldingObjData.buildingTypeConfigDefine.length - 1]);
                if (transform != null)
                {
                    CoreUtils.assetService.Destroy(transform.gameObject);
                }
            }
        }
        /// <summary>
        /// "建造"HUd
        /// </summary>
        private void UpdateBuildHud()
        {
            if (m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.BuilderHut) !=null&& m_selectBuildID == m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.BuilderHut).buildingIndex)
            {
                ClosebuilderHutHud();
                return;
            }
            if (m_cityBuildingProxy.IsbuildQueueleisur())
            {
                if (m_builderHutHud == null || m_builderHutHud.uiObj == null)
                {
                    GameObject target = null;
                    if (m_cityBuildingProxy.MyCityObjData != null)
                    {
                        if (m_cityBuildingProxy.MyCityObjData.buildingListByType.ContainsKey(EnumCityBuildingType.BuilderHut))
                        {
                            Dictionary<long, BuldingObjData> builderHuts = m_cityBuildingProxy.MyCityObjData.buildingListByType[EnumCityBuildingType.BuilderHut];
                            if (builderHuts.Count > 0)
                            {
                                BuldingObjData buldingObjData = builderHuts.Values.ToList()[0];
                                target = buldingObjData.transform_tip.gameObject;
                            }
                        }
                    }
                    if (target == null)
                    {
                        return;
                    }
                    if (m_builderHutHudLoading)
                    {
                        return;
                    }
                    m_builderHutHudLoading = true;
                    m_builderHutHud = HUDUI.Register(UI_Pop_TextOnBuildingView.VIEW_NAME, typeof(UI_Pop_TextOnBuildingView), HUDLayer.city, target).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 100f)).SetInitCallback((ui) =>
                    {
                        m_builderHutHudLoading = false;
                        UI_Pop_TextOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_TextOnBuildingView>(ui.uiObj);
                        hudView.m_btn_click_GameButton.gameObject.SetActive(false);
                        hudView.m_lbl_languageText_LanguageText.text = LanguageUtils.getText(180520);
                    }).Show();
                }

            }
            else
            {
                ClosebuilderHutHud();
            }
        }
        private void ClosebuilderHutHud()
        {
            if (m_builderHutHud != null)
            {
                m_builderHutHud.Close();
                m_builderHutHud = null;
                m_builderHutHudLoading = false;
            }
        }

        /// <summary>
        /// 更新火焰hud
        /// </summary>
        private void UpdateFireHud(long _rid = 0)
        {
            long rid = _rid;
            if (rid == 0)
            {
                rid = m_playerProxy.CurrentRoleInfo.rid;
            }
            if (m_selectBuildID == m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.CityWall).buildingIndex)
            {
                CloseFireHud();
                return;
            }
            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(rid);
            bool FireIcon = false;
            if (cityObjData != null)
            {
                if (cityObjData.fireState == FireState.FIRED)
                {
                    string key = string.Format("{0}_{1}", m_playerProxy.CurrentRoleInfo.rid, "firestate");

                    string key_firestate = string.Format("{0}_{1}", "Firestate", (int)m_playerProxy.CurrentRoleInfo.rid);
                    string key_burnTime = string.Format("{0}_{1}", "BurnTime", (int)m_playerProxy.CurrentRoleInfo.rid);
                    int value_firestate = PlayerPrefs.GetInt(key_firestate);
                    int value_burnTime = PlayerPrefs.GetInt(key_burnTime);
                    List<BuldingObjData> allList = m_cityBuildingProxy.GetAllBuldingObjData();
                    if (m_serviceHud != null)
                    {
                        m_serviceHud.Close();
                     //   Debug.LogError("111");
                        m_serviceHud = null;
                        loading2 = false;
                    }
                    else
                    {
                      //  Debug.LogError("111");
                    }
                    FireIcon = true;
                    if (m_extinguishingHud == null || m_extinguishingHud.uiObj == null)
                    {
                        GameObject target = null;
                        if (cityObjData.buildingListByType.ContainsKey(EnumCityBuildingType.CityWall))
                        {
                            Dictionary<long, BuldingObjData> cityWalls = cityObjData.buildingListByType[EnumCityBuildingType.CityWall];
                            if (cityWalls.Count > 0)
                            {
                                BuldingObjData buldingObjData = cityWalls.Values.ToList()[0];
                                if (buldingObjData.boxColliders != null && buldingObjData.boxColliders.Length != 0)
                                {
                                    target = buldingObjData.transform_tip.gameObject;
                                }
                            }
                        }
                        if (target == null)
                        {
                            return;
                        }
                        if (loading)
                        {
                            return;
                        }
                        loading = true;
                        m_extinguishingHud = HUDUI.Register(UI_Pop_IconOnTheWallView.VIEW_NAME, typeof(UI_Pop_IconOnTheWallView), HUDLayer.city, target).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 40)).SetInitCallback((ui) =>
                        {
                            loading = false;
                            UI_Pop_IconOnTheWallView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnTheWallView>(ui.uiObj);
                            hudView.gameObject.name = "FireIcon";//TODO:
                            if (rid == m_playerProxy.CurrentRoleInfo.rid)
                            {
                                hudView.m_btn_click_GameButton.onClick.AddListener(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_theWall);
                                });
                            }
                            hudView.m_img_icon_PolygonImage.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1).SetLoops(-1,LoopType.Yoyo);
                        }).Show();
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (m_extinguishingHud != null)
                    {
                        m_extinguishingHud.Close();
                        m_extinguishingHud = null;
                        loading = false;
                    }
                }
                if (cityObjData.lostHp && !FireIcon)
                {
                    if (m_serviceHud == null || m_serviceHud.uiObj == null)
                    {
                        GameObject target = null;
                        if (cityObjData.buildingListByType.ContainsKey(EnumCityBuildingType.CityWall))
                        {
                            Dictionary<long, BuldingObjData> cityWalls = cityObjData.buildingListByType[EnumCityBuildingType.CityWall];
                            if (cityWalls.Count > 0)
                            {
                                BuldingObjData buldingObjData = cityWalls.Values.ToList()[0];
                                if (buldingObjData.boxColliders != null && buldingObjData.boxColliders.Length != 0)
                                {
                                    target = buldingObjData.boxColliders[0].gameObject;
                                }

                            }
                        }
                        if (target == null)
                        {
                            return;
                        }
                        if (loading2)
                        {
                            return;
                        }
                        loading2 = true;
                        m_serviceHud = HUDUI.Register(UI_Pop_TextOnBuildingView.VIEW_NAME, typeof(UI_Pop_TextOnBuildingView), HUDLayer.city, target).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 100f)).SetInitCallback((ui) =>
                        {
                            if (!cityObjData.lostHp)
                            {
                                if (m_serviceHud != null)
                                {
                                    m_serviceHud.Close();
                                    m_serviceHud = null;
                                    loading2 = false;
                                }
                            }
                            UI_Pop_TextOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_TextOnBuildingView>(ui.uiObj);
                            hudView.m_btn_click_GameButton.gameObject.SetActive(true);
                            hudView.gameObject.name = "Firetext";//TODO:
                            hudView.m_lbl_languageText_LanguageText.text = LanguageUtils.getText(181165);
                            if (rid == m_playerProxy.CurrentRoleInfo.rid)
                            {
                                hudView.m_btn_click_GameButton.onClick.AddListener(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_theWall);
                                });
                            }
                        }).Show();
                    }
                    else
                    {
                    }
                }
                if (!cityObjData.lostHp)
                {
                    CloseFireHud();
                }
            }
        }

        private void CloseFireHud()
        {
            if (m_serviceHud != null)
            {
                m_serviceHud.Close();
                m_serviceHud = null;
                loading2 = false;
            }
            if (m_extinguishingHud != null)
            {
                m_extinguishingHud.Close();
                m_extinguishingHud = null;
                loading = false;
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
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_researchProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        #endregion

        // 更新建造进度条
        private void UpdateBuildBar()
        {
            if (m_playerProxy.CurrentRoleInfo.buildQueue != null)
            {
                OnBuildBarHud(m_playerProxy.CurrentRoleInfo.buildQueue, m_buildBarHuds);
            }
        }

        private void CreateTip(long buildingIndex)
        {
            BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
            if (buildingInfoEntity != null)
            {
                BuildingTypeConfigDefine buildingTypeDefine = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>((int)(buildingInfoEntity.type));
                long level = buildingInfoEntity.level == 0 ? 1 : buildingInfoEntity.level;
                string str = LanguageUtils.getTextFormat(180502, level, LanguageUtils.getText(buildingTypeDefine.l_nameId));
                Tip.CreateTip(str).Show();
            }
        }
        private void OnBuildBarHud(Dictionary<long, QueueInfo> Queues, Dictionary<long, HUDUI> m_huds)
        {
            foreach (var element in Queues.Values)
            {
                //Debug.LogErrorFormat("OnBuildBarHud {0}:{1}", element.buildingIndex, element.finishTime);
                if (m_timer.ContainsKey(element.buildingIndex) && m_timer[element.buildingIndex] != null)
                {
                    m_timer[element.buildingIndex].Cancel();
                    m_timer[element.buildingIndex] = null;
                }
                GameObject go = CityObjData.GeBuildTipTargetGameObject(element.buildingIndex);
                if (go == null)
                {
                    // Debug.LogErrorFormat("建造完成后建筑为空或建筑Index不正确 {0}:{1}", element.buildingIndex, element.finishTime);
                    continue;
                }
                if (element.finishTime == -1 || element.finishTime == -2)
                {
                    HUDUI hUDUI = null;
                    if (m_huds.TryGetValue(element.buildingIndex, out hUDUI))
                    {
                        hUDUI.Close();
                        m_huds.Remove(element.buildingIndex);
                    }
                }
                else
                {
                    if (!m_huds.ContainsKey(element.buildingIndex))
                    {
                        m_huds[element.buildingIndex] = HUDUI.Register(UI_Tip_BuildingBarView.VIEW_NAME, typeof(UI_Tip_BuildingBarView), HUDLayer.city, go).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, -50f)).SetData(element).SetInitCallback((ui) =>
                        {
                            if (go != null && m_huds[element.buildingIndex] != null && m_huds[element.buildingIndex].uiObj != null)
                            {
                                OnBuildingBarDetail(m_huds[element.buildingIndex]);
                            }
                        }).Show();
                    }
                    else if (m_huds[element.buildingIndex] != null && m_huds[element.buildingIndex].uiObj != null)
                    {
                        m_huds[element.buildingIndex].SetTargetGameObject(go);
                        m_huds[element.buildingIndex].SetData(element);
                        OnBuildingBarDetail(m_huds[element.buildingIndex]);
                    }
                }
            }
        }
        private void RefreshhudView(UI_Tip_BuildingBarView hudView, long costTime, long finishTime, long buildingIndex)
        {
            long leftTime = finishTime - ServerTimeModule.Instance.GetServerTime();
            float pro = (float)(costTime - leftTime) / costTime;
            if (leftTime < 0)
            {
                hudView.m_pb_rogressBar_GameSlider.value = 1;
                hudView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(0);
                if (m_timer[buildingIndex] != null)
                {
                    m_timer[buildingIndex].Cancel();
                    m_timer[buildingIndex] = null;
                }
            }
            else
            {
                hudView.m_pb_rogressBar_GameSlider.value = pro;
                hudView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)leftTime);
            }
        }

        private void OnBuildingBarDetail(HUDUI hud)
        {
            UI_Tip_BuildingBarView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_BuildingBarView>(hud.uiObj);
            hudView.m_lbl_name_LanguageText.gameObject.SetActive(false);
            QueueInfo element = hud.data as QueueInfo;
            m_timer[element.buildingIndex] = null;
            long level = m_cityBuildingProxy.GetBuildingInfoByindex(element.buildingIndex).level;
            if (level == 0)
            {
                ClientUtils.LoadSprite(hudView.m_img_icon_PolygonImage, RS.MMU_building);
            }
            else
            {
                ClientUtils.LoadSprite(hudView.m_img_icon_PolygonImage, RS.MMU_levelup);
            }
            long CostTime = element.firstFinishTime - element.beginTime;
            RefreshhudView(hudView, CostTime,element.finishTime,element.buildingIndex);
            m_timer[element.buildingIndex] = Timer.Register(1.0f, () =>
            {
                if (hud.bDispose)
                {
                    Timer.Cancel(m_timer[element.buildingIndex]);
                    m_timer[element.buildingIndex] = null;
                    return;
                }
                RefreshhudView(hudView, CostTime, element.finishTime, element.buildingIndex);

            }, null, true, true, hudView.vb);
        }
    }
}

