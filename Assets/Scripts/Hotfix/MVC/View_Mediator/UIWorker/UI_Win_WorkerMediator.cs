// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    UI_Win_WorkerMediator
// Copyright IGG All rights reserved.
// ===============================================================================
//m_UI_Model_DoubleLineButton_Blue加速按钮 m_UI_Model_StandardButton_Blue 建造按钮
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;
using SprotoType;
using UnityEngine.UI;

namespace Game {
    public class UI_Win_WorkerMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_WorkerMediator";


        private Timer m_buildqueue1 = null, m_buildqueue2 = null, m_hiretimeDown = null;//计时器
        private long m_buildCostTime1 = 0, m_buildCostTime2 = 0;//建造耗时

        private DataProxy m_dataProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;
        private BagProxy m_bagProxy;
        private TaskProxy m_taskProxy;
        private WorkerProxy m_workerProxy;

        QueueInfo m_mainQueueInfo = null;//主队列
        QueueInfo m_secendQueueInfo = null;//第二队列
        BuildQueueState m_mainQueueState ;
        BuildQueueState m_secendQueueState;
        private int m_buildingTime = 0;
        ItemDefine m_itemDefine = null;//建筑队列延时道具
        #endregion

        //IMediatorPlug needs
        public UI_Win_WorkerMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_WorkerView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                CmdConstant.buildQueueChange,
                Build_UnlockBuildQueue.TagName,
                CmdConstant.UpdateCurrency,
                CmdConstant.ItemInfoChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.buildQueueChange:
                    refreshData();
                    refreshTextAreaView();
                    refreshView();
                    break;
                case Build_UnlockBuildQueue.TagName:
                    Build_UnlockBuildQueue.response response = notification.Body as Build_UnlockBuildQueue.response;
                    if (response != null)
                    {
                        if (response.result)
                        {
                            Tip.CreateTip(180523, Tip.TipStyle.Top).Show();
                        }
                    }
                    break;
                case CmdConstant.UpdateCurrency:
                case CmdConstant.ItemInfoChange:
                    RefrashCurrencyView();
                    break;

                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {
            CancelTimerqueue1();
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_dataProxy = AppFacade.GetInstance().RetrieveProxy(DataProxy.ProxyNAME) as DataProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            m_workerProxy = AppFacade.GetInstance().RetrieveProxy(WorkerProxy.ProxyNAME) as WorkerProxy;
            if (view.data is int)
            {
                m_buildingTime = (int)view.data;
            }
            m_itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(m_playerProxy.ConfigDefine.workQueueItem);
            refreshData();
        }


