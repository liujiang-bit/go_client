// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_CaptainData_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using System.Collections.Generic;
using Data;

namespace Game {
    public partial class UI_Item_CaptainData_SubView : UI_SubView
    {
        private HeroProxy.Hero m_Hero;
        private List<UI_Model_CaptainStar_SubView> m_Stars = new List<UI_Model_CaptainStar_SubView>();
        private Dictionary<string,UI_Item_CaptainEquipUse_SubView> m_equipObjs = new Dictionary<string, UI_Item_CaptainEquipUse_SubView>();
        
        private int s_SkillMaxlLevel = 5;
        private int s_SkillMaxNum = 5;
        protected override void BindEvent()
        {
            m_UI_Model_StandardButton_Blue.AddClickEvent(OnSummonClick);
            m_UI_Model_CaptainHeadBar.AddClickEvent(OnAddItemClick);
            m_UI_Model_CaptainExpBar.AddClickEvent(OnAddExpClick);
            m_btn_arnyCountInfo_GameButton.onClick.AddListener(OnShowTroopCapClick);
            m_btn_title_GameButton.onClick.AddListener(OnShowTileClick);
        }
        private void OnAddExpClick()
        {
            if(m_Hero.IsLevelLimitByStar())
            {
                Tip.CreateTip(LanguageUtils.getText(145019)).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            CoreUtils.uiManager.ShowUI(UI.s_captainLevelUp, null, new CaptainLevelUpViewData() { Captain = m_Hero});
        }
        private void OnAddItemClick()
        {
            //CoreUtils.logService.Info("打开增加道具界面", Color.green);
            //Tip.CreateTip(100045).Show();
           
            CaptainItemSourceViewData data = new CaptainItemSourceViewData
            {
                ResourceType = EnumCaptainLevelResourceType.Summer,
                RequireItemId = m_Hero.config.getItem,
            };

            CoreUtils.uiManager.ShowUI(UI.s_captainItemSource, null, data);
        }
        private void OnTalentClick()
        {
            CoreUtils.logService.Info("打开天赋界面", Color.green);
            Tip.CreateTip(100045).Show();
        }
        private void OnSummonClick()
        {
            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            heroProxy.SummonHero(m_Hero.config.ID);
        }
        private void OnShowTroopCapClick()
        {
            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            var troopCapHero = playerAttributeProxy.GetHeroAttribute(m_Hero.config.ID, Data.attrType.troopsCapacity).origvalue;
            var troopCapBuild = playerAttributeProxy.GetCityAttribute(Data.attrType.troopsCapacity).origvalue;
            var troopCapHeroMuli = playerAttributeProxy.GetHeroAttribute(m_Hero.config.ID, Data.attrType.troopsCapacityMulti).value;
            var troopCapBuildMuli =  playerAttributeProxy.GetCityAttribute(Data.attrType.troopsCapacityMulti).value;
            long troopCapBase = troopCapHero + troopCapBuild;
            long troopCapTotal = Mathf.FloorToInt(troopCapBase * (1 + troopCapHeroMuli + troopCapBuildMuli));

            var troopCapHeroSkillSource = playerAttributeProxy.GetHeroSourceAttribute(m_Hero.config.ID, Data.attrType.troopsCapacityMulti, EnumSourceAttr.HeroSkill);
            string troopCapHeroSkillDesc = "0";
            if (troopCapHeroSkillSource.value > 0)
            {
                var troopCapHeroSkill = Mathf.FloorToInt(troopCapBase * troopCapHeroSkillSource.value);
                troopCapHeroSkillDesc = LanguageUtils.getTextFormat(300251, ClientUtils.FormatComma(troopCapHeroSkill), troopCapHeroSkillSource.GetShowValue());
            }
            var troopCapHeroTalentSource = playerAttributeProxy.GetHeroSourceAttribute(m_Hero.config.ID, Data.attrType.troopsCapacityMulti, EnumSourceAttr.HeroTalent);
            string troopCapHeroTalentDesc = "0";
            if (troopCapHeroTalentSource.value > 0)
            {
                var troopCapHeroTalent = Mathf.FloorToInt(troopCapBase * troopCapHeroTalentSource.value);
                troopCapHeroTalentDesc = LanguageUtils.getTextFormat(300251, ClientUtils.FormatComma(troopCapHeroTalent), troopCapHeroTalentSource.GetShowValue());
            }

            var data1 = LanguageUtils.getTextFormat(145024, ClientUtils.FormatComma(troopCapTotal), ClientUtils.FormatComma(playerProxy.GetTownHall()),
                ClientUtils.FormatComma(troopCapBuild), ClientUtils.FormatComma(troopCapHero), troopCapHeroSkillDesc, troopCapHeroTalentDesc);

            HelpTip.CreateTip(data1, m_btn_arnyCountInfo_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }
        private void OnShowTileClick()
        {
            var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)m_Hero.config.civilization);
            var data1 = LanguageUtils.getText(civilizationInfo.l_civilizationID);
            HelpTip.CreateTip(data1, m_btn_title_GameButton.transform ).SetStyle(HelpTipData.Style.arrowUp).Show();
        }
        public void AddShowMoreEvent(UnityAction call)
        {
            m_btn_info_GameButton.onClick.AddListener(call);
        }
        public void AddStarUpButtonClickedEvent(UnityAction call)
        {
            m_btn_starUp_GameButton.onClick.AddListener(call);
        }

