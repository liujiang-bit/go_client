// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年1月8日
// Update Time         :    2020年1月8日
// Class Description   :    HospitalMediator 医院
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
using System;
using Data;
using UnityEngine.UI;

namespace Game {

    public class HospitalMediator : GameMediator {
        #region Member
        public static string NameMediator = "HospitalMediator";

        private PlayerProxy m_playerProxy;
        private HospitalProxy m_hospitalProxy;
        private SoldierProxy m_soldierProxy;
        private CurrencyProxy m_currencyProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private int m_showStatus;
        private Int64 m_capacity = 0;

        private Int64 m_buildingIndex;

        private long m_needDenar;       //立即完成所需代币
        private Color m_originDenarTextColor; //代币的字体颜色

        //有伤兵
        private List<ArmsDefine> m_templeteList;
        private List<Int64> m_selectList;
        private List<Int64> m_resCostList;
        private Int64 m_totalWoundNum = 0;
        private Int64 m_totalTimeCost;
        private ResCostModel m_resCostModel;
        private bool m_isShowMaxBtn;

        //治疗中
        private QueueInfo m_treatmentQueueInfo;
        private Timer m_timer;
        private int m_treatmentIndex;
        private Int64 m_treatmentTime;      //已治疗时间
        private Int64 m_totalTreatmentTime; //总治疗时间

        private List<SoldierInfo> m_soldierList;
        private List<Int64> m_timeCostList;
        private Int64 m_listTotalTime;
        private Int64 m_listTotalCount;

        private bool m_isShowedExplain;
        private int m_cureMinTime;
        private bool m_isDispose;

        private bool m_isInitList;

        private bool m_isLoadPrefab;

        #endregion

        //IMediatorPlug needs
        public HospitalMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public HospitalView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                //CmdConstant.SeriousInjuredChange,
                CmdConstant.treatmentChange,
                Role_Treatment.TagName,
                CmdConstant.UpdateCurrency,
                CmdConstant.AddResWinClose,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (!m_isLoadPrefab)
            {
                return;
            }
            switch (notification.Name)
            {
                //case CmdConstant.SeriousInjuredChange:
                //    ReadWoundNum();
                //    view.m_lbl_countTotal_LanguageText.text = LanguageUtils.getTextFormat(181104, ClientUtils.FormatComma(m_totalWoundNum), ClientUtils.FormatComma(m_capacity));
                //    break;
                case CmdConstant.treatmentChange:
                    TreatmentQueueChange(notification.Body);
                    break;
                case Role_Treatment.TagName: //处理立即完成的士兵获取
                    ImmediatelyCompletedProcess(notification.Body);
                    break;
                case CmdConstant.UpdateCurrency:
                    if (m_showStatus != (int)EnumHospitalStatus.Treatment)
                    {
                        RefrshCostRes();
                        RefreshCostTime();
                    }
                    break;
                case CmdConstant.AddResWinClose:
                    if (m_isShowMaxBtn)
                    {
                        DataProcess2();
                        RefreshMaxButton();
                        RefreshContent();
                    }
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

        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            m_buildingIndex = (Int64)view.data;
            m_hospitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            view.gameObject.SetActive(false);

            DataProcess();

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_armyList_ListView.ItemPrefabDataList, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window.m_btn_close_GameButton.onClick.AddListener(Close);
            view.m_btn_countInfo_GameButton.onClick.AddListener(OnExplain);
            view.m_UI_Model_blue.m_btn_languageButton_GameButton.onClick.AddListener(OnTreatment);
            view.m_UI_Model_yellow.m_btn_languageButton_GameButton.onClick.AddListener(OnNowTreatment);
            view.m_UI_Model_max.m_btn_languageButton_GameButton.onClick.AddListener(OnMax);
            view.m_UI_Model_Window.m_btn_back_GameButton.onClick.AddListener(OnBack);
        }

        protected override void BindUIData()
        {

        }

        public override void OnRemove()
        {
            m_isDispose = true;
            //CancelTimer();
        }

        #endregion

        private void DataProcess()
        {
            m_capacity = m_hospitalProxy.GetHospitalCapacity();

            //判断一下医院状态
            m_showStatus = m_hospitalProxy.GetHospitalStatus();
            if (m_showStatus == (int)EnumHospitalStatus.Wound) //有伤兵
            {
                ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                m_cureMinTime = config.cureMinTime;
                m_selectList = new List<Int64>();
                m_timeCostList = new List<Int64>();
                m_templeteList = new List<ArmsDefine>();
                m_resCostList = new List<Int64>();
                for (int i = 0; i < 4; i++)
                {
                    m_resCostList.Add(0);
                }
                m_soldierList = m_hospitalProxy.GetWoundedData();

                m_totalWoundNum = 0;
                ArmsDefine define;
                int tempId;
                for (int i = 0; i < m_soldierList.Count; i++)
                {
                    m_totalWoundNum = m_totalWoundNum + m_soldierList[i].num;
                    tempId = m_soldierProxy.GetTemplateId((int)m_soldierList[i].type, (int)m_soldierList[i].level);
                    define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                    m_selectList.Add(0);
                    m_timeCostList.Add(0);
                    m_templeteList.Add(define);
                }
                DataProcess2();
            } //治疗中
            else if (m_showStatus == (int)EnumHospitalStatus.Treatment)
            {
                ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                m_cureMinTime = config.cureMinTime;
                m_selectList = new List<Int64>();
                m_timeCostList = new List<Int64>();
                m_templeteList = new List<ArmsDefine>();
                m_soldierList = m_hospitalProxy.GetTreatmentData(1);

                ReadWoundNum();

                ArmsDefine define;
                int tempId;
                for (int i = 0; i < m_soldierList.Count; i++)
                {
                    tempId = m_soldierProxy.GetTemplateId((int)m_soldierList[i].type, (int)m_soldierList[i].level);
                    define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                    m_selectList.Add(0);
                    m_timeCostList.Add(0);
                    m_templeteList.Add(define);
                }
                DataProcess3();
            }
            else
            {
                //Debug.LogFormat("sb卡点操作：{0}", m_showStatus);
                if (m_showStatus == (int)EnumHospitalStatus.Finished)
                {
                    Timer.Register(0.02f, () => {
                        if (m_isDispose)
                        {
                            return;
                        }
                        Close();
                    });
                }
            }
        }

