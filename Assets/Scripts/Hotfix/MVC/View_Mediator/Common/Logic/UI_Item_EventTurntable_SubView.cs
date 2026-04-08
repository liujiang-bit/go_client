// =============================================================================== 
// Author              :    xzl
// Create Time         :    Saturday, 17 October 2020
// Update Time         :    Saturday, 17 October 2020
// Class Description   :    UI_Item_EventTurntable_SubView 转盘活动
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;
using PureMVC.Interfaces;
using DG.Tweening;
using System;
using SprotoType;
using Hotfix.Utils;

namespace Game {

    public class ItemInfoBody
    {
        public long PackageId;
        public long ItemId;
        public long ItemNum;
    }

    public class EventTurntableBody
    {
        public int ActivityId;
        public List<TurntableDrawProgressDefine> ExRewardList;
        public Vector3 WorldPos;
    }

    public partial class UI_Item_EventTurntable_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;

        private bool m_isInit;
        private ActivityItemData m_menuData;
        private ActivityScheduleData m_scheduleData;

        private List<UI_Item_EventTurntablePro_SubView> m_probabilityItemList;

        private List<UI_Item_EventTurntableItem_SubView> m_itemSubViewList;

        private List<ItemInfoBody> m_itemDataList = new List<ItemInfoBody>();

        private int m_findIndex;

        private long m_endTime;
        private Timer m_timer;

        private int m_drawType1;
        private int m_drawType2;

        private int m_dayDrawCount; //每日可抽奖次数
        private int m_freeCount;    //免费次数
        private int m_turntableLvl;

        private int m_discountPrice;    //打折价格
        private int m_singlePrice;      //单个价格
        private int m_mulPrice;         //多个价格

        private TurntableDrawDefine m_drawTypeDefine1;
        private TurntableDrawDefine m_drawTypeDefine2;

        private List<TurntableDrawProgressDefine> m_exRewardList = new List<TurntableDrawProgressDefine>();

        private bool m_isRequesting;
        private bool m_isAniming;

        private List<long> m_rewardIdList = new List<long>();
        private RewardInfo m_rewardInfo;

