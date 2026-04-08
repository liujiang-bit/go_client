// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_CaptainTalent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_CaptainTalent_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_CaptainTalent";

        public UI_Model_CaptainTalent_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Model_CaptainTalent_Animator;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_btn_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_btn_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_btn_text_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_CaptainTalent_Animator = gameObject.GetComponent<Animator>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");
			m_btn_btn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn");

			m_btn_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/btn_text");
			m_btn_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/btn_text");


			BindEvent();
        }

        #endregion
    }
}