        public void AddSkillUpButtonClickedEvent(UnityAction call)
        {
            m_UI_Model_StandardButton_skillup.AddClickEvent(call);
        }

        public void AddTalentButtonClickedEvent(UnityAction call)
        {
            m_UI_Model_StandardButton_gift.AddClickEvent(call);
        }

        public void setHero(HeroProxy.Hero hero)
        {
            m_Hero = hero;
            var heroInfo = hero.config;

            m_lbl_name_LanguageText.text = LanguageUtils.getText(heroInfo.l_nameID);
            m_lbl_title_LanguageText.text = LanguageUtils.getText(heroInfo.l_appellationID);

            var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)heroInfo.civilization);
            if (civilizationInfo != null)
            {
                ClientUtils.LoadSprite(m_img_titleimg_PolygonImage, civilizationInfo.civilizationMark);
                m_img_titleimg_PolygonImage.enabled = true;
            }
            else
            {
                m_img_titleimg_PolygonImage.enabled = false;
            }

            m_UI_Model_CaptainTalent1.SetTalentId(heroInfo.talent[0]);
            m_UI_Model_CaptainTalent2.SetTalentId(heroInfo.talent[1]);
            m_UI_Model_CaptainTalent3.SetTalentId(heroInfo.talent[2]);

            m_UI_Model_CaptainStar1.setHight(hero.star > 0);
            m_UI_Model_CaptainStar2.setHight(hero.star > 1);
            m_UI_Model_CaptainStar3.setHight(hero.star > 2);
            m_UI_Model_CaptainStar4.setHight(hero.star > 3);
            m_UI_Model_CaptainStar5.setHight(hero.star > 4);
            m_UI_Model_CaptainStar6.setHight(hero.star > 5);

            var starInfo = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>((int)hero.star);
            m_lbl_level_LanguageText.text = string.Format(LanguageUtils.getText(300003), string.Format(LanguageUtils.getText(300001), hero.level, starInfo.starLimit));

            if(hero.IsMaxLevel())
            {
                var levelInfo = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>((int)(heroInfo.rare * 10000 + hero.level - 2));
                m_UI_Model_CaptainExpBar.SetExp((int)levelInfo.exp, levelInfo.exp);
            }
            else
            {
                var levelInfo = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>((int)(heroInfo.rare * 10000 + hero.level - 1));
                m_UI_Model_CaptainExpBar.SetExp((int)hero.exp, levelInfo.exp);
            }
           

            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            var troopCapHero = playerAttributeProxy.GetHeroAttribute(hero.config.ID, attrType.troopsCapacity).origvalue;
            var troopCapBuild = playerAttributeProxy.GetCityAttribute(attrType.troopsCapacity).origvalue;
            var troopCapHeroMuli = playerAttributeProxy.GetHeroAttribute(hero.config.ID, attrType.troopsCapacityMulti).value;
            var troopCapBuildMuli = playerAttributeProxy.GetCityAttribute(attrType.troopsCapacityMulti).value;
            var troopCapTotal = Mathf.FloorToInt((troopCapHero + troopCapBuild) * (1 + troopCapHeroMuli + troopCapBuildMuli));