        private int m_currShowIndex = 0;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.UpdateActivityReddot,
                    CmdConstant.SwitchActivityMenu,
                    CmdConstant.OnCloseUI,
                    CmdConstant.ActivityTurnTableReturn,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateActivityReddot:
                    RefreshContent();
                    break;
                case CmdConstant.SwitchActivityMenu: //切换菜单
                    IsShowRewardWin();
                    break;
                case CmdConstant.OnCloseUI: //关闭活动界面
                    UIInfo uIInfo = notification.Body as UIInfo;
                    if (uIInfo != null && uIInfo == UI.s_eventDate)
                    {
                        IsShowRewardWin();
                    }
                    break;
                case CmdConstant.ActivityTurnTableReturn:
                    m_isRequesting = false;
                    break;
                default:
                    break;
            }
        }

        public void Init(ActivityItemData menuData)
        {
            m_menuData = menuData;
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

                m_itemSubViewList = new List<UI_Item_EventTurntableItem_SubView>();
                m_itemSubViewList.Add(m_UI_Item_EventTurntable1);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable2);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable3);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable4);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable5);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable6);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable7);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable8);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable9);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable10);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable11);
                m_itemSubViewList.Add(m_UI_Item_EventTurntable12);

                m_btn_info_GameButton.onClick.AddListener(OnInfo);
                m_btn_back_GameButton.onClick.AddListener(OnBack);

                m_btn_free.m_btn_languageButton_GameButton.onClick.AddListener(OnFreeDraw);
                m_btn_one.m_btn_languageButton_GameButton.onClick.AddListener(OnSingleDraw);
                m_btn_more.m_btn_languageButton_GameButton.onClick.AddListener(OnMulDraw);
                m_btn_ex_box_GameButton.onClick.AddListener(OnExReward);

                ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                List<int> dList = configDefine.turntableDrawParam;
                m_freeCount = dList[0];
                m_dayDrawCount = dList[1];
                m_turntableLvl = configDefine.turntableLev;

                m_isInit = true;
            }

            ReadData();

            gameObject.SetActive(true);

            Refresh();
        }

        //是否有折扣
        private bool IsHasDiscount()
        {
            if (m_scheduleData.Info.discount)
            {
                return false;
            }
            return true;
        }

        private void ReadData()
        {
            m_isRequesting = false;
            m_isAniming = false;

            m_rewardInfo = null;

            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);

            //if (m_scheduleData == null)
            //{
            //    m_scheduleData = new ActivityScheduleData();
            //    m_scheduleData.Info = new SprotoType.Activity();
            //}

            //转盘类型
            string[] arrStr = m_menuData.Define.drawType.Split('|');
            m_drawType1 = int.Parse(arrStr[0]);
            m_drawType2 = int.Parse(arrStr[1]);

            m_drawTypeDefine1 = CoreUtils.dataService.QueryRecord<TurntableDrawDefine>(m_drawType1*100+1);
            m_drawTypeDefine2 = CoreUtils.dataService.QueryRecord<TurntableDrawDefine>(m_drawType2*100+1);

            m_singlePrice = m_drawTypeDefine1.Cost;
            m_discountPrice = Mathf.FloorToInt(m_drawTypeDefine1.Cost * ((float)m_drawTypeDefine1.Cost_firt_discount / 100));
            m_mulPrice = m_drawTypeDefine2.Cost;

            //物品列表
            m_itemDataList.Clear();
            int packId = m_drawTypeDefine1.itempack * 1000;
            for (int i = 1; i < 500; i++)
            {
                var define2 = CoreUtils.dataService.QueryRecord<ItemPackageShowDefine>(packId + i);
                if (define2 == null)
                {
                    break;
                }
                else
                {
                    ItemInfoBody body = new ItemInfoBody();
                    body.PackageId = define2.ID;
                    body.ItemId = define2.typeData;
                    body.ItemNum = define2.number;
                    m_itemDataList.Add(body);
                }
            }

            //额外奖励列表
            m_exRewardList.Clear();
            int id2 = m_menuData.Define.ID * 100;
            for (int i = 0; i < 100; i++)
            {
                var define2 = CoreUtils.dataService.QueryRecord<TurntableDrawProgressDefine>(id2 + i);
                if (define2 == null)
                {
                    break;
                }
                else
                {
                    m_exRewardList.Add(define2);
                }
            }
        }

        private void Refresh()
        {
            for (int i = 0; i < m_itemDataList.Count; i++)
            {
                m_itemSubViewList[i].Refresh(m_itemDataList[i]);
            }

            //刷新额外奖励进度
            RefreshExtraRewardProcess();

            RefreshResidueCount();

            RefreshBtnLayer();

            RefreshSpine();

            //倒计时
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (m_menuData.Data.endTime > serverTime)
            {
                m_endTime = m_menuData.Data.endTime;
                UpdateTime();
                StartTimer();
            }
            else
            {
                CancelTimer();
                Timer.Register(0.02f, OnClose);
                return;
            }
        }

        //刷新
        private void RefreshContent()
        {
            RefreshExtraRewardProcess();

            RefreshResidueCount();

            RefreshBtnLayer();
        }

        //刷新额外奖励进度
        private void RefreshExtraRewardProcess()
        {

            int findIndex = m_exRewardList.Count - 1;
            for (int i = 0; i < m_exRewardList.Count; i++)
            {
                if (m_scheduleData.Info.count < m_exRewardList[i].reach)
                {
                    findIndex = i;
                    break;
                }
            }
            if (findIndex < 0 || findIndex >= m_exRewardList.Count)
            {
                return;
            }
            m_lbl_box_num_LanguageText.text = LanguageUtils.getTextFormat(180714, m_scheduleData.Info.count, m_exRewardList[findIndex].reach);

            //刷新宝箱动画
            int findIndex2 = -1;
            bool isHasCanReceive = false;
            for (int i = 0; i < m_exRewardList.Count; i++)
            {
                if (m_scheduleData.Info.count >= m_exRewardList[i].reach)
                {
                    if (!m_scheduleData.IsReceive(m_exRewardList[i].ID))
                    {
                        isHasCanReceive = true;
                        findIndex2 = i;
                        break;
                    }
                }
            }

            //变更宝箱样式
            if (findIndex2 >= 0)
            {
                if (m_img_box.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName != m_exRewardList[findIndex2].treasureModel)
                {
                    m_img_box.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName = m_exRewardList[findIndex2].treasureModel;
                }
            }
            else
            {
                if (m_img_box.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName != m_exRewardList[findIndex].treasureModel)
                {
                    m_img_box.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName = m_exRewardList[findIndex].treasureModel;
                }
            }

            if (isHasCanReceive)
            {
                m_img_box.SetBox(false, true);
            }
            else
            {
                m_img_box.SetBox(false, false);
            }


        }

        //刷新剩余次数
        private void RefreshResidueCount()
        {
            m_lbl_chance_LanguageText.text = LanguageUtils.getTextFormat(762240, m_dayDrawCount - (int)m_scheduleData.Info.dayCount);
        }

        //刷新按钮显示
        private void RefreshBtnLayer()
        {
            if (m_freeCount > 0 && m_scheduleData.Info.free < m_freeCount)
            {
                //免费
                m_btn_free.gameObject.SetActive(true);
                m_btn_free.m_img_redpoint_PolygonImage.gameObject.SetActive(true);
                m_pl_one.gameObject.SetActive(false);
            }
            else
            {
                //单抽
                m_btn_free.gameObject.SetActive(false);
                m_pl_one.gameObject.SetActive(true);

                //判断是否有折扣
                if (IsHasDiscount())
                {
                    //有折扣
                    m_img_discount_PolygonImage.gameObject.SetActive(true);
                    m_lbl_discount_LanguageText.text = LanguageUtils.getTextFormat(300107, m_drawTypeDefine1.Cost_firt_discount);
                    m_btn_one.m_lbl_line1_LanguageText.text = LanguageUtils.getTextFormat(762241, m_drawTypeDefine1.fornum);
                    m_btn_one.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_discountPrice);
                }
                else
                {
                    //无折扣
                    m_img_discount_PolygonImage.gameObject.SetActive(false);
                    m_btn_one.m_lbl_line1_LanguageText.text = LanguageUtils.getTextFormat(762241, m_drawTypeDefine1.fornum);
                    m_btn_one.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_singlePrice);
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_btn_one.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }

            //多抽
            m_btn_more.m_lbl_line1_LanguageText.text = LanguageUtils.getTextFormat(762241, m_drawTypeDefine2.fornum);
            m_btn_more.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_mulPrice);
            m_btn_more.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_btn_more.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_lbl_box_num_LanguageText);
            }
        }

        //取消定时器
        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void UpdateTime()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 diffTime = m_endTime - serverTime;
            if (diffTime < 0)
            {
                //倒计时结束 关闭界面
                CancelTimer();
                CoreUtils.uiManager.CloseUI(UI.s_newRoleActivity);
            }
            else
            {
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        #region 动画表现

        private void ResetItemViewStatus()
        {
            for (int i = 0; i < m_itemSubViewList.Count; i++)
            {
                m_itemSubViewList[i].Reset();
            }
        }

        private void RotateAni()
        {
            if (gameObject == null)
            {
                return;
            }
            m_img_turntable_PolygonImage.transform.DOLocalRotate(new Vector3(0, 0, 720 + m_findIndex * 30), 3.5f, RotateMode.LocalAxisAdd).OnComplete(RotateAniEnd);
        }

        private void RotateAniEnd()
        {
            //数量加1
            for (int i = 0; i < m_itemSubViewList.Count; i++)
            {
                if (m_itemSubViewList[i].GetPackageId() == m_itemDataList[m_findIndex].PackageId)
                {
                    m_itemSubViewList[i].AddGetNum();
                    break;
                }
            }

            List<ItemInfoBody> itemList = new List<ItemInfoBody>();
            for (int i = m_findIndex; i < m_itemDataList.Count; i++)
            {
                itemList.Add(m_itemDataList[i]);
            }

            for (int i = 0; i < m_findIndex; i++)
            {
                itemList.Add(m_itemDataList[i]);
            }
            m_itemDataList = itemList;

            if (m_rewardIdList.Count > 1)
            {
                //多抽
                m_currShowIndex = m_currShowIndex + 1;

                if (m_currShowIndex >= m_rewardIdList.Count)
                {
                    //表现结束 弹出奖励界面
                    AniEnd();

                    RewardGetData rewardGetData = new RewardGetData();
                    RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                    rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByRewardInfo(m_rewardInfo);
                    rewardGetData.nameType = 2;
                    if (rewardGetData.rewardGroupDataList.Count != 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                    }

                    //CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, m_rewardInfo);
                }
                else
                {
                    m_findIndex = -1;
                    //查找下一个
                    for (int i = 0; i < m_itemDataList.Count; i++)
                    {
                        if (m_itemDataList[i].PackageId == m_rewardIdList[m_currShowIndex])
                        {
                            m_findIndex = i;
                            break;
                        }
                    }
                    if (m_findIndex < 0)
                    {
                        Debug.LogErrorFormat("物品没找到:{0}", m_rewardIdList[m_currShowIndex]);
                        return;
                    }
                    Timer.Register(0.5f, RotateAni);
                }
            }
            else
            {
                //单抽 直接弹出奖励界面
                AniEnd();

                //CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, m_rewardInfo);
                RewardGetData rewardGetData = new RewardGetData();
                RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByRewardInfo(m_rewardInfo);
                rewardGetData.nameType = 2;
                if (rewardGetData.rewardGroupDataList.Count != 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                }
            }
        }

        private void AniEnd()
        {
            m_isAniming = false;
        }

        #endregion

        #region 活动信息

        private void OnInfo()
        {
            if (m_isRequesting || m_isAniming)
            {
                return;
            }

            m_pl_info_CanvasGroup.gameObject.SetActive(true);
            m_pl_mes.gameObject.SetActive(false);

            m_lbl_info_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_ruleID);

            m_UI_Item_EventTurntablePro.gameObject.SetActive(false);

            if (m_probabilityItemList == null)
            {
                m_probabilityItemList = new List<UI_Item_EventTurntablePro_SubView>();
            }

            List<TurntableRangeShowDefine> defineList = new List<TurntableRangeShowDefine>();
            int id = m_menuData.Define.ID;
            for (int i = 0; i < 500; i++)
            {
                var define = CoreUtils.dataService.QueryRecord<TurntableRangeShowDefine>(id * 1000 + i);
                if (define == null)
                {
                    break;
                }
                defineList.Add(define);
            }
            defineList.Sort(delegate (TurntableRangeShowDefine x, TurntableRangeShowDefine y)
            {
                int re = x.order.CompareTo(y.order);
                return re;
            });

            int count = defineList.Count - m_probabilityItemList.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject go = CoreUtils.assetService.Instantiate(m_UI_Item_EventTurntablePro.gameObject);
                    go.transform.SetParent(m_pl_probMes_VerticalLayoutGroup.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;
                    UI_Item_EventTurntablePro_SubView subView = new UI_Item_EventTurntablePro_SubView(go.GetComponent<RectTransform>());
                    m_probabilityItemList.Add(subView);
                }
            }
            for (int i = 0; i < m_probabilityItemList.Count; i++)
            {
                if (i < defineList.Count)
                {
                    m_probabilityItemList[i].gameObject.SetActive(true);
                    var define = CoreUtils.dataService.QueryRecord<ItemDefine>(defineList[i].itemId);
                    if (define != null)
                    {
                        m_probabilityItemList[i].m_lbl_itemName_LanguageText.text = LanguageUtils.getText(define.l_nameID);
                        m_probabilityItemList[i].m_lbl_itemNum_LanguageText.text = LanguageUtils.getTextFormat(145048, defineList[i].num);
                        m_probabilityItemList[i].m_lbl_prob_LanguageText.text = LanguageUtils.getTextFormat(180357, (float)defineList[i].range / 100);
                    }
                }
                else
                {
                    m_probabilityItemList[i].gameObject.SetActive(false);
                }
            }
            float height = m_probabilityItemList.Count * m_UI_Item_EventTurntablePro.gameObject.GetComponent<RectTransform>().rect.height + m_pl_title.rect.height;
            RectTransform rect = m_pl_probMes_VerticalLayoutGroup.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_probMes_VerticalLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_c_info_VerticalLayoutGroup.GetComponent<RectTransform>());
        }

        private void OnBack()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(false);
            m_pl_mes.gameObject.SetActive(true);
        }

        #endregion

        #region spine刷新

        private void RefreshSpine()
        {
            ClientUtils.LoadSpine(m_pl_char_SkeletonGraphic, m_menuData.Define.heroShow, LoadSpineCallback);
            m_pl_char_SkeletonGraphic.startingAnimation = "idle";
            m_pl_char_SkeletonGraphic.Initialize(true);
        }

        private void LoadSpineCallback()
        {
            if (gameObject == null)
            {
                return;
            }
            m_pl_char_SkeletonGraphic.gameObject.SetActive(true);
        }
        #endregion

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_eventDate);
        }

        //额外奖励
        private void OnExReward()
        {
            if (m_isRequesting)
            {
                return;
            }
            if (m_isAniming)
            {
                return;
            }
            EventTurntableBody body = new EventTurntableBody();
            body.ActivityId = m_menuData.Define.ID;
            body.ExRewardList = m_exRewardList;
            body.WorldPos = m_btn_ex_box_GameButton.gameObject.transform.position;
            CoreUtils.uiManager.ShowUI(UI.s_eventTurntableRewards, null, body);
        }

        //免费抽奖
        private void OnFreeDraw()
        {
            SendCheck(m_drawType1, true, false, 0);
        }

        //单抽
        private void OnSingleDraw()
        {
            bool isDiscount = IsHasDiscount();
            int price = 0;
            if (isDiscount)
            {
                price = m_discountPrice;
            }
            else
            {
                price = m_singlePrice;
            }
            SendCheck(m_drawType1, false, isDiscount, price);
        }

        //多抽
        private void OnMulDraw()
        {
            SendCheck(m_drawType2, false, false, m_mulPrice);
        }

        //发送检查
        private void SendCheck(int drawType, bool isFree, bool isDiscount, int price)
        {
            if (m_isRequesting)
            {
                return;
            }
            if (m_isAniming)
            {
                return;
            }

            //城堡等级是否足够
            if (m_playerProxy.CurrentRoleInfo.level < m_turntableLvl)
            {
                Tip.CreateTip(LanguageUtils.getTextFormat(762243, m_turntableLvl)).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }

            if (isFree) //免费
            {
                Send(drawType, isFree, isDiscount);
            }
            else if (isDiscount) //折扣
            {
                //是否花费提醒
                if (IsCostTipRemind(drawType, isFree, isDiscount, price))
                {
                    return;
                }

                //判断代币是否足够
                if (m_currencyProxy.ShortOfDenar(price))
                {
                    return;
                }

                Send(drawType, isFree, isDiscount);
            }
            else
            {
                //当日抽奖次数是否已用完
                if (m_scheduleData.Info.dayCount >= m_dayDrawCount)
                {
                    Tip.CreateTip(762267).Show();
                    return;
                }

                //次数超出上限！
                int count = (drawType == m_drawType2) ? m_drawTypeDefine2.fornum : m_drawTypeDefine1.fornum;
                if (m_scheduleData.Info.dayCount + count > m_dayDrawCount)
                {
                    Tip.CreateTip(762268).Show();
                    return;
                }

                //是否花费提醒
                if (IsCostTipRemind( drawType, isFree, isDiscount, price))
                {
                    return;
                }

                //判断代币是否足够
                if (m_currencyProxy.ShortOfDenar(price))
                {
                    return;
                }

                Send(drawType, isFree, isDiscount);
            }
        }

        private bool IsCostTipRemind(int drawType, bool isFree, bool isDiscount, int price)
        {
            GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
            if (isRemind)
            {
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                string currencyiconId = currencyProxy.GeticonIdByType(104);
                Data.SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                if (settingPersonalityDefine != null)
                {
                    string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                    string str = LanguageUtils.getTextFormat(300072, ClientUtils.FormatComma(price));
                    Alert.CreateAlert(str, LanguageUtils.getText(610021)).
                                      SetLeftButton().
                                      SetRightButton(null, LanguageUtils.getText(730038)).
                                      SetCurrencyRemind((isBool) =>
                                      {
                                          if (isBool)
                                          {
                                              generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                                              //判断代币是否足够
                                          }
                                          if (m_currencyProxy.ShortOfDenar(price))
                                          {
                                              return;
                                          }
                                          Send(drawType, isFree, isDiscount);
                                      },
                                      price, s_remind, currencyiconId).Show();
                }
                return true;
            }
            return false;
        }

        //发送数据
        private void Send(int drawType, bool isFree, bool isDiscount)
        {
            m_isRequesting = true;
            var sp = new Activity_TurnTable.request();
            sp.activityId = m_menuData.Define.ID;
            sp.type = drawType;
            sp.free = isFree;
            sp.discount = isDiscount;
            Debug.LogFormat("activityid:{0} type:{1} free:{2} discount:{3}", sp.activityId, sp.type, sp.free, sp.discount);
            AppFacade.GetInstance().SendSproto(sp);
        }

        private void IsShowRewardWin()
        {
            if (m_isAniming)
            {
                if (m_rewardInfo != null)
                {
                    RewardGetData rewardGetData = new RewardGetData();
                    RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                    rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByRewardInfo(m_rewardInfo);
                    rewardGetData.nameType = 2;
                    if (rewardGetData.rewardGroupDataList.Count != 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                    }
                }
            }
        }

        //准备开始转盘表现
        public void TurnTableProcess(object body)
        {
            var result = body as Activity_TurnTable.response;
            m_rewardInfo = result.rewardInfo;
            m_rewardIdList.Clear();
            m_rewardIdList.AddRange(result.packageIds);
            m_currShowIndex = 0;
            m_findIndex = -1;
            for (int i = 0; i < m_itemDataList.Count; i++)
            {
                if (m_itemDataList[i].PackageId == m_rewardIdList[m_currShowIndex])
                {
                    m_findIndex = i;
                    break;
                }
            }
            if (m_findIndex < 0)
            {
                Debug.LogError("没找到奖励物品");
                return;
            }
            m_isAniming = true;
            ResetItemViewStatus();
            RotateAni();
            m_img_turntable_arrow_1_Animation.Stop();
            m_img_turntable_arrow_1_Animation.Play();
        }

        //是否是当前活动
        public bool IsCurrActivityId(int id)
        {
            if (m_menuData != null && m_menuData.Define.ID == id)
            {
                return true;
            }
            return false;
        }
    }
}