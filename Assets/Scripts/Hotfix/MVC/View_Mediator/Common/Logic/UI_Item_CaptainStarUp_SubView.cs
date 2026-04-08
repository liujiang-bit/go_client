// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月26日
// Update Time         :    2020年4月26日
// Class Description   :    UI_Item_CaptainStarUp_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using System;

namespace Game {
    public partial class UI_Item_CaptainStarUp_SubView : UI_SubView
    {
        public bool IsShowStarUpDescription { get; private set; }
        private bool isPlayingEffNow = false;

        protected override void BindEvent()
        {
            m_btn_auto.AddClickEvent(OnClickedAutoPut);
            m_UI_culture.AddClickEvent(OnClickedApply);
            m_btn_stardesc_GameButton.onClick.AddListener(OnClickedShowStarUpDes);
            m_btn_preview_GameButton.onClick.AddListener(OnClickedShowStarUpPreview);
        }

        private void OnClickedApply()
        {
            if (isPlayingEffNow)
            {
                return;
            }

            if (m_hero == null) return;

            if(m_hero.IsStarMaxLevel())
            {
                Tip.CreateTip(166055, Tip.TipStyle.Middle).Show();
                return;
            }

            var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(m_hero.star);
            if (heroStarCfg == null) return;
            if (m_hero.level < heroStarCfg.starLimit)
            {
                Tip.CreateTip(LanguageUtils.getTextFormat(166056, heroStarCfg.starLimit, m_hero.star + 1), Tip.TipStyle.Middle).Show();
                return;
            }
            if (GetPutOnItemCount() == 0)
            {
                Tip.CreateTip(166057, Tip.TipStyle.Middle).Show();
                return;
            }
            int heroExp = (int)m_hero.data.starExp;
            for (int i = 0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                if (m_putOnAddStarExpItems[i] == null) continue;
                heroExp += m_putOnAddStarExpItems[i].exp;
            }
            var needExp = GetHeroStarLevelUpNeedExp(m_hero.star);
            if(heroExp > needExp && GetHeroStarLevelUpNeedExp(m_hero.star + 1) == 0)
            {
                string tipText = string.Format(LanguageUtils.getText(166058), heroExp - needExp);
                Alert.CreateAlert(tipText).SetLeftButton().SetRightButton(() =>
                {
                    SendAddStarExp();
                }).Show();
                return;
            }
            SendAddStarExp();
        }
        
        private void SendAddStarExp()
        {
            List<SprotoType.Hero_HeroStarUp.request.Item> items = new List<SprotoType.Hero_HeroStarUp.request.Item>();
            for (int i = 0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                if (m_putOnAddStarExpItems[i] != null)
                {
                    bool isFound = false;
                    foreach (var item in items)
                    {
                        if (item.itemId == m_putOnAddStarExpItems[i].itemID)
                        {
                            item.itemNum++;
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        items.Add(new SprotoType.Hero_HeroStarUp.request.Item()
                        {
                            itemId = m_putOnAddStarExpItems[i].itemID,
                            itemNum = 1,
                        });
                    }
                }
            }
            m_heroOldStar = m_hero.star;
            SprotoType.Hero_HeroStarUp.request request = new SprotoType.Hero_HeroStarUp.request()
            {
                items = items,
                heroId = m_hero.config.ID,
            };
            AppFacade.GetInstance().SendSproto(request);
        }

        private void OnClickedAutoPut()
        {
            if (isPlayingEffNow)
            {
                return;
            }
            AutoPutAddStarExpItems(true);
        }

        private void AutoPutAddStarExpItems(bool isNeedCheckSure)
        {
            for (int i = 0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                if (m_putOnAddStarExpItems[i] != null) continue;
                Data.ItemDefine itemCfg = null;
                Data.HeroStarExpDefine heroStarExpCfg = null;
                int index = 0;
                if (!GetCanPutOnItem(out itemCfg, out heroStarExpCfg, out index))
                {
                    break;
                }
                if (heroStarExpCfg.sure == 1 && isNeedCheckSure)
                {
                    ShowSureTip(() =>
                    {
                        AutoPutAddStarExpItems(false);
                    });
                    return;
                }
                TryPutItem(itemCfg, heroStarExpCfg, index / s_addExpItemListCol, isNeedCheckSure);
            }
        }

        private bool GetCanPutOnItem(out Data.ItemDefine itemCfg, out Data.HeroStarExpDefine heroStarExpCfg, out int index)
        {
            itemCfg = null;
            heroStarExpCfg = null;
            index = 0;
            bool isFound = false;
            for(int i = 0; i < m_addStarExpItemRecords.Count; ++i)
            {
                int getPutOnCount = GetPutOnItemCount(m_addStarExpItemRecords[i].itemID);
                if(getPutOnCount >= m_bagProxy.GetItemNum(m_addStarExpItemRecords[i].itemID))
                {
                    continue;
                }
                itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_addStarExpItemRecords[i].itemID);
                heroStarExpCfg = m_addStarExpItemRecords[i];
                index = i;
                isFound = true;
                break;
            }
            return isFound;
        }