        private void ReadWoundNum()
        {
            m_totalWoundNum = 0;
            List<SoldierInfo> woundedList = m_hospitalProxy.GetWoundedData2();
            for (int i = 0; i < woundedList.Count; i++)
            {
                m_totalWoundNum = m_totalWoundNum + woundedList[i].num;
            }
        }

        #region 加成相关

        //治疗时间加成
        private float GetHealSpeedMulti()
        {
            return 1 + m_playerAttributeProxy.GetCityAttribute(attrType.healSpeedMulti).value;
        }

        //治疗资源消耗加成
        private float GetHealResCostMulti()
        {
            return 1 + m_playerAttributeProxy.GetCityAttribute(attrType.troopsHealthResourcesMulti).value;
        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            if (m_isDispose)
            {
                return;
            }
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);

            int status = m_hospitalProxy.GetHospitalStatus();

            if (status != m_showStatus)
            {
                Close();
                return;
            }

            if (m_showStatus == (int)EnumHospitalStatus.Finished)
            {
                Close();
                return;
            }
            Refresh();
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());

            m_isLoadPrefab = true;
        }

        private void Refresh()
        {
            CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_playerProxy.GetCivilization());
            if (define !=  null)
            {
                ClientUtils.LoadSprite(view.m_img_img_PolygonImage, RS.HospitalMarkFrame[define.hospitalMark]);
            }

            //无伤兵
            if (m_showStatus == (int)EnumHospitalStatus.None)
            {
                view.m_pl_notlEmpty_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_pl_hostipalEmpty_PolygonImage.gameObject.SetActive(true);

                view.m_pl_bg_effect.gameObject.SetActive(false);
                view.m_img_manNone_PolygonImage.gameObject.SetActive(true);
                view.m_img_manFull_PolygonImage.gameObject.SetActive(false);

                view.m_lbl_countTotal_LanguageText.text = LanguageUtils.getTextFormat(181104, ClientUtils.FormatComma(0), ClientUtils.FormatComma(m_capacity));

                return;
            }

            view.m_img_manNone_PolygonImage.gameObject.SetActive(false);
            view.m_img_manFull_PolygonImage.gameObject.SetActive(true);
            view.m_lbl_countTotal_LanguageText.text = LanguageUtils.getTextFormat(181104, ClientUtils.FormatComma(m_totalWoundNum), ClientUtils.FormatComma(m_capacity));

            //有伤兵
            if (m_showStatus == (int)EnumHospitalStatus.Wound)
            {
                view.m_lbl_title2_LanguageText.text = LanguageUtils.getText(181107);
                RefreshMaxButton();
                view.m_UI_Model_yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(181105);

                view.m_pl_bg_effect.gameObject.SetActive(false);
                view.m_pl_res_PolygonImage.gameObject.SetActive(true);
                view.m_pl_timebar_PolygonImage.gameObject.SetActive(false);

                if (m_resCostModel == null)
                {
                    m_resCostModel = new ResCostModel();
                    m_resCostModel.SetTransform(view.m_UI_Model_ResourcesConsume1.gameObject.transform,
                        view.m_UI_Model_ResourcesConsume2.gameObject.transform,
                        view.m_UI_Model_ResourcesConsume3.gameObject.transform,
                        view.m_UI_Model_ResourcesConsume4.gameObject.transform);
                    m_resCostModel.SetText(view.m_UI_Model_ResourcesConsume1.m_lbl_languageText_LanguageText,
                        view.m_UI_Model_ResourcesConsume2.m_lbl_languageText_LanguageText,
                        view.m_UI_Model_ResourcesConsume3.m_lbl_languageText_LanguageText,
                        view.m_UI_Model_ResourcesConsume4.m_lbl_languageText_LanguageText);

                    view.m_UI_Model_ResourcesConsume1.m_btn_btn_GameButton.onClick.AddListener(ClickFood);
                    view.m_UI_Model_ResourcesConsume2.m_btn_btn_GameButton.onClick.AddListener(ClickWood);
                    view.m_UI_Model_ResourcesConsume3.m_btn_btn_GameButton.onClick.AddListener(ClickStone);
                    view.m_UI_Model_ResourcesConsume4.m_btn_btn_GameButton.onClick.AddListener(ClickGold);
                }

                if (!m_isInitList)
                {
                    m_isInitList = true;
                    ListView.FuncTab functab = new ListView.FuncTab();
                    functab.ItemEnter = ListViewItemByIndex;
                    view.m_sv_armyList_ListView.SetInitData(m_assetDic, functab);
                }

                RefreshContent();
            }
            else//治疗中
            {
                view.m_pl_effect.gameObject.SetActive(false);
                view.m_pl_bg_effect.gameObject.SetActive(true);
                view.m_UI_Model_blue.m_lbl_line1_LanguageText.text = LanguageUtils.getText(124001);
                view.m_UI_Model_blue.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false);
                view.m_UI_Model_yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(181105);

                view.m_lbl_title2_LanguageText.text = LanguageUtils.getText(181108);

                //m_timer = Timer.Register(1.0f, UpdateCountDown, null, true, true);
                UpdateCountDown();
                //QueueInfo queueData = new QueueInfo();
                //queueData.finishTime = m_treatmentQueueInfo.finishTime;
                //queueData.beginTime = m_treatmentQueueInfo.beginTime;
                //queueData.treatmentSoldiers = m_treatmentQueueInfo.treatmentSoldiers;
                //queueData.buildingIndex = m_buildingIndex;

                CityHudCountDownManager.Instance.AddUiQueue(null, view.m_lbl_countdown_LanguageText, view.m_sd_count_GameSlider, m_treatmentQueueInfo, UpdateCountDown, TimeEndCallback);

                view.m_pl_res_PolygonImage.gameObject.SetActive(false);
                view.m_pl_timebar_PolygonImage.gameObject.SetActive(true);

                if (!m_isInitList)
                {
                    m_isInitList = true;
                    ListView.FuncTab functab = new ListView.FuncTab();
                    functab.ItemEnter = ListViewItemByIndex;
                    view.m_sv_armyList_ListView.SetInitData(m_assetDic, functab);
                }

                view.m_sv_armyList_ListView.FillContent(m_soldierList.Count);
            }
        }

        private void ClickFood()
        {
            long[] m_rss = new long[4];
            m_rss[0] = m_resCostModel.ResCostData[(int)EnumCurrencyType.food];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void ClickWood()
        {
            long[] m_rss = new long[4];
            m_rss[1] = m_resCostModel.ResCostData[(int)EnumCurrencyType.wood];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void ClickStone()
        {
            long[] m_rss = new long[4];
            m_rss[2] = m_resCostModel.ResCostData[(int)EnumCurrencyType.stone];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void ClickGold()
        {
            long[] m_rss = new long[4];
            m_rss[3] = m_resCostModel.ResCostData[(int)EnumCurrencyType.gold];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void RefreshMaxButton()
        {
            if (m_isShowMaxBtn)
            {
                view.m_UI_Model_max.m_lbl_line1_LanguageText.text = LanguageUtils.getText(200024);
                view.m_UI_Model_max.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false);

                view.m_UI_Model_max.gameObject.SetActive(true);
                view.m_UI_Model_blue.gameObject.SetActive(false);
                view.m_UI_Model_yellow.gameObject.SetActive(false);
            }
            else
            {
                view.m_UI_Model_blue.m_lbl_line1_LanguageText.text = LanguageUtils.getTextFormat(181106);
                view.m_UI_Model_blue.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                ClientUtils.LoadSprite(view.m_UI_Model_blue.m_img_icon2_PolygonImage, RS.CountDownIcon);

                view.m_UI_Model_max.gameObject.SetActive(false);
                view.m_UI_Model_blue.gameObject.SetActive(true);
                view.m_UI_Model_yellow.gameObject.SetActive(true);
            }
        }

        private void RefreshContent()
        {
            view.m_sv_armyList_ListView.FillContent(m_soldierList.Count);

            RefrshCostRes();
            RefreshCostTime();
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_blue.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_HospitalArmyCount_SubView itemView = null; 
            if (listItem.data != null)
            {
                itemView = listItem.data as UI_Item_HospitalArmyCount_SubView;
            }
            else
            {
                itemView = new UI_Item_HospitalArmyCount_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = itemView;

                InputSliderControl control = new InputSliderControl();
                control.Init(itemView.m_ipt_ArmyInput_GameInput, itemView.m_sd_count_GameSlider, itemView.m_lbl_show_LanguageText, NumChangeCallback);
                itemView.InputSdControl = control;
            }

            if (m_showStatus == (int)EnumHospitalStatus.Treatment)
            {
                if (itemView.m_img_handle_PolygonImage.gameObject.activeSelf)
                {
                    itemView.m_img_handle_PolygonImage.gameObject.SetActive(false);
                    itemView.m_sd_count_GameSlider.interactable = false;

                    itemView.m_ipt_ArmyInput_GameInput.interactable = false;
                    itemView.m_ipt_ArmyInput_PolygonImage.raycastTarget = false;
                }
                itemView.m_pl_effect.gameObject.SetActive(false);
                //itemView.m_pl_effect.gameObject.SetActive(listItem.index == m_treatmentIndex);
            }
            else
            {
                if (!itemView.m_img_handle_PolygonImage.gameObject.activeSelf)
                {
                    itemView.m_img_handle_PolygonImage.gameObject.SetActive(true);
                    itemView.m_sd_count_GameSlider.interactable = true;
                    itemView.m_ipt_ArmyInput_GameInput.interactable = true;
                    itemView.m_ipt_ArmyInput_PolygonImage.raycastTarget = true;
                }
                itemView.m_pl_effect.gameObject.SetActive(false);
            }

            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, SoldierProxy.GetArmyHeadIcon(m_templeteList[listItem.index]));
            if (m_showStatus == (int)EnumHospitalStatus.Wound)
            {
                itemView.m_lbl_ArmyName_LanguageText.text = LanguageUtils.getTextFormat(192035, LanguageUtils.getText(m_templeteList[listItem.index].l_armsID), ClientUtils.FormatComma(m_soldierList[listItem.index].num));
            }
            else
            {
                itemView.m_lbl_ArmyName_LanguageText.text = LanguageUtils.getText(m_templeteList[listItem.index].l_armsID);
            }
            itemView.m_lbl_level_LanguageText.text = m_soldierList[listItem.index].level.ToString();

            itemView.InputSdControl.UpdateMinMax(0, (int)m_soldierList[listItem.index].num);
            itemView.InputSdControl.UpdateIndex(listItem.index);
            itemView.InputSdControl.SetInputVal((int)m_selectList[listItem.index]);
        }

        private void NumChangeCallback(int num, int index)
        {
            Int64 oldNum = m_selectList[index];
            m_selectList[index] = num;
            if (m_showStatus == (int)EnumHospitalStatus.Wound)
            {
                if (m_templeteList[index].woundedFood > 0)
                {
                    m_resCostList[0] = m_resCostList[0] - (Int64)Mathf.Floor(m_templeteList[index].woundedFood * oldNum * GetHealResCostMulti()) + (Int64)Mathf.Floor(m_templeteList[index].woundedFood * num * GetHealResCostMulti());
                }
                if (m_templeteList[index].woundedWood > 0)
                {
                    m_resCostList[1] = m_resCostList[1] - (Int64)Mathf.Floor(m_templeteList[index].woundedWood * oldNum * GetHealResCostMulti()) + (Int64)Mathf.Floor(m_templeteList[index].woundedWood * num * GetHealResCostMulti());
                }
                if (m_templeteList[index].woundedStone > 0)
                {
                    m_resCostList[2] = m_resCostList[2] - (Int64)Mathf.Floor(m_templeteList[index].woundedStone * oldNum * GetHealResCostMulti()) + (Int64)Mathf.Floor(m_templeteList[index].woundedStone * num * GetHealResCostMulti());
                }
                if (m_templeteList[index].woundedGlod > 0)
                {
                    m_resCostList[3] = m_resCostList[3] - (Int64)Mathf.Floor(m_templeteList[index].woundedGlod * oldNum * GetHealResCostMulti()) + (Int64)Mathf.Floor(m_templeteList[index].woundedGlod * num * GetHealResCostMulti());
                }

                m_listTotalCount = m_listTotalCount - oldNum + num;
                m_listTotalTime = m_listTotalTime - m_timeCostList[index];
                m_timeCostList[index] = m_templeteList[index].woundedTime * num;
                m_listTotalTime = m_listTotalTime + m_timeCostList[index];

                m_totalTimeCost = Mathf.FloorToInt(m_listTotalTime / GetHealSpeedMulti());

                if (m_totalTimeCost < m_cureMinTime)
                {
                    if (m_listTotalCount > 0)
                    {
                        m_totalTimeCost = m_cureMinTime;
                    }
                }

                RefrshCostRes();
                RefreshCostTime();

                m_isShowMaxBtn = (m_totalTimeCost == 0);
                if (m_isShowMaxBtn != view.m_UI_Model_max.gameObject.activeSelf)
                {
                    RefreshMaxButton();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_blue.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                }
            }
        }

        private void RefreshCostTime()
        {
            view.m_UI_Model_blue.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown((int)m_totalTimeCost);

            m_needDenar = m_currencyProxy.CaculateImmediatelyFinishPrice(m_totalTimeCost, m_resCostList[0], m_resCostList[1], m_resCostList[2], m_resCostList[3]);
            view.m_UI_Model_yellow.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
            if (m_originDenarTextColor != Color.red)
            {
                m_originDenarTextColor = view.m_UI_Model_yellow.m_lbl_line2_LanguageText.color;
            }
            Color changedColor = m_currencyProxy.Gem < m_needDenar ? Color.red : m_originDenarTextColor;
            view.m_UI_Model_yellow.m_lbl_line2_LanguageText.color = changedColor;
        }

        private void RefrshCostRes()
        {
            if (m_resCostModel != null)
            {
                m_resCostModel.UpdateResCost(m_resCostList[0], m_resCostList[1], m_resCostList[2], m_resCostList[3]);
            }         
        }

        private void UpdateCountDown()
        {
            if (m_isDispose)
            {
                return;
            }
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 speedTime = m_treatmentQueueInfo.firstFinishTime - m_treatmentQueueInfo.finishTime;
            Int64 residueTime = m_treatmentQueueInfo.finishTime - serverTime; //剩余时间
            residueTime = (residueTime < 0) ? 0 : residueTime;
            Int64 costTime = serverTime - m_treatmentQueueInfo.beginTime + speedTime;
            costTime = (costTime < 0) ? 0 : costTime;

            m_needDenar = m_currencyProxy.CaculateImmediatelyFinishPrice(residueTime);
            view.m_UI_Model_yellow.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
            if (m_originDenarTextColor != Color.red)
            {
                m_originDenarTextColor = view.m_UI_Model_yellow.m_lbl_line2_LanguageText.color;
            }
            Color changedColor = m_currencyProxy.Gem < m_needDenar ? Color.red : m_originDenarTextColor;
            view.m_UI_Model_yellow.m_lbl_line2_LanguageText.color = changedColor;
            float pro = 0f;
            if (costTime > m_treatmentTime)
            {
                int beforeIndex = m_treatmentIndex;
                m_treatmentIndex = m_treatmentIndex + 1;
                if (m_treatmentIndex >= m_timeCostList.Count)
                {
                    return;
                }
                m_treatmentTime = m_treatmentTime + m_timeCostList[m_treatmentIndex];

                //刷新上一个listItem显示
                if (beforeIndex > -1 && beforeIndex < m_timeCostList.Count)
                {
                    m_selectList[beforeIndex] = m_soldierList[beforeIndex].num;
                    view.m_sv_armyList_ListView.RefreshItem(beforeIndex);
                }
            }
            if (m_timeCostList[m_treatmentIndex] > 0)
            {
                pro = (float)(costTime - (m_treatmentTime - m_timeCostList[m_treatmentIndex])) / m_timeCostList[m_treatmentIndex];
            }
            m_selectList[m_treatmentIndex] = (Int64)(pro * m_soldierList[m_treatmentIndex].num);

            ListView.ListItem itemList = view.m_sv_armyList_ListView.GetItemByIndex(m_treatmentIndex);
            if (itemList != null && itemList.data != null)
            {
                UI_Item_HospitalArmyCount_SubView itemView = itemList.data as UI_Item_HospitalArmyCount_SubView;
                itemView.InputSdControl.SetInputVal((int)m_selectList[m_treatmentIndex]);
                itemView.m_pl_effect.gameObject.SetActive(false);
            }
        }

        private void TimeEndCallback(Int64 buildingIndex)
        {
            if (m_isDispose)
            {
                return;
            }
            for (int i = 0; i < m_soldierList.Count; i++)
            {
                m_selectList[i] = m_soldierList[i].num;
            }
            view.m_sv_armyList_ListView.ForceRefresh();
            Timer.Register(0.2f, Close);
        }

        //说明
        private void OnExplain()
        {
            view.m_pl_explain.gameObject.SetActive(true);
            view.m_pl_right_content_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_count.gameObject.SetActive(false);
            view.m_UI_Model_Window.m_btn_back_GameButton.gameObject.SetActive(true);

            if (!m_isShowedExplain)
            {
                m_isShowedExplain = true;
                view.m_lbl_explain_LanguageText.text = LanguageUtils.getText(181001);
                float textHeight = view.m_lbl_explain_LanguageText.preferredHeight- view.m_lbl_explain_LanguageText.GetComponent<RectTransform>().anchoredPosition.y;
                RectTransform listRect = view.m_c_explain_view.GetComponent<RectTransform>();
                listRect.sizeDelta = new Vector2(listRect.sizeDelta.x, textHeight);
            }
        }

        private void OnBack()
        {
            view.m_pl_explain.gameObject.SetActive(false);
            view.m_pl_right_content_ArabLayoutCompment.gameObject.SetActive(true);
            view.m_pl_count.gameObject.SetActive(true);
            view.m_UI_Model_Window.m_btn_back_GameButton.gameObject.SetActive(false);
        }

        //治疗 加速
        private void OnTreatment()
        {
            if (m_showStatus == (int)EnumHospitalStatus.Treatment) //正在治疗中
            {
                m_treatmentQueueInfo = m_playerProxy.GetTreatmentQueue();
                CivilizationDefine civilizationDefine = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_playerProxy.GetCivilization());
                if (civilizationDefine != null)
                {
                    SpeedUpData data = new SpeedUpData
                    {
                        type = EnumSpeedUpType.heal,
                        queue = m_treatmentQueueInfo,
                        iconRes = RS.HospitalMarkFrame[civilizationDefine.hospitalMark]
                    };
                    AppFacade.GetInstance().SendNotification(CmdConstant.SpeedUp, data);
                }
                else
                {
                    Debug.LogError("文明读取失败");
                }
                return;
            }
            if (m_selectList == null)
            {
                Debug.LogError("m_selectList is null");
                return;
            }

            if (m_resCostModel == null)
            {
                Debug.LogError("m_resCostModel is null");
                return;
            }

            //至少需要选择一个
            Int64 count = 0;
            for (int i = 0; i < m_selectList.Count; i++)
            {
                count = m_selectList[i] + count;
            }
            if (count == 0)
            {
                Debug.LogError("至少需要选择一个");
                return;
            }

            bool isAllEnough = true;
            Dictionary<int, Int64> notEnoughResDic = new Dictionary<int, long>();
            Int64 num = 0;
            foreach (var data in m_resCostModel.ResCostData)
            {
                if (data.Value > 0)
                {
                    num = data.Value - m_playerProxy.GetResNumByType(data.Key);
                    if (num > 0)
                    {
                        isAllEnough = false;
                        notEnoughResDic[data.Key] = num;
                    }
                }
            }
            if (!isAllEnough)
            {
                //Debug.LogError(LanguageUtils.getText(192013));
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                Dictionary<int, long> cost = m_resCostModel.ResCostData;
                long food = cost.ContainsKey((int)EnumCurrencyType.food) ? cost[(int)EnumCurrencyType.food] : 0;
                long wood = cost.ContainsKey((int)EnumCurrencyType.wood) ? cost[(int)EnumCurrencyType.wood] : 0;
                long stone = cost.ContainsKey((int)EnumCurrencyType.stone) ? cost[(int)EnumCurrencyType.stone] : 0;
                long gold = cost.ContainsKey((int)EnumCurrencyType.gold) ? cost[(int)EnumCurrencyType.gold] : 0;
                currencyProxy.LackOfResources(food, wood, stone, gold);
                return;
            }

            //发包
            Dictionary<Int64, SoldierInfo> soldierDic = new Dictionary<Int64, SoldierInfo>();
            for (int i = 0; i < m_selectList.Count; i++)
            {
                if (m_selectList[i] > 0)
                {
                    SoldierInfo sInfo = new SoldierInfo();
                    sInfo.id = m_soldierList[i].id;
                    sInfo.type = m_soldierList[i].type;
                    sInfo.level = m_soldierList[i].level;
                    sInfo.num = m_selectList[i];                    
                    soldierDic[m_soldierList[i].id] = sInfo;
                }
            }

            CoreUtils.audioService.PlayOneShot(RS.SoundUiStartHealing, null);

            //发包
            var sp = new Role_Treatment.request();
            sp.soldiers = soldierDic;
            AppFacade.GetInstance().SendSproto(sp);

            Close();
        }

        //立即治疗
        private void OnNowTreatment()
        {
            if(m_currencyProxy.ShortOfDenar(m_needDenar))
            {
                return;
            }

            if (m_selectList == null)
            {
                Debug.LogError("m_selectList is null");
                return;
            }

            //至少需要选择一个
            Int64 count = 0;
            for (int i = 0; i < m_selectList.Count; i++)
            {
                count = m_selectList[i] + count;
            }
            if (count == 0)
            {
                Debug.LogError("至少需要选择一个");
                return;
            }
            UIHelper.DenarCostRemain(m_needDenar, () =>
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiStartHealing, null);
                //发包
                var sp = new Role_Treatment.request();
                sp.immediately = true;
                if (m_showStatus == (int)EnumHospitalStatus.Treatment)
                {
                    QueueInfo queueInfo = m_playerProxy.GetTreatmentQueue();
                    if (queueInfo == null)
                    {
                        return;
                    }
                    long queueIndex = queueInfo.queueIndex;
                    sp.treatmentQueueIndex = queueIndex;
                    OnPowerUpShow(queueInfo.treatmentSoldiers);
                }
                else
                {
                    Dictionary<Int64, SoldierInfo> soldierDic = new Dictionary<Int64, SoldierInfo>();
                    for (int i = 0; i < m_selectList.Count; i++)
                    {
                        if (m_selectList[i] > 0)
                        {
                            SoldierInfo sInfo = new SoldierInfo();
                            sInfo.id = m_soldierList[i].id;
                            sInfo.type = m_soldierList[i].type;
                            sInfo.level = m_soldierList[i].level;
                            sInfo.num = m_selectList[i];
                            soldierDic[m_soldierList[i].id] = sInfo;
                        }
                    }
                    sp.soldiers = soldierDic;
                    OnPowerUpShow(soldierDic);
                }
                AppFacade.GetInstance().SendSproto(sp);
            });

        }

        private void OnPowerUpShow(Dictionary<long, SoldierInfo> soldiers)
        {
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            foreach (var element in soldiers.Values)
            {
                int tmpID = m_soldierProxy.GetTemplateId((int)element.type, (int)element.level);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tmpID);
                if(define!=null)
                {
                    CoreUtils.assetService.LoadAssetAsync<Sprite>(define.icon,(asset)=>
                    {
                        UnityEngine.Object go = asset.asset() as UnityEngine.Object;
                        Sprite sprite = go as Sprite;
                        GameObject tmpObj = new GameObject();
                        tmpObj.AddComponent<Image>().sprite = sprite;
                        mt.FlyPowerUpEffect(tmpObj, view.m_UI_Model_yellow.m_img_img_PolygonImage.rectTransform, Vector3.one);
                        GameObject.DestroyImmediate(tmpObj);
                    }, view.gameObject);
                }
            }
        }

        //最大
        private void OnMax()
        {
            Int64 totalTime = DataProcess2(false);
            if (totalTime > 0)
            {
                DataProcess2();
                RefreshMaxButton();
                RefreshContent();
            }
            else
            {
                Int64 food = 0 ;
                Int64 wood = 0;
                Int64 stone = 0;
                Int64 gold = 0;
                Int64 num =0;
                for (int i = 0; i < m_soldierList.Count; i++)
                {
                    num = m_soldierList[i].num;
                    if (m_templeteList[i].woundedFood > 0)
                    {
                        food += (Int64)Mathf.Floor(m_templeteList[i].woundedFood * num* GetHealResCostMulti());
                    }
                    if (m_templeteList[i].woundedWood > 0)
                    {
                        wood += (Int64)Mathf.Floor(m_templeteList[i].woundedWood * num* GetHealResCostMulti());
                    }
                    if (m_templeteList[i].woundedStone > 0)
                    {
                        stone += (Int64)Mathf.Floor(m_templeteList[i].woundedStone * num* GetHealResCostMulti());
                    }
                    if (m_templeteList[i].woundedGlod > 0)
                    {
                        gold += (Int64)Mathf.Floor(m_templeteList[i].woundedGlod * num* GetHealResCostMulti());
                    }
                }
                //Debug.LogError("弹出资源获取界面");
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                currencyProxy.LackOfResources(food, wood, stone, gold);
            }
        }

        //取消倒计时
        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private Int64 DataProcess2(bool isRefreshVal = true)
        {
            ArmsDefine define;
            Int64 costTime = 0;
            Int64 tmpTotal = 0;
            Int64 ownFood = m_playerProxy.GetResNumByType((int)EnumCurrencyType.food);
            Int64 ownWood = m_playerProxy.GetResNumByType((int)EnumCurrencyType.wood);
            Int64 ownStone = m_playerProxy.GetResNumByType((int)EnumCurrencyType.stone);
            Int64 ownGold = m_playerProxy.GetResNumByType((int)EnumCurrencyType.gold);

            Int64 food = 0;
            Int64 wood = 0;
            Int64 stone = 0;
            Int64 gold = 0;

            List<float> tempList = new List<float>();
            for (int i = 0; i < 4; i++)
            {
                tempList.Add(0);
            }

            if (isRefreshVal)
            {
                m_resCostList[0] = 0;
                m_resCostList[1] = 0;
                m_resCostList[2] = 0;
                m_resCostList[3] = 0;
                m_totalTimeCost = 0;
                m_listTotalTime = 0;
                m_isShowMaxBtn = true;
                m_listTotalCount = 0;
            }

            Int64 canCount = 0;
            Int64 tempTotalTime = 0;
            for (int i = 0; i < m_soldierList.Count; i++)
            {
                define = m_templeteList[i];
                food = (Int64)Mathf.Floor(define.woundedFood * m_soldierList[i].num* GetHealResCostMulti());
                wood = (Int64)Mathf.Floor(define.woundedWood * m_soldierList[i].num* GetHealResCostMulti());
                stone = (Int64)Mathf.Floor(define.woundedStone * m_soldierList[i].num* GetHealResCostMulti());
                gold = (Int64)Mathf.Floor(define.woundedGlod * m_soldierList[i].num* GetHealResCostMulti());

                if (ownFood >= food && ownWood >= wood && ownStone >= stone && ownGold >= gold)
                {
                    ownFood = ownFood - food;
                    ownWood = ownWood - wood;
                    ownStone = ownStone - stone;
                    ownGold = ownGold - gold;

                    canCount = m_soldierList[i].num;

                    if (isRefreshVal)
                    {
                        m_resCostList[0] = m_resCostList[0] + food;
                        m_resCostList[1] = m_resCostList[1] + wood;
                        m_resCostList[2] = m_resCostList[2] + stone;
                        m_resCostList[3] = m_resCostList[3] + gold;
                        m_selectList[i] = canCount;
                        m_isShowMaxBtn = false;
                    }
                }
                else
                {
                    tempList[0] = (define.woundedFood > 0) ? (float)ownFood / define.woundedFood* GetHealResCostMulti() : -1f;
                    tempList[1] = (define.woundedWood > 0) ? (float)ownWood / define.woundedWood* GetHealResCostMulti() : -1f;
                    tempList[2] = (define.woundedStone > 0) ? (float)ownStone / define.woundedStone* GetHealResCostMulti() : -1f;
                    tempList[3] = (define.woundedGlod > 0) ? (float)ownGold / define.woundedGlod * GetHealResCostMulti() : -1f;

                    float num = -1;
                    for (int j = 0; j < tempList.Count; j++)
                    {
                        if (tempList[j] > -1)
                        {
                            if (num == -1)
                            {
                                num = tempList[j];
                            }
                            else
                            {
                                if (tempList[j] < num)
                                {
                                    num = tempList[j];
                                }
                            }
                        }
                    }
                    int count = (int)Mathf.Floor(num);
                    if (count > 0)
                    {
                        Int64 costFood = (Int64)Mathf.Floor(count * define.woundedFood * GetHealResCostMulti());
                        Int64 costWood = (Int64)Mathf.Floor(count * define.woundedWood * GetHealResCostMulti());
                        Int64 costStone = (Int64)Mathf.Floor(count * define.woundedStone * GetHealResCostMulti());
                        Int64 costGold = (Int64)Mathf.Floor(count * define.woundedGlod * GetHealResCostMulti());
                        ownFood = ownFood - costFood;
                        ownWood = ownWood - costWood;
                        ownStone = ownStone - costStone;
                        ownGold = ownGold - costGold;

                        canCount = count;

                        if (isRefreshVal)
                        {
                            m_resCostList[0] = m_resCostList[0] + costFood;
                            m_resCostList[1] = m_resCostList[1] + costWood;
                            m_resCostList[2] = m_resCostList[2] + costStone;
                            m_resCostList[3] = m_resCostList[3] + costGold;
                            m_selectList[i] = canCount;
                            m_isShowMaxBtn = false;
                        }
                    }
                    else
                    {
                        canCount = 0;

                        if (isRefreshVal)
                        {
                            m_selectList[i] = canCount;
                        }
                    }
                }

                costTime = canCount * define.woundedTime;

                tempTotalTime = tempTotalTime + costTime;

                tmpTotal = tmpTotal + canCount;

                if (isRefreshVal)
                {
                    m_timeCostList[i] = costTime;
                    m_listTotalTime = tempTotalTime;
                    m_listTotalCount = tmpTotal;
                }
            }

            tempTotalTime = Mathf.FloorToInt((float)tempTotalTime / GetHealSpeedMulti());
            if (tempTotalTime < m_cureMinTime)
            {
                if (tmpTotal > 0)
                {
                    tempTotalTime = m_cureMinTime;
                }
            }

            if (isRefreshVal)
            {
                //总消耗时间
                m_totalTimeCost = tempTotalTime;
            }

            return tempTotalTime;
        }

        //治疗中数据处理
        private void DataProcess3()
        {
            m_treatmentQueueInfo = m_playerProxy.GetTreatmentQueue();
            if (m_treatmentQueueInfo == null)
            {
                Debug.LogError("治疗队列不存在");
                return;
            }
            m_totalTreatmentTime = m_treatmentQueueInfo.finishTime - m_treatmentQueueInfo.beginTime;
            m_totalTreatmentTime = (m_totalTreatmentTime <= 0) ? 1 : m_totalTreatmentTime;

            //治疗时加成
            //float healSpeedMulti = 1 + ((float)m_treatmentQueueInfo.healSpeedMulti / 1000);

            //计算加成前总时间
            Int64 initMultiTotalTime = m_treatmentQueueInfo.firstFinishTime - m_treatmentQueueInfo.beginTime;

            Int64 woundedTotalTime = 0;
            Dictionary<int, Int64> typeNumDic = new Dictionary<int, Int64>();
            Dictionary<int, Int64> timeNumDic = new Dictionary<int, Int64>();
            Dictionary<int, List<int>> woundDicList = new Dictionary<int, List<int>>();
            ArmsDefine define;
            for (int i = 0; i < m_soldierList.Count; i++)
            {
                define = m_templeteList[i];
                if (!typeNumDic.ContainsKey(define.armsLv))
                {
                    timeNumDic[define.armsLv] = 0;
                    typeNumDic[define.armsLv] = 0;
                    woundDicList[define.armsLv] = new List<int>();
                }
                typeNumDic[define.armsLv] = typeNumDic[define.armsLv] + m_soldierList[i].num * define.woundedTime;
                woundedTotalTime = woundedTotalTime + m_soldierList[i].num * define.woundedTime;
                woundDicList[define.armsLv].Add(i);
            }
            int lastKey = 0;
            Int64 tempTime = initMultiTotalTime;
            foreach (var data in typeNumDic)
            {
                if (data.Key > 1)
                {
                    timeNumDic[data.Key] = Mathf.FloorToInt(((float)data.Value/ woundedTotalTime) * initMultiTotalTime);
                    tempTime = tempTime - timeNumDic[data.Key];
                    lastKey = data.Key;
                }
            }
            if (timeNumDic.ContainsKey(1))
            {
                timeNumDic[1] = tempTime;
            }
            else
            {
                timeNumDic[lastKey] = timeNumDic[lastKey] + tempTime;
            }
            //Debug.LogError("字典");
            //ClientUtils.Print(timeNumDic);

            Int64 tempTime2 = 0;
            Int64 residueTime = 0;
            int index = 0;
            foreach (var data in timeNumDic)
            {
                tempTime2 = data.Value;
                residueTime = data.Value;
                for (int i = 0; i < woundDicList[data.Key].Count; i++)
                {
                    index = woundDicList[data.Key][i];
                    if (residueTime > 0)
                    {
                        int num = 0;
                        if (typeNumDic[data.Key] > 0)
                        {
                            num = Mathf.FloorToInt((float)(m_soldierList[index].num * m_templeteList[index].woundedTime) / typeNumDic[data.Key] * tempTime2);
                        }
                        else
                        {
                            num = Mathf.CeilToInt((float)(i + 1) / woundDicList[data.Key].Count * tempTime2);
                        }
                        if (residueTime - num >= 0)
                        {
                            residueTime = residueTime - num;
                            m_timeCostList[index] = num;
                        }
                        else
                        {
                            m_timeCostList[index] = residueTime > 0? residueTime:0;
                            residueTime = 0;
                        }
                    }
                    else
                    {
                        m_timeCostList[index] = 0;
                    }
                }
            }
            //ClientUtils.Print(m_timeCostList);
            UpdateTreatmentData();
        }

        private void UpdateTreatmentData()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 speedTime = m_treatmentQueueInfo.firstFinishTime - m_treatmentQueueInfo.finishTime;
            Int64 diffTime = serverTime - m_treatmentQueueInfo.beginTime + speedTime;
            diffTime = (diffTime < 0) ? 0 : diffTime;

            Int64 tTime = 0;
            m_treatmentTime = 0;
            for (int i = 0; i < m_soldierList.Count; i++)
            {
                tTime = tTime + m_timeCostList[i];
                if (diffTime < tTime)
                {
                    m_treatmentTime = tTime;
                    m_treatmentIndex = i;

                    float pro = (float)(diffTime - (m_treatmentTime - m_timeCostList[i])) / m_timeCostList[i];
                    m_selectList[i] = (Int64)(pro * m_soldierList[i].num);
                    break;
                }
                else
                {
                    m_selectList[i] = m_soldierList[i].num;
                }
            }
        }

        //立即完成回包处理
        private void ImmediatelyCompletedProcess(object body)
        {
            var respone = body as Role_Treatment.response;
            if (respone == null)
            {
                return;
            }
            if (respone.soldiers == null)
            {
                return;
            }
            //tip提醒 获取多少士兵
            Int64 num = 0;
            foreach (var data in respone.soldiers)
            {
                num = num + data.Value.num;
            }
            if (num == 0)
            {
                return;
            }
            string str = LanguageUtils.getTextFormat(181101, num);
            Tip.CreateTip(str).Show();

            //清除定时器
            if (m_showStatus == (int)EnumHospitalStatus.Treatment)
            {
                CityHudCountDownManager.Instance.RemoveUiQueue(view.m_sd_count_GameSlider.GetHashCode());
            }
            //清空列表
            view.m_sv_armyList_ListView.RefreshAndRestPos(0);
            //重新设置数据
            DataProcess();
            //刷新
            Refresh();
        }

        //治疗队列信息变更
        private void TreatmentQueueChange(object body)
        {
            RoleInfo info = body as RoleInfo;
            if (info != null)
            {
                if (info.treatmentQueue != null)
                {
                    //Debug.LogFormat("治疗： beginTime:{0} finishTime:{1} firstTime:{2}", info.treatmentQueue.beginTime, info.treatmentQueue.finishTime, info.treatmentQueue.firstFinishTime);
                    if (info.treatmentQueue.finishTime > 0)
                    {
                        if (m_showStatus == (int)EnumHospitalStatus.Treatment)
                        {
                            m_treatmentQueueInfo = m_playerProxy.GetTreatmentQueue();
                            CityHudCountDownManager.Instance.AddUiQueue(null, view.m_lbl_countdown_LanguageText, view.m_sd_count_GameSlider, m_treatmentQueueInfo, UpdateCountDown, TimeEndCallback);

                            //刷新数据
                            UpdateTreatmentData();
                            //刷新列表
                            view.m_sv_armyList_ListView.FillContent(m_soldierList.Count);
                        }
                    }
                }
            }
        }

        private void Close()
        {
            if (m_isDispose)
            {
                return;
            }
            CoreUtils.uiManager.CloseUI(UI.s_hospitalInfo);
        }
    }
}