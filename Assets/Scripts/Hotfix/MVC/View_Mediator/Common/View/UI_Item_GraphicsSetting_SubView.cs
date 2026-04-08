// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GraphicsSetting_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_GraphicsSetting_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GraphicsSetting";

        public UI_Item_GraphicsSetting_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_GraphicsSetting;
		[HideInInspector] public LanguageText m_lbl_dec1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec3_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec4_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec4_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_picture_PolygonImage;

		[HideInInspector] public GameSlider m_sd_picture_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_picture_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_framerate_PolygonImage;

		[HideInInspector] public GameSlider m_sd_framerate_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_framerate_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_GraphicsSetting = gameObject.GetComponent<RectTransform>();
			m_lbl_dec1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec1");
			m_lbl_dec1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec1");

			m_lbl_dec2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec2");
			m_lbl_dec2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec2");

			m_lbl_dec3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec3");
			m_lbl_dec3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec3");

			m_lbl_dec4_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec4");
			m_lbl_dec4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec4");

			m_img_picture_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_picture");

			m_sd_picture_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_picture");
			m_sd_picture_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sd_picture");

			m_img_framerate_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_framerate");

			m_sd_framerate_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_framerate");
			m_sd_framerate_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sd_framerate");


			BindEvent();
        }

        #endregion
    }
}