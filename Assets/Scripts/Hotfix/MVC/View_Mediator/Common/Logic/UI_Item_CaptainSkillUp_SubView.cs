// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Item_CaptainSkillUp_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.Rendering;
using Data;

namespace Game {
    public partial class UI_Item_CaptainSkillUp_SubView : UI_SubView
    {
        protected override void BindEvent()
        {
            m_btn_add_GameButton.onClick.AddListener(OnClickedGetMore);
            m_UI_btn_lvup.m_btn_languageButton_GameButton.onClick.AddListener(OnClickedUpgrade);
            m_btn_change_GameButton.onClick.AddListener(OnClickedExchange);
        }

        public void Show(HeroProxy.Hero hero)
        {
            gameObject.SetActive(true);
            if (hero == null) return;
            m_hero = hero;

            if(!m_isInited)
            {
                m_isInited = true;
                InitData();
            }
            else
            {
                CheckSkillLine();
            }

            RefreshPanel();
        }

        public void Refresh()
        {
            RefreshPanel();
        }

        public void HideEffect()
        {
            for (int i = 0; i < m_captainSkillSubViewList.Count; ++i)
            {
                Transform effectObj = m_captainSkillSubViewList[i].gameObject.transform.Find("UI_10015");
                if (effectObj != null)
                {
                    effectObj.gameObject.SetActive(false);
                }
            }
            
            Transform lastEffectObj = m_captainSkillSubViewList[m_captainSkillSubViewList.Count - 1].gameObject.transform.Find("UI_10053");
            if (lastEffectObj != null)
            {
                lastEffectObj.gameObject.SetActive(false);
            }

            foreach (var awakenSubView in m_skillAwakenSubViewList)
            {
                awakenSubView.gameObject.SetActive(false);
                Transform effectObj = awakenSubView.gameObject.transform.Find("UI_10054");
                if (effectObj != null)
                {
                    effectObj.gameObject.SetActive(false);
                }
            }
        }

        public void Hide()
        {
            if (m_effectTimer != null)
            {
                m_effectTimer.Cancel();
                m_effectTimer = null;
            }
            
            if (m_skillEffectTimer != null)
            {
                m_skillEffectTimer.Cancel();
                m_skillEffectTimer = null;
            }
            
            if (m_awakeningTimer != null)
            {
                m_awakeningTimer.Cancel();
                m_awakeningTimer = null;
            }
            HideEffect();
            gameObject.SetActive(false);
        }

        private void InitData()
        {
            
            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.HeroSkillUpSuccess,
                CmdConstant.SetHeroSkillLineSort
            },this.gameObject, OnNotification);
            
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (m_bagProxy == null) return;

            m_captainSkillSubViewList.Add(m_UI_Item_CaptainSkill1);
            m_captainSkillSubViewList.Add(m_UI_Item_CaptainSkill2);
            m_captainSkillSubViewList.Add(m_UI_Item_CaptainSkill3);
            m_captainSkillSubViewList.Add(m_UI_Item_CaptainSkill4);
            m_captainSkillSubViewList.Add(m_UI_Item_CaptainSkill5);

            m_skillAwakenSubViewList.Add(m_UI_Item_awaken1);
            m_skillAwakenSubViewList.Add(m_UI_Item_awaken2);
            m_skillAwakenSubViewList.Add(m_UI_Item_awaken3);
            m_skillAwakenSubViewList.Add(m_UI_Item_awaken4);
            
            m_skillLineList.Add(m_pl_effect_line1.gameObject);
            m_skillLineList.Add(m_pl_effect_line2.gameObject);
            m_skillLineList.Add(m_pl_effect_line3.gameObject);
            m_skillLineList.Add(m_pl_effect_line4.gameObject);
            
