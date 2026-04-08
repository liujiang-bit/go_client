// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Item_BattleTroopsTipsCaptain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;

namespace Game {

    public class BattleTroopsTipHeroData
    {
        public int HeroId { get; set; }
        public int HeroLevel { get; set; }
        public int HeroStar { get; set; }
        public bool HeroAwake { get; set; }
        public List<int> HeroSkillLevel { get; set; }
    }

    public partial class UI_Item_BattleTroopsTipsCaptain_SubView : UI_SubView
    {
        private List<UI_Item_CaptainSkill_SubView> m_skillViewList = new List<UI_Item_CaptainSkill_SubView>();

        protected override void BindEvent()
        {
            m_skillViewList.Add(m_UI_Item_CaptainSkill1);
            m_skillViewList.Add(m_UI_Item_CaptainSkill2);
            m_skillViewList.Add(m_UI_Item_CaptainSkill3);
            m_skillViewList.Add(m_UI_Item_CaptainSkill4);
            m_skillViewList.Add(m_UI_Item_CaptainSkill5);
        }

        public void Show(BattleTroopsTipHeroData data)
        {
            Data.HeroDefine heroCfg = CoreUtils.dataService.QueryRecord<Data.HeroDefine>(data.HeroId);
            if (heroCfg == null) return;

            m_UI_Model_CaptainHead.SetHero(data.HeroId);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(heroCfg.l_nameID);
            m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(300003, data.HeroLevel);
            m_UI_Model_HeadStar1.gameObject.SetActive(data.HeroStar >= 1);
            m_UI_Model_HeadStar2.gameObject.SetActive(data.HeroStar >= 2);
            m_UI_Model_HeadStar3.gameObject.SetActive(data.HeroStar >= 3);
            m_UI_Model_HeadStar4.gameObject.SetActive(data.HeroStar >= 4);
            m_UI_Model_HeadStar5.gameObject.SetActive(data.HeroStar >= 5);
            m_UI_Model_HeadStar6.gameObject.SetActive(data.HeroStar >= 6);

            for (int i = 0; i < heroCfg.skill.Count && i < m_skillViewList.Count; ++i)
            {
                if(data.HeroSkillLevel.Count > i)
                {
                    m_skillViewList[i].SetSkillInfo(data.HeroId, heroCfg.skill[i], data.HeroSkillLevel[i], data.HeroAwake, false);
                }
                else if(i == heroCfg.skill.Count -1)
                {
                    m_skillViewList[i].SetSkillInfo(data.HeroId, heroCfg.skill[i], data.HeroAwake ? 1 : 0, data.HeroAwake, false);
                }
            }

            for(int i= heroCfg.skill.Count; i < m_skillViewList.Count;++i)
            {
                m_skillViewList[i].gameObject.SetActive(false);
            }
        }
    }
}