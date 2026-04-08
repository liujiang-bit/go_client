// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月7日
// Update Time         :    2020年1月7日
// Class Description   :    UI_Item_BuildingUpLimit_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game
{
    public partial class UI_Item_BuildingUpLimit_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction call)
        {
            m_UI_Model_StandardButton_Blue.AddClickEvent(call);
        }
        public void SetIcon(string assetName)
        {
            CoreUtils.assetService.Instantiate(assetName, (gameObject) =>
             {
                 gameObject.transform.SetParent(m_pl_TechIcon);
                 gameObject.transform.localPosition = Vector3.zero;
                 gameObject.transform.localScale = Vector3.one;
             });
        }

        public void SetName(string name)
        {
            m_lbl_languageText_LanguageText.text = name;
        }
    }
}