// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, December 27, 2019
// Update Time         :    Friday, December 27, 2019
// Class Description   :    ServerPushMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using SprotoType;
using Skyunion;
using Data;
using Client;
using System;
using System.Linq;

namespace Game
{
    public class GOScrptGuide
    {
        public EnumBuildingGroupType buildingGroupkType;
        public EnumTaskType taskType;
        public int param1;
        public int param2;
        public string menuItemName;
        public GOScrptGuide(EnumTaskType taskType, int param1, int param2, string menuItemName)
        {
            this.taskType = taskType;
            this.param1 = param1;
            this.param2 = param2;
            this.menuItemName = menuItemName;

        }
        public GOScrptGuide(EnumTaskType taskType, int param1, int param2)
        {
            this.taskType = taskType;
            this.param1 = param1;
            this.param2 = param2;
        }
        public GOScrptGuide(EnumBuildingGroupType buildingGroupkType)
        {
            this.buildingGroupkType = buildingGroupkType;
            
        }
        public GOScrptGuide(int buildingtype)
        {
            this.param1 = buildingtype;
        }
        public GOScrptGuide(EnumTaskType taskType, int param1)
        {
            this.taskType = taskType;
            this.param1 = param1;
        }
        public GOScrptGuide(EnumTaskType taskType)
        {
            this.taskType = taskType;
        }
    }
    public class TaskScriptGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "TaskScriptGlobalMediator";


        private PlayerProxy m_playerProxy;
        private NetProxy m_netProxy;
        private CityBuildingProxy m_buildingProxy;
        private TaskProxy m_taskProxy;
        private CurrencyProxy m_currencyProxy;
        private AllianceProxy m_allianceProxy;
        private ResearchProxy m_researchProxy;
        #endregion

