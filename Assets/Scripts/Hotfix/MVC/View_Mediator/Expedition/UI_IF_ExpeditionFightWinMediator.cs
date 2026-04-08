// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_IF_ExpeditionFightWinMediator
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

    public class ExpeditionFightWinViewData
    {
        public Data.ExpeditionDefine ExpeditionCfg { get; set; }
        public int Star { get; set; }
        public bool IsFirstReward { get; set; }

        public List<RewardInfo> Rewards { get; set; }
        public List<long> StarResult { get; set; }
    }

    public class UI_IF_ExpeditionFightWinMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_ExpeditionFightWinMediator";


        #endregion

        //IMediatorPlug needs
        public UI_IF_ExpeditionFightWinMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_ExpeditionFightWinView view;

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
            foreach(var timer in m_starEffectTimer)
            {
                if(timer != null)
                {
                    timer.Cancel();
                }
            }
            m_starEffectTimer.Clear();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_viewData = view.data as ExpeditionFightWinViewData;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

            m_firstRewardViews.Add(view.m_UI_Model_RewardGet1);
            m_firstRewardViews.Add(view.m_UI_Model_RewardGet2);
            m_firstRewardViews.Add(view.m_UI_Model_RewardGet3);

            m_dailyRewardViews.Add(view.m_UI_Model_RewardGet4);
            m_dailyRewardViews.Add(view.m_UI_Model_RewardGet5);
            m_dailyRewardViews.Add(view.m_UI_Model_RewardGet6);

            RefreshHeroSpine();
            RefreshFirstWinReward();
            RefreshReward();
            RefreshStar();
            Data.ConfigDefine config = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0);

            view.m_UI_Pop_TalkTip.SetLanguageId(config.expeditionVictoryBubble[Random.Range(0,config.expeditionVictoryBubble.Count)]);
            view.m_img_firsticon_PolygonImage.gameObject.SetActive(m_viewData.IsFirstReward);
            view.m_lbl_haveget_LanguageText.gameObject.SetActive(m_viewData.Rewards == null || m_viewData.Rewards.Count == 0);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_ck_GameButton.onClick.AddListener(ShowFlyRewardEffect);
            CoreUtils.audioService.PlayOneShot("Sound_Ui_Victory");
        }

        protected override void BindUIData()
        {

        }

        public override bool onMenuBackCallback()
        {
            BackToExpeditionUI();
            return true;
        }

        #endregion

        private void RefreshHeroSpine()
        {
            var playerTroopData = m_expeditionProxy.GetPlayerTroopData(1);
            if (playerTroopData == null) return;
            Data.HeroDefine heroCfg = CoreUtils.dataService.QueryRecord<Data.HeroDefine>(playerTroopData.MainHeroId);
            if (heroCfg == null) return;
            ClientUtils.LoadSpine(view.m_pl_spinGraphic_SkeletonGraphic, heroCfg.heroModel);
        }

        private void RefreshFirstWinReward()
        {
            view.m_pl_reward1_GridLayoutGroup.gameObject.SetActive(m_viewData.IsFirstReward);
            if(m_viewData.IsFirstReward)
            {
                List<Data.ItemPackageDefine> rewards = m_rewardGroupProxy.GetRewardByGroupId(m_viewData.ExpeditionCfg.firstReward);
                for (int i = 0; i < m_firstRewardViews.Count; ++i)
                {
                    if (rewards.Count >= i)
                    {
                        m_firstRewardViews[i].gameObject.SetActive(true);
                        m_firstRewardViews[i].RefreshReward(rewards[i]);
                    }
                    else
                    {
                        m_firstRewardViews[i].gameObject.SetActive(false);
                    }
                }
            }

        }

        private void RefreshReward()
        {
            view.m_pl_reward2_GridLayoutGroup.gameObject.SetActive(m_viewData.Rewards != null);
            if(m_viewData.Rewards != null)
            {
                for(int i = 0;i < m_dailyRewardViews.Count; ++i)
                {
                    if (m_viewData.Rewards.Count > i)
                    {
                        m_dailyRewardViews[i].gameObject.SetActive(true);
                        m_dailyRewardViews[i].RefreshRewardInfo(m_viewData.Rewards[i]);
                    }
                    else
                    {
                        m_dailyRewardViews[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void RefreshStar()
        {
            view.m_UI_Item_ExpeditionFightTask.Show(m_viewData.ExpeditionCfg, m_viewData.StarResult);
            bool isShow = m_viewData.StarResult[0] == 1;
            float delay = 0;
            view.m_img_starHighlight1_PolygonImage.gameObject.SetActive(isShow);
            if (isShow)
            {
                ShowStarEffect(view.m_img_starHighlight1_PolygonImage.transform,delay);
            }
            isShow = m_viewData.StarResult[1] == 1;
            view.m_img_starHighlight2_PolygonImage.gameObject.SetActive(isShow);
            if (isShow)
            {
                delay += 0.4f;
                ShowStarEffect(view.m_img_starHighlight2_PolygonImage.transform,delay);
            }
            isShow = m_viewData.StarResult[2] == 1;
            view.m_img_starHighlight3_PolygonImage.gameObject.SetActive(isShow);
            if (isShow)
            {
                delay += 0.4f;
                ShowStarEffect(view.m_img_starHighlight3_PolygonImage.transform,delay);
            }
        }

        private void ShowStarEffect(Transform parent,float delay)
        {
            m_starEffectTimer.Add(Timer.Register(delay, () =>
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    parent.GetChild(i).gameObject.SetActive(true);
                }
            }));
        }

        private void ShowFlyRewardEffect()
        {
            if (m_viewData.Rewards == null || m_viewData.Rewards.Count == 0) 
            {
                BackToExpeditionUI();
                return;
            }
            GlobalEffectMediator effectMediator = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (m_viewData.IsFirstReward)
            {
                List<Data.ItemPackageDefine> rewards = m_rewardGroupProxy.GetRewardByGroupId(m_viewData.ExpeditionCfg.firstReward);
                for(int i =0; i < rewards.Count && i < m_firstRewardViews.Count; ++i)
                {
                    switch ((EnumRewardType)rewards[i].type)
                    {
                        case EnumRewardType.Currency:
                            effectMediator.FlyUICurrency(rewards[i].typeData, rewards[i].number, m_firstRewardViews[i].m_root_RectTransform.position);
                            break;
                        case EnumRewardType.Item:
                            effectMediator.FlyItemEffect(rewards[i].typeData, rewards[i].number, m_firstRewardViews[i].m_root_RectTransform);
                            break;
                    }
                }
            }
            for(int i = 0; i < m_viewData.Rewards.Count && i < m_dailyRewardViews.Count; ++i)
            {
                var rewardInfo = m_viewData.Rewards[i];
                bool isCurrency = false;
                EnumCurrencyType currencyType = EnumCurrencyType.food;
                int number = 0;
                if (rewardInfo.HasFood)
                {
                    isCurrency = true;
                    currencyType = EnumCurrencyType.food;
                    number = (int)rewardInfo.food;
                }
                if (rewardInfo.HasWood)
                {
                    isCurrency = true;
                    currencyType = EnumCurrencyType.wood;
                    number = (int)rewardInfo.wood;
                }
                if (rewardInfo.HasStone)
                {
                    isCurrency = true;
                    currencyType = EnumCurrencyType.stone;
                    number = (int)rewardInfo.stone;
                }
                if (rewardInfo.HasGold)
                {
                    isCurrency = true;
                    currencyType = EnumCurrencyType.gold;
                    number = (int)rewardInfo.gold;
                }
                if (rewardInfo.HasDenar)
                {
                    isCurrency = true;
                    currencyType = EnumCurrencyType.denar;
                    number = (int)rewardInfo.denar;
                }
                if (rewardInfo.HasExpeditionCoin)
                {
                    isCurrency = true;
                    currencyType = EnumCurrencyType.conquerorMedal;
                    number = (int)rewardInfo.expeditionCoin;
                }
                if(isCurrency)
                {
                    effectMediator.FlyUICurrencyFromWorld(currencyType, number, m_dailyRewardViews[i].m_root_RectTransform.position);
                    break;
                }
                if (rewardInfo.HasItems)
                {
                    var item = rewardInfo.items[0];
                    effectMediator.FlyItemEffect((int)item.itemId, (int)item.itemNum, m_dailyRewardViews[i].m_root_RectTransform);
                    break;
                }
            }
            view.m_btn_ck_GameButton.onClick.RemoveAllListeners();
            Timer.Register(1, BackToExpeditionUI);
        }

        private void BackToExpeditionUI()
        {
            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightWin);
            AppFacade.GetInstance().SendNotification(CmdConstant.ExitExpeditionMap);
        }

        private ExpeditionProxy m_expeditionProxy = null;
        private RewardGroupProxy m_rewardGroupProxy = null;
        private HeroProxy m_heroProxy = null;
        private ExpeditionFightWinViewData m_viewData = null;
        private List<UI_Model_RewardGet_SubView> m_firstRewardViews = new List<UI_Model_RewardGet_SubView>();
        private List<UI_Model_RewardGet_SubView> m_dailyRewardViews = new List<UI_Model_RewardGet_SubView>();
        private List<Timer> m_starEffectTimer = new List<Timer>();
    }
}