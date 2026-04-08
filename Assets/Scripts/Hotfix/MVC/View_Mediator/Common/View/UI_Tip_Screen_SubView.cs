// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Tip_Screen_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Tip_Screen_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Tip_Screen";

        public UI_Tip_Screen_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Tip_Screen_ViewBinder;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public GameButton m_img_icon_GameButton;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public GameButton m_img_bg_GameButton;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrow1_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Tip_Screen_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_GameButton = FindUI<GameButton>(gameObject.transform ,"img_icon");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");
			m_img_bg_GameButton = FindUI<GameButton>(gameObject.transform ,"img_bg");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_languageText");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_arrow");

			m_img_arrow1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_arrow/img_arrow1");


			BindEvent();
        }

        #endregion
    }
}