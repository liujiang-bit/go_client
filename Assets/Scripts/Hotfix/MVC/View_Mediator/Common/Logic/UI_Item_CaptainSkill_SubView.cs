// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_CaptainSkill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Item_CaptainSkill_SubView : UI_SubView
    {
        public HeroProxy.Hero m_hero;
        public int m_skillID;
        public int m_skillLevel;
        private int m_skillTipShowType;
        private bool m_isHeroAwake = false;
        private int m_heroAwakeSkillId = 0;


        protected override void BindEvent()
        {
            m_btn_Button_GameButton.onClick.AddListener(OnSkillClick);
        }

        protected virtual void OnSkillClick()
        {
            var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(m_skillID);
            if (skillConfig == null)
            {
                Debug.LogError($"{m_skillID} not found!");
                return;
            }

            //技能觉醒并且是增强技能，用另一种Tips显示
            if(m_isHeroAwake)
            {
                var lastSkillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(m_heroAwakeSkillId);
                if (lastSkillConfig != null && lastSkillConfig.open == 5 && lastSkillConfig.type == 4 && lastSkillConfig.awakenEnhance == skillConfig.ID)
                {
                    ShowAwakenSkillTip(skillConfig, lastSkillConfig);
                    return;
                }
            }

            string strDesc = LanguageUtils.getTextFormat(166006, LanguageUtils.getText(skillConfig.l_nameID), LanguageUtils.getText(skillConfig.l_typeID));
            strDesc += "\r\n";
            if(skillConfig.anger > 0)
            {
                strDesc+= LanguageUtils.getTextFormat(166007, skillConfig.anger);
                strDesc += "\r\n";
            }

            if(skillConfig.type != 4)
            {
                string subDesc = getSkillEffect(skillConfig, m_skillLevel);
                strDesc += LanguageUtils.getTextFormat(166008, subDesc);
                strDesc += "\r\n";
            }          

            if (skillConfig.open != 5)
            {
                string upgradeDesc = "";
                if (skillConfig.l_lvEffect1 != 0)
                {
                    upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect1, m_skillLevel, skillConfig.lvEffectDate1);
                    upgradeDesc += "\r\n";
                }
                if (skillConfig.l_lvEffect2 != 0)
                {
                    upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect2, m_skillLevel, skillConfig.lvEffectDate2);
                    upgradeDesc += "\r\n";
                }
                if (skillConfig.l_lvEffect3 != 0)
                {
                    upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect3, m_skillLevel, skillConfig.lvEffectDate3);
                    upgradeDesc += "\r\n";
                }
                if (skillConfig.l_lvEffect4 != 0)
                {
                    upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect4, m_skillLevel, skillConfig.lvEffectDate4);
                    upgradeDesc += "\r\n";
                }
                strDesc += "\r\n";
                strDesc += LanguageUtils.getTextFormat(166009, upgradeDesc);
                strDesc += "\r\n";
            }
            else
            {
                if(skillConfig.type == 4)
                {
                    var awakenEnhanceSkillCfg = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(skillConfig.awakenEnhance);
                    if(awakenEnhanceSkillCfg != null)
                    {
                        strDesc += LanguageUtils.getTextFormat(166054, getSkillEffect(awakenEnhanceSkillCfg, 5),
                            getSkillEffect(skillConfig, m_skillLevel));
                        strDesc += "\r\n";
                    }
                }
            }

            if(m_skillLevel == 0)
            {
                strDesc += "\r\n";
                strDesc += LanguageUtils.getTextFormat(166010, LanguageUtils.getText(skillConfig.l_openID));
            }

            var tipData = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(3003);

            if (m_skillTipShowType == 1)
            {
                HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetWidth(tipData.width).Show();
            }
            else if(m_skillTipShowType == 0)
            {
                HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetAutoFilter(false).SetWidth(tipData.width).Show();
            }
            else if (m_skillTipShowType == 2)
            {
                HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).SetWidth(tipData.width).Show();
            }
            else
            {
                HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetAutoFilter(false).SetWidth(tipData.width).Show();
            }
        }

        public string getSkillEffect(HeroSkillDefine skillConfig, int showLevel)
        {
            string subDesc = "";
            if (showLevel == 0)
                showLevel = 1;

            if (skillConfig.l_lvEffect4 != 0)
            {
                subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
                    skillConfig.lvEffectDate1[showLevel - 1],
                    skillConfig.lvEffectDate2[showLevel - 1],
                    skillConfig.lvEffectDate3[showLevel - 1],
                    skillConfig.lvEffectDate4[showLevel - 1]);
            }
            else if (skillConfig.l_lvEffect3 != 0)
            {
                subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
                    skillConfig.lvEffectDate1[showLevel - 1],
                    skillConfig.lvEffectDate2[showLevel - 1],
                    skillConfig.lvEffectDate3[showLevel - 1]);
            }
            else if (skillConfig.l_lvEffect2 != 0)
            {
                subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
                    skillConfig.lvEffectDate1[showLevel - 1],
                    skillConfig.lvEffectDate2[showLevel - 1]);
            }
            else if (skillConfig.l_lvEffect1 != 0)
            {
                subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
                    skillConfig.lvEffectDate1[showLevel - 1]);
            }
            else
            {
                subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID);
            }
            return subDesc;
        }

        private void ShowAwakenSkillTip(HeroSkillDefine curSkill, HeroSkillDefine awakenSkill)
        {
            string strDesc = LanguageUtils.getTextFormat(166006, LanguageUtils.getText(curSkill.l_nameID), LanguageUtils.getText(curSkill.l_typeID));
            strDesc += "\r\n";
            if (curSkill.anger > 0)
            {
                strDesc += LanguageUtils.getTextFormat(166007, curSkill.anger);
                strDesc += "\r\n";
            }

            string subDesc = getSkillEffect(awakenSkill, 1);         

            strDesc += LanguageUtils.getTextFormat(166008, subDesc);
            strDesc += "\r\n";
            strDesc += LanguageUtils.getText(166053);

            var tipData = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(3003);

            if (m_skillTipShowType == 1)
            {
                HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetWidth(tipData.width).Show();
            }
            else
            {
                HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetAutoFilter(false).SetWidth(tipData.width).Show();
            }
        }

        public virtual string GetEffectDataStr(int tid, int level, List<string> data)
        {
            string strData = "";
            if (level == 0)
            {
                strData = LanguageUtils.getTextFormat(166011, data[0], data[1], data[2], data[3], data[4]);
            }
            else if(level == 1)
            {
                strData = LanguageUtils.getTextFormat(166011, LanguageUtils.getTextFormat(166012, data[0]), data[1], data[2], data[3], data[4]);
            }
            else if (level == 2)
            {
                strData = LanguageUtils.getTextFormat(166011, data[0], LanguageUtils.getTextFormat(166012, data[1]), data[2], data[3], data[4]);
            }
            else if (level == 3)
            {
                strData = LanguageUtils.getTextFormat(166011, data[0], data[1], LanguageUtils.getTextFormat(166012, data[2]), data[3], data[4]);
            }
            else if (level == 4)
            {
                strData = LanguageUtils.getTextFormat(166011, data[0], data[1], data[2], LanguageUtils.getTextFormat(166012, data[3]), data[4]);
            }
            else if (level == 5)
            {
                strData = LanguageUtils.getTextFormat(166011, data[0], data[1], data[2], data[3], LanguageUtils.getTextFormat(166012, data[4]));
            }
            return LanguageUtils.getTextFormat(166024, LanguageUtils.getText(tid), strData);
        }

        public void SetSkillInfo(HeroProxy.Hero hero, int idx, int skillTipShowType = 1)
        {
            m_skillTipShowType = skillTipShowType;
            if (idx >= hero.config.skill.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            var id = hero.config.skill[idx];
            m_skillID = id;
            m_hero = hero;

            var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(id);
            if(skillConfig == null)
            {
                Debug.LogError($"{id} not found!");
                return;
            }

            ClientUtils.LoadSprite(m_img_icon_PolygonImage, skillConfig.icon);
            ClientUtils.LoadSprite(m_img_icon_gray_PolygonImage, skillConfig.icon);
            if (skillConfig.open == 5)
            {
                m_img_icon_gray_PolygonImage.enabled = !hero.IsAllSkillMax();
            }
            else
            {
                m_img_icon_gray_PolygonImage.enabled = hero.star < skillConfig.open;
            }
            m_img_icon_gray_PolygonImage.gameObject.SetActive(m_img_icon_gray_PolygonImage.enabled);

            if (hero.data != null)
            {
                m_skillLevel = 0;
                m_lbl_level_LanguageText.enabled = false;
                m_img_lvevlBg_PolygonImage.enabled = false;
                foreach (var skill in hero.data.skills)
                {
                    if (id == skill.skillId)
                    {
                        m_lbl_level_LanguageText.text = skill.skillLevel.ToString();
                        m_skillLevel = (int)skill.skillLevel;
                        m_lbl_level_LanguageText.enabled = true;
                        m_img_lvevlBg_PolygonImage.enabled = true;
                        break;
                    }
                }
                m_isHeroAwake = hero.IsAllSkillMax();
                m_heroAwakeSkillId = hero.config.skill[hero.config.skill.Count - 1];
            }
            else
            {
                if (skillConfig.open == 1)
                {
                    m_lbl_level_LanguageText.enabled = true;
                    m_lbl_level_LanguageText.text = "1";
                    m_skillLevel = 1;
                    m_img_lvevlBg_PolygonImage.enabled = true;
                }
                else
                {
                    m_lbl_level_LanguageText.enabled = false;
                    m_img_lvevlBg_PolygonImage.enabled = false;
                    m_skillLevel = 0;
                }
            }
        }

        public void SetPreviewSkillInfo(HeroProxy.Hero hero, int idx)
        {
            m_btn_Button_GameButton.interactable = false;
            if (idx >= hero.config.skill.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            var id = hero.config.skill[idx];
            m_skillID = id;
            m_hero = hero;

            var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(id);
            if (skillConfig == null)
            {
                Debug.LogError($"{id} not found!");
                return;
            }

            ClientUtils.LoadSprite(m_img_icon_PolygonImage, skillConfig.icon);
            m_img_icon_gray_PolygonImage.gameObject.SetActive(false);
            m_lbl_level_LanguageText.enabled = false;
            m_img_lvevlBg_PolygonImage.enabled = false;
            m_skillLevel = 0;
        }

        public void SetSkillInfo(SkillInfo skillInfo, int star)
        {
            m_skillID = (int)skillInfo.skillId;
            m_skillLevel = (int)skillInfo.skillLevel;
            var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>((int)skillInfo.skillId);

            ClientUtils.LoadSprite(m_img_icon_PolygonImage, skillConfig.icon);
            m_img_icon_gray_PolygonImage.gameObject.SetActive(false);

            m_lbl_level_LanguageText.text = skillInfo.skillLevel.ToString();
            m_lbl_level_LanguageText.enabled = true;
            m_img_lvevlBg_PolygonImage.enabled = true; 
        }

        public void SetSkillInfo(int heroId, int skillId, int skillLevel, bool isHeroAwaken, bool needShowLevel)
        {
            HeroDefine heroCfg = CoreUtils.dataService.QueryRecord<HeroDefine>(heroId);
            if (heroCfg == null) return;
            if(isHeroAwaken)
            {
                m_isHeroAwake = true;
                m_heroAwakeSkillId = heroCfg.skill[heroCfg.skill.Count - 1];
            }

            m_skillID = skillId;
            m_skillLevel = skillLevel;

            var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(skillId);
            if (skillConfig == null)
            {
                return;
            }

            if(skillLevel > 0)
            {
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, skillConfig.icon);                
            }
            else
            {
                ClientUtils.LoadSprite(m_img_icon_gray_PolygonImage, skillConfig.icon);
            }
            ClientUtils.LoadSprite(m_img_icon_gray_PolygonImage, skillConfig.icon);
            m_img_icon_PolygonImage.gameObject.gameObject.SetActive(skillLevel > 0);
            m_img_icon_gray_PolygonImage.gameObject.gameObject.SetActive(skillLevel == 0);
            if(needShowLevel)
            {
                m_lbl_level_LanguageText.text = skillLevel.ToString();               
            }
            m_lbl_level_LanguageText.gameObject.SetActive(needShowLevel);
            m_img_lvevlBg_PolygonImage.gameObject.SetActive(needShowLevel);
        }
    }
}