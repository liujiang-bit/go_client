// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    UI_Item_CMDBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using System.Collections.Generic;
using Hotfix;

namespace Game {

    public class CMDBtnsParam
    {
        public TroopHudIconType IconType { get; set; }
        public object ClickParam { get; set; }        
        public Action<object> OnBtnClickListener { get; set; }
    }

    public partial class UI_Item_CMDBtns_SubView : UI_SubView
    {
        private List<CMDBtnsParam> m_btnParams = null;

        public void SetRallTroopActive(bool isShow)
        {
            this.m_UI_Model_CommandBtn3.gameObject.SetActive(isShow);
            this.m_UI_Model_CommandBtn4.gameObject.SetActive(isShow);
            this.m_UI_Model_CommandBtn5.gameObject.SetActive(isShow);
            SetTroopActive(false);
        }

        public void SetTroopActive(bool isShow)
        {
            this.m_UI_Model_CommandBtn.gameObject.SetActive(isShow);
            this.m_UI_Model_CommandBtn1.gameObject.SetActive(isShow);
        }

        public void SetBtn0Active(bool isShow)
        {
            this.m_UI_Model_CommandBtn0.gameObject.SetActive(isShow);
        }

        public  void SetBtnParams(List<CMDBtnsParam> btnParams)
        {
            if (btnParams == null || btnParams.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            m_btnParams = btnParams;
            int count = btnParams.Count;
            SetBtnCount(count);
            switch (count)
            {
                case 1:
                    InitBtn(m_UI_Model_CommandBtn0, 0);
                    break;
                case 2:
                    InitBtn(m_UI_Model_CommandBtn, 0);
                    InitBtn(m_UI_Model_CommandBtn1, 1);
                    break;
                case 3:
                    InitBtn(m_UI_Model_CommandBtn3, 0);
                    InitBtn(m_UI_Model_CommandBtn4, 1);
                    InitBtn(m_UI_Model_CommandBtn5, 2);
                    break;
                case 4:
                    InitBtn(m_UI_Model_CommandBtn6, 0);
                    InitBtn(m_UI_Model_CommandBtn7, 1);
                    InitBtn(m_UI_Model_CommandBtn8, 2);
                    InitBtn(m_UI_Model_CommandBtn9, 3);
                    break;
            }
        }

        private void InitBtn(UI_Model_CommandBtn_SubView btn, int index)
        {
            ClientUtils.LoadSprite(btn.m_btn_noTextButton_PolygonImage, TroopHelp.GetTroopHudIcon(m_btnParams[index].IconType));
            btn.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
            btn.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                if (m_btnParams == null || index >= m_btnParams.Count) return;
                m_btnParams[index].OnBtnClickListener?.Invoke(m_btnParams[index].ClickParam);
            }); 
        }

        private void SetBtnCount(int count)
        {
            switch (count)
            {
                case 1:
                    m_UI_Model_CommandBtn0.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn1.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn3.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn4.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn5.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn6.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn7.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn8.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn9.gameObject.SetActive(false);
                    break;
                case 2:
                    m_UI_Model_CommandBtn0.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn1.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn3.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn4.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn5.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn6.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn7.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn8.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn9.gameObject.SetActive(false);
                    break;
                case 3:
                    m_UI_Model_CommandBtn0.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn1.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn3.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn4.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn5.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn6.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn7.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn8.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn9.gameObject.SetActive(false);
                    break;
                case 4:
                    m_UI_Model_CommandBtn0.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn1.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn3.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn4.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn5.gameObject.SetActive(false);
                    m_UI_Model_CommandBtn6.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn7.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn8.gameObject.SetActive(true);
                    m_UI_Model_CommandBtn9.gameObject.SetActive(true);
                    break;
            }
        }
    }
}