// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, January 7, 2020
// Update Time         :    Tuesday, January 7, 2020
// Class Description   :    UI_Model_PowerItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_PowerItem_SubView : UI_SubView
    {

        public void SetData(int langID, long value)
        {

            m_lbl_name_LanguageText.text = LanguageUtils.getText(langID);
            m_lbl_value_LanguageText.text = ClientUtils.FormatComma(value);
        }
    }
}