            m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(145000, troopCapTotal.ToString("N0"));
            
            var heroEquipTypes =  CoreUtils.dataService.QueryRecord<ConfigDefine>(0).heroEquipType;
            m_equipObjs.Clear();
            m_equipObjs.Add(heroEquipTypes[0] + "_1",m_UI_Item_EquipUse.m_UI_Equipslo1);
            m_equipObjs.Add(heroEquipTypes[1] + "_2",m_UI_Item_EquipUse.m_UI_Equipslo2);
            m_equipObjs.Add(heroEquipTypes[2] + "_3",m_UI_Item_EquipUse.m_UI_Equipslo3);
            m_equipObjs.Add(heroEquipTypes[3] + "_4",m_UI_Item_EquipUse.m_UI_Equipslo4);
            m_equipObjs.Add(heroEquipTypes[4] + "_5",m_UI_Item_EquipUse.m_UI_Equipslo5);
            m_equipObjs.Add(heroEquipTypes[5] + "_6",m_UI_Item_EquipUse.m_UI_Equipslo6);
            m_equipObjs.Add(heroEquipTypes[6] + "_7",m_UI_Item_EquipUse.m_UI_Equipslo7);
            m_equipObjs.Add(heroEquipTypes[7] + "_8",m_UI_Item_EquipUse.m_UI_Equipslo8);
            
            foreach (var keyValue in m_equipObjs)
            {
                keyValue.Value.InitEquipForThumbnail(keyValue.Key,hero);
            }

            if (hero.data != null)
            {
                m_pl_summon_ArabLayoutCompment.gameObject.SetActive(false);
                m_pl_featureBtn_HorizontalLayoutGroup.gameObject.SetActive(true);
                m_UI_Model_CaptainExpBar.ShowBtn(!hero.IsMaxLevel());
                m_UI_Model_CaptainHeadBar.ShowBtn(true);
                m_btn_starUp_GameButton.gameObject.SetActive(!hero.IsStarMaxLevel());
                m_pl_skill.gameObject.SetActive(true);
                m_pl_skill.SetHero(m_Hero);
                m_pl_armyCount_ArabLayoutCompment.gameObject.SetActive(true);

                //m_btn_info_GameButton.gameObject.SetActive(true);
                var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(hero.star);
                if (heroStarCfg != null)
                {
                    m_img_forbid_PolygonImage.gameObject.SetActive(hero.level < heroStarCfg.starLimit);
                }

                int talentPoint = m_Hero.GetCurPageRemainPoint(m_Hero.talentIndex);
                if (talentPoint > 0)
                {
                    m_UI_Model_StandardButton_gift.m_lbl_num_LanguageText.text = talentPoint.ToString();
                    m_UI_Model_StandardButton_gift.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                    m_UI_Model_StandardButton_gift.m_img_redpointNum_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_StandardButton_gift.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                    m_UI_Model_StandardButton_gift.m_img_redpointNum_PolygonImage.gameObject.SetActive(false);
                }
                if (m_Hero.IsCanUpSkill())
                {
                    m_UI_Model_StandardButton_skillup.m_lbl_num_LanguageText.text = "1";
                    m_UI_Model_StandardButton_skillup.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                    m_UI_Model_StandardButton_skillup.m_img_redpointNum_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    m_UI_Model_StandardButton_skillup.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                    m_UI_Model_StandardButton_skillup.m_img_redpointNum_PolygonImage.gameObject.SetActive(false);

                }

                

            }
            else
            {
                m_UI_Model_CaptainExpBar.ShowBtn(false);
                //m_UI_Model_CaptainHeadBar.ShowBtn(false);
                setCaptainHeadBarBtnShow();
                m_pl_summon_ArabLayoutCompment.gameObject.SetActive(true);
                m_pl_skill.gameObject.SetActive(false);
                m_btn_starUp_GameButton.gameObject.SetActive(false);
                m_UI_Item_SkillInSummon.SetHero(m_Hero);
                m_pl_armyCount_ArabLayoutCompment.gameObject.SetActive(false);
                //m_btn_info_GameButton.gameObject.SetActive(false);

                m_pl_featureBtn_HorizontalLayoutGroup.gameObject.SetActive(false);
                var starMax = 1;
                switch (hero.config.rare)
                {
                    case 1: starMax = starInfo.rare1; break;
                    case 2: starMax = starInfo.rare2; break;
                    case 3: starMax = starInfo.rare3; break;
                    case 4: starMax = starInfo.rare4; break;
                    case 5: starMax = starInfo.rare5; break;
                }
                m_UI_Model_CaptainHeadBar.SetExp((int)hero.itemCount, hero.config.getItemNum);
                if(hero.itemCount >= hero.config.getItemNum)
                {
                    m_UI_Model_StandardButton_Blue.Enable(true);
                }
                else
                {
                    m_UI_Model_StandardButton_Blue.Enable(false);
                }
                ClientUtils.LoadSprite(m_img_sumIcon_PolygonImage, hero.itemConfig.itemIcon);
                ClientUtils.LoadSprite(m_img_sumIconframe_PolygonImage, RS.ItemQualityBg[hero.config.rare - 1]);
                m_img_forbid_PolygonImage.gameObject.SetActive(false);
            }

