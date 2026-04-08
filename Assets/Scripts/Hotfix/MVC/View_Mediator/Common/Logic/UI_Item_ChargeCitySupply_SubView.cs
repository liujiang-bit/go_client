// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeCitySupply_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public partial class UI_Item_ChargeCitySupply_SubView : UI_SubView
    {
        private RechargeProxy m_RecgargeProxy;
        
        private List<Data.RechargeSupplyDefine> m_rechargeSupplyDataList = new List<Data.RechargeSupplyDefine>();
        
        protected override void BindEvent()
        {
            base.BindEvent();
            //m_btn_powerShow_GameButton.onClick.AddListener(OnPowerWindow);
        }

        public void Show()
        {
            Init();
        }

        private void Init()
        {
            InitInfo();
            InitSupplyList();
        }

        private void InitInfo()
        {
            m_RecgargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.UpdateSupplyInfo,
                Recharge_AwardRechargeSupply.TagName,
                CmdConstant.PageClick
            },this.gameObject, OnNotification);
        }
        
        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateSupplyInfo:
                    m_sv_list_ListView.ForceRefresh();
                    break;
                case Recharge_AwardRechargeSupply.TagName:
                    Recharge_AwardRechargeSupply.response response =
                        notification.Body as Recharge_AwardRechargeSupply.response;
                    if (response != null)
                    {
                        PlayRewardAni((int) response.id, response.rewardInfo);
                    }

                    break;
                case CmdConstant.PageClick:
                    m_UI_Item_ChargeCitySupplyPop.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void InitSupplyList()
        {
            InitSupplyData();
            m_sv_list_ListView.Clear();
            ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, LoadSupplyItemFinish);
        }

        private void InitSupplyData()
        {
            m_rechargeSupplyDataList = CoreUtils.dataService.QueryRecords<Data.RechargeSupplyDefine>();
            m_rechargeSupplyDataList.Sort(SortRechargeSupply);
        }

        private int SortRechargeSupply(Data.RechargeSupplyDefine data1, Data.RechargeSupplyDefine data2)
        {
            return data1.ID.CompareTo(data2.ID);
        }

        private void LoadSupplyItemFinish(Dictionary<string, GameObject> dic)
        {

            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitSupplyListItem;
            functab.ItemRemove = SupplyListItemRemove;
            m_sv_list_ListView.SetInitData(dic, functab);
            RefreshSupplyInfo(m_rechargeSupplyDataList.Count);

        }
        
        private void InitSupplyListItem(ListView.ListItem item)
        {
            UI_Item_ChargeCitySuppyItem_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_ChargeCitySuppyItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_ChargeCitySuppyItem_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_rechargeSupplyDataList.Count) return;
            Data.RechargeSupplyDefine supplyInfo = m_rechargeSupplyDataList[item.index];
            if (supplyInfo != null)
            {
                Supply supplyRewardInfo = m_RecgargeProxy.GetSupplyInfoById(supplyInfo.price);
                subView.InitData(supplyInfo,supplyRewardInfo);
                subView.RefreshUI();
                subView.AddTipClickListener(() =>
                {
                    Vector3 position = subView.m_btn_tips_GameButton.GetComponent<RectTransform>().position;
                    bool isLastOrFirst = false;
                    if (LanguageUtils.IsArabic())
                    {
                        isLastOrFirst = item.index == m_rechargeSupplyDataList.Count - 1;
                    }
                    else
                    {
                        isLastOrFirst = item.index == 0;
                    }

                    RefreshPopTipInfo(supplyInfo,position,isLastOrFirst);
                });
            }
        }

        private void SupplyListItemRemove(ListView.ListItem item)
        {
            UI_Item_ChargeCitySuppyItem_SubView subView = null;
            if (item.data != null)
            {
                subView = item.data as UI_Item_ChargeCitySuppyItem_SubView;
            }

            if (subView == null) return;
            
            subView.OnItemDestroy();
        }

        private void RefreshSupplyInfo(int count)
        {
            m_sv_list_ListView.FillContent(count);
        }

        private void PlayRewardAni(int id,RewardInfo rewardInfo)
        {

            for (int i = 0; i < m_rechargeSupplyDataList.Count; i++)
            {
                ListView.ListItem item = m_sv_list_ListView.GetItemByIndex(i);
                if (item.data != null)
                {
                    UI_Item_ChargeCitySuppyItem_SubView subView = item.data as UI_Item_ChargeCitySuppyItem_SubView;
                    if (subView.SupplyDefine.price == id)
                    {
                        if (rewardInfo != null)
                        {
                            string rewardTip = "";
                            if (rewardInfo.HasItems)
                            {
                                foreach (var rewardItem in rewardInfo.items)
                                {
                                    var globalEffectMediator = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                                    if (globalEffectMediator != null)
                                    {
                                        globalEffectMediator.FlyItemEffect((int)rewardItem.itemId, (int)rewardItem.itemNum, subView.m_btn_get.gameObject.GetComponent<RectTransform>());
                                    }

                                    if (!string.IsNullOrEmpty(rewardTip))
                                    {
                                        rewardTip += ",";
                                    }
                                    var itemData = CoreUtils.dataService.QueryRecord<Data.ItemDefine>((int)rewardItem.itemId);
                                    rewardTip += LanguageUtils.getText(itemData.l_nameID) + "x" +
                                                 rewardItem.itemNum.ToString();
                                }
                                Tip.CreateTip(300237,rewardTip).Show();
                            }
                            
                        }

                        break;
                    }
                }
            }
        }

        private void RefreshPopTipInfo(Data.RechargeSupplyDefine supply, Vector3 position,bool isLast)
        {
            Vector2 arrpos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_UI_Item_ChargeCitySupply, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(position), CoreUtils.uiManager.GetUICamera(), out arrpos);
            if (isLast)
            {
                m_UI_Item_ChargeCitySupplyPop.m_pl_pos_UIDefaultValue.GetComponent<RectTransform>().anchoredPosition = new Vector2(arrpos.x + 140,arrpos.y);
            }
            else
            {
                m_UI_Item_ChargeCitySupplyPop.m_pl_pos_UIDefaultValue.GetComponent<RectTransform>().anchoredPosition = arrpos;
            }
            m_UI_Item_ChargeCitySupplyPop.SetInfo(supply);
        }
    }
}