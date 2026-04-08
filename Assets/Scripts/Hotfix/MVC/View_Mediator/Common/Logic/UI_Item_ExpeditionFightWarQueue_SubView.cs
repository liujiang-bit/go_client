// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_Item_ExpeditionFightWarQueue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Hotfix;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ExpeditionFightWarQueue_SubView : UI_SubView
    {
        public void Show(ArmyData armyData, UnityAction onClickListener)
        {
            if (armyData == null) return;
            HeroProxy heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            if (heroProxy == null) return;
            HeroProxy.Hero mainHero = heroProxy.GetHeroByID(armyData.heroId);
            if (mainHero == null) return;
            m_UI_Model_CaptainHead.SetHero(mainHero);
            if(TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
            {
                m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_GrayChildrens.Gray();
            }
            m_lbl_name_LanguageText.text = LanguageUtils.getText(mainHero.config.l_nameID);
            m_lbl_num_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
            m_btn_ck_GameButton.onClick.RemoveAllListeners();
            m_btn_ck_GameButton.onClick.AddListener(onClickListener);

            m_img_bg_GameButton.onClick.RemoveAllListeners();
            m_img_bg_GameButton.onClick.AddListener(onClickListener);
        }
    }
}