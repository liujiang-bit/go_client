// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventHellBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventHellBox_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventHellBox";

        public UI_Item_EventHellBox_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventHellBox;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_box_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name0_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_source0_LanguageText;

		[HideInInspector] public LanguageText m_lbl_source1_LanguageText;

		[HideInInspector] public GameSlider m_sd_GameSlider_GameSlider;



        private void UIFinder()
        {       
			m_UI_Item_EventHellBox = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_box");

			m_lbl_name0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name0");

			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name1");

			m_lbl_source0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_source0");

			m_lbl_source1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_source1");

			m_sd_GameSlider_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_GameSlider");


			BindEvent();
        }

        #endregion
    }
}