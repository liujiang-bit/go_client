// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月3日
// Update Time         :    2020年4月3日
// Class Description   :    StoreMediator 商店
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
using UnityEngine.UI;

namespace Game {
    public class StoreMediator : GameMediator {
        #region Member
        public static string NameMediator = "StoreMediator";

        private StoreProxy m_storeProxy;
        private BagProxy m_bagProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;

        private Dictionary<int, List<int>> m_shopData;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private int m_itemCol = 4;
        private int m_itemLineCount = 0;
        private int m_currType = 0;

        private Dictionary<int, int> m_selectDic = new Dictionary<int, int>();
        private ItemBagView m_selectItem;

        private Vector3 m_zeroVec = Vector3.zero;
        private Vector3 m_oneVec = Vector3.one;
        private float m_detailTextHeight;
        private float m_detailTextOffsetY;

        private int m_overlayLimit = 9999;
        private ItemDefine m_selectDefine;

        private bool m_isDispose = false;
        private bool m_isForceSwitch = false;
        #endregion

        //IMediatorPlug needs
        public StoreMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public StoreView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Shop_BuyShopItem.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Shop_BuyShopItem.TagName:
                    var result = notification.Body as Shop_BuyShopItem.response;
                    if (result.itemNum <= 0)
                    {
                        return;
                    }
                    //刷新拥有的物品数量
                    RefreshOwnNum();

