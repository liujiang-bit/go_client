// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_CommandBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_CommandBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_CommandBtn";

        public UI_Model_CommandBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_CommandBtn_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_noTextButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_noTextButton_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_lbl1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_lbl2_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_CommandBtn_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_noTextButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_noTextButton");
			m_btn_noTextButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_noTextButton");
			m_btn_noTextButton_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_noTextButton");

			m_lbl_lbl1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_lbl1");

			m_lbl_lbl2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_lbl2");


			BindEvent();
        }

        #endregion
    }
}