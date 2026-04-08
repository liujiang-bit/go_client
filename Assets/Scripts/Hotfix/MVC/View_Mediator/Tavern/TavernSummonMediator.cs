// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月10日
// Update Time         :    2020年4月10日
// Class Description   :    TavernSummonMediator 酒馆招募
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
using UnityEngine.UI;
using System;

namespace Game {

    public enum EnumBoxRewardShowType
    {
        Silver = 1,       //白银宝箱标题
        SilverData = 2,   //白银宝箱数据
        Gold = 3,         //黄金宝箱标题
        GoldData = 4,     //4黄金宝箱数据
    }

    public enum EnumBoxRewardType
    {
        Silver = 1, //白银宝箱
        Gold = 2,   //黄金宝箱
    }

    public class BoxRewardInfo
    {
        public Heros HeroInfo;
        public RewardItem ItemInfo;
    }

    public class BoxPreviewRewardData
    {
        public int ShowType; //1白银宝箱标题 2白银宝箱数据 3黄金宝箱标题 4黄金宝箱数据
        public List<int> DataList;
    }

    public class TavernSummonMediator : GameMediator {
        #region Member
        public static string NameMediator = "TavernSummonMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private BagProxy m_bagProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;

        private ConfigDefine m_configDefine;

        private long m_silverBoxNum;
        private long m_goldBoxNum;

        private int m_multiOpenNum;

        private Timer m_timer;

        private bool m_isUpdateSilverTime;
        private bool m_isUpdateGoldTime;

        private ItemDefine m_silverDefine;
        private ItemDefine m_goldDefine;

        private bool m_isRequesting;
        private List<BoxRewardInfo> m_singleBoxRewardList;
        private List<UI_Item_TavernSingleReward_SubView> m_singleRewardViewList;
        private EnumBoxRewardType m_openBoxType = 0; //1白银宝箱 2黄金宝箱

        private List<BoxPreviewRewardData> m_previewRewardList;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private int m_previewRewardColCount = 3;

        private System.Random m_random;

        private bool m_isDispose;

        private int m_boxOpenWay = 0;

        private float m_tavernRewardItemHeight;
        private float m_tavernRewardTitleHeight;

        #endregion

