// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月13日
// Update Time         :    2020年4月13日
// Class Description   :    UI_Item_GraphicsSetting_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_GraphicsSetting_SubView : UI_SubView
    {

        protected override void BindEvent()
        {
            m_sd_framerate_GameSlider.onValueChanged.AddListener(OnFramerateSetting);
        }
        public void Refresh()
        {
           
            m_sd_picture_GameSlider.value = (float)QualitySetting.GetGraphicLevel();
            m_sd_framerate_GameSlider.value = (float)QualitySetting.GetFrameRateLevel();

            var defaultLevel = QualitySetting.GetDefaultGraphicLevel();
            int suggest = 300221;
            if (defaultLevel == CoreUtils.GraphicLevel.MEDIUM)
            {
                suggest = 300222;
            }
            else if (defaultLevel == CoreUtils.GraphicLevel.HIGH)
            {
                suggest = 300223;
            }
            //根据您的设备性能，我们推荐您选择图像质量:
            m_lbl_dec2_LanguageText.text = LanguageUtils.getTextFormat(300227, LanguageUtils.getText(suggest));
        }
        public void AddSdPictureEvent(UnityAction<float> unityAction)
        {
            m_sd_picture_GameSlider.onValueChanged.AddListener(unityAction);
        }
        private void OnFramerateSetting(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            QualitySetting.SetFrameRateLevel((FrameRateLevel)value);
        }

    }
}