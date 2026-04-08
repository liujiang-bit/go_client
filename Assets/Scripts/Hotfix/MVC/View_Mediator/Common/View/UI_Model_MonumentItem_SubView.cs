// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_MonumentItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_MonumentItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_MonumentItem";

        public UI_Model_MonumentItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_MonumentItem_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_animButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_animButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_animButton_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Shadow m_lbl_name_Shadow;



        private void UIFinder()
        {       
			m_UI_Model_MonumentItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_animButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton");
			m_btn_animButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_animButton");
			m_btn_animButton_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_animButton");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_animButton/lbl_name");


			BindEvent();
        }

        #endregion
    }
}