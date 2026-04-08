// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ResourcesConsume_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_ResourcesConsume_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ResourcesConsume";

        public UI_Model_ResourcesConsume_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_ResourcesConsume_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;
		[HideInInspector] public BtnAnimation m_btn_btn_BtnAnimation;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_ResourcesConsume_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");
			m_btn_btn_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_btn");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_languageText");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_icon");


			BindEvent();
        }

        #endregion
    }
}