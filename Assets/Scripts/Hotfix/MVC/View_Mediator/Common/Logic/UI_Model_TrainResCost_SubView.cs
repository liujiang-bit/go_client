// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    UI_Model_TrainResCost_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Model_TrainResCost_SubView : UI_SubView
    {
        private bool m_isChangeInput;
        private bool m_isChangeSilder;
        private InputSliderControl m_control;

        public void Init( UnityAction<int, int> callback)
        {
            m_control = new InputSliderControl();
            m_control.Init(m_ipt_count_GameInput, m_sd_count_GameSlider, m_lbl_count_format_LanguageText, callback);
        }

        public void SetInputVal(int val)
        {
            if (m_control != null)
            {
                m_control.SetInputVal(val);
            }
        }

        public void SetSilderVal(float val)
        {
            if (m_control != null)
            {
                m_control.SetSilderVal(val);
            }
        }

        public void UpdateMinMax(int minVal, int maxVal)
        {
            if (m_control != null)
            {
                m_control.UpdateMinMax(minVal, maxVal);
            }
        }
    }

  
}