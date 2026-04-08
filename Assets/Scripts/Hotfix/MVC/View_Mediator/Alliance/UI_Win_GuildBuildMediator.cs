// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, April 20, 2020
// Update Time         :    Monday, April 20, 2020
// Class Description   :    UI_Win_GuildBuildMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using Hotfix;

namespace Game
{
    public class UI_Win_GuildBuildMediator : GameMediator
    {
        #region Member

        // 徐帆与郑捷沟通，此界面数据不走同一套流程，可能不同建筑数据来自不同的表，
        // 故将界面要使用的数据抽成这个类，数据塞到这里。
        class BuildInfo
        {
            /// <summary> 
            /// 部队容量上限
            /// </summary>
            public int armyCntLimit = 0 ;  
            /// <summary> 
            /// 联盟建筑名称
            /// </summary>
            public int l_nameId = 0 ;
            /// <summary> 
            /// 建造进度上限
            /// </summary>
            public float S = 0 ;
            /// <summary> 
            /// 建筑UI图标
            /// </summary>
            public string iconImg = string.Empty ;
            /// <summary> 
            /// 前置科技类型要求
            /// </summary>
            public int scienceReq = 0 ;
            /// <summary> 
            /// 建设联盟个人积分
            /// </summary>
            public int allianceCoinReward = 0 ;
        }
        
        class AllArmyInfo_Item
        {
            public List<UI_Item_SoldierHead_SubView> subViews;
        }
        
        private enum CurViewTab
        {
            Reinforce,    // 增援界面
            Info,        // 增援信息界面
        }
        
        public static string NameMediator = "UI_Win_GuildBuildMediator";

        private WorldMapObjectProxy m_worldProxy;

        private AllianceProxy m_allianceProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;

        private MapObjectInfoEntity m_mapData;

        private TroopProxy m_troopProxy;

        private SoldierProxy m_soldierProxy;

        private BuildInfo m_buildInfo;

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        private List<AllianceBuildArmyLevel> m_armysList = new List<AllianceBuildArmyLevel>();
        private List<AllianceBuildArmyLevel> m_armysSortList = new List<AllianceBuildArmyLevel>();
        
        private Dictionary<long, long> m_AllArmyInfoDict = new Dictionary<long, long>();    // 该建筑所有军队士兵信息与数量
        
        private List<ReinforceDetailItemData> m_reinforceList = new List<ReinforceDetailItemData>();

        private long m_armyCount = 0;
        private int m_armyPlayerCount = 0;

        private CityReinforceInfo m_cityReinforceInfo;//目标的城市增援信息

        private CurViewTab m_curViewTab = CurViewTab.Reinforce;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildBuildMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_GuildBuildView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.AllianceBuildArmyUpdate,
                CmdConstant.MapObjectChange,
                CmdConstant.MapObjectRemove,
                Rally_RepatriationReinforce.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceBuildArmyUpdate:
                    ReList();
                    UpdateInfo();
                    break;
                case CmdConstant.MapObjectChange:
                {
                    MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                    if (mapItemInfo == null)
                    {
                        return;
                    }
                        if (m_mapData!=null)
                        {
                            if (mapItemInfo.objectId == m_mapData.objectId)
                            {
                                UpdateInfo();
                            }
                        }
                  
                }
                    break;
                case CmdConstant.MapObjectRemove:
                {
                    MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                    if (mapItemInfo == null)
                    {
                        return;
                    }
                        if (m_mapData != null)
                        {
                            if (mapItemInfo.objectId == m_mapData.objectId)
                            {
                                onClose();
                            }
                        }
                }
                    break;
                case Rally_RepatriationReinforce.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            Tip.CreateTip(200564).Show();
                        }
                        else
                        {
                            Rally_RepatriationReinforce.response response =
                    notification.Body as Rally_RepatriationReinforce.response;
                            if (response != null)
                            {

                            }
                        }
                        CoreUtils.uiManager.CloseUI(UI.s_AllianceBuild);
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
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
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
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;

            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            
            m_buildInfo = new BuildInfo();

