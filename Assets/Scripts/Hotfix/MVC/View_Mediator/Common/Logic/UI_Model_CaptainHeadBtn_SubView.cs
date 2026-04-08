// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Model_CaptainHeadBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Model_CaptainHeadBtn_SubView : UI_SubView
    {
        public int ItemData1;
        public object ItemData2;

        public Action<UI_Model_CaptainHeadBtn_SubView> BtnClickListener;

        public void AddBtnListener()
        {
            m_btn_languageButton_GameButton.onClick.AddListener(ClickCallback);
        }

        public void ClickCallback()
        {
            if (BtnClickListener != null)
            {
                BtnClickListener(this);
            }
        }
    }
}