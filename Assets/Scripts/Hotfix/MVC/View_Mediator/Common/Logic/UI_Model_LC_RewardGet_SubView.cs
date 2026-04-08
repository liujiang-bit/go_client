// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月16日
// Update Time         :    2020年5月16日
// Class Description   :    UI_Model_LC_RewardGet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;
using Data;
using System;

namespace Game {
    public partial class UI_Model_LC_RewardGet_SubView : UI_SubView
    {
        private List<UI_Model_RewardGet_SubView> m_itemViewList;
        private bool m_isInit;
        private int itemCol;

        public Action<UI_Model_RewardGet_SubView> BtnClickListener;
        private int m_selectedIndex;

        public void Refresh(List<RewardGroupData> itemList)
        {
            if (!m_isInit)
            {
                Init();
                m_isInit = true;
            }

            int count = itemList.Count;
            for (int i = 0; i < itemCol; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(itemList[i], 4, true);
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
        }

        public void Init()
        {
            itemCol = m_UI_Model_LC_RewardGet_GridLayoutGroup.constraintCount;
            m_itemViewList = new List<UI_Model_RewardGet_SubView>();
            m_itemViewList.Add(m_UI_Model_RewardGet1);
            m_itemViewList.Add(m_UI_Model_RewardGet2);
            m_itemViewList.Add(m_UI_Model_RewardGet3);
        }

        //刷新(带有选中状态)
        public void Refresh2(List<RewardGroupData> itemList, int currColIndex, int selectIndex)
        {
            if (!m_isInit)
            {
                Init();

                if (BtnClickListener != null)
                {
                    for (int i = 0; i < m_itemViewList.Count; i++)
                    {
                        m_itemViewList[i].m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(true);
                        m_itemViewList[i].BtnClickListener = BtnClickListener;
                    }
                }

                m_isInit = true;
            }

            m_selectedIndex = -1;
            int baseIndex = currColIndex * itemCol;
            int count = itemList.Count;
            for (int i = 0; i < itemCol; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(itemList[i], 4, true);
                    int index = baseIndex + i;
                    m_itemViewList[i].m_UI_Model_Item.SetSelectImgActive(index == selectIndex);
                    m_itemViewList[i].ItemData3 = index;
                    if (index == selectIndex)
                    {
                        m_selectedIndex = i;
                    }
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
        }

        public UI_Model_RewardGet_SubView GetSelectedSubView()
        {
            if (m_selectedIndex > -1)
            {
                return m_itemViewList[m_selectedIndex];
            }
            return null;
        }
    }
}