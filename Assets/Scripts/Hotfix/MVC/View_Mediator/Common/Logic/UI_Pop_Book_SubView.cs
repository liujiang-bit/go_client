// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月19日
// Update Time         :    2020年9月19日
// Class Description   :    UI_Pop_Book_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public enum MapMarkerTranslateState
    {
        NoTranslation,
        Translating,
        Translated
    }

    public partial class UI_Pop_Book_SubView : UI_SubView
    {
        private MapMarkerTranslateState m_translateState = MapMarkerTranslateState.NoTranslation;
        private string m_description = string.Empty;

        protected override void BindEvent()
        {
            m_btn_translate_GameButton.onClick.AddListener(OnTranslateBtnClick);
        }

        public void SetDescription(string desc)
        {
            m_description = desc;

            m_lbl_text_LanguageText.text = desc;
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_img_bg_PolygonImage.rectTransform);

            m_translateState = MapMarkerTranslateState.NoTranslation;
        }

        private void OnTranslateBtnClick()
        {
            if (m_translateState == MapMarkerTranslateState.NoTranslation)
            {
                GameHelper.GetTranslator().translateText(new IGGTranslationSource(m_description), (value1) => {
                    m_lbl_text_LanguageText.text = value1.getByIndex(0).getText();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(m_img_bg_PolygonImage.rectTransform);

                    m_translateState = MapMarkerTranslateState.Translated;
                }, (value2, value3) => {
                    m_translateState = MapMarkerTranslateState.NoTranslation;
                });

                m_translateState = MapMarkerTranslateState.Translating;
            }
            else if (m_translateState == MapMarkerTranslateState.Translated)
            {
                m_lbl_text_LanguageText.text = m_description;
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_img_bg_PolygonImage.rectTransform);

                m_translateState = MapMarkerTranslateState.NoTranslation; 
            }            
        }
    }
}