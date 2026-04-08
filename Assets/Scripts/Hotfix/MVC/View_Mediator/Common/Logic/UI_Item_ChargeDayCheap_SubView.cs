// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeDayCheap_SubView
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
    public partial class UI_Item_ChargeDayCheap_SubView : UI_SubView
    {
         private RechargeProxy m_RecgargeProxy;
        private List<Data.RechargeDailySpecialDefine> m_rechargeDailySpecialDataList = new List<Data.RechargeDailySpecialDefine>();
        
        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_box_GameButton.onClick.AddListener(OnRewardClickEvent);
        }
        
        public void Show()
        {
            Init();
        }

        public void ShowRewardItemFly(RewardInfo items)
        {
            m_UI_AnimationBox.OpenBox();
            UIHelper.FlyBoxRewardEffect(m_btn_box_PolygonImage.transform, items);
        }

        private void Init()
        {
            m_RecgargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.UpdateFreeDailyBoxInfo,
                CmdConstant.UpdateDailySpecialInfo,
            },this.gameObject, OnNotification);
            InitInfo();
            InitSpecailList();
        }
        
        private void InitInfo()
        {
            if (!m_RecgargeProxy.IsFreeGiftGot())
            {
                m_lbl_get_LanguageText.text = LanguageUtils.getText(762149);
                m_UI_AnimationBox.SetBox(false,true);
                m_btn_box_GameButton.interactable = true;
            }
            else
            {
                m_lbl_get_LanguageText.text = LanguageUtils.getText(570009);
                m_UI_AnimationBox.SetBox(true);
                m_btn_box_GameButton.interactable = false;
            }
        }
        
        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateFreeDailyBoxInfo:
                    InitInfo();
                    break;
                case CmdConstant.UpdateDailySpecialInfo:
                    InitSpecailData();
                    m_sv_list_ListView.FillContent(m_rechargeDailySpecialDataList.Count);;
                    m_sv_list_ListView.ForceRefresh();
                    break;
                default:
                    break;
            }
        }

        private void InitSpecailList()
        {
            ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, LoadSpecailItemFinish);
        }
        
        private void LoadSpecailItemFinish(Dictionary<string, GameObject> dic)
        {
            InitSpecailData();
            m_sv_list_ListView.Clear();
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitSpecialListItem;
            functab.ItemRemove = SpecialListItemRemove;
            m_sv_list_ListView.SetInitData(dic, functab);
            m_sv_list_ListView.FillContent(m_rechargeDailySpecialDataList.Count);;

        }
        
        private void InitSpecailData()
        {
            m_rechargeDailySpecialDataList.Clear();
            var specialList = CoreUtils.dataService.QueryRecords<Data.RechargeDailySpecialDefine>();
            foreach (var specialData in specialList)
            {
                if (m_RecgargeProxy.GetDayCheapGiftRewardId(specialData) > 0)
                {
                    m_rechargeDailySpecialDataList.Add(specialData);
                }
            }
            
            m_rechargeDailySpecialDataList.Sort(SortSuperGift1);
        }

        private int SortSuperGift1(Data.RechargeDailySpecialDefine data1, Data.RechargeDailySpecialDefine data2)
        {
            return data1.ID.CompareTo(data2.ID);
        }

        private void InitSpecialListItem(ListView.ListItem item)
        {
            UI_Item_ChargeDayCheapItem_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_ChargeDayCheapItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_ChargeDayCheapItem_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_rechargeDailySpecialDataList.Count) return;
            Data.RechargeDailySpecialDefine specialInfo = m_rechargeDailySpecialDataList[item.index];
            if (specialInfo != null)
            {
                subView.InitData(specialInfo);
                subView.RefreshUI();
            }
        }

        private void SpecialListItemRemove(ListView.ListItem item)
        {
            UI_Item_ChargeSuperGiftItem_SubView subView = null;
            if (item.data != null)
            {
                subView = item.data as UI_Item_ChargeSuperGiftItem_SubView;
            }

            if (subView == null) return;
            
            subView.OnItemDestroy();
        }
        
        public void OnRewardClickEvent()
        {
            Role_GetFreeDaily.request request = new Role_GetFreeDaily.request()
            {
                
            };
            
            AppFacade.GetInstance().SendSproto(request);
        }
    }
}