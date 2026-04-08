// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    UI_Item_ExpeditionMapLevel_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ExpeditionMapLevel_SubView : UI_SubView
    {
        public void Show(ExpeditionLevelType levelType, ExpeditionInfo info, ExpeditionDefine cfg, bool isUnlock)
        {
            switch (levelType)
            {
                case ExpeditionLevelType.Normal:
                    m_img_flag1_PolygonImage.gameObject.SetActive(isUnlock);
                    m_img_flag2_PolygonImage.gameObject.SetActive(!isUnlock);
                    m_img_flag3_PolygonImage.gameObject.SetActive(false);
                    m_img_flag4_PolygonImage.gameObject.SetActive(false);
                    break;
                case ExpeditionLevelType.Boss:
                    m_img_flag1_PolygonImage.gameObject.SetActive(false);
                    m_img_flag2_PolygonImage.gameObject.SetActive(false);
                    m_img_flag3_PolygonImage.gameObject.SetActive(isUnlock);
                    m_img_flag4_PolygonImage.gameObject.SetActive(!isUnlock);
                    break;
            }
            m_lbl_level_LanguageText.text = $"{cfg.level}";
            m_pl_dark_GridLayoutGroup.gameObject.SetActive(isUnlock);
            m_pl_star_GridLayoutGroup.gameObject.SetActive(isUnlock);            
            if (isUnlock)
            {
                if(info != null)
                {
                    m_img_star1_PolygonImage.gameObject.SetActive(info.star >= 1);
                    m_img_star2_PolygonImage.gameObject.SetActive(info.star >= 2);
                    m_img_star3_PolygonImage.gameObject.SetActive(info.star >= 3);
                }
                else
                {
                    m_img_star1_PolygonImage.gameObject.SetActive(false);
                    m_img_star2_PolygonImage.gameObject.SetActive(false);
                    m_img_star3_PolygonImage.gameObject.SetActive(false);
                }
            }
            UpdateRewardByInfo(info);
        }

        public void UpdateRewardByInfo(ExpeditionInfo info)
        {
            m_img_box_PolygonImage.gameObject.SetActive(info != null && !info.reward && info.star == 3);
        }

        public void AddClickListener(UnityAction action)
        {
            m_btn_btn_GameButton.onClick.RemoveAllListeners();
            m_btn_btn_GameButton.onClick.AddListener(action);
        }
    }
}