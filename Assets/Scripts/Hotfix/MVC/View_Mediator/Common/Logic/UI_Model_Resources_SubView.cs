// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    UI_Model_Resources_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Model_Resources_SubView : UI_SubView
    {
        /// <summary>
        /// 0,千分位添加逗号，1货币化显示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        /// <param name="Forma"></param>
        public void SetRes(int id , long num,int formatType)
        {
            CurrencyDefine define =
                CoreUtils.dataService.QueryRecord<CurrencyDefine>(id);
            ClientUtils.LoadSprite(m_img_icon_PolygonImage , define.iconID);
            if (formatType == 0)
            {
                m_lbl_val_LanguageText.text = num.ToString("N0");
            }
            else if (formatType == 1)
            {
                m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(num);
            }
            else
            {
                m_lbl_val_LanguageText.text = num.ToString("N0");
            }

        }
    }
}