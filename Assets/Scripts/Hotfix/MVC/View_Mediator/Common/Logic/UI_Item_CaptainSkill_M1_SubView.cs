// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月4日
// Update Time         :    2020年3月4日
// Class Description   :    UI_Item_CaptainSkill_M1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;

namespace Game {
    public partial class UI_Item_CaptainSkill_M1_SubView : UI_Item_CaptainSkill_SubView
    {
        //private HelpTipData.Style style = HelpTipData.Style.arrowDown;
        //protected override void BindEvent()
        //{
        //    m_btn_Button_GameButton.onClick.AddListener(OnSkillClick);
        //}

        //protected override void OnSkillClick()
        //{
        //    var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(m_skillID);
        //    if (skillConfig == null)
        //    {
        //        Debug.LogError($"{m_skillID} not found!");
        //        return;
        //    }

        //    string strDesc = LanguageUtils.getTextFormat(166006, LanguageUtils.getText(skillConfig.l_nameID), LanguageUtils.getText(skillConfig.l_typeID));
        //    strDesc += "\r\n";
        //    if (skillConfig.anger > 0)
        //    {
        //        strDesc += LanguageUtils.getTextFormat(166007, skillConfig.anger);
        //        strDesc += "\r\n";
        //    }

        //    string subDesc = "";
        //    int showLevel = m_skillLevel;
        //    if (showLevel == 0)
        //        showLevel = 1;

        //    if (skillConfig.l_lvEffect4 != 0)
        //    {
        //        subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
        //            skillConfig.lvEffectDate1[showLevel - 1],
        //            skillConfig.lvEffectDate2[showLevel - 1],
        //            skillConfig.lvEffectDate3[showLevel - 1],
        //            skillConfig.lvEffectDate4[showLevel - 1]);
        //    }
        //    else if (skillConfig.l_lvEffect3 != 0)
        //    {
        //        subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
        //            skillConfig.lvEffectDate1[showLevel - 1],
        //            skillConfig.lvEffectDate2[showLevel - 1],
        //            skillConfig.lvEffectDate3[showLevel - 1]);
        //    }
        //    else if (skillConfig.l_lvEffect2 != 0)
        //    {
        //        subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
        //            skillConfig.lvEffectDate1[showLevel - 1],
        //            skillConfig.lvEffectDate2[showLevel - 1]);
        //    }
        //    else if (skillConfig.l_lvEffect1 != 0)
        //    {
        //        subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID,
        //            skillConfig.lvEffectDate1[showLevel - 1]);
        //    }
        //    else
        //    {
        //        subDesc = LanguageUtils.getTextFormat(skillConfig.l_mesID);
        //    }

        //    strDesc += LanguageUtils.getTextFormat(166008, subDesc);
        //    strDesc += "\r\n";

        //    if (skillConfig.open != 5)
        //    {
        //        string upgradeDesc = "";
        //        if (skillConfig.l_lvEffect1 != 0)
        //        {
        //            upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect1, m_skillLevel, skillConfig.lvEffectDate1);
        //            upgradeDesc += "\r\n";
        //        }
        //        if (skillConfig.l_lvEffect2 != 0)
        //        {
        //            upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect2, m_skillLevel, skillConfig.lvEffectDate2);
        //            upgradeDesc += "\r\n";
        //        }
        //        if (skillConfig.l_lvEffect3 != 0)
        //        {
        //            upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect3, m_skillLevel, skillConfig.lvEffectDate3);
        //            upgradeDesc += "\r\n";
        //        }
        //        if (skillConfig.l_lvEffect4 != 0)
        //        {
        //            upgradeDesc += GetEffectDataStr(skillConfig.l_lvEffect4, m_skillLevel, skillConfig.lvEffectDate4);
        //            upgradeDesc += "\r\n";
        //        }
        //        strDesc += "\r\n";
        //        strDesc += LanguageUtils.getTextFormat(166009, upgradeDesc);
        //        strDesc += "\r\n";
        //    }

        //    if (m_skillLevel == 0)
        //    {
        //        strDesc += "\r\n";
        //        strDesc += LanguageUtils.getTextFormat(166010, LanguageUtils.getText(skillConfig.l_openID));
        //    }

