// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月18日
// Update Time         :    2020年5月18日
// Class Description   :    BagGiftOpenMediator 礼包类道具 奖励获取
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
using System;
using Data;
using UnityEngine.UI;

namespace Game {
    public class BagGiftOpenParam
    {
        public Int64 ItemIndex;
        public Int64 ItemId;
        public Int64 Num;
        public Int64 ShowType;
    }

    public class BagGiftOpenMediator : GameMediator {
        #region Member
        public static string NameMediator = "BagGiftOpenMediator";

        private RewardGroupProxy m_rewardGroupProxy;
        public Int64 m_itemIndex;
        public Int64 m_itemId;
        public Int64 m_itemNum;

        private List<UI_Model_RewardGet_SubView> m_selectList;
        private UI_Model_RewardGet_SubView m_selectRewardView;
        private int m_selectId;

        private int m_col = 0;
        private List<RewardGroupData> m_itemList;
        private List<List<RewardGroupData>> m_arrList;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private int m_selectIndex;

        #endregion

        //IMediatorPlug needs
        public BagGiftOpenMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public BagGiftOpenView view;

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
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

            BagGiftOpenParam param = view.data as BagGiftOpenParam;
            m_itemIndex = param.ItemIndex;
            m_itemId = param.ItemId;
            m_itemNum = param.Num;

