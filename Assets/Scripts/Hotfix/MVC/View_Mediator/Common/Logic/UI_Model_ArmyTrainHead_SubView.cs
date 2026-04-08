// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    UI_Model_ArmyTrainHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_ArmyTrainHead_SubView : UI_SubView
    {
        public int m_index;
        public Action<int> m_callback;
        public Action<int> m_cancelCallback;
        private Color m_colorHighlight = ClientUtils.HexRGBToColor("ffffff");
        private Color m_colorGray = ClientUtils.HexRGBToColor("918c8a");

        public void SetIndex(int val)
        {
            m_index = val;
        }

        public void SetHead(string image)
        {
            ClientUtils.LoadSprite(m_img_army_icon_PolygonImage, image);
        }

        public void SetNum(string num)
        {
            m_lbl_num_LanguageText.text = num;
        }

        public void SetUp(bool isUp)
        {
            m_img_up_PolygonImage.gameObject.SetActive(isUp);
        }

        public void SetGray(bool isGray)
        {
            if (isGray)
            {
                m_img_army_icon_GrayChildrens.Gray();
            } else
            {
                m_img_army_icon_GrayChildrens.Normal();
            }
        }

        public void SetSelectStatus(bool isShow)
        {
            m_img_select_PolygonImage.gameObject.SetActive(isShow);
            if (isShow)
            {
                m_lbl_num_LanguageText.color = m_colorHighlight;
            }
            else
            {
                m_lbl_num_LanguageText.color = m_colorGray;
            }
        }

        public void AddClickListener(Action<int> callback)
        {
            m_callback = callback;
            m_btn_event_GameButton.onClick.AddListener(()=> {
                m_callback(m_index);
            });
        }

        public void AddCancelListener(Action<int> callback)
        {
            m_cancelCallback = callback;
            m_btn_dismiss_GameButton.onClick.AddListener(()=> {
                m_cancelCallback(m_index);
            });
        }
    }
}