        //    var tipData = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(3003);

        //    HelpTip.CreateTip(strDesc, m_btn_Button_GameButton.transform).SetStyle(style).SetWidth(tipData.width).Show();
        //}
        //public void Setstyle(HelpTipData.Style style)
        //{
        //    this.style = style;
        //}

        //public override string GetEffectDataStr(int tid, int level, List<string> data)
        //{
        //    string strData = "";
        //    if (level == 0)
        //    {
        //        strData = LanguageUtils.getTextFormat(166011, data[0], data[1], data[2], data[3], data[4]);
        //    }
        //    else if (level == 1)
        //    {
        //        strData = LanguageUtils.getTextFormat(166011, LanguageUtils.getTextFormat(166012, data[0]), data[1], data[2], data[3], data[4]);
        //    }
        //    else if (level == 2)
        //    {
        //        strData = LanguageUtils.getTextFormat(166011, data[0], LanguageUtils.getTextFormat(166012, data[1]), data[2], data[3], data[4]);
        //    }
        //    else if (level == 3)
        //    {
        //        strData = LanguageUtils.getTextFormat(166011, data[0], data[1], LanguageUtils.getTextFormat(166012, data[2]), data[3], data[4]);
        //    }
        //    else if (level == 4)
        //    {
        //        strData = LanguageUtils.getTextFormat(166011, data[0], data[1], data[2], LanguageUtils.getTextFormat(166012, data[3]), data[4]);
        //    }
        //    else if (level == 5)
        //    {
        //        strData = LanguageUtils.getTextFormat(166011, data[0], data[1], data[2], data[3], LanguageUtils.getTextFormat(166012, data[4]));
        //    }
        //    return LanguageUtils.getTextFormat(166024, LanguageUtils.getText(tid), strData);
        //}

        //public void SetSkillInfo(HeroProxy.Hero hero, int idx)
        //{
        //    if (idx >= hero.config.skill.Count)
        //    {
        //        gameObject.SetActive(false);
        //        return;
        //    }
        //    gameObject.SetActive(true);
        //    var id = hero.config.skill[idx];
        //    m_skillID = id;
        //    m_hero = hero;

        //    var skillConfig = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(id);
        //    if (skillConfig == null)
        //    {
        //        Debug.LogError($"{id} not found!");
        //        return;
        //    }

        //    ClientUtils.LoadSprite(m_img_icon_PolygonImage, skillConfig.icon);
        //    ClientUtils.LoadSprite(m_img_icon_gray_PolygonImage, skillConfig.icon);
        //    if (skillConfig.open == 5)
        //    {
        //        m_img_icon_gray_PolygonImage.enabled = !hero.IsAllSkillMax();
        //    }
        //    else
        //    {
        //        m_img_icon_gray_PolygonImage.enabled = hero.star < skillConfig.open;
        //    }
        //    m_img_icon_gray_PolygonImage.gameObject.SetActive(m_img_icon_gray_PolygonImage.enabled);

        //    if (hero.data != null)
        //    {
        //        m_lbl_level_LanguageText.enabled = false;
        //        m_img_lvevlBg_PolygonImage.gameObject.SetActive(false);
        //        foreach (var skill in hero.data.skills)
        //        {
        //            if(id == skill.skillId)
        //            {
        //                m_lbl_level_LanguageText.text = skill.skillLevel.ToString();
        //                m_lbl_level_LanguageText.enabled = true;
        //                m_img_lvevlBg_PolygonImage.gameObject.SetActive(skill.skillLevel>0);
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (skillConfig.open == 1)
        //        {
        //            m_lbl_level_LanguageText.enabled = true;
        //            m_lbl_level_LanguageText.text = "1";
        //            m_skillLevel = 1;
        //            m_img_lvevlBg_PolygonImage.enabled = true;
        //            m_img_lvevlBg_PolygonImage.gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            m_lbl_level_LanguageText.enabled = false;
        //            m_img_lvevlBg_PolygonImage.enabled = false;
        //            m_img_lvevlBg_PolygonImage.gameObject.SetActive(false);
        //            m_skillLevel = 0;
        //        }
        //    }
        //}
    }
}