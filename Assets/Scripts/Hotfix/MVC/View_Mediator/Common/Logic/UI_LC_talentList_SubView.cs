// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_LC_talentList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_LC_talentList_SubView : UI_SubView
    {
        public void Init(HeroProxy.Hero heroInfo,int tabIndex,HeroTalentGainTreeDefine defineBlue,HeroTalentGainTreeDefine defineRed,HeroTalentGainTreeDefine defineYellow,RectTransform parentRoot,UI_Item_TalentSkillPop_SubView popView)
        {
            if (defineBlue != null)
            {
                m_UI_Item_TalentSkill3.gameObject.SetActive(true);
                m_UI_Item_TalentSkill3.Init(heroInfo,tabIndex,defineBlue,parentRoot,popView);
            }
            else
            {
                m_UI_Item_TalentSkill3.gameObject.SetActive(false);
            }
            
            if (defineRed != null)
            {
                m_UI_Item_TalentSkill2.gameObject.SetActive(true);
                m_UI_Item_TalentSkill2.Init(heroInfo,tabIndex,defineRed,parentRoot,popView);
            }
            else
            {
                m_UI_Item_TalentSkill2.gameObject.SetActive(false);
            }
            
            if (defineYellow != null)
            {
                m_UI_Item_TalentSkill1.gameObject.SetActive(true);
                m_UI_Item_TalentSkill1.Init(heroInfo,tabIndex,defineYellow,parentRoot,popView);
            }
            else
            {
                m_UI_Item_TalentSkill1.gameObject.SetActive(false);
            }
        }
    }
}