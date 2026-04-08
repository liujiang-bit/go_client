// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Item_CaptainLevelUpOnHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_CaptainLevelUpOnHead_SubView : UI_SubView
    {
        public void SetData(HeroProxy.Hero m_hero)
        {
            if (m_hero == null)
            {
                return;
            }

            this.gameObject.SetActive(true);
            this.m_UI_Item_CaptainLevelUpOnHead_Animator.gameObject.SetActive(false);
            this.m_UI_Item_CaptainLevelUpOnHead_Animator.gameObject.SetActive(true);
            this.m_UI_Item_CaptainLevelUpOnHead_Animator.Play("Show");
            int soldiersAddCount = m_hero.levelConfig.soldiers;
            var preLevelConfig = CoreUtils.dataService.QueryRecord<Data.HeroLevelDefine>(m_hero.level + m_hero.config.rare * 10000 - 2);
            if(preLevelConfig != null)
            {
                soldiersAddCount -=  preLevelConfig.soldiers;
            }
            this.m_lbl_addatt_LanguageText.text = string.Format(LanguageUtils.getText(145049), soldiersAddCount);
            this.m_lbl_addatt1_LanguageText.gameObject.SetActive(false);
            //this.m_lbl_addatt1_LanguageText.text = string.Format(LanguageUtils.getText(145050), 1);;          
        }
    }
}