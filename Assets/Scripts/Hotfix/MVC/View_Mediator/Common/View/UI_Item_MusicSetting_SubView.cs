// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MusicSetting_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using System;

namespace Game {
    public partial class UI_Item_MusicSetting_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MusicSetting";

        public UI_Item_MusicSetting_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MusicSetting;
		[HideInInspector] public LanguageText m_lbl_dec1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec3_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec4_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec4_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_soundfra_PolygonImage;

		[HideInInspector] public GameSlider m_sd_sound_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_sound_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_musicfra_PolygonImage;

		[HideInInspector] public GameSlider m_sd_music_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_music_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MusicSetting = gameObject.GetComponent<RectTransform>();
			m_lbl_dec1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec1");
			m_lbl_dec1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec1");

			m_lbl_dec2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec2");
			m_lbl_dec2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec2");

			m_lbl_dec3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec3");
			m_lbl_dec3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec3");

			m_lbl_dec4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec4");
			m_lbl_dec4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec4");

			m_img_soundfra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_soundfra");

			m_sd_sound_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_sound");
			m_sd_sound_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sd_sound");

			m_img_musicfra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_musicfra");

			m_sd_music_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_music");
			m_sd_music_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sd_music");


			BindEvent();
        }
        #endregion
    }
}