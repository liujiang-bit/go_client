// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Win_VipMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.EventSystems;

namespace Game {
    public class UI_Win_VipMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_VipMediator";
        
        private int m_currentVipLevel;
        private int m_showVipItemIndex;
        private int m_curVipIndex;
        private Timer m_dailyPointBoxTimer;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<VipDefine> m_vipInfoList = new List<VipDefine>();
        private PlayerProxy m_playerProxy;
        private RechargeProxy m_rechargeProxy;
        private Vector2 m_tmpMousePos;
        private int m_isReverse = 1;
        private UI_Pop_TextTip_SubView m_popTipSubView = null;
        private int m_jumpVipLevel;
        #endregion

        //IMediatorPlug needs
        public UI_Win_VipMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_VipView view;


        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.VipBuySpecialBox,
                CmdConstant.VipClaimFreeBox,
                CmdConstant.VipPointChange,
                CmdConstant.VipLevelUP,
                CmdConstant.VipDailyPointFlagChange,
                CmdConstant.VipFreeBoxFlagChange,
                CmdConstant.ContinueLoginDayChange,
                CmdConstant.VipSpecialBoxChange,
                Role_GetVipFreeBox.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.VipBuySpecialBox:
                    BuySpecialBox((int)notification.Body);
                    break;
                case CmdConstant.VipClaimFreeBox:
                    ClaimFreeBox();
                    break;
                case CmdConstant.VipPointChange:
                    RefreshView(m_playerProxy.CurrentRoleInfo,true);
                    break;
                case CmdConstant.VipLevelUP:
                    RefreshVipDetailInfo();
                    SetShowVipItemIndex((int)m_playerProxy.VipLevel,false);
                    break;
                case CmdConstant.VipDailyPointFlagChange:
                    RefreshDailyPointBox();
                    if (m_playerProxy.CurrentRoleInfo.vipExpFlag)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_VipPointDayReward);
                    }
                    break;
                case CmdConstant.VipFreeBoxFlagChange:
                    RefreshVipDetailInfo();
                    break;
                case CmdConstant.VipSpecialBoxChange:
                    RefreshVipDetailInfo();
                    break;
                case CmdConstant.ContinueLoginDayChange:
                    RefreshContinueLoginDay();
                    break;
                case Role_GetVipFreeBox.TagName:
                    Role_GetVipFreeBox.response res = notification.Body as Role_GetVipFreeBox.response;
                    if (res == null)
                    {
                        return;
                    }
                    RewardGetData rewardGetData = new RewardGetData();
                    RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                    rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByRewardInfo(res.rewardInfo);

                    if (rewardGetData.rewardGroupDataList.Count != 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                    }
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

        public override void WinClose()
        {
            ClearTimer();
            ClearAllDetailInfoItem();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            if (view.data is int )
            {
                m_jumpVipLevel = (int)view.data;
            }
            m_dailyPointBoxTimer = null;
            m_currentVipLevel = 0;
            m_vipInfoList = CoreUtils.dataService.QueryRecords<Data.VipDefine>();

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_listVip_ListView.ItemPrefabDataList, LoadVipListItemFinish);
            m_showVipItemIndex = (int)m_playerProxy.GetVipLevel();
            if (m_jumpVipLevel!=0)
            {
                m_showVipItemIndex = m_jumpVipLevel;
            }
            
        }

        protected override void BindUIEvent()
        {
            view.m_btn_pointBox_GameButton.onClick.AddListener(OnClickDailyPointBox);
            view.m_btn_add_GameButton.onClick.AddListener(OnClickAddVipPoint);
            view.m_btn_question_GameButton.onClick.AddListener(OnClickDailyPointDescription);
            view.m_btn_arrowBack_GameButton.onClick.AddListener(OnClickLeftPage);
            view.m_btn_arrowNext_GameButton.onClick.AddListener(OnClickRightPage);
            view.m_UI_Model_Window_Type2.setCloseHandle(OnClickClose);
        }

        protected override void BindUIData()
        {
            RefreshView(m_playerProxy.CurrentRoleInfo);
            RefreshDailyPointBox();
        }
        #endregion

        public int GetClaimableBoxIndex()
        {
            var curVIPLevel = m_playerProxy.GetVipLevel();
            if (m_playerProxy.CurrentRoleInfo.vipFreeBox == false || !m_playerProxy.CurrentRoleInfo.vipSpecialBox.Contains(curVIPLevel))
            {
                return m_curVipIndex;
            }
            return -1;
        }

        public int GetCurrShowIndex()
        {
            return m_showVipItemIndex;
        }

        private void RefreshView(RoleInfoEntity roleInfo,bool showBarMove = false)
        {
            VipDefine vipConfig = null;
            VipDayPointDefine vipDayPointConfig = null;
            bool isLevelMax = false;

            RefreshContinueLoginDay();
            GetVIPInfo(roleInfo,out vipConfig,out vipDayPointConfig ,out isLevelMax);
            if (vipConfig == null || vipDayPointConfig == null)
            {
                Debug.LogWarning($"获取当前VIP等级配置失败,请检查!!!Vip点数:{roleInfo.vip}");
                return;
            }
            
            ClientUtils.LoadSprite(view.m_img_vip_PolygonImage, vipConfig.icon);
            view.m_lbl_vipLevel_LanguageText.text = vipConfig.level.ToString();
            view.m_lbl_vipPoint_LanguageText.text = LanguageUtils.getTextFormat(800019, vipDayPointConfig.point);
            if (isLevelMax)
            {
                view.m_btn_add_GameButton.gameObject.SetActive(false);
                view.m_lbl_barText_LanguageText.text = LanguageUtils.getText(800026);
            }
            else
            {
                view.m_btn_add_GameButton.gameObject.SetActive(true);
                view.m_lbl_barText_LanguageText.text =string.Format("{0}/{1}", ClientUtils.FormatComma(roleInfo.vip), ClientUtils.FormatComma(vipConfig.point));
            }

            var barValue = (float) roleInfo.vip / vipConfig.point;
            if (showBarMove)
            {
                view.m_pb_vipBar_SmoothBar.SetValue(barValue > 1 ? 1 : barValue);
            }
            else
            {
                view.m_pb_vipBar_GameSlider.value = barValue>1?1: barValue;
            }
        }

        private void RefreshContinueLoginDay()
        {
            view.m_lbl_vipDay_LanguageText.text = LanguageUtils.getTextFormat(800020, m_playerProxy.CurrentRoleInfo.continuousLoginDay);
        }

        private void GetVIPInfo(RoleInfoEntity roleInfo, out VipDefine vipDefine, out VipDayPointDefine vipDayPointDefine,out bool isLevelMax)
        {
            vipDefine = null;
            vipDayPointDefine = null;
            isLevelMax = false;
            for (int i = 0; i < m_vipInfoList.Count; i++)
            {
                if (m_vipInfoList[i].point > roleInfo.vip)
                {
                    vipDefine = m_vipInfoList[i];
                    m_currentVipLevel = vipDefine.level;
                    break;
                }
            }
            

            if (vipDefine == null && m_vipInfoList.Count > 0)
            {
                vipDefine = m_vipInfoList[m_vipInfoList.Count - 1];
                m_currentVipLevel = vipDefine.level;
                isLevelMax = true;
            }
            vipDayPointDefine = m_playerProxy.GetVipDayPointInfo();
        }
        
        #region VipDetailInfoList

        private void RefreshVipDetailInfo()
        {
            view.m_sv_listVip_ListView.FillContent(m_vipInfoList.Count);
            SetShowVipItemIndex(m_showVipItemIndex);
        }
        
        private void LoadVipListItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitVipListItem;
            functab.ItemRemove = RemoveVipListItem;
            view.m_sv_listVip_ListView.onDragBegin += OnVipItemListDragBegin;
            view.m_sv_listVip_ListView.onDragEnd += OnVipItemListDragEnd;
            if (m_isReverse == -1)
            {
                view.m_sv_listVip_ListView.layoutType = ListView.ListViewLayoutType.RightToLeft;
            }
            else
            {
                view.m_sv_listVip_ListView.layoutType = ListView.ListViewLayoutType.LeftToRight;
            }
            view.m_sv_listVip_ListView.SetInitData(m_assetDic, functab);
            RefreshVipDetailInfo();
            SetShowVipItemIndex(m_showVipItemIndex,false);

            //功能介绍引导
            if (!m_playerProxy.CurrentRoleInfo.vipExpFlag)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.ReceiveVipPoint);
                return;
            }
            if (!m_playerProxy.CurrentRoleInfo.vipFreeBox)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.ReceiveVipBox);
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.ReceiveVipBoxIntro);
        }

        private void InitVipListItem(ListView.ListItem item)
        {
            UI_Item_VipListItem_SubView vipItem = null;
            if (item.data != null)
            {
                vipItem = item.data as UI_Item_VipListItem_SubView;
            }
            else
            {
                vipItem = new UI_Item_VipListItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = vipItem;
            }

            vipItem.OnShowBoxTip += ShowBoxItemTip;
            if (m_currentVipLevel == m_vipInfoList[item.index].level)
            {
                m_curVipIndex = item.index;
            }
            vipItem.SetInfo(m_vipInfoList[item.index],m_currentVipLevel,m_playerProxy.CurrentRoleInfo.vipFreeBox,m_playerProxy.CurrentRoleInfo.vipSpecialBox.Contains(m_vipInfoList[item.index].level));
            vipItem.m_sv_listBuff_ListView.SetParentListView(view.m_sv_listVip_ListView);
            
        }

        private void RemoveVipListItem(ListView.ListItem item)
        {
            UI_Item_VipListItem_SubView vipItem = null;
            if (item.data != null)
            {
                vipItem = item.data as UI_Item_VipListItem_SubView;
            }
            if (vipItem == null)
            {
                return;
            }
            vipItem.OnShowBoxTip -= ShowBoxItemTip;
            vipItem.ClearTimer();
        }

        private void OnVipItemListDragBegin(PointerEventData eventData)
        {
            m_tmpMousePos = eventData.position;
        }

        private void OnVipItemListDragEnd(PointerEventData eventData)
        {
            var width = view.m_sv_listVip_PolygonImage.rectTransform.rect.width;
            var dragDis = eventData.position.x - m_tmpMousePos.x;
            var dragNum = (int)(dragDis/width);
            if (dragDis >= width / 3)
            {
                dragNum++;
            }else if (dragDis <= -width / 3)
            {
                dragNum--;
            }
            SetShowVipItemIndex(m_showVipItemIndex-dragNum*m_isReverse);
        }
        
        private void SetShowVipItemIndex(int num,bool isScroll = true)
        {
            if (num < 0)
            {
                m_showVipItemIndex = 0;
            }else if (num >= m_vipInfoList.Count)
            {
                m_showVipItemIndex = m_vipInfoList.Count - 1;
            }
            else
            {
                m_showVipItemIndex = num;
            }

            if (m_isReverse == -1)
            {
                view.m_btn_arrowBack_GameButton.gameObject.SetActive(m_showVipItemIndex < m_vipInfoList.Count - 1);
                view.m_btn_arrowNext_GameButton.gameObject.SetActive(m_showVipItemIndex > 0);
            }
            else
            {
                view.m_btn_arrowBack_GameButton.gameObject.SetActive(m_showVipItemIndex > 0);
                view.m_btn_arrowNext_GameButton.gameObject.SetActive(m_showVipItemIndex < m_vipInfoList.Count - 1);
            }

            
            if (isScroll)
            {
                view.m_sv_listVip_ListView.ScrollPanelToItemIndex(m_showVipItemIndex);
            }
            else
            {
                view.m_sv_listVip_ListView.MovePanelToItemIndex(m_showVipItemIndex);
            }
        }

        private void ClearAllDetailInfoItem()
        {
            view.m_sv_listVip_ListView.Clear();
        }
        
        #endregion

        #region DailyPointBox

        private void RefreshDailyPointBox()
        {
            bool isOpened = m_playerProxy.CurrentRoleInfo.vipExpFlag;
            view.m_pl_pointbox.SetBox(isOpened);
            view.m_img_redpoint_PolygonImage.gameObject.SetActive(!isOpened);
            ClearTimer();
            
            if (isOpened)
            {
                long refreshTime = ServerTimeModule.Instance.GetServerTime() +
                                   ServerTimeModule.Instance.GetDistanceZeroTime();
                UpdateTime(refreshTime);
                m_dailyPointBoxTimer = Timer.Register(1, () =>
                {
                    UpdateTime(refreshTime);
                },null,true);
            }
            else
            {
                view.m_lbl_boxTime_LanguageText.text = LanguageUtils.getText(762149);
            }
        }

        private void UpdateTime(long refreshTime)
        {
            var remainTimeStr = UIHelper.GetHMSCounterDown(refreshTime);
            if (remainTimeStr ==null)
            {
                ClearTimer();
                return;
            }
            view.m_lbl_boxTime_LanguageText.text = remainTimeStr;
        }
        
        #endregion
        
        

        private void ClearTimer()
        {
            if (m_dailyPointBoxTimer != null)
            {
                m_dailyPointBoxTimer.Cancel();
                m_dailyPointBoxTimer = null;
            }
        }

        #region ClickEvent
        
        private void ShowBoxItemTip(int boxID,string title,Vector3 pos,float radius)
        {
            view.m_UI_Tip_BoxReward.SetInfo(boxID,pos,radius,title);
        }

        private void ClaimFreeBox()
        {
            if (m_playerProxy.CurrentRoleInfo.vipFreeBox)
            {
                return;
            }
            m_playerProxy.CurrentRoleInfo.vipFreeBox = true;
            Role_GetVipFreeBox.request req = new Role_GetVipFreeBox.request();
            AppFacade.GetInstance().SendSproto(req);
        }

        private void BuySpecialBox(int price)
        {
            var priceCfg = CoreUtils.dataService.QueryRecord<PriceDefine>(price);
            m_rechargeProxy.CallSdkBuyByPcid(priceCfg, priceCfg.rechargeID.ToString(), priceCfg.price.ToString("N2"));
        }

        private void OnClickDailyPointBox()
        {
            if (view.m_pl_pointbox.IsOpened())
            {
                return;
            }
            Role_GetVipPoint.request req = new Role_GetVipPoint.request();
            AppFacade.GetInstance().SendSproto(req);
            
        }

        private void OnClickAddVipPoint()
        {
            CoreUtils.uiManager.ShowUI(UI.s_useItem,null, new UseItemViewData()
            {
                ItemType = UseItemType.VipPoint
            });
        }

        private void OnClickLeftPage()
        {
            SetShowVipItemIndex( m_showVipItemIndex - m_isReverse);
        }

        private void OnClickRightPage()
        {
            SetShowVipItemIndex( m_showVipItemIndex + m_isReverse);
        }

        private void OnClickDailyPointDescription()
        {
            HelpTip.CreateTip(LanguageUtils.getText(800032), view.m_btn_question_PolygonImage.transform ).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        private void OnClickClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_Vip);
        }
        #endregion

    }
}