        //IMediatorPlug needs
        public TaskScriptGlobalMediator() : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                    CmdConstant.GoScript,
                    CmdConstant.CityBuildingLevelUP,
                    Role_RoleLogin.TagName,
                    CmdConstant.UpdateTaskStatistics,
                    CmdConstant.technologyChange,
                    CmdConstant.FinishSideTasks,
                    CmdConstant. TaskStatisticsSum ,
                    CmdConstant.MainLineTaskId ,
                    CmdConstant.ChapterTasks ,
                    CmdConstant.ChapterId ,
                    CmdConstant.UpdatePlayerHistoryPower,
                    CmdConstant.InSoldierInfoChange,
                    Task_TaskInfo.TagName,
                    CmdConstant.TaskStateChange,
                     CmdConstant.TipRewardGroup,
                    CmdConstant.BuildingMenuJump,
                    CmdConstant.denseFogOpenFlag,
                    Hero_HeroInfo.TagName,
                    CmdConstant.ExpeditionInfoChange,
        }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.GoScript:
                    {
                        if (notification.Body is TaskData)
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_Taskinfo);
                            TaskData taskData = notification.Body as TaskData;
                            int type = 0, p1 = 0, p2 = 0, r1 = 0;
                            switch (taskData.taskPageType)
                            {
                                case EnumTaskPageType.TaskChapter:
                                    {
                                        type = taskData.taskChapterDefine.type;
                                        p1 = taskData.taskChapterDefine.param1;
                                        p2 = taskData.taskChapterDefine.param2;
                                        r1 = taskData.taskChapterDefine.require;
                                    }
                                    break;
                                case EnumTaskPageType.TaskDaily:
                                    {
                                        type = taskData.taskDailyDefine.type;
                                        p1 = taskData.taskDailyDefine.param1;
                                        p2 = taskData.taskDailyDefine.param2;
                                        r1 = taskData.taskDailyDefine.require;
                                    }
                                    break;
                                case EnumTaskPageType.TaskMain:
                                    {
                                        type = taskData.taskMainDefine.type;
                                        p1 = taskData.taskMainDefine.param1;
                                        p2 = taskData.taskMainDefine.param2;
                                        r1 = taskData.taskMainDefine.require;
                                    }
                                    break;
                                case EnumTaskPageType.TaskSide:
                                    {
                                        type = taskData.taskSideDefine.type;
                                        p1 = taskData.taskSideDefine.param1;
                                        p2 = taskData.taskSideDefine.param2;
                                        r1 = taskData.taskSideDefine.require;
                                    }
                                    break;
                            }

                            GoScript(type, p1, p2, r1);
                        }
                    }
                    break;
                case Task_TaskInfo.TagName:
                    {
                            Task_TaskInfo.request TaskInfos = notification.Body as Task_TaskInfo.request;
                        if (TaskInfos != null)
                        {
                            TaskInfos.taskInfo.Values.ToList().ForEach((taskInfo) =>
                           {
                                 //                Debug.LogErrorFormat("{0},,,,{1}", taskInfo.taskId, taskInfo.taskSchedule);
                                m_taskProxy.SetTaskDailyeReceived(taskInfo);
                           });
                     
                        }
                    }
                    break;
                case CmdConstant.TaskStateChange:
                    {
                        TaskData taskData = notification.Body as TaskData;
                        if (taskData != null)
                        {
                            if (taskData.taskState == TaskState.finished)
                            {
                                switch (taskData.taskPageType)
                                {
                                    case EnumTaskPageType.TaskChapter:
                                        Tip.CreateTip(LanguageUtils.getTextFormat(700013, LanguageUtils.getText(taskData.l_nameId))).Show();
                                        break;
                                    case EnumTaskPageType.TaskDaily:
                                        Tip.CreateTip(LanguageUtils.getTextFormat(700012, LanguageUtils.getText(taskData.l_nameId))).Show();
                                        break;
                                    case EnumTaskPageType.TaskMain:
                                        Tip.CreateTip(LanguageUtils.getTextFormat(700010, LanguageUtils.getText(taskData.l_nameId))).Show();
                                        break;
                                    case EnumTaskPageType.TaskSide:
                                        Tip.CreateTip(LanguageUtils.getTextFormat(700011, LanguageUtils.getText(taskData.l_nameId))).Show();
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.TipRewardGroup:
                    {
                        List<RewardGroupData> rewardGroupDataList = notification.Body as List<RewardGroupData>;
                        if (rewardGroupDataList != null)
                        {
                            int count = rewardGroupDataList.Count;
                            if (count == 1)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(700025, LanguageUtils.getText(rewardGroupDataList[0].name), rewardGroupDataList[0].number)).Show();
                            }
                            else if (count == 2)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(700026, LanguageUtils.getText(rewardGroupDataList[0].name), rewardGroupDataList[0].number, LanguageUtils.getText(rewardGroupDataList[1].name), rewardGroupDataList[1].number)).Show();
                            }
                            else if (count == 3)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(700027, LanguageUtils.getText(rewardGroupDataList[0].name), rewardGroupDataList[0].number, LanguageUtils.getText(rewardGroupDataList[1].name), rewardGroupDataList[1].number, LanguageUtils.getText(rewardGroupDataList[2].name), rewardGroupDataList[2].number)).Show();
                            }
                            else if (count == 4)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(700028, LanguageUtils.getText(rewardGroupDataList[0].name), rewardGroupDataList[0].number, LanguageUtils.getText(rewardGroupDataList[1].name), rewardGroupDataList[1].number, LanguageUtils.getText(rewardGroupDataList[2].name), rewardGroupDataList[2].number, LanguageUtils.getText(rewardGroupDataList[3].name), rewardGroupDataList[3].number)).Show();
                            }
                            else if (count == 5)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(700029, LanguageUtils.getText(rewardGroupDataList[0].name), rewardGroupDataList[0].number, LanguageUtils.getText(rewardGroupDataList[1].name), rewardGroupDataList[1].number, LanguageUtils.getText(rewardGroupDataList[2].name), rewardGroupDataList[2].number, LanguageUtils.getText(rewardGroupDataList[3].name), rewardGroupDataList[3].number, LanguageUtils.getText(rewardGroupDataList[4].name), rewardGroupDataList[4].number)).Show();
                            }
                            else
                            {
                                Debug.LogError("not find rewardgroup");
                            }
                        }
                    }
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    m_taskProxy.UpdateTaskStateBuild();
                    break;
                case CmdConstant.ExpeditionInfoChange:
                    m_taskProxy.UpdateTaskStateYuanztongguan();
                    break;
                case CmdConstant.technologyChange:
                    {
                        m_taskProxy.UpdateTaskStatetechnologyChange();
                    }
                    break;
                case Role_RoleLogin.TagName:
                    m_taskProxy.UpdateTaskStateBuild();
                    m_taskProxy.UpdateTaskStateRisingStar();
                    if (!m_taskProxy.NotifiShow)
                    {
                        m_taskProxy.NotifiShow = true;
                    }
                    break;
                case CmdConstant.UpdatePlayerHistoryPower:
                    m_taskProxy.UpdateTaskStatePowerReached();
                    break;
                    case CmdConstant.denseFogOpenFlag:
  //                  Debug.LogError(m_playerProxy.CurrentRoleInfo.denseFogOpenFlag);
                    m_taskProxy.UpdateTaskStateExploreFog();
                    break;
                case CmdConstant.FinishSideTasks:
                    {
                        Dictionary<long, FinishTaskInfo> FinishSideTasks = notification.Body as Dictionary<long, FinishTaskInfo>;
                        if (FinishSideTasks != null)
                        {
                            FinishSideTasks.Values.ToList().ForEach((taskid) =>
                            {
                                m_taskProxy.SetTaskSideReceived(taskid.taskId);
                            });
                        }
                    }
                    break;
                case CmdConstant.TaskStatisticsSum:
                    {
                        Dictionary<long, TaskStatistics> TaskStatistics = notification.Body as Dictionary<long, TaskStatistics>;
                        if (TaskStatistics != null)
                        {
                            TaskStatistics.Keys.ToList().ForEach((type) =>
                            {
                                //for (int i = 0; i < TaskStatistics[type].statistics.Count; i++)
                                //{
                                //    Debug.LogErrorFormat("type:{0}arg:{1}num:{2}", (EnumTaskType)type, TaskStatistics[type].statistics[i].arg, TaskStatistics[type].statistics[i].num);
                                //}
                                m_taskProxy.UpdateTaskSideStateTaskStatistics(type, TaskStatistics[type]);
                            });
                        }

                    }
                    break;
                case CmdConstant.ChapterId:
                    {
                        m_taskProxy.UpdateTaskChapterCount();
                    }
                    break;
                case CmdConstant.InSoldierInfoChange:
                    {
                        m_taskProxy.UpdateTaskStateRisingStar();
                    }
                    break;
                case Hero_HeroInfo.TagName:
                    m_taskProxy.UpdateTaskStateHeroInfo();
                    break;
                case CmdConstant.MainLineTaskId:
                    m_taskProxy.UpdateTaskMainState();
                    break;
                case CmdConstant.ChapterTasks:
                    {
                        Dictionary<long, ChapterTaskInfo> chapterTasks = notification.Body as Dictionary<long, ChapterTaskInfo>;
                        if (chapterTasks != null)
                        {
                            chapterTasks.Values.ToList().ForEach((taskid) =>
                            {
                                //  Debug.LogErrorFormat("taskid:{0}status:{1}",taskid.taskId, taskid.status);
                                if (taskid.status == 1)
                                {
                                    m_taskProxy.SetTaskChapterReceived(taskid.taskId);
                                }
                            });
                        }
                    }
                    break;
                case CmdConstant.BuildingMenuJump: //活动跳转
                    {
                        JumpTypeDefine jumpDefine = notification.Body as JumpTypeDefine;
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType(jumpDefine.typeData1);
                        if (buildingInfoEntity == null)
                        {
                            Debug.LogFormat("building not find type:{0}", jumpDefine.typeData1);
                            return;
                        }
                        BuildingMenuDataDefine define2 = CoreUtils.dataService.QueryRecord<BuildingMenuDataDefine>(jumpDefine.typeData2);
                        if (define2 == null)
                        {
                            Debug.LogFormat("BuildingMenuDataDefine not find:{0}", jumpDefine.typeData2);
                            return;
                        }
                        MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.None, (int)buildingInfoEntity.buildingIndex, 0, define2.func));
                        });
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
            m_netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            m_buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_researchProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
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
        /// <summary>
        /// 跳转逻辑
        /// </summary>
        private void GoScript(int type, int p1, int p2, int r1)
        {
            switch ((EnumTaskType)(type))
            {
                case EnumTaskType.RebelsNemesis:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ClickExitCity);
                        CoreUtils.uiManager.ShowUI(UI.s_iF_SearchRes, null, new GOScrptGuide(EnumTaskType.RebelsNemesis, (int)SearchType.Barbarian, p1));
                    }
                    break;
                case EnumTaskType.BuildSomething:
                    {
                        if (GuideManager.Instance.IsGuideBuildScoutCamp) //引导创建斥候营地 特殊处理
                        {
                            return;
                        }
                        MoveAndEnterCityToBuild(() => { CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.BuildSomething, p1)); });
                    }
                    break;
                case EnumTaskType.CityCollect:
                    {
                        if (p1 == 1)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Farm);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.CityCollect, (int)EnumCityBuildingType.Farm, p2));

                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Farm).buildingIndex, p2, "harvestFood"));
                                });
                            }



                        }
                        else if (p1 == 2)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Sawmill);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.CityCollect, (int)EnumCityBuildingType.Sawmill, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Sawmill).buildingIndex, p2, "harvestWood"));
                                });
                            }



                        }
                        else if (p1 == 3)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Quarry);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.CityCollect, (int)EnumCityBuildingType.Quarry, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Quarry).buildingIndex, p2, "harvestStone"));

                                });
                            }



                        }
                        else if (p1 == 4)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.SilverMine);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.CityCollect, (int)EnumCityBuildingType.SilverMine, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.SilverMine).buildingIndex, p2, "harvestGold"));
                                });
                            }



                        }
                        else
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Farm);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.CityCollect, (int)EnumCityBuildingType.Farm, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Farm).buildingIndex, p2, "harvestFood"));
                                });
                            }


                        }

                    }
                    break;
                case EnumTaskType.ExploreFog:

                    {
                        Debug.Log("选中最近迷雾，并打开探索窗口");
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.ScoutCamp);
                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ExploreFog));
                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
                            FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;

                            var cityBuildingProxy =
                                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                            var worldPos = fogMedia.FindClosestAtWorldPos(cityBuildingProxy.RolePos.x, cityBuildingProxy.RolePos.y);

                            int fgId = fogMedia.GetFadeGroupId(worldPos.x, worldPos.y, WarFogMgr.GROUP_SIZE);
                            WarFogMgr.RemoveFadeGroupByType(FogSystemMediator.FADE_TYPE_CLICK);
                            WarFogMgr.CreateFadeGroup(fgId, 1, 5);

                            CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
                            WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.y, 1000, null);
                            float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
                            WorldCamera.Instance().SetCameraDxf(dxf, 1000, () =>
                            {

                                CoreUtils.uiManager.ShowUI(UI.s_scoutSearchMenuu, () =>
                                {
                                    UI_Pop_ExploreMistView UI_Pop_ExploreMistView = CoreUtils.uiManager.GetUI(4002).View as UI_Pop_ExploreMistView;
                                    FingerTargetParam param = new FingerTargetParam();
                                    param.AreaTarget = UI_Pop_ExploreMistView.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.gameObject;
                                    param.ArrowDirection = (int)EnumArrorDirection.Up;
                                    CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                                }, worldPos);

                            });
                        }
                    }
                    break;
                case EnumTaskType.Healandcollect:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Hospital);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Healandcollect, (int)EnumCityBuildingType.Hospital, p2));

                            });
                        }
                        else
                        {
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)buildingInfoEntity.buildingIndex, p2, "cure"));
                            });
                        }
                    }
                    break;
                case EnumTaskType.MapGather://(0,1,2,3,4)任意资源
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ClickExitCity);
                        if (p1 == 0)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_iF_SearchRes, null, new GOScrptGuide(EnumTaskType.MapGather, 1));
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_iF_SearchRes, null, new GOScrptGuide(EnumTaskType.MapGather, p1));
                        }

                    }
                    break;
                case EnumTaskType.PowerReached:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfominlevel();
                        MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.CityCollect, (int)buildingInfoEntity.buildingIndex, p2, "openBuildingUpdata"));
                        });
                    }
                    break;

                case EnumTaskType.Production:
                    {
                        long jumpBuildType = p1 + 1;

                        if (jumpBuildType == (long)EnumCityBuildingType.Farm)
                        {
                            BuildingInfoEntity buildingInfoEntity1 = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Farm);
                            if (buildingInfoEntity1 == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Production, (int)EnumCityBuildingType.Barracks, p2));
                                });
                                return;
                            }

                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Farm);
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                string status = m_buildingProxy.IsUpgrading(buildingInfoEntity.buildingIndex) ? "openBuildingSpeedUp" : "openBuildingUpdata";
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Production, (int)buildingInfoEntity.buildingIndex, p2, status));
                            });

                        }
                        else if (jumpBuildType == (long)EnumCityBuildingType.Sawmill)
                        {
                            BuildingInfoEntity buildingInfoEntity1 = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Sawmill);
                            if (buildingInfoEntity1 == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Production, (int)EnumCityBuildingType.Sawmill, p2));
                                });
                                return;
                            }

                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Sawmill);
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                             {
                                 string status = m_buildingProxy.IsUpgrading(buildingInfoEntity.buildingIndex) ? "openBuildingSpeedUp" : "openBuildingUpdata";
                                 AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Production, (int)buildingInfoEntity.buildingIndex, p2, status));
                             });
                        }
                        else if (jumpBuildType == (long)EnumCityBuildingType.Quarry)
                        {
                            BuildingInfoEntity buildingInfoEntity1 = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Quarry);
                            if (buildingInfoEntity1 == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Production, (int)EnumCityBuildingType.Quarry, p2));
                                });
                                return;
                            }

                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Quarry);
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                             {
                                 string status = m_buildingProxy.IsUpgrading(buildingInfoEntity.buildingIndex) ? "openBuildingSpeedUp" : "openBuildingUpdata";
                                 AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Production, (int)buildingInfoEntity.buildingIndex, p2, status));
                             });
                        }
                        else if (jumpBuildType == (long)EnumCityBuildingType.SilverMine)
                        {
                            BuildingInfoEntity buildingInfoEntity1 = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.SilverMine);
                            if (buildingInfoEntity1 == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Production, (int)EnumCityBuildingType.SilverMine, p2));
                                });
                                return;
                            }

                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.SilverMine);
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                             {
                                 string status = m_buildingProxy.IsUpgrading(buildingInfoEntity.buildingIndex) ? "openBuildingSpeedUp" : "openBuildingUpdata";
                                 AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Production, (int)buildingInfoEntity.buildingIndex, p2, status));
                             });
                        }
                    }
                    break;
                case EnumTaskType.Recruit:
                    {
                        if (p1 == 1)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Barracks);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Recruit, (int)EnumCityBuildingType.Barracks, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Recruit, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Barracks).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 2)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Stable);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Recruit, (int)EnumCityBuildingType.Stable, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Recruit, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Stable).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 3)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.ArcheryRange);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Recruit, (int)EnumCityBuildingType.ArcheryRange, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Recruit, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.ArcheryRange).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 4)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.SiegeWorkshop);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Recruit, (int)EnumCityBuildingType.SiegeWorkshop, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Recruit, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.SiegeWorkshop).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else
                        {
                            Debug.Log("not find type");
                        }
                    }
                    break;
                case EnumTaskType.RisingStar:
                    {
                        if (p1 == 1|| p1 == 0)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Barracks);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.RisingStar, (int)EnumCityBuildingType.Barracks, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.RisingStar, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Barracks).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 2)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Stable);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.RisingStar, (int)EnumCityBuildingType.Stable, p2));

                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.RisingStar, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Stable).buildingIndex, p2, "drill"));
                                });
                            }

                        }
                        else if (p1 == 3)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.ArcheryRange);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.RisingStar, (int)EnumCityBuildingType.ArcheryRange, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.RisingStar, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.ArcheryRange).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 4)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.SiegeWorkshop);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.RisingStar, (int)EnumCityBuildingType.SiegeWorkshop, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.RisingStar, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.SiegeWorkshop).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else
                        {
                            Debug.Log("not find type");
                        }

                    }
                    break;
                case EnumTaskType.SendRroops:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ClickExitCity);
                        CoreUtils.uiManager.ShowUI(UI.s_iF_SearchRes, null, new GOScrptGuide(EnumTaskType.SendRroops, (int)SearchType.Barbarian));
                    }
                    break;
                case EnumTaskType.TechnologyResearch:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Academy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.TechnologyResearch, (int)EnumCityBuildingType.Academy));
                            });
                        }
                        else
                        {
                            if (m_buildingProxy.IsUpgrading(buildingInfoEntity.buildingIndex))
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.TechnologyResearch, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Academy).buildingIndex, p2, "openBuildingSpeedUp"));

                                });
                            }
                            else
                            {
                                bool CrrTechnologEnd = false;//有研究完成未领取的科技
                                QueueInfo info = m_researchProxy.GetCrrTechnologying();
                                if (info != null && info.HasTechnologyType && info.technologyType > 0)
                                {
                                    if (info.finishTime < 0)
                                    {
                                        CrrTechnologEnd = true;
                                    }
                                }
                                if (CrrTechnologEnd)
                                {
                                    MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.TechnologyResearch, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Academy).buildingIndex, p2, "study"));

                                    });
                                }
                                else
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_ResearchMain, null, new GOScrptGuide(EnumTaskType.TechnologyResearch, p1, p2));
                                }
                            }

                        }
                    }
                    break;
                case EnumTaskType.Train:
                    {
                        if (p1 == 1)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Barracks);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Train, (int)EnumCityBuildingType.Barracks, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Train, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Barracks).buildingIndex, p2, "drill"));

                                });
                            }


                        }
                        else if (p1 == 2)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Stable);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Train, (int)EnumCityBuildingType.Stable, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Train, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Stable).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 3)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.ArcheryRange);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Train, (int)EnumCityBuildingType.ArcheryRange, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Train, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.ArcheryRange).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else if (p1 == 4)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.SiegeWorkshop);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Train, (int)EnumCityBuildingType.SiegeWorkshop, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Train, (int)m_buildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.SiegeWorkshop).buildingIndex, p2, "drill"));
                                });
                            }


                        }
                        else
                        {
                            Debug.Log("not find type");
                        }
                    }
                    break;
                case EnumTaskType.UpgradeBuilding:
                    {
                        if (p1 != 0)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType(p1);

                            if (buildingInfoEntity == null)
                            {
                                MoveAndEnterCityToBuild(() =>
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.UpgradeBuilding, p1, p2));
                                });
                            }
                            else
                            {
                                MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                                {
                                    string status = "openBuildingUpdata";
                                    if (m_buildingProxy.IsUpgrading(buildingInfoEntity.buildingIndex))
                                    {
                                        status = "openBuildingSpeedUp";
                                    }
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.UpgradeBuilding, (int)buildingInfoEntity.buildingIndex, p2, status));
                                });
                            }


                        }
                        else
                        {
                            BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfominlevel();
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () => { AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.UpgradeBuilding, (int)buildingInfoEntity.buildingIndex, p2, "openBuildingUpdata")); });

                        }
                    }
                    break;
                case EnumTaskType.Research:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Academy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.Research, (int)EnumCityBuildingType.Academy, p2));
                            });
                        }
                        else
                        {
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.Research, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Academy).buildingIndex, p2, "study"));
                            });
                        }



                    }
                    break;
                case EnumTaskType.SwornAllies:
                    if (m_allianceProxy.HasJionAlliance())
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                    }
                    break;
                case EnumTaskType.AbandonedFortMaster:
                    if (m_allianceProxy.HasJionAlliance())
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                    }
                    break;
                case EnumTaskType.Stranglehold:
                    if (m_allianceProxy.HasJionAlliance())
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                    }
                    break;
                case EnumTaskType.InMyName:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_PlayerInfo, () =>
                        {
                            PlayerDataView playerDataView = CoreUtils.uiManager.GetUI(12000).View as PlayerDataView;
                            FingerTargetParam param = new FingerTargetParam();
                            param.AreaTarget = playerDataView.m_btn_name_GameButton.gameObject;
                            param.ArrowDirection = (int)EnumArrorDirection.Up;
                            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                        });
                    }
                    break;
                case EnumTaskType.StrangeEncounter:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Tavern);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.StrangeEncounter, (int)EnumCityBuildingType.Tavern, p2));

                            });
                        }
                        else
                        {
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.StrangeEncounter, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Tavern).buildingIndex, p2, "tavernCall"));
                            });
                        }

                    }
                    break;
                case EnumTaskType.ImposingAura:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_captain, null, new GOScrptGuide(EnumTaskType.ImposingAura));
                    }
                    break;
                case EnumTaskType.MasterTactician:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_captain, null, new GOScrptGuide(EnumTaskType.MasterTactician));
                    }
                    break;
                case EnumTaskType.ALegendaryPerson:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_captain, null, new GOScrptGuide(EnumTaskType.ALegendaryPerson));
                    }
                    break;
                case EnumTaskType.TheTalentedOne:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_captain, null, new GOScrptGuide(EnumTaskType.TheTalentedOne));
                    }
                    break;
                case EnumTaskType.DestroytheStronghold:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                        float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                        WorldCamera.Instance().SetCameraDxf(dxf, 500, null);
                    }
                    break;
                case EnumTaskType.ScoutingMission:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.ScoutCamp);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ScoutingMission, (int)EnumCityBuildingType.ScoutCamp, p2));

                            });
                        }
                        else
                        {
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.ScoutingMission, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.ScoutCamp).buildingIndex, p2, "openUI"));
                            });
                        }
                    }
                    break;
                case EnumTaskType.OneinaMillion:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_PlayerInfo,()=> {
                            PlayerDataView playerDataView = CoreUtils.uiManager.GetUI(12000).View as PlayerDataView;
                            FingerTargetParam param = new FingerTargetParam();
                            param.AreaTarget = playerDataView.m_UI_Model_StandardButton_MiniBlue.gameObject;
                            param.EffectMountTarget = playerDataView.m_UI_Model_StandardButton_MiniBlue.gameObject;
                            param.ArrowDirection = (int)EnumArrorDirection.Up;
                            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                        });
                    }
                    break;
                case EnumTaskType.AFriend:
                case EnumTaskType.CaveInvestigation:
                case EnumTaskType.AgainstAllOdds:
                case EnumTaskType.TheToweringCity:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                        float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                        WorldCamera.Instance().SetCameraDxf(dxf, 500, null);
                    }
                    break;
                case EnumTaskType.AllianceHelp:
                    if (m_allianceProxy.HasJionAlliance())
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceHelp);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                    }
                    break;
                case EnumTaskType.UseItem:
                    {
                        if (p1 == 50101)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_captain,null, new GOScrptGuide(EnumTaskType.UseItem));
                            return;
                        }
                        BagParam bagParam = new BagParam();
                        int itemtype = 0;
                        if (p2 != 0)
                        {
                            itemtype = p2;
                            bagParam.TypeGroup = p1;

                        }
                        else
                        {
                            itemtype = p1 / 10000;
                            bagParam.SubType = p1;
                        }
                        bagParam.PageType = itemtype;
                        bagParam.Type = itemtype;
                        bagParam.IsFindItem = true;
                        CoreUtils.uiManager.ShowUI(UI.s_bagInfo, null, bagParam);
                    }
                    break;
                case EnumTaskType.shangdiangoumai:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Market);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ScoutingMission, (int)EnumCityBuildingType.Market, p2));

                            });
                        }
                        else
                        {
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.ScoutingMission, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Market).buildingIndex, p2, "announcement"));
                            });
                        }
                    }
                    break;
                case EnumTaskType.KingdomTrade:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.CourierStation);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ScoutingMission, (int)EnumCityBuildingType.CourierStation, p2));

                            });
                        }
                        else
                        {
                            MoveAndEnterCityToBuildToFinger(buildingInfoEntity, () =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndFinger, new GOScrptGuide(EnumTaskType.ScoutingMission, (int)m_buildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.CourierStation).buildingIndex, p2, "openMysteryStore"));
                            });
                        }
                    }
                    break;
                case EnumTaskType.LongJourney:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_expeditionFight, null, new GOScrptGuide(EnumTaskType.LongJourney));
                    }
                    break;
                case EnumTaskType.ForgeEquipment:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Smithy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ForgeEquipment, (int)EnumCityBuildingType.Smithy, p2));

                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Equip, null, 1);
                        }
                    }
                 
                    break;
                case EnumTaskType.FuseEquipmentBlueprint:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Smithy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ForgeEquipment, (int)EnumCityBuildingType.Smithy, p2));

                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Mix);
                        }

                        break;

                    }
                case EnumTaskType.FuseEquipmentMaterial:
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Smithy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ForgeEquipment, (int)EnumCityBuildingType.Smithy, p2));

                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Mix);
                        }

                        break;

                    }
                case EnumTaskType.ProduceEquipmentMaterials:
                  
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Smithy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ForgeEquipment, (int)EnumCityBuildingType.Smithy, p2));

                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Production);
                        }

                        break;

                    }
                case EnumTaskType.DismantleEquipment:
                    
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Smithy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ForgeEquipment, (int)EnumCityBuildingType.Smithy, p2));

                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Equip, null, 2);
                        }

                        break;

                    }
                case EnumTaskType.DismantleEquipmentMaterial:
                    
                    {
                        BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Smithy);

                        if (buildingInfoEntity == null)
                        {
                            MoveAndEnterCityToBuild(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(EnumTaskType.ForgeEquipment, (int)EnumCityBuildingType.Smithy, p2));

                            });
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Resolve);
                        }

                        break;

                    }
                default:
                  
                    Debug.LogError("not find type ");
                    break;
            }
        }
        /// <summary>
        /// 移动并指向建造
        /// </summary>
        /// <param name="actionfloat"></param>
        /// <param name="translationTime"></param>
        /// <param name="DxfTime"></param>
        private void MoveAndEnterCityToBuild(Action actionfloat, float translationTime = 500, float DxfTime = 500)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            //  Debug.Log(WorldCamera.Instance().GetViewCenter());
            //  Debug.Log(WorldCamera.Instance().getCurrentCameraDxf());
            float dxf = WorldCamera.Instance().getCameraDxf("city_default");
            if (!SmallDistance(new Vector2(m_buildingProxy.RolePos.x, m_buildingProxy.RolePos.y), WorldCamera.Instance().GetViewCenter(), 2))
            {
                WorldCamera.Instance().ViewTerrainPos(m_buildingProxy.RolePos.x, m_buildingProxy.RolePos.y, translationTime, () =>
                {
                    if (Mathf.Abs(WorldCamera.Instance().getCurrentCameraDxf() - dxf) < 35)
                    {
                        actionfloat?.Invoke();
                    }
                    else
                    {
                        WorldCamera.Instance().SetCameraDxf(dxf, DxfTime, actionfloat);
                    }
                });
            }
            else
            {
                if (Mathf.Abs(WorldCamera.Instance().getCurrentCameraDxf() - dxf) < 35)
                {
                    actionfloat?.Invoke();
                }
                else
                {
                    WorldCamera.Instance().SetCameraDxf(dxf, DxfTime, actionfloat);
                }
            }
        }
        /// <summary>
        /// 移动并指向建筑菜单
        /// </summary>
        /// <param name="buildingInfoEntity"></param>
        /// <param name="actionfloat"></param>
        /// <param name="translationTime"></param>
        /// <param name="DxfTime"></param>
        public void MoveAndEnterCityToBuildToFinger(BuildingInfoEntity buildingInfo, Action actionfloat, float translationTime = 500, float DxfTime = 500)
        {
            GameObject obj = CityObjData.GetMenuTargetGameObject(buildingInfo.buildingIndex,false);
            float dxf = WorldCamera.Instance().getCameraDxf("city_default");
            // Debug.Log(WorldCamera.Instance().GetViewCenter());
            // Debug.Log(WorldCamera.Instance().getCurrentCameraDxf());
            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            if (obj!=null)
            {
                    Transform findObj = obj.transform;
                    if (!SmallDistance(new Vector2(findObj.position.x, findObj.position.z), WorldCamera.Instance().GetViewCenter(), 2))
                    {
                        WorldCamera.Instance().ViewTerrainPos(findObj.position.x, findObj.position.z, translationTime, () =>
                        {
                            if (Mathf.Abs(WorldCamera.Instance().getCurrentCameraDxf() - dxf) < 35)
                            {
                                actionfloat?.Invoke();
                            }
                            else
                            {
                                WorldCamera.Instance().SetCameraDxf(dxf, DxfTime, actionfloat);
                            }
                        });
                    }
                    else
                    {
                        if (Mathf.Abs(WorldCamera.Instance().getCurrentCameraDxf() - dxf) < 35)
                        {
                            actionfloat?.Invoke();
                        }
                        else
                        {
                            WorldCamera.Instance().SetCameraDxf(dxf, DxfTime, actionfloat);
                        }
                    }
            }
        }

        /// <summary>
        /// 距离小不用移动摄像头
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        bool SmallDistance(Vector2 startPos, Vector2 endPos, float distance)
        {
            bool SmallDistance = false;
            float x, y = 0;
            x = endPos.x - startPos.x;
            y = endPos.y - startPos.y;
            SmallDistance = (x * x + y * y) < distance;
            return SmallDistance;
        }

    }
}