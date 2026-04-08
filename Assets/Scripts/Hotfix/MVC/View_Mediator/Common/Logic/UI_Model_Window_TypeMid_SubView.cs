// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    UI_Model_Window_TypeMid_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_Window_TypeMid_SubView : UI_SubView
    {
        
        public void setWindowTitle(string title)
        {

            m_lbl_title_LanguageText.text = title;
            
            
        }

        public void SetCloseVisible(bool bVisible)
        {
            m_btn_close_GameButton.gameObject.SetActive(bVisible);
        }

        public void setCloseHandle( UnityAction closeHandle)
        {
            Debug.Log("关闭窗口"+closeHandle);
            m_btn_close_GameButton.onClick.RemoveAllListeners();
            m_btn_close_GameButton.onClick.AddListener(closeHandle);
        }
        public void setBackHandle(UnityAction closeHandle)
        {
            Debug.Log("关闭窗口" + closeHandle);
            m_btn_back_GameButton.onClick.RemoveAllListeners();
            m_btn_back_GameButton.onClick.AddListener(closeHandle);
        }
    }
}