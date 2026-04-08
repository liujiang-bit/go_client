// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailContactTag_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailContactTag_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailContactTag";

        public UI_Item_MailContactTag_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailContactTag_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_titleName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_titleName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_select_PolygonImage;
		[HideInInspector] public GameButton m_btn_select_GameButton;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_MailContactTag_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_titleName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_titleName");
			m_lbl_titleName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_titleName");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_count");

			m_btn_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/btn_select");
			m_btn_select_GameButton = FindUI<GameButton>(gameObject.transform ,"bg/btn_select");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/btn_select/img_select");


			BindEvent();
        }

        #endregion
    }
}