        private void OnClickedShowStarUpPreview()
        {
            m_pl_starpreview_Button.gameObject.SetActive(true);
            for(int i = 0; i < m_addStarPreivewList.Count; ++i)
            {
                m_addStarPreivewList[i].Show(i + 2);
                var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(i + 2);
                if (heroStarCfg == null) continue;
                if(heroStarCfg.starEffect == 1)
                {
                    m_addStarPreivewList[i].m_UI_Item_CaptainSkill.gameObject.SetActive(true);
                    m_addStarPreivewList[i].m_UI_Item_CaptainSkill.SetPreviewSkillInfo(m_hero, i + 1);
                }
                else if(heroStarCfg.starEffect == 2)
                {
                    m_addStarPreivewList[i].m_UI_Item_CaptainSkill.gameObject.SetActive(false);
                    m_addStarPreivewList[i].m_lbl_plus_LanguageText.text = $"+{heroStarCfg.starEffectData}";
                }
                m_addStarPreivewList[i].m_lbl_effect_LanguageText.text = LanguageUtils.getText(heroStarCfg.l_starEffectID);
            }

            m_pl_starpreview_Button.onClick.RemoveAllListeners();
            m_pl_starpreview_Button.onClick.AddListener(() =>
            {
                m_pl_starpreview_Button.gameObject.SetActive(false);
            });
        }

        private void OnClickedShowStarUpDes()
        {
            IsShowStarUpDescription = true;
            m_pl_stardesc_ArabLayoutCompment.gameObject.SetActive(true);
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(166070);
            m_pl_left_ArabLayoutCompment.gameObject.SetActive(false);
        }

        public void HideStarUpDescription()
        {
            IsShowStarUpDescription = false;
            m_pl_stardesc_ArabLayoutCompment.gameObject.SetActive(false);
            m_pl_left_ArabLayoutCompment.gameObject.SetActive(true);
        }

        public void Show(HeroProxy.Hero hero)
        {
            gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_lbl_stars_GridLayoutGroup.transform as RectTransform);
            if (hero == null) return;
            m_hero = hero;
            if(!m_isInited)
            {
                m_isInited = true;
                InitUI();
            }
            ClearPutOnItem();
            InitAddStarExpItemRecords();
            RefreshAddExpItemList();
            RefreshStarLevelProcessUI();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            m_UI_Common_Crit.gameObject.SetActive(true);
            isPlayingEffNow = false;
        }

        public void OnAddStarExpSuccess2(int index, float length)
        {
            if (m_putOnAddStarExpItems[index] == null)
            {
                return;
            }
            Timer.Register(length-0.1f, () =>
            {
                if (gameObject == null)
                {
                    return;
                }
                int itemId = m_putOnAddStarExpItems[index].itemID;
                var globalEffectMediator = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                int curStar = (int)m_hero.data.star;
                globalEffectMediator.FlyHeroStarUpEffect(itemId, 1, m_addStarExpUIItemList[index].m_root_RectTransform, m_captainStarList[curStar - 1].m_root_RectTransform);
            });
        }