            if (param.ShowType == 1)
            {
                RefreshSelectReward();
            }
            else
            {
                RefreshExchange();
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(OnClose);

            view.m_UI_ChoseSure.m_btn_languageButton_GameButton.onClick.AddListener(OnSure);
            view.m_UI_exchangeSure.m_btn_languageButton_GameButton.onClick.AddListener(OnExchange);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        //刷新奖励选择
        private void RefreshSelectReward()
        {
            view.m_pl_chose.gameObject.SetActive(true);
            view.m_pl_exchange.gameObject.SetActive(false);

            ItemDefine define = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_itemId);

            view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(128013);
            view.m_lbl_ChoseCount_LanguageText.text = LanguageUtils.getTextFormat(128014, ClientUtils.FormatComma(m_itemNum));

            m_selectList = new List<UI_Model_RewardGet_SubView>();
            m_selectList.Add(view.m_UI_Model_RewardSelect1);
            m_selectList.Add(view.m_UI_Model_RewardSelect2);
            m_selectList.Add(view.m_UI_Model_RewardSelect3);
            m_selectList.Add(view.m_UI_Model_RewardSelect4);
            m_selectList.Add(view.m_UI_Model_RewardSelect5);
            m_selectList.Add(view.m_UI_Model_RewardSelect6);

            List<RewardGroupData> groupData = m_rewardGroupProxy.GetChoiceRewardDataByGroup(define.data2);
            Debug.LogFormat("物品数量：{0}", groupData.Count);
            //ClientUtils.Print(groupData);
            if (groupData.Count > m_selectList.Count)
            {
                m_itemList = groupData;
                view.m_pl_itemsInChose_GridLayoutGroup.gameObject.SetActive(false);
                view.m_sv_list_view_ListView.gameObject.SetActive(true);
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
                prefabNames.Add("UI_Model_LC_RewardGet");
                ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
            }
            else
            {
                view.m_pl_itemsInChose_GridLayoutGroup.gameObject.SetActive(true);
                view.m_sv_list_view_ListView.gameObject.SetActive(false);
                for (int i = 0; i < m_selectList.Count; i++)
                {
                    if (i < groupData.Count)
                    {
                        groupData[i].number = groupData[i].number;
                        m_selectList[i].SetScale(view.m_pl_itemsInChose_GridLayoutGroup.transform.localScale.x);
                        m_selectList[i].m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(true);
                        m_selectList[i].gameObject.SetActive(true);
                        m_selectList[i].RefreshByGroup(groupData[i], 3, true);
                        m_selectList[i].m_UI_Model_Item.SetSelectImgActive(i == 0);
                        m_selectList[i].BtnClickListener = ClickRewardItem;
                        if (i == 0)
                        {
                            m_selectRewardView = m_selectList[i];
                            m_selectId = groupData[i].Id;
                        }
                    }
                    else
                    {
                        m_selectList[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            m_col = m_assetDic["UI_Model_LC_RewardGet"].GetComponent<GridLayoutGroup>().constraintCount;
            RefreshList();
        }

        private void RefreshList()
        {
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
                int index = (int)Math.Ceiling((float)(i + 1) / m_col);
                m_arrList[index - 1].Add(m_itemList[i]);
            }
            m_selectIndex = 0;
            if (m_arrList.Count > 0 && m_arrList[0].Count > 0)
            {
                m_selectId = m_arrList[0][0].Id;
            }
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ItemByIndex;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_view_ListView.FillContent(m_arrList.Count);
        }

        //刷新菜单item
        private void ItemByIndex(ListView.ListItem listItem)
        {
            UI_Model_LC_RewardGet_SubView subView;
            if (listItem.data == null)
            {
                subView = new UI_Model_LC_RewardGet_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickListener = ClickListRewardItem;
                subView.Refresh2(m_arrList[listItem.index], listItem.index, m_selectIndex);
            }
            else
            {
                subView = listItem.data as UI_Model_LC_RewardGet_SubView;
                subView.Refresh2(m_arrList[listItem.index], listItem.index, m_selectIndex);
            }
            UI_Model_RewardGet_SubView itemView = subView.GetSelectedSubView();
            if (itemView != null)
            {
                if (m_selectRewardView != null)
                {
                    if (m_selectRewardView != itemView)
                    {
                        m_selectRewardView.m_UI_Model_Item.SetSelectImgActive(false);
                        m_selectRewardView = itemView;
                    }
                }
                else
                {
                    m_selectRewardView = itemView;
                }
            }
        }

        //刷新兑换
        private void RefreshExchange()
        {           
            view.m_UI_Model_Window_TypeMid.m_lbl_title_LanguageText.text = LanguageUtils.getText(128015);
            ItemDefine define = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_itemId);

            view.m_pl_chose.gameObject.SetActive(false);
            view.m_pl_exchange.gameObject.SetActive(true);

            RewardGroupData beforeData = m_rewardGroupProxy.ConvertRewardGroupData((int)EnumRewardType.Item, (int)m_itemId, 1);
            beforeData.number = beforeData.number * (int)m_itemNum;
            view.m_UI_itemBefore.RefreshByGroup(beforeData, 3, true);

            List<RewardGroupData> groupData = m_rewardGroupProxy.GetRewardDataByGroup(define.data2, false);
            if (groupData.Count < 1)
            {
                Debug.LogErrorFormat("奖励组物品小于1 group:{0} itemId:{1}", define.data2, m_itemId);
                return;
            }
            RewardGroupData afterData = groupData[0];
            afterData.number = afterData.number * (int)m_itemNum;
            view.m_UI_itemAfter.RefreshByGroup(afterData, 3, true);
        }

        private void ClickRewardItem(UI_Model_RewardGet_SubView subView)
        {
            if (m_selectRewardView != null)
            {
                m_selectRewardView.m_UI_Model_Item.SetSelectImgActive(false);
            }
            m_selectRewardView = subView;
            RewardGroupData data = subView.ItemData2 as RewardGroupData;
            m_selectId = data.Id;
            subView.m_UI_Model_Item.SetSelectImgActive(true);
        }

        private void ClickListRewardItem(UI_Model_RewardGet_SubView subView)
        {
            if (m_selectRewardView != null)
            {
                m_selectRewardView.m_UI_Model_Item.SetSelectImgActive(false);
            }
            m_selectIndex = (int)subView.ItemData3;
            m_selectRewardView = subView;
            RewardGroupData data = subView.ItemData2 as RewardGroupData;
            m_selectId = data.Id;
            subView.m_UI_Model_Item.SetSelectImgActive(true);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_bagGiftOpen);
        }

        private void OnSure()
        {
            view.m_UI_ChoseSure.m_btn_languageButton_GameButton.interactable = false;
            var sp = new Item_ItemUse.request();
            sp.itemIndex = m_itemIndex;
            sp.itemNum = m_itemNum;
            sp.id = m_selectId;
            Debug.LogFormat("itemIndex:{0} itemNum:{1}  id:{2}", m_itemIndex, m_itemNum, m_selectId);
            AppFacade.GetInstance().SendSproto(sp);
            Timer.Register(0.3f, () =>
            {
                OnClose();
            });
        }

        private void OnExchange()
        {
            view.m_UI_exchangeSure.m_btn_languageButton_GameButton.interactable = false;
            var sp = new Item_ItemUse.request();
            sp.itemIndex = m_itemIndex;
            sp.itemNum = m_itemNum;
            Debug.LogFormat("itemIndex:{0} itemNum:{1}  id:{2}", m_itemIndex, m_itemNum, m_selectId);
            AppFacade.GetInstance().SendSproto(sp);
            //OnClose();
            //Timer.Register(0.3f, () =>
            //{
            //    OnClose();
            //});
        }
    }
}