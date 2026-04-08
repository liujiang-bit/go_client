// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月1日
// Update Time         :    2020年1月1日
// Class Description   :    UI_Model_TrainSoldierSkill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_TrainSoldierSkill_SubView : UI_SubView
    {
        public int index;

        public void SetSkillIcon(string image)
        {
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, image);
        }

        public void AddBtnListener(UnityAction<int, Transform> callback)
        {
            m_btn_bottom_GameButton.onClick.AddListener(()=> {
                callback(index, m_btn_bottom_GameButton.transform);
            });
        }
    }
}