            m_pl_starUp_effect_ArabLayoutCompment.gameObject.SetActive(!m_img_forbid_PolygonImage.gameObject.activeSelf && m_btn_starUp_GameButton.gameObject.activeSelf);

            // 功能不开发都隐藏掉
            //m_UI_Model_StandardButton_skillup.gameObject.SetActive(false);
            //m_UI_Model_StandardButton_gift.gameObject.SetActive(false);
            //m_btn_starUp_GameButton.gameObject.SetActive(false);
            //m_UI_Model_CaptainExpBar.ShowBtn(false);
            //m_UI_Model_CaptainHeadBar.ShowBtn(false);
        }

        //如果统帅的道具来源为空，入口置灰禁用。
        private void setCaptainHeadBarBtnShow()
        {
            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            if (heroProxy == null) return;
            var hero = heroProxy.GetHeroByID(m_Hero.config.ID);
            if (hero == null) return;
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(hero.config.getItem);
            if (itemCfg == null)
            {
                m_UI_Model_CaptainHeadBar.m_btn_add_GameButton.gameObject.SetActive(false);
            }
            else
            {
                GrayChildrens btnGray = m_UI_Model_CaptainHeadBar.m_btn_add_GameButton.gameObject.GetComponent<GrayChildrens>();
                if (btnGray == null)
                {
                    btnGray = m_UI_Model_CaptainHeadBar.m_btn_add_GameButton.gameObject.AddComponent<GrayChildrens>();
                }
                
                ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                List<int>  itemGetResourceData = itemCfg.get;
                if (itemGetResourceData.Count < 1 || (itemGetResourceData.Count==1&& configDefine.itemGetHide == itemGetResourceData[0]))
                {
                    btnGray.Gray();
                    m_UI_Model_CaptainHeadBar.m_btn_add_GameButton.enabled = false;
                }
                else
                {
                    btnGray.Normal();
                    m_UI_Model_CaptainHeadBar.m_btn_add_GameButton.gameObject.SetActive(true);
                    m_UI_Model_CaptainHeadBar.m_btn_add_GameButton.enabled = true;
                }
            }
        }
        
        public void Open()
        {
            gameObject.SetActive(true);
            var anim = gameObject.GetComponent<Animator>();
            if (!anim)
                return;
            if (LanguageUtils.IsArabic())
            {
                ClientUtils.PlayUIAnimation(anim, "OpenArb");
            }
            else
            {
                ClientUtils.PlayUIAnimation(anim, "OpenNoArb");
            }
            foreach (var keyValue in m_equipObjs)
            {
                keyValue.Value.InitEquipForThumbnail(keyValue.Key,m_Hero);
            }
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}