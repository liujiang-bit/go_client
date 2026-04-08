// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 22, 2020
// Update Time         :    Wednesday, April 22, 2020
// Class Description   :    UI_Model_GuildRes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_GuildRes_SubView : UI_SubView
    {

        public void setNum(int num,bool isRich=true)
        {
            this.gameObject.SetActive(num>0);
            if (num>0)
            {
                this.m_lbl_resnum_LanguageText.text = ClientUtils.CurrencyFormat(num);

                if (isRich==false)
                {
                    ClientUtils.TextSetColor(m_lbl_resnum_LanguageText, Color.red);
                }
                else
                {
                    ClientUtils.TextSetColor(m_lbl_resnum_LanguageText,Color.black);
                }
            }
        }
    }
}