// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    UI_IF_ExpeditionFightMediator
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
using UnityEngine.Events;
using Data;

namespace Game {

    public class ExpeditionLevelFlagUIData
    {
        public int ExpeditionId { get; set; }
        public Vector2 PosInItem { get; set; }
    }



    public class UI_IF_ExpeditionFightMediator : GameMediator {

        #region Member
        public static string NameMediator = "UI_IF_ExpeditionFightMediator";

        public bool m_delayRefreshCDing;
        public bool m_delayRefreshCurrencying;

        #endregion

        //IMediatorPlug needs
        public UI_IF_ExpeditionFightMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_ExpeditionFightView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.PlayerExpeditionInfoChange,
                Expedition_OneKeyAward.TagName,
                CmdConstant.ExpeditionCoinChange,
                CmdConstant.UpdateFloatCurrency,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Expedition_OneKeyAward.TagName:
                    var resposne = notification.Body as Expedition_OneKeyAward.response;
                    if(resposne != null)
                    {
                        OnOneKeyGetRewardResponse(resposne);
                    }
                    break;
                case CmdConstant.PlayerExpeditionInfoChange:
                    if (m_delayRefreshCDing)
                    {
                        return;
                    }
                    m_delayRefreshCDing = true;
                    Timer.Register(0.1f, () => {
                        if (view.gameObject == null)
                        {
                            return;
                        } 
                        RefreshRewardCD();
                        RefreshExpeditionRewardInfo();
                        m_delayRefreshCDing = false;
                    });
                    break;
                case CmdConstant.UpdateFloatCurrency:
                case CmdConstant.ExpeditionCoinChange:
                    if (m_delayRefreshCurrencying)
                    {
                        return;
                    }
                    m_delayRefreshCurrencying = true;
                    Timer.Register(0.1f, ()=> {
                        if (view.gameObject == null)
                        {
                            return;
                        }
                        RefreshCurrecny();
                        m_delayRefreshCurrencying = false;
                    });
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
            m_expeditionLevelFlagPrefab = null;
            ClearGetRewardCdTimer();
            ClearStoreResetTimer();          
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

            m_selectedLevelId = m_lastUnlockLevelId = GetLastUnlockLevelId();
            if(m_expeditionProxy.LastSelectedLevelId != 0)
            {
                m_selectedLevelId = m_expeditionProxy.LastSelectedLevelId;
            }
            
            m_rewardSubViews1.Add(view.m_UI_Model_RewardGet1);
            m_rewardSubViews1.Add(view.m_UI_Model_RewardGet2);
            m_rewardSubViews1.Add(view.m_UI_Model_RewardGet3);

            m_rewardSubViews2.Add(view.m_UI_Model_RewardGet4);
            m_rewardSubViews2.Add(view.m_UI_Model_RewardGet5);
            m_rewardSubViews2.Add(view.m_UI_Model_RewardGet6);
            List<string> preLoadPrefabs = new List<string>();
            preLoadPrefabs.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            preLoadPrefabs.Add(m_expeditionLevelFlagPrefabName);
            view.m_pl_buttom_CanvasGroup.gameObject.SetActive(false);
            view.m_btn_box_GameButton.gameObject.SetActive(false);
            view.m_pl_coin_CanvasGroup.gameObject.SetActive(false);
            ClientUtils.PreLoadRes(view.gameObject, preLoadPrefabs, OnPreLoadResFinish);
            InitStoreRedPoint();
            if (CoreUtils.audioService.GetCurBgmName() != ExpeditionProxy.UIBgm)
            {
                m_cacheBgm = CoreUtils.audioService.GetCurBgmName();
                CoreUtils.audioService.PlayBgm(ExpeditionProxy.UIBgm);
            }

        }

