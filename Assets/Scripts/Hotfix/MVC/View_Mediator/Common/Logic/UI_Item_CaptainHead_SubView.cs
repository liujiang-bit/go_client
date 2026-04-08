// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月29日
// Update Time         :    2019年12月29日
// Class Description   :    UI_Item_CaptainHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using System;

namespace Game {
    public partial class UI_Item_CaptainHead_SubView : UI_SubView
    {
        private HeroProxy.Hero hero;
        public void SetHeroID(int id)
        {
            var heroInfo = CoreUtils.dataService.QueryRecord<Data.HeroDefine>((int)id);
            m_UI_Model_CaptainHead.SetIcon(heroInfo.heroIcon);
            m_UI_Model_CaptainHead.SetRare(heroInfo.rare);
            m_UI_Model_HeadStar1.SetShow(heroInfo.initStar > 0);
            m_UI_Model_HeadStar2.SetShow(heroInfo.initStar > 1);
            m_UI_Model_HeadStar3.SetShow(heroInfo.initStar > 2);
            m_UI_Model_HeadStar4.SetShow(heroInfo.initStar > 3);
            m_UI_Model_HeadStar5.SetShow(heroInfo.initStar > 4);
            m_UI_Model_HeadStar6.SetShow(heroInfo.initStar > 5);

            m_lbl_lv_LanguageText.text = 1.ToString();
        }
        public void AddClickEvent(Action<HeroProxy.Hero> call)
        {
            m_btn_select_GameButton.onClick.AddListener(()=>
            {
                call(this.hero);
            });
        }

        public void AddClickEvent(Action<HeroProxy.Hero, UI_Item_CaptainHead_SubView> call)
        {
            m_btn_select_GameButton.onClick.AddListener(() =>
            {
                call(this.hero, this);
            });
        }

        public bool Selected(HeroProxy.Hero hero)
        {
            m_img_selected_PolygonImage.gameObject.SetActive(this.hero == hero);
            m_img_selected_PolygonImage.enabled = this.hero == hero;
            return this.hero == hero;
        }

        public void SetHero(HeroProxy.Hero hero)
        {
            this.hero = hero;
            m_img_selected_PolygonImage.enabled = false;
            m_UI_Model_CaptainHead.SetIcon(hero.config.heroIcon);
            m_UI_Model_CaptainHead.SetRare(hero.config.rare);
            SetEffect(hero.config.rare,hero.config.ID);
            m_UI_Model_HeadStar1.SetShow(hero.star > 0);
            m_UI_Model_HeadStar2.SetShow(hero.star > 1);
            m_UI_Model_HeadStar3.SetShow(hero.star > 2);
            m_UI_Model_HeadStar4.SetShow(hero.star > 3);
            m_UI_Model_HeadStar5.SetShow(hero.star > 4);
            m_UI_Model_HeadStar6.SetShow(hero.star > 5);
            Color heroHeadImageColor = GetHeroHeadImageColor(hero);
            if(hero.data != null)
            {
                m_img_level_PolygonImage.gameObject.SetActive(true);
                m_lbl_lv_LanguageText.text = hero.level.ToString();
                m_pl_stars_GridLayoutGroup.gameObject.SetActive(true);
                m_pl_summon_VerticalLayoutGroup.gameObject.SetActive(false);
                m_img_headDark_PolygonImage.gameObject.SetActive(false);
                m_UI_Model_CaptainHead.m_img_char_PolygonImage.color = heroHeadImageColor;
                m_img_promote_PolygonImage.gameObject.SetActive(hero.GetCurPageRemainPoint(hero.talentIndex) > 0 || hero.IsCanUpSkill());
            }
            else
            {
                m_img_level_PolygonImage.gameObject.SetActive(false);
                m_lbl_lv_LanguageText.text = "";
                m_pl_stars_GridLayoutGroup.gameObject.SetActive(false);
                m_pl_summon_VerticalLayoutGroup.gameObject.SetActive(true);

                m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(181104, hero.itemCount, hero.config.getItemNum);
                if (hero.CanSummon())
                {
                    m_img_summon_PolygonImage.gameObject.SetActive(true);
                    m_img_headDark_PolygonImage.gameObject.SetActive(false);
                    m_UI_Model_CaptainHead.m_img_char_PolygonImage.color = heroHeadImageColor;
                }
                else
                {
                    m_img_summon_PolygonImage.gameObject.SetActive(false);
                    m_img_headDark_PolygonImage.gameObject.SetActive(false);
                    m_UI_Model_CaptainHead.m_img_char_PolygonImage.color = heroHeadImageColor;
                }
            }
            
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy != null)
            {
                if (playerProxy.CurrentRoleInfo.mainHeroId == hero.config.ID ||
                    playerProxy.CurrentRoleInfo.deputyHeroId == hero.config.ID)
                {
                    SetHeroState(RS.HeroState_defendCity);
                }
                else
                {
                    m_img_state_defense_PolygonImage.gameObject.SetActive(false);
                }
            }
        }

        private void SetEffect(int rare,int id)
        {
            m_pl_effect.gameObject.SetActive(false);
            if (id > 0)
            {
                HeroProxy heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                if (heroProxy != null)
                {
                    HeroProxy.Hero hero = heroProxy.GetHeroByID(id);
                    if (hero != null && hero.data != null && hero.IsAwakening())
                    {
                        if (rare == 5)
                        {
                            ClientUtils.LoadSprite(m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage, RS.HeroUpQualityBg[1]);
                            LoadEffect(RS.HeroUpEffect_5);
                        }
                        else if (rare == 4)
                        {
                            ClientUtils.LoadSprite(m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage, RS.HeroUpQualityBg[0]);
                            LoadEffect(RS.HeroUpEffect_4);
                        }
                    }
                }
            }
        }
        
        private void LoadEffect(string effectName)
        {
            m_pl_effect.gameObject.SetActive(true);
            if (m_pl_effect.transform.childCount > 0)
            {
                var childNode = m_pl_effect.transform.GetChild(0);

                if (childNode.name.Equals(effectName))
                {
                    childNode.gameObject.SetActive(true);
                    return;
                }

                GameObject.Destroy(childNode.gameObject);
            }
            
            CoreUtils.assetService.Instantiate(effectName, (effect) =>
            {
                effect.transform.SetParent(m_pl_effect.transform);
                effect.name = effectName;
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localEulerAngles = Vector3.zero;
                effect.transform.localScale = Vector3.one;
            });
        }

        private void SetHeroState(string path)
        {
            if (!string.IsNullOrEmpty(path))
                m_img_state_defense_PolygonImage.gameObject.SetActive(true);
            ClientUtils.LoadSprite(m_img_state_defense_PolygonImage, path);
        }

        private Color GetHeroHeadImageColor(HeroProxy.Hero hero)
        {
            if(hero.data != null)
            {
                return Color.white;
            }
            else if(hero.CanSummon())
            {
                return Color.white;
            }
            else
            {
                return new Color(70.0f / 255, 65.0f/255, 65.0f / 255, 1);
            }
        }
    }
}