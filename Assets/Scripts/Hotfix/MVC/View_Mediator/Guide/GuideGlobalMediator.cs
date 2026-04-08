// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    GuideGlobalMediator 新手引导
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections.Generic;
using PureMVC.Interfaces;
using Skyunion;
using System;
using Client;
using Data;
using UnityEngine.UI;
using SprotoType;
using DG.Tweening;
using Hotfix;

namespace Game
{
    public enum EnumGuideFindStatus
    {
        /// <summary>
        /// 不需要查找
        /// </summary>
        None = 0,
        /// <summary>
        /// 已找到
        /// </summary>
        Found = 1,
        /// <summary>
        /// 查找中
        /// </summary>
        Finding = 2,
        /// <summary>
        /// 剧情特殊表现
        /// </summary>
        DialogSpecial = 3,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 4,
    }

    public class GuideGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "GuideGlobalMediator";

        private int m_quickGuideStatus = 1; // 0使用配置 1强制引导 2强制不引导

        private int m_guideStatus = 0;

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private TaskProxy m_taskProxy;
        private GuideProxy m_guideProxy;
        private MonsterProxy m_monsterProxy;
        private ScoutProxy m_scoutProxy;
        private WorldMapObjectProxy m_worldProxy;
        private TroopProxy m_troopProxy;

        private Dictionary<int, List<int>> m_stageDataDic;

        private int m_findStatus;    //查找状态: 0不需要查找 1查找中 2已找到 3剧情
        private int m_findWay;      //查找方式
        private GuideDefine m_targetDefine;

        private int m_hudLayerId;

        private GameObject m_guideMaskLayer;

        private int m_specialStatus = 0;

        private bool m_isGuideEnd = false;

        private bool m_isDispose;

        private List<Int64> m_monsterIdList = new List<Int64>();

        private ConfigDefine m_configDefine;

        private Sequence m_sequence;

        private int m_initFindStage = -1;
        private bool m_isExecute = false;

        private bool m_isInit = false;

        private bool m_mapInitScaleIsEnd = false;
        private bool m_mainViewInitIsEnd = false;   //主界面是否加载完成

        private Int64 m_cityGuideObjectId;

        private bool m_isEnterNpcDialog;

        private int CreateScoutCampId;
        private int OpenTavernBoxId;
        private int UpgradeWallId;
        private int CreateTavernId;

        private bool m_fogIsInitFinish = false;

        #endregion

