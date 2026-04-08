// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    TrainArmyMediator 军队训练
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
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;

namespace Game {
    public class TrainArmyResCost
    {
        public Int64 Cost;
        public Int64 OwnNum;
    }

    public class TrainArmyMediator : GameMediator {
        #region Member
        public static string NameMediator = "TrainArmyMediator";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_buildingProxy;
        private TrainProxy m_trainProxy;
        private SoldierProxy m_soldierProxy;
        private CurrencyProxy m_currencyProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;

        private Dictionary<int, int> ArmTypeKeyMap = new Dictionary<int, int>(); // 兵种类型映射

        private bool m_isTraining;             //是否正在训练 
        private int m_buildingType = 0;        //建筑类型
        private Int64 m_buildingIndex = 0;
        private BuildingInfoEntity m_buildingInfo;
        private int m_armyType = 0;         //兵种类型
        private int m_unlockMaxIndex = 0;   //已解锁最大index
        private int m_selectIndex;          //被选中索引
        private Int64 m_trainCapacity;      //训练容量
        private Int64 m_currTrainCount;     //当前训练数量
        private long m_needDenar;            //立即完成所需代币
        private Color m_originDenarTextColor; //代币的字体颜色

        private string m_titleText = "";

        //待训练
        private List<Int64> m_armyDataList = new List<Int64>();
        private List<ArmsDefine> m_armyList = new List<ArmsDefine>();
        private Dictionary<int, bool> m_unlockDic = new Dictionary<int, bool>();    //解锁统计
        private List<UI_Model_ArmyTrainHead_SubView> m_headViewList = new List<UI_Model_ArmyTrainHead_SubView>();

        //属性详情
        private int m_detailLoadStatus; // 0未加载 1加载中 2已加载
        private GameObject m_detailObj;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        //训练中 
        private Int64 m_finishTime;
        private Int64 m_startTime;
        private Int64 m_trainCostTime;
        private Int64 m_trainLevel;
        private Int64 m_trainNum;
        private int m_trainingIndex;

        //晋升
        private int m_upItemLoadStatus; // 0未加载 1加载中 2已加载
        private GameObject m_upItemObj;
        private ItemTrainUpView m_itemTrainUpView;
        private Int64 m_currUpCount;

        private bool m_isDispose;

        private bool m_isInitData;

        #endregion

        //IMediatorPlug needs
        public TrainArmyMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public TrainArmyView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                CmdConstant.armyQueueChange,
                CmdConstant.InSoldierInfoChange,
                Role_TrainArmy.TagName,
                CmdConstant.UpdateCurrency,
                Role_TrainEnd.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.armyQueueChange:
                    TrainQueueChange(notification.Body);
                    break;
                case CmdConstant.InSoldierInfoChange:
                    ReadArmyData();
                    RefreshLeft(false);
                    RefreshHead(false);
                    break;
                case Role_TrainArmy.TagName:
                    ImmediatelyCompletedProcess(notification.Body);
                    break;
                case CmdConstant.UpdateCurrency:
                    ResNumChangeNotice();
                    break;
                case Role_TrainEnd.TagName: //取消训练
                    var response = notification.Body as Role_TrainEnd.response;
                    if (response.buildingIndex == m_buildingIndex)
                    {
                        CancelTrainProcess();
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
            if (view.data == null)
            {
                Debug.LogError("参数为空");
                return;
            }
            m_buildingIndex = (Int64)view.data;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_trainProxy = AppFacade.GetInstance().RetrieveProxy(TrainProxy.ProxyNAME) as TrainProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_buildingInfo = m_buildingProxy.GetBuildingInfoByindex(m_buildingIndex);
            if (m_buildingInfo == null)
            {
                Debug.LogErrorFormat("not find buildingIndex:{0}", m_buildingIndex);
                return;
            }
            m_buildingType = (int)m_buildingInfo.type;

            if (m_buildingType == (int)EnumCityBuildingType.Barracks)
            {
                m_titleText = LanguageUtils.getText(192014);
            }
            else if (m_buildingType == (int)EnumCityBuildingType.Stable)
            {
                m_titleText = LanguageUtils.getText(192016);
            }
            else if (m_buildingType == (int)EnumCityBuildingType.ArcheryRange)
            {
                m_titleText = LanguageUtils.getText(192015);
            }
            else
            {
                m_titleText = LanguageUtils.getText(192017);
            }

            TrainProxy.OpenTrainViewType = m_buildingType;

            view.m_lbl_lock_LanguageText.text = "";
            view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = m_titleText;

            view.m_pl_spine_SkeletonGraphic.gameObject.SetActive(false);
            view.m_UI_Model_TrainResCost.m_ipt_count_GameInput.textComponent.gameObject.SetActive(false);

            //判断下是否正在训练中
            QueueInfo queueInfo = m_trainProxy.GetTrainInfo(m_buildingIndex);
            if (queueInfo != null)
            {
                m_isTraining = true;

                m_startTime = queueInfo.beginTime;
                m_finishTime = queueInfo.finishTime;
                m_trainLevel = queueInfo.newArmyLevel;
                m_trainCostTime = m_finishTime - m_startTime;
                m_trainNum = queueInfo.armyNum;
            }

            DataProcess();

            m_isInitData = true;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.setCloseHandle(Close);

            //共有
            view.m_btn_detail_GameButton.onClick.AddListener(OnDetail);
            view.m_UI_Model_Window_Type2.AddBackEvent(OnDetailReturn);

            //待训练
            view.m_UI_Model_TrainResCost.Init(NumChangeCallback);
            view.m_UI_btn_complete.m_btn_languageButton_GameButton.onClick.AddListener(OnComplete);
            view.m_UI_btn_train.m_btn_languageButton_GameButton.onClick.AddListener(OnTrain);
            view.m_btn_up_GameButton.onClick.AddListener(OpenAmryUpView);

            ClientUtils.LoadSprite(view.m_UI_btn_train.m_img_icon2_PolygonImage, RS.CountDownIcon);

            //训练中
            view.m_UI_btn_complete_speedUp.m_btn_languageButton_GameButton.onClick.AddListener(OnComplete);
            view.m_UI_btn_speedUp.m_btn_languageButton_GameButton.onClick.AddListener(OnSpeedUp);
            view.m_UI_Model_ArmyTrainHead.AddClickListener(OnCancelTrain);
            view.m_UI_Model_ArmyTrainHead.AddCancelListener(OnCancelTrain);

            if (!m_isInitData)
            {
                return;
            }
            InitRefresh();
        }

