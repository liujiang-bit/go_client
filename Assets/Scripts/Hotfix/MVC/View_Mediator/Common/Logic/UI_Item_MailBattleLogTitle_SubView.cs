// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月2日
// Update Time         :    2020年7月2日
// Class Description   :    UI_Item_MailBattleLogTitle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_MailBattleLogTitle_SubView : UI_SubView
    {
        public void Refresh(long times, Vector2 pos)
        {
            DateTime dt = ServerTimeModule.Instance.ConverToServerDateTime(times);

            string dateStr = LanguageUtils.getTextFormat(200503, dt.Month, dt.Day);
            string timeStr = LanguageUtils.getTextFormat(200504, string.Format("{0:d2}", dt.Hour), string.Format("{0:d2}", dt.Minute), string.Format("{0:d2}", dt.Second));

            m_lbl_date_LanguageText.text = dateStr + " " + timeStr;

            int x = (int)pos.x;
            int y = (int)pos.y;
            m_UI_Model_Link.m_UI_Model_Link_LanguageText.text = LanguageUtils.getTextFormat(200505, x, y);
            m_UI_Model_Link.SetPos(x,y);
            m_UI_Model_Link.RegisterPosJumpEvent(OnPosJump);
        }

        public void OnPosJump()
        {
            CoreUtils.uiManager.CloseUI(UI.s_emailBattleLog);
        }
    }
}