                    //tip显示
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)result.itemId);
                    if (itemDefine == null)
                    {
                        return;
                    }
                    string str = LanguageUtils.getTextFormat(200030, 
                                                             LanguageUtils.getText(itemDefine.l_nameID), 
                                                             ClientUtils.FormatComma(result.itemNum));
                    str = LanguageUtils.getTextFormat(300073, str);
                    Tip.CreateTip(str).Show();

                    //飘飞特效
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyItemEffect((int)result.itemId, (int)result.itemNum, 
                                     view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.GetComponent<RectTransform>());

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

        public override void OnRemove()
        {
            base.OnRemove();
            m_isDispose = true;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_storeProxy = AppFacade.GetInstance().RetrieveProxy(StoreProxy.ProxyNAME) as StoreProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            m_shopData = m_storeProxy.GetShopData();

            m_selectDic[1] = 0;
            m_selectDic[2] = 0;
            m_selectDic[3] = 0;
            m_selectDic[5] = 0;

            RectTransform detailRect = view.m_lbl_detail_LanguageText.GetComponent<RectTransform>();
            m_detailTextHeight = detailRect.sizeDelta.y;
            m_detailTextOffsetY = detailRect.anchoredPosition.y;

            view.gameObject.SetActive(false);

            List<string> prefabNames = new List<string>();
            prefabNames.Add("UI_Item_Bag");
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(OnBuy);
            view.m_btn_substract_GameButton.onClick.AddListener(OnSubstract);
            view.m_btn_add_GameButton.onClick.AddListener(OnAdd);
            view.m_ipt_num_GameInput.onValueChanged.AddListener(OnInputChange);

            view.m_ck_res_GameToggle.onValueChanged.AddListener(OnMenuRes);
            view.m_ck_speed_GameToggle.onValueChanged.AddListener(OnMenuSpeed);
            view.m_ck_gain_GameToggle.onValueChanged.AddListener(OnMenuGain);
            view.m_ck_other_GameToggle.onValueChanged.AddListener(OnMenuOther);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            view.m_pl_mes_Image.gameObject.SetActive(true);

            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            view.gameObject.SetActive(true);

            InitList();

            m_currType = 1;
            SetSelectMenu();
        }


        //初始化list
        private void InitList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            BagColElement itemColScript = null;
            if (listItem.isInit == false)
            {
                itemColScript = MonoHelper.AddHotFixViewComponent<BagColElement>(listItem.go);
                listItem.isInit = true;
                for (int i = 0; i < m_itemCol; i++)
                {
                    GameObject itemObj = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_Bag"]);

                    itemObj.transform.SetParent(listItem.go.transform);
                    itemObj.transform.localPosition = Vector3.zero;
                    itemObj.transform.localScale = Vector3.one;
                    ItemBagView itemView = MonoHelper.AddHotFixViewComponent<ItemBagView>(itemObj.gameObject);
                    itemView.m_UI_Model_Item.SetSelectImgActive(false);
                    itemColScript.ElemItemList.Add(itemView);
                    itemColScript.ElemIndexList.Add(-1);

                    int num = i;
                    itemView.m_UI_Model_Item.AddBtnListener(() => {
                        ClickItem(itemColScript.ElemItemList[num], itemColScript.ElemIndexList[num]);
                    });
                }
            }
            else
            {
                itemColScript = MonoHelper.GetOrAddHotFixViewComponent<BagColElement>(listItem.go);
            }

            ItemBagView nodeView;
            int min = listItem.index * m_itemCol;
            int max = min + (m_itemCol - 1);
            int tnum = -1;
            for (int i = min; i <= max; i++)
            {
                tnum = i - min;
                nodeView = itemColScript.ElemItemList[tnum];
                itemColScript.ElemIndexList[tnum] = i;
                if (i >= m_shopData[m_currType].Count)
                {
                    nodeView.gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    nodeView.gameObject.SetActive(true);
                }
                RefreshItem(nodeView, i);
            }
        }

        //刷新item
        private void RefreshItem(ItemBagView itemView, int index)
        {
            int itemId = m_shopData[m_currType][index];

            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemId);
            if (itemDefine == null)
            {
                Debug.LogWarningFormat("not find itemId:{0}", itemId);
                return;
            }

            if (m_selectDic[m_currType] == index)
            {
                itemView.m_UI_Model_Item.Refresh(itemDefine, 0, true);
                m_selectItem = itemView;
            }
            else
            {
                itemView.m_UI_Model_Item.Refresh(itemDefine, 0, false);
            }
        }

        private void SetSelectMenu()
        {
            m_isForceSwitch = true;
            if (m_currType == 1)
            {
                view.m_ck_res_GameToggle.isOn = true;
            }
            else if (m_currType == 2)
            {
                view.m_ck_speed_GameToggle.isOn = true;
            }
            else if (m_currType == 3)
            {
                view.m_ck_gain_GameToggle.isOn = true;
            }
            else if (m_currType == 5)
            {
                view.m_ck_other_GameToggle.isOn = true;
            }
        }

        //资源
        private void OnMenuRes(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(1);
            }
        }

        //加速
        private void OnMenuSpeed(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(2);
            }
        }

        //增益
        private void OnMenuGain(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(3);
            }
        }

        //其它
        private void OnMenuOther(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(5);
            }
        }

        private void SwitchMenu(int type)
        {
            if (!m_isForceSwitch)
            {
                if (type == m_currType)
                {
                    return;
                }
            }
            m_isForceSwitch = false;

            m_currType = type;
            SwitchMenuRefresh();
        }

        private void SwitchMenuRefresh()
        {
            RefreshContent(true);
        }

        private void RefreshContent(bool isRefreshListPos)
        {
            if (m_shopData[m_currType].Count <= 0)
            {
                view.m_pl_list_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_pl_detail_PolygonImage.gameObject.SetActive(false);
                view.m_lbl_no_item_LanguageText.gameObject.SetActive(true);
            }
            else
            {
                view.m_pl_list_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_pl_detail_PolygonImage.gameObject.SetActive(true);
                view.m_lbl_no_item_LanguageText.gameObject.SetActive(false);

                RefreshBagList(isRefreshListPos);
            }
            RefreshItemDetail();
        }

        private void RefreshBagList(bool isForceRefreshPos = true)
        {
            if (isForceRefreshPos)
            {
                view.m_sv_list_view_ListView.RefreshAndRestPos();
            }
            m_itemLineCount = (int)Math.Ceiling((float)m_shopData[m_currType].Count / m_itemCol);
            view.m_sv_list_view_ListView.FillContent(m_itemLineCount);
        }

        #region 详情
        //刷新详情
        private void RefreshItemDetail()
        {
            int index = m_selectDic[m_currType];
            int itemId= m_shopData[m_currType][index];
   
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemId);
            m_selectDefine = itemDefine;
            //设置品质图片
            ClientUtils.LoadSprite(view.m_img_item_quality_PolygonImage, RS.ItemQualityBg[itemDefine.quality-1]);
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
            if (textHeight > listRect.sizeDelta.y)
            {
                view.m_c_list_detail.sizeDelta = new Vector2(view.m_c_list_detail.sizeDelta.x, textHeight);
                textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, textRect.anchoredPosition.y - (view.m_lbl_detail_LanguageText.preferredHeight - m_detailTextHeight) / 2);
            }
            else
            {
                view.m_c_list_detail.sizeDelta = new Vector2(view.m_c_list_detail.sizeDelta.x, listRect.sizeDelta.y);
                textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, m_detailTextOffsetY);
            }
            RefreshOwnNum();

            //刷新ipt
            view.m_pl_input_ArabLayoutCompment.gameObject.SetActive(true);          
            SetIptText(1);
            view.m_img_substract_gray_PolygonImage.gameObject.SetActive(true);
            view.m_img_substract_normal_PolygonImage.gameObject.SetActive(false);
            view.m_img_add_gray_PolygonImage.gameObject.SetActive(false);
            view.m_img_add_normal_PolygonImage.gameObject.SetActive(true);
        }

        private void RefreshOwnNum()
        {
            if (m_selectDefine == null)
            {
                return;
            }
            view.m_lbl_own_num_LanguageText.text = string.Format(LanguageUtils.getText(300074), ClientUtils.FormatComma(m_bagProxy.GetItemNum(m_selectDefine.ID)));
        }

        private void RefreshPrice(int num)
        {
            if (m_selectDefine == null)
            {
                return;
            }
            view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(num * m_selectDefine.shopPrice);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        #endregion

        private void ClickItem(ItemBagView itemView, int index)
        {
            if (m_selectItem != null)
            {
                m_selectItem.m_UI_Model_Item.SetSelectImgActive(false);
            }
            m_selectDic[m_currType] = index;
            m_selectItem = itemView;
            m_selectItem.m_UI_Model_Item.SetSelectImgActive(true);
            RefreshItemDetail();
        }

        private int GetInputNum()
        {
            string inputStr = view.m_ipt_num_GameInput.text;
            int num = int.Parse(inputStr);
            return num;
        }

        //减少
        private void OnSubstract()
        {
            int num = GetInputNum();
            if (num <= 1)
            {
                return;
            }
            view.m_img_add_gray_PolygonImage.gameObject.SetActive(false);
            view.m_img_add_normal_PolygonImage.gameObject.SetActive(true);
            num = num - 1;
            if (num == 1)
            {
                view.m_img_substract_gray_PolygonImage.gameObject.SetActive(true);
                view.m_img_substract_normal_PolygonImage.gameObject.SetActive(false);
            }
            SetIptText(num);
        }

        private void SetIptText(int num)
        {
            view.m_ipt_num_GameInput.text = num.ToString();
            SetIptFormatText(num);
        }

        private void SetIptFormatText(int num)
        {
            view.m_lbl_ipt_format_LanguageText.text = ClientUtils.FormatComma(num);
            RefreshPrice(num);
        }

        //增加
        private void OnAdd()
        {
            int num = GetInputNum();
            int index = m_selectDic[m_currType];
            int itemId = m_shopData[m_currType][index];
            if (num >= 999)
            {
                return;
            }
            num = num + 1;
            SetIptText(num);
        }

        //最大
        private void OnMax()
        {
            SetIptText(m_overlayLimit);
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
            SetIptFormatText(num);
            view.m_img_add_gray_PolygonImage.gameObject.SetActive(num == m_overlayLimit);
            view.m_img_add_normal_PolygonImage.gameObject.SetActive(!(num == m_overlayLimit));
            view.m_img_substract_gray_PolygonImage.gameObject.SetActive(num == 1);
            view.m_img_substract_normal_PolygonImage.gameObject.SetActive(!(num == 1));
        }

        //购买
        private void OnBuy()
        {
            int num = GetInputNum();
            Int64 ownNum = m_playerProxy.GetResNumByType((int)EnumCurrencyType.denar);
            Int64 cost = m_selectDefine.shopPrice * num;

            //宝石是否足够
            if (m_currencyProxy.ShortOfDenar(cost))
            {
                return;
            }

            //判断是否提醒
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
                    string str = LanguageUtils.getTextFormat(300072, cost);
                    Alert.CreateAlert(str, LanguageUtils.getText(300075)).
                                      SetLeftButton().
                                      SetRightButton().
                                      SetCurrencyRemind((isBool) =>
                                      {
                                          if (isBool)
                                          {
                                              generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                                          }
                                          Buy(num);
                                      },
                                      (int)cost, s_remind, currencyiconId).
                                      Show();
                }
                return;
            }
            Buy(num);
        }

        private void Buy(int num)
        {
            var sp = new Shop_BuyShopItem.request();
            sp.itemId = m_selectDefine.ID;
            sp.itemNum = num;
            AppFacade.GetInstance().SendSproto(sp);
        }
    }
}