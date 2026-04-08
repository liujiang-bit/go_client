// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月9日
// Update Time         :    2020年1月9日
// Class Description   :    TaskProxy
// Copyright IGG All rights reserved.
// ===============================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using Client;
using Data;
using SprotoTemp;
using System;
using System.Linq;
using PureMVC.Interfaces;
using SprotoType;

namespace Game
{
    public class TaskData
    {
        public TaskData(RewardGroupProxy proxy, TaskProxy taskProxy)
        {
            rewardGroupProxy = proxy;
            this.taskProxy = taskProxy;
        }
        public int taskID;
        public TaskState taskState;
        public TaskMainDefine taskMainDefine;
        public TaskSideDefine taskSideDefine;
        public TaskChapterDefine taskChapterDefine;
        public TaskDailyDefine taskDailyDefine;
        public EnumTaskPageType taskPageType;
        public int l_nameId;
        public int _needNum = -1;
        public int needNum
        {
            set
            {

            }
            get
            {
                if(_needNum == -1)
                {
                    _needNum = taskProxy.SetTaskStatisticsNeedNum(this);
                }
                return _needNum;
            }
        }
        public int Num;
        public EnumTaskType type;
        public int SortId;
        public List<RewardGroupData> _rewardGroupDataList = null;
        public List<RewardGroupData> rewardGroupDataList
        {
            set
            {
                _rewardGroupDataList = value;
            }
            get
            {
                if(_rewardGroupDataList == null)
                {
                    if (taskMainDefine != null)
                    {
                        _rewardGroupDataList = rewardGroupProxy.GetRewardDataByGroup(taskMainDefine.reward, true);
                    }
                    else if (taskSideDefine != null)
                    {
                        _rewardGroupDataList = rewardGroupProxy.GetRewardDataByGroup(taskSideDefine.reward, true);
                    }
                    else if (taskDailyDefine != null)
                    {
                        _rewardGroupDataList = rewardGroupProxy.GetRewardDataByGroup(taskDailyDefine.reward, true);
                    }
                    else if (taskChapterDefine != null)
                    {
                        _rewardGroupDataList = rewardGroupProxy.GetRewardDataByGroup(taskChapterDefine.reward, true);
                    }
                }
                return _rewardGroupDataList;
            }
        }
        private string _desc = string.Empty;
        public string desc
        {
            set
            {
                _desc = value;
            }
            get
            {
                if (_desc == string.Empty)
                {
                    _desc = taskProxy.SetDesc(this);
                }

                return _desc;
            }
        }
        private RewardGroupProxy rewardGroupProxy;
        private TaskProxy taskProxy;
    }

    public enum TaskState
    {
        /// <summary>
        /// 进度未完成
        /// </summary>
        unfinished = 2,
        /// <summary>
        /// 进度已完成
        /// </summary>
        finished = 1,
        /// <summary>
        /// 奖励已领取
        /// </summary>
        received = 3
    }
    public class TaskProxy : GameProxy
    {

        #region Member
        public const string ProxyNAME = "TaskProxy";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private CurrencyProxy m_currencyProxy;
        private ResearchProxy m_researchProxy;
        private SoldierProxy m_soldierProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private RewardGroupProxy m_rewardGroupProxy;
        private HeroProxy mHeroProxy;
        
        private List<TaskMainDefine> m_taskMainList = new List<TaskMainDefine>();
        private List<TaskSideDefine> m_taskSideList = new List<TaskSideDefine>();
        private List<TaskDailyDefine> m_taskDailyList = new List<TaskDailyDefine>();
        private List<TaskChapterDefine> m_taskChapterList = new List<TaskChapterDefine>();
        private List<TaskData> m_taskMainDataList = new List<TaskData>();
        private List<TaskData> m_taskSideDataList = new List<TaskData>();
        private List<TaskData> m_taskDailyDataList = new List<TaskData>();
        private List<TaskData> m_taskChapterDataList = new List<TaskData>();

        private List<TaskData> m_TaskSideUnfinishedList = new List<TaskData>();//支线数据未完成
        private List<TaskData> m_TaskSideFinishedList = new List<TaskData>();//支线数据未领取奖励
        private List<TaskData> m_TaskDailyUnfinishedList = new List<TaskData>();//每日任务未完成
        private List<TaskData> m_TaskDailyFinishedList = new List<TaskData>();//每日任务未领取奖励
        private List<int> unfinishedGroupList = new List<int>();
        private Dictionary<long, List<TaskData>> m_idTaskChapterDic = new Dictionary<long, List<TaskData>>();//章节id，章节任务
        private Dictionary<long, TaskData> m_taskDataDic = new Dictionary<long, TaskData>();//任务数据
        List<ItemPackageShowDefine> allItemPackageShowList = new List<ItemPackageShowDefine>(); //奖励信息
        private RoleInfoEntity CurrentRoleInfo;

        private int m_finishCount = 0;//章节任务已完成
        private int m_receivedCount = 0; //章节任务已领取奖励
        private int m_taskChapterCount = 0;//章节任务总数

        private bool m_notifiShow = false;

        public bool NotifiShow { get => m_notifiShow; set => m_notifiShow = value; }
        #endregion

        // Use this for initialization
        public TaskProxy(string proxyName)
            : base(proxyName)
        {

        }


        public override void OnRegister()
        {
            Debug.Log(" TaskProxy register");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_researchProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;

            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            mHeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;

        }

