// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    StrongerPlayerRankMediator 最强执政官 查看历史排名
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
    public class StrongerPlayerRankMediator : GameMediator {
        #region Member
        public static string NameMediator = "StrongerPlayerRankMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private bool m_isDispose;

        private List<Activity_GetHistoryRank.response.HistoryRank> m_rankList = new List<Activity_GetHistoryRank.response.HistoryRank>();
        #endregion

        //IMediatorPlug needs
        public StrongerPlayerRankMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public StrongerPlayerRankView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Activity_GetHistoryRank.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Activity_GetHistoryRank.TagName:
                    ProcessData(notification.Body);
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
            m_isDispose = true;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(Close);
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
            var sp = new Activity_GetHistoryRank.request();
            AppFacade.GetInstance().SendSproto(sp);
        }

        private void ProcessData(object body)
        {
            if (m_isDispose)
            {
                return;
            }
            var response = body as Activity_GetHistoryRank.response;
            if (response.rankList == null)
            {
                SetNoRankTextShow(true);
                return;
            }
            if (response.rankList.Count < 1)
            {
                SetNoRankTextShow(true);
                return;
            }
            //ClientUtils.Print(response.rankList);
            foreach (var data in response.rankList)
            {
                if (data.Value.historyInfo != null && data.Value.historyInfo.Count > 0)
                {
                    data.Value.historyInfo.Sort(delegate (HistoryInfo x, 
                                                          HistoryInfo y)
                    {
                        int re = x.rank.CompareTo(x.rank);
                        return re;
                    });
                }
                m_rankList.Add(data.Value);
            }
            m_rankList.Sort(delegate (Activity_GetHistoryRank.response.HistoryRank x, Activity_GetHistoryRank.response.HistoryRank y)
            {
                int re = y.index.CompareTo(x.index);
                return re;
            });
            RefreshList();
        }

        public void RefreshList()
        {
            bool isShowText = (m_rankList.Count > 0) ? false : true;
            SetNoRankTextShow(isShowText);

            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ItemEventByIndex;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_ListView.FillContent(m_rankList.Count);
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            if (listItem.data == null)
            {
                var subView = new UI_Item_StrongerPlayerRank_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.Refresh(m_rankList[listItem.index]);
            }
            else
            {
                var subView = listItem.data as UI_Item_StrongerPlayerRank_SubView;
                subView.Refresh(m_rankList[listItem.index]);
            }
        }

        private void SetNoRankTextShow(bool isShow)
        {
            view.m_lbl_no_rank_LanguageText.gameObject.SetActive(isShow);
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_strongerPlayerRank);
        }
    }
}