        public void OnAddStarExpSuccess()
        {
            isPlayingEffNow = true;
            for (int i = 0; i < m_addStarExpUIItemList.Count; ++i)
            {
                int index = i;
                if (m_addStarExpUIItemList[i] == null) continue;
                int randomNum = UnityEngine.Random.Range(0, 30);
                float randomFloat = (float)randomNum / 100;
                Timer.Register(randomFloat, () => {
                    if (gameObject == null)
                    {
                        return;
                    }
                    var putOnItemSubView = m_addStarExpUIItemList[index];
                    putOnItemSubView.m_UI_Item_CaptainStarUpItem_Animator.Play("UE_Star_Up");
                    addStarLevelUpEffect(putOnItemSubView.gameObject);
                    float length = ClientUtils.GetAnimationLength(putOnItemSubView.m_UI_Item_CaptainStarUpItem_Animator, "UE_Star_Up");
                    OnAddStarExpSuccess2(index, length);
                });
            }

            Timer.Register(1f + 0.3f + 1f, () =>
            {
                if (gameObject == null)
                {
                    return;
                }
                ClearPutOnItem();
                int preCount = m_addStarExpItemRecords.Count;
                InitAddStarExpItemRecords();
                int nowCount = m_addStarExpItemRecords.Count;
                if (nowCount == preCount)
                {
                    m_sv_listAddStar_ListView.ForceRefresh();
                }
                else
                {
                    int count = Mathf.CeilToInt(m_addStarExpItemRecords.Count * 1.0f / s_addExpItemListCol);
                    m_sv_listAddStar_ListView.FillContent(count);
                }
                RefreshStarLevelProcessUI(true);

                if (m_heroOldStar != m_hero.star)
                {
                    CoreUtils.uiManager.ShowUI(UI.s_captainStarUpSuccess, null, new CaptainStarUpSuccessViewData()
                    {
                        Hero = m_hero,
                        OldHeroStar = m_heroOldStar,
                    });
                    m_heroOldStar = m_hero.star;
                }

                isPlayingEffNow = false;
            });

        }

        private void InitUI()
        {
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

            m_captainStarList.Add(m_UI_CaptainStar0);
            m_captainStarList.Add(m_UI_CaptainStar1);
            m_captainStarList.Add(m_UI_CaptainStar2);
            m_captainStarList.Add(m_UI_CaptainStar3);
            m_captainStarList.Add(m_UI_CaptainStar4);
            m_captainStarList.Add(m_UI_CaptainStar5);

            m_addStarExpUIItemList.Add(m_img_item1);
            m_addStarExpUIItemList.Add(m_img_item2);
            m_addStarExpUIItemList.Add(m_img_item3);
            m_addStarExpUIItemList.Add(m_img_item4);
            m_addStarExpUIItemList.Add(m_img_item5);
            m_addStarExpUIItemList.Add(m_img_item6);

            m_addStarPreivewList.Add(m_UI_Item_AddStarPreview0);
            m_addStarPreivewList.Add(m_UI_Item_AddStarPreview1);
            m_addStarPreivewList.Add(m_UI_Item_AddStarPreview2);
            m_addStarPreivewList.Add(m_UI_Item_AddStarPreview3);
            m_addStarPreivewList.Add(m_UI_Item_AddStarPreview4);

        }

        private void RefreshAddExpItemList()
        {
            m_sv_listAddStar_ListView.Clear();
            ClientUtils.PreLoadRes(gameObject, m_sv_listAddStar_ListView.ItemPrefabDataList, OnItemPrefabLoadFinish);
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            if (dict.Count == 0) return;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            m_sv_listAddStar_ListView.SetInitData(dict, funcTab);
            int count = Mathf.CeilToInt(m_addStarExpItemRecords.Count * 1.0f / s_addExpItemListCol);
            m_sv_listAddStar_ListView.FillContent(count);
        }

        private void ItemEnter(ListView.ListItem item)
        {
            if (item == null || item.index >= m_addStarExpItemRecords.Count) return;
            UI_LC_CaptainAddStar_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_LC_CaptainAddStar_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_LC_CaptainAddStar_SubView;
            }

