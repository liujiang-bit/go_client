// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_CaptainBar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_CaptainBar_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_CaptainBar";

        public UI_Model_CaptainBar_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_CaptainBar;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_value_LanguageText;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public BtnAnimation m_btn_add_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_CaptainBar = gameObject.GetComponent<RectTransform>();
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pb_rogressBar");

			m_lbl_value_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pb_rogressBar/lbl_value");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_add");
			m_btn_add_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_add");


			BindEvent();
        }

        #endregion
    }
}