        public GuideGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        //IMediatorPlug needs
        public GuideGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.GuideTriggerCheck,
                CmdConstant.GuideInitData,
                CmdConstant.OnNPCDiaglogEnd,
                CmdConstant.GuideShow,
                CmdConstant.NextGuideStep,
                CmdConstant.CityBuildingLevelUP,
                CmdConstant.FirstExploreFog,
                Task_TaskFinish.TagName,
                CmdConstant.FirstGetHeroGuide,
                CmdConstant.GuideFoundMonster,
                CmdConstant.ForceCloseGuide,
                CmdConstant.MonsterFightEnd,
                CmdConstant.StartProcessGuide,
                CmdConstant.HideGuideMask,
                CmdConstant.MainViewFirstInitEnd,
                CmdConstant.TaskFinishToGuide,
                CmdConstant.FogSystemLoadEnd,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (m_isGuideEnd)
            {
                return;
            }           
            if (m_guideStatus >0 && !GuideProxy.IsOpenGuide)
            {
                return;
            }
            switch (notification.Name)
            {
                case CmdConstant.GuideInitData:
                    InitGuideData();
                    break;
                case CmdConstant.GuideTriggerCheck:
                    int stage = (int)notification.Body;
                    TriggerCheck(stage);
                    break;
                case CmdConstant.OnNPCDiaglogEnd:
                    if (notification.Body == null)
                    {
                        Debug.LogErrorFormat("CmdConstant.OnNPCDiaglogEnd 参数 不能为空");
                        return;
                    }
                    int dialogId = (int)notification.Body;
                    if (dialogId <= 0)
                    {
                        return;
                    }
                    if (m_targetDefine != null && m_targetDefine.guideDialog == dialogId)
                    {
                        NextGuideStep(m_targetDefine);
                    }
                    break;
                case CmdConstant.GuideShow:
                    CoreUtils.uiManager.ShowUI(UI.s_guideInfo, () => {
                        HideGuideMask();
                    }, notification.Body);
                    break;
                case CmdConstant.NextGuideStep:
                    NextGuideStep(notification.Body);
                    break;
                case CmdConstant.CityBuildingLevelUP://升级建筑
                    Debug.LogFormat("升级建筑: {0}", notification.Body);
                    BuildingCreateUp(notification.Body);
                    break;
                case Task_TaskFinish.TagName:
                    if (m_targetDefine == null)
                    {
                        return;
                    }
                    if (notification.Body == null)
                    {
                        return;
                    }
                    Task_TaskFinish.response taskResponse = notification.Body as Task_TaskFinish.response;
                    if (taskResponse == null)
                    {
                        return;
                    }
                    Debug.LogFormat("完成任务taskId: {0}", taskResponse.taskId);
                    if (taskResponse.taskId == 101003) //迷雾探险奖励
                    {
                        if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 9))
                        {
                            NextGuideStep(m_targetDefine, true);
                        }
                    }
                    break;
                case CmdConstant.FirstExploreFog: //第一次迷雾探险
                    if (m_targetDefine == null)
                    {
                        return;
                    }
                    GuideManager.Instance.IsExploreFogGuide = false;
                    if (m_targetDefine != null && m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 6))
                    {
                        if (m_specialStatus >= 3)
                        {
                            FogExploreEnd();
                        }
                    }
                    break;
                case CmdConstant.FirstGetHeroGuide: //获得首个统帅
                    if (m_targetDefine == null)
                    {
                        return;
                    }
                    if (IsFirstGetHeroGuide())
                    {
                        ShowFirstGetHeroDialog();
                    }
                    break;
                case CmdConstant.GuideFoundMonster: //搜索到野蛮人
                    if (m_targetDefine != null)
                    {
                        if(m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.AttackMonster, 1) ||
                           m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.AttackMonster, 8))
                        {
                            m_monsterIdList.Clear();
                            Int64 id = (Int64)notification.Body;
                            m_monsterIdList.Add(id);
                            //Debug.LogError("monster pos:" + pos1);
                            NextGuideStep(m_targetDefine, true);
                        }
                    }
                    break;
                case CmdConstant.ForceCloseGuide: //强制关闭引导
                    Debug.LogFormat("强制关闭引导");
                    WorldCamera.Instance().SetCanDrag(true);
                    WorldCamera.Instance().SetCanZoom(true);
                    CoreUtils.uiManager.CloseUI(UI.s_guideInfo);
                    m_isGuideEnd = true;
                    ClearData();
                    RecoverCityPos();
                    AppFacade.GetInstance().SendNotification(CmdConstant.SwitchDayNight, DayNightSwitch.AUTO);
                    break;
                case CmdConstant.MonsterFightEnd: //战斗结束通知
                    if (m_targetDefine != null)
                    {
                        if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.AttackMonster, 8))//第1次战斗结束
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.GuideSecondSearchMonster);
                        }
                        else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.AttackMonster, 12))//第2次战斗结束
                        {
                            NextGuideStep(m_targetDefine, true);
                        }
                    }
                    break;
                case CmdConstant.StartProcessGuide:
                    m_mapInitScaleIsEnd = true;
                    StartProcessGuide();
                    break;
                case CmdConstant.HideGuideMask:
                    if (m_isEnterNpcDialog)
                    {
                        m_isEnterNpcDialog = false;
                        HideGuideMask();
                    }
                    break;
                case CmdConstant.MainViewFirstInitEnd:
                    m_mainViewInitIsEnd = true;
                    StartProcessGuide();
                    break;
                case CmdConstant.TaskFinishToGuide:
                    long taskId = (long)notification.Body;
                    if (taskId == 101002 && m_specialStatus == 5) //建造斥候营地引导-领取奖励任务完成通知
                    {
                        Timer.Register(0.1f, () =>
                        {
                            if (m_isDispose)
                            {
                                return;
                            }
                            StartGuide((int)EnumNewbieGuide.ExploreFog, 8);
                        });
                    }
                    break;
                case CmdConstant.FogSystemLoadEnd: //迷雾初始化完成
                    m_fogIsInitFinish = true;
                    ForceOpenFog();

                    if (m_targetDefine != null && m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 6))
                    {
                        Debug.LogFormat("检测 重连后 迷雾探险是否完成 {0}：", m_specialStatus);
                        if (m_specialStatus >= 3)
                        {
                            TaskState taskStatus = m_taskProxy.GetTaskState(101003);
                            if (taskStatus == TaskState.finished)
                            {
                                FogExploreEnd();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            m_guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            m_monsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_hudLayerId = (int)UILayer.HUDLayer;

            CreateScoutCampId = m_guideProxy.GetId((int)EnumNewbieGuide.ExploreFog, 2);
            OpenTavernBoxId = m_guideProxy.GetId((int)EnumNewbieGuide.OpenTavernBox, 2);
            UpgradeWallId = m_guideProxy.GetId((int)EnumNewbieGuide.UpgradeWall, 2);
            CreateTavernId = m_guideProxy.GetId((int)EnumNewbieGuide.CreateTavern, 3);
        }

        public void ClearData()
        {
            GuideManager.Instance.ClearData();
            GuideProxy.IsLightingGuideControl = false;
            GuideProxy.IsOpenGuide = false;
            GuideProxy.IsGuideing = false;
            m_targetDefine = null;
            m_specialStatus = 0;
            m_findStatus = 0;
            RemoveGuideMask();
            KillSeq();
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        public override void OnRemove()
        {
            m_isDispose = true;
            CoreUtils.uiManager.CloseUI(UI.s_guideInfo);
            ClearData();
        }

        private void RemoveGuideMask()
        {
            if (m_guideMaskLayer != null)
            {
                GameObject.Destroy(m_guideMaskLayer);
                m_guideMaskLayer = null;
            }
        }

        public override void Update()
        {
            if (m_findStatus > 1)
            {
                FindTargetNode();
            }
        }

        public override void LateUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }

        #endregion

        //初始化下引导数据
        private void InitGuideData()
        {
            if (m_isInit)
            {
                return;
            }
            m_isInit = true;
            m_guideStatus = 1;
            if (CoreUtils.dataService.QueryRecord<ConfigDefine>(0).guideSwich == 1)
            {
                GuideProxy.IsOpenGuide = true;
            }
            else
            {
                GuideProxy.IsOpenGuide = false;
            }
            if (m_quickGuideStatus > 0)
            {
                if (m_quickGuideStatus == 1)
                {
                    GuideProxy.IsOpenGuide = true;
                }
                else
                {
                    GuideProxy.IsOpenGuide = false;
                }
            }

            m_targetDefine = null;
            m_configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0); 
            m_stageDataDic = m_guideProxy.GetStageDataDic();
            int findStage = m_guideProxy.GetNeedNextGuideStage(0);
            m_initFindStage = findStage;

            //判断引导过打怪 如果没有 则使用假的城市中心 方便引导
            if (!m_guideProxy.IsCompleteAttackMonsterGuide())
            {
                PosInfo pos = new PosInfo();
                pos.x = m_playerProxy.CurrentRoleInfo.pos.x;
                pos.y = m_playerProxy.CurrentRoleInfo.pos.y;
                m_guideProxy.SetCityPos(pos);
                Vector2 centerPos = m_guideProxy.GetGuideCityPos();
                m_playerProxy.ChangeCityPos((long)centerPos.x, (long)centerPos.y);

                if (m_cityBuildingProxy.MyCityObjData != null) //强制变更城市坐标
                {
                    Debug.Log("强制变更城市坐标");
                    m_cityBuildingProxy.MyCityObjData.pos = new Vector2(m_playerProxy.CurrentRoleInfo.pos.x / 100f, m_playerProxy.CurrentRoleInfo.pos.y / 100f);
                }
            }
            InitGuideOther();

            ForceOpenFog();
        }

        private void ForceOpenFog()
        {
            if (!m_isInit)
            {
                return;
            }
            if (!m_fogIsInitFinish)
            {
                return;
            }
            if (!m_guideProxy.IsCompleteAttackMonsterGuide())
            {
                Debug.Log("新手引导 强制开启迷雾");
                //打怪引导尚未结束 强制开启虚拟城市坐标对应区域的迷雾 引导结束后 再恢复迷雾
                List<Vector2Int> needList = new List<Vector2Int>();
                FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                var tilePos = fogMedia.Pos2Tile((float)m_playerProxy.CurrentRoleInfo.pos.x / 100, (float)m_playerProxy.CurrentRoleInfo.pos.y / 100);
                int radius = 3;
                Vector2 minPos = new Vector2(tilePos.x - radius, tilePos.y - radius);
                int count = radius * 2 + 1;
                for (int i = 0; i < count; i++)
                {
                    for (int k = 0; k < count; k++)
                    {
                        int x = (int)minPos.x + i;
                        int y = (int)minPos.y + k;
                        if (WarFogMgr.HasFogAt(x, y))
                        {
                            needList.Add(new Vector2Int(x, y));
                            WarFogMgr.OpenFog(x, y);
                        }
                    }
                }
                m_guideProxy.SetNeedHideFogList(needList);
            }
        }

        private void InitGuideOther()
        {
            RectTransform trans = CoreUtils.uiManager.GetUILayer((int)UILayer.GuideLayer);
            if (trans != null)
            {
                GameObject maskObj = new GameObject("GuideMaskLayer");
                maskObj.layer = LayerMask.NameToLayer("UI");
                maskObj.transform.SetParent(trans);
                maskObj.transform.SetAsFirstSibling();
                maskObj.AddComponent<Empty4Raycast>();
                m_guideMaskLayer = maskObj;
                maskObj.SetActive(false);
            }

            int findStage = m_initFindStage;

            if (!m_guideProxy.IsCompletedByStage(m_configDefine.daySwitchGuideA))
            {
                GuideProxy.IsLightingGuideControl = true;
                //保持白天
                SwitchDayNight((int)DayNightSwitch.DAY);
            }
            else if (!m_guideProxy.IsCompletedByStage(m_configDefine.nightSwitchGuideB))
            {
                GuideProxy.IsLightingGuideControl = true;
                //保持黄昏
                SwitchDayNight((int)DayNightSwitch.DUST);
            }
            else if (!m_guideProxy.IsCompletedByStage(m_configDefine.duskSwitchGuideC))
            {
                GuideProxy.IsLightingGuideControl = true;
                //保持夜晚
                SwitchDayNight((int)DayNightSwitch.NIGHT);
            }
            else if (findStage >= m_configDefine.normalSwitchGuideD)//D阶段
            {
                GuideProxy.IsLightingGuideControl = false;
            }

            if (findStage < 0)
            {
                return;
            }

            //添加虚拟的城堡数据
            if (findStage >= (int)EnumNewbieGuide.CreateFarm && findStage <= (int)EnumNewbieGuide.AttackMonster)
            {
                m_cityGuideObjectId = 999999999;
                WorldMapObjectProxy m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                MapObjectInfoEntity entity = new MapObjectInfoEntity();
                entity.objectId = m_cityGuideObjectId;
                entity.cityRid = m_playerProxy.CurrentRoleInfo.rid;
                entity.objectType = 3;
                entity.objectPos = new PosInfo();
                entity.objectPos.x = m_playerProxy.CurrentRoleInfo.pos.x;
                entity.objectPos.y = m_playerProxy.CurrentRoleInfo.pos.y;
                entity.cityName = m_playerProxy.CurrentRoleInfo.name;
                entity.cityLevel = m_playerProxy.CurrentRoleInfo.level;
                m_worldMapProxy.AddWorldObj(entity);
            }

            GuideProxy.IsGuideing = true;
            CoreUtils.uiManager.SetGuideStatus(true);

            Debug.LogFormat("find need guide stage:{0}", findStage);
            //Debug.LogErrorFormat("find need guide stage:{0}", findStage);

            //打开引导界面 默认只显示遮罩 防止玩家点击
            ShowGuideMask();

            WorldCamera.Instance().SetCanDrag(false);
            WorldCamera.Instance().SetCanZoom(false);

            if (m_mapInitScaleIsEnd) //地图是否缩放结束
            {
                StartProcessGuide();
            }
        }

        private void StartProcessGuide()
        {
            Debug.LogFormat("mapScaleEnd guide m_initFindStage:{0}", m_initFindStage);
            if (!m_isInit)
            {
                return;
            }
            if (!m_mapInitScaleIsEnd) //地图是否缩放结束
            {
                return;
            }
            if (!m_mainViewInitIsEnd)
            {
                return;
            }
            if (m_isExecute) //防止多次执行
            {
                return;
            }
            m_isExecute = true;
            if (m_initFindStage < 0)
            {
                return;
            }
            int step = GetStartStepByStage(m_initFindStage);
            StartGuide(m_initFindStage, step);
        }

        private void ShowGuideMask()
        {
            if (m_guideMaskLayer != null)
            {
                m_guideMaskLayer.gameObject.SetActive(true);
            }
            else
            {

            }
        }

        private void HideGuideMask()
        {
            if (m_guideMaskLayer != null)
            {
                m_guideMaskLayer.gameObject.SetActive(false);
            }
        }

        private void GuideClose()
        {
            Debug.Log("guide 新手引导结束");

            WorldCamera.Instance().SetCanDrag(true);
            WorldCamera.Instance().SetCanZoom(true);

            //关闭引导界面
            HideGuideView();

            //数据清空下
            GuideProxy.IsOpenGuide = false;
            GuideProxy.IsLightingGuideControl = false;
            GuideProxy.IsGuideing = false;
            m_findStatus = (int)EnumGuideFindStatus.None;
            m_targetDefine = null;

            //关闭切换白天黑夜序列
            KillSeq();

            AppFacade.GetInstance().SendNotification(CmdConstant.GuideFinished);
            CoreUtils.uiManager.SetGuideStatus(false);
        }

        private void ShowMaskAndCloseWin()
        {
            m_isEnterNpcDialog = true;
            ShowGuideMask();
            CloseGuideWin();
        }

        private void HideGuideView()
        {
            HideGuideMask();
            CloseGuideWin();
        }

        private void CloseGuideWin()
        {
            CoreUtils.uiManager.HideUI(UI.s_guideInfo);
        }

        private void DaiLightSwitch(int stage)
        {
            //白天黑夜处理
            if (stage == m_configDefine.daySwitchGuideA)
            {
                //Debug.LogError("白天->黄昏过渡状态->黄昏");
                //白天->黄昏过渡状态->黄昏
                SwitchDayNight((int)DayNightSwitch.DAY_TO_DUST);
            }
            else if (stage == m_configDefine.nightSwitchGuideB)
            {
                //Debug.LogError("黄昏->黄昏过渡->夜晚");
                //黄昏->黄昏过渡->夜晚
                SwitchDayNight((int)DayNightSwitch.DUST_NIGHT);
            }
            else if (stage == m_configDefine.duskSwitchGuideC)
            {
                //Debug.LogError("夜晚->白天的中间过渡状态->白天");
                //夜晚->白天的中间过渡状态->白天
                //SwitchDayNight((int)DayNightSwitch.DAWN_TO_DAY);

                KillSeq();
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    SwitchDayNight((int)DayNightSwitch.AUTO);
                });
                seq.AppendInterval(4);
                seq.AppendCallback(() =>
                {
                    SwitchDayNight((int)DayNightSwitch.DAWN_TO_DAY);
                });
                m_sequence = seq;
            }
            else if (stage == m_configDefine.normalSwitchGuideD)
            {
                KillSeq();
                GameEventGlobalMediator tMediator = AppFacade.GetInstance().RetrieveMediator(GameEventGlobalMediator.NameMediator) as GameEventGlobalMediator;
                if (tMediator != null)
                {
                    int index = tMediator.GetCurrLightIndex();

                    if (index == (int)DayNightSwitch.DAY || index == (int)DayNightSwitch.DAWN_TO_DAY)
                    {
                        //当前是白天
                        Debug.Log("新手引导昼夜替换 当前是白天");
                    }
                    else if (index == (int)DayNightSwitch.DUST || index == (int)DayNightSwitch.DAY_TO_DUST)
                    {
                        //当前是黄昏
                        SwitchDayNight((int)DayNightSwitch.DAY_TO_DUST);
                        Debug.Log("新手引导昼夜替换 当前是黄昏");
                    }
                    else if (index == (int)DayNightSwitch.NIGHT || index == (int)DayNightSwitch.DUST_NIGHT)
                    {
                        //当前是夜晚
                        Sequence seq = DOTween.Sequence();
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.DAY_TO_DUST);
                        });
                        seq.AppendInterval(2);
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.DUST_NIGHT);
                        });
                        m_sequence = seq;
                        Debug.Log("新手引导昼夜替换 当前是夜晚");
                    }
                    else if (index == (int)DayNightSwitch.DAWN)
                    {
                        //当前是黎明
                        Sequence seq = DOTween.Sequence();
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.DAY_TO_DUST);
                        });
                        seq.AppendInterval(2);
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.DUST_NIGHT);
                        });
                        seq.AppendInterval(2);
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.AUTO); //从夜晚切换到黎明
                        });
                        m_sequence = seq;
                        Debug.Log("新手引导昼夜替换 当前是黎明");
                    }
                    else if (index == (int)DayNightSwitch.AUTO)
                    {
                        //AUTO 从夜晚切换到黎明阶段
                        Sequence seq = DOTween.Sequence();
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.DAY_TO_DUST);
                        });
                        seq.AppendInterval(2);
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.DUST_NIGHT);
                        });
                        seq.AppendInterval(2);
                        seq.AppendCallback(() =>
                        {
                            SwitchDayNight((int)DayNightSwitch.AUTO); 
                        });
                        m_sequence = seq;
                        Debug.Log("新手引导昼夜替换 当前AUTO");
                    }
                }
            }
            else if (stage > m_configDefine.normalSwitchGuideD)
            {
                KillSeq();
            }
        }

        private void KillSeq()
        {
            if (m_sequence != null)
            {
                m_sequence.Kill();
                m_sequence = null;
            }
        }

        private void SwitchDayNight(int type)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.SwitchDayNight, type);
        }

        //开始引导
        private void StartGuide(int stage, int step)
        {
            if (!m_stageDataDic.ContainsKey(stage))
            {
                Debug.LogErrorFormat("异常 新手stage:{0}", stage);
                return;
            }
            m_specialStatus = 0;
            if (step >= m_stageDataDic[stage].Count) //阶段引导结束
            {
                m_guideProxy.RequestRecordGuideStage(stage, step);

                Debug.LogFormat("阶段引导结束:{0}", stage);

                //白天黑夜切换
                DaiLightSwitch(stage);

                //判断是否进入下一个阶段引导
                int findStage = m_guideProxy.GetNeedNextGuideStage(stage);
                if (findStage < 0)
                {
                    GuideClose();
                    return;
                }
                Debug.LogFormat("开始新的阶段:{0}", findStage);
                //开始新的阶段引导
                StartGuide(findStage, 0);
                return;
            }
            m_targetDefine = CoreUtils.dataService.QueryRecord<GuideDefine>(m_stageDataDic[stage][step]);
            Debug.LogFormat("当前引导id：{0}", m_targetDefine.ID);
            AppFacade.GetInstance().SendNotification(CmdConstant.OnGuideMainInterfaceModule, m_targetDefine);

            if (m_targetDefine.guideDialog > 0) //通知播放剧情
            {
                if (IsFirstGetHeroGuide())//首次获得英雄
                {
                    ShowGuideMask();
                    CloseGuideWin();
                    m_findStatus = (int)EnumGuideFindStatus.DialogSpecial;
                    m_specialStatus = 1;
                    return;
                }
                else if (IsFogAdventure())//迷雾探险剧情
                {
                    ShowGuideMask();
                    CloseGuideWin();
                    m_findStatus = (int)EnumGuideFindStatus.DialogSpecial;
                    m_specialStatus = 1;
                    return;
                }

                m_findStatus = (int)EnumGuideFindStatus.None;

                ShowMaskAndCloseWin();
                AppFacade.GetInstance().SendNotification(CmdConstant.ShowNPCDiaglog, m_targetDefine.guideDialog);

            }
            else
            {
                ShowGuideMask();

                if(m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.UpgradeCity, 1) ||
                   m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateScoutCamp, 1) ||
                   m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 1) ||
                   m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ChapterTaskReward, 1))
                {
                    //判断一下是否在城外 如果在城内则跳过回城引导表现
                    if (IsInCity())
                    {
                        Debug.Log("在城内--");
                        if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 1))
                        {
                            if (WarFogMgr.IsAllFogOpen())//迷雾已全部打开
                            {
                                if (CoreUtils.uiManager.ExistUI(UI.s_Taskinfo)) //已打开任务面板
                                {
                                    m_findStatus = (int)EnumGuideFindStatus.Other;
                                    m_specialStatus = 5;
                                    return;
                                }
                                StartGuide(stage, step + 7);
                                return;
                            }
                        }
                        //在城内 跳过引导
                        StartGuide(stage, step + 1);
                        return;
                    }
                    else
                    {
                        if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ChapterTaskReward, 1))
                        {
                            if (CoreUtils.uiManager.ExistUI(UI.s_Taskinfo)) //已打开任务面板
                            {
                                StartGuide(stage, step + 1);
                                return;
                            }
                        }
                    }
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 2))//探索迷雾
                {
                    m_specialStatus = 1;
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.UpgradeWall, 2))//城墙升级
                {
                    m_specialStatus = 1;
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateScoutCamp, 2))//建造斥候营地
                {
                    if (CoreUtils.uiManager.ExistUI(UI.s_Taskinfo)) //已打开任务面板
                    {
                        StartGuide(stage, step + 1);
                        return;
                    }
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ChapterTaskReward, 2))
                {
                    if (!CoreUtils.uiManager.ExistUI(UI.s_Taskinfo)) //打开任务面板
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Taskinfo);
                    }
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.AttackMonster, 1)) //攻击野蛮人
                {
                    if (IsInCity()) //在城内
                    {

                    }
                    else//在城外
                    {
                        ShowGuideMask();
                        CloseGuideWin();
                        m_findStatus = (int)EnumGuideFindStatus.Other;
                        m_specialStatus = 1;
                        return;
                    }
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateTavern, 1)) //建造酒馆
                {
                    if (IsInCity()) //在城内
                    {
                        //在城内 跳过引导
                        StartGuide(stage, step + 1);
                        return;
                    }
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateTavern, 3)) //建造酒馆 建造按钮
                {
                    //判断一下是否需要移动到中心
                    m_specialStatus = 1;
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.OpenTavernBox, 1))
                {
                    if (IsInCity()) //在城内
                    {
                        //在城内 跳过引导
                        StartGuide(stage, step + 1);
                        return;
                    }
                }
                else if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.OpenTavernBox, 2))
                {
                    m_specialStatus = 1;
                }

                m_findStatus = (int)EnumGuideFindStatus.Finding;
                m_findWay = m_targetDefine.findWay;
            }
        }

        private bool IsCreateTavern(int id)
        {
            if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateTavern, 6))
            {
                return true;
            }
            return false;
        }

        private bool IsWallUpGuide(int id)
        {
            if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.UpgradeWall, 4))
            {
                return true;
            }
            return false;
        }

        private bool IsTownHallUpGuide(int id)
        {
            if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.UpgradeCity, 6))
            {
                return true;
            }
            return false;
        }

        private bool IsScoutCampCreateGuide(int id)
        {
            if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateScoutCamp, 8))
            {
                return true;
            }
            return false;
        }

        private bool IsFirstGetHeroGuide()
        {
            if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.CreateTavern, 1))
            {
                return true;
            }
            return false;
        }

        //是否是迷雾探险
        private bool IsFogAdventure()
        {
            if (m_guideProxy.IdEqual(m_targetDefine.ID, (int)EnumNewbieGuide.ExploreFog, 6))
            {
                return true;
            }
            return false;
        }

        //引导触发检查
        private void TriggerCheck(int stage)
        {
            //if (!m_isOpenGuide)
            //{
            //    return;
            //}

            //if (m_guideProxy.IsCompletedByStage(stage))
            //{
            //    return;
            //}

        } 

        //获取当前阶段引导的起始步骤
        private int GetStartStepByStage(int stage)
        {
            int step = 0;
            switch (stage)
            {
                case (int)EnumNewbieGuide.CreateFarm:
                    BuildingInfoEntity buildingInfoEntity2 = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.Farm));
                    if (buildingInfoEntity2 != null)
                    {
                        return 5;
                    }
                    break;
                case (int)EnumNewbieGuide.TrainSoldier: //训练士兵
                    //判断一下是否正在训练 或者 已训练完成
                    Dictionary<Int64, QueueInfo> trainQueue = m_playerProxy.GetTrainQueue();
                    if (trainQueue != null)
                    {
                        foreach (var data in trainQueue)
                        {
                            if (data.Value.armyNum > 0)
                            {
                                step = 4;
                                break;
                            }
                        }
                    }
                    break;
                case (int)EnumNewbieGuide.UpgradeCity: //升级市政大厅
                    if (m_playerProxy.GetTownHall() >= 2)
                    {
                        if (m_taskProxy.GetTaskState(101001) != TaskState.received)//判断是否领取奖励
                        {
                            step = 6;
                        }
                    }
                    else
                    {
                        if (!IsInCity())//正在城外
                        {
                            Debug.Log("在城外");
                            step = 0;
                        }
                        else
                        {
                            //判断一下任务面板是否已打开
                            if (CoreUtils.uiManager.ExistUI(UI.s_Taskinfo))
                            {
                                step = 2;
                            }
                            else
                            {
                                step = 1;
                            }
                        }
                    }
                    break;
                case (int)EnumNewbieGuide.CreateScoutCamp://建造斥候营地
                    BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.ScoutCamp));
                    if (buildingInfoEntity != null)//判断是否创建营地
                    {
                        if (m_taskProxy.GetTaskState(101002) != TaskState.received)//判断是否领取奖励
                        {
                            step = 8;
                        }
                    }
                    break;
                case (int)EnumNewbieGuide.ExploreFog://迷雾探险
                    TaskState taskStatus = m_taskProxy.GetTaskState(101003);
                    if (taskStatus == TaskState.finished)
                    {
                        step = 7;
                    }
                    else
                    {
                        Debug.LogFormat("guide 迷雾引导 异常任务状态:{0}", taskStatus);
                    }
                    break;
                case (int)EnumNewbieGuide.ChapterTaskReward: //领取章节奖励
                    if (!IsInCity()) //在城外
                    {
                        step = 0;
                    }
                    else
                    {
                        step = 1;
                    }
                    break;
                default:
                    break;
            }
            return step;
        }



        private void SpecialStatusProcess()
        {
            if (m_targetDefine.ID == CreateScoutCampId)//强制打开斥候营地菜单
            {
                if (m_specialStatus == 1)
                {
                    BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.ScoutCamp);
                    if (buildingInfo == null)
                    {
                        return;
                    }
                    GameObject obj = CityObjData.GetMenuTargetGameObject(buildingInfo.buildingIndex);
                    if (obj == null)
                    {
                        return;
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenu, buildingInfo);
                    m_specialStatus = 0;
                }
            }
            else if (m_targetDefine.ID == OpenTavernBoxId) //强制打开酒馆菜单
            {
                if (m_specialStatus == 1)
                {
                    BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Tavern);
                    if (buildingInfo == null)
                    {
                        return;
                    }
                    GameObject obj = CityObjData.GetMenuTargetGameObject(buildingInfo.buildingIndex);
                    if (obj == null)
                    {
                        return;
                    }

                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenu, buildingInfo);
                    m_specialStatus = 0;
                }
            }
            else if (m_targetDefine.ID == UpgradeWallId)//城墙升级
            {
                if (m_specialStatus == 1)
                {
                    BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.CityWall);
                    if (buildingInfo == null)
                    {
                        return;
                    }
                    GameObject obj  = CityObjData.GetMenuTargetGameObject(buildingInfo.buildingIndex);
                    if (obj == null)
                    {
                        return;
                    }
                    Transform findObj = obj.transform;
                    m_specialStatus = 2;
                    //将城墙移动到中间
                    WorldCamera.Instance().ViewTerrainPos(findObj.position.x, findObj.position.z, 500, () =>
                    {
                        m_specialStatus = 0;
                    });
                }
            }
            else if (m_targetDefine.ID == CreateTavernId)//建造酒馆
            {
                if (m_specialStatus == 1)
                {
                    //将地图到中间
                    WorldCamera.Instance().ViewTerrainPos(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y, 500, () =>
                    {
                        m_specialStatus = 0;
                    });
                }
            }
            else if (IsFogAdventure())//迷雾探险
            {
                if (m_specialStatus == 1)
                {
                    m_specialStatus = 2;
                    WorldCamera.Instance().ViewTerrainPos(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y, 500, () =>
                    {
                        m_specialStatus = 3;
                    });
                }
                else if (m_specialStatus == 3)
                {
                    Debug.Log("跟随镜头");
                    ScoutProxy.ScoutInfo info = m_scoutProxy.GetScoutInfo(0);
                    if (info != null && info.state == ScoutProxy.ScoutState.Fog)
                    {
                        Hotfix.ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId(info.id);
                        if (armyData != null)
                        {
                            Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                            if (formation != null)
                            {
                                m_specialStatus = 4;
                                TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
                                touchSelectScout.id = info.id;
                                touchSelectScout.isAutoMove = false;
                                touchSelectScout.isShowSelectHud = false;
                                AppFacade.GetInstance().SendNotification(CmdConstant.TouchScoutSelect, touchSelectScout);
                            }
                        }
                    }
                }
            }
        }

        //迷雾探险结束
        private void FogExploreEnd()
        {
            m_specialStatus = 0;
            //显示剧情
            m_findStatus = (int)EnumGuideFindStatus.None;
            ShowMaskAndCloseWin();
            AppFacade.GetInstance().SendNotification(CmdConstant.ShowNPCDiaglog, m_targetDefine.guideDialog);
        }

        //显示首次获得英雄引导
        private void ShowFirstGetHeroDialog()
        {
            m_findStatus = (int)EnumGuideFindStatus.None;
            m_specialStatus = 0;
            ShowMaskAndCloseWin();
            AppFacade.GetInstance().SendNotification(CmdConstant.ShowNPCDiaglog, m_targetDefine.guideDialog);
        }

        //查找目标节点
        public void FindTargetNode()
        {
            if (m_targetDefine == null)
            {
                return;
            }
            if (m_specialStatus > 0)
            {
                SpecialStatusProcess();
                return;
            }
            if (m_findWay == 1) //查找ShowUI界面上的节点
            {
                if (CoreUtils.uiManager.ExistUI(m_targetDefine.uiId))
                {
                    UIInfo info = CoreUtils.uiManager.GetUI(m_targetDefine.uiId);
                    if (info.uiObj == null)
                    {
                        return;
                    }
                    FindUiNode(info.uiObj.transform);
                }
            }
            else if (m_findWay == 2) //查找HUDLayer下面的节点
            {
                RectTransform trans = CoreUtils.uiManager.GetUILayer(m_hudLayerId);
                if (trans == null)
                {
                    return;
                }
                FindHudUiNode(trans);
            }
            else if (m_findWay == 3) //查找城市建筑
            {
                BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByType(m_targetDefine.guideNodeType);
                if (info == null)
                {
                    return;
                }
                GameObject go;
                if (!m_cityBuildingProxy.BuildObjDic.TryGetValue(info.buildingIndex, out go))
                {
                    return;
                }
                if (!string.IsNullOrEmpty(m_targetDefine.path))
                {
                    Transform trans = go.transform.Find(m_targetDefine.path);
                    if (trans != null)
                    {
                        FindCityBuildingNode(trans.gameObject);
                    }
                }
                else
                {
                    FindCityBuildingNode(go);
                }
            }
            else if (m_findWay == 4)//地图上野蛮人
            {
                FindMapPoint();
            }
            else
            {
                Debug.LogErrorFormat("findWay 未知类型:{0} ID:{1} 跳过 findStatus:{2}", m_targetDefine.findWay, m_targetDefine.ID, m_findStatus);
                StartGuide(m_targetDefine.stage, m_targetDefine.step);
            }
        }

        private void FindHudUiNode(Transform parent)
        {
            //获取父节点
            Transform trans1 = parent.Find(m_targetDefine.path);
            if (trans1 == null)
            {
                return;
            }
            Transform trans2 = parent.Find(m_targetDefine.guideNode);
            if (trans2 == null)
            {
                return;
            }
            if (!trans2.gameObject.activeInHierarchy)
            {
                return;
            }
            //获取父节点的缩放比率
            Vector3 localScale = trans1.localScale;
            if (m_targetDefine.guideNodeType == 1) //按钮
            {
                m_findStatus = (int)EnumGuideFindStatus.Found;
                GuideTargetParam param = new GuideTargetParam();
                param.AreaTarget = trans2.gameObject;
                param.DefineData = m_targetDefine;
                param.ScaleStatus = 2;
                param.ScaleParentTrans = trans1;
                AppFacade.GetInstance().SendNotification(CmdConstant.GuideShow, param);
            }
        }

        private void FindUiNode(Transform parent)
        {
            Transform trans1 = parent.Find(m_targetDefine.path);
            if (trans1 == null)
            {
                return;
            }
            if (!trans1.gameObject.activeInHierarchy)
            {
                return;
            }
            if (m_targetDefine.uiType == 1) //按钮
            {
                m_findStatus = (int)EnumGuideFindStatus.Found;
                GuideTargetParam param = new GuideTargetParam();
                param.AreaTarget = trans1.gameObject;
                param.DefineData = m_targetDefine;
                AppFacade.GetInstance().SendNotification(CmdConstant.GuideShow, param);
            }
            else //list
            {
                ListView listView = trans1.gameObject.GetComponent<ListView>();
                if (listView == null)
                {
                    return;
                }
                ListView.ListItem list_item = listView.GetItemByIndex(m_targetDefine.listIndex);
                if (list_item == null)
                {
                    return;
                }
                if (list_item.go == null)
                {
                    return;
                }
                Transform node1 = list_item.go.transform.Find(m_targetDefine.guideNode);
                if (node1 == null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(m_targetDefine.guideEffectPos))
                {
                    Transform node2 = list_item.go.transform.Find(m_targetDefine.guideEffectPos);
                    if (node2 == null)
                    {
                        return;
                    }
                    m_findStatus = (int)EnumGuideFindStatus.Found;
                    GuideTargetParam param = new GuideTargetParam();
                    param.AreaTarget = node1.gameObject;
                    param.EffectMountTarget = node2.gameObject;
                    param.DefineData = m_targetDefine;
                    param.ListNode = trans1.gameObject;
                    AppFacade.GetInstance().SendNotification(CmdConstant.GuideShow, param);
                }
                else
                {
                    m_findStatus = (int)EnumGuideFindStatus.Found;
                    GuideTargetParam param = new GuideTargetParam();
                    param.AreaTarget = node1.gameObject;
                    param.EffectMountTarget = node1.gameObject;
                    param.DefineData = m_targetDefine;
                    param.ListNode = trans1.gameObject;
                    AppFacade.GetInstance().SendNotification(CmdConstant.GuideShow, param);
                }
            }
        }

        private void FindCityBuildingNode(GameObject targetNode)
        {
            m_findStatus = (int)EnumGuideFindStatus.Found;
            GuideTargetParam param = new GuideTargetParam();
            param.AreaTarget = targetNode;
            param.DefineData = m_targetDefine;
            AppFacade.GetInstance().SendNotification(CmdConstant.GuideShow, param);
        }

        private void FindMapPoint()
        {
            if (m_monsterIdList.Count > 0)
            {
                MapObjectInfoEntity data = m_worldProxy.GetWorldMapObjectByobjectId(m_monsterIdList[0]);
                if (data!=null && data.gameobject != null)
                {
                    m_findStatus = (int)EnumGuideFindStatus.Found;
                    GuideTargetParam param = new GuideTargetParam();
                    param.DefineData = m_targetDefine;
                    param.AreaTarget = data.gameobject;
                    AppFacade.GetInstance().SendNotification(CmdConstant.GuideShow, param);
                }
                else
                {
                    Debug.LogError("not find monster");
                }
            }
            else
            {
                Debug.LogError("list is 0");
            }
        }

        //是否在城内
        private bool IsInCity()
        {
            GlobalViewLevelMediator globalViewLevel = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
            if (globalViewLevel != null)
            {
                return globalViewLevel.IsInCity();
            }
            return false;
        }

        //建筑创建升级引导
        private void BuildingCreateUp(object body)
        {
            if (body == null)
            {
                return;
            }
            if (m_targetDefine == null)
            {
                return;
            }
            Int64 buildingIndex = (Int64)body;
            BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
            if (buildingInfo == null)
            {
                return;
            }
            if (m_targetDefine.stage == (int)EnumNewbieGuide.CreateFarm) //资源收集引导
            {
                if (buildingInfo.type != (int)EnumCityBuildingType.Farm)
                {
                    return;
                }
                Debug.LogFormat("农场创建 level:{0}", buildingInfo.level);
                if (buildingInfo.level <= 1)
                {
                    //第1阶段 资源收集引导
                    AppFacade.GetInstance().SendNotification(CmdConstant.GuideForceShowResCollect, buildingInfo);
                }
            }
            else if (m_targetDefine.stage == (int)EnumNewbieGuide.UpgradeWall)//升级城墙
            {
                if (IsWallUpGuide(m_targetDefine.ID))
                {
                    NextGuideStep(m_targetDefine, true);
                }
            }
            else if (m_targetDefine.stage == (int)EnumNewbieGuide.CreateScoutCamp) //斥候营地创建成功引导
            {
                if (buildingInfo.type != (int)EnumCityBuildingType.ScoutCamp)
                {
                    return;
                }
                Debug.LogFormat("斥候营地创建 level:{0} Id:{1}", buildingInfo.level, m_targetDefine.ID);
                if (IsScoutCampCreateGuide(m_targetDefine.ID))
                {
                    NextGuideStep(m_targetDefine, true);
                }
            }
            else if (m_targetDefine.stage == (int)EnumNewbieGuide.UpgradeCity) //市政大厅升级2级
            {
                Debug.LogFormat("市政大厅升级 level:{0} Id:{1}", buildingInfo.level, m_targetDefine.ID);
                if (IsTownHallUpGuide(m_targetDefine.ID))
                {
                    NextGuideStep(m_targetDefine, true);
                }
            }
            else if (m_targetDefine.stage == (int)EnumNewbieGuide.CreateTavern)//创建酒馆
            {
                if (buildingInfo.type != (int)EnumCityBuildingType.Tavern)
                {
                    return;
                }
                if (IsCreateTavern(m_targetDefine.ID))
                {
                    NextGuideStep(m_targetDefine, true);
                }
            }
           
        }

        //下一步骤引导
        private void NextGuideStep(object body, bool isJump = false)
        {
            GuideDefine obj = body as GuideDefine;
            if (obj == null)
            {
                return;
            }
            if (!(m_stageDataDic.ContainsKey(obj.stage)))
            {
                return;
            }
            int index = m_stageDataDic[obj.stage].FindIndex(item => item.Equals(obj.ID));
            if (index < 0)
            {
                Debug.LogErrorFormat("异常数据 stage:{0}", obj.stage);
                HideGuideView();
                return;
            }
            if (!isJump)
            {
                if (IsWallUpGuide(obj.ID)) //等待城墙升级通知
                {
                    return;
                }
                else if (IsCreateTavern(obj.ID))//等待酒馆创建成功通知
                {
                    return;
                }
                else if (IsTownHallUpGuide(obj.ID)) //等待市政厅升级通知
                {
                    return;
                }
                else if (IsScoutCampCreateGuide(obj.ID)) //等待斥候营地升级通知
                {
                    return;
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.ExploreFog, 9)) //等待迷雾探险奖励领取成功通知
                {
                    return;
                }
                else if (IsFirstGetHeroGuide()) //打开获得英雄界面
                {
                    //打开英雄界面
                    var m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                    Int64 civilization = m_playerProxy.GetCivilization();
                    CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)civilization);
                    CoreUtils.uiManager.ShowUI(UI.s_captainSummon, null, (Int64)define.initialHero);
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.AttackMonster, 1))//第一次搜索野蛮人
                {
                    GuideManager.Instance.IsGuideFightBarbarian = true;
                    Timer.Register(1f, () =>
                    {
                        if (m_isDispose)
                        {
                            return;
                        }
                        //搜索野蛮人
                        AppFacade.GetInstance().SendNotification(CmdConstant.GuideFindMonster);
                    });
                    return;
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.AttackMonster, 2))//第二次搜索野蛮人
                {

                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.AttackMonster, 8)) //等待第1次战斗结束通知
                {
                    return;
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.AttackMonster, 12)) //等待第2次战斗结束通知
                {
                    return;
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.AttackMonster, 13))//第二次打怪结束
                {
                    GuideManager.Instance.IsGuideFightSecondBarbarian = false;

                    //移除虚拟数据和节点
                    GlobalViewLevelMediator globalViewLevel = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
                    globalViewLevel.RemoveMapObjLodAndHud(999999999);

                    WorldMapObjectProxy m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                    m_worldMapProxy.DelMapObject(m_cityGuideObjectId);
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.GetSecondHero, 1))//获得第二统帅
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.GuideArmyReturn);
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.ExploreFog, 2)) //迷雾探险开始
                {
                    GuideManager.Instance.IsExploreFogGuide = true;
                }
                else if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.UpgradeCity, 8))
                {
                    //角色主城坐标位置恢复
                    RecoverCityPos();
                } 
            }
            else
            {
                if (m_guideProxy.IdEqual(obj.ID, (int)EnumNewbieGuide.AttackMonster, 8))//第一次打怪结束
                {
                    GuideManager.Instance.IsGuideFightBarbarian = false;
                    GuideManager.Instance.IsGuideFightSecondBarbarian = true;
                }
            }

            m_guideProxy.RequestRecordGuideStep(obj.stage, obj.step, obj.ID);
            index = index + 1;
            StartGuide(obj.stage, index);
            SendClientDeviceInfoMedia.ReportData();
        }

        //恢复城市坐标
        private void RecoverCityPos()
        {
            if (m_guideProxy.IsChangeCityPos())
            {
                Debug.Log("guide 恢复主城坐标");
                m_playerProxy.RecoverCityPos(m_guideProxy.GetCityPos());
                m_guideProxy.SetCityPosStatus(false);
                AppFacade.GetInstance().SendNotification(CmdConstant.ChangeRolePosGuide);
                //恢复迷雾显示
                List<Vector2Int> vecList = m_guideProxy.GetNeedHideFogList();
                if (vecList != null)
                {
                    for (int i = 0; i < vecList.Count; i++)
                    {
                        WarFogMgr.CloseFog(vecList[i].x, vecList[i].y);
                    }
                }
            }
        }
    }
}