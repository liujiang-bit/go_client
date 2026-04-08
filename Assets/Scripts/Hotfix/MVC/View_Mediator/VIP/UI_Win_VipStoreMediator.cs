// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Win_VipStoreMediator
// Copyright IGG All rights reserved.
// ===============================================================================

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
    public class UI_Win_VipStoreMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_VipStoreMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;
        private Timer m_refreshTimer;
        private List<VipStoreDefine> m_storeItemInfo = new List<VipStoreDefine>();
        private int m_curQuickBuyItemID = 0;
        private int m_curVipLevel = -1;
        private bool m_clickDown = false;
        private Transform m_clickBtn = null;
        #endregion

        //IMediatorPlug needs
        public UI_Win_VipStoreMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
            IsOpenUpdate = true;
        }


        public UI_Win_VipStoreView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.VipPointChange,
                CmdConstant.VipStoreInfoChange,
                CmdConstant.UpdateCurrency,
                Shop_BuyVipStore.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.VipPointChange:
                    OnVIPPointChange();
                    break;
                case CmdConstant.VipStoreInfoChange:
                    Dictionary<long,VipStore> data = notification.Body as Dictionary<long,VipStore>;
                    if (data == null)
                    {
                        return;
                    }
                    OnBuyVIPStoreSuccess(data);
                    break;
                case CmdConstant.UpdateCurrency:
                    ShowQuickBuy(m_curQuickBuyItemID);
                    break;
                case Shop_BuyVipStore.TagName:
                    if (m_clickBtn == null) return;
                    
                    Shop_BuyVipStore.response response = notification.Body as Shop_BuyVipStore.response;
                    if (response != null && response.HasItemId)
                    {

                        var globalEffectMediator =
                            AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as
                                GlobalEffectMediator;
                        globalEffectMediator.FlyItemEffect((int)response.itemId, (int)response.itemNum,m_clickBtn.GetComponent<RectTransform>());
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

        public override void WinClose(){
            ClearTimer();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            if (m_clickDown&& CoreUtils.inputManager.GetTouchCount() == 0)
            {
                m_clickDown = false;
                CloseQuickBuy();
            }
            if (!m_clickDown && m_curQuickBuyItemID != 0 && CoreUtils.inputManager.GetTouchCount() > 0)
            {
                m_clickDown = true;
            }
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, LoadStoreItemFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_vipBarAdd_GameButton.onClick.AddListener(OnClickAddVIPPoint);
            view.m_UI_Model_Window_Type2.setCloseHandle(OnClickClose);
        }

        protected override void BindUIData()
        {
            CloseQuickBuy();
            RefreshTime();
            InitStoreInfo();
            RefreshVIPInfo();

            if (m_playerProxy.IsShowVipStorePop())
            {
                m_playerProxy.SetShowVipStorePop();
            }
        }
       
        #endregion

        private void RefreshVIPInfo()
        {
            OnVIPPointChange();
        }

        #region 商品信息

        private void InitStoreInfo()
        {
            m_storeItemInfo = CoreUtils.dataService.QueryRecords<VipStoreDefine>();
        }

        private void RefreshStoreItemInfo()
        {
            var num = (m_storeItemInfo.Count+1) / 2;
            view.m_sv_list_ListView.FillContent(Mathf.CeilToInt(num));
        }

        private void LoadStoreItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitStoreItem;
            view.m_sv_list_ListView.onDragBegin+= OnItemListBeginDrag;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            RefreshStoreItemInfo();
        }

        private void InitStoreItem(ListView.ListItem item)
        {
            UI_Item_VipStoreList_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_VipStoreList_SubView;
            }
            else
            {
                itemView = new UI_Item_VipStoreList_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            int curIndex = item.index * 2;
            VipStoreDefine nextItemInfo = curIndex + 1 >= m_storeItemInfo.Count ? null : m_storeItemInfo[curIndex + 1];
            itemView.SetInfo(m_storeItemInfo[curIndex],nextItemInfo,OnBuy);
            if (m_storeItemInfo[curIndex].ID == m_curQuickBuyItemID ||
                nextItemInfo != null && nextItemInfo.ID == m_curQuickBuyItemID)
            {
                itemView.SetBtnBuyPosition(view.m_UI_Pop_UseAddItemInWin.m_root_RectTransform, m_curQuickBuyItemID);
                itemView.SetBtnBuyPosition(view.m_UI_Pop_UseItemInPop.m_root_RectTransform, m_curQuickBuyItemID);
            }
        }

        private void CloseQuickBuy()
        {
            view.m_UI_Pop_UseItemInPop.gameObject.SetActive(false);
            view.m_UI_Pop_UseAddItemInWin.gameObject.SetActive(false);
            m_curQuickBuyItemID = 0;
        }

        private void ShowQuickBuy(int ID)
        {
            CloseQuickBuy();
            
            var index = GetItemIndexByStoreID(ID);
            var num1 = -1;
            var num2 = -1;

            m_curQuickBuyItemID = ID;
            if (index == -1)
            {
                return;
            }
            
            var storeInfo = m_playerProxy.GetVIPStoreInfo(ID);
            int boughtCount = storeInfo == null ? 0 : (int)storeInfo.count;
            var vipStoreDefine = m_storeItemInfo.Find(x=>x.ID== ID);
            var maxNum = vipStoreDefine.number - boughtCount;
            var itemView = view.m_sv_list_ListView.GetItemByIndex(index).data as UI_Item_VipStoreList_SubView;
            
            if (maxNum <= 0)
            {
                return;
            }
            
            if (maxNum <= 10)
            {
                num1 = maxNum;
            }else if (maxNum <= 100)
            {
                num1 = 5;
                num2 = maxNum;
            }
            else
            {
                num1 = 20;
                num2 = maxNum;
            }

            if (num2!=-1 && num1 > num2)
            {
                num1 = num2;
                num2 = -1;
            }
            
            if (num2 == -1)
            {
                view.m_UI_Pop_UseItemInPop.gameObject.SetActive(false);
                view.m_UI_Pop_UseAddItemInWin.gameObject.SetActive(true);
                view.m_UI_Pop_UseAddItemInWin.m_UI_Model_StandardButton_Blue_use.m_lbl_Text_LanguageText.text =
                   "x"+ num1;
                view.m_UI_Pop_UseAddItemInWin.m_UI_Model_StandardButton_Blue_use.RemoveAllClickEvent();
                view.m_UI_Pop_UseAddItemInWin.m_UI_Model_StandardButton_Blue_use.AddClickEvent(() =>
                {
                    OnBuy(vipStoreDefine,num1,view.m_UI_Pop_UseAddItemInWin.gameObject.GetComponent<RectTransform>());
                });
                itemView.SetBtnBuyPosition(view.m_UI_Pop_UseAddItemInWin.m_root_RectTransform,ID);
            }
            else
            {
                view.m_UI_Pop_UseItemInPop.gameObject.SetActive(true);
                view.m_UI_Pop_UseAddItemInWin.gameObject.SetActive(false);
                view.m_UI_Pop_UseItemInPop.m_UI_Model_StandardButton_Blue_left.RemoveAllClickEvent();
                view.m_UI_Pop_UseItemInPop.m_UI_Model_StandardButton_Blue_left.m_lbl_Text_LanguageText.text = "x"+ num1;
                view.m_UI_Pop_UseItemInPop.m_UI_Model_StandardButton_Blue_right.RemoveAllClickEvent();
                view.m_UI_Pop_UseItemInPop.m_UI_Model_StandardButton_Blue_right.m_lbl_Text_LanguageText.text = "x"+ num2;
                view.m_UI_Pop_UseItemInPop.m_UI_Model_StandardButton_Blue_left.AddClickEvent(() =>
                {
                    OnBuy(vipStoreDefine,num1,view.m_UI_Pop_UseItemInPop.gameObject.GetComponent<RectTransform>());
                });
                view.m_UI_Pop_UseItemInPop.m_UI_Model_StandardButton_Blue_right.AddClickEvent(() =>
                {
                    OnBuy(vipStoreDefine,num2,view.m_UI_Pop_UseItemInPop.gameObject.GetComponent<RectTransform>());
                });
                itemView.SetBtnBuyPosition(view.m_UI_Pop_UseItemInPop.m_root_RectTransform,ID);
            }
        }
        
        private void OnBuyVIPStoreSuccess( Dictionary<long,VipStore> info)
        {
            int quickBuyID = -1;
            var lst = new List<int>();
            foreach (var vipStore in info)
            {
                var index = GetItemIndexByStoreID((int)vipStore.Key);
                if (index != -1 && !lst.Contains(index))
                {
                    lst.Add(index);
                }
                if (vipStore.Value.count > 0)
                {
                    quickBuyID = (int)vipStore.Key;
                }
            }
            lst.ForEach(x=> view.m_sv_list_ListView.RefreshItem(x));
            ShowQuickBuy(quickBuyID);
        }

        private void OnItemListBeginDrag(PointerEventData eventData)
        {
            CloseQuickBuy();
        }
        
        private int GetItemIndexByStoreID(int ID)
        {
            for (int i = 0; i < m_storeItemInfo.Count; i++)
            {
                if (m_storeItemInfo[i].ID == ID)
                {
                    return i / 2;
                }
            }
            return -1;
        }

        #endregion
        
        #region 刷新时间

        private void RefreshTime()
        {
            ClearTimer();
            long refreshTime = ServerTimeModule.Instance.GetServerTime() +
                               ServerTimeModule.Instance.GetNextSundayTime(1);
            UpdateRefreshTime(refreshTime);
            m_refreshTimer = Timer.Register(1, () =>
            {
                UpdateRefreshTime(refreshTime);
            },null,true);
        }

        private void ClearTimer()
        {
            if (m_refreshTimer != null)
            {
                m_refreshTimer.Cancel();
                m_refreshTimer = null;
            }
        }

        private void UpdateRefreshTime(long refreshTime)
        {
            if (refreshTime < ServerTimeModule.Instance.GetServerTime())
            {
                RefreshTime();
                return;
            }
            view.m_lbl_lastTime_LanguageText.text =LanguageUtils.getTextFormat(800117,UIHelper.GetDHMSCounterDown(refreshTime));
        }

        #endregion

        private void OnBuy(VipStoreDefine itemInfo, int num, Transform btn = null)
        {
            var totalPrice = num * itemInfo.price;
            var isEnough = totalPrice <= m_playerProxy.GetResNumByType(itemInfo.type);
            if (!isEnough)
            {
                switch (itemInfo.type)
                {
                    case (int)EnumCurrencyType.food:
                        m_currencyProxy.LackOfResources(totalPrice);
                        break;
                    case (int)EnumCurrencyType.wood:
                        m_currencyProxy.LackOfResources(0,totalPrice);
                        break;
                    case (int)EnumCurrencyType.stone:
                        m_currencyProxy.LackOfResources(0,0,totalPrice);
                        break;
                    case (int)EnumCurrencyType.gold:
                        m_currencyProxy.LackOfResources(0,0,0,totalPrice);
                        break;
                    case (int)EnumCurrencyType.denar:
                        m_currencyProxy.ShortOfDenar(totalPrice);
                        break;
                }
                return;
            }
            
            if (btn != null)
            {
                m_clickBtn = btn;
            }

            if (itemInfo.type == (int) EnumCurrencyType.denar)
            {
                UIHelper.DenarCostRemain(totalPrice, () => { BuyItem(itemInfo.ID, num); });
            }
            else
            {
                BuyItem(itemInfo.ID, num);
            }
        }

        private void BuyItem(int id, int num)
        {
            Shop_BuyVipStore.request req = new Shop_BuyVipStore.request()
            {
                id = id,
                num = num
            };
            AppFacade.GetInstance().SendSproto(req);
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(m_storeItemInfo.Find(x=>x.ID==id).itemID);
            Tip.CreateTip(128005,"", LanguageUtils.getText(itemCfg.l_nameID)).Show();
        }
        
        private void OnVIPPointChange(bool showBarMove = false)
        {
            var vipDefine = m_playerProxy.GetCurVipInfo();
            var curVIPPoint = m_playerProxy.CurrentRoleInfo.vip;
            if (vipDefine != null)
            {
                var vipDefineList = CoreUtils.dataService.QueryRecords<VipDefine>();
                var nextVIPDefine = vipDefineList.Find(x => x.level > vipDefine.level);
                if (nextVIPDefine != null)
                {
                    view.m_lbl_need_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_need_LanguageText.text = LanguageUtils.getTextFormat(800118,
                        (vipDefine.point - curVIPPoint).ToString("N0"), LanguageUtils.getText(nextVIPDefine.l_nameID));
                }
                else
                {
                    view.m_lbl_need_LanguageText.gameObject.SetActive(false);
                }
                ClientUtils.LoadSprite(view.m_img_vipBg_PolygonImage,vipDefine.icon);
                view.m_lbl_vipLevel0_LanguageText.text = vipDefine.level.ToString();
                view.m_lbl_level_LanguageText.text = LanguageUtils.getText(vipDefine.l_nameID);
                if (showBarMove)
                {
                    view.m_pb_vipBar_SmoothBar.SetValue(curVIPPoint * 1.0f / vipDefine.point);
                }
                else
                {
                    view.m_pb_vipBar_GameSlider.value = curVIPPoint * 1.0f / vipDefine.point;
                }

                view.m_lbl_barText_LanguageText.text = string.Format("{0}/{1}", curVIPPoint.ToString("N0"), vipDefine.point.ToString("N0"));
                if (m_curVipLevel >= 0&&m_curVipLevel<vipDefine.level)
                {
                    RefreshStoreItemInfo();
                }
                m_curVipLevel = vipDefine.level;
            }
        }

        private void OnClickAddVIPPoint()
        {
            CoreUtils.uiManager.ShowUI(UI.s_useItem,null, new UseItemViewData()
            {
                ItemType = UseItemType.VipPoint
            });
        }

        private void OnClickClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_VipStore);
        }
    }
}