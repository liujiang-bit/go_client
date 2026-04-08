// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_TrainResCost_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_TrainResCost_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_TrainResCost";

        public UI_Model_TrainResCost_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_TrainResCost;
		[HideInInspector] public PolygonImage m_ipt_count_PolygonImage;
		[HideInInspector] public GameInput m_ipt_count_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_count_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_count_format_LanguageText;

		[HideInInspector] public GameSlider m_sd_count_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_count_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ResCost_SubView m_UI_Model_ResCost;


        private void UIFinder()
        {       
			m_UI_Model_TrainResCost = gameObject.GetComponent<RectTransform>();
			m_ipt_count_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ipt_count");
			m_ipt_count_GameInput = FindUI<GameInput>(gameObject.transform ,"ipt_count");
			m_ipt_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ipt_count");

			m_lbl_count_format_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ipt_count/lbl_count_format");

			m_sd_count_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_count");
			m_sd_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sd_count");

			m_UI_Model_ResCost = new UI_Model_ResCost_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_ResCost"));

			BindEvent();
        }

        #endregion
    }
}