        //IMediatorPlug needs
        public TavernSummonMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public TavernSummonView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                Build_Tavern.TagName,
                CmdConstant.TavernBoxInfoChange,
                CmdConstant.UpdateFloatCurrency,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Build_Tavern.TagName:
                    //m_isRequesting = false;
                    var response = notification.Body as Build_Tavern.response;
                    if (response == null)
                    {
                        return;
                    }
                    //ClientUtils.Print(response);
                    if (response.count >= m_multiOpenNum) 
                    {
                        //多开
                        ShowMultiReward(response);
                    }
                    else
                    {
                        //单开
                        ShowSingleReward(response.rewardInfo, (int)response.count);
                    }
                    break;
                case CmdConstant.TavernBoxInfoChange:
                    int type = (int)notification.Body;
                    BoxInfoChange(type);
                    break;
                case CmdConstant.UpdateFloatCurrency: //更新代币
                    RefreshTitleBoxNum();
                    break;
                default:
                    break;
            }
        }

        private void BoxInfoChange(int type)
        {
            if (type == (int)EnumBoxRewardType.Silver)
            {
                //白银
                ReadSilverBoxNum();
                RefreshTitleBoxNum();
                RefreshSilverBox();
            }
            else
            {
                //黄金
                ReadGoldBoxNum();
                RefreshTitleBoxNum();
                RefreshGoldBox();
            }
        }


        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

        }

        public override void OnRemove()
        {
            m_isDispose = true;
            CancelTimer();
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            m_random = new System.Random();

            m_configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_silverDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(m_configDefine.silverBoxOpenItem);
            m_goldDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(m_configDefine.goldBoxOpenItem);
            m_multiOpenNum = 10;

            view.m_lbl_woodbox_name_LanguageText.text = LanguageUtils.getText(760033);
            view.m_lbl_goldbox_name_LanguageText.text = LanguageUtils.getText(760032);

            ReadSilverBoxNum();
            ReadGoldBoxNum();

            view.m_btn_rewardMask_GameButton.gameObject.SetActive(false);
            view.m_pl_resultShow_PolygonImage.gameObject.SetActive(false);
            view.m_pl_showReward_Animator.gameObject.SetActive(false);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Interface.AddClickEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_tavernSummon);
            });
            view.m_btn_showall_GameButton.onClick.AddListener(ShowRewardPreview);
            view.m_btn_rewardMask_GameButton.onClick.AddListener(HideRewardPreview);
            view.m_UI_Dia.m_btn_btn_GameButton.onClick.AddListener(OnRecharge);

            view.m_UI_Free_woodbox.m_btn_languageButton_GameButton.onClick.AddListener(OnSliverFreeOpen);
            view.m_UI_Open_woodbox.m_btn_languageButton_GameButton.onClick.AddListener(OnSilverSingleOpen);
            view.m_UI_OpenAll_woodbox.m_btn_languageButton_GameButton.onClick.AddListener(OnSilverAllOpen);

            view.m_UI_Free_goldbox.m_btn_languageButton_GameButton.onClick.AddListener(OnGoldFreeOpen);
            view.m_UI_Open_goldbox.m_btn_languageButton_GameButton.onClick.AddListener(OnGoldSingleOpen);
            view.m_UI_OpenAll_goldbox.m_btn_languageButton_GameButton.onClick.AddListener(OnGoldAllOpen);

            view.m_UI_Free.m_btn_languageButton_GameButton.onClick.AddListener(OnSingleRewardViewSureBtn);
            view.m_UI_Open.m_btn_languageButton_GameButton.onClick.AddListener(OnSingleRewardViewOpenBtn);
            view.m_UI_OpenAll.m_btn_languageButton_GameButton.onClick.AddListener(OnSingleRewardViewMulBtn);

            view.m_pl_woodbox_ani_Animator.gameObject.GetComponent<Animator>().Play("UA_btn_woodbox_idle");
            view.m_pl_goldbox_ani_Animator.gameObject.GetComponent<Animator>().Play("UA_btn_goldbox_idle");

            Refresh();
        }

        protected override void BindUIData()
        {

        }

        #endregion

        #region 主界面逻辑
        private void Refresh()
        {
            RefreshTitleBoxNum();

            RefreshSilverBox();

            RefreshGoldBox();
        }

        //刷新白银宝箱
        private void RefreshSilverBox(bool isAllowRefreshTime = true)
        {
            long serverTime = ServerTimeModule.Instance.GetServerTime();

            bool isShowTime = false;
            if (m_playerProxy.CurrentRoleInfo.silverFreeCount > 0)
            {
                if (m_playerProxy.CurrentRoleInfo.openNextSilverTime < serverTime)
                {
                    int total = GetSilverFreeTotal();
                    view.m_lbl_wooddesc_LanguageText.text = LanguageUtils.getTextFormat(760017, m_playerProxy.CurrentRoleInfo.silverFreeCount, total);

                    //显示免费宝箱
                    view.m_UI_Free_woodbox.gameObject.SetActive(true);
                    view.m_UI_Open_woodbox.gameObject.SetActive(false);
                    view.m_UI_OpenAll_woodbox.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    isShowTime = true;
                }
            }

            if (isShowTime && isAllowRefreshTime)
            {
                m_isUpdateSilverTime = true;
                CreateTimer();
                RefreshSilverTime();
            }
            else
            {
                int total = GetSilverFreeTotal();
                view.m_lbl_wooddesc_LanguageText.text = LanguageUtils.getTextFormat(760017, m_playerProxy.CurrentRoleInfo.silverFreeCount, total);
                //view.m_lbl_wooddesc_LanguageText.text = "";
            }
            //显示道具宝箱
            view.m_UI_Free_woodbox.gameObject.SetActive(false);
            view.m_UI_Open_woodbox.gameObject.SetActive(true);
            view.m_UI_Open_woodbox.m_lbl_line2_LanguageText.text = "1";
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Open_woodbox.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());

            if (m_silverBoxNum >= m_multiOpenNum)
            {
                view.m_UI_OpenAll_woodbox.gameObject.SetActive(true);
                view.m_UI_OpenAll_woodbox.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_silverBoxNum);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_OpenAll_woodbox.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
            else
            {
                view.m_UI_OpenAll_woodbox.gameObject.SetActive(false);
            }

        }

        //刷新黄金宝箱
        private void RefreshGoldBox(bool isAllowRefreshTime = true)
        {
            if (GuideProxy.IsGuideing)
            {
                if (m_goldBoxNum > 0)
                {
                    view.m_lbl_golddesc_LanguageText.text = "";
                    view.m_UI_Free_goldbox.gameObject.SetActive(false);
                    view.m_UI_OpenAll_goldbox.gameObject.SetActive(false);
                    view.m_UI_Open_goldbox.gameObject.SetActive(true);
                    view.m_UI_Open_goldbox.m_lbl_line2_LanguageText.text = "1";
                    LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Open_goldbox.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                }
                else
                {
                    view.m_lbl_golddesc_LanguageText.text = "";
                    view.m_UI_Free_goldbox.gameObject.SetActive(true);
                    view.m_UI_OpenAll_goldbox.gameObject.SetActive(false);
                    view.m_UI_Open_goldbox.gameObject.SetActive(false);
                }
                return;
            }

            long serverTime = ServerTimeModule.Instance.GetServerTime();

            bool isShowTime = false;
            if (m_playerProxy.CurrentRoleInfo.addGoldFreeAddTime < serverTime)
            {
                view.m_lbl_golddesc_LanguageText.text = LanguageUtils.getTextFormat(760018, m_playerProxy.CurrentRoleInfo.goldFreeCount, 1);

                //显示免费宝箱
                view.m_UI_Free_goldbox.gameObject.SetActive(true);
                view.m_UI_Open_goldbox.gameObject.SetActive(false);
                view.m_UI_OpenAll_goldbox.gameObject.SetActive(false);
                return;
            }
            else
            {
                isShowTime = true;
            }

            if (isShowTime && isAllowRefreshTime)
            {
                m_isUpdateGoldTime = true;
                CreateTimer();
                RefreshGoldTime();
            }
            else
            {
                view.m_lbl_golddesc_LanguageText.text = "";
            }

            //显示道具宝箱
            view.m_UI_Free_goldbox.gameObject.SetActive(false);
            view.m_UI_Open_goldbox.gameObject.SetActive(true);
            view.m_UI_Open_goldbox.m_lbl_line2_LanguageText.text = "1";
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Open_goldbox.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());

            if (m_goldBoxNum >= m_multiOpenNum)
            {
                view.m_UI_OpenAll_goldbox.gameObject.SetActive(true);
                view.m_UI_OpenAll_goldbox.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_goldBoxNum);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_OpenAll_goldbox.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
            else
            {
                view.m_UI_OpenAll_goldbox.gameObject.SetActive(false);
            }
        }

        private void ReadSilverBoxNum()
        {
            m_silverBoxNum = (int)m_bagProxy.GetItemNum(m_configDefine.silverBoxOpenItem);
        }

        private void ReadGoldBoxNum()
        {
            m_goldBoxNum = m_bagProxy.GetItemNum(m_configDefine.goldBoxOpenItem);
        }

        private void RefreshTitleBoxNum()
        {
            view.m_UI_Sil.m_lbl_val_LanguageText.text = ClientUtils.FormatComma(m_silverBoxNum);
            view.m_UI_Gold.m_lbl_val_LanguageText.text = ClientUtils.FormatComma(m_goldBoxNum);
            view.m_UI_Dia.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_playerProxy.GetResNumByType((int)EnumCurrencyType.denar));
        }

        //获取白银宝箱免费次数
        private int GetSilverFreeTotal()
        {
            BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByType((int)EnumCityBuildingType.Tavern);
            if (info == null)
            {
                Debug.LogError("没找到酒馆数据");
                return 0;
            }
            BuildingTavernDefine define = CoreUtils.dataService.QueryRecord<BuildingTavernDefine>((int)info.level);
            if (define == null)
            {
                return 0;
            }
            return define.silverBoxCnt;
        }

        private void CreateTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, false);
            }
        }

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
            if (m_isUpdateSilverTime)
            {
                RefreshSilverTime();
            }
            if (m_isUpdateGoldTime)
            {
                RefreshGoldTime();
            }
        }

        private void RefreshSilverTime()
        {
            long serverTime = ServerTimeModule.Instance.GetServerTime();
            long diff = m_playerProxy.CurrentRoleInfo.openNextSilverTime - serverTime;
            if (diff < 0)
            {
                m_isUpdateSilverTime = false;
                RefreshSilverBox(false);
            }
            else
            {
                view.m_lbl_wooddesc_LanguageText.text = LanguageUtils.getTextFormat(760019, ClientUtils.FormatCountDown((int)diff));
            }
        }

        private void RefreshGoldTime()
        {
            long serverTime = ServerTimeModule.Instance.GetServerTime();
            long diff = m_playerProxy.CurrentRoleInfo.addGoldFreeAddTime - serverTime;
            if (diff < 0)
            {
                m_isUpdateGoldTime = false;
                RefreshGoldBox(false);
            }
            else
            {
                view.m_lbl_golddesc_LanguageText.text = LanguageUtils.getTextFormat(760019, ClientUtils.FormatCountDown((int)diff));
            }
        }

        //充值界面
        private void OnRecharge()
        {
            Tip.CreateTip(100045).Show();
        }

        //白银宝箱 免费开启
        private void OnSliverFreeOpen()
        {
            Send((int)EnumBoxRewardType.Silver, true, 0, false);
        }

        //白银宝箱 单个开启
        private void OnSilverSingleOpen()
        {
            //if (m_isRequesting)
            //{
            //    return;
            //} 

            bool isUseDenar = false;
            if (m_silverBoxNum < m_configDefine.silverBoxOpenItemNum)
            {
                //判断是否提醒
                GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
                bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.RecruitPurchaseConfirmation);
                if (isRemind)
                {
                    CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                    string currencyiconId = currencyProxy.GeticonIdByType(104);
                    Data.SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.RecruitPurchaseConfirmation);
                    if (settingPersonalityDefine != null)
                    {
                        string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                        string str = LanguageUtils.getTextFormat(760016, LanguageUtils.getText(m_silverDefine.l_nameID), m_silverDefine.shopPrice);
                        Alert.CreateAlert(str, LanguageUtils.getText(610021)).
                                          SetLeftButton().
                                          SetRightButton(null, LanguageUtils.getText(730038)).
                                          SetCurrencyRemind((isBool) =>
                                          {
                                              if (isBool)
                                              {
                                                  generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.RecruitPurchaseConfirmation);
                                              }
                                          //判断代币是否足够
                                          if (m_currencyProxy.ShortOfDenar(m_silverDefine.shopPrice))
                                              {
                                                  return;
                                              }
                                              Send((int)EnumBoxRewardType.Silver, false, 1, true);
                                          },
                                          m_silverDefine.shopPrice, s_remind, currencyiconId).
                                          Show();
                    }
                    return;
                }

                //判断代币是否足够
                if (m_currencyProxy.ShortOfDenar(m_silverDefine.shopPrice))
                {
                    return;
                }
                isUseDenar = true;
            }
            Send((int)EnumBoxRewardType.Silver, false, 1, isUseDenar);
        }

        //白银宝箱 全部开启
        private void OnSilverAllOpen()
        {
            //if (m_isRequesting)
            //{
            //    return;
            //}
            string str = LanguageUtils.getTextFormat(760036, m_silverBoxNum, LanguageUtils.getText(m_silverDefine.l_nameID));
            Alert.CreateAlert(str, LanguageUtils.getText(610021)).
                  SetLeftButton().
                  SetRightButton(() => {
                      Send((int)EnumBoxRewardType.Silver, false, (int)m_silverBoxNum, false);
                  }).
                  Show();
        }

        //黄金宝箱 免费开启
        private void OnGoldFreeOpen()
        {
            //if (m_isRequesting)
            //{
            //    return;
            //}
            Send((int)EnumBoxRewardType.Gold, true, 0, false);
        }

        //黄金宝箱 单个开启
        private void OnGoldSingleOpen()
        {
            //if (m_isRequesting)
            //{
            //    return;
            //}
            bool isUseDenar = false;
            if (m_goldBoxNum < m_configDefine.goldBoxOpenItemNum)
            {
                //判断是否提醒
                GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
                bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.RecruitPurchaseConfirmation);
                if (isRemind)
                {
                    CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                    string currencyiconId = currencyProxy.GeticonIdByType(104);
                    Data.SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.RecruitPurchaseConfirmation);
                    if (settingPersonalityDefine != null)
                    {
                        string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                        string str = LanguageUtils.getTextFormat(760016, LanguageUtils.getText(m_goldDefine.l_nameID), m_goldDefine.shopPrice);
                        Alert.CreateAlert(str, LanguageUtils.getText(610021)).
                                          SetLeftButton().
                                          SetRightButton(null, LanguageUtils.getText(730038)).
                                          SetCurrencyRemind((isBool) =>
                                          {
                                              if (isBool)
                                              {
                                                  generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.RecruitPurchaseConfirmation);
                                              }
                                          //判断代币是否足够
                                          if (m_currencyProxy.ShortOfDenar(m_goldDefine.shopPrice))
                                              {
                                                  return;
                                              }
                                              Send((int)EnumBoxRewardType.Gold, false, 1, true);
                                          },
                                          m_goldDefine.shopPrice, s_remind, currencyiconId). 
                                          Show();
                    }
                    return;
                }

                //判断代币是否足够
                if (m_currencyProxy.ShortOfDenar(m_goldDefine.shopPrice))
                {
                    return;
                }
                isUseDenar = true;
            }
            Send((int)EnumBoxRewardType.Gold, false, 1, isUseDenar);
        }

        //黄金宝箱 全部开启
        private void OnGoldAllOpen()
        {
            //if (m_isRequesting)
            //{
            //    return;
            //}
            string str = LanguageUtils.getTextFormat(760036, m_goldBoxNum, LanguageUtils.getText(m_goldDefine.l_nameID));
            Alert.CreateAlert(str, LanguageUtils.getText(610021)).
                  SetLeftButton().
                  SetRightButton(() => {
                      Send((int)EnumBoxRewardType.Gold, false, (int)m_goldBoxNum, false);
                  }).
                  Show();
        }

        #endregion

        #region 奖励一栏

        //显示奖励预览
        private void ShowRewardPreview()
        {
            view.m_btn_rewardMask_GameButton.gameObject.SetActive(true);
            view.m_pl_showReward_Animator.gameObject.SetActive(true);

            view.m_btn_showall_GameButton.gameObject.SetActive(false);
            view.m_UI_Model_Interface.m_btn_back_GameButton.gameObject.SetActive(false);

            view.m_lbl_woodbox_name_LanguageText.gameObject.SetActive(false);
            view.m_lbl_wooddesc_LanguageText.gameObject.SetActive(false);
            view.m_pl_woodbox_btns_GridLayoutGroup.gameObject.SetActive(false);

            view.m_lbl_goldbox_name_LanguageText.gameObject.SetActive(false);
            view.m_lbl_golddesc_LanguageText.gameObject.SetActive(false);
            view.m_pl_goldbox_btns_GridLayoutGroup.gameObject.SetActive(false);

            if (m_assetDic.ContainsKey("UI_LC_TavernReward"))
            {
                RefershPreviewList();
            }
            else
            {
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
            }
        }

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
            m_previewRewardColCount = m_assetDic["UI_LC_TavernReward"].GetComponent<GridLayoutGroup>().constraintCount;

            m_tavernRewardTitleHeight = m_assetDic["UI_Item_TavernRewardProb"].GetComponent<RectTransform>().rect.height;

            m_tavernRewardItemHeight = m_assetDic["UI_LC_TavernReward"].GetComponent<RectTransform>().rect.height;

            if (m_previewRewardList == null)
            {
                ReadPreviewRewardData();
            }
            InitPreviewList();
        }

        private void RefershPreviewList()
        {
            view.m_sv_list_view_ListView.FillContent(m_previewRewardList.Count);
        }

        private void InitPreviewList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            functab.GetItemPrefabName = OnGetItemPrefabName;
            functab.GetItemSize = OnGetItemSize;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_view_ListView.FillContent(m_previewRewardList.Count);
        }

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            if (m_previewRewardList[listItem.index].ShowType == (int)EnumBoxRewardShowType.Silver || 
                m_previewRewardList[listItem.index].ShowType == (int)EnumBoxRewardShowType.Gold)
            {
                return "UI_Item_TavernRewardProb";
            }
            else
            {
                return "UI_LC_TavernReward";
            }
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            if (m_previewRewardList[listItem.index].ShowType == (int)EnumBoxRewardShowType.Silver || 
                m_previewRewardList[listItem.index].ShowType == (int)EnumBoxRewardShowType.Gold)
            {
                return m_tavernRewardTitleHeight;
            }
            else
            {
                return m_tavernRewardItemHeight;
            }
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            var itemData = m_previewRewardList[listItem.index];
            if (itemData.ShowType == (int)EnumBoxRewardShowType.Silver || itemData.ShowType == (int)EnumBoxRewardShowType.Gold)
            {
                if (listItem.data == null)
                {
                    var subView = new UI_Item_TavernRewardProb_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.AddBtnListener();
                    subView.RefreshItem(itemData);
                }
                else
                {
                    var subView = listItem.data as UI_Item_TavernRewardProb_SubView;
                    subView.RefreshItem(itemData);
                }
            }
            else
            {
                if (listItem.data == null)
                {
                    var subView = new UI_LC_TavernReward_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.Init();
                    subView.RefreshItem(itemData);
                }
                else
                {
                    var subView = listItem.data as UI_LC_TavernReward_SubView;
                    subView.RefreshItem(itemData);

                }
            }
        }
        Dictionary<int, TavernRankDefine> m_silverTavernRankDic = new Dictionary<int, TavernRankDefine>();
        List<int> m_silverTavernRankList = new List<int>();
        Dictionary<int, TavernRankDefine> m_goldTavernRankDic = new Dictionary<int, TavernRankDefine>();
        List<int> m_goldTavernRankList = new List<int>();
        private void FillTavernRankDic()
        {
            List<TavernRankDefine> tavernRankDefines = CoreUtils.dataService.QueryRecords<TavernRankDefine>();
            tavernRankDefines.ForEach((tavernRankDefine) => {
                if (tavernRankDefine.group == 2)
                {
                    if (!m_goldTavernRankDic.ContainsKey(tavernRankDefine.ID))
                    {
                        m_goldTavernRankDic.Add(tavernRankDefine.ID, tavernRankDefine);
                    }
                    if (!m_goldTavernRankList.Contains(tavernRankDefine.ID))
                    {
                        m_goldTavernRankList.Add(tavernRankDefine.ID);
                    }
                }
                else if (tavernRankDefine.group == 1)
                {
                    if (!m_silverTavernRankDic.ContainsKey(tavernRankDefine.ID))
                    {
                        m_silverTavernRankDic.Add(tavernRankDefine.ID, tavernRankDefine);
                    }
                    if (!m_silverTavernRankList.Contains(tavernRankDefine.ID))
                    {
                        m_silverTavernRankList.Add(tavernRankDefine.ID);
                    }
                }
            });
        }
        private bool FillBoxPreviewRewardDataList()
        {
            bool secess = false;
            //整理数据
            BoxPreviewRewardData data3 = new BoxPreviewRewardData();
            data3.ShowType = (int)EnumBoxRewardShowType.Gold;
            m_previewRewardList.Add(data3);

            if (m_goldTavernRankList.Count > 0)
            {
                int total = m_goldTavernRankList.Count;
                int lineCount = (int)Math.Ceiling((float)total / m_previewRewardColCount);
                int count = 0;
                int tIndex = 0;
                for (int i = 0; i < lineCount; i++)
                {
                    BoxPreviewRewardData data4 = new BoxPreviewRewardData();
                    data4.ShowType = (int)EnumBoxRewardShowType.GoldData;
                    data4.DataList = new List<int>();
                    m_previewRewardList.Add(data4);
                    for (int j = 0; j < m_previewRewardColCount; j++)
                    {
                        tIndex = count + j;
                        if (tIndex < total)
                        {
                            data4.DataList.Add(m_goldTavernRankList[tIndex]);
                        }
                    }
                    count = count + m_previewRewardColCount;
                }
            }

            BoxPreviewRewardData data1 = new BoxPreviewRewardData();
            data1.ShowType = (int)EnumBoxRewardShowType.Silver;
            m_previewRewardList.Add(data1);

            if (m_silverTavernRankList.Count > 0)
            {
                int total = m_silverTavernRankList.Count;
                int lineCount = (int)Math.Ceiling((float)total / m_previewRewardColCount);
                int count = 0;
                int tIndex = 0;
                for (int i = 0; i < lineCount; i++)
                {
                    BoxPreviewRewardData data2 = new BoxPreviewRewardData();
                    data2.ShowType = (int)EnumBoxRewardShowType.SilverData;
                    data2.DataList = new List<int>();
                    m_previewRewardList.Add(data2);
                    for (int j = 0; j < m_previewRewardColCount; j++)
                    {
                        tIndex = count + j;
                        if (tIndex < total)
                        {
                            data2.DataList.Add(m_silverTavernRankList[tIndex]);
                        }
                    }
                    count = count + m_previewRewardColCount;
                }
            }
            return secess;
        }
        private void ReadPreviewRewardData()
        {
            m_previewRewardList = new List<BoxPreviewRewardData>();
            FillTavernRankDic();
            FillBoxPreviewRewardDataList();
        }

        //隐藏奖励预览
        private void HideRewardPreview()
        {
            view.m_btn_rewardMask_GameButton.gameObject.SetActive(false);
            view.m_pl_showReward_Animator.gameObject.SetActive(false);

            view.m_btn_showall_GameButton.gameObject.SetActive(true);
            view.m_UI_Model_Interface.m_btn_back_GameButton.gameObject.SetActive(true);

            view.m_lbl_woodbox_name_LanguageText.gameObject.SetActive(true);
            view.m_lbl_wooddesc_LanguageText.gameObject.SetActive(true);
            view.m_pl_woodbox_btns_GridLayoutGroup.gameObject.SetActive(true);

            view.m_lbl_goldbox_name_LanguageText.gameObject.SetActive(true);
            view.m_lbl_golddesc_LanguageText.gameObject.SetActive(true);
            view.m_pl_goldbox_btns_GridLayoutGroup.gameObject.SetActive(true);
        }

        #endregion
       
        #region 单个宝箱开启表现

        //显示单个宝箱奖励
        private void ShowSingleReward(RewardInfo rewardInfo, int openCount =0)
        {
            if (rewardInfo == null)
            {
                return;
            }

            if (openCount > 0)
            {
                if (m_openBoxType == EnumBoxRewardType.Silver)
                {
                    ReadSilverBoxNum();
                    RefreshSilverBox();
                    RefreshTitleBoxNum();
                }
                else
                {
                    ReadGoldBoxNum();
                    RefreshGoldBox();
                    RefreshTitleBoxNum();
                }
            }

            view.m_btn_showall_GameButton.gameObject.SetActive(false); //预览一栏
            view.m_UI_Model_Interface.m_btn_back_GameButton.gameObject.SetActive(false); //关闭按钮

            if (view.m_pl_box.gameObject.activeSelf)
            {
                //开始动画表现
                if (m_openBoxType == EnumBoxRewardType.Silver)
                {
                    view.m_pl_goldbox.gameObject.SetActive(false);
                    view.m_pl_woodbox_content.gameObject.SetActive(false);

                    Animator ani = view.m_pl_woodbox_ani_Animator.gameObject.GetComponent<Animator>();
                    ani.Play("UA_btn_woodbox_in");
                    view.m_UI_woodbox.PlayAni(view.m_UI_woodbox.AniJump, ()=> {
                        if (m_isDispose)
                        {
                            return;
                        }
                        //显示特效 
                        view.m_pl_woodbox_standby_effect.gameObject.SetActive(true);
                        view.m_pl_woodbox_standby_effect.transform.position = view.m_pl_woodbox_ani_Animator.transform.position;
                        view.m_UI_woodbox.PlayAni(view.m_UI_woodbox.AniOpen, () =>
                        {
                            if (m_isDispose)
                            {
                                return;
                            }
                            ani.Play("UA_btn_woodbox_idle");
                            view.m_UI_woodbox.PlayAni(view.m_UI_woodbox.AniClose);
                            view.m_pl_woodbox_standby_effect.gameObject.SetActive(false);
                            ShowSingleRewardView(rewardInfo);
                        });
                    });
                }
                else
                {
                    view.m_pl_woodbox.gameObject.SetActive(false);
                    view.m_pl_goldbox_content.gameObject.SetActive(false);

                    Animator ani = view.m_pl_goldbox_ani_Animator.gameObject.GetComponent<Animator>();
                    ani.Play("UA_btn_goldbox_in");
                    view.m_UI_goldbox.PlayAni(view.m_UI_goldbox.AniJump, ()=> {
                        if (m_isDispose)
                        {
                            return;
                        }
                        //显示特效
                        view.m_pl_goldbox_standby_effect.gameObject.SetActive(true);
                        view.m_pl_goldbox_standby_effect.transform.position = view.m_pl_goldbox_ani_Animator.transform.position;
                        view.m_UI_goldbox.PlayAni(view.m_UI_goldbox.AniOpen, () =>
                        {
                            if (m_isDispose)
                            {
                                return;
                            }
                            ani.Play("UA_btn_goldbox_idle");
                            view.m_UI_goldbox.PlayAni(view.m_UI_goldbox.AniClose);
                            view.m_pl_goldbox_standby_effect.gameObject.SetActive(false);
                            ShowSingleRewardView(rewardInfo);
                        });
                    });
                }
            }
            else
            {
                ShowSingleRewardView(rewardInfo);
            }
        }

        private void ShowSingleRewardView(RewardInfo rewardInfo)
        {
            view.m_pl_box.gameObject.SetActive(false);
            view.m_pl_resultShow_PolygonImage.gameObject.SetActive(true); //单个宝箱奖励界面
            if (m_singleBoxRewardList == null)
            {
                m_singleBoxRewardList = new List<BoxRewardInfo>();
                m_singleRewardViewList = new List<UI_Item_TavernSingleReward_SubView>();
                m_singleRewardViewList.Add(view.m_UI_Item_TavernSingleReward1);
                m_singleRewardViewList.Add(view.m_UI_Item_TavernSingleReward2);
                m_singleRewardViewList.Add(view.m_UI_Item_TavernSingleReward3);
                m_singleRewardViewList.Add(view.m_UI_Item_TavernSingleReward4);
            }
            m_singleBoxRewardList.Clear();

            view.m_pl_resul_btns_GridLayoutGroup.gameObject.SetActive(false);
            HideAllRewardItem();
            RemoveRewardEffect();

            if (rewardInfo.HasItems)
            {
                for (int i = 0; i < rewardInfo.items.Count; i++)
                {
                    BoxRewardInfo reward = new BoxRewardInfo();
                    reward.ItemInfo = rewardInfo.items[i];
                    m_singleBoxRewardList.Add(reward);
                }
            }
            Dictionary<Int64, int> heroMap = new Dictionary<Int64, int>();
            if (rewardInfo.HasHeros)
            {
                for (int i = 0; i < rewardInfo.heros.Count; i++)
                {
                    BoxRewardInfo reward = new BoxRewardInfo();
                    reward.HeroInfo = rewardInfo.heros[i];
                    int num = m_random.Next(m_singleBoxRewardList.Count);
                    m_singleBoxRewardList.Insert(num, reward);
                    if (reward.HeroInfo.isNew == 1)
                    {
                        heroMap[reward.HeroInfo.heroId] = 1;
                    }
                }
            }
            if (m_singleBoxRewardList.Count < 4)
            {
                Debug.LogError("奖励物品小于4 找服务端");
                return;
            }

            //随机插入
            List<BoxRewardInfo> tList = new List<BoxRewardInfo>();
            int count = m_singleBoxRewardList.Count;
            for (int i = 0; i < count; i++)
            {
                tList.Insert(m_random.Next(tList.Count), m_singleBoxRewardList[i]);
            }
            //将新英雄排在前面显示
            for (int i = 0; i < tList.Count; i++)
            {
                if (tList[i].HeroInfo != null)
                {
                    if (heroMap.ContainsKey(tList[i].HeroInfo.heroId))
                    {
                        if (heroMap[tList[i].HeroInfo.heroId] == 1)
                        {
                            heroMap[tList[i].HeroInfo.heroId] = 2;
                            tList[i].HeroInfo.isNew = 1;
                        }
                        else
                        {
                            tList[i].HeroInfo.isNew = 0;
                        }
                    }
                }
            }
            m_singleBoxRewardList = tList;

            view.m_btn_singleRewardMask_GameButton.gameObject.SetActive(true);
            ShowRewardItem(0);
            RefreshSingleRewardView(m_openBoxType == EnumBoxRewardType.Silver);
        }

        private void ShowRewardItem(int index)
        {
            if (index >= m_singleBoxRewardList.Count)
            {
                view.m_btn_singleRewardMask_GameButton.gameObject.SetActive(false);
                view.m_pl_resul_btns_GridLayoutGroup.gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Open.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_OpenAll.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                return;
            }
            BoxRewardInfo boxInfo = m_singleBoxRewardList[index];
            m_singleRewardViewList[index].RefreshItem(boxInfo, ()=> {
                if (m_isDispose)
                {
                    return;
                }
                ShowRewardItem(index+1);
            });
        }

        private void HideBtnAndRewardItem()
        {
            view.m_pl_resul_btns_GridLayoutGroup.gameObject.SetActive(false);
            HideAllRewardItem();
        }

        //private void SetRewardEffectVisible(bool isVisible)
        //{
        //    if (m_singleRewardViewList != null)
        //    {
        //        for (int i = 0; i < m_singleRewardViewList.Count; i++)
        //        {
        //            m_singleRewardViewList[i].SetEffectVisible(isVisible);
        //        }
        //    }
        //}

        private void RemoveRewardEffect()
        {
            if (m_singleRewardViewList != null)
            {
                for (int i = 0; i < m_singleRewardViewList.Count; i++)
                {
                    m_singleRewardViewList[i].RemoveEffect();
                }
            }
        }

        private void HideAllRewardItem()
        {
            for (int i = 0; i < m_singleRewardViewList.Count; i++)
            {
                m_singleRewardViewList[i].gameObject.SetActive(false);
            }
        }

        private void RefreshSingleRewardView(bool isSilver)
        {
            if (isSilver) 
            {
                //开启白银宝箱
                RefreshSingleRewardSilver();
            }
            else
            {
                //开启黄金宝箱
                RefreshSingleRewardGold();
            }
        }

        //刷新白银宝箱 单个奖励界面
        private void RefreshSingleRewardSilver()
        {
            view.m_UI_Free.gameObject.SetActive(true);
            view.m_UI_Free.m_lbl_Text_LanguageText.text = LanguageUtils.getText(100036);

            //显示道具宝箱
            ClientUtils.LoadSprite(view.m_UI_Open.m_img_icon2_PolygonImage, RS.TavernSummonBoxKey[0]);
            view.m_UI_Open.gameObject.SetActive(true);
            view.m_UI_Open.m_lbl_line2_LanguageText.text = "1";
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Open.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());

            if (m_silverBoxNum >= m_multiOpenNum)
            {
                ClientUtils.LoadSprite(view.m_UI_OpenAll.m_img_icon2_PolygonImage, RS.TavernSummonBoxKey[0]);
                view.m_UI_OpenAll.gameObject.SetActive(true);
                view.m_UI_OpenAll.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_silverBoxNum);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_OpenAll.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
            else
            {
                view.m_UI_OpenAll.gameObject.SetActive(false);
            }
        }

        //刷新黄金宝箱 单个奖励界面
        private void RefreshSingleRewardGold()
        {
            view.m_UI_Free.gameObject.SetActive(true);
            view.m_UI_Free.m_lbl_Text_LanguageText.text = LanguageUtils.getText(100036);

            //显示道具宝箱
            ClientUtils.LoadSprite(view.m_UI_Open.m_img_icon2_PolygonImage, RS.TavernSummonBoxKey[1]);
            view.m_UI_Open.gameObject.SetActive(true);
            view.m_UI_Open.m_lbl_line2_LanguageText.text = "1";
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Open.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());

            if (m_goldBoxNum >= m_multiOpenNum)
            {
                ClientUtils.LoadSprite(view.m_UI_OpenAll.m_img_icon2_PolygonImage, RS.TavernSummonBoxKey[1]);
                view.m_UI_OpenAll.gameObject.SetActive(true);
                view.m_UI_OpenAll.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(m_goldBoxNum);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_OpenAll.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
            else
            {
                view.m_UI_OpenAll.gameObject.SetActive(false);
            }
        }

        //单宝箱奖励界面 确认
        private void OnSingleRewardViewSureBtn()
        {
            view.m_pl_resultShow_PolygonImage.gameObject.SetActive(false);
            view.m_UI_Model_Interface.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_pl_box.gameObject.SetActive(true);

            view.m_pl_woodbox.gameObject.SetActive(true);
            view.m_pl_woodbox_content.gameObject.SetActive(true);
            view.m_pl_woodbox_ani_Animator.gameObject.GetComponent<Animator>().Play("UA_btn_woodbox_idle");

            view.m_pl_goldbox.gameObject.SetActive(true);
            view.m_pl_goldbox_content.gameObject.SetActive(true);
            view.m_pl_goldbox_ani_Animator.gameObject.GetComponent<Animator>().Play("UA_btn_goldbox_idle");

            view.m_btn_showall_GameButton.gameObject.SetActive(true);
            RemoveRewardEffect();
        }

        //单个宝箱奖励界面 单开
        private void OnSingleRewardViewOpenBtn()
        {
            if (m_openBoxType == EnumBoxRewardType.Silver)
            {
                OnSilverSingleOpen();
            }
            else
            {
                OnGoldSingleOpen();
            }
        }

        //单宝箱奖励界面 多开
        private void OnSingleRewardViewMulBtn()
        {
            if (m_openBoxType == EnumBoxRewardType.Silver)
            {
                OnSilverAllOpen();
            }
            else
            {
                OnGoldAllOpen();
            }
        }

        #endregion


        #region 多个宝箱开启表现

        //显示多个宝箱奖励
        private void ShowMultiReward(Build_Tavern.response response)
        {
            //SetRewardEffectVisible(false); 
            if (m_openBoxType == EnumBoxRewardType.Silver)
            {
                ReadSilverBoxNum();
                RefreshSilverBox();
                RefreshTitleBoxNum();
            }
            else
            {
                ReadGoldBoxNum();
                RefreshGoldBox();
                RefreshTitleBoxNum();
            }
            // 刷新奖励界面按钮
            if (view.m_pl_resultShow_PolygonImage.gameObject.activeSelf)
            {
                RefreshSingleRewardView(m_openBoxType == EnumBoxRewardType.Silver);
            }

            CoreUtils.uiManager.ShowUI(UI.s_tavernReward, null, response);
        }

        #endregion

        //发包
        private void Send(int type, bool isFree, int count, bool isUseDenar = false)
        {
            if (view.m_pl_resultShow_PolygonImage.gameObject.activeSelf) 
            {
                if(count==1)
                {
                    //在单开奖励界面 开启单个宝箱
                    HideBtnAndRewardItem();
                }
            }

            m_openBoxType = (EnumBoxRewardType)type;

            //m_isRequesting = true;
            var sp = new Build_Tavern.request();
            sp.type = type;
            sp.free = isFree;
            if (!isFree)
            {
                sp.count = count;
            }
            sp.useDenar = isUseDenar;
            AppFacade.GetInstance().SendSproto(sp);
        }
    }
}