        protected override void BindUIData()
        {

        }

        public override void OnRemove()
        {
            m_isDispose = true;
            TrainProxy.OpenTrainViewType = 0;
        }

        #endregion

        private void InitRefresh()
        {
            //刷新兵种类型icon
            ClientUtils.LoadSprite(view.m_img_army_type_icon_PolygonImage, RS.ArmyTypeIcon[m_armyType - 1]);

            RefreshLeft();

            RefreshHead(true);

            if (m_isTraining)
            {
                view.m_pl_time_cost.gameObject.SetActive(true);
                view.m_pl_res_cost.gameObject.SetActive(false);

                RefreshTrainingContent();

                //特殊情况
                QueueInfo queueInfo = m_trainProxy.GetTrainInfo(m_buildingIndex);
                if (queueInfo != null && queueInfo.finishTime < ServerTimeModule.Instance.GetServerTime())
                {
                    Timer.Register(0.02f, ()=> {
                        Close();
                    });
                }
            }
            else
            {
                view.m_pl_time_cost.gameObject.SetActive(false);
                view.m_pl_res_cost.gameObject.SetActive(true);

                view.m_UI_Model_TrainResCost.UpdateMinMax(1, (int)m_trainCapacity);
                RefreshInputNum((int)m_trainCapacity);

                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_train.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_complete.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
            RefreshTrainCapacityGain();
        }

        //部队容量增益
        private void RefreshTrainCapacityGain()
        {
            if (!m_isTraining && m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount > 0)
            {
                view.m_pl_num.gameObject.SetActive(true);
                view.m_lbl_num_up_LanguageText.text = LanguageUtils.getTextFormat(192036, ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacity));
                view.m_lbl_last_num_LanguageText.text = LanguageUtils.getTextFormat(192037, ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount));
            }
            else
            {
                view.m_pl_num.gameObject.SetActive(false);
            }
        }

        private void RefreshHead(bool isAddListener = false)
        {
            for (int i = 0; i < 5; i++)
            {
                m_headViewList[i].SetIndex(i);
                m_headViewList[i].SetHead(SoldierProxy.GetArmyHeadIcon(m_armyList[i].ID));
                m_headViewList[i].SetNum(ClientUtils.FormatComma(m_armyDataList[i]));
                m_headViewList[i].SetUp(IsShowUpStatus(i));
                m_headViewList[i].SetGray(!m_unlockDic[i]);
                m_headViewList[i].SetSelectStatus(i == m_selectIndex);
                if (isAddListener)
                {
                    m_headViewList[i].AddClickListener(ClickHead);
                }
            }
        }

        private void SwitchRefresh()
        {
            if (m_isTraining)
            {
                view.m_pl_time_cost.gameObject.SetActive(true);
                view.m_pl_res_cost.gameObject.SetActive(false);

                RefreshTrainingContent();
            }
            else
            {
                view.m_pl_time_cost.gameObject.SetActive(false);
                view.m_pl_res_cost.gameObject.SetActive(true);

                RefreshWaitTrainContent((int)m_trainCapacity);
            }
        }

        private void RefreshWaitTrainContent(int trainCount)
        {
            view.m_UI_Model_TrainResCost.UpdateMinMax(1, (int)m_trainCapacity);
            RefreshInputNum(trainCount);

            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_train.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_complete.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        //资源变更通知
        private void ResNumChangeNotice()
        {
            //刷新资源消耗显示
            if (!m_isTraining)
            {
                RefreshTrainRes();

                if (m_itemTrainUpView != null)
                {
                    RefreshUpRes();
                }
            }
        }

        #region 待训练

        private void RefreshInputNum(int trainCount)
        {
            view.m_UI_Model_TrainResCost.SetInputVal(trainCount);
            view.m_UI_Model_TrainResCost.SetSilderVal((float)m_currTrainCount / m_trainCapacity);

            RefreshTrainTime();
            RefreshTrainRes();
        }

        private void RefreshTrainTime()
        {
            //更新消耗时间
            int costTime = GetTrainTime(m_selectIndex, (int)m_currTrainCount);
            if (GuideManager.Instance.IsGuideSoldierTrain)
            {
                int num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).trainingFirstTime;
                view.m_UI_btn_train.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(num);
            }
            else
            {
                view.m_UI_btn_train.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(costTime);
            }
        }

        private void RefreshTrainRes()
        {
            //更新资源消耗
            UpdateResCostData();
        }

        private void NumChangeCallback(int num, int index)
        {
            m_currTrainCount = num;
            RefreshTrainTime();
            RefreshTrainRes();
        }

        private void RefreshLeft(bool isRefreshSpine = true)
        {
            if (m_isTraining)
            {
                view.m_btn_up_GameButton.gameObject.SetActive(false);
            }
            else
            {
                view.m_btn_up_GameButton.gameObject.SetActive(IsShowUpStatus(m_selectIndex));
            }

            view.m_pl_Body.sprite = null;

            string bodyPath = m_armyList[m_selectIndex].armsShow + "_Body.png";
            Debug.Log(bodyPath);
            view.m_pl_Body.gameObject.SetActive(false);
            view.m_pl_spine_SkeletonGraphic.gameObject.SetActive(false);

            CoreUtils.assetService.LoadAssetAsync<Sprite>(bodyPath, (sprite) => 
            { 
                Sprite s = (Sprite)sprite.asset();
                if (sprite == null || s == null)
                {
                    if (isRefreshSpine)
                    {
                        RefreshSpine();
                        RefreshDesc();
                    }
                    return;
                }
                view.m_pl_Body.sprite = s;
                view.m_pl_Body.SetNativeSize();
                view.m_pl_Body.transform.localScale = new Vector3(1.2f, 1.2f);
                view.m_pl_Body.gameObject.SetActive(true);
            });
        }

