// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月2日
// Update Time         :    2020年4月2日
// Class Description   :    UI_Item_MusicSetting_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_MusicSetting_SubView : UI_SubView
    {
        protected override void BindEvent()
        {
            m_sd_sound_GameSlider.onValueChanged.AddListener(OnSfxSliderChange);
            m_sd_music_GameSlider.onValueChanged.AddListener(OnBGMSliderChange);
        }
        public void Refresh()
        {
            m_sd_sound_GameSlider.value = CoreUtils.audioService.GetSfxVolume();
            m_sd_music_GameSlider.value = CoreUtils.audioService.GetMusicVolume();
        }

        private void OnSfxSliderChange(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            CoreUtils.audioService.SetSfxVolume(value);
            PlayerPrefs.SetFloat(RS.SfxVolume, value);
        }

        private void OnBGMSliderChange(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            CoreUtils.audioService.SetMusicVolume(value);
            PlayerPrefs.SetFloat(RS.BGMVolume, value);
        }
    }
}