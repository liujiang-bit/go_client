// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_Model_EquipAtt_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Model_EquipAtt_SubView : UI_SubView
    {
        public void SetAttInfo(int attrID, int attAdd,float plus = -1)
        {
            var attrCfg = CoreUtils.dataService.QueryRecord<EquipAttDefine>(attrID);
            if (attrCfg.l_nameID == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            m_lbl_equipAtt_LanguageText.text = LanguageUtils.getTextFormat(182063,LanguageUtils.getText(attrCfg.l_nameID),attAdd);
            if (plus > 0)
            {
                m_lbl_plus_LanguageText.gameObject.SetActive(true);
                var plusValue = Mathf.Round(plus * attAdd * 2) / 2;
                m_lbl_plus_LanguageText.text = LanguageUtils.getTextFormat(500783,plusValue);   
            }
            else
            {
                m_lbl_plus_LanguageText.gameObject.SetActive(false);
            }

            ClientUtils.LoadSprite(m_img_icon_PolygonImage, attrCfg.icon);
            Color color;
            if (!ColorUtility.TryParseHtmlString(attrCfg.color, out color))
            {
                color = Color.white;
            }
            m_img_icon_PolygonImage.color = color;

        }
    }
}