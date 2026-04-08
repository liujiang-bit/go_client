// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_TalentPage_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;

namespace Game {
    public partial class UI_Item_TalentPage_SubView : UI_SubView
    {
        private HeroInfo.TalentTrees m_talentTree;
        private int m_index;
        private HeroProxy.Hero m_hero;
        
        protected override void BindEvent()
        {
            m_btn_name_GameButton.onClick.AddListener(OnEditorNameEvent);
            m_btn_page_GameButton.onClick.AddListener(OnClickEvent);
        }
        
        public void Init(HeroProxy.Hero heroInfo,int index,int SelectIndex)
        {
            m_index = index;
            if (heroInfo != null)
            {
                m_hero = heroInfo;
                m_talentTree = heroInfo.GetTalentTreesByIndex(index);
                if (heroInfo.talentIndex == index)
                {
                    m_img_use_PolygonImage.gameObject.SetActive(true);
                    m_img_unuse_PolygonImage.gameObject.SetActive(false);
                }
                else
                {
                    m_img_use_PolygonImage.gameObject.SetActive(false);
                    m_img_unuse_PolygonImage.gameObject.SetActive(true);
                }
                

                m_img_choose_PolygonImage.gameObject.SetActive(SelectIndex == index);
                m_lbl_page_LanguageText.text = m_talentTree != null && !string.IsNullOrEmpty(m_talentTree.name)?m_talentTree.name:LanguageUtils.getText(175020 + index - 1);
                var countList = heroInfo.GetActiveTalentCountByIndex(index);
                m_lbl_num3_LanguageText.text = countList[0].ToString();
                m_lbl_num2_LanguageText.text = countList[1].ToString();
                m_lbl_num1_LanguageText.text = countList[2].ToString();
                

            }
        }

        private void OnClickEvent()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ClickTalentPage,m_index);
        }

        private void OnEditorNameEvent()
        {
            if (m_hero != null)
            {
                CoreUtils.uiManager.ShowUI(UI.s_talentChangeNameAlert, null, new TalentChangeNameData(m_hero,m_index));
            }
        }
    }
}