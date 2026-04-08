// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_Item_MainIFArm_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Hotfix;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_MainIFArm_SubView : UI_SubView
    {
        public void Show(ArmyData armyData, UnityAction onClickAction, UnityAction onPressClickAction)
        {
            if (armyData == null) return;
            HeroProxy heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            if (heroProxy == null) return;
            HeroProxy.Hero mainHero = heroProxy.GetHeroByID(armyData.heroId);
            if (mainHero == null) return;
            m_UI_Model_CaptainHead.SetHero(mainHero);
            if(TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
            {
                m_UI_Item_MainIFArm_GrayChildrens.Gray();
            }
            ClientUtils.LoadSprite(m_img_collect_PolygonImage, TroopHelp.GetIcon(armyData.armyStatus));
            m_btn_btn_GameButton.onClick.RemoveAllListeners();
            m_btn_btn_GameButton.onClick.AddListener(onClickAction);
            
            m_btn_btn_UIPressBtn.RemoveAllPressClick();
            m_btn_btn_UIPressBtn.Register(0,true);
            m_btn_btn_UIPressBtn.AddPressClick(() =>
            {
                if (onPressClickAction != null)
                {
                    onPressClickAction.Invoke();
                }
            });

            m_UI_Common_TroopsState.SetData((long)armyData.armyStatus);
        }

        public void SetChooseState(bool state)
        {
            m_img_choose_PolygonImage.gameObject.SetActive(state);
        }
    }
}