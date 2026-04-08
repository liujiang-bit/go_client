// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月23日
// Update Time         :    2020年4月23日
// Class Description   :    UI_Win_CaptainLevelUpMediator
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

    public class CaptainLevelUpViewData
    {
        public HeroProxy.Hero Captain { get; set; }
    }

    public class UI_Win_CaptainLevelUpMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_CaptainLevelUpMediator";
        public bool IsOpenUpdate = true;


        #endregion

        //IMediatorPlug needs
        public UI_Win_CaptainLevelUpMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_CaptainLevelUpView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Hero_AddHeroExp.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Hero_AddHeroExp.TagName:
                    {
                        var response = notification.Body as Hero_AddHeroExp.response;
                        if(response != null && response.result)
                        {
                            OnAddExpSuccess(response);                           
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

        public override void WinClose()
        {
            if(m_addExpEffectTimer != null)
            {
                m_addExpEffectTimer.Cancel();
                m_addExpEffectTimer = null;
            }
            if(m_levelUpEffectTimer != null)
            {
                m_levelUpEffectTimer.Cancel();
                m_levelUpEffectTimer = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            if (!m_isPlayingProgressEffect) return;
            view.m_pb_rogressBar_GameSlider.value += Time.deltaTime * 2;

            if (m_oldHeroLevel != m_hero.level)
            {
                var oldLevelConfig = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>(m_oldHeroLevel + m_hero.config.rare * 10000 - 1);
                view.m_lbl_num_LanguageText.text = $"{ClientUtils.FormatComma(oldLevelConfig.exp)}/{ClientUtils.FormatComma(oldLevelConfig.exp)}";
                if (view.m_pb_rogressBar_GameSlider.value >= 1)
                {
                    m_oldHeroLevel++;
                    view.m_pb_rogressBar_GameSlider.value = 0;
                    PlayHeroLevelUpEffect();
                    view.m_UI_Model_CaptainHead.SetHero(m_hero, m_oldHeroLevel);
                }
            }
            else
            {
                view.m_lbl_num_LanguageText.text = $"{ClientUtils.FormatComma(m_hero.exp)}/{ClientUtils.FormatComma(m_hero.levelConfig.exp)}";
                if (m_hero.levelConfig.exp == 0)
                {
                    view.m_pb_rogressBar_GameSlider.value = 1;
                    var levelConfig = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>(m_hero.level + m_hero.config.rare * 10000 - 2);
                    if (levelConfig != null)
                    {
                        view.m_lbl_num_LanguageText.text =
                            $"{ClientUtils.FormatComma(levelConfig.exp)}/{ClientUtils.FormatComma(levelConfig.exp)}";
                    }

                    m_isPlayingProgressEffect = false;
                }
                else
                {
                    float newValue = m_hero.exp * 1.0f / m_hero.levelConfig.exp;

                    if (view.m_pb_rogressBar_GameSlider.value >= newValue)
                    {
                        view.m_pb_rogressBar_GameSlider.value = newValue;
                        m_oldHeroLevel = m_hero.level;
                        m_isPlayingProgressEffect = false;
                    }
                }

            }
        }        

        protected override void InitData()
        {
            var data = view.data as CaptainLevelUpViewData;
            if (data == null) return;
            if (data.Captain == null) return;
            m_hero = data.Captain;
            if (m_hero == null) return;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (m_bagProxy == null) return;

            InitAddExpItemId(); 
            RefreshHeroInfo();
            bool hasExpItem = HasAddExpItem();
            view.m_lbl_NoneUse_LanguageText.gameObject.SetActive(!hasExpItem);
            if(hasExpItem)
            {
                ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, OnItemPrefabLoadFinish);
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.AddCloseEvent(OnCloseButtonClicked);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OnCloseButtonClicked()
        {
            CoreUtils.uiManager.CloseUI(UI.s_captainLevelUp);
        }

        private bool HasAddExpItem()
        {
            foreach (var item in m_listAddExpItems)
            {
                if (m_bagProxy.GetItemNum(item.ID) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnAddExpSuccess(Hero_AddHeroExp.response response)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUseExpBook);
            PlayerAddExpEffect((int)response.itemId, (int)response.itemNum);
            PlayProgressBarEffect();
            if(m_selectItemIndex != -1 && m_bagProxy.GetItemNum(m_listAddExpItems[m_selectItemIndex].ID) == 0)
            {
                m_listAddExpItems.RemoveAt(m_selectItemIndex);
                view.m_sv_list_ListView.RemoveAt(m_selectItemIndex);
                m_selectItemIndex = -1;
            }
            else
            {
                RefreshList();
            }
            view.m_lbl_NoneUse_LanguageText.gameObject.SetActive(!HasAddExpItem());
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_ListView.SetInitData(dict, funcTab);
            view.m_sv_list_ListView.FillContent(m_listAddExpItems != null ? m_listAddExpItems.Count : 0);
        }

        private void ItemEnter(ListView.ListItem item)
        {
            if (item.index >= m_listAddExpItems.Count) return;

            UI_Item_StandardUseItemView itemView = null;
            if(item.data == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
                item.data = itemView;
            }
            else
            {
                itemView = item.data as UI_Item_StandardUseItemView;
            }
            if (itemView == null) return;
            var itemCfg = m_listAddExpItems[item.index];

            itemView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(itemCfg, m_bagProxy.GetItemNum(itemCfg.ID), false);
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
            itemView.m_lbl_itemDesc_LanguageText.text = string.Format(LanguageUtils.getText(itemCfg.l_desID), ClientUtils.FormatComma(itemCfg.desData1));
            itemView.m_lbl_itemCount_LanguageText.text = string.Format(LanguageUtils.getText(300074), ClientUtils.FormatComma(m_bagProxy.GetItemNum(itemCfg.ID)));
            itemView.m_UI_Model_Blue_big.gameObject.SetActive(true);
            itemView.m_UI_Model_Yellow.gameObject.SetActive(false);
            itemView.m_pl_quick_ArabLayoutCompment.gameObject.SetActive(false);
            itemView.m_UI_Model_Blue_big.RemoveAllClickEvent();
            itemView.m_UI_Model_Blue_big.AddClickEvent(() =>
            {
                if(m_selectItemIndex != -1 && m_selectItemIndex != item.index)
                {
                    ShowQuickUsePop(itemCfg, m_selectItemIndex, false);
                    m_selectItemIndex = -1;
                }
                if(m_selectItemIndex == -1)
                {
                    m_selectItemIndex = item.index;
                    ShowQuickUsePop(itemCfg, m_selectItemIndex, true);
                }
                UseExpItem(itemCfg,  1);
            });
            if(m_selectItemIndex == item.index)
            {
                ShowQuickUsePop(itemCfg, m_selectItemIndex, true);
            }            
        }

        private void ShowQuickUsePop(Data.ItemDefine itemCfg, int index, bool isShow)
        {
            var listItem = view.m_sv_list_ListView.GetItemByIndex(index);
            if (listItem == null || listItem.data == null) return;
            var itemView = listItem.data as UI_Item_StandardUseItemView;
            if (itemView == null) return;

            isShow = isShow && m_bagProxy.GetItemNum(itemCfg.ID) > 0 && !(m_hero.IsMaxLevel() || m_hero.IsLevelLimitByStar());
            itemView.m_pl_quick_ArabLayoutCompment.gameObject.SetActive(isShow);
            if (!isShow) return;            
            itemView.m_UI_Model_StandardButton_MiniBlue.RemoveAllClickEvent();
            itemView.m_UI_Model_StandardButton_MiniBlue.AddClickEvent(() =>
            {
                int quickUseCount = getQuickUseCount(itemCfg);
                UseExpItem(itemCfg, quickUseCount);
            });
            itemView.m_UI_Model_StandardButton_MiniBlue.m_lbl_Text_LanguageText.text = string.Format(LanguageUtils.getText(145048),getQuickUseCount(itemCfg));
        }

        private int getQuickUseCount(Data.ItemDefine itemCfg)
        {
            int needExp = m_hero.levelConfig.exp - (int)m_hero.data.exp;
            return Mathf.Min((int)m_bagProxy.GetItemNum(itemCfg.ID), Mathf.CeilToInt(needExp * 1.0f / itemCfg.desData1));
        }

        private void PlayerAddExpEffect(int itemId, int itemCount)
        {
            if(m_addExpEffectTimer != null)
            {
                Timer.Cancel(m_addExpEffectTimer);
                m_addExpEffectTimer = null;
            }
            if(view.m_lbl_add_LanguageText.gameObject.activeSelf)
            {
                view.m_lbl_add_LanguageText.gameObject.SetActive(false);
            }
            view.m_lbl_add_LanguageText.gameObject.SetActive(true);
            view.m_lbl_add_LanguageText.text = $"+{GetAddExpValue(itemId, itemCount)}";
            float animationTime = ClientUtils.GetAnimationLength(view.m_lbl_add_Animator, 0);
            m_addExpEffectTimer = Timer.Register(animationTime, () =>
            {
                view.m_lbl_add_LanguageText.gameObject.SetActive(false);
                m_addExpEffectTimer = null;
            });
        }

        private void PlayProgressBarEffect()
        {
            m_isPlayingProgressEffect = true;
        }

        private void PlayHeroLevelUpEffect()
        {
            if (view.m_UI_Item_CaptainLevelUpOnHead.gameObject.activeSelf)
            {
                view.m_UI_Item_CaptainLevelUpOnHead.gameObject.SetActive(false);
            }
            view.m_UI_Item_CaptainLevelUpOnHead.gameObject.SetActive(true);

            int soldiersAddCount = m_hero.levelConfig.soldiers;
            var preLevelConfig = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>(m_hero.level + m_hero.config.rare * 10000 - 2);
            if(preLevelConfig != null)
            {
                soldiersAddCount -=  preLevelConfig.soldiers;
            }
            view.m_UI_Item_CaptainLevelUpOnHead.m_lbl_addatt1_LanguageText.text = string.Format(LanguageUtils.getText(145049), soldiersAddCount);
            if(m_hero.levelConfig.starEffectData == 0)
            {
                view.m_UI_Item_CaptainLevelUpOnHead.m_lbl_addatt_LanguageText.text = string.Empty;
            }
            else
            {
                view.m_UI_Item_CaptainLevelUpOnHead.m_lbl_addatt_LanguageText.text = string.Format(LanguageUtils.getText(145010), m_hero.levelConfig.starEffectData);
            }

            if (m_levelUpEffectTimer != null)
            {
                Timer.Cancel(m_levelUpEffectTimer);
                m_levelUpEffectTimer = null;
            }

            float animationTime = ClientUtils.GetAnimationLength(view.m_UI_Item_CaptainLevelUpOnHead.m_UI_Item_CaptainLevelUpOnHead_Animator, 0);
            m_levelUpEffectTimer = Timer.Register(animationTime, () =>
            {
                view.m_UI_Item_CaptainLevelUpOnHead.gameObject.SetActive(false);
                m_levelUpEffectTimer = null;
            });


            CoreUtils.assetService.Instantiate("UE_CaptainPower", (go) =>
            {
                if (m_levelUpEffectGO != null)
                {
                    CoreUtils.assetService.Destroy(m_levelUpEffectGO);
                    m_levelUpEffectGO = null;
                }
                m_levelUpEffectGO = go;
                go.transform.SetParent(view.gameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                var textRect = go.transform.Find("pl_offset/lbl_text");                
                if(textRect != null)
                {
                    var languageText = textRect.gameObject.GetComponent<UnityEngine.UI.LanguageText>();
                    if(languageText != null)
                    {
                        int scoreAdd = m_hero.levelConfig.score;
                        if(preLevelConfig != null)
                        {
                            scoreAdd -= preLevelConfig.score;
                        }
                        languageText.text = string.Format(LanguageUtils.getText(145051), scoreAdd);
                    }
                }
                if (m_levelUpPowerTimer != null)
                {
                    Timer.Cancel(m_levelUpPowerTimer);
                    m_levelUpPowerTimer = null;
                }

                var animator = go.GetComponentInChildren<Animator>();
                float delayTime = ClientUtils.GetAnimationLength(animator, 0);
                m_levelUpPowerTimer = Timer.Register(delayTime, () =>
                {
                    if (m_levelUpEffectGO != null)
                    {
                        CoreUtils.assetService.Destroy(m_levelUpEffectGO);
                        m_levelUpEffectGO = null;
                    }
                    m_levelUpPowerTimer = null;
                });

            });           
           
        }

        private void RefreshHeroInfo()
        {
            m_oldHeroLevel = m_hero.level;
            view.m_UI_Model_CaptainHead.SetHero(m_hero, m_hero.level);
            view.m_pb_rogressBar_GameSlider.value =
            m_hero.levelConfig.exp == 0 ? 1 : m_hero.exp * 1.0f / m_hero.levelConfig.exp;
            if (m_hero.IsMaxLevel())
            {
                var levelConfig =
                    CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>(
                        m_hero.level + m_hero.config.rare * 10000 - 2);
                if (levelConfig == null) return;
                view.m_lbl_num_LanguageText.text =
                    $"{ClientUtils.FormatComma(levelConfig.exp)}/{ClientUtils.FormatComma(levelConfig.exp)}";
            }
            else
            {
                view.m_lbl_num_LanguageText.text =
                    $"{ClientUtils.FormatComma(m_hero.exp)}/{ClientUtils.FormatComma(m_hero.levelConfig.exp)}";
            }
            
        }

        private void RefreshList()
        {
            view.m_sv_list_ListView.ForceRefresh();
        }

        private void InitAddExpItemId()
        {            
            var items = CoreUtils.dataService.QueryRecords<Data.ItemDefine>();
            foreach(var item in items)
            {
                if(item.subType == 50101 && m_bagProxy.GetItemNum(item.ID) > 0)
                {
                    m_listAddExpItems.Add(item);
                }
            }
        }

        private bool CheckCanAddExp()
        {
            if(m_hero.IsMaxLevel())
            {
                Tip.CreateTip(LanguageUtils.getText(145019)).SetStyle(Tip.TipStyle.Middle).Show();
                return false;
            }
            if(m_hero.IsLevelLimitByStar())
            {
                Tip.CreateTip(LanguageUtils.getText(145019)).SetStyle(Tip.TipStyle.Middle).Show();
                return false;
            }
            return true;
        }

        private void UseExpItem(Data.ItemDefine itemCfg, int useCount)
        {
            if (!CheckCanAddExp()) return;
            if (m_bagProxy.GetItemNum(itemCfg.ID) < useCount)
            {
                Tip.CreateTip(LanguageUtils.getText(145022)).Show();
                return;
            }
            int exceedExp = GetExceedExp(itemCfg, useCount);
            if(exceedExp > 0)
            {
                string tipText = string.Format(LanguageUtils.getText(145023), ClientUtils.FormatComma(exceedExp));
                Alert.CreateAlert(tipText).SetLeftButton().SetRightButton(() =>
                {
                    SendAddExpMsg(itemCfg.ID, useCount);
                }).Show();
                return;
            }
            SendAddExpMsg(itemCfg.ID, useCount);
        }

        private int GetExceedExp(Data.ItemDefine itemCfg, int useCount)
        {
            int newExp = GetAddExpValue(itemCfg, useCount) + (int)m_hero.data.exp;
            if (newExp <= m_hero.levelConfig.exp) return 0;
            int maxUpLevel = GetMaxUpLevel();
            int heroLevel = m_hero.level;
            while(newExp > 0 && heroLevel < maxUpLevel)
            {
                var levelConfig = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>((heroLevel + m_hero.config.rare * 10000 - 1));
                if (levelConfig == null) break;
                newExp -= levelConfig.exp;
                heroLevel++;
            }
            return Mathf.Max(newExp, 0);
        }      

        private int GetMaxUpLevel()
        {
            var starConfig = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>((int)m_hero.data.star);
            if (starConfig == null) return 0;
            return starConfig.starLimit;
        }

        private int GetAddExpValue(int itemId, int useCount)
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(itemId);
            if (itemCfg == null) return 0;
            return GetAddExpValue(itemCfg, useCount);
        }

        private int GetAddExpValue(Data.ItemDefine itemCfg, int useCount)
        {
            return itemCfg.desData1 * useCount;
        }

        private void SendAddExpMsg(int itemId, int useCount)
        {           
            Hero_AddHeroExp.request request = new Hero_AddHeroExp.request()
            {
                heroId = m_hero.config.ID,
                itemId = itemId,
                itemNum = useCount
            };

            AppFacade.GetInstance().SendSproto(request);
        }

        private BagProxy m_bagProxy = null;
        private HeroProxy.Hero m_hero = null;
        private List<Data.ItemDefine> m_listAddExpItems = new List<Data.ItemDefine>();
        private int m_selectItemIndex = -1;
        private bool m_isPlayingProgressEffect = false;
        private int m_oldHeroLevel = 0;

        private Timer m_levelUpEffectTimer = null;
        private Timer m_levelUpPowerTimer = null;
        private Timer m_addExpEffectTimer = null;
        private GameObject m_levelUpEffectGO = null;
    }
}