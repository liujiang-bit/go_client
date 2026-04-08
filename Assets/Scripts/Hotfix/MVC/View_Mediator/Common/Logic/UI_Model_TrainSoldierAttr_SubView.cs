// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月1日
// Update Time         :    2020年1月1日
// Class Description   :    UI_Model_TrainSoldierAttr_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Model_TrainSoldierAttr_SubView : UI_SubView
    {
        private int m_tipId;

        public void SetTipId(int id)
        {
            m_tipId = id;
        }

        public void SetDesc(string str)
        {
            m_lbl_desc_LanguageText.text = str;
        }

        public void SetNum(string str)
        {
            m_lbl_num_LanguageText.text = str;
        }

        public void AddBtnListener(Action<object, Transform> callback)
        {
            m_btn_anim_GameButton.onClick.AddListener(()=> {
                callback(m_tipId, m_btn_anim_GameButton.transform);
            });
        }
    }
}