        protected override void BindUIEvent()
        {
            view.m_btn_box_GameButton.onClick.AddListener(OnClickedOneKeyGetReward);
            view.m_btn_shop_GameButton.onClick.AddListener(OnClickedStore);
            view.m_btn_rank_GameButton.onClick.AddListener(OnClickedRank);
            view.m_btn_rule_GameButton.onClick.AddListener(OnClickedRule);
            view.m_btn_challage.m_btn_languageButton_GameButton.onClick.AddListener(OnClickedChallageButton);
            view.m_UI_Model_Interface.AddClickEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_expeditionFight);
                m_expeditionProxy.ClearPlayerTroopDataCache();
                if (!string.IsNullOrEmpty(m_cacheBgm))
                {
                    CoreUtils.audioService.PlayBgm(m_cacheBgm);
                }
                else
                {
                    GameEventGlobalMediator tMediator = AppFacade.GetInstance().RetrieveMediator(GameEventGlobalMediator.NameMediator) as GameEventGlobalMediator;
                    if (tMediator != null)
                    {
                        CoreUtils.audioService.PlayBgm(!tMediator.IsDay() ? RS.SoundCityNight : RS.SoundCityDay);
                    }
                }
            });

            UnityAction tipFun = () =>
            {
                HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(6002);
                if (define == null)
                {
                    return;
                }
                Data.ExpeditionDefine selectLevelCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(m_selectedLevelId);
                if (selectLevelCfg == null) return;
                HelpTip.CreateTip(string.Format(LanguageUtils.getText(define.l_typeID), LanguageUtils.getTextFormat(define.l_data1, selectLevelCfg.troopsNumber)), view.m_pl_challageNumbg_PolygonImage.rectTransform)
                    .SetAutoFilter(true)
                    .SetStyle(HelpTipData.Style.arrowDown)
                    .SetOffset(view.m_pl_challageNumbg_PolygonImage.rectTransform.rect.height / 2)
                    .Show();
            };

            view.m_pl_challageNumbg_GameButton.onClick.AddListener(tipFun);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void OnPreLoadResFinish(Dictionary<string, GameObject> dict)
        {
            view.m_pl_buttom_CanvasGroup.gameObject.SetActive(true);
            view.m_btn_box_GameButton.gameObject.SetActive(true);
            view.m_pl_coin_CanvasGroup.gameObject.SetActive(true);
            if (!dict.TryGetValue(m_expeditionLevelFlagPrefabName, out m_expeditionLevelFlagPrefab))
            {
                return;
            }
            GameObject listItemObject = null;
            if(!dict.TryGetValue(view.m_sv_list_view_ListView.ItemPrefabDataList[0], out listItemObject))
            {
                return;
            }
            dict.Remove(m_expeditionLevelFlagPrefabName);
            RectTransform rect = listItemObject.transform as RectTransform;
            if (rect == null) return;
            m_listItemHeight = rect.rect.height;
            m_listItemWidth = rect.rect.width;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            funcTab.GetItemPrefabName = GetItemPrefabName;
            view.m_sv_list_view_ListView.SetInitData(dict, funcTab);
            InitLevelFlagUIData();
            view.m_sv_list_view_ListView.FillContent(m_listItemCount);
            RefreshUI();
            LocationLastUnlockLevel();

            if (m_selectedLevelId == 1 && m_selectedLevelId == m_lastUnlockLevelId)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.Expedition);
            }

            if (view.data != null)
            {
                if (view.data is GOScrptGuide)
                {
                    GoScrptGuide(view.data as GOScrptGuide);
                }
            }
        }

        private void GoScrptGuide(GOScrptGuide guide)
        {
            switch (guide.taskType)
            {
                case EnumTaskType.LongJourney:
                    {
                        ShowGuide();
                    }
                    break;
            }
        }

        private void ShowGuide()
        {
            var selectCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(m_selectedLevelId);
            int itemIndex = Mathf.CeilToInt(selectCfg.battleCoordinate[1] / m_listItemHeight) - 1;
            var listItem = view.m_sv_list_view_ListView.GetItemByIndex(itemIndex);
            if (listItem == null) return;
            Transform levelFlagParent = GetFlagParent(listItem);
            if (levelFlagParent == null) return;
            int flagIndex = 0;
            List<ExpeditionLevelFlagUIData> flagUIs = null;
            if (!m_dictLevelFlagsUIDatas.TryGetValue(itemIndex, out flagUIs))
            {
                return;
            }
            for(int i = 0; i < flagUIs.Count; ++i)
            {
                if(flagUIs[i].ExpeditionId == m_selectedLevelId)
                {
                    flagIndex = i;
                    break;
                }
            }
            var flagTransform = levelFlagParent.GetChild(flagIndex);
            if (flagTransform == null) return;
            FingerTargetParam param = new FingerTargetParam();
            param.AreaTarget = flagTransform.gameObject;
            param.ArrowDirection = (int)EnumArrorDirection.Up;
            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            int index = item.index % 3;
            return view.m_sv_list_view_ListView.ItemPrefabDataList[index];
        }

        private Transform GetFlagParent(ListView.ListItem item)
        {
            Transform levelFlagParent = null;
            if (item.data == null) return null;
            switch (item.index % 3)
            {
                case 0:
                    {
                        var subView = item.data as UI_Item_ExpeditionFightMap1View;
                        if (subView != null)
                        {
                            levelFlagParent = subView.m_img_map_PolygonImage.transform;
                        }
                    }
                    break;
                case 1:
                    {
                        var subView = item.data as UI_Item_ExpeditionFightMap2View;
                        if (subView != null)
                        {
                            levelFlagParent = subView.m_img_map_PolygonImage.transform;
                        }
                    }
                    break;
                case 2:
                    {
                        var subView = item.data as UI_Item_ExpeditionFightMap3View;
                        if (subView != null)
                        {
                            levelFlagParent = subView.m_img_map_PolygonImage.transform;
                        }
                    }
                    break;
            }
            return levelFlagParent;
        }

        private void ItemEnter(ListView.ListItem item)
        {
            item.go.transform.SetSiblingIndex(item.index);
            Transform levelFlagParent = null;
            int index = item.index % 3;
            if(item.data != null)
            {
                levelFlagParent = GetFlagParent(item);
                if (levelFlagParent != null)
                {
                    foreach(Transform child in levelFlagParent)
                    {
                        CoreUtils.assetService.Destroy(child.gameObject);
                    }
                }
            }
            else
            {
                switch(index)
                {
                    case 0:
                        {
                            var subView = MonoHelper.AddHotFixViewComponent<UI_Item_ExpeditionFightMap1View>(item.go);
                            levelFlagParent = subView.m_img_map_PolygonImage.transform;
                            item.data = subView;
                        }
                        break;
                    case 1:
                        {
                            var subView = MonoHelper.AddHotFixViewComponent<UI_Item_ExpeditionFightMap2View>(item.go);
                            levelFlagParent = subView.m_img_map_PolygonImage.transform;
                            item.data = subView;
                        }
                        break;
                    case 2:
                        {
                            var subView = MonoHelper.AddHotFixViewComponent<UI_Item_ExpeditionFightMap3View>(item.go);
                            levelFlagParent = subView.m_img_map_PolygonImage.transform;
                            item.data = subView;
                        }
                        break;
                }
            }

            List<ExpeditionLevelFlagUIData> levelFlagsInItem = null;
            if(!m_dictLevelFlagsUIDatas.TryGetValue(item.index, out levelFlagsInItem))
            {
                return;
            }
            for (int i = 0; i < levelFlagsInItem.Count; ++i)
            {
                var dataInItem = levelFlagsInItem[i];
                var expeditionCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(dataInItem.ExpeditionId);
                if (expeditionCfg == null) continue;
                ExpeditionInfo expeditionInfo = null;
                if(m_playerProxy.CurrentRoleInfo.expeditionInfo != null)
                {
                    m_playerProxy.CurrentRoleInfo.expeditionInfo.TryGetValue(expeditionCfg.ID, out expeditionInfo);
                }
                var levelFlagObject = CoreUtils.assetService.Instantiate(m_expeditionLevelFlagPrefab);
                levelFlagObject.name = $"{levelFlagObject.name} : {expeditionCfg.ID}";
                levelFlagObject.transform.SetParent(levelFlagParent);
                RectTransform rect = levelFlagObject.transform as RectTransform;
                rect.localScale = Vector3.one;
                rect.anchoredPosition = dataInItem.PosInItem;
                UI_Item_ExpeditionMapLevel_SubView levelFlagView = new UI_Item_ExpeditionMapLevel_SubView(rect);
                levelFlagView.Show((ExpeditionLevelType)expeditionCfg.type, expeditionInfo, expeditionCfg,
                    expeditionInfo != null || (m_playerProxy.CurrentRoleInfo.expeditionInfo != null &&
                    m_playerProxy.CurrentRoleInfo.expeditionInfo.ContainsKey(expeditionCfg.frontNumber) || expeditionCfg.frontNumber == 0));
                if (m_selectedLevelId == expeditionCfg.ID)
                {
                    ShowSelectedEffect(levelFlagView.m_root_RectTransform);
                }
                levelFlagView.AddClickListener(() =>
                {
                    OnClickedLevelFlag(expeditionCfg, expeditionInfo);
                    ShowSelectedEffect(levelFlagView.m_root_RectTransform);

                });
            }
        }

        private void Hide()
        {
            view.gameObject.SetActive(false);
        }

        private void Show()
        {
            view.gameObject.SetActive(true);
        }

        private void RefreshUI()
        {
            RefreshSelectedLevelInfo();
            RefreshRewardCD();
            RefreshCurrecny();
        }

        private void RefreshCurrecny()
        {
            view.m_UI_medal.SetRes((int)EnumCurrencyType.conquerorMedal, m_playerProxy.CurrentRoleInfo.expeditionCoin,1);
            view.m_UI_gem.SetRes((int)EnumCurrencyType.denar, m_playerProxy.CurrentRoleInfo.denar,1);
        }

        private void RefreshExpeditionRewardInfo()
        {
            view.m_sv_list_view_ListView.ForceRefresh();
        }

        private Transform m_selectedEffect;
        private Transform m_effectTarget;
        private bool m_isLoadingEffect;
        private void ShowSelectedEffect(Transform target)
        {
            if (m_selectedEffect == null)
            {
                if (m_isLoadingEffect)
                {
                    m_effectTarget = target;
                    return;
                }
                m_isLoadingEffect = true;
                ClientUtils.UIAddEffect("UI_10047",target, (obj) =>
                {
                    m_isLoadingEffect = false;
                    m_selectedEffect = obj.transform;
                    if (m_effectTarget != null)
                    {
                        m_selectedEffect.SetParent(m_effectTarget);
                    }
                    m_selectedEffect.localPosition = Vector3.zero;
                    m_selectedEffect.localScale = Vector3.one;
                });
            }
            else
            {
                m_selectedEffect.SetParent(target);
                m_selectedEffect.localPosition = Vector3.zero;
            }
        }

        private void OnClickedLevelFlag(Data.ExpeditionDefine cfg, ExpeditionInfo info)
        {
            if(info == null || info.reward || info.star < 3)
            {
                m_selectedLevelId = cfg.ID;
                ClientUtils.PlayUIAnimation(view.m_pl_buttom_Animator, "Show");
                RefreshSelectedLevelInfo();
            }
            else
            {
                Expedition_OneKeyAward.request request = new Expedition_OneKeyAward.request()
                {
                    id = cfg.ID,
                };
                AppFacade.GetInstance().SendSproto(request);
            }
        }

        private void RefreshSelectedLevelInfo()
        {
            Data.ExpeditionDefine selectLevelCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(m_selectedLevelId);
            if (selectLevelCfg == null) return;
            view.m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(805048, selectLevelCfg.level, m_maxExpeditionLevel);
            ExpeditionInfo info = null;
            if(m_playerProxy.CurrentRoleInfo.expeditionInfo != null)
            {
                m_playerProxy.CurrentRoleInfo.expeditionInfo.TryGetValue(m_selectedLevelId, out info);
            }
            view.m_lbl_rewardget1_LanguageText.text = info != null ? LanguageUtils.getText(805006) : LanguageUtils.getText(805007);
            view.m_img_starHighlight1_PolygonImage.gameObject.SetActive(info != null && info.star >= 1);
            view.m_img_starHighlight2_PolygonImage.gameObject.SetActive(info != null && info.star >= 2);
            view.m_img_starHighlight3_PolygonImage.gameObject.SetActive(info != null && info.star >= 3);
            if(info == null)
            {
                RefreshRewards(selectLevelCfg.firstReward, m_rewardSubViews1);                
            }
            view.m_pl_reward1_GridLayoutGroup.gameObject.SetActive(info == null);
            RefreshRewards(new List<int> { selectLevelCfg.reward1, selectLevelCfg.reward2, selectLevelCfg.reward3 }, m_rewardSubViews2);

            view.m_img_firstBg_PolygonImage.gameObject.SetActive(info == null);           

            view.m_img_blueman0_PolygonImage.gameObject.SetActive(selectLevelCfg.troopsNumber >= 1);
            view.m_img_blueman1_PolygonImage.gameObject.SetActive(selectLevelCfg.troopsNumber >= 2);
            view.m_img_blueman2_PolygonImage.gameObject.SetActive(selectLevelCfg.troopsNumber >= 3);
            view.m_img_blueman3_PolygonImage.gameObject.SetActive(selectLevelCfg.troopsNumber >= 4);
            view.m_img_blueman4_PolygonImage.gameObject.SetActive(selectLevelCfg.troopsNumber >= 5);

            view.m_btn_challage.m_btn_languageButton_GameButton.interactable = info != null || m_selectedLevelId == m_lastUnlockLevelId;
            view.m_btn_challage.m_img_forbid_PolygonImage.gameObject.SetActive(info == null && m_selectedLevelId != m_lastUnlockLevelId);
        }

        private void RefreshRewards(List<int> rewardGroupIds, List<UI_Model_RewardGet_SubView> subViews)
        {
            for (int i = 0; i < subViews.Count; ++i)
            {
                if (rewardGroupIds.Count > i)
                {
                    List<Data.ItemPackageDefine> rewards = m_rewardGroupProxy.GetRewardByGroupId(rewardGroupIds[i]);
                    if(rewards.Count > 0)
                    {
                        subViews[i].gameObject.SetActive(true);
                        subViews[i].RefreshReward(rewards[0]);
                    }                   
                }
                else
                {
                    subViews[i].gameObject.SetActive(false);
                }
            }
        }

        private void RefreshRewards(int rewardGroupId, List<UI_Model_RewardGet_SubView> subViews)
        {
            List<Data.ItemPackageDefine> rewards = m_rewardGroupProxy.GetRewardByGroupId(rewardGroupId);
            for (int i = 0; i < subViews.Count; ++i)
            {
                if (rewards.Count > i)
                {
                    subViews[i].gameObject.SetActive(true);
                    subViews[i].RefreshReward(rewards[i]);
                }
                else
                {
                    subViews[i].gameObject.SetActive(false);
                }
            }
        }

        private void RefreshRewardCD()
        {
            bool isCanGetReward = false;
            if(m_playerProxy.CurrentRoleInfo.expeditionInfo != null)
            {
                foreach (var info in m_playerProxy.CurrentRoleInfo.expeditionInfo)
                {
                    if (!info.Value.reward && info.Value.star == 3)
                    {
                        isCanGetReward = true;
                        break;
                    }
                }
            }
            
            view.m_btn_box_GameButton.interactable = isCanGetReward;
            view.m_lbl_time_LanguageText.gameObject.SetActive(!isCanGetReward);
            if (isCanGetReward)
            {
                view.m_img_box_GrayChildrens.Normal();
                view.m_UI_Model_AnimationBox.SetBox(false, true);
                view.m_UI_Model_AnimationBox.m_UI_Model_AnimationBox_GrayChildrens.NormalSkeletonGraphic();
            }
            else
            {
                view.m_img_box_GrayChildrens.Gray();
                view.m_UI_Model_AnimationBox.SetBox(true, true);
                view.m_UI_Model_AnimationBox.m_UI_Model_AnimationBox_GrayChildrens.GraySkeletonGraphic();
                
            }

            if (!isCanGetReward)
            {            
                if(m_getRewardCdTimer == null)
                {
                    m_getRewardCdTimer = Timer.Register(1, UpdateGetRewardCdTime, null, true);
                }
                UpdateGetRewardCdTime();
            }
            else
            {
                ClearGetRewardCdTimer();
            }
        }

        private void UpdateGetRewardCdTime()
        {
            long leftTime = ServerTimeModule.Instance.GetDistanceZeroTime();
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)leftTime);
        }

        private void ClearGetRewardCdTimer()
        {
            if (m_getRewardCdTimer != null)
            {
                m_getRewardCdTimer.Cancel();
                m_getRewardCdTimer = null;
            }
        }

        private void OnClickedOneKeyGetReward()
        {
            Expedition_OneKeyAward.request request = new Expedition_OneKeyAward.request();
            AppFacade.GetInstance().SendSproto(request);
            CoreUtils.audioService.PlayOneShot("Sound_Ui_OpenBox");
        }

        private bool GetExpeditionFlagIndexById(int id, ref int mapIndex, ref int flagIndex)
        {
            var expeditionCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(id);
            if (expeditionCfg == null) return false;
            mapIndex = Mathf.CeilToInt(expeditionCfg.battleCoordinate[1] / m_listItemHeight) - 1;
            List<ExpeditionLevelFlagUIData> flagDatas = null;
            if(!m_dictLevelFlagsUIDatas.TryGetValue(mapIndex, out flagDatas))
            {
                return false;
            }
            for(int i = 0; i < flagDatas.Count; ++i)
            {
                if(flagDatas[i].ExpeditionId == id)
                {
                    flagIndex = i;
                    return true;
                }
            }
            return false;
        }

        private Transform GetFlagTransformByIndex(int mapIndex, int flagIndex)
        {
            var item = view.m_sv_list_view_ListView.GetItemByIndex(mapIndex);
            if (item == null) return null;
            var flagParent = GetFlagParent(item);
            if (flagParent == null) return null;
            return flagParent.GetChild(flagIndex);
        }

        private void OnOneKeyGetRewardResponse(Expedition_OneKeyAward.response response)
        {
            if(response.id == 0)
            {
                CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, response.rewardInfo);
            }
            else
            {
                int mapIndex = 0, flagIndex = 0;
                if(GetExpeditionFlagIndexById((int)response.id, ref mapIndex, ref flagIndex))
                {
                    var flagTransform = GetFlagTransformByIndex(mapIndex, flagIndex);
                    if(flagTransform != null)
                    {
                        RefreshFlyReward(flagTransform, response.rewardInfo);
                    }
                }                
            }
            RefreshExpeditionRewardInfo();
            RefreshCurrecny();
        }


        private void RefreshFlyReward(Transform flagTransform, RewardInfo reward)
        {
            var rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            var rewardGroupDatas = rewardGroupProxy.GetRewardDataByRewardInfo(reward);
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            for (int i = 0; i < rewardGroupDatas.Count; i++)
            {
                RewardGroupData rewardGroupData = rewardGroupDatas[i];
                switch ((EnumRewardType)rewardGroupData.RewardType)
                {
                    case EnumRewardType.Currency:
                        {                           
                            mt.FlyUICurrency(rewardGroupData.CurrencyData.ID, (int)rewardGroupData.number, flagTransform.position);

                        }
                        break;
                    case EnumRewardType.Item:
                        {
                            mt.FlyItemEffect(rewardGroupData.ItemData.ID, (int)rewardGroupData.number, flagTransform.position);
                        }
                        break;
                }
            }
        }

        private int GetLastUnlockLevelId()
        {
            if(m_playerProxy.CurrentRoleInfo.expeditionInfo == null)
            {
                return 1;
            }
            var allExpditions = CoreUtils.dataService.QueryRecords<Data.ExpeditionDefine>();
            if(allExpditions.Count == m_playerProxy.CurrentRoleInfo.expeditionInfo.Count)
            {
                return allExpditions[allExpditions.Count - 1].ID;
            }
            int unlockLevelId = allExpditions[0].ID;
            foreach (var expdition in allExpditions)
            {
                if(!m_playerProxy.CurrentRoleInfo.expeditionInfo.ContainsKey(expdition.ID) &&
                    m_playerProxy.CurrentRoleInfo.expeditionInfo.ContainsKey(expdition.frontNumber))
                {
                    unlockLevelId = expdition.ID;
                    break;
                }
            }
            return unlockLevelId;
        }

        private void LocationLastUnlockLevel()
        {
            var cfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(m_selectedLevelId);
            if (cfg == null) return;
            Vector2 pos = new Vector2(cfg.battleCoordinate[0], cfg.battleCoordinate[1]);
            float viewSize = view.m_sv_list_view_ListView.GetComponent<RectTransform>().rect.height;
            float totalHeight = view.m_sv_list_view_ListView.listContainer.rect.height;
            float targetPos = pos.y - viewSize / 2;
            targetPos = Mathf.Max(0, targetPos);
            targetPos = Mathf.Min(totalHeight - viewSize, targetPos);
            view.m_sv_list_view_ListView.SetContainerPos(targetPos);
        }

        private void OnClickedChallageButton()
        {
            var cfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionDefine>(m_selectedLevelId);
            if (cfg == null) return;
            m_expeditionProxy.LastSelectedLevelId = m_selectedLevelId;
            AppFacade.GetInstance().SendNotification(CmdConstant.EnterExpeditionMap, cfg);
        }

        private void OnClickedStore()
        {
            TipRemindProxy.SaveRemind(TipRemindProxy.ExpeiditonStoreResetRemind);
            view.m_UI_Common_Redpoint.HideRedPoint();
            RunStoreResetTimer();
            CoreUtils.uiManager.ShowUI(UI.s_expeditionStore);
        }

        private void OnClickedRank()
        {
            CoreUtils.uiManager.ShowUI(UI.s_ranking, null, new RankingViewData()
            {
                TargetLeaderboard = CoreUtils.dataService.QueryRecord<Data.LeaderboardDefine>(208),
            });
        }

        private void OnClickedRule()
        {
            CoreUtils.uiManager.ShowUI(UI.s_expeditionRule);        
        }

        private void InitLevelFlagUIData()
        {
            m_maxExpeditionLevel = 0;
            m_listItemCount = 0;
            var allExpditions = CoreUtils.dataService.QueryRecords<Data.ExpeditionDefine>();
            foreach (var expeditionCfg in allExpditions)
            {
                if (m_maxExpeditionLevel < expeditionCfg.level)
                {
                    m_maxExpeditionLevel = expeditionCfg.level;
                }
                int itemIndex = Mathf.CeilToInt(expeditionCfg.battleCoordinate[1] / m_listItemHeight) - 1;
                if(m_listItemCount < itemIndex + 1)
                {
                    m_listItemCount = itemIndex + 1;
                }
                List<ExpeditionLevelFlagUIData> levelFlagDatasInItem = null;
                if (!m_dictLevelFlagsUIDatas.TryGetValue(itemIndex, out levelFlagDatasInItem))
                {
                    levelFlagDatasInItem = new List<ExpeditionLevelFlagUIData>();
                    m_dictLevelFlagsUIDatas.Add(itemIndex, levelFlagDatasInItem);
                }
                levelFlagDatasInItem.Add(new ExpeditionLevelFlagUIData()
                {
                    ExpeditionId = expeditionCfg.ID,
                    PosInItem = CalLevelFlagYInItem(itemIndex, expeditionCfg.battleCoordinate[0], expeditionCfg.battleCoordinate[1]),
                });
            }
            m_expeditionProxy.MaxExpeditionLevel = m_maxExpeditionLevel;
        }

        private Vector2 CalLevelFlagYInItem(int itemIndex, float posX, float posY)
        {
            return new Vector2(posX - m_listItemWidth / 2, m_listItemHeight / 2 - (posY - itemIndex * m_listItemHeight));
        }

        private void InitStoreRedPoint()
        {
            if (TipRemindProxy.IsShowRemind(TipRemindProxy.ExpeiditonStoreResetRemind))
            {
                view.m_UI_Common_Redpoint.ShowSmallRedPoint(1);
            }
            else
            {
                view.m_UI_Common_Redpoint.HideRedPoint();
                RunStoreResetTimer();
            }
        }

        private void RefreshStoreRedPoint()
        {
            if (TipRemindProxy.IsShowRemind(TipRemindProxy.ExpeiditonStoreResetRemind))
            {
                view.m_UI_Common_Redpoint.ShowSmallRedPoint(1);
                ClearStoreResetTimer();
            }
        }

        private void ClearStoreResetTimer()
        {
            if (m_timerForStoreReset != null)
            {
                m_timerForStoreReset.Cancel();
                m_timerForStoreReset = null;
            }
        }

        private void RunStoreResetTimer()
        {
            ClearStoreResetTimer();
            m_timerForStoreReset = Timer.Register(1, () =>
            {
                RefreshStoreRedPoint();
            }, null, true);
        }

        private Timer m_getRewardCdTimer = null;
        private float m_listItemWidth = 0;
        private float m_listItemHeight = 0;
        private int m_lastUnlockLevelId = 0;
        private int m_selectedLevelId = 0;
        private int m_maxExpeditionLevel = 0;
        private PlayerProxy m_playerProxy = null;
        private readonly string m_expeditionLevelFlagPrefabName = "UI_Item_ExpeditionMapLevel";
        private GameObject m_expeditionLevelFlagPrefab = null;
        private ExpeditionProxy m_expeditionProxy = null;
        private RewardGroupProxy m_rewardGroupProxy = null;
        private Dictionary<int, List<ExpeditionLevelFlagUIData>> m_dictLevelFlagsUIDatas = new Dictionary<int, List<ExpeditionLevelFlagUIData>>();
        private int m_listItemCount = 0;
        private List<UI_Model_RewardGet_SubView> m_rewardSubViews1 = new List<UI_Model_RewardGet_SubView>();
        private List<UI_Model_RewardGet_SubView> m_rewardSubViews2 = new List<UI_Model_RewardGet_SubView>();
        private Timer m_timerForStoreReset = null;
        private string m_cacheBgm = string.Empty;
    }
}