// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月16日
// Update Time         :    2020年5月16日
// Class Description   :    RewardGetWinMediator
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
using UnityEngine.UI;
using System;

namespace Game {
    public class RewardGetWinMediator : GameMediator {
        #region Member
        public static string NameMediator = "RewardGetWinMediator";

        private RewardGroupProxy m_rewardGroupProxy;

        private List<RewardGroupData> m_itemList;
        private List<List<RewardGroupData>> m_arrList;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private int m_col = 0;

        #endregion

        //IMediatorPlug needs
        public RewardGetWinMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public RewardGetWinView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            CoreUtils.uiManager.CloseUI(UI.s_bagGiftOpen);
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
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            RewardInfo reward = view.data as RewardInfo;
            m_itemList = m_rewardGroupProxy.GetRewardDataByRewardInfo(reward);

            //ClientUtils.Print(m_itemList);
            view.m_UI_Model_LC_RewardGet.gameObject.SetActive(false);

            //预加载列表预设
            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_pl_btn.m_btn_languageButton_GameButton.onClick.AddListener(Close);
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(Close);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            m_col = m_assetDic["UI_Model_LC_RewardGet"].GetComponent<GridLayoutGroup>().constraintCount;
            Refresh();
        }

        private void Refresh()
        {
            if (m_itemList.Count <= m_col)
            {
                view.m_sv_list_view_ListView.gameObject.SetActive(false);
                view.m_UI_Model_LC_RewardGet.Refresh(m_itemList);
                view.m_UI_Model_LC_RewardGet.gameObject.SetActive(true);
                return;
            }
            view.m_UI_Model_LC_RewardGet.gameObject.SetActive(false);
            if (m_col <= 0)
            {
                return;
            }
            int count = m_itemList.Count;
            m_arrList = new List<List<RewardGroupData>>();
            int line = (int)Math.Ceiling((float)count / m_col);
            for (int i = 0; i < line; i++)
            {
                m_arrList.Add(new List<RewardGroupData>());
            }
            for (int i = 0; i < m_itemList.Count; i++)
            {
                int index = (int)Math.Ceiling((float)(i+1) / m_col);
                m_arrList[index - 1].Add(m_itemList[i]);
            }
            RefreshList();
        }

        private void RefreshList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ItemByIndex;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_view_ListView.FillContent(m_arrList.Count);
        }

        //刷新菜单item
        private void ItemByIndex(ListView.ListItem listItem)
        {
            if (listItem.data == null)
            {
                var subView = new UI_Model_LC_RewardGet_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.Refresh(m_arrList[listItem.index]);
            }
            else
            {
                var subView = listItem.data as UI_Model_LC_RewardGet_SubView;
                subView.Refresh(m_arrList[listItem.index]);
            }
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_rewardGetWin);
        }
    }
}