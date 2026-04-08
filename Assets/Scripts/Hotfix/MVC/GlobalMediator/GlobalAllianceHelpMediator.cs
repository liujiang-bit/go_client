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
using System;
namespace Game
{
    public class AllianceHelp
    {
        public GameObject go;
        public long type;
        public long queueIndex;
        public HUDUI hudui;
        public bool bDispose;

        public AllianceHelp(GameObject go,Action<HUDUI> callback)
        {
            this.bDispose = false;
            this.go = go;
            this.hudui = HUDUI.Register(UI_Pop_IconOnHelpView.VIEW_NAME,typeof(UI_Pop_IconOnHelpView),HUDLayer.city,go).SetCameraLodDist(0,270f).SetScaleAutoAnchor(true).SetInitCallback(callback).Show();
        }

        public void Dispose()
        {
            bDispose = true;
            hudui.Close();
        }
    }

    public class GlobalAllianceHelpMediator : GameMediator
    {
        public static string NameMediator = "GlobalAllianceHelpMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private AllianceProxy m_allianceProxy;
        private FuncGuideProxy m_funcGuideProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private GlobalViewLevelMediator m_globalViewLevelMediator;

        public Dictionary<long, AllianceHelp> buildingQueue = new Dictionary<long, AllianceHelp>();
        public Dictionary<long, AllianceHelp> treatmentQueue = new Dictionary<long, AllianceHelp>();

        //public Dictionary<long, AllianceHelp> armyQueue = new Dictionary<long, AllianceHelp>();

        public AllianceHelp techQueue;

        public AllianceHelp allianceHelp;

        public const string AskForHelp = "ui_common[icon_com_help3]";

        public const string HelpOther = "ui_common[icon_com_help2]";

        private int m_allianceRecommendedLimit;

        private HUDUI m_GuildCityWarnTip;

        //IMediatorPlug needs
        public GlobalAllianceHelpMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalAllianceHelpMediator(object viewComponent) : base(NameMediator, null) { }


        private RallyTroopsProxy m_rallyTroopsProxy;

        private Timer m_recommendTimer;

        private Timer m_redPointCheck;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.CityBuildingDone,
                CmdConstant.CityBuildingStart,
                CmdConstant.buildQueueChange,
                CmdConstant.technologyQueueChange,
                CmdConstant.treatmentChange,
                CmdConstant.AllianceEixt,
                Guild_ApplyJoinGuild.TagName,
                Guild_CreateGuild.TagName,
                Guild_GuildNotify.TagName,
                Guild_HelpGuildMembers.TagName,        
                Guild_SearchGuild.TagName,
                CmdConstant.AllianceHelp,
                Guild_HelpGuildMembers.TagName,
                CmdConstant.HospitalCanGetReward,
                CmdConstant.AllianceJoinUpdate,
                CmdConstant.CityBuildinginfoFirst,
                CmdConstant.MapObjectChange_ObjectInfoReq,
                CmdConstant.RallyTroopChange,
                CmdConstant.ChangeGuildCityHud,
                CmdConstant.BuidingMenuOpen
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch(notification.Name)
            {
                case CmdConstant.AllianceJoinUpdate:
                case CmdConstant.CityBuildinginfoFirst:
                    CheckStartRedPoint();
                    break;
                case CmdConstant.CityBuildingDone:
                case CmdConstant.CityBuildingStart:
                case CmdConstant.buildQueueChange:
                    OnBuildingHelp();
                    OnGuildNotify(m_allianceProxy.getHelps());
                    CheckGuildCityWarTip();
                    break;
                case CmdConstant.technologyQueueChange:
                    OnTechHelp();
                    break;
                case CmdConstant.HospitalCanGetReward:
                case CmdConstant.treatmentChange:
                    OnTreatmentHelp();
                    break;
                case Guild_CreateGuild.TagName:
                case Guild_ApplyJoinGuild.TagName:
//                    CoreUtils.uiManager.CloseUI(UI.s_AllianceInviteTip);
                    OnBuildingHelp();
                    OnTechHelp();
                    OnTreatmentHelp();
                    break;
                case CmdConstant.AllianceHelp:
                    OnGuildNotify(m_allianceProxy.getHelps());
                    break;
                case Guild_HelpGuildMembers.TagName:
                    OnCloseGuildHelp();
                    break;
                case CmdConstant.AllianceEixt:
                    PlayerPrefs.SetInt(string.Format($"{m_playerProxy.Rid}_GuildWelcome"), 0);
                    OnBuildingHelp();
                    OnTechHelp();
                    OnTreatmentHelp();
                    OnCloseGuildHelp();

                    CheckStopRedPoint();
                    CleanHolyLandInfos();
                    break;
                case CmdConstant.BuidingMenuOpen:
                    OnBuildingHelp();
                    break;
                case Guild_SearchGuild.TagName:

                    Guild_SearchGuild.response response = notification.Body as Guild_SearchGuild.response;

                    if (response.type == 1 && response.HasGuildList && response.guildList.Count>0)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceInviteTip,null,response.guildList[0]);
                    }
                    break;
                case CmdConstant.MapObjectChange_ObjectInfoReq:
                    MapObjectInfo mapObjectInfo = notification.Body as MapObjectInfo;
                    if (mapObjectInfo != null && m_allianceProxy != null)
                    {
                        MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(mapObjectInfo.objectId);
                        RssType rssType = (RssType)mapObjectInfoEntity.objectType;
                        if (rssType == RssType.CheckPoint || rssType == RssType.HolyLand)
                        {
                            
                            if (mapObjectInfo.HasGuildId && m_allianceProxy.HasJionAlliance() && m_allianceProxy.GetAlliance() != null)
                            {
                                if (mapObjectInfo.guildId == m_allianceProxy.GetAlliance().guildId)
                                {
                                    OnOccupyHolyLandPlayEffect(mapObjectInfoEntity, rssType);    
                                }
                            }
                        }
                        
                    }
                    
                    break;
                
                case CmdConstant.RallyTroopChange:
                case CmdConstant.ChangeGuildCityHud:
                    CheckGuildCityWarTip();
                    break;
                default:break;
            }
        }

        public RallyTroopsProxy MRallyTroopsProxy
        {
            get
            {
                if (m_rallyTroopsProxy==null)
                {
                    m_rallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
                }
                return m_rallyTroopsProxy;
            }
        }


        private void CheckGuildCityWarTip()
        {
            if (MRallyTroopsProxy!=null)
            {
                if (MRallyTroopsProxy.ShowGuildCityHudState())
                {
                    if (m_GuildCityWarnTip==null)
                    {
                        var buildingInfo = m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Castel);
                        if (buildingInfo == null)
                        {
                            return;
                        }
                        GameObject obj = CityObjData.GeBuildTipTargetGameObject(buildingInfo.buildingIndex);
                        if (obj!=null)
                        {
                            HUDUI curHud = HUDUI.Register(UI_Pop_IconOnWarView.VIEW_NAME, typeof(UI_Pop_IconOnWarView), HUDLayer.city, obj);
                            curHud.SetScaleAutoAnchor(true);
                            curHud.SetCameraLodDist(0, 270f);
                            curHud.SetInitCallback(onCreateGuildCityWarTip);
                            ClientUtils.hudManager.ShowHud(curHud);
                            m_GuildCityWarnTip = curHud;
                        }
                    }
                }
                else
                {
                    if (m_GuildCityWarnTip!=null)
                    {
                        m_GuildCityWarnTip.Close();
                        m_GuildCityWarnTip = null;
                    }
                }
            }
        }

        private void onCreateGuildCityWarTip(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null)
            {
                Debug.LogWarning("节点被干掉了");
                return;
            }
            
            UI_Pop_IconOnWarView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnWarView>(hud.uiObj);
            hudView.m_btn_click_GameButton.onClick.AddListener(() => {
                m_rallyTroopsProxy.ClearGuildCityHud();
                // m_rallyTroopsProxy.SetReadedRallyRedPoint();
                
                CoreUtils.uiManager.ShowUI(UI.s_AlianceWar);
            });
           
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

        public override void OnRemove()
        {
            if (m_recommendTimer != null)
            {
                m_recommendTimer.Cancel();
                m_recommendTimer = null;
            }

            if (m_redPointCheck!=null)
            {
                m_redPointCheck.Cancel();
                m_redPointCheck = null;
            }
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
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_funcGuideProxy = AppFacade.GetInstance().RetrieveProxy(FuncGuideProxy.ProxyNAME) as FuncGuideProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_globalViewLevelMediator = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;

            m_allianceRecommendedLimit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).allianceRecommendedLimit;

            if (m_recommendTimer == null)
            {
                m_recommendTimer = Timer.Register(m_allianceProxy.Config.allianceRecommendedTime,onTick,null,true,true);
            }
        }


        private void CheckStartRedPoint()
        {
            if (m_playerProxy!=null && m_playerProxy.CurrentRoleInfo.guildId>0 && m_playerProxy.CurrentRoleInfo.lastGuildDonateTime>0 && m_recommendTimer==null)
            {
                long remianTime=m_allianceProxy.Config.AllianceStudyGiftCD-((ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)%m_allianceProxy.Config.AllianceStudyGiftCD);
                m_redPointCheck = Timer.Register(remianTime,onRedPointCheck,null,true,true);
                onRedPointCheck();
            }
        }

        private void CheckStopRedPoint()
        {
            if (m_playerProxy.CurrentRoleInfo.guildId==0)
            {
                if (m_redPointCheck!=null)
                {
                    m_redPointCheck.Cancel();
                    m_redPointCheck = null;
                }
            }   
        }


        private void onRedPointCheck()
        {
            
            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceStudyDonateRedCount);
            
        }

        private void onTick()
        {
            if (m_playerProxy!=null && m_playerProxy.CurrentRoleInfo!=null && m_playerProxy.CurrentRoleInfo.guildId==0 && 
                CoreUtils.uiManager.LayerCount(UILayer.WindowLayer)==0&& GuideProxy.IsGuideing==false
                )
            {
                if(CoreUtils.uiManager.LayerCount(UILayer.WindowPopLayer) > 1 ||
                    CoreUtils.uiManager.LayerCount(UILayer.WindowPopLayer) == 1 && !CoreUtils.uiManager.ExistUI(UI.s_fingerInfo))
                {
                    return;
                }
                
                //屏蔽loading界面
                if (CoreUtils.uiManager.ExistUI(UI.s_Loading))
                {
                    return;
                }
                
                //判断一下配置的引导是否通过了
                if (m_allianceRecommendedLimit > 0 && !m_funcGuideProxy.IsCompletedByStage(m_allianceRecommendedLimit))
                {
                    return;
                }

                m_allianceProxy.SearchAlliance(1);
                m_recommendTimer.Pause();
            }
        }

        public void ReTime()
        {
            m_recommendTimer.Resume();
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void OnBuildingHelp()
        {
            if(m_playerProxy.CurrentRoleInfo.buildQueue==null)
            {
                return;
            }
            if(!m_allianceProxy.HasJionAlliance())
            {
                foreach(var item in buildingQueue)
                {
                    item.Value.Dispose();
                }
                return;
            }
            foreach(var item in m_playerProxy.CurrentRoleInfo.buildQueue)
            {
                if (item.Value.requestGuildHelp||item.Value.finishTime<=0)
                {
                    if (buildingQueue.TryGetValue(item.Value.buildingIndex, out AllianceHelp helper))
                    {
                        helper.Dispose();
                    }
                }
                else if(!item.Value.requestGuildHelp)
                {
                    GameObject go =  CityObjData.GeBuildTipTargetGameObject(item.Value.buildingIndex);
                    if(go==null)
                    {
                        if(buildingQueue.TryGetValue(item.Value.buildingIndex,out var needDispose))
                        {
                            needDispose.Dispose();
                        }
                        continue;
                    }
                    if (!buildingQueue.ContainsKey(item.Value.buildingIndex) || buildingQueue[item.Value.buildingIndex] == null || buildingQueue[item.Value.buildingIndex].bDispose)
                    {
                        buildingQueue[item.Value.buildingIndex] = new AllianceHelp(go, (ui) =>
                        {
                            UI_Pop_IconOnHelpView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnHelpView>(ui.uiObj);
                            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, AskForHelp);
                            itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                            itemView.m_btn_click_GameButton.onClick.AddListener(() =>
                            {
                                m_allianceProxy.SendRequestHelp(1, item.Key);
                            });
                        });
                    }
                    else
                    {
                        if (buildingQueue[item.Value.buildingIndex] != null&& buildingQueue[item.Value.buildingIndex].hudui!=null)
                        {
                            buildingQueue[item.Value.buildingIndex].hudui.SetTargetGameObject(go);
                        }
                    }
                }
            }
        }


        private void OnTechHelp()
        {
            if (!m_allianceProxy.HasJionAlliance())
            {
                if(techQueue!=null)
                {
                    techQueue.Dispose();
                }
                return;
            }

            if (m_playerProxy.CurrentRoleInfo.technologyQueue!=null)
            {
                if (m_playerProxy.CurrentRoleInfo.technologyQueue.requestGuildHelp || m_playerProxy.CurrentRoleInfo.technologyQueue.finishTime <= 0)
                {
                    if (techQueue != null)
                    {
                        techQueue.Dispose();
                    }
                }
                else if (!m_playerProxy.CurrentRoleInfo.technologyQueue.requestGuildHelp && (techQueue == null || techQueue.bDispose))
                {
                    BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Academy));
                    if (buildingInfoEntity == null)
                    {
                        return;
                    }
                    GameObject go = CityObjData.GeBuildTipTargetGameObject(buildingInfoEntity.buildingIndex);
                    if (go != null)
                    {
                        techQueue = new AllianceHelp(go, (ui) =>
                        {
                            UI_Pop_IconOnHelpView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnHelpView>(ui.uiObj);
                            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, AskForHelp);
                            itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                            itemView.m_btn_click_GameButton.onClick.AddListener(() =>
                            {
                                m_allianceProxy.SendRequestHelp(3, m_playerProxy.CurrentRoleInfo.technologyQueue.queueIndex);
                            });
                        });
                    }
                }
            }
        }

        private void OnTreatmentHelp()
        {
            bool show = ShowOrHideTreatmentHelp();

            List<BuildingInfoEntity> buildingList = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.Hospital);

            for(int i = 0;i<buildingList.Count;i++)
            {
                GameObject go = CityObjData.GeBuildTipTargetGameObject(buildingList[i].buildingIndex);
                if (go == null)
                {
                    return;
                }

                if(!show&&treatmentQueue.ContainsKey(buildingList[i].buildingIndex))
                {
                    treatmentQueue[buildingList[i].buildingIndex].Dispose();
                }
                else if (show && (!treatmentQueue.ContainsKey(buildingList[i].buildingIndex) || treatmentQueue[buildingList[i].buildingIndex].bDispose))
                {
                    treatmentQueue[buildingList[i].buildingIndex] = new AllianceHelp(go, (ui) =>
                    {
                        UI_Pop_IconOnHelpView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnHelpView>(ui.uiObj);
                        ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, AskForHelp);
                        itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_click_GameButton.onClick.AddListener(() =>
                        {
                            m_allianceProxy.SendRequestHelp(2, m_playerProxy.CurrentRoleInfo.treatmentQueue.queueIndex);
                        });
                    });
                }
            }
        }

        private bool ShowOrHideTreatmentHelp()
        {
            if (!m_allianceProxy.HasJionAlliance())
            {
                return false;
            }

            if(m_playerProxy.CurrentRoleInfo.treatmentQueue!=null)
            {
                HospitalProxy hostPitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
                if (m_playerProxy.CurrentRoleInfo.treatmentQueue.requestGuildHelp || m_playerProxy.CurrentRoleInfo.treatmentQueue.finishTime <= 0)
                {
                    return false;
                }
                else if(!m_playerProxy.CurrentRoleInfo.treatmentQueue.requestGuildHelp&& hostPitalProxy.GetHospitalStatus() == (int)EnumHospitalStatus.Treatment)
                {
                    return true;
                }

            }


            return false;
        }

        private void OnGuildNotify(object body)
        {
            List<GuildRequestHelpInfoEntity> req = body as List<GuildRequestHelpInfoEntity>;
            if(req!=null&& req.Exists(i=>i.rid!=m_playerProxy.Rid))
            {
                if(allianceHelp==null||allianceHelp.bDispose)
                {
                    BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.AllianceCenter));
                    if (buildingInfoEntity == null)
                    {
                        OnCloseGuildHelp();
                        return;
                    }
                    GameObject go = CityObjData.GeBuildTipTargetGameObject(buildingInfoEntity.buildingIndex);
                    if (go == null)
                    {
                        OnCloseGuildHelp();
                        return;
                    }
                    allianceHelp = new AllianceHelp(go,(ui)=>
                    {
                        UI_Pop_IconOnHelpView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnHelpView>(ui.uiObj);
                        ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, HelpOther);
                        itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_click_GameButton.onClick.AddListener(() =>
                        {
                            m_allianceProxy.SendHelpGuildMembers();
                            
                            Tip.CreateTip(730002).SetStyle(Tip.TipStyle.AllianceHelp).Show();
                        });
                    });
                }
            }
            else
            {
                OnCloseGuildHelp();
            }
        }

        private void OnCloseGuildHelp()
        {
            if(allianceHelp!=null)
            {
                allianceHelp.Dispose();
            }
        }


        // 当占领圣地播放特效
        private void OnOccupyHolyLandPlayEffect(MapObjectInfoEntity info, RssType rssType)
        {
            if (info.gameobject != null && Common.IsInViewPort(WorldCamera.Instance().GetCamera(), info.gameobject.transform.position, 0))
            {
                StrongHoldDataDefine strongHoldCardDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int) info.strongHoldId);
                StrongHoldTypeDefine strongHoldTypeDefine =
                    CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldCardDefine.type);

                int lanId = 500775;    // 圣地
                if (rssType == RssType.CheckPoint)
                {
                    lanId = 500776;    // 关卡
                }
                
                string content =
                    LanguageUtils.getTextFormat(lanId, LanguageUtils.getText(strongHoldTypeDefine.l_nameId));
                
                CoreUtils.uiManager.ShowUI(UI.s_FightVictory, null, content);         
            }

            
        }

        private void CleanHolyLandInfos()
        {
            m_playerAttributeProxy.RemoveHolyland(m_allianceProxy.GetGuildHolyLandInfos());
            m_allianceProxy.CleanHolyLandInfos();
            m_playerAttributeProxy.UpdateHolyland(m_allianceProxy.GetGuildHolyLandInfos());
        }

    }
}