            List<string> prefabNames = new List<string>();
            prefabNames.Add("UI_10055_1");
            prefabNames.Add("UI_10055_2");
            ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);

            // 先去掉
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_hero.config.getItem);
            if (itemCfg == null) return;
            if (itemCfg.get == null || itemCfg.get.Count == 0)
            {
                m_btn_add_GameButton.enabled = false;
                m_btn_add_GrayChildrens.Gray();
            }
            else
            {
                m_btn_add_GameButton.enabled = true;
                m_btn_add_GrayChildrens.Normal();
            }
        }
        
        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.HeroSkillUpSuccess:
                    PlayHeroSkillUpEffect();
                    break;
                case CmdConstant.SetHeroSkillLineSort:
                    if (notification.Body != null)
                    {
                        int sort = (int) notification.Body;
                        m_pl_effect_SortingGroup.sortingOrder = sort;
                    }

                    break;
                default:
                    break;
            }
        }
        
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            CheckSkillLine();
        }

        private void OnClickedUpgrade()
        {
            if (IsHeroAllSkillMaxLevel()) return;
            if(IsHeroAllUnlockSkillMaxLevel())
            {
                Tip.CreateTip(166052, Tip.TipStyle.Middle).Show();
                return;
            }
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_hero.config.getItem);
            if (itemCfg == null) return;
            m_UI_herolvup_item.Refresh(itemCfg, 0, false);
            long itemCount = m_bagProxy.GetItemNum(itemCfg.ID);
            int costItemCount = m_hero.GetSkillLevelUpCostItemNum();
            if (itemCount < costItemCount) return;
            Hero_HeroSkillLevelUp.request request = new Hero_HeroSkillLevelUp.request()
            {
                heroId = m_hero.config.ID,
            };
            AppFacade.GetInstance().SendSproto(request);
            m_isWaitForServerResponse = true;
        }

        private void OnClickedGetMore()
        {
            CaptainItemSourceViewData data = new CaptainItemSourceViewData
            {
                ResourceType = EnumCaptainLevelResourceType.SkillLevel,
                CaptainId = m_hero.config.ID,
            };
            AppFacade.GetInstance().SendNotification(CmdConstant.SetHeroSkillLineSort,5000);
            CoreUtils.uiManager.ShowUI(UI.s_captainItemSource, null, data);
        }

        private void OnClickedExchange()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.SetHeroSkillLineSort,5000);
            CoreUtils.uiManager.ShowUI(UI.s_itemExchange, null, new ItemExchangeViewData()
            {
                Hero = m_hero,
            });
        }

        private void RefreshPanel()
        {
            RefreshSkillSubView();
            RefreshSkillUpProgress();
        }

        private void HeroSkillUpRefresh()
        {
            for (int i = 0; i < m_captainSkillSubViewList.Count; ++i)
            {
                m_captainSkillSubViewList[i].SetSkillInfo(m_hero, i, 0);
            }
            RefreshSkillUpProgress();
        }


        private void PlayHeroSkillUpEffect()
        {
            int upIndex = -1;
            for (int i = 0; i < m_captainSkillSubViewList.Count - 1; ++i)
            {
                if (IsHeroSkillMaxLevel(i) && m_hero.config.skill[i] == m_curUpSkillId)
                {
                    Transform effectObj = m_captainSkillSubViewList[i].gameObject.transform.Find("UI_10015");
                    if (effectObj != null)
                    {
                        effectObj.gameObject.SetActive(true);
                        upIndex = i;
                        break;
                    }
                }
            }

            if (upIndex >= 0)
            {
                if (m_skillEffectTimer != null)
                {
                    m_skillEffectTimer.Cancel();
                    m_skillEffectTimer = null;
                }

                m_skillEffectTimer = Timer.Register(1.1f, () =>
                {

                    if (upIndex < m_skillLineList.Count)
                    {
                        LoadLine("UI_10055_2",upIndex);
                    }

                    if (m_hero.config.skill.Count == m_captainSkillSubViewList.Count)
                    {
                        int unlockNum = 0;
                        for (int skillIndex = 0; skillIndex < m_hero.config.skill.Count - 1; ++skillIndex)
                        {
                            bool isSkillMaxLevel = IsHeroSkillMaxLevel(skillIndex);
                            if (isSkillMaxLevel)
                            {
                                unlockNum++;

                                if (m_hero.config.skill[skillIndex] == m_curUpSkillId)
                                {
                                    foreach (var star in m_skillAwakenSubViewList)
                                    {
                                        if (!star.m_img_awakenlight_PolygonImage.gameObject
                                            .activeSelf)
                                        {
                                            star.m_img_awakenlight_PolygonImage.gameObject
                                                .SetActive(true);
                                            Transform effectObj = star.gameObject.transform
                                                .Find("UI_10054");
                                            if (effectObj != null)
                                            {
                                                effectObj.gameObject.SetActive(true);
                                            }

                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (unlockNum >= (m_hero.config.skill.Count - 1))
                        {
                            if (m_awakeningTimer != null)
                            {
                                m_awakeningTimer.Cancel();
                                m_awakeningTimer = null;
                            }
                            
                            m_awakeningTimer = Timer.Register(1.5f, () =>
                            {
                                Transform effectObj = m_captainSkillSubViewList[m_captainSkillSubViewList.Count - 1].gameObject.transform.Find("UI_10053");
                                if (effectObj != null)
                                {
                                    effectObj.gameObject.SetActive(true);
                                }

                                if (m_effectTimer != null)
                                {
                                    m_effectTimer.Cancel();
                                    m_effectTimer = null;
                                }

                                m_effectTimer = Timer.Register(1.5f, () =>
                                {

                                    foreach (var awakenSubView in m_skillAwakenSubViewList)
                                    {
                                        awakenSubView.gameObject.SetActive(false);
                                    }

                                    m_img_cover_PolygonImage.gameObject.SetActive(false);
                                
                                });
                            });
                            
                        }

                        m_img_cover_PolygonImage.gameObject.SetActive(true);

                    }
                });
                
            }
        }

        private void CheckSkillLine()
        {
            for (int i = 0; i < m_captainSkillSubViewList.Count - 1; ++i)
            {
                if (IsHeroSkillMaxLevel(i))
                {
                    LoadLine("UI_10055_2", i);
                }
                else
                {
                    LoadLine("UI_10055_1", i);
                }
            }
        }

        private void LoadLine(string name, int index)
        {
            if (index >= m_skillLineList.Count) return;
            
            if (m_skillLineList[index].transform.childCount > 0)
            {
                var childNode = m_skillLineList[index].transform.GetChild(0);
                
                if (childNode.name.Equals(name)) return;
                
                GameObject.Destroy(childNode.gameObject);
            }
            
            if (m_assetDic.ContainsKey(name))
            {
                GameObject lineEffect = GameObject.Instantiate(m_assetDic[name]);
                
                lineEffect.transform.SetParent(m_skillLineList[index].transform);
                lineEffect.name = name;
                lineEffect.transform.localPosition = Vector3.zero;
                lineEffect.transform.localEulerAngles = Vector3.zero;
                lineEffect.transform.localScale = Vector3.one;
            }
            else
            {
                CoreUtils.assetService.Instantiate(name, (lineEffect) =>
                {
                    lineEffect.transform.SetParent(m_skillLineList[index].transform);
                    lineEffect.name = name;
                    lineEffect.transform.localPosition = Vector3.zero;
                    lineEffect.transform.localEulerAngles = Vector3.zero;
                    lineEffect.transform.localScale = Vector3.one;
                });
            }
        }

        private void RefreshSkillSubView()
        {
            for (int i = 0; i < m_captainSkillSubViewList.Count; ++i)
            {
                m_captainSkillSubViewList[i].SetSkillInfo(m_hero, i, 0);
                if (i == m_captainSkillSubViewList.Count - 1 && m_hero.config.skill.Count == m_captainSkillSubViewList.Count)
                {
                    if (m_hero.data.skills.Count == m_hero.config.skill.Count && m_hero.data.skills[i].skillLevel > 0)
                    {
                        foreach (var awakenSubView in m_skillAwakenSubViewList)
                        {
                            awakenSubView.gameObject.SetActive(false);
                            
                        }
                        m_img_cover_PolygonImage.gameObject.SetActive(false);
                    }
                    else
                    {
                        foreach (var star in m_skillAwakenSubViewList)
                        {
                            star.gameObject.SetActive(true);
                            star.m_img_awakenlight_PolygonImage.gameObject.SetActive(false);
                        }
                        
                        int index = -1;
                        for (int skillIndex = 0; skillIndex < m_hero.config.skill.Count - 1; ++skillIndex)
                        {
                            bool isSkillMaxLevel = IsHeroSkillMaxLevel(skillIndex);
                            if (isSkillMaxLevel)
                            {
                                index++;
                                m_skillAwakenSubViewList[index].m_img_awakenlight_PolygonImage.gameObject.SetActive(true);
                            }

                        }
                        m_img_cover_PolygonImage.gameObject.SetActive(true);
                    }
                }
            }
        }

        private bool IsHeroSkillMaxLevel(int index)
        {
            if (m_hero.data.skills.Count <= index) return false;
            return m_hero.data.skills[index].skillLevel >= s_SkillMaxlLevel;
        }

        private bool IsHeroAllSkillMaxLevel()
        {
            return m_hero.GetSkillAllLevel() >= s_SkillMaxlLevel * (s_SkillMaxNum - 1);
        }

        private bool IsHeroAllUnlockSkillMaxLevel()
        {
            foreach(var skillInfo in m_hero.data.skills)
            {
                if(skillInfo.skillLevel > 0 && skillInfo.skillLevel < s_SkillMaxlLevel)
                {
                    return false;
                }
            }
            return true;
        }

        private void RefreshSkillUpProgress()
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_hero.config.getItem);
            if (itemCfg == null) return;
            m_UI_herolvup_item.Refresh(itemCfg, 0, false);
            long itemCount = m_bagProxy.GetItemNum(itemCfg.ID);

            if (IsHeroAllSkillMaxLevel())
            {
                m_pb_rogressBar_GameSlider.value = 1;
                m_lbl_itemNum_LanguageText.text = ClientUtils.FormatComma(itemCount);
                m_UI_btn_lvup.m_img_forbid_PolygonImage.gameObject.SetActive(true);
                m_UI_btn_lvup.m_lbl_Text_LanguageText.text = LanguageUtils.getText(145057);
            }
            else
            {
                int costItemCount = m_hero.GetSkillLevelUpCostItemNum();
                m_pb_rogressBar_GameSlider.value = Mathf.Min(1, itemCount * 1.0f / costItemCount);
                m_lbl_itemNum_LanguageText.text = LanguageUtils.getTextFormat(300001, ClientUtils.FormatComma(itemCount), ClientUtils.FormatComma(costItemCount));
                m_UI_btn_lvup.m_img_forbid_PolygonImage.gameObject.SetActive(itemCount < costItemCount);
                m_UI_btn_lvup.m_lbl_Text_LanguageText.text = LanguageUtils.getText(180359);
            }

            bool isChangeButtonVisible = m_hero.config.exchange != 0 && !IsHeroAllSkillMaxLevel();
            m_btn_change_GameButton.gameObject.SetActive(isChangeButtonVisible);
            if (isChangeButtonVisible)
            {
                var exchangeItemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_hero.config.exchange);
                if (exchangeItemCfg == null) return;
                ClientUtils.LoadSprite(m_img_currency_icon_PolygonImage, exchangeItemCfg.itemIcon);
                long exchangeItemCount = m_bagProxy.GetItemNum(exchangeItemCfg.ID);
                m_lbl_currency_iconNum_LanguageText.text = ClientUtils.FormatComma(exchangeItemCount);
            }

            if(IsHeroAllSkillMaxLevel())
            {
                m_lbl_desc_LanguageText.text = LanguageUtils.getText(166027);
            }
            else
            {
                m_lbl_desc_LanguageText.text = LanguageUtils.getText(166026);
            }
            m_lbl_name_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
        }

        public void HeroSkillLevelUpSuceess(int skillId, int skillLevel)
        {
            m_isWaitForServerResponse = false;
            m_curUpSkillId = skillId;
            HeroSkillUpRefresh();
            CoreUtils.uiManager.ShowUI(UI.s_captainSkillUpSuccess, null, new CaptainSkillUpSuccessViewData()
            {
                Hero = m_hero,
                SkillId = skillId,
                SkillLevel = skillLevel
            });
        }

        private bool m_isInited = false;
        private BagProxy m_bagProxy = null;
        private HeroProxy.Hero m_hero = null;
        private List<UI_Item_CaptainSkill_SubView> m_captainSkillSubViewList = new List<UI_Item_CaptainSkill_SubView>();
        private List<UI_Item_awaken_SubView> m_skillAwakenSubViewList = new List<UI_Item_awaken_SubView>();
        private List<GameObject> m_skillLineList = new List<GameObject>();
        private bool m_isWaitForServerResponse = false;
        private int m_curUpSkillId;
        private Timer m_effectTimer = null;
        private Timer m_skillEffectTimer = null; 
        private Timer m_awakeningTimer = null;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private int s_SkillMaxlLevel = 5;
        private int s_SkillMaxNum = 5;
    }
}