// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月7日
// Update Time         :    2020年9月7日
// Class Description   :    UI_Item_ArmyConst_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ArmyConst_SubView : UI_SubView
    {

        private string nameCp1 = "";
        private string nameCp2 = "";
        public void SetCaptain1(long mainHeroId)
        {
            m_UI_Captain1.gameObject.SetActive(mainHeroId > 0);
            if (mainHeroId > 0)
            {
                   nameCp1 = m_UI_Captain1.LoadHeadID(mainHeroId,false);
            }
        }

        public void SetCaptain2(long deputyHeroId )
        {
            m_UI_Captain2.gameObject.SetActive(deputyHeroId > 0);
            if (deputyHeroId > 0)
            {
                nameCp2 = m_UI_Captain2.LoadHeadID(deputyHeroId, false);
            }
        }
        public void SetName()
        {
            if (string.IsNullOrEmpty(nameCp1) && string.IsNullOrEmpty(nameCp2))
            {
                m_lbl_captainName_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                m_lbl_captainName_LanguageText.gameObject.SetActive(true);
                m_lbl_captainName_LanguageText.text = string.IsNullOrEmpty(nameCp2) ? nameCp1:LanguageUtils.getTextFormat(300001, nameCp1, nameCp2);
            }
   
        }
        public void soldiers(long num)
        {
            m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(300263, num.ToString("N0"));
        }
        public void SetSelect(bool isSelect,bool canSelect = false)
        {
            if (canSelect)
            {
                m_img_arrow_down_PolygonImage.gameObject.SetActive(isSelect);
                m_img_arrow_up_PolygonImage.gameObject.SetActive(!isSelect);
            }
            else
            {
                m_img_arrow_down_PolygonImage.gameObject.SetActive(false);
                m_img_arrow_up_PolygonImage.gameObject.SetActive(false);
            }
        }
        public void AddItemEvent(UnityAction call)
        {
            m_btn_Join_GameButton.onClick.AddListener(call);
        }
        public void RemoveItemEvent()
        {
            m_btn_Join_GameButton.onClick.RemoveAllListeners();
        }
        public void InitData()
        {
            nameCp1 = "";
            nameCp2 = "";
        }
        public void Clear()
        {
            
        }


    }
}