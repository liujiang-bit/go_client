// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Win_ExpeditionStoreMediator
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
using Spine.Unity;

namespace Game {
    public class UI_Win_ExpeditionStoreMediator : GameMediator {
        private enum HeroSpineShowType
        {
            Normal,
            Left, 
            Right,
        }

        #region Member
        public static string NameMediator = "UI_Win_ExpeditionStoreMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_ExpeditionStoreMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ExpeditionStoreView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Shop_GetLimitHeroInfo.TagName,
                Shop_BuyExpeditionStore.TagName,
                Shop_RefreshExpeditionStore.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Shop_GetLimitHeroInfo.TagName:
                    {
                        Shop_GetLimitHeroInfo.response response = notification.Body as Shop_GetLimitHeroInfo.response;
                        if (response != null)
                        {
                            OnGetLimitHeroInfo(response);
                        }
                    }
                    break;
                case Shop_BuyExpeditionStore.TagName:
                    {
                        Shop_BuyExpeditionStore.response response = notification.Body as Shop_BuyExpeditionStore.response;
                        if (response != null && response.result)
                        {
                            RefreshItems();
                            RefreshHead2And3();
                            RefreshCurrecny();
                            ItemFly();
                        }                        
                    }
                    break;
                case Shop_RefreshExpeditionStore.TagName:
                    {
                        Shop_RefreshExpeditionStore.response response = notification.Body as Shop_RefreshExpeditionStore.response;
                        if (response != null && response.result)
                        {
                            RefreshItems();
                            RefreshRefreshTimes();
                            RefreshCurrecny();
                        }                        
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
            if(m_nextRefreshTimer != null)
            {
                m_nextRefreshTimer.Cancel();
                m_nextRefreshTimer = null;
            }
            if(m_timerAutoShowNextHeroSpine != null)
            {
                m_timerAutoShowNextHeroSpine.Cancel();
                m_timerAutoShowNextHeroSpine = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (m_playerProxy == null) return;
            m_configCfg = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0);
            if (m_configCfg == null) return;

            m_shopItems.Add(view.m_pl_storeItem1);
            m_shopItems.Add(view.m_pl_storeItem2);
            m_shopItems.Add(view.m_pl_storeItem3);
            m_shopItems.Add(view.m_pl_storeItem4);
            m_shopItems.Add(view.m_pl_storeItem5);
            m_shopItems.Add(view.m_pl_storeItem6);
            Shop_GetLimitHeroInfo.request request = new Shop_GetLimitHeroInfo.request();
            AppFacade.GetInstance().SendSproto(request);
            RefreshItems();
            RefreshHead2And3();
            RefreshRefreshTimes();
            view.m_pl_horeSpineBefore_SkeletonGraphic.gameObject.SetActive(false);
            RefreshCurrecny();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_expeditionStore);
            });
            view.m_btn_info_GameButton.onClick.AddListener(OnClickedInfoButton);
            view.m_btn_left_GameButton.onClick.AddListener(OnClickPreHeroSpine);
            view.m_btn_right_GameButton.onClick.AddListener(OnClickNextHeroSpine);
        }

        protected override void BindUIData()
        {

        }

        #endregion
        

        private void RefreshHead2And3()
        {
            var heroHead2 = m_configCfg.heroHead2;
            if(heroHead2.Count >=2)
            {
            AddUiEffect(view.m_pl_item_effect2, heroHead2[0]);
                view.m_pl_commodity2.RefreshHead(heroHead2[0], heroHead2[1]);
                view.m_pl_commodity2.RemoveAllButtonListener();
                view.m_pl_commodity2.AddBuyButtonListener(() =>
                {
                    SendBuyHeadMsg(2, heroHead2[1]);
                    if (m_playerProxy.CurrentRoleInfo.expeditionCoin >= heroHead2[1])
                    {
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyItemEffect(heroHead2[0], 1, view.m_pl_commodity2.m_btn_buy.m_root_RectTransform);
                    }
                });
            }

            var heroHead3 = m_configCfg.heroHead3;
            if(heroHead3.Count >=3)
            {
                AddUiEffect(view.m_pl_item_effect3, heroHead3[0]);
                view.m_pl_commodity3.RefreshHead(heroHead3[0], heroHead3[1], heroHead3[2] - (int)m_playerProxy.CurrentRoleInfo.expedition.headCount);
                view.m_pl_commodity3.RemoveAllButtonListener();
                view.m_pl_commodity3.AddBuyButtonListener(() =>
                {
                    if(m_playerProxy.CurrentRoleInfo.expedition.headCount >= heroHead3[2])
                    {
                        return;
                    }
                    SendBuyHeadMsg(3, heroHead3[1]);
                    if (m_playerProxy.CurrentRoleInfo.expeditionCoin >= heroHead3[1])
                    {
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyItemEffect(heroHead3[0], 1, view.m_pl_commodity3.m_btn_buy.m_root_RectTransform);
                    }
                });
            }
        }

        private void RefreshHead1()
        {
            var headCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionHeadDefine>(m_curHead1Id);
            if (headCfg == null) return;
            AddUiEffect(view.m_pl_item_effect1, headCfg.itemID);
            view.m_pl_commodity1.RefreshHead(headCfg.itemID, headCfg.price);
            view.m_pl_commodity1.RemoveAllButtonListener();
            view.m_pl_commodity1.AddBuyButtonListener(() =>
            {
                SendBuyHeadMsg(1, headCfg.price);
                if (m_playerProxy.CurrentRoleInfo.expeditionCoin >= headCfg.price)
                {
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyItemEffect(headCfg.itemID, 1, view.m_pl_commodity1.m_btn_buy.m_root_RectTransform);
                }
   
            });
            UpdateHead1NextRefreshTime();
            m_nextRefreshTimer = Timer.Register(1, () =>
            {
                UpdateHead1NextRefreshTime();               
            }, null, true);            
            m_curHeroSpineIndex = 1;
            RefreshHeroSpine(HeroSpineShowType.Normal);
        }
        #region 特效
        private List<int> AssetloadedList = new List<int>();
        private void AddUiEffect(Transform targetPart, int itemId)
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(itemId);
            if (itemCfg == null) return;
            if (m_configCfg.expeditionHeadeffect.Count>= itemCfg.quality)
            {
                if (!AssetloadedList.Contains(itemId))
                {
                    AssetloadedList.Add(itemId);
                    string assetname = m_configCfg.expeditionHeadeffect[itemCfg.quality - 1];
                    ClientUtils.UIAddEffect(assetname, targetPart, (go) =>
                    {

                    });
                }
            }
        }
        #endregion

        private void UpdateHead1NextRefreshTime()
        {
            int time = (int)(m_head1NextRefreshTime - ServerTimeModule.Instance.GetServerTime());
            if (time < 0) time = 0;
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(time);
            if (time == 0 && m_nextRefreshTimer != null)
            {
                m_nextRefreshTimer.Cancel();
                m_nextRefreshTimer = null;
            }
        }

        private void RefreshHeroSpine(HeroSpineShowType showType)
        {
            int heroId = GetHeroIdByIndex();
            Data.HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<Data.HeroDefine>(heroId);
            if (heroDefine == null)
            {
                return;
            }
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(heroDefine.l_nameID);
            view.m_lbl_time_LanguageText.gameObject.SetActive(m_curHeroSpineIndex == 1);
            view.m_bl_et_PolygonImage.gameObject.SetActive(m_curHeroSpineIndex == 1);
            view.m_lbl_et_LanguageText.gameObject.SetActive(m_curHeroSpineIndex == 1);
            view.m_btn_info_GameButton.gameObject.SetActive(m_curHeroSpineIndex == 1);
            switch(showType)
            {
                case HeroSpineShowType.Normal:
                    {
                        ClientUtils.LoadSpine(view.m_pl_horeSpineBefore_SkeletonGraphic, heroDefine.heroModel, ()=>
                        {
                            if (view == null || view.gameObject == null) return;
                            view.m_pl_horeSpineBefore_SkeletonGraphic.gameObject.SetActive(true);
                        });
                        m_curSkeleton = view.m_pl_horeSpineBefore_SkeletonGraphic;
                    }
                    break;
                case HeroSpineShowType.Left:
                    {
                        ShowHeroSwitchAnimation(m_curSkeleton, "ToHide_ArrowLeft");
                        m_curSkeleton = GetAnotherHero();
                        ClientUtils.LoadSpine(m_curSkeleton, heroDefine.heroModel);
                        ShowHeroSwitchAnimation(m_curSkeleton, "ToShow_ArrowLeft");
                    }
                    break;
                case HeroSpineShowType.Right:
                    {
                        ShowHeroSwitchAnimation(m_curSkeleton, "ToHide_ArrowRight");
                        m_curSkeleton = GetAnotherHero();
                        ClientUtils.LoadSpine(m_curSkeleton, heroDefine.heroModel);
                        ShowHeroSwitchAnimation(m_curSkeleton, "ToShow_ArrowRight");
                    }
                    break;
            }
            if(m_timerAutoShowNextHeroSpine != null)
            {
                m_timerAutoShowNextHeroSpine.Cancel();
                m_timerAutoShowNextHeroSpine = null;
            }
            m_timerAutoShowNextHeroSpine = Timer.Register(m_configCfg.heroHeadTime * 1.0f / 1000, OnTimerShowNextHeroSpine, null, true);
        }

        private SkeletonGraphic GetAnotherHero()
        {
            if(m_curSkeleton == view.m_pl_horeSpineBefore_SkeletonGraphic)
            {
                return view.m_pl_horeSpineAfter_SkeletonGraphic;
            }
            else
            {
                return view.m_pl_horeSpineBefore_SkeletonGraphic;
            }
        }

        private void ShowHeroSwitchAnimation(SkeletonGraphic hero, string animation)
        {
            if (hero == null)
            {
                return;
            }

            Animator animator = hero.GetComponent<Animator>();
            if(animator != null)
            {
                animator.Play(animation, 0);
            }

        }

        private void OnClickPreHeroSpine()
        {
            m_curHeroSpineIndex--;
            if(m_curHeroSpineIndex <=0)
            {
                m_curHeroSpineIndex = 3;
            }

            RefreshHeroSpine(HeroSpineShowType.Left);
        }

        private void OnTimerShowNextHeroSpine()
        {
            OnClickNextHeroSpine();
        }

        private void OnClickNextHeroSpine()
        {
            m_curHeroSpineIndex++;
            if (m_curHeroSpineIndex >3)
            {
                m_curHeroSpineIndex = 1;
            }
            RefreshHeroSpine(HeroSpineShowType.Right);
        }

        private int GetHeroIdByIndex()
        {
            int heroId = 0;
            switch(m_curHeroSpineIndex)
            {
                case 1:
                    {
                        var headCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionHeadDefine>(m_curHead1Id);
                        if(headCfg != null)
                        {
                            heroId = headCfg.heroID;
                        }
                    }
                    break;
                case 2:
                    heroId = m_configCfg.heroHead2[2];
                    break;
                case 3:
                    heroId = m_configCfg.heroHead3[3];
                    break;
            }
            return heroId;
        }

        private void OnGetLimitHeroInfo(Shop_GetLimitHeroInfo.response response)
        {
            m_curHead1Id = (int)response.id;
            m_head1NextRefreshTime = response.nextRefreshTime;
            RefreshHead1();
        }

        private void RefreshRefreshTimes()
        {
            int refreshTimes = m_configCfg.refreshPrice.Count - (int)m_playerProxy.CurrentRoleInfo.expedition.refreshCount;
            view.m_lbl_refreshNum_LanguageText.text = LanguageUtils.getTextFormat(805032, refreshTimes, m_configCfg.refreshPrice.Count);
            int refreshCost = 0;
            if (m_playerProxy.CurrentRoleInfo.expedition.refreshCount >= m_configCfg.refreshPrice.Count)
            {
                refreshCost = m_configCfg.refreshPrice[m_configCfg.refreshPrice.Count - 1];
            }
            else
            {
                refreshCost = m_configCfg.refreshPrice[(int)m_playerProxy.CurrentRoleInfo.expedition.refreshCount];
            }
            if (refreshCost == 0)
            {
                view.m_lbl_free_LanguageText.gameObject.SetActive(true);
                view.m_UI_Model_Resources.gameObject.SetActive(false);
            }
            else
            {
                view.m_lbl_free_LanguageText.gameObject.SetActive(false);
                view.m_UI_Model_Resources.gameObject.SetActive(true);
                view.m_UI_Model_Resources.m_lbl_val_LanguageText.text = ClientUtils.FormatComma(refreshCost);
                if(m_playerProxy.CurrentRoleInfo.denar < refreshCost)
                {
                    view.m_UI_Model_Resources.m_lbl_val_LanguageText.color = Color.red;
                }
                else
                {
                    view.m_UI_Model_Resources.m_lbl_val_LanguageText.color = Color.white;
                }
            }
            view.m_btn_reflash_GameButton.interactable = m_playerProxy.CurrentRoleInfo.expedition.refreshCount < m_configCfg.refreshPrice.Count;
            view.m_btn_reflash_GameButton.onClick.RemoveAllListeners();
            view.m_btn_reflash_GameButton.onClick.AddListener(() =>
            {
                if (m_playerProxy.CurrentRoleInfo.expedition.refreshCount >= m_configCfg.refreshPrice.Count) return;
                var currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                if (currencyProxy.ShortOfDenar(refreshCost))
                {                   
                    return;
                }
                if (refreshCost <= 0)
                {
                    Shop_RefreshExpeditionStore.request request = new Shop_RefreshExpeditionStore.request();
                    AppFacade.GetInstance().SendSproto(request);
                }
                else
                {
                    UIHelper.DenarCostRemain(refreshCost, () =>
                    {
                        Shop_RefreshExpeditionStore.request request = new Shop_RefreshExpeditionStore.request();
                        AppFacade.GetInstance().SendSproto(request);
                    });
                }
               
            });
        }

        private void RefreshItems()
        {
            if (m_playerProxy.CurrentRoleInfo == null) return;
            List<Expedition.ShopItem> items = new List<Expedition.ShopItem>(m_playerProxy.CurrentRoleInfo.expedition.shopItem.Values);
            items.Sort((Expedition.ShopItem t1, Expedition.ShopItem t2) =>
            {
                return t1.itemId.CompareTo(t2.itemId);
            });
            for(int i = 0; i < items.Count && i < m_shopItems.Count; ++i)
            {
                int itemId = (int)items[i].itemId;
                m_shopItems[i].RefreshItem(itemId, items[i].buyCount > 0);
                m_shopItems[i].RemoveAllButtonListener();
                m_shopItems[i].AddBuyButtonListener(() =>
                {
                    SendBuyItemMsg(itemId);
                });
            }  
        }

        private void SendBuyItemMsg(int id)
        {
            Data.ExpeditionShopDefine shopCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionShopDefine>(id);
            if (shopCfg == null) return;
            if (m_playerProxy.CurrentRoleInfo.expeditionCoin < shopCfg.price)
            {
                Tip.CreateTip(805046).Show();
                return;
            }
            m_curBuyItemIDs.Enqueue(id);
            Shop_BuyExpeditionStore.request request = new Shop_BuyExpeditionStore.request()
            {
                type = 2,
                itemId = id,
            };
            AppFacade.GetInstance().SendSproto(request);
        }

        private void SendBuyHeadMsg(int index, int price)
        {
            if (m_playerProxy.CurrentRoleInfo.expeditionCoin < price)
            {
                Tip.CreateTip(805046).Show();
                return;
            }
            Shop_BuyExpeditionStore.request request = new Shop_BuyExpeditionStore.request()
            {
                type = 1,
                itemId = index,
            };
            AppFacade.GetInstance().SendSproto(request);
        }

        private void OnClickedInfoButton()
        {
            HelpTip.CreateTip(6001, view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        private void RefreshCurrecny()
        {
            view.m_pl_conquerorMedal.SetRes((int)EnumCurrencyType.conquerorMedal, m_playerProxy.CurrentRoleInfo.expeditionCoin,1);
            view.m_pl_gem.SetRes((int)EnumCurrencyType.denar, m_playerProxy.CurrentRoleInfo.denar,1);
        }

        private void ItemFly()
        {
            if (m_curBuyItemIDs.Count <= 0)
            {
                return;
            }
            //飘飞特效
            var itemID = m_curBuyItemIDs.Dequeue();
            var shopItem = m_shopItems.Find(x => x.ItemID == itemID);
            if (shopItem == null)
            {
                return;
            }
            Data.ExpeditionShopDefine shopCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionShopDefine>(itemID);
            if (shopCfg == null) return;
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            mt.FlyItemEffect(shopCfg.itemID, 1, 
                shopItem.m_UI_Model_Item.m_root_RectTransform);

        }

        private PlayerProxy m_playerProxy = null;
        private List<UI_Item_ExpeditionStoreItem_SubView> m_shopItems = new List<UI_Item_ExpeditionStoreItem_SubView>();
        private Queue<int> m_curBuyItemIDs= new Queue<int>();
        private int m_curHeroSpineIndex = 0;
        private Data.ConfigDefine m_configCfg = null;
        private int m_curHead1Id = 0;
        private long m_head1NextRefreshTime = 0;
        private Timer m_nextRefreshTimer = null;
        private SkeletonGraphic m_curSkeleton = null;
        private Timer m_timerAutoShowNextHeroSpine = null;
    }
}