// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Win_ItemCollectionMediator 王国地图 没有迷雾 回收道具
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
    public class UI_Win_ItemCollectionMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ItemCollectionMediator";

        private RewardGroupProxy m_rewardGroupProxy;
        private List<RewardGroupData> m_itemList;

        #endregion

        //IMediatorPlug needs
        public UI_Win_ItemCollectionMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ItemCollectionView view;

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
            var result = view.data as Item_ItemUse.response;
            RewardInfo reward = result.rewardInfo;

            view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(128018);

            if (result.itemId == 502020001)//工人招募
            {
                view.m_lbl_tips_LanguageText.text = LanguageUtils.getText(180535);
            }
            else if(result.itemId == 502100001) //王国地图
            {
                view.m_lbl_tips_LanguageText.text = LanguageUtils.getText(128019);
            }

            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            m_itemList = m_rewardGroupProxy.GetRewardDataByRewardInfo(reward);

            if (m_itemList.Count < 1)
            {
                return;
            }
            view.m_UI_Model_RewardGet.RefreshByGroup(m_itemList[0], 3, true);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.setCloseHandle(OnClose);
            view.m_UI_BtnSure.m_btn_languageButton_GameButton.onClick.AddListener(OnClose);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_itemCollection);
        }
    }
}