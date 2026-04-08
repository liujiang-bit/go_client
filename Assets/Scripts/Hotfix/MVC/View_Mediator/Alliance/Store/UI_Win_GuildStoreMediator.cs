// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月27日
// Update Time         :    2020年5月27日
// Class Description   :    UI_Win_GuildStoreMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_GuildStoreMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildStoreMediator";


        enum EnumGuildStoreListViewDataType
        {
            Title,
            ItemLine,//目前设定一行四个
            EmptyText,//空内容则显示文本
        }

        struct GuildStoreListViewData
        {
            public EnumGuildStoreListViewDataType type;
            public List<int> lstId;//目前设定一行四个
            public List<long> lstNum;//目前设定一行四个
            public string txt;
        }

        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;
        private BagProxy m_bagProxy;
        private EnumAllianceStorePageType m_curPageType;
        private ItemEditNumView m_EditNumView;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private readonly int[] GroupTypeName = { 124001, 733103, 730081, 124003 };
        // private List<int> m_lstItemId = new List<int>();
        private List<GuildStoreListViewData> m_ListViewData = new List<GuildStoreListViewData>();
        private int m_CurSelectedItemId = -1;
        private UI_Model_Item_SubView m_CurSelectedItemView = null;
        public int CurSelectedItemId
        {
            get => m_CurSelectedItemId;
            set {
                m_CurSelectedItemId = value;
                if (value != -1)
                    OnItemSelected(value);
                else
                {
                    m_CurSelectedItemView = null;
                }
            }
        }

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildStoreMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_GuildStoreView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_ShopQuery.TagName,
                Guild_ShopBuy.TagName,
                Guild_ShopStock.TagName,

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_ShopBuy.TagName:
                    if (notification.Type != RpcTypeExtend.RESPONSE_ERROR)
                    {
                        var res = notification.Body as Guild_ShopBuy.response;
                        if (res.result)
                            OnClickBtnBuyCallback(CurSelectedItemId, m_EditNumView.CurNum);
                    }
                    else
                    {
                        TipError(notification);
                    }
                    SendReqShopItems();
                    break;
                case Guild_ShopStock.TagName:
                    if (notification.Type != RpcTypeExtend.RESPONSE_ERROR)
                    {
                        var res2 = notification.Body as Guild_ShopStock.response;
                        if (res2.HasResult && res2.result)
                            OnClickBtnStockCallback(CurSelectedItemId, m_EditNumView.CurNum);
                    }
                    SendReqShopItems();
                    break;
                case Guild_ShopQuery.TagName:
                    OnSendReqShopItemsCallback(notification.Body as Guild_ShopQuery.response);
                    break;
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {
            SendReqShopItems();
        }

        public override void WinClose()
        {
            CurSelectedItemId = -1;
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_curPageType = EnumAllianceStorePageType.Shoping;
            m_EditNumView = new ItemEditNumView(this.view, OnEditViewCurNumChanged);

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, LoadFinish);
        }


        protected override void BindUIEvent()
        {
            view.m_bg.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_AllianceStore);
            });
            view.m_bg.m_pl_side1.m_ck_ck_GameToggle.onValueChanged.AddListener((isOn) =>
            {
                m_curPageType = EnumAllianceStorePageType.Shoping;
                CurSelectedItemId = -1;
                RefreshAllView();
                view.m_sv_list_ListView.ScrollPanelToItemIndex(0);
                SendReqShopItems();
            });
            view.m_bg.m_pl_side2.m_ck_ck_GameToggle.onValueChanged.AddListener((isOn) =>
            {
                m_curPageType = EnumAllianceStorePageType.Stock;
                CurSelectedItemId = -1;
                RefreshAllView();
                view.m_sv_list_ListView.ScrollPanelToItemIndex(0);
            });

            view.m_btn_record1_GameButton.onClick.AddListener(() =>
            {
                //明细 二期不处理
            });

            view.m_btn_record2_GameButton.onClick.AddListener(() =>
            {
                //明细 二期不处理
            });

            view.m_btn_info1_GameButton.onClick.AddListener(() =>
            {
                HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(4020);
                if (define == null)
                {
                    return;
                }
                var m_helpTip = HelpTip.CreateTip(string.Format(LanguageUtils.getText(define.l_typeID), LanguageUtils.getText(define.l_data1)), view.m_btn_info1_PolygonImage.rectTransform)
                    .SetAutoFilter(true)
                    .SetOffset(view.m_btn_info1_PolygonImage.rectTransform.rect.width / 2)
                    .Show();
            });
            view.m_btn_info2_GameButton.onClick.AddListener(() =>
            {
                HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(4021);
                if (define == null)
                {
                    return;
                }
                var m_helpTip = HelpTip.CreateTip(string.Format(LanguageUtils.getText(define.l_typeID), LanguageUtils.getText(define.l_data1)), view.m_btn_info2_PolygonImage.rectTransform)
                    .SetAutoFilter(true)
                    .SetOffset(view.m_btn_info2_PolygonImage.rectTransform.rect.width / 2)
                    .Show();
            });
            view.m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(OnClickBtnBuy);
            view.m_btn_stock.m_btn_languageButton_GameButton.onClick.AddListener(OnClickBtnStock);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void RefreshAllView()
        {
            //            CurSelectedItemId = -1;
            RefreshTitle();
            RefreshItemList();
        }

        private void OnItemSelected(int value)
        {
            m_EditNumView.Reset();
            m_EditNumView.SetItemById(value);
            var count = m_allianceProxy.GetStoreItemCountByItemType(value);// 根据ID获得剩余数量
            m_EditNumView.SetMinAndMax(1, m_curPageType == EnumAllianceStorePageType.Shoping ? (count > 9999 ? 9999 : (int)count) : 9999);
            view.m_lbl_num_LanguageText.text =
                LanguageUtils.getTextFormat(m_curPageType == EnumAllianceStorePageType.Shoping ? 145021 : 733107,
                    m_curPageType == EnumAllianceStorePageType.Shoping ? m_bagProxy.GetItemNum(value) : count);
            OnEditViewCurNumChanged(m_EditNumView.CurNum);
        }

        private void OnEditViewCurNumChanged(int value)
        {
            if (CurSelectedItemId == -1)
                return;
            var guildStoreConfig = CoreUtils.dataService.QueryRecord<Data.AllianceShopItemDefine>(CurSelectedItemId);
            switch (m_curPageType)
            {
                case EnumAllianceStorePageType.Shoping:
                    long price = value * (long)guildStoreConfig.sellingPrice;
                    view.m_btn_buy.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(price);
                    long point = m_playerProxy.CurrentRoleInfo.guildPoint;// 获得个人积分
                    view.m_btn_buy.m_lbl_line2_LanguageText.color = point < price ? Color.red : Color.white;

                    view.m_btn_buy.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                    view.m_btn_buy.m_lbl_line2_ContentSizeFitter.SetLayoutVertical();
                    view.m_btn_buy.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                    view.m_btn_buy.m_pl_line2_HorizontalLayoutGroup.SetLayoutVertical();
                    break;
                case EnumAllianceStorePageType.Stock:
                    long price2 = value * (long)guildStoreConfig.stockPrice;
                    view.m_btn_stock.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(price2);
                    long point2 = m_allianceProxy.GetDepotCurrency_AlliancePointNum();
                    view.m_btn_stock.m_lbl_line2_LanguageText.color = point2 < price2 ? Color.red : Color.white;

                    view.m_btn_stock.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                    view.m_btn_stock.m_lbl_line2_ContentSizeFitter.SetLayoutVertical();
                    view.m_btn_stock.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                    view.m_btn_stock.m_pl_line2_HorizontalLayoutGroup.SetLayoutVertical();
                    break;
            }
        }
        private void RefreshTitle()
        {
            switch (m_curPageType)
            {
                case EnumAllianceStorePageType.Shoping:
                    view.m_pl_title1.gameObject.SetActive(true);
                    view.m_pl_title2.gameObject.SetActive(false);

                    view.m_btn_buy.gameObject.gameObject.SetActive(true);
                    view.m_btn_stock.gameObject.gameObject.SetActive(false);
                    var value1 = ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.guildPoint);
                    view.m_lbl_num1_LanguageText.text = value1;
                    view.m_bg.m_lbl_title_LanguageText.text = LanguageUtils.getText(730253);
                    break;
                case EnumAllianceStorePageType.Stock:
                    view.m_pl_title1.gameObject.SetActive(false);
                    view.m_pl_title2.gameObject.SetActive(true);

                    view.m_btn_buy.gameObject.gameObject.SetActive(false);
                    view.m_btn_stock.gameObject.gameObject.SetActive(true);
                    var value2 = ClientUtils.FormatComma(m_allianceProxy.GetDepotCurrency_AlliancePointNum());
                    view.m_lbl_num2_LanguageText.text = value2;
                    view.m_bg.m_lbl_title_LanguageText.text = LanguageUtils.getText(733111);
                    break;
            }
        }
        #region 联盟商店列表数据产出

        private readonly int MaxCol = 4;
        private readonly GuildStoreListViewData m_EmptyTextListDataCache = new GuildStoreListViewData
        {
            type = EnumGuildStoreListViewDataType.EmptyText, txt = LanguageUtils.getText(730254)
        };

        private List<GuildStoreListViewData> m_TitleListDataCache;
        private List<GuildStoreListViewData> GetTitleListData(List<AllianceShopItemDefine> lstConfig)
        {
            if (m_TitleListDataCache != null)
            {
                return m_TitleListDataCache;
            }

            m_TitleListDataCache = new List<GuildStoreListViewData>();
            var temp_GroupType = 0;
            foreach (var config in lstConfig)
            {
                //增加抬头
                if (config.@group != temp_GroupType)
                {
                    temp_GroupType = config.@group;
                    if (GroupTypeName.Length < temp_GroupType)
                    {
                        Debug.LogError("GuildStore not contain GroupTypeName:" + temp_GroupType);
                        break;
                    }

                    var title = new GuildStoreListViewData();
                    title.type = EnumGuildStoreListViewDataType.Title;
                    title.txt = LanguageUtils.getText(GroupTypeName[temp_GroupType - 1]);
                    m_TitleListDataCache.Add(title);
                }
            }
            return m_TitleListDataCache;
        }
        private List<GuildStoreListViewData> GetListData()
        {
            var ret = new List<GuildStoreListViewData>();
            var lstConfig = CoreUtils.dataService.QueryRecords<AllianceShopItemDefine>();

            Dictionary<int, List<GuildStoreListViewData>> dicItemLine = new Dictionary<int, List<GuildStoreListViewData>>();

            foreach (var config in lstConfig)
            {
                var count = m_allianceProxy.GetStoreItemCountByItemType(config.itemType);// GetServerCountById

                if ((count > 0 && m_curPageType == EnumAllianceStorePageType.Shoping) || (m_curPageType == EnumAllianceStorePageType.Stock))
                {
                    var groupType = config.@group;
                    if (!dicItemLine.ContainsKey(groupType))
                    {
                        if (GroupTypeName.Length < groupType)
                        {
                            Debug.LogError("GuildStore not contain GroupTypeName:" + groupType);
                            break;
                        }
                        dicItemLine[groupType] = new List<GuildStoreListViewData>();
                    }
                    if (dicItemLine[groupType].Count <= 0 || dicItemLine[groupType][dicItemLine[groupType].Count - 1].lstId.Count == MaxCol)
                    {
                        var itemLine = new GuildStoreListViewData();
                        itemLine.type = EnumGuildStoreListViewDataType.ItemLine;
                        itemLine.lstId = new List<int>();
                        itemLine.lstNum = new List<long>();
                        dicItemLine[groupType].Add(itemLine);
                    }

                    var curLastItem = dicItemLine[groupType][dicItemLine[groupType].Count - 1];
                    curLastItem.lstId.Add(config.itemType);
                    curLastItem.lstNum.Add(count);
                }
            }

            var titleList = GetTitleListData(lstConfig);
            for (int i = 0; i < titleList.Count; i++)
            {
                ret.Add(titleList[i]);
                if (!dicItemLine.ContainsKey(i + 1))
                {
                    ret.Add(m_EmptyTextListDataCache);
                }
                else
                {
                    foreach (var line in dicItemLine[i + 1])
                    {
                        ret.Add(line);
                    }
                }
            }
            return ret;
        }
        #endregion
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            InitList();
        }
        private void InitList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ItemEnter;
            functab.GetItemPrefabName = GetItemPrefabName;
            functab.GetItemSize = GetItemSize;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            RefreshItemList();
        }
        private float GetItemSize(ListView.ListItem listItem)
        {
            var name = GetItemPrefabName(listItem);
            return m_assetDic[name].GetComponent<RectTransform>().sizeDelta.y;
        }
        private string GetItemPrefabName(ListView.ListItem listItem)
        {
            var index = listItem.index;
            var viewData = m_ListViewData[index];
            switch (viewData.type)
            {
                case EnumGuildStoreListViewDataType.Title:
                    return "UI_Item_GuildStoreLine";
                case EnumGuildStoreListViewDataType.EmptyText:
                    return "UI_Item_GuildStoreEmpty";
                case EnumGuildStoreListViewDataType.ItemLine:
                    return "UI_Item_GuildStore";
                default:
                    return "UI_Item_GuildStoreLine";
            }
        }
        private void ItemEnter(ListView.ListItem listItem)
        {
            var index = listItem.index;
            var viewData = m_ListViewData[index];
            switch (viewData.type)
            {
                case EnumGuildStoreListViewDataType.Title:
                    UI_Item_GuildStoreLineView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_GuildStoreLineView>(listItem.go);
                    itemView.m_lbl_Text_LanguageText.text = viewData.txt;
                    break;
                case EnumGuildStoreListViewDataType.EmptyText:
                    UI_Item_GuildStoreEmptyView itemView2 = MonoHelper.AddHotFixViewComponent<UI_Item_GuildStoreEmptyView>(listItem.go);
                    itemView2.m_lbl_Text_LanguageText.text = viewData.txt;
                    break;
                case EnumGuildStoreListViewDataType.ItemLine:
                    UI_Item_GuildStoreView itemView3 = MonoHelper.AddHotFixViewComponent<UI_Item_GuildStoreView>(listItem.go);
                    UI_Model_Item_SubView[] itemSubViews =
                    {
                        itemView3.m_UI_Model_Item1, itemView3.m_UI_Model_Item2, itemView3.m_UI_Model_Item3,
                        itemView3.m_UI_Model_Item4
                    };
                    for (int i = 0; i < itemSubViews.Length; i++)
                    {
                        var hasContent = viewData.lstId.Count >= i + 1;
                        itemSubViews[i].gameObject.SetActive(hasContent);
                        if (hasContent)
                        {
                            var itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(viewData.lstId[i]);
                            itemSubViews[i].Refresh(itemConfig, viewData.lstNum[i]);
                            itemSubViews[i].m_lbl_count_LanguageText.gameObject.SetActive(m_curPageType == EnumAllianceStorePageType.Shoping);
                            itemSubViews[i].RemoveBtnAllListener();
                            var temp_i = i;
                            if (CurSelectedItemId == -1)
                            {
                                CurSelectedItemId = viewData.lstId[temp_i];
                            }

                            if (CurSelectedItemId == viewData.lstId[temp_i])
                            {
                                m_CurSelectedItemView = itemSubViews[temp_i];
                                m_CurSelectedItemView.SetSelectImgActive(true);
                            }
                            itemSubViews[i].AddBtnListener(() =>
                            {
                                CurSelectedItemId = viewData.lstId[temp_i];
                                m_CurSelectedItemView?.SetSelectImgActive(false);
                                m_CurSelectedItemView = itemSubViews[temp_i];
                                m_CurSelectedItemView.SetSelectImgActive(true);
                            });
                        }
                    }
                    break;

            }
        }

        private void RefreshItemList()
        {
            if (m_assetDic.Count <= 0)
                return;


            m_ListViewData = GetListData();

            //检查CurSelectedItemId是否还在
            var isFindCur = false;
            foreach (var line in m_ListViewData)
            {
                if (line.lstId == null || line.lstId.Count <= 0)
                {
                    continue;
                }
                foreach (var id in line.lstId)
                {
                    if (CurSelectedItemId == -1)
                    {
                        CurSelectedItemId = id;
                        isFindCur = true;
                        break;
                    }
                    if (CurSelectedItemId == id)
                    {
                        isFindCur = true;
                        break;
                    }
                }
                if (isFindCur)
                    break;
            }
            if (!isFindCur)
            {
                CurSelectedItemId = -1;
            }

            view.m_sv_list_ListView.FillContent(m_ListViewData.Count);
            view.m_sv_list_ListView.ForceRefresh();

            if (CurSelectedItemId == -1)
            {
                m_EditNumView.SetDetailGray(true);
            }
            else
            {
                m_EditNumView.SetDetailGray(false);
                m_EditNumView.Reset();
            }
        }



        private void OnClickBtnBuyCallback(int id, int num)
        {
            var itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(id);
            if (itemConfig == null)
                return;
            if (num == 1)
            {
                Tip.CreateTip(LanguageUtils.getTextFormat(300073, LanguageUtils.getText(itemConfig.l_nameID))).Show();
            }
            else
            {
                Tip.CreateTip(LanguageUtils.getTextFormat(733104, LanguageUtils.getText(itemConfig.l_nameID), num)).Show();
            }
            var globalEffectMediator = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            RectTransform startRect = null;
            if (m_CurSelectedItemView != null)
                startRect = m_CurSelectedItemView.m_root_RectTransform;
            else
            {
                startRect = view.m_btn_buy.m_root_RectTransform;
            }
            if (startRect != null)
                globalEffectMediator.FlyItemEffect(id, num, startRect);
        }
        private void OnClickBtnBuy()
        {
            if (CurSelectedItemId == -1)
            {
                return;
            }
            var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (!allianceProxy.HasJionAlliance())
            {
                Tip.CreateTip(732029, Tip.TipStyle.Middle).Show();
                return;
            }
            var guildStoreConfig = CoreUtils.dataService.QueryRecord<Data.AllianceShopItemDefine>(CurSelectedItemId);
            var price = guildStoreConfig.sellingPrice * m_EditNumView.CurNum;
            var point = m_playerProxy.CurrentRoleInfo.guildPoint;// 获得联盟个人积分
            if (point < price)
            {
                Tip.CreateTip(733101, Tip.TipStyle.Middle).Show();
                return;
            }

            var serverRemainCount = allianceProxy.GetStoreItemCountByItemType(CurSelectedItemId);
            var editNum = m_EditNumView.CurNum;
            if (serverRemainCount < editNum)
            {
                Tip.CreateTip(733102, Tip.TipStyle.Middle).Show();
                return;
            }
            //判断是否提醒
            GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;

            bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.AllianceShopPurchaseConfirmation);

            if (isRemind)
            {
                var itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(guildStoreConfig.itemType);

                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                string currencyiconId = currencyProxy.GeticonIdByType(106);
                if (itemConfig != null  )
                {
                    SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.AllianceShopPurchaseConfirmation);
                    if (settingPersonalityDefine != null)
                    {
                        string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                        string str = LanguageUtils.getTextFormat(733122, price.ToString("N0"), m_EditNumView.CurNum.ToString("N0"), LanguageUtils.getText(itemConfig.l_nameID));
                        Alert.CreateAlert(str, LanguageUtils.getText(300075)).
                                          SetLeftButton().
                                          SetRightButton().
                                          SetCurrencyRemind((isBool) =>
                                          {
                                              if (isBool)
                                              {
                                                  generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.AllianceShopPurchaseConfirmation);
                                              }

                                              SendShopBuy(CurSelectedItemId, m_EditNumView.CurNum);
                                          }, (int)price, s_remind, currencyiconId).Show();

                    }       
                }
            }
            else
            {
                SendShopBuy(CurSelectedItemId, m_EditNumView.CurNum);
            }
        }

        private void OnClickBtnStockCallback(int id, int num)
        {
            var itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(id);
            if (itemConfig == null)
                return;
            Tip.CreateTip(LanguageUtils.getTextFormat(733109, LanguageUtils.getText(itemConfig.l_nameID), num)).Show();
        }
        private void OnClickBtnStock()
        {
            if (CurSelectedItemId == -1)
            {
                return;
            }
            var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (!allianceProxy.HasJionAlliance())
            {
                Tip.CreateTip(732029, Tip.TipStyle.Middle).Show();
                return;
            }

            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (allianceProxy.getMemberInfo(playerProxy.Rid).guildJob < 4)
            {
                Tip.CreateTip(730136, Tip.TipStyle.Middle).Show();
                return;
            }

            var guildStoreConfig = CoreUtils.dataService.QueryRecord<Data.AllianceShopItemDefine>(CurSelectedItemId);
            var price = guildStoreConfig.stockPrice * m_EditNumView.CurNum;
            var point = m_allianceProxy.GetDepotCurrency_AlliancePointNum();
            if (point < price)
            {
                Tip.CreateTip(732015, Tip.TipStyle.Middle).Show();
                return;
            }
            //判断是否提醒
            GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.AllianceShopReplenishConfirmation);
            
            if (isRemind)
            {
                var itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(guildStoreConfig.itemType);
                SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.AllianceShopReplenishConfirmation);

                string currencyiconId = currencyProxy.GeticonIdByType(107);
                if (itemConfig != null&& settingPersonalityDefine!=null)
                {
                    string str = LanguageUtils.getTextFormat(733123, price.ToString("N0"), m_EditNumView.CurNum.ToString("N0"), LanguageUtils.getText(itemConfig.l_nameID));
                    string s_remind =  settingPersonalityDefine.resetTiem==0 ? LanguageUtils.getText( 300071) : LanguageUtils.getText(300294);

                    Alert.CreateAlert(str, LanguageUtils.getText(300075)).
                                      SetLeftButton().
                                      SetRightButton().
                                      SetCurrencyRemind((isBool) =>
                                      {
                                          if (isBool)
                                          {
                                              generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.AllianceShopReplenishConfirmation);
                                          }
                                          // Send补充
                                          SendShopStock(CurSelectedItemId, m_EditNumView.CurNum);
                                      }, (int)price, s_remind,currencyiconId).Show();
                }
            }
            else
            {
                SendShopStock(CurSelectedItemId, m_EditNumView.CurNum);
            }
        }
        
        private void SendShopStock(int idItemType, int nCount)
        {
            Debug.Log("Send联盟商店补充");
            Guild_ShopStock.request req = new Guild_ShopStock.request();
            req.idItemType = idItemType;
            req.nCount = nCount;
            AppFacade.GetInstance().SendSproto(req);
        }
        private void SendShopBuy(int idItemType, int nCount)
        {
            // Send购买
            Debug.Log("Send联盟商店购买");
            //            OnClickBtnBuyCallback(CurSelectedItemId,m_EditNumView.CurNum);

            Guild_ShopBuy.request req = new Guild_ShopBuy.request();
            req.idItemType = idItemType;
            req.nCount = nCount;
            AppFacade.GetInstance().SendSproto(req);
        }

        private void SendReqShopItems()
        {
            Guild_ShopQuery.request req = new Guild_ShopQuery.request();
            AppFacade.GetInstance().SendSproto(req);
        }

        private void OnSendReqShopItemsCallback(Guild_ShopQuery.response res)
        {
            m_allianceProxy.SetStoreItems(res);
            RefreshAllView();
        }

        private void TipError(INotification notification)
        {
            ErrorMessage error = (ErrorMessage)notification.Body;
            if (error.HasErrorCode)
            {
                switch ((ErrorCode)error.errorCode)
                {
                    case ErrorCode.GUILD_INDIVIDUAL_POINT://12072, 联盟个人积分不足
                        Tip.CreateTip(733101, Tip.TipStyle.Middle).Show();
                        break;
                    case ErrorCode.GUILD_SHOP_ITEM_NOT_EXIST://12073, 联盟商店商品不存在
//                        Tip.CreateTip(733102, Tip.TipStyle.Middle).Show();
                        break;
                    case ErrorCode.GUILD_SHOP_ITEM_NOT_ENOUGH://12074, 联盟商店商品数量不足
                        Tip.CreateTip(733102, Tip.TipStyle.Middle).Show();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 这里简单封装   未来可以抽取为公用SubView
    /// </summary>
    public class ItemEditNumView
    {
        private UI_Win_GuildStoreView view;
        private int m_curNum;
        private Action<int> m_OnCurNumChangeCallback;

        public int CurNum
        {
            get => m_curNum;
            set
            {
                m_curNum = value;
                RefreshBtn();
                m_OnCurNumChangeCallback?.Invoke(value);
            } 
        }

        private int m_Max;
        private int m_Min;
        private int m_overlayLimit = 9999;
        private Dictionary<int, string> m_qualityDic = new Dictionary<int, string>();
        private Vector3 m_zeroVec = Vector3.zero;
        private Vector3 m_oneVec = Vector3.one;

        private float m_detailTextHeight;
        private float m_detailTextOffsetY;
        private float m_listDefaultHeight;
        public ItemEditNumView(UI_Win_GuildStoreView pView ,Action<int> onCurNumChangeCallback)
        {
            view = pView;
            m_OnCurNumChangeCallback = onCurNumChangeCallback;
            m_qualityDic[1] = RS.ItemQualityBg[0];
            m_qualityDic[2] = RS.ItemQualityBg[1];
            m_qualityDic[3] = RS.ItemQualityBg[2];
            m_qualityDic[4] = RS.ItemQualityBg[3];
            m_qualityDic[5] = RS.ItemQualityBg[4];
            
            RectTransform detailRect = view.m_lbl_detail_LanguageText.GetComponent<RectTransform>();
            m_detailTextHeight = detailRect.sizeDelta.y;
            m_detailTextOffsetY = detailRect.anchoredPosition.y;
            m_listDefaultHeight = view.m_sv_list_detail_ListView.GetComponent<RectTransform>().rect.height;
            
            SetIptText(1);
            view.m_ipt_num_GameInput.onValueChanged.RemoveAllListeners();
            view.m_ipt_num_GameInput.onValueChanged.AddListener(OnInputChange);
            view.m_ipt_num_GameInput.onEndEdit.AddListener(OnInputChange);
            view.m_btn_add_GameButton.onClick.RemoveAllListeners();
            view.m_btn_add_GameButton.onClick.AddListener(OnAdd);
            view.m_btn_substract_GameButton.onClick.RemoveAllListeners();
            view.m_btn_substract_GameButton.onClick.AddListener(OnSubstract);
        }
        public void SetDetailGray(bool isGray)
        {
            if (isGray)
            {
                var noText = "-";
                view.m_lbl_desc_LanguageText.text = string.Empty;
                view.m_lbl_item_name_LanguageText.text = noText;
                view.m_lbl_detail_LanguageText.text = noText;
                view.m_lbl_num_LanguageText.text = noText;
                view.m_ipt_num_GameInput.gameObject.SetActive(false);
                view.m_img_item_PolygonImage.gameObject.SetActive(false);
                ClientUtils.LoadSprite(view.m_img_item_quality_PolygonImage, RS.ItemQualityBg[0]);
                view.m_btn_buy.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false);
                view.m_btn_stock.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false);
                view.m_img_add_gray_PolygonImage.gameObject.SetActive(true);
                view.m_img_add_normal_PolygonImage.gameObject.SetActive(false);
                view.m_img_substract_gray_PolygonImage.gameObject.SetActive(true);
                view.m_img_substract_normal_PolygonImage.gameObject.SetActive(false);
                view.m_btn_add_GameButton.interactable = false;
                view.m_btn_substract_GameButton.interactable = false;
            }
            else
            {
                view.m_ipt_num_GameInput.gameObject.SetActive(true);
                view.m_img_item_PolygonImage.gameObject.SetActive(true);
                view.m_btn_buy.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                view.m_btn_stock.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                view.m_img_add_gray_PolygonImage.gameObject.SetActive(false);
                view.m_img_add_normal_PolygonImage.gameObject.SetActive(true);
                view.m_img_substract_gray_PolygonImage.gameObject.SetActive(false);
                view.m_img_substract_normal_PolygonImage.gameObject.SetActive(true);
                view.m_btn_add_GameButton.interactable = true;
                view.m_btn_substract_GameButton.interactable = true;
            }
            
            view.m_btn_buy.SetGray(isGray);
        }
        public void Reset()
        {
            CurNum = 1;
            SetIptText(1);
        }
        /// <summary>
        /// 设置大道具图标相关 图标、名字、图标抬头、描述
        /// </summary>
        /// <param name="Id"></param>
        public void SetItemById(int id)
        {
            SetDetailGray(false);
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)id);
            //设置品质图片
            ClientUtils.LoadSprite(view.m_img_item_quality_PolygonImage, GetQualityImg(itemDefine.quality));
            //设置icon
            ClientUtils.LoadSprite(view.m_img_item_PolygonImage, itemDefine.itemIcon);

            if (itemDefine.l_topID < 1)
            {
                view.m_pl_desc_bg_PolygonImage.transform.localScale = m_zeroVec;
            }
            else
            {
                view.m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
                view.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), ClientUtils.FormatComma(itemDefine.topData));
            }
            //名称
            view.m_lbl_item_name_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);

            //详情
            view.m_lbl_detail_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_desID), ClientUtils.FormatComma(itemDefine.desData1), ClientUtils.FormatComma(itemDefine.desData2));
            RectTransform listRect = view.m_sv_list_detail_ScrollRect.GetComponent<RectTransform>();
            RectTransform textRect = view.m_lbl_detail_LanguageText.GetComponent<RectTransform>();
            float textHeight = view.m_lbl_detail_LanguageText.preferredHeight - m_detailTextOffsetY;

            if (textHeight > m_listDefaultHeight)
            {
                view.m_c_list_detail.sizeDelta = new Vector2(view.m_c_list_detail.sizeDelta.x, textHeight);
                textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, textRect.anchoredPosition.y-(view.m_lbl_detail_LanguageText.preferredHeight-m_detailTextHeight)/2);
            }
            else
            {
                view.m_c_list_detail.sizeDelta = new Vector2(view.m_c_list_detail.sizeDelta.x, m_listDefaultHeight);
                textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, m_detailTextOffsetY);
            }
        }
        //获取品质图标
        private string GetQualityImg(int quality)
        {
            if(m_qualityDic.ContainsKey(quality))
            {
                return m_qualityDic[quality];
            }
            return "";
        }
        /// <summary>
        /// 设置可编辑的最大最小值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetMinAndMax(int min , int max)
        {
            m_Min = min;
            m_Max = max;
            SetIptText(CurNum);
        }

        public void RefreshBtn()
        {
            view.m_img_add_gray_PolygonImage.gameObject.SetActive(CurNum >= m_Max);
            view.m_img_add_normal_PolygonImage.gameObject.SetActive(CurNum < m_Max);
            view.m_img_substract_gray_PolygonImage.gameObject.SetActive(CurNum <= m_Min);
            view.m_img_substract_normal_PolygonImage.gameObject.SetActive(CurNum > m_Min);
        }
        private int GetInputNum()
        {
            string inputStr = view.m_ipt_num_GameInput.text;
            int num = int.Parse(inputStr);
            return num;
        }
        private void OnAdd()
        {
            int num = GetInputNum();
            if (num >= m_Max)
            {
                return;
            }
            num = num + 1;
            SetIptText(num);
        }

        private void OnSubstract()
        {
            int num = GetInputNum();
            if (num <= 1)
            {
                return;
            }
            num = num - 1;
            SetIptText(num);
        }
        private void SetIptText(int num)
        {
            num = Mathf.Min(m_Max, Mathf.Max(m_Min, num));
            view.m_ipt_num_GameInput.text = num.ToString();
            CurNum = num;
            SetIptFormatText(num);
        }

        private void SetIptFormatText(int num)
        {
            view.m_lbl_ipt_format_LanguageText.text = ClientUtils.FormatComma(num);
        }
        //输入框变更
        private void OnInputChange(string val)
        {
            if (val.Length < 1)
            {
                SetIptText(1);
                return;
            }
            int num = 0;
            bool isChange = false;
            if (val.Length < 1)
            {
                num = 1;
                isChange = true;
            }
            else
            {
                try
                {
                    num = int.Parse(val);
                }
                catch
                {
                    num = 1;
                    isChange = true;
                }
                if (val.Length != num.ToString().Length)
                {
                    SetIptText(num);
                    return;
                }
            }
            if (num < 1)
            {
                num = 1;
                isChange = true;
            }
            else if (num > m_overlayLimit)
            {
                num = m_overlayLimit;
                isChange = true;
            }
            if (isChange)
            {
                SetIptText(num);
                return;
            }
            SetIptText(num);
        }
    }
}