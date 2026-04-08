// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Win_WarnMediator
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

namespace Game {
    public class UI_Win_WarnMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_WarnMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_WarnMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_WarnView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.WarWarningInfoChanged
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.WarWarningInfoChanged:
                    {
                        OnWarningInfoChanged();
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
            view.m_sv_list_view_ListView.Clear();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_warWarningProxy = AppFacade.GetInstance().RetrieveProxy(WarWarningProxy.ProxyNAME) as WarWarningProxy;
            if (m_warWarningProxy == null) return;
            InitWarningInfoData();
            List<string> prefagNames = new List<string>();
            prefagNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            prefagNames.Add("UI_Item_SoldierHead");
            ClientUtils.PreLoadRes(view.gameObject, prefagNames, OnItemPrefabLoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window.AddBackEvent(() =>
            {
                OnBackButtonClicked();
            });
            view.m_UI_Model_Window.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_warWarning);
            });
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private enum WarnViewType
        {
            Preview,
            Detail
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            m_dictItemPrefabInfo = dict;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            funcTab.GetItemPrefabName = GetItemPrefabName;
            funcTab.GetItemSize = GetItemSize;
            funcTab.ItemRemove = ItemRemove;
            view.m_sv_list_view_ListView.SetInitData(dict, funcTab);
            RefreshList();
        }

        private void ItemRemove(ListView.ListItem item)
        {
            if(item.data != null)
            {
                UI_Item_WarnInfo_SubView subView = item.data as UI_Item_WarnInfo_SubView;
                if(subView != null)
                {
                    subView.Clear();
                }
            }
        }

        private float GetItemSize(ListView.ListItem item)
        {
            float height = 0;
            GameObject go = null;
            switch (m_viewType)
            {
                case WarnViewType.Preview:
                    {
                        if (item.index == 0)
                        {
                            go = m_dictItemPrefabInfo[s_ignoreItemName];
                        }
                        else
                        {
                            go = m_dictItemPrefabInfo[s_warnInfoName];
                        }
                    }
                    break;
                case WarnViewType.Detail:
                    {
                        if (item.index == 0)
                        {
                            go = m_dictItemPrefabInfo[s_warnInfoName];
                        }
                        else
                        {
                            go = m_dictItemPrefabInfo[s_warnDetailName];
                        }
                    }
                    break;
            }
            if(go != null)
            {
                RectTransform rect = go.transform as RectTransform;
                if(rect != null)
                {
                    height = rect.sizeDelta.y;
                }
            }
            return height;
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            string name = string.Empty;
            switch (m_viewType)
            {
                case WarnViewType.Preview:
                    {
                        if(item.index ==0)
                        {
                            name = s_ignoreItemName;
                        }
                        else
                        {
                            name = s_warnInfoName;
                        }
                    }
                    break;
                case WarnViewType.Detail:
                    {
                        if (item.index == 0)
                        {
                            name = s_warnInfoName;
                        }
                        else
                        {
                            name = s_warnDetailName;
                        }
                    }
                    break;
            }
            return name;
        }

        private void ItemEnter(ListView.ListItem item)
        {
            switch(m_viewType)
            {
                case WarnViewType.Preview:
                    {
                        if(item.index == 0)
                        {
                            IgnoreItemEnter(item);
                        }
                        else
                        {
                            InfoItemEnter(item);
                        }
                    }
                    break;
                case WarnViewType.Detail:
                    {
                        if (item.index == 0)
                        {
                            InfoItemEnter(item);
                        }
                        else
                        {
                            DetailItemEnter(item);
                        }
                    }
                    break;
            }
             
        }

        private void IgnoreItemEnter(ListView.ListItem item)
        {
            UI_Item_WarnIgnoreView subView = null;
            if(item.data == null)
            {
                subView = MonoHelper.AddHotFixViewComponent<UI_Item_WarnIgnoreView>(item.go);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_WarnIgnoreView;
            }
            if (subView == null) return;
            subView.m_UI_Model_btn.RemoveAllClickEvent();
            subView.m_UI_Model_btn.AddClickEvent(() =>
            {
                List<long> willIgnoreList = new List<long>();
                foreach(var info in m_warWarningInfoList)
                {
                    if(!info.isShield)
                    {
                        willIgnoreList.Add(info.earlyWarningIndex);
                    }
                }
                if (willIgnoreList.Count == 0) return;
                Role_ShiledEarlyWarning.request request = new Role_ShiledEarlyWarning.request()
                {
                    earlyWarningIndex = willIgnoreList,
                };
                AppFacade.GetInstance().SendSproto(request);
            });
        }

        private void InfoItemEnter(ListView.ListItem item)
        {
            UI_Item_WarnInfo_SubView subView = null;
            if(item.data == null)
            {
                subView = new UI_Item_WarnInfo_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_WarnInfo_SubView;
            }
            if (subView == null) return;

            EarlyWarningInfoEntity info = null;
            
            switch (m_viewType)
            {
                case WarnViewType.Preview:
                    {
                        if(item.index - 1 < m_warWarningInfoList.Count)
                        {
                            info = m_warWarningInfoList[item.index - 1];
                        }
                    }
                    break;
                case WarnViewType.Detail:
                    {
                        if(item.index == 0 && m_curDetailInfoIndex < m_warWarningInfoList.Count)
                        {
                            info = m_warWarningInfoList[m_curDetailInfoIndex];
                        }
                    }
                    break;
            }
            if (info == null) return;
            subView.Refresh(info, m_viewType == WarnViewType.Detail);
            subView.SetShowDetailButtonInteractable(m_viewType == WarnViewType.Preview && WarWarningType.Scout != (WarWarningType)info.earlyWarningType);
            subView.RemoveAllClickListener();

            if(!info.isShield)
            {
                subView.AddIgnoreButtonListener(() =>
                {
                    Role_ShiledEarlyWarning.request request = new Role_ShiledEarlyWarning.request()
                    {
                        earlyWarningIndex = new List<long>(){ info.earlyWarningIndex},
                    };
                    AppFacade.GetInstance().SendSproto(request);
                });
            }   
            if(m_viewType == WarnViewType.Preview && WarWarningType.Scout != (WarWarningType)info.earlyWarningType)
            {
                subView.AddShowDetailButtonListener(() =>
                {
                    m_curDetailInfoIndex = item.index - 1;
                    ShowDetail();
                });
            }
            subView.AddLocationTargetButtonListener(() =>
            {
                Vector3 worldPos = subView.GetLocationPosition();
                WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.z, 1000, null);
                CoreUtils.uiManager.CloseUI(UI.s_warWarning);
            });
        }

        private void DetailItemEnter(ListView.ListItem item)
        {
            if (item.index != 1) return;
            UI_Item_WarnDetail_SubView subView = null;
            if(item.data == null)
            {
                subView = new UI_Item_WarnDetail_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_WarnDetail_SubView;
            }
            if (subView == null) return;
            if (m_curDetailInfoIndex >= m_warWarningInfoList.Count) return;
            subView.Refresh(m_warWarningInfoList[m_curDetailInfoIndex], m_dictItemPrefabInfo[s_soldierItemName]);
        }

        private void OnBackButtonClicked()
        {
            m_viewType = WarnViewType.Preview;
            RefreshList();
        }

        private void ShowDetail()
        {
            m_viewType = WarnViewType.Detail;
            RefreshList();
        }

        private void RefreshList()
        {
            int contentCount = GetListItemCount();
            view.m_sv_list_view_ListView.FillContent(contentCount);
            view.m_UI_Model_Window.m_btn_back_GameButton.gameObject.SetActive(m_viewType == WarnViewType.Detail);
        }

        private int GetListItemCount()
        {
            int contentCount = 0;
            switch (m_viewType)
            {
                case WarnViewType.Preview:
                    {
                        contentCount = m_warWarningInfoList.Count + 1;
                    }
                    break;
                case WarnViewType.Detail:
                    {
                        contentCount = 2;

                    }
                    break;
            }
            return contentCount;
        }

        private void InitWarningInfoData()
        {
            m_warWarningInfoList = m_warWarningProxy.GetWarWarningInfoList();
        }

        private void OnWarningInfoChanged()
        {            
            int oldCount = m_warWarningInfoList.Count;
            m_warWarningInfoList = m_warWarningProxy.GetWarWarningInfoList();
            if(m_dictItemPrefabInfo != null && m_dictItemPrefabInfo.Count != 0)
            {
                if (m_warWarningInfoList.Count == oldCount)
                {
                    view.m_sv_list_view_ListView.ForceRefresh();
                }
                else
                {
                    int contentCount = GetListItemCount();
                    view.m_sv_list_view_ListView.FillContent(contentCount);
                }
            }
        }

        private List<EarlyWarningInfoEntity> m_warWarningInfoList = new List<EarlyWarningInfoEntity>();
        private WarnViewType m_viewType = WarnViewType.Preview;
        private WarWarningProxy m_warWarningProxy = null;
        private int m_curDetailInfoIndex = 0;
        private Dictionary<string, GameObject> m_dictItemPrefabInfo = null;

        private readonly string s_ignoreItemName = "UI_Item_WarnIgnore";
        private readonly string s_warnInfoName = "UI_Item_WarnInfo";
        private readonly string s_warnDetailName = "UI_Item_WarnDetail";
        private readonly string s_soldierItemName = "UI_Item_SoldierHead";
    }
}