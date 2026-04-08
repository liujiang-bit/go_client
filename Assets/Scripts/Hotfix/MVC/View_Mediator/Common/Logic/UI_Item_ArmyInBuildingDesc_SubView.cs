// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Item_ArmyInBuildingDesc_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_ArmyInBuildingDesc_SubView : UI_SubView
    {
        public int m_index;
        public Action<int> m_callback;
        public Action<int> m_dismissCallback;

        public void SetIndex(int val)
        {
            m_index = val;
        }

        public void SetHead(string image)
        {
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, image);
        }

        public void SetGray(bool isGray)
        {
            if (isGray)
            {
                m_img_icon_MakeChildrenGray.Gray();
            }
            else
            {
                m_img_icon_MakeChildrenGray.Normal();
            }
        }

        public void SetNum(string num)
        {
            m_lbl_count_LanguageText.text = num;
        }

        public void AddDissMissListener(Action<int> callback)
        {
            m_dismissCallback = callback;
            m_btn_des_GameButton.onClick.AddListener(() => {
                m_dismissCallback(m_index);
            });
        }

        public void SetDissMissBtnShow(bool isHide)
        {
            m_btn_des_GameButton.gameObject.SetActive(isHide);
        }
    }
}