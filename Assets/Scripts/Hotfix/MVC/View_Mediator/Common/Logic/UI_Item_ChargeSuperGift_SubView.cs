// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeSuperGift_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using UnityEngine.EventSystems;

namespace Game {
    public partial class UI_Item_ChargeSuperGift_SubView : UI_SubView
    {
        private RechargeProxy m_RecgargeProxy;
        private List<Data.RechargeSaleDefine> m_rechargeSuperGiftDataList = new List<Data.RechargeSaleDefine>();
        private Vector2 m_tmpMousePos;
        
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
                CmdConstant.UpdateSuperGiftInfo,
            },this.gameObject, OnNotification);
        }
        
        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateSuperGiftInfo:
                    InitSuperGiftData();
                    RefreshSuperGiftInfo(m_rechargeSuperGiftDataList.Count);
                    m_sv_list_ListView.ForceRefresh();
                    break;
                default:
                    break;
            }
        }

        private void InitSupplyList()
        {
            ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, LoadSuperGiftItemFinish);
        }
        
        private void InitSuperGiftData()
        {
            m_rechargeSuperGiftDataList.Clear();
            var superGiftMap = new Dictionary<long,List<Data.RechargeSaleDefine>>();
            var rechargeSuperGiftDataList = CoreUtils.dataService.QueryRecords<Data.RechargeSaleDefine>();
            foreach (var data in rechargeSuperGiftDataList)
            {
                if (!superGiftMap.ContainsKey(data.group))
                {
                    superGiftMap[data.group] = new List<Data.RechargeSaleDefine>();
                }
                
                superGiftMap[data.group].Add(data);
            }

            foreach (var KeyValue in superGiftMap)
            {
                KeyValue.Value.Sort(SortSuperGift1);
                Data.RechargeSaleDefine showData = null;
                for ( int i = 0; i < KeyValue.Value.Count; i++)
                {
                    var data = KeyValue.Value[i];
                    if (m_RecgargeProxy.isSuperGiftCanShow(data, i == (KeyValue.Value.Count - 1)))
                    {
                        m_rechargeSuperGiftDataList.Add(data);
                        break;
                    }
                }
            }
            m_rechargeSuperGiftDataList.Sort(SortSuperGift2);
            
        }

        private int SortSuperGift1(Data.RechargeSaleDefine data1, Data.RechargeSaleDefine data2)
        {
            return data1.ID.CompareTo(data2.ID);
        }

        private int SortSuperGift2(Data.RechargeSaleDefine data1, Data.RechargeSaleDefine data2)
        {
            bool isBought1 = m_RecgargeProxy.isSuperGiftbought(data1.group, data1.price);
            bool isBought2 = m_RecgargeProxy.isSuperGiftbought(data2.group, data2.price);
            if (isBought1 == isBought2)
            {
                return data1.ID.CompareTo(data2.ID);
            }
            else if (isBought1)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        
        private void LoadSuperGiftItemFinish(Dictionary<string, GameObject> dic)
        {
            m_sv_list_ListView.Clear();
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitSuperGiftListItem;
            functab.ItemRemove = SuperGiftListItemRemove;
            m_sv_list_ListView.SetInitData(dic, functab);
            InitSuperGiftData();
            RefreshSuperGiftInfo(m_rechargeSuperGiftDataList.Count);
        }

        private void InitSuperGiftListItem(ListView.ListItem item)
        {
            UI_Item_ChargeSuperGiftItem_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_ChargeSuperGiftItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_ChargeSuperGiftItem_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_rechargeSuperGiftDataList.Count) return;
            Data.RechargeSaleDefine superGiftInfo = m_rechargeSuperGiftDataList[item.index];
            if (superGiftInfo != null)
            {
                subView.InitData(superGiftInfo);
                subView.m_sv_list_ListView.SetParentListView(this.m_sv_list_ListView);
                subView.RefreshUI();
            }
        }

        private void SuperGiftListItemRemove(ListView.ListItem item)
        {
            UI_Item_ChargeSuperGiftItem_SubView subView = null;
            if (item.data != null)
            {
                subView = item.data as UI_Item_ChargeSuperGiftItem_SubView;
            }

            if (subView == null) return;
            
            subView.OnItemDestroy();
        }
        
        private void RefreshSuperGiftInfo(int count)
        {
            m_sv_list_ListView.FillContent(count);
        }
    }
}