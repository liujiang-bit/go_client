// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeGemShop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_ChargeGemShop_SubView : UI_SubView
    {
        private Dictionary<string, GameObject> m_assetDic;
        private List<RechargeGemMallDefine> m_lstItemData;
        private RechargeProxy m_RechargeProxy;
        private const int colNum = 3;

        protected override void BindEvent()
        {
            base.BindEvent();
            
            
            
            m_RechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            InitListView();
        }
        
        public void InitListView()
        {
            InitItemData();
            ClientUtils.PreLoadRes(this.gameObject,m_sv_list_ListView.ItemPrefabDataList,OnItemPrefabLoadFinish);
        }

        public void RefreshListView()
        {
            InitItemData();
            m_sv_list_ListView.ForceRefresh();
        }
        
        public void InitItemData()
        {
            if (m_lstItemData == null)
                m_lstItemData = new List<RechargeGemMallDefine>();
            m_lstItemData.Clear();
            var result = CoreUtils.dataService.QueryRecords<Data.RechargeGemMallDefine>();
            m_lstItemData.AddRange(result);
        }

        public List<RechargeGemMallDefine> GetLineData(int lineIndex)
        {
            List<RechargeGemMallDefine> result = new List<RechargeGemMallDefine>();    
            int indexMin = colNum * lineIndex;
            int indexMax = colNum * (lineIndex + 1);
            for (int i = 0; i < m_lstItemData.Count; i++)
            {
                if (i >= indexMin && i < indexMax)
                {
                    result.Add(m_lstItemData[i]);
                }
            }
            return result; 
        }
        public void OnItemPrefabLoadFinish(Dictionary<string,GameObject> assetDic)
        {
            m_assetDic = assetDic;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            
            m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            m_sv_list_ListView.FillContent(Mathf.CeilToInt((m_lstItemData.Count != null ? m_lstItemData.Count : 0) / colNum ));
        }

        void ItemEnter(ListView.ListItem scrollItem)
        {
            if (scrollItem == null || m_lstItemData == null || scrollItem.index >= m_lstItemData.Count) return;
            var lstLineData = GetLineData(scrollItem.index);
            UI_Item_ChargeGemShopList_SubView itemView = null;
            if (scrollItem.data == null)
            {
                itemView = new UI_Item_ChargeGemShopList_SubView(scrollItem.go.GetComponent<RectTransform>());
                scrollItem.data = itemView;
            }
            else
            {
                itemView = scrollItem.data as UI_Item_ChargeGemShopList_SubView;
            }
            if(itemView == null) return;
            itemView.Refresh(lstLineData);
        }

        private void OnServiceEvent()
        {
            //暂时只有问题提交
            SDKURLBundle.shareInstance().serviceURL((exception, url) =>
            {
                if (exception.isNone())
                {
                    SDKUtils.shareInstance().OpenBrowser(url);
                }
            });
        }
    }
}