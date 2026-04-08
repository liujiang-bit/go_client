// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_EquipComposeAtt_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_EquipComposeAtt_SubView : UI_SubView
    {
        private Color originColor;
        protected override void BindEvent()
        {
            base.BindEvent();
            originColor = m_UI_Item_EquipComposeAtt_LanguageText.color;
        }

        public void SetText(string txt,bool isActive)
        {
            m_UI_Item_EquipComposeAtt_LanguageText.text = txt;
            m_UI_Item_EquipComposeAtt_LanguageText.color = isActive ? originColor : Color.gray;
        }
    }
}