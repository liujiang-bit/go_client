// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月19日
// Update Time         :    2020年10月19日
// Class Description   :    UI_Pop_EventTurntableRewardsMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;

namespace Game {
    public class UI_Pop_EventTurntableRewardsMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_EventTurntableRewardsMediator";

        private List<UI_Item_EventTribeKingBox_SubView> m_subviewList = new List<UI_Item_EventTribeKingBox_SubView>();

        private EventTurntableBody m_eventBody;
        private ActivityProxy m_activityProxy;
        private ActivityScheduleData m_scheduleData;

        private bool m_isRequesting;


        private List<float> m_widthList;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_EventTurntableRewardsMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_EventTurntableRewardsView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Activity_TurnReward.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Activity_TurnReward.TagName:
                    ProcessReceiveReward(notification.Body);
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            view.m_UI_Tip_BoxReward.gameObject.SetActive(false);

            EventTurntableBody body = view.data as EventTurntableBody;
            m_eventBody = body;

            m_scheduleData = m_activityProxy.GetActivitySchedule(m_eventBody.ActivityId);

            //if (m_scheduleData == null)
            //{
            //    m_scheduleData = new ActivityScheduleData();
            //    m_scheduleData.Info = new SprotoType.Activity();
            //}
            //m_scheduleData.Info.count = 80;

            m_subviewList.Add(view.m_img_box1);
            m_subviewList.Add(view.m_img_box2);
            m_subviewList.Add(view.m_img_box3);
            m_subviewList.Add(view.m_img_box4);
            m_subviewList.Add(view.m_img_box5);

            for (int i = 0; i < m_subviewList.Count; i++)
            {
                m_subviewList[i].Init(i);
                m_subviewList[i].BtnClickEvent = ClickBtnEvent;
                m_subviewList[i].ChangeSkinName(m_eventBody.ExRewardList[i].treasureModel);
                m_subviewList[i].RefreshBoxStatus(GetBoxStatus(i));
            }
            RefreshPro();
        }

        private void RefreshPro()
        {
            if (m_widthList == null)
            {
                float width = view.m_pb_rogressBar_GameSlider.GetComponent<RectTransform>().rect.width;
                m_widthList = new List<float>();
                for (int i = 0; i < m_subviewList.Count; i++)
                {
                    m_widthList.Add(Mathf.Abs(m_subviewList[i].m_root_RectTransform.anchoredPosition.x) / width);
                }
            }

            List<TurntableDrawProgressDefine> behaviorList = m_eventBody.ExRewardList;
            long score = m_scheduleData.Info.count;

            int count = behaviorList.Count;
            if (count > 0)
            {
                float pro = (float)m_scheduleData.Info.count / behaviorList[behaviorList.Count - 1].reach;
                int stage = -1;
                for (int i = 0; i < behaviorList.Count; i++)
                {
                    if (score > behaviorList[i].reach)
                    {
                        stage = i;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            pro = (float)score / behaviorList[i].reach * m_widthList[0];
                        }
                        else
                        {
                            int beforeScore = behaviorList[i - 1].reach;
                            int afterScore = behaviorList[i].reach;
                            int diffScore = (afterScore - beforeScore);
                            if (diffScore > 0)
                            {
                                float diffPro = m_widthList[i] - m_widthList[i - 1];
                                pro = ((float)(score - beforeScore) / diffScore) * diffPro;
                            }
                        }
                        break;
                    }
                }
                if (stage > -1)
                {
                    pro = m_widthList[stage] + pro;
                }
                view.m_pb_rogressBar_GameSlider.value = pro * 100;
            }
        }

        private int GetBoxStatus(int index)
        {
            if (m_scheduleData.Info.count < m_eventBody.ExRewardList[index].reach)
            {
                return (int)EventHellBoxStatus.NotCanReceive;
            }
            else if (m_scheduleData.Info.count >= m_eventBody.ExRewardList[index].reach)
            {
                if (m_scheduleData.IsReceive(m_eventBody.ExRewardList[index].ID))
                {
                    return (int)EventHellBoxStatus.AlreadyReceive;
                }
                else
                {
                    return (int)EventHellBoxStatus.CanReceive;
                }
            }
            return 0;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void ClickBtnEvent(int index)
        {
            int boxStatus = m_subviewList[index].GetBoxStatus();
            if (boxStatus != (int)EventHellBoxStatus.CanReceive)
            {
                //显示tip
                view.m_UI_Tip_BoxReward.SetInfo3(m_eventBody.ExRewardList[index].itempack,
                                               null,
                                               m_subviewList[index].m_btn_box_GameButton.transform.position,
                                               10, 4);
                return;
            }
            if (m_isRequesting)
            {
                return;
            }
            m_isRequesting = true;
            var sp = new Activity_TurnReward.request();
            sp.activityId = m_eventBody.ActivityId;
            sp.id = m_eventBody.ExRewardList[index].ID;
            Debug.LogFormat("activityid:{0} type:{1}", sp.activityId, m_eventBody.ExRewardList[index].ID);
            AppFacade.GetInstance().SendSproto(sp);

        }

        //处理奖励信息
        private void ProcessReceiveReward(object body)
        {
            m_isRequesting = false;
            var result = body as Activity_TurnReward.response;
            if (result == null)
            {
                return;
            }
            //ClientUtils.Print(result);
            if (result.rewardInfo == null)
            {
                return;
            }

            if (view.gameObject == null)
            {
                return;
            }

            int index = -1;
            for (int i = 0; i < m_eventBody.ExRewardList.Count; i++)
            {
                if (m_eventBody.ExRewardList[i].ID == result.id)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return;
            }

            //开宝箱动画
            m_subviewList[index].OpenBoxAni(() =>
            {
                if (view.gameObject == null)
                {
                    return;
                }
                //宝箱开启动画表现完再更新状态
                if (index < m_subviewList.Count)
                {
                    m_subviewList[index].RefreshBoxStatus(GetBoxStatus(index));
                }
            });

            //奖励飘飞表现
            RectTransform rectTrans = null;
            if (index >= 0 && index < m_subviewList.Count)
            {
                rectTrans = m_subviewList[index].m_root_RectTransform;
                m_subviewList[index].ChangeBoxStatus(GetBoxStatus(index));
            }
            if (rectTrans == null)
            {
                return;
            }
            //飘飞特效
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (result.rewardInfo.items != null)
            {
                for (int i = 0; i < result.rewardInfo.items.Count; i++)
                {
                    mt.FlyItemEffect((int)result.rewardInfo.items[i].itemId,
                                     (int)result.rewardInfo.items[i].itemNum,
                                     rectTrans);
                }
            }
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_eventTurntableRewards);
        }
    }
}