        //是否显示升级状态
        private bool IsShowUpStatus(int index)
        {
            if (m_isTraining)
            {
                return false;
            }
            if (index < m_unlockMaxIndex)
            {
                if (m_armyDataList[index] > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void RefreshDesc()
        {
            view.m_lbl_desc_name_LanguageText.text = LanguageUtils.getText(m_armyList[m_selectIndex].l_armsID);
            view.m_lbl_desc_content_LanguageText.text = LanguageUtils.getText(m_armyList[m_selectIndex].l_desID);
        }

        private void RefreshSpine()
        {
            ClientUtils.LoadSpine(view.m_pl_spine_SkeletonGraphic, m_armyList[m_selectIndex].armsShow, LoadSpineCallback);
        }

        private void LoadSpineCallback()
        {
            if (m_isDispose)
            {
                return;
            }
            view.m_pl_spine_SkeletonGraphic.gameObject.SetActive(true);
        }

        private void ClickHead(int index)
        {
            m_headViewList[m_selectIndex].SetSelectStatus(false);
            m_headViewList[index].SetSelectStatus(true);

            m_selectIndex = index;
            m_currTrainCount = m_trainCapacity;

            RefreshLeft();

            if (!m_isTraining)
            {
                RefreshInputNum((int)m_trainCapacity);

                if (m_selectIndex > m_unlockMaxIndex)
                {
                    view.m_UI_Model_TrainResCost.gameObject.SetActive(false);
                    view.m_lbl_lock_LanguageText.text = LanguageUtils.getTextFormat(192012, LanguageUtils.getText(m_armyList[m_selectIndex].l_armsID));
                    view.m_UI_btn_train.gameObject.SetActive(false);
                    view.m_UI_btn_complete.gameObject.SetActive(false);
                }
                else
                {
                    view.m_UI_Model_TrainResCost.gameObject.SetActive(true);
                    view.m_lbl_lock_LanguageText.text = "";
                    view.m_UI_btn_train.gameObject.SetActive(true);
                    view.m_UI_btn_complete.gameObject.SetActive(true);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_train.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_complete.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        //获取训练容量
        private int GetTrainCapacity(int buildType)
        {
            int level = (int)m_buildingInfo.level;
            int gainCount = 0;
            if (m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount > 0)
            {
                gainCount = (int)m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacity;
            }

            int count = 0;
            if (buildType == (int)EnumCityBuildingType.Barracks)
            {
                BuildingBarracksDefine define = CoreUtils.dataService.QueryRecord<BuildingBarracksDefine>(level);
                if (define != null)
                {
                    count = (int)m_playerAttributeProxy.GetCityAttribute(attrType.infantryTrainNumber).value;
                }
            }
            else if (buildType == (int)EnumCityBuildingType.Stable)
            {
                BuildingStableDefine define = CoreUtils.dataService.QueryRecord<BuildingStableDefine>(level);
                if (define != null)
                {
                    count =(int)m_playerAttributeProxy.GetCityAttribute(attrType.cavalryTrainNumber).value;
                }
            }
            else if (buildType == (int)EnumCityBuildingType.ArcheryRange)
            {
                BuildingArcheryrangeDefine define = CoreUtils.dataService.QueryRecord<BuildingArcheryrangeDefine>(level);
                if (define != null)
                {
                    count =(int)m_playerAttributeProxy.GetCityAttribute(attrType.bowmenTrainNumber).value;
                }
            }
            else if (buildType == (int)EnumCityBuildingType.SiegeWorkshop)
            {
                BuildingSiegeWorkshopDefine define = CoreUtils.dataService.QueryRecord<BuildingSiegeWorkshopDefine>(level);
                if (define != null)
                {
                    count = (int)m_playerAttributeProxy.GetCityAttribute(attrType.siegeCarTrainNumber).value;
                }
            }
            count = count + gainCount;
            return count;
        }

        //获取训练时间
        private int GetTrainTime(int index, int count)
        {
            int time = (int)(m_armyList[index].endTime * count * GetTranSpeedMulti());
            return time;
        }

        //刷新训练消耗
        private void UpdateResCostData()
        {
            UI_Model_TrainResCost_SubView nodeView = view.m_UI_Model_TrainResCost;
            int trainNum = (int)m_currTrainCount;

            int foodNum = 0;
            int woodNum = 0;
            int stoneNum = 0;
            int goldNum = 0;
            if (m_armyList[m_selectIndex].needFood > 0)
            {
                foodNum = m_armyList[m_selectIndex].needFood * trainNum;
            }
            if (m_armyList[m_selectIndex].needWood > 0)
            {
                woodNum = m_armyList[m_selectIndex].needWood * trainNum;
            }
            if (m_armyList[m_selectIndex].needStone > 0)
            {
                stoneNum = m_armyList[m_selectIndex].needStone * trainNum;
            }
            if (m_armyList[m_selectIndex].needGlod > 0)
            {
                goldNum = m_armyList[m_selectIndex].needGlod * trainNum;
            }
            view.m_UI_Model_TrainResCost.m_UI_Model_ResCost.UpdateResCost(foodNum, woodNum, stoneNum, goldNum);
            float time = m_armyList[m_selectIndex].endTime * trainNum * GetTranSpeedMulti();
            m_needDenar = m_currencyProxy.CaculateImmediatelyFinishPrice(time, foodNum, woodNum, stoneNum, goldNum);
            view.m_UI_btn_complete.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
            if (m_originDenarTextColor != Color.red)
            {
                m_originDenarTextColor = view.m_UI_btn_complete.m_lbl_line2_LanguageText.color;
            }
            Color changedColor = m_currencyProxy.Gem < m_needDenar ? Color.red : m_originDenarTextColor;
            view.m_UI_btn_complete.m_lbl_line2_LanguageText.color = changedColor;
            view.m_UI_btn_complete_speedUp.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
        }

        //刷新晋升消耗
        private void UpdateUpResCostData()
        {
            UI_Model_TrainResCost_SubView nodeView = m_itemTrainUpView.m_UI_Model_UpResCost;
            int trainNum = (int)m_currUpCount;

            int foodNum = 0;
            int woodNum = 0;
            int stoneNum = 0;
            int goldNum = 0;
            if (m_armyList[m_unlockMaxIndex].needFood > 0)
            {
                foodNum = m_armyList[m_unlockMaxIndex].needFood * trainNum - m_armyList[m_selectIndex].needFood * trainNum;
            }
            if (m_armyList[m_unlockMaxIndex].needWood > 0)
            {
                woodNum = m_armyList[m_unlockMaxIndex].needWood * trainNum - m_armyList[m_selectIndex].needWood * trainNum;
            }
            if (m_armyList[m_unlockMaxIndex].needStone > 0)
            {
                stoneNum = m_armyList[m_unlockMaxIndex].needStone * trainNum - m_armyList[m_selectIndex].needStone * trainNum;
            }
            if (m_armyList[m_unlockMaxIndex].needGlod > 0)
            {
                goldNum = m_armyList[m_unlockMaxIndex].needGlod * trainNum - m_armyList[m_selectIndex].needGlod * trainNum;
            }
            nodeView.m_UI_Model_ResCost.UpdateResCost(foodNum, woodNum, stoneNum, goldNum);

            float time = (m_armyList[m_unlockMaxIndex].endTime * trainNum - m_armyList[m_selectIndex].endTime * trainNum) * GetTranSpeedMulti();
            m_needDenar = m_currencyProxy.CaculateImmediatelyFinishPrice(time, foodNum, woodNum, stoneNum, goldNum);
            m_itemTrainUpView.m_UI_btn_Upcomplete.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
            //view.m_UI_btn_complete_speedUp.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
        }

        //立即完成
        private void OnComplete()
        {
            if (!m_isInitData)
            {
                return;
            }
            //Debug.Log("立即完成");
            if (m_currencyProxy.ShortOfDenar(m_needDenar))
            {
                return;
            }
            UIHelper.DenarCostRemain(m_needDenar, () =>
            {
                PlayTrainSound();

                Role_TrainArmy.request req = new Role_TrainArmy.request();
                req.immediately = true;
                if (m_isTraining)
                {
                    QueueInfo info = m_trainProxy.GetTrainInfo(m_buildingIndex);
                    req.armyQueueIndex = info.queueIndex;

                    OnPowerUpShow((int)info.newArmyLevel, (int)info.armyType);
                }
                else
                {
                    req.buildingIndex = m_buildingIndex;
                    req.type = m_armyList[m_selectIndex].armsType;
                    req.trainNum = m_currTrainCount;
                    req.level = m_armyList[m_selectIndex].armsLv;
                    OnPowerUpShow(m_armyList[m_selectIndex].armsLv, m_armyList[m_selectIndex].armsType);
                }
                AppFacade.GetInstance().SendSproto(req);
            });
  
        }

        //战力飘飞
        private void OnPowerUpShow(int level,int type)
        {
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            int tmpID = m_soldierProxy.GetTemplateId(type, level);
            ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tmpID);
            if (define != null)
            {
                CoreUtils.assetService.LoadAssetAsync<Sprite>(define.icon, (asset) =>
                {
                    UnityEngine.Object go = asset.asset() as UnityEngine.Object;
                    Sprite sprite = go as Sprite;
                    GameObject tmpObj = new GameObject();
                    tmpObj.AddComponent<Image>().sprite = sprite;
                    mt.FlyPowerUpEffect(tmpObj, view.m_UI_btn_complete.m_btn_languageButton_GameButton.GetComponent<RectTransform>(), Vector3.one);
                    GameObject.DestroyImmediate(tmpObj);
                }, view.gameObject);
            }
        }



        //训练
        private void OnTrain()
        {
            if (!m_isInitData)
            {
                return;
            }
            Debug.Log("训练");
            if (m_buildingProxy.IsUpgrading(m_buildingIndex))
            {
                Debug.LogError(LanguageUtils.getText(192028));
                return;
            }
            if (m_trainProxy.GetTrainInfo(m_buildingIndex) != null)
            {
                Debug.LogError(string.Format(LanguageUtils.getText(192011), TrainProxy.GetBuildingName(m_buildingType)));
                return;
            }
            if (m_selectIndex > m_unlockMaxIndex)
            {
                Debug.LogError(string.Format(LanguageUtils.getText(192012), LanguageUtils.getText(m_armyList[m_selectIndex].l_armsID)));
                return;
            }

            //资源是否足够
            bool isAllEnough = true;
            Dictionary<int, Int64> notEnoughResDic = new Dictionary<int, long>();
            Int64 num = 0;
            foreach (var data in view.m_UI_Model_TrainResCost.m_UI_Model_ResCost.ResCostModel.ResCostData)
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
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                Dictionary<int, long> cost = view.m_UI_Model_TrainResCost.m_UI_Model_ResCost.ResCostModel.ResCostData;
                long food = cost.ContainsKey((int)EnumCurrencyType.food) ? cost[(int)EnumCurrencyType.food] : 0;
                long wood = cost.ContainsKey((int)EnumCurrencyType.wood) ? cost[(int)EnumCurrencyType.wood] : 0;
                long stone = cost.ContainsKey((int)EnumCurrencyType.stone) ? cost[(int)EnumCurrencyType.stone] : 0;
                long gold = cost.ContainsKey((int)EnumCurrencyType.gold) ? cost[(int)EnumCurrencyType.gold] : 0;
                currencyProxy.LackOfResources(food, wood, stone, gold);
                return;
            }

            PlayTrainSound();

            //发包
            var sp = new Role_TrainArmy.request();
            sp.buildingIndex = m_buildingIndex;
            sp.type = m_armyList[m_selectIndex].armsType;
            sp.trainNum = m_currTrainCount;
            sp.level = m_armyList[m_selectIndex].armsLv;
            Debug.Log(string.Format("type:{0}", sp.type));
            Debug.Log(string.Format("level:{0}", sp.level));
            sp.isUpdate = 0; //普通训练
            if (GuideManager.Instance.IsGuideSoldierTrain)
            {
                sp.guide = true;
            }
            AppFacade.GetInstance().SendSproto(sp);

            Close();
        }

        //播放音效
        private void PlayTrainSound()
        {
            if (m_buildingType == (int)EnumCityBuildingType.Barracks) //兵营
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiTrainingWarriors, null);
            }
            else if (m_buildingType == (int)EnumCityBuildingType.Stable)//马槽
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiTrainingKnights, null);
            }
            else if (m_buildingType == (int)EnumCityBuildingType.ArcheryRange)//靶场
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiTrainingArcher, null);
            }
            else
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiTrainingSieges, null);
            }
        }

        //加速
        private void OnSpeedUp()
        {
            Debug.Log("加速------");
            QueueInfo queueInfo = m_trainProxy.GetTrainInfo(m_buildingIndex);
            SpeedUpData data = new SpeedUpData
            {
                type = EnumSpeedUpType.train,
                icon = view.m_UI_Model_ArmyTrainHead.m_img_army_icon_PolygonImage.sprite,
                queue = queueInfo,
                cancelCallback = () => { OnCancelTrain((int)queueInfo.queueIndex); },
            };
            AppFacade.GetInstance().SendNotification(CmdConstant.SpeedUp, data);
        }

        //详情界面
        private void OnDetail()
        {
            if (m_detailLoadStatus == 1)
            {
                return;
            }
            if (m_detailLoadStatus == 2)//已加载
            {
                ShowAttrDetail(false);
                return;
            }
            m_detailLoadStatus = 1;
            List<string> prefabNames = new List<string> { "UI_Item_TrainSoldierDetail" };
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadDetailFinish);
        }

        private void LoadDetailFinish(Dictionary<string, IAsset> dic)
        {
            view.m_pl_mes_Image.gameObject.SetActive(true);
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            GameObject obj = GameObject.Instantiate(m_assetDic["UI_Item_TrainSoldierDetail"], view.m_pl_detail_ArabLayoutCompment.transform);
            m_detailObj = obj;
            m_detailLoadStatus = 2;
            ShowAttrDetail(true);
        }

        //属性详情
        private void ShowAttrDetail(bool isFirst)
        {
            m_detailObj.gameObject.SetActive(true);
            m_detailObj.GetComponent<Animator>().Play("Show");
            view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_pl_detailUp.gameObject.SetActive(false);
            ArmsDefine define = m_armyList[m_selectIndex];

            ItemTrainSoldierDetailView nodeView = MonoHelper.GetOrAddHotFixViewComponent<ItemTrainSoldierDetailView>(m_detailObj);

            List<UI_Model_TrainSoldierSkill_SubView> viewList = new List<UI_Model_TrainSoldierSkill_SubView>();
            viewList.Add(nodeView.m_UI_Model_TrainSoldierSkill_1);
            viewList.Add(nodeView.m_UI_Model_TrainSoldierSkill_2);
            viewList.Add(nodeView.m_UI_Model_TrainSoldierSkill_3);
            if (isFirst)
            {
                ClientUtils.LoadSprite(nodeView.m_img_army_type_icon_PolygonImage, RS.ArmyTypeIcon[m_armyType - 1]);
                for (int i = 0; i < viewList.Count; i++)
                {
                    viewList[i].index = i;
                    viewList[i].AddBtnListener(OnDetailSkillTip);
                }
            }

            nodeView.m_lbl_desc_name_LanguageText.text = LanguageUtils.getText(define.l_armsID);
            nodeView.m_lbl_desc_content_LanguageText.text = LanguageUtils.getText(define.l_desID);

            nodeView.m_UI_Model_TrainSoldierAttr_1.SetTipId(1000);
            nodeView.m_UI_Model_TrainSoldierAttr_1.SetNum(TrainProxy.GetAgeStr(define.age));
            nodeView.m_UI_Model_TrainSoldierAttr_1.AddBtnListener(OnDetailAttrTip);

            nodeView.m_UI_Model_TrainSoldierAttr_2.SetTipId(1001);
            nodeView.m_UI_Model_TrainSoldierAttr_2.SetNum(GetAttrFormat(define.attack, 1));
            nodeView.m_UI_Model_TrainSoldierAttr_2.AddBtnListener(OnDetailAttrTip);

            nodeView.m_UI_Model_TrainSoldierAttr_3.SetTipId(1002);
            nodeView.m_UI_Model_TrainSoldierAttr_3.SetNum(GetAttrFormat(define.defense, 2));
            nodeView.m_UI_Model_TrainSoldierAttr_3.AddBtnListener(OnDetailAttrTip);

            nodeView.m_UI_Model_TrainSoldierAttr_4.SetTipId(1003);
            nodeView.m_UI_Model_TrainSoldierAttr_4.SetNum(GetAttrFormat(define.hpMax, 3));
            nodeView.m_UI_Model_TrainSoldierAttr_4.AddBtnListener(OnDetailAttrTip);

            nodeView.m_UI_Model_TrainSoldierAttr_5.SetTipId(1004);
            nodeView.m_UI_Model_TrainSoldierAttr_5.SetNum(define.speed.ToString());
            nodeView.m_UI_Model_TrainSoldierAttr_5.AddBtnListener(OnDetailAttrTip);

            nodeView.m_UI_Model_TrainSoldierAttr_6.SetTipId(1005);
            nodeView.m_UI_Model_TrainSoldierAttr_6.SetNum(define.capacity.ToString());
            nodeView.m_UI_Model_TrainSoldierAttr_6.AddBtnListener(OnDetailAttrTip);

            nodeView.m_UI_Model_TrainSoldierAttr_7.SetTipId(1006);
            nodeView.m_UI_Model_TrainSoldierAttr_7.SetNum(define.militaryCapability.ToString());
            nodeView.m_UI_Model_TrainSoldierAttr_7.AddBtnListener(OnDetailAttrTip);

            //士兵技能信息
            List<RectTransform> rectList = new List<RectTransform>();
            if (define.armsSkill == null)
            {
                return;
            }

            ArmsSkillDefine skillDefine = null;

            for (int i = 0; i < viewList.Count; i++)
            {
                if (i < define.armsSkill.Count)
                {
                    skillDefine = CoreUtils.dataService.QueryRecord<ArmsSkillDefine>(define.armsSkill[i]);
                    if (skillDefine != null)
                    {
                        viewList[i].SetSkillIcon(skillDefine.skillIcon);
                        viewList[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    viewList[i].gameObject.SetActive(false);
                }
            }
        }

        //技能tip
        private void OnDetailSkillTip(int index, Transform trans)
        {
            //todo
            //Debug.Log("tipId:" + (int)index);
            ArmsDefine define = m_armyList[m_selectIndex];
            if (index >= define.armsSkill.Count)
            {
                return;
            }
            ArmsSkillDefine skillDefine = CoreUtils.dataService.QueryRecord<ArmsSkillDefine>(define.armsSkill[index]);
            if (skillDefine == null)
            {
                Debug.LogWarningFormat("ArmsSkillDefine not find id:{0}", define.armsSkill[index]);
                return;
            }
            HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(skillDefine.tipsID);
            if (tipDefine == null)
            {
                return;
            }
            HelpTip.CreateTip(LanguageUtils.getTextFormat(tipDefine.l_typeID, LanguageUtils.getText(tipDefine.l_data1), LanguageUtils.getText(tipDefine.l_data2)),
                trans).SetStyle(HelpTipData.Style.arrowDown).SetOffset(30).SetWidth(500).Show();
        }

        //属性tip
        private void OnDetailAttrTip(object obj, Transform trans)
        {
            //Debug.Log("tipId:" + (int)obj);
            HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>((int)obj);
            if (tipDefine == null)
            {
                return;
            }
            HelpTip.CreateTip(LanguageUtils.getText(tipDefine.l_data1), trans).SetStyle(HelpTipData.Style.arrowDown).SetOffset(23).SetWidth(500).Show();
        }

        //属性格式化文本
        private string GetAttrFormat(int num, int type)
        {
            int num1 = 0;
            if (type == 1) //攻击
            {
                if (m_buildingType == (int)EnumCityBuildingType.Barracks)//步兵
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.infantryAttackMulti).value);
                }
                else if (m_buildingType == (int)EnumCityBuildingType.Stable)//骑兵
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.cavalryAttackMulti).value);
                }
                else if (m_buildingType == (int)EnumCityBuildingType.ArcheryRange)//弓手
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.bowmenAttackMulti).value);
                }
                else
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.siegeCarAttackMulti).value);
                }
            }
            else if (type == 2) //防御
            {
                if (m_buildingType == (int)EnumCityBuildingType.Barracks)//步兵
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.infantryDefenseMulti).value);
                }
                else if (m_buildingType == (int)EnumCityBuildingType.Stable)//骑兵
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.cavalryDefenseMulti).value);
                }
                else if (m_buildingType == (int)EnumCityBuildingType.ArcheryRange)//弓手
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.bowmenDefenseMulti).value);
                }
                else
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.siegeCarDefenseMulti).value);
                }
            }
            else if (type == 3) //生命
            {
                if (m_buildingType == (int)EnumCityBuildingType.Barracks)//步兵
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.infantryHpMaxMulti).value);
                }
                else if (m_buildingType == (int)EnumCityBuildingType.Stable)//骑兵
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.infantryHpMaxMulti).value);
                }
                else if (m_buildingType == (int)EnumCityBuildingType.ArcheryRange)//弓手
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.infantryHpMaxMulti).value);
                }
                else
                {
                    num1 = (int)(num * (float)m_playerAttributeProxy.GetCityAttribute(attrType.infantryHpMaxMulti).value);
                }
            }

            if (num1 > 0)
            {
                return LanguageUtils.getTextFormat(180356, num, num1);
            }
            else
            {
                return num.ToString();
            }
        }

        private void OnDetailReturn()
        {
            if (m_detailObj != null && m_detailObj.activeSelf) //正处于详情界面
            {
                m_detailObj.SetActive(false);
                view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_pl_train_Animator.Play("Show");
            }

            if (m_upItemObj != null && m_upItemObj.activeSelf) //正处于晋升界面
            {
                m_upItemObj.SetActive(false);
                view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(true);
            }
            view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = m_titleText;
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(false);
            view.m_pl_detailUp.gameObject.SetActive(true);
        }

        #endregion

        #region 训练中

        private void RefreshTrainingContent()
        {
            QueueInfo queueInfo = m_trainProxy.GetTrainInfo(m_buildingIndex);
            CityHudCountDownManager.Instance.AddUiQueue(null, view.m_lbl_countdown_LanguageText, view.m_pb_time_GameSlider, queueInfo, OnImmediatelyComplete, EndCallback);
            OnImmediatelyComplete();
            view.m_lbl_time_cost_desc_LanguageText.text = ClientUtils.FormatComma(m_trainNum);
            view.m_UI_Model_ArmyTrainHead.SetHead(SoldierProxy.GetArmyHeadIcon(m_armyList[m_trainingIndex].ID));

            view.m_UI_btn_speedUp.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_btn_complete_speedUp.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        private void EndCallback(Int64 buildingIndex)
        {
            Timer.Register(0.2f, Close);
        }


        //立即完成
        private void OnImmediatelyComplete()
        {
            if (view == null)
            {
                return;
            }
            QueueInfo queueInfo = m_trainProxy.GetTrainInfo(m_buildingIndex);
            if (queueInfo != null)
            {
                m_needDenar = m_currencyProxy.CaculateImmediatelyFinishPrice(queueInfo.finishTime - ServerTimeModule.Instance.GetServerTime());
                if (view.gameObject != null)
                {
                    view.m_UI_btn_complete_speedUp.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
                }
            }
        }

        //取消训练
        private void OnCancelTrain(int index)
        {
            string name = LanguageUtils.getText(m_armyList[m_trainingIndex].l_armsID);
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            string str = LanguageUtils.getTextFormat(192031, name, config.trainingTerminate / 10);
            Alert.CreateAlert(str, LanguageUtils.getText(192030)).SetLeftButton().SetRightButton(() => {
                if (m_isDispose)
                {
                    return;
                }
                //发包
                var sp = new Role_TrainEnd.request();
                sp.buildingIndex = m_buildingIndex;
                sp.type = m_armyType;
                AppFacade.GetInstance().SendSproto(sp);
            }).Show();
        }

        private void CancelTrainProcess()
        {
            m_isTraining = false;
            ReadArmyData();
            InitWaitTrainData();
            RefreshHead();
            SwitchRefresh();
            RefreshLeft();
            RefreshTrainCapacityGain();
        }

        #endregion

        #region 晋升中

        //打开晋升界面
        private void OpenAmryUpView()
        {
            if (m_upItemLoadStatus == 1)
            {
                return;
            }
            if (m_upItemLoadStatus == 2)//已加载
            {
                ShowUpView(false);
                return;
            }
            m_upItemLoadStatus = 1;
            List<string> prefabNames = new List<string> { "UI_Item_TrainUp" };
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadUpItemFinish);
        }

        private void LoadUpItemFinish(Dictionary<string, IAsset> dic)
        {
            view.m_pl_mes_Image.gameObject.SetActive(true);
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            GameObject obj = GameObject.Instantiate(m_assetDic["UI_Item_TrainUp"], view.m_pl_upgrade_ArabLayoutCompment.transform);
            m_upItemObj = obj;
            m_upItemLoadStatus = 2;

            ShowUpView(true);
        }

        private void ShowUpView(bool isFirst)
        {
            view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = LanguageUtils.getText(192029);
            m_upItemObj.gameObject.SetActive(true);
            view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_detailUp.gameObject.SetActive(false);
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);

            ItemTrainUpView nodeView = m_itemTrainUpView;
            if (nodeView == null)
            {
                nodeView = MonoHelper.GetOrAddHotFixViewComponent<ItemTrainUpView>(m_upItemObj);

                m_itemTrainUpView = nodeView;
                nodeView.m_UI_Model_UpResCost.Init(UpTrainNumChangeCallback);
                nodeView.m_UI_btn_Uptrain.m_btn_languageButton_GameButton.onClick.AddListener(OnArmyUp);
                nodeView.m_UI_btn_Upcomplete.m_btn_languageButton_GameButton.onClick.AddListener(OnArmyUpComplete);
                nodeView.m_UI_Model_UpResCost.m_ipt_count_GameInput.textComponent.gameObject.SetActive(false);
            }

            Int64 trainUpLimit = m_trainCapacity;
            if (m_armyDataList[m_selectIndex] < m_trainCapacity)
            {
                trainUpLimit = m_armyDataList[m_selectIndex];
            }

            if (m_currUpCount == 0)
            {
                m_currUpCount = (int)trainUpLimit;
            }
            else if (m_currUpCount > trainUpLimit)
            {
                m_currUpCount = (int)trainUpLimit;
            }

            //旧
            ClientUtils.LoadSprite(nodeView.m_img_headBefore_PolygonImage, SoldierProxy.GetArmyHeadIcon(m_armyList[m_selectIndex].ID));
            nodeView.m_lbl_numBefore_LanguageText.text = LanguageUtils.getText(m_armyList[m_selectIndex].l_armsID);

            //新
            ClientUtils.LoadSprite(nodeView.m_img_headAfter_PolygonImage, SoldierProxy.GetArmyHeadIcon(m_armyList[m_unlockMaxIndex].ID));
            nodeView.m_lbl_numAfter_LanguageText.text = LanguageUtils.getText(m_armyList[m_unlockMaxIndex].l_armsID);

            nodeView.m_UI_Model_UpResCost.UpdateMinMax(1, (int)trainUpLimit);
            nodeView.m_UI_Model_UpResCost.SetInputVal((int)m_currUpCount);

            LayoutRebuilder.ForceRebuildLayoutImmediate(nodeView.m_UI_btn_Uptrain.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(nodeView.m_UI_btn_Upcomplete.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());

            if (m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount > 0)
            {
                nodeView.m_pl_num.gameObject.SetActive(true);
                nodeView.m_lbl_num_up_LanguageText.text = LanguageUtils.getTextFormat(192036, ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacity));
                nodeView.m_lbl_last_num_LanguageText.text = LanguageUtils.getTextFormat(192037, ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount));
            }
            else
            {
                nodeView.m_pl_num.gameObject.SetActive(false);
            }
            UpTrainNumChangeCallback((int)m_currUpCount, 0);
        }

        private void UpTrainNumChangeCallback(int num, int index)
        {
            m_currUpCount = num;
            RefreshUpTime();
            RefreshUpRes();
        }

        //刷新升级时间和资源
        private void RefreshUpTime()
        {
            //更新消耗时间
            int oldTime = GetTrainTime(m_selectIndex, (int)m_currUpCount);
            int newTime = GetTrainTime(m_unlockMaxIndex, (int)m_currUpCount);
            m_itemTrainUpView.m_UI_btn_Uptrain.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(newTime - oldTime);
        }

        private void RefreshUpRes()
        {
            //更新资源消耗
            UpdateUpResCostData();
        }

        //立即完成 晋升
        private void OnArmyUpComplete()
        {
            if (m_currencyProxy.ShortOfDenar(m_needDenar))
            {
                return;
            }
            UIHelper.DenarCostRemain(m_needDenar, () =>
            {
                Debug.Log("晋升 立即完成");
                PlayTrainSound();
                Role_TrainArmy.request req = new Role_TrainArmy.request();
                req.immediately = true;
                if (m_isTraining)
                {
                    QueueInfo info = m_trainProxy.GetTrainInfo(m_buildingIndex);
                    req.armyQueueIndex = info.queueIndex;
                    OnPowerUpShow((int)info.newArmyLevel, (int)info.armyType);

                }
                else
                {
                    req.buildingIndex = m_buildingIndex;
                    req.type = m_armyList[m_selectIndex].armsType;
                    req.trainNum = m_currUpCount;
                    req.level = m_armyList[m_selectIndex].armsLv;
                    OnPowerUpShow(m_armyList[m_selectIndex].armsLv, m_armyList[m_selectIndex].armsType);
                    //Debug.LogError("m_currUpCount:" + m_currUpCount);
                }
                //Debug.LogError("level:"+ req.level);
                req.isUpdate = 1; //晋升
                AppFacade.GetInstance().SendSproto(req);
            });
       
        }

        //晋升
        private void OnArmyUp()
        {
            Debug.Log("晋升");
            if (m_trainProxy.GetTrainInfo(m_buildingIndex) != null)
            {
                Debug.LogError(string.Format(LanguageUtils.getText(192011), TrainProxy.GetBuildingName(m_buildingType)));
                return;
            }

            //资源是否足够
            bool isAllEnough = true;
            Dictionary<int, Int64> notEnoughResDic = new Dictionary<int, long>();
            Int64 num = 0;
            foreach (var data in m_itemTrainUpView.m_UI_Model_UpResCost.m_UI_Model_ResCost.ResCostModel.ResCostData)
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
                //弹出资源不足界面
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                Dictionary<int, long> cost = m_itemTrainUpView.m_UI_Model_UpResCost.m_UI_Model_ResCost.ResCostModel.ResCostData;
                long food = cost.ContainsKey((int)EnumCurrencyType.food) ? cost[(int)EnumCurrencyType.food] : 0;
                long wood = cost.ContainsKey((int)EnumCurrencyType.wood) ? cost[(int)EnumCurrencyType.wood] : 0;
                long stone = cost.ContainsKey((int)EnumCurrencyType.stone) ? cost[(int)EnumCurrencyType.stone] : 0;
                long gold = cost.ContainsKey((int)EnumCurrencyType.gold) ? cost[(int)EnumCurrencyType.gold] : 0;
                currencyProxy.LackOfResources(food, wood, stone, gold);
                return;
            }

            PlayTrainSound();

            //发包
            var sp = new Role_TrainArmy.request();
            sp.buildingIndex = m_buildingIndex;
            sp.type = m_armyList[m_selectIndex].armsType;
            sp.trainNum = m_currUpCount;
            sp.level = m_armyList[m_selectIndex].armsLv;
            sp.isUpdate = 1; //晋升
            AppFacade.GetInstance().SendSproto(sp);

            Close();
        }

        #endregion

        #region 数据处理

        private void DataProcess()
        {
            //m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount = 1;
            //m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacity = 1000;

            ArmTypeKeyMap[(int)EnumCityBuildingType.Barracks] = (int)EnumSoldierType.Infantry;
            ArmTypeKeyMap[(int)EnumCityBuildingType.Stable] = (int)EnumSoldierType.Cavalry;
            ArmTypeKeyMap[(int)EnumCityBuildingType.ArcheryRange] = (int)EnumSoldierType.Bowmen;
            ArmTypeKeyMap[(int)EnumCityBuildingType.SiegeWorkshop] = (int)EnumSoldierType.SiegeEngines;
            if (!ArmTypeKeyMap.TryGetValue(m_buildingType, out m_armyType))
            {
                Debug.LogErrorFormat("建筑类型异常:{0}", m_buildingType);
                return;
            }

            //获取模版数据
            m_soldierProxy.SetSpecialArmy();
            for (int i = 0; i < 5; i++)
            {
                int id = m_soldierProxy.GetTemplateId(m_armyType, i + 1);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (define == null)
                {
                    Debug.LogErrorFormat("ArmsDefine not find:{0}", id);
                    return;
                }
                m_armyList.Add(define);
            }
            if (m_armyList.Count < 5)
            {
                Debug.LogError("兵种模版数据不能小于5个");
                return;
            }

            //兵种解锁记录
            for (int i = 0; i < 5; i++)
            {
                if (m_soldierProxy.IsUnlock(m_armyList[i]))
                {
                    m_unlockDic[i] = true;
                    m_unlockMaxIndex = i;
                }
                else
                {
                    m_unlockDic[i] = false;
                }
            }

            //获取当前已有兵种数据
            ReadArmyData();

            //view存列表 方便调用
            m_headViewList.Add(view.m_UI_Model_ArmyTrainHead_1);
            m_headViewList.Add(view.m_UI_Model_ArmyTrainHead_2);
            m_headViewList.Add(view.m_UI_Model_ArmyTrainHead_3);
            m_headViewList.Add(view.m_UI_Model_ArmyTrainHead_4);
            m_headViewList.Add(view.m_UI_Model_ArmyTrainHead_5);

            if (m_isTraining) //训练中
            {
                m_selectIndex = (int)m_trainLevel - 1;
                m_trainingIndex = m_selectIndex;
            }
            else //待训练
            {
                m_selectIndex = m_unlockMaxIndex;
                InitWaitTrainData();
            }
        }

        private void ReadArmyData()
        {
            if (m_armyDataList.Count < 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    m_armyDataList.Add(i);
                }
            }
            Dictionary<Int64, Int64> countDic = m_soldierProxy.GetInSoldierByType(m_armyType);
            int key = 0;
            for (int i = 0; i < 5; i++)
            {
                key = i + 1;
                if (countDic.ContainsKey(key))
                {
                    m_armyDataList[i] = countDic[key];
                }
                else
                {
                    m_armyDataList[i] = 0;
                }
            }
        }

        private void InitWaitTrainData()
        {
            m_trainCapacity = GetTrainCapacity(m_buildingType);
            m_currTrainCount = m_trainCapacity;
        }

        #endregion 

        //训练时间加成
        private float GetTranSpeedMulti()
        {
            return 1 - m_playerAttributeProxy.GetCityAttribute(attrType.trainSpeedMulti).value;
        }

        //训练队列信息变更
        private void TrainQueueChange(object body)
        {
            RoleInfo info = body as RoleInfo;
            if (info != null)
            {
                if (info.armyQueue != null)
                {
                    foreach (var data in info.armyQueue)
                    {
                        if (data.Value.finishTime > 0)
                        {
                            if (m_isTraining && data.Value.buildingIndex == m_buildingIndex)
                            {
                                CityHudCountDownManager.Instance.AddUiQueue(null, view.m_lbl_countdown_LanguageText, view.m_pb_time_GameSlider, data.Value, OnImmediatelyComplete, EndCallback);
                            }
                        }
                    }
                }
            }
        }

        //立即完成回包处理
        private void ImmediatelyCompletedProcess(object body)
        {
            //Debug.Log("ImmediatelyCompletedProcess");
            var response = body as Role_TrainArmy.response;
            if (response == null)
            {
                return;
            }
            if (response.soldiers == null)
            {
                return;
            }
            if (response.soldiers.Count < 1)
            {
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyTrainEnd, m_buildingIndex);
            foreach (var data in response.soldiers)
            {
                //Debug.LogFormat("type:{0} level:{1}", (int)data.Value.type, (int)data.Value.level);
                int tempId = m_soldierProxy.GetTemplateId((int)data.Value.type, (int)data.Value.level);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                if (define != null)
                {
                    string str = LanguageUtils.getTextFormat(192007, LanguageUtils.getText(define.l_armsID), data.Value.num);
                    Tip.CreateTip(str).Show();
                }
            }
            //如果正在晋升界面 则先返回训练界面
            if (m_upItemObj != null && m_upItemObj.activeSelf)
            {
                OnDetailReturn();
            }

            if (m_isTraining)//正在训练中
            {
                m_isTraining = false;
                InitWaitTrainData();
                SwitchRefresh();
            }
            else//待训练状态
            {
                long trainCount = m_currTrainCount;
                long count = m_trainCapacity;
                InitWaitTrainData();
                m_currTrainCount = (trainCount > m_trainCapacity) ? m_trainCapacity : trainCount;
                if (count != m_trainCapacity) //判断一下容量上限 是否变更
                {
                    RefreshWaitTrainContent((int)m_currTrainCount);
                }
                else
                {
                    RefreshTrainRes();
                }
            }
            RefreshTrainCapacityGain();
        }

        private void Close()
        {
            if (m_isDispose)
            {
                return;
            }
            CoreUtils.uiManager.CloseUI(UI.s_trainArmy);
        }
    }
}