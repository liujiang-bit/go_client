// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    UI_Model_Window_Type3_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_Window_Type3_SubView : UI_SubView
    {
        protected override void BindEvent()
        {

        }


        public void setWindowTitle(string title)
        {

            m_lbl_title_LanguageText.text = title;


        }

        public void setCloseHandle(UnityAction closeHandle)
        {
            Debug.Log("关闭窗口" + closeHandle);
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
    }
}