// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_Item_EquipTalent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_EquipTalent_SubView : UI_SubView
    {
        public void SetInfo(int talentID,bool clickable,UnityAction<int> callback = null)
        {
            var talentCfg = CoreUtils.dataService.QueryRecord<HeroTalentTypeDefine>(talentID);

            m_lbl_name_LanguageText.text = LanguageUtils.getText(talentCfg.l_talentID);
            ClientUtils.LoadSprite(m_img_talent_PolygonImage, talentCfg.equipIcon);

            m_btn_btn_GameButton.interactable = clickable;
            if (clickable)
            {
                m_btn_btn_GameButton.onClick.RemoveAllListeners();
                m_btn_btn_GameButton.onClick.AddListener(() => { callback?.Invoke(talentID); });
            }
        }
    }
}