        protected override void BindUIEvent()
        {
            view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnSpeedUp1BtnClick);
            view.m_UI_Item_Worker1.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnBuilde1BtnClick);
            view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnSpeedUp2BtnClick);
            view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnBuilde2BtnClick);
            view.m_UI_Item_Worker2.m_btn_languageButton_GameButton.onClick.AddListener(OnQueueOpenBtnClick);
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(() => {
                CoreUtils.uiManager.CloseUI(UI.s_worker);
            });
        }
        protected override void BindUIData()
        {
            view.m_UI_Item_Worker2.gameObject.SetActive(true);
            view.m_UI_Item_Worker1.gameObject.SetActive(true);
            view.m_UI_Item_Worker1.m_img_empty_PolygonImage .gameObject.SetActive(false);
            view.m_UI_Item_Worker2.m_img_empty_PolygonImage.gameObject.SetActive(false);
            view.m_UI_Item_Worker1.m_img_place_PolygonImage.gameObject.SetActive(true);
            view.m_UI_Item_Worker2.m_img_place_PolygonImage.gameObject.SetActive(true);
            view.m_UI_Item_Worker2.m_lbl_btnTip_LanguageText.gameObject.SetActive(true);
            view.m_UI_Item_Worker2.m_lbl_btnTip_LanguageText.text = LanguageUtils.getText(180524);
            view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180518);
            InitTextAreaView();
            refreshView();

            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideCheck, (int)EnumFuncGuide.BuilderHut);
        }

        #endregion

        public void refreshData()
        {
            m_playerProxy.GetBuildQueue().TryGetValue(1, out m_mainQueueInfo);
            m_playerProxy.GetBuildQueue().TryGetValue(2, out m_secendQueueInfo);
            m_mainQueueState = m_workerProxy.GetMainQueueState();
            m_secendQueueState = m_workerProxy.GetSecendQueueState(m_buildingTime);
        }
        public void refreshView()
        {
            RefreshMainQueueView();
            RefresSeceneQueueView();
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_ContentSizeFitter.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_ContentSizeFitter.GetComponent<RectTransform>());
        }

        public void refreshTextAreaView()
        {
            if (m_mainQueueState == BuildQueueState.LEISURE1)
            {
                view.m_lbl_TextArea_ArabLayoutCompment .gameObject.SetActive(false);   
                 view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180518);
            }
            else
            {
                switch (m_secendQueueState)
                {
                    case BuildQueueState.LOCK:
                        {
                            if (m_itemDefine != null && m_bagProxy.GetItemNum(m_itemDefine.ID) > 0)
                            {
                                view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180529, LanguageUtils.getText(m_itemDefine.l_nameID));
                            }
                            else
                            {
                                view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180529, LanguageUtils.getText(m_currencyProxy.GetNameidByType(EnumCurrencyType.denar)));
                            }

                        }
                        break;
                    case BuildQueueState.NOLEISURE:
                        view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getText(180531);
                        view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180530);
                        break;
                    case BuildQueueState.LEISURE1:
                        {
                            view.m_lbl_TextArea_ArabLayoutCompment.gameObject.SetActive(false);
                        }
                        break;
                    case BuildQueueState.LEISURE2:
                        long num = m_secendQueueInfo.expiredTime - ServerTimeModule.Instance.GetServerTime();
                        if (m_itemDefine != null && m_bagProxy.GetItemNum(m_itemDefine.ID) > 0)
                        {
                            view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180527, ClientUtils.FormatCountDown((int)num), ClientUtils.FormatCountDown((int)m_buildingTime), LanguageUtils.getText(m_itemDefine.l_nameID));

                        }
                        else
                        {
                            view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180527, ClientUtils.FormatCountDown((int)num), ClientUtils.FormatCountDown((int)m_buildingTime), LanguageUtils.getText(m_currencyProxy.GetNameidByType(EnumCurrencyType.denar)));

                        }
                        view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180525);
                        break;

                }
        }
        }
        public void InitTextAreaView()
        {
            if (m_buildingTime != 0)
            {
                switch (m_secendQueueState)
                {
                    case BuildQueueState.LOCK:
                        {

                            if (m_itemDefine != null && m_bagProxy.GetItemNum(m_itemDefine.ID) > 0)
                            {
                                view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180529, LanguageUtils.getText(m_itemDefine.l_nameID));
                            }
                            else
                            {
                                view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180529, LanguageUtils.getText(m_currencyProxy.GetNameidByType(EnumCurrencyType.denar)));
                            }
                            view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180530);
                        }
                        break;
                    case BuildQueueState.NOLEISURE:
                        view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getText(180531);
                        view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180530);
                        break;
                    case BuildQueueState.LEISURE2:
                        long num = m_secendQueueInfo.expiredTime - ServerTimeModule.Instance.GetServerTime();
                        if (m_itemDefine != null && m_bagProxy.GetItemNum(m_itemDefine.ID) > 0)
                        {
                            view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180527, ClientUtils.FormatCountDown((int)num), ClientUtils.FormatCountDown((int)m_buildingTime), LanguageUtils.getText(m_itemDefine.l_nameID));

                        }
                        else
                        {
                            view.m_lbl_TextArea_LanguageText.text = LanguageUtils.getTextFormat(180527, ClientUtils.FormatCountDown((int)num), ClientUtils.FormatCountDown((int)m_buildingTime), LanguageUtils.getText(m_currencyProxy.GetNameidByType(EnumCurrencyType.denar)));

                        }
                        view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180525);
                        break;

                }
                view.m_lbl_TextArea_ArabLayoutCompment.gameObject.SetActive(true);
            }
            else
            {
                view.m_lbl_TextArea_ArabLayoutCompment.gameObject.SetActive(false);
            }
        }
        public void RefreshMainQueueView()
        {
            if (m_mainQueueState == BuildQueueState.LEISURE1)
            {
                CancelTimerqueue1();
                view.m_UI_Item_Worker1.m_lbl_name_LanguageText.text = LanguageUtils.getText(180517);
                view.m_UI_Item_Worker1.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180510);

                ClientUtils.LoadSprite(view.m_UI_Item_Worker1.m_img_place_PolygonImage, RS.Atrix_icon_1005);
                view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(false);
                view.m_UI_Item_Worker1.m_UI_Model_StandardButton_Blue.gameObject.SetActive(true);
                view.m_UI_Item_Worker1.m_pb_rogressBar_GameSlider.value = 0;

            }
            else
            {
                m_buildCostTime1 = m_mainQueueInfo.firstFinishTime - m_mainQueueInfo.beginTime;
                BuldingObjData buildingInfo = m_cityBuildingProxy.GetBuldingObjDataByIndex(m_mainQueueInfo.buildingIndex);
                if (buildingInfo != null)
                {
                    view.m_UI_Item_Worker1.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(180515, buildingInfo.level + 1, LanguageUtils.getText(buildingInfo.buildingTypeConfigDefine.l_nameId));
                    view.m_UI_Item_Worker1.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180511);
                    ClientUtils.LoadSprite(view.m_UI_Item_Worker1.m_img_place_PolygonImage, RS.Atrix_icon_1004);
                    view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(true);
                    view.m_UI_Item_Worker1.m_UI_Model_StandardButton_Blue.gameObject.SetActive(false);
                    RefreshBuildContent1(m_mainQueueInfo);
                }
            }
        }
        public void RefresSeceneQueueView()
        {
            switch (m_secendQueueState)
            {
                case BuildQueueState.LEISURE1:
                    {
                        CancelTimerqueue2();
                        view.m_UI_Item_Worker2.m_lbl_name_LanguageText.text = LanguageUtils.getText(180517);
                        view.m_UI_Item_Worker2.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180510);

                        ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_place_PolygonImage, RS.Atrix_icon_1005);
                        view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(false);
                        view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Blue.gameObject.SetActive(true);
                        view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Yellow.gameObject.SetActive(false);
                        view.m_UI_Item_Worker2.m_pb_rogressBar_GameSlider.value = 0;
                        view.m_UI_Item_Worker2.m_lbl_btnTip_LanguageText.gameObject.SetActive(false);
                        if ( m_secendQueueInfo.expiredTime == -1)
                        {

                        }
                        else
                        {
                            RefreshHireTimerDown();
                        }

                    }
                    break;
                case BuildQueueState.LEISURE2:
                    {
                        CancelTimerqueue2();
                        view.m_UI_Item_Worker2.m_lbl_name_LanguageText.text = LanguageUtils.getText(180517);
                        view.m_UI_Item_Worker2.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180526);

                        ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_place_PolygonImage, RS.Atrix_icon_1005);
                        view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(false);
                        view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Blue.gameObject.SetActive(false);
                        view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Yellow.gameObject.SetActive(true);
                        view.m_UI_Item_Worker2.m_pb_rogressBar_GameSlider.value = 0;
                        view.m_UI_Item_Worker2.m_lbl_btnTip_LanguageText.gameObject.SetActive(true);
                        RefrashCurrencyView();
                    }
                    break;
                case BuildQueueState.LOCK:
                    {
                        CancelTimerqueue2();
                        view.m_UI_Item_Worker2.m_lbl_name_LanguageText.text = LanguageUtils.getText(180521);
                        view.m_UI_Item_Worker2.m_lbl_armyCount_LanguageText.text = string.Empty;

                        ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_place_PolygonImage, RS.Atrix_icon_1000);
                        view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(false);
                        view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Blue.gameObject.SetActive(false);
                        view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Yellow.gameObject.SetActive(true);
                        view.m_UI_Item_Worker2.m_pb_rogressBar_GameSlider.value = 0;
                        view.m_UI_Item_Worker2.m_lbl_btnTip_LanguageText.gameObject.SetActive(true);

                        RefrashCurrencyView();
                    }
                    break;
                case BuildQueueState.NOLEISURE:
                    {
                        m_buildCostTime2 = m_secendQueueInfo.firstFinishTime - m_secendQueueInfo.beginTime;
                        BuldingObjData buildingInfo = m_cityBuildingProxy.GetBuldingObjDataByIndex(m_secendQueueInfo.buildingIndex);
                        if (buildingInfo != null)
                        {
                            view.m_UI_Item_Worker2.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(180515, buildingInfo.level + 1, LanguageUtils.getText(buildingInfo.buildingTypeConfigDefine.l_nameId));
                            view.m_UI_Item_Worker2.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180511);
                            ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_place_PolygonImage, RS.Atrix_icon_1004);
                            view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(true);
                            view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Blue.gameObject.SetActive(false);
                            view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Yellow.gameObject.SetActive(false);
                            view.m_UI_Item_Worker2.m_lbl_btnTip_LanguageText.gameObject.SetActive(false);

                            RefreshBuildContent2(m_secendQueueInfo);
                        }
                    }
                    break;
                default:
                    {
                        Debug.LogErrorFormat("not find type");
                    }
                    break;
            }
        }
        public void RefrashCurrencyView()
        {
            switch (m_secendQueueState)
            {
                case BuildQueueState.LOCK:
                case BuildQueueState.LEISURE2:
                    {
                        if (m_itemDefine != null && m_bagProxy.GetItemNum(m_itemDefine.ID) > 0)
                        {
                            view.m_UI_Item_Worker2.m_img_frame_PolygonImage.enabled = true;
                            ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_frame_PolygonImage, RS.ItemQualityBg[m_itemDefine.quality - 1]);
                            ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_icon2_PolygonImage, m_itemDefine.itemIcon);
                            view.m_UI_Item_Worker2.m_lbl_line2_LanguageText.text = (LanguageUtils.getTextFormat(180528, 1));
                        }
                        else
                        {
                            view.m_UI_Item_Worker2.m_img_frame_PolygonImage.enabled = false;
                            ClientUtils.LoadSprite(view.m_UI_Item_Worker2.m_img_icon2_PolygonImage, m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.denar));
                            long needDenar = m_playerProxy.ConfigDefine.workQueueDenar;
                            view.m_UI_Item_Worker2.m_lbl_line2_LanguageText.text = (needDenar.ToString("N0"));
                            view.m_UI_Item_Worker2.m_lbl_line2_LanguageText.color = m_currencyProxy.Gem < needDenar ? Color.red : RS.OriginDenarTextColor;

                        }
                        float width = view.m_UI_Item_Worker2.m_lbl_line2_LanguageText.preferredWidth;
                        view.m_UI_Item_Worker2.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 27.7f);
                        LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_Worker2.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                    }
                    break;
            }
            }
        #region 点击事件
        private void OnSpeedUp1BtnClick()
        {
            BuildingInfoEntity BuildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(m_mainQueueInfo.buildingIndex);
            m_cityBuildingProxy.AddBuildingUpdateSpeed(BuildingInfo);
        }
        private void OnBuilde1BtnClick()
        {
            BuildeBtn();
        }
        /// <summary>
        ///  第二队列开启
        /// </summary>
        private void OnQueueOpenBtnClick()
        {
            if (m_itemDefine != null && m_bagProxy.GetItemNum(m_itemDefine.ID) > 0)
            {
                Build_UnlockBuildQueue.request req = new Build_UnlockBuildQueue.request();
                req.itemId = m_itemDefine.ID;
                AppFacade.GetInstance().SendSproto(req);
            }
            else
            {
                long needDenar = m_playerProxy.ConfigDefine.workQueueDenar;
                if (!m_currencyProxy.ShortOfDenar(needDenar))
                {
                    Build_UnlockBuildQueue.request req = new Build_UnlockBuildQueue.request();
                    AppFacade.GetInstance().SendSproto(req);
                }
            }
        }
        private void OnSpeedUp2BtnClick()
        {
            BuildingInfoEntity BuildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(m_secendQueueInfo.buildingIndex);
            m_cityBuildingProxy.AddBuildingUpdateSpeed(BuildingInfo);

        }
        private void OnBuilde2BtnClick()
        {
            BuildeBtn();
        }
        #endregion
        private void BuildeBtn()
        {
            CoreUtils.uiManager.CloseUI(UI.s_worker);
            if (CoreUtils.uiManager.ExistUI(UI.s_buildingUpdate))
            {
                return;
            }
            if (HUDManager.Instance().ExistSingleUIInfo(HUDLayer.citymenu))
            {
                return;
            }
            int citytype = 0;
            if (m_cityBuildingProxy.TryBuildableType(out citytype))
            {
                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, citytype);
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndMoveCameraToBuilding, m_cityBuildingProxy.GetBuildingInfominlevel());
        }
        private void UpdateHireCountDown1(QueueInfo queueInfo)
        {
                long num = queueInfo.expiredTime - ServerTimeModule.Instance.GetServerTime();
            if (num < 0)
            {
                if (m_hiretimeDown != null)
                {
                    m_hiretimeDown.Cancel();
                    m_hiretimeDown = null;
                    m_workerProxy.UpdateBuildQueue();
                    refreshView();
                }
            }
            else
            {
                view.m_UI_Item_Worker2.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(180512, ClientUtils.FormatCountDown((int)num));
            }
        }
        /// <summary>
        /// 更新建造队列倒计时
        /// </summary>
        private void UpdateBuildCountDown1(QueueInfo queueInfo)
        {
            long num = queueInfo.finishTime - ServerTimeModule.Instance.GetServerTime();
            if (num < 0)
            {
                CancelTimerqueue1();
                view.m_UI_Item_Worker1.m_lbl_name_LanguageText.text = LanguageUtils.getText(180517);
                view.m_UI_Item_Worker1.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180510);
                view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(false);
                view.m_UI_Item_Worker1.m_UI_Model_StandardButton_Blue.gameObject.SetActive(true);
                view.m_UI_Item_Worker1.m_pb_rogressBar_GameSlider.value = 0;
            }
            else
            {
                float pro = (float)(m_buildCostTime1 - num) / m_buildCostTime1;
                view.m_UI_Item_Worker1.m_pb_rogressBar_GameSlider.value = pro;
                view.m_UI_Item_Worker1.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_LanguageText.text =  ClientUtils.FormatCountDown((int)num);
            }
        }

        private void UpdateBuildCountDown2(QueueInfo queueInfo)
        {
            long num = queueInfo.finishTime - ServerTimeModule.Instance.GetServerTime();
            if (num <= 0)
            {
                CancelTimerqueue1();
                view.m_UI_Item_Worker2.m_lbl_name_LanguageText.text = LanguageUtils.getText(180517);
                view.m_UI_Item_Worker2.m_lbl_armyCount_LanguageText.text = LanguageUtils.getText(180510);
                view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.gameObject.SetActive(false);
                view.m_UI_Item_Worker2.m_UI_Model_StandardButton_Blue.gameObject.SetActive(true);
                view.m_UI_Item_Worker2.m_pb_rogressBar_GameSlider.value = 0;
            }
            else
            {
                float pro = (float)(m_buildCostTime2 - num) / m_buildCostTime2;
                view.m_UI_Item_Worker2.m_pb_rogressBar_GameSlider.value = pro;
                view.m_UI_Item_Worker2.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown((int)num);
            }
        }
        public void CancelTimerqueue1()
        {
            if (m_buildqueue1 != null)
            {
                m_buildqueue1.Cancel();
                m_buildqueue1 = null;
            }

        }
        public void CancelTimerqueue2()
        {
            if (m_buildqueue2 != null)
            {
                m_buildqueue2.Cancel();
                m_buildqueue2 = null;
            }
        }
        /// <summary>
        /// 雇佣倒计时
        /// </summary>
        public void CancelHireTimerDown()
        {
            if (m_hiretimeDown != null)
            {
                m_hiretimeDown.Cancel();
                m_hiretimeDown = null;
            }
        }
        private void RefreshBuildContent1(QueueInfo queueInfo)
        {
            CancelTimerqueue1();
            UpdateBuildCountDown1(queueInfo);
            if (m_buildqueue1 == null)
            {
                m_buildqueue1 = Timer.Register(1.0f,()=> { UpdateBuildCountDown1(queueInfo); }, null, true, true,view.vb);
            }
        }
        private void RefreshHireTimerDown()
        {
            CancelHireTimerDown();
            UpdateHireCountDown1(m_secendQueueInfo);
            if (m_hiretimeDown == null)
            {
                m_hiretimeDown = Timer.Register(1.0f, () => { UpdateHireCountDown1(m_secendQueueInfo); }, null, true, true, view.vb);
            }
        }
        private void RefreshBuildContent2(QueueInfo queueInfo)
        {
            CancelTimerqueue2();
            UpdateBuildCountDown2(queueInfo);
            if (m_buildqueue2 == null)
            {
                m_buildqueue2 = Timer.Register(1.0f, () => { UpdateBuildCountDown2(queueInfo); }, null, true, true, view.vb);
            }
        }
    }
}