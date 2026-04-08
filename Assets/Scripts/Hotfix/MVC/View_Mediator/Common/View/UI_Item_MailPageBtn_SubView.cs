// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailPageBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailPageBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailPageBtn";

        public UI_Item_MailPageBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailPageBtn_ViewBinder;

		[HideInInspector] public PolygonImage m_img_activebg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_active_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public Shadow m_lbl_languageText_Shadow;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_reddot_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_reddotcount_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_MailPageBtn_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_activebg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_activebg");

			m_img_active_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_active");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_languageText");
			m_lbl_languageText_Shadow = FindUI<Shadow>(gameObject.transform ,"lbl_languageText");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_reddot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_reddot");

			m_lbl_reddotcount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_reddot/lbl_reddotcount");


			BindEvent();
        }

        #endregion
    }
}