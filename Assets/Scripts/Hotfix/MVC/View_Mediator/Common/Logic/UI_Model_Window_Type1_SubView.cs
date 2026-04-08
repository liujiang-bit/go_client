// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Model_Window_Type1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_Window_Type1_SubView : UI_SubView
    {
        private UI_Model_SideToggle_SubView[] m_pageItems = new UI_Model_SideToggle_SubView[4];
        protected override void BindEvent()
        {
            
        }


        public void setWindowTitle(string title)
        {

            m_lbl_title_LanguageText.text = title;
            
            
        }

        public void setCloseHandle( UnityAction closeHandle)
        {
            Debug.Log("关闭窗口"+closeHandle);
            m_btn_close_GameButton.onClick.RemoveAllListeners();
            m_btn_close_GameButton.onClick.AddListener(closeHandle);
        }
        public void AddCloseEvent(UnityAction call)
        {
            m_btn_close_GameButton.onClick.AddListener(call);
        }
        public void AddBackEvent(UnityAction call)
        {
            m_btn_back_GameButton.onClick.AddListener(call);
        }

        public void AddPageListener(int index, UnityAction<bool> call)
        {
            switch (index)
            {
                case 0:
                    m_pageItems[index] = m_pl_side1;
                    break;
                case 1:
                    m_pageItems[index] = m_pl_side2;
                    break;
                case 2:
                    m_pageItems[index] = m_pl_side3;
                    break;
                case 3:
                    m_pageItems[index] = m_pl_side4;
                    break;
            }
            m_pageItems[index].m_ck_ck_GameToggle.onValueChanged.RemoveAllListeners();
            m_pageItems[index].m_ck_ck_GameToggle.onValueChanged.AddListener(call);
        }

        public void ActivePage(int index)
        {
            if (m_pageItems.Length <= index)
            {
                return;
            }
            m_pageItems[index].m_ck_ck_GameToggle.isOn = true;
        }

        public void ShowRedDotInPageSide(int index, int num)
        {
            if (m_pageItems[index] == null)
            {
                return;
            }
            m_pageItems[index].m_UI_Common_Redpoint.ShowRedPoint(num);
        }
    }
}