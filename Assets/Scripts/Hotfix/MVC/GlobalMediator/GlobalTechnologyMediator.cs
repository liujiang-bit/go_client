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
    public class GlobalTechnologyMediator : GameMediator
    {
        public static string NameMediator = "GlobalTechnologyMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private ResearchProxy m_researchProxy;

        private Dictionary<GameObject, HUDUI> m_technologyBarHuds = new Dictionary<GameObject, HUDUI>();//研究进度条
        private Dictionary<long, Timer> m_timer = new Dictionary<long, Timer>();

        private HUDUI m_techHarvestHud;//点击研究的进度条
        private bool m_bLoadTechEffect;
        private GameObject m_techEffect; //正在研究的建筑效果
        private HUDUI m_techIdleHud;//科技空闲状态标志
        private long m_selectBuildID = 0;
        //IMediatorPlug needs
        public GlobalTechnologyMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalTechnologyMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.CityBuildingDone,
                CmdConstant.technologyQueueChange,
                 CmdConstant.AwardTechnology,
                 CmdConstant.CityBuildingLevelUP,
                 Technology_ResearchTechnology.TagName,
                 Technology_AwardTechnology.TagName,
                 CmdConstant.OnCityLoadFinished,
                 CmdConstant.buildQueueChange,
                              CmdConstant.BuildSelected,
                CmdConstant.BuildSelectReset,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch (notification.Name)
            {
                case CmdConstant.CityBuildingDone:
                    {
                        UpdateTechnologyBar();
                        OnTechnologyHarvestHud();
                        OnTechnologyIdle();
                        break;
                    }
                case CmdConstant.CityBuildingLevelUP:
                case CmdConstant.buildQueueChange:
                    {
                        OnTechnologyIdle();
                    }
                    break;
                case CmdConstant.AwardTechnology:
                    {
                        long technologyType = (long)notification.Body;
                        var data = m_researchProxy.GetTechnologyByLevel((int)technologyType, 1);
                        if (data != null)
                        {
                            SendEndTechnology(data);
                        }
                    }
                    break;
                case CmdConstant.technologyQueueChange:
                    UpdateTechnologyBar();
                    OnTechnologyHarvestHud();
                    OnTechnologyIdle();
                    break;
                case CmdConstant.OnCityLoadFinished:
                case SprotoType.Technology_ResearchTechnology.TagName:
                case SprotoType.Technology_AwardTechnology.TagName:
                    OnTechnologyIdle();
                    break;
                case CmdConstant.BuildSelected:
                    {
                        m_selectBuildID = (long)notification.Body;
                        OnTechnologyIdle();
                    }
                    break;
                case CmdConstant.BuildSelectReset:
                    {
                        m_selectBuildID = 0;
                        OnTechnologyIdle();
                    }
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

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
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
        //科技空闲状态
        private void OnTechnologyIdle()
        {
            BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Academy));

            if (buildingInfoEntity == null)
            {
                return;
            }
            GameObject go = CityObjData.GeBuildTipTargetGameObject(buildingInfoEntity.buildingIndex);
            if (go == null)
            {
                return;
            }

            bool showHud = !(m_selectBuildID == buildingInfoEntity.buildingIndex);//学院没有在选中
            bool showHud1 = true;//学院没有在研究
            bool showHud2 = true;//学院没有在升级
            bool showHud3 = false;//有下一级的可升级的科技


            if (showHud)
            {
                showHud1 = m_playerProxy.CurrentRoleInfo.technologyQueue == null || m_playerProxy.CurrentRoleInfo.technologyQueue.finishTime <= 0;
                if (m_techHarvestHud != null && !m_techHarvestHud.bDispose)
                {
                    showHud1 = false;
                }
            }
  
            if (showHud&&showHud1)
            {
                if (m_playerProxy.CurrentRoleInfo.buildQueue != null)
                {
                    foreach (var item in m_playerProxy.CurrentRoleInfo.buildQueue)
                    {
                        if (item.Value.buildingIndex == buildingInfoEntity.buildingIndex)
                        {
                            showHud2 = false;
                        }
                    }
                }
            }

            if (showHud && showHud1&&showHud2)
            {
                List<StudyDefine> list = m_researchProxy.Technologys();
                int unLoukCount = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (!m_playerProxy.IsTechnologyUnlock(list[i].ID))
                    {
                        if (buildingInfoEntity.level < list[i].campusLv)
                        {
                            continue;
                        }
                        var pres = m_researchProxy.CheckPreAllTechnology(list[i]);
                        if (pres == null || pres.Count == 0)
                        {
                            showHud3 = true;
                            break;
                        }
                    }
                }
            }
            if (showHud&& showHud1 && showHud2 && showHud3)
            {
                if (m_techIdleHud == null || m_techIdleHud.bDispose)
                {
                    m_techIdleHud = HUDUI.Register(UI_Pop_TextOnBuildingView.VIEW_NAME, typeof(UIPopTrainOnBuildingView), HUDLayer.city, go).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 100)).SetInitCallback((ui) =>
                    {
                        UI_Pop_TextOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_TextOnBuildingView>(ui.uiObj);
                        hudView.m_lbl_languageText_LanguageText.text = LanguageUtils.getText(402015);
                    }).Show();
                }
            }
            else if (m_techIdleHud != null && !m_techIdleHud.bDispose)
            {
                m_techIdleHud.Close();
            }
        }


        // 更新研究进度条
        private void UpdateTechnologyBar()
        {
            if (m_playerProxy.CurrentRoleInfo.technologyQueue != null)
            {
                OnTechnologyBarHud(m_playerProxy.CurrentRoleInfo.technologyQueue, m_technologyBarHuds);
            }
        }

        private void OnTechnologyBarHud(QueueInfo Queue, Dictionary<GameObject, HUDUI> m_huds)
        {
            BuildingInfoEntity BuildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Academy));

            if (BuildingInfoEntity == null)
            {
                return;
            }
            GameObject go = CityObjData.GeBuildTipTargetGameObject(BuildingInfoEntity.buildingIndex);
            if (go == null)
            {
                return;
            }
            Debug.LogFormat("{0}:{1}", (EnumCityBuildingType)BuildingInfoEntity.type, BuildingInfoEntity.finishTime);
            long leftTime = Queue.finishTime - ServerTimeModule.Instance.GetServerTime();
            if (Queue.finishTime < 0 || leftTime <= 0)
            {
                if (m_huds.ContainsKey(go) && m_huds[go] != null)
                {
                    m_huds[go].Close();
                }
                if (m_timer.ContainsKey(Queue.buildingIndex) && m_timer[Queue.buildingIndex] != null)
                {
                    m_timer[Queue.buildingIndex].Cancel();
                    m_timer[Queue.buildingIndex] = null;
                }
                if (m_techEffect != null)
                {
                    GameObject.DestroyImmediate(m_techEffect);
                    m_techEffect = null;
                }
            }
            else if (leftTime > 0)
            {
                if (!m_huds.ContainsKey(go) || m_huds[go] == null || m_huds[go].uiObj == null)
                {
                    m_huds[go] = HUDUI.Register(UI_Tip_BuildingBarView.VIEW_NAME, typeof(UI_Tip_BuildingBarView), HUDLayer.city, go).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, -50f)).SetData(Queue).SetInitCallback((ui) =>
                    {
                        OnTechnologyHudDetail(m_huds, go);
                    }).Show();
                }
                else if (!m_huds[go].bDispose)
                {
                    m_huds[go].SetData(Queue);
                }
                if (m_techEffect == null && !m_bLoadTechEffect)
                {
                    m_bLoadTechEffect = true;
                    CoreUtils.assetService.Instantiate("build_3001", (tmpEffect) =>
                    {
                        m_bLoadTechEffect = false;
                        m_techEffect = tmpEffect;
                        m_techEffect.transform.SetParent(go.transform);
                        m_techEffect.transform.localPosition = Vector3.zero;
                    });
                }
            }
        }

        private void OnTechnologyHudDetail(Dictionary<GameObject, HUDUI> m_huds, GameObject go)
        {
            var outsideQueue = m_huds[go].data as QueueInfo;
            var define = m_researchProxy.GetTechnologyByLevel((int)outsideQueue.technologyType, 1);
            UI_Tip_BuildingBarView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_BuildingBarView>(m_huds[go].uiObj);

            var isSoldierStudy1 = m_researchProxy.IsSoldierRes(define.ID);
            var spImg = isSoldierStudy1 != null ? isSoldierStudy1.icon : define.icon;

            ClientUtils.LoadSprite(hudView.m_img_icon_PolygonImage, spImg);//等待图片id
            hudView.m_lbl_name_LanguageText.text = "和倒计时交替显示";
            hudView.m_lbl_name_LanguageText.gameObject.SetActive(false);
            Timer timer = null;
            m_timer[outsideQueue.buildingIndex] = timer;

            long outsideCurrentTime = ServerTimeModule.Instance.GetServerTime();
            long outsideLeftTime = outsideQueue.finishTime - outsideCurrentTime;
            float outsideProgress = (float)(outsideCurrentTime - outsideQueue.beginTime) / (outsideCurrentTime - outsideQueue.finishTime);
            hudView.m_pb_rogressBar_GameSlider.value = outsideProgress;
            hudView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)outsideLeftTime);
            timer = Timer.Register(1.0f, () =>
            {
                QueueInfo insideQueue = m_huds[go].data as QueueInfo;
                long insideCurrentTime = ServerTimeModule.Instance.GetServerTime();
                long insideLeftTime = insideQueue.finishTime - insideCurrentTime;
                long CostTime2 = insideQueue.firstFinishTime - insideQueue.beginTime;
                if (insideLeftTime <= 0)
                {
                    OnTechnologyHarvestHud();
                    Timer.Cancel(timer);
                    timer = null;
                    m_huds[go].Close();
                }
                else if (m_huds[go].uiObj)
                {
                    hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_BuildingBarView>(m_huds[go].uiObj);
                    float pro2 = (insideCurrentTime - insideQueue.beginTime + insideQueue.firstFinishTime - insideQueue.finishTime) / (float)(CostTime2);
                    hudView.m_pb_rogressBar_GameSlider.value = pro2;
                    //间隔500毫秒显示名称或者倒计时
                    if ((insideLeftTime / 5) % 2 == 0)
                    {
                        hudView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)insideLeftTime);
                    }
                    else
                    {
                        var insideDefine = m_researchProxy.GetTechnologyByLevel((int)insideQueue.technologyType, 1);
                        var isSoldierStudy = m_researchProxy.IsSoldierRes(insideDefine.ID);
                        var spLanID = isSoldierStudy != null ? isSoldierStudy.l_studyNameID : insideDefine.l_nameID;
                        hudView.m_lbl_time_LanguageText.text = LanguageUtils.getText(spLanID);
                    }
                }

            }, null, true, true);
        }

        private void OnTechnologyHarvestHud()
        {
            if (m_techHarvestHud != null)
            {
                m_techHarvestHud.Close();
            }
            BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Academy));
            if (buildingInfoEntity == null)
            {
                return;
            }
            GameObject go = CityObjData.GeBuildTipTargetGameObject(buildingInfoEntity.buildingIndex);
            if (go == null)
            {
              //  Debug.LogError("学院的建筑不存在？");
                return;
            }
            QueueInfo info = m_researchProxy.GetCrrTechnologying();
            if (info != null && info.HasTechnologyType && info.technologyType > 0)
            {
                if (info.finishTime < 0)
                {
                    var data = m_researchProxy.GetTechnologyByLevel((int)info.technologyType, 1);
                    if (data != null)
                    {
                        //研究结束
                        Tip.CreateTip(402001, LanguageUtils.getText(data.l_nameID), m_researchProxy.GetCrrTechnologyLv((int)info.technologyType) + 1).Show();

                        //                        CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
                        CoreUtils.uiManager.CloseUI(UI.s_ResearchMain);
                    }

                    m_techHarvestHud = HUDUI.Register(UI_Pop_IconOnResearchView.VIEW_NAME, typeof(UI_Pop_IconOnResearchView), HUDLayer.city, go).SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 50f)).SetInitCallback((ui) =>
                    {
                        UI_Pop_IconOnResearchView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnResearchView>(ui.uiObj);
                        ClientUtils.LoadSprite(hudView.m_img_icon_PolygonImage, data.icon);
                        hudView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                        hudView.m_btn_click_GameButton.onClick.AddListener(() =>
                        {
                            SendEndTechnology(data);
                        });
                    }).Show();
                    return;
                }
            }
        }

        public void SendEndTechnology(StudyDefine data)
        {
            UI_Pop_IconOnResearchView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnResearchView>(m_techHarvestHud.uiObj);
            CoreUtils.assetService.Instantiate("UE_ResFly_research", (effectGO) =>
            {
                UE_ResFly_researchView itemView = MonoHelper.GetOrAddHotFixViewComponent<UE_ResFly_researchView>(effectGO);
                //飘飞特效
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                CoreUtils.audioService.PlayOneShot("Sound_Ui_GetSthButton");
                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, data.icon, false, () =>
                {
                    if (effectGO == null)
                    {
                        return;
                    }
                    itemView.m_img_icon_PolygonImage.rectTransform.anchoredPosition = new Vector2(0, 0);
                    itemView.m_img_rare_PolygonImage.rectTransform.anchoredPosition = new Vector2(0, 0);
                    mt.FlyPowerUpEffect(effectGO, hudView.m_img_icon_PolygonImage.rectTransform, Vector3.one, null, 1f);
                    GameObject.DestroyImmediate(effectGO);
                    //领取科技
                    m_researchProxy.SendEndTechnology();
                    Tip.CreateTip(402002, LanguageUtils.getText(data.l_nameID), m_researchProxy.GetCrrTechnologyLv((int)data.studyType) + 1).Show();
                });
            });

        }
    }
}

