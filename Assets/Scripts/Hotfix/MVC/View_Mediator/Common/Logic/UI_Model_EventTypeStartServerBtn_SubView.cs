// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    UI_Model_EventTypeStartServerBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_EventTypeStartServerBtn_SubView : UI_SubView
    {
        public void SetSelectStatus(bool isSelect)
        {
            if (isSelect)
            {
                ClientUtils.TextSetColor(m_lbl_name_LanguageText, "#ffffff");
            }
            else
            {
                ClientUtils.TextSetColor(m_lbl_name_LanguageText, "#a49d92");
            }
            m_img_select_PolygonImage.gameObject.SetActive(isSelect);
        }

        public void SetLockStatus(bool isLock)
        {
            m_img_lock_PolygonImage.gameObject.SetActive(isLock);
        }

        public void SetRedpotStatus(bool isShow)
        {
            m_img_red_PolygonImage.gameObject.SetActive(isShow);
        }
    }
}