        public override void OnRemove()
        {
            Debug.Log(" TaskProxy remove");
        }
        public void InitData()
        {
            if (CurrentRoleInfo == null)
            {
                CurrentRoleInfo = m_playerProxy.CurrentRoleInfo;
                SetTskInfo();
            }
            else
            {
                RefreshRewardData();
            }
        }
        /// <summary>
        /// 文明更换切换奖励
        /// </summary>
        private void RefreshRewardData()
        {
            m_idTaskChapterDic.Values.ToList().ForEach((chapterList) => {
                chapterList.ForEach((taskData) => { 
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskData.taskChapterDefine.reward, true);
                    taskData.rewardGroupDataList = null;
                });
            });
            m_taskDailyDataList.ForEach((taskData) => {
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskData.taskDailyDefine.reward, true);
                taskData.rewardGroupDataList = null;
            });
            m_taskSideDataList.ForEach((taskData) => {
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskData.taskSideDefine.reward, true);
                taskData.rewardGroupDataList = null;
            });
            m_taskMainDataList.ForEach((taskData) => {
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskData.taskMainDefine.reward, true);
                taskData.rewardGroupDataList = null;
            });
        }

        public void SetTskInfo()
        {
            if (m_taskMainList.Count == 0)
            {
                m_taskMainList = CoreUtils.dataService.QueryRecords<TaskMainDefine>();
            }
            if (m_taskSideList.Count == 0)
            {
                m_taskSideList = CoreUtils.dataService.QueryRecords<TaskSideDefine>();
            }
            if (m_taskDailyList.Count == 0)
            {
                m_taskDailyList = CoreUtils.dataService.QueryRecords<TaskDailyDefine>();

            }
            if (m_taskChapterList.Count == 0)
            {
                m_taskChapterList = CoreUtils.dataService.QueryRecords<TaskChapterDefine>();
            }
            if (allItemPackageShowList.Count == 0)
            {
                allItemPackageShowList = CoreUtils.dataService.QueryRecords<ItemPackageShowDefine>();//奖励信息
            }

            m_taskChapterList.ForEach((taskChapterDefine)=>
            {
                TaskData taskData = new TaskData(m_rewardGroupProxy, this);
                taskData.Num = 0;
                taskData.taskChapterDefine = taskChapterDefine;
                taskData.taskDailyDefine = null;
                taskData.taskID = taskChapterDefine.ID;
                taskData.taskMainDefine = null;
                taskData.taskSideDefine = null;
                taskData.taskState = TaskState.unfinished;
                taskData.taskPageType = EnumTaskPageType.TaskChapter;
                taskData.type = (EnumTaskType)(taskChapterDefine.type);
                //taskData.desc = SetDesc(taskData);
                //taskData.needNum = SetTaskStatisticsNeedNum(taskData);
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskChapterDefine.reward,true);
                taskData.l_nameId = taskChapterDefine.l_nameId;
                m_taskDataDic.Add(taskData.taskID, taskData);
                m_taskChapterDataList.Add(taskData);
                if (!m_idTaskChapterDic.ContainsKey(taskChapterDefine.chapterId))
                {
                    m_idTaskChapterDic[taskChapterDefine.chapterId] = new List<TaskData>();
                }
                m_idTaskChapterDic[taskChapterDefine.chapterId].Add(taskData);
            });
            //m_taskChapterDataList = m_taskDataDic.Values.ToList();
            m_taskDailyList.ForEach((taskDailyDefine) =>
            {
                TaskData taskData = new TaskData(m_rewardGroupProxy, this);
                taskData.Num = 0;
                taskData.taskChapterDefine = null;
                taskData.taskDailyDefine = taskDailyDefine;
                taskData.taskID = taskDailyDefine.ID;
                taskData.taskMainDefine = null;
                taskData.taskSideDefine = null;
                taskData.taskState = TaskState.unfinished;
                taskData.taskPageType = EnumTaskPageType.TaskDaily;
                taskData.type = (EnumTaskType)(taskDailyDefine.type);
                //taskData.desc = SetDesc(taskData);
                //taskData.needNum = SetTaskStatisticsNeedNum(taskData);
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskDailyDefine.reward, true);
                taskData.l_nameId = taskDailyDefine.l_nameId;
                m_taskDataDic.Add(taskData.taskID, taskData);
                m_taskDailyDataList.Add(taskData);
            });
            //m_taskDailyDataList = m_taskDataDic.Values.ToList();
            m_taskSideList.ForEach((taskSideDefine) =>
            {
                TaskData taskData = new TaskData(m_rewardGroupProxy, this);
                taskData.Num = 0;
                taskData.taskChapterDefine = null;
                taskData.taskDailyDefine = null;
                taskData.taskID = taskSideDefine.ID;
                taskData.taskMainDefine = null;
                taskData.taskSideDefine = taskSideDefine;
                taskData.taskState = TaskState.unfinished;
                taskData.taskPageType = EnumTaskPageType.TaskSide;
                taskData.type = (EnumTaskType)(taskSideDefine.type);
                //taskData.desc = SetDesc(taskData);
                //taskData.needNum = SetTaskStatisticsNeedNum(taskData);
                //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskSideDefine.reward, true);
                taskData.l_nameId = taskSideDefine.l_nameId;
                taskData.SortId = taskSideDefine.group1 * 10000 + taskSideDefine.order;
                m_taskDataDic.Add(taskData.taskID, taskData);
                m_taskSideDataList.Add(taskData);
            });
            //m_taskSideDataList = m_taskDataDic.Values.ToList();
            m_taskMainList.ForEach((taskMainDefine) =>
            {
                TaskData taskData = new TaskData(m_rewardGroupProxy, this);
                taskData.Num = 0;
                 taskData.taskChapterDefine = null;
                 taskData.taskDailyDefine = null;
                 taskData.taskID = taskMainDefine.ID;
                 taskData.taskMainDefine = taskMainDefine;
                 taskData.taskSideDefine = null;
                 taskData.taskState = TaskState.unfinished;
                 taskData.taskPageType = EnumTaskPageType.TaskMain;
                 taskData.type = (EnumTaskType)(taskMainDefine.type);
                 //taskData.desc = SetDesc(taskData);
                 //taskData.needNum = SetTaskStatisticsNeedNum(taskData);
                 //taskData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskMainDefine.reward, true);
                 taskData.l_nameId = taskMainDefine.l_nameId;
                 m_taskDataDic.Add(taskData.taskID, taskData);
                m_taskMainDataList.Add(taskData);
            });
            //m_taskMainDataList = m_taskDataDic.Values.ToList();

            m_taskSideDataList.Sort(TaskSort);
        }

        private int TaskSort(TaskData x, TaskData y)
        {
            if (x.SortId > y.SortId)
                return 1;
            if (x.SortId < y.SortId)
                return -1;
            return 0;
        }

        /// <summary>
        /// 设置支线任务状态为领取奖励
        /// </summary>
        public void SetTaskSideReceived(long taskid)
        {
            TaskData taskData = null;
            if (m_taskDataDic.TryGetValue(taskid, out taskData))
            {
                taskData.taskState = TaskState.received;
            }
        }
        public void SetTaskDailyeReceived(TaskInfo taskInfo)
        {
            TaskData taskData = null;
            if (m_taskDataDic.TryGetValue(taskInfo.taskId, out taskData))
            {
                if (taskInfo.taskSchedule == -1)
                {
                    taskData.taskState = TaskState.received;
                    if (m_TaskDailyUnfinishedList.Contains(taskData))
                    {
                        m_TaskDailyUnfinishedList.Remove(taskData);
                    }
                    if (m_TaskDailyFinishedList.Contains(taskData))
                    {
                        m_TaskDailyFinishedList.Remove(taskData);
                    }
                }
                else if (taskInfo.taskSchedule == taskData.needNum)
                {
                    taskData.Num = (int)taskInfo.taskSchedule;
                    taskData.taskState = TaskState.finished;
                    if (!m_TaskDailyFinishedList.Contains(taskData))
                    {
                        if (m_notifiShow)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.TaskStateChange, taskData);
                        }
                      //  Debug.LogErrorFormat("taskid{0}", taskData.taskID);
                        m_TaskDailyFinishedList.Add(taskData);
                    }
                    if (m_TaskDailyUnfinishedList.Contains(taskData))
                    {
                        m_TaskDailyUnfinishedList.Remove (taskData);
                    }
                }
                else
                {
                    taskData.Num = (int)taskInfo.taskSchedule;
                    taskData.taskState = TaskState.unfinished;
                    if (!m_TaskDailyUnfinishedList.Contains(taskData))
                    {
                        m_TaskDailyUnfinishedList.Add(taskData);
                    }
                }
            }
        }

        /// <summary>
        /// 设置主线任务状态为领取奖励
        /// </summary>
        public void SetTaskMainReceived(long taskid)
        {
            TaskData taskData = null;
            if (m_taskDataDic.TryGetValue(taskid, out taskData))
            {
                taskData.taskState = TaskState.received;
            }
        }

        /// <summary>
        /// 设置章节任务状态为领取奖励
        /// </summary>
        public void SetTaskChapterReceived(long taskid)
        {
            TaskData taskData = null;
            if (m_taskDataDic.TryGetValue(taskid, out taskData))
            {
                m_receivedCount++;
                if (taskData.taskState == TaskState.finished)
                {
                    m_finishCount--;
                }
                taskData.taskState = TaskState.received;
            }
        }
        /// <summary>
        /// 主线任务状态
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        private void SetTaskMainFinished(TaskData taskData)
        {
            if (taskData.taskState == TaskState.unfinished)
            {
                if (taskData.needNum <= taskData.Num)
                {
                    taskData.taskState = TaskState.finished;
                    if (m_notifiShow&& taskData.taskMainDefine.ID == (int)CurrentRoleInfo.mainLineTaskId)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.TaskStateChange, taskData);
                    }
                }
                else
                {
                    taskData.taskState = TaskState.unfinished;
                }
            }
        }

        /// <summary>
        ///  设置支线任务状态已完成
        /// </summary>
        /// <param name="taskData"></param>
        /// <returns></returns>
        private void SetTaskSideFinished(TaskData taskData)
        {
            if (taskData.taskState == TaskState.unfinished)
            {
                if (taskData.needNum <= taskData.Num)
                {
                    taskData.taskState = TaskState.finished;
                    if (m_notifiShow)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.TaskStateChange, taskData);
                    }
                }
                else
                {
                    taskData.taskState = TaskState.unfinished;
                }
            }
        }
        /// <summary>
        ///  设置章节任务状态已完成
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        private void SetTaskChapterFinished(TaskData taskData)
        {
            TaskState taskState = taskData.taskState;
            if (taskState == TaskState.unfinished)
            {
                if (taskData.needNum <= taskData.Num)
                {
                    taskData.taskState = TaskState.finished;
                    if (taskData.taskChapterDefine.chapterId == (int)CurrentRoleInfo.chapterId)
                    {
                        m_finishCount++;
                    }
                    if (m_notifiShow && taskData.taskChapterDefine.chapterId == (int)CurrentRoleInfo.chapterId)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.TaskStateChange, taskData);
                    }
                }
                else
                {
                    taskData.taskState = TaskState.unfinished;
                }
            }
        }
        #region 公有属性
        public TaskState GetTaskState(long taskid)
        {
            TaskState taskState = TaskState.unfinished;
            TaskData taskdata = null;
            if (m_taskDataDic.TryGetValue(taskid, out taskdata))
            {
                taskState = taskdata.taskState;
            }
            return taskState;
        }


        /// <summary>
        /// 更新所有类型的任务
        /// </summary>
        /// <param name="taskDataList"></param>
        /// <param name="dataCallBackEvent"></param>
        private void UpdateTaskState(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) =>
            {
                if (dataCallBackEvent != null)
                {
                     dataCallBackEvent(taskData);
                }
            });
        }

        private void UpdateTaskStateBuilding(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.UpgradeBuilding:
                    case EnumTaskType.BuildSomething:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                            dataCallBackEvent(taskData);
                        }
                        break;
                }

            });
        }
        private void UpdateTaskStateHeroInfo(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.ImposingAura:
                    case EnumTaskType.ALegendaryPerson:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                            dataCallBackEvent(taskData);
                        }
                        break;
                }

            });
        }
        private void UpdateTaskStateReached(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.PowerReached:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                            dataCallBackEvent(taskData);
                        }
                        break;
                }
            });
        }
        private void UpdateTaskStateExploreFog(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.ExploreFog:
                    case EnumTaskType.AgainstAllOdds:
                    case EnumTaskType.TheToweringCity:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                            dataCallBackEvent(taskData);
                        }
                        break;
                }
            });
        }
        
        /// <summary>
        ///  本地任务进度刷新
        /// </summary>
        public void UpdateTaskStatePowerReached()
        {
            UpdateTaskStateReached(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStateReached(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStateReached(m_taskSideDataList, SetTaskSideFinished);
        }
        /// <summary>
        ///  本地任务进度刷新迷雾
        /// </summary>
        public void UpdateTaskStateExploreFog()
        {
            UpdateTaskStateExploreFog(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStateExploreFog(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStateExploreFog(m_taskSideDataList, SetTaskSideFinished);
        }
        /// <summary>
        /// 本地任务进度刷新
        /// </summary>
        public void UpdateTaskStateBuild()
        {
            UpdateTaskStateBuilding(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStateBuilding(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStateBuilding(m_taskSideDataList, SetTaskSideFinished);
        }
        /// <summary>
        /// 本地任务进度刷新统帅
        /// </summary>
        public void UpdateTaskStateHeroInfo()
        {
            UpdateTaskStateHeroInfo(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStateHeroInfo(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStateHeroInfo(m_taskSideDataList, SetTaskSideFinished);
        }
        public void UpdateTaskStatetechnologyChange(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.TechnologyResearch:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                             dataCallBackEvent(taskData);
                        }
                        break;
                }

            });
        }
        public void UpdateTaskStateRisingStar(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.RisingStar:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                           dataCallBackEvent(taskData);
                        }
                        break;
                }

            });
        }
        public void UpdateTaskStateYuanztongguan(List<TaskData> taskDataList, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) => {
                switch (taskData.type)
                {
                    case EnumTaskType.LongJourney:
                        taskData.Num = (int)GetTaskStatisticsNum(taskData);
                        if (dataCallBackEvent != null)
                        {
                            dataCallBackEvent(taskData);
                        }
                        break;
                }

            });
        }

        /// <summary>
        /// 更新科技相关任务
        /// </summary>
        public void UpdateTaskStatetechnologyChange()
        {
            UpdateTaskStatetechnologyChange(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStatetechnologyChange(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStatetechnologyChange(m_taskSideDataList, SetTaskSideFinished);
        }
        /// <summary>
        /// 更新部队相关任务
        /// </summary>
        public void UpdateTaskStateRisingStar()
        {
            UpdateTaskStateRisingStar(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStateRisingStar(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStateRisingStar(m_taskSideDataList, SetTaskSideFinished);
        }
        /// <summary>
        /// 更新远征相关任务
        /// </summary>
        public void UpdateTaskStateYuanztongguan()
        {
            UpdateTaskStateYuanztongguan(m_taskMainDataList, SetTaskMainFinished);
            UpdateTaskStateYuanztongguan(m_taskChapterDataList, SetTaskChapterFinished);
            UpdateTaskStateYuanztongguan(m_taskSideDataList, SetTaskSideFinished);
        }
        public void UpdateTaskMainState()
        {
            UpdateTaskState(m_taskMainDataList, SetTaskMainFinished);
        }
        public void UpdateTaskStatistics(List<TaskData> taskDataList ,long type, TaskStatistics taskStatistics, Action<TaskData> dataCallBackEvent)
        {
            taskDataList.ForEach((taskData) =>
            {
                if ((long)taskData.type == type)
                {
                    taskData.Num = (int)GetTaskStatisticsNum(taskData, type, taskStatistics);
                    if (dataCallBackEvent != null)
                    {
                        dataCallBackEvent(taskData);
                    }
                }
            });
        }
        /// <summary>
        /// 更新任务进度
        /// </summary>
        /// <param name="type"></param>
        public void UpdateTaskSideStateTaskStatistics(long type, TaskStatistics taskStatistics)
        {
            UpdateTaskStatistics(m_taskSideDataList ,type, taskStatistics, SetTaskSideFinished);
            UpdateTaskStatistics(m_taskMainDataList, type, taskStatistics, SetTaskMainFinished);
            UpdateTaskStatistics(m_taskChapterDataList, type, taskStatistics, SetTaskChapterFinished);
        }
        public TaskData GetTaskDataByid(long taskid)
        {
            TaskData taskData = null;
            m_taskDataDic.TryGetValue(taskid,out taskData);
            return taskData;
        }
        #endregion
        /// <summary>
        /// 获取任务需要的统计数量
        /// </summary>
        /// <returns></returns>
        public int SetTaskStatisticsNeedNum(TaskData taskData)
        {
            EnumTaskPageType TaskPageType = taskData.taskPageType;


            int NeedNum = 1;
            int  p1 = 0, p2 = 0, r1 = 0;
            switch (TaskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    {
                        p1 = taskData.taskChapterDefine.param1;
                        p2 = taskData.taskChapterDefine.param2;
                        r1 = taskData.taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskMain:
                    {
                        p1 = taskData.taskMainDefine.param1;
                        p2 = taskData.taskMainDefine.param2;
                        r1 = taskData.taskMainDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskSide:
                    {
                        p1 = taskData.taskSideDefine.param1;
                        p2 = taskData.taskSideDefine.param2;
                        r1 = taskData.taskSideDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        p1 = taskData.taskDailyDefine.param1;
                        p2 = taskData.taskDailyDefine.param2;
                        r1 = taskData.taskDailyDefine.require;
                    }
                    break;
            }
            switch (taskData.type)
            {
                case EnumTaskType.UpgradeBuilding:
                    {
                        if (p1 != 0)
                        {
                            NeedNum = p2;
                        }
                        else
                        {
                            NeedNum = r1;
                        }
                    }
                    break;
                case EnumTaskType.TechnologyResearch:
                    {
                        if (p1 != 0)
                        {
                            NeedNum = p2;
                        }
                        else
                        {
                            NeedNum = r1;
                        }
                    }
                    break;
                case EnumTaskType.LongJourney:
                    {
                        NeedNum = p1;
                    }
                    break;
                default:
                    NeedNum = r1;
                    break;
            }
            return NeedNum;
        }

        /// <summary>
        /// 获取任务统计数量
        /// </summary>
        /// <returns></returns>
        private long GetTaskStatisticsNum(TaskData taskData, long type, TaskStatistics taskStatistics)
        {
            long num = taskData.Num;
            int p1 = 0, p2 = 0, p3 = 0;
            switch (taskData.taskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    {
                        TaskChapterDefine taskChapterDefine = taskData.taskChapterDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskMain:

                    {
                        TaskMainDefine taskChapterDefine = taskData.taskMainDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskSide:
                    {
                        TaskSideDefine taskChapterDefine = taskData.taskSideDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        TaskDailyDefine taskChapterDefine = taskData.taskDailyDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;

            }

            int count = taskStatistics.statistics.Count;
            if (p1 == 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Statistics statistics = taskStatistics.statistics[i];
                    if (statistics.arg == -1 )
                    {
                        num = statistics.num;
                        break;
                    }
                }

            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Statistics statistics = taskStatistics.statistics[i];
                    if (statistics.arg == p1)
                    {
                        num = statistics.num;
                        break;
                    }
                }
            }
            num = num > taskData.needNum ? taskData.needNum : num;
            return num ;
        }
            /// <summary>
        /// 获取任务统计数量
        /// </summary>
        /// <returns></returns>
        private long GetTaskStatisticsNum(TaskData taskData)
        {
            long num = taskData.Num;
            int  p1 = 0, p2 = 0, p3 = 0;
            switch (taskData.taskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    {
                        TaskChapterDefine taskChapterDefine = taskData.taskChapterDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskMain:

                    {
                        TaskMainDefine taskChapterDefine = taskData.taskMainDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskSide:
                    {
                        TaskSideDefine taskChapterDefine = taskData.taskSideDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        TaskDailyDefine taskChapterDefine = taskData.taskDailyDefine;
                        p1 = taskChapterDefine.param1;
                        p2 = taskChapterDefine.param2;
                        p3 = taskChapterDefine.require;
                    }
                    break;

            }

            switch (taskData.type)
            {
                case EnumTaskType.UpgradeBuilding:
                    {
                        if (p1 != 0)
                        {
                            num = m_cityBuildingProxy.GetMaxLevelofType(p1);
                        }
                    }
                    break;
                case EnumTaskType.TechnologyResearch:
                    {
                        if (p1 != 0)
                        {
                            num = m_researchProxy.GetCrrTechnologyLv(p1);
                        }
                    }
                    break;
                case EnumTaskType.BuildSomething:
                    {
                        if (p1 != 0)
                        {
                            num = m_cityBuildingProxy.GetBuildCount(p1);
                        }
                    }
                    break;
                case EnumTaskType.RisingStar:
                    {
                       long  tempnum = 0;
                        if (p1 != 0)
                        {
                            Dictionary<long, long> solders = m_soldierProxy.GetInSoldierByType(p1);
                            solders.Values.ToList().ForEach((solder) =>
                            {
                                tempnum += solder;
                            });
                        }
                        else
                        {
                            Dictionary<long, SoldierInfo> solders = m_playerProxy.GetInArmyInfo();
                            solders.Values.ToList().ForEach((solder) =>
                            {
                                tempnum += solder.num;
                            });
                        }
                        num = tempnum > num ? tempnum : num;
                    }
                    break;
                case EnumTaskType.PowerReached:
                    {
                        num = CurrentRoleInfo.historyPower;
                    }
                    break;
                case EnumTaskType.Production:
                    {
                        num = 0;
                        List<BuildingInfoEntity> Farms = m_cityBuildingProxy.GetAllBuildingInfoByType((long)EnumCityBuildingType.Farm);
                        Farms.ForEach((farm)=>
                        {
                            int id = int.Parse((farm.level < 10) ? string.Format("{0}0{1}", farm.type, farm.level) : string.Format("{0}{1}", farm.type, farm.level));
                            BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                            if (define1 != null)
                            {
                                num += (long)(Mathf.Floor(define1.produceSpeed * (float)m_playerAttributeProxy.GetCityAttribute(attrType.foodCapacityMulti).value));
                            }
                        });
                    }
                    break;
                case EnumTaskType.ExploreFog:
                case EnumTaskType.AgainstAllOdds:
                case EnumTaskType.TheToweringCity:
                    {
                        if (m_playerProxy.CurrentRoleInfo.denseFogOpenFlag)
                        {
                            num = taskData.needNum;
                        }
                    }
                    break;
                case EnumTaskType.ImposingAura:
                    {
                         num = 0;
                        List<HeroProxy.Hero> m_ownHeros;
                        List<HeroProxy.Hero> m_summonHeros;
                        List<HeroProxy.Hero> m_noSummomHeros;
        mHeroProxy.GetHerosBySort(out m_ownHeros, out m_summonHeros, out m_noSummomHeros, HeroProxy.SortType.Level);
                        m_ownHeros.ForEach((hero) => {
                            num = hero.level > num ? hero.level : num;
                        });
                    }
                    break;
                case EnumTaskType.ALegendaryPerson:
                    {
                        num = 0;
                        List<HeroProxy.Hero> m_ownHeros;
                        List<HeroProxy.Hero> m_summonHeros;
                        List<HeroProxy.Hero> m_noSummomHeros;
                        mHeroProxy.GetHerosBySort(out m_ownHeros, out m_summonHeros, out m_noSummomHeros, HeroProxy.SortType.Level);
                        m_ownHeros.ForEach((hero) => {
                            if (hero.star >=p1)
                            {
                                num ++;
                            }
                        });
                    }
                    break;
                case EnumTaskType.LongJourney:
                    {
                            if (m_playerProxy.CurrentRoleInfo.expeditionInfo != null)
                            {
                            m_playerProxy.CurrentRoleInfo.expeditionInfo.Values.ToList().ForEach((expeditionInfo) => {
                                if (expeditionInfo.id < 100)
                                {
                                    if (expeditionInfo.id > num)
                                    {
                                        num = expeditionInfo.id;
                                    }
                                }
                            });
                            }
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
            num = num > taskData.needNum ? taskData.needNum : num;
            return num;
        }

        /// <summary>
        /// 章节任务数和已完成任务数
        /// </summary>
        /// <param name="TaskChapterFinishCount"></param>
        /// <returns></returns>
        public int GetTaskChapterFinishCount(long chapter, out int TaskChapterCount)
        {
            TaskChapterCount = m_taskChapterCount;
            return m_receivedCount;
        }

        public List<TaskData> GetTaskChapterListByChapter(long chapter = 0)
        {
            List<TaskData> taskChapterList = new List<TaskData>();
            int chapterId = (int)chapter;
            if (chapter == 0)
            {
                chapter = CurrentRoleInfo.chapterId;
            }
            m_idTaskChapterDic.TryGetValue(chapterId, out taskChapterList); 
            taskChapterList.Sort((TaskData x, TaskData y) =>
            {
                int re = 0;
                if (chapterId != 1)
                {
                    re = ((int)x.taskState).CompareTo((int)y.taskState);
                }
                if (re == 0)
                {
                    return x.taskChapterDefine.order.CompareTo(y.taskChapterDefine.order);
                }

                return re;

            });
            return taskChapterList;
        }

      
        /// <summary>
        /// 更新章节任务完成数量
        /// </summary>
        public void UpdateTaskChapterCount()
        {
            if (CurrentRoleInfo == null)
            {
                CurrentRoleInfo = m_playerProxy.CurrentRoleInfo;
            }
            List<TaskData> allTaskChapterList = new List<TaskData>();
            m_receivedCount = 0;
            m_finishCount = 0;
            m_taskChapterCount = 0;
            if (m_idTaskChapterDic.TryGetValue(CurrentRoleInfo.chapterId, out allTaskChapterList))
            {
                m_taskChapterCount = allTaskChapterList.Count;
                    allTaskChapterList.ForEach((taskData) =>
                    {
                        if (taskData.taskState == TaskState.finished)
                        {
                            m_finishCount++;
                        }
                    });
            }
        }

        /// <summary>
        /// 用于显示的支线任务
        /// </summary>
        /// <returns></returns>
        public List<TaskData> GetTaskSideList()
        {
            List<TaskData> taskSideList = new List<TaskData>();
            m_TaskSideFinishedList.Clear();
            m_TaskSideUnfinishedList.Clear();
            unfinishedGroupList.Clear();
            m_taskSideDataList.ForEach((taskData) =>
            {
                switch (taskData.taskState)
                {
                    case TaskState.finished:
                        if (!m_TaskSideFinishedList.Contains(taskData))
                        {
                            m_TaskSideFinishedList.Add(taskData);
                        }
                        break;
                    case TaskState.unfinished:
                        if (!unfinishedGroupList.Contains(taskData.taskSideDefine.group1))
                        {
                            if (CurrentRoleInfo.level >= taskData.taskSideDefine.showLevelReq)
                            {
                                if (taskData.taskSideDefine.showBuildingReq == 0 || m_cityBuildingProxy.GetMaxLevelofType((long)taskData.taskSideDefine.showBuildingReq) != 0)
                                {
                                    m_TaskSideUnfinishedList.Add(taskData);
                                    unfinishedGroupList.Add(taskData.taskSideDefine.group1);
                                   // Debug.LogError(taskData.desc);
                                }
                            }

                        }
                        break;
                }
            });
            int count = m_TaskSideUnfinishedList.Count;
            int limint = m_playerProxy.ConfigDefine.taskSideShowLimit;
            // int limint = 3;
            if (count > limint)
            {
                m_TaskSideUnfinishedList.RemoveRange(limint, count - limint);
            }
            taskSideList.AddRange(m_TaskSideFinishedList);
            taskSideList.AddRange(m_TaskSideUnfinishedList);
            return taskSideList;
        }
        /// <summary>
        /// 每日任务面板显示的任务
        /// </summary>
        /// <returns></returns>
        public List<TaskDataItem> GetTaskListPagDaily()
        {
            int age = (int)m_cityBuildingProxy.GetAgeType();
            List<TaskDataItem> taskDataItemList = new List<TaskDataItem>();
            m_TaskDailyFinishedList.ForEach((taskDaily) =>
            {
                TaskDataItem taskDataItem = new TaskDataItem();
                taskDataItem.type = 5;
                taskDataItem.taskState = taskDaily.taskState;
                taskDataItem.desc = LanguageUtils.getTextFormat(700014, taskDaily.desc, LanguageUtils.getTextFormat(181104, taskDaily.Num, taskDaily.needNum));
                taskDataItem.taskData = taskDaily;
                taskDataItemList.Add(taskDataItem);
            });
            m_TaskDailyUnfinishedList.ForEach((taskDaily) =>
            {
                TaskDataItem taskDataItem = new TaskDataItem();
                taskDataItem.type = 5;
                taskDataItem.taskState = taskDaily.taskState;
                taskDataItem.desc = LanguageUtils.getTextFormat(700014, taskDaily.desc, LanguageUtils.getTextFormat(181104, taskDaily.Num, taskDaily.needNum));
                taskDataItem.taskData = taskDaily;
                taskDataItemList.Add(taskDataItem);
            });

            return taskDataItemList;
        }
        /// <summary>
        /// 主界面显示的任务
        /// </summary>
        /// <returns></returns>
        public List<TaskData> GetTaskList()
        {
            List<TaskData> taskDataList = new List<TaskData>();
            if (CurrentRoleInfo != null)
            {
                if (ShowOtherTask())
                {
                    TaskData taskdata = null;
                    if (m_taskDataDic.TryGetValue(CurrentRoleInfo.mainLineTaskId, out taskdata))
                    {
                        taskDataList.Add(taskdata);
                    }
                    else
                    {
                        Debug.Log(CurrentRoleInfo.mainLineTaskId);
                    }
                    taskDataList.AddRange(GetTaskSideList());
                }
                if (CurrentRoleInfo.chapterId != -1)
                {
                    TaskData taskdata = new TaskData(m_rewardGroupProxy, this);
                    taskdata.taskPageType = EnumTaskPageType.TaskChapter;
                    taskDataList.Add(taskdata);
                }
            }
            return taskDataList;
        }
        public List<TaskDataItem> GetTaskListPageMain()
        {
            List<TaskDataItem> taskDataItemList = new List<TaskDataItem>();
            if (ShowOtherTask())
            {
                TaskData taskdata = null;
                if (m_taskDataDic.TryGetValue(CurrentRoleInfo.mainLineTaskId, out taskdata))
             {
                    {
                        TaskDataItem taskDataItem = new TaskDataItem();
                        taskDataItem.type = 1;
                        taskDataItemList.Add(taskDataItem);
                    }
                    {
                        TaskDataItem taskDataItem = new TaskDataItem();
                        taskDataItem.type = 2;
                        taskDataItem.taskData = taskdata;
                        taskDataItemList.Add(taskDataItem);
                    }
                }

            }
            List<TaskData> taskDataSideList = GetTaskSideList();
            if (taskDataSideList.Count != 0)
            {
                {
                    TaskDataItem taskDataItem = new TaskDataItem();
                    taskDataItem.type = 3;

                    taskDataItemList.Add(taskDataItem);
                }
                taskDataSideList.ForEach((taskDataSide) =>
                {
                    TaskDataItem taskDataItem = new TaskDataItem();
                    taskDataItem.type = 4;
                    taskDataItem.taskData = taskDataSide;
                    taskDataItemList.Add(taskDataItem);
                });
            }
            return taskDataItemList;
        }

        /// <summary>
        /// 任务描述
        /// </summary>
        /// <param name="taskData"></param>
        /// <returns></returns>
        public string SetDesc(TaskData taskData)
        {
            string s_desc = string.Empty;
            switch (taskData.taskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    {
                        s_desc = LanguageUtils.getText(taskData.taskChapterDefine.l_desc);
                        s_desc = s_desc.Replace("{p1}", taskData.taskChapterDefine.param1.ToString("N0"));
                        s_desc = s_desc.Replace("{p2}", taskData.taskChapterDefine.param2.ToString("N0"));
                        s_desc = s_desc.Replace("{p3}", taskData.taskChapterDefine.require.ToString("N0"));
                        s_desc = s_desc.Replace("{sn1}", GetSn1(taskData.taskChapterDefine.type, taskData.taskChapterDefine.param1, taskData.taskChapterDefine.param2));
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        s_desc = LanguageUtils.getText(taskData.taskDailyDefine.l_desc);
                        s_desc = s_desc.Replace("{p1}", taskData.taskDailyDefine.param1.ToString("N0"));
                        s_desc = s_desc.Replace("{p2}", taskData.taskDailyDefine.param2.ToString("N0"));
                        s_desc = s_desc.Replace("{p3}", taskData.taskDailyDefine.require.ToString("N0"));
                        s_desc = s_desc.Replace("{sn1}", GetSn1(taskData.taskDailyDefine.type, taskData.taskDailyDefine.param1, taskData.taskDailyDefine.param2));
                    }
                    break;
                case EnumTaskPageType.TaskMain:
                    {
                        s_desc = LanguageUtils.getText(taskData.taskMainDefine.l_desc);
                        s_desc = s_desc.Replace("{p1}", taskData.taskMainDefine.param1.ToString("N0"));
                        s_desc = s_desc.Replace("{p2}", taskData.taskMainDefine.param2.ToString("N0"));
                        s_desc = s_desc.Replace("{p3}", taskData.taskMainDefine.require.ToString("N0"));
                        s_desc = s_desc.Replace("{sn1}", GetSn1(taskData.taskMainDefine.type, taskData.taskMainDefine.param1, taskData.taskMainDefine.param2));
                    }
                    break;
                case EnumTaskPageType.TaskSide:
                    {
                        s_desc = LanguageUtils.getText(taskData.taskSideDefine.l_desc);
                        
                        s_desc = s_desc.Replace("{p1}", taskData.taskSideDefine.param1.ToString("N0"));
                        s_desc = s_desc.Replace("{p2}", taskData.taskSideDefine.param2.ToString("N0"));
                        s_desc = s_desc.Replace("{p3}", taskData.taskSideDefine.require.ToString("N0"));
                        s_desc = s_desc.Replace("{sn1}", GetSn1(taskData.taskSideDefine.type, taskData.taskSideDefine.param1, taskData.taskSideDefine.param2));
                    }
                    break;
            }
            return s_desc;
        }
        private string GetSn1(int type, int p1, int p2)
        {
            string s_desc = string.Empty;
            if (p1 != 0)
            {
                switch ((EnumTaskType)type)
                {

                    case EnumTaskType.Research:
                        {
                            StudyDefine StudyDefine = m_researchProxy.GetTechnologyByLevel(p1,1);
                            s_desc = LanguageUtils.getText(StudyDefine.l_nameID);
                        }
                        break;
                    case EnumTaskType.TechnologyResearch:
                        {
                            StudyDefine studyDefine = m_researchProxy.GetTechnologyByLevel(p1, 1);
                            if (studyDefine != null)
                            {
                                s_desc = LanguageUtils.getText(studyDefine.l_nameID);
                            }

                        }
                        break;
                      
                    default:
                        {
                            s_desc = p1.ToString("N0");
                        }
                        break;
                }
            }
            else
            {
                s_desc = p1.ToString("N0");
            }

            return s_desc;
        }

        public void  RefreshRedPoint()
        {

        }

        public int GetRedPoint(EnumTaskPageType taskPageType)
        {
            int num = 0;
            switch (taskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    if (CurrentRoleInfo.chapterId == -1)
                    {
                        num = 0;
                    }
                    else
                    {
                        if (m_receivedCount == m_taskChapterCount)
                        {
                            num = 1;
                        }
                        else
                        {
                            num = m_finishCount;
                        }
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        num = m_TaskDailyFinishedList.Count;
                    }
                    break;
                case EnumTaskPageType.TaskMain:
                    {
                        long taskId = CurrentRoleInfo.mainLineTaskId;
                        if (taskId != -1)
                        {
                            TaskData taskdata = m_taskDataDic[taskId];
                            if (taskdata.taskState == TaskState.finished)
                            {
                                num =  1;
                            }
                        }
                        else
                        {
                            
                        }
                    }
                    break;
                case EnumTaskPageType.TaskSide:
                    {
                        num = m_TaskSideFinishedList.Count;
                    }
                    break;
            }
            return num;
        }
        public int GetRedPointActivePointRewards()
            {
                long activePoint = m_playerProxy.CurrentRoleInfo.activePoint;
            int redPointActivePointRewards = 0;
                if (m_playerProxy.CurrentRoleInfo.activePointRewards != null)
            {
                TaskActivityRewardDefine taskActivityRewardDefine = CoreUtils.dataService.QueryRecord<TaskActivityRewardDefine>((int)m_playerProxy.CurrentRoleInfo.level);
                    if (activePoint >= taskActivityRewardDefine.activePoints1)
                    {
                        if (!m_playerProxy.CurrentRoleInfo.activePointRewards.Contains((long)taskActivityRewardDefine.activePoints1))
                        {
                            redPointActivePointRewards += 1;
                        }
                    }
                    if (activePoint >= taskActivityRewardDefine.activePoints2)
                    {
                        if (!m_playerProxy.CurrentRoleInfo.activePointRewards.Contains((long)taskActivityRewardDefine.activePoints2))
                        {
                            redPointActivePointRewards += 1;
                        }
                    }
                    if (activePoint >= taskActivityRewardDefine.activePoints3)
                    {
                        if (!m_playerProxy.CurrentRoleInfo.activePointRewards.Contains((long)taskActivityRewardDefine.activePoints3))
                        {
                            redPointActivePointRewards += 1;
                        }
                    }
                    if (activePoint >= taskActivityRewardDefine.activePoints4)
                    {
                        if (!m_playerProxy.CurrentRoleInfo.activePointRewards.Contains((long)taskActivityRewardDefine.activePoints4))
                        {
                            redPointActivePointRewards += 1;
                        }
                    }
                    if (activePoint >= taskActivityRewardDefine.activePoints5)
                    {
                        if (!m_playerProxy.CurrentRoleInfo.activePointRewards.Contains((long)taskActivityRewardDefine.activePoints5))
                        {
                            redPointActivePointRewards += 1;
                        }
                    }
                }
            return redPointActivePointRewards;
            }

        public int GetRedPoint()
        {
            int num = 0;
            if (ShowOtherTask())
            {
                num = GetRedPoint(EnumTaskPageType.TaskChapter) + GetRedPoint(EnumTaskPageType.TaskMain) + GetRedPoint(EnumTaskPageType.TaskSide) + GetRedPoint(EnumTaskPageType.TaskDaily)+ GetRedPointActivePointRewards();
            }
            else
            {
                num = GetRedPoint(EnumTaskPageType.TaskChapter) ;
            }
            return num;
        }

        public bool ShowOtherTask()
        {
            bool show = false;
            if ((CurrentRoleInfo.chapterId > m_playerProxy.ConfigDefine.preChapter || CurrentRoleInfo.chapterId == -1))
            {
                show = true;
            }
            else
            {
                show = false;
            }
            return show;
        }
    }
}