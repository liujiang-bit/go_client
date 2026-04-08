// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月8日
// Update Time         :    2020年4月8日
// Class Description   :    UI_Model_TreatmentHeadAlign_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;

namespace Game {
    public partial class UI_Model_TreatmentHeadAlign_SubView : UI_SubView
    {
        private List<UI_Item_TreatmentHeadAlign_SubView> m_soldierImgList;
        private int m_imgCount;
        private bool m_isInit;

        public void RefreshHead(List<ArmsDefine> list)
        {
            if (!m_isInit)
            {
                m_soldierImgList = new List<UI_Item_TreatmentHeadAlign_SubView>(4);
                m_soldierImgList.Add(m_UI_Item_TreatmentHeadAlign1);
                m_soldierImgList.Add(m_UI_Item_TreatmentHeadAlign2);
                m_soldierImgList.Add(m_UI_Item_TreatmentHeadAlign3);
                m_soldierImgList.Add(m_UI_Item_TreatmentHeadAlign4);
                m_imgCount = m_soldierImgList.Count;
                m_isInit = true;
            }
            int count = list.Count;
            for (int i = 0; i < m_imgCount; i++)
            {
                if (i >= count)
                {
                    m_soldierImgList[i].gameObject.SetActive(false);
                }
                else
                {
                    m_soldierImgList[i].gameObject.SetActive(true);
                    ClientUtils.LoadSprite(m_soldierImgList[i].m_UI_Item_TreatmentHead.m_img_char_PolygonImage, list[i].icon);
                }
            }
        }
    }
}