            for(int i = 0; i < s_addExpItemListCol; ++i)
            {
                int recordIndex = item.index * s_addExpItemListCol + i;
                bool isVisible = recordIndex < m_addStarExpItemRecords.Count;
                subView.CaptainAddStarItemList[i].gameObject.SetActive(isVisible);
                if (!isVisible)
                {
                    continue;
                }
                var captainAddStarItem = subView.CaptainAddStarItemList[i];
                var addStarExpItemCfg = m_addStarExpItemRecords[recordIndex];
                var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(addStarExpItemCfg.itemID);
                if (itemCfg == null) return;
                int itemNum = (int)m_bagProxy.GetItemNum(addStarExpItemCfg.itemID);
                itemNum -= GetPutOnItemCount(addStarExpItemCfg.itemID);
                captainAddStarItem.m_UI_Model_Item.Refresh(itemCfg, itemNum, false);
                if(itemNum == 0)
                {
                    captainAddStarItem.m_UI_Model_Item.m_lbl_count_LanguageText.text = $"<color=red>0</color>";
                }
                captainAddStarItem.m_UI_Model_Item.RemoveBtnAllListener();
                captainAddStarItem.m_UI_Model_Item.AddBtnListener(() =>
                {
                    if (itemNum == 0) return;
                    TryPutItem(itemCfg, addStarExpItemCfg, item.index, true);
                });
                captainAddStarItem.m_lbl_starExpNum_LanguageText.text = ClientUtils.FormatComma(addStarExpItemCfg.exp);
                captainAddStarItem.m_lbl_luckyNum_LanguageText.text = string.Format(LanguageUtils.getText(300102), addStarExpItemCfg.lucky);
                if (itemNum == 0 && addStarExpItemCfg.sure == 0)
                {
                    captainAddStarItem.m_btn_get.gameObject.SetActive(true);
                    captainAddStarItem.m_lbl_starExpNum_LanguageText.gameObject.SetActive(false);
                    captainAddStarItem.m_lbl_luckyNum_LanguageText.gameObject.SetActive(false);
                }
                else
                {
                    captainAddStarItem.m_btn_get.gameObject.SetActive(false);
                    captainAddStarItem.m_lbl_starExpNum_LanguageText.gameObject.SetActive(true);
                    captainAddStarItem.m_lbl_luckyNum_LanguageText.gameObject.SetActive(true);
                }
                
                captainAddStarItem.m_btn_get.RemoveAllClickEvent();
                captainAddStarItem.m_btn_get.AddClickEvent(() =>
                {
                    CaptainItemSourceViewData data = new CaptainItemSourceViewData
                    {
                        ResourceType = EnumCaptainLevelResourceType.StarLevel,
                        RequireItemId = addStarExpItemCfg.itemID,
                    };
                    CoreUtils.uiManager.ShowUI(UI.s_captainItemSource, null, data);
                });
            }           
        }

        private void RefreshStarLevelProcessUI(bool isStarExpSuccess = false)
        {
            for(int i = 0; i < m_captainStarList.Count && i < m_hero.star; ++i)
            {
                m_captainStarList[i].m_img_starHighlight_PolygonImage.gameObject.SetActive(true);
                m_captainStarList[i].m_img_starHighlight_PolygonImage.fillAmount = 1;
                m_captainStarList[i].m_img_starPreview_PolygonImage.gameObject.SetActive(false);
            }

            for(int i = m_hero.star; i < m_captainStarList.Count; ++i)
            {
                if(i == m_hero.star && m_hero.data.starExp > 0)
                {
                    m_captainStarList[i].m_img_starHighlight_PolygonImage.gameObject.SetActive(true);
                    int nextLevelNeexExp = GetHeroStarLevelUpNeedExp();
                    m_captainStarList[i].m_img_starHighlight_PolygonImage.fillAmount = nextLevelNeexExp == 0? 1: m_hero.data.starExp * 1.0f / nextLevelNeexExp;
                }
                else
                {
                    m_captainStarList[i].m_img_starHighlight_PolygonImage.gameObject.SetActive(false);
                }
                m_captainStarList[i].m_img_starPreview_PolygonImage.gameObject.SetActive(false);
            }

            RefreshStarLevelProcessPreview(isStarExpSuccess);
        }

        private void RefreshStarLevelProcessPreview(bool isStarExpSuccess = false)
        {
            int heroExp = (int)m_hero.data.starExp;
            int heroLucky = 0;
            bool isPutItem = false;
            int itemAddExp = 0;

            for(int i =0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                if (m_putOnAddStarExpItems[i] == null) continue;
                heroExp += m_putOnAddStarExpItems[i].exp;
                heroLucky += m_putOnAddStarExpItems[i].lucky;
                isPutItem = true;
                itemAddExp += m_putOnAddStarExpItems[i].exp;
            }

            if (isPutItem)
            {
                m_itemAddExp = itemAddExp;
                m_oldHeroExp = heroExp - itemAddExp;
            }
            else
            {
                int addExp = heroExp - m_oldHeroExp;
                if (isStarExpSuccess && addExp > 0)
                {
                    SetStarLevelProcessEffect(addExp);
                }
                else if (isStarExpSuccess && addExp < 0)
                {
                    //升星
                    if (m_hero.star > m_heroOldStar)
                    {
                        int oldHeroLevelUpExp = GetHeroStarLevelUpNeedExp(m_heroOldStar);
                        int offsetExp = oldHeroLevelUpExp - m_oldHeroExp;
                        int levelUpAddExp = offsetExp + (int)m_hero.data.starExp;
                        SetStarLevelProcessEffect(levelUpAddExp);
                    }
                }
                else
                {
                    m_UI_Common_Crit.gameObject.SetActive(false);
                }
            }

            m_lbl_luckyNum_LanguageText.text = string.Format(LanguageUtils.getText(300102), heroLucky);
            m_lbl_starExpNum_LanguageText.text = ClientUtils.FormatComma(heroExp - (int)m_hero.data.starExp);

            if(m_hero.IsStarMaxLevel())
            {
                m_img_exp_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                m_img_exp_PolygonImage.gameObject.SetActive(true);
                int starPreviewLevel = m_hero.star;
                float heroCurProcess = 0;
                float heroPreviewProcess = 0;
                if (heroExp > m_hero.data.starExp)
                {
                    while (heroExp >= 0 && starPreviewLevel < m_captainStarList.Count)
                    {
                        int nextLevelNeedExp = GetHeroStarLevelUpNeedExp(starPreviewLevel);
                        m_captainStarList[starPreviewLevel].m_img_starPreview_PolygonImage.gameObject.SetActive(true);
                        float fillCount = Mathf.Min(1, heroExp * 1.0f / nextLevelNeedExp);
                        m_captainStarList[starPreviewLevel].m_img_starPreview_PolygonImage.fillAmount = fillCount;

                        if (starPreviewLevel == m_hero.star)
                        {
                            heroPreviewProcess = (heroExp - m_hero.starExp) * 1.0f / nextLevelNeedExp * 100;
                        }
                        else
                        {
                            heroPreviewProcess = (heroExp) * 1.0f / nextLevelNeedExp * 100;
                        }
                        if (heroExp == 0) break;
                        heroExp -= nextLevelNeedExp;
                        if(heroExp >=0)
                        {
                            starPreviewLevel++;
                        }
                    }
                }
                if (starPreviewLevel == m_hero.star || starPreviewLevel == m_captainStarList.Count)
                {
                    int needExp = GetHeroStarLevelUpNeedExp();
                    heroCurProcess = needExp == 0 ? 100 : m_hero.data.starExp * 1.0f / GetHeroStarLevelUpNeedExp() * 100;
                    if (starPreviewLevel == m_captainStarList.Count)
                    {
                        starPreviewLevel--;
                    }
                }
                
                var starSubView = m_captainStarList[starPreviewLevel];
                var screenPos = RectTransformUtility.WorldToScreenPoint(CoreUtils.uiManager.GetUICamera(), starSubView.gameObject.transform.position);
                Vector3 worldPos = Vector3.zero;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(m_img_exp_PolygonImage.transform.parent as RectTransform, screenPos,
                    CoreUtils.uiManager.GetUICamera(), out worldPos);
                Vector3 oldWorldPos = m_img_exp_PolygonImage.transform.position;
                oldWorldPos.x = worldPos.x;
                m_img_exp_PolygonImage.transform.position = oldWorldPos;
                if (GetPutOnItemCount() > 0)
                {
                    m_lbl_exp_LanguageText.text = string.Format(LanguageUtils.getText(300103), ClientUtils.FormatF2(heroCurProcess), ClientUtils.FormatF2(heroPreviewProcess));
                }
                else
                {
                    m_lbl_exp_LanguageText.text = string.Format(LanguageUtils.getText(300102), ClientUtils.FormatF2(heroCurProcess));
                }
            }            
        }

        private void SetStarLevelProcessEffect(int addExp)
        {
            m_UI_Common_Crit.gameObject.SetActive(true);
            m_UI_Common_Crit.m_pl_source_Animator.gameObject.SetActive(false);
            m_UI_Common_Crit.m_lbl_value_LanguageText.text = addExp.ToString();
            m_UI_Common_Crit.m_pl_source_Animator.gameObject.SetActive(true);
            Transform eff = m_UI_Common_Crit.m_UI_Common_Crit.Find("UI_10050");
            
            if (eff != null)
            {
                eff.transform.gameObject.SetActive(false);
                eff.transform.gameObject.SetActive(true);
            }

            if (addExp > m_itemAddExp)
            {
                //todo:暴击
                m_UI_Common_Crit.m_pl_cri_Animator.gameObject.SetActive(true);

            }
            else
            {
                //todo:普通加经验
                m_UI_Common_Crit.m_pl_cri_Animator.gameObject.SetActive(false);
            }
        }

        private int GetHeroStarLevelUpNeedExp()
        {
            return GetHeroStarLevelUpNeedExp(m_hero.star);
        }

        private int GetHeroStarLevelUpNeedExp(int starLevel)
        {
            var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(starLevel);
            if (heroStarCfg == null) return 0;
            int needExp = 0;
            switch ((EnumRareType)m_hero.config.rare)
            {
                case EnumRareType.White:
                    {
                        needExp = heroStarCfg.rare1;
                    }
                    break;
                case EnumRareType.Green:
                    {
                        needExp = heroStarCfg.rare2;
                    }
                    break;
                case EnumRareType.Blue:
                    {
                        needExp = heroStarCfg.rare3;
                    }
                    break;
                case EnumRareType.Purple:
                    {
                        needExp = heroStarCfg.rare4;
                    }
                    break;
                case EnumRareType.Orange:
                    {
                        needExp = heroStarCfg.rare5;
                    }
                    break;
            }
            return needExp;
        }

        private void InitAddStarExpItemRecords()
        {
            m_addStarExpItemRecords.Clear();
            var heroStarExpRecords = CoreUtils.dataService.QueryRecords<Data.HeroStarExpDefine>();
            foreach(var record in heroStarExpRecords)
            {
                if(record.rareGroup == m_hero.config.rare)
                {
                    if(record.sure == 0 || m_bagProxy.GetItemNum(record.itemID) > 0)
                    m_addStarExpItemRecords.Add(record);
                }
            }
        }

        private void TryPutItem(Data.ItemDefine item, Data.HeroStarExpDefine heroStarExp, int listItemIndex, bool isNeedCheckSure)
        {
            if (isPlayingEffNow)
            {
                return;
            }
            if (m_hero.star >= m_addStarExpUIItemList.Count) return;
            if (GetPutOnItemCount() >= m_addStarExpUIItemList.Count) 
            {
                Tip.CreateTip(166067, Tip.TipStyle.Middle).Show();
                return;
            }
            if(heroStarExp.sure == 1 && isNeedCheckSure)
            {
                ShowSureTip(() =>
                {
                    PutOnItem(item, heroStarExp, listItemIndex);
                });
                return;
            }
            PutOnItem(item, heroStarExp, listItemIndex);
        }

        private void ShowSureTip(Action onConfirmCallback)
        {
            if(TipRemindProxy.IsShowRemind(TipRemindProxy.AddStarExpSureItemRemind))
            {
                Alert.CreateAlert(LanguageUtils.getText(166068))
                .SetLeftButton()
                .SetRightButton()
                .SetAlertToggle((isOn) =>
                {
                    if(isOn)
                    {
                        TipRemindProxy.SaveRemind(TipRemindProxy.AddStarExpSureItemRemind);
                    }
                    onConfirmCallback?.Invoke();
                }).Show();
            }
            else
            {
                onConfirmCallback?.Invoke();
            }
        }

        private void addStarLevelUpEffect(GameObject gameObject)
        {
            Transform eff = gameObject.transform.Find(RS.HeroStarLevelUpEffectName + "(Clone)");
            if (eff != null)
            {
                eff.transform.gameObject.SetActive(false);
                eff.transform.gameObject.SetActive(true);
                return;
            }

            CoreUtils.assetService.Instantiate(RS.HeroStarLevelUpEffectName, (go) =>
            {
                go.transform.SetParent(gameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            });
        }

        private void addPutOnItemEffect(GameObject gameObject)
        {
            Transform effLevelUp = gameObject.transform.Find(RS.HeroStarLevelUpEffectName + "(Clone)");
            if (effLevelUp != null)
            {
                effLevelUp.transform.gameObject.SetActive(false);
                return;
            }

            Transform eff = gameObject.transform.Find(RS.HeroPutItemEffectName + "(Clone)");
            if (eff != null)
            {
                eff.transform.gameObject.SetActive(false);
                eff.transform.gameObject.SetActive(true);
                return;
            }

            CoreUtils.assetService.Instantiate(RS.HeroPutItemEffectName, (go) =>
            {
                go.transform.SetParent(gameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            });
        }

        private void PutOnItem(Data.ItemDefine item, Data.HeroStarExpDefine heroStarExp, int listItemIndex)
        {
            int putOnIndex = GetTryPutOnItemIndex();
            if (putOnIndex == -1) return;
            m_putOnAddStarExpItems[putOnIndex] = heroStarExp;
            var putOnItemSubView = m_addStarExpUIItemList[putOnIndex];
            if (putOnItemSubView == null) return;
            var col = putOnItemSubView.m_img_reduce_PolygonImage.color;
            col.a = 1;
            putOnItemSubView.m_img_reduce_PolygonImage.color = col;
            putOnItemSubView.gameObject.SetActive(true);
            addPutOnItemEffect(putOnItemSubView.gameObject);
            ClientUtils.LoadSprite(putOnItemSubView.m_UI_Item_CaptainStarUpItem_PolygonImage, item.itemIcon);
            putOnItemSubView.m_btn_reduce_GameButton.onClick.RemoveAllListeners();
            putOnItemSubView.m_btn_reduce_GameButton.onClick.AddListener(() =>
            {
                putOnItemSubView.gameObject.SetActive(false);
                m_putOnAddStarExpItems[putOnIndex] = null;
                m_sv_listAddStar_ListView.RefreshItem(listItemIndex);
                RefreshStarLevelProcessUI();
            });
            m_sv_listAddStar_ListView.RefreshItem(listItemIndex);
            RefreshStarLevelProcessUI();

            putOnItemSubView.m_UI_Item_CaptainStarUpItem_Animator.Play("UE_Star_Down");
        }

        private int GetTryPutOnItemIndex()
        {
            for(int i = 0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                if (m_putOnAddStarExpItems[i] == null) return i;
            }
            return -1;
        }

        private int GetPutOnItemCount(int itemId)
        {
            int count = 0;
            foreach(var item in m_putOnAddStarExpItems)
            {
                if (item != null && item.itemID == itemId) count++;
            }
            return count;
        }

        private int GetPutOnItemCount()
        {
            int count = 0;
            for(int i = 0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                if (m_putOnAddStarExpItems[i] != null) count++;

            }
            return count;
        }

        private void ClearPutOnItem()
        {           
            for(int i = 0; i < m_putOnAddStarExpItems.Length; ++i)
            {
                m_putOnAddStarExpItems[i] = null;
            }
            foreach(var addStarExpUiItem in m_addStarExpUIItemList)
            {
                addStarExpUiItem.gameObject.SetActive(false);
            }
        }

        private bool m_isInited = false;
        private BagProxy m_bagProxy = null;
        private List<Data.HeroStarExpDefine> m_addStarExpItemRecords = new List<Data.HeroStarExpDefine>();
        private List<UI_Model_CaptainStar_SubView> m_captainStarList = new List<UI_Model_CaptainStar_SubView>();
        private List<UI_Item_CaptainStarUpItem_SubView> m_addStarExpUIItemList = new List<UI_Item_CaptainStarUpItem_SubView>();
        private List<UI_Item_AddStarPreview_SubView> m_addStarPreivewList = new List<UI_Item_AddStarPreview_SubView>();
        private HeroProxy.Hero m_hero = null;
        private Data.HeroStarExpDefine[] m_putOnAddStarExpItems = new Data.HeroStarExpDefine[s_maxPutOnItemCount];
        private int m_heroOldStar = 0;

        private const int s_maxPutOnItemCount = 6;
        private const int s_addExpItemListCol = 3;

        private int m_oldHeroExp = 0;
        private int m_itemAddExp = 0;
    }
}