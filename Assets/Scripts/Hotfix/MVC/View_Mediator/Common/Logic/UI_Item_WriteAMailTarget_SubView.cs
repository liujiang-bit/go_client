// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    UI_Item_WriteAMailTarget_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_WriteAMailTarget_SubView : UI_SubView
    {
        private bool m_isInit;
        private int m_index;
        public Action<int> DeleteTargetCallback;

        public void Refresh(WriteAMailData itemData, int index)
        {
            if (!m_isInit)
            {
                m_btn_clear_GameButton.onClick.AddListener(OnClear);
                m_isInit = true;
            }
            m_index = index;
            m_lbl_name_LanguageText.text = itemData.stableName;
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_UI_Item_WriteAMailTarget_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        private void OnClear()
        {
            if (DeleteTargetCallback != null)
            {
                DeleteTargetCallback(m_index);
            }
        }
    }
}