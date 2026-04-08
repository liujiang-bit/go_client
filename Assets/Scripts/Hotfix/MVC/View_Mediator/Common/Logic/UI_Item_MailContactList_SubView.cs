// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_Item_MailContactList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailContactList_SubView : UI_SubView
    {
        private WriteEmailItemData m_itemData;
        private bool m_isInit;
        public Action<UI_Item_MailContactList_SubView, UI_Item_MailContact_SubView> SelectCallback;
        public List<UI_Item_MailContact_SubView> m_listView;

        public void Refresh(WriteEmailItemData itemData)
        {
            InitData();
            m_itemData = itemData;

            if (itemData.DataType == 1)
            {
                int count = itemData.TargerDataList.Count;
                for (int i = 0; i < 2; i++)
                {
                    if (i < count)
                    {
                        m_listView[i].gameObject.SetActive(true);
                        m_listView[i].Refresh(itemData.TargerDataList[i], itemData.SelectedStatusList[i], i);
                    }
                    else
                    {
                        m_listView[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                int count = itemData.MemberDataList.Count;
                for (int i = 0; i < 2; i++)
                {
                    if (i < count)
                    {
                        m_listView[i].gameObject.SetActive(true);
                        m_listView[i].Refresh(itemData.MemberDataList[i], itemData.SelectedStatusList[i], i);
                    }
                    else
                    {
                        m_listView[i].gameObject.SetActive(false);
                    }
                }
            }

        }

        private void InitData()
        {
            if (!m_isInit)
            {
                m_UI_Item_MailContact1.SelectCallback = ContactBtnListener;
                m_UI_Item_MailContact2.SelectCallback = ContactBtnListener;
                m_listView = new List<UI_Item_MailContact_SubView>();
                m_listView.Add(m_UI_Item_MailContact1);
                m_listView.Add(m_UI_Item_MailContact2);

                m_isInit = true;
            }
        }

        private void ContactBtnListener(UI_Item_MailContact_SubView subView, int index)
        {
            m_itemData.SelectedStatusList[index] = subView.m_img_select_PolygonImage.gameObject.activeSelf;
            if (SelectCallback != null)
            {
                SelectCallback(this, subView);
            }
        }
    }
}