// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月13日
// Update Time         :    2020年6月13日
// Class Description   :    UI_Item_ExpeditionFightList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {

    public enum FightTroopViewType
    {
        Normal,
        Lock,
    }


    public partial class UI_Item_ExpeditionFightList_SubView : UI_SubView
    {
        public void SetViewType(FightTroopViewType viewType)
        {
            switch (viewType)
            {
                case FightTroopViewType.Lock:
                    m_img_key_PolygonImage.gameObject.SetActive(true);
                    
                    break;
            }            
        }

        public void SetTroop(HeroProxy.Hero hero)
        {
            m_UI_Model_CaptainHead.gameObject.SetActive(true);
            m_UI_Model_CaptainHead.SetHero(hero, hero.level);
            m_btn_reduce_GameButton.gameObject.SetActive(true);
        }

        public void RemoveTroop()
        {
            m_UI_Model_CaptainHead.gameObject.SetActive(false);
            m_btn_reduce_GameButton.gameObject.SetActive(false);
        }

        public void SetSelected(bool select)
        {
            m_img_select_PolygonImage.gameObject.SetActive(select);
        }

        public void AddRemoveListener(UnityAction action)
        {
            m_btn_reduce_GameButton.onClick.AddListener(action);
        }

        public void AddAddListener(UnityAction action)
        {
            m_img_normal_GameButton.onClick.AddListener(action);
        }
    }
}