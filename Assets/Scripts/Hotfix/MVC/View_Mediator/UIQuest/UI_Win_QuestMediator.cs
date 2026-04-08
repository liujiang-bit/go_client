// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月13日
// Update Time         :    2020年1月13日
// Class Description   :    UI_Win_QuestMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;
using SprotoType;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class TaskDataItem
    {
        /// <summary>
        /// 1,主线任务title， 2主线任务 ，3 支线任务title 4，支线任务
        /// </summary>
        public int type;
        public TaskData taskData;
        public string desc;
        public TaskState taskState;
    }
    public class UI_Win_QuestMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_QuestMediator";

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private TaskChapterDataDefine m_curTaskChapterData = new TaskChapterDataDefine();//当前章节
        private List<TaskData> m_taskChapterList = new List<TaskData>();//章节任务
        private List<TaskDataItem> m_taskPageMainList = new List<TaskDataItem>();//主线任务页签任务
        private List<TaskDataItem> m_taskPageDailyList = new List<TaskDataItem>();//每日任务页签任务

        private List<RewardGroupData> m_itemPackageShowList = new List<RewardGroupData>();//当前章节奖励列表

        TaskActivityRewardDefine taskActivityRewardDefine = null;// 活跃度奖励
        private bool m_assetsReady = false;

        private Timer m_timer;

        private DataProxy m_dataProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;
        private BagProxy m_bagProxy;
        private TaskProxy m_taskProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        private EnumTaskPageType m_taskPageType;

        private bool mainAniming = false;
        private UI_Pop_BoxRewardView powTipView;
        private HelpTip m_tipView;

        private int redPoint1;
        private int redPointMain;
        private int redPointSide;
        private int redPointDaily;
        private int redPointActivePointRewards;
        private bool m_isFirstRefresh = true;
        #endregion

        //IMediatorPlug needs
        public UI_Win_QuestMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
            this.IsOpenUpdate = true;
        }


        public UI_Win_QuestView view;



        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
               Task_TaskFinish.TagName,
               Task_ChapterFinish.TagName,
                 CmdConstant.ActivePointChange,
                 CmdConstant.ActivePointRewardsChange,
                 Task_TaskInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Task_ChapterFinish.TagName:
                    {
                        Task_ChapterFinish.response response = notification.Body as Task_ChapterFinish.response;
                        if (response != null)
                        {
                            if (m_playerProxy.CurrentRoleInfo.chapterId != -1)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ChapterIdChange, response.chapterId);
                            }
                            else
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ChapterIdChange, response.chapterId);
                            }
                        }
                    }
                    break;
                case Task_TaskFinish.TagName:
                    {
                        Task_TaskFinish.response response = notification.Body as Task_TaskFinish.response;
                        if (response != null)
                        {
                            if (response.taskId / 100000 == 1)
                            {
                                RefreshData();
                                RefreshView();
                                view.m_sv_chapterQuest_ListView.FillContent(m_taskChapterList.Count);
                                view.m_sv_chapterQuest_ListView.ForceRefresh();
                                view.m_sv_list_view_ListView.ForceRefresh();

                                AppFacade.GetInstance().SendNotification(CmdConstant.TaskFinishToGuide, response.taskId);
                            }
                            else if (response.taskId / 100000 == 2)
                            {
                                redPointMain = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskMain);
                                RefreshRedPointView(EnumTaskPageType.TaskSide);
                            }
                        }
                        else
                        {
                            RefreshData();
                            RefreshView();
                            view.m_sv_chapterQuest_ListView.FillContent(m_taskChapterList.Count);
                            view.m_sv_chapterQuest_ListView.ForceRefresh();
                            view.m_sv_list_view_ListView.ForceRefresh();

                            view.m_sv_questDaily_ListView.FillContent(m_taskPageDailyList.Count);
                            view.m_sv_questDaily_ListView.ForceRefresh();

                            view.m_sv_questMain_ListView.FillContent(m_taskPageMainList.Count);
                            view.m_sv_questMain_ListView.ForceRefresh();
                        }
                    }
                    break;
                case CmdConstant.ActivePointChange:
                case CmdConstant.ActivePointRewardsChange:
                    RefreshDailyRedPointData();
                    RefreshDailyView();
                    RefreshRedPointView();
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
           // CoreUtils.assetService.Destroy(m_assetDic["UI_Item_QuestReward"]);//TODO:会报错，先删掉
        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {
            if (animShow)
            {
                if (m_deleteItem != null)
                {
                    if (m_deleteItem.data is TaskDataItem)
                    {
                        TaskDataItem taskDataItem = m_deleteItem.data as TaskDataItem;
                        m_deleteTime = m_deleteTime - Time.deltaTime;
                        if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskMain)
                        {
                        
                        }
                        else if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskSide)
                        {

                            view.m_sv_questMain_ListView.UpdateItemSize(m_deleteItem.index, m_height * m_deleteItem.go.transform.localScale.y);
                            if (m_deleteTime <= 0)
                            {
                                PlayAnimEnd(m_deleteItem);
                                view.m_sv_questMain_ListView.RemoveAt(m_deleteItem.index);
                                m_deleteItem = null;
                                animShow = false;
                            }
                        }
                        else if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskDaily)
                        {
                            view.m_sv_questDaily_ListView.UpdateItemSize(m_deleteItem.index, m_height * m_deleteItem.go.transform.localScale.y);
                            if (m_deleteTime <= 0)
                            {
                                PlayAnimEnd(m_deleteItem);
                                view.m_sv_questDaily_ListView.RemoveAt(m_deleteItem.index);
                                m_deleteItem = null;
                                animShow = false;
                            }
                        }
                    }
                }
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            m_dataProxy = AppFacade.GetInstance().RetrieveProxy(DataProxy.ProxyNAME) as DataProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

            if (view.data is int)
            {
                m_taskPageType = (EnumTaskPageType)(int)view.data;
            }
            else
            {
                m_taskPageType = EnumTaskPageType.TaskChapter;
            }

            if (m_playerProxy.CurrentRoleInfo.chapterId != -1)
            {
                m_curTaskChapterData = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int)m_playerProxy.CurrentRoleInfo.chapterId);
                m_itemPackageShowList = m_rewardGroupProxy.GetRewardDataByGroup(m_curTaskChapterData.reward);
                m_taskChapterList = m_taskProxy.GetTaskChapterListByChapter(m_playerProxy.CurrentRoleInfo.chapterId);
            }
            m_taskPageMainList = m_taskProxy.GetTaskListPageMain();
            m_taskPageDailyList = m_taskProxy.GetTaskListPagDaily();
            taskActivityRewardDefine = CoreUtils.dataService.QueryRecord<TaskActivityRewardDefine>((int)m_playerProxy.CurrentRoleInfo.level);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_PageButton_1.m_btn_btn_GameButton.onClick.AddListener(OnTaskCaptionBtnClick);
            view.m_UI_Item_QuestBox20.AddClickEvent(() => { OnTaskDailyBoxBtnClick(taskActivityRewardDefine.activePoints1, taskActivityRewardDefine.reward1, view.m_UI_Item_QuestBox20); });
            view.m_UI_Item_QuestBox40.AddClickEvent(() => { OnTaskDailyBoxBtnClick(taskActivityRewardDefine.activePoints2, taskActivityRewardDefine.reward2, view.m_UI_Item_QuestBox40); });
            view.m_UI_Item_QuestBox60.AddClickEvent(() => { OnTaskDailyBoxBtnClick(taskActivityRewardDefine.activePoints3, taskActivityRewardDefine.reward3, view.m_UI_Item_QuestBox60); });
            view.m_UI_Item_QuestBox80.AddClickEvent(() => { OnTaskDailyBoxBtnClick(taskActivityRewardDefine.activePoints4, taskActivityRewardDefine.reward4, view.m_UI_Item_QuestBox80); });
            view.m_UI_Item_QuestBox100.AddClickEvent(() => { OnTaskDailyBoxBtnClick(taskActivityRewardDefine.activePoints5, taskActivityRewardDefine.reward5, view.m_UI_Item_QuestBox100); });
            view.m_UI_Model_PageButton_2.m_btn_btn_GameButton.onClick.AddListener(OnTaskMainBtnClick);
            view.m_UI_Model_PageButton_3.m_btn_btn_GameButton.onClick.AddListener(OnTaskDailyBtnClick);
            view.m_UI_Model_Window.m_btn_close_GameButton.onClick.AddListener(() => { CoreUtils.uiManager.CloseUI(UI.s_Taskinfo); });
        }

        protected override void BindUIData()
        {
            InitView();
            m_preLoadRes.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_chapterQuest_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_questDaily_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_questMain_ListView.ItemPrefabDataList);
            m_preLoadRes.Add("UI_Item_QuestReward");

                ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
                {
                    m_assetDic = assetDic;
                    m_assetsReady = true;
                    InitItems();
                    JumpTaskPage();
                });
        }

        #endregion

        private void JumpTaskPage()
        {
            switch (m_taskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    {
                        if (m_playerProxy.CurrentRoleInfo.chapterId != -1)
                        {
                            OnTaskCaptionBtnClick();
                        }
                        else
                        {
                            OnTaskMainBtnClick();
                        }
                    }
                    break;
                case EnumTaskPageType.TaskMain:
                    {
                        OnTaskMainBtnClick();
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        OnTaskDailyBtnClick();
                    }
                    break;
                default:
                    {
                        OnTaskMainBtnClick();
                    }
                    break;
            }
        }
        private void InitView()
        {
            view.m_img_char2_SkeletonGraphic.gameObject.SetActive(false);
            view.m_pl_chapter.gameObject.SetActive(false);
            view.m_sv_questMain_ListView.gameObject.SetActive(false);
            view.m_pl_daily.gameObject.SetActive(false);
            if (m_playerProxy.CurrentRoleInfo.chapterId == -1)
            {
                view.m_UI_Model_PageButton_1.gameObject.SetActive(false);
            }
            if (!m_taskProxy.ShowOtherTask())
            {
                view.m_UI_Model_PageButton_2.gameObject.SetActive(false);
                view.m_UI_Model_PageButton_3.gameObject.SetActive(false);
            }
            view.m_UI_Model_PageButton_1.m_lbl_reddotcount_LanguageText.gameObject.SetActive(true);
            view.m_UI_Model_PageButton_2.m_lbl_reddotcount_LanguageText.gameObject.SetActive(true);
            view.m_UI_Model_PageButton_3.m_lbl_reddotcount_LanguageText.gameObject.SetActive(true);
            view.m_pb_ap_GameSlider.minValue = 0;
            view.m_pb_ap_GameSlider.maxValue = 1;
            view.m_pb_ap_GameSlider.wholeNumbers = false;

        }

        private void RefreshTimer()
        {
            view.m_lbl_timetext_LanguageText.text = ClientUtils.FormatTimeTroop((int)ServerTimeModule.Instance.GetDistanceZeroTime());
        }

        private void RefreshData()
        {
            if (m_playerProxy.CurrentRoleInfo.chapterId != -1)
            {
                m_curTaskChapterData = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int)m_playerProxy.CurrentRoleInfo.chapterId);
                m_itemPackageShowList = m_rewardGroupProxy.GetRewardDataByGroup(m_curTaskChapterData.reward);
                m_taskChapterList = m_taskProxy.GetTaskChapterListByChapter(m_playerProxy.CurrentRoleInfo.chapterId);
            }
            
            m_taskPageMainList = m_taskProxy.GetTaskListPageMain();
            m_taskPageDailyList = m_taskProxy.GetTaskListPagDaily();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        void RefreshView()
        {
            int TaskChapterFinishCount = 0, TaskChapterCount = 0;
            if (m_playerProxy.CurrentRoleInfo.chapterId != -1)
            {
                TaskChapterFinishCount = m_taskProxy.GetTaskChapterFinishCount(m_playerProxy.CurrentRoleInfo.chapterId, out TaskChapterCount);

                if (TaskChapterFinishCount == TaskChapterCount)
                {
                    view.m_UI_Model_btn.gameObject.SetActive(true);
                    view.m_lbl_progress_LanguageText.gameObject.SetActive(false);
                    view.m_UI_Model_btn.m_lbl_Text_LanguageText.text = LanguageUtils.getText(700005);
                    view.m_UI_Model_btn.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    view.m_UI_Model_btn.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                        Task_ChapterFinish.request request = new Task_ChapterFinish.request();
                        AppFacade.GetInstance().SendSproto(request);
                        view.m_UI_Model_btn.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    });
                }
                else
                {
                    view.m_UI_Model_btn.gameObject.SetActive(false);
                    view.m_lbl_progress_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_progress_LanguageText.text = LanguageUtils.getTextFormat(700022, TaskChapterFinishCount, TaskChapterCount);
                    view.m_UI_Model_btn.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                }
                RefreshView2();
                RefreshView3();
            }
            RefreshRedPointData();
            RefreshRedPointView();
            RefreshDailyView();
        }
        /// <summary>
        /// 这里有个未找到的报错，拆开函数定位
        /// </summary>
        void RefreshView2()
        {
                view.m_lbl_text_LanguageText.text = LanguageUtils.getText(m_curTaskChapterData.l_descId2);
                int imgAgeResindex = ((int)m_cityBuildingProxy.GetAgeType()) - 1;
                if (m_curTaskChapterData.imgAgeRes.Count > imgAgeResindex + 1)
                {
                    ClientUtils.LoadSpine(view.m_img_char2_SkeletonGraphic, m_curTaskChapterData.imgAgeRes[imgAgeResindex], () =>
                    {
                        view.m_img_char2_SkeletonGraphic.gameObject.SetActive(true);
                    });
                }
                else
                {

                }
        }
        void RefreshView3()
        {
                view.m_UI_Model_PageButton_2.gameObject.SetActive(m_taskProxy.ShowOtherTask());
                view.m_UI_Model_PageButton_3.gameObject.SetActive(m_taskProxy.ShowOtherTask());
                switch (m_taskPageType)
                {
                    case EnumTaskPageType.TaskChapter:
                        view.m_UI_Model_Window.m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(700024, LanguageUtils.getText(m_curTaskChapterData.l_titleNameId), LanguageUtils.getText(m_curTaskChapterData.l_descId1));
                        break;
                    case EnumTaskPageType.TaskMain:
                        view.m_UI_Model_Window.m_lbl_title_LanguageText.text = LanguageUtils.getText(700000);
                        break;
                    case EnumTaskPageType.TaskDaily:
                        view.m_UI_Model_Window.m_lbl_title_LanguageText.text = LanguageUtils.getText(700000);
                        break;
            }
        }
        void RefreshRedPointData()
        {
            redPoint1 = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskChapter);
            redPointMain = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskMain);
            redPointSide = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskSide);
            redPointDaily = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskDaily);
            RefreshDailyRedPointData();
        }
        void RefreshRedPointView()
        {
            string s_redPoint1 = UIHelper.NumerBeyondFormat(redPoint1);
            string s_redPoint2 = UIHelper.NumerBeyondFormat(redPointSide+ redPointMain);
            string s_redPoint3 = UIHelper.NumerBeyondFormat(redPointDaily + redPointActivePointRewards);
            view.m_UI_Model_PageButton_1.m_img_redpot_PolygonImage.gameObject.SetActive(redPoint1 != 0);
            view.m_UI_Model_PageButton_1.m_lbl_reddotcount_LanguageText.text = s_redPoint1;
            view.m_UI_Model_PageButton_2.m_img_redpot_PolygonImage.gameObject.SetActive((redPointMain+redPointSide) != 0);
            view.m_UI_Model_PageButton_2.m_lbl_reddotcount_LanguageText.text = s_redPoint2;
            view.m_UI_Model_PageButton_3.m_img_redpot_PolygonImage.gameObject.SetActive((redPointDaily + redPointActivePointRewards) != 0);
            view.m_UI_Model_PageButton_3.m_lbl_reddotcount_LanguageText.text = s_redPoint3;
        }
        void RefreshRedPointView(EnumTaskPageType enumTaskPageType)
        {
            switch (enumTaskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    {
                        string s_redPoint1 = UIHelper.NumerBeyondFormat(redPoint1);
                        view.m_UI_Model_PageButton_1.m_img_redpot_PolygonImage.gameObject.SetActive(redPoint1 != 0);
                        view.m_UI_Model_PageButton_1.m_lbl_reddotcount_LanguageText.text = s_redPoint1;
                    }
                    break;
                case EnumTaskPageType.TaskMain:
                case EnumTaskPageType.TaskSide:
                    {
                        string s_redPoint2 = UIHelper.NumerBeyondFormat(redPointMain + redPointSide);
                        view.m_UI_Model_PageButton_2.m_img_redpot_PolygonImage.gameObject.SetActive((redPointMain + redPointSide) != 0);
                        view.m_UI_Model_PageButton_2.m_lbl_reddotcount_LanguageText.text = s_redPoint2;
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    {
                        string s_redPoint3 = UIHelper.NumerBeyondFormat(redPointDaily+redPointActivePointRewards);
                        view.m_UI_Model_PageButton_3.m_img_redpot_PolygonImage.gameObject.SetActive((redPointDaily+redPointActivePointRewards) != 0);
                        view.m_UI_Model_PageButton_3.m_lbl_reddotcount_LanguageText.text = s_redPoint3;
                    }
                    break;
            }    
        }
        void RefreshDailyRedPointData()
        {
            redPointActivePointRewards = m_taskProxy.GetRedPointActivePointRewards();
        }
            void RefreshDailyView()
        {

            long activePoint = m_playerProxy.CurrentRoleInfo.activePoint;
            view.m_UI_Item_QuestBox20.SetClose();
            view.m_UI_Item_QuestBox40.SetClose();
            view.m_UI_Item_QuestBox60.SetClose();
            view.m_UI_Item_QuestBox80.SetClose();
            view.m_UI_Item_QuestBox100.SetClose();
            if (m_playerProxy.CurrentRoleInfo.activePointRewards != null)
            {
                m_playerProxy.CurrentRoleInfo.activePointRewards.ForEach((rewardActivePoint) =>
                {
                    if (rewardActivePoint == taskActivityRewardDefine.activePoints1)
                    {
                        view.m_UI_Item_QuestBox20.SetOpened();
                    }
                    else if (rewardActivePoint == taskActivityRewardDefine.activePoints2)
                    {
                        view.m_UI_Item_QuestBox40.SetOpened();
                    }
                    else if (rewardActivePoint == taskActivityRewardDefine.activePoints3)
                    {
                        view.m_UI_Item_QuestBox60.SetOpened();
                    }
                    else if (rewardActivePoint == taskActivityRewardDefine.activePoints4)
                    {
                        view.m_UI_Item_QuestBox80.SetOpened();
                    }
                    else if (rewardActivePoint == taskActivityRewardDefine.activePoints5)
                    {
                        view.m_UI_Item_QuestBox100.SetOpened();
                    }
                });
            }

            if (activePoint >= taskActivityRewardDefine.activePoints1)
            {
                view.m_UI_Item_QuestBox20.SetStay();
            }
            if (activePoint >= taskActivityRewardDefine.activePoints2)
            {
                view.m_UI_Item_QuestBox40.SetStay();
            }
            if (activePoint >= taskActivityRewardDefine.activePoints3)
            {
                view.m_UI_Item_QuestBox60.SetStay();
            }
            if (activePoint >= taskActivityRewardDefine.activePoints4)
            {
                view.m_UI_Item_QuestBox80.SetStay();
            }
            if (activePoint >= taskActivityRewardDefine.activePoints5)
            {
                view.m_UI_Item_QuestBox100.SetStay();
            }
            if (m_isFirstRefresh)
            {
                view.m_pb_ap_GameSlider.value = activePoint/100f;
                m_isFirstRefresh = false;
            }
            else
            {
                view.m_pb_ap_SmoothBar.SetValue(activePoint/100f);
            }
            view.m_lbl_aptext_LanguageText.text = LanguageUtils.getTextFormat(700024, LanguageUtils.getText(700006), activePoint);

        }
        #region 点击事件
        private void OnTaskDailyBoxBtnClick(int activePoint, int reward, UI_Item_QuestBox_SubView subView)
        {
            if (m_playerProxy.CurrentRoleInfo.activePointRewards != null)
            {

                if (m_playerProxy.CurrentRoleInfo.activePointRewards.Contains(activePoint))
                {
                    ShowBoxReward(true, activePoint, reward, subView.m_root_RectTransform.position);
                }
                else
                {
                    if (m_playerProxy.CurrentRoleInfo.activePoint >= activePoint)
                    {
                        Task_TakeActivePointReward.request request = new Task_TakeActivePointReward.request();
                        request.activePoint = activePoint;
                        AppFacade.GetInstance().SendSproto(request);
                        subView.SetOpen();
                        ClientUtils.UIAddEffect(RS.TaskBoxBgOpenEffect, subView.m_root_RectTransform, null);
                        Timer.Register(0.5f, () =>
                        {
                            Transform transform = subView.m_root_RectTransform.Find(RS.TaskBoxBgOpenEffect);
                            if (transform != null)
                            {
                                CoreUtils.assetService.Destroy(transform.gameObject);
                            }
                            RewardGetData rewardGetData = new RewardGetData();
                            RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                            rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(reward);

                            if (rewardGetData.rewardGroupDataList.Count != 0)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                            }
                        }, null, false, false, view.vb);

                    }
                    else
                    {
                        //  Tip.CreateTip("显示没有打开的箱子帮助tip").Show();
                        ShowBoxReward(false, activePoint, reward, subView.m_root_RectTransform.position);

                    }
                }
            }
        }
        public void GetBoxByAction()
        {

        }
        /// <summary>
        /// 显示礼物列表
        /// </summary>
        /// <param name="open"></param>
        /// <param name="reward"></param>
        private void ShowBoxReward(bool open, int activePoint, int reward, Vector3 position)
        {
            CoreUtils.assetService.Instantiate("UI_Pop_BoxReward", (obj) =>
            {
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_BoxRewardView>(obj);
                GameObject childobj = powTipView.m_UI_Item_BoxTipsItem.gameObject;
                childobj.SetActive(false);
                GrayChildrens makeChildrenGray = powTipView.m_pl_info_MakeChildrenGray;

                RewardGetData rewardGetData = new RewardGetData();
                RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(reward);
                rewardGetData.rewardGroupDataList.ForEach((RewardGroupData) =>
                {
                    GameObject go = CoreUtils.assetService.Instantiate(childobj);
                    go.transform.SetParent(powTipView.m_pl_boxTips_GridLayoutGroup.transform);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    UI_Item_BoxTipsItem_SubView subView = new UI_Item_BoxTipsItem_SubView(go.GetComponent<RectTransform>());
                    subView.m_pl_item.Refresh(RewardGroupData);
                    subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(RewardGroupData.name);
                    subView.m_lbl_num_LanguageText.text = RewardGroupData.number.ToString("N0");
                });
                if (open)
                {
                    powTipView.m_lbl_dec_LanguageText.text = LanguageUtils.getText(700031);
                    if (makeChildrenGray != null)
                    {
                        makeChildrenGray.Gray();
                    }
                }
                else
                {
                    powTipView.m_lbl_dec_LanguageText.text = LanguageUtils.getTextFormat(700030, activePoint);
                    if (makeChildrenGray != null)
                    {
                        makeChildrenGray.Normal();
                    }
                }
                ClientUtils.UIReLayout(powTipView.m_pl_boxTips_GridLayoutGroup.gameObject);
                ClientUtils.UIReLayout(powTipView.m_pl_info_VerticalLayoutGroup.gameObject);

                int itemHeight = (int)powTipView.m_pl_boxTips_GridLayoutGroup.cellSize.y;
                int rewardCount = rewardGetData.rewardGroupDataList.Count;
                float decHeight = powTipView.m_lbl_dec_LanguageText.rectTransform.sizeDelta.y;
                Vector2 sizeDelta = new Vector2(powTipView.m_pl_boxTips_GridLayoutGroup.GetComponent<RectTransform>().sizeDelta.x, decHeight + rewardCount * itemHeight);
                RectTransform content = powTipView.m_pl_info_VerticalLayoutGroup.GetComponent<RectTransform>();
                content.anchorMin = new Vector2(0.5f,0.5f);
                content.anchorMax = new Vector2(0.5f,0.5f);
                content.anchoredPosition = Vector2.zero;
                content.sizeDelta = new Vector2(sizeDelta.x,sizeDelta.y);
                m_tipView = HelpTip.CreateTip(powTipView.gameObject, sizeDelta, position).SetOffset(60)
                    .SetStyle(HelpTipData.Style.arrowUp).TouchCloseOneselfClose() .Show();

         
            });
        }
        private void OnTaskCaptionBtnClick()
        {
            if (m_assetsReady)
            {
                RefreshData();
                RefreshView();
                view.m_sv_chapterQuest_ListView.FillContent(m_taskChapterList.Count);
                view.m_sv_chapterQuest_ListView.ForceRefresh();
                view.m_sv_list_view_ListView.ForceRefresh();
                ShowBuildingGroupTypeList(EnumTaskPageType.TaskChapter);
            }
        }
        private void OnTaskMainBtnClick()
        {
            if (m_assetsReady)
            {
                RefreshData();
                RefreshView();
                view.m_sv_questMain_ListView.FillContent(m_taskPageMainList.Count);
                view.m_sv_questMain_ListView.ForceRefresh();
                ShowBuildingGroupTypeList(EnumTaskPageType.TaskMain);

            }

        }
        private void OnTaskDailyBtnClick()
        {
            if (m_assetsReady)
            {
                RefreshData();
                RefreshView();
                view.m_sv_questDaily_ListView.FillContent(m_taskPageDailyList.Count);
                view.m_sv_questDaily_ListView.ForceRefresh();

                ShowBuildingGroupTypeList(EnumTaskPageType.TaskDaily);
            }
        }
        #endregion

        void ItemTaskChapterEnter(ListView.ListItem scrollItem)
        {
            UI_Item_ChapterQuest_SubView itemView = null;

            var taskChapter = m_taskChapterList[scrollItem.index];
            Transform rewards = null;
            if (taskChapter != null)
            {
                scrollItem.go.SetActive(true);
                if (scrollItem.data == null)
                {
                    itemView = new UI_Item_ChapterQuest_SubView(scrollItem.go.GetComponent<RectTransform>());
                    rewards = itemView.gameObject.transform.Find("content/reward/rewards").transform;
                    itemView.m_UI_Model_Green.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                        Task_TaskFinish.request req = new Task_TaskFinish.request();
                        req.taskId = m_taskChapterList[itemView.Index].taskID;
                        AppFacade.GetInstance().SendSproto(req);
                        FlyRewardEffect(rewards, m_taskChapterList[itemView.Index].rewardGroupDataList);
                    });
                    itemView.m_UI_Model_Blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.GoScript, m_taskChapterList[itemView.Index]);
                    });
                    scrollItem.data = itemView;
                }
                else
                {
                    itemView = scrollItem.data as UI_Item_ChapterQuest_SubView;
                    rewards = itemView.gameObject.transform.Find("content/reward/rewards").transform;
                }
                itemView.Index = scrollItem.index;
                itemView.gameObject.name = taskChapter.taskID.ToString();
                itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getTextFormat(700014, taskChapter.desc, LanguageUtils.getTextFormat(181104, taskChapter.Num, taskChapter.needNum));

                int count = rewards.childCount;
                for (int i = count - 1; i >= 0; i--)
                {
                    rewards.GetChild(i).gameObject.SetActive(false);
                }
                GameObject temp = rewards.GetChild(0).gameObject;
                for (int i = 0; i < taskChapter.rewardGroupDataList.Count; i++)
                {
                    GameObject gameObject = null;
                    if (i < count)
                    {
                        gameObject = rewards.GetChild(i).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(temp);
                    }
                    gameObject.transform.SetParent(rewards);
                    gameObject.SetActive(true);
                    UI_Model_ResourcesConsumeInCQ_SubView resourcesConsumeInCQ_SubView = new UI_Model_ResourcesConsumeInCQ_SubView(gameObject.GetComponent<RectTransform>());
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    resourcesConsumeInCQ_SubView.SetResourcesConsumeReward(taskChapter.rewardGroupDataList[i]);
                }
                TaskState taskState = taskChapter.taskState;
                if (taskState == TaskState.finished)
                {
                    itemView.m_UI_Model_Green.gameObject.SetActive(true);
                    itemView.m_UI_Model_Blue.gameObject.SetActive(false);
                    itemView.m_lbl_finish_LanguageText.gameObject.SetActive(false);
                    itemView.m_UI_Model_Green.m_lbl_Text_LanguageText.text = LanguageUtils.getText(700005);
                }
                else if (taskState == TaskState.unfinished)
                {
                    itemView.m_UI_Model_Green.gameObject.SetActive(false);
                    itemView.m_UI_Model_Blue.gameObject.SetActive(true);
                    itemView.m_lbl_finish_LanguageText.gameObject.SetActive(false);
                    itemView.m_UI_Model_Blue.m_lbl_Text_LanguageText.text = LanguageUtils.getText(124006);

                }
                else if (taskState == TaskState.received)
                {
                    itemView.m_UI_Model_Green.gameObject.SetActive(false);
                    itemView.m_UI_Model_Blue.gameObject.SetActive(false);
                    itemView.m_lbl_finish_LanguageText.gameObject.SetActive(true);
                }


            }
        }
        private float GetItemSizeReward(ListView.ListItem listItem)
        {
            return 134f;
        }
        private string GetItemPrefabNamePageMain(ListView.ListItem listItem)
        {
            int index = listItem.index;
            TaskDataItem taskItemData = m_taskPageMainList[index];
            if (taskItemData.type == 1)
            {
                return "UI_Item_QuestMainTag";
            }
            else if (taskItemData.type == 3)
            {
                return "UI_Item_QuestBranchTag";
            }
            else if (taskItemData.type == 2)
            {
                return "UI_Item_QuestMain";
            }
            else
            {
                return "UI_Item_Quest";
            }

        }
        private string GetItemPrefabNamePageDaily(ListView.ListItem listItem)
        {
            return "UI_Item_Quest";
        }

        private float GetItemSize(ListView.ListItem listItem)
        {
            int index = listItem.index;
            TaskDataItem taskItemData = m_taskPageMainList[index];
            if (taskItemData.type == 1)
            {
                return m_assetDic["UI_Item_QuestMainTag"].GetComponent<RectTransform>().sizeDelta.y;
            }
            else if (taskItemData.type == 3)
            {
                return m_assetDic["UI_Item_QuestBranchTag"].GetComponent<RectTransform>().sizeDelta.y;
            }
            else if (taskItemData.type == 2)
            {
                return m_assetDic["UI_Item_Quest"].GetComponent<RectTransform>().sizeDelta.y;
            }
            else
            {
                return m_assetDic["UI_Item_Quest"].GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        void ItemTaskSideEnter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            TaskDataItem taskDataItem = m_taskPageMainList[index];

            if (taskDataItem.type == 2 || taskDataItem.type == 4)
            {
                UI_Item_QuestView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_QuestView>(scrollItem.go);
                scrollItem.data = taskDataItem;
                switch (taskDataItem.taskData.taskPageType)
                {
                    case EnumTaskPageType.TaskMain:
                        if (!string.Equals(itemView.m_img_icon_PolygonImage.assetName, taskDataItem.taskData.taskMainDefine.iconImg))
                        {
                            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, taskDataItem.taskData.taskMainDefine.iconImg, false);
                        }
                        break;
                    case EnumTaskPageType.TaskSide:
                        if (!string.Equals(itemView.m_img_icon_PolygonImage.assetName, taskDataItem.taskData.taskSideDefine.iconImg))
                        {
                            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, taskDataItem.taskData.taskSideDefine.iconImg, false);
                        }
                        break;
                }

                itemView.m_lbl_itemDesc_LanguageText.text = LanguageUtils.getTextFormat(700014, taskDataItem.taskData.desc, LanguageUtils.getTextFormat(181104, taskDataItem.taskData.Num, taskDataItem.taskData.needNum));
                scrollItem.go.name = taskDataItem.taskData.taskID.ToString();
                itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(taskDataItem.taskData.l_nameId);
                int objCount = itemView.m_pl_rewards_GridLayoutGroup.transform.childCount;
                int rewardCount = taskDataItem.taskData.rewardGroupDataList.Count;
                if (objCount > rewardCount)
                {
                    for (int i = objCount - 1; i >=  rewardCount; i--)
                    {
                        itemView.m_pl_rewards_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                for (int i = 0; i < rewardCount; i++)
                {
                    GameObject gameObject = null;
                    if (i < objCount)
                    {
                        gameObject = itemView.m_pl_rewards_GridLayoutGroup.transform.GetChild(i).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_QuestReward"]);
                    }
                    gameObject.transform.SetParent(itemView.m_pl_rewards_GridLayoutGroup.transform);
                    gameObject.SetActive(true);
                    gameObject.name = taskDataItem.taskData.rewardGroupDataList[i].RewardType.ToString();
                    UI_Item_QuestReward_SubView questReward_SubView = new UI_Item_QuestReward_SubView(gameObject.GetComponent<RectTransform>());
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(taskDataItem.taskData.rewardGroupDataList[i]);
                    questReward_SubView.m_lbl_languageText_LanguageText.text = taskDataItem.taskData.rewardGroupDataList[i].number.ToString("N0");
                }
                if (taskDataItem.taskData.taskState == TaskState.finished)
                {
                    itemView.m_UI_Model_green.gameObject.SetActive(true);
                    itemView.m_UI_Model_blue.gameObject.SetActive(false);
                    itemView.m_UI_Model_green.m_lbl_Text_LanguageText.text = LanguageUtils.getText(700005);
                    itemView.m_UI_Model_green.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Model_green.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {

                        if (taskDataItem.taskData.taskState == TaskState.finished)
                        {
                            if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskMain)
                            {
                                if (!mainAniming)
                                {
                                    FlyRewardEffect(itemView.m_pl_rewards_GridLayoutGroup.transform, taskDataItem.taskData.rewardGroupDataList);
                                    OnTaskDataItemClick(scrollItem);
                                }
                            }
                            else if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskSide)
                            {
                                if (!animShow)
                                {
                                    FlyRewardEffect(itemView.m_pl_rewards_GridLayoutGroup.transform, taskDataItem.taskData.rewardGroupDataList);
                                    OnTaskDataItemClick(scrollItem);
                                }
                            }
                        }
                    });
                }
                else
                {
                    itemView.m_UI_Model_green.gameObject.SetActive(false);
                    itemView.m_UI_Model_blue.gameObject.SetActive(true);
                    itemView.m_UI_Model_blue.m_lbl_Text_LanguageText.text = LanguageUtils.getText(124006);
                    itemView.m_UI_Model_blue.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Model_blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.GoScript, taskDataItem.taskData);
                    });
                }
            }
        }
        void ItemTaskDailyEnter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            TaskDataItem taskDataItem = m_taskPageDailyList[index];
            UI_Item_QuestView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_QuestView>(scrollItem.go);
            scrollItem.data = taskDataItem;
            if (!string.Equals(itemView.m_img_icon_PolygonImage.assetName, taskDataItem.taskData.taskDailyDefine.iconImg))
            {
                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, taskDataItem.taskData.taskDailyDefine.iconImg, false);
            }

            itemView.m_lbl_itemDesc_LanguageText.text = LanguageUtils.getTextFormat(700014, taskDataItem.taskData.desc, LanguageUtils.getTextFormat(181104, taskDataItem.taskData.Num, taskDataItem.taskData.needNum));
            scrollItem.go.name = taskDataItem.taskData.taskID.ToString();
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(taskDataItem.taskData.l_nameId);
            int objCount = itemView.m_pl_rewards_GridLayoutGroup.transform.childCount;
            int rewardCount = taskDataItem.taskData.rewardGroupDataList.Count;
            for (int i = objCount - 1; i >= 0; i--)
            {
                itemView.m_pl_rewards_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < rewardCount; i++)
            {
                GameObject temp = null;
                if (i < objCount)
                {
                    temp = itemView.m_pl_rewards_GridLayoutGroup.transform.GetChild(i).gameObject;
                }
                else
                {
                    temp = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_QuestReward"]);
                }
                temp.transform.SetParent(itemView.m_pl_rewards_GridLayoutGroup.transform);
                temp.SetActive(true);
                temp.name = taskDataItem.taskData.rewardGroupDataList[i].RewardType.ToString();
                UI_Item_QuestReward_SubView subView = new UI_Item_QuestReward_SubView(temp.GetComponent<RectTransform>());
                temp.transform.localScale = new Vector3(1, 1, 1);
                subView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(taskDataItem.taskData.rewardGroupDataList[i]);
                subView.m_lbl_languageText_LanguageText.text = taskDataItem.taskData.rewardGroupDataList[i].number.ToString("N0");
            }

            GameObject gameObject = null;
            if (objCount < rewardCount + 1)
            {
                gameObject = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_QuestReward"]);
            }
            else
            {
                gameObject = itemView.m_pl_rewards_GridLayoutGroup.transform.GetChild(rewardCount).gameObject;
            }
            gameObject.transform.SetParent(itemView.m_pl_rewards_GridLayoutGroup.transform);
            gameObject.SetActive(true);

            UI_Item_QuestReward_SubView questReward_SubView = new UI_Item_QuestReward_SubView(gameObject.GetComponent<RectTransform>());
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            CurrencyDefine currencyDefine = null;
            RewardGroupData rewardGroupData = new RewardGroupData();
            if (m_currencyProxy.CurrencyDefine.TryGetValue((int)EnumCurrencyType.activePoint, out currencyDefine))
            {
                rewardGroupData.RewardType = (int)EnumRewardType.Currency;
                rewardGroupData.CurrencyData = new RewardCurrencyData();
                rewardGroupData.CurrencyData.currencyDefine = currencyDefine;
                rewardGroupData.number = taskDataItem.taskData.taskDailyDefine.score;
                gameObject.name = currencyDefine.name;
                questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(currencyDefine);
                questReward_SubView.m_lbl_languageText_LanguageText.text = taskDataItem.taskData.taskDailyDefine.score.ToString("N0");
                questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.enabled = true;
                questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                float offset = questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
                questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.AddListener(() =>
                {
                    HelpTip.CreateTip(LanguageUtils.getText(currencyDefine.l_desID), questReward_SubView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                });
            }


            if (taskDataItem.taskData.taskState == TaskState.finished)
            {
                itemView.m_UI_Model_green.gameObject.SetActive(true);
                itemView.m_UI_Model_blue.gameObject.SetActive(false);
                itemView.m_UI_Model_green.m_lbl_Text_LanguageText.text = LanguageUtils.getText(700005);
                itemView.m_UI_Model_green.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_green.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    if (taskDataItem.taskData.taskState == TaskState.finished)
                    {
                        FlyRewardEffect(itemView.m_pl_rewards_GridLayoutGroup.transform, taskDataItem.taskData.rewardGroupDataList, rewardGroupData);
                        OnTaskDailyDataItemClick(scrollItem);
                        itemView.m_UI_Model_green.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                        redPointDaily--;
                        RefreshRedPointView(EnumTaskPageType.TaskDaily);
                    }
                });
            }
            else
            {
                itemView.m_UI_Model_green.gameObject.SetActive(false);
                itemView.m_UI_Model_blue.gameObject.SetActive(true);
                itemView.m_UI_Model_blue.m_lbl_Text_LanguageText.text = LanguageUtils.getText(124006);
                itemView.m_UI_Model_blue.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.GoScript, taskDataItem.taskData);
                });
            }
        }
        /// <summary>
        /// 章节奖励item
        /// </summary>
        /// <param name="scrollItem"></param>
        void ItemChapterRewardEnter(ListView.ListItem scrollItem)
        {
            ItemBagView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBagView>(scrollItem.go);
            int index = scrollItem.index;
            var itemPackageShow = m_itemPackageShowList[index];

            if (itemPackageShow != null)
            {
                scrollItem.go.SetActive(true);
                string icon = string.Empty;
                switch ((EnumRewardType)itemPackageShow.RewardType)
                {
                    case EnumRewardType.Currency:
                        {
                            CurrencyDefine currencyDefine = CoreUtils.dataService.QueryRecord<CurrencyDefine>(itemPackageShow.CurrencyData.ID);
                            itemView.m_UI_Model_Item.Refresh(currencyDefine, itemPackageShow.number, false);
                            float offset = itemView.m_UI_Model_Item.m_btn_animButton_GameButton.GetComponent<RectTransform>().sizeDelta.y / 4;
                            itemView.m_UI_Model_Item.AddBtnListener(() => { HelpTip.CreateTip(LanguageUtils.getText(currencyDefine.l_desID), itemView.m_UI_Model_Item.m_btn_animButton_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show(); });

                        }
                        break;
                    case EnumRewardType.Soldier:
                        {
                            ArmsDefine armsDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(itemPackageShow.SoldierData.ID);
                            itemView.m_UI_Model_Item.Refresh(armsDefine, itemPackageShow.number, false);
                            float offset = itemView.m_UI_Model_Item.m_btn_animButton_GameButton.GetComponent<RectTransform>().sizeDelta.y / 4;
                            itemView.m_UI_Model_Item.AddBtnListener(() => { HelpTip.CreateTip(LanguageUtils.getText(armsDefine.l_armsID), itemView.m_UI_Model_Item.m_btn_animButton_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show(); });
                        }
                        break;
                    case EnumRewardType.Item:
                        {
                            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemPackageShow.ItemData.ID);
                            itemView.m_UI_Model_Item.Refresh(itemDefine, itemPackageShow.number, false);
                            float offset = itemView.m_UI_Model_Item.m_btn_animButton_GameButton.GetComponent<RectTransform>().sizeDelta.y / 4;
                            itemView.m_UI_Model_Item.AddBtnListener(() => { HelpTip.CreateTip(LanguageUtils.getText(itemDefine.l_nameID), itemView.m_UI_Model_Item.m_btn_animButton_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show(); });
                        }
                        break;
                }
            }
        }
        private ListView.ListItem m_deleteItem;
        private float m_deleteTime;
        private float m_height;
        private bool animShow = false;
        void OnTaskDataItemClick(ListView.ListItem listItem)
        {
            if (listItem.data is TaskDataItem)
            {
                TaskDataItem taskDataItem = listItem.data as TaskDataItem;
                Task_TaskFinish.request req = new Task_TaskFinish.request();
                req.taskId = taskDataItem.taskData.taskID;
             //   Debug.LogError(taskDataItem.taskData.desc);
                AppFacade.GetInstance().SendSproto(req);
                if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskMain)
                {
                    PlayTaskMainAnim(listItem, () =>
                    {
                        mainAniming = false;
                        taskDataItem.taskData = m_taskProxy.GetTaskDataByid(taskDataItem.taskData.taskMainDefine.nextId);
                        if (taskDataItem.taskData == null)
                        {

                        }
                        else
                        {
                            view.m_sv_questMain_ListView.RefreshItem(1);
                        }
                        redPointMain = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskMain);
                        RefreshRedPointView(EnumTaskPageType.TaskSide);
                    });
                }
                else
                {
                    m_deleteItem = listItem;
                    Animation ani = listItem.go.GetComponent<Animation>();
                    AnimationClip clip = ani.GetClip("MissionItemRemove");
                    m_deleteTime = clip.length;
                    animShow = true;
                    m_height = view.m_sv_questMain_ListView.GetItemSizeByIndex(m_deleteItem.index);
                    if (ani != null)
                    {
                        ani.Play("MissionItemRemove");
                    }
                    redPointSide--;
                    RefreshRedPointView(EnumTaskPageType.TaskSide);
                }
            }
        }
        private void PlayAnimEnd(ListView.ListItem listItem)
            {
            if (listItem.data is TaskDataItem)
            {
                TaskDataItem taskDataItem = listItem.data as TaskDataItem;
                if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskMain)
                {
                 
                }
                else if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskSide)
                {
                    m_taskPageMainList.Remove(taskDataItem);
                }
                else if (taskDataItem.taskData.taskPageType == EnumTaskPageType.TaskDaily)
                {
                    m_taskPageDailyList.Remove(taskDataItem);
                }
            }
        }
        void OnTaskDailyDataItemClick(ListView.ListItem listItem)
        {
            if (listItem.data is TaskDataItem)
            {
                if (animShow)
                {
                    return;
                }
                TaskDataItem taskDataItem = listItem.data as TaskDataItem;
                Task_TaskFinish.request req = new Task_TaskFinish.request();
                req.taskId = taskDataItem.taskData.taskID;
                AppFacade.GetInstance().SendSproto(req);
                m_deleteItem = listItem;
                Animation ani = listItem.go.GetComponent<Animation>();
                AnimationClip clip = ani.GetClip("MissionItemRemove");
                m_deleteTime = clip.length;
                animShow = true;
                m_height = view.m_sv_questDaily_ListView.GetItemSizeByIndex(m_deleteItem.index);
                if (ani != null)
                {
                    ani.Play("MissionItemRemove");
                }
            }
        }

        void PlayTaskMainAnim(ListView.ListItem ListItem, Action action)
        {
            if (ListItem.data is TaskDataItem)
            {
                mainAniming = true;
                TaskDataItem taskDataItem = ListItem.data as TaskDataItem;
                GameObject obj = ListItem.go;
                RectTransform rt = obj.transform.GetComponent<RectTransform>();
                MaskableGraphic[] MaskableGraphics = obj.GetComponentsInChildren<MaskableGraphic>();

                for (int i = 0; i < MaskableGraphics.Length; i++)
                {
                    int j = i;
                    MaskableGraphic maskableGraphics = MaskableGraphics[j];
                    float myValue2 = maskableGraphics.color.a;
                    if (myValue2 != 0)
                    {
                        DOTween.To(() => myValue2, x => myValue2 = x, 0, 0.3f).SetUpdate(true).OnUpdate(() =>
                        {
                            maskableGraphics.color = new Color(maskableGraphics.color.r, maskableGraphics.color.g, maskableGraphics.color.b, myValue2);
                        }).OnComplete(() =>
                        {
                            maskableGraphics.color = new Color(maskableGraphics.color.r, maskableGraphics.color.g, maskableGraphics.color.b, 1);
                        });
                    }
                }
                obj.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    if (ListItem != null && ListItem.go != null)
                    {
                        action?.Invoke();
                    }

                });

            }
        }

        void ShowBuildingGroupTypeList(EnumTaskPageType taskPageType)
        {
            RefreshRedPointData();
            RefreshRedPointView();
            m_taskPageType = taskPageType;
            switch (taskPageType)
            {
                case EnumTaskPageType.TaskChapter:
                    view.m_UI_Model_Window.m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(700024, LanguageUtils.getText(m_curTaskChapterData.l_titleNameId), LanguageUtils.getText(m_curTaskChapterData.l_descId1));
                    view.m_UI_Model_PageButton_1.m_img_dark_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_1.m_img_highLight_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_2.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_2.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_3.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_3.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_pl_chapter.gameObject.SetActive(true);
                    view.m_sv_questMain_ListView.gameObject.SetActive(false);
                    view.m_pl_daily.gameObject.SetActive(false);
                    if (m_timer != null)
                    {
                        m_timer.Cancel();
                        m_timer = null;
                    }
                    break;
                case EnumTaskPageType.TaskMain:
                    view.m_UI_Model_Window.m_lbl_title_LanguageText.text = LanguageUtils.getText(700000);
                    view.m_UI_Model_PageButton_1.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_1.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_2.m_img_dark_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_2.m_img_highLight_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_3.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_3.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_pl_chapter.gameObject.SetActive(false);
                    view.m_sv_questMain_ListView.gameObject.SetActive(true);
                    view.m_sv_questMain_ListView.FillContent(m_taskPageMainList.Count);
                    view.m_sv_questMain_ListView.ForceRefresh();
                    view.m_pl_daily.gameObject.SetActive(false);
                    if (m_timer != null)
                    {
                        m_timer.Cancel();
                        m_timer = null;
                    }
                    break;
                case EnumTaskPageType.TaskDaily:
                    view.m_UI_Model_Window.m_lbl_title_LanguageText.text = LanguageUtils.getText(700003);
                    view.m_UI_Model_PageButton_1.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_1.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_2.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_2.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_3.m_img_dark_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_3.m_img_highLight_PolygonImage.gameObject.SetActive(true);
                    view.m_pl_chapter.gameObject.SetActive(false);
                    view.m_sv_questMain_ListView.gameObject.SetActive(false);
                    view.m_pl_daily.gameObject.SetActive(true);
                    m_taskPageDailyList = m_taskProxy.GetTaskListPagDaily();
                    view.m_sv_questDaily_ListView.FillContent(m_taskPageDailyList.Count);
                    view.m_sv_questDaily_ListView.ForceRefresh();
                    if (m_timer != null)
                    {
                        m_timer.Cancel();
                        m_timer = null;
                    }
                    RefreshTimer();
                    m_timer = Timer.Register(1.0f, RefreshTimer, null, true, false, view.vb);
                    break;
                default:
                    Debug.Log("not find type");
                    break;

            }

        }
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitItems()
        {
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemChapterRewardEnter;
                funcTab.GetItemSize = GetItemSizeReward;
                view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_view_ListView.FillContent(m_itemPackageShowList.Count);
            }

            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemTaskChapterEnter;
                view.m_sv_chapterQuest_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_chapterQuest_ListView.FillContent(m_taskChapterList.Count);
            }
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemTaskSideEnter;
                funcTab.GetItemPrefabName = GetItemPrefabNamePageMain;
                funcTab.GetItemSize = GetItemSize;
                view.m_sv_questMain_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_questMain_ListView.FillContent(m_taskPageMainList.Count);
            }
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemTaskDailyEnter;
                view.m_sv_questDaily_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_questDaily_ListView.FillContent(m_taskPageDailyList.Count);
            }
        }
        private void FlyRewardEffect(Transform rewards, List<RewardGroupData> rewardGroupDataList, RewardGroupData currencyData = null)
        {
            if (rewards != null)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.TipRewardGroup, rewardGroupDataList);
                for (int i = 0; i < rewardGroupDataList.Count; i++)
                {
                    RewardGroupData rewardGroupData = rewardGroupDataList[i];
                    switch ((EnumRewardType)rewardGroupData.RewardType)
                    {
                        case EnumRewardType.Currency:
                            {
                                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                                mt.FlyUICurrency(rewardGroupData.CurrencyData.ID, rewardGroupData.number, rewards.GetChild(i).transform.position);

                            }
                            break;
                        case EnumRewardType.Soldier:
                            {
                                //飘飞特效
                                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                                mt.FlyPowerUpEffect(rewards.GetChild(i).gameObject, rewards.GetChild(i).transform.GetComponent<RectTransform>(), Vector3.one);
                            }
                            break;
                        case EnumRewardType.Item:
                            {
                                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                                mt.FlyItemEffect(rewardGroupData.ItemData.ID, (int)rewardGroupData.number, rewards.GetChild(i).GetComponent<RectTransform>());
                            }
                            break;
                    }
                }
            }
            if (currencyData != null)
            {
                RectTransform startTransform = rewards.Find(currencyData.CurrencyData.currencyDefine.name) as RectTransform;
                if (startTransform != null)
                {
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyUICurrency((int)EnumCurrencyType.activePoint, currencyData.number, startTransform.position, view.m_img_apicon_PolygonImage.rectTransform.position);
                }
           
            }

        }
    }
}