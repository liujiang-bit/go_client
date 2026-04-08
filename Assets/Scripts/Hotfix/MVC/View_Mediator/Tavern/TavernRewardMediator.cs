// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    TavernRewardMediator 酒馆多开奖励界面
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
using Data;
using UnityEngine.UI;
using System;

namespace Game {
    public class TavernRewardItemData
    {
        public Heros HeroInfo;
        public RewardItem ItemInfo;
        public int Quality;
        public int Type = 0;//0 = hero,1 = Item;
        public long Weight = 0;

        public static int Compare(TavernRewardItemData tavernRewardItemDataA, TavernRewardItemData tavernRewardItemDataB)
        {
            int result  =  (tavernRewardItemDataA.Type).CompareTo(tavernRewardItemDataB.Type);
            if (result == 0)
            {
                result = (tavernRewardItemDataA.Weight).CompareTo(tavernRewardItemDataB.Weight);
            }
            return result;
        }
    }

    public class TavernRewardMediator : GameMediator {
        #region Member
        public static string NameMediator = "TavernRewardMediator";

        private Build_Tavern.response m_rewardInforesponse;
        private List<TavernRewardItemData> m_rewardList;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private int m_lineCount;
        private int m_constraintCount;

        private bool m_isAutoShowExchange = false;

        #endregion

        //IMediatorPlug needs
        public TavernRewardMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public TavernRewardView view;

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

        private void FillTavernRankDic( )
        {
            List<TavernRankDefine> tavernRankDefines = CoreUtils.dataService.QueryRecords<TavernRankDefine>();
            tavernRankDefines.ForEach((tavernRankDefine) => {
                if (tavernRankDefine.group == 2)
                {
                    if (!m_goldTavernRank.ContainsKey(tavernRankDefine.typeData))
                    {
                        m_goldTavernRank.Add(tavernRankDefine.typeData, tavernRankDefine.ID);
                    }
                }
                else if (tavernRankDefine.group == 1)
                {
                    if (!m_silverTavernRank.ContainsKey(tavernRankDefine.typeData))
                    {
                        m_silverTavernRank.Add(tavernRankDefine.typeData, tavernRankDefine.ID);
                    }
                }
            });
        }
        private bool FillTavernRewardItemDataList()
        {
            bool secess = true;
            if (m_rewardInforesponse.rewardInfo.HasHeros)
            {
                for (int i = 0; i < m_rewardInforesponse.rewardInfo.heros.Count; i++)
                {
                    TavernRewardItemData item = new TavernRewardItemData();
                    item.HeroInfo = m_rewardInforesponse.rewardInfo.heros[i];
                    HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)item.HeroInfo.heroId);
                    if (heroDefine != null)
                    {
                        item.Quality = heroDefine.rare;
                        item.Type = 0;

                        switch (m_enumBoxRewardType)
                        {
                            case EnumBoxRewardType.Silver:
                                if (m_silverTavernRank.ContainsKey(heroDefine.ID))
                                {
                                    item.Weight = m_silverTavernRank[heroDefine.ID];
                                }
                                else
                                {
                                    item.Weight = 0;
                                }
                                break;
                            case EnumBoxRewardType.Gold:
                                if (m_goldTavernRank.ContainsKey(heroDefine.ID))
                                {
                                    item.Weight = m_goldTavernRank[heroDefine.ID];
                                }
                                else
                                {
                                    item.Weight = 0;
                                }
                                break;
                        }
                    }
                    m_rewardList.Add(item);
                    if (item.HeroInfo.isNew != 1)
                    {
                        m_isAutoShowExchange = true;
                    }
                }
            }
            if (m_rewardInforesponse.rewardInfo.HasItems)
            {
                for (int i = 0; i < m_rewardInforesponse.rewardInfo.items.Count; i++)
                {
                    TavernRewardItemData item = new TavernRewardItemData();
                    item.ItemInfo = m_rewardInforesponse.rewardInfo.items[i];
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)item.ItemInfo.itemId);
                    if (itemDefine != null)
                    {
                        item.Quality = itemDefine.quality;
                        item.Type = 1;
                        switch (m_enumBoxRewardType)
                        {
                            case EnumBoxRewardType.Silver:
                                if (m_silverTavernRank.ContainsKey(itemDefine.ID))
                                {
                                    item.Weight = m_silverTavernRank[itemDefine.ID];
                                }
                                else
                                {
                                    item.Weight = 0;
                                }
                                break;
                            case EnumBoxRewardType.Gold:
                                if (m_goldTavernRank.ContainsKey(itemDefine.ID))
                                { 
                                    item.Weight = m_goldTavernRank[itemDefine.ID];
                                }
                                else
                                {
                                    item.Weight = 0;
                                }
                                break;
                        }
                    }
                    m_rewardList.Add(item);
                }
            }
            m_rewardList.Sort(TavernRewardItemData.Compare);
            return secess;
        }
        Dictionary<long, long> m_silverTavernRank = new Dictionary<long, long>();
        Dictionary<long, long> m_goldTavernRank = new Dictionary<long, long>();
        EnumBoxRewardType m_enumBoxRewardType = EnumBoxRewardType.Gold;
        protected override void InitData()
        {
            m_rewardInforesponse = view.data as Build_Tavern.response;
            m_rewardList = new List<TavernRewardItemData>();
            m_enumBoxRewardType = (EnumBoxRewardType)m_rewardInforesponse.type;
            FillTavernRankDic();
            FillTavernRewardItemDataList();
            view.m_lbl_desc_LanguageText.gameObject.SetActive(m_isAutoShowExchange);

            List<string> prefabNames = new List<string>();
            prefabNames.Add("UI_Item_TavernMulReward");
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_sure.m_btn_languageButton_GameButton.onClick.AddListener(Close);   
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
            m_constraintCount = m_assetDic["UI_Item_TavernMulRewardCol"].GetComponent<GridLayoutGroup>().constraintCount;

            //Debug.LogError("m_constraintCount:"+ m_constraintCount);
            m_lineCount = (int)Math.Ceiling((float)m_rewardList.Count / m_constraintCount);
            //Debug.LogError("count:"+ m_lineCount);
            InitList();
        }

        private void InitList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_view_ListView.FillContent(m_lineCount);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_TavernMulRewardCol_SubView subView;
            if (listItem.data == null)
            {
                subView = new UI_Item_TavernMulRewardCol_SubView(listItem.go.GetComponent<RectTransform>());
                subView.Init(m_assetDic["UI_Item_TavernMulReward"]);
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_TavernMulRewardCol_SubView;
            }

            UI_Item_TavernMulRewardView nodeView;
            int min = listItem.index * subView.ConstraintCount;
            int max = min + (subView.ConstraintCount - 1);
            int tnum = -1;
            for (int i = min; i <= max; i++)
            {
                tnum = i - min;
                nodeView = subView.ElemItemList[tnum];
                if (i >= m_rewardList.Count)
                {
                    nodeView.gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    nodeView.gameObject.SetActive(true);
                }
                subView.RefreshItem(m_rewardList[i], tnum);
            }
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_tavernReward);
        }

    }
}