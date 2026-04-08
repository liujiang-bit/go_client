// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月25日
// Update Time         :    2020年4月25日
// Class Description   :    ActivityCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using SprotoType;
using Hotfix.Utils;
using Skyunion;

namespace Game {
    public class ActivityCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            ActivityProxy m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            switch (notification.Name)
            {
                case Activity_Reward.TagName:       //领取奖励
                    {
                        m_activityProxy.UpdateReward(notification.Body);
                        break;
                    }
                case Activity_RewardBox.TagName:   //领取宝箱
                    {
                        m_activityProxy.RewardBoxUpdate(notification.Body);
                        break;
                    }
                case Activity_ScheduleInfo.TagName://进度更新
                    {
                        m_activityProxy.UpdateScheduleVal(notification.Body);
                        break;
                    }
                case CmdConstant.ItemInfoChange:   //物品变更
                    m_activityProxy.UpdateExchangeActivity();
                    break;
                case Activity_Exchange.TagName:   //兑换
                    m_activityProxy.UpdateExchageData(notification.Body);
                    break;
                case CmdConstant.SystemDayChange: //日期天数变更 重新获取兑换类型活动
                    m_activityProxy.FindExchangeActivity();
                    //日历红点重新计算
                    m_activityProxy.SetCalendarReddotStatus(true);
                    AppFacade.GetInstance().SendNotification(CmdConstant.RefreshActivityNewFlag, -2);
                    break;
                case Activity_Rank.TagName: 
                    m_activityProxy.UpdateArchonData(notification.Body);
                    break;
                case CmdConstant.ActivityActivePointUpdate:
                    var activityScheduleData = m_activityProxy.GetActivitySchedule(ActivityProxy.CreateRoleActivityId);
                    if (activityScheduleData != null)
                    {
                        activityScheduleData.IsReddotChange = true;
                        AppFacade.GetInstance().SendNotification(CmdConstant.ActivityActivePointChange);
                    }
                    //m_activityProxy
                    break;
                case Activity_TurnTable.TagName:
                    AppFacade.GetInstance().SendNotification(CmdConstant.ActivityTurnTableReturn);
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        //错误码处理
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    var result = notification.Body as Activity_TurnTable.response;
                    if (result.packageIds == null || result.packageIds.Count < 1)
                    {
                        Debug.LogError("packageIds is null or < 1");
                        return;
                    }
                    if (result.rewardInfo == null)
                    {
                        Debug.LogError("result.rewardInfo  is null");
                        return;
                    }

                    bool isShowRewardWin = true;
                    if (CoreUtils.uiManager.ExistUI(UI.s_eventDate))
                    {
                        EventDateMediator eventDateMediator = AppFacade.GetInstance().RetrieveMediator(EventDateMediator.NameMediator) as EventDateMediator;
                        if (eventDateMediator != null)
                        {
                           object subObject = eventDateMediator.GetContentSubview();
                            if (subObject != null && (subObject is UI_Item_EventTurntable_SubView))
                            {
                                UI_Item_EventTurntable_SubView subView = subObject as UI_Item_EventTurntable_SubView;
                                if (subView != null)
                                {
                                    if (subView.IsCurrActivityId((int)result.activityId))
                                    {
                                        isShowRewardWin = false;
                                        //准备开始转盘表现
                                        subView.TurnTableProcess(notification.Body);
                                    }                                       
                                }
                            }
                        }
                    }
                    if (isShowRewardWin)
                    {
                        RewardGetData rewardGetData = new RewardGetData();
                        RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                        rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByRewardInfo(result.rewardInfo);
                        rewardGetData.nameType = 2;
                        if (rewardGetData.rewardGroupDataList.Count != 0)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                        }
                    }
                    break;
                case Activity_ClickActivity.TagName:
                    m_activityProxy.UpdateActivityNewFlag(notification.Body);
                    break;
                default:
                    break;
            }
        }
    }
}