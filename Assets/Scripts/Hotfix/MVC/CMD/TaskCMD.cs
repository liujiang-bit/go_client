// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    TroopsCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using PureMVC.Interfaces;
using UnityEngine;
using PureMVC.Patterns;
using SprotoType;
using Skyunion;
using Data;
using Client;

namespace Game {
    public class TaskCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            PlayerProxy m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            switch (notification.Name)
            {
                case CmdConstant.ChapterTimelineShow:
                    TaskChapterDataDefine taskChapterDataDefine = notification.Body as TaskChapterDataDefine;
                    if (!string.IsNullOrEmpty(taskChapterDataDefine.preImg))
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_chapterScene,null, taskChapterDataDefine);
                    }
            break;
                case CmdConstant.ChapterIdChange:
                    {
              
                        bool showbtneffect = false;
                             if (CoreUtils.uiManager.ExistUI(11000))
                        {
                            UIInfo info = CoreUtils.uiManager.GetUI(11000);
                            if (info.uiObj == null)
                            {
                                return;
                            }
                            showbtneffect = true;
                          
                        }
                        if (showbtneffect)
                        {
                            UI_Win_QuestView questView = CoreUtils.uiManager.GetUI(11000).View as UI_Win_QuestView;
                            if (questView.m_UI_Model_btn.m_root_RectTransform.transform.gameObject.activeSelf)
                            {
                                Animation animation = questView.m_UI_Model_btn.m_btn_languageButton_GameButton.GetComponent<Animation>();
                                if (animation != null)
                                {
                                    animation.cullingType = AnimationCullingType.AlwaysAnimate;
                                    animation.Play(RS.TaskFinshCollectReward); 
                                }
                            }
                            Timer.Register(0.5f, () =>
                            {
                                OpenChapterReward();


                            }, null, false);
                        }
                        else
                        {
                            OpenChapterReward();
                        }
                    }
                    break;
                case CmdConstant.OnChapterDiaglogEnd:
                    {
                        int m_currentGroup = (int)notification.Body;
                        int m_country = (int)m_playerProxy.GetCivilization() / 100;
                        int chapterId = (int)m_playerProxy.CurrentRoleInfo.chapterId;
                        TaskChapterDataDefine TaskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>(chapterId);
                        if(TaskChapterDataDefine!=null)
                        { 
                        TaskChapterDataDefine lasttaskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>(chapterId - 1);
                            if (lasttaskChapterDataDefine != null)
                            {
                                if (lasttaskChapterDataDefine.dialogEnd.Count >= m_country)
                                {
                                    if (m_currentGroup == lasttaskChapterDataDefine.dialogEnd[m_country - 1])
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.ChapterTimelineShow, TaskChapterDataDefine);

                                    }
                                }
                                if (TaskChapterDataDefine.dialogBegin.Count >= m_country)
                                {
                                    if (m_currentGroup == TaskChapterDataDefine.dialogBegin[m_country - 1])
                                    {
                                        if (chapterId == 2) //第一章节任务完成 直接进入 加入联盟弱引导
                                        {
                                            var allianceProxy  = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                                            if (allianceProxy.HasJionAlliance())
                                            {
                                              //  CoreUtils.uiManager.ShowUI(UI.s_Taskinfo);
                                            }
                                            else
                                            {
                                                //CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                                                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.GoOnTaskGuide);
                                            }
                                        }
                                        else
                                        {
                                          //  CoreUtils.uiManager.ShowUI(UI.s_Taskinfo);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    break;
                case CmdConstant.TaskRewardEnd:
                    {
                        int m_country = (int)m_playerProxy.GetCivilization() / 100;
                        TaskChapterDataDefine TaskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int)m_playerProxy.CurrentRoleInfo.chapterId);
                        TaskChapterDataDefine lasttaskChapterDataDefine = null;
                        if (m_playerProxy.CurrentRoleInfo.chapterId == -1)
                        {
                            lasttaskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>(5);
                           
                        }
                        else
                        {
                            lasttaskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int)m_playerProxy.CurrentRoleInfo.chapterId - 1);
                        }
                        if (lasttaskChapterDataDefine != null)
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_Taskinfo);
                            if (lasttaskChapterDataDefine.dialogEnd.Count >= m_country)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ShowChapterDiaglog, lasttaskChapterDataDefine.dialogEnd[m_country - 1]);
                            }
                            else
                            {
                                if (TaskChapterDataDefine != null)
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ChapterTimelineShow, TaskChapterDataDefine);
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.OnNPCDiaglogSkip:
                    {

                    }
                    break;

                default: break;
            }

        }
        private void OpenChapterReward()
        {
            PlayerProxy m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            RewardGetData rewardGetData = new RewardGetData();
            TaskChapterDataDefine taskChapterDataDefine = null;
            if (m_playerProxy.CurrentRoleInfo.chapterId != -1)
            {
                taskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int)m_playerProxy.CurrentRoleInfo.chapterId - 1);
            }
            else
            {
                taskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>(5);
            }
            if (taskChapterDataDefine != null)
            {
                RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(taskChapterDataDefine.reward);
            }
            if (rewardGetData.rewardGroupDataList.Count != 0)
            {
                rewardGetData.playSound = true;
                rewardGetData.playTitleEffect = true;
                rewardGetData.CloseCallback = () =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.TaskRewardEnd);
                };
                CoreUtils.uiManager.CloseUI(UI.s_Taskinfo);
                AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
            }
            else
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.TaskRewardEnd);
            }
        }
    }
}