// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月6日
// Update Time         :    2020年2月6日
// Class Description   :    UI_Item_SkillInSummon_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_SkillInSummon_SubView : UI_SubView
    {
        public void SetHero(HeroProxy.Hero hero)
        {
            m_UI_Item_CaptainSkill_1.SetSkillInfo(hero, 0);
            m_UI_Item_CaptainSkill_2.SetSkillInfo(hero, 1);
            m_UI_Item_CaptainSkill_3.SetSkillInfo(hero, 2);
            m_UI_Item_CaptainSkill_4.SetSkillInfo(hero, 3);
            m_UI_Item_CaptainSkill_5.SetSkillInfo(hero, 4);
        }
    }
}