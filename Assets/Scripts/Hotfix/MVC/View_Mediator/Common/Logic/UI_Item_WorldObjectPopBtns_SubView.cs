// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月21日
// Update Time         :    2020年1月21日
// Class Description   :    UI_Item_WorldObjectPopBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_WorldObjectPopBtns_SubView : UI_SubView
    {
        public void SetAttackFightActive()
        {
            m_UI_Model_3gbtn1.gameObject.SetActive(true);
            m_UI_Model_3gbtn2.gameObject.SetActive(true);
            m_UI_Model_3gbtn3.gameObject.SetActive(false);
        }

    }
}