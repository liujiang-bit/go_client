// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Win_UseItemMediator
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

namespace Game {

    public enum UseItemType
    {
        ActionPoint,
        VipPoint,
    }

    public class UseItemViewData
    {
        public UseItemType ItemType { get; set; }
        public int costAp { get; set; }
    }


    public class UI_Win_UseItemMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_UseItemMediator";

        private int m_vipLevel;
        private int m_lastVipLevel= -1;

        private bool m_isFirstRefresh = true;

        #endregion

        //IMediatorPlug needs
        public UI_Win_UseItemMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_UseItemView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Item_ItemChangeResource.TagName,
                CmdConstant.UpdatePlayerActionPower,
                CmdConstant.VipPointChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Item_ItemChangeResource.TagName:
                    {
                        var response = notification.Body as Item_ItemChangeResource.response;
                        if (response == null || response.itemId == 0 || response.itemNum == 0)
                        {
                            return;
                        }
                        OnUseItemSuccess((int)response.itemId);
                    }
                    break;
                case CmdConstant.UpdatePlayerActionPower:
                    {
                        if(m_viewData.ItemType == UseItemType.ActionPoint)
                        {
                            RefreshProgress();
                        }
                    }
                    break;
                case CmdConstant.VipPointChange:
                    {
                        if (m_viewData.ItemType == UseItemType.VipPoint)
                        {
                            RefreshProgress();
                        }
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
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_viewData = view.data as UseItemViewData;
            if (m_viewData == null) return;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (m_playerProxy == null) return;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (m_bagProxy == null) return;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            if (m_playerAttributeProxy == null) return;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            if (m_currencyProxy == null) return;
            InitItemListData();
            InitList();
            RefreshTitle();
            RefreshItemIcon();
            RefreshProgress();
            m_isFirstRefresh = false;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_useItem);
            });
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OnUseItemSuccess(int itemId)
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(itemId);
            if (itemCfg == null) return;
            Tip.CreateTip(300070, LanguageUtils.getText(itemCfg.l_nameID)).Show();
            int oldItemCount = m_itemList.Count;
            InitItemListData();
            int nowItemCount = m_itemList.Count;
            if(m_viewData.ItemType == UseItemType.ActionPoint && nowItemCount == 0)
            {
                CoreUtils.uiManager.CloseUI(UI.s_useItem);
                return;
            }
            if (oldItemCount != nowItemCount)
            {
                view.m_sv_list_ListView.FillContent(m_itemList.Count);
            }
            else
            {
                view.m_sv_list_ListView.ForceRefresh();
            }
            var itemCount = m_bagProxy.GetItemNum(itemCfg.ID);
            if (itemCount > 0)
            {
                ShowItemQuickUse(itemCfg.ID);
            }
        }

        private void InitItemListData()
        {
            m_itemList.Clear();
            int itemSubType = 0;
            switch (m_viewData.ItemType)
            {
                case UseItemType.ActionPoint:
                    {
                        itemSubType = 50208;
                    }
                    break;
                case UseItemType.VipPoint:
                    {
                        itemSubType = 10101;
                    }
                    break;
            }
            var itemCfgs = CoreUtils.dataService.QueryRecords<Data.ItemDefine>();
            foreach(var itemCfg in itemCfgs)
            {
                if(itemCfg.subType == itemSubType)
                {
                    switch (m_viewData.ItemType)
                    {
                        case UseItemType.VipPoint:
                            if (itemCfg.shortcutPrice != 0||m_bagProxy.GetItemNum(itemCfg.ID) > 0)
                            {
                                m_itemList.Add(itemCfg);
                            }
                            break;
                        default:
                            if (m_bagProxy.GetItemNum(itemCfg.ID) > 0)
                            {
                                m_itemList.Add(itemCfg);
                            }
                            break;
                    }
                }                
            }
        }

        private void InitList()
        {
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, OnItemPrefabLoadFinish);
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_ListView.SetInitData(dict, funcTab);
            view.m_sv_list_ListView.FillContent(m_itemList.Count);
        }

        private void ItemEnter(ListView.ListItem item)
        {
            if (item.index >= m_itemList.Count) return;

            UI_Item_StandardUseItemView itemView = null;
            if (item.data == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
                item.data = itemView;
            }
            else
            {
                itemView = item.data as UI_Item_StandardUseItemView;
            }
            if (itemView == null) return;
            var itemCfg = m_itemList[item.index];
            var itemCount = m_bagProxy.GetItemNum(itemCfg.ID);
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
            itemView.m_lbl_itemDesc_LanguageText.text = string.Format(LanguageUtils.getText(itemCfg.l_desID), ClientUtils.FormatComma(itemCfg.desData1));
            itemView.m_lbl_itemCount_LanguageText.text = string.Format(LanguageUtils.getText(300074), ClientUtils.FormatComma(itemCount));
            itemView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(itemCfg, 0, false);
            itemView.m_pl_quick_ArabLayoutCompment.gameObject.SetActive(false);
            itemView.m_UI_Model_Blue_big.gameObject.SetActive(itemCount>0);
            itemView.m_UI_Model_Blue_big.RemoveAllClickEvent();
            itemView.m_UI_Model_Blue_big.AddClickEvent(() =>
            {
                CheckAndUseItem(itemCfg.ID,1);
            });
            itemView.m_UI_Model_Yellow.gameObject.SetActive(itemCount==0);
            itemView.m_UI_Model_Yellow.SetNum(ClientUtils.FormatComma(itemCfg.shortcutPrice));
            itemView.m_UI_Model_Yellow.RemoveAllClickEvent();
            itemView.m_UI_Model_Yellow.AddClickEvent(() =>
            {
                BuyItem(itemCfg);
            });

            if (m_curQuickUseItemID == itemCfg.ID)
            {
                ShowItemQuickUse(m_curQuickUseItemID);
            }
        }

        private void ClearOtherQuickUse(int curItemID)
        {
            for (int i = 0; i < m_itemList.Count; i++)
            {
                if (m_itemList[i].ID == curItemID)
                {
                    continue;
                }
                var itemView = view.m_sv_list_ListView.GetItemByIndex(i).data as UI_Item_StandardUseItemView;
                if (itemView == null)
                {
                    continue;
                }
                itemView.m_pl_quick_ArabLayoutCompment.gameObject.SetActive(false);
            }
        }

        private void ShowItemQuickUse(int itemID)
        {
            if (m_isMaxValue)
            {
                ClearOtherQuickUse(0);
                return;
            }
            ClearOtherQuickUse(itemID);
            m_curQuickUseItemID = itemID;
            var index = m_itemList.FindIndex(x => x.ID == itemID);
            var item = view.m_sv_list_ListView.GetItemByIndex(index);
            var itemView = item.data as UI_Item_StandardUseItemView;
            long useNum =Mathf.CeilToInt((m_maxValue - m_curValue)/(float)m_itemList[index].data1);
            var currentItemNum = m_bagProxy.GetItemNum(itemID);
            useNum = useNum > currentItemNum ? currentItemNum : useNum;
            
            itemView.m_pl_quick_ArabLayoutCompment.gameObject.SetActive(useNum>0);
            itemView.m_UI_Model_StandardButton_MiniBlue.m_lbl_Text_LanguageText.text = "x" + useNum;
            itemView.m_UI_Model_StandardButton_MiniBlue.RemoveAllClickEvent();
            itemView.m_UI_Model_StandardButton_MiniBlue.AddClickEvent(() =>
            {
                CheckAndUseItem(itemID,useNum);
            });
        }

        private void RefreshTitle()
        {
            string title = string.Empty;
            switch(m_viewData.ItemType)
            {
                case UseItemType.ActionPoint:
                    {
                        title = LanguageUtils.getText(300061);
                    }
                    break;
                case UseItemType.VipPoint:
                    {
                        title = LanguageUtils.getText(800030);
                    }
                    break;
            }
            view.m_UI_Model_Window_Type1.setWindowTitle(title);
        }

        private void RefreshItemIcon()
        {
            switch (m_viewData.ItemType)
            {
                case UseItemType.ActionPoint:
                    {
                        view.m_img_icon_PolygonImage.gameObject.SetActive(false);
                        view.m_img_item_PolygonImage.gameObject.SetActive(true);
                    }
                    break;
                case UseItemType.VipPoint:
                    {
                        var vipDefine = m_playerProxy.GetCurVipInfo();
                        if (vipDefine != null)
                        {
                            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage,vipDefine.icon);
                        }
                        view.m_img_icon_PolygonImage.gameObject.SetActive(true);
                        view.m_img_item_PolygonImage.gameObject.SetActive(false);
                        
                    }
                    break;
            }            
        }

        private void RefreshProgress()
        {
            long curValue = 0;
            long maxValue = 0;
            m_isMaxValue = false;
            switch(m_viewData.ItemType)
            {
                case UseItemType.ActionPoint:
                    {
                        curValue = m_playerProxy.CurrentRoleInfo.actionForce;
                        maxValue = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0).vitalityLimit + (int)m_playerAttributeProxy.GetCityAttribute(Data.attrType.maxVitality).value;
                        m_isMaxValue = curValue >= maxValue;
                    }
                    break;
                case UseItemType.VipPoint:
                    {
                        curValue = m_playerProxy.CurrentRoleInfo.vip;
                        var vipDefine = m_playerProxy.GetCurVipInfo();
                        if (vipDefine != null )
                        {
                            m_vipLevel = vipDefine.level;
                            maxValue = vipDefine.point;
                            if (m_lastVipLevel == -1)
                            {
                                m_lastVipLevel = vipDefine.level;
                                view.m_lbl_vipLevel_LanguageText.text = vipDefine.level.ToString();
                            }
                            else
                            {
                                if (vipDefine.level > m_lastVipLevel)
                                {
                                    ShowUpLevelAni();
                                }
                            }
                        }

                        if (maxValue == 0)
                        {
                            m_isMaxValue = true;
                        }
                    }
                    break;
            }

            if (m_viewData.costAp > 0)
            {
                maxValue = m_viewData.costAp;
            }
            m_maxValue = maxValue;
            m_curValue = curValue;

            if (m_isFirstRefresh)
            {
                view.m_pb_rogressBar_GameSlider.value = curValue * 1.0f / maxValue;
            }
            else
            {
                float newVal = curValue * 1.0f / maxValue;
                view.m_pb_rogressBar_SmoothBar.SetValue(view.m_pb_rogressBar_GameSlider.value + (newVal - view.m_pb_rogressBar_GameSlider.value));
            }
            if (m_viewData.ItemType == UseItemType.VipPoint && m_maxValue == 0)
            {
                view.m_lbl_num_LanguageText.text = LanguageUtils.getText(800026);
            }
            else
            {
                view.m_lbl_num_LanguageText.text = string.Format("{0}/{1}", ClientUtils.FormatComma(curValue), ClientUtils.FormatComma(maxValue));
            }
        }

        private Timer m_levelUpAniTimer = null;
        //表现升级动画
        private void ShowUpLevelAni()
        {
            m_lastVipLevel = m_vipLevel;
            view.m_lbl_vipLevel_LanguageText.text = m_vipLevel.ToString();
            view.m_UI_Item_LevelUp.gameObject.SetActive(true);
            AnimationClip[] clips = view.m_UI_Item_LevelUp.m_UI_Item_CaptainLevelUpOnHead_Animator.runtimeAnimatorController.animationClips;
            view.m_UI_Item_LevelUp.m_UI_Item_CaptainLevelUpOnHead_Animator.Play("Show",-1,0);
            if (m_levelUpAniTimer != null)
            {
                m_levelUpAniTimer.Cancel();
                m_levelUpAniTimer = null;
            }
            m_levelUpAniTimer = Timer.Register(clips[0].length, () => {
                if (view.gameObject == null)
                {
                    return;
                }
                view.m_UI_Item_LevelUp.gameObject.SetActive(false);
            });
        }

        //获取溢出数量
        private long CheckOverFlow(int itemID, long num)
        {
            if (m_viewData.ItemType == UseItemType.VipPoint)
            {

                var vipCfgs = CoreUtils.dataService.QueryRecords<VipDefine>();
                var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
                
                if (m_maxValue == 0)
                {
                    return itemCfg.data1 * num;
                }
                if (vipCfgs.Count > 2 && vipCfgs[vipCfgs.Count - 2].point == m_maxValue)
                {
                    var overflow = m_curValue + itemCfg.data1 * num - m_maxValue;
                    return overflow>0? overflow:0;
                }
            }

            return 0;
        }

        private void CheckAndUseItem(int itemID, long num)
        {
            if (m_viewData.ItemType == UseItemType.VipPoint)
            {
                var overflow = CheckOverFlow(itemID, num);
                if (m_isMaxValue)
                {
                    Tip.CreateTip(800026).Show();
                }else if (overflow == 0)
                {
                    UseItem(itemID, num);
                }
                else
                {
                    Alert.CreateAlert(LanguageUtils.getTextFormat(800031,ClientUtils.FormatComma(overflow))).SetLeftButton().SetRightButton(() => { UseItem(itemID,num); })
                        .Show();
                }
            }
            else
            {
                UseItem(itemID,num);
            }
        }

        private void UseItem(int itemID, long num)
        {
            Item_ItemChangeResource.request request = new Item_ItemChangeResource.request()
            {
                itemIndex = m_bagProxy.GetItemIndex(itemID),
                itemNum = num,
            };
            AppFacade.GetInstance().SendSproto(request);
            ClearOtherQuickUse(0);
            m_curQuickUseItemID = 0;
        }

        private void BuyItem(ItemDefine itemCfg)
        {
            if (m_isMaxValue)
            {
                Tip.CreateTip(800026).Show();
                return;
            }
            if (m_currencyProxy.ShortOfDenar(itemCfg.shortcutPrice))
            {
                return;
            }
            UIHelper.DenarCostRemain(itemCfg.shortcutPrice, () =>
            {
                long overFlow = CheckOverFlow(itemCfg.ID, 1);
                if (overFlow > 0)
                {
                    Alert.CreateAlert(LanguageUtils.getTextFormat(800031,ClientUtils.FormatComma(overFlow))).SetLeftButton().SetRightButton(() =>
                        {
                            m_currencyProxy.BuyAndUseCurrencyResources(itemCfg.ID);
                            Tip.CreateTip(128005, itemCfg.data1, LanguageUtils.getText(itemCfg.l_tipsID)).Show();
                        })
                        .Show();
                    return;
                }
                m_currencyProxy.BuyAndUseCurrencyResources(itemCfg.ID);
                Tip.CreateTip(128005, itemCfg.data1, LanguageUtils.getText(itemCfg.l_tipsID)).Show();
            });
        }

        private long m_maxValue=0;
        private long m_curValue = 0;
        private int m_curQuickUseItemID = 0;
        private bool m_isMaxValue; //已到最高上限
        private PlayerAttributeProxy m_playerAttributeProxy;
        private PlayerProxy m_playerProxy = null;
        private CurrencyProxy m_currencyProxy = null;
        private BagProxy m_bagProxy = null;
        private UseItemViewData m_viewData = null;
        private List<Data.ItemDefine> m_itemList = new List<Data.ItemDefine>();
    }
}