            if (view.data is MapObjectInfoEntity)
            {
                m_mapData = view.data as MapObjectInfoEntity;
                SetBuildInfo();
                m_allianceProxy.SendMonitorBuildArmy(m_mapData.objectId);
            }
            else if(view.data is CityReinforceInfo)
            {
                m_cityReinforceInfo = view.data as CityReinforceInfo;
            }

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);

            if (m_mapData != null)
            {
                bool isBuilding = m_mapData.guildBuildStatus == (long)GuildBuildState.building;
                if (!isBuilding)
                {
                    if (m_mapData.rssType >= RssType.GuildFoodResCenter)
                    {
                        view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(170014));//采集
                    }
                    else
                    {
                        view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730122));//增援
                    }
                }
                else
                {
                    view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(180520));//建造
                }
                view.m_UI_Model_Reinforce.m_btn_cancel_GameButton.onClick.AddListener(onRemoveBuild);

                view.m_UI_Model_Reinforce.m_btn_cancel_GameButton.gameObject.SetActive(
                    m_allianceProxy.GetSelfRoot(GuildRoot.removeBuild));

                if (m_mapData.rssType == RssType.HolyLand || m_mapData.rssType == RssType.CheckPoint)    // 奇观建筑
                {
                    view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730122));//增援
                    view.m_UI_Model_Reinforce.m_btn_cancel_GameButton.gameObject.SetActive(false);
                    view.m_UI_Model_Reinforce.m_btn_info_GameButton.gameObject.SetActive(true);
                    view.m_UI_Model_Reinforce.m_btn_info_GameButton.onClick.AddListener(onClickInfo);
                }
                
                m_timer = Timer.Register(1, onTime, null, true);
            }
            else
            {
                view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730122));//增援
            }
        }

        private void onRemoveBuild()
        {
            Alert.CreateAlert(732022, LanguageUtils.getText(300099))
                .SetLeftButton(() => { m_allianceProxy.SendRemoveBuild(m_mapData.objectId); },
                    LanguageUtils.getText(730154)).SetRightButton(null, LanguageUtils.getText(730155)).Show();
        }

        private void onClickInfo()
        {
            SetCurViewTab(CurViewTab.Info);
            view.m_sv_all_ListView.ForceRefresh();
            
            
            CoreUtils.logService.Warn($"onClickInfo    援助详情界面功能完成后调用  ");   
        }

        private void onClose()
        {
            if (m_curViewTab == CurViewTab.Info)    // 当前在增援信息界面，则返回
            {
                SetCurViewTab(CurViewTab.Reinforce);
                return;
            }
            
            CoreUtils.uiManager.CloseUI(UI.s_AllianceBuild,true,true);
        }


        private void onTimeEnd()
        {
        }

        private Timer m_timer;

        private void onTime()
        {
            UpdateInfo();

            if (m_armysList.Count > 0)
            {
                for (int i = 0; i < m_armysList.Count; i++)
                {
                    var item = view.m_UI_Model_Reinforce.m_sv_list_ListView.GetItemByIndex(i);
                    if (item != null && item.go != null)
                    {
                        ViewItemByIndex(item);
                    }
                }
            }
        }


        private void UpdateInfo()
        {
            view.m_UI_Model_Reinforce.m_pb_rogressBar_GameSlider.value =
                (float) m_armyCount / m_buildInfo.armyCntLimit;

            view.m_UI_Model_Reinforce.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_buildInfo.l_nameId);

            switch ((GuildBuildState) m_mapData.guildBuildStatus)
            {
                case GuildBuildState.building:

                    view.m_UI_Model_Reinforce.m_lbl_desc_LanguageText.text = LanguageUtils.getText(732084);
                    
                    if (m_mapData.buildFinishTime > 0)
                    {
                        float rate = ((float) m_buildInfo.S - m_mapData.buildProgress) /
                                     (m_mapData.buildFinishTime - m_mapData.buildProgressTime);

                        int pro = Mathf.FloorToInt(
                            (m_mapData.buildProgress +
                             rate * (ServerTimeModule.Instance.GetServerTime() - m_mapData.buildProgressTime)) /
                            m_buildInfo.S * 100f);

                        //建造进度
                        view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text =
                            LanguageUtils.getTextFormat(732094, pro);
                        //剩余时间
                        view.m_UI_Model_Reinforce.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(732095,
                            ClientUtils.FormatTimeSplit(
                                (int) (m_mapData.buildFinishTime - ServerTimeModule.Instance.GetServerTime())));
                    }
                    else
                    {
                        //建造进度
                        view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text = LanguageUtils.getTextFormat(732094,
                            (int) ((float) m_mapData.buildProgress / m_buildInfo.S * 100));
                        //剩余时间
                        view.m_UI_Model_Reinforce.m_lbl_time_LanguageText.text =
                            LanguageUtils.getTextFormat(732095,
                                ClientUtils.FormatTimeSplit((int) m_mapData.needBuildTime));
                        
                        
                    }

                    break;
                default:
                    
                    
                    if (m_mapData.rssType == RssType.HolyLand || m_mapData.rssType == RssType.CheckPoint)
                    {
                        view.m_UI_Model_Reinforce.m_lbl_desc_LanguageText.text = LanguageUtils.getText(732085);
                        view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.gameObject.SetActive(false);
                        view.m_UI_Model_Reinforce.m_lbl_time_LanguageText.gameObject.SetActive(false);
                        
                    }
                    else if (m_mapData.rssType <= RssType.GuildFlag)
                    {
                        view.m_UI_Model_Reinforce.m_lbl_desc_LanguageText.text = LanguageUtils.getText(732085);


                        switch ((GuildBuildState) m_mapData.guildBuildStatus)
                        {
                            case GuildBuildState.fire:
                                float durable = m_mapData.durable -
                                                (ServerTimeModule.Instance.GetServerTime() - m_mapData.buildBurnTime) *
                                                m_mapData.buildBurnSpeed / 100f;

                                //耐久度
                                view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text =
                                    LanguageUtils.getTextFormat(732072,
                                        Mathf.FloorToInt(durable / m_mapData.durableLimit * 100f));
                                break;
                            case GuildBuildState.fix:

                                int buildID = m_allianceProxy.GetBuildServerTypeToConfigType((long) m_mapData.rssType);

                                var buildInfo = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(buildID);

                                float durablefix = m_mapData.durable +
                                                   (ServerTimeModule.Instance.GetServerTime() -
                                                    m_mapData.buildDurableRecoverTime) *
                                                   buildInfo.durableUp / 3600f;

                                if (durablefix > m_mapData.durableLimit)
                                {
                                    durablefix = m_mapData.durableLimit;
                                }

                                //耐久度
                                view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text =
                                    LanguageUtils.getTextFormat(732072,
                                        Mathf.FloorToInt(durablefix / m_mapData.durableLimit * 100f));
                                break;

                            default:
                                //耐久度
                                view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text =
                                    LanguageUtils.getTextFormat(732072,
                                        Mathf.FloorToInt(m_mapData.durable / m_mapData.durableLimit * 100f));
                                break;
                        }
                       

                        
                        
                        //状态
                        view.m_UI_Model_Reinforce.m_lbl_time_LanguageText.text = RS.GetGuildBuildState(m_mapData);
                    }
                    else
                    {
                        view.m_UI_Model_Reinforce.m_lbl_desc_LanguageText.text = LanguageUtils.getText(730251);
                        view.m_UI_Model_Reinforce.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                        
                        long passTime = ServerTimeModule.Instance.GetServerTime() - m_mapData.collectTime;
                        long collectRes = passTime * m_mapData.collectSpeed / 10000;
                        long coloect = m_mapData.resourceAmount - collectRes;

                        if (coloect<0)
                        {
                            coloect = 0;
                        }
                        //储量
                        view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text =
                            LanguageUtils.getTextFormat(732096, ClientUtils.FormatComma(coloect));
                        //采集人数
                        view.m_UI_Model_Reinforce.m_lbl_time_LanguageText.text =
                            LanguageUtils.getTextFormat(732097, m_mapData.collectRoleNum);
                    }

                    break;
            }
        }

        protected override void BindUIData()
        {
            SetCurViewTab(CurViewTab.Reinforce);
            if (m_mapData!=null)
            {
                view.m_UI_Model_Reinforce.m_UI_PlayerHead.gameObject.SetActive(false);

                ClientUtils.LoadSprite(view.m_UI_Model_Reinforce.m_img_icon_PolygonImage, m_buildInfo.iconImg);
                UpdateInfo();
                m_preLoadRes.AddRange(view.m_UI_Model_Reinforce.m_sv_list_ListView.ItemPrefabDataList);
                m_preLoadRes.AddRange(view.m_sv_all_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
                {
                    m_assetDic = assetDic;

                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ViewItemByIndex;
                    funcTab.GetItemSize = GetItemSize;
                    funcTab.GetItemPrefabName = GetItemPrefabName;
                    
                    view.m_UI_Model_Reinforce.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_armysList.Count);

                    ReList();
                    
                    ListView.FuncTab allFuncTab = new ListView.FuncTab();
                    allFuncTab.ItemEnter = ListItemByIndex_All;
                    view.m_sv_all_ListView.SetInitData(m_assetDic, allFuncTab);
                    view.m_sv_all_ListView.FillContent(1);
                });
                
            }
            else
            {
                view.m_UI_Model_Reinforce.m_lbl_pro_LanguageText.text = "";
                view.m_UI_Model_Reinforce.m_lbl_time_LanguageText.text = "";
                view.m_UI_Model_Reinforce.m_lbl_statepro_LanguageText.text = "";
                view.m_UI_Model_Reinforce.m_btn_cancel_GameButton.gameObject.SetActive(false);
                view.m_UI_Model_Reinforce.m_lbl_desc_LanguageText.text = LanguageUtils.getText(730249);
                view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730316));
                var info = m_allianceProxy.GetAlliance();
                if (m_cityReinforceInfo.type == 1)
                {
                    view.m_UI_Model_Reinforce.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(730317, LanguageUtils.getTextFormat(300030, info.abbreviationName, m_cityReinforceInfo.targetGuildMemberInfoEntity.name));
                    view.m_UI_Model_Reinforce.m_UI_PlayerHead.LoadPlayerIcon(m_cityReinforceInfo.targetGuildMemberInfoEntity.headId, m_cityReinforceInfo.targetGuildMemberInfoEntity.headFrameID);

                }
                else if (m_cityReinforceInfo.type == 2)
                {
                    view.m_UI_Model_Reinforce.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(730317, LanguageUtils.getTextFormat(300030, info.abbreviationName, m_cityReinforceInfo.targetMapObjectInfoEntity.cityName));
                    view.m_UI_Model_Reinforce.m_UI_PlayerHead.LoadPlayerIcon(m_cityReinforceInfo.targetMapObjectInfoEntity.headId, m_cityReinforceInfo.targetMapObjectInfoEntity.headFrameID);
                }

                m_preLoadRes.AddRange(view.m_UI_Model_Reinforce.m_sv_list_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
                {
                    m_assetDic = assetDic;
                    InitView(m_cityReinforceInfo.response);
                });
            }
        }


        private AllianceBuildArmyLevel myCreate;


        public void ReList(bool isReInit =false)
        {
            if (m_assetDic.Count > 0)
            {                
                m_armysSortList.Clear();
                m_armysList.Clear();

                m_armyPlayerCount = 0;

                if (myCreate == null)
                {
                    myCreate = new AllianceBuildArmyLevel();
                    myCreate.prefab_index = 0;
                }

                bool canCreateArmy = false;
                List<AllianceBuildArmyLevel> initdatas;

                
                RssType rssType = (RssType) m_mapData.objectType;
                //CoreUtils.logService.Error($"[ReList]   rssType:[{rssType}]  objectId:[{m_mapData.objectId}]");
                if (rssType == RssType.HolyLand || rssType == RssType.CheckPoint)
                {
                    initdatas = m_allianceProxy.GetBuildArmyInHolyLand(m_mapData.objectId);
                    canCreateArmy = m_allianceProxy.GetMyArmyCountInHolyLand(m_mapData.objectId) < m_troopProxy.GetTroopDispatchNum();
                    
                }
                else
                {
                    initdatas = m_allianceProxy.GetBuildArmy(m_mapData.objectId);
                    canCreateArmy = !m_allianceProxy.HasMyArmyInBuild(m_mapData.objectId);

                    if (m_mapData.guildBuildStatus !=(long)GuildBuildState.building && m_mapData.rssType<RssType.GuildFoodResCenter)
                    {
                        canCreateArmy = true;
                    }
                }
                if (canCreateArmy)
                {
                    m_armysList.Add(myCreate);
                }
                

                if (initdatas != null)
                {
                    m_armysList.AddRange(initdatas);
                }

                OnSort(canCreateArmy);   
                m_armyCount = 0;
                foreach (var army in m_armysList)
                {
                    m_armyCount += army.armyCount;
                }

                m_armyPlayerCount = m_armysList.Count;

                if (!m_allianceProxy.HasMyArmyInBuild(m_mapData.objectId))
                {
                    m_armyPlayerCount = m_armyPlayerCount - 1;
                }

                int len = m_armysList.Count;
                
                HashSet<AllianceBuildArmyLevel> hashSet = new HashSet<AllianceBuildArmyLevel>();
                for (int i = len-1; i >=0; i--)
                {
                    for (int j = len -1; j>=0; j--)
                    {
                        var level = m_armysList[j];

                        if (level.isSelected)
                        {
                            if (level.isHolyLand==false)
                            {
                                if (level.LevelMember!=null && level.LevelMember.buildArmyIndex > 0 && level.prefab_index == 1 && !hashSet.Contains(level))
                                {
                                    AddMember(level.LevelMember.buildArmyIndex, j, level);
                                    hashSet.Add(level);
                                }
                            }
                            else
                            {
                                HolyLandArmyInfoEntity holyLandArmy = level.holyLandArmyInfoEntity;
                                if(holyLandArmy != null && !hashSet.Contains(level))
                                {
                                    AddMember(holyLandArmy.buildArmyIndex, j, level);
                                    hashSet.Add(level);
                                }
                            }
                        }
                    }
                }

                
                

                //部队容量
                view.m_UI_Model_Reinforce.m_lbl_pro_LanguageText.text =
                    LanguageUtils.getTextFormat(730318, ClientUtils.FormatComma(m_armyCount),
                        ClientUtils.FormatComma(m_buildInfo.armyCntLimit));

                view.m_UI_Model_Reinforce.m_pb_rogressBar_GameSlider.value =
                    (float)m_armyCount / m_buildInfo.armyCntLimit;

                view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_armysList.Count);
            }

            ReAllArmyInfoDict();
        }

        #region 重新排序 找出队长插到最前面

        private void OnSort(bool canCreateArmy)
        {
            m_armysSortList.Clear();
            foreach (var info in m_armysList)
            {
                GuildBuildArmyInfoEntity armyInfo = info.LevelMember;
                if (armyInfo != null)
                {
                    if (m_allianceProxy.IsBuildArmsLeader(m_mapData.objectId, armyInfo.buildArmyIndex))
                    {
                        if (canCreateArmy)
                        {
                            m_armysSortList.Insert(1, info);
                        }
                        else
                        {
                            m_armysSortList.Insert(0, info);
                        }
                    }
                    else
                    {
                        m_armysSortList.Add(info);
                    }
                }
                else
                {
                    m_armysSortList.Add(info);
                }
            }

            m_armysList.Clear();
            foreach (var info in m_armysSortList)
            {
                m_armysList.Add(info);
            }
        }

        #endregion
        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = m_armysList[item.index];

            return view.m_UI_Model_Reinforce.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
        }

        #region 城市增援界面功能

        private void InitView(Role_GetCityReinforceInfo.response response)
        {
            view.m_UI_Model_Reinforce.m_pb_rogressBar_GameSlider.value =
                (float) response.armyCount / response.armyCountMax;
            view.m_UI_Model_Reinforce.m_lbl_pro_LanguageText.text =
                LanguageUtils.getTextFormat(730318, response.armyCount.ToString("N0"), response.armyCountMax.ToString("N0"));
            view.m_UI_Model_Reinforce.m_pb_rogressBar_GameSlider.value = (float)response.armyCount/response.armyCountMax;

          
            if (response.reinforceArmyInfo.Count == 0)
            {
                if (response.armyCount != response.armyCountMax)
                {
                    ReinforceDetailItemData reinforceDetailItemData = new ReinforceDetailItemData();
                    reinforceDetailItemData.prefab_index = 0;
                    m_reinforceList.Add(reinforceDetailItemData);
                }
            }
            else if (response.reinforceArmyInfo.Count == 1)
            {
                ReinforceDetailItemData reinforceDetailItemData = new ReinforceDetailItemData();
                reinforceDetailItemData.prefab_index = 1;
                ReinforceArmyInfo reinforceArmyInfo = null;
                if (response.reinforceArmyInfo.TryGetValue(m_playerProxy.CurrentRoleInfo.rid, out reinforceArmyInfo))
                {
                    reinforceDetailItemData.armyCount = m_allianceProxy.CountSoldiers(reinforceArmyInfo.soldiers);
                }

                reinforceDetailItemData.LevelMember = reinforceArmyInfo;

                m_reinforceList.Add(reinforceDetailItemData);
            }
            else
            {
                Debug.LogErrorFormat("增援部队数量部队");
            }

            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ViewItemByIndexCity;
            funcTab.GetItemSize = GetItemSizeCity;
            funcTab.GetItemPrefabName = GetItemPrefabNameCity;

            view.m_UI_Model_Reinforce.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_reinforceList.Count);
        }

        private string GetItemPrefabNameCity(ListView.ListItem item)
        {
            var data = m_reinforceList[item.index];

            return view.m_UI_Model_Reinforce.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
        }

        private float GetItemSizeCity(ListView.ListItem item)
        {
            var data = m_reinforceList[item.index];

            if (data.prefab_index == 0)
            {
                return 120f;
            }
            else if (data.prefab_index == 1)
            {
                return 120f;
            }

            return 120;
        }

        private void ViewItemByIndexCity(ListView.ListItem scrollItem)
        {
            var data = m_reinforceList[scrollItem.index];
            if (data.prefab_index == 0) //UI_Item_WarMemberJoin
            {
                UI_Item_WarMemberJoinView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberJoinView>(scrollItem.go);

                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceBuild);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);

                    if (m_cityReinforceInfo.type == 1)
                    {
                        MinimapProxy minimapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
                        MemberPosInfo memberPosInfo = null;
                        if (minimapProxy.MemberPos.TryGetValue(m_cityReinforceInfo.targetGuildMemberInfoEntity.rid, out memberPosInfo))
                        {
                            FightHelper.Instance.Reinfore((int)m_cityReinforceInfo.targetGuildMemberInfoEntity.cityObjectIndex, 0, (int)m_cityReinforceInfo.targetGuildMemberInfoEntity.cityObjectIndex, memberPosInfo.pos.x, memberPosInfo.pos.y, false, m_cityReinforceInfo.response.armyCountMax - m_cityReinforceInfo.response.armyCount,false);
                        }
                    }
                    else
                    {
                        FightHelper.Instance.Reinfore((int)m_cityReinforceInfo.targetMapObjectInfoEntity.objectId, 0, (int)m_cityReinforceInfo.targetMapObjectInfoEntity.objectId, (float)m_cityReinforceInfo.targetMapObjectInfoEntity.objectPos.x, (float)m_cityReinforceInfo.targetMapObjectInfoEntity.objectPos.y, false, m_cityReinforceInfo.response.armyCountMax - m_cityReinforceInfo.response.armyCount);
                    }
                });
            }
            else if (data.prefab_index == 1) //UI_Item_WarMember  //
            {
                UI_Item_WarMemberView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberView>(scrollItem.go);

                var armyInfo = data.LevelMember;
                //----------------------------------------------------------
                itemView.m_pb_collect_GameSlider.maxValue = 1;
                itemView.m_pb_collect_GameSlider.minValue = 0;
                itemView.m_pb_collect_GameSlider.value = 1;
                itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                itemView.m_btn_leader_GameButton.gameObject.SetActive(false); //队长标志
                itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                itemView.m_lbl_colPro_LanguageText.gameObject.SetActive(false);
                //----------------------------------------------------------
                int armyCount = 0;

                itemView.m_UI_PlayerHead.LoadPlayerIcon((int)armyInfo.headId , (int)armyInfo.headFrameID);
                itemView.m_lbl_name_LanguageText.text = armyInfo.name;


                itemView.m_UI_Captain1.gameObject.SetActive(armyInfo.mainHeroId > 0);

                string nameCp1 = String.Empty;
                string nameCp2 = String.Empty;

                if (armyInfo.mainHeroId > 0)
                {
                    nameCp1 = itemView.m_UI_Captain1.LoadHeadID(armyInfo.mainHeroId, armyInfo.mainHeroLevel);
                }

                itemView.m_UI_Captain2.gameObject.SetActive(armyInfo.deputyHeroId > 0);
                if (armyInfo.deputyHeroId > 2)
                {
                    nameCp2 = itemView.m_UI_Captain2.LoadHeadID(armyInfo.deputyHeroId, armyInfo.deputyHeroLevel);
                }

                //队伍信息
                itemView.m_lbl_armyCount_LanguageText.text =
                    LanguageUtils.getTextFormat(730309, ClientUtils.FormatComma(data.armyCount));

                itemView.m_lbl_captainName_LanguageText.text = armyInfo.deputyHeroId > 0
                    ? LanguageUtils.getTextFormat(300001, nameCp1, nameCp2)
                    : nameCp1;
                RefreshView(itemView, data);
                m_timer = Timer.Register(1, () => { RefreshView(itemView, data); }, null, true, true, view.vb);
                //队伍返回

                itemView.m_btn_back_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_back_GameButton.onClick.AddListener(() =>
                {
                    ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                        .GetArmyDataByArmyId((int) data.LevelMember.armyIndex);

                    //     if (armyData != null)
                    {
                        //自己部队返回
                        Rally_RepatriationReinforce.request request = new Rally_RepatriationReinforce.request();
                        if (m_cityReinforceInfo.type == 1)
                        {
                            request.repatriationRid = m_cityReinforceInfo.targetGuildMemberInfoEntity.rid;
                            request.fromObjectIndex = m_cityReinforceInfo.targetGuildMemberInfoEntity.cityObjectIndex;
                        }
                        else
                        {
                            request.repatriationRid = m_cityReinforceInfo.targetMapObjectInfoEntity.cityRid ;
                            request.fromObjectIndex = m_cityReinforceInfo.targetMapObjectInfoEntity.objectId;
                        }
                        request.repatriationArmyIndex = data.LevelMember.armyIndex;
                        request.isSelfBack = true;
                        AppFacade.GetInstance().SendSproto(request);
                    }
                });

                itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);
                

                //加入队伍
                itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                {
                    //selected tag
                    data.isSelected = !data.isSelected;

                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);

                    if (data.isSelected)
                    {
                        AddMemberCity(scrollItem.index, data);
                        view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_reinforceList.Count);
                    }
                    else
                    {
                        RemoveMemberCity(scrollItem.index, data);
                        view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_reinforceList.Count);
                    }
                });
            }
            else
            {
                UI_Item_WarMenberDetialView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
                UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[]
                    {itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4};


                var len = subItems.Length;
                for (int i = 0; i < len; i++)
                {
                    var subItem = subItems[i];
                    var subData = data.subItemData.Count - 1 >= i ? data.subItemData[i] : null;

                    subItem.gameObject.SetActive(subData != null);

                    if (subData != null)
                    {
                        subItem.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int) subData.id), (int) subData.num);
                    }
                }
            }
        }

        private void RefreshView(UI_Item_WarMemberView itemView, ReinforceDetailItemData data)
        {
            int leftTimeArrival = (int)(data.LevelMember.arrivalTime - ServerTimeModule.Instance.GetServerTime());
            TimeSpan m_formatTimeSpan;

            if (leftTimeArrival>=0)
            {
                m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeArrival);
                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(200088, //增援行军中
                    ClientUtils.FormatTimeSplit(leftTimeArrival));
            }
            else
            {
                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText( 732076);
            }
        }

        public void AddMemberCity(int index, ReinforceDetailItemData tag)
        {
            List<SoldierInfo> soldierInfos = new List<SoldierInfo>();
            soldierInfos = tag.LevelMember.soldiers.Values.ToList();
            soldierInfos.Sort((SoldierInfo x, SoldierInfo y) =>
            {
                int re = 0;
                re = ((int) y.level).CompareTo((int) x.level);
                if (re == 0)
                {
                    return x.type.CompareTo(y.type);
                }

                return re;
            });
            int len = soldierInfos.Count;
            for (int i = 0; i < len; i = i + 4)
            {
                ReinforceDetailItemData itemWarDetialData = new ReinforceDetailItemData();

                itemWarDetialData.prefab_index = 2; //兵种
                itemWarDetialData.subItemData = new List<SoldierInfo>();
                for (int j = i; j < i + 4; j++)
                {
                    if (j < len)
                    {
                        itemWarDetialData.subItemData.Add(soldierInfos[j]);
                    }
                }

                m_reinforceList.Insert(index + 1, itemWarDetialData);
                index++;
            }
        }

        public void RemoveMemberCity(int index, ReinforceDetailItemData tag)
        {
            int startIndex = 0;

            int count = 0;

            int len = m_reinforceList.Count;

            for (int i = index + 1; i < len; i++)
            {
                var item = m_reinforceList[i];
                if (startIndex == 0)
                {
                    startIndex = i;
                }

                count++;
            }

            m_reinforceList.RemoveRange(startIndex, count);
        }

        #endregion

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_armysList[scrollItem.index];
            if (data.prefab_index == 0) //UI_Item_WarMemberJoin
            {
                UI_Item_WarMemberJoinView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberJoinView>(scrollItem.go);

                itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                {
                    if (m_buildInfo.scienceReq > 0)
                    {
                        ResearchProxy researchProxy =
                            AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
                        if (researchProxy.GetCrrTechnologyLv(m_buildInfo.scienceReq) == 0 )
                        {
                            var resConfig = researchProxy.GetTechnologyList(m_buildInfo.scienceReq);
                            Tip.CreateTip(500101, LanguageUtils.getText(resConfig[0].l_nameID),
                                LanguageUtils.getText(m_buildInfo.l_nameId)).Show();
                            return;
                        }
                    }

                    if (m_mapData.rssType >= RssType.GuildFoodResCenter && m_mapData.rssType<= RssType.GuildGoldResCenter)
                    {
                        if (m_playerProxy.CurrentRoleInfo.level< m_allianceProxy.Config.allianceResourcePointReqLevel)
                        {
                            Tip.CreateTip(LanguageUtils.getTextFormat(732023,m_allianceProxy.Config.allianceResourcePointReqLevel)).Show();
                            return;
                        }
                    }

                    FightHelper.Instance.Reinfore((int)m_mapData.objectId, 0, (int)m_mapData.objectId, m_mapData.gameobject.transform.position.x * 100, m_mapData.gameobject.transform.position.z * 100, true, m_buildInfo.armyCntLimit - m_armyCount);

                    onClose();
                });
            }
            else if (data.prefab_index == 1) //UI_Item_WarMember  //
            {
                UI_Item_WarMemberView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberView>(scrollItem.go);

                
                
                
                /*itemView.m_btn_back_GameButton.gameObject.SetActive(armyInfo.rid == m_playerProxy.Rid);
                int armyCount = 0;
                bool isMyCollect = armyInfo.rid == m_playerProxy.CurrentRoleInfo.rid &&
                                   m_mapData.rssType >= RssType.GuildFoodStorage  ;
                //采集进度条
                itemView.m_pb_collect_ArabLayoutCompment.gameObject.SetActive(isMyCollect && m_mapData.guildBuildStatus>(int)GuildBuildState.building);
                
                if (isMyCollect)
                {
                    //TODO zj
                    Int64 collectNum = ArmyInfoHelp.Instance.GetArmyCollectNum((int) armyInfo.armyIndex);
                    float m_canCollectNum = ArmyInfoHelp.Instance.GetArmyWeight((int) armyInfo.armyIndex);
                    itemView.m_pb_collect_GameSlider.maxValue =
                        ArmyInfoHelp.Instance.GetArmyCollectMaxTime((int) armyInfo.armyIndex);
                    itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(500008,
                        ClientUtils.FormatComma(collectNum),
                        m_canCollectNum);
                    itemView.m_pb_collect_GameSlider.value =
                        ArmyInfoHelp.Instance.GetArmyCollectCurValue((int) armyInfo.armyIndex);
                    
                }

                if (m_armyCount>0)
                {
                    UpdateInfo();
                }
               

                itemView.m_UI_PlayerHead.LoadPlayerIcon((int)member.headId, (int)member.headFrameID);

                itemView.m_lbl_name_LanguageText.text = member.name;


                itemView.m_UI_Captain1.gameObject.SetActive(armyInfo.mainHeroId > 0);

                string nameCp1 = String.Empty;
                string nameCp2 = String.Empty;

                if (armyInfo.mainHeroId > 0)
                {
                    nameCp1 = itemView.m_UI_Captain1.LoadHeadID(armyInfo.mainHeroId, armyInfo.mainHeroLevel);
                }

                itemView.m_UI_Captain2.gameObject.SetActive(armyInfo.deputyHeroId > 0);
                if (armyInfo.deputyHeroId > 2)
                {
                    nameCp2 = itemView.m_UI_Captain2.LoadHeadID(armyInfo.deputyHeroId, armyInfo.deputyHeroLevel);
                }

                //队伍信息
                itemView.m_lbl_armyCount_LanguageText.text =
                    LanguageUtils.getTextFormat(730309, ClientUtils.FormatComma(data.armyCount));

                itemView.m_lbl_name_LanguageText.text = m_allianceProxy.getMemberInfo(armyInfo.rid).name;

                itemView.m_lbl_captainName_LanguageText.text = armyInfo.deputyHeroId > 0
                    ? LanguageUtils.getTextFormat(1, nameCp1, nameCp2)
                    : nameCp1;

                bool isBuilding = m_mapData.guildBuildStatus == (long) GuildBuildState.building;

                //状态
                itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
             
                itemView.m_pl_time_ArabLayoutCompment.gameObject.SetActive(
                    !TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.REINFORCE_MARCH) && isBuilding);


                bool isMarching = TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.REINFORCE_MARCH);
                
                if (isMarching )
                {
                    itemView.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(200088, //增援行军中
                        ClientUtils.FormatTimeSplit(
                            (int) (armyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime())));
                }
                else
                {
                    itemView.m_pl_time_ArabLayoutCompment.gameObject.SetActive(isBuilding && armyInfo.rid == m_playerProxy.CurrentRoleInfo.rid);
                    
                    //已到达 采集中
                    if (isBuilding)
                    {
                        itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(181240);//已到达

                        
                        
                        //TODO time
                        int time = (int) (ServerTimeModule.Instance.GetServerTime() - armyInfo.arrivalTime);//时间
                        itemView.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(732098,
                            ClientUtils.FormatTimeSplit(time));
                        float rate = m_buildInfo.allianceCoinReward / 3600;
                        itemView.m_lbl_curNum_LanguageText.text = ClientUtils.FormatComma((long) (time * rate));
                    }
                    else
                    {
                        if (m_mapData.rssType<= RssType.GuildFlag)
                        {
                            itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732076);//已到达
                        }
                        else
                        {
                            itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732088);//采集中
                        }
                    }
                }


                //队伍返回
                itemView.m_btn_back_GameButton.gameObject.SetActive(
                    data.LevelMember.rid == m_playerProxy.CurrentRoleInfo.rid);
                itemView.m_btn_back_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_back_GameButton.onClick.AddListener(() =>
                {
                    //自己部队返回

                    m_troopProxy.TroopMapMarCh((int) data.LevelMember.armyIndex, TroopAttackType.Retreat, 0, null);
                });

                //领队
                itemView.m_btn_leader_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_leader_GameButton.onClick.AddListener(() =>
                {
                    HelpTip.CreateTip(4007, itemView.m_btn_leader_GameButton.transform)
                        .SetStyle(HelpTipData.Style.arrowDown).Show();
                });
                
                
                itemView.m_btn_leader_GameButton.gameObject.SetActive(m_allianceProxy.IsBuildArmsLeader(m_mapData.objectId,data.LevelMember.buildArmyIndex) && isMarching==false);

                //扩展
                itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(data.isSelected);
                itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(!data.isSelected);

                //加入队伍
                itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                {
                    //selected tag
                    data.isSelected = !data.isSelected;

                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(!data.isSelected);
                    if (data.isSelected)
                    {
                        AddMember(data.LevelMember.buildArmyIndex, scrollItem.index, data);
                    }
                    else
                    {
                        RemoveMember(data.LevelMember.buildArmyIndex, scrollItem.index);
                    }
                });*/

                if (data.isHolyLand)    // 圣地建筑处理
                {
                    HolyLandArmyInfoEntity holyLandArmy = data.holyLandArmyInfoEntity;
                    ViewItemByIndex_Prefab1(itemView, holyLandArmy.rid, holyLandArmy.roleHeadId, holyLandArmy.roleHeadFrameId, holyLandArmy.roleName, holyLandArmy.armyIndex,
                        holyLandArmy.mainHeroId, holyLandArmy.mainHeroLevel, holyLandArmy.deputyHeroId, holyLandArmy.deputyHeroLevel, holyLandArmy.status, holyLandArmy.arrivalTime,  holyLandArmy.buildArmyIndex, scrollItem.index, data);    
                }
                else
                {
                    var armyInfo = data.LevelMember;
                    ViewItemByIndex_Prefab1(itemView, armyInfo.rid, armyInfo.roleHeadId, armyInfo.roleHeadFrameId, armyInfo.roleName, armyInfo.armyIndex,
                        armyInfo.mainHeroId, armyInfo.mainHeroLevel, armyInfo.deputyHeroId, armyInfo.deputyHeroLevel, armyInfo.status, armyInfo.arrivalTime, armyInfo.buildArmyIndex,scrollItem.index, data);    
                }
            }
            else
            {
                UI_Item_WarMenberDetialView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
                UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[]
                    {itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4};


                var len = subItems.Length;
                for (int i = 0; i < len; i++)
                {
                    var subItem = subItems[i];
                    var subData = data.subItemData.Count - 1 >= i ? data.subItemData[i] : null;

                    subItem.gameObject.SetActive(subData != null);

                    if (subData != null)
                    {
                        subItem.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int) subData.id), (int) subData.num);
                    }
                }
            }
        }


        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_armysList[item.index];

            if (data.prefab_index == 0)
            {
                return 98f;
            }
            else if (data.prefab_index == 1)
            {
                return 98f;
            }

            return 98f;
        }


        public void AddMember(long buildArmyIndex, int index, AllianceBuildArmyLevel tag)
        {
            
            
            
            //建筑 资源点
            Dictionary<long, SoldierInfo> soldierInfoDict = null;
            if (tag.isHolyLand )
            {
                if (tag.holyLandArmyInfoEntity!=null)
                {
                    soldierInfoDict = tag.holyLandArmyInfoEntity.soldiers;
                }
                
            }
            else
            {
                if (tag.LevelMember!=null)
                {
                    soldierInfoDict = tag.LevelMember.soldiers;
                }
            }

            if (soldierInfoDict==null || view.gameObject ==null)
            {
                return;
            }
            
            int len = soldierInfoDict.Count;
            
            List<SoldierInfo> soldierInfos = soldierInfoDict.Values.ToList();
            soldierInfos.Sort((SoldierInfo x, SoldierInfo y) =>
            {
                int re = 0;
                re = ((int) y.level).CompareTo((int) x.level);
                if (re == 0)
                {
                    return x.type.CompareTo(y.type);
                }

                return re;
            });
            for (int i = 0; i < len; i=i+4)
            {
                AllianceBuildArmyLevel buildLineData = new AllianceBuildArmyLevel();


                buildLineData.prefab_index = 2; //兵种


                buildLineData.subItemData = new List<SoldierInfo>();
                buildLineData.LevelMember = tag.LevelMember;
                buildLineData.holyLandArmyInfoEntity = tag.holyLandArmyInfoEntity;
                

                for (int j = i; j < i + 4; j++)
                {
                    if (j < len)
                    {
                        buildLineData.subItemData.Add(soldierInfos[j]);
                    }
                }

                m_armysList.Insert(index + 1, buildLineData);
                index++;
            }

           


            view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_armysList.Count);
        }

        public void RemoveMember(long buildArmyIndex, int index)
        {
            int startIndex = 0;

            int count = 0;

            int len = m_armysList.Count;

            for (int i = 0; i < len; i++)
            {
                var item = m_armysList[i];

                if ((item.LevelMember !=null && item.LevelMember.buildArmyIndex == buildArmyIndex) || 
                    (item.holyLandArmyInfoEntity != null && item.holyLandArmyInfoEntity.buildArmyIndex == buildArmyIndex))
                {
                    if (startIndex == 0)
                    {
                        startIndex = i + 1;
                    }
                    else
                    {
                        count = count + 1;
                    }
                }
            }

            m_armysList.RemoveRange(startIndex, count);
            view.m_UI_Model_Reinforce.m_sv_list_ListView.FillContent(m_armysList.Count);
        }

        #endregion

        void ViewItemByIndex_Prefab1(UI_Item_WarMemberView itemView, long rid, long headId, long headFrameID, string name, long armyIndex, long mainHeroId, long mainHeroLevel, long deputyHeroId, long deputyHeroLevel, long status, long arrivalTime, long buildArmyIndex, int index, AllianceBuildArmyLevel data)
        {
                bool isSelf = rid == m_playerProxy.Rid;
                itemView.m_btn_back_GameButton.gameObject.SetActive(isSelf);
                int armyCount = 0;

                long remainArrivalTime = arrivalTime - ServerTimeModule.Instance.GetServerTime();
                
                bool isMarching = remainArrivalTime>0;

               
                if (m_armyCount>0)
                {
                    UpdateInfo();
                }
                itemView.m_UI_PlayerHead.LoadPlayerIcon((int)headId, (int)headFrameID);
                itemView.m_lbl_name_LanguageText.text = name;


                itemView.m_UI_Captain1.gameObject.SetActive(mainHeroId > 0);

                string nameCp1 = String.Empty;
                string nameCp2 = String.Empty;

                if (mainHeroId > 0)
                {
                    nameCp1 = itemView.m_UI_Captain1.LoadHeadID(mainHeroId, mainHeroLevel);
                }

                itemView.m_UI_Captain2.gameObject.SetActive(deputyHeroId > 0);
                if (deputyHeroId > 2)
                {
                    nameCp2 = itemView.m_UI_Captain2.LoadHeadID(deputyHeroId, deputyHeroLevel);
                }

                //队伍信息
                itemView.m_lbl_armyCount_LanguageText.text =
                    LanguageUtils.getTextFormat(730309, ClientUtils.FormatComma(data.armyCount));

                itemView.m_lbl_name_LanguageText.text = name;

                itemView.m_lbl_captainName_LanguageText.text = deputyHeroId > 0
                    ? LanguageUtils.getTextFormat(180714, nameCp1, nameCp2)
                    : nameCp1;

                bool isBuilding = m_mapData.guildBuildStatus == (long) GuildBuildState.building;

                //状态
                itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
             
                itemView.m_pl_time_ArabLayoutCompment.gameObject.SetActive(
                    !TroopHelp.IsHaveState(status, ArmyStatus.REINFORCE_MARCH) && isBuilding);

                
                bool isMyCollect = isSelf &&
                                   TroopHelp.IsCollectGuildType(m_mapData.rssType) && m_mapData.guildBuildStatus>(int)GuildBuildState.building;

                itemView.m_lbl_curNum_LanguageText.text = "";
                itemView.m_img_cur_PolygonImage.gameObject.SetActive(isBuilding);
                
                //采集进度条
                itemView.m_pb_collect_ArabLayoutCompment.gameObject.SetActive(isMyCollect && isMarching==false);

                if (isMarching )
                {
                    if(TroopHelp.IsCollectGuildType(m_mapData.rssType))
                    {
                        itemView.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(200087, //采集行军中
                            ClientUtils.FormatTimeSplit(
                                (int) (remainArrivalTime)));
                    }
                    else
                    {
                        itemView.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(200088, //增援行军中
                            ClientUtils.FormatTimeSplit(
                                (int) (remainArrivalTime)));
                    }
                }
                else
                {
                    itemView.m_pl_time_ArabLayoutCompment.gameObject.SetActive(isBuilding && isSelf);

                    if (isMyCollect)
                    {
                        //计算自己采集量
                        Int64 collectNum = ArmyInfoHelp.Instance.GetArmyCollectNum((int) armyIndex);
                        float m_canCollectNum = ArmyInfoHelp.Instance.GetArmyWeight((int) armyIndex);
                    
                        //采集量
                        itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(500008,
                            ClientUtils.FormatComma(collectNum),
                            ClientUtils.FormatComma((long)m_canCollectNum));
                        itemView.m_pb_collect_GameSlider.value =(float)collectNum/m_canCollectNum;
                        itemView.m_lbl_state_LanguageText.text = "";
                    }else if(isBuilding)
                    {
                        //已到达 采集中
                        itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(181240);//已到达

                        
                        //建造积分
                        int time = (int) (ServerTimeModule.Instance.GetServerTime() - arrivalTime);//时间
                        itemView.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(732098,
                            ClientUtils.FormatTimeSplit(time));
                        float rate = m_buildInfo.allianceCoinReward / 3600;
                        itemView.m_lbl_curNum_LanguageText.text = ClientUtils.FormatComma(time>0?(long) (time * rate):0);
                        
                    }
                    else
                    {
                        if (m_mapData.rssType<= RssType.GuildFlag || TroopHelp.IsStrongHoldType(m_mapData.rssType))
                        {
                            itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732076);//已到达
                        }
                        else
                        {

                            if (!isMyCollect)
                            {
                                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732088);//采集中
                            }
                        }
                    }
                }


                //队伍返回
                itemView.m_btn_back_GameButton.gameObject.SetActive(
                    rid == m_playerProxy.CurrentRoleInfo.rid);
                itemView.m_btn_back_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_back_GameButton.onClick.AddListener(() =>
                {
                    //自己部队返回

                    m_troopProxy.TroopMapMarCh((int) armyIndex, TroopAttackType.Retreat, 0, null);
                });

                //领队
                itemView.m_btn_leader_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_leader_GameButton.onClick.AddListener(() =>
                {
                    HelpTip.CreateTip(4007, itemView.m_btn_leader_GameButton.transform)
                        .SetStyle(HelpTipData.Style.arrowDown).Show();
                });
                
                
                itemView.m_btn_leader_GameButton.gameObject.SetActive(m_allianceProxy.IsBuildArmsLeader(m_mapData.objectId,buildArmyIndex) && isMarching==false);

                //扩展
                itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);

                //加入队伍
                itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                {
                    //selected tag
                    data.isSelected = !data.isSelected;

                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);
                    if (data.isSelected)
                    {
                        AddMember(buildArmyIndex, index, data);
                    }
                    else
                    {
                        RemoveMember(buildArmyIndex, index);
                    }
                });
        }

        // 刷新该建筑所有军队士兵信息与数量
        void ReAllArmyInfoDict()
        {
            m_AllArmyInfoDict.Clear();

            for (int i = 0; i < m_armysList.Count; i++)
            {
                AllianceBuildArmyLevel army = m_armysList[i];    // 一个军队里的所有士兵信息
                Dictionary<long, SoldierInfo> soliderInfo = null;
                if (army.isHolyLand)
                {
                    if(army.holyLandArmyInfoEntity != null)
                    {
                        soliderInfo = army.holyLandArmyInfoEntity.soldiers;
                    }
                       
                }
                else
                {
                    if(army.LevelMember != null)
                    {
                        soliderInfo = army.LevelMember.soldiers;
                    }                  
                }
                if (soliderInfo == null) continue;
                foreach (var kv in soliderInfo)
                {
                    if (!m_AllArmyInfoDict.ContainsKey(kv.Value.id))
                    {
                        m_AllArmyInfoDict.Add(kv.Value.id, kv.Value.num);
                    }
                    else
                    {
                        m_AllArmyInfoDict[kv.Value.id] += kv.Value.num;
                    }
                }
            }
        }

        void SetCurViewTab(CurViewTab value)
        {
            bool isInfo = value == CurViewTab.Info;
            view.m_sv_all_ListView.gameObject.SetActive(isInfo);
            view.m_lbl_notroops_LanguageText.gameObject.SetActive(isInfo);
            view.m_UI_Model_Reinforce.gameObject.SetActive(!isInfo);
            m_curViewTab = value;
        }

        void ListItemByIndex_All(ListView.ListItem scrollItem)
        {
            UI_Item_WarMenberDetialView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
            UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[]
                {itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4};

            AllArmyInfo_Item allArmyInfoItem = scrollItem.data as AllArmyInfo_Item; 
            if (allArmyInfoItem == null)
            {
                allArmyInfoItem = new AllArmyInfo_Item();
                allArmyInfoItem.subViews = new List<UI_Item_SoldierHead_SubView>() { itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4 };
                scrollItem.data = allArmyInfoItem;
            }

            for (int i = 0; i < allArmyInfoItem.subViews.Count; i++)
            {
                allArmyInfoItem.subViews[i].gameObject.SetActive(false);
            }

            int index = 0;
            foreach (var dict in m_AllArmyInfoDict)
            {
                if (index >= allArmyInfoItem.subViews.Count)
                {
                    GameObject obj = CoreUtils.assetService.Instantiate(itemView.m_UI_head1.gameObject);
                    allArmyInfoItem.subViews.Add(new UI_Item_SoldierHead_SubView(obj.GetComponent<RectTransform>()));
                    obj.transform.SetParent(itemView.m_pl_addHere_GridLayoutGroup.transform);
                    obj.transform.localScale = Vector3.one;
                }

                UI_Item_SoldierHead_SubView subView = allArmyInfoItem.subViews[index];
                subView.gameObject.SetActive(true);
                subView.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int) dict.Key), (int) dict.Value);
                index++;
            }

            if (m_curViewTab == CurViewTab.Info)
            {
                view.m_lbl_notroops_LanguageText.gameObject.SetActive(m_AllArmyInfoDict.Count <= 0);
            }
            else
            {
                view.m_lbl_notroops_LanguageText.gameObject.SetActive(false);
            }
        }

        
        void SetBuildInfo()
        {
            RssType rssType = (RssType) m_mapData.objectType;

            switch (rssType)
            {
                case RssType.HolyLand:
                case RssType.CheckPoint:

                    StrongHoldDataDefine strongHoldDataDefine =
                        CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)m_mapData.strongHoldId);
                    StrongHoldTypeDefine strongHoldTypeDefine =
                        CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);

                    m_buildInfo.l_nameId = 730305;    // 增援部队容量
                    m_buildInfo.iconImg = strongHoldTypeDefine.iconImg;
                    m_buildInfo.armyCntLimit = strongHoldTypeDefine.armyCntLimit;
                    break;
                default:
                    
                    int m_buildType = m_allianceProxy.GetBuildServerTypeToConfigType(m_mapData.objectType);

                    AllianceBuildingTypeDefine m_buildTypeConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(m_buildType);

                    m_buildInfo.l_nameId = m_buildTypeConfig.l_nameId;
                    m_buildInfo.iconImg = m_buildTypeConfig.iconImg;
                    m_buildInfo.S = m_buildTypeConfig.S;
                    m_buildInfo.scienceReq = m_buildTypeConfig.scienceReq;
                    m_buildInfo.allianceCoinReward = m_buildTypeConfig.allianceCoinReward;
                    m_buildInfo.armyCntLimit = m_buildTypeConfig.armyCntLimit;
                    
                    break;